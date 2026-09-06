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
using System.Configuration;

namespace Student_Management_Syestem
{
    public partial class Student : Form
    {
        public Student()
        {
            InitializeComponent();
        }

        private void Student_Load(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Dashboardform dashboard = new Dashboardform();
            dashboard.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString =
       @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\Database1.mdf;
        Integrated Security=True;
        Connect Timeout=30";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Student (Studentid,FullName, Email, Phone, Address) VALUES (@Studentid,@Fullname, @email, @phone, @address)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Studentid", textBox1.Text);
                    command.Parameters.AddWithValue("@Fullname", textBox2.Text);
                    command.Parameters.AddWithValue("@email", textBox3.Text);
                    command.Parameters.AddWithValue("@phone", textBox4.Text);
                    command.Parameters.AddWithValue("@address", textBox5.Text);
                    command.ExecuteNonQuery();

                }
                connection.Close();
                MessageBox.Show("Student added successfully!");

            }
        }
    }
}
