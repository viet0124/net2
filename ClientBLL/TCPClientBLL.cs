using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;

namespace ClientBLL
{
    public class TCPClientBLL
    {
        private TcpClient client = null!;
        private static TCPClientBLL? _instance;
        private NetworkStream? stream;
        private BinaryReader? reader;
        private BinaryWriter? writer;
        private TCPClientBLL()
        {
            client = new TcpClient();
            
        }
        public static TCPClientBLL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TCPClientBLL();
                }
                return _instance;
            }
        }
        public void Close()
        {
            client.Close();
        }
        public void Connect()
        {
            client.Connect(IPAddress.Loopback, 8888);
            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            //string MAC = GetMacAddress(client);
            //SendMessage(Encoding.ASCII.GetBytes(MAC));
        }
        //public string GetMacAddress(TcpClient tcpClient)
        //{

        //    // Get the local IP address
        //    var localIpAddress = ((IPEndPoint)tcpClient.Client.LocalEndPoint!).Address;

        //    // Find the network interface associated with the local IP address
        //    var networkInterface = NetworkInterface.GetAllNetworkInterfaces()
        //        .FirstOrDefault(ni => ni.GetIPProperties().UnicastAddresses
        //            .Any(ua => ua.Address.Equals(localIpAddress)));

        //    // Get the MAC address
        //    var macAddressBytes = networkInterface!.GetPhysicalAddress().GetAddressBytes();
        //    var macAddress = string.Join(":", macAddressBytes.Select(b => b.ToString("X2")));

        //    return macAddress;
        //}
        public void SendMessage(byte[] message)
        {
            int len = message.Length;
            writer!.Write(len);
            writer.Write(message);
        }

        public byte[] ReceiveMessage()
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
        public void SendMessage(byte[] message, BinaryWriter writer)
        {
            int len = message.Length;
            writer.Write(len);
            writer.Write(message);
        }

        public byte[] ReceiveMessage(BinaryReader reader)
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
    }
}
