

namespace Cyberbian.Common.OpenAI
{
    public class OpenAIClient
    {
        private string _userQuestion = string.Empty;
        private readonly List<Message> _conversationHistory = new();
        private bool _isSendingMessage;
        private string _chatBotKnowledgeScope;

        private OpenAIService _openAIService;

        public OpenAIClient() { 
            _openAIService = new OpenAIService(new HttpClient());
        }

        public void ClearInput() => _userQuestion = string.Empty;

        public void ClearConversation()
        {
            ClearInput();
            _conversationHistory.Clear();
        }

        public void SetKnowledgeScope(string chatBotKnowledgeScope)
        {
            _chatBotKnowledgeScope = chatBotKnowledgeScope;
            _conversationHistory.Add(new Message { role = "system", content = _chatBotKnowledgeScope });
        }

        public void SetUserQuestion(string userQuestion)
        {
            _userQuestion = userQuestion;
            AddUserQuestionToConversation();
        }

        public async Task CreateCompletion()
        {
            _isSendingMessage = true;
            var assistantResponse = await _openAIService.CreateChatCompletion(_conversationHistory);
            _conversationHistory.Add(assistantResponse);
            _isSendingMessage = false;
        }

        public void AddUserQuestionToConversation()
     => _conversationHistory.Add(new Message { role = "user", content = _userQuestion });

        public async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(_userQuestion)) return;
            AddUserQuestionToConversation();
            await CreateCompletion();
            ClearInput();
        }

        public List<Message> Messages => _conversationHistory.Where(c => c.role is not "system").ToList();

    }
}
