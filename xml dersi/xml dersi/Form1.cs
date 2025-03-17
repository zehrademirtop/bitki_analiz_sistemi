using System.Xml.Linq;
namespace xml_dersi

{
    public partial class Form1 : Form
    {
        private string xmlFilePath = "kitaplar.xml";

        public Form1()
        {
            InitializeComponent();
            InitializeListView();
            LoadBooksFromXml();
        }
        private void InitializeListView()
        {
            listViewKitaplar.View = View.Details; // Detay görünümü
            listViewKitaplar.Columns.Add("Baþlýk", 150);
            listViewKitaplar.Columns.Add("Yazar", 150);
            listViewKitaplar.FullRowSelect = true; // Tüm satýrý seçme
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeListView();
            LoadBooksFromXml();
        }

        private void btnekle_Click(object sender, EventArgs e)
        {
            string baslik = txtBaslik.Text.Trim();
            string yazar = txtYazar.Text.Trim();

            if (string.IsNullOrWhiteSpace(baslik) || string.IsNullOrWhiteSpace(yazar))
            {
                MessageBox.Show("Lütfen tüm alanlarý doldurun!", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddBookToXml(baslik, yazar); // XML'e ekle

            // ListView'e ekle
            var listViewItem = new ListViewItem(new[] { baslik, yazar });
            listViewKitaplar.Items.Add(listViewItem);

            MessageBox.Show("Kitap baþarýyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtBaslik.Clear();
            txtYazar.Clear();
        }
        private void LoadBooksFromXml()
        {
            if (!System.IO.File.Exists(xmlFilePath)) return; // XML dosyasý yoksa yükleme yapma

            XDocument xmlDoc = XDocument.Load(xmlFilePath); // XML dosyasýný yükle

            foreach (XElement kitap in xmlDoc.Descendants("kitap"))
            {
                string baslik = kitap.Element("baslik")?.Value; // <baslik> elemanýnýn deðeri
                string yazar = kitap.Element("yazar")?.Value;   // <yazar> elemanýnýn deðeri

                // ListView'e ekle
                var listViewItem = new ListViewItem(new[] { baslik, yazar });
                listViewKitaplar.Items.Add(listViewItem);
            }
        }
        private void AddBookToXml(string baslik, string yazar)
        {
            XDocument xmlDoc;
            if (System.IO.File.Exists(xmlFilePath))
            {
                xmlDoc = XDocument.Load(xmlFilePath); // Mevcut XML dosyasýný yükle
            }
            else
            {
                xmlDoc = new XDocument(new XElement("kitaplar")); // Yeni bir XML oluþtur
            }

            // Yeni kitap elemaný oluþtur ve ekle
            XElement yeniKitap = new XElement("kitap",
                new XElement("baslik", baslik),
                new XElement("yazar", yazar)
            );

            xmlDoc.Root.Add(yeniKitap); // Kök elemanýna ekle
            xmlDoc.Save(xmlFilePath);  // XML dosyasýný kaydet
        }
        private void UpdateBookInXml(string eskiBaslik, string eskiYazar, string yeniBaslik, string yeniYazar)
        {
            if (!System.IO.File.Exists(xmlFilePath)) return; // Dosya yoksa iþlem yapma

            XDocument xmlDoc = XDocument.Load(xmlFilePath);
            XElement kitapToUpdate = xmlDoc.Descendants("kitap")
                .FirstOrDefault(k => k.Element("baslik")?.Value == eskiBaslik && k.Element("yazar")?.Value == eskiYazar);

            if (kitapToUpdate != null)
            {
                kitapToUpdate.Element("baslik").Value = yeniBaslik; // Yeni baþlýk
                kitapToUpdate.Element("yazar").Value = yeniYazar;   // Yeni yazar
                xmlDoc.Save(xmlFilePath); // Güncellenmiþ XML dosyasýný kaydet
            }
        }
        private void DeleteBookFromXml(string baslik, string yazar)
        {
            if (!System.IO.File.Exists(xmlFilePath)) return; // Dosya yoksa iþlem yapma

            XDocument xmlDoc = XDocument.Load(xmlFilePath);
            XElement kitapToDelete = xmlDoc.Descendants("kitap")
                .FirstOrDefault(k => k.Element("baslik")?.Value == baslik && k.Element("yazar")?.Value == yazar);

            if (kitapToDelete != null)
            {
                kitapToDelete.Remove(); // XML'den elemaný kaldýr
                xmlDoc.Save(xmlFilePath); // Güncellenmiþ XML dosyasýný kaydet
            }
        }
        private void btnsil_Click(object sender, EventArgs e)
        {
            if (listViewKitaplar.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen silmek istediðiniz kitabý seçin!", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = listViewKitaplar.SelectedItems[0];
            string baslik = selectedItem.SubItems[0].Text;
            string yazar = selectedItem.SubItems[1].Text;

            DeleteBookFromXml(baslik, yazar); // XML'den sil

            // ListView'den sil
            listViewKitaplar.Items.Remove(selectedItem);

            MessageBox.Show("Kitap baþarýyla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btndüzenle_Click(object sender, EventArgs e)
        {
            if (listViewKitaplar.SelectedItems.Count == 0)
            {
                MessageBox.Show("Lütfen düzenlemek istediðiniz kitabý seçin!", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = listViewKitaplar.SelectedItems[0];
            string eskiBaslik = selectedItem.SubItems[0].Text;
            string eskiYazar = selectedItem.SubItems[1].Text;

            string yeniBaslik = txtBaslik.Text.Trim();
            string yeniYazar = txtYazar.Text.Trim();

            if (string.IsNullOrWhiteSpace(yeniBaslik) || string.IsNullOrWhiteSpace(yeniYazar))
            {
                MessageBox.Show("Lütfen tüm alanlarý doldurun!", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateBookInXml(eskiBaslik, eskiYazar, yeniBaslik, yeniYazar); // XML'de güncelle

            // ListView'de güncelle
            selectedItem.SubItems[0].Text = yeniBaslik;
            selectedItem.SubItems[1].Text = yeniYazar;

            MessageBox.Show("Kitap baþarýyla güncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtBaslik.Clear();
            txtYazar.Clear();

        }
    }
    }

