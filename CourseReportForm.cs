using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class CourseReportForm : Form
    {
        private DataGridView dgvCourses;
        private TextBox txtSearch;
        private Label lblTotalStats;
        private DataTable _courseDt;

        public CourseReportForm()
        {
            InitializeComponent();
            SetupUi();
            LoadCourseData();
        }

        private void SetupUi()
        {
            this.Text = "Course Report";

            // Title
            Label title = new Label();
            title.Text = "COURSE DIRECTORY REPORT";
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
            dgvCourses = new DataGridView();
            dgvCourses.Location = new Point(30, 110);
            dgvCourses.Size = new Size(915, 450);
            dgvCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCourses.AllowUserToAddRows = false;
            dgvCourses.ReadOnly = true;
            dgvCourses.RowHeadersVisible = false;
            dgvCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCourses.EnableHeadersVisualStyles = false;
            dgvCourses.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvCourses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCourses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvCourses.ColumnHeadersHeight = 35;
            dgvCourses.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvCourses.RowTemplate.Height = 28;
            dgvCourses.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvCourses.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.Controls.Add(dgvCourses);

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

        private void LoadCourseData()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT 
                                        CourseID AS [Course ID], 
                                        CourseName AS [Course Name], 
                                        CourseCode AS [Course Code], 
                                        Duration AS [Duration], 
                                        Instructor AS [Instructor], 
                                        Description AS [Description] 
                                     FROM Course 
                                     ORDER BY CourseID";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        _courseDt = new DataTable();
                        da.Fill(_courseDt);
                        dgvCourses.DataSource = _courseDt;
                        UpdateSummary();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading courses: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary()
        {
            if (_courseDt != null)
            {
                int count = _courseDt.DefaultView.Count;
                lblTotalStats.Text = $"Showing {count} Course(s)";
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_courseDt == null) return;

            string search = txtSearch.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(search))
            {
                _courseDt.DefaultView.RowFilter = "";
            }
            else
            {
                _courseDt.DefaultView.RowFilter = string.Format(
                    "Convert([Course ID], 'System.String') LIKE '%{0}%' OR [Course Name] LIKE '%{0}%' OR [Course Code] LIKE '%{0}%' OR [Instructor] LIKE '%{0}%' OR [Description] LIKE '%{0}%'",
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