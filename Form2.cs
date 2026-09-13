using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class Dashboardform : Form
    {
        public Dashboardform()
        {
            InitializeComponent();
            this.FormClosing += Dashboardform_FormClosing;
            this.Load += Dashboardform_Load;
            this.Activated += Dashboardform_Activated;
        }

        private void Dashboardform_Load(object sender, EventArgs e)
        {
            RefreshDashboard();
        }

        private void Dashboardform_Activated(object sender, EventArgs e)
        {
            RefreshDashboard();
        }

        public void RefreshDashboard()
        {
            // 1. Enforce Role Permissions
            bool isAdmin = UserSession.IsAdmin;
            btnStudents.Visible = isAdmin;
            btnCourses.Visible = isAdmin;
            btnEnrollment.Visible = isAdmin;
            btnPayments.Visible = isAdmin;
            btnAttendance.Visible = isAdmin;
            btnReports.Visible = true;
            btnDashboard.Visible = true;
            btnReports.Location = isAdmin ? new System.Drawing.Point(15, 408) : new System.Drawing.Point(15, 158);

            label1.Text = "Logged in as: " + UserSession.Role + (isAdmin ? "" : " (" + UserSession.UserName + ")");

            // 2. Load live metrics from Database
            try
            {
                using (System.Data.SqlClient.SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();

                    // Total Students
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT COUNT(*) FROM Student", conn))
                    {
                        object res = cmd.ExecuteScalar();
                        lblStudentCount.Text = res != null ? res.ToString() : "0";
                    }

                    // Total Courses
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT COUNT(*) FROM Course", conn))
                    {
                        object res = cmd.ExecuteScalar();
                        lblCourseCount.Text = res != null ? res.ToString() : "0";
                    }

                    // Total Enrollments
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT COUNT(*) FROM Enrollment", conn))
                    {
                        object res = cmd.ExecuteScalar();
                        lblEntrollmentCount.Text = res != null ? res.ToString() : "0";
                    }

                    // Total Payments
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("SELECT COUNT(*) FROM Payment", conn))
                    {
                        object res = cmd.ExecuteScalar();
                        lblPaymentCount.Text = res != null ? res.ToString() : "0";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error refreshing dashboard counts: " + ex.Message);
            }
        }

        private void Dashboardform_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Loginform login = new Loginform();
            login.Show();
            this.Hide();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            ReportHubForm report = new ReportHubForm();
            report.Show();
            this.Hide();
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            Student studentpage = new Student();
            studentpage.Show();
            this.Hide();
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            Fprm7 course = new Fprm7();
            course.Show();
            this.Hide();
        }

        private void btnEnrollment_Click(object sender, EventArgs e)
        {
            Form5 entrollment = new Form5();
            entrollment.Show();
            this.Hide();
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            Form6 payment = new Form6();
            payment.Show();
            this.Hide();
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            AttendanceForm attendance = new AttendanceForm();
            attendance.Show();
            this.Hide();
        }
    }
}