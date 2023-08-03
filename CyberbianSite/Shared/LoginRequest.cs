using System.ComponentModel.DataAnnotations;

namespace CyberbianSite.Shared
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string UserName { get; set; }
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
