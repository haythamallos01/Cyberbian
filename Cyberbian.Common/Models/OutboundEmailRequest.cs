

namespace Cyberbian.Common.Models
{
    public class OutboundEmailRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public string MessageStream { get; set; }
    }
}
