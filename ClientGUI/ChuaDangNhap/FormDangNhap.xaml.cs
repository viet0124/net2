using ClientBLL;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace ClientGUI.ChuaDangNhap
{
    /// <summary>
    /// Interaction logic for FormDangNhap.xaml
    /// </summary>
    public partial class FormDangNhap : Window
    {
        public bool DaDangNhap { get; private set; } = false;
        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void TaoTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            FormDangKy formDangKy = new FormDangKy();
            this.Hide();
            formDangKy.ShowDialog();
            this.ShowDialog();
        }

        private void button_DangNhap_Click(object sender, RoutedEventArgs e)
        {
            string TenDangNhap = textBox_TenDangNhap.Text.Trim();
            string MatKhau = passwordBox_MatKhau.Password.Trim();
            if (string.IsNullOrEmpty(TenDangNhap) || string.IsNullOrEmpty(MatKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            string? ketqua = TaiKhoanBLL.DangNhap(TenDangNhap, MatKhau);
            if (String.IsNullOrEmpty(ketqua))
            {
                DaDangNhap = true;
                this.Close();
                return;
            }
            MessageBox.Show(ketqua);
        }

        private void button_CheDoKhach_Click(object sender, RoutedEventArgs e)
        {
            TaiKhoanBLL.TaoTaiKhoanAnDanh();
        }

        private void passwordBox_MatKhau_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordBox_MatKhau.PasswordChanged -= passwordBox_MatKhau_PasswordChanged;
            passwordBox_MatKhau.Password = passwordBox_MatKhau.Password.ToUpper();
            passwordBox_MatKhau.PasswordChanged += passwordBox_MatKhau_PasswordChanged;
        }
    }
}
