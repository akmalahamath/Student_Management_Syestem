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

                    // Check Signup table for student credentials
                    string query = "SELECT Firstname, Lastname, Email FROM Signup WHERE Email=@email AND Password=@password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string firstName = reader["Firstname"] != DBNull.Value ? reader["Firstname"].ToString() : "";
                                string lastName = reader["Lastname"] != DBNull.Value ? reader["Lastname"].ToString() : "";
                                string userEmail = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : email;

                                UserSession.SetUser((firstName + " " + lastName).Trim(), userEmail);

                                MessageBox.Show("Login Successful! Welcome, " + UserSession.UserName + ".", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                textBox1.Clear();
                                textBox2.Clear();

                                Dashboardform dashboard = new Dashboardform();
                                dashboard.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Email or Password.\n\nDemo Student: student@nsbm.lk / student123", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
