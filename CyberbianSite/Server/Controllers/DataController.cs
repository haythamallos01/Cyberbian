using Cyberbian.Common.ORM;
using CyberbianSite.Server.Config;
using CyberbianSite.Shared;
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

        [HttpPost]
        [Route("api/data/aimember")]
        public ActionResult<AIMember> Create([FromBody] AIMember aimember)
        {
            AIMemberORM aiMemberORM = new AIMemberORM(_optionsDatabase.Value.ConnectionString);
            aimember.PersonaPrompt = ConfigStrings.AIMemberDefaultPersonaPrompt;
            aimember.UserQuestionPrompt = ConfigStrings.AIMemberDefaultUserQuestionPrompt;

            aimember = aiMemberORM.Create(aimember);
            if (aimember == null)
            {
                return Unauthorized();
            }
            else
            {
                return aimember;
            }
        }

        [HttpGet("api/data/aimember")]
        public ActionResult<AIMember> GetAIMember(long memberid, long aitypeid)
        {
            AIMemberORM aimemberORM = new AIMemberORM(_optionsDatabase.Value.ConnectionString);
            AIMember aimember = aimemberORM.Get(memberid, aitypeid);
            return aimember;
        }

        [HttpPost]
        [Route("api/data/incomingchat")]
        public ActionResult<IncomingChat> Create([FromBody] IncomingChat incomingchat)
        {
            IncomingChatORM incomingChatORM = new IncomingChatORM(_optionsDatabase.Value.ConnectionString);

            incomingchat = incomingChatORM.Create(incomingchat);
            if (incomingchat == null)
            {
                return Unauthorized();
            }
            else
            {
                return incomingchat;
            }
        }

        [HttpPost]
        [Route("api/data/pokerlog")]
        public ActionResult<Pokerlog> Create([FromBody] Pokerlog pokerlog)
        {
            PokerlogORM pokerlogORM = new PokerlogORM(_optionsDatabase.Value.ConnectionString);
            pokerlog = pokerlogORM.Create(pokerlog);
            if (pokerlog == null)
            {
                return Unauthorized();
            }
            else
            {
                return pokerlog;
            }
        }
    }
}
