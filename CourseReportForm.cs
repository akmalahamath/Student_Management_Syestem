using System;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class CourseReportForm : Form
    {
        public CourseReportForm()
        {
            InitializeComponent();

            // =========================
            // FORM SETTINGS
            // =========================
            this.BackColor = Color.AliceBlue;
            this.Text = "Course Report";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;


            // =========================
            // TITLE
            // =========================
            Label title = new Label();

            title.Text = "COURSE REPORT";

            title.Font =
                new Font("Arial", 20, FontStyle.Bold);

            // Title color
            title.ForeColor =
                Color.DarkBlue;

            title.AutoSize = false;

            title.TextAlign =
                ContentAlignment.MiddleCenter;

            title.Location =
                new Point(20, 20);

            title.Size =
                new Size(760, 50);

            this.Controls.Add(title);


            // =========================
            // COURSE TABLE
            // =========================
            DataGridView dgvCourses =
                new DataGridView();

            dgvCourses.Location =
                new Point(20, 90);

            dgvCourses.Size =
                new Size(760, 310);

            dgvCourses.ColumnCount = 4;

            dgvCourses.Columns[0].Name = "Course ID";
            dgvCourses.Columns[1].Name = "Course Name";
            dgvCourses.Columns[2].Name = "Duration";
            dgvCourses.Columns[3].Name = "Instructor";


            // Course data
            dgvCourses.Rows.Add(
                "C001",
                "Computer Science",
                "4 Years",
                "Mr. Silva");

            dgvCourses.Rows.Add(
                "C002",
                "Information Technology",
                "4 Years",
                "Ms. Perera");

            dgvCourses.Rows.Add(
                "C003",
                "Software Engineering",
                "4 Years",
                "Mr. Fernando");


            // =========================
            // TABLE SETTINGS
            // =========================
            dgvCourses.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvCourses.AllowUserToAddRows = false;

            dgvCourses.ReadOnly = true;

            dgvCourses.RowHeadersVisible = false;

            dgvCourses.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;


            // =========================
            // TABLE HEADER COLOR
            // =========================
            dgvCourses.ColumnHeadersDefaultCellStyle.BackColor =
                Color.SteelBlue;

            dgvCourses.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvCourses.ColumnHeadersDefaultCellStyle.Font =
                new Font("Arial", 10, FontStyle.Bold);

            dgvCourses.ColumnHeadersHeight = 35;

            dgvCourses.EnableHeadersVisualStyles = false;


            // =========================
            // TABLE ROW STYLE
            // =========================
            dgvCourses.DefaultCellStyle.Font =
                new Font("Arial", 10);

            dgvCourses.RowTemplate.Height = 30;

            dgvCourses.DefaultCellStyle.SelectionBackColor =
                Color.LightSteelBlue;

            dgvCourses.DefaultCellStyle.SelectionForeColor =
                Color.Black;


            this.Controls.Add(dgvCourses);


            // =========================
            // BACK BUTTON
            // =========================
            Button back = new Button();

            back.Text = "Back";

            back.Size =
                new Size(120, 40);

            // Moved higher
            back.Location =
                new Point(20, 415);

            back.BackColor =
                Color.SteelBlue;

            back.ForeColor =
                Color.White;

            back.Font =
                new Font("Arial", 10, FontStyle.Bold);

            back.FlatStyle =
                FlatStyle.Flat;

            back.FlatAppearance.BorderSize = 0;


            // Back button
            back.Click += (s, e) =>
            {
                ReportHubForm report =
                    new ReportHubForm();

                report.Show();

                this.Close();
            };

            this.Controls.Add(back);
        }


        private void CourseReportForm_Load(
            object sender, EventArgs e)
        {

        }
    }
}