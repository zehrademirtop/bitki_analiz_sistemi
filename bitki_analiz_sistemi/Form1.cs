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
        private string connectionString = @"Data Source=C:\Users\demir\Desktop\bitkiler.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxcap.Items.AddRange(new string[] { "boş", "1-3 mm", "2,5-5 mm" });
            comboBoxTuyDurumu.Items.AddRange(new string[] { "boş", "seyrek Tüylü ","Tüylü", "Tüysüz" });
            comboBoxYuzey.Items.AddRange(new string[] { "boş", "Tüylü", "Seyrek Tüylü", "Salgı Tüylü" });
            comboBoxDallanma.Items.AddRange(new string[] { "boş", "Tabanda Sık", "Tabanda Birkaç", "0.0"});
            comboBoxNodyum.Items.AddRange(new string[] { "boş", "İnternodlar Kısa", "İnternodlar Belirgin" });
            comboBoxUzunluk.Items.AddRange(new string[] { "boş" });
            comboBoxDurus.Items.AddRange(new string[] { "boş" });
            comboBoxRenk.Items.AddRange(new string[] { "boş" });
        }

        private void Bilgiver_Click(object sender, EventArgs e)
        {
            string bitkiAdi = label1.Text.Trim();
            if (!string.IsNullOrEmpty(bitkiAdi))
            {
                Form2 bilgiFormu = new Form2(bitkiAdi);
                bilgiFormu.ShowDialog();
            }
            else
            {
                MessageBox.Show("Önce bir bitki türü seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TürAra_Click(object sender, EventArgs e)
        {
            // ComboBox'larda herhangi bir öğe seçilip seçilmediğini kontrol et
            if (comboBoxcap.SelectedItem == null || comboBoxTuyDurumu.SelectedItem == null || comboBoxYuzey.SelectedItem == null ||
                comboBoxDallanma.SelectedItem == null || comboBoxNodyum.SelectedItem == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            // Kullanıcı tarafından seçilen combobox değerlerini alıyoruz
            string cap = comboBoxcap.SelectedItem.ToString();
            string TuyDurumu = comboBoxTuyDurumu.SelectedItem.ToString();
            string Yuzey = comboBoxYuzey.SelectedItem.ToString();
            string Dallanma = comboBoxDallanma.SelectedItem.ToString();
            string Nodyum = comboBoxNodyum.SelectedItem.ToString();

            // SQLite bağlantısını kuruyoruz
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Veritabanı sorgusunu hazırlıyoruz (boş değerler için esneklik ekliyoruz)
                    string query = "SELECT * FROM Bitkiler WHERE 1=1";

                    // Seçilen değerlerin boş olup olmadığını kontrol ediyoruz
                    if (cap != "boş")
                        query += " AND cap = @cap";
                    if (TuyDurumu != "boş")
                        query += " AND TuyDurumu = @TuyDurumu";
                    if (Yuzey != "boş")
                        query += " AND Yuzey = @Yuzey";
                    if (Dallanma != "boş")
                        query += " AND Dallanma = @Dallanma";
                    if (Nodyum != "boş")
                        query += " AND Nodyum = @Nodyum";

                    // Veritabanı sorgusunu oluşturuyoruz
                    SQLiteCommand command = new SQLiteCommand(query, connection);

                    // Parametreleri ekliyoruz
                    if (cap != "boş") command.Parameters.AddWithValue("@cap", cap);
                    if (TuyDurumu != "boş") command.Parameters.AddWithValue("@TuyDurumu", TuyDurumu);
                    if (Yuzey != "boş") command.Parameters.AddWithValue("@Yuzey", Yuzey);
                    if (Dallanma != "boş") command.Parameters.AddWithValue("@Dallanma", Dallanma);
                    if (Nodyum != "boş") command.Parameters.AddWithValue("@Nodyum", Nodyum);

                    // Sorguyu çalıştırıyoruz
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

