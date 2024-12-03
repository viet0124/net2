using ClientBLL;

namespace testClientChat
{
    public partial class Form1 : Form
    {
        private FileSystemWatcher fileWatcher;
        private string filepath = "E:/testclient.txt";
        public Form1()
        {
            InitializeComponent();
            fileWatcher = new FileSystemWatcher();
            fileWatcher.Path = Path.GetDirectoryName(filepath);
            fileWatcher.Filter = Path.GetFileName(filepath);
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite; // Theo dõi thay ??i n?i dung
            fileWatcher.Changed += FileWatcher_Changed;
            fileWatcher.EnableRaisingEvents = true;
        }
        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            // ??c n?i dung file và c?p nh?t RichTextBox
            if (File.Exists(filepath))
            {
                string content = File.ReadAllText(filepath);
                Invoke(new Action(() =>
                {
                    richTextBox1.AppendText(content); // C?p nh?t RichTextBox
                }));
                TCPClientChat.Instance.ClearLog(filepath);
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
    }
}
