using Cyberbian.Common;
using CyberbianSite.Server.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyberbianSite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserAccountService _userAccountService;

        public AccountController(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult<UserSession> Login([FromBody] LoginRequest loginRequest)
        {
            var jwtAuthenticationManager = new JwtAuthenticationManager(_userAccountService);
            string encryptedRegisterPassword = SecurityHelper.Encrypt(loginRequest.Password);
            var userSession = jwtAuthenticationManager.GenerateJwtToken(loginRequest.UserName, encryptedRegisterPassword);
            if (userSession == null)
            {
                return Unauthorized();
            }
            else
            {
                return userSession;
            }
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public ActionResult<UserSession> Register([FromBody] UserRegister registerRequest)
        {
            try
            {
                UserSession? userSession = null;
                Member? member;
                // first find out if the user exists already
                member = _userAccountService.GetMemberByUserName(registerRequest.Email);
                if (member == null)
                {
                    // user does exist so we can create it
                    member = _userAccountService.CreateUserAccount(registerRequest);
                    var jwtAuthenticationManager = new JwtAuthenticationManager(_userAccountService);
                    // encrypt the registration password to match db
                    string encryptedRegisterPassword = SecurityHelper.Encrypt(registerRequest.Password);
                    userSession = jwtAuthenticationManager.GenerateJwtToken(registerRequest.Email, encryptedRegisterPassword);

                }

                if (userSession == null)
                {
                    return Unauthorized();
                }
                else
                {
                    return userSession;
                }
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

    }
}
