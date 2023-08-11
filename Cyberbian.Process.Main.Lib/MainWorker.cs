using Cyberbian.Data.ORM;
using CyberbianSite.Shared;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Cyberbian.Process.Main.Lib.Models;
using Cyberbian.Process.Main.Lib.OpenAI;
using System.Xml.Schema;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Cyberbian.Process.Main.Lib
{
    public class MainWorker
    {
        public bool HasError { get; set; }
        public Exception Exc { get; set; }

        private string _connectionString { get; set; }
        private string _sendEmailAPIToken { get; set; }
        private string _sendEmailUrl { get; set; }

        public MainWorker()
        {
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
                _connectionString = configuration["AppSettings:" + "ConnectionString"];
            }
            catch { }

            try
            {
                _sendEmailAPIToken = configuration["AppSettings:" + "SendEmailAPIToken"];
            }
            catch { }

            try
            {
                _sendEmailUrl = configuration["AppSettings:" + "SendEmailUrl"];
            }
            catch { }

        }

        public async Task<bool> Run()
        {
            bool success = false;
            try
            {
                await ProcessIncomingEmail();
            }
            catch (Exception ex)
            {
                HasError = true;
                Exc = ex;
                Log("MainWork:Run", null, ex);
            }
            return success;
        }

        private void sendEmail(string outboundEmailRequestText)
        {
            var data = Encoding.ASCII.GetBytes(outboundEmailRequestText);

            var request = (HttpWebRequest)WebRequest.Create(_sendEmailUrl);
            request.Method = "POST";
            request.Headers["X-Postmark-Server-Token"] = _sendEmailAPIToken;
            request.Headers["Accept"] = "application/json";
            request.Headers["Content-Type"] = "application/json";
            //request.ContentLength = outboundEmailRequestText.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        }

        private async Task ProcessIncomingEmail()
        {
            // check incoming email
            MemberORM memberORM = new MemberORM(_connectionString);
            IncomingEmailORM incomingEmailORM = new IncomingEmailORM(_connectionString);
            List<IncomingEmail>? lstIncomingEmailUnprocessed = incomingEmailORM.GetUnprocessed();
            if (lstIncomingEmailUnprocessed != null && lstIncomingEmailUnprocessed.Count > 0 )
            {
                foreach (IncomingEmail currentIncomingEmail in lstIncomingEmailUnprocessed)
                {
                    string jsonPayloadString = JsonConvert.SerializeObject(currentIncomingEmail);
                    try
                    {
                        IncomingEmailPayload incomingEmailPayload = JsonConvert.DeserializeObject<IncomingEmailPayload>(currentIncomingEmail.RawData);
                        string toIncomingEmailAddress = incomingEmailPayload.To;
                        // get the handle id from to address
                        int bInd = toIncomingEmailAddress.IndexOf('.');
                        int eInd = toIncomingEmailAddress.IndexOf('@');
                        string handleId = toIncomingEmailAddress.Substring(bInd + 1, eInd - bInd - 1);
                        Member member = memberORM.GetByHandleId(handleId);
                        if (member != null)
                        {
                            string knowledgeScope = string.Empty;
                            knowledgeScope += incomingEmailPayload.Subject;
                            knowledgeScope += incomingEmailPayload.TextBody;
                            OpenAIClient openAIClient = new OpenAIClient();
                            openAIClient.SetKnowledgeScope(knowledgeScope);
                            openAIClient.SetUserQuestion("What would a kind and friendly reply be?");
                            await openAIClient.SendMessage();
                            List<Message> messages = openAIClient.Messages;
                            if (messages != null && messages.Count > 0)
                            {
                                Message replyMessage = messages.LastOrDefault();
                                OutboundEmailRequest outboundEmailRequest = new OutboundEmailRequest();
                                outboundEmailRequest.To = incomingEmailPayload.From;
                                outboundEmailRequest.From = toIncomingEmailAddress;
                                outboundEmailRequest.Subject = incomingEmailPayload.Subject;
                                outboundEmailRequest.TextBody = incomingEmailPayload.TextBody;
                                outboundEmailRequest.HtmlBody = "<html><body>" + replyMessage.content + "</body></html>";
                                outboundEmailRequest.MessageStream = "outbound";
                                string jsonOutboundEmailRequest = JsonConvert.SerializeObject(outboundEmailRequest);
                                openAIClient.ClearConversation();
                                sendEmail(jsonOutboundEmailRequest);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        currentIncomingEmail.IsError = true;
                        currentIncomingEmail.ErrorStr = exc.Message + Environment.NewLine + exc.StackTrace;
                        Log("ProcessIncomingEmail", jsonPayloadString, exc);
                    }
                }

            }
        }
        public void Log(string MsgSource, string Payload = null, Exception ex = null)
        {
            try
            {
                SyslogORM orm = new SyslogORM(_connectionString);
                Syslog syslog = new Syslog();
                syslog.MsgSource = MsgSource;
                if (Payload != null)
                {
                    syslog.Payload = Payload;
                }
                if (ex != null)
                {
                    syslog.MsgText = ex.Message + Environment.NewLine + ex.StackTrace;
                }
                syslog = orm.Create(syslog);
            }
            catch { }

        }
    }
}