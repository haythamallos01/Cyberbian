
/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		08/20/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'IncomingSMS')
BEGIN
		PRINT 'Dropping Table IncomingSMS'
		DROP  Table IncomingSMS
END
GO
CREATE TABLE [IncomingSMS](
	[IncomingSMSId] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[MemberId] [numeric](10, 0) NULL,
	[DateCreated] [datetime] NULL,
	[DateModified] [datetime] NULL,
	[DateBeginProcessing] [datetime] NULL,
	[DateEndProcessing] [datetime] NULL,
	[ProcessingDurationInMS] [int] NULL,
	[MsgSource] [nvarchar](255) NULL,
	[IsProcessed] [bit] NULL,
	[IsError] [bit] NULL,
	[ErrorStr] [text] NULL,
	[IncomingRawText] [text] NULL
)
IF OBJECT_ID('IncomingSMS') IS NOT NULL
    PRINT '<<< CREATED TABLE IncomingSMS >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE IncomingSMS >>>'
GO

INSERT INTO [Version] (VersionId, DateCreated, MajorNum, MinorNum,Notes) VALUES (6, GETDATE(), 1, 5,'Initial 1.5');
GO