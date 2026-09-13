using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class StudentReportForm : Form
    {
        private DataGridView dgvStudents;
        private TextBox txtSearch;
        private Label lblTotalStats;
        private DataTable _studentDt;

        public StudentReportForm()
        {
            InitializeComponent();
            SetupUi();
            LoadStudentData();
        }

        private void SetupUi()
        {
            this.Text = "Student Report";

            // Title
            Label title = new Label();
            title.Text = "STUDENT DIRECTORY REPORT";
            title.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            title.ForeColor = Color.DarkBlue;
            title.AutoSize = false;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(20, 15);
            title.Size = new Size(935, 45);
            this.Controls.Add(title);

            // Search label & box
            Label lblSearch = new Label();
            lblSearch.Text = "Search:";
            lblSearch.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            lblSearch.Location = new Point(30, 72);
            lblSearch.Size = new Size(70, 25);
            this.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.Location = new Point(105, 68);
            txtSearch.Size = new Size(300, 30);
            txtSearch.TextChanged += TxtSearch_TextChanged;
            this.Controls.Add(txtSearch);

            // Stats summary label
            lblTotalStats = new Label();
            lblTotalStats.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            lblTotalStats.ForeColor = Color.DarkGreen;
            lblTotalStats.TextAlign = ContentAlignment.MiddleRight;
            lblTotalStats.Location = new Point(450, 70);
            lblTotalStats.Size = new Size(495, 28);
            this.Controls.Add(lblTotalStats);

            // DataGridView
            dgvStudents = new DataGridView();
            dgvStudents.Location = new Point(30, 110);
            dgvStudents.Size = new Size(915, 540);
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.ReadOnly = true;
            dgvStudents.RowHeadersVisible = false;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.EnableHeadersVisualStyles = false;
            dgvStudents.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvStudents.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvStudents.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvStudents.ColumnHeadersHeight = 35;
            dgvStudents.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvStudents.RowTemplate.Height = 28;
            dgvStudents.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvStudents.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.Controls.Add(dgvStudents);

            // Back button
            Button back = new Button();
            back.Text = "Back to Report Hub";
            back.Size = new Size(180, 42);
            back.Location = new Point(30, 665);
            back.BackColor = Color.SteelBlue;
            back.ForeColor = Color.White;
            back.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            back.FlatStyle = FlatStyle.Flat;
            back.FlatAppearance.BorderSize = 0;
            back.Cursor = Cursors.Hand;
            back.Click += (s, e) => ReturnToReportHub();
            this.Controls.Add(back);

            this.FormClosing += (s, e) => ReturnToReportHub();
        }

        private void LoadStudentData()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT 
                                        Studentid AS [Student ID], 
                                        Fullname AS [Full Name], 
                                        email AS [Email], 
                                        phone AS [Phone], 
                                        address AS [Address] 
                                     FROM Student 
                                     ORDER BY Studentid";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        _studentDt = new DataTable();
                        da.Fill(_studentDt);
                        dgvStudents.DataSource = _studentDt;
                        UpdateSummary();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary()
        {
            if (_studentDt != null)
            {
                int count = _studentDt.DefaultView.Count;
                lblTotalStats.Text = $"Showing {count} Student(s)";
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_studentDt == null) return;

            string search = txtSearch.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(search))
            {
                _studentDt.DefaultView.RowFilter = "";
            }
            else
            {
                _studentDt.DefaultView.RowFilter = string.Format(
                    "Convert([Student ID], 'System.String') LIKE '%{0}%' OR [Full Name] LIKE '%{0}%' OR [Email] LIKE '%{0}%' OR [Phone] LIKE '%{0}%' OR [Address] LIKE '%{0}%'",
                    search);
            }
            UpdateSummary();
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
    }
}