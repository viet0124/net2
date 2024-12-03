using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ClientBLL
{
    public class TCPClientChat
    {
        private TcpClient client = null;
        private static TCPClientChat _instance;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        private TCPClientChat()
        {
            client = new TcpClient();
        }
        public static TCPClientChat Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TCPClientChat();
                return _instance;
            }
        }

        public void Connect()
        {
            client.Connect(IPAddress.Loopback, 9000);
            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        string message = Receive();
                        WriteLog(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in receiving: {ex.Message}");
                }
            }).Start();
        }

        public void WriteLog(string message)
        {
            try
            {
                // Mở file để ghi thêm (append mode)
                using (StreamWriter writer = new StreamWriter("E:/testclient.txt", true))
                {
                    string logEntry = $"{DateTime.Now}: Server: {message}"; // Tạo nội dung log với thời gian
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
        public void Send(string message)
        {
            byte[] mess = Encoding.UTF8.GetBytes(message);
            int len = mess.Length;
            writer.Write(len);
            writer.Write(mess);
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
