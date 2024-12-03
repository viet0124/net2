namespace ServerGUI
{
    partial class DoiMatKhau
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
            textBox_TenDangNhap = new TextBox();
            label2 = new Label();
            textBox_MatKhau = new TextBox();
            label3 = new Label();
            textBox_HoVaTen = new TextBox();
            button_XacNhan = new Button();
            button_HuyBo = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 21);
            label1.Name = "label1";
            label1.Size = new Size(129, 25);
            label1.TabIndex = 0;
            label1.Text = "Tên đăng nhập";
            // 
            // textBox_TenDangNhap
            // 
            textBox_TenDangNhap.Location = new Point(155, 18);
            textBox_TenDangNhap.Name = "textBox_TenDangNhap";
            textBox_TenDangNhap.ReadOnly = true;
            textBox_TenDangNhap.Size = new Size(150, 31);
            textBox_TenDangNhap.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(21, 121);
            label2.Name = "label2";
            label2.Size = new Size(122, 25);
            label2.TabIndex = 0;
            label2.Text = "Mật khẩu mới";
            // 
            // textBox_MatKhau
            // 
            textBox_MatKhau.CharacterCasing = CharacterCasing.Upper;
            textBox_MatKhau.Location = new Point(155, 118);
            textBox_MatKhau.Name = "textBox_MatKhau";
            textBox_MatKhau.Size = new Size(150, 31);
            textBox_MatKhau.TabIndex = 1;
            textBox_MatKhau.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(21, 72);
            label3.Name = "label3";
            label3.Size = new Size(89, 25);
            label3.TabIndex = 0;
            label3.Text = "Họ và tên";
            // 
            // textBox_HoVaTen
            // 
            textBox_HoVaTen.Location = new Point(155, 69);
            textBox_HoVaTen.Name = "textBox_HoVaTen";
            textBox_HoVaTen.ReadOnly = true;
            textBox_HoVaTen.Size = new Size(150, 31);
            textBox_HoVaTen.TabIndex = 1;
            // 
            // button_XacNhan
            // 
            button_XacNhan.Location = new Point(21, 180);
            button_XacNhan.Name = "button_XacNhan";
            button_XacNhan.Size = new Size(112, 34);
            button_XacNhan.TabIndex = 2;
            button_XacNhan.Text = "Xác nhận";
            button_XacNhan.UseVisualStyleBackColor = true;
            button_XacNhan.Click += button_XacNhan_Click;
            // 
            // button_HuyBo
            // 
            button_HuyBo.Location = new Point(193, 180);
            button_HuyBo.Name = "button_HuyBo";
            button_HuyBo.Size = new Size(112, 34);
            button_HuyBo.TabIndex = 2;
            button_HuyBo.Text = "Huỷ bỏ";
            button_HuyBo.UseVisualStyleBackColor = true;
            button_HuyBo.Click += button_HuyBo_Click;
            // 
            // DoiMatKhau
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(328, 241);
            Controls.Add(button_HuyBo);
            Controls.Add(button_XacNhan);
            Controls.Add(textBox_HoVaTen);
            Controls.Add(textBox_MatKhau);
            Controls.Add(textBox_TenDangNhap);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "DoiMatKhau";
            Text = "Đổi mật khẩu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox_TenDangNhap;
        private Label label2;
        private TextBox textBox_MatKhau;
        private Label label3;
        private TextBox textBox_HoVaTen;
        private Button button_XacNhan;
        private Button button_HuyBo;
    }
}