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

    }
}
