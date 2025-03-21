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

            // Seçilen bilgileri Form2'ye aktarıyoruz
            form2.SecilenBitkiAdi = TürAdı.Text;
            form2.SecilenYuzey = comboBoxYuzey.Text;
            form2.SecilenDallanma = comboBoxDallanma.Text;
            form2.SecilenCap = comboBoxcap.Text;
            form2.SecilenNodyum = comboBoxNodyum.Text;

            // Form2'yi aç ama sadece Show() kullan, ShowDialog() değil
            form2.Show();
        }

        private void TürAra_Click(object sender, EventArgs e)
        {
            // Kullanıcı tarafından seçilen combobox değerlerini güvenli şekilde alıyoruz
            string cap = comboBoxcap.SelectedItem?.ToString() ?? "";
            string TuyDurumu = comboBoxTuyDurumu.SelectedItem?.ToString() ?? "";
            string Yuzey = comboBoxYuzey.SelectedItem?.ToString() ?? "";
            string Dallanma = comboBoxDallanma.SelectedItem?.ToString() ?? "";
            string Nodyum = comboBoxNodyum.SelectedItem?.ToString() ?? "";

            // Eğer "Yüzey" combobox'ı "Tüysüz" veya "Seyrek Tüylü" seçilmişse direkt sonucu verelim
            if (Yuzey == "Tüylü" || Yuzey == "Seyrek Tüylü")
            {
                TürAdı.Text = "Bulunan Bitki: Ankyropetalum arsusianum";
                return;
            }

            // Eğer "Tabanda Sık" Dallanma veya "1-3 mm" Cap ve "İnternodlar Kısa" Nodyum seçilirse, "Ankyropetalum reuteri" bulunmalı
            if (Dallanma == "Tabanda Sık" && cap == "1-3 mm" && Nodyum == "İnternodlar Kısa")
            {
                TürAdı.Text = "Bulunan Bitki: Ankyropetalum reuteri";
                return;
            }

            // Eğer "Tabanda Birkaç" Dallanma veya "2,5-5 mm" Cap ve "İnternodlar Belirgin" Nodyum seçilirse, "Ankyropetalum gypsophiloides" bulunmalı
            if (Dallanma == "Tabanda Birkaç" && cap == "2,5-5 mm" && Nodyum == "İnternodlar Belirgin")
            {
                TürAdı.Text = "Bulunan Bitki: Ankyropetalum gypsophiloides";
                return;
            }

            // Eğer seçim yapılmadan "Boş" gibi özel seçenekler seçildiyse, bunları SQL'de yokmuş gibi ele alalım
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Esnek bir SQL sorgusu oluşturuyoruz, boş bırakılan combobox'ları dikkate almıyoruz
                    string query = "SELECT * FROM Bitkiler WHERE 1=1";

                    if (!string.IsNullOrWhiteSpace(cap)) query += " AND Cap = @cap";
                    if (!string.IsNullOrWhiteSpace(TuyDurumu)) query += " AND TuyDurumu = @TuyDurumu";
                    if (!string.IsNullOrWhiteSpace(Yuzey)) query += " AND Yuzey = @Yuzey";
                    if (!string.IsNullOrWhiteSpace(Dallanma)) query += " AND Dallanma = @Dallanma";
                    if (!string.IsNullOrWhiteSpace(Nodyum)) query += " AND Nodyum = @Nodyum";

                    SQLiteCommand command = new SQLiteCommand(query, connection);

                    if (!string.IsNullOrWhiteSpace(cap)) command.Parameters.AddWithValue("@cap", cap);
                    if (!string.IsNullOrWhiteSpace(TuyDurumu)) command.Parameters.AddWithValue("@TuyDurumu", TuyDurumu);
                    if (!string.IsNullOrWhiteSpace(Yuzey)) command.Parameters.AddWithValue("@Yuzey", Yuzey);
                    if (!string.IsNullOrWhiteSpace(Dallanma)) command.Parameters.AddWithValue("@Dallanma", Dallanma);
                    if (!string.IsNullOrWhiteSpace(Nodyum)) command.Parameters.AddWithValue("@Nodyum", Nodyum);

                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        string bitkiAdi = reader["BitkiAdi"].ToString();
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
