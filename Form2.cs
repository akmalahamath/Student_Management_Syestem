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
        private string userRole;

        public Dashboardform(string role)
        {
            InitializeComponent();

            userRole = role;

            // Only Admin can see User Management
            btnUserManagement.Visible =
                userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);

            // Show the current user's role
            label1.Text = "Logged in as: " + userRole;
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

        private void btnStudents_Click(object sender, EventArgs e)
        {
            Student studentpage = new Student(userRole);
            studentpage.Show();
            this.Hide();
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            UserManagement userManagement = new UserManagement(userRole);
            userManagement.Show();
            this.Hide();
        }
    }
}