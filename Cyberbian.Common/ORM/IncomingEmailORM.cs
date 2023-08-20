using CyberbianSite.Shared;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Cyberbian.Common.ORM
{
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
              ,[IncomingTo]
              ,[OutgoingFrom]
              ,[OutgoingTo]
              ,[OutgoingReplyTo]
              ,[OutgoingSubject]
              ,[OutgoingTextBody]
              ,[OutgoingHtmlBody]
              ,[OutgoingMessageStream]
              ,[HandleId]
              ,[IncomingFromName]
              ,[IncomingFrom]
              ,[IncomingSubject]
              ,[MemberId]
          FROM [IncomingEmail]";

        public static readonly string INSERT_SQL =
           @"INSERT INTO [IncomingEmail]
           (DateCreated
           ,IsTest
           ,MsgSource
           ,RawData
           ,IncomingTo
           ,OutgoingFrom
           ,OutgoingTo
           ,OutgoingReplyTo
           ,OutgoingSubject
           ,OutgoingTextBody
           ,OutgoingHtmlBody
           ,OutgoingMessageStream
           ,HandleId
           ,IncomingFromName
           ,IncomingFrom
           ,IncomingSubject
           ,MemberId
        )
     VALUES
           (@DateCreated
           ,@IsTest
           ,@MsgSource
           ,@RawData
           ,@IncomingTo
           ,@OutgoingFrom
           ,@OutgoingTo
           ,@OutgoingReplyTo
           ,@OutgoingSubject
           ,@OutgoingTextBody
           ,@OutgoingHtmlBody
           ,@OutgoingMessageStream
           ,@HandleId
           ,@IncomingFromName
           ,@IncomingFrom
           ,@IncomingSubject
           ,@MemberId
            ) 
            SELECT CAST(SCOPE_IDENTITY() as numeric(10))";

        public static readonly string UPDATE_OUTGOING_SQL =
           @"
            UPDATE [IncomingEmail] SET 
            [DateModified]=@DateModified, 
            [DateBeginProcessing]=@DateBeginProcessing, 
            [DateEndProcessing]=@DateEndProcessing, 
            [ProcessingDurationInMS]=@ProcessingDurationInMS, 
            [IsProcessed]=@IsProcessed, 
            [OutgoingFrom]=@OutgoingFrom, 
            [OutgoingTo]=@OutgoingTo, 
            [OutgoingReplyTo]=@OutgoingReplyTo, 
            [OutgoingSubject]=@OutgoingSubject, 
            [OutgoingTextBody]=@OutgoingTextBody, 
            [OutgoingHtmlBody]=@OutgoingHtmlBody, 
            [OutgoingMessageStream]=@OutgoingMessageStream 
            ";

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

        public List<IncomingEmail> GetUnprocessed()
        {
            List<IncomingEmail>? ret = null;
            var sql = SELECT_SQL + " WHERE (IsProcessed=0 or IsProcessed is null) and (IsError = 0 or isError is null)";
            using (var connection = new SqlConnection(_connectionString))
            {
                ret = connection.Query<IncomingEmail>(sql).ToList();
            }
            return ret;
        }

        public List<IncomingEmail> GetSameSubjectListForMember(long memberId, string subjectText)
        {
            List<IncomingEmail>? ret = null;
            var sql = SELECT_SQL + " WHERE (MemberId = @MemberId) and (IncomingSubject like CONCAT('%', @IncomingSubject, '%'))";
            using (var connection = new SqlConnection(_connectionString))
            {
                ret = connection.Query<IncomingEmail>(sql, new { MemberId = memberId, IncomingSubject = subjectText }).ToList();
            }
            return ret;
        }

        public void UpdateOutgoingData(IncomingEmail incomingEmail)
        {
            incomingEmail.DateModified = DateTime.UtcNow;
            var sql = UPDATE_OUTGOING_SQL + " WHERE IncomingEmailId = @IncomingEmailId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.ExecuteScalar<int>(sql, incomingEmail);
            }
        }
    }
}
