namespace CyberbianSite.Shared
{
    public class IncomingChat
    {
        public long IncomingChatId { get; set; }
        public long MemberId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateBeginProcessing { get; set; }
        public DateTime DateEndProcessing { get; set; }
        public long ProcessingDurationInMS { get; set; }
        public string? MsgSource { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsError { get; set; }
        public string? ErrorStr { get; set; }
        public string? QuestionPrompt { get; set; }
        public string? QuestionPromptResponse { get; set; }
        public string? SessionGuid { get; set; }
        public string? ConversationHistory { get; set; }
    }
}
