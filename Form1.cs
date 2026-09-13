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

namespace Student_Management_Syestem
{
    public partial class Loginform : Form
    {
        public Loginform()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += Loginform_FormClosing;
        }

        private void Loginform_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter Email and Password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DbHelper.GetConnection())
                {
                    connection.Open();

                    // Check Signup table (support email or 'admin' shortcut for admin@nsbm.lk)
                    string query = "SELECT Firstname, Lastname, Role, Email FROM Signup " +
                                   "WHERE (Email=@email OR (Email='admin@nsbm.lk' AND @email='admin')) AND Password=@password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["Role"] != DBNull.Value ? reader["Role"].ToString() : "Student";
                                string firstName = reader["Firstname"] != DBNull.Value ? reader["Firstname"].ToString() : "";
                                string lastName = reader["Lastname"] != DBNull.Value ? reader["Lastname"].ToString() : "";
                                string userEmail = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : email;

                                if (email.Equals("admin", StringComparison.OrdinalIgnoreCase) || email.StartsWith("admin@", StringComparison.OrdinalIgnoreCase))
                                {
                                    role = "Admin";
                                }

                                UserSession.SetUser(role, (firstName + " " + lastName).Trim(), userEmail);

                                MessageBox.Show("Login Successful! Welcome, " + UserSession.UserName + " (" + UserSession.Role + ").", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                textBox1.Clear();
                                textBox2.Clear();

                                Dashboardform dashboard = new Dashboardform();
                                dashboard.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Email or Password.\n\nDefault Admin: admin@nsbm.lk / admin123\nDefault Student: student@nsbm.lk / student123", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Signup sign = new Signup();
            sign.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
