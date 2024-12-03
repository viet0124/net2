namespace ServerGUI.QuanLyMay
{
    partial class ThemMay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            textBox_DiaChiIP = new TextBox();
            textBox_DiaChiMAC = new TextBox();
            comboBox_LoaiMay = new ComboBox();
            button_XacNhan = new Button();
            button_Huy = new Button();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 9);
            label2.Name = "label2";
            label2.Size = new Size(83, 25);
            label2.TabIndex = 0;
            label2.Text = "Loại máy";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 54);
            label3.Name = "label3";
            label3.Size = new Size(85, 25);
            label3.TabIndex = 0;
            label3.Text = "Địa chỉ IP";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 98);
            label4.Name = "label4";
            label4.Size = new Size(109, 25);
            label4.TabIndex = 0;
            label4.Text = "Địa chỉ MAC";
            // 
            // textBox_DiaChiIP
            // 
            textBox_DiaChiIP.Location = new Point(167, 51);
            textBox_DiaChiIP.Name = "textBox_DiaChiIP";
            textBox_DiaChiIP.Size = new Size(257, 31);
            textBox_DiaChiIP.TabIndex = 1;
            // 
            // textBox_DiaChiMAC
            // 
            textBox_DiaChiMAC.Location = new Point(167, 95);
            textBox_DiaChiMAC.Name = "textBox_DiaChiMAC";
            textBox_DiaChiMAC.Size = new Size(257, 31);
            textBox_DiaChiMAC.TabIndex = 1;
            // 
            // comboBox_LoaiMay
            // 
            comboBox_LoaiMay.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_LoaiMay.FormattingEnabled = true;
            comboBox_LoaiMay.Location = new Point(167, 6);
            comboBox_LoaiMay.Name = "comboBox_LoaiMay";
            comboBox_LoaiMay.Size = new Size(257, 33);
            comboBox_LoaiMay.TabIndex = 2;
            // 
            // button_XacNhan
            // 
            button_XacNhan.Location = new Point(30, 161);
            button_XacNhan.Name = "button_XacNhan";
            button_XacNhan.Size = new Size(112, 34);
            button_XacNhan.TabIndex = 3;
            button_XacNhan.Text = "Xác nhận";
            button_XacNhan.UseVisualStyleBackColor = true;
            button_XacNhan.Click += button_XacNhan_Click;
            // 
            // button_Huy
            // 
            button_Huy.Location = new Point(312, 161);
            button_Huy.Name = "button_Huy";
            button_Huy.Size = new Size(112, 34);
            button_Huy.TabIndex = 3;
            button_Huy.Text = "Huỷ";
            button_Huy.UseVisualStyleBackColor = true;
            button_Huy.Click += button_Huy_Click;
            // 
            // ThemMay
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 226);
            Controls.Add(button_Huy);
            Controls.Add(button_XacNhan);
            Controls.Add(comboBox_LoaiMay);
            Controls.Add(textBox_DiaChiMAC);
            Controls.Add(textBox_DiaChiIP);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Name = "ThemMay";
            Text = "ThemMay";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBox_DiaChiIP;
        private TextBox textBox_DiaChiMAC;
        private ComboBox comboBox_LoaiMay;
        private Button button_XacNhan;
        private Button button_Huy;
    }
}