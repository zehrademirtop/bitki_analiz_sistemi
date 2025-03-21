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

                // 📌 Bitki adına göre resim göster
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

            // 📌 Bitki adına göre uygun resmi belirle
            switch (bitkiAdi)
            {
                case "Ankyropetalum arsusianum":
                    resimYolu = "C:\\Users\\HP\\Desktop\\Ankyropetalum arsusianum.png"; // Gerçek yolu gir
                    break;
                case "Ankyropetalum reuteri":
                    resimYolu = "C:\\Users\\HP\\Desktop\\Ankyropetalum reuteri.png";
                    break;
                case "Ankyropetalum gypsophiloides":
                    resimYolu = "C:\\Users\\HP\\Desktop\\Ankyropetalum gypsophiloides.png";
                    break;
                default:
                    resimYolu = "C:\\Resimler\\varsayilan.jpg"; // Bilinmeyen bitki için varsayılan resim
                    break;
            }

            // 📌 Resmi yükle
            if (System.IO.File.Exists(resimYolu)) // Dosya var mı kontrol et
            {
                pictureBoxBitki.Image = Image.FromFile(resimYolu);
            }
            else
            {
                MessageBox.Show("Resim bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

       
    }
}
