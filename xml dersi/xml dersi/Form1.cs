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
            listViewKitaplar.View = View.Details; // Detay g�r�n�m�
            listViewKitaplar.Columns.Add("Ba�l�k", 150);
            listViewKitaplar.Columns.Add("Yazar", 150);
            listViewKitaplar.FullRowSelect = true; // T�m sat�r� se�me
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
                MessageBox.Show("L�tfen t�m alanlar� doldurun!", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddBookToXml(baslik, yazar); // XML'e ekle

            // ListView'e ekle
            var listViewItem = new ListViewItem(new[] { baslik, yazar });
            listViewKitaplar.Items.Add(listViewItem);

            MessageBox.Show("Kitap ba�ar�yla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtBaslik.Clear();
            txtYazar.Clear();
        }
        private void LoadBooksFromXml()
        {
            if (!System.IO.File.Exists(xmlFilePath)) return; // XML dosyas� yoksa y�kleme yapma

            XDocument xmlDoc = XDocument.Load(xmlFilePath); // XML dosyas�n� y�kle

            foreach (XElement kitap in xmlDoc.Descendants("kitap"))
            {
                string baslik = kitap.Element("baslik")?.Value; // <baslik> eleman�n�n de�eri
                string yazar = kitap.Element("yazar")?.Value;   // <yazar> eleman�n�n de�eri

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
                xmlDoc = XDocument.Load(xmlFilePath); // Mevcut XML dosyas�n� y�kle
            }
            else
            {
                xmlDoc = new XDocument(new XElement("kitaplar")); // Yeni bir XML olu�tur
            }

            // Yeni kitap eleman� olu�tur ve ekle
            XElement yeniKitap = new XElement("kitap",
                new XElement("baslik", baslik),
                new XElement("yazar", yazar)
            );

            xmlDoc.Root.Add(yeniKitap); // K�k eleman�na ekle
            xmlDoc.Save(xmlFilePath);  // XML dosyas�n� kaydet
        }
        private void UpdateBookInXml(string eskiBaslik, string eskiYazar, string yeniBaslik, string yeniYazar)
        {
            if (!System.IO.File.Exists(xmlFilePath)) return; // Dosya yoksa i�lem yapma

            XDocument xmlDoc = XDocument.Load(xmlFilePath);
            XElement kitapToUpdate = xmlDoc.Descendants("kitap")
                .FirstOrDefault(k => k.Element("baslik")?.Value == eskiBaslik && k.Element("yazar")?.Value == eskiYazar);

            if (kitapToUpdate != null)
            {
                kitapToUpdate.Element("baslik").Value = yeniBaslik; // Yeni ba�l�k
                kitapToUpdate.Element("yazar").Value = yeniYazar;   // Yeni yazar
                xmlDoc.Save(xmlFilePath); // G�ncellenmi� XML dosyas�n� kaydet
            }
        }
        private void DeleteBookFromXml(string baslik, string yazar)
        {
            if (!System.IO.File.Exists(xmlFilePath)) return; // Dosya yoksa i�lem yapma

            XDocument xmlDoc = XDocument.Load(xmlFilePath);
            XElement kitapToDelete = xmlDoc.Descendants("kitap")
                .FirstOrDefault(k => k.Element("baslik")?.Value == baslik && k.Element("yazar")?.Value == yazar);

            if (kitapToDelete != null)
            {
                kitapToDelete.Remove(); // XML'den eleman� kald�r
                xmlDoc.Save(xmlFilePath); // G�ncellenmi� XML dosyas�n� kaydet
            }
        }
        private void btnsil_Click(object sender, EventArgs e)
        {
            if (listViewKitaplar.SelectedItems.Count == 0)
            {
                MessageBox.Show("L�tfen silmek istedi�iniz kitab� se�in!", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = listViewKitaplar.SelectedItems[0];
            string baslik = selectedItem.SubItems[0].Text;
            string yazar = selectedItem.SubItems[1].Text;

            DeleteBookFromXml(baslik, yazar); // XML'den sil

            // ListView'den sil
            listViewKitaplar.Items.Remove(selectedItem);

            MessageBox.Show("Kitap ba�ar�yla silindi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnd�zenle_Click(object sender, EventArgs e)
        {
            if (listViewKitaplar.SelectedItems.Count == 0)
            {
                MessageBox.Show("L�tfen d�zenlemek istedi�iniz kitab� se�in!", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = listViewKitaplar.SelectedItems[0];
            string eskiBaslik = selectedItem.SubItems[0].Text;
            string eskiYazar = selectedItem.SubItems[1].Text;

            string yeniBaslik = txtBaslik.Text.Trim();
            string yeniYazar = txtYazar.Text.Trim();

            if (string.IsNullOrWhiteSpace(yeniBaslik) || string.IsNullOrWhiteSpace(yeniYazar))
            {
                MessageBox.Show("L�tfen t�m alanlar� doldurun!", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateBookInXml(eskiBaslik, eskiYazar, yeniBaslik, yeniYazar); // XML'de g�ncelle

            // ListView'de g�ncelle
            selectedItem.SubItems[0].Text = yeniBaslik;
            selectedItem.SubItems[1].Text = yeniYazar;

            MessageBox.Show("Kitap ba�ar�yla g�ncellendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtBaslik.Clear();
            txtYazar.Clear();

        }
    }
    }

