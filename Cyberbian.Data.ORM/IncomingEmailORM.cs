using CyberbianSite.Shared;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Cyberbian.Data.ORM
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
    }
}
