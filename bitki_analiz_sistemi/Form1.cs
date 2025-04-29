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
        private string apiKey = "fafc3e9f50a6e2dfd4b249e972a4fe8a"; // Weather API Key
        private string cityName = "Iğdır";  // Hava durumu bilgisini almak istediğimiz şehir

        // SQLite bağlantı dizesi
        private string connectionString = @"Data Source=C:\Users\demir\Desktop\Bitkiler.db;Version=3;";

        public string SecilenUzunluk { get; private set; }

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

        private async Task GetWeatherData(string city)
        {
            string apiKey = "fafc3e9f50a6e2dfd4b249e972a4fe8a"; // OpenWeatherMap API Anahtarını buraya ekle
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=tr";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseData = await response.Content.ReadAsStringAsync();

                    var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(responseData);

                    labelHavaDurumu.Text = $"{weatherData.Name}: {weatherData.Main.Temp}°C\n" +
                                           $"Durum: {weatherData.Weather[0].Description}\n" +
                                           $"Rüzgar: {weatherData.Wind.Speed} m/s";
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
            public string Name { get; set; }
            public MainWeather Main { get; set; }
            public List<WeatherDescription> Weather { get; set; }
            public Wind Wind { get; set; }
        }

        public class MainWeather
        {
            public double Temp { get; set; }
        }

        public class WeatherDescription
        {
            public string Description { get; set; }
        }

        public class Wind
        {
            public double Speed { get; set; }
        }

        private string GetSecilenUzunluk()
        {
            return SecilenUzunluk;
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
                        TürAdı.Text = "" + bitkiAdi;
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

        private void Bilgiver_Click(object sender, EventArgs e)
        {

            // Bitki adını temizle, boşlukları ve fazladan karakterleri kaldır
            string bitkiAdi = TürAdı.Text.Replace("Bulunan Bitki: ", "").Trim();
            // Ekstra temizlik: birden fazla boşluğu tek boşluğa indir, alt tireyi boşlukla değiştir
            bitkiAdi = System.Text.RegularExpressions.Regex.Replace(bitkiAdi, @"\s+", " ").Replace("_", " ");
            // Debug: Gelen bitki adını göster
            Form2 form2 = new Form2
            {
                SecilenBitkiAdi = bitkiAdi,
                SecilenYuzey = comboBoxYuzey.Text,
                SecilenDallanma = comboBoxDallanma.Text,
                SecilenCap = comboBoxcap.Text,
                SecilenNodyum = comboBoxNodyum.Text,
                SecilenTuyDurumu = comboBoxTuyDurumu.Text,
                secilenUzunluk = comboBoxUzunluk.Text,
                SecilenDurus = comboBoxDurus.Text,
                SecilenRenk = comboBoxRenk.Text
            };
            form2.Show();
        }
    }
}