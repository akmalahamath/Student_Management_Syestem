using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class Form5 : Form
    {
        private DataTable _enrollmentDt;

        public Form5()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += Form5_FormClosing;
            this.textBox1.Leave += TextBox1_Leave;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadEnrollments();
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            string idStr = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(idStr)) return;

            int studentId;
            if (!int.TryParse(idStr, out studentId)) return;

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
                            textBox2.Text = result.ToString();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void LoadEnrollments()
        {
            try
            {
                using (SqlConnection connection = DbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT StudentID, StudentName, Course, AcademicYear, Semester, EnrollmentDate, Status FROM Enrollment";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        _enrollmentDt = new DataTable();
                        adapter.Fill(_enrollmentDt);
                        dgvEnrollment.DataSource = _enrollmentDt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading enrollments: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ADD ENROLLMENT
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter Student ID, Student Name, and Course.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentId;
            if (!int.TryParse(textBox1.Text.Trim(), out studentId))
            {
                MessageBox.Show("Student ID must be a numeric value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Please select a value for Academic Year, Semester, and Status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DbHelper.GetConnection())
                {
                    connection.Open();

                    // Check if student already enrolled in same course
                    string checkSql = "SELECT COUNT(*) FROM Enrollment WHERE StudentID = @sid AND Course = @crs";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@sid", studentId);
                        checkCmd.Parameters.AddWithValue("@crs", textBox3.Text.Trim());
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (exists > 0)
                        {
                            MessageBox.Show("Student " + studentId + " is already enrolled in " + textBox3.Text.Trim() + "!", "Duplicate Enrollment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = "INSERT INTO Enrollment (StudentID, StudentName, Course, AcademicYear, Semester, EnrollmentDate, Status) " +
                                   "VALUES (@StudentID, @StudentName, @Course, @AcademicYear, @Semester, @EnrollmentDate, @Status)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentId);
                        command.Parameters.AddWithValue("@StudentName", textBox2.Text.Trim());
                        command.Parameters.AddWithValue("@Course", textBox3.Text.Trim());
                        command.Parameters.AddWithValue("@AcademicYear", comboBox1.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Semester", comboBox2.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@EnrollmentDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Status", comboBox3.SelectedItem.ToString());

                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Student enrolled successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button3_Click(sender, e);
                LoadEnrollments();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // UPDATE ENROLLMENT
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter Student ID and Course to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentId;
            if (!int.TryParse(textBox1.Text.Trim(), out studentId))
            {
                MessageBox.Show("Student ID must be a numeric value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Please select a value for Academic Year, Semester, and Status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "UPDATE Enrollment SET StudentName=@StudentName, AcademicYear=@AcademicYear, Semester=@Semester, EnrollmentDate=@EnrollmentDate, Status=@Status " +
                                   "WHERE StudentID=@StudentID AND Course=@Course";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentId);
                        command.Parameters.AddWithValue("@StudentName", textBox2.Text.Trim());
                        command.Parameters.AddWithValue("@Course", textBox3.Text.Trim());
                        command.Parameters.AddWithValue("@AcademicYear", comboBox1.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Semester", comboBox2.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@EnrollmentDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Status", comboBox3.SelectedItem.ToString());

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Enrollment record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEnrollments();
                        }
                        else
                        {
                            MessageBox.Show("No enrollment record found for Student ID " + studentId + " in course " + textBox3.Text.Trim(), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DELETE ENROLLMENT
        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please select an enrollment record to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int studentId;
            if (!int.TryParse(textBox1.Text.Trim(), out studentId))
            {
                MessageBox.Show("Student ID must be a numeric value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string course = textBox3.Text.Trim();

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete the enrollment record for Student ID " + studentId + " in course '" + course + "'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using (SqlConnection connection = DbHelper.GetConnection())
                {
                    connection.Open();
                    string query = string.IsNullOrEmpty(course)
                        ? "DELETE FROM Enrollment WHERE StudentID = @StudentID"
                        : "DELETE FROM Enrollment WHERE StudentID = @StudentID AND Course = @Course";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", studentId);
                        if (!string.IsNullOrEmpty(course))
                        {
                            command.Parameters.AddWithValue("@Course", course);
                        }

                        int rows = command.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Enrollment record deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button3_Click(sender, e);
                            LoadEnrollments();
                        }
                        else
                        {
                            MessageBox.Show("No enrollment record found to delete.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
        }

        // BACK
        private void button4_Click(object sender, EventArgs e)
        {
            ReturnToDashboard();
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            ReturnToDashboard();
        }

        private void dgvEnrollment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvEnrollment.Rows.Count)
            {
                DataGridViewRow row = dgvEnrollment.Rows[e.RowIndex];
                if (row.Cells["StudentID"]?.Value != null) textBox1.Text = row.Cells["StudentID"].Value.ToString();
                if (row.Cells["StudentName"]?.Value != null) textBox2.Text = row.Cells["StudentName"].Value.ToString();
                if (row.Cells["Course"]?.Value != null) textBox3.Text = row.Cells["Course"].Value.ToString();

                string acYear = row.Cells["AcademicYear"]?.Value?.ToString();
                if (!string.IsNullOrEmpty(acYear)) comboBox1.SelectedItem = acYear;

                string sem = row.Cells["Semester"]?.Value?.ToString();
                if (!string.IsNullOrEmpty(sem)) comboBox2.SelectedItem = sem;

                string dateStr = row.Cells["EnrollmentDate"]?.Value?.ToString();
                if (DateTime.TryParse(dateStr, out DateTime dt)) dateTimePicker1.Value = dt;

                string stat = row.Cells["Status"]?.Value?.ToString();
                if (!string.IsNullOrEmpty(stat)) comboBox3.SelectedItem = stat;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_enrollmentDt != null)
                {
                    string filter = txtSearch.Text.Trim().Replace("'", "''");
                    if (string.IsNullOrEmpty(filter))
                    {
                        _enrollmentDt.DefaultView.RowFilter = "";
                    }
                    else
                    {
                        _enrollmentDt.DefaultView.RowFilter = string.Format(
                            "StudentName LIKE '%{0}%' OR Course LIKE '%{0}%' OR Convert(StudentID, 'System.String') LIKE '%{0}%' OR Status LIKE '%{0}%'",
                            filter);
                    }
                }
            }
            catch
            {
            }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
    }
}
