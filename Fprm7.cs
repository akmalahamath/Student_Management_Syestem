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
    public partial class Fprm7 : Form
    {
        private DataTable _coursesTable;

        public Fprm7()
        {
            InitializeComponent();
            InitCoursesTable();
        }

        private void InitCoursesTable()
        {
            _coursesTable = new DataTable();
            _coursesTable.Columns.Add("CourseID", typeof(string));
            _coursesTable.Columns.Add("CourseName", typeof(string));
            _coursesTable.Columns.Add("CourseCode", typeof(string));
            _coursesTable.Columns.Add("Duration", typeof(string));
            _coursesTable.Columns.Add("Instructor", typeof(string));
            _coursesTable.Columns.Add("Description", typeof(string));

            _coursesTable.Rows.Add("C001", "Computer Science", "CS101", "4 Years", "Mr. Silva", "Foundations of computing");
            _coursesTable.Rows.Add("C002", "Information Technology", "IT101", "4 Years", "Ms. Perera", "Applied IT and networking");
            _coursesTable.Rows.Add("C003", "Software Engineering", "SE101", "4 Years", "Mr. Fernando", "Enterprise software development");
        }

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

            _coursesTable.Rows.Add(courseId, courseName, courseCode, duration, instructor, description);
            MessageBox.Show("Course added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearFields();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
