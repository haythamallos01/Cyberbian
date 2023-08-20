
ALTER TABLE [AIMember] ADD [PersonaPrompt] text NULL;
GO
ALTER TABLE [AIMember] ADD [UserQuestionPrompt] text NULL;
GO

UPDATE [AIMember] SET [PersonaPrompt] = 'Assume the following set of email messages in chronological order that were sent and received by the user.  The user sent the message and you replied to the message.  In the email messages format below, your reply text starts with " > ".  The other text is what the user emailed you.  Each complete email message is delimited by triple quotes.  When I ask you to write something, you will reply with text that contains a kind reply to the most recent text the user wrote.  Be spontaneous and elaborate.  Keep your reaponse under 300 words.  Make sure to vary the reply from previous messages.  In your reply, do not include anything about scheduling a meeting or chatting one-on-one.';
GO

UPDATE [AIMember] SET [UserQuestionPrompt] = 'Write a kind reply to the message or question?  Only provide the email Body Text.';
GO

INSERT INTO [Version] (VersionId, DateCreated, MajorNum, MinorNum,Notes) VALUES (3, GETDATE(), 1, 2,'Initial 1.2');
GO