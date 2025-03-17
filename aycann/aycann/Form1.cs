namespace aycann
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "HH:mm";

        }
        private void button1_Click(object sender, EventArgs e)
        {

            label5.Text = textBox1.Text;
            label12.Text = comboBox1.Text;
            label13.Text = comboBox2.Text;
            label15.Text = comboBox3.Text;
            label14.Text = dateTimePicker1.Text + " " + dateTimePicker2.Text;

        }

        private void comboBox3_DropDown(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            for (int i = 1; i <= 50; i++)
                comboBox3.Items.Add(i);
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            string[] yeniOgeler = { "Özdiyarbakýr", "Hasdiyarbakýr", "Vangölü" };
            comboBox2.Items.AddRange(yeniOgeler);
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            string[] yeniOgeler = { "diyarbakýr", "Mersin", "izmir", "aðrý", "Van" };
            comboBox1.Items.AddRange(yeniOgeler);
        }

   
    }
}
