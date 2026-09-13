using System;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class AttendanceReportForm : Form
    {
        public AttendanceReportForm()
        {
            InitializeComponent();

            // Title
            Label title = new Label();
            title.Text = "ATTENDANCE REPORT";
            title.Font = new Font("Arial", 20, FontStyle.Bold);
            title.ForeColor = Color.DarkBlue;
            title.AutoSize = false;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(20, 15);
            title.Size = new Size(760, 45);
            this.Controls.Add(title);

            // DataGridView
            DataGridView dgv = new DataGridView();
            dgv.Location = new Point(20, 75);
            dgv.Size = new Size(760, 330);
            dgv.ColumnCount = 6;
            dgv.Columns[0].Name = "Attendance ID";
            dgv.Columns[1].Name = "Student ID";
            dgv.Columns[2].Name = "Student Name";
            dgv.Columns[3].Name = "Course";
            dgv.Columns[4].Name = "Date";
            dgv.Columns[5].Name = "Status";

            dgv.Rows.Add("A001", "1001", "Kamal Perera", "Software Engineering", DateTime.Today.ToString("yyyy-MM-dd"), "Present");
            dgv.Rows.Add("A002", "1002", "Nimal Silva", "Computer Science", DateTime.Today.ToString("yyyy-MM-dd"), "Absent");
            dgv.Rows.Add("A003", "1003", "Saman Kumara", "Information Technology", DateTime.Today.ToString("yyyy-MM-dd"), "Late");
            dgv.Rows.Add("A004", "1004", "Anura Fernando", "Software Engineering", DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"), "Present");
            dgv.Rows.Add("A005", "1005", "Dilani Jayasinghe", "Business Management", DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"), "Present");

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 35;

            dgv.DefaultCellStyle.Font = new Font("Arial", 10);
            dgv.RowTemplate.Height = 28;
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            this.Controls.Add(dgv);

            // Back button
            Button back = new Button();
            back.Text = "Back";
            back.Size = new Size(120, 40);
            back.Location = new Point(20, 420);
            back.BackColor = Color.SteelBlue;
            back.ForeColor = Color.White;
            back.Font = new Font("Arial", 10, FontStyle.Bold);
            back.FlatStyle = FlatStyle.Flat;
            back.FlatAppearance.BorderSize = 0;
            back.Click += (s, e) =>
            {
                ReportHubForm report = new ReportHubForm();
                report.Show();
                this.Close();
            };
            this.Controls.Add(back);
        }
    }
}
