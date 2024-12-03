namespace ServerGUI
{
    partial class ThemTaiKhoan
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBox_TenDangNhap = new TextBox();
            textBox_MatKhau = new TextBox();
            textBox_HoVaTen = new TextBox();
            button_Tao = new Button();
            button_DatLai = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(129, 25);
            label1.TabIndex = 0;
            label1.Text = "Tên đăng nhập";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 59);
            label2.Name = "label2";
            label2.Size = new Size(86, 25);
            label2.TabIndex = 1;
            label2.Text = "Mật khẩu";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 106);
            label3.Name = "label3";
            label3.Size = new Size(89, 25);
            label3.TabIndex = 2;
            label3.Text = "Họ và tên";
            // 
            // textBox_TenDangNhap
            // 
            textBox_TenDangNhap.CharacterCasing = CharacterCasing.Upper;
            textBox_TenDangNhap.Location = new Point(147, 9);
            textBox_TenDangNhap.Name = "textBox_TenDangNhap";
            textBox_TenDangNhap.Size = new Size(191, 31);
            textBox_TenDangNhap.TabIndex = 3;
            // 
            // textBox_MatKhau
            // 
            textBox_MatKhau.CharacterCasing = CharacterCasing.Upper;
            textBox_MatKhau.Location = new Point(147, 56);
            textBox_MatKhau.Name = "textBox_MatKhau";
            textBox_MatKhau.Size = new Size(191, 31);
            textBox_MatKhau.TabIndex = 3;
            textBox_MatKhau.UseSystemPasswordChar = true;
            // 
            // textBox_HoVaTen
            // 
            textBox_HoVaTen.Location = new Point(147, 103);
            textBox_HoVaTen.Name = "textBox_HoVaTen";
            textBox_HoVaTen.Size = new Size(191, 31);
            textBox_HoVaTen.TabIndex = 3;
            // 
            // button_Tao
            // 
            button_Tao.Location = new Point(13, 163);
            button_Tao.Name = "button_Tao";
            button_Tao.Size = new Size(112, 34);
            button_Tao.TabIndex = 4;
            button_Tao.Text = "Tạo mới";
            button_Tao.UseVisualStyleBackColor = true;
            button_Tao.Click += button_Tao_Click;
            // 
            // button_DatLai
            // 
            button_DatLai.Location = new Point(226, 163);
            button_DatLai.Name = "button_DatLai";
            button_DatLai.Size = new Size(112, 34);
            button_DatLai.TabIndex = 4;
            button_DatLai.Text = "Đặt lại";
            button_DatLai.UseVisualStyleBackColor = true;
            button_DatLai.Click += button_DatLai_Click;
            // 
            // ThemTaiKhoan
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(350, 211);
            Controls.Add(button_DatLai);
            Controls.Add(button_Tao);
            Controls.Add(textBox_HoVaTen);
            Controls.Add(textBox_MatKhau);
            Controls.Add(textBox_TenDangNhap);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ThemTaiKhoan";
            Text = "Tạo tài khoản mới";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox_TenDangNhap;
        private TextBox textBox_MatKhau;
        private TextBox textBox_HoVaTen;
        private Button button_Tao;
        private Button button_DatLai;
    }
}