using Dapper;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Cyberbian.Data.ORM.Lib
{
    public class Member
    {
        public long MemberId { get; set; }
        public long MemberRoleId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        public string? PasswordEncrypted { get; set; }
        public string? PictureUrl { get; set; }
        public byte[] PictureData { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string? DefaultHandle { get; set; }

    }

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
    }
}
