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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Http;
using Newtonsoft.Json;
namespace bitki_analiz_sistemi
{
    public partial class Form1 : Form
    {
        private string apiKey = "b205766cf4a248458c2210504252403"; // WeatherAPI API Key
        private string cityName = "Iğdır";  // Hava durumu bilgisini almak istediğiniz şehir

        // SQLite bağlantı dizesi
        private string connectionString = @"Data Source=C:\Users\demir\Desktop\bitki_analiz_sistemi\Bitkiler.db;Version=3;";




        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await GetWeatherData(cityName);
            comboBoxcap.Items.AddRange(new string[] { "boş", "1-3 mm", "2,5-5 mm" });
            comboBoxTuyDurumu.Items.AddRange(new string[] { "boş", "Tüylü", "Tüysüz" });
            comboBoxYuzey.Items.AddRange(new string[] { "boş", "Tüylü", "Seyrek Tüylü", "Salgı Tüylü" });
            comboBoxDallanma.Items.AddRange(new string[] { "boş", "Tabanda Sık", "Tabanda Birkaç" });
            comboBoxNodyum.Items.AddRange(new string[] { "boş", "İnternodlar Kısa", "İnternodlar Belirgin" });
            comboBoxUzunluk.Items.AddRange(new string[] { "boş" });
            comboBoxDurus.Items.AddRange(new string[] { "boş" });
            comboBoxRenk.Items.AddRange(new string[] { "boş" });
        }
        // Hava durumu verisini almak için API'yi çağırıyoruz
        private async Task GetWeatherData(string city)
        {
            string url = $"https://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}&lang=tr"; // WeatherAPI url

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // API'den veri alıyoruz
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();  // Başarılı bir yanıt almazsak hata verecek
                    string responseData = await response.Content.ReadAsStringAsync();

                    // JSON formatında gelen veriyi parse ediyoruz
                    var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(responseData);

                    // Hava durumu bilgilerini formda gösteriyoruz
                    labelHavaDurumu.Text = $"Şehir: {weatherData.Location.Name}\n" +
                                           $"Sıcaklık: {weatherData.Current.TempC}°C\n" +
                                           $"Durum: {weatherData.Current.Condition.Text}\n" +
                                           $"Rüzgar: {weatherData.Current.WindKph} km/h";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hava durumu verisi alınırken bir hata oluştu: {ex.Message}");
                }
            }
        }
        // API'den gelen veriyi deserialize edebilmek için sınıf oluşturuyoruz
        public class WeatherResponse
        {
            public Location Location { get; set; }
            public CurrentWeather Current { get; set; }
        }
        public class Location
        {
            public string Name { get; set; }
        }

        public class CurrentWeather
        {
            public double TempC { get; set; }
            public Condition Condition { get; set; }
            public double WindKph { get; set; }
        }
        public class Condition
        {
            public string Text { get; set; }
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

            // Form2'yi açıyoruz
            form2.Show();
        }

        private void TürAra_Click(object sender, EventArgs e)
        {
            string cap = comboBoxcap.SelectedItem?.ToString() ?? "";
            string TuyDurumu = comboBoxTuyDurumu.SelectedItem?.ToString() ?? "";
            string Yuzey = comboBoxYuzey.SelectedItem?.ToString() ?? "";
            string Dallanma = comboBoxDallanma.SelectedItem?.ToString() ?? "";
            string Nodyum = comboBoxNodyum.SelectedItem?.ToString() ?? "";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Esnek SQL sorgusu oluşturuyoruz
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
                        // Verileri Form2'ye aktarıyoruz
                        Form2 form2 = new Form2();
                        form2.SecilenBitkiAdi = bitkiAdi;
                        form2.Show();
                    }
                    else
                    {
                        TürAdı.Text = "Bitki bulunamadı.";
                    }

                    reader.Close();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"SQLite Hatası: {ex.Message}", "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Genel Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
