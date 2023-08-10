/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		07/28/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Version')
BEGIN
		PRINT 'Dropping Table Version'
		DROP  Table Version
END
GO

CREATE TABLE [Version](
	[VersionId] [numeric](10, 0) NOT NULL,
	[DateCreated] [datetime] NULL,
	[MajorNum] [int] NULL,
	[MinorNum] [int] NULL,
	[Notes] [text] NULL
)
GO
IF OBJECT_ID('Version') IS NOT NULL
    PRINT '<<< CREATED TABLE Version >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE Version >>>'
GO

/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		07/28/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Syslog')
BEGIN
		PRINT 'Dropping Table Syslog'
		DROP  Table Syslog
END
GO
CREATE TABLE [Syslog](
	[SyslogId] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime] NULL,
	[MsgSource] [nvarchar](255) NULL,
	[Payload] [text] NULL,
	[MsgText] [text] NULL
)
IF OBJECT_ID('Syslog') IS NOT NULL
    PRINT '<<< CREATED TABLE Syslog >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE Syslog >>>'
GO

/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		07/28/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'MemberRole')
BEGIN
		PRINT 'Dropping Table MemberRole'
		DROP  Table MemberRole
END
GO
CREATE TABLE [MemberRole](
	[MemberRoleId] [numeric](10, 0) NOT NULL,
	[DateCreated] [datetime] NULL,
	[Code] [nvarchar](255) NULL
)
IF OBJECT_ID('MemberRole') IS NOT NULL
    PRINT '<<< CREATED TABLE MemberRole >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE MemberRole >>>'
GO

/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		07/28/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Member')
BEGIN
		PRINT 'Dropping Table Member'
		DROP  Table Member
END
GO
CREATE TABLE [Member](
	[MemberId] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[MemberRoleId] [numeric](10, 0) NULL,
	[DateCreated] [datetime] NULL,
	[DateModified] [datetime] NULL,
	[FirstName] [nvarchar](255) NULL,
	[MiddleName] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[Username] [nvarchar](255) NULL,
	[PasswordEncrypted] [nvarchar](255) NULL,
	[PictureUrl] [nvarchar](255) NULL,
	[PictureData] [varbinary](max) NULL,
	[IsDisabled] [bit] NULL,
	[LastLoginDate] [datetime] NULL
)
IF OBJECT_ID('Member') IS NOT NULL
    PRINT '<<< CREATED TABLE Member >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE Member >>>'
GO

/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		07/28/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'IncomingEmail')
BEGIN
		PRINT 'Dropping Table IncomingEmail'
		DROP  Table IncomingEmail
END
GO
CREATE TABLE [IncomingEmail](
	[IncomingEmailId] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime] NULL,
	[DateModified] [datetime] NULL,
	[DateBeginProcessing] [datetime] NULL,
	[DateEndProcessing] [datetime] NULL,
	[ProcessingDurationInMS] [int] NULL,
	[MsgSource] [nvarchar](255) NULL,
	[IsProcessed] [bit] NULL,
	[IsTest] [bit] NULL,
	[IsError] [bit] NULL,
	[ErrorStr] [text] NULL,
	[RawData] [text] NULL
)
IF OBJECT_ID('IncomingEmail') IS NOT NULL
    PRINT '<<< CREATED TABLE IncomingEmail >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE IncomingEmail >>>'
GO

INSERT [dbo].[MemberRole] ([MemberRoleId], [DateCreated], [Code]) VALUES (1, GETDATE(), 'MEMBER_ROLE_ADMIN')
GO
INSERT [dbo].[MemberRole] ([MemberRoleId], [DateCreated], [Code]) VALUES (2, GETDATE(), 'MEMBER_ROLE_USER')
GO

INSERT INTO [Version] (VersionId, DateCreated, MajorNum, MinorNum,Notes) VALUES (1, GETDATE(), 1, 0,'Initial 1.0');
GO