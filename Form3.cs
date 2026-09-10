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

namespace Student_Management_Syestem
{
   
    public partial class Signup : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True; Connect Timeout=30";
        public Signup()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Loginform login = new Loginform();
            login.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Signup (Firstname, Lastname, Email, Password, Idnumber, Faculty) VALUES (@firstname, @lastname, @email, @password, @idnumber, @faculty)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@firstname", textBox1.Text);
                command.Parameters.AddWithValue("@lastname", textBox2.Text);
                command.Parameters.AddWithValue("@email", textBox3.Text);
                command.Parameters.AddWithValue("@password", textBox4.Text);
                command.Parameters.AddWithValue("@idnumber", textBox5.Text);
                command.Parameters.AddWithValue("@faculty", textBox6.Text);

                command.ExecuteNonQuery();

                connection.Close();
                MessageBox.Show("Sign up successfully!");

                Loginform login = new Loginform();
                login.Show();
                this.Hide();

            }
        }
    }
}
