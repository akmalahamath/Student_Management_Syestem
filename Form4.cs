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

        private string GetConnectionString()
        {
            if (ConfigurationManager.ConnectionStrings["Student"] != null)
            {
                return ConfigurationManager.ConnectionStrings["Student"].ConnectionString;
            }
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True;Connect Timeout=30";
        }

        private void Student_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

        private void LoadStudents(string searchTerm = "")
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = string.IsNullOrWhiteSpace(searchTerm)
                        ? "SELECT Studentid, Fullname, email, phone, address FROM Student"
                        : "SELECT Studentid, Fullname, email, phone, address FROM Student WHERE CAST(Studentid AS VARCHAR) LIKE @search OR Fullname LIKE @search OR email LIKE @search OR phone LIKE @search";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrWhiteSpace(searchTerm))
                        {
                            command.Parameters.AddWithValue("@search", "%" + searchTerm.Trim() + "%");
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dataGridView1.AutoGenerateColumns = false;
                            if (dataGridView1.Columns.Contains("Student_ID"))
                                dataGridView1.Columns["Student_ID"].DataPropertyName = "Studentid";
                            if (dataGridView1.Columns.Contains("Full_Name"))
                                dataGridView1.Columns["Full_Name"].DataPropertyName = "Fullname";
                            if (dataGridView1.Columns.Contains("Email"))
                                dataGridView1.Columns["Email"].DataPropertyName = "email";
                            if (dataGridView1.Columns.Contains("Phone"))
                                dataGridView1.Columns["Phone"].DataPropertyName = "phone";

                            dataGridView1.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox1.Focus();
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
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter Student ID and Full Name.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentId;
            if (!int.TryParse(textBox1.Text.Trim(), out studentId))
            {
                MessageBox.Show("Student ID must be a valid number.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "INSERT INTO Student (Studentid, Fullname, email, phone, address) VALUES (@Studentid, @Fullname, @email, @phone, @address)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Studentid", SqlDbType.Int).Value = studentId;
                        command.Parameters.Add("@Fullname", SqlDbType.VarChar, 100).Value = textBox2.Text.Trim();
                        command.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = textBox3.Text.Trim();
                        command.Parameters.Add("@phone", SqlDbType.VarChar, 15).Value = textBox4.Text.Trim();
                        command.Parameters.Add("@address", SqlDbType.VarChar, 200).Value = textBox5.Text.Trim();

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Student added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadStudents();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("A student with this ID already exists.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please specify a Student ID to update.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentId;
            if (!int.TryParse(textBox1.Text.Trim(), out studentId))
            {
                MessageBox.Show("Student ID must be a valid number.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "UPDATE Student SET Fullname = @Fullname, email = @email, phone = @phone, address = @address WHERE Studentid = @Studentid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Studentid", SqlDbType.Int).Value = studentId;
                        command.Parameters.Add("@Fullname", SqlDbType.VarChar, 100).Value = textBox2.Text.Trim();
                        command.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = textBox3.Text.Trim();
                        command.Parameters.Add("@phone", SqlDbType.VarChar, 15).Value = textBox4.Text.Trim();
                        command.Parameters.Add("@address", SqlDbType.VarChar, 200).Value = textBox5.Text.Trim();

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadStudents();
                        }
                        else
                        {
                            MessageBox.Show("No student found with ID: " + studentId, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter or select a Student ID to delete.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentId;
            if (!int.TryParse(textBox1.Text.Trim(), out studentId))
            {
                MessageBox.Show("Student ID must be a valid number.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Are you sure you want to delete student with ID: " + studentId + "?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    string query = "DELETE FROM Student WHERE Studentid = @Studentid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Studentid", SqlDbType.Int).Value = studentId;

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadStudents();
                        }
                        else
                        {
                            MessageBox.Show("No student found with ID: " + studentId, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearFields();
            textBox6.Clear();
            LoadStudents();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            LoadStudents(textBox6.Text);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells.Count > 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells["Student_ID"].Value != null && row.Cells["Student_ID"].Value != DBNull.Value)
                {
                    textBox1.Text = row.Cells["Student_ID"].Value.ToString();
                    textBox2.Text = row.Cells["Full_Name"].Value != null ? row.Cells["Full_Name"].Value.ToString() : "";
                    textBox3.Text = row.Cells["Email"].Value != null ? row.Cells["Email"].Value.ToString() : "";
                    textBox4.Text = row.Cells["Phone"].Value != null ? row.Cells["Phone"].Value.ToString() : "";

                    // Fetch address directly from bound DataTable row if available
                    DataRowView drv = row.DataBoundItem as DataRowView;
                    if (drv != null && drv.Row.Table.Columns.Contains("address"))
                    {
                        textBox5.Text = drv.Row["address"] != DBNull.Value ? drv.Row["address"].ToString() : "";
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);
        }
    }
}

