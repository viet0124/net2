using ServerBLL;
using DTO;
using System.Collections.ObjectModel;

namespace ServerGUI.QuanLyMay
{
    public partial class ThemMay : Form
    {
        //private ObservableCollection<LoaiMay> comboBox_LoaiMay_Source = new ObservableCollection<LoaiMay>();
        public ThemMay()
        {
            InitializeComponent();
            comboBox_LoaiMay_Load();
        }

        private void comboBox_LoaiMay_Load()
        {
            List<string> list = MayBLL.TatCaTenLoaiMay();
            comboBox_LoaiMay.DataSource = list;
            comboBox_LoaiMay.SelectedIndex = 0;
        }

        private void button_XacNhan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_DiaChiIP.Text) || string.IsNullOrEmpty(textBox_DiaChiMAC.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };

            if (!MayBLL.ThemMayMoi(textBox_DiaChiIP.Text, textBox_DiaChiMAC.Text, (string)comboBox_LoaiMay.SelectedItem!, out string error))
            {
                MessageBox.Show(error);
            }
            else this.Close();
        }

        private void button_Huy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
