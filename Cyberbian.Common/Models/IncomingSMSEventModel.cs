


namespace Cyberbian.Common.Models
{
    public class IncomingSMSEventModel
    {
        public string id { get; set; }
        public string topic { get; set; }
        public string subject { get; set; }
        public Data data { get; set; }
        public string eventType { get; set; }
        public string dataVersion { get; set; }
        public string metadataVersion { get; set; }
        public DateTime eventTime { get; set; }
    }

    public class Data
    {
        public string messageId { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string message { get; set; }
        public DateTime receivedTimestamp { get; set; }
    }
}
