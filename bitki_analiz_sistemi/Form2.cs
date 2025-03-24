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


namespace bitki_analiz_sistemi
{
    public partial class Form2 : Form
    {
        public string SecilenBitkiAdi { get; set; }
        public string SecilenYuzey { get; set; }
        public string SecilenDallanma { get; set; }
        public string SecilenCap { get; set; }
        public string SecilenNodyum { get; set; }
        public string ResimYolu { get; set; } // Burada ResimYolu özelliğini tanımlıyoruz

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

                // ListView'e verileri ekle
                listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Bitki Adı", SecilenBitkiAdi }));
                listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Yüzey", SecilenYuzey }));
                listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Dallanma", SecilenDallanma }));
                listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Çap", SecilenCap }));
                listViewBilgiler.Items.Add(new ListViewItem(new string[] { "Nodyum", SecilenNodyum }));

                // Resmi yükle
                ShowBitkiImage(SecilenBitkiAdi);
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre!", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSifre.Clear();
                txtSifre.Focus();
            }
        }

        // 📌 Seçilen bitkiye göre resmi gösteren fonksiyon
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
                default:
                    resimYolu = Path.Combine(Application.StartupPath, "resimler", "default.jpg");
                    break;
            }

            MessageBox.Show("Resim Yolu: " + resimYolu);  // Resim yolunu kontrol etmek için

            if (System.IO.File.Exists(resimYolu))
            {
                // Resmi yükleyip boyutlandırma
                Image originalImage = Image.FromFile(resimYolu);
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
                    Image resizedImage = new Bitmap(originalImage, newWidth, newHeight);
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
            labelBitkiAdi.Text = "Bitki Adı: " + SecilenBitkiAdi;

            // Başlangıçta resim yüklenmesin, sadece giriş butonuna basıldığında yüklensin
            pictureBoxBitki.Image = null;
        }
        private void ListeyiYenile()
        {
            listViewBilgiler.Items.Clear();  // ListView'ı temizleyin

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=bitkiler.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT * FROM Bitkiler";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["BitkiAdi"].ToString());
                        listViewBilgiler.Items.Add(item);
                    }
                }
            }
        }

       
        

        // ListView'tan seçilen öğeyi almak ve TextBox'lara yerleştirmek

        List<string> bitkiler = new List<string> {
            "Ankyropetalum arsusianum",
           "Ankyropetalum reuteri",
            "Ankyropetalum gypsophiloides"
              };
        private void btnSil_Click(object sender, EventArgs e)
        {
            // Seçili öğeyi almak
            if (listViewBilgiler.SelectedItems.Count > 0)
            {
                // Seçili öğeyi alıyoruz
                string seciliBitki = listViewBilgiler.SelectedItems[0].Text;

                // Veritabanı veya koleksiyon üzerinden silme işlemi (örneğin koleksiyon üzerinden)
                bitkiler.Remove(seciliBitki);

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
    }


    }

