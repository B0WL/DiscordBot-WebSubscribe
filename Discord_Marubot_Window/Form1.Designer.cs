namespace Discord_Marubot_Window
{
    partial class Form1
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.list_perm = new System.Windows.Forms.ListBox();
            this.btn_perm_input = new System.Windows.Forms.Button();
            this.btn_perm_del = new System.Windows.Forms.Button();
            this.Logbox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.list_list = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_domain = new System.Windows.Forms.TextBox();
            this.tb_title = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_perm = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 539);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(426, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "퍼미션";
            // 
            // list_perm
            // 
            this.list_perm.FormattingEnabled = true;
            this.list_perm.ItemHeight = 12;
            this.list_perm.Location = new System.Drawing.Point(12, 162);
            this.list_perm.Name = "list_perm";
            this.list_perm.Size = new System.Drawing.Size(120, 172);
            this.list_perm.TabIndex = 2;
            this.list_perm.SelectedIndexChanged += new System.EventHandler(this.list_perm_SelectedIndexChanged);
            // 
            // btn_perm_input
            // 
            this.btn_perm_input.Location = new System.Drawing.Point(12, 367);
            this.btn_perm_input.Name = "btn_perm_input";
            this.btn_perm_input.Size = new System.Drawing.Size(52, 22);
            this.btn_perm_input.TabIndex = 3;
            this.btn_perm_input.Text = "추가";
            this.btn_perm_input.UseVisualStyleBackColor = true;
            this.btn_perm_input.Click += new System.EventHandler(this.btn_perm_input_Click);
            // 
            // btn_perm_del
            // 
            this.btn_perm_del.Location = new System.Drawing.Point(80, 367);
            this.btn_perm_del.Name = "btn_perm_del";
            this.btn_perm_del.Size = new System.Drawing.Size(52, 22);
            this.btn_perm_del.TabIndex = 4;
            this.btn_perm_del.Text = "제거";
            this.btn_perm_del.UseVisualStyleBackColor = true;
            this.btn_perm_del.Click += new System.EventHandler(this.btn_perm_del_Click);
            // 
            // Logbox
            // 
            this.Logbox.Location = new System.Drawing.Point(12, 395);
            this.Logbox.Multiline = true;
            this.Logbox.Name = "Logbox";
            this.Logbox.ReadOnly = true;
            this.Logbox.Size = new System.Drawing.Size(426, 138);
            this.Logbox.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Discord_Marubot_Window.Properties.Resources._41221421;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(120, 120);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // list_list
            // 
            this.list_list.FormattingEnabled = true;
            this.list_list.ItemHeight = 12;
            this.list_list.Location = new System.Drawing.Point(140, 85);
            this.list_list.Name = "list_list";
            this.list_list.Size = new System.Drawing.Size(298, 304);
            this.list_list.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(138, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "최근기록";
            // 
            // tb_domain
            // 
            this.tb_domain.Location = new System.Drawing.Point(140, 39);
            this.tb_domain.Name = "tb_domain";
            this.tb_domain.ReadOnly = true;
            this.tb_domain.Size = new System.Drawing.Size(298, 21);
            this.tb_domain.TabIndex = 14;
            this.tb_domain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_title
            // 
            this.tb_title.Location = new System.Drawing.Point(197, 12);
            this.tb_title.Name = "tb_title";
            this.tb_title.ReadOnly = true;
            this.tb_title.Size = new System.Drawing.Size(241, 21);
            this.tb_title.TabIndex = 15;
            this.tb_title.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "검색어/유저/채널";
            // 
            // tb_perm
            // 
            this.tb_perm.Location = new System.Drawing.Point(12, 340);
            this.tb_perm.Name = "tb_perm";
            this.tb_perm.Size = new System.Drawing.Size(119, 21);
            this.tb_perm.TabIndex = 18;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(336, 569);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "저장";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 604);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_perm);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_title);
            this.Controls.Add(this.tb_domain);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.list_list);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Logbox);
            this.Controls.Add(this.btn_perm_del);
            this.Controls.Add(this.btn_perm_input);
            this.Controls.Add(this.list_perm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Name = "Form1";
            this.Text = "Bowl";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox list_perm;
        private System.Windows.Forms.Button btn_perm_input;
        private System.Windows.Forms.Button btn_perm_del;
        private System.Windows.Forms.TextBox Logbox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox list_list;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_title;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_domain;
        private System.Windows.Forms.TextBox tb_perm;
        private System.Windows.Forms.Button button1;
    }
}

