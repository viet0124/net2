using DTO;
using ServerBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ServerGUI
{
    public partial class DoiMatKhau : Form
    {
        private TaiKhoan taiKhoan = null!;
        public DoiMatKhau(TaiKhoan tk, int containerWidth, int containerHeight)
        {
            taiKhoan = tk;
            InitializeComponent();
            this.Load += (sender, e)
                =>
            { this.Location = new System.Drawing.Point((int)(containerWidth / 2 - this.Width / 2), (int)(containerHeight / 2 - this.Height / 2)); };

            InitializeValue();
        }

        private void InitializeValue()
        {
            textBox_HoVaTen.Text = taiKhoan.HoTen;
            textBox_TenDangNhap.Text = taiKhoan.TenDangNhap;
        }

        private void button_XacNhan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_MatKhau.Text))
            {
                System.Windows.Forms.MessageBox.Show("Mật khẩu không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string error;
                AccountBLL.DoiMatKhau(taiKhoan.Id, textBox_MatKhau.Text, out error);
                if (!String.IsNullOrEmpty(error))
                {
                    System.Windows.MessageBox.Show(error, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                this.Close();
            }
        }

        private void button_HuyBo_Click(object sender, EventArgs e)
        {
            this.FormClosed += (e, args) => this.Dispose();
            this.Close();
        }
    }
}
