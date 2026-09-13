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
        public Signup()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += Signup_FormClosing;
        }

        private void Signup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Loginform login = new Loginform();
                login.Show();
            }
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
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please fill in First Name, Email, and Password.");
                return;
            }

            try
            {
                using (SqlConnection connection = DbHelper.GetConnection())
                {
                    connection.Open();

                    string role = "Student";
                    string query = "INSERT INTO Signup (Firstname, Lastname, Email, Password, Idnumber, Faculty, Role) VALUES (@firstname, @lastname, @email, @password, @idnumber, @faculty, @role)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@firstname", textBox1.Text.Trim());
                        command.Parameters.AddWithValue("@lastname", textBox2.Text.Trim());
                        command.Parameters.AddWithValue("@email", textBox3.Text.Trim());
                        command.Parameters.AddWithValue("@password", textBox4.Text.Trim());
                        int idNum = 0;
                        int.TryParse(textBox5.Text.Trim(), out idNum);
                        command.Parameters.AddWithValue("@idnumber", idNum);
                        command.Parameters.AddWithValue("@faculty", textBox6.Text.Trim());
                        command.Parameters.AddWithValue("@role", role);

                        command.ExecuteNonQuery();
                    }

                    // If a student signed up, also create a student record so they appear in student lists
                    int studentId = 0;
                    if (int.TryParse(textBox5.Text.Trim(), out studentId) && studentId > 0)
                    {
                        string fullName = (textBox1.Text.Trim() + " " + textBox2.Text.Trim()).Trim();
                        string addStudentSql = @"
IF NOT EXISTS (SELECT 1 FROM Student WHERE Studentid = @id)
BEGIN
    INSERT INTO Student (Studentid, Fullname, email, phone, address)
    VALUES (@id, @name, @email, '', @faculty);
END";
                        using (SqlCommand cmdStudent = new SqlCommand(addStudentSql, connection))
                        {
                            cmdStudent.Parameters.AddWithValue("@id", studentId);
                            cmdStudent.Parameters.AddWithValue("@name", fullName);
                            cmdStudent.Parameters.AddWithValue("@email", textBox3.Text.Trim());
                            cmdStudent.Parameters.AddWithValue("@faculty", textBox6.Text.Trim());
                            cmdStudent.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Sign up successfully! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Loginform login = new Loginform();
                    login.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
