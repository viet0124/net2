using ClientBLL;
using ServerBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form2 : Form
    {
        private FileSystemWatcher fileWatcher;
        private string filepath = "E:/testclient.txt";
        public Form2()
        {
            InitializeComponent();
            fileWatcher = new FileSystemWatcher();
            fileWatcher.Path = Path.GetDirectoryName(filepath);
            fileWatcher.Filter = Path.GetFileName(filepath);
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite; // Theo dõi thay đổi nội dung
            fileWatcher.Changed += FileWatcher_Changed;
            fileWatcher.EnableRaisingEvents = true;
        }
        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            // Đọc nội dung file và cập nhật RichTextBox
            if (File.Exists(filepath))
            {
                string content = File.ReadAllText(filepath);
                Invoke(new Action(() =>
                {
                    richTextBox1.AppendText(content); // Cập nhật RichTextBox
                }));
                TCPClientChat.Instance.ClearLog(filepath);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            TCPClientChat.Instance.Connect();
            while (true)
            {
                richTextBox1.Text += TCPClientChat.Instance.Receive();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Hay Nhap tin Nhan");
            }
            else
            {
                TCPClientChat.Instance.Send(textBox1.Text);
                richTextBox1.AppendText($"{DateTime.Now}: Client: " + textBox1.Text + "\n");
                textBox1.ResetText();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            TCPClientChat.Instance.ClearLog(filepath);
        }
    }
}
