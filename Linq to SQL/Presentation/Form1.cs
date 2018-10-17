using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogic;
using System.IO;
using System.Data.Linq;
using DataAccess;

namespace Presentation
{
    public partial class Form1 : Form
    {
        #region Bien toan cuc
        string path = "../../Hinh";
        Table<SINHVIEN> Bang_SINHVIEN;
        Table<LOP> Bang_LOP;
        QLSinhVienBDDataContext db;
        BindingManagerBase DSSV;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            db = new QLSinhVienBDDataContext();

            Bang_SINHVIEN = db.SINHVIENs;
            Bang_LOP = db.GetTable<LOP>();

            LoadCBOLop();
            LoadDGVHocSinh();

            txtMaSV.DataBindings.Add("text", Bang_SINHVIEN, "MaSV", true);
            txtHoTen.DataBindings.Add("text", Bang_SINHVIEN, "HoTen", true);
            dtpNgaySinh.DataBindings.Add("text", Bang_SINHVIEN, "NgaySinh", true);
            radNam.DataBindings.Add("checked", Bang_SINHVIEN, "GioiTinh", true);
            cbbLop.DataBindings.Add("SelectedValue", Bang_SINHVIEN, "MaLop", true);
            txtDiaChi.DataBindings.Add("text", Bang_SINHVIEN, "DiaChi", true);
            pictureBoxHinh.DataBindings.Add("ImageLocation", Bang_SINHVIEN, "Hinh", true);

            DSSV = this.BindingContext[Bang_SINHVIEN];
            enabledNutLenh(false);
        }

        private void LoadCBOLop()
        {
            cbbLop.DataSource = Bang_LOP;
            cbbLop.DisplayMember = "TenLop";
            cbbLop.ValueMember = "MaLop";
        }

        private void LoadDGVHocSinh()
        {
            gvSinhVien.AutoGenerateColumns = false;
            gvSinhVien.DataSource = Bang_SINHVIEN;
        }

        private void enabledNutLenh(bool isEnabled)
        {
            btnThem.Enabled = !isEnabled;
            btnXoa.Enabled = !isEnabled;
            btnSua.Enabled = !isEnabled;
            btnThoat.Enabled = !isEnabled;
            btnLuu.Enabled = isEnabled;
            btnHuy.Enabled = isEnabled;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DSSV.AddNew();
            enabledNutLenh(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                DSSV.EndCurrentEdit();
                db.SubmitChanges();
                enabledNutLenh(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DSSV.CancelCurrentEdit();
            ChangeSet cs = db.GetChangeSet();
            db.Refresh(RefreshMode.OverwriteCurrentValues, cs.Updates);
            enabledNutLenh(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
                DSSV.RemoveAt(DSSV.Position);
                db.SubmitChanges();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            enabledNutLenh(true);
        }

        private void btnChonHinh_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG File|*.jpg|PNG File|*.png|All File|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.SafeFileName;
                string pathFile = path + "/" + fileName;
                if (!File.Exists(pathFile)) File.Copy(openFileDialog1.FileName, pathFile);
                pictureBoxHinh.ImageLocation = pathFile;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            DSSV.Position = Bang_SINHVIEN.ToList().FindIndex(gvSinhVien => gvSinhVien.MaSV.Contains(txtTimKiem.Text));
        }

        private void txtTimKiem_MouseDown(object sender, MouseEventArgs e)
        {
            txtTimKiem.Text = "";
        }

        private void radNam_CheckedChanged(object sender, EventArgs e)
        {
            radNu.Checked = !radNam.Checked;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
