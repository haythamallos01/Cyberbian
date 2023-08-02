using Microsoft.Extensions.Options;
using CyberbianSite.Client.Models.DID.Clips;
using CyberbianSite.Client.Models.DID.Talks.Request;
using CyberbianSite.Client.Models.DID.Talks.Response;
using Newtonsoft.Json;
using RestSharp;
using CyberbianSite.Client.Config;

namespace CyberbianSite.Client.Services
{
    public class DIDService
    {
        private readonly IOptions<DIDOptions> _options;

        public DIDService(IOptions<DIDOptions> options)
        {
            _options = options;
        }

        public async Task<DIDClipsActorsResponse> GetClipsActors()
        {
            var options = new RestClientOptions("https://api.d-id.com/clips/actors");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("Authorization", "Basic " + _options.Value.ApiKey);
            request.AddHeader("Accept", "application/json");
            var response = await client.GetAsync(request);
            var completionResponse = JsonConvert.DeserializeObject<DIDClipsActorsResponse>(response.Content);
            return completionResponse;
        }

        public async Task<DIDTalksResponse> PostTalks(DIDTalksRequest didTalksRequest)
        {
            var options = new RestClientOptions("https://api.d-id.com/talks");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("Authorization", "Basic " + _options.Value.ApiKey);
            request.AddHeader("Accept", "application/json");
            string json = JsonConvert.SerializeObject(didTalksRequest);
            request.AddJsonBody(json, false);
            var response = await client.PostAsync(request);
            var completionResponse = JsonConvert.DeserializeObject<DIDTalksResponse>(response.Content);
            return completionResponse;

        }

        public async Task<DIDTalksIdResponse> GetTalksById(string id)
        {
            var options = new RestClientOptions("https://api.d-id.com/talks/" + id);
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("Authorization", "Basic " + _options.Value.ApiKey);
            request.AddHeader("Accept", "application/json");
            var response = await client.GetAsync(request);
            var completionResponse = JsonConvert.DeserializeObject<DIDTalksIdResponse>(response.Content);
            return completionResponse;
        }

        public async Task<DIDTalksIdResponse> GetTalksById(string id, int millisecondsToWait)
        {
            Thread.Sleep(millisecondsToWait);
            DIDTalksIdResponse completionResponse = await GetTalksById(id);
            return completionResponse;
        }
    }
}
