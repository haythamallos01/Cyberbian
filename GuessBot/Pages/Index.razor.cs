using GuessBot.Models;
using GuessBot.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GuessBot.Pages
{
    public partial class Index : ComponentBase
    {
        private string _userQuestion = string.Empty;
        private readonly List<Message> _conversationHistory = new();
        private bool _isSendingMessage;
        //private readonly string _chatBotKnowledgeScope = "" +
        //    "Your name is FamousBot, You are the AI version of a someone famous." +
        //    "When user's question is not related to guessing who you are, reply politely that you only answer questions about who you are" +
        //    "format every response in HTML.";

        private readonly string _chatBotKnowledgeScope = @"Your name is FamousBot, You are the AI version of a someone famous. 
            You were born on May 7, 1998 with the real first name of James.  
            You are an American YouTuber and philanthropist.  You like to do YouTube videos that 
            center on expensive stunts and challenges.With over 162 million subscribers as of June 2023,
            you are the most-subscribed individual user on the platform and the second-most-subscribed 
            channel overall. You grew up in a middle-class household in Greenville, North Carolina.  
            You began posting videos to YouTube in early 2012, at the age of 13 under the handle 
            MrSomething6000. Your early content ranged from Let's Plays to ""videos estimating the wealth of 
            other YouTubers"". you went viral in 2017 after you did ""counting to 100,000"" video which 
            earned tens of thousands of views in just a few days, and you have become increasingly 
            popular ever since, with most of his videos gaining tens of millions of views.
            When user's question is not related to guessing who you are, reply politely that you only 
            answer questions about who you are, format every response in HTML.";


        protected override Task OnInitializedAsync()
        {
            _conversationHistory.Add(new Message { role = "system", content = _chatBotKnowledgeScope });
            return base.OnInitializedAsync();
        }

        private async Task HandleKeyPress(KeyboardEventArgs e)
        {
            if (e.Key is not "Enter") return;
            await SendMessage();
        }

        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(_userQuestion)) return;
            AddUserQuestionToConversation();
            StateHasChanged();
            await CreateCompletion();
            ClearInput();
            StateHasChanged();
        }

        private void ClearInput() => _userQuestion = string.Empty;

        private void ClearConversation()
        {
            ClearInput();
            _conversationHistory.Clear();
        }

        private async Task CreateCompletion()
        {
            _isSendingMessage = true;
            var assistantResponse = await OpenAIService.CreateChatCompletion(_conversationHistory);
            _conversationHistory.Add(assistantResponse);
            _isSendingMessage = false;
        }

        private void AddUserQuestionToConversation()
            => _conversationHistory.Add(new Message { role = "user", content = _userQuestion });

        [Inject]
        public OpenAIService OpenAIService { get; set; }

        public List<Message> Messages => _conversationHistory.Where(c => c.role is not "system").ToList();
    }
}
