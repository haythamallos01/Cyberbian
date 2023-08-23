
ALTER TABLE [AIMember] ADD [PhoneNumberSMSE163] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventId] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventTopic] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventSubject] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventDataMessageId] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventDataFrom] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventDataTo] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventDataMessage] text NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventDataReceivedTimestamp] datetime NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventEventType] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventDataVersion] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventMetadataVersion] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [EventEventTime] datetime NULL;
GO
ALTER TABLE [IncomingSMS] ADD [OutgoingMessage] text NULL;
GO
ALTER TABLE [IncomingSMS] ADD [OutgoingFrom] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [OutgoingTo] nvarchar(255) NULL;
GO
ALTER TABLE [IncomingSMS] ADD [AIMemberId] [numeric](10, 0) NULL;
GO

INSERT INTO [Version] (VersionId, DateCreated, MajorNum, MinorNum,Notes) VALUES (7, GETDATE(), 1, 6,'Initial 1.6');
GO