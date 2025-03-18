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

namespace bitki_analiz_sistemi
{
    public partial class Form1 : Form
    {

        // SQLite bağlantı dizesi
        private string connectionString = @"Data Source=C:\Users\HP\Desktop\Bitkiler.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            comboBoxcap.Items.AddRange(new string[] { "boş", "1-3 mm", "2,5-5 mm" });
            comboBoxTuyDurumu.Items.AddRange(new string[] { "boş", "Tüylü", "Tüysüz" });
            comboBoxYuzey.Items.AddRange(new string[] { "boş", "Tüylü", "Seyrek Tüylü", "Salgı Tüylü" });
            comboBoxDallanma.Items.AddRange(new string[] { "boş", "Tabanda Sık", "Tabanda Birkaç" });
            comboBoxNodyum.Items.AddRange(new string[] { "boş", "İnternodlar Kısa", "İnternodlar Belirgin" });
            comboBoxUzunluk.Items.AddRange(new string[] { "boş" });
            comboBoxDurus.Items.AddRange(new string[] { "boş" });
            comboBoxRenk.Items.AddRange(new string[] { "boş" });
        }

        private void Bilgiver_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();  
        }

        private void TürAra_Click(object sender, EventArgs e)
        {
            // Kullanıcı tarafından seçilen combobox değerlerini alıyoruz
            string cap = comboBoxcap.SelectedItem.ToString();
            string TuyDurumu = comboBoxTuyDurumu.SelectedItem != null
    ? comboBoxTuyDurumu.SelectedItem.ToString()
    : "Seçilmedi"; // Varsayılan bir değer belirleyelim
            string Yuzey = comboBoxYuzey.SelectedItem.ToString();
            string Dallanma = comboBoxDallanma.SelectedItem.ToString();
            string Nodyum = comboBoxNodyum.SelectedItem.ToString();

            // SQLite bağlantısını kuruyoruz
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Veritabanı sorgusu, seçilen değerlere göre bitkileri arıyoruz
                    string query = "SELECT * FROM Bitkiler WHERE cap = @cap AND TuyDurumu = @TuyDurumu AND Yuzey = @Yuzey AND Dallanma = @Dallanma AND Nodyum = @Nodyum";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@cap", cap);
                    command.Parameters.AddWithValue("@TuyDurumu", TuyDurumu);
                    command.Parameters.AddWithValue("@Yuzey", Yuzey);
                    command.Parameters.AddWithValue("@Dallanma", Dallanma);
                    command.Parameters.AddWithValue("@Nodyum", Nodyum);

                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        // Eğer eşleşen sonuç varsa, veritabanından bitki adını alıyoruz
                        reader.Read(); // İlk sonucu alıyoruz
                        string bitkiAdi = reader["bitkiAdi"].ToString(); // bitkiAdi sütunundan adı alıyoruz

                        // Sonuçları label'a yazdırıyoruz
                        TürAdı.Text = "Bulunan Bitki: " + bitkiAdi;
                    }
                    else
                    {
                        // Eğer eşleşen sonuç yoksa, kullanıcıya bildirimde bulunuyoruz
                        TürAdı.Text = "Bitki bulunamadı.";
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}");
                }
            }
        }
    }
}
