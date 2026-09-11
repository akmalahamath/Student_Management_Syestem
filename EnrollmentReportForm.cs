using System;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class EnrollmentReportForm : Form
    {
        public EnrollmentReportForm()
        {
            InitializeComponent();

            // Form settings
            this.BackColor = Color.AliceBlue;
            this.Text = "Enrollment Report";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            Label title = new Label();
            title.Text = "ENROLLMENT REPORT";
            title.Font = new Font("Arial", 20, FontStyle.Bold);
            title.ForeColor = Color.DarkBlue;
            title.AutoSize = false;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(20, 20);
            title.Size = new Size(760, 50);

            this.Controls.Add(title);

            // DataGridView
            DataGridView dgvEnrollments = new DataGridView();

            dgvEnrollments.Location = new Point(20, 90);
            dgvEnrollments.Size = new Size(760, 310);

            // Columns
            dgvEnrollments.ColumnCount = 5;

            dgvEnrollments.Columns[0].Name = "Enrollment ID";
            dgvEnrollments.Columns[1].Name = "Student ID";
            dgvEnrollments.Columns[2].Name = "Student Name";
            dgvEnrollments.Columns[3].Name = "Course";
            dgvEnrollments.Columns[4].Name = "Date";

            // Sample data
            dgvEnrollments.Rows.Add(
                "E001", "S001", "John", "Computer Science", "2026-09-01");

            dgvEnrollments.Rows.Add(
                "E002", "S002", "Anna", "Information Technology", "2026-09-02");

            dgvEnrollments.Rows.Add(
                "E003", "S003", "David", "Software Engineering", "2026-09-03");

            // Table settings
            dgvEnrollments.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvEnrollments.AllowUserToAddRows = false;
            dgvEnrollments.ReadOnly = true;
            dgvEnrollments.RowHeadersVisible = false;
            dgvEnrollments.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            // Table header style
            dgvEnrollments.EnableHeadersVisualStyles = false;
            dgvEnrollments.ColumnHeadersDefaultCellStyle.BackColor =
                Color.SteelBlue;
            dgvEnrollments.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;
            dgvEnrollments.ColumnHeadersDefaultCellStyle.Font =
                new Font("Arial", 10, FontStyle.Bold);

            dgvEnrollments.ColumnHeadersHeight = 35;

            // Table row style
            dgvEnrollments.DefaultCellStyle.Font =
                new Font("Arial", 10);

            dgvEnrollments.RowTemplate.Height = 30;

            dgvEnrollments.DefaultCellStyle.SelectionBackColor =
                Color.LightSteelBlue;
            dgvEnrollments.DefaultCellStyle.SelectionForeColor =
                Color.Black;

            this.Controls.Add(dgvEnrollments);

            // Back button
            Button back = new Button();

            back.Text = "Back";
            back.Size = new Size(120, 40);
            back.Location = new Point(20, 415);

            back.BackColor = Color.SteelBlue;
            back.ForeColor = Color.White;
            back.Font = new Font("Arial", 10, FontStyle.Bold);
            back.FlatStyle = FlatStyle.Flat;
            back.FlatAppearance.BorderSize = 0;

            // Back to Report Hub
            back.Click += (s, e) =>
            {
                ReportHubForm report = new ReportHubForm();
                report.Show();
                this.Close();
            };

            this.Controls.Add(back);
        }

        private void EnrollmentReportForm_Load(object sender, EventArgs e)
        {

        }
    }
}