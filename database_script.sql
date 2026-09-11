/****** Object: Table [dbo].[Enrollment] ******/
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

/****** Object: Table [dbo].[Signup] ******/
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

/****** Object: Table [dbo].[Student] ******/
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
GO