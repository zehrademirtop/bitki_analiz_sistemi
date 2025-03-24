﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace bitki_analiz_sistemi
{
    public partial class Form2 : Form
    {
        public string SecilenBitkiAdi { get; set; }
        public string SecilenYuzey { get; set; }
        public string SecilenDallanma { get; set; }
        public string SecilenCap { get; set; }
        public string SecilenNodyum { get; set; }

        public Form2()
        {

            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string dogruKullaniciAdi = "admin";
            string dogruSifre = "1234";

            string girilenKullaniciAdi = txtKullaniciAdi.Text.Trim();
            string girilenSifre = txtSifre.Text.Trim();

            if (IsGirisDogru(girilenKullaniciAdi, girilenSifre, dogruKullaniciAdi, dogruSifre))
            {
                MessageBox.Show("Giriş başarılı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 📌 ListView'e verileri ekle
                GuncelleListView();

                // 📌 Bitki adına göre resim göster
                ShowBitkiImage(SecilenBitkiAdi); // Burada bitki resmini yükle
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre!", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSifre.Clear();
                txtSifre.Focus();
            }
        }

        // 📌 Giriş bilgilerini kontrol eden fonksiyon
        private bool IsGirisDogru(string girilenKullaniciAdi, string girilenSifre, string dogruKullaniciAdi, string dogruSifre)
        {
            return girilenKullaniciAdi.Equals(dogruKullaniciAdi, StringComparison.OrdinalIgnoreCase) && girilenSifre == dogruSifre;
        }

        // 📌 ListView'e verileri eklemek için fonksiyon
        private void GuncelleListView()
        {
            listViewBilgiler.Items.Clear(); // Önce ListView'i temizle

            listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Bitki Adı", SecilenBitkiAdi }));
            listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Yüzey", SecilenYuzey }));
            listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Dallanma", SecilenDallanma }));
            listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Çap", SecilenCap }));
            listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Nodyum", SecilenNodyum }));
        }

        // 📌 Seçilen bitkiye göre resmi gösteren fonksiyon
        private void ShowBitkiImage(string bitkiAdi)
        {
            // Resimlerin bulunduğu klasör
            string resimKlasoru = Path.Combine(Application.StartupPath, "resimler");

            // Resim dosyasının yolunu oluştur
            string resimYolu = Path.Combine(resimKlasoru, $"{bitkiAdi}.png");

            Console.WriteLine($"Kontrol edilen resim yolu: {resimYolu}");

            if (File.Exists(resimYolu))
            {
                try
                {
                    // Resmi yükle
                    using (var tempImage = System.Drawing.Image.FromFile(resimYolu))
                    {
                        pictureBoxBitki.Image = new Bitmap(tempImage); // PictureBox'a resmi yükle
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Resim yüklenemedi. Hata: {ex.Message}", "Resim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Resim bulunamadı! Beklenen Yol:\n{resimYolu}", "Resim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            listViewBilgiler.Items.Clear(); // Açılınca temizle

            // Eğer sütunlar otomatik olarak eklenmiyorsa:
            listViewBilgiler.Columns.Clear();
            listViewBilgiler.View = View.Details; // Detay görünümü olsun
            listViewBilgiler.Columns.Add("Özellik", 250);
            listViewBilgiler.Columns.Add("Değer", 300);           
        }

        private void btnPdfOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                string pdfYolu = Path.Combine(Application.StartupPath, "BitkiBilgileri.pdf");

                Document doc = new Document();
                PdfWriter.GetInstance(doc, new FileStream(pdfYolu, FileMode.Create));
                doc.Open();

                // 📌 iTextSharp'ın Font sınıfını açıkça belirtiyoruz
                string arialTtf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont arialBaseFont = BaseFont.CreateFont(arialTtf, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font turkceFont = new iTextSharp.text.Font(arialBaseFont, 12, iTextSharp.text.Font.NORMAL);

                doc.Add(new Paragraph("Bitki Bilgileri\n", turkceFont));
                doc.Add(new Paragraph("----------------------------------------\n", turkceFont));

                foreach (ListViewItem item in listViewBilgiler.Items)
                {
                    string satir = $"{item.SubItems[0].Text}: {item.SubItems[1].Text}";
                    doc.Add(new Paragraph(satir, turkceFont));
                }

                doc.Close();
                MessageBox.Show("PDF başarıyla oluşturuldu!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDF oluşturulurken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPdfGoster_Click(object sender, EventArgs e)
        {
            string pdfYolu = Path.Combine(Application.StartupPath, "BitkiBilgileri.pdf");

            if (File.Exists(pdfYolu))
            {
                System.Diagnostics.Process.Start(pdfYolu);
            }
            else
            {
                MessageBox.Show("Önce PDF oluşturmalısınız!", "Dosya Bulunamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
