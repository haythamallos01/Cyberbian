using Dapper;
using System;
using System.Data.SqlClient;

namespace Cyberbian.Data.ORM.Lib
{
    public class IncomingEmail
    {
        public long IncomingEmailId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateBeginProcessing { get; set; }
        public DateTime DateEndProcessing { get; set; }
        public long ProcessingDurationInMS { get; set; }
        public string? MsgSource { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsTest { get; set; }
        public bool IsError { get; set; }
        public string? ErrorStr { get; set; }
        public string? RawData { get; set; }
    }
    public class IncomingEmailORM : ORMBase
    {
        public static readonly string TABLE_NAME = "IncomingEmail";
        public static readonly string SELECT_SQL =
            @"SELECT [IncomingEmailId]
              ,[DateCreated]
              ,[DateModified]
              ,[DateBeginProcessing]
              ,[DateEndProcessing]
              ,[ProcessingDurationInMS]
              ,[MsgSource]
              ,[IsProcessed]
              ,[IsError]
              ,[ErrorStr]
              ,[RawData]
              ,[FromAddress]
              ,[ToAddress]
              ,[Body]
          FROM [IncomingEmail]";
        public static readonly string INSERT_SQL =
           @"INSERT INTO [IncomingEmail]
           (DateCreated
           ,IsTest
           ,MsgSource
           ,RawData)
     VALUES
           (@DateCreated
           ,@IsTest
           ,@MsgSource
           ,@RawData) SELECT CAST(SCOPE_IDENTITY() as numeric(10))";

        public IncomingEmailORM(string connString)
        {
            _connectionString = connString;
        }

        public IncomingEmail Create(IncomingEmail incomingEmail)
        {
            incomingEmail.DateCreated = DateTime.UtcNow;

            using (var connection = new SqlConnection(_connectionString))
            {
                incomingEmail.IncomingEmailId = connection.ExecuteScalar<long>(INSERT_SQL, incomingEmail);
            }
            return incomingEmail;
        }
    }
}
