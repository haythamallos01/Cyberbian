using CyberbianSite.Shared;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Cyberbian.Common.ORM
{
    public class IncomingChatORM : ORMBase
    {
        public static readonly string TABLE_NAME = "IncomingChat";
        public static readonly string SELECT_SQL =
            @"SELECT [IncomingChatId]
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
              ,[QuestionPrompt]
              ,[QuestionPromptResponse]
              ,[SessionGuid]
              ,[ConversationHistory]
          FROM [IncomingChat]";

        public static readonly string INSERT_SQL =
           @"INSERT INTO [IncomingChat]
           (
           DateCreated
           ,MemberId
           ,MsgSource
           ,QuestionPrompt
           ,QuestionPromptResponse
           ,SessionGuid
           ,ConversationHistory
           ,DateBeginProcessing
           ,DateEndProcessing
           ,ProcessingDurationInMS
           ,IsProcessed
        )
     VALUES
           (
           @DateCreated
           ,@MemberId
           ,@MsgSource
           ,@QuestionPrompt
           ,@QuestionPromptResponse
           ,@SessionGuid
           ,@ConversationHistory
           ,@DateBeginProcessing
           ,@DateEndProcessing
           ,@ProcessingDurationInMS
           ,@IsProcessed
            ) 
            SELECT CAST(SCOPE_IDENTITY() as numeric(10))";

        public IncomingChatORM(string connString)
        {
            _connectionString = connString;
        }

        public IncomingChat Create(IncomingChat incomingChat)
        {
            incomingChat.DateCreated = DateTime.UtcNow;

            using (var connection = new SqlConnection(_connectionString))
            {
                incomingChat.IncomingChatId = connection.ExecuteScalar<long>(INSERT_SQL, incomingChat);
            }
            return incomingChat;
        }

        public List<IncomingChat> GetSameSubjectListForMember(long memberId)
        {
            List<IncomingChat>? ret = null;
            var sql = SELECT_SQL + " WHERE (MemberId = @MemberId) ";
            using (var connection = new SqlConnection(_connectionString))
            {
                ret = connection.Query<IncomingChat>(sql, new { MemberId = memberId}).ToList();
            }
            return ret;
        }
    }
}
