using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class AttendanceForm : Form
    {
        private int _selectedAttendanceId = -1;
        private DataTable _attendanceTable;
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True; Connect Timeout=30";

        public AttendanceForm()
        {
            InitializeComponent();
        }

        private void AttendanceForm_Load(object sender, EventArgs e)
        {
            InitTable();
            cmbStatus.SelectedIndex = 0; // Present
            cmbSession.SelectedIndex = 0; // Morning
            if (cmbCourse.Items.Count > 0) cmbCourse.SelectedIndex = 0;
            dtpDate.Value = DateTime.Today;

            txtStudentID.Leave += TxtStudentID_Leave;
            UpdateStats();
        }

        private void InitTable()
        {
            _attendanceTable = new DataTable();
            _attendanceTable.Columns.Add("AttendanceID", typeof(int));
            _attendanceTable.Columns.Add("StudentID", typeof(string));
            _attendanceTable.Columns.Add("StudentName", typeof(string));
            _attendanceTable.Columns.Add("Course", typeof(string));
            _attendanceTable.Columns.Add("Date", typeof(string));
            _attendanceTable.Columns.Add("Session", typeof(string));
            _attendanceTable.Columns.Add("Status", typeof(string));
            _attendanceTable.Columns.Add("Remarks", typeof(string));

            // Populate initial realistic records
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            string yesterday = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");

            _attendanceTable.Rows.Add(1, "1001", "Kamal Perera", "Software Engineering", today, "Morning", "Present", "On time");
            _attendanceTable.Rows.Add(2, "1002", "Nimal Silva", "Computer Science", today, "Morning", "Absent", "Medical leave");
            _attendanceTable.Rows.Add(3, "1003", "Saman Kumara", "Information Technology", today, "Morning", "Late", "Traffic delay");
            _attendanceTable.Rows.Add(4, "1004", "Anura Fernando", "Software Engineering", yesterday, "Morning", "Present", "Regular");
            _attendanceTable.Rows.Add(5, "1005", "Dilani Jayasinghe", "Business Management", yesterday, "Afternoon", "Present", "Regular");

            dgvAttendance.DataSource = _attendanceTable;
            SetColumnHeaders();
        }

        private void SetColumnHeaders()
        {
            if (dgvAttendance.Columns["AttendanceID"] != null) dgvAttendance.Columns["AttendanceID"].Visible = false;
            if (dgvAttendance.Columns["StudentID"] != null) dgvAttendance.Columns["StudentID"].HeaderText = "Student ID";
            if (dgvAttendance.Columns["StudentName"] != null) dgvAttendance.Columns["StudentName"].HeaderText = "Student Name";
            if (dgvAttendance.Columns["Course"] != null) dgvAttendance.Columns["Course"].HeaderText = "Course";
            if (dgvAttendance.Columns["Date"] != null) dgvAttendance.Columns["Date"].HeaderText = "Date";
            if (dgvAttendance.Columns["Session"] != null) dgvAttendance.Columns["Session"].HeaderText = "Session";
            if (dgvAttendance.Columns["Status"] != null) dgvAttendance.Columns["Status"].HeaderText = "Status";
            if (dgvAttendance.Columns["Remarks"] != null) dgvAttendance.Columns["Remarks"].HeaderText = "Remarks";
        }

        private void TxtStudentID_Leave(object sender, EventArgs e)
        {
            string id = txtStudentID.Text.Trim();
            if (string.IsNullOrEmpty(id) || !string.IsNullOrEmpty(txtStudentName.Text)) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Fullname FROM Student WHERE Studentid = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            txtStudentName.Text = result.ToString();
                        }
                    }
                }
            }
            catch
            {
                // Ignore database connection issues for lookup
            }
        }

        private void btnMark_Click(object sender, EventArgs e)
        {
            string studentId = txtStudentID.Text.Trim();
            string studentName = txtStudentName.Text.Trim();
            string course = cmbCourse.SelectedItem != null ? cmbCourse.SelectedItem.ToString() : "";
            string date = dtpDate.Value.ToString("yyyy-MM-dd");
            string status = cmbStatus.SelectedItem != null ? cmbStatus.SelectedItem.ToString() : "Present";
            string session = cmbSession.SelectedItem != null ? cmbSession.SelectedItem.ToString() : "Morning";
            string remarks = txtRemarks.Text.Trim();

            if (string.IsNullOrEmpty(studentId))
            {
                MessageBox.Show("Please enter a Student ID.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentID.Focus();
                return;
            }

            if (string.IsNullOrEmpty(studentName))
            {
                MessageBox.Show("Please enter the Student Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(course))
            {
                MessageBox.Show("Please select a Course.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCourse.Focus();
                return;
            }

            int newId = _attendanceTable.Rows.Count + 1;
            _attendanceTable.Rows.Add(newId, studentId, studentName, course, date, session, status, remarks);

            MessageBox.Show("Attendance marked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ResetForm();
            UpdateStats();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedAttendanceId <= 0)
            {
                MessageBox.Show("Please select a row from the table to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string studentId = txtStudentID.Text.Trim();
            string studentName = txtStudentName.Text.Trim();
            string course = cmbCourse.SelectedItem != null ? cmbCourse.SelectedItem.ToString() : "";
            string date = dtpDate.Value.ToString("yyyy-MM-dd");
            string status = cmbStatus.SelectedItem != null ? cmbStatus.SelectedItem.ToString() : "Present";
            string session = cmbSession.SelectedItem != null ? cmbSession.SelectedItem.ToString() : "Morning";
            string remarks = txtRemarks.Text.Trim();

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(studentName))
            {
                MessageBox.Show("Student ID and Name are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRow[] rows = _attendanceTable.Select("AttendanceID = " + _selectedAttendanceId);
            if (rows.Length > 0)
            {
                DataRow row = rows[0];
                row["StudentID"] = studentId;
                row["StudentName"] = studentName;
                row["Course"] = course;
                row["Date"] = date;
                row["Session"] = session;
                row["Status"] = status;
                row["Remarks"] = remarks;
                _attendanceTable.AcceptChanges();

                MessageBox.Show("Attendance record updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                UpdateStats();
            }
            else
            {
                MessageBox.Show("Record not found.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedAttendanceId <= 0)
            {
                MessageBox.Show("Please select a row from the table to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this attendance record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            DataRow[] rows = _attendanceTable.Select("AttendanceID = " + _selectedAttendanceId);
            if (rows.Length > 0)
            {
                rows[0].Delete();
                _attendanceTable.AcceptChanges();
                MessageBox.Show("Attendance record deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                UpdateStats();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            dgvAttendance.DataSource = _attendanceTable;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtStudentID.Text.Trim();
            if (string.IsNullOrEmpty(query)) query = txtStudentName.Text.Trim();

            DataView dv = new DataView(_attendanceTable);

            if (!string.IsNullOrEmpty(query))
            {
                dv.RowFilter = string.Format("StudentID LIKE '%{0}%' OR StudentName LIKE '%{0}%'", query.Replace("'", "''"));
            }
            else if (cmbStatus.SelectedItem != null)
            {
                dv.RowFilter = string.Format("Status = '{0}'", cmbStatus.SelectedItem.ToString());
            }

            dgvAttendance.DataSource = dv;

            if (dgvAttendance.Rows.Count == 0)
            {
                MessageBox.Show("No records found matching the search criteria.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ReturnToDashboard();
        }

        private void AttendanceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ReturnToDashboard();
        }

        private bool _isReturning = false;
        private void ReturnToDashboard()
        {
            if (_isReturning) return;
            _isReturning = true;

            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is Dashboardform)
                {
                    openForm.Show();
                    this.Dispose();
                    return;
                }
            }

            Dashboardform dashboard = new Dashboardform();
            dashboard.Show();
            this.Dispose();
        }

        private void ResetForm()
        {
            _selectedAttendanceId = -1;
            txtStudentID.Clear();
            txtStudentName.Clear();
            txtRemarks.Clear();
            cmbStatus.SelectedIndex = 0;
            cmbSession.SelectedIndex = 0;
            dtpDate.Value = DateTime.Today;
            txtStudentID.Focus();
        }

        private void dgvAttendance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvAttendance.Rows.Count)
            {
                DataGridViewRow row = dgvAttendance.Rows[e.RowIndex];
                if (row == null || row.Cells["AttendanceID"] == null) return;

                if (row.Cells["AttendanceID"].Value != null && row.Cells["AttendanceID"].Value != DBNull.Value)
                    _selectedAttendanceId = Convert.ToInt32(row.Cells["AttendanceID"].Value);

                txtStudentID.Text = row.Cells["StudentID"]?.Value?.ToString() ?? "";
                txtStudentName.Text = row.Cells["StudentName"]?.Value?.ToString() ?? "";

                string course = row.Cells["Course"]?.Value?.ToString() ?? "";
                int courseIdx = cmbCourse.FindStringExact(course);
                if (courseIdx >= 0) cmbCourse.SelectedIndex = courseIdx;

                if (DateTime.TryParse(row.Cells["Date"]?.Value?.ToString(), out DateTime dt))
                    dtpDate.Value = dt;

                string status = row.Cells["Status"]?.Value?.ToString() ?? "Present";
                int statusIdx = cmbStatus.FindStringExact(status);
                if (statusIdx >= 0) cmbStatus.SelectedIndex = statusIdx;

                string session = row.Cells["Session"]?.Value?.ToString() ?? "Morning";
                int sessionIdx = cmbSession.FindStringExact(session);
                if (sessionIdx >= 0) cmbSession.SelectedIndex = sessionIdx;

                txtRemarks.Text = row.Cells["Remarks"]?.Value?.ToString() ?? "";
            }
        }

        private void UpdateStats()
        {
            int total = _attendanceTable.Rows.Count;
            string today = DateTime.Today.ToString("yyyy-MM-dd");
            int presentToday = 0;
            int absentToday = 0;

            foreach (DataRow row in _attendanceTable.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;
                string d = row["Date"]?.ToString() ?? "";
                string s = row["Status"]?.ToString() ?? "";
                if (d == today)
                {
                    if (s.Equals("Present", StringComparison.OrdinalIgnoreCase)) presentToday++;
                    else if (s.Equals("Absent", StringComparison.OrdinalIgnoreCase)) absentToday++;
                }
            }

            lblStats.Text = string.Format("Total: {0}  |  Present Today: {1}  |  Absent Today: {2}", total, presentToday, absentToday);
        }
    }
}
