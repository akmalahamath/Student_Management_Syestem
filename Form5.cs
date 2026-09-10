using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Student_Management_Syestem
{
   
    public partial class Form5 : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True; Connect Timeout=30";
        public Form5()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Entrollment (StudentID,StudentName, Course, AcademicYear, Semester, EnrollmentDate, Status) VALUES (@Studentid,@StudentName, @Course, @Academicyear, @Semester, @Entrollmentdate,@Status)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", textBox1.Text);
                    command.Parameters.AddWithValue("@StudentName", textBox2.Text);
                    command.Parameters.AddWithValue("@Course", textBox3.Text);
                    command.Parameters.AddWithValue("@AcademicYear", comboBox1.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@Semester", comboBox2.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@EnrollmentDate", dateTimePicker1.Value.Date);
                    command.Parameters.AddWithValue("@Status", comboBox2.SelectedItem.ToString());

                    command.ExecuteNonQuery();

                }
                connection.Close();
                MessageBox.Show("Student added successfully!");

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
