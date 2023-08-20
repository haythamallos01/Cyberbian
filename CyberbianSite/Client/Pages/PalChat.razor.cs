using CyberbianSite.Client.Authentication;
using CyberbianSite.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Net.Http.Json;
using CyberbianSite.Client.Models;
using CyberbianSite.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using CyberbianSite.Client.Models.DID.Talks.Response;

namespace CyberbianSite.Client.Pages
{
    public partial class PalChat : ComponentBase
    {
        private string _userId;
        private Member? _member;
        private AIMember? _aimember;
        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();
        private string? botEmailAddress = string.Empty;
        private string _authMessage;

        private string _userQuestion = string.Empty;
        private readonly List<Message> _conversationHistory = new();
        private bool _isSendingMessage;
        private string? _sessionGuid = string.Empty;

        private string? _chatBotKnowledgeScope = string.Empty;

        protected override async Task OnParametersSetAsync()
        {
            Guid guid = Guid.NewGuid();
            _sessionGuid = guid.ToString();
            await GetClaimsPrincipalData();
            await base.OnParametersSetAsync();
        }

        private async Task GetClaimsPrincipalData()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                //_authMessage = $"{user.Identity.Name} is authenticated.";
                _claims = user.Claims;
                _userId = $"User Id: {user.FindFirst(c => c.Type == "sub")?.Value}";
                var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
                string jwtToken = await customAuthStateProvider.GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var memberResponse = await httpClient.GetAsync("/api/data/member/" + user.Identity.Name);
                if (memberResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _member = await memberResponse.Content.ReadFromJsonAsync<Member>();
                    botEmailAddress = "bert." + _member.DefaultHandle + "@inbound.cyberbian.ai";

                    var aimemberResponse = await httpClient.GetAsync("/api/data/aimember?memberid=" + _member.MemberId + "&aitypeid=1");
                    if (aimemberResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (aimemberResponse.Content != null)
                        {
                            _aimember = await aimemberResponse.Content.ReadFromJsonAsync<AIMember>();
                            if (_aimember != null)
                            {
                                _chatBotKnowledgeScope = _aimember.PersonaPrompt;
                                _chatBotKnowledgeScope += "Your name is " + _aimember.AliasName + ".  You were born on the date of " + _aimember.DateCreated.ToString("F") + " UTC timezone";
                                _chatBotKnowledgeScope += "Your gender is " + _aimember.Gender + ".";
                                _conversationHistory.Add(new Message { role = "system", content = _chatBotKnowledgeScope });
                            }
                        }
                    }
                    StateHasChanged();

                }
            }
            else
            {
                _authMessage = "The user is NOT authenticated.";
            }
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
            Guid guid = Guid.NewGuid();
            _sessionGuid = guid.ToString();
        }

        private async Task SaveChat(string responseText, DateTime beginDateTime, DateTime endDateTime)
        {
            IncomingChat incomingChat = new IncomingChat();
            string jsonConversationHistoryString = JsonConvert.SerializeObject(_conversationHistory);
            incomingChat.ConversationHistory = jsonConversationHistoryString;
            incomingChat.MemberId = _member.MemberId;
            incomingChat.SessionGuid = _sessionGuid;
            incomingChat.MsgSource = "MAIN_SITE";
            incomingChat.QuestionPrompt = _userQuestion;
            incomingChat.DateBeginProcessing = beginDateTime;
            incomingChat.DateEndProcessing = endDateTime;
            TimeSpan span = endDateTime - beginDateTime;
            incomingChat.ProcessingDurationInMS = (int)span.TotalMilliseconds;
            incomingChat.QuestionPromptResponse = responseText;
            incomingChat.IsProcessed = true;
            var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
            string jwtToken = await customAuthStateProvider.GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var postResponse = await httpClient.PostAsJsonAsync<IncomingChat>("/api/data/incomingchat", incomingChat);
        }

        private async Task CreateCompletion()
        {
            DateTime beginDateTime = DateTime.UtcNow;
            _isSendingMessage = true;
            var assistantResponse = await OpenAIService.CreateChatCompletion(_conversationHistory);
            _conversationHistory.Add(assistantResponse);
            _isSendingMessage = false;
            DateTime endDateTime = DateTime.UtcNow;
            await SaveChat(assistantResponse.content, beginDateTime, endDateTime);
        }

        private void AddUserQuestionToConversation()
            => _conversationHistory.Add(new Message { role = "user", content = _userQuestion });

        [Inject]
        public OpenAIService OpenAIService { get; set; }

        public List<Message> Messages => _conversationHistory.Where(c => c.role is not "system").ToList();
    }
}
