using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberbianSite.Shared
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
}
