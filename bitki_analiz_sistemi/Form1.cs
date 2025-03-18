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
            // Kullanıcı tarafından seçilen combobox değerlerini güvenli şekilde alıyoruz
            string cap = comboBoxcap.SelectedItem?.ToString() ?? ""; // Eğer seçim yapılmadıysa boş string
            string TuyDurumu = comboBoxTuyDurumu.SelectedItem?.ToString() ?? "";
            string Yuzey = comboBoxYuzey.SelectedItem?.ToString() ?? "";
            string Dallanma = comboBoxDallanma.SelectedItem?.ToString() ?? "";
            string Nodyum = comboBoxNodyum.SelectedItem?.ToString() ?? "";

            // SQLite bağlantısını kuruyoruz
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Daha esnek bir SQL sorgusu oluşturuyoruz
                    string query = "SELECT * FROM Bitkiler WHERE " +
                                   "(Cap = @cap OR @cap = '') " +
                                   "AND (TuyDurumu = @TuyDurumu OR @TuyDurumu = '') " +
                                   "AND (Yuzey = @Yuzey OR @Yuzey = '') " +
                                   "AND (Dallanma = @Dallanma OR @Dallanma = '') " +
                                   "AND (Nodyum = @Nodyum OR @Nodyum = '')";

                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@cap", cap);
                    command.Parameters.AddWithValue("@TuyDurumu", TuyDurumu);
                    command.Parameters.AddWithValue("@Yuzey", Yuzey);
                    command.Parameters.AddWithValue("@Dallanma", Dallanma);
                    command.Parameters.AddWithValue("@Nodyum", Nodyum);

                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read(); // İlk sonucu alıyoruz
                        string bitkiAdi = reader["BitkiAdi"].ToString(); // Dikkat: Büyük/küçük harf duyarlılığı olabilir

                        // Sonuçları label'a yazdırıyoruz
                        TürAdı.Text = "Bulunan Bitki: " + bitkiAdi;
                    }
                    else
                    {
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
