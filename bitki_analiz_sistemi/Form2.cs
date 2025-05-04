using System;
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
using System.Net.Http;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data.SQLite;



namespace bitki_analiz_sistemi
{
    public partial class Form2 : Form
    {
        internal string secilenUzunluk;
        private string sonPdfYolu;

        public string SecilenBitkiAdi { get; set; }
        public string SecilenYuzey { get; set; }
        public string SecilenDallanma { get; set; }
        public string SecilenCap { get; set; }
        public string SecilenNodyum { get; set; }
        public string SecilenUzunluk { get; private set; }
        public string SecilenDuruş { get; private set; }
        public string SecilenTüyDurumu { get; private set; }
        public string SecilenTuyDurumu { get; internal set; }
        public string SecilenDurus { get; internal set; }
        public string SecilenRenk { get; internal set; }

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
            listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Renk", SecilenRenk }));
            listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Tüy Durumu", SecilenTuyDurumu }));
            listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Uzunluk", secilenUzunluk }));
            listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Duruş", SecilenDurus }));
        }

        // 📌 Seçilen bitkiye göre resmi gösteren fonksiyon
        private void ShowBitkiImage(string bitkiAdi)
        {
            if (string.IsNullOrEmpty(bitkiAdi))
            {
                pictureBoxBitki.Image = null;
                MessageBox.Show("Bitki adı boş!", "Debug");
                return;
            }

            // Resim dosya adlarını bitki adlarıyla eşleştir, alternatif adlar ekle
            var resimEslestirme = new Dictionary<string, string>
            {
                { "Ankyropetalum arsusianum", "Ankyropetalum_arsusianum.png" },
                { "Ankyropetalum_arsusianum", "Ankyropetalum_arsusianum.png" }, // Alternatif
                { "Ankyropetalum reuteri", "Ankyropetalum_reuteri.png" },
                { "Ankyropetalum gypsophiloides", "Ankyropetalum_gypsophiloides.png" }
            };

            string resimKlasoru = Path.Combine(Application.StartupPath, "resimler");
            string resimDosyaAdi = resimEslestirme.ContainsKey(bitkiAdi) ? resimEslestirme[bitkiAdi] : $"{bitkiAdi.Replace(" ", "_")}.png";
            string resimYolu = Path.Combine(resimKlasoru, resimDosyaAdi);

          

            try
            {
                if (File.Exists(resimYolu))
                {
                    pictureBoxBitki.Image = System.Drawing.Image.FromFile(resimYolu); // Belirsizlik çözüldü
                    pictureBoxBitki.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBoxBitki.Image = null;
                    MessageBox.Show($"Bitki resmi bulunamadı: {resimDosyaAdi}\nKlasör: {resimKlasoru}\nBitki Adı: {bitkiAdi}", "Resim Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                pictureBoxBitki.Image = null;
                MessageBox.Show($"Resim yüklenirken hata: {ex.Message}\nYol: {resimYolu}\nBitki Adı: {bitkiAdi}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            SaveFileDialog saveFile = new SaveFileDialog
            {
                Filter = "PDF Dosyaları|*.pdf",
                Title = "PDF Dosyası Kaydet",
                FileName = "BitkiBilgileri_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf" // Benzersiz dosya adı
            };

            string logoYolu = @"C:\Users\HP\Desktop\images1.png"; // Logo dosya yolu
            string resimKlasoru = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resimler"); // Mutlak yol
            if (string.IsNullOrEmpty(SecilenBitkiAdi))
            {
                MessageBox.Show("Seçilen bitki adı boş!", "Hata");
                return;
            }

            // SecilenBitkiAdi’yi temizle
            string temizBitkiAdi = SecilenBitkiAdi
                .Trim() // Baştaki/sondaki boşlukları sil
                .Replace(" ", "_") // Boşlukları _ ile değiştir
                .Replace("__", "_"); // Çift _’yi tek yap
            string bitkiResimYolu = Path.Combine(resimKlasoru, $"{temizBitkiAdi}.png");

            // Dosya var mı kontrol et
            if (!File.Exists(bitkiResimYolu))
            {
                // Büyük-küçük harf duyarlılığını atla
                string[] resimDosyalari = Directory.GetFiles(resimKlasoru, "*.png", SearchOption.TopDirectoryOnly);
                foreach (string dosya in resimDosyalari)
                {
                    string dosyaAdi = Path.GetFileNameWithoutExtension(dosya);
                    if (dosyaAdi.Equals(temizBitkiAdi, StringComparison.OrdinalIgnoreCase))
                    {
                        bitkiResimYolu = dosya;
                        break;
                    }
                }
            }

            // ListView verisini debug et
            if (listViewBilgiler.Items.Count == 0)
            {
                MessageBox.Show("ListView'de veri yok! Tablo boş.", "Debug");
                return;
            }
            else
            {
                string items = "ListView Verisi:\n";
                foreach (ListViewItem item in listViewBilgiler.Items)
                {
                    items += $"{item.SubItems[0].Text}: {item.SubItems[1].Text}\n";
                }
                MessageBox.Show(items, "Debug");
            }


            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        Document document = new Document(PageSize.A4, 50, 50, 100, 50); // Üst boşluk
                        PdfWriter writer = PdfWriter.GetInstance(document, stream);
                        document.Open();

                        // Türkçe Karakterler İçin Font Ayarı
                        BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font baslikFont = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD); // Başlık fontu
                        iTextSharp.text.Font kalinFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD); // Kalın font
                        iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL); // Normal font

                        // LOGO EKLEME (Sol Üst Köşe)
                        if (File.Exists(logoYolu))
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoYolu);
                            logo.ScaleAbsolute(70, 70); // Daha küçük logo
                            logo.SetAbsolutePosition(50, PageSize.A4.Height - 70); // Sol üst
                            document.Add(logo);
                        }
                        else
                        {
                            MessageBox.Show("Logo bulunamadı! Devam ediliyor...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        // "Iğdır Üniversitesi" YAZISI (Sağ Üst Köşe)
                        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase("IĞDIR ÜNİVERSİTESİ", normalFont), PageSize.A4.Width - 50, PageSize.A4.Height - 60, 0);

                        // BİTKİ RESMİ EKLEME (Tablonun üstünde, yatay ortalı, sabit)
                        if (File.Exists(bitkiResimYolu))
                        {
                            iTextSharp.text.Image bitkiResim = iTextSharp.text.Image.GetInstance(bitkiResimYolu);
                            bitkiResim.ScaleAbsolute(180, 180); // Resim boyutu
                            float x = (PageSize.A4.Width - bitkiResim.ScaledWidth) / 2; // Yatay ortalı
                            float y = PageSize.A4.Height - 250; // Sayfanın üstünden 250 birim aşağı
                            bitkiResim.SetAbsolutePosition(x, y);
                            document.Add(bitkiResim);
                        }
                        else
                        {
                            MessageBox.Show($"Bitki resmi bulunamadı: {bitkiResimYolu}! Devam ediliyor...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        // "Bitki Bilgileri" YAZISI (Tablonun sol üst köşesinde)
                        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Bitki Bilgileri", baslikFont), 50, 550, 0);

                        // TABLO OLUŞTURMA (Resmin altında, sayfanın alt kısmında, mutlak konum)
                        PdfPTable table = new PdfPTable(2) // 2 sütun: Özellik ve Değer
                        {
                            WidthPercentage = 100 // Tabloyu sayfaya yay
                        };
                        table.DefaultCell.Padding = 4;
                        table.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                        // Sütun Başlıkları (Kalın)
                        table.AddCell(new Phrase("Özellik", kalinFont));
                        table.AddCell(new Phrase("Değer", kalinFont));

                        // ListView'den Satırları Ekle
                        foreach (ListViewItem item in listViewBilgiler.Items)
                        {
                            table.AddCell(new Phrase(item.SubItems[0].Text, kalinFont)); // Özellik: Kalın
                            table.AddCell(new Phrase(item.SubItems[1].Text, normalFont)); // Değer: Normal
                        }

                        // Tabloyu mutlak konuma sabitle (resmin altına, sayfanın alt kısmı)
                        table.TotalWidth = PageSize.A4.Width - 100; // Kenar boşlukları için
                        table.WriteSelectedRows(0, -1, 50, 540, writer.DirectContent); // y=400 (~450-550 aralığı)

                        document.Close();
                    }

                    sonPdfYolu = saveFile.FileName; // PDF yolunu kaydet
                    MessageBox.Show("PDF başarıyla oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"PDF oluşturulurken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPdfGoster_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(sonPdfYolu) && File.Exists(sonPdfYolu))
            {
                try
                {
                    System.Diagnostics.Process.Start(sonPdfYolu); // PDF’yi varsayılan görüntüleyicide aç
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"PDF açılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("PDF dosyası bulunamadı! Lütfen önce PDF oluşturun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (listViewBilgiler.SelectedItems.Count > 0)
            {
                ListViewItem selected = listViewBilgiler.SelectedItems[0];

                try
                {
                    // En az 2 sütun olduğundan emin ol
                    if (listViewBilgiler.Columns.Count < 2)
                    {
                        MessageBox.Show("ListView'de yeterli sütun yok. En az 2 sütun olmalı.", "Hata");
                        return;
                    }

                    // SubItems[1] olduğundan emin ol
                    if (selected.SubItems.Count < 2)
                    {
                        MessageBox.Show("Seçili satırda değer sütunu eksik.", "Hata");
                        return;
                    }

                    // Sadece Değer sütununu delete et (boş yap)
                    selected.SubItems[1].Text = "";

                    // ListView'i yenile
                    listViewBilgiler.BeginUpdate();
                    listViewBilgiler.Refresh();
                    listViewBilgiler.Invalidate();
                    listViewBilgiler.Update();
                    listViewBilgiler.EndUpdate();

                    MessageBox.Show("Değer başarıyla delete edildi! 🎉", "Başarı");
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("ListView sütun sayısı yetersiz veya SubItems doğru tanımlanmamış.", "Hata");
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme sırasında hata oluştu: " + ex.Message, "Hata");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Lütfen delete etmek için bir kayıt seçin.", "Uyarı");
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (listViewBilgiler.SelectedItems.Count > 0)
            {
                ListViewItem selected = listViewBilgiler.SelectedItems[0];

                try
                {
                    // En az 2 sütun olduğundan emin ol
                    if (listViewBilgiler.Columns.Count < 2)
                    {
                        MessageBox.Show("ListView'de yeterli sütun yok. En az 2 sütun olmalı.", "Hata");
                        return;
                    }

                    // SubItems[1] olduğundan emin ol
                    if (selected.SubItems.Count < 2)
                    {
                        MessageBox.Show("Seçili satırda değer sütunu eksik.", "Hata");
                        return;
                    }

                    // Seçili satırın özelliğini al (Özellik sütunu)
                    string ozellik = selected.SubItems[0].Text;

                    // Özelliğe göre doğru TextBox'tan değeri al
                    string newValue = selected.SubItems[1].Text; // Varsayılan olarak mevcut değeri koru
                    switch (ozellik)
                    {
                        case "Bitki Adı":
                            newValue = txtBitkiAdi.Text;
                            break;
                        case "Yüzey":
                            newValue = txtYuzey.Text;
                            break;
                        case "Dallanma":
                            newValue = txtDallanma.Text;
                            break;
                        case "Çap":
                            newValue = txtCap.Text;
                            break;
                        case "Nodyum":
                            newValue = txtNodyum.Text;
                            break;
                        case "Tüy Durumu":
                            newValue = txtTuyDurumu.Text;
                            break;
                        case "Uzunluk":
                            newValue = txtUzunluk.Text;
                            break;
                        case "Renk":
                            newValue = txtRenk.Text;
                            break;
                        case "Duruş":
                            newValue = txtDurus.Text;
                            break;
                    }

                    // Yeni değeri update et
                    selected.SubItems[1].Text = string.IsNullOrEmpty(newValue) ? selected.SubItems[1].Text : newValue;
                    // ListView'i yenile
                    listViewBilgiler.BeginUpdate();
                    listViewBilgiler.Refresh();
                    listViewBilgiler.Invalidate();
                    listViewBilgiler.Update();
                    listViewBilgiler.EndUpdate();
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("ListView sütun sayısı yetersiz veya SubItems doğru tanımlanmamış.", "Hata");
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Güncelleme sırasında hata oluştu: " + ex.Message, "Hata");
                    return;
                }

                MessageBox.Show("Değer başarıyla update edildi! 🎉", "Başarı");
            }
            else
            {
                MessageBox.Show("Lütfen update etmek için bir kayıt seçin.", "Uyarı");
            }
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            try
            {
                // Dolu TextBox'ları kontrol et ve ekle
                if (!string.IsNullOrEmpty(txtBitkiAdi.Text))
                    listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Bitki Adı", txtBitkiAdi.Text }));
                if (!string.IsNullOrEmpty(txtYuzey.Text))
                    listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Yüzey", txtYuzey.Text }));
                if (!string.IsNullOrEmpty(txtDallanma.Text))
                    listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Dallanma", txtDallanma.Text }));
                if (!string.IsNullOrEmpty(txtCap.Text))
                    listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Çap", txtCap.Text }));
                if (!string.IsNullOrEmpty(txtNodyum.Text))
                    listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Nodyum", txtNodyum.Text }));
                if (!string.IsNullOrEmpty(txtTuyDurumu.Text))
                    listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Tüy Durumu", txtTuyDurumu.Text }));
                if (!string.IsNullOrEmpty(txtUzunluk.Text))
                    listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Uzunluk", txtUzunluk.Text }));
                if (!string.IsNullOrEmpty(txtRenk.Text))
                    listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Renk", txtRenk.Text }));
                if (!string.IsNullOrEmpty(txtDurus.Text))
                    listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Duruş", txtDurus.Text }));

                // ListView'i yenile
                listViewBilgiler.BeginUpdate();
                listViewBilgiler.Refresh();
                listViewBilgiler.Invalidate();
                listViewBilgiler.Update();
                listViewBilgiler.EndUpdate();

                // TextBox'ları temizle
                txtBitkiAdi.Clear();
                txtYuzey.Clear();
                txtDallanma.Clear();
                txtCap.Clear();
                txtNodyum.Clear();
                txtTuyDurumu.Clear();
                txtUzunluk.Clear();
                txtRenk.Clear();
                txtDurus.Clear();

                MessageBox.Show("Değerler başarıyla add edildi! 🌱", "Başarı");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme sırasında hata oluştu: " + ex.Message, "Hata");
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                // ListView'deki her satırı kontrol et
                foreach (ListViewItem item in listViewBilgiler.Items)
                {
                    // Sütun sayısını kontrol et
                    if (item.SubItems.Count < 2)
                    {
                        MessageBox.Show("Bir satırda değer sütunu eksik.", "Hata");
                        return;
                    }

                    // Özelliği al
                    string ozellik = item.SubItems[0].Text;

                    // Eğer Değer kısmı boşsa, ilgili TextBox'tan yeni değeri al
                    if (string.IsNullOrEmpty(item.SubItems[1].Text))
                    {
                        string newValue = "";
                        switch (ozellik)
                        {
                            case "Bitki Adı": newValue = txtBitkiAdi.Text; break;
                            case "Yüzey": newValue = txtYuzey.Text; break;
                            case "Dallanma": newValue = txtDallanma.Text; break;
                            case "Çap": newValue = txtCap.Text; break;
                            case "Nodyum": newValue = txtNodyum.Text; break;
                            case "Tüy Durumu": newValue = txtTuyDurumu.Text; break;
                            case "Uzunluk": newValue = txtUzunluk.Text; break;
                            case "Renk": newValue = txtRenk.Text; break;
                            case "Duruş": newValue = txtDurus.Text; break;
                        }
                        // Yeni değeri ListView'e yaz, TextBox boşsa mevcut durumu koru
                        item.SubItems[1].Text = string.IsNullOrEmpty(newValue) ? item.SubItems[1].Text : newValue;
                    }
                }

                // ListView'i yenile
                listViewBilgiler.BeginUpdate();
                listViewBilgiler.Refresh();
                listViewBilgiler.Invalidate();
                listViewBilgiler.Update();
                listViewBilgiler.EndUpdate();

                // Güncellenmiş verileri JSON'a kaydet
                var veriler = new List<KeyValuePair<string, string>>();
                foreach (ListViewItem item in listViewBilgiler.Items)
                {
                    veriler.Add(new KeyValuePair<string, string>(item.SubItems[0].Text, item.SubItems[1].Text));
                }
                string json = JsonConvert.SerializeObject(veriler, Formatting.Indented);
                string dosyaYolu = Path.Combine(Application.StartupPath, "bitki_verileri.json");
                File.WriteAllText(dosyaYolu, json);

                MessageBox.Show("Veriler başarıyla save edildi! 💾", "Başarı");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kaydetme sırasında hata oluştu: " + ex.Message, "Hata");
            }
        }

    }
}

