using Cyberbian.Data.ORM.Lib;
using CyberbianSite.Client.Config;
using Microsoft.Extensions.Options;
using Cyberbian.Data.ORM.Lib;

namespace CyberbianSite.Client.Services
{
    public class DbService
    {
        private readonly IOptions<DatabaseOptions> _options;

        public DbService(IOptions<DatabaseOptions> options)
        {
            _options = options;
        }

        public Member GetMember(string username)
        {
            MemberORM memberORM = new MemberORM(_options.Value.ConnectionString);
            Member member = memberORM.Get(username);
            return member;
        }
    }
}
