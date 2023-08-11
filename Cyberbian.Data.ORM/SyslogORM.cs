using CyberbianSite.Shared;
using Dapper;
using Microsoft.Data.SqlClient;
namespace Cyberbian.Data.ORM
{
    public class SyslogORM : ORMBase
    {
        public static readonly string TABLE_NAME = "Syslog";
        public static readonly string INSERT_SQL =
           @"INSERT INTO [Syslog]
           ([DateCreated]
           ,[MsgSource]
           ,[Payload]
           ,[MsgText])
     VALUES
           (@DateCreated
           ,@MsgSource
           ,@Payload
           ,@MsgText) SELECT CAST(SCOPE_IDENTITY() as numeric(10))";

        public SyslogORM(string connString)
        {
            _connectionString = connString;
        }

        public Syslog Create(Syslog syslog)
        {
            syslog.DateCreated = DateTime.UtcNow;

            using (var connection = new SqlConnection(_connectionString))
            {
                syslog.SyslogId = connection.ExecuteScalar<long>(INSERT_SQL, syslog);
            }
            return syslog;
        }
    }
}
