namespace gorsel3
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnExportPdf = new Button();
            dataGridView1 = new DataGridView();
            btnImportPdf = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // btnExportPdf
            // 
            btnExportPdf.Location = new Point(12, 12);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.Size = new Size(94, 29);
            btnExportPdf.TabIndex = 0;
            btnExportPdf.Text = "button1";
            btnExportPdf.UseVisualStyleBackColor = true;
            btnExportPdf.Click += btnExportPdf_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 117);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(324, 220);
            dataGridView1.TabIndex = 1;
            // 
            // btnImportPdf
            // 
            btnImportPdf.Location = new Point(12, 65);
            btnImportPdf.Name = "btnImportPdf";
            btnImportPdf.Size = new Size(94, 29);
            btnImportPdf.TabIndex = 2;
            btnImportPdf.Text = "button1";
            btnImportPdf.UseVisualStyleBackColor = true;
            btnImportPdf.Click += btnImportPdf_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnImportPdf);
            Controls.Add(dataGridView1);
            Controls.Add(btnExportPdf);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnExportPdf;
        private DataGridView dataGridView1;
        private Button btnImportPdf;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
