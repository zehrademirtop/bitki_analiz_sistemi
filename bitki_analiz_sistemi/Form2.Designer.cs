namespace bitki_analiz_sistemi
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKullaniciAdi = new System.Windows.Forms.TextBox();
            this.txtSifre = new System.Windows.Forms.TextBox();
            this.btnGiris = new System.Windows.Forms.Button();
            this.listViewBilgiler = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureBoxBitki = new System.Windows.Forms.PictureBox();
            this.btnPdfOlustur = new System.Windows.Forms.Button();
            this.btnPdfGoster = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBitki)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(56, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "KullanıcıAdı";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Maroon;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Şifre";
            // 
            // txtKullaniciAdi
            // 
            this.txtKullaniciAdi.Location = new System.Drawing.Point(94, 35);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.Size = new System.Drawing.Size(100, 22);
            this.txtKullaniciAdi.TabIndex = 3;
            // 
            // txtSifre
            // 
            this.txtSifre.Location = new System.Drawing.Point(94, 67);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '*';
            this.txtSifre.Size = new System.Drawing.Size(100, 22);
            this.txtSifre.TabIndex = 4;
            // 
            // btnGiris
            // 
            this.btnGiris.Location = new System.Drawing.Point(36, 95);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(113, 24);
            this.btnGiris.TabIndex = 5;
            this.btnGiris.Text = "Giriş";
            this.btnGiris.UseVisualStyleBackColor = true;
            this.btnGiris.Click += new System.EventHandler(this.btnGiris_Click);
            // 
            // listViewBilgiler
            // 
            this.listViewBilgiler.BackColor = System.Drawing.Color.DarkKhaki;
            this.listViewBilgiler.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewBilgiler.FullRowSelect = true;
            this.listViewBilgiler.GridLines = true;
            this.listViewBilgiler.HideSelection = false;
            this.listViewBilgiler.Location = new System.Drawing.Point(421, 9);
            this.listViewBilgiler.MultiSelect = false;
            this.listViewBilgiler.Name = "listViewBilgiler";
            this.listViewBilgiler.Size = new System.Drawing.Size(590, 518);
            this.listViewBilgiler.TabIndex = 6;
            this.listViewBilgiler.UseCompatibleStateImageBehavior = false;
            this.listViewBilgiler.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Özellik";
            this.columnHeader1.Width = 250;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Değer";
            this.columnHeader2.Width = 300;
            // 
            // pictureBoxBitki
            // 
            this.pictureBoxBitki.Location = new System.Drawing.Point(200, 9);
            this.pictureBoxBitki.Name = "pictureBoxBitki";
            this.pictureBoxBitki.Size = new System.Drawing.Size(215, 216);
            this.pictureBoxBitki.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBitki.TabIndex = 7;
            this.pictureBoxBitki.TabStop = false;
            // 
            // btnPdfOlustur
            // 
            this.btnPdfOlustur.Location = new System.Drawing.Point(3, 273);
            this.btnPdfOlustur.Name = "btnPdfOlustur";
            this.btnPdfOlustur.Size = new System.Drawing.Size(130, 23);
            this.btnPdfOlustur.TabIndex = 8;
            this.btnPdfOlustur.Text = "PdfOlustur";
            this.btnPdfOlustur.UseVisualStyleBackColor = true;
            this.btnPdfOlustur.Click += new System.EventHandler(this.btnPdfOlustur_Click);
            // 
            // btnPdfGoster
            // 
            this.btnPdfGoster.Location = new System.Drawing.Point(3, 302);
            this.btnPdfGoster.Name = "btnPdfGoster";
            this.btnPdfGoster.Size = new System.Drawing.Size(130, 27);
            this.btnPdfGoster.TabIndex = 9;
            this.btnPdfGoster.Text = "PdfAç";
            this.btnPdfGoster.UseVisualStyleBackColor = true;
            this.btnPdfGoster.Click += new System.EventHandler(this.btnPdfGoster_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Tan;
            this.ClientSize = new System.Drawing.Size(1023, 542);
            this.Controls.Add(this.btnPdfGoster);
            this.Controls.Add(this.btnPdfOlustur);
            this.Controls.Add(this.pictureBoxBitki);
            this.Controls.Add(this.listViewBilgiler);
            this.Controls.Add(this.btnGiris);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.txtKullaniciAdi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBitki)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKullaniciAdi;
        private System.Windows.Forms.TextBox txtSifre;
        private System.Windows.Forms.Button btnGiris;
        private System.Windows.Forms.ListView listViewBilgiler;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.PictureBox pictureBoxBitki;
        private System.Windows.Forms.Button btnPdfOlustur;
        private System.Windows.Forms.Button btnPdfGoster;
    }
}