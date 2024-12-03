using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientBLL
{
    public class TaiKhoanBLL
    {
        private static HttpClient httpRequest = new();
        private static System.Timers.Timer BoDemGio = new System.Timers.Timer(1000);
        private static TaiKhoanClient? _UserHienTai = null;
        public static TaiKhoanClient? UserHienTai
        {
            get => _UserHienTai;
            private set
            {
                if ((_UserHienTai == null && value != null) || (_UserHienTai != null && value == null))
                {
                    _UserHienTai = value;
                }
            }
        }
        static TaiKhoanBLL()
        {
            httpRequest.DefaultRequestHeaders.Add("x-client-id", "24c3d1a3-4db6-4e00-8ba5-4a5fe12e4288");
            httpRequest.DefaultRequestHeaders.Add("x-api-key", "99ab2b19-0be1-4328-8097-a66c03b494ba");
        }
        //private static void HetThoiGian(object sender, EventArgs e)
        //{
        //    DangXuat();
        //}

        private static decimal ByteArrayToDecimal(byte[] bytes)
        {
            if (bytes.Length != 16) throw new ArgumentException("Byte array must be exactly 16 bytes long.");
            int[] bits = new int[4];
            bits[0] = BitConverter.ToInt32(bytes, 0);
            bits[1] = BitConverter.ToInt32(bytes, 4);
            bits[2] = BitConverter.ToInt32(bytes, 8);
            bits[3] = BitConverter.ToInt32(bytes, 12);
            return new decimal(bits);
        }
        private static byte[] DecimalToByteArray(decimal value)
        {
            int[] arrayRepresentation = decimal.GetBits(value);
            MemoryStream buffer = new();
            for (int i = 0; i < 4; i++)
            {
                buffer.Write(BitConverter.GetBytes(arrayRepresentation[i]));
            }
            return buffer.ToArray();
        }
        public static string? DangNhap(string username, string password)
        {
            TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes("DangNhap"));
            TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes(username));
            byte TDNstatus = TCPClientBLL.Instance.ReceiveMessage()[0];
            if (TDNstatus == 1)
            // 0 = tài khoản không tồn tại, 1 = tài khoản tồn tại, 2 = lỗi khác
            {
                TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes(password));
                byte status = TCPClientBLL.Instance.ReceiveMessage()[0];
                //1 = đăng nhập thành công, 0 = sai mật khẩu, 2 = lỗi khác
                if (status == 1)
                {
                    var ID = BitConverter.ToInt32(TCPClientBLL.Instance.ReceiveMessage());
                    var SoDu = ByteArrayToDecimal(TCPClientBLL.Instance.ReceiveMessage());
                    decimal MoneyPerSecond = ByteArrayToDecimal(TCPClientBLL.Instance.ReceiveMessage());
                    UserHienTai = new(ID, username, password, SoDu, MoneyPerSecond);
                    BoDemGio.Elapsed += BoDemGio_Elapsed;
                    BoDemGio.Start();
                    return null;
                }
                else if (status == 0)
                {
                    return "Sai mật khẩu";
                }
                else if (status == 3)
                {
                    return "Hết thời gian sử dụng";
                }
                else
                {
                    return Encoding.Unicode.GetString(TCPClientBLL.Instance.ReceiveMessage());
                }
            }
            else if (TDNstatus == 0)
            {
                return "Tài khoản không tồn tại";
            }
            else
            {
                return Encoding.ASCII.GetString(TCPClientBLL.Instance.ReceiveMessage());
            }
        }

        private static void BoDemGio_Elapsed(object? sender, System.Timers.ElapsedEventArgs? args)
        {
            UserHienTai!.GiamThoiGian1Giay();
        }

        public static void TaoTaiKhoanAnDanh()
        {

        }

        public static string? TaoTaiKhoan(string user, string password, string fullname)
        {
            TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes("TaoTaiKhoan"));
            TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes(user));
            byte status = TCPClientBLL.Instance.ReceiveMessage()[0];
            if (status == 0) //status = 0 : tài khoản đã tồn tại
                return "Tên đăng nhập đã tồn tại";
            else
            {
                TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes(password));
                TCPClientBLL.Instance.SendMessage(Encoding.Unicode.GetBytes(fullname));
                byte ThanhCong = TCPClientBLL.Instance.ReceiveMessage()[0];
                if (ThanhCong == 1)
                {
                    return null;
                }
                else
                {
                    return Encoding.ASCII.GetString(TCPClientBLL.Instance.ReceiveMessage());
                }
            }
        }
        public static string? DangXuat()
        {
            try
            {
                BoDemGio.Stop();
                BoDemGio.Elapsed -= BoDemGio_Elapsed;
                TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes("DangXuat"));
                TCPClientBLL.Instance.SendMessage(DecimalToByteArray(UserHienTai!.SoDu));
                UserHienTai = null;
                byte status = TCPClientBLL.Instance.ReceiveMessage()[0];
                if (status == 1)
                {
                    return null;
                }
                else
                {
                    return Encoding.ASCII.GetString(TCPClientBLL.Instance.ReceiveMessage());
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            
        }

        public async static Task MockQRNapTien()
        {
            var body = new VietQRRequestBody()
            {
                accountNo = "0942142790",
                accountName = "HOANG HUY HOC",
                acqId = 970422,
                amount = 1000,
                addInfo = $"Gia lap nap tien",
                template = "print"
            };
            string json = JsonSerializer.Serialize(body);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpRequest!.PostAsync("https://api.vietqr.io/v2/generate", content);
        }

        public async static Task<Tuple<string?, string?>> QRNapTien(int amount)
        {
            string? error = null;
            string? result = null;
            try
            {
                var body = new VietQRRequestBody()
                {
                    accountNo = "0942142790",
                    accountName = "HOANG HUY HOC",
                    acqId = 970422,
                    amount = amount,
                    addInfo = $"{DateTime.Now.ToString("dd")} {DateTime.Now.ToString("MM")} {DateTime.Now.ToString("yyyy")}  {DateTime.Now.ToString("HH")} {DateTime.Now.ToString("mm")}   TAIKHOAN {UserHienTai!.TenDangNhap} NAPTIEN",
                    template = "print"
                };
                string json = JsonSerializer.Serialize(body);
                StringContent content = new(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpRequest!.PostAsync("https://api.vietqr.io/v2/generate", content);
                response.EnsureSuccessStatusCode();
                VietQRResponse? responseBody = await response.Content.ReadFromJsonAsync<VietQRResponse>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (responseBody!.code != "00")
                {
                    error = responseBody!.desc!;
                }
                else
                {
                    string qrDataString = responseBody.data!.qrDataURL!;
                    result = Regex.Match(qrDataString, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return new Tuple<string?, string?>(result, error);
        }

        /// <summary>
        /// Gửi thông báo nạp tiền về máy chủ
        /// </summary>
        /// <param name="sotien"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"/>
        async public static Task<string?> NapTien(int sotien, CancellationToken cancellationToken)
        {
            TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes("NapTien"));
            TCPClientBLL.Instance.SendMessage(BitConverter.GetBytes(UserHienTai!.Id));
            TCPClientBLL.Instance.SendMessage(BitConverter.GetBytes(sotien));
            TcpListener listenerForReply = new TcpListener(System.Net.IPAddress.Any, 50000);
            listenerForReply.Start();
            TcpClient server = await listenerForReply.AcceptTcpClientAsync(cancellationToken);
            BinaryReader reader = new BinaryReader(server.GetStream());
            if (TCPClientBLL.Instance.ReceiveMessage(reader)[0] == (byte)1)
            {
                return null;
            }
            else
            {
                return Encoding.ASCII.GetString(TCPClientBLL.Instance.ReceiveMessage(reader));
            }
        }
        public static string? DoiMatKhau(string password)
        {
            TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes("DoiMatKhau"));
            TCPClientBLL.Instance.SendMessage(BitConverter.GetBytes(UserHienTai!.Id));
            TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes(password));
            byte status = TCPClientBLL.Instance.ReceiveMessage()[0];
            if (status == 1)
            {
                return null;
            }
            else return Encoding.ASCII.GetString(TCPClientBLL.Instance.ReceiveMessage());
        }

        public static string MuaDo(string vatpham,int soluong)
        {
            TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes("MuaDo"));
            TCPClientBLL.Instance.SendMessage(BitConverter.GetBytes(UserHienTai!.Id));
            TCPClientBLL.Instance.SendMessage(Encoding.ASCII.GetBytes(vatpham));
            TCPClientBLL.Instance.SendMessage(BitConverter.GetBytes(soluong));
            byte status = TCPClientBLL.Instance.ReceiveMessage()[0];
            if(status == 0)
            {
                return "Loi Dat Hang";
            }
            else
            {
                int total = BitConverter.ToInt32(TCPClientBLL.Instance.ReceiveMessage());
                return "Dat Hang Thanh Cong, So Tien Can Thanh Toan La: "+ total;
            }
        }

        class VietQRResponse
        {
            public string code { get; set; } = null!;
            public string desc { get; set; } = null!;
            public VietQRResponseData? data { get; set; }
        }
        class VietQRResponseData
        {
            public string? qrDataURL { get; set; }
        }
        class VietQRRequestBody
        {
            public string accountNo { get; set; } = null!;
            public string accountName { get; set; } = null!;
            public int acqId { get; set; }
            public int amount { get; set; }
            public string addInfo { get; set; } = null!;
            public string template { get; set; } = null!;
        }
    }
}
