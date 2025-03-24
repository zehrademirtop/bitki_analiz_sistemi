using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Data.SQLite;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec;


namespace bitki_analiz_sistemi
    
{
    public partial class Form2 : Form

    {
        public string SecilenBitkiAdi { get; set; }
        public string SecilenYuzey { get; set; }
        public string SecilenDallanma { get; set; }
        public string SecilenCap { get; set; }
        public string SecilenNodyum { get; set; }
        // ResimYolu özelliği kaldırıldı

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

            if (girilenKullaniciAdi.Equals(dogruKullaniciAdi, StringComparison.OrdinalIgnoreCase) &&
                girilenSifre == dogruSifre)
            {
                MessageBox.Show("Giriş başarılı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ListView'e verileri ekliyoruz
                listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Bitki Adı", SecilenBitkiAdi }));
                listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Yüzey", SecilenYuzey }));
                listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Dallanma", SecilenDallanma }));
                listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Çap", SecilenCap }));
                listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Nodyum", SecilenNodyum }));
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre!", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSifre.Clear();
                txtSifre.Focus();
            }
        }

         // Seçilen bitkiye göre resmi gösteren fonksiyon
        private void ShowBitkiImage(string bitkiAdi)
        {
            string resimYolu = "";

            switch (bitkiAdi.ToLower())
            {
                case "ankyropetalum arsusianum":
                    resimYolu = Path.Combine(Application.StartupPath, "resimler", "Ankyropetalum arsusianum.png");

                    break;
                case "ankyropetalum reuteri":
                    resimYolu = Path.Combine(Application.StartupPath, "resimler", "Ankyropetalum reuteri.png");
                    break;
                case "ankyropetalum gypsophiloides":
                    resimYolu = Path.Combine(Application.StartupPath, "resimler", "Ankyropetalum gypsophiloides.png");
                    break;

            }

            MessageBox.Show("Resim Yolu: " + resimYolu);  // Resim yolunu kontrol etmek için

            if (System.IO.File.Exists(resimYolu))
            {
                // Resmi yükleyip boyutlandırma
                System.Drawing.Image originalImage = System.Drawing.Image.FromFile(resimYolu);
                int maxWidth = 800;  // Maksimum genişlik
                int maxHeight = 600; // Maksimum yükseklik

                // Resmin boyutunu kontrol et
                if (originalImage.Width > maxWidth || originalImage.Height > maxHeight)
                {
                    // Oranları koruyarak yeniden boyutlandır
                    double ratio = Math.Min((double)maxWidth / originalImage.Width, (double)maxHeight / originalImage.Height);
                    int newWidth = (int)(originalImage.Width * ratio);
                    int newHeight = (int)(originalImage.Height * ratio);

                    // Yeni boyutta resmi oluştur
                    System.Drawing.Image resizedImage = new Bitmap(originalImage, newWidth, newHeight);
                    pictureBoxBitki.Image = resizedImage;
                }
                else
                {
                    pictureBoxBitki.Image = originalImage;
                }
            }
            else
            {
                MessageBox.Show("Resim bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {
           
            // Bitki adı formda gösterilsin

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            // Seçili öğeyi almak
            if (listViewBilgiler.SelectedItems.Count > 0)
            {
                // Seçili öğeyi alıyoruz
                string seciliBitki = listViewBilgiler.SelectedItems[0].Text;

                // Veritabanı veya koleksiyon üzerinden silme işlemi (örneğin koleksiyon üzerinden)
                List<string> bitkiler = new List<string>();

                // ListView'den öğeyi kaldırma
                listViewBilgiler.Items.Remove(listViewBilgiler.SelectedItems[0]);

                // ListView'ı güncelleme
                MessageBox.Show($"{seciliBitki} başarıyla silindi!", "Silme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir bitki seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        private void btnPdfgoster_Click(object sender, EventArgs e)
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






        // ListView'tan seçilen öğeyi almak ve TextBox'lara yerleştirmek


    };
       
      

     

     
    }




    

