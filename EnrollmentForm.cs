using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class EnrollmentForm : Form

    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True; Connect Timeout=30";

        public EnrollmentForm()
        {
            InitializeComponent();
            this.Load += EnrollmentForm_Load;
        }

        private void EnrollmentForm_Load(object sender, EventArgs e)
        {
            LoadStudentsDropdown();
            LoadCoursesDropdown();
            LoadStatusDropdown();
            LoadEnrollmentsGrid();
        }

        private void LoadStatusDropdown() { cmbStatus.Items.Add("Enrolled"); cmbStatus.Items.Add("Completed"); cmbStatus.SelectedIndex = -1; }

        private void LoadStudentsDropdown()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Studentid, Fullname FROM Student";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbStudent.DataSource = dt;
                cmbStudent.DisplayMember = "Fullname";
                cmbStudent.ValueMember = "Studentid";
                cmbStudent.SelectedIndex = -1;
            }
        }

        private void LoadCoursesDropdown()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CourseID, CourseName, Capacity FROM Course";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbCourse.DataSource = dt;
                cmbCourse.DisplayMember = "CourseName";
                cmbCourse.ValueMember = "CourseName";
                cmbCourse.SelectedIndex = -1;
            }
        }

        private void LoadEnrollmentsGrid()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Enrollment";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvEnrollments.DataSource = dt;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

              
           private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbStudent.Text) || string.IsNullOrWhiteSpace(cmbCourse.Text) || string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                MessageBox.Show("Please select a student, course, and status.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Enrollment (StudentID, StudentName, Course, AcademicYear, Semester, EnrollmentDate, Status) " +
                               "VALUES (@StudentID, @StudentName, @Course, @AcademicYear, @Semester, @EnrollmentDate, @Status)";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@StudentID", cmbStudent.SelectedValue);
                cmd.Parameters.AddWithValue("@StudentName", cmbStudent.Text);
                cmd.Parameters.AddWithValue("@Course", cmbCourse.Text);
                cmd.Parameters.AddWithValue("@AcademicYear", DateTime.Now.Year.ToString());
                cmd.Parameters.AddWithValue("@Semester", "1");
                cmd.Parameters.AddWithValue("@EnrollmentDate", dtpEnrollDate.Value);
                cmd.Parameters.AddWithValue("@Status", cmbStatus.Text);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Enrollment added successfully!");
            LoadEnrollmentsGrid();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvEnrollments.CurrentRow == null)
            {
                MessageBox.Show("Please select a row to update.");
                return;
            }

            int enrollmentId = Convert.ToInt32(dgvEnrollments.CurrentRow.Cells["EnrollmentID"].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Enrollment SET StudentID=@StudentID, StudentName=@StudentName, Course=@Course, AcademicYear=@AcademicYear, Semester=@Semester, EnrollmentDate=@EnrollmentDate, Status=@Status WHERE EnrollmentID=@EnrollmentID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", cmbStudent.SelectedValue);
                cmd.Parameters.AddWithValue("@StudentName", cmbStudent.Text);
                cmd.Parameters.AddWithValue("@Course", cmbCourse.Text);
                cmd.Parameters.AddWithValue("@AcademicYear", DateTime.Now.Year.ToString());
                cmd.Parameters.AddWithValue("@Semester", "1");
                cmd.Parameters.AddWithValue("@EnrollmentDate", dtpEnrollDate.Value);
                cmd.Parameters.AddWithValue("@Status", cmbStatus.Text);
                cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Enrollment updated successfully!");
            LoadEnrollmentsGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvEnrollments.CurrentRow == null)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            int enrollmentId = Convert.ToInt32(dgvEnrollments.CurrentRow.Cells["EnrollmentID"].Value);

            if (MessageBox.Show("Delete this enrollment?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Enrollment WHERE EnrollmentID=@EnrollmentID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Enrollment deleted successfully!");
            LoadEnrollmentsGrid();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbStudent.SelectedIndex = -1;
            cmbCourse.Text = "";
            dtpEnrollDate.Value = DateTime.Now;
            cmbStatus.SelectedIndex = -1;
            dgvEnrollments.ClearSelection();
        }

        private void dgvEnrollments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    }