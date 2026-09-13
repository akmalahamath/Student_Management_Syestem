using System;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class EnrollmentReportForm : Form
    {
        public EnrollmentReportForm()
        {
            InitializeComponent();

            // Form settings
            this.BackColor = Color.AliceBlue;
            this.Text = "Enrollment Report";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            Label title = new Label();
            title.Text = "ENROLLMENT REPORT";
            title.Font = new Font("Arial", 20, FontStyle.Bold);
            title.ForeColor = Color.DarkBlue;
            title.AutoSize = false;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(20, 20);
            title.Size = new Size(760, 50);

            this.Controls.Add(title);

            // DataGridView
            DataGridView dgvEnrollments = new DataGridView();

            dgvEnrollments.Location = new Point(20, 90);
            dgvEnrollments.Size = new Size(760, 310);

            // Columns
            dgvEnrollments.ColumnCount = 5;

            dgvEnrollments.Columns[0].Name = "Enrollment ID";
            dgvEnrollments.Columns[1].Name = "Student ID";
            dgvEnrollments.Columns[2].Name = "Student Name";
            dgvEnrollments.Columns[3].Name = "Course";
            dgvEnrollments.Columns[4].Name = "Date";

            // Sample data
            dgvEnrollments.Rows.Add(
                "E001", "S001", "John", "Computer Science", "2026-09-01");

            dgvEnrollments.Rows.Add(
                "E002", "S002", "Anna", "Information Technology", "2026-09-02");

            dgvEnrollments.Rows.Add(
                "E003", "S003", "David", "Software Engineering", "2026-09-03");

            // Table settings
            dgvEnrollments.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvEnrollments.AllowUserToAddRows = false;
            dgvEnrollments.ReadOnly = true;
            dgvEnrollments.RowHeadersVisible = false;
            dgvEnrollments.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            // Table header style
            dgvEnrollments.EnableHeadersVisualStyles = false;
            dgvEnrollments.ColumnHeadersDefaultCellStyle.BackColor =
                Color.SteelBlue;
            dgvEnrollments.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;
            dgvEnrollments.ColumnHeadersDefaultCellStyle.Font =
                new Font("Arial", 10, FontStyle.Bold);

            dgvEnrollments.ColumnHeadersHeight = 35;

            // Table row style
            dgvEnrollments.DefaultCellStyle.Font =
                new Font("Arial", 10);

            dgvEnrollments.RowTemplate.Height = 30;

            dgvEnrollments.DefaultCellStyle.SelectionBackColor =
                Color.LightSteelBlue;
            dgvEnrollments.DefaultCellStyle.SelectionForeColor =
                Color.Black;

            this.Controls.Add(dgvEnrollments);

            // Back button
            Button back = new Button();

            back.Text = "Back";
            back.Size = new Size(120, 40);
            back.Location = new Point(20, 415);

            back.BackColor = Color.SteelBlue;
            back.ForeColor = Color.White;
            back.Font = new Font("Arial", 10, FontStyle.Bold);
            back.FlatStyle = FlatStyle.Flat;
            back.FlatAppearance.BorderSize = 0;

            try
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True; Connect Timeout=30";
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT StudentID, StudentName, Course, EnrollmentDate FROM Enrollment";
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    using (System.Data.SqlClient.SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            dgvEnrollments.Rows.Clear();
                            int id = 1;
                            while (r.Read())
                            {
                                dgvEnrollments.Rows.Add("E" + id.ToString("D3"), r[0].ToString(), r[1].ToString(), r[2].ToString(), r[3].ToString());
                                id++;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Fallback to sample rows if DB is unavailable
            }

            // Back to Report Hub
            back.Click += (s, e) =>
            {
                ReturnToReportHub();
            };

            this.FormClosing += (s, e) =>
            {
                ReturnToReportHub();
            };

            this.Controls.Add(back);
        }

        private bool _isReturning = false;
        private void ReturnToReportHub()
        {
            if (_isReturning) return;
            _isReturning = true;

            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is ReportHubForm)
                {
                    openForm.Show();
                    this.Dispose();
                    return;
                }
            }

            ReportHubForm report = new ReportHubForm();
            report.Show();
            this.Dispose();
        }

        private void EnrollmentReportForm_Load(object sender, EventArgs e)
        {

        }
    }
}