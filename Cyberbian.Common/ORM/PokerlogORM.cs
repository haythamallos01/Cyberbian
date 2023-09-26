using CyberbianSite.Shared;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Cyberbian.Common.ORM
{
    public class PokerlogORM : ORMBase
    {
        public static readonly string TABLE_NAME = "Pokerlog";
        public static readonly string INSERT_SQL =
           @"INSERT INTO [Pokerlog]
           ([MemberId]
           ,[DateCreated]
           ,[IsNewHand]
           ,[PromptRequest]
           ,[PromptResponse]
            )
     VALUES
           (@MemberId
           ,@DateCreated
           ,@IsNewHand
           ,@PromptRequest
           ,@PromptResponse) SELECT CAST(SCOPE_IDENTITY() as numeric(10))";

        public PokerlogORM(string connString)
        {
            _connectionString = connString;
        }

        public Pokerlog Create(Pokerlog Pokerlog)
        {
            Pokerlog.DateCreated = DateTime.UtcNow;

            using (var connection = new SqlConnection(_connectionString))
            {
                Pokerlog.PokerlogId = connection.ExecuteScalar<long>(INSERT_SQL, Pokerlog);
            }
            return Pokerlog;
        }
    }
}
