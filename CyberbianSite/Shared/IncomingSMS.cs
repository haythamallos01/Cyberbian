namespace CyberbianSite.Shared
{
    public class IncomingSMS
    {
        public long IncomingSMSId { get; set; }
        public long MemberId { get; set; }
        public long AIMemberId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateBeginProcessing { get; set; }
        public DateTime DateEndProcessing { get; set; }
        public long ProcessingDurationInMS { get; set; }
        public string? MsgSource { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsError { get; set; }
        public string? ErrorStr { get; set; }
        public string? IncomingRawText { get; set; }

        public string? EventId { get; set; }
        public string? EventTopic { get; set; }
        public string? EventSubject { get; set; }
        public string? EventDataMessageId { get; set; }
        public string? EventDataFrom { get; set; }
        public string? EventDataTo { get; set; }
        public string? EventDataMessage { get; set; }
        public DateTime? EventDataReceivedTimestamp { get; set; }
        public string? EventEventType { get; set; }
        public string? EventDataVersion { get; set; }
        public string? EventMetadataVersion { get; set; }
        public DateTime? EventEventTime { get; set; }
        public string? OutgoingMessage { get; set; }
        public string? OutgoingTo { get; set; }
        public string? OutgoingFrom { get; set; }
    }
}
