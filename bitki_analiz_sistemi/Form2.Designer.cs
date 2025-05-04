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
            this.btnEkle = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            this.btnGuncelle = new System.Windows.Forms.Button();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.txtCap = new System.Windows.Forms.TextBox();
            this.txtTuyDurumu = new System.Windows.Forms.TextBox();
            this.txtRenk = new System.Windows.Forms.TextBox();
            this.txtDallanma = new System.Windows.Forms.TextBox();
            this.txtYuzey = new System.Windows.Forms.TextBox();
            this.txtUzunluk = new System.Windows.Forms.TextBox();
            this.txtDurus = new System.Windows.Forms.TextBox();
            this.txtNodyum = new System.Windows.Forms.TextBox();
            this.Çap = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBitkiAdi = new System.Windows.Forms.TextBox();
            this.BitkiAdi = new System.Windows.Forms.Label();
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
            this.btnGiris.Location = new System.Drawing.Point(20, 95);
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
            this.listViewBilgiler.Location = new System.Drawing.Point(503, 9);
            this.listViewBilgiler.MultiSelect = false;
            this.listViewBilgiler.Name = "listViewBilgiler";
            this.listViewBilgiler.Size = new System.Drawing.Size(564, 259);
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
            this.pictureBoxBitki.Location = new System.Drawing.Point(210, 9);
            this.pictureBoxBitki.Name = "pictureBoxBitki";
            this.pictureBoxBitki.Size = new System.Drawing.Size(287, 320);
            this.pictureBoxBitki.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBitki.TabIndex = 7;
            this.pictureBoxBitki.TabStop = false;
            // 
            // btnPdfOlustur
            // 
            this.btnPdfOlustur.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnPdfOlustur.Location = new System.Drawing.Point(3, 330);
            this.btnPdfOlustur.Name = "btnPdfOlustur";
            this.btnPdfOlustur.Size = new System.Drawing.Size(130, 23);
            this.btnPdfOlustur.TabIndex = 8;
            this.btnPdfOlustur.Text = "PdfOlustur";
            this.btnPdfOlustur.UseVisualStyleBackColor = true;
            this.btnPdfOlustur.Click += new System.EventHandler(this.btnPdfOlustur_Click);
            // 
            // btnPdfGoster
            // 
            this.btnPdfGoster.Location = new System.Drawing.Point(3, 359);
            this.btnPdfGoster.Name = "btnPdfGoster";
            this.btnPdfGoster.Size = new System.Drawing.Size(130, 27);
            this.btnPdfGoster.TabIndex = 9;
            this.btnPdfGoster.Text = "PdfAç";
            this.btnPdfGoster.UseVisualStyleBackColor = true;
            this.btnPdfGoster.Click += new System.EventHandler(this.btnPdfGoster_Click);
            // 
            // btnEkle
            // 
            this.btnEkle.Location = new System.Drawing.Point(3, 392);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(130, 28);
            this.btnEkle.TabIndex = 10;
            this.btnEkle.Text = "btnEkle";
            this.btnEkle.UseVisualStyleBackColor = true;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // btnSil
            // 
            this.btnSil.Location = new System.Drawing.Point(3, 426);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(130, 28);
            this.btnSil.TabIndex = 11;
            this.btnSil.Text = "btnSil";
            this.btnSil.UseVisualStyleBackColor = true;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // btnGuncelle
            // 
            this.btnGuncelle.Location = new System.Drawing.Point(3, 460);
            this.btnGuncelle.Name = "btnGuncelle";
            this.btnGuncelle.Size = new System.Drawing.Size(130, 28);
            this.btnGuncelle.TabIndex = 12;
            this.btnGuncelle.Text = "btnGuncelle";
            this.btnGuncelle.UseVisualStyleBackColor = true;
            this.btnGuncelle.Click += new System.EventHandler(this.btnGuncelle_Click);
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(3, 494);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(130, 31);
            this.btnKaydet.TabIndex = 13;
            this.btnKaydet.Text = "btnKaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // txtCap
            // 
            this.txtCap.Location = new System.Drawing.Point(747, 285);
            this.txtCap.Name = "txtCap";
            this.txtCap.Size = new System.Drawing.Size(100, 22);
            this.txtCap.TabIndex = 14;
            // 
            // txtTuyDurumu
            // 
            this.txtTuyDurumu.Location = new System.Drawing.Point(1009, 285);
            this.txtTuyDurumu.Name = "txtTuyDurumu";
            this.txtTuyDurumu.Size = new System.Drawing.Size(100, 22);
            this.txtTuyDurumu.TabIndex = 15;
            // 
            // txtRenk
            // 
            this.txtRenk.Location = new System.Drawing.Point(747, 335);
            this.txtRenk.Name = "txtRenk";
            this.txtRenk.Size = new System.Drawing.Size(100, 22);
            this.txtRenk.TabIndex = 16;
            // 
            // txtDallanma
            // 
            this.txtDallanma.Location = new System.Drawing.Point(1009, 335);
            this.txtDallanma.Name = "txtDallanma";
            this.txtDallanma.Size = new System.Drawing.Size(100, 22);
            this.txtDallanma.TabIndex = 17;
            // 
            // txtYuzey
            // 
            this.txtYuzey.Location = new System.Drawing.Point(747, 384);
            this.txtYuzey.Name = "txtYuzey";
            this.txtYuzey.Size = new System.Drawing.Size(100, 22);
            this.txtYuzey.TabIndex = 18;
            // 
            // txtUzunluk
            // 
            this.txtUzunluk.Location = new System.Drawing.Point(1009, 384);
            this.txtUzunluk.Name = "txtUzunluk";
            this.txtUzunluk.Size = new System.Drawing.Size(100, 22);
            this.txtUzunluk.TabIndex = 19;
            // 
            // txtDurus
            // 
            this.txtDurus.Location = new System.Drawing.Point(747, 441);
            this.txtDurus.Name = "txtDurus";
            this.txtDurus.Size = new System.Drawing.Size(100, 22);
            this.txtDurus.TabIndex = 20;
            // 
            // txtNodyum
            // 
            this.txtNodyum.Location = new System.Drawing.Point(1009, 437);
            this.txtNodyum.Name = "txtNodyum";
            this.txtNodyum.Size = new System.Drawing.Size(100, 22);
            this.txtNodyum.TabIndex = 21;
            // 
            // Çap
            // 
            this.Çap.AutoSize = true;
            this.Çap.BackColor = System.Drawing.Color.Tan;
            this.Çap.ForeColor = System.Drawing.Color.Black;
            this.Çap.Location = new System.Drawing.Point(709, 288);
            this.Çap.Name = "Çap";
            this.Çap.Size = new System.Drawing.Size(32, 16);
            this.Çap.TabIndex = 22;
            this.Çap.Text = "Çap";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Tan;
            this.label5.Location = new System.Drawing.Point(702, 338);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 16);
            this.label5.TabIndex = 23;
            this.label5.Text = "Renk";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Tan;
            this.label6.Location = new System.Drawing.Point(697, 387);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 16);
            this.label6.TabIndex = 24;
            this.label6.Text = "Yüzey";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Tan;
            this.label7.Location = new System.Drawing.Point(699, 444);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 16);
            this.label7.TabIndex = 25;
            this.label7.Text = "Duruş";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Tan;
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(927, 288);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 16);
            this.label8.TabIndex = 26;
            this.label8.Text = "TüyDurumu";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Tan;
            this.label9.Location = new System.Drawing.Point(938, 338);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 16);
            this.label9.TabIndex = 27;
            this.label9.Text = "Dallanma";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Tan;
            this.label10.Location = new System.Drawing.Point(949, 387);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 16);
            this.label10.TabIndex = 28;
            this.label10.Text = "Uzunluk";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Tan;
            this.label11.Location = new System.Drawing.Point(945, 440);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 16);
            this.label11.TabIndex = 29;
            this.label11.Text = "Nodyum";
            // 
            // txtBitkiAdi
            // 
            this.txtBitkiAdi.Location = new System.Drawing.Point(883, 474);
            this.txtBitkiAdi.Name = "txtBitkiAdi";
            this.txtBitkiAdi.Size = new System.Drawing.Size(100, 22);
            this.txtBitkiAdi.TabIndex = 30;
            // 
            // BitkiAdi
            // 
            this.BitkiAdi.AutoSize = true;
            this.BitkiAdi.BackColor = System.Drawing.Color.Tan;
            this.BitkiAdi.Location = new System.Drawing.Point(825, 480);
            this.BitkiAdi.Name = "BitkiAdi";
            this.BitkiAdi.Size = new System.Drawing.Size(52, 16);
            this.BitkiAdi.TabIndex = 31;
            this.BitkiAdi.Text = "BitkiAdi";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Tan;
            this.ClientSize = new System.Drawing.Size(1117, 609);
            this.Controls.Add(this.BitkiAdi);
            this.Controls.Add(this.txtBitkiAdi);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Çap);
            this.Controls.Add(this.txtNodyum);
            this.Controls.Add(this.txtDurus);
            this.Controls.Add(this.txtUzunluk);
            this.Controls.Add(this.txtYuzey);
            this.Controls.Add(this.txtDallanma);
            this.Controls.Add(this.txtRenk);
            this.Controls.Add(this.txtTuyDurumu);
            this.Controls.Add(this.txtCap);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.btnGuncelle);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.btnEkle);
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
        private System.Windows.Forms.Button btnEkle;
        private System.Windows.Forms.Button btnSil;
        private System.Windows.Forms.Button btnGuncelle;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.TextBox txtCap;
        private System.Windows.Forms.TextBox txtTuyDurumu;
        private System.Windows.Forms.TextBox txtRenk;
        private System.Windows.Forms.TextBox txtDallanma;
        private System.Windows.Forms.TextBox txtYuzey;
        private System.Windows.Forms.TextBox txtUzunluk;
        private System.Windows.Forms.TextBox txtDurus;
        private System.Windows.Forms.TextBox txtNodyum;
        private System.Windows.Forms.Label Çap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBitkiAdi;
        private System.Windows.Forms.Label BitkiAdi;
    }
}