using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;

namespace ÇağrıMerkeziSimülasyonu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> senaryolar; // Müşteri senaryoları listesi
        private Random rastgele;
        public MainWindow()
        {
            InitializeComponent();
            rastgele = new Random();
            SenaryolariYukle();
            RastgeleSenaryoGoster();
        }
        private void SenaryolariYukle()
        {
            senaryolar = new List<string>
            {
                "Ürünüm hala teslim edilmedi, ne yapmam gerekiyor?",
                "Faturamda yanlış bir ücretlendirme var.",
                "Hizmetinizden memnun değilim, iptal etmek istiyorum.",
                "Kargo adresim yanlış görünüyor, nasıl düzeltebilirim?"
            };
        }
        // Rastgele bir senaryoyu ekrana yazdırır
        private void RastgeleSenaryoGoster()
        {
            int index = rastgele.Next(senaryolar.Count);
            txtSenaryo.Text = senaryolar[index];
        }
        // Gönder butonuna tıklanınca yanıtı değerlendirir
        private void BtnGonder_Click(object sender, RoutedEventArgs e)
        {
           
            string yanit = txtYanıt.Text.Trim();
            if (string.IsNullOrEmpty(yanit))
            {
                lblSonuc.Text = "Lütfen bir yanıt girin.";
                return;

            }
           

            // Basit bir puanlama (örnek)
            int puan = yanit.Length > 20 ? 10 : 5;
            lblSonuc.Text = $"Yanıtınız değerlendirildi! Puan: {puan}";
            txtYanıt.Clear();

            RastgeleSenaryoGoster(); // Yeni bir senaryo göster
            VeritabaninaKaydet(txtSenaryo.Text, yanit, puan);
        }
        private void VeritabaninaKaydet(string senaryo, string yanit, int puan)
        {
            string connectionString = "Data Source=C:\\Veritabanlari\\Performans.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Performans (Tarih, Senaryo, Yanıt, Puan) VALUES (@Tarih, @Senaryo, @Yanıt, @Puan)";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@Tarih", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@Senaryo", senaryo);
                command.Parameters.AddWithValue("@Yanıt", yanit);
                command.Parameters.AddWithValue("@Puan", puan);
                command.ExecuteNonQuery();
            }
        }
    }
}
