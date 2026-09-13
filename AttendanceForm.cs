using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class AttendanceForm : Form
    {
        private int _selectedAttendanceId = -1;
        private DataTable _attendanceTable;

        public AttendanceForm()
        {
            InitializeComponent();
        }

        private void AttendanceForm_Load(object sender, EventArgs e)
        {
            LoadCoursesIntoCombo();
            LoadAttendanceFromDb();

            cmbStatus.SelectedIndex = 0; // Present
            cmbSession.SelectedIndex = 0; // Morning
            dtpDate.Value = DateTime.Today;

            txtStudentID.Leave += TxtStudentID_Leave;
        }

        private void LoadCoursesIntoCombo()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT CourseName FROM Course", conn))
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            cmbCourse.Items.Clear();
                            while (r.Read())
                            {
                                cmbCourse.Items.Add(r["CourseName"].ToString());
                            }
                            if (cmbCourse.Items.Count > 0) cmbCourse.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void LoadAttendanceFromDb()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT AttendanceID, StudentID, StudentName, Course, Date, Session, Status, Remarks FROM Attendance ORDER BY AttendanceID DESC";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        _attendanceTable = new DataTable();
                        adapter.Fill(_attendanceTable);
                        dgvAttendance.DataSource = _attendanceTable;
                        SetColumnHeaders();
                        UpdateStats();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading attendance: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            if (string.IsNullOrEmpty(id)) return;

            int studentId;
            if (!int.TryParse(id, out studentId)) return;

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT Fullname FROM Student WHERE Studentid = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", studentId);
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
            }
        }

        // MARK (ADD) ATTENDANCE
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

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string insertSql = "INSERT INTO Attendance (StudentID, StudentName, Course, Date, Session, Status, Remarks) " +
                                       "VALUES (@sid, @sname, @crs, @dt, @sess, @stat, @rem)";
                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", studentId);
                        cmd.Parameters.AddWithValue("@sname", studentName);
                        cmd.Parameters.AddWithValue("@crs", course);
                        cmd.Parameters.AddWithValue("@dt", date);
                        cmd.Parameters.AddWithValue("@sess", session);
                        cmd.Parameters.AddWithValue("@stat", status);
                        cmd.Parameters.AddWithValue("@rem", remarks);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Attendance marked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadAttendanceFromDb();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // UPDATE ATTENDANCE
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

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string updateSql = "UPDATE Attendance SET StudentID=@sid, StudentName=@sname, Course=@crs, Date=@dt, Session=@sess, Status=@stat, Remarks=@rem " +
                                       "WHERE AttendanceID=@id";
                    using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _selectedAttendanceId);
                        cmd.Parameters.AddWithValue("@sid", studentId);
                        cmd.Parameters.AddWithValue("@sname", studentName);
                        cmd.Parameters.AddWithValue("@crs", course);
                        cmd.Parameters.AddWithValue("@dt", date);
                        cmd.Parameters.AddWithValue("@sess", session);
                        cmd.Parameters.AddWithValue("@stat", status);
                        cmd.Parameters.AddWithValue("@rem", remarks);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Attendance record updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetForm();
                            LoadAttendanceFromDb();
                        }
                        else
                        {
                            MessageBox.Show("Record not found to update.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DELETE ATTENDANCE
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedAttendanceId <= 0)
            {
                MessageBox.Show("Please select a row from the table to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this attendance record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string deleteSql = "DELETE FROM Attendance WHERE AttendanceID = @id";
                    using (SqlCommand cmd = new SqlCommand(deleteSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _selectedAttendanceId);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Attendance record deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetForm();
                            LoadAttendanceFromDb();
                        }
                        else
                        {
                            MessageBox.Show("Record not found to delete.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // CLEAR
        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadAttendanceFromDb();
        }

        // SEARCH
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtStudentID.Text.Trim();
            if (string.IsNullOrEmpty(query)) query = txtStudentName.Text.Trim();

            if (_attendanceTable == null) return;

            DataView dv = new DataView(_attendanceTable);

            if (!string.IsNullOrEmpty(query))
            {
                dv.RowFilter = string.Format("StudentID LIKE '%{0}%' OR StudentName LIKE '%{0}%' OR Course LIKE '%{0}%'", query.Replace("'", "''"));
            }
            else if (cmbStatus.SelectedItem != null)
            {
                dv.RowFilter = string.Format("Status = '{0}'", cmbStatus.SelectedItem.ToString());
            }

            dgvAttendance.DataSource = dv;

            if (dv.Count == 0)
            {
                MessageBox.Show("No records found matching the search criteria.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // BACK
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
                else cmbCourse.Text = course;

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
            if (_attendanceTable == null) return;

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
