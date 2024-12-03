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

namespace ClientGUI.DaDangNhap
{
    /// <summary>
    /// Interaction logic for Locked.xaml
    /// </summary>
    public partial class Locked : Window
    {
        public bool MatKhauDung { get; private set; } = false;
        public Locked()
        {
            InitializeComponent();
        }

        private void button_MoKhoa_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordBox_MatKhau.Password;
            if (password == TaiKhoanBLL.UserHienTai!.MatKhau)
            {
                MatKhauDung = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai mật khẩu");
            }
        }

        private void passwordBox_MatKhau_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordBox_MatKhau.PasswordChanged -= passwordBox_MatKhau_PasswordChanged;
            passwordBox_MatKhau.Password = passwordBox_MatKhau.Password.ToUpper();
            passwordBox_MatKhau.PasswordChanged += passwordBox_MatKhau_PasswordChanged;
        }
    }
}
