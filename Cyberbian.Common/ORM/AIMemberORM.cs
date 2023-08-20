

using CyberbianSite.Shared;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Cyberbian.Common.ORM
{
    public class AIMemberORM : ORMBase
    {
        public static readonly string TABLE_NAME = "AIMember";

        private enum Gender
        {
            MALE,
            FEMALE,
            BINARY
        }

        public static readonly string SELECT_SQL =
     @"SELECT [AIMemberId]
      ,[MemberId]
      ,[AITypeId]
      ,[DateCreated]
      ,[DateModified]
      ,[Birthdate]
      ,[EmailAddress]
      ,[Seed]
      ,[GeneratedSeed]
      ,[BirthCertificateContent]
      ,[IsDisabled]
      ,[PersonaPrompt]
      ,[UserQuestionPrompt]
      ,[AliasName]
      ,[Gender]
        FROM [AIMember]";

        public static readonly string INSERT_SQL =
      @"INSERT INTO [AIMember]
           (
           [MemberId]
           ,[AITypeId]
           ,[DateCreated]
           ,[Birthdate]
           ,[EmailAddress]
           ,[Seed]
           ,[GeneratedSeed]
           ,[BirthCertificateContent]
           ,[IsDisabled]
           ,[PersonaPrompt]
           ,[UserQuestionPrompt]
           ,[AliasName]
           ,[Gender]
          )
     VALUES
           (@MemberId
           ,@AITypeId
           ,@DateCreated
           ,@Birthdate
           ,@EmailAddress
           ,@Seed
           ,@GeneratedSeed
           ,@BirthCertificateContent
           ,@IsDisabled
           ,@PersonaPrompt
           ,@UserQuestionPrompt
           ,@AliasName
           ,@Gender
            ) SELECT CAST(SCOPE_IDENTITY() as numeric(10))";

        public AIMemberORM(string connString)
        {
            _connectionString = connString;
        }
        public AIMember Create(AIMember aimember)
        {
            aimember.DateCreated = DateTime.UtcNow;
            if (string.IsNullOrEmpty(aimember.AliasName))
            {
                aimember.AliasName = "Bert";
            }
            if (string.IsNullOrEmpty(aimember.Gender))
            {
                aimember.AliasName = Gender.MALE.ToString();
            }
            using (var connection = new SqlConnection(_connectionString))
            {
                aimember.AIMemberId = connection.ExecuteScalar<long>(INSERT_SQL, aimember);
            }
            return aimember;
        }

        public AIMember Get(long aimemberId)
        {
            AIMember? aimember = null;
            var sql = SELECT_SQL + " WHERE AIMemberId=@AIMemberId";
            using (var connection = new SqlConnection(_connectionString))
            {
                aimember = connection.Query<AIMember>(sql, new { AIMemberId = aimemberId }).FirstOrDefault();
            }
            return aimember;
        }

        public AIMember Get(long aimemberId, long aitypeid)
        {
            AIMember? aimember = null;
            var sql = SELECT_SQL + " WHERE AIMemberId=@AIMemberId and AITypeId=@AITypeId";
            using (var connection = new SqlConnection(_connectionString))
            {
                aimember = connection.Query<AIMember>(sql, new { AIMemberId = aimemberId, AITypeId = aitypeid }).FirstOrDefault();
            }
            return aimember;
        }

    }
}
