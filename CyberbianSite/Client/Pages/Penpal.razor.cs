using Azure;
using CyberbianSite.Client.Authentication;
using CyberbianSite.Client.Services;
using CyberbianSite.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace CyberbianSite.Client.Pages
{
    public partial class Penpal
    {

        private string? botEmailAddress = string.Empty;
        private string _authMessage;
        private string _userId;
        private IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

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
                    var member = await memberResponse.Content.ReadFromJsonAsync<Member>();
                    botEmailAddress = "bert." + member.DefaultHandle + "@inbound.cyberbian.ai";
                }
            }
            else
            {
                _authMessage = "The user is NOT authenticated.";
            }
        }


    }
}
