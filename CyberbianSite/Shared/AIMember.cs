using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberbianSite.Shared
{
    public class AIMember
    {
        public long AIMemberId { get; set; }
        public long MemberId { get; set; }
        public long AITypeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime Birthdate { get; set; }
        public string? EmailAddress { get; set; }
        public string? Seed { get; set; }
        public string? GeneratedSeed { get; set; }
        public string? BirthCertificateContent { get; set; }
        public bool? IsDisabled { get; set; }
        public string? PersonaPrompt { get; set; }
        public string? UserQuestionPrompt { get; set; }
        public string? AliasName { get; set; }
        public string? Gender { get; set; }
    }
}
