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

            // PdfViewer'� formu dolduracak �ekilde ayarl�yoruz
            _pdfViewer = new PdfViewer();
            _pdfViewer.Dock = DockStyle.Fill; // PdfViewer'� panelin tamam�n� dolduracak �ekilde ayarl�yoruz
            this.Controls.Add(_pdfViewer); // PdfViewer'� form kontrol�ne ekliyoruz

            // E�er ba�ka ba�lang�� i�lemleri yapman�z gerekirse buraya ekleyebilirsiniz
            AddColumnsToGrid();  // �rne�in verileri grid'e eklemek gibi
            AddDataToGrid();     // Verileri eklemeyi ba�latabilirsiniz


        }

        // Form1_Load metodunu EventHandler parametreleriyle d�zeltelim
        private void Form1_Load(object sender, EventArgs e)
        {
           
            // Y�kleme i�lemleri burada yap�labilir
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Dosyas�|*.pdf";
            saveFileDialog.Title = "PDF Kaydet";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportDataGridViewToPdf(saveFileDialog.FileName);
            }
        }

        private void AddColumnsToGrid()
        {
            // DataGridView'e s�tun ekleme
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "G�nderen";
            dataGridView1.Columns[1].Name = "Mesaj";
        }

        private void AddDataToGrid()
        {
            // �nce s�tunlar� ekleyelim
            AddColumnsToGrid();

            // Sonra verileri ekleyelim
            dataGridView1.Rows.Add("Zehra", "Merhaba!");
            dataGridView1.Rows.Add("aycan", "Nas�ls�n?");
            dataGridView1.Rows.Add("Zehra", "�yi, sen?");
        }

        private void ExportDataGridViewToPdf(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(stream); // iText.PdfWriter
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer); // iText.Kernel.Pdf.PdfDocument
                iText.Layout.Document document = new iText.Layout.Document(pdf);

                // Font olu�turun ve kal�n (bold) yap�n
                iText.Kernel.Font.PdfFont font = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
                iText.Layout.Element.Paragraph paragraph = new iText.Layout.Element.Paragraph("DataGridView Verileri")
                    .SetFont(font)  // Fontu paragrafa ekleyin
                    .SetFontSize(14);  // Font boyutunu ayarlay�n
                document.Add(paragraph);  // Ba�l��� PDF'e ekleyin

                // Tablo olu�tur
                iText.Layout.Element.Table table = new iText.Layout.Element.Table(dataGridView1.ColumnCount);

                // Ba�l�klar� ekleyelim
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    table.AddHeaderCell(column.HeaderText);
                }

                // Sat�rlar� ekleyelim
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow) // Bo� sat�r eklememek i�in
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

                MessageBox.Show("PDF ba�ar�yla olu�turuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnImportPdf_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Dosyas�|*.pdf";
            openFileDialog.Title = "PDF Se�";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // PdfiumViewer ile PDF dosyas�n� y�klemek
                _pdfViewer.Load(openFileDialog.FileName); // PDF dosyas�n� y�klemek i�in Load fonksiyonunu kullan�yoruz
            }
        }
    }
}
