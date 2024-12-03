
using Microsoft.IdentityModel.Tokens;
using ServerBLL;

namespace test
{
    public partial class Form1 : Form
    {
        private FileSystemWatcher fileWatcher;
        private string filepath = "E:/testserver.txt";
        public Form1()
        {
            InitializeComponent();
            //setComboBox();
            fileWatcher = new FileSystemWatcher();
            fileWatcher.Path = Path.GetDirectoryName(filepath);
            fileWatcher.Filter = Path.GetFileName(filepath);
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite; 
            fileWatcher.Changed += FileWatcher_Changed;
            fileWatcher.EnableRaisingEvents = true;
        }
        private void setComboBox()
        {
            // L?y danh sách các tên client
            List<string> items = TCPServerChat.Instance.ClientName;

            // Xóa các m?c hi?n t?i trong comboBox ?? tránh trùng l?p
            comboBox1.Items.Clear();

            // Thêm t?ng m?c vào comboBox
            if(!items.IsNullOrEmpty())
            {
                try
                {
                    foreach (var item in items)
                    {
                        comboBox1.Items.Add(item);
                    }
                }
                catch { }
            }
        }
        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            // ??c n?i dung file và c?p nh?t RichTextBox
            //filepath = $"E:/{comboBox1.SelectedItem?.ToString()}.txt";
            if (File.Exists(filepath))
            {
                string content = File.ReadAllText(filepath);
                Invoke(new Action(() =>
                {
                    richTextBox1.AppendText(content); // C?p nh?t RichTextBox
                }));
                TCPServerChat.Instance.ClearLog(filepath);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Hay Nhap tin Nhan");
            }
            else
            {
                string ChosenIP = comboBox1.SelectedItem.ToString();
                TCPServerChat.Instance.Send(textBox1.Text, ChosenIP);
                richTextBox1.AppendText($"{DateTime.Now}: Server: " + textBox1.Text + "\n");
                textBox1.ResetText();
            }
        }
    }
}
