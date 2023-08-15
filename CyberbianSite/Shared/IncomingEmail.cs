namespace CyberbianSite.Shared
{
    public class IncomingEmail
    {
        public long IncomingEmailId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateBeginProcessing { get; set; }
        public DateTime DateEndProcessing { get; set; }
        public long ProcessingDurationInMS { get; set; }
        public string? MsgSource { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsTest { get; set; }
        public bool IsError { get; set; }
        public string? ErrorStr { get; set; }
        public string? RawData { get; set; }
        public string? IncomingTo { get; set; }
        public string? OutgoingFrom { get; set; }
        public string? OutgoingTo { get; set; }
        public string? OutgoingReplyTo { get; set; }
        public string? OutgoingSubject { get; set; }
        public string? OutgoingTextBody { get; set; }
        public string? OutgoingHtmlBody { get; set; }
        public string? OutgoingMessageStream { get; set; }
        public string? HandleId { get; set; }
        public string? IncomingFromName { get; set; }
        public string? IncomingFrom { get; set; }
        public string? IncomingSubject { get; set; }
        public long MemberId { get; set; }

    }
}
