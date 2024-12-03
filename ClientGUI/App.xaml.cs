using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows;
using ClientBLL;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            new Thread(() =>
            {
                TCPClientBLL.Instance.Connect();
                TCPListenerBLL.Instance.Run();
            }).Start();
            Task.Run(() =>
            {
                TaiKhoanBLL.MockQRNapTien().Wait();
            });
        }
    }

}
