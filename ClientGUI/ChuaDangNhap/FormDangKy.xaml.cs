using ClientBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientGUI.ChuaDangNhap
{
    /// <summary>
    /// Interaction logic for FormDangKy.xaml
    /// </summary>
    public partial class FormDangKy : Window
    {
        public FormDangKy()
        {
            InitializeComponent();
        }

        private void QuayLai_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_DangKy_Click(object sender, RoutedEventArgs e)
        {
            string HoVaTen = textBox_HoVaTen.Text;
            string TenDN = textBox_TenDangNhap.Text;
            string MatKhau = passwordBox_MatKhau.Password;
            string? result = TaiKhoanBLL.TaoTaiKhoan(TenDN, MatKhau, HoVaTen);
            if (!string.IsNullOrEmpty(result))
                MessageBox.Show(result);
            else MessageBox.Show("Tạo tài khoản thành công");
            this.Close();
        }

        private void passwordBox_MatKhau_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordBox_MatKhau.PasswordChanged -= passwordBox_MatKhau_PasswordChanged;
            passwordBox_MatKhau.Password = passwordBox_MatKhau.Password.ToUpper();
            passwordBox_MatKhau.PasswordChanged += passwordBox_MatKhau_PasswordChanged;
        }
    }
}
