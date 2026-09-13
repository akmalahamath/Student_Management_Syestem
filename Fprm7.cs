using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class Fprm7 : Form
    {
        private DataTable _coursesDt;

        public Fprm7()
        {
            InitializeComponent();
            this.Load += Fprm7_Load;
        }

        private void Fprm7_Load(object sender, EventArgs e)
        {
            LoadCourses();
        }

        private void LoadCourses()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT CourseID, CourseName, CourseCode, Duration, Instructor, Description FROM Course";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        _coursesDt = new DataTable();
                        adapter.Fill(_coursesDt);
                        dgvCourses.DataSource = _coursesDt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading courses: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ADD COURSE
        private void button1_Click(object sender, EventArgs e)
        {
            string courseId = maskedTextBox1.Text.Trim();
            string courseName = textBox1.Text.Trim();
            string courseCode = textBox2.Text.Trim();
            string duration = textBox3.Text.Trim();
            string instructor = textBox4.Text.Trim();
            string description = textBox5.Text.Trim();

            if (string.IsNullOrEmpty(courseId))
            {
                MessageBox.Show("Please enter a Course ID.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBox1.Focus();
                return;
            }

            if (string.IsNullOrEmpty(courseName))
            {
                MessageBox.Show("Please enter a Course Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();

                    // Check if Course ID already exists
                    string checkSql = "SELECT COUNT(*) FROM Course WHERE CourseID = @id";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", courseId);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Course ID '" + courseId + "' already exists. Please use UPDATE to modify it.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string insertSql = "INSERT INTO Course (CourseID, CourseName, CourseCode, Duration, Instructor, Description) " +
                                       "VALUES (@id, @name, @code, @dur, @inst, @desc)";
                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", courseId);
                        cmd.Parameters.AddWithValue("@name", courseName);
                        cmd.Parameters.AddWithValue("@code", courseCode);
                        cmd.Parameters.AddWithValue("@dur", duration);
                        cmd.Parameters.AddWithValue("@inst", instructor);
                        cmd.Parameters.AddWithValue("@desc", description);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Course added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadCourses();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // UPDATE COURSE
        private void button4_Click(object sender, EventArgs e)
        {
            string courseId = maskedTextBox1.Text.Trim();
            string courseName = textBox1.Text.Trim();
            string courseCode = textBox2.Text.Trim();
            string duration = textBox3.Text.Trim();
            string instructor = textBox4.Text.Trim();
            string description = textBox5.Text.Trim();

            if (string.IsNullOrEmpty(courseId))
            {
                MessageBox.Show("Please enter or select a Course ID to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBox1.Focus();
                return;
            }

            if (string.IsNullOrEmpty(courseName))
            {
                MessageBox.Show("Please enter a Course Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string updateSql = "UPDATE Course SET CourseName=@name, CourseCode=@code, Duration=@dur, Instructor=@inst, Description=@desc " +
                                       "WHERE CourseID=@id";
                    using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", courseId);
                        cmd.Parameters.AddWithValue("@name", courseName);
                        cmd.Parameters.AddWithValue("@code", courseCode);
                        cmd.Parameters.AddWithValue("@dur", duration);
                        cmd.Parameters.AddWithValue("@inst", instructor);
                        cmd.Parameters.AddWithValue("@desc", description);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Course updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadCourses();
                        }
                        else
                        {
                            MessageBox.Show("No course found with ID: " + courseId, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DELETE COURSE
        private void button5_Click(object sender, EventArgs e)
        {
            string courseId = maskedTextBox1.Text.Trim();

            if (string.IsNullOrEmpty(courseId))
            {
                MessageBox.Show("Please select or enter a Course ID to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBox1.Focus();
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete course '" + courseId + "'?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string deleteSql = "DELETE FROM Course WHERE CourseID = @id";
                    using (SqlCommand cmd = new SqlCommand(deleteSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", courseId);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Course deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadCourses();
                        }
                        else
                        {
                            MessageBox.Show("No course found with ID: " + courseId, "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void button2_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // BACK
        private void button3_Click(object sender, EventArgs e)
        {
            ReturnToDashboard();
        }

        private void Fprm7_FormClosing(object sender, FormClosingEventArgs e)
        {
            ReturnToDashboard();
        }

        private void ClearFields()
        {
            maskedTextBox1.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            maskedTextBox1.Focus();
        }

        private void dgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvCourses.Rows.Count)
            {
                DataGridViewRow row = dgvCourses.Rows[e.RowIndex];
                if (row.Cells["CourseID"]?.Value != null) maskedTextBox1.Text = row.Cells["CourseID"].Value.ToString();
                if (row.Cells["CourseName"]?.Value != null) textBox1.Text = row.Cells["CourseName"].Value.ToString();
                if (row.Cells["CourseCode"]?.Value != null) textBox2.Text = row.Cells["CourseCode"].Value.ToString();
                if (row.Cells["Duration"]?.Value != null) textBox3.Text = row.Cells["Duration"].Value.ToString();
                if (row.Cells["Instructor"]?.Value != null) textBox4.Text = row.Cells["Instructor"].Value.ToString();
                if (row.Cells["Description"]?.Value != null) textBox5.Text = row.Cells["Description"].Value.ToString();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (_coursesDt != null)
                {
                    string filter = txtSearch.Text.Trim().Replace("'", "''");
                    if (string.IsNullOrEmpty(filter))
                    {
                        _coursesDt.DefaultView.RowFilter = "";
                    }
                    else
                    {
                        _coursesDt.DefaultView.RowFilter = string.Format(
                            "CourseID LIKE '%{0}%' OR CourseName LIKE '%{0}%' OR CourseCode LIKE '%{0}%' OR Instructor LIKE '%{0}%'",
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

        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
    }
}
