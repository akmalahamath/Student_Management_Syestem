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
        private Button btnPaymentReport;
        private Button btnBack;

        public ReportHubForm()
        {
            InitializeComponent();
            CreateReportButtons();
        }

        private void CreateReportButtons()
        {
            this.Controls.Clear();

            // Form title label
            Label title = new Label();
            title.Text = "REPORT MANAGEMENT HUB";
            title.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            title.ForeColor = Color.DarkGreen;
            title.AutoSize = false;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(50, 30);
            title.Size = new Size(875, 50);
            this.Controls.Add(title);

            int btnWidth = 340;
            int btnHeight = 55;
            int startX = (this.ClientSize.Width - btnWidth) / 2;
            int startY = 120;
            int gap = 20;

            // =========================
            // Student Report Button
            // =========================
            btnStudentReport = CreateHubButton("Student Report", startX, startY, btnWidth, btnHeight);
            btnStudentReport.Click += BtnStudentReport_Click;
            this.Controls.Add(btnStudentReport);

            // =========================
            // Course Report Button
            // =========================
            btnCourseReport = CreateHubButton("Course Report", startX, startY + (btnHeight + gap) * 1, btnWidth, btnHeight);
            btnCourseReport.Click += BtnCourseReport_Click;
            this.Controls.Add(btnCourseReport);

            // =========================
            // Enrollment Report Button
            // =========================
            btnEnrollmentReport = CreateHubButton("Enrollment Report", startX, startY + (btnHeight + gap) * 2, btnWidth, btnHeight);
            btnEnrollmentReport.Click += BtnEnrollmentReport_Click;
            this.Controls.Add(btnEnrollmentReport);

            // =========================
            // Attendance Report Button
            // =========================
            btnAttendanceReport = CreateHubButton("Attendance Report", startX, startY + (btnHeight + gap) * 3, btnWidth, btnHeight);
            btnAttendanceReport.Click += BtnAttendanceReport_Click;
            this.Controls.Add(btnAttendanceReport);

            // =========================
            // Payment Report Button
            // =========================
            btnPaymentReport = CreateHubButton("Payment Report", startX, startY + (btnHeight + gap) * 4, btnWidth, btnHeight);
            btnPaymentReport.Click += BtnPaymentReport_Click;
            this.Controls.Add(btnPaymentReport);

            // =========================
            // Back Button
            // =========================
            btnBack = new Button();
            btnBack.Text = "Back to Dashboard";
            btnBack.Size = new Size(200, 48);
            btnBack.Location = new Point(50, 520);
            btnBack.BackColor = Color.DarkSlateGray;
            btnBack.ForeColor = Color.White;
            btnBack.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Cursor = Cursors.Hand;
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);
        }

        private Button CreateHubButton(string text, int x, int y, int width, int height)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(width, height);
            btn.Location = new Point(x, y);
            btn.BackColor = Color.White;
            btn.ForeColor = Color.Navy;
            btn.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.Navy;
            btn.FlatAppearance.BorderSize = 2;
            btn.Cursor = Cursors.Hand;
            return btn;
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

        private void BtnPaymentReport_Click(object sender, EventArgs e)
        {
            PaymentReportForm paymentReport = new PaymentReportForm();
            paymentReport.Show();
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