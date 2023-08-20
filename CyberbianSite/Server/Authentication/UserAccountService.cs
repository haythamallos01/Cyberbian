using Cyberbian.Common;
using Microsoft.Extensions.Options;
using Cyberbian.Common.ORM;
using CyberbianSite.Server.Config;

namespace CyberbianSite.Server.Authentication
{
    public class UserAccountService
    {
        //private List<UserAccount> _userAccountList;
        private readonly IOptions<DatabaseOptions> _optionsDatabase;

        public UserAccountService(IOptions<DatabaseOptions> optionsDatabase) {
            _optionsDatabase = optionsDatabase;
            //_userAccountList = new List<UserAccount>()
            //{
            //    new UserAccount {UserName="admin", Password="admin", Role="Administrator" },
            //    new UserAccount {UserName="user", Password="user", Role="User" }
            //};
        }

        public UserAccount? GetUserAccountByUserName(string userName)
        {
            UserAccount? userAccount = null;

            MemberORM memberORM = new MemberORM(_optionsDatabase.Value.ConnectionString);
            Member member = memberORM.Get(userName);
            if (member != null)
            {
                string role = "User";
                if (member.MemberRoleId == 2)
                {
                    role = "Administrator";
                }
                userAccount = new UserAccount { UserName = member.Username, Password = member.PasswordEncrypted, Role = role };
            }
            return userAccount;
            //return _userAccountList.FirstOrDefault(x => x.UserName == userName);
        }

        public Member? GetMemberByUserName(string userName)
        {
            MemberORM memberORM = new MemberORM(_optionsDatabase.Value.ConnectionString);
            Member member = memberORM.Get(userName);
            return member;
        }

        public Member CreateUserAccount(UserRegister userRegister)
        {
            Member member = new Member
            {
                Username = userRegister.Email,
                PasswordEncrypted = SecurityHelper.Encrypt(userRegister.Password),
                MemberRoleId = 2,
                FirstName = userRegister.FirstName,
                LastName = userRegister.LastName
            };
            MemberORM memberORM = new MemberORM(_optionsDatabase.Value.ConnectionString);
            member.DefaultHandle = StringHelper.RandomString(9);
            member = memberORM.Create(member);
            return member;
        }
    }
}
