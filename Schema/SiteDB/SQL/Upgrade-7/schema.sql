
/*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		09/25/23		HA		Created
*******************************************************************************/
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Pokerlog')
BEGIN
		PRINT 'Dropping Table Pokerlog'
		DROP  Table Pokerlog
END
GO
CREATE TABLE [Pokerlog](
	[PokerlogId] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[MemberId] [numeric](10, 0) NULL,
	[DateCreated] [datetime] NULL,
	[IsNewHand] [bit] NULL,
	[PromptRequest] [text] NULL,
	[PromptResponse] [text] NULL
)
IF OBJECT_ID('Pokerlog') IS NOT NULL
    PRINT '<<< CREATED TABLE Pokerlog >>>'
ELSE
    PRINT '<<< FAILED CREATING TABLE Pokerlog >>>'
GO

INSERT INTO [Version] (VersionId, DateCreated, MajorNum, MinorNum,Notes) VALUES (8, GETDATE(), 1, 7,'Initial 1.7');
GO