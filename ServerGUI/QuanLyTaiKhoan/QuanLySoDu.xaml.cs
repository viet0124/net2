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
using DTO;
using ServerBLL;

namespace ServerGUI.QuanLyTaiKhoan
{
    /// <summary>
    /// Interaction logic for QuanLySoDu.xaml
    /// </summary>
    public partial class QuanLySoDu : Window
    {
        private TaiKhoan _taiKhoan = null!;
        public TaiKhoan taiKhoan { get => _taiKhoan; }
        public QuanLySoDu(TaiKhoan tk)
        {
            _taiKhoan = tk;
            InitializeComponent();
            this.Loaded += QuanLySoDu_Loaded;
        }

        private void QuanLySoDu_Loaded(object sender, RoutedEventArgs e)
        {
            //InitializeValue();
        }

        //private void InitializeValue()
        //{
        //    textBox_TenDangNhap.Text = taiKhoan.TenDangNhap;
        //    textBox_SoDu.Text = taiKhoan.SoDu.ToString("N0");
        //    textBox_ThayDoi.Text = "0";
        //}

        private void button_Withdraw_Click(object sender, RoutedEventArgs e)
        {
            string money = textBox_ThayDoi.Text.Replace(",", "");
            int SoTien = -int.Parse(money);
            // Rút tiền
            string? error = AccountBLL.ThayDoiTien(_taiKhoan.Id, SoTien);
            if (!string.IsNullOrEmpty(error))
            {
                System.Windows.MessageBox.Show(error);
            }
            else this.Close();

        }

        private void button_Deposit_Click(object sender, RoutedEventArgs e)
        {
            string money = textBox_ThayDoi.Text.Replace(",", "");
            int SoTien = int.Parse(money);
            // Rút tiền
            //Task<string?> tangtien = AccountBLL.TangTien(taiKhoan.Id, SoTien);
            //tangtien.Wait();
            //string? error = tangtien.Result;
            string? error = AccountBLL.ThayDoiTien(_taiKhoan.Id, SoTien);
            if (!string.IsNullOrEmpty(error))
            {
                System.Windows.MessageBox.Show(error);
            }
            else this.Close();
        }

        private void button_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBox_ThayDoi_TextChanged(object sender, TextChangedEventArgs e)
        {
                textBox_ThayDoi.TextChanged -= textBox_ThayDoi_TextChanged;
                string input = textBox_ThayDoi.Text;
                StringBuilder output = new();
                char check;
                int max = (input.Length > 10) ? 9 : input.Length - 1;
                for (int i = max; i >= 0; i--)
                {
                    check = input[i];
                    if (char.IsDigit(check)) output.Insert(0, check);
                    else textBox_ThayDoi.Text = textBox_ThayDoi.Text.Remove(i, 1);
                }
                string outString = output.ToString();

                if (int.TryParse(outString, out int value))
                {
                textBox_ThayDoi.Text = string.Format("{0:N0}", value);
                textBox_ThayDoi.SelectionStart = textBox_ThayDoi.Text.Length; // Move cursor to the end
                }


            // Re-attach event handler
            textBox_ThayDoi.TextChanged += textBox_ThayDoi_TextChanged;

        }
    }
}
