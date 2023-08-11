namespace CyberbianSite.Shared
{
    public class Syslog
    {
        public long SyslogId { get; set; }
        public DateTime DateCreated { get; set; }
        public string MsgSource { get; set; }
        public string Payload { get; set; }
        public string MsgText { get; set; }
    }
}
