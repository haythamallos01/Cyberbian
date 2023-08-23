using CyberbianSite.Shared;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Cyberbian.Common.ORM
{
    public class IncomingSMSORM : ORMBase
    {
        public static readonly string TABLE_NAME = "IncomingSMS";
        public static readonly string SELECT_SQL =
            @"SELECT [IncomingSMSId]
              ,[MemberId]
             ,[DateCreated]
              ,[DateModified]
              ,[DateBeginProcessing]
              ,[DateEndProcessing]
              ,[ProcessingDurationInMS]
              ,[MsgSource]
              ,[IsProcessed]
              ,[IsError]
              ,[ErrorStr]
              ,[IncomingRawText]
          FROM [IncomingSMS]";

        public static readonly string INSERT_SQL =
           @"INSERT INTO [IncomingSMS]
           (
           DateCreated
           ,MemberId
           ,MsgSource
           ,IncomingRawText
        )
     VALUES
           (
           @DateCreated
           ,@MemberId
           ,@MsgSource
           ,@IncomingRawText
            ) 
            SELECT CAST(SCOPE_IDENTITY() as numeric(10))";

        public static readonly string UPDATE_OUTGOING_SQL =
    @"
            UPDATE [IncomingSMS] SET 
            [MemberId]=@MemberId, 
            [AIMemberId]=@AIMemberId, 
            [DateModified]=@DateModified, 
            [DateBeginProcessing]=@DateBeginProcessing, 
            [DateEndProcessing]=@DateEndProcessing, 
            [ProcessingDurationInMS]=@ProcessingDurationInMS, 
            [IsProcessed]=@IsProcessed, 
            [EventId]=@EventId, 
            [EventTopic]=@EventTopic, 
            [EventSubject]=@EventSubject, 
            [EventDataMessageId]=@EventDataMessageId, 
            [EventDataFrom]=@EventDataFrom, 
            [EventDataTo]=@EventDataTo, 
            [EventDataMessage]=@EventDataMessage, 
            [EventDataReceivedTimestamp]=@EventDataReceivedTimestamp, 
            [EventEventType]=@EventEventType, 
            [EventDataVersion]=@EventDataVersion, 
            [EventMetadataVersion]=@EventMetadataVersion, 
            [EventEventTime]=@EventEventTime, 
            [OutgoingMessage]=@OutgoingMessage, 
            [OutgoingFrom]=@OutgoingFrom, 
            [OutgoingTo]=@OutgoingTo  
            ";

        public IncomingSMSORM(string connString)
        {
            _connectionString = connString;
        }

        public IncomingSMS Create(IncomingSMS incomingSMS)
        {
            incomingSMS.DateCreated = DateTime.UtcNow;

            using (var connection = new SqlConnection(_connectionString))
            {
                incomingSMS.IncomingSMSId = connection.ExecuteScalar<long>(INSERT_SQL, incomingSMS);
            }
            return incomingSMS;
        }

        public List<IncomingSMS> GetUnprocessed()
        {
            List<IncomingSMS>? ret = null;
            var sql = SELECT_SQL + " WHERE (IsProcessed=0 or IsProcessed is null) and (IsError = 0 or isError is null)";
            using (var connection = new SqlConnection(_connectionString))
            {
                ret = connection.Query<IncomingSMS>(sql).ToList();
            }
            return ret;
        }

        public List<IncomingSMS> GetSameSubjectListForMember(long memberId)
        {
            List<IncomingSMS>? ret = null;
            var sql = SELECT_SQL + " WHERE (MemberId = @MemberId) ";
            using (var connection = new SqlConnection(_connectionString))
            {
                ret = connection.Query<IncomingSMS>(sql, new { MemberId = memberId}).ToList();
            }
            return ret;
        }

        public void UpdateOutgoingData(IncomingSMS incomingSMS)
        {
            incomingSMS.DateModified = DateTime.UtcNow;
            var sql = UPDATE_OUTGOING_SQL + " WHERE IncomingSMSId = @IncomingSMSId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.ExecuteScalar<int>(sql, incomingSMS);
            }
        }
    }
}
