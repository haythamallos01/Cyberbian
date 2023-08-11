using Cyberbian.Data.ORM;
using CyberbianSite.Client.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CyberbianSite.Server.Controllers
{
    [ApiController]
    [Authorize]
    public class DataController : ControllerBase
    {
        private readonly IOptions<DatabaseOptions> _optionsDatabase;

        public DataController(IOptions<DatabaseOptions> optionsDatabase)
        {
            _optionsDatabase = optionsDatabase;
        }

        [HttpGet("api/data/member/{userName}")]
        public ActionResult<Member> GetMemberByUserName(string userName)
        {
            MemberORM memberORM = new MemberORM(_optionsDatabase.Value.ConnectionString);
            Member member = memberORM.Get(userName);
            if (member == null)
            {
                return Unauthorized();
            }
            else
            {
                return member;
            }
        }
    }
}
