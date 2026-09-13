using System;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class StudentReportForm : Form
    {
        public StudentReportForm()
        {
            InitializeComponent();

            // =========================
            // FORM SETTINGS
            // =========================
            this.BackColor = Color.AliceBlue;
            this.Text = "Student Report";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;


            // =========================
            // STUDENT TABLE
            // =========================
            dgvStudents.ColumnCount = 5;

            dgvStudents.Columns[0].Name = "Student ID";
            dgvStudents.Columns[1].Name = "Name";
            dgvStudents.Columns[2].Name = "Gender";
            dgvStudents.Columns[3].Name = "Email";
            dgvStudents.Columns[4].Name = "Phone";

            dgvStudents.Rows.Add(
                "S001", "John", "Male", "john@gmail.com", "0771234567");

            dgvStudents.Rows.Add(
                "S002", "Anna", "Female", "anna@gmail.com", "0772345678");

            dgvStudents.Rows.Add(
                "S003", "David", "Male", "david@gmail.com", "0773456789");


            // Table settings
            dgvStudents.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.ReadOnly = true;
            dgvStudents.RowHeadersVisible = false;
            dgvStudents.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvStudents.Location = new Point(20, 90);
            dgvStudents.Size = new Size(760, 310);

            dgvStudents.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;


            // =========================
            // TABLE HEADER STYLE
            // =========================
            dgvStudents.ColumnHeadersDefaultCellStyle.BackColor =
                Color.SteelBlue;

            dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvStudents.ColumnHeadersDefaultCellStyle.Font =
                new Font("Arial", 10, FontStyle.Bold);

            dgvStudents.ColumnHeadersHeight = 35;

            dgvStudents.EnableHeadersVisualStyles = false;


            // =========================
            // TABLE ROW STYLE
            // =========================
            dgvStudents.DefaultCellStyle.Font =
                new Font("Arial", 10);

            dgvStudents.RowTemplate.Height = 30;

            dgvStudents.DefaultCellStyle.SelectionBackColor =
                Color.LightSteelBlue;

            dgvStudents.DefaultCellStyle.SelectionForeColor =
                Color.Black;


            // =========================
            // TITLE
            // =========================
            Label title = new Label();

            title.Text = "STUDENT REPORT";

            title.Font =
                new Font("Arial", 20, FontStyle.Bold);

            title.ForeColor =
                Color.DarkBlue;

            title.AutoSize = false;

            title.TextAlign =
                ContentAlignment.MiddleCenter;

            title.Location =
                new Point(20, 20);

            title.Size =
                new Size(760, 50);

            this.Controls.Add(title);


            // =========================
            // BACK BUTTON
            // =========================
            Button back = new Button();

            back.Text = "Back";

            back.Size =
                new Size(120, 40);

            // Moved slightly higher
            back.Location =
                new Point(20, 415);

            back.BackColor =
                Color.SteelBlue;

            back.ForeColor =
                Color.White;

            back.Font =
                new Font("Arial", 10, FontStyle.Bold);

            back.FlatStyle =
                FlatStyle.Flat;

            back.FlatAppearance.BorderSize = 0;


            try
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True; Connect Timeout=30";
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Studentid, Fullname, email, phone FROM Student";
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    using (System.Data.SqlClient.SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            dgvStudents.Rows.Clear();
                            while (r.Read())
                            {
                                dgvStudents.Rows.Add(r[0].ToString(), r[1].ToString(), "N/A", r[2].ToString(), r[3].ToString());
                            }
                        }
                    }
                }
            }
            catch
            {
                // Fallback to sample rows if DB is unavailable
            }

            // Back button click
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

        private void StudentReportForm_Load(
            object sender, EventArgs e)
        {

        }
    }
}