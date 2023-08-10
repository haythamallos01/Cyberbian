using Cyberbian.Web.Webhook.Email.Config;
using Cyberbian.Web.Webhook.Email.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Web;

namespace Cyberbian.Web.Webhook.Email
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json");
            var config = configuration.Build();
            DatabaseOptions databaseOptions = config.GetRequiredSection("Database").Get<DatabaseOptions>();
            WebhookOptions webhookOptions = config.GetRequiredSection("Webhook").Get<WebhookOptions>();

            app.MapGet("/", () => "Cyberbian webhooks");
            app.MapPost("/webhook/email/inbound", async (HttpRequest request) =>
            {

                var auth = request.Headers["Authorization"].ToString();
                if (auth.ToLower().StartsWith("basic "))
                {
                    var credentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6)));

                    string userName = credentials.Split(':')[0];
                    string password = credentials.Split(':')[1];

                    if ((userName == webhookOptions.PostmarkAuthUsername) 
                    && (password == webhookOptions.PostmarkAuthPassword))
                    {
                        string body = "";
                        using (StreamReader stream = new StreamReader(request.Body))
                        {
                            body = await stream.ReadToEndAsync();
                        }
                        string decodedBody = HttpUtility.UrlDecode(body);
                        // store in db
                        StorageHelper storageHelper = new StorageHelper(databaseOptions.ConnectionString);
                        storageHelper.SaveEmailInbound(decodedBody, "POSTMARK-INBOUND");
                        return HttpUtility.UrlDecode(body);
                    }
                }

                return null;

            });
            app.Run();
        }
    }
}