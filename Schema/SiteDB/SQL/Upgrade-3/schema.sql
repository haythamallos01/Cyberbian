
ALTER TABLE [AIMember] ADD [AliasName] [nvarchar](255) NULL;
GO

ALTER TABLE [AIMember] ADD [Gender] [nvarchar](255) NULL;
GO

UPDATE [AIMember] SET [Gender] = 'MALE';
GO

UPDATE [AIMember] SET [AliasName] = 'Bert';
GO

INSERT INTO [Version] (VersionId, DateCreated, MajorNum, MinorNum,Notes) VALUES (4, GETDATE(), 1, 3,'Initial 1.3');
GO