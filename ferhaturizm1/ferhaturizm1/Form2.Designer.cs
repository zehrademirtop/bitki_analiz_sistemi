namespace ferhaturizm1
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            kaydet = new Button();
            listele = new Button();
            getir = new Button();
            temizle = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            listView1 = new ListView();
            label1 = new Label();
            Marka = new Label();
            Model = new Label();
            ModelYili = new Label();
            Kapasite = new Label();
            SuspendLayout();
            // 
            // kaydet
            // 
            kaydet.Location = new Point(665, 27);
            kaydet.Name = "kaydet";
            kaydet.Size = new Size(94, 29);
            kaydet.TabIndex = 0;
            kaydet.Text = "kaydet";
            kaydet.UseVisualStyleBackColor = true;
            kaydet.Click += kaydet_Click;
            // 
            // listele
            // 
            listele.Location = new Point(665, 76);
            listele.Name = "listele";
            listele.Size = new Size(94, 29);
            listele.TabIndex = 1;
            listele.Text = "listele";
            listele.UseVisualStyleBackColor = true;
            listele.Click += listele_Click;
            // 
            // getir
            // 
            getir.Location = new Point(665, 115);
            getir.Name = "getir";
            getir.Size = new Size(94, 29);
            getir.TabIndex = 2;
            getir.Text = "getir";
            getir.UseVisualStyleBackColor = true;
            getir.Click += getir_Click;
            // 
            // temizle
            // 
            temizle.Location = new Point(665, 169);
            temizle.Name = "temizle";
            temizle.Size = new Size(94, 29);
            temizle.TabIndex = 3;
            temizle.Text = "temizle";
            temizle.UseVisualStyleBackColor = true;
            temizle.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 328);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 5;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(157, 328);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 27);
            textBox2.TabIndex = 6;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(288, 328);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(125, 27);
            textBox3.TabIndex = 7;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(431, 328);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(125, 27);
            textBox4.TabIndex = 8;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(578, 328);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(125, 27);
            textBox5.TabIndex = 9;
            // 
            // listView1
            // 
            listView1.Location = new Point(171, 12);
            listView1.Name = "listView1";
            listView1.Size = new Size(458, 310);
            listView1.TabIndex = 10;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 372);
            label1.Name = "label1";
            label1.Size = new Size(72, 20);
            label1.TabIndex = 11;
            label1.Text = "OtobusID";
            // 
            // Marka
            // 
            Marka.AutoSize = true;
            Marka.Location = new Point(180, 372);
            Marka.Name = "Marka";
            Marka.Size = new Size(50, 20);
            Marka.TabIndex = 12;
            Marka.Text = "Marka";
            // 
            // Model
            // 
            Model.AutoSize = true;
            Model.Location = new Point(318, 372);
            Model.Name = "Model";
            Model.Size = new Size(52, 20);
            Model.TabIndex = 13;
            Model.Text = "Model";
            // 
            // ModelYili
            // 
            ModelYili.AutoSize = true;
            ModelYili.Location = new Point(465, 372);
            ModelYili.Name = "ModelYili";
            ModelYili.Size = new Size(72, 20);
            ModelYili.TabIndex = 14;
            ModelYili.Text = "ModelYili";
            // 
            // Kapasite
            // 
            Kapasite.AutoSize = true;
            Kapasite.Location = new Point(608, 372);
            Kapasite.Name = "Kapasite";
            Kapasite.Size = new Size(66, 20);
            Kapasite.TabIndex = 15;
            Kapasite.Text = "Kapasite";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Kapasite);
            Controls.Add(ModelYili);
            Controls.Add(Model);
            Controls.Add(Marka);
            Controls.Add(label1);
            Controls.Add(listView1);
            Controls.Add(textBox5);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(temizle);
            Controls.Add(getir);
            Controls.Add(listele);
            Controls.Add(kaydet);
            Name = "Form2";
            Text = "Form2";
           
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button kaydet;
        private Button listele;
        private Button getir;
        private Button temizle;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private ListView listView1;
        private Label label1;
        private Label Marka;
        private Label Model;
        private Label ModelYili;
        private Label Kapasite;
    }
}