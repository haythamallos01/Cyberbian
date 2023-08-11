using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Cyberbian.Process.Main.Lib.OpenAI
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private string _ApiKey { get; set; }
        private string _ApiUrl { get; set; }
        private string _GtpModel { get; set; }

        public OpenAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            setConfig();
        }

        private void setConfig()
        {
            IConfiguration configuration = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
              .Build();

            try
            {
                _ApiKey = configuration["OpenAI:" + "ApiKey"];
            }
            catch { }

            try
            {
                _ApiUrl = configuration["OpenAI:" + "ApiUrl"];
            }
            catch { }

            try
            {
                _GtpModel = configuration["OpenAI:" + "GtpModel"];
            }
            catch { }
        }
        public async Task<Message> CreateChatCompletion(List<Message> messages)
        {
            var request = new { model = _GtpModel, messages = messages.ToArray() };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _ApiKey);

            var response = await _httpClient.PostAsJsonAsync(_ApiUrl, request);
            response.EnsureSuccessStatusCode();

            var chatCompletionResponse = await response.Content.ReadFromJsonAsync<ChatbotResponse>();
            return chatCompletionResponse?.choices.First().message;
        }
    }

    public class ChatbotResponse
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int created { get; set; }
        public Choice[] choices { get; set; }
        public Usage usage { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    public class Choice
    {
        public int index { get; set; }
        public Message message { get; set; }
        public string finish_reason { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }

        [JsonIgnore]
        public bool IsUser => role == "user";
    }


}
