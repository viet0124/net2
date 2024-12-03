using DAL;
using DTO;
using System.Net.NetworkInformation;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Text;

namespace ServerBLL
{
    public class MayBLL
    {
        public static List<string> TatCaTenLoaiMay()
        {
            Pbl4Context context = new Pbl4Context();
            List<string> list = context.TableLoaiMay.Select(lm => lm.TenLoaiMay).ToList();
            context.Dispose();
            return list;
        }
        public static List<May> LayTatCaMay()
        {
            Pbl4Context context = new();
            List<May> listMay = context.TableMay.Select(may => may).Include(may => may.DangHoatDong)
                .ThenInclude(dhd => dhd.IdTaiKhoanNavigation)
                .Include(may => may.IdLoaiMayNavigation)
                .Include(may => may.IdTinhTrangNavigation)
                .ToList();
            context.Dispose();
            return listMay;
        }

        public static bool ThemMayMoi(string IP, string MAC, string LoaiMay, out string error)
        {
            Pbl4Context context = new();
            error = string.Empty;
            try
            {
                IEnumerable<May> mays = context.TableMay.Select(may => may);
                int SoLuongHienTai = mays.Count();
                short Id = (SoLuongHienTai > 0) ? mays.Max(may => may.Id) : (short)1;
                Id++;
                IEnumerable<May> maythuongs = mays.Where(may => may.IdLoaiMayNavigation.TenLoaiMay == LoaiMay).Select(may => may);
                string TenMay;
                if (maythuongs.Count() > 0)
                {
                    TenMay = maythuongs.Max(may => may.TenMay)!;
                    int SoMay = int.Parse(TenMay.Substring(3));
                    TenMay = "MAY" + (SoMay + 1);
                }
                else
                {
                    TenMay = "MAY1";
                }
                context.TableMay.Add(
                    new May()
                    {
                        Id = Id,
                        TenMay = TenMay,
                        DiaChiIpv4 = IP,
                        DiaChiMac = MAC,
                        IdLoaiMay = 0,
                        IdTinhTrang = 0
                    });
                context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                context.Dispose();
                return false;
            }
            context.Dispose();
            return true;
        }

        public static bool XoaMay(short Id, out string error)
        {
            Pbl4Context context = new();
            error = string.Empty;
            try
            {
                May may = context.TableMay.Single(may => may.Id == Id);
                context.TableMay.Remove(may);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                error = e.Message;
                context.Dispose();
                return false;
            }
            context.Dispose();
            return true;
        }


        public static void KiemTraTinhTrang()
        {
            Pbl4Context context = new Pbl4Context();
            IEnumerable<May> TatCaMay = context.TableMay.Select(may => may);
            try
            {
                foreach (May may in TatCaMay)
                {
                    //byte IdTinhTrangMay;
                    using (Ping ping = new Ping())
                    {
                        PingReply reply = ping.Send(may.DiaChiIpv4, 300);

                        if (reply.Status == IPStatus.Success)
                        {
                            //IdTinhTrangMay = 2;
                            //bool DaDangNhap = context.TableDangHoatDong.Any(dhd => dhd.IdMay == may.Id);
                            //if (DaDangNhap) IdTinhTrangMay = 1;
                        }
                        else
                        {
                            //IdTinhTrangMay = 0;
                            DangHoatDong? dhd = context.TableDangHoatDong.SingleOrDefault(dhd => dhd.IdMay == may.Id);
                            if (dhd != null)
                                context.TableDangHoatDong.Remove(dhd);
                            may.IdTinhTrang = 0;
                        }
                        
                    }
                }
                context.SaveChanges();
                context.Dispose();
            }
            catch (Exception ex)
            {
                context.Dispose();
                throw;
            }
        }

        public static void KhoiDongMay(string macAddress)
        {
            TcpClientBLL.WakeOnLAN(macAddress);
        }

        public static void TatMay(string IP)
        {
            TcpClientBLL.TatMay(IP);
        }

    }
}
