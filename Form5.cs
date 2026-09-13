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
            this.AutoSize = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximumSize = new Size(997, 800);
            this.MinimumSize = new Size(997, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += Form5_FormClosing;
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Dashboardform dashboard = new Dashboardform();
                dashboard.Show();
            }
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
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter Student ID, Student Name, and Course.");
                return;
            }

            if (!int.TryParse(textBox1.Text.Trim(), out int studentId))
            {
                MessageBox.Show("Student ID must be a valid number.");
                return;
            }

            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Please select a value for Academic Year, Semester, and Status.");
                return; 
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Enrollment (StudentID, StudentName, Course, AcademicYear, Semester, EnrollmentDate, Status) " +
                                   "VALUES (@StudentID, @StudentName, @Course, @AcademicYear, @Semester, @EnrollmentDate, @Status)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentId);
                        command.Parameters.AddWithValue("@StudentName", textBox2.Text.Trim());
                        command.Parameters.AddWithValue("@Course", textBox3.Text.Trim());
                        command.Parameters.AddWithValue("@AcademicYear", comboBox1.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Semester", comboBox2.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@EnrollmentDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Status", comboBox3.SelectedItem.ToString());

                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Enrollment added successfully!");
                button3_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter Student ID, Student Name, and Course.");
                return;
            }

            if (!int.TryParse(textBox1.Text.Trim(), out int studentId))
            {
                MessageBox.Show("Student ID must be a valid number.");
                return;
            }

            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Please select a value for Academic Year, Semester, and Status.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Enrollment SET StudentName=@StudentName, Course=@Course, AcademicYear=@AcademicYear, Semester=@Semester, EnrollmentDate=@EnrollmentDate, Status=@Status WHERE StudentID=@StudentID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentId);
                        command.Parameters.AddWithValue("@StudentName", textBox2.Text.Trim());
                        command.Parameters.AddWithValue("@Course", textBox3.Text.Trim());
                        command.Parameters.AddWithValue("@AcademicYear", comboBox1.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Semester", comboBox2.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@EnrollmentDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Status", comboBox3.SelectedItem.ToString());

                        int rows = command.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Enrollment updated successfully!");
                        }
                        else
                        {
                            MessageBox.Show("No enrollment record found with Student ID: " + studentId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }
    }
}
