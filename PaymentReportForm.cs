using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public partial class PaymentReportForm : Form
    {
        private DataGridView dgvPayments;
        private TextBox txtSearch;
        private Label lblTotalStats;
        private DataTable _paymentDt;

        public PaymentReportForm()
        {
            InitializeComponent();
            SetupUi();
            LoadPaymentData();
        }

        private void SetupUi()
        {
            this.Text = "Payment Report";

            // Title
            Label title = new Label();
            title.Text = "PAYMENT & FEE REPORT";
            title.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            title.ForeColor = Color.DarkBlue;
            title.AutoSize = false;
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(20, 15);
            title.Size = new Size(935, 45);
            this.Controls.Add(title);

            // Search label & box
            Label lblSearch = new Label();
            lblSearch.Text = "Search:";
            lblSearch.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            lblSearch.Location = new Point(30, 72);
            lblSearch.Size = new Size(70, 25);
            this.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.Location = new Point(105, 68);
            txtSearch.Size = new Size(300, 30);
            txtSearch.TextChanged += TxtSearch_TextChanged;
            this.Controls.Add(txtSearch);

            // Stats summary label
            lblTotalStats = new Label();
            lblTotalStats.Font = new Font("Segoe UI Semibold", 10, FontStyle.Bold);
            lblTotalStats.ForeColor = Color.DarkGreen;
            lblTotalStats.TextAlign = ContentAlignment.MiddleRight;
            lblTotalStats.Location = new Point(450, 70);
            lblTotalStats.Size = new Size(495, 28);
            this.Controls.Add(lblTotalStats);

            // DataGridView
            dgvPayments = new DataGridView();
            dgvPayments.Location = new Point(30, 110);
            dgvPayments.Size = new Size(915, 450);
            dgvPayments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPayments.AllowUserToAddRows = false;
            dgvPayments.ReadOnly = true;
            dgvPayments.RowHeadersVisible = false;
            dgvPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayments.EnableHeadersVisualStyles = false;
            dgvPayments.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvPayments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPayments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPayments.ColumnHeadersHeight = 35;
            dgvPayments.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgvPayments.RowTemplate.Height = 28;
            dgvPayments.DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgvPayments.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.Controls.Add(dgvPayments);

            // Back button
            Button back = new Button();
            back.Text = "Back to Report Hub";
            back.Size = new Size(180, 42);
            back.Location = new Point(30, 575);
            back.BackColor = Color.SteelBlue;
            back.ForeColor = Color.White;
            back.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            back.FlatStyle = FlatStyle.Flat;
            back.FlatAppearance.BorderSize = 0;
            back.Cursor = Cursors.Hand;
            back.Click += (s, e) => ReturnToReportHub();
            this.Controls.Add(back);

            this.FormClosing += (s, e) => ReturnToReportHub();
        }

        private void LoadPaymentData()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT PaymentID AS [ID], StudentID AS [Student ID], StudentName AS [Student Name], " +
                                   "PaymentMode AS [Mode], ChequeNo AS [Cheque No], BankName AS [Bank], " +
                                   "Amount AS [Amount (Rs.)], PaymentDate AS [Date], Status AS [Status] FROM Payment ORDER BY PaymentID DESC";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        _paymentDt = new DataTable();
                        adapter.Fill(_paymentDt);
                        dgvPayments.DataSource = _paymentDt;

                        if (dgvPayments.Columns["Amount (Rs.)"] != null)
                        {
                            dgvPayments.Columns["Amount (Rs.)"].DefaultCellStyle.Format = "N2";
                        }

                        UpdateSummary();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payments: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary()
        {
            if (_paymentDt == null) return;

            int count = _paymentDt.DefaultView.Count;
            decimal totalAmount = 0;

            foreach (DataRowView drv in _paymentDt.DefaultView)
            {
                if (decimal.TryParse(drv["Amount (Rs.)"]?.ToString(), out decimal val))
                {
                    totalAmount += val;
                }
            }

            lblTotalStats.Text = string.Format("Total Records: {0}   |   Total Collected: Rs. {1:N2}", count, totalAmount);
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_paymentDt == null) return;

            string filter = txtSearch.Text.Trim().Replace("'", "''");
            if (string.IsNullOrEmpty(filter))
            {
                _paymentDt.DefaultView.RowFilter = "";
            }
            else
            {
                _paymentDt.DefaultView.RowFilter = string.Format(
                    "[Student ID] LIKE '%{0}%' OR [Student Name] LIKE '%{0}%' OR [Mode] LIKE '%{0}%' OR [Bank] LIKE '%{0}%' OR [Status] LIKE '%{0}%'",
                    filter);
            }
            UpdateSummary();
        }

        private bool _isReturning = false;
        private void ReturnToReportHub()
        {
            if (_isReturning) return;
            _isReturning = true;

            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is ReportHubForm)
                {
                    openForm.Show();
                    this.Dispose();
                    return;
                }
            }

            ReportHubForm report = new ReportHubForm();
            report.Show();
            this.Dispose();
        }
    }
}
