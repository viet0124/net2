using ClientBLL;
using ClientGUI.ChuaDangNhap;
using DTO;
using System.ComponentModel;
using System.Net;
using System.Windows;

namespace ClientGUI.DaDangNhap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoggedIn : Window
    {
        private CancellationTokenSource cancelAllTaskToken = new CancellationTokenSource();
        private void Window_Closed(object? sender, System.EventArgs e)
        {
            cancelAllTaskToken.Cancel();
        }
        public LoggedIn()
        {
            InitializeComponent();
            this.Closed += Window_Closed;
            TaiKhoanBLL.UserHienTai!.HetThoiGian += HetThoiGian;
        }

        private void HetThoiGian(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(()=> button_DangXuat_Click(sender, new RoutedEventArgs()), System.Windows.Threading.DispatcherPriority.Send);
        }

        private void PositionTopRight(object sender, RoutedEventArgs e)
        {
            double ScreenWidth = SystemParameters.PrimaryScreenWidth;
            double WindowWidth = this.ActualWidth;
            this.Left = ScreenWidth - WindowWidth;
            this.Top = 0;
        }

        private void button_DangXuat_Click(object sender, RoutedEventArgs e)
        {
            string? ketqua = TaiKhoanBLL.DangXuat();
            if (ketqua == null)
            {
                //Thread closeThis = new Thread(() => this.Dispatcher.Invoke(() => this.Close()));
                //closeThis.Start();
                Task.Factory.StartNew(() => this.Dispatcher.Invoke(() => this.Close()));
                LoggedOut loggedOut = new LoggedOut(LoggedOut.DangXuat);
                loggedOut.Show();
            }
            else
            {
                MessageBox.Show(ketqua, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_KhoaMay_Click(object sender, RoutedEventArgs e)
        {
            //Thread closeThis = new Thread(() => this.Dispatcher.Invoke(() =>this.Close()));
            //closeThis.Start();
            Task.Factory.StartNew(() => this.Dispatcher.Invoke(() => this.Close()));
            LoggedOut loggedOut = new LoggedOut(LoggedOut.KhoaMay);
            loggedOut.Show();
        }

        private void button_TaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            QuanLyTaiKhoan taiKhoan = new QuanLyTaiKhoan(this.ActualWidth, this.ActualHeight);
            taiKhoan.ShowDialog();
        }
    }

}