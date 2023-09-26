namespace CyberbianSite.Shared
{
    public class Pokerlog
    {
        public long PokerlogId { get; set; }
        public long MemberId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsNewHand { get; set; }
        public string PromptRequest { get; set; }
        public string PromptResponse { get; set; }
    }
}
