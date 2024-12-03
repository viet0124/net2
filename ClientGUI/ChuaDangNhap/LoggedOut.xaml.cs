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
using ClientGUI.DaDangNhap;

namespace ClientGUI.ChuaDangNhap
{
    /// <summary>
    /// Interaction logic for LoggedOut.xaml
    /// </summary>
    public partial class LoggedOut : Window
    {
        public const byte DangXuat = 1;
        public const byte KhoaMay = 2;
        private byte Status;
        private Window? CuaSo;
        public LoggedOut(byte status)
        {
            InitializeComponent();
            Status = status;
        }

        public LoggedOut()
        {
            InitializeComponent();
            Status = DangXuat;
            //CuaSo = new FormDangNhap();
        }

        private void ShowFormDangNhap()
        {
            CuaSo = new FormDangNhap();
            CuaSo.ShowDialog();
            if ((CuaSo as FormDangNhap)!.DaDangNhap)
            {
                LoggedIn loggedIn = new LoggedIn();
                loggedIn.Show();
                this.Close();
            }
            CuaSo = null;
        }

        private void ShowFormKhoaMay()
        {
            CuaSo = new Locked();
            CuaSo.ShowDialog();
            if ((CuaSo as Locked)!.MatKhauDung)
            {
                LoggedIn loggedIn = new LoggedIn();
                loggedIn.Show();
                this.Close();
            }
            CuaSo = null;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CuaSo != null && CuaSo.IsVisible)
            {
                CuaSo.Activate();
            }
            else
            {
                if (Status == DangXuat)
                {
                    ShowFormDangNhap();
                }
                else
                {
                    ShowFormKhoaMay();
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = true;
            //if (CuaSo != null && CuaSo.IsVisible)
            //{
            //    CuaSo.Activate();
            //}
            //else
            //{
            //    if (Status == DangXuat)
            //    {
            //        ShowFormDangNhap();
            //    }
            //    else
            //    {
            //        ShowFormKhoaMay();
            //    }
            //}
            //ShowFormDangNhap();
        }
    }
}
