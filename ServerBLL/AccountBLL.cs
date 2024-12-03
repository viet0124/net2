using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using DAL;
using DTO;

namespace ServerBLL
{
    public static class AccountBLL
    {
        public static decimal MoneyPerSecond { get; set; } = 5000m / 3600m;
        public static List<TaiKhoan> LayTatCaTaiKhoan()
        {
            Pbl4Context context = new();
            List<TaiKhoan> listTaiKhoan = new List<TaiKhoan>();
            listTaiKhoan = context.TableTaiKhoan.Select(tk => tk).ToList();
            context.Dispose();
            return listTaiKhoan;
        }
        public static int MuaDo(string mathang, int soluong)
        {
            Pbl4Context context = new();
            int thanhtoan = context.TableMatHang.Single(s => s.TenMatHang == mathang).Gia * soluong;
            context.Dispose();
            return thanhtoan;
        }
        public static void KiemTraSoDu()
        {
            Pbl4Context pbl4Context = new();
            List<DangHoatDong> dangHoatDongs = pbl4Context.TableDangHoatDong.ToList();
            foreach (DangHoatDong dhd in dangHoatDongs)
            {
                dhd.IdTaiKhoanNavigation.SoDu = TcpClientBLL.LaySoDu(dhd.IdMay);
            }
            pbl4Context.SaveChanges();
            pbl4Context.Dispose();
        }

        public static bool DangNhap(int idTaiKhoan, string IPv4Address, out string error)
        {
            Pbl4Context context = new();
            error = string.Empty;
            try
            {
                if (context.TableTaiKhoan.Single(tk => tk.Id == idTaiKhoan).SoDu <= 0)
                {
                    throw new Exception("Hết thời gian sử dụng");
                }
                May maydangnhap = context.TableMay.Single(may => may.DiaChiIpv4 == IPv4Address);
                maydangnhap.IdTinhTrang = 1;
                context.TableDangHoatDong.Add(
                    new DangHoatDong() { IdMay = maydangnhap.Id, IdTaiKhoan = idTaiKhoan });
                context.SaveChanges();
                context.Dispose();
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                context.Dispose();
                return false;
            }
        }
        public static bool DangXuat(string IPv4Address, decimal SoDuTaiKhoan, out string error)
        {
            Pbl4Context context = new();
            error = string.Empty;
            try
            {
                May maydangnhap = context.TableMay.Single(may => may.DiaChiIpv4 == IPv4Address);
                maydangnhap.IdTinhTrang = 2;
                DangHoatDong dangHoatDong = context.TableDangHoatDong.Single(dhd => dhd.IdMay == maydangnhap.Id);
                dangHoatDong.IdTaiKhoanNavigation.SoDu = SoDuTaiKhoan;
                context.TableDangHoatDong.Remove(dangHoatDong);
                context.SaveChanges();
                context.Dispose();
                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                context.Dispose();
                return false;
            }
        }

        public static bool TenDangNhapDaTonTai(string username)
        {
            Pbl4Context context = new();
            bool ketqua = context.TableTaiKhoan.Any(tk => tk.TenDangNhap == username);
            context.Dispose();
            return ketqua;
        }
        public static bool TaoTaiKhoan(string username, string password, string fullname, string vaiTro, out string error)
        {
            error = string.Empty;
            Pbl4Context context = new Pbl4Context();
            try
            {
                byte roleId = context.TableVaiTro.Single(vt => vt.TenVaiTro == vaiTro).Id;
                int id = context.TableTaiKhoan.Count() > 0 ? context.TableTaiKhoan.Max(tk => tk.Id) + 1 : 1;
                context.TableTaiKhoan.Add(new TaiKhoan
                {
                    Id = id,
                    TenDangNhap = username!,
                    MatKhau = password!,
                    HoTen = fullname!,
                    IdVaiTro = roleId,
                    SoDu = 0
                });
                context.SaveChanges();
                context.Dispose();
            }
            catch (Exception e)
            {
                error = e.Message;
                context.Dispose();
                return false;
            }
            return true;
        }

        public static bool DoiMatKhau(int id, string password, out string error)
        {
            error = string.Empty;
            Pbl4Context context = new();
            try
            {
                TaiKhoan tk = context.TableTaiKhoan.Single(tk => tk.Id == id);
                if (tk.MatKhau != password)
                {
                    tk.MatKhau = password;
                    context.SaveChanges();
                }
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

        async public static Task<string?> AwaitingMoney(int Id, int amount, CancellationToken cancellationToken)
        {
            HttpClient httpRequest = new HttpClient(new HttpClientHandler
            {
                UseProxy = false
            });
            string API_Token = "WLO82DIKLYX4UVKVETEU3CMPHUS9QDJN1EBMDJS0GGWW7F6XOZN4HRHH8JYQ12SV";
            httpRequest.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {API_Token}");

            try
            {
                DateTime InitialTime = DateTime.Now;
                string initialTimeString = InitialTime.ToString("yyyy-MM-dd HH:mm:ss");
                SepayResponseTransaction? transaction;
                string UserName = new Pbl4Context().TableTaiKhoan.Single(tk => tk.Id == Id).TenDangNhap;
                UriBuilder uriBuilder = new UriBuilder("https://my.sepay.vn/userapi/transactions/list");
                while (true)
                {
                    DateTime CurrentTime = DateTime.Now;
                    string currentTimeString = CurrentTime.ToString("yyyy-MM-dd HH:mm:ss");
                    NameValueCollection parameters = new NameValueCollection()
                {
                    { "transaction_date_min", initialTimeString },
                    { "transaction_date_max", currentTimeString },
                };
                    string query = string.Empty;
                    foreach (var key in parameters.AllKeys)
                    {
                        query += $"{key}={parameters.Get(key)}&";
                    }
                    query = query.Remove(query.Length - 1);

                    //uriBuilder.Query = query;
                    HttpResponseMessage responseMessage = await httpRequest.GetAsync(uriBuilder.Uri + $"?{query}", cancellationToken);
                    responseMessage.EnsureSuccessStatusCode();
                    SepayResponse? response = await responseMessage.Content.ReadFromJsonAsync<SepayResponse>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }, cancellationToken);
                    if (!(response!.messages!.success)) throw new HttpRequestException(response.error);
                    transaction = response!.transactions.SingleOrDefault(tr => tr.transaction_content!.Contains("TAIKHOAN " + UserName + " NAPTIEN", StringComparison.OrdinalIgnoreCase) && tr.transaction_date_inDateTime >= InitialTime && tr.transaction_date_inDateTime <= CurrentTime);
                    if (transaction != null)
                    {
                        httpRequest.Dispose();
                        string? KetQuaTangTien = ThayDoiTien(Id, amount);
                        if (!string.IsNullOrEmpty(KetQuaTangTien))
                        {
                            return KetQuaTangTien;
                        }
                        else return null;
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Delay(2000);
                }
            }
            catch (OperationCanceledException)
            {
                httpRequest.Dispose();
                return "TimeoutOrCancelled";
            }
            catch (Exception e)
            {
                httpRequest.Dispose();
                return e.Message;
            }
        }

        public static string? ThayDoiTien(int Id, int amount)
        {
            Pbl4Context context = new();
            try
            {
                TaiKhoan tk = context.TableTaiKhoan.Single(tk => tk.Id == Id);
                if (tk.SoDu + amount < 0) throw new ArgumentException("Số dư âm");
                tk.SoDu += amount;
                context.SaveChanges();
                int? IdMay = context.TableDangHoatDong.SingleOrDefault(dhd => dhd.IdTaiKhoan == tk.Id)?.IdMay;
                if (IdMay != null)
                {
                    TcpClientBLL.CapNhatSoDu(IdMay.Value, tk.SoDu);
                }
                context.Dispose();
                return null;
            }
            catch (Exception e)
            {
                context.Dispose();
                return e.Message;
            }
        }

        public static bool XoaTaiKhoan(int id, out string error)
        {
            Pbl4Context context = new();
            error = string.Empty;
            try
            {
                var taiKhoanCanXoa = context.TableTaiKhoan.Find(id);
                context.TableTaiKhoan.Remove(taiKhoanCanXoa!);
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

        public static TaiKhoan? LayTaiKhoan(int? id, string? username, out string error)
        {
            Pbl4Context context = new();
            error = string.Empty;
            try
            {
                //Nếu không có phần tử, taiKhoan1 = null; nếu có nhiều hơn 1 thì ném ngoại lệ
                TaiKhoan? taiKhoan;
                if (username != null) taiKhoan = context.TableTaiKhoan.SingleOrDefault(tk => tk.TenDangNhap == username);
                else taiKhoan = context.TableTaiKhoan.SingleOrDefault(tk => tk.Id == id);
                context.Dispose();
                return taiKhoan;
            }
            catch (Exception e)
            {
                context.Dispose();
                error = e.Message;
                return null;
            }
        }


        class SepayResponse
        {
            public string? error { get; set; }
            public SepayResponseMessage? messages { get; set; }
            public IEnumerable<SepayResponseTransaction> transactions { get; set; } = new List<SepayResponseTransaction>();
        }
        class SepayResponseMessage
        {
            public bool success { get; set; }
        }
        class SepayResponseTransaction
        {
            public string transaction_date { get; set; } = null!;
            public DateTime transaction_date_inDateTime
            {
                get => DateTime.Parse(transaction_date);
            }
            public string amount_in { get; set; } = null!;
            public decimal amount_in_inInt { get => decimal.Parse(amount_in); }
            public string? transaction_content { get; set; }
        }
    }
}
