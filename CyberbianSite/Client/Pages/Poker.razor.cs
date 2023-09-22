using CyberbianSite.Client.Models;
using CyberbianSite.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CyberbianSite.Client.Pages
{
    public partial class Poker : ComponentBase
    {
        private string _userQuestion = string.Empty;
        private readonly List<Message> _conversationHistory = new();
        private bool _isSendingMessage;

        private readonly string _chatBotKnowledgeScope = @"
        You are now pretending to be Dylan. Dylan is a very shrewd, competent Texas hold 'em poker player. His primary drive is to win every single poker game.  He plays poker
        in accordance with Game Theory Optimal Strategy of Poker.  Use this strategy in giving the best action to take given the current state of the Texas Holdem hand.
        The current state of the game will be described.  Number of players will vary and they will be designated with a pattern of player 1, player 2 and so on.
        The players with big and small blind will be provided.  You will analyze the current state of the game and provide an action output in accordance with the template
        below.  
        Here is the current state of the game.
";
        //private string value;
        //EventConsole console;

        void OnTextAreaChange(string value, string name)
        {
            //console.Log($"{name} value changed to {value}");
        }

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

        private async Task OnSpeechCaptured(string speechValue, bool updateTextArea, string name)
        {
            //console.Log($"Speech Captured from {name}: {speechValue}");

            if (updateTextArea)
            {
                _userQuestion += speechValue + Environment.NewLine;
                if ((_userQuestion.Length > 0) && 
                    (_userQuestion.IndexOf("Submit", StringComparison.CurrentCultureIgnoreCase) != -1)) 
                {
                    if (_userQuestion.IndexOf("full analysis", StringComparison.CurrentCultureIgnoreCase) == -1)
                    {
                        _userQuestion += "Keep your output to no more than two sentences. Include expected value if it can be calculated.";
                    }
                    else
                    {
                        _userQuestion += "Perform full analysis based on Game Theory Optimal Strategy of Poker.  Provide any supportive details from the theory.  Include expected value if it can be calculated.";
                    }
                    await SendMessage();
                }
              
            }
        }
    }
}
