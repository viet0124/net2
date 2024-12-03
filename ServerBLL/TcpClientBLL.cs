using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServerBLL
{
    public class TcpClientBLL
    {
        public static Dictionary<int, TcpClient> ClientList { get; set; } = new Dictionary<int, TcpClient>();
        public static void AddClient(int IdMay, IPAddress IP)
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(IP, 8889);
            ClientList.Add(IdMay, tcpClient);
        }
        public static void RemoveClients(int IdMay)
        {
            TcpClient? tcpClient = ClientList.GetValueOrDefault(IdMay);
            if (tcpClient != null) tcpClient.Close();
            ClientList.Remove(IdMay);
        }
        internal static void SendData(byte[] message, BinaryWriter writer)
        {
            int len = message.Length;
            writer.Write(len);
            writer.Write(message);
        }

        internal static byte[] ReceiveData(BinaryReader reader)
        {
            int length = reader!.ReadInt32();
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
        internal static void WakeOnLAN(string macAddress)
        {
            // Kiểm tra địa chỉ MAC có hợp lệ hay không (định dạng 6 cặp hexadecimal)
            if (!Regex.IsMatch(macAddress, "^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$"))
            {
                throw new ArgumentException("Địa chỉ MAC không hợp lệ");
            }

            // Loại bỏ ký tự ":" hoặc "-" khỏi địa chỉ MAC
            macAddress = macAddress.Replace(":", "").Replace("-", "");

            // Chuyển địa chỉ MAC từ chuỗi sang mảng byte
            byte[] macBytes = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                macBytes[i] = Convert.ToByte(macAddress.Substring(i * 2, 2), 16);
            }

            // Tạo gói tin Magic Packet
            byte[] magicPacket = new byte[102];

            // Gói tin Magic Packet bắt đầu với 6 byte giá trị 0xFF
            for (int i = 0; i < 6; i++)
            {
                magicPacket[i] = 0xFF;
            }

            // Phần còn lại là địa chỉ MAC lặp lại 16 lần
            for (int i = 1; i <= 16; i++)
            {
                Array.Copy(macBytes, 0, magicPacket, i * 6, 6);
            }

            // Gửi gói tin Magic Packet qua UDP tới địa chỉ broadcast
            using (UdpClient client = new UdpClient())
            {
                client.Connect(IPAddress.Broadcast, 9);  // Cổng 9 thường được dùng cho WOL
                client.Send(magicPacket, magicPacket.Length);
            }
        }
        internal static void TatMay(string IP)
        {
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(IP), 8889);
            BinaryWriter writer = new BinaryWriter(client.GetStream());
            SendData(Encoding.ASCII.GetBytes("Shutdown"), writer);
        }
        internal static void CapNhatSoDu(int IdMay, decimal NewSoTien)
        {
            BinaryWriter writer = new BinaryWriter(ClientList.GetValueOrDefault(IdMay)!.GetStream());
            SendData(Encoding.ASCII.GetBytes("CapNhatSoDu"), writer);
            SendData(DecimalToByteArray(NewSoTien), writer);
        }
        internal static decimal LaySoDu(int IdMay)
        {
            TcpClient client = ClientList.GetValueOrDefault(IdMay)!;
            BinaryWriter writer = new(client.GetStream());
            BinaryReader reader = new(client.GetStream());
            SendData(Encoding.ASCII.GetBytes("LaySoDu"), writer);
            return ByteArrayToDecimal(ReceiveData(reader));
            
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
    }
}
