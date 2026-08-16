namespace Student_Management_Syestem
{
    partial class Dashboardform
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboardform));
            this.lblWelcomeTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblStudentTitle = new System.Windows.Forms.Label();
            this.lblCourseTitle = new System.Windows.Forms.Label();
            this.lblEntrollmentTitle = new System.Windows.Forms.Label();
            this.lblPaymentTitle = new System.Windows.Forms.Label();
            this.lbldashboard = new System.Windows.Forms.Label();
            this.lblStudentCount = new System.Windows.Forms.Label();
            this.lblCourseCount = new System.Windows.Forms.Label();
            this.lblEntrollmentCount = new System.Windows.Forms.Label();
            this.lblPaymentCount = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWelcomeTitle
            // 
            this.lblWelcomeTitle.AutoSize = true;
            this.lblWelcomeTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcomeTitle.Location = new System.Drawing.Point(13, 75);
            this.lblWelcomeTitle.Name = "lblWelcomeTitle";
            this.lblWelcomeTitle.Size = new System.Drawing.Size(693, 32);
            this.lblWelcomeTitle.TabIndex = 2;
            this.lblWelcomeTitle.Text = "WELCOME TO THE NSBM STUDENT MANAGEMENT SYSTEM";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(767, -33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(221, 178);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel1.Controls.Add(this.lblStudentCount);
            this.panel1.Controls.Add(this.lblStudentTitle);
            this.panel1.Location = new System.Drawing.Point(338, 151);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(243, 140);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel2.Controls.Add(this.lblCourseCount);
            this.panel2.Controls.Add(this.lblCourseTitle);
            this.panel2.Location = new System.Drawing.Point(638, 154);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(240, 137);
            this.panel2.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel3.Controls.Add(this.lblEntrollmentCount);
            this.panel3.Controls.Add(this.lblEntrollmentTitle);
            this.panel3.Location = new System.Drawing.Point(338, 332);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(243, 140);
            this.panel3.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.panel4.Controls.Add(this.lblPaymentCount);
            this.panel4.Controls.Add(this.lblPaymentTitle);
            this.panel4.Location = new System.Drawing.Point(638, 332);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(240, 137);
            this.panel4.TabIndex = 6;
            // 
            // lblStudentTitle
            // 
            this.lblStudentTitle.AutoSize = true;
            this.lblStudentTitle.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudentTitle.Location = new System.Drawing.Point(9, 29);
            this.lblStudentTitle.Name = "lblStudentTitle";
            this.lblStudentTitle.Size = new System.Drawing.Size(226, 32);
            this.lblStudentTitle.TabIndex = 15;
            this.lblStudentTitle.Text = "TOTAL STUDENTS";
            // 
            // lblCourseTitle
            // 
            this.lblCourseTitle.AutoSize = true;
            this.lblCourseTitle.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCourseTitle.Location = new System.Drawing.Point(18, 27);
            this.lblCourseTitle.Name = "lblCourseTitle";
            this.lblCourseTitle.Size = new System.Drawing.Size(208, 32);
            this.lblCourseTitle.TabIndex = 16;
            this.lblCourseTitle.Text = "TOTAL COURSES";
            // 
            // lblEntrollmentTitle
            // 
            this.lblEntrollmentTitle.AutoSize = true;
            this.lblEntrollmentTitle.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntrollmentTitle.Location = new System.Drawing.Point(18, 29);
            this.lblEntrollmentTitle.Name = "lblEntrollmentTitle";
            this.lblEntrollmentTitle.Size = new System.Drawing.Size(206, 32);
            this.lblEntrollmentTitle.TabIndex = 17;
            this.lblEntrollmentTitle.Text = "ENTROLLMENTS";
            // 
            // lblPaymentTitle
            // 
            this.lblPaymentTitle.AutoSize = true;
            this.lblPaymentTitle.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentTitle.Location = new System.Drawing.Point(54, 29);
            this.lblPaymentTitle.Name = "lblPaymentTitle";
            this.lblPaymentTitle.Size = new System.Drawing.Size(149, 32);
            this.lblPaymentTitle.TabIndex = 18;
            this.lblPaymentTitle.Text = "PAYMENTS";
            // 
            // lbldashboard
            // 
            this.lbldashboard.AutoSize = true;
            this.lbldashboard.Font = new System.Drawing.Font("Segoe UI Black", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldashboard.Location = new System.Drawing.Point(11, 13);
            this.lbldashboard.Name = "lbldashboard";
            this.lbldashboard.Size = new System.Drawing.Size(256, 48);
            this.lbldashboard.TabIndex = 7;
            this.lbldashboard.Text = "DASHBOARD";
            // 
            // lblStudentCount
            // 
            this.lblStudentCount.AutoSize = true;
            this.lblStudentCount.Font = new System.Drawing.Font("Segoe UI Black", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStudentCount.ForeColor = System.Drawing.Color.Blue;
            this.lblStudentCount.Location = new System.Drawing.Point(78, 71);
            this.lblStudentCount.Name = "lblStudentCount";
            this.lblStudentCount.Size = new System.Drawing.Size(92, 45);
            this.lblStudentCount.TabIndex = 16;
            this.lblStudentCount.Text = "1500";
            // 
            // lblCourseCount
            // 
            this.lblCourseCount.AutoSize = true;
            this.lblCourseCount.Font = new System.Drawing.Font("Segoe UI Black", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCourseCount.ForeColor = System.Drawing.Color.Blue;
            this.lblCourseCount.Location = new System.Drawing.Point(78, 68);
            this.lblCourseCount.Name = "lblCourseCount";
            this.lblCourseCount.Size = new System.Drawing.Size(92, 45);
            this.lblCourseCount.TabIndex = 17;
            this.lblCourseCount.Text = "1500";
            // 
            // lblEntrollmentCount
            // 
            this.lblEntrollmentCount.AutoSize = true;
            this.lblEntrollmentCount.Font = new System.Drawing.Font("Segoe UI Black", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntrollmentCount.ForeColor = System.Drawing.Color.Blue;
            this.lblEntrollmentCount.Location = new System.Drawing.Point(78, 72);
            this.lblEntrollmentCount.Name = "lblEntrollmentCount";
            this.lblEntrollmentCount.Size = new System.Drawing.Size(92, 45);
            this.lblEntrollmentCount.TabIndex = 18;
            this.lblEntrollmentCount.Text = "1500";
            // 
            // lblPaymentCount
            // 
            this.lblPaymentCount.AutoSize = true;
            this.lblPaymentCount.Font = new System.Drawing.Font("Segoe UI Black", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentCount.ForeColor = System.Drawing.Color.Blue;
            this.lblPaymentCount.Location = new System.Drawing.Point(81, 67);
            this.lblPaymentCount.Name = "lblPaymentCount";
            this.lblPaymentCount.Size = new System.Drawing.Size(92, 45);
            this.lblPaymentCount.TabIndex = 19;
            this.lblPaymentCount.Text = "1500";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 143);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(224, 58);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(19, 217);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(224, 58);
            this.button2.TabIndex = 9;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(19, 293);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(224, 58);
            this.button3.TabIndex = 10;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(19, 376);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(224, 58);
            this.button4.TabIndex = 11;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(19, 453);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(224, 58);
            this.button5.TabIndex = 12;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(19, 524);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(224, 58);
            this.button6.TabIndex = 13;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(828, 503);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(129, 79);
            this.button7.TabIndex = 14;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // Dashboardform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 594);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbldashboard);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblWelcomeTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 650);
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "Dashboardform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboardform";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWelcomeTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblStudentTitle;
        private System.Windows.Forms.Label lblCourseTitle;
        private System.Windows.Forms.Label lblEntrollmentTitle;
        private System.Windows.Forms.Label lblPaymentTitle;
        private System.Windows.Forms.Label lbldashboard;
        private System.Windows.Forms.Label lblStudentCount;
        private System.Windows.Forms.Label lblCourseCount;
        private System.Windows.Forms.Label lblEntrollmentCount;
        private System.Windows.Forms.Label lblPaymentCount;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}