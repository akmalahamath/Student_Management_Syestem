USE [C:\USERS\AKMAL AHAMATH\DESKTOP\C# GROUP PROJECT\STUDENT_MANAGEMENT_SYESTEM\DATABASE1.MDF]
GO

/****** Object: Table [dbo].[Enrollment] Script Date: 9/11/2026 3:26:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Enrollment] (
    [StudentID]      INT           NOT NULL,
    [StudentName]    VARCHAR (100) NOT NULL,
    [Course]         VARCHAR (100) NOT NULL,
    [AcademicYear]   VARCHAR (100) NOT NULL,
    [Semester]       VARCHAR (100) NOT NULL,
    [EnrollmentDate] VARCHAR (100) NOT NULL,
    [Status]         VARCHAR (100) NOT NULL
);

GO
USE [C:\USERS\AKMAL AHAMATH\DESKTOP\C# GROUP PROJECT\STUDENT_MANAGEMENT_SYESTEM\DATABASE1.MDF]
GO

/****** Object: Table [dbo].[Signup] Script Date: 9/11/2026 3:27:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Signup] (
    [Firstname] VARCHAR (100) NOT NULL,
    [Lastname]  VARCHAR (100) NOT NULL,
    [Email]     VARCHAR (100) NOT NULL,
    [Password]  VARCHAR (100) NOT NULL,
    [Idnumber]  INT           NOT NULL,
    [Faculty]   VARCHAR (100) NOT NULL
);


GO
USE [C:\USERS\AKMAL AHAMATH\DESKTOP\C# GROUP PROJECT\STUDENT_MANAGEMENT_SYESTEM\DATABASE1.MDF]
GO

/****** Object: Table [dbo].[Student] Script Date: 9/11/2026 3:29:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Student] (
    [Studentid] INT           NOT NULL,
    [Fullname]  VARCHAR (100) NOT NULL,
    [email]     VARCHAR (100) NOT NULL,
    [phone]     VARCHAR (15)  NOT NULL,
    [address]   VARCHAR (200) NOT NULL
);