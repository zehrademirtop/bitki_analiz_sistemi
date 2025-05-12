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
            this.pictureBoxBitki = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBitki)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(74, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Silver;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(7, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "KullanıcıAdı";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Silver;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(26, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Şifre";
            // 
            // txtKullaniciAdi
            // 
            this.txtKullaniciAdi.Location = new System.Drawing.Point(101, 43);
            this.txtKullaniciAdi.Name = "txtKullaniciAdi";
            this.txtKullaniciAdi.Size = new System.Drawing.Size(100, 22);
            this.txtKullaniciAdi.TabIndex = 3;
            // 
            // txtSifre
            // 
            this.txtSifre.Location = new System.Drawing.Point(101, 88);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '*';
            this.txtSifre.Size = new System.Drawing.Size(100, 22);
            this.txtSifre.TabIndex = 4;
            // 
            // btnGiris
            // 
            this.btnGiris.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGiris.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGiris.ForeColor = System.Drawing.Color.ForestGreen;
            this.btnGiris.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGiris.Location = new System.Drawing.Point(10, 126);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(171, 36);
            this.btnGiris.TabIndex = 5;
            this.btnGiris.Text = "GİRİŞ";
            this.btnGiris.UseVisualStyleBackColor = false;
            this.btnGiris.Click += new System.EventHandler(this.btnGiris_Click);
            // 
            // listViewBilgiler
            // 
            this.listViewBilgiler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.listViewBilgiler.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewBilgiler.FullRowSelect = true;
            this.listViewBilgiler.GridLines = true;
            this.listViewBilgiler.HideSelection = false;
            this.listViewBilgiler.Location = new System.Drawing.Point(515, 7);
            this.listViewBilgiler.MultiSelect = false;
            this.listViewBilgiler.Name = "listViewBilgiler";
            this.listViewBilgiler.Size = new System.Drawing.Size(564, 283);
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
            // btnPdfOlustur
            // 
            this.btnPdfOlustur.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnPdfOlustur.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnPdfOlustur.Location = new System.Drawing.Point(3, 527);
            this.btnPdfOlustur.Name = "btnPdfOlustur";
            this.btnPdfOlustur.Size = new System.Drawing.Size(130, 27);
            this.btnPdfOlustur.TabIndex = 8;
            this.btnPdfOlustur.Text = "PdfOlustur";
            this.btnPdfOlustur.UseVisualStyleBackColor = true;
            this.btnPdfOlustur.Click += new System.EventHandler(this.btnPdfOlustur_Click);
            // 
            // btnPdfGoster
            // 
            this.btnPdfGoster.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnPdfGoster.Location = new System.Drawing.Point(151, 527);
            this.btnPdfGoster.Name = "btnPdfGoster";
            this.btnPdfGoster.Size = new System.Drawing.Size(130, 27);
            this.btnPdfGoster.TabIndex = 9;
            this.btnPdfGoster.Text = "PdfAç";
            this.btnPdfGoster.UseVisualStyleBackColor = true;
            this.btnPdfGoster.Click += new System.EventHandler(this.btnPdfGoster_Click);
            // 
            // btnEkle
            // 
            this.btnEkle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEkle.Location = new System.Drawing.Point(151, 570);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(130, 28);
            this.btnEkle.TabIndex = 10;
            this.btnEkle.Text = "btnEkle";
            this.btnEkle.UseVisualStyleBackColor = true;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // btnSil
            // 
            this.btnSil.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSil.Location = new System.Drawing.Point(151, 613);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(130, 28);
            this.btnSil.TabIndex = 11;
            this.btnSil.Text = "btnSil";
            this.btnSil.UseVisualStyleBackColor = true;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // btnGuncelle
            // 
            this.btnGuncelle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGuncelle.Location = new System.Drawing.Point(3, 570);
            this.btnGuncelle.Name = "btnGuncelle";
            this.btnGuncelle.Size = new System.Drawing.Size(130, 28);
            this.btnGuncelle.TabIndex = 12;
            this.btnGuncelle.Text = "btnGuncelle";
            this.btnGuncelle.UseVisualStyleBackColor = true;
            this.btnGuncelle.Click += new System.EventHandler(this.btnGuncelle_Click);
            // 
            // btnKaydet
            // 
            this.btnKaydet.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKaydet.Location = new System.Drawing.Point(3, 613);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(130, 31);
            this.btnKaydet.TabIndex = 13;
            this.btnKaydet.Text = "btnKaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // txtCap
            // 
            this.txtCap.Location = new System.Drawing.Point(750, 296);
            this.txtCap.Name = "txtCap";
            this.txtCap.Size = new System.Drawing.Size(100, 22);
            this.txtCap.TabIndex = 14;
            // 
            // txtTuyDurumu
            // 
            this.txtTuyDurumu.Location = new System.Drawing.Point(979, 296);
            this.txtTuyDurumu.Name = "txtTuyDurumu";
            this.txtTuyDurumu.Size = new System.Drawing.Size(100, 22);
            this.txtTuyDurumu.TabIndex = 15;
            // 
            // txtRenk
            // 
            this.txtRenk.Location = new System.Drawing.Point(751, 329);
            this.txtRenk.Name = "txtRenk";
            this.txtRenk.Size = new System.Drawing.Size(100, 22);
            this.txtRenk.TabIndex = 16;
            // 
            // txtDallanma
            // 
            this.txtDallanma.Location = new System.Drawing.Point(979, 329);
            this.txtDallanma.Name = "txtDallanma";
            this.txtDallanma.Size = new System.Drawing.Size(100, 22);
            this.txtDallanma.TabIndex = 17;
            // 
            // txtYuzey
            // 
            this.txtYuzey.Location = new System.Drawing.Point(750, 366);
            this.txtYuzey.Name = "txtYuzey";
            this.txtYuzey.Size = new System.Drawing.Size(100, 22);
            this.txtYuzey.TabIndex = 18;
            // 
            // txtUzunluk
            // 
            this.txtUzunluk.Location = new System.Drawing.Point(979, 366);
            this.txtUzunluk.Name = "txtUzunluk";
            this.txtUzunluk.Size = new System.Drawing.Size(100, 22);
            this.txtUzunluk.TabIndex = 19;
            // 
            // txtDurus
            // 
            this.txtDurus.Location = new System.Drawing.Point(751, 403);
            this.txtDurus.Name = "txtDurus";
            this.txtDurus.Size = new System.Drawing.Size(100, 22);
            this.txtDurus.TabIndex = 20;
            // 
            // txtNodyum
            // 
            this.txtNodyum.Location = new System.Drawing.Point(979, 403);
            this.txtNodyum.Name = "txtNodyum";
            this.txtNodyum.Size = new System.Drawing.Size(100, 22);
            this.txtNodyum.TabIndex = 21;
            // 
            // Çap
            // 
            this.Çap.AutoSize = true;
            this.Çap.BackColor = System.Drawing.Color.Silver;
            this.Çap.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Çap.ForeColor = System.Drawing.Color.Black;
            this.Çap.Location = new System.Drawing.Point(709, 302);
            this.Çap.Name = "Çap";
            this.Çap.Size = new System.Drawing.Size(35, 16);
            this.Çap.TabIndex = 22;
            this.Çap.Text = "Çap";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Silver;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(702, 338);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 16);
            this.label5.TabIndex = 23;
            this.label5.Text = "Renk";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Silver;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.Location = new System.Drawing.Point(695, 372);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 16);
            this.label6.TabIndex = 24;
            this.label6.Text = "Yüzey";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Silver;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(695, 406);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 16);
            this.label7.TabIndex = 25;
            this.label7.Text = "Duruş";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Silver;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(888, 302);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 16);
            this.label8.TabIndex = 26;
            this.label8.Text = "TüyDurumu";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Silver;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label9.Location = new System.Drawing.Point(900, 335);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 16);
            this.label9.TabIndex = 27;
            this.label9.Text = "Dallanma";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Silver;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label10.Location = new System.Drawing.Point(912, 372);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 16);
            this.label10.TabIndex = 28;
            this.label10.Text = "Uzunluk";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Silver;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(909, 409);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 16);
            this.label11.TabIndex = 29;
            this.label11.Text = "Nodyum";
            // 
            // txtBitkiAdi
            // 
            this.txtBitkiAdi.Location = new System.Drawing.Point(850, 442);
            this.txtBitkiAdi.Name = "txtBitkiAdi";
            this.txtBitkiAdi.Size = new System.Drawing.Size(100, 22);
            this.txtBitkiAdi.TabIndex = 30;
            // 
            // BitkiAdi
            // 
            this.BitkiAdi.AutoSize = true;
            this.BitkiAdi.BackColor = System.Drawing.Color.Silver;
            this.BitkiAdi.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BitkiAdi.Location = new System.Drawing.Point(784, 448);
            this.BitkiAdi.Name = "BitkiAdi";
            this.BitkiAdi.Size = new System.Drawing.Size(60, 16);
            this.BitkiAdi.TabIndex = 31;
            this.BitkiAdi.Text = "BitkiAdi";
            // 
            // pictureBoxBitki
            // 
            this.pictureBoxBitki.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pictureBoxBitki.Location = new System.Drawing.Point(217, 7);
            this.pictureBoxBitki.Name = "pictureBoxBitki";
            this.pictureBoxBitki.Size = new System.Drawing.Size(292, 283);
            this.pictureBoxBitki.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBitki.TabIndex = 7;
            this.pictureBoxBitki.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BackgroundImage = global::bitki_analiz_sistemi.Properties.Resources.istockphoto_504616634_612x6124;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1090, 660);
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