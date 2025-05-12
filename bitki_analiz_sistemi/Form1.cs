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
using System.Net.Http;
using Newtonsoft.Json;
namespace bitki_analiz_sistemi
{
    public partial class Form1 : Form
    {
        private string apiKey = "fafc3e9f50a6e2dfd4b249e972a4fe8a"; // Weather API Key
        private string cityName = "Iğdır";  // Hava durumu bilgisini almak istediğimiz şehir
        private readonly HttpClient _httpClient = new HttpClient();
        private const string menseiApiUrl = "https://localhost:7219/api/mensei"; // API portu doğru
        private string connectionString = @"Data Source=C:\Users\HP\Desktop\Bitkiler.db;Version=3;";
        public string SecilenUzunluk { get; private set; }
        public string Uzunluk { get; private set; }
        public string Durus { get; private set; }
        public string Renk { get; private set; }
        public Form1()
        {
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            await GetWeatherData(cityName);
            comboBoxcap.Items.AddRange(new string[] { "boş", "1-3 mm", "2,5-5 mm", "2 mm" });
            comboBoxTuyDurumu.Items.AddRange(new string[] { "boş", "Tüylü", "Tüysüz" });
            comboBoxYuzey.Items.AddRange(new string[] { "boş", "Tüylü", "Salgı Tüylü" });
            comboBoxDallanma.Items.AddRange(new string[] { "boş", "Tabanda Sık", "Tabanda Birkaç" });
            comboBoxNodyum.Items.AddRange(new string[] { "boş", "İnternodlar Kısa", "İnternodlar Belirgin" });
            comboBoxUzunluk.Items.AddRange(new string[] { "boş", "50–70 cm", "80 cm", "50–80 cm" });
            comboBoxDurus.Items.AddRange(new string[] { "boş", "Dik" });
            comboBoxRenk.Items.AddRange(new string[] { "boş", "Açık Vişne", "Solgun Yeşil" });
            // ComboBox'ların SelectedIndexChanged olaylarını bağla
            comboBoxcap.SelectedIndexChanged += ComboBox_Changed;
            comboBoxTuyDurumu.SelectedIndexChanged += ComboBox_Changed;
            comboBoxYuzey.SelectedIndexChanged += ComboBox_Changed;
            comboBoxDallanma.SelectedIndexChanged += ComboBox_Changed;
            comboBoxNodyum.SelectedIndexChanged += ComboBox_Changed;
            comboBoxUzunluk.SelectedIndexChanged += ComboBox_Changed;
            comboBoxDurus.SelectedIndexChanged += ComboBox_Changed;
            comboBoxRenk.SelectedIndexChanged += ComboBox_Changed;
        }
        private void ComboBox_Changed(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                string selectedItem = comboBox.SelectedItem?.ToString();
                // Add logic to handle the selected item  
               
            }
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
       private async Task<string> GetMenseiFromApi(string bitkiAdi)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{menseiApiUrl}?bitki={Uri.EscapeDataString(bitkiAdi)}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var menseiData = JsonConvert.DeserializeObject<BitkiMensei>(content);
                return menseiData.Mensei;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Menşei alınırken hata: {ex.Message}\nBitki: {bitkiAdi}", "API Hatası");
                return "Bilinmiyor";
            }
        }
        public class BitkiMensei
        {
            public string Bitki { get; set; }
            public string Mensei { get; set; }
        }
        private async void TürAra_Click(object sender, EventArgs e)
        {
            GuncelleTurAdi();
        }
        private void GuncelleTurAdi()
        {
            // ComboBox'lardan seçilen değerleri al
            string cap = comboBoxcap.SelectedItem?.ToString() ?? "";
            string tuyDurumu = comboBoxTuyDurumu.SelectedItem?.ToString() ?? "";
            string yuzey = comboBoxYuzey.SelectedItem?.ToString() ?? "";
            string dallanma = comboBoxDallanma.SelectedItem?.ToString() ?? "";
            string nodyum = comboBoxNodyum.SelectedItem?.ToString() ?? "";
            string uzunluk = comboBoxUzunluk.SelectedItem?.ToString() ?? "";
            string durus = comboBoxDurus.SelectedItem?.ToString() ?? "";
            string renk = comboBoxRenk.SelectedItem?.ToString() ?? "";
            // Eşleşen bitkileri topla
            List<string> bitkiAdlari = new List<string>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Esnek SQL sorgusu: En az bir özellik eşleşirse bitkiyi getir
                    string query = "SELECT DISTINCT BitkiAdi FROM Bitkiler WHERE 1=0";
                    List<string> parameters = new List<string>();
                    if (!string.IsNullOrWhiteSpace(cap) && cap != "boş")
                    {
                        query += " OR Cap = @cap";
                        parameters.Add("@cap");
                    }
                    if (!string.IsNullOrWhiteSpace(tuyDurumu) && tuyDurumu != "boş")
                    {
                        query += " OR TuyDurumu = @tuyDurumu";
                        parameters.Add("@tuyDurumu");
                    }
                    if (!string.IsNullOrWhiteSpace(yuzey) && yuzey != "boş")
                    {
                        query += " OR Yuzey = @yuzey OR Yuzey LIKE @yuzey || ',%' OR Yuzey LIKE '%,' || @yuzey || ',%' OR Yuzey LIKE '%,' || @yuzey";
                        parameters.Add("@yuzey");
                    }
                    if (!string.IsNullOrWhiteSpace(dallanma) && dallanma != "boş")
                    {
                        query += " OR Dallanma = @dallanma";
                        parameters.Add("@dallanma");
                    }
                    if (!string.IsNullOrWhiteSpace(nodyum) && nodyum != "boş")
                    {
                        query += " OR Nodyum = @nodyum";
                        parameters.Add("@nodyum");
                    }
                    if (!string.IsNullOrWhiteSpace(uzunluk) && uzunluk != "boş")
                    {
                        query += " OR Uzunluk = @uzunluk";
                        parameters.Add("@uzunluk");
                    }
                    if (!string.IsNullOrWhiteSpace(durus) && durus != "boş")
                    {
                        query += " OR Durus = @durus";
                        parameters.Add("@durus");
                    }
                    if (!string.IsNullOrWhiteSpace(renk) && renk != "boş")
                    {
                        query += " OR Renk = @renk";
                        parameters.Add("@renk");
                    }
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    // Parametreleri ekle
                    if (parameters.Contains("@cap")) command.Parameters.AddWithValue("@cap", cap);
                    if (parameters.Contains("@tuyDurumu")) command.Parameters.AddWithValue("@tuyDurumu", tuyDurumu);
                    if (parameters.Contains("@yuzey")) command.Parameters.AddWithValue("@yuzey", yuzey);
                    if (parameters.Contains("@dallanma")) command.Parameters.AddWithValue("@dallanma", dallanma);
                    if (parameters.Contains("@nodyum")) command.Parameters.AddWithValue("@nodyum", nodyum);
                    if (parameters.Contains("@uzunluk")) command.Parameters.AddWithValue("@uzunluk", uzunluk);
                    if (parameters.Contains("@durus")) command.Parameters.AddWithValue("@durus", durus);
                    if (parameters.Contains("@ren" +
                        "k")) command.Parameters.AddWithValue("@renk", renk);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string bitkiAdi = reader["BitkiAdi"].ToString();
                        if (!bitkiAdlari.Contains(bitkiAdi))
                        {
                            bitkiAdlari.Add(bitkiAdi);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Veritabanı hatası: {ex.Message}", "Hata");
                    bitkiAdlari.Add("Hata oluştu.");
                }
            }
            // comboBoxTurAdi’yı güncelle
            comboBoxTurAdi.Items.Clear();
            if (bitkiAdlari.Count > 0)
            {
                comboBoxTurAdi.Items.AddRange(bitkiAdlari.ToArray());
                comboBoxTurAdi.SelectedIndex = 0; // İlkini seç
            }
            else
            {
                comboBoxTurAdi.Items.Add("Bitki bulunamadı.");
                comboBoxTurAdi.SelectedIndex = 0;
                labelMensei.Text = "Menşei: Bilinmiyor";
            }
        }
        private void Bilgiver_Click(object sender, EventArgs e)
        {

            // Bitki adını temizle, boşlukları ve fazladan karakterleri kaldır
            string bitkiAdi = comboBoxTurAdi.Text.Replace("Bulunan Bitki: ", "").Trim();
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
        private async void comboBoxTurAdi_SelectedIndexChanged(object sender, EventArgs e)
        {
            string secilenBitki = comboBoxTurAdi.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(secilenBitki) && secilenBitki != "Bitki bulunamadı." && secilenBitki != "Hata oluştu.")
            {
                labelMensei.Text = "Menşei: " + await GetMenseiFromApi(secilenBitki);
            }
            else
            {
                labelMensei.Text = "Menşei: Bilinmiyor";
            }
        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
//if (Yuzey == "Tüylü")
//{ss
//    if (cap == "2 mm" && Renk == "Açık Vişne" && TuyDurumu == "Tüysüz" && Uzunluk == "50–70 cm" && Durus == "Dik")
//    {
//        bitkiAdlari.Add("Ankyropetalum arsusianum");
//    }
//}
//if (Dallanma == "Tabanda Sık" && cap == "1-3 mm" && Nodyum == "İnternodlar Kısa")
//{
//    if (Renk == "Açık Vişne" && TuyDurumu == "Tüylü" && Uzunluk == "80 cm" && Durus == "Dik")
//    {
//        bitkiAdlari.Add("Ankyropetalum reuteri");
//    }
//}
//if (Dallanma == "Tabanda Birkaç" && cap == "2,5-5 mm" && Nodyum == "İnternodlar Belirgin")
//{
//    if (Renk == "Solgun Yeşil" && Yuzey == "Tüylü" && Uzunluk == "50–80 cm")
//    {
//        bitkiAdlari.Add("Ankyropetalum gypsophiloides");
//    }