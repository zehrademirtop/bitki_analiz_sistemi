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
using Microsoft.Data.Sqlite;
namespace ferhaturizm1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            SQLitePCL.Batteries.Init();
        }
        private void kaydet_Click(object sender, EventArgs e)
        {

            string connectionString = @"Data Source=C:\Users\demir\Desktop\ferhatturizm.db";

            using (SqliteConnection connection = new SqliteConnection(connectionString))

            {
                try
                {
                    // Veritabanına bağlan
                    connection.Open();


                    // Insert komutu
                    string kayit = "INSERT INTO ferhatturizm(OtobusId, Marka, Model, ModelYili, Kapasite) VALUES(@OtobusId, @Marka, @Model, @ModelYili, @Kapasite)";
                    SqliteCommand komut = new SqliteCommand(kayit, connection);

                    // Parametreleri ekle
                    komut.Parameters.AddWithValue("@OtobusId", textBox1.Text);
                    komut.Parameters.AddWithValue("@Marka", textBox2.Text);
                    komut.Parameters.AddWithValue("@Model", textBox3.Text);
                    komut.Parameters.AddWithValue("@ModelYili", textBox4.Text);
                    komut.Parameters.AddWithValue("@Kapasite", textBox5.Text);

                    // Veriyi ekle
                    int rowsAffected = komut.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kayıt başarıyla eklendi");
                    }
                    else
                    {
                        MessageBox.Show("Kayıt eklenemedi");
                    }
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata Meydana Geldi: " + hata.Message);
                }
            }
        }

        private void listele_Click(object sender, EventArgs e)
        {

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }
        private void VerileriListele()
        {
            if (listView1.Columns.Count == 0)
            {
                listView1.Columns.Add("OtobusId", 100);
                listView1.Columns.Add("Marka", 100);
                listView1.Columns.Add("Model", 100);
                listView1.Columns.Add("ModelYili", 100);
                listView1.Columns.Add("Kapasite", 100);

                listView1.View = View.Details;
            }

            string connectionString = @"Data Source=C:\Users\demir\Desktop\ferhatturizm.db";


            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    // Veritabanından verileri çek
                    string sorgu = "SELECT OtobusId, Marka, Model, ModelYili, Kapasite FROM ferhatturizm";
                    SqliteCommand komut = new SqliteCommand(sorgu, connection);
                    SqliteDataReader reader = komut.ExecuteReader();

                    // ListView'i temizle
                    listView1.Items.Clear();

                    // Verileri ListView'e ekle
                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["OtobusId"].ToString());
                        item.SubItems.Add(reader["Marka"].ToString());
                        item.SubItems.Add(reader["Model"].ToString());
                        item.SubItems.Add(reader["ModelYili"].ToString());
                        item.SubItems.Add(reader["Kapasite"].ToString());

                        listView1.Items.Add(item);
                    }

                    reader.Close();
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("Veri Çekerken Hata: " + hata.Message);
            }
        }
        private void getir_Click(object sender, EventArgs e)
        {
            VerileriListele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

    }
}



