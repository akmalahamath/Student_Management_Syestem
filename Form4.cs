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
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True; Connect Timeout=30";
        public Student()
        {
            InitializeComponent();
            this.FormClosing += Student_FormClosing;
            this.textBox6.TextChanged += textBox6_TextChanged;
            this.dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void Student_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Dashboardform dashboard = new Dashboardform();
                dashboard.Show();
            }
        }

        private void Student_Load(object sender, EventArgs e)
        {
            LoadStudents();
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Fprm7 course = new Fprm7();
            course.Show();
            this.Hide();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form5 enrollment = new Form5();
            enrollment.Show();
            this.Hide();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Form6 payment = new Form6();
            payment.Show();
            this.Hide();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            ReportHubForm report = new ReportHubForm();
            report.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Please enter Student ID and Full Name.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Student (Studentid,FullName, Email, Phone, Address) VALUES (@Studentid,@Fullname, @email, @phone, @address)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Studentid", textBox1.Text.Trim());
                        command.Parameters.AddWithValue("@Fullname", textBox2.Text.Trim());
                        command.Parameters.AddWithValue("@email", textBox3.Text.Trim());
                        command.Parameters.AddWithValue("@phone", textBox4.Text.Trim());
                        command.Parameters.AddWithValue("@address", textBox5.Text.Trim());

                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Student added successfully!");
                    button4_Click(sender, e);
                    LoadStudents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }
      
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells["Studentid"]?.Value != null) textBox1.Text = row.Cells["Studentid"].Value.ToString();
                if (row.Cells["FullName"]?.Value != null) textBox2.Text = row.Cells["FullName"].Value.ToString();
                if (row.Cells["Email"]?.Value != null) textBox3.Text = row.Cells["Email"].Value.ToString();
                if (row.Cells["Phone"]?.Value != null) textBox4.Text = row.Cells["Phone"].Value.ToString();
                if (row.Cells["Address"]?.Value != null) textBox5.Text = row.Cells["Address"].Value.ToString();
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.DataSource is DataTable dt)
                {
                    DataView dv = dt.DefaultView;
                    string filter = textBox6.Text.Trim().Replace("'", "''");
                    if (string.IsNullOrEmpty(filter))
                    {
                        dv.RowFilter = "";
                    }
                    else
                    {
                        dv.RowFilter = string.Format("Fullname LIKE '%{0}%' OR Email LIKE '%{0}%' OR Convert(Studentid, 'System.String') LIKE '%{0}%'", filter);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void LoadStudents()
        {
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Student";

                    SqlDataAdapter adapter =
                        new SqlDataAdapter(query, connection);

                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }
    }
}
