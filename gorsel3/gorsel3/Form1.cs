using System;
using System.IO;
using System.Windows.Forms;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using PdfiumViewer;


namespace gorsel3
{
    public partial class Form1 : Form
    {
        private PdfViewer _pdfViewer;

        public Form1()
        {
            InitializeComponent();

            // PdfViewer'ý formu dolduracak þekilde ayarlýyoruz
            _pdfViewer = new PdfViewer();
            _pdfViewer.Dock = DockStyle.Fill; // PdfViewer'ý panelin tamamýný dolduracak þekilde ayarlýyoruz
            this.Controls.Add(_pdfViewer); // PdfViewer'ý form kontrolüne ekliyoruz

            // Eðer baþka baþlangýç iþlemleri yapmanýz gerekirse buraya ekleyebilirsiniz
            AddColumnsToGrid();  // Örneðin verileri grid'e eklemek gibi
            AddDataToGrid();     // Verileri eklemeyi baþlatabilirsiniz


        }

        // Form1_Load metodunu EventHandler parametreleriyle düzeltelim
        private void Form1_Load(object sender, EventArgs e)
        {
           
            // Yükleme iþlemleri burada yapýlabilir
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Dosyasý|*.pdf";
            saveFileDialog.Title = "PDF Kaydet";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportDataGridViewToPdf(saveFileDialog.FileName);
            }
        }

        private void AddColumnsToGrid()
        {
            // DataGridView'e sütun ekleme
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Gönderen";
            dataGridView1.Columns[1].Name = "Mesaj";
        }

        private void AddDataToGrid()
        {
            // Önce sütunlarý ekleyelim
            AddColumnsToGrid();

            // Sonra verileri ekleyelim
            dataGridView1.Rows.Add("Zehra", "Merhaba!");
            dataGridView1.Rows.Add("aycan", "Nasýlsýn?");
            dataGridView1.Rows.Add("Zehra", "Ýyi, sen?");
        }

        private void ExportDataGridViewToPdf(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(stream); // iText.PdfWriter
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer); // iText.Kernel.Pdf.PdfDocument
                iText.Layout.Document document = new iText.Layout.Document(pdf);

                // Font oluþturun ve kalýn (bold) yapýn
                iText.Kernel.Font.PdfFont font = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
                iText.Layout.Element.Paragraph paragraph = new iText.Layout.Element.Paragraph("DataGridView Verileri")
                    .SetFont(font)  // Fontu paragrafa ekleyin
                    .SetFontSize(14);  // Font boyutunu ayarlayýn
                document.Add(paragraph);  // Baþlýðý PDF'e ekleyin

                // Tablo oluþtur
                iText.Layout.Element.Table table = new iText.Layout.Element.Table(dataGridView1.ColumnCount);

                // Baþlýklarý ekleyelim
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    table.AddHeaderCell(column.HeaderText);
                }

                // Satýrlarý ekleyelim
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Boþ satýr eklememek için
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            table.AddCell(cell.Value?.ToString() ?? "");
                        }
                    }
                }

                // Tabloyu PDF'e ekleyelim
                document.Add(table);
                document.Close();

                MessageBox.Show("PDF baþarýyla oluþturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnImportPdf_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Dosyasý|*.pdf";
            openFileDialog.Title = "PDF Seç";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // PdfiumViewer ile PDF dosyasýný yüklemek
                _pdfViewer.Load(openFileDialog.FileName); // PDF dosyasýný yüklemek için Load fonksiyonunu kullanýyoruz
            }
        }
    }
}
