using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Student_Management_Syestem
{
    public static class DbHelper
    {
        public static string ConnectionString =>
            @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True; Connect Timeout=30";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static void InitializeDatabase()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    // 1. Student Table
                    string studentSql = @"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Student')
BEGIN
    CREATE TABLE [dbo].[Student] (
        [Studentid] INT NOT NULL,
        [Fullname]  VARCHAR (100) NOT NULL,
        [email]     VARCHAR (100) NOT NULL,
        [phone]     VARCHAR (15)  NOT NULL,
        [address]   VARCHAR (200) NOT NULL
    );
END";
                    ExecuteNonQuery(conn, studentSql);

                    // 2. Course Table
                    string courseSql = @"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Course')
BEGIN
    CREATE TABLE [dbo].[Course] (
        [CourseID]    VARCHAR (50)  NOT NULL PRIMARY KEY,
        [CourseName]  VARCHAR (100) NOT NULL,
        [CourseCode]  VARCHAR (50)  NOT NULL,
        [Duration]    VARCHAR (50)  NOT NULL,
        [Instructor]  VARCHAR (100) NOT NULL,
        [Description] VARCHAR (200) NOT NULL
    );
END";
                    ExecuteNonQuery(conn, courseSql);

                    // 3. Enrollment Table
                    string enrollmentSql = @"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Enrollment')
BEGIN
    CREATE TABLE [dbo].[Enrollment] (
        [StudentID]      INT           NOT NULL,
        [StudentName]    VARCHAR (100) NOT NULL,
        [Course]         VARCHAR (100) NOT NULL,
        [AcademicYear]   VARCHAR (100) NOT NULL,
        [Semester]       VARCHAR (100) NOT NULL,
        [EnrollmentDate] VARCHAR (100) NOT NULL,
        [Status]         VARCHAR (100) NOT NULL
    );
END";
                    ExecuteNonQuery(conn, enrollmentSql);

                    // 4. Payment Table
                    string paymentSql = @"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Payment')
BEGIN
    CREATE TABLE [dbo].[Payment] (
        [PaymentID]     INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
        [StudentID]     VARCHAR (50)    NOT NULL,
        [StudentName]   VARCHAR (100)   NOT NULL,
        [PaymentMode]   VARCHAR (50)    NOT NULL,
        [ChequeNo]      VARCHAR (50)    NULL,
        [BankName]      VARCHAR (100)   NULL,
        [IsInstallment] BIT             NOT NULL DEFAULT 0,
        [InstallmentNo] VARCHAR (50)    NULL,
        [Amount]        DECIMAL (18, 2) NOT NULL,
        [PaymentDate]   VARCHAR (50)    NOT NULL,
        [Status]        VARCHAR (50)    NOT NULL
    );
END";
                    ExecuteNonQuery(conn, paymentSql);

                    // 5. Attendance Table
                    string attendanceSql = @"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Attendance')
BEGIN
    CREATE TABLE [dbo].[Attendance] (
        [AttendanceID] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
        [StudentID]    VARCHAR (50)  NOT NULL,
        [StudentName]  VARCHAR (100) NOT NULL,
        [Course]       VARCHAR (100) NOT NULL,
        [Date]         VARCHAR (50)  NOT NULL,
        [Session]      VARCHAR (50)  NOT NULL,
        [Status]       VARCHAR (50)  NOT NULL,
        [Remarks]      VARCHAR (200) NULL
    );
END";
                    ExecuteNonQuery(conn, attendanceSql);

                    // 6. Signup Table & Role Column
                    string signupSql = @"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Signup')
BEGIN
    CREATE TABLE [dbo].[Signup] (
        [Firstname] VARCHAR (100) NOT NULL,
        [Lastname]  VARCHAR (100) NOT NULL,
        [Email]     VARCHAR (100) NOT NULL,
        [Password]  VARCHAR (100) NOT NULL,
        [Idnumber]  INT           NOT NULL,
        [Faculty]   VARCHAR (100) NOT NULL,
        [Role]      VARCHAR (50)  NOT NULL DEFAULT 'Student'
    );
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Signup' AND COLUMN_NAME = 'Role')
    BEGIN
        ALTER TABLE [dbo].[Signup] ADD [Role] VARCHAR (50) NOT NULL DEFAULT 'Student';
    END
END";
                    ExecuteNonQuery(conn, signupSql);

                    // 7. Seed Default Accounts
                    string seedAccountsSql = @"
IF NOT EXISTS (SELECT 1 FROM [dbo].[Signup] WHERE [Email] = 'admin@nsbm.lk' OR [Email] = 'admin')
BEGIN
    INSERT INTO [dbo].[Signup] ([Firstname], [Lastname], [Email], [Password], [Idnumber], [Faculty], [Role])
    VALUES ('System', 'Admin', 'admin@nsbm.lk', 'admin123', 100, 'Administration', 'Admin');
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Signup] WHERE [Email] = 'student@nsbm.lk')
BEGIN
    INSERT INTO [dbo].[Signup] ([Firstname], [Lastname], [Email], [Password], [Idnumber], [Faculty], [Role])
    VALUES ('Kamal', 'Perera', 'student@nsbm.lk', 'student123', 1001, 'Computing', 'Student');
END";
                    ExecuteNonQuery(conn, seedAccountsSql);

                    // 8. Seed Initial Sample Courses if empty
                    string seedCoursesSql = @"
IF NOT EXISTS (SELECT 1 FROM [dbo].[Course])
BEGIN
    INSERT INTO [dbo].[Course] ([CourseID], [CourseName], [CourseCode], [Duration], [Instructor], [Description])
    VALUES 
    ('C001', 'Computer Science', 'CS101', '4 Years', 'Mr. Silva', 'Foundations of computing and algorithm design'),
    ('C002', 'Information Technology', 'IT101', '4 Years', 'Ms. Perera', 'Applied IT infrastructure and networking'),
    ('C003', 'Software Engineering', 'SE101', '4 Years', 'Mr. Fernando', 'Enterprise software architecture and lifecycle');
END";
                    ExecuteNonQuery(conn, seedCoursesSql);

                    // 9. Seed Initial Sample Students if empty
                    string seedStudentsSql = @"
IF NOT EXISTS (SELECT 1 FROM [dbo].[Student])
BEGIN
    INSERT INTO [dbo].[Student] ([Studentid], [Fullname], [email], [phone], [address])
    VALUES
    (1001, 'Kamal Perera', 'kamal@gmail.com', '0771234567', '12 Colombo Rd, Nugegoda'),
    (1002, 'Nimal Silva', 'nimal@gmail.com', '0772345678', '45 Kandy Rd, Kadawatha'),
    (1003, 'Saman Kumara', 'saman@gmail.com', '0773456789', '88 Galle Rd, Moratuwa');
END";
                    ExecuteNonQuery(conn, seedStudentsSql);

                    // 10. Seed Initial Sample Enrollments if empty
                    string seedEnrollmentSql = @"
IF NOT EXISTS (SELECT 1 FROM [dbo].[Enrollment])
BEGIN
    INSERT INTO [dbo].[Enrollment] ([StudentID], [StudentName], [Course], [AcademicYear], [Semester], [EnrollmentDate], [Status])
    VALUES
    (1001, 'Kamal Perera', 'Computer Science', '1st Year', 'Semester1', '2026-09-01', 'Active'),
    (1002, 'Nimal Silva', 'Software Engineering', '2nd Year', 'Semester 3', '2026-09-02', 'Active');
END";
                    ExecuteNonQuery(conn, seedEnrollmentSql);

                    // 11. Seed Initial Sample Payments if empty
                    string seedPaymentSql = @"
IF NOT EXISTS (SELECT 1 FROM [dbo].[Payment])
BEGIN
    INSERT INTO [dbo].[Payment] ([StudentID], [StudentName], [PaymentMode], [ChequeNo], [BankName], [IsInstallment], [InstallmentNo], [Amount], [PaymentDate], [Status])
    VALUES
    ('1001', 'Kamal Perera', 'Cash', '', '', 0, '', 50000.00, '2026-09-10', 'Paid'),
    ('1002', 'Nimal Silva', 'Cheque', 'CHQ8892', 'BOC', 1, 'Inst 1', 25000.00, '2026-09-11', 'Paid');
END";
                    ExecuteNonQuery(conn, seedPaymentSql);

                    // 12. Seed Initial Sample Attendance if empty
                    string seedAttendanceSql = @"
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attendance])
BEGIN
    INSERT INTO [dbo].[Attendance] ([StudentID], [StudentName], [Course], [Date], [Session], [Status], [Remarks])
    VALUES
    ('1001', 'Kamal Perera', 'Computer Science', '2026-09-14', 'Morning', 'Present', 'On time'),
    ('1002', 'Nimal Silva', 'Software Engineering', '2026-09-14', 'Morning', 'Present', 'Regular');
END";
                    ExecuteNonQuery(conn, seedAttendanceSql);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DbHelper.InitializeDatabase error: " + ex.Message);
            }
        }

        private static void ExecuteNonQuery(SqlConnection conn, string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
