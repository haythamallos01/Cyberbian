
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SearchAPI
{
    internal class Program
    {
        // Add your Bing Search V7 subscription key and endpoint to your environment variables
        static string subscriptionKey = "5720b8300a0d4c7abad4e97b1fff66fc";
        static string endpoint = "https://api.bing.microsoft.com/" + "/v7.0/search";

        const string query = "Mr. Beast";

        static void Main(string[] args)
        {
            // Create a dictionary to store relevant headers
            Dictionary<String, String> relevantHeaders = new Dictionary<String, String>();

            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Searching the Web for: " + query);

            // Construct the URI of the search request
            var uriQuery = endpoint + "?q=" + Uri.EscapeDataString(query);

            // Perform the Web request and get the response
            WebRequest request = HttpWebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = subscriptionKey;
            HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Extract Bing HTTP headers
            foreach (String header in response.Headers)
            {
                if (header.StartsWith("BingAPIs-") || header.StartsWith("X-MSEdge-"))
                    relevantHeaders[header] = response.Headers[header];
            }

            // Show headers
            Console.WriteLine("Relevant HTTP Headers:");
            foreach (var header in relevantHeaders)
                Console.WriteLine(header.Key + ": " + header.Value);

            Console.WriteLine("JSON Response:");
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            Console.WriteLine(JsonConvert.SerializeObject(parsedJson, Formatting.Indented));

            JObject data = JObject.Parse(json);
            JArray webpages = (JArray)data["webPages"]["value"];
            List<String> links = new List<String>();
            foreach (JObject item in webpages) // <-- Note that here we used JObject instead of usual JProperty
            {
                //string name = item.GetValue("name").ToString();
                string url = item.GetValue("url").ToString();
                links.Add(url);
            }

            JArray news = (JArray)data["news"]["value"];
            foreach (JObject item in news)
            {
                string url = item.GetValue("url").ToString();
                links.Add(url);
            }

            JArray videos = (JArray)data["videos"]["value"];
            foreach (JObject item in videos)
            {
                string url = item.GetValue("contentUrl").ToString();
                links.Add(url);
            }

            foreach (var value in links)
            {
                System.IO.File.AppendAllText(@"./links.txt", value + Environment.NewLine);
            }
        }
    }
}