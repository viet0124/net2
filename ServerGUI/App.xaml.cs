using DAL;
using DTO;
using ServerBLL;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ServerGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            Task tcp = new Task(TcpServerBLL.Run, TaskCreationOptions.LongRunning);
            tcp.Start();
            Task.Run(async () =>
            {
                Pbl4Context context = new();
                List<DangHoatDong> dangHoatDongs = context.TableDangHoatDong.ToList();
                context.TableDangHoatDong.RemoveRange(dangHoatDongs);
                await context.SaveChangesAsync();
            });

        }
    }

}
