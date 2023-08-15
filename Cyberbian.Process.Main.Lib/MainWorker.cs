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
            request.ContentLength = outboundEmailRequestText.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        }

        private List<IncomingEmail> getMemberMostRecentIncomingEmail(long memberId, string subjectText)
        {
            List<IncomingEmail>? lstIncomingEmail = new List<IncomingEmail>();
            int maxTextBodyLengthCombinedInBytes = 6000;
            if ((memberId > 0) && (!string.IsNullOrEmpty(subjectText)))
            {
                IncomingEmailORM incomingEmailORM = new IncomingEmailORM(_connectionString);
                List<IncomingEmail>? lstIncomingEmailSameSubject = incomingEmailORM.GetSameSubjectListForMember(memberId,
                    subjectText);
                int sumWordCount = 0;
                for (int i = 0; i < lstIncomingEmailSameSubject.Count; i++)
                {
                    sumWordCount += lstIncomingEmailSameSubject[lstIncomingEmailSameSubject.Count - 1].RawData.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;

                    if (sumWordCount <= maxTextBodyLengthCombinedInBytes)
                    {
                        lstIncomingEmail.Insert(0, lstIncomingEmailSameSubject[lstIncomingEmailSameSubject.Count - i - 1]);
                    }
                }
            }


            return lstIncomingEmail;
        }

        private async Task ProcessIncomingEmail()
        {
            // check incoming email
            MemberORM memberORM = new MemberORM(_connectionString);
            IncomingEmailORM incomingEmailORM = new IncomingEmailORM(_connectionString);
            List<IncomingEmail>? lstIncomingEmailUnprocessed = incomingEmailORM.GetUnprocessed();
            if (lstIncomingEmailUnprocessed != null && lstIncomingEmailUnprocessed.Count > 0)
            {
                foreach (IncomingEmail currentIncomingEmail in lstIncomingEmailUnprocessed)
                {
                    currentIncomingEmail.DateBeginProcessing = DateTime.UtcNow;
                    string jsonPayloadString = JsonConvert.SerializeObject(currentIncomingEmail);
                    try
                    {
                        IncomingEmailPayload incomingEmailPayload = JsonConvert.DeserializeObject<IncomingEmailPayload>(currentIncomingEmail.RawData);
                        if (currentIncomingEmail.MemberId > 0)
                        {
                            Member currentMember = memberORM.GetById(currentIncomingEmail.MemberId);
                            List<IncomingEmail>? lstIncomingEmailSameSubject = null;
                            string filteredIncomingSubject = currentIncomingEmail.IncomingSubject.Replace("Re: ", string.Empty);
                            lstIncomingEmailSameSubject = getMemberMostRecentIncomingEmail(currentIncomingEmail.MemberId,
                                filteredIncomingSubject);
                            //lstIncomingEmailSameSubject = incomingEmailORM.GetSameSubjectListForMember(currentIncomingEmail.MemberId,
                            //    filteredIncomingSubject);


                            string knowledgeScope = string.Empty;
                            //knowledgeScope += filteredIncomingSubject + Environment.NewLine;
                            knowledgeScope += "Your name is Bert.  You were born on the date of " + currentMember.DateCreated.ToString("F") + " UTC timezone";
                            knowledgeScope += @"Assume the following set of email messages that are in chronological order.  Each complete email message 
                            is separated by the term [[END]] before the next one.";
                            foreach (IncomingEmail sameSubjectIncomingEmail in lstIncomingEmailSameSubject)
                            {
                                IncomingEmailPayload sameSubjectIncomingEmailPayload = JsonConvert.DeserializeObject<IncomingEmailPayload>(sameSubjectIncomingEmail.RawData);
                                knowledgeScope += @"From:  " + sameSubjectIncomingEmail.IncomingFrom + Environment.NewLine;
                                knowledgeScope += @"To:  " + sameSubjectIncomingEmail.IncomingTo + Environment.NewLine;
                                knowledgeScope += @"Recipient Name:  " + currentMember.FirstName + Environment.NewLine;
                                knowledgeScope += @"DateReceived:  " + sameSubjectIncomingEmail.DateCreated + Environment.NewLine;
                                knowledgeScope += @"Subject:  " + sameSubjectIncomingEmail.IncomingSubject + Environment.NewLine;
                                knowledgeScope += @"BodyText:  " + sameSubjectIncomingEmailPayload.TextBody.Replace("bert", string.Empty) + Environment.NewLine;
                                knowledgeScope += @"[[END]]" + Environment.NewLine;
                                //knowledgeScope += sameSubjectIncomingEmailPayload.TextBody + Environment.NewLine;
                            }
                            OpenAIClient openAIClient = new OpenAIClient();
                            openAIClient.SetKnowledgeScope(knowledgeScope);
                            openAIClient.SetUserQuestion("What would a kind and friendly reply be?  Only provide the BodyText.  The recipient name is " + currentMember.FirstName + Environment.NewLine);
                            await openAIClient.SendMessage();
                            List<Message> messages = openAIClient.Messages;
                            if (messages != null && messages.Count > 0)
                            {
                                Message replyMessage = messages.LastOrDefault();
                                OutboundEmailRequest outboundEmailRequest = new OutboundEmailRequest();
                                outboundEmailRequest.To = incomingEmailPayload.From;
                                outboundEmailRequest.From = incomingEmailPayload.To;
                                outboundEmailRequest.ReplyTo = incomingEmailPayload.To;
                                outboundEmailRequest.Subject = incomingEmailPayload.Subject;
                                outboundEmailRequest.TextBody = incomingEmailPayload.TextBody;
                                outboundEmailRequest.HtmlBody = "<html><body>" + replyMessage.content + "</body></html>";
                                outboundEmailRequest.MessageStream = "outbound";
                                string jsonOutboundEmailRequest = JsonConvert.SerializeObject(outboundEmailRequest);
                                openAIClient.ClearConversation();
                                sendEmail(jsonOutboundEmailRequest);

                                // success.  Updated the db
                                currentIncomingEmail.DateEndProcessing = DateTime.UtcNow;
                                TimeSpan span = currentIncomingEmail.DateEndProcessing - currentIncomingEmail.DateBeginProcessing;
                                currentIncomingEmail.ProcessingDurationInMS = (int)span.TotalMilliseconds;
                                currentIncomingEmail.IsProcessed = true;
                                currentIncomingEmail.OutgoingTo = outboundEmailRequest.To;
                                currentIncomingEmail.OutgoingFrom = outboundEmailRequest.From;
                                currentIncomingEmail.OutgoingSubject = outboundEmailRequest.Subject;
                                currentIncomingEmail.OutgoingReplyTo = outboundEmailRequest.ReplyTo;
                                currentIncomingEmail.OutgoingTextBody = outboundEmailRequest.TextBody;
                                currentIncomingEmail.OutgoingHtmlBody = outboundEmailRequest.HtmlBody;
                                currentIncomingEmail.OutgoingMessageStream = outboundEmailRequest.MessageStream;
                                incomingEmailORM.UpdateOutgoingData(currentIncomingEmail);
                            }
                        }
                        else
                        {
                            // this inbound email is not associated with any member. Don't process
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