using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.IO;
using System.Threading;
using DAL;
using DTO;
using System.Net.NetworkInformation;
namespace ServerBLL
{
    public class TCPServerChat
    {
        private TcpListener TcpServer = new TcpListener(IPAddress.Any, 9000);
        private static bool IsServerActive = false;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private static TCPServerChat _instance;
        public List<string> clientIPList;
        public List<string> ClientName;
        public static TCPServerChat Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TCPServerChat();
                return _instance;
            }
        }
        public void StopServer()
        {
            TcpServer.Stop();
        }
        Dictionary<string, TcpClient> connections = new Dictionary<string, TcpClient>();
        public void StartServer()
        {
            TcpServer.Start();
            TcpClient client = TcpServer.AcceptTcpClient();
            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            string clientIp = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            string mac = 
            Pbl4Context context = new Pbl4Context();
            string TenMay = context.TableMay.Single(s => s.DiaChiIpv4 == clientIp).TenMay;
            ClientName.Add(TenMay);
            
            new Thread(() =>
            {
                HandleClient(client/*, clientIp*/);
            }).Start();
            //connections[clientIp] = client;
        }
        private void AddClient(TcpClient client)
        {
            Pbl4Context context = new Pbl4Context();
            


        }
        public void HandleClient(TcpClient client/*, string clientIp*/)
        {
            
            while (true)
            {
                string message = Receive();
                WriteLog(message/*, clientIp*/);
            }
        }
        public static void WriteLog(string message/*, string ip*/)
        {
            try
            {
                // Mở file để ghi thêm (append mode)
               /* Pbl4Context context = new Pbl4Context();
                string TenMay = context.TableMay.Single(s => s.DiaChiIpv4 == ip).TenMay;*/

                using (StreamWriter writer = new StreamWriter($"E:/testserver.txt", true))
                {
                    string logEntry = $"{DateTime.Now}: Client: {message}"; // Tạo nội dung log với thời gian
                    writer.WriteLine(logEntry); // Ghi log vào file
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ghi log: {ex.Message}");
            }
        }
        public void ClearLog(string filepath)
        {
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Truncate))
                { }
            }
            catch
            {

            }
        }
        public void SendData(TcpClient client, string message)
        {
            stream = client.GetStream();
            BinaryWriter writer1 = new BinaryWriter(stream);         
            byte[] mess = Encoding.UTF8.GetBytes(message);
            int len = mess.Length;
            writer1.Write(len);
            writer1.Write(mess);
        }
        public void Send(string message,string tenmay)
        {
            Pbl4Context context = new Pbl4Context();
            string IP = context.TableMay.Single(s => s.TenMay == tenmay).DiaChiIpv4;
            SendData(connections[IP], message);
        }

        public string Receive()
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
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}
