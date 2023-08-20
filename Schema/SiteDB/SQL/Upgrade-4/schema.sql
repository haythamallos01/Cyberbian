
/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		08/20/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'IncomingChat')
BEGIN
		PRINT 'Dropping Table IncomingChat'
		DROP  Table IncomingChat
END
GO
CREATE TABLE [IncomingChat](
	[IncomingChatId] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[MemberId] [numeric](10, 0) NULL,
	[DateCreated] [datetime] NULL,
	[DateModified] [datetime] NULL,
	[DateBeginProcessing] [datetime] NULL,
	[DateEndProcessing] [datetime] NULL,
	[ProcessingDurationInMS] [int] NULL,
	[SessionGuid] [nvarchar](255) NULL,
	[MsgSource] [nvarchar](255) NULL,
	[IsProcessed] [bit] NULL,
	[IsError] [bit] NULL,
	[ErrorStr] [text] NULL,
	[ConversationHistory] [text] NULL,
	[QuestionPrompt] [text] NULL,
	[QuestionPromptResponse] [text] NULL

)
IF OBJECT_ID('IncomingChat') IS NOT NULL
    PRINT '<<< CREATED TABLE IncomingChat >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE IncomingChat >>>'
GO

INSERT INTO [Version] (VersionId, DateCreated, MajorNum, MinorNum,Notes) VALUES (5, GETDATE(), 1, 4,'Initial 1.4');
GO