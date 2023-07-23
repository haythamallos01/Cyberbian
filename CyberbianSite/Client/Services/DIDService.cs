using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CyberbianSite.Client.Models.DID.Clips;
using CyberbianSite.Client.Models.DID.Talks.Request;
using CyberbianSite.Client.Models.DID.Talks.Response;

namespace CyberbianSite.Client.Services
{
    public class DIDService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<DIDOptions> _options;
        private Uri _baseUri;

        public DIDService(HttpClient httpClient, IOptions<DIDOptions> options)
        {
            _httpClient = httpClient;
            _options = options;
            _baseUri = new Uri(_options.Value.ApiUrl);
        }

        public async Task<DIDClipsActorsResponse> GetClipsActors()
        {
            Uri myUri = new Uri(_baseUri, _options.Value.ApiRelativeUrlClipsActors);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Basic " + _options.Value.ApiKey);

            var response = await _httpClient.GetAsync(myUri);
            response.EnsureSuccessStatusCode();
           
            var clipsActorsCompletionResponse = await response.Content.ReadFromJsonAsync<DIDClipsActorsResponse>();
            //string responseBody = await response.Content.ReadAsStringAsync();
            return clipsActorsCompletionResponse;
        }

        public async Task<DIDTalksResponse> PostTalks(DIDTalksRequest didTalksRequest)
        {
            Uri myUri = new Uri(_baseUri, _options.Value.ApiRelativeUrlTalks);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Basic " + _options.Value.ApiKey);

            var response = await _httpClient.PostAsJsonAsync(myUri, didTalksRequest);
            response.EnsureSuccessStatusCode();

            var talksCompletionResponse = await response.Content.ReadFromJsonAsync<DIDTalksResponse>();
            return talksCompletionResponse;
        }

        public async Task<DIDClipsActorsResponse> GetTalksById(string id)
        {
            Uri myUri = new Uri(_baseUri, id);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Basic " + _options.Value.ApiKey);

            var response = await _httpClient.GetAsync(myUri);
            response.EnsureSuccessStatusCode();

            var talksCompletionResponse = await response.Content.ReadFromJsonAsync<DIDClipsActorsResponse>();
            return talksCompletionResponse;
        }

    }
}
