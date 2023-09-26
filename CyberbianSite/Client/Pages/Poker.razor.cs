using Azure.Core;
using CyberbianSite.Client.Authentication;
using CyberbianSite.Client.Models;
using CyberbianSite.Client.Services;
using CyberbianSite.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CyberbianSite.Client.Pages
{
    public partial class Poker : ComponentBase
    {
        private string _userQuestion = string.Empty;
        private readonly List<Message> _conversationHistory = new();
        private bool _isSendingMessage;
        private Member? _member;

        private readonly string _chatBotKnowledgeScope = @"
        You are now pretending to be Dillon. Dillon is a very shrewd, competent Texas hold 'em poker player. His primary drive is to win every single poker game.  He plays poker
        in accordance with Game Theory Optimal Strategy of Poker.  Use this strategy in giving the best action to take given the current state of the Texas Holdem hand.
        The current state of the game will be described.  Number of players will vary and they will be designated with a pattern of player 1, player 2 and so on.
        The players with big and small blind will be provided.  You will analyze the current state of the game and provide an action output in accordance with the template
        below.  
        Action:  Provide no more than three sentences on the action to take. Include expected value if it can be calculated.
        Analysis:  Then perform full analysis of the move based on Game Theory Optimal Strategy of Poker.  Provide any supportive details from the theory.  Include expected value if it can be calculated.
        Here is the current state of the game.
";

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
            Log(string.Empty, string.Empty, true);

        }

        private async Task CreateCompletion()
        {
            _isSendingMessage = true;
            var assistantResponse = await OpenAIService.CreateChatCompletion(_conversationHistory);
            _conversationHistory.Add(assistantResponse);
            _isSendingMessage = false;
            Log(_userQuestion, assistantResponse.content);
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
                    await SendMessage();
                }

            }
        }

        private async Task Log(string promptRequest, string promptResponse, bool isNewHand = false)
        {
            try
            {
                var authState = await authStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;

                var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
                string jwtToken = await customAuthStateProvider.GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var memberResponse = await httpClient.GetAsync("/api/data/member/" + user.Identity.Name);
                if (memberResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _member = await memberResponse.Content.ReadFromJsonAsync<Member>();
                    Pokerlog pokerlog = new Pokerlog();
                    pokerlog.PromptRequest = promptRequest;
                    pokerlog.PromptResponse = promptResponse;
                    pokerlog.MemberId = _member.MemberId;
                    pokerlog.IsNewHand = isNewHand;
                    var response = await httpClient.PostAsJsonAsync("/api/data/pokerlog", pokerlog);
                    response.EnsureSuccessStatusCode();
                    var pokerlogResponse = await response.Content.ReadFromJsonAsync<Pokerlog>();
                }

            }
            catch { }
        }
    }
}
