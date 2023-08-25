

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
      ,[PhoneNumberSMSE163]
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
           ,[PhoneNumberSMSE163]
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
           ,@PhoneNumberSMSE163
            ) SELECT CAST(SCOPE_IDENTITY() as numeric(10))";

        public static readonly string UPDATE_OUTGOING_SQL =
   @"
            UPDATE [AIMember] SET 
            [DateModified]=@DateModified, 
            [PhoneNumberSMSE163]=@PhoneNumberSMSE163
            ";
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
                aimember.Gender = Gender.MALE.ToString();
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

        public AIMember Get(long memberId, long aitypeid)
        {
            AIMember? aimember = null;
            var sql = SELECT_SQL + " WHERE MemberId=@MemberId and AITypeId=@AITypeId";
            using (var connection = new SqlConnection(_connectionString))
            {
                aimember = connection.Query<AIMember>(sql, new { MemberId = memberId, AITypeId = aitypeid }).FirstOrDefault();
            }
            return aimember;
        }

        public AIMember GetByPhoneNumber(string phoneNumber)
        {
            AIMember? aimember = null;
            var sql = SELECT_SQL + " WHERE PhoneNumberSMSE163=@PhoneNumberSMSE163";
            using (var connection = new SqlConnection(_connectionString))
            {
                aimember = connection.Query<AIMember>(sql, new { PhoneNumberSMSE163 = phoneNumber }).FirstOrDefault();
            }
            return aimember;
        }

        public void UpdatePhoneNumber(long memberId, long aiTypeId, string phoneNumber)
        {
            DateTime DateModified = DateTime.UtcNow;
            var sql = UPDATE_OUTGOING_SQL + " WHERE AIMemberId=@AIMemberId and AITypeId=@AITypeId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.ExecuteScalar<int>(sql, new {DateModified = DateModified, PhoneNumberSMSE163 = phoneNumber, AIMemberId = memberId, AITypeId = aiTypeId });
            }
        }

    }
}
