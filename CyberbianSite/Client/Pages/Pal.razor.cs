using CyberbianSite.Client.Authentication;
using CyberbianSite.Client.Models;
using CyberbianSite.Client.Services;
using CyberbianSite.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace CyberbianSite.Client.Pages
{
    public partial class Pal
    {

        private string? botEmailAddress = string.Empty;
        private string _authMessage;
        private string _userId;
        private bool isSeeded = false;
        private string seedContent = string.Empty;
        private Member? _member;
        private AIMember? _aimember;
        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

        [Inject]
        public OpenAIService OpenAIService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await GetClaimsPrincipalData();
            await base.OnParametersSetAsync();
        }

        private async Task GetClaimsPrincipalData()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                _authMessage = $"{user.Identity.Name} is authenticated.";
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
                                isSeeded = true;
                            }
                        }
                    }

                }
            }
            else
            {
                _authMessage = "The user is NOT authenticated.";
            }
        }

        protected async Task OnSubmitCreatePal()
        {
            try
            {
                if (string.IsNullOrEmpty(seedContent))
                {
                    seedContent = "Be kind and loving so when I need you can cheer me up.";
                }

                List<Message> conversationHistory = new();
                conversationHistory.Add(new Message { role = "system", content = seedContent });
                string userQuestion = @"Elaborate on this in less than 100 words.";
                conversationHistory.Add(new Message { role = "user", content = userQuestion });
                var assistantResponse = await OpenAIService.CreateChatCompletion(conversationHistory);


                var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
                string jwtToken = await customAuthStateProvider.GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                AIMember aimember = new AIMember();
                aimember.Seed = seedContent.Trim();
                aimember.GeneratedSeed = assistantResponse.content.Trim();
                aimember.AITypeId = 1;
                aimember.MemberId = _member.MemberId;
                aimember.IsDisabled = false;
                aimember.Birthdate = DateTime.UtcNow;
                botEmailAddress = "bert." + _member.DefaultHandle + "@inbound.cyberbian.ai";
                aimember.EmailAddress = botEmailAddress;
                var aimemberResponse = await httpClient.PostAsJsonAsync<AIMember>("/api/data/aimember", aimember);
                await GetClaimsPrincipalData();
                StateHasChanged();
            }
            catch (Exception ex) { }

        }

    }
}
