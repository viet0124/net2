using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClientBLL;
using DTO;

namespace ClientGUI.DaDangNhap
{
    /// <summary>
    /// Interaction logic for TaiKhoan.xaml
    /// </summary>
    public partial class QuanLyTaiKhoan : Window
    {
        //private static readonly Regex regex = new Regex("[^0-9]+"); // Regex that matches non-numeric text
        private CancellationTokenSource cancelTokenNapTien = new();
        private const short CountdownSeconds = 120;
        private readonly double ParentWidth, ParentHeight;
        public QuanLyTaiKhoan(double ParentWidth, double ParentHeight)
        {
            InitializeComponent();
            this.ParentWidth = ParentWidth;
            this.ParentHeight = ParentHeight;
            this.Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemParameters.PrimaryScreenWidth - ParentWidth - this.ActualWidth;
            this.Top = ParentHeight / 2;
        }

        private void passwordBox_MatKhauCu_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordBox_MatKhauCu.PasswordChanged -= passwordBox_MatKhauCu_PasswordChanged;
            passwordBox_MatKhauCu.Password = passwordBox_MatKhauCu.Password.ToUpper();
            passwordBox_MatKhauCu.PasswordChanged += passwordBox_MatKhauCu_PasswordChanged;
        }

        private void passwordBox_MatKhauMoi_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordBox_MatKhauMoi.PasswordChanged -= passwordBox_MatKhauMoi_PasswordChanged;
            passwordBox_MatKhauMoi.Password = passwordBox_MatKhauMoi.Password.ToUpper();
            passwordBox_MatKhauMoi.PasswordChanged += passwordBox_MatKhauMoi_PasswordChanged;
        }

        private void passwordBox_XacNhanMatKhau_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordBox_XacNhanMatKhau.PasswordChanged -= passwordBox_XacNhanMatKhau_PasswordChanged;
            passwordBox_XacNhanMatKhau.Password = passwordBox_XacNhanMatKhau.Password.ToUpper();
            passwordBox_XacNhanMatKhau.PasswordChanged += passwordBox_XacNhanMatKhau_PasswordChanged;
        }

        private void button_DoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            textBlock_MatKhauCu.Visibility = Visibility.Hidden;
            textBlock_MatKhauMoi.Visibility = Visibility.Hidden;
            textBlock_XacNhanMatKhauMoi.Visibility = Visibility.Hidden;

            string MatKhauCu = passwordBox_MatKhauCu.Password;

            if (MatKhauCu == string.Empty)
            {
                textBlock_MatKhauCu.Text = "Vui lòng nhập mật khẩu cũ";
                textBlock_MatKhauCu.Visibility = Visibility.Visible;
                passwordBox_MatKhauMoi.Password = string.Empty;
                passwordBox_XacNhanMatKhau.Password = string.Empty;
                return;
            }

            string MatKhauMoi = passwordBox_MatKhauMoi.Password;

            if (MatKhauMoi == string.Empty)
            {
                textBlock_MatKhauMoi.Text = "Vui lòng nhập mật khẩu mới";
                textBlock_MatKhauMoi.Visibility = Visibility.Visible;
                passwordBox_MatKhauCu.Password = string.Empty;
                passwordBox_XacNhanMatKhau.Password = string.Empty;
                return;
            }

            string XacNhanMatKhau = passwordBox_XacNhanMatKhau.Password;

            if (XacNhanMatKhau == string.Empty)
            {
                textBlock_XacNhanMatKhauMoi.Text = "Vui lòng nhập lại mật khẩu mới";
                textBlock_XacNhanMatKhauMoi.Visibility = Visibility.Visible;
                passwordBox_MatKhauCu.Password = string.Empty;
                passwordBox_MatKhauMoi.Password = string.Empty;
                return;
            }

            if (!string.Equals(MatKhauCu, TaiKhoanBLL.UserHienTai!.MatKhau))
            {
                textBlock_MatKhauCu.Text = "Sai mật khẩu";
                textBlock_MatKhauCu.Visibility = Visibility.Visible;
                passwordBox_MatKhauCu.Password = string.Empty;
                passwordBox_MatKhauMoi.Password = string.Empty;
                passwordBox_XacNhanMatKhau.Password = string.Empty;
                return;
            }

            if (!MatKhauMoi.Equals(XacNhanMatKhau))
            {
                textBlock_XacNhanMatKhauMoi.Text = "Mật khẩu không khớp";
                textBlock_XacNhanMatKhauMoi.Visibility = Visibility.Visible;
                passwordBox_MatKhauCu.Password = string.Empty;
                passwordBox_MatKhauMoi.Password = string.Empty;
                passwordBox_XacNhanMatKhau.Password = string.Empty;
                return;
            }

            string? result = TaiKhoanBLL.DoiMatKhau(MatKhauMoi);
            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show("Đổi mật khẩu thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show(result, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void button_HuyBo_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_HuyBo1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void textBox_SoTien_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            textBox_SoTien.TextChanged -= textBox_SoTien_TextChanged;
            string input = textBox_SoTien.Text;
            StringBuilder output = new();
            char check;
            int max = (input.Length > 10) ? 9 : input.Length - 1;
            for (int i = max; i >= 0; i--)
            {
                check = input[i];
                if (char.IsDigit(check)) output.Insert(0, check);
                else textBox_SoTien.Text = textBox_SoTien.Text.Remove(i, 1);
            }
            string outString = output.ToString();

            if (int.TryParse(outString, out int value))
            {
                textBox_SoTien.Text = string.Format("{0:N0}", value);
                textBox_SoTien.SelectionStart = textBox_SoTien.Text.Length; // Move cursor to the end
            }


            // Re-attach event handler
            textBox_SoTien.TextChanged += textBox_SoTien_TextChanged;
        }

        private async void button_NapTien_Click(object sender, RoutedEventArgs e)
        {
            grid_NapTien.IsEnabled = false;
            string money = textBox_SoTien.Text.Replace(",", "");
            int SoTien = int.Parse(money);
            var KetQuaQR = await TaiKhoanBLL.QRNapTien(SoTien);
            string? Base64QRstring = KetQuaQR.Item1;
            string? error = KetQuaQR.Item2;
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                BitmapImage image = new();
                image.BeginInit();
                image.StreamSource = new MemoryStream(Convert.FromBase64String(Base64QRstring!));
                image.EndInit();
                grid_NapTien.IsEnabled = true;
                grid_Nhap_Tien.Visibility = Visibility.Collapsed;
                image_QR.Source = image;
                grid_QR.Visibility = Visibility.Visible;
                cancelTokenNapTien = new CancellationTokenSource();
                cancelTokenNapTien.Token.Register(async () =>
                {
                    await Dispatcher.InvokeAsync(() =>
                    {
                        textBox_SoTien.Text = "";
                        grid_Nhap_Tien.Visibility = Visibility.Visible;
                        image_QR.Source = null;
                        grid_QR.Visibility = Visibility.Collapsed;
                    });
                });
                
                await Task.Run(async () =>
                {
                    short seconds = CountdownSeconds;
                    cancelTokenNapTien.CancelAfter(CountdownSeconds * 1000);
                    Task<string?> ChoKetQuaNapTien = TaiKhoanBLL.NapTien(SoTien, cancelTokenNapTien.Token);
                    do
                    {
                        if (ChoKetQuaNapTien.IsCompletedSuccessfully)
                        {
                            string? KetQuaNapTien = await ChoKetQuaNapTien;
                            if (string.IsNullOrEmpty(KetQuaNapTien))
                            {
                                TaiKhoanBLL.UserHienTai!.Set_SoDu(SoTien);
                            }
                            else
                            {
                                MessageBox.Show(KetQuaNapTien, "Lỗi nạp tiền", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            cancelTokenNapTien.Cancel();
                            break;
                        }
                        else if (ChoKetQuaNapTien.IsCanceled) break;
                        await Dispatcher.InvokeAsync(() => label_DemNguoc.Content = $"Còn {seconds--} giây để thanh toán");
                        await Task.Delay(1000);
                    }
                    while (!cancelTokenNapTien.Token.IsCancellationRequested);
                });
            }

        }

        private async void button_HuyBo2_Click(object sender, RoutedEventArgs e)
        {
            await cancelTokenNapTien!.CancelAsync();
            TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes("HuyNapTien"));
        }
    }
}
