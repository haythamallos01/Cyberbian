using Microsoft.Extensions.Options;

namespace CyberbianSite.Client.Config
{

    public class InitializeApp
    {
        private readonly IOptions<AzureOptions> _options;
        public InitializeApp(IOptions<AzureOptions> options)
        {
            _options = options;
        }

    }
}
