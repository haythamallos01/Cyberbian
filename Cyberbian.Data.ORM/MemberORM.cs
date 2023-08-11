using CyberbianSite.Shared;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Cyberbian.Data.ORM
{

    public class MemberORM : ORMBase
    {
        public static readonly string TABLE_NAME = "Member";
        public static readonly string SELECT_SQL =
            @"SELECT [MemberId]
          ,[MemberRoleId]
          ,[DateCreated]
          ,[DateModified]
          ,[FirstName]
          ,[MiddleName]
          ,[LastName]
          ,[Username]
          ,[PasswordEncrypted]
          ,[PictureUrl]
          ,[IsDisabled]
          ,[LastLoginDate]
          ,[DefaultHandle]
          FROM [Member]";

        public static readonly string INSERT_SQL =
            @"INSERT INTO [Member]
           ([MemberRoleId]
           ,[DateCreated]
           ,[FirstName]
           ,[MiddleName]
           ,[LastName]
           ,[Username]
           ,[PasswordEncrypted]
           ,[PictureUrl]
           ,[IsDisabled]
           ,[DefaultHandle]
           )
     VALUES
           (@MemberRoleId
           ,@DateCreated
           ,@FirstName
           ,@MiddleName
           ,@LastName
           ,@Username
           ,@PasswordEncrypted
           ,@PictureUrl
           ,@IsDisabled
           ,@DefaultHandle
            ) SELECT CAST(SCOPE_IDENTITY() as numeric(10))";

        public MemberORM(string connString)
        {
            _connectionString = connString;
        }

        public Member Create(Member member)
        {
            member.DateCreated = DateTime.UtcNow;

            using (var connection = new SqlConnection(_connectionString))
            {
                member.MemberId = connection.ExecuteScalar<long>(INSERT_SQL, member);
            }
            return member;
        }

        public Member Get(string username, string passwordencrypted)
        { 
            Member? member = null;
            var sql = SELECT_SQL + " WHERE Username=@Username and PasswordEncrypted=@PasswordEncrypted";
            using (var connection = new SqlConnection(_connectionString))
            {
                member = connection.Query<Member>(sql, new { Username = username, PasswordEncrypted = passwordencrypted }).FirstOrDefault();
            }
            return member;
        }

        public Member Get(string username)
        {
            Member? member = null;
            var sql = SELECT_SQL + " WHERE Username=@Username";
            using (var connection = new SqlConnection(_connectionString))
            {
                member = connection.Query<Member>(sql, new { Username = username }).FirstOrDefault();
            }
            return member;
        }

        public Member GetByHandleId(string handleId)
        {
            Member? member = null;
            var sql = SELECT_SQL + " WHERE DefaultHandle=@handleId";
            using (var connection = new SqlConnection(_connectionString))
            {
                member = connection.Query<Member>(sql, new { handleId = handleId }).FirstOrDefault();
            }
            return member;
        }
    }
}
