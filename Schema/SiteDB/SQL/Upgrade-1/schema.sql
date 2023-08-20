
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
	[EmailAddress] [nvarchar](255) NULL,
	[Seed] text NULL,
	[GeneratedSeed] text NULL,
	[BirthCertificateContent] text NULL,
	[IsDisabled] [bit] NULL
)
IF OBJECT_ID('AIMember') IS NOT NULL
    PRINT '<<< CREATED TABLE AIMember >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE AIMember >>>'
GO

ALTER TABLE [Member] ADD [DefaultHandle] nvarchar(255) NULL;
GO

ALTER TABLE [IncomingEmail] ADD [IncomingTo] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingEmail] ADD [OutgoingFrom] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingEmail] ADD [OutgoingTo] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingEmail] ADD [OutgoingReplyTo] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingEmail] ADD [OutgoingSubject] text NULL;
GO
ALTER TABLE [IncomingEmail] ADD [OutgoingTextBody] text NULL;
GO
ALTER TABLE [IncomingEmail] ADD [OutgoingHtmlBody] text NULL;
GO
ALTER TABLE [IncomingEmail] ADD [OutgoingMessageStream] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingEmail] ADD [HandleId] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingEmail] ADD [IncomingFromName] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingEmail] ADD [IncomingFrom] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingEmail] ADD [IncomingSubject] text NULL;
GO
ALTER TABLE [IncomingEmail] ADD [MemberId] numeric (10, 0) NULL;
GO

INSERT [dbo].[AIType] ([AITypeId], [DateCreated], [Code]) VALUES (1, GETDATE(), 'PAL')
GO


INSERT INTO [Version] (VersionId, DateCreated, MajorNum, MinorNum,Notes) VALUES (2, GETDATE(), 1, 1,'Initial 1.1');
GO