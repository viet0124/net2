using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientBLL
{
    public class TCPListenerBLL
    {
        private TcpListener listener = null!;
        private static TCPListenerBLL? _instance;
        public static TCPListenerBLL Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TCPListenerBLL();
                }
                return _instance;
            }
        }
        
        private TCPListenerBLL()
        {
            listener = new TcpListener(IPAddress.Any, 8889);
        }
        public void Run()
        {
            listener.Start();
            TcpClient serverPC = listener.AcceptTcpClient();
            BinaryReader reader = new BinaryReader(serverPC.GetStream());
            BinaryWriter writer = new BinaryWriter(serverPC.GetStream());
            while (true)
            {
                
                string request = Encoding.ASCII.GetString(ReceiveData(reader));
                switch(request)
                {
                    case "Shutdown":
                        {
                            MayBLL.TatMay();
                            break;
                        }
                    case "CapNhatSoDu":
                        {
                            decimal SoTien = ByteArrayToDecimal(ReceiveData(reader));
                            TaiKhoanBLL.UserHienTai!.Set_SoDu(SoTien);
                            break;
                        }
                    case "LaySoDu":
                        {
                            SendData(DecimalToByteArray(TaiKhoanBLL.UserHienTai!.SoDu), writer);
                            break;
                        }
                    default: break;
                }    
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
        private void SendData(byte[] message, BinaryWriter writer)
        {
            int len = message.Length;
            writer.Write(len);
            writer.Write(message);
        }

        private byte[] ReceiveData(BinaryReader reader)
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
