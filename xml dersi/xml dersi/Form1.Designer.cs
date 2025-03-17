namespace xml_dersi
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
            lblbaslik = new Label();
            yazar = new Label();
            txtBaslik = new TextBox();
            txtYazar = new TextBox();
            btnekle = new Button();
            btndüzenle = new Button();
            btnsil = new Button();
            listViewKitaplar = new ListView();
            listView2 = new ListView();
            SuspendLayout();
            // 
            // lblbaslik
            // 
            lblbaslik.AutoSize = true;
            lblbaslik.BackColor = SystemColors.Highlight;
            lblbaslik.Location = new Point(163, 21);
            lblbaslik.Name = "lblbaslik";
            lblbaslik.Size = new Size(47, 20);
            lblbaslik.TabIndex = 0;
            lblbaslik.Text = "Başlık";
            // 
            // yazar
            // 
            yazar.AutoSize = true;
            yazar.BackColor = SystemColors.Highlight;
            yazar.Location = new Point(12, 21);
            yazar.Name = "yazar";
            yazar.Size = new Size(44, 20);
            yazar.TabIndex = 1;
            yazar.Text = "Yazar";
            // 
            // txtBaslik
            // 
            txtBaslik.Location = new Point(575, 40);
            txtBaslik.Name = "txtBaslik";
            txtBaslik.Size = new Size(125, 27);
            txtBaslik.TabIndex = 2;
            // 
            // txtYazar
            // 
            txtYazar.Location = new Point(593, 320);
            txtYazar.Name = "txtYazar";
            txtYazar.Size = new Size(125, 27);
            txtYazar.TabIndex = 3;
            // 
            // btnekle
            // 
            btnekle.Location = new Point(606, 261);
            btnekle.Name = "btnekle";
            btnekle.Size = new Size(94, 29);
            btnekle.TabIndex = 4;
            btnekle.Text = "ekle";
            btnekle.UseVisualStyleBackColor = true;
            btnekle.Click += btnekle_Click;
            // 
            // btndüzenle
            // 
            btndüzenle.Location = new Point(606, 181);
            btndüzenle.Name = "btndüzenle";
            btndüzenle.Size = new Size(94, 29);
            btndüzenle.TabIndex = 5;
            btndüzenle.Text = "düzenle";
            btndüzenle.UseVisualStyleBackColor = true;
            btndüzenle.Click += btndüzenle_Click;
            // 
            // btnsil
            // 
            btnsil.Location = new Point(606, 104);
            btnsil.Name = "btnsil";
            btnsil.Size = new Size(94, 29);
            btnsil.TabIndex = 7;
            btnsil.Text = "sil";
            btnsil.UseVisualStyleBackColor = true;
            btnsil.Click += btnsil_Click;
            // 
            // listViewKitaplar
            // 
            listViewKitaplar.BackColor = SystemColors.Info;
            listViewKitaplar.Location = new Point(4, 58);
            listViewKitaplar.Name = "listViewKitaplar";
            listViewKitaplar.Size = new Size(553, 252);
            listViewKitaplar.TabIndex = 9;
            listViewKitaplar.UseCompatibleStateImageBehavior = false;
            // 
            // listView2
            // 
            listView2.Location = new Point(357, 336);
            listView2.Name = "listView2";
            listView2.Size = new Size(91, 71);
            listView2.TabIndex = 10;
            listView2.UseCompatibleStateImageBehavior = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(listView2);
            Controls.Add(listViewKitaplar);
            Controls.Add(btnsil);
            Controls.Add(btndüzenle);
            Controls.Add(btnekle);
            Controls.Add(txtYazar);
            Controls.Add(txtBaslik);
            Controls.Add(yazar);
            Controls.Add(lblbaslik);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblbaslik;
        private Label yazar;
        private TextBox txtBaslik;
        private TextBox txtYazar;
        private Button btnekle;
        private Button btndüzenle;
        private ListBox listBoxKitaplar;
        private Button btnsil;
        private ListView listViewKitaplar;
        private ListView listView2;
    }
}
