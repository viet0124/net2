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
using System.Windows.Forms;

namespace ServerGUI
{
    public partial class ThemTaiKhoan : Form
    {
        public ThemTaiKhoan(int containerWidth, int containerHeight)
        {
            InitializeComponent();
            this.Load += (sender, e)
                =>
            { this.Location = new System.Drawing.Point((containerWidth / 2 - this.Width / 2), (containerHeight / 2 - this.Height / 2)); };
        }

        private void button_Tao_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_HoVaTen.Text)
                || string.IsNullOrEmpty(textBox_TenDangNhap.Text)
                || string.IsNullOrEmpty(textBox_MatKhau.Text)
                || string.IsNullOrWhiteSpace(textBox_HoVaTen.Text)
                || string.IsNullOrWhiteSpace(textBox_TenDangNhap.Text)
                || string.IsNullOrWhiteSpace(textBox_MatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string HoVaTen = textBox_HoVaTen.Text.Trim();
                string TenDangNhap = textBox_TenDangNhap.Text.Trim();
                string MatKhau = textBox_MatKhau.Text.Trim();
                //tao tai khoan moi
                bool ThanhCong = AccountBLL.TaoTaiKhoan(TenDangNhap, MatKhau, HoVaTen, "Khách hàng", out string error);
                if (!ThanhCong)
                {
                    MessageBox.Show(error);
                }
                else
                {
                    MessageBox.Show("Tạo tài khoản thành công!");
                    this.Close();
                }

            }
        }
        private void button_DatLai_Click(object sender, EventArgs e)
        {
            textBox_HoVaTen.Text = string.Empty;
            textBox_TenDangNhap.Text = string.Empty;
            textBox_MatKhau.Text = string.Empty;
        }
    }
}
