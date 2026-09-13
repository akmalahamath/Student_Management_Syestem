using System;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class ReportHubForm : Form
    {
        private Button btnStudentReport;
        private Button btnCourseReport;
        private Button btnEnrollmentReport;
        private Button btnAttendanceReport;
        private Button btnBack;

        public ReportHubForm()
        {
            InitializeComponent();
            CreateReportButtons();
        }

        private void CreateReportButtons()
        {
            // Form title label
            Label title = new Label();
            title.Text = "REPORT MANAGEMENT HUB";
            title.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            title.ForeColor = Color.DarkGreen;
            title.AutoSize = false;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(20, 18);
            title.Size = new Size(660, 40);
            this.Controls.Add(title);

            // =========================
            // Student Report Button
            // =========================
            btnStudentReport = new Button();
            btnStudentReport.Text = "Student Report";
            btnStudentReport.Size = new Size(260, 50);
            btnStudentReport.Location = new Point(220, 80);
            btnStudentReport.BackColor = Color.White;
            btnStudentReport.ForeColor = Color.Black;
            btnStudentReport.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnStudentReport.FlatStyle = FlatStyle.Flat;
            btnStudentReport.Cursor = Cursors.Hand;
            btnStudentReport.Click += BtnStudentReport_Click;
            this.Controls.Add(btnStudentReport);

            // =========================
            // Course Report Button
            // =========================
            btnCourseReport = new Button();
            btnCourseReport.Text = "Course Report";
            btnCourseReport.Size = new Size(260, 50);
            btnCourseReport.Location = new Point(220, 145);
            btnCourseReport.BackColor = Color.White;
            btnCourseReport.ForeColor = Color.Black;
            btnCourseReport.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnCourseReport.FlatStyle = FlatStyle.Flat;
            btnCourseReport.Cursor = Cursors.Hand;
            btnCourseReport.Click += BtnCourseReport_Click;
            this.Controls.Add(btnCourseReport);

            // =========================
            // Enrollment Report Button
            // =========================
            btnEnrollmentReport = new Button();
            btnEnrollmentReport.Text = "Enrollment Report";
            btnEnrollmentReport.Size = new Size(260, 50);
            btnEnrollmentReport.Location = new Point(220, 210);
            btnEnrollmentReport.BackColor = Color.White;
            btnEnrollmentReport.ForeColor = Color.Black;
            btnEnrollmentReport.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnEnrollmentReport.FlatStyle = FlatStyle.Flat;
            btnEnrollmentReport.Cursor = Cursors.Hand;
            btnEnrollmentReport.Click += BtnEnrollmentReport_Click;
            this.Controls.Add(btnEnrollmentReport);

            // =========================
            // Attendance Report Button
            // =========================
            btnAttendanceReport = new Button();
            btnAttendanceReport.Text = "Attendance Report";
            btnAttendanceReport.Size = new Size(260, 50);
            btnAttendanceReport.Location = new Point(220, 275);
            btnAttendanceReport.BackColor = Color.White;
            btnAttendanceReport.ForeColor = Color.Black;
            btnAttendanceReport.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnAttendanceReport.FlatStyle = FlatStyle.Flat;
            btnAttendanceReport.Cursor = Cursors.Hand;
            btnAttendanceReport.Click += BtnAttendanceReport_Click;
            this.Controls.Add(btnAttendanceReport);

            // =========================
            // Back Button
            // =========================
            btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Size = new Size(130, 42);
            btnBack.Location = new Point(25, 405);
            btnBack.BackColor = Color.DarkSlateGray;
            btnBack.ForeColor = Color.White;
            btnBack.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Cursor = Cursors.Hand;
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);
        }

        private void BtnStudentReport_Click(object sender, EventArgs e)
        {
            StudentReportForm studentReport = new StudentReportForm();
            studentReport.Show();
            this.Hide();
        }

        private void BtnCourseReport_Click(object sender, EventArgs e)
        {
            CourseReportForm courseReport = new CourseReportForm();
            courseReport.Show();
            this.Hide();
        }

        private void BtnEnrollmentReport_Click(object sender, EventArgs e)
        {
            EnrollmentReportForm enrollmentReport = new EnrollmentReportForm();
            enrollmentReport.Show();
            this.Hide();
        }

        private void BtnAttendanceReport_Click(object sender, EventArgs e)
        {
            AttendanceReportForm attendanceReport = new AttendanceReportForm();
            attendanceReport.Show();
            this.Hide();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            ReturnToDashboard();
        }

        private void ReportHubForm_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}