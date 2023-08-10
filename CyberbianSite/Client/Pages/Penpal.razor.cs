using CyberbianSite.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Cyberbian.Data.ORM.Lib;

namespace CyberbianSite.Client.Pages
{
    public partial class Penpal
    {
        [Inject]
        public DbService dbService { get; set; }
        ClaimsPrincipal User => authStateProvider.GetAuthenticationStateAsync().Result.User;

        public Member member { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        public string botEmailAddress(string username)
        {
            Member member = dbService.GetMember(username);
            string botEmailAddress = "bert." + member.DefaultHandle + "@inbound.cyberbian.ai";
            return botEmailAddress;
        }


    }
}
