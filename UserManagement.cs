using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class UserManagement : Form
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True; Connect Timeout=30";

        public UserManagement(string role)
        {
            InitializeComponent();

            // Only Admin can access User Management
            if (!role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Access denied. Only Admin users can access User Management.");
                this.Close();
                return;
            }

            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Staff");
            cmbRole.SelectedIndex = 1;
        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT Idnumber, Firstname, Lastname, Email, Faculty, Role, IsActive
                                     FROM Signup";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvUsers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        // ADD USER
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                    string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtIdNumber.Text) ||
                    string.IsNullOrWhiteSpace(txtFaculty.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                if (!int.TryParse(txtIdNumber.Text, out int idNumber))
                {
                    MessageBox.Show("ID Number must be a number.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string checkQuery = @"SELECT COUNT(*)
                                          FROM Signup
                                          WHERE Email = @Email OR Idnumber = @Idnumber";

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        checkCommand.Parameters.AddWithValue("@Idnumber", idNumber);

                        int existingUser = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (existingUser > 0)
                        {
                            MessageBox.Show("A user with this Email or ID Number already exists.");
                            return;
                        }
                    }

                    string insertQuery = @"INSERT INTO Signup
                                           (Firstname, Lastname, Email, Password, Idnumber, Faculty, Role, IsActive)
                                           VALUES
                                           (@Firstname, @Lastname, @Email, @Password, @Idnumber, @Faculty, @Role, 1)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Firstname", txtFirstName.Text.Trim());
                        command.Parameters.AddWithValue("@Lastname", txtLastName.Text.Trim());
                        command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        command.Parameters.AddWithValue("@Password", txtPassword.Text);
                        command.Parameters.AddWithValue("@Idnumber", idNumber);
                        command.Parameters.AddWithValue("@Faculty", txtFaculty.Text.Trim());
                        command.Parameters.AddWithValue("@Role", cmbRole.Text);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("User added successfully!");

                LoadUsers();
                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtIdNumber.Clear();
            txtFaculty.Clear();
            txtPassword.Clear();

            cmbRole.SelectedIndex = 1;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Dashboardform dashboard = new Dashboardform("Admin");
            dashboard.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label8_Click(object sender, EventArgs e)
        {
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];

                txtIdNumber.Text = row.Cells["Idnumber"].Value.ToString();
                txtFirstName.Text = row.Cells["Firstname"].Value.ToString();
                txtLastName.Text = row.Cells["Lastname"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtFaculty.Text = row.Cells["Faculty"].Value.ToString();
                cmbRole.Text = row.Cells["Role"].Value.ToString();

                // Password is not displayed
                txtPassword.Clear();
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtIdNumber.Text))
                {
                    MessageBox.Show("Please select a user first.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                    string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtFaculty.Text))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                if (!int.TryParse(txtIdNumber.Text, out int idNumber))
                {
                    MessageBox.Show("ID Number must be a number.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"UPDATE Signup
                                     SET Firstname = @Firstname,
                                         Lastname = @Lastname,
                                         Email = @Email,
                                         Faculty = @Faculty,
                                         Role = @Role
                                     WHERE Idnumber = @Idnumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Firstname", txtFirstName.Text.Trim());
                        command.Parameters.AddWithValue("@Lastname", txtLastName.Text.Trim());
                        command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        command.Parameters.AddWithValue("@Faculty", txtFaculty.Text.Trim());
                        command.Parameters.AddWithValue("@Role", cmbRole.Text);
                        command.Parameters.AddWithValue("@Idnumber", idNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User updated successfully!");
                        }
                        else
                        {
                            MessageBox.Show("User could not be found.");
                        }
                    }
                }

                LoadUsers();
                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message);
            }
        }
        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtIdNumber.Text))
                {
                    MessageBox.Show("Please select a user first.");
                    return;
                }

                if (!int.TryParse(txtIdNumber.Text, out int idNumber))
                {
                    MessageBox.Show("Invalid ID Number.");
                    return;
                }

                // Prevent the Admin account from being deactivated
                if (cmbRole.Text.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("The Admin account cannot be deactivated.");
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to deactivate this user?",
                    "Confirm Deactivation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"UPDATE Signup
                                     SET IsActive = 0
                                     WHERE Idnumber = @Idnumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Idnumber", idNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User deactivated successfully!");
                        }
                        else
                        {
                            MessageBox.Show("User could not be found.");
                        }
                    }
                }

                LoadUsers();
                btnClear_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deactivating user: " + ex.Message);
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearch.Text.Trim();

                // If search box is empty, show all users
                if (string.IsNullOrEmpty(searchText))
                {
                    LoadUsers();
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"SELECT Idnumber, Firstname, Lastname, Email, Faculty, Role, IsActive
                                     FROM Signup
                                     WHERE Firstname LIKE @Search
                                        OR Lastname LIKE @Search
                                        OR Email LIKE @Search
                                        OR CAST(Idnumber AS NVARCHAR) LIKE @Search";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Search", "%" + searchText + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvUsers.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching users: " + ex.Message);
            }
        }
    }
}