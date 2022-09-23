using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

using IronOcr;

namespace ocr_db
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection connect = new SqlConnection(@"Data Source=localhost;Initial Catalog=ocrDB;Integrated Security=True");

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            if (open.ShowDialog() == DialogResult.OK)
            {
                var ocr = new IronTesseract();
                ocr.Language = OcrLanguage.English;
                var Result = ocr.Read(open.FileName);
                string TxtResult = Result.Text;
                richTextBox1.Text = TxtResult;
            }
            else {
                MessageBox.Show("Closed");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            if(save.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(save.FileName, RichTextBoxStreamType.RichText);
                MessageBox.Show("Successful");
            }
            else
            {
                MessageBox.Show("Failed");
            }
        }

        
        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("INSERT INTO OcrTable values('"+richTextBox1.Text+"')", connect);
            connect.Open();
            
            command.ExecuteNonQuery();

            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Successful add data to database");
            else
                MessageBox.Show("Failed to add data to database");

            connect.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            connect.Open();

            SqlCommand command = new SqlCommand("SELECT text FROM OcrTable WHERE id = @id", connect);
            command.Parameters.AddWithValue("id", textBox1.Text);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                richTextBox2.Text = reader["text"].ToString();
            }
            else
            {
                MessageBox.Show("Data not exist");
            }

            connect.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
