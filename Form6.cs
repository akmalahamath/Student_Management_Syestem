using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class Form6 : Form
    {
        private int _selectedPaymentId = -1;
        private DataTable _paymentTable;

        public Form6()
        {
            InitializeComponent();
            this.FormClosing += Form6_FormClosing;
            this.txtStudentID.Leave += TxtStudentID_Leave;
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                ReturnToDashboard();
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            LoadPaymentsFromDb();
            UpdatePaymentModeUi();
            UpdateInstallmentUi();
        }

        private void TxtStudentID_Leave(object sender, EventArgs e)
        {
            string idStr = txtStudentID.Text.Trim();
            if (string.IsNullOrEmpty(idStr)) return;

            int studentId;
            if (!int.TryParse(idStr, out studentId)) return;

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT Fullname FROM Student WHERE Studentid = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", studentId);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            textBox2.Text = result.ToString();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        // Load payments from Database
        private void LoadPaymentsFromDb()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT PaymentID, StudentID, StudentName, PaymentMode, ChequeNo, BankName, IsInstallment, InstallmentNo, Amount, PaymentDate, Status FROM Payment ORDER BY PaymentID DESC";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        _paymentTable = new DataTable();
                        adapter.Fill(_paymentTable);
                        dgvStudents.DataSource = _paymentTable;
                        SetColumnHeaders();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payments: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetColumnHeaders()
        {
            if (dgvStudents.Columns["PaymentID"] != null) dgvStudents.Columns["PaymentID"].Visible = false;
            if (dgvStudents.Columns["StudentID"] != null) dgvStudents.Columns["StudentID"].HeaderText = "Student ID";
            if (dgvStudents.Columns["StudentName"] != null) dgvStudents.Columns["StudentName"].HeaderText = "Student Name";
            if (dgvStudents.Columns["PaymentMode"] != null) dgvStudents.Columns["PaymentMode"].HeaderText = "Mode";
            if (dgvStudents.Columns["ChequeNo"] != null) dgvStudents.Columns["ChequeNo"].HeaderText = "Cheque No";
            if (dgvStudents.Columns["BankName"] != null) dgvStudents.Columns["BankName"].HeaderText = "Bank";
            if (dgvStudents.Columns["IsInstallment"] != null) dgvStudents.Columns["IsInstallment"].HeaderText = "Installment";
            if (dgvStudents.Columns["InstallmentNo"] != null) dgvStudents.Columns["InstallmentNo"].HeaderText = "Inst. No";
            if (dgvStudents.Columns["Amount"] != null)
            {
                dgvStudents.Columns["Amount"].HeaderText = "Amount (Rs.)";
                dgvStudents.Columns["Amount"].DefaultCellStyle.Format = "N2";
            }
            if (dgvStudents.Columns["PaymentDate"] != null) dgvStudents.Columns["PaymentDate"].HeaderText = "Date";
            if (dgvStudents.Columns["Status"] != null) dgvStudents.Columns["Status"].HeaderText = "Status";
        }

        // UI helpers
        private void UpdatePaymentModeUi()
        {
            bool isCheque = radioButton1.Checked;
            txtChequeNo.Enabled = isCheque;
            txtBankName.Enabled = isCheque;
            if (!isCheque) { txtChequeNo.Clear(); txtBankName.Clear(); }
        }

        private void UpdateInstallmentUi()
        {
            txtInstallmentNo.Enabled = checkBox1.Checked;
            if (!checkBox1.Checked) txtInstallmentNo.Clear();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) { UpdatePaymentModeUi(); }
        private void radioButton2_CheckedChanged(object sender, EventArgs e) { UpdatePaymentModeUi(); }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { UpdateInstallmentUi(); }

        // ADD BUTTON
        private void button4_Click(object sender, EventArgs e)
        {
            string studentId = txtStudentID.Text.Trim();
            string studentName = textBox2.Text.Trim();
            string paymentMode = radioButton1.Checked ? "Cheque" : "Cash";
            string chequeNo = txtChequeNo.Text.Trim();
            string bankName = txtBankName.Text.Trim();
            bool isInstallment = checkBox1.Checked;
            string installmentNo = txtInstallmentNo.Text.Trim();
            string amountStr = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(studentId))
            { MessageBox.Show("Please enter a Student ID.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtStudentID.Focus(); return; }

            if (string.IsNullOrEmpty(studentName))
            { MessageBox.Show("Please enter the Student Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); textBox2.Focus(); return; }

            if (radioButton1.Checked && string.IsNullOrEmpty(chequeNo))
            { MessageBox.Show("Please enter Cheque Number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtChequeNo.Focus(); return; }

            if (radioButton1.Checked && string.IsNullOrEmpty(bankName))
            { MessageBox.Show("Please enter Bank Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtBankName.Focus(); return; }

            if (isInstallment && string.IsNullOrEmpty(installmentNo))
            { MessageBox.Show("Please enter Installment Number.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtInstallmentNo.Focus(); return; }

            if (string.IsNullOrEmpty(amountStr) || !decimal.TryParse(amountStr, out decimal amount) || amount <= 0)
            { MessageBox.Show("Please enter a valid positive amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); textBox1.Focus(); return; }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string insertSql = "INSERT INTO Payment (StudentID, StudentName, PaymentMode, ChequeNo, BankName, IsInstallment, InstallmentNo, Amount, PaymentDate, Status) " +
                                       "VALUES (@sid, @sname, @mode, @chq, @bank, @isInst, @instNo, @amt, @pdate, @status)";
                    using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", studentId);
                        cmd.Parameters.AddWithValue("@sname", studentName);
                        cmd.Parameters.AddWithValue("@mode", paymentMode);
                        cmd.Parameters.AddWithValue("@chq", chequeNo);
                        cmd.Parameters.AddWithValue("@bank", bankName);
                        cmd.Parameters.AddWithValue("@isInst", isInstallment);
                        cmd.Parameters.AddWithValue("@instNo", installmentNo);
                        cmd.Parameters.AddWithValue("@amt", amount);
                        cmd.Parameters.AddWithValue("@pdate", DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@status", "Paid");

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Payment record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadPaymentsFromDb();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // UPDATE BUTTON
        private void button3_Click(object sender, EventArgs e)
        {
            if (_selectedPaymentId <= 0)
            { MessageBox.Show("Please select a row from the table to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            string studentId = txtStudentID.Text.Trim();
            string studentName = textBox2.Text.Trim();
            string paymentMode = radioButton1.Checked ? "Cheque" : "Cash";
            string chequeNo = txtChequeNo.Text.Trim();
            string bankName = txtBankName.Text.Trim();
            bool isInstallment = checkBox1.Checked;
            string installmentNo = txtInstallmentNo.Text.Trim();

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(studentName))
            { MessageBox.Show("Student ID and Name are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (!decimal.TryParse(textBox1.Text.Trim(), out decimal amount) || amount <= 0)
            { MessageBox.Show("Please enter a valid amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string updateSql = "UPDATE Payment SET StudentID=@sid, StudentName=@sname, PaymentMode=@mode, ChequeNo=@chq, BankName=@bank, IsInstallment=@isInst, InstallmentNo=@instNo, Amount=@amt " +
                                       "WHERE PaymentID=@id";
                    using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _selectedPaymentId);
                        cmd.Parameters.AddWithValue("@sid", studentId);
                        cmd.Parameters.AddWithValue("@sname", studentName);
                        cmd.Parameters.AddWithValue("@mode", paymentMode);
                        cmd.Parameters.AddWithValue("@chq", chequeNo);
                        cmd.Parameters.AddWithValue("@bank", bankName);
                        cmd.Parameters.AddWithValue("@isInst", isInstallment);
                        cmd.Parameters.AddWithValue("@instNo", installmentNo);
                        cmd.Parameters.AddWithValue("@amt", amount);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Payment record updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetForm();
                            LoadPaymentsFromDb();
                        }
                        else
                        {
                            MessageBox.Show("Record not found to update.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DELETE BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            if (_selectedPaymentId <= 0)
            { MessageBox.Show("Please select a row from the table to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (MessageBox.Show("Are you sure you want to delete this payment record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string deleteSql = "DELETE FROM Payment WHERE PaymentID = @id";
                    using (SqlCommand cmd = new SqlCommand(deleteSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", _selectedPaymentId);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Record deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetForm();
                            LoadPaymentsFromDb();
                        }
                        else
                        {
                            MessageBox.Show("Record not found to delete.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // SEARCH BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            string query = txtStudentID.Text.Trim();
            if (string.IsNullOrEmpty(query)) query = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(query))
            {
                dgvStudents.DataSource = _paymentTable;
                return;
            }

            if (_paymentTable != null)
            {
                DataView dv = new DataView(_paymentTable);
                dv.RowFilter = string.Format(
                    "StudentID LIKE '%{0}%' OR StudentName LIKE '%{0}%'", query.Replace("'", "''"));

                dgvStudents.DataSource = dv;

                if (dv.Count == 0)
                    MessageBox.Show("No records found matching: " + query, "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // RESET BUTTON
        private void button6_Click(object sender, EventArgs e)
        {
            ResetForm();
            LoadPaymentsFromDb();
        }

        // CANCEL / BACK BUTTON
        private void button5_Click(object sender, EventArgs e)
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

        private void ResetForm()
        {
            _selectedPaymentId = -1;
            txtStudentID.Clear();
            textBox2.Clear();
            radioButton2.Checked = true;
            txtChequeNo.Clear();
            txtBankName.Clear();
            checkBox1.Checked = false;
            txtInstallmentNo.Clear();
            textBox1.Clear();
            UpdatePaymentModeUi();
            UpdateInstallmentUi();
            txtStudentID.Focus();
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvStudents.Rows.Count)
                PopulateFormFromRow(dgvStudents.Rows[e.RowIndex]);
        }

        private void PopulateFormFromRow(DataGridViewRow row)
        {
            if (row == null || row.Cells["PaymentID"] == null) return;

            if (row.Cells["PaymentID"].Value != null && row.Cells["PaymentID"].Value != DBNull.Value)
                _selectedPaymentId = Convert.ToInt32(row.Cells["PaymentID"].Value);

            txtStudentID.Text = row.Cells["StudentID"]?.Value?.ToString() ?? "";
            textBox2.Text = row.Cells["StudentName"]?.Value?.ToString() ?? "";

            string mode = row.Cells["PaymentMode"]?.Value?.ToString() ?? "Cash";
            if (mode.Equals("Cheque", StringComparison.OrdinalIgnoreCase))
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;

            txtChequeNo.Text = row.Cells["ChequeNo"]?.Value?.ToString() ?? "";
            txtBankName.Text = row.Cells["BankName"]?.Value?.ToString() ?? "";

            bool isInst = false;
            if (row.Cells["IsInstallment"]?.Value != null && row.Cells["IsInstallment"].Value != DBNull.Value)
                bool.TryParse(row.Cells["IsInstallment"].Value.ToString(), out isInst);

            checkBox1.Checked = isInst;
            txtInstallmentNo.Text = row.Cells["InstallmentNo"]?.Value?.ToString() ?? "";

            if (row.Cells["Amount"]?.Value != null && row.Cells["Amount"].Value != DBNull.Value)
            {
                if (decimal.TryParse(row.Cells["Amount"].Value.ToString(), out decimal amt))
                    textBox1.Text = amt.ToString("0.00");
                else
                    textBox1.Text = row.Cells["Amount"].Value.ToString();
            }

            UpdatePaymentModeUi();
            UpdateInstallmentUi();
        }
    }
}
