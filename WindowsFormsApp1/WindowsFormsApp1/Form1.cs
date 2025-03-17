using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace gorsel3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // DataGridView'deki verileri PDF'ye aktarma
        private void ExportDataGridViewToPdf(string filePath)
        {
            // DataGridView'in sütun sayısını kontrol et
            if (dataGridView1.ColumnCount == 0)
            {
                MessageBox.Show("DataGridView içinde sütun yok!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;  // İşlemi durdur
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(stream);
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdf);

                // **Türkçe Karakter Destekleyen Fontu Kullan**
                string fontPath = "C:\\Windows\\Fonts\\arial.ttf"; // Windows'taki Arial fontunun yolu
                iText.Kernel.Font.PdfFont font = iText.Kernel.Font.PdfFontFactory.CreateFont(fontPath, iText.IO.Font.PdfEncodings.IDENTITY_H);


                // **Logo Dosya Yolu**
                string logoPath = "C:\\Users\\demir\\Desktop\\Iğdır_University_logo.svg.png";

                // **Logo Görselini Ekle**
                iText.Layout.Element.Image logo = new iText.Layout.Element.Image(iText.IO.Image.ImageDataFactory.Create(logoPath));

                // **Logo Boyutunu Küçült**
                logo.ScaleToFit(80, 80); // Genişlik: 80px, Yükseklik: 80px
                logo.SetFixedPosition(36, pdf.GetDefaultPageSize().GetTop() - 80); // Sol üst köşeye ayarlar
                document.Add(logo); // Logoyu ekleyelim

                // **Sağ Üst Köşeye "Iğdır Üniversitesi" Yazısı**
                iText.Layout.Element.Paragraph universityText = new iText.Layout.Element.Paragraph("IĞDIR ÜNİVERSİTESİ")
                    .SetFont(font)
                    .SetFontSize(14)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                document.Add(universityText); // Yazıyı ekleyelim

                // **Başlık (Ortalı)**
                iText.Layout.Element.Paragraph paragraph = new iText.Layout.Element.Paragraph("")
                    .SetFont(font)
                    .SetFontSize(14)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                document.Add(paragraph);

                // **Tabloyu Ortalamak İçin Sayfa Genişliği Al**
                float pageWidth = pdf.GetDefaultPageSize().GetWidth() - document.GetLeftMargin() - document.GetRightMargin();

                // **Tabloyu Oluştur**
                iText.Layout.Element.Table table = new iText.Layout.Element.Table(dataGridView1.ColumnCount)
                    .SetWidth(pageWidth * 0.8f)
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                // **Sütun Başlıklarını Ekleyelim**
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(column.HeaderText).SetFont(font)));
                }

                // **Satırları Ekleyelim**
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Boş satır eklememek için
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(cell.Value?.ToString() ?? "").SetFont(font)));
                        }
                    }
                }

                document.Add(table);  // **Tabloyu PDF'e ekle**
                document.Close();  // **PDF'i Kaydet ve Kapat**

                MessageBox.Show("PDF başarıyla oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void AddColumnsToGrid()
        {
            // DataGridView'e sütun ekleyelim
            if (dataGridView1.ColumnCount == 0)
            {
                dataGridView1.ColumnCount = 2;
                dataGridView1.Columns[0].Name = "Gönderen";
                dataGridView1.Columns[1].Name = "Mesaj";
            }
        }

        private void AddDataToGrid()
        {
            // DataGridView'e veri ekleyelim
            dataGridView1.Rows.Add("Zehra", "Merhaba!");
            dataGridView1.Rows.Add("Aycan", "Nasılsın?");
            dataGridView1.Rows.Add("Zehra", "İyi, sen?");
        }
        // PDF dosyasından metin okuma
        public string ReadPdf(string filePath)
        {
            StringBuilder text = new StringBuilder();

            // PDF dosyasını açıyoruz
            using (PdfReader reader = new PdfReader(filePath))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    // Sayfa metnini çıkartıyoruz
                    string pageText = PdfTextExtractor.GetTextFromPage(reader, i);
                    text.Append(pageText);
                }
            }

            // Çıkarılan metni döndürüyoruz
            return text.ToString();
        }
        // PDF'yi seçme ve okuma işlemi
        private void btnImportPdf_Click(object sender, EventArgs e)
        {
           
        }

        // PDF dosyasına DataGridView verilerini aktar
        private void btnExportPdf_Click(object sender, EventArgs e)
        {
           
        }

        private void btnExportPdf_Click_1(object sender, EventArgs e)
        {
            try
            {
                // PDF dosyasını kaydetmek için SaveFileDialog
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Dosyası|*.pdf";
                saveFileDialog.Title = "PDF Kaydet";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Dosyayı kaydetme
                    ExportDataGridViewToPdf(saveFileDialog.FileName);  // PDF'i kaydet
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }


        private void btnImportPdf_Click_1(object sender, EventArgs e)
        {

            // OpenFileDialog ile kullanıcıdan PDF dosyası seçmesini iste
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Dosyası|*.pdf";
            openFileDialog.Title = "PDF Seç";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // PDF dosyasının yolunu al
                string filePath = openFileDialog.FileName;

                // WebBrowser üzerinden PDF dosyasını görüntüle
                webBrowser1.Navigate(filePath);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddColumnsToGrid(); // Sütunları ekle
            AddDataToGrid();    // Verileri ekle
        }
    }
    }

