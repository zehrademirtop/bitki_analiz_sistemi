using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace bitki_analiz_sistemi
{
    public partial class Form2 : Form
    {
        private string bitkiAdi; // 🔹 Bitki adını saklamak için değişken
        private SQLiteConnection sqliteConn;
        private string pdfPath;
        private Dictionary<string, string> users = new Dictionary<string, string>()

        {
            { "admin", "1234" },
            { "zehra", "5678" },
            { "kullanici", "password" }
        };
        public Form2(string bitkiAdi)
        {
            InitializeComponent();
            this.bitkiAdi = bitkiAdi;
        }
        private void ConnectToDatabase()
        {
            sqliteConn = new SQLiteConnection("Data Source=bitkiler.db;Version=3;");
            sqliteConn.Open();
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            txtSifre.UseSystemPasswordChar = true;

        }

        private void btngiris_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text.Trim();

            if (kullaniciAdi == "admin" && sifre == "1234") // Sabit kullanıcı adı ve şifre
            {
                OpenPdf();
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre!", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            string pdfPath = @"C:\Users\demir\Desktop\pdfler\bitki_raporu.pdf";
            if (File.Exists(pdfPath))
            {
                System.Diagnostics.Process.Start(pdfPath);
            }
            else
            {
                MessageBox.Show("PDF dosyası bulunamadı! Lütfen dosyanın doğru konumda olduğundan emin olun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void OpenPdf()
        {
            string pdfPath = $@"C:\Users\demir\Desktop\{bitkiAdi}.pdf"; // Seçilen bitkinin PDF dosyasını belirler

            if (File.Exists(pdfPath))
            {
                System.Diagnostics.Process.Start(pdfPath);
            }
            else
            {
                MessageBox.Show($"{bitkiAdi} için PDF bulunamadı! Lütfen dosyanın doğru konumda olduğunu kontrol edin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void KullanıcıAdı_Click(object sender, EventArgs e)
        {

        }
    }
}
