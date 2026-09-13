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
        private Button btnBack;

        public ReportHubForm()
        {
            InitializeComponent();

            this.BackColor = Color.LightSteelBlue;

            CreateReportButtons();
        }

        private void CreateReportButtons()
        {
            // Form settings
            this.Text = "Reports";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // =========================
            // Student Report Button
            // =========================
            btnStudentReport = new Button();
            btnStudentReport.Text = "Student Report";
            btnStudentReport.Size = new Size(250, 60);
            btnStudentReport.Location = new Point(220, 80);

            btnStudentReport.BackColor = Color.White;
            btnStudentReport.ForeColor = Color.Black;
            btnStudentReport.Font = new Font("Arial", 11, FontStyle.Bold);
            btnStudentReport.FlatStyle = FlatStyle.Flat;

            btnStudentReport.Click += BtnStudentReport_Click;
            this.Controls.Add(btnStudentReport);


            // =========================
            // Course Report Button
            // =========================
            btnCourseReport = new Button();
            btnCourseReport.Text = "Course Report";
            btnCourseReport.Size = new Size(250, 60);
            btnCourseReport.Location = new Point(220, 160);

            btnCourseReport.BackColor = Color.White;
            btnCourseReport.ForeColor = Color.Black;
            btnCourseReport.Font = new Font("Arial", 11, FontStyle.Bold);
            btnCourseReport.FlatStyle = FlatStyle.Flat;

            btnCourseReport.Click += BtnCourseReport_Click;
            this.Controls.Add(btnCourseReport);


            // =========================
            // Enrollment Report Button
            // =========================
            btnEnrollmentReport = new Button();
            btnEnrollmentReport.Text = "Enrollment Report";
            btnEnrollmentReport.Size = new Size(250, 60);
            btnEnrollmentReport.Location = new Point(220, 240);

            btnEnrollmentReport.BackColor = Color.White;
            btnEnrollmentReport.ForeColor = Color.Black;
            btnEnrollmentReport.Font = new Font("Arial", 11, FontStyle.Bold);
            btnEnrollmentReport.FlatStyle = FlatStyle.Flat;

            btnEnrollmentReport.Click += BtnEnrollmentReport_Click;
            this.Controls.Add(btnEnrollmentReport);


            // =========================
            // Back Button
            // =========================
            btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Size = new Size(120, 45);
            btnBack.Location = new Point(20, 400);

            btnBack.BackColor = Color.White;
            btnBack.ForeColor = Color.Black;
            btnBack.Font = new Font("Arial", 10, FontStyle.Bold);
            btnBack.FlatStyle = FlatStyle.Flat;

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

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Dashboardform dashboard = new Dashboardform();
            dashboard.Show();
            this.Close();
        }
    }
}