using System.Text;
using System.Net;
using System.Net.Sockets;
using DTO;
using DAL;
using System.Reflection.PortableExecutable;

namespace ServerBLL
{
    public static class TcpServerBLL
    {
        private static readonly TcpListener TcpServer = new TcpListener(IPAddress.Any, 8888);
        private static bool IsServerActive = false;
        public static void StopServer()
        {
            IsServerActive = false;
            TcpServer.Stop();
        }
        private static void SendData(byte[] message, BinaryWriter writer)
        {
            int len = message.Length;
            writer.Write(len);
            writer.Write(message);
        }
        private static byte[] ReceiveData(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            byte[] buffer = new byte[(1024 >= length) ? length : 1024];
            MemoryStream memoryStream = new MemoryStream();
            while (length > 0)
            {
                int read = reader.Read(buffer, 0, (buffer.Length < length) ? buffer.Length : length);
                memoryStream.Write(buffer, 0, read);
                length -= read;
            }
            return memoryStream.ToArray();
        }
        public static void Run()
        {
            TcpServer.Start();
            IsServerActive = true;
            while (IsServerActive)
            {
                TcpClient client = TcpServer.AcceptTcpClient();
                new Thread(() =>
                {
                    HandleClient(client);
                }).Start();
            }
        }
        private static void HandleClient(TcpClient client)
        {
            CancellationTokenSource cancelTokenNapTien = new CancellationTokenSource();
            Pbl4Context context = new Pbl4Context();
            May may = context.TableMay.Single(may => may.DiaChiIpv4 == ((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString());
            may.IdTinhTrang = 2;
            TcpClientBLL.AddClient(may.Id, IPAddress.Parse(may.DiaChiIpv4));

            context.SaveChanges();
            context.Dispose();
            NetworkStream clientStream = client.GetStream();
            using (BinaryReader reader = new BinaryReader(clientStream))
            using (BinaryWriter writer = new BinaryWriter(clientStream))
            {
                try
                {
                    while (true)
                    {
                        string header = Encoding.ASCII.GetString(ReceiveData(reader));
                        switch (header)
                        {
                            case "LayMatKhau":
                                {
                                    LayMatKhau(reader, writer);
                                    break;
                                }
                            case "DangNhap":
                                {
                                    DangNhap(client, reader, writer);
                                    break;
                                }

                            case "DangXuat":
                                {
                                    DangXuat(client, reader, writer);
                                    break;
                                }

                            case "TaoTaiKhoan":
                                {
                                    TaoTaiKhoan(reader, writer);
                                    break;
                                }

                            case "DoiMatKhau":
                                {
                                    DoiMatKhau(reader, writer);
                                    break;
                                }
                            case "MuaDo":
                                {
                                    MuaDo(client, reader, writer);
                                    break;
                                }
                            /*case "NhanTinNhan":
                                {
                                    ServerChatBLL.Instance.WriteLog(Encoding.ASCII.GetString(ReceiveData(reader)));
                                    break;
                                }*/
                            case "NapTien":
                                {
                                    cancelTokenNapTien = new(120000);
                                    NapTien(client, reader, cancelTokenNapTien.Token);
                                    break;
                                }
                            case "HuyNapTien":
                                {
                                    HuyNapTien(cancelTokenNapTien);
                                    break;
                                }
                            case "TaoTaiKhoanAnDanh":
                                {
                                    string IPAddress = ((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString();
                                    TaiKhoan? TaiKhoanAnDanh = AccountBLL.LayTaiKhoan(null, IPAddress, out string error);
                                    if (!string.IsNullOrEmpty(error)) //Có lỗi trong quá trình tìm kiếm
                                    {
                                        SendData([2], writer);
                                        SendData(Encoding.ASCII.GetBytes(error), writer);
                                    }
                                    else if (TaiKhoanAnDanh == null) //Chưa có tài khoản
                                    {
                                        if (AccountBLL.TaoTaiKhoan(IPAddress, IPAddress, "Ẩn danh", "Khách hàng ẩn danh", out error))
                                        {   //tao thanh cong
                                            TaiKhoan? taiKhoan = AccountBLL.LayTaiKhoan(null, IPAddress, out error);
                                            if (!string.IsNullOrEmpty(error))
                                            {
                                                SendData([2], writer);
                                                SendData(Encoding.ASCII.GetBytes(error), writer);
                                                break;
                                            }
                                            bool DangNhapThanhCong = AccountBLL.DangNhap(taiKhoan!.Id, IPAddress, out error);
                                            if (DangNhapThanhCong)
                                            {

                                            }
                                        }
                                        else //lỗi khác
                                        {

                                        }
                                    }
                                    else
                                    {
                                        SendData([1], writer);
                                    }
                                    break;
                                }

                            default:
                                SendData(Encoding.ASCII.GetBytes("Yeu cau khong hop le"), writer);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error handling client: " + ex.Message);
                }
            }

        }

        private static void MuaDo(TcpClient client, BinaryReader reader, BinaryWriter writer)
        {
            int IdTaiKhoan = BitConverter.ToInt32(ReceiveData(reader));
            Pbl4Context context = new Pbl4Context();     
            int MayMuaHang = context.TableDangHoatDong.Single(s => s.IdTaiKhoan == IdTaiKhoan).IdMay;
            string MatHang = Encoding.ASCII.GetString(ReceiveData(reader));
            int SoLuong = BitConverter.ToInt32(ReceiveData(reader));
            int ThanhToan = AccountBLL.MuaDo(MatHang, SoLuong);
            if(ThanhToan == 0)
            {
                SendData([0], writer);
            }
            else
            {
                int id = context.TableGiaoDich.Count() > 0 ? context.TableTaiKhoan.Max(tk => tk.Id) + 1 : 1;
                context.TableGiaoDich.Add(new GiaoDich
                {
                    Id = id,
                    IdTaiKhoan = IdTaiKhoan,
                    ThoiGian = DateTime.Now,
                    SoTien = ThanhToan,
                    NoiDungGiaoDich = "Mua " + MatHang
                });
                context.SaveChanges();
                context.Dispose();
                SendData([1], writer);
                SendData(BitConverter.GetBytes(ThanhToan), writer);
            }
        }
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

        private static void LayMatKhau(BinaryReader reader, BinaryWriter writer)
        {
            int id = BitConverter.ToInt32(ReceiveData(reader));
            TaiKhoan taiKhoan = AccountBLL.LayTaiKhoan(id, null, out string error)!;
            SendData(Encoding.ASCII.GetBytes(taiKhoan.MatKhau), writer);
        }
        private static void DangNhap(TcpClient client, BinaryReader reader, BinaryWriter writer)
        {
            string usernameDN = Encoding.ASCII.GetString(ReceiveData(reader));
            TaiKhoan? taiKhoan = AccountBLL.LayTaiKhoan(null, usernameDN, out string error);
            // 0 = tài khoản không tồn tại, 1 = tài khoản tồn tại, 2 = lỗi khác
            if (!string.IsNullOrEmpty(error))
            {
                SendData([2], writer);
                SendData(Encoding.ASCII.GetBytes(error), writer);
            }
            else if (taiKhoan == null)
            {
                SendData([0], writer);
            }
            else
            {
                SendData([1], writer);
                string passwordDN = Encoding.ASCII.GetString(ReceiveData(reader));

                if (passwordDN != taiKhoan.MatKhau)
                {
                    SendData([0], writer); //0 = sai mật khẩu
                }
                else if (AccountBLL.DangNhap(taiKhoan.Id, ((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString(), out error))
                {
                    SendData([1], writer); //1 = đăng nhập thành công
                    SendData(BitConverter.GetBytes(taiKhoan.Id), writer);
                    SendData(DecimalToByteArray(taiKhoan.SoDu), writer);
                    SendData(DecimalToByteArray(AccountBLL.MoneyPerSecond), writer);
                }
                else if (!error.Equals("Hết thời gian sử dụng")) // 2 = lỗi trong quá trình đăng nhập
                {
                    SendData([2], writer);
                    SendData(Encoding.ASCII.GetBytes(error), writer);
                }
                else
                {
                    SendData([3], writer); //3 = hết thời gian
                }
            }
        }
        private static void TaoTaiKhoan(BinaryReader reader, BinaryWriter writer)
        {
            string username = Encoding.ASCII.GetString(ReceiveData(reader));
            if (AccountBLL.TenDangNhapDaTonTai(username))
            {// true : tài khoản đã tồn tại, gửi về giá trị 0 
                SendData([0], writer);
                return;
            }
            else SendData([1], writer);
            string password = Encoding.ASCII.GetString(ReceiveData(reader));
            string fullname = Encoding.Unicode.GetString(ReceiveData(reader));
            // Gọi hàm tạo tài khoản
            bool ThanhCong = AccountBLL.TaoTaiKhoan(username, password, fullname, "Khách hàng", out string error);
            if (ThanhCong)
            {
                SendData([1], writer);

            }
            else
            {
                SendData([0], writer);
                SendData(Encoding.ASCII.GetBytes(error), writer);
            }
        }
        private static void DangXuat(TcpClient client, BinaryReader reader, BinaryWriter writer)
        {
            decimal SoDu = ByteArrayToDecimal(ReceiveData(reader));
            string IP = ((IPEndPoint)client.Client.RemoteEndPoint!).Address.ToString();
            if (AccountBLL.DangXuat(IP, SoDu, out string error))
            {
                SendData([1], writer);
            }
            else
            {
                SendData([0], writer);
                SendData(Encoding.ASCII.GetBytes(error), writer);
            }
        }

        private static void NapTien(TcpClient client, BinaryReader reader, CancellationToken cancelTokenNapTien)
        {
            int userID = BitConverter.ToInt32(ReceiveData(reader));
            int SoTien = BitConverter.ToInt32(ReceiveData(reader));
            new Task(() =>
            {
                Task<string?> TaskKetQua = AccountBLL.AwaitingMoney(userID, SoTien, cancelTokenNapTien);
                TaskKetQua.Wait();
                string? KetQua = TaskKetQua.Result;

                if (!string.Equals(KetQua, "TimeoutOrCancelled"))
                {
                    TcpClient client1 = new TcpClient();
                    client1.Connect(new IPEndPoint((client.Client.RemoteEndPoint as IPEndPoint)!.Address, 50000));
                    var writer1 = new BinaryWriter(client1.GetStream());
                    if (string.IsNullOrEmpty(KetQua))
                    {
                        SendData([1], writer1);
                    }
                    else
                    {
                        SendData([0], writer1);
                        SendData(Encoding.ASCII.GetBytes(KetQua), writer1);
                    }
                }
            }, cancelTokenNapTien, TaskCreationOptions.LongRunning).Start();
        }
        private static void HuyNapTien(CancellationTokenSource cancellationToken)
        {
            cancellationToken.Cancel();
        }
        private static void DoiMatKhau(BinaryReader reader, BinaryWriter writer)
        {
            int IdTaiKhoan = BitConverter.ToInt32(ReceiveData(reader));
            string newPassword = Encoding.ASCII.GetString(ReceiveData(reader));
            // Gọi hàm đổi mật khẩu
            if (AccountBLL.DoiMatKhau(IdTaiKhoan, newPassword, out string error))
                SendData([1], writer);
            else
            {
                SendData([0], writer);
                SendData(Encoding.ASCII.GetBytes(error), writer);
            }
        }
    }
}
