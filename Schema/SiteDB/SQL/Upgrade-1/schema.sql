
/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		08/09/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'AIType')
BEGIN
		PRINT 'Dropping Table AIType'
		DROP  Table AIType
END
GO
CREATE TABLE [AIType](
	[AITypeId] [numeric](10, 0) NOT NULL,
	[DateCreated] [datetime] NULL,
	[Code] [nvarchar](255) NULL
)
IF OBJECT_ID('AIType') IS NOT NULL
    PRINT '<<< CREATED TABLE AIType >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE AIType >>>'
GO

/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		08/08/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'AIMember')
BEGIN
		PRINT 'Dropping Table AIMember'
		DROP  Table AIMember
END
GO
CREATE TABLE [AIMember](
	[AIMemberId] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[MemberId] [numeric](10, 0) NULL,
	[AITypeId] [numeric](10, 0) NULL,
	[DateCreated] [datetime] NULL,
	[DateModified] [datetime] NULL,
	[Birthdate] [datetime] NULL,
	[Fullname] [nvarchar](255) NULL,
	[EmailAddress] [nvarchar](255) NULL,
	[IsDisabled] [bit] NULL
)
IF OBJECT_ID('AIMember') IS NOT NULL
    PRINT '<<< CREATED TABLE AIMember >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE AIMember >>>'
GO

ALTER TABLE [Member] ADD [DefaultHandle] nvarchar(255) NULL;
GO

INSERT [dbo].[AIType] ([AITypeId], [DateCreated], [Code]) VALUES (1, GETDATE(), 'PENPAL')
GO

/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		07/28/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'OutgoingEmail')
BEGIN
		PRINT 'Dropping Table OutgoingEmail'
		DROP  Table OutgoingEmail
END
GO
CREATE TABLE [OutgoingEmail](
	[OutgoingEmailId] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[IncomingEmailId] [numeric](10, 0) NOT NULL,
	[MemberId] [numeric](10, 0) NOT NULL,
	[DateCreated] [datetime] NULL,
	[SourceRawData] [text] NULL,
	[Subject] [text] NULL,
	[Request] [text] NULL,
	[Response] [text] NULL,
	[ErrorCode] [int] default 0,
	[StatusCode] [nvarchar](255) NULL,
	[MessageID] [nvarchar](255) NULL,
	[SubmittedAt] [nvarchar](255) NULL,
	[FromSourceEmailAddress] [nvarchar](255) NULL,
	[ToSendEmailAddress] [nvarchar](255) NULL,
	[ToEmailReplyEmailAddress] [nvarchar](255) NULL,
	[IsSuccess] [bit] NULL
)
IF OBJECT_ID('OutgoingEmail') IS NOT NULL
    PRINT '<<< CREATED TABLE OutgoingEmail >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE OutgoingEmail >>>'
GO

INSERT INTO [Version] (VersionId, DateCreated, MajorNum, MinorNum,Notes) VALUES (2, GETDATE(), 1, 1,'Initial 1.1');
GO