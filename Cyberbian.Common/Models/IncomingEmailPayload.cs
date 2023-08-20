namespace Cyberbian.Common.Models
{

    public class IncomingEmailPayload
    {
        public string? FromName { get; set; }
        public string? MessageStream { get; set; }
        public string? From { get; set; }
        public Fromfull FromFull { get; set; }
        public string? To { get; set; }
        public Tofull[] ToFull { get; set; }
        public string? Cc { get; set; }
        public object[] CcFull { get; set; }
        public string? Bcc { get; set; }
        public object[] BccFull { get; set; }
        public string? OriginalRecipient { get; set; }
        public string? Subject { get; set; }
        public string? MessageID { get; set; }
        public string? ReplyTo { get; set; }
        public string? MailboxHash { get; set; }
        public string? Date { get; set; }
        public string? TextBody { get; set; }
        public string? HtmlBody { get; set; }
        public string? StrippedTextReply { get; set; }
        public string? Tag { get; set; }
        public Header[] Headers { get; set; }
        public object[] Attachments { get; set; }
    }

    public class Fromfull
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? MailboxHash { get; set; }
    }

    public class Tofull
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? MailboxHash { get; set; }
    }

    public class Header
    {
        public string? Name { get; set; }
        public string? Value { get; set; }
    }

}
