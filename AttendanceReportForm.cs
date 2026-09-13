using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class AttendanceReportForm : Form
    {
        private DataGridView dgvAttendance;
        private TextBox txtSearch;
        private Label lblTotalStats;
        private DataTable _attendanceDt;

        public AttendanceReportForm()
        {
            InitializeComponent();
            SetupUi();
            LoadAttendanceData();
        }

        private void SetupUi()
        {
            this.Text = "Attendance Report";

            // Title
            Label title = new Label();
            title.Text = "ATTENDANCE ACTIVITY REPORT";
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
            lblTotalStats.Location = new Point(420, 70);
            lblTotalStats.Size = new Size(525, 28);
            this.Controls.Add(lblTotalStats);

            // DataGridView
            dgvAttendance = new DataGridView();
            dgvAttendance.Location = new Point(30, 110);
            dgvAttendance.Size = new Size(915, 450);
            dgvAttendance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAttendance.AllowUserToAddRows = false;
            dgvAttendance.ReadOnly = true;
            dgvAttendance.RowHeadersVisible = false;
            dgvAttendance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAttendance.EnableHeadersVisualStyles = false;
            dgvAttendance.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvAttendance.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAttendance.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvAttendance.ColumnHeadersHeight = 35;
            dgvAttendance.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvAttendance.RowTemplate.Height = 28;
            dgvAttendance.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvAttendance.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.Controls.Add(dgvAttendance);

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

        private void LoadAttendanceData()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT 
                                        AttendanceID AS [ID], 
                                        StudentID AS [Student ID], 
                                        StudentName AS [Student Name], 
                                        Course AS [Course], 
                                        Date AS [Date], 
                                        Session AS [Session], 
                                        Status AS [Status], 
                                        Remarks AS [Remarks] 
                                     FROM Attendance 
                                     ORDER BY AttendanceID DESC";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        _attendanceDt = new DataTable();
                        da.Fill(_attendanceDt);
                        dgvAttendance.DataSource = _attendanceDt;
                        UpdateSummary();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading attendance records: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary()
        {
            if (_attendanceDt != null)
            {
                int total = _attendanceDt.DefaultView.Count;
                int present = 0;
                int absent = 0;

                foreach (DataRowView drv in _attendanceDt.DefaultView)
                {
                    string status = drv["Status"]?.ToString() ?? "";
                    if (string.Equals(status, "Present", StringComparison.OrdinalIgnoreCase)) present++;
                    else if (string.Equals(status, "Absent", StringComparison.OrdinalIgnoreCase)) absent++;
                }

                lblTotalStats.Text = $"Records: {total} | Present: {present} | Absent: {absent}";
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_attendanceDt == null) return;

            string search = txtSearch.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(search))
            {
                _attendanceDt.DefaultView.RowFilter = "";
            }
            else
            {
                _attendanceDt.DefaultView.RowFilter = string.Format(
                    "Convert([ID], 'System.String') LIKE '%{0}%' OR [Student ID] LIKE '%{0}%' OR [Student Name] LIKE '%{0}%' OR [Course] LIKE '%{0}%' OR [Status] LIKE '%{0}%' OR [Session] LIKE '%{0}%' OR [Remarks] LIKE '%{0}%'",
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
