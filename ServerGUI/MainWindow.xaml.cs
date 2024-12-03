using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;
using DTO;
using ServerGUI.QuanLyTaiKhoan;
using ServerBLL;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ServerGUI.QuanLyMay;
using System.Windows.Threading;

namespace ServerGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<May> TableMayDataSource = new ObservableCollection<May>();
        public ObservableCollection<TaiKhoan> TableTaiKhoanDataSource = new ObservableCollection<TaiKhoan>();

        public MainWindow()
        {
            InitializeComponent();
            datagrid_TaiKhoan.ItemsSource = TableTaiKhoanDataSource;
            datagrid_May.ItemsSource = TableMayDataSource;

            Task.Run(async () =>
            {
                while (true)
                {
                    Task reloadMay = Task.Run(ReloadTableMay);
                    Task reloadTK = Task.Run(ReloadTableTaiKhoan);
                    await Task.WhenAll(reloadMay, reloadTK);
                    await Task.Delay(1000);
                }
            });
        }

        private void datagrid_TaiKhoan_Loaded(object sender, RoutedEventArgs e)
        {
            //ReloadTableTaiKhoan().Wait();
        }

        private void datagrid_May_Loaded(object sender, RoutedEventArgs e)
        {
            //ReloadTableMay().Wait();
        }

        private async Task ReloadTableMay()
        {
            MayBLL.KiemTraTinhTrang();
            List<May> newMay = MayBLL.LayTatCaMay();

            //Dispatcher.Invoke(() => TableMayDataSource = new(newMay));
            var oldId = TableMayDataSource.Select(may => may.Id).ToArray();
            var newId = newMay.Select(may => may.Id).ToList();
            foreach (var Id in oldId)
            {
                if (newId.Contains(Id))
                {
                    May MayCu = TableMayDataSource.Single(may => may.Id == Id);
                    May MayMoi = newMay.Single(may => may.Id == Id);
                    await Dispatcher.InvokeAsync(() =>
                    {
                        MayCu.IdTinhTrang = MayMoi.IdTinhTrang;
                        MayCu.IdTinhTrangNavigation = MayMoi.IdTinhTrangNavigation;
                        MayCu.DangHoatDong = MayMoi.DangHoatDong;
                    });
                    newId.Remove(Id);
                }
                else
                {
                    await Dispatcher.InvokeAsync(() => TableMayDataSource.Remove(TableMayDataSource.Single(may => may.Id == Id)));
                }
            }
            foreach (var Id in newId)
            {
                await Dispatcher.InvokeAsync(() => TableMayDataSource.Add(newMay.Single(may => may.Id == Id)));
            }
        }

        private async Task ReloadTableTaiKhoan()
        {
            AccountBLL.KiemTraSoDu();
            List<TaiKhoan> newTaiKhoan = AccountBLL.LayTatCaTaiKhoan();

            var oldId = TableTaiKhoanDataSource.Select(tk => tk.Id).ToArray();
            var newId = newTaiKhoan.Select(tk => tk.Id).ToList();

            foreach (var Id in oldId)
            {
                if (newId.Contains(Id))
                {
                    TaiKhoan old = TableTaiKhoanDataSource.Single(tk => tk.Id == Id);
                    TaiKhoan New = newTaiKhoan.Single(tk => tk.Id == Id);
                    await Dispatcher.InvokeAsync(() =>
                    {
                        old.SoDu = New.SoDu;
                    });
                    newId.Remove(Id);
                }
                else
                {
                    await Dispatcher.InvokeAsync(() => TableTaiKhoanDataSource.Remove(TableTaiKhoanDataSource.Single(tk => tk.Id == Id)));
                }
            }
            foreach (var Id in newId)
            {
                await Dispatcher.InvokeAsync(() => TableTaiKhoanDataSource.Add(newTaiKhoan.Single(tk => tk.Id == Id)));
            }

        }
        private void button_DoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_TaiKhoan.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn tài khoản!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                DoiMatKhau form = new DoiMatKhau((TaiKhoan)datagrid_TaiKhoan.SelectedItem, Screen.PrimaryScreen!.Bounds.Width, Screen.PrimaryScreen!.Bounds.Height);
                form.ShowDialog();
                //ReloadTableTaiKhoan();
            }
        }

        private void button_XoaTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_TaiKhoan.SelectedItem != null)
            {
                TaiKhoan selected = (TaiKhoan)datagrid_TaiKhoan.SelectedItem;
                MessageBoxResult result = System.Windows.MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    string error;
                    if (!AccountBLL.XoaTaiKhoan(selected.Id, out error))
                    {
                        System.Windows.MessageBox.Show(error, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    //ReloadTableTaiKhoan();
                }
            }
            else System.Windows.MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void button_NapTien_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_TaiKhoan.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn tài khoản cần nạp tiền!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                QuanLySoDu form = new QuanLySoDu((TaiKhoan)datagrid_TaiKhoan.SelectedItem);
                form.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                form.ShowDialog();
            }
        }

        private void button_ThemTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            ThemTaiKhoan form = new ThemTaiKhoan(Screen.PrimaryScreen!.Bounds.Width, Screen.PrimaryScreen!.Bounds.Height);
            form.ShowDialog();
            //ReloadTableTaiKhoan();
        }

        private void button_NapTien1_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_May.SelectedItem == null || ((May)datagrid_May.SelectedItem).DangHoatDong == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn tài khoản cần nạp tiền!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                TaiKhoan tk = ((IEnumerable<TaiKhoan>)datagrid_TaiKhoan.ItemsSource).Single(tk => tk.Id == ((May)datagrid_May.SelectedItem).DangHoatDong!.IdTaiKhoanNavigation.Id);
                QuanLySoDu form = new QuanLySoDu(tk);
                form.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                form.ShowDialog();
            }
        }

        private async void button_May_Refresh_Click(object sender, RoutedEventArgs e)
        {

            await Task.Delay(2000);
            //Task.Run(ReloadTableMay);
            await Task.Run(() =>
            {
                foreach (May may in datagrid_May.Items)
                {
                    may.TenMay = "HAHAHA";
                }
                datagrid_May.Dispatcher.Invoke(() => { datagrid_May.Items.Refresh(); });
            });

        }

        private void button_TaiKhoan_Refresh_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(ReloadTableTaiKhoan);
        }

        private void button_KhoiDongMay_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_May.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn máy cần khởi động!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (((May)datagrid_May.SelectedItem).IdTinhTrang != 0)
            {
                System.Windows.MessageBox.Show("Máy đã khởi động!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MayBLL.KhoiDongMay(((May)datagrid_May.SelectedItem).DiaChiMac);
                //ReloadTableMay();
            }
        }

        private void button_TatMay_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_May.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn máy cần tắt nguồn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (((May)datagrid_May.SelectedItem).IdTinhTrang == 0)
            {
                System.Windows.MessageBox.Show("Máy đã tắt nguồn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MayBLL.TatMay(((May)datagrid_May.SelectedItem).DiaChiIpv4);
                //ReloadTableMay();
            }
        }

        private void button_May_Them_Click(object sender, RoutedEventArgs e)
        {
            new ThemMay().ShowDialog();
            //ReloadTableMay();
        }

        private void button_May_Xoa_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_May.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn máy cần xoá!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (MayBLL.XoaMay(((May)datagrid_May.SelectedItem).Id, out string error))
                    {
                        //ReloadTableMay();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(error);
                    }
                }

            }
        }

        private void menuItem_TaiKhoan_NapTien_Click(object sender, RoutedEventArgs e)
        {
            button_NapTien_Click(sender, e);
        }

        private void menuItem_TaiKhoan_DoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            button_DoiMatKhau_Click(sender, e);
        }

        private void menuItem_TaiKhoan_Xoa_Click(object sender, RoutedEventArgs e)
        {
            button_XoaTaiKhoan_Click(sender, e);
        }

        private void datagrid_TaiKhoan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagrid_TaiKhoan.SelectedItem != null)
            {
                button_DoiMatKhau.IsEnabled = true;
                button_NapTien.IsEnabled = true;
                button_XoaTaiKhoan.IsEnabled = true;
            }
            else
            {
                button_DoiMatKhau.IsEnabled = false;
                button_NapTien.IsEnabled = false;
                button_XoaTaiKhoan.IsEnabled = false;
            }
        }

        private void datagrid_May_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagrid_May.SelectedItem != null)
            {
                if (((May)datagrid_May.SelectedItem).IdTinhTrang == 0)
                {
                    button_KhoiDongMay.IsEnabled = true;
                    menuItem_May_KhoiDongMay.IsEnabled = true;
                    button_TatMay.IsEnabled = false;
                    menuItem_May_TatMay.IsEnabled = false;
                }
                else
                {
                    button_KhoiDongMay.IsEnabled = false;
                    menuItem_May_KhoiDongMay.IsEnabled = false;
                    button_TatMay.IsEnabled = true;
                    menuItem_May_TatMay.IsEnabled = true;
                }
                if (((May)datagrid_May.SelectedItem).DangHoatDong != null)
                {
                    button_NapTien1.IsEnabled = true;
                    menuItem_May_NapTien.IsEnabled = true;
                }
                else
                {
                    button_NapTien1.IsEnabled = false;
                    menuItem_May_NapTien.IsEnabled = false;
                }
                button_May_Xoa.IsEnabled = true;
                menuItem_May_Xoa.IsEnabled = true;
            }
            else
            {
                button_KhoiDongMay.IsEnabled = false;
                menuItem_May_KhoiDongMay.IsEnabled = false;
                button_TatMay.IsEnabled = false;
                menuItem_May_TatMay.IsEnabled = false;
                button_NapTien1.IsEnabled = false;
                menuItem_May_NapTien.IsEnabled = false;
                button_May_Xoa.IsEnabled = false;
                menuItem_May_Xoa.IsEnabled = false;
            }
        }

        private void menuItem_May_KhoiDongMay_Click(object sender, RoutedEventArgs e)
        {
            button_KhoiDongMay_Click(sender, e);
        }

        private void menuItem_May_TatMay_Click(object sender, RoutedEventArgs e)
        {
            button_TatMay_Click(sender, e);
        }

        private void menuItem_May_NapTien_Click(object sender, RoutedEventArgs e)
        {
            button_NapTien1_Click(sender, e);
        }

        private void menuItem_May_Xoa_Click(object sender, RoutedEventArgs e)
        {
            button_May_Xoa_Click(sender, e);
        }
    }
}