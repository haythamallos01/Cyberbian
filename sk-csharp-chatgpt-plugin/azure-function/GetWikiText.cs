using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;

namespace WikiPlugin
{
    public class GetWiki
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger _logger;

        public GetWiki(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetWiki>();
        }

        [OpenApiOperation(operationId:"GetWikiText", tags: new[] { "ExecuteFunction" }, Description ="Gets the text of a wiki page for a given query so it can be summarized by SummarizeWikiArticle")]
        [OpenApiParameter(name:"title", Description ="The title of a Wikipedia article", Required = true, In =ParameterLocation.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType:"text/plain", bodyType: typeof(string), Description = "The text of a wiki")]
        [Function("GetWikiText")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            string article = await FetchWikipediaArticleContent(req.Query["title"]);

            if (article == "Article content not found.")
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "application/json");
                response.WriteString("Article content not found.");

                _logger.LogInformation($"A wiki article for {req.Query["title"]} was not found.");
                return response;
            }
            else
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json");

                // Return the first 10,0000 characters of the article
                article = article.Substring(0, Math.Min(article.Length, 10000));
                response.WriteString(article);

                _logger.LogInformation($"A wiki article for {req.Query["title"]} was found.");
                return response;
            }
        }

        private static async Task<string> FetchWikipediaArticleContent(string query)
        {
            var url = $"https://en.wikipedia.org/w/api.php?format=json&action=query&prop=revisions&rvprop=content&redirects=1&titles={query}";

            var responseString = await client.GetStringAsync(url);
            var json = JObject.Parse(responseString);
            var pages = json["query"]["pages"];

            foreach(var page in pages)
            {
                var revisions = page.First["revisions"];
                if (revisions != null)
                {
                    var content = revisions[0]["*"];
                    return content.ToString();
                }
            }

            return "Article content not found.";
        }
    }
}
