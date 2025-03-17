namespace aycann
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
            label1 = new Label();
            label2 = new Label();
            comboBox1 = new ComboBox();
            label3 = new Label();
            comboBox2 = new ComboBox();
            label4 = new Label();
            comboBox3 = new ComboBox();
            dateTimePicker1 = new DateTimePicker();
            dateTimePicker2 = new DateTimePicker();
            label6 = new Label();
            label7 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            button1 = new Button();
            textBox1 = new TextBox();
            label5 = new Label();
            label12 = new Label();
            label13 = new Label();
            label14 = new Label();
            label15 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(73, 20);
            label1.TabIndex = 0;
            label1.Text = "Ad Soyad";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 50);
            label2.Name = "label2";
            label2.Size = new Size(94, 20);
            label2.TabIndex = 2;
            label2.Text = "Gidilecek Yer";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(118, 50);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(136, 28);
            comboBox1.TabIndex = 3;
           
            comboBox1.Click += comboBox1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(280, 12);
            label3.Name = "label3";
            label3.Size = new Size(57, 20);
            label3.TabIndex = 4;
            label3.Text = "Otobüs";
           
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(343, 8);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(124, 28);
            comboBox2.TabIndex = 5;
       
            comboBox2.Click += comboBox2_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(286, 50);
            label4.Name = "label4";
            label4.Size = new Size(51, 20);
            label4.TabIndex = 6;
            label4.Text = "Koltuk";
          
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(343, 42);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(78, 28);
            comboBox3.TabIndex = 7;
            comboBox3.DropDown += comboBox3_DropDown;
          
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(504, 6);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(250, 27);
            dateTimePicker1.TabIndex = 8;
          
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Format = DateTimePickerFormat.Time;
            dateTimePicker2.Location = new Point(504, 42);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(110, 27);
            dateTimePicker2.TabIndex = 9;
            dateTimePicker2.Value = new DateTime(2024, 11, 28, 0, 0, 0, 0);
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 234);
            label6.Name = "label6";
            label6.Size = new Size(73, 20);
            label6.TabIndex = 11;
            label6.Text = "Ad Soyad";
           
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 292);
            label7.Name = "label7";
            label7.Size = new Size(90, 20);
            label7.TabIndex = 12;
            label7.Text = "Gidilicek Yer";
          
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(223, 234);
            label9.Name = "label9";
            label9.Size = new Size(57, 20);
            label9.TabIndex = 14;
            label9.Text = "Otobüs";
          
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(447, 234);
            label10.Name = "label10";
            label10.Size = new Size(92, 20);
            label10.TabIndex = 15;
            label10.Text = "Tarih ve Saat";

            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(256, 292);
            label11.Name = "label11";
            label11.Size = new Size(72, 20);
            label11.TabIndex = 16;
            label11.Text = "Koltuk no";
          
            // 
            // button1
            // 
            button1.Location = new Point(256, 126);
            button1.Name = "button1";
            button1.Size = new Size(126, 29);
            button1.TabIndex = 17;
            button1.Text = "Bilet Oluştur";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(118, 8);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 18;
          
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(91, 234);
            label5.Name = "label5";
            label5.Size = new Size(50, 20);
            label5.TabIndex = 19;
            label5.Text = "label5";
           
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(118, 292);
            label12.Name = "label12";
            label12.Size = new Size(58, 20);
            label12.TabIndex = 20;
            label12.Text = "label12";
          
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(286, 234);
            label13.Name = "label13";
            label13.Size = new Size(58, 20);
            label13.TabIndex = 21;
            label13.Text = "label13";
        
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(556, 234);
            label14.Name = "label14";
            label14.Size = new Size(58, 20);
            label14.TabIndex = 22;
            label14.Text = "label14";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(363, 292);
            label15.Name = "label15";
            label15.Size = new Size(58, 20);
            label15.TabIndex = 23;
            label15.Text = "label15";
          
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label15);
            Controls.Add(label14);
            Controls.Add(label13);
            Controls.Add(label12);
            Controls.Add(label5);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(dateTimePicker2);
            Controls.Add(dateTimePicker1);
            Controls.Add(comboBox3);
            Controls.Add(label4);
            Controls.Add(comboBox2);
            Controls.Add(label3);
            Controls.Add(comboBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private ComboBox comboBox1;
        private Label label3;
        private ComboBox comboBox2;
        private Label label4;
        private ComboBox comboBox3;
        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private Label label6;
        private Label label7;
        
        private Label label9;
        private Label label10;
        private Label label11;
        private Button button1;
        private TextBox textBox1;
        private Label label5;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
    }
}
