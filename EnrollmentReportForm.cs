using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class EnrollmentReportForm : Form
    {
        private DataGridView dgvEnrollments;
        private TextBox txtSearch;
        private Label lblTotalStats;
        private DataTable _enrollmentDt;

        public EnrollmentReportForm()
        {
            InitializeComponent();
            SetupUi();
            LoadEnrollmentData();
        }

        private void SetupUi()
        {
            this.Text = "Enrollment Report";

            // Title
            Label title = new Label();
            title.Text = "ENROLLMENT DIRECTORY REPORT";
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
            dgvEnrollments = new DataGridView();
            dgvEnrollments.Location = new Point(30, 110);
            dgvEnrollments.Size = new Size(915, 450);
            dgvEnrollments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEnrollments.AllowUserToAddRows = false;
            dgvEnrollments.ReadOnly = true;
            dgvEnrollments.RowHeadersVisible = false;
            dgvEnrollments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEnrollments.EnableHeadersVisualStyles = false;
            dgvEnrollments.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvEnrollments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEnrollments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvEnrollments.ColumnHeadersHeight = 35;
            dgvEnrollments.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvEnrollments.RowTemplate.Height = 28;
            dgvEnrollments.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvEnrollments.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.Controls.Add(dgvEnrollments);

            // Back button
            Button back = new Button();
            back.Text = "Back to Report Hub";
            back.Size = new Size(180, 42);
            back.Location = new Point(30, 575);
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

        private void LoadEnrollmentData()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT 
                                        StudentID AS [Student ID], 
                                        StudentName AS [Student Name], 
                                        Course AS [Course], 
                                        AcademicYear AS [Academic Year], 
                                        Semester AS [Semester], 
                                        EnrollmentDate AS [Date], 
                                        Status AS [Status] 
                                     FROM Enrollment 
                                     ORDER BY StudentID";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        _enrollmentDt = new DataTable();
                        da.Fill(_enrollmentDt);
                        dgvEnrollments.DataSource = _enrollmentDt;
                        UpdateSummary();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading enrollments: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary()
        {
            if (_enrollmentDt != null)
            {
                int count = _enrollmentDt.DefaultView.Count;
                lblTotalStats.Text = $"Showing {count} Enrollment(s)";
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_enrollmentDt == null) return;

            string search = txtSearch.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(search))
            {
                _enrollmentDt.DefaultView.RowFilter = "";
            }
            else
            {
                _enrollmentDt.DefaultView.RowFilter = string.Format(
                    "Convert([Student ID], 'System.String') LIKE '%{0}%' OR [Student Name] LIKE '%{0}%' OR [Course] LIKE '%{0}%' OR [Academic Year] LIKE '%{0}%' OR [Semester] LIKE '%{0}%' OR [Status] LIKE '%{0}%'",
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