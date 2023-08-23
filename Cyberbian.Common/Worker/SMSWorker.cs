using Azure.Communication.Sms;
using Cyberbian.Common.Logger;
using Cyberbian.Common.Models;
using Cyberbian.Common.OpenAI;
using Cyberbian.Common.ORM;
using CyberbianSite.Shared;
using Newtonsoft.Json;
using System.Net.Mail;

namespace Cyberbian.Common.Worker
{
    public class SMSWorker : BaseWorker
    {

        public SMSWorker(string connString, string SMSFromPhoneNumberE164, string AzureCommunicationServiceEndpoint)
        {
            _connectionString = connString;
            _AzureCommunicationServiceEndpoint = AzureCommunicationServiceEndpoint;
            _SMSFromPhoneNumberE164 = SMSFromPhoneNumberE164;
            _logger = new MyLogger(connString);
        }

        public async Task<bool> Run()
        {
            bool success = false;
            try
            {
                await Process();
            }
            catch (Exception ex)
            {
                HasError = true;
                Exc = ex;
                _logger.Log("MainProcess:SMSWorker:Run", null, ex);
            }
            return success;
        }

        private void sendSMS(string toPhoneNumber, string messageContent)
        {
            SmsClient smsClient = new SmsClient(_AzureCommunicationServiceEndpoint);
            smsClient.Send(
                from: _SMSFromPhoneNumberE164, // Your e.164 formatted from phone number used to send SMS
                to: new string[] { toPhoneNumber },
                message: messageContent,
                options: new SmsSendOptions(enableDeliveryReport: true)
                {
                    Tag = "botchat", //optional custom tags
                }
                );
        }

        private async Task Process()
        {
            // check incoming sms
            MemberORM memberORM = new MemberORM(_connectionString);
            IncomingSMSORM incomingSMSORM = new IncomingSMSORM(_connectionString);
            List<IncomingSMS>? lstIncomingSMSUnprocessed = incomingSMSORM.GetUnprocessed();
            if (lstIncomingSMSUnprocessed != null && lstIncomingSMSUnprocessed.Count > 0)
            {
                for (int i = 0; i < lstIncomingSMSUnprocessed.Count; i++)
                {
                    IncomingSMS incomingSMS = lstIncomingSMSUnprocessed[i];
                    incomingSMS.DateBeginProcessing = DateTime.UtcNow;
                    if (!string.IsNullOrEmpty(incomingSMS.IncomingRawText))
                    {
                        var lstIncomingSMSEventModel = JsonConvert.DeserializeObject<List<IncomingSMSEventModel>>(incomingSMS.IncomingRawText);
                        if (lstIncomingSMSEventModel != null && lstIncomingSMSEventModel.Count > 0)
                        {
                            if (lstIncomingSMSEventModel[0].eventType != "Microsoft.Communication.SMSReceived")
                            {
                                continue;
                            }

                            string message = lstIncomingSMSEventModel[0].data.message.Trim();
                            AIMemberORM aiMemberORM = new AIMemberORM(_connectionString);
                            bool isNewSMSUser = false;
                            Member member;
                            if (message.Length == 3)
                            {
                                // this is a register message
                                member = memberORM.GetByPartialHandleId(message);
                                if (member != null)
                                {
                                    aiMemberORM.UpdatePhoneNumber(member.MemberId, 1, lstIncomingSMSEventModel[0].data.from);
                                    isNewSMSUser = true;
                                }
                            }
                            // now do the reply
                            AIMember aiMember = aiMemberORM.GetByPhoneNumber(lstIncomingSMSEventModel[0].data.from);
                            if (aiMember != null)
                            {
                                member = memberORM.GetById(aiMember.MemberId);
                                incomingSMS.MemberId = aiMember.MemberId;
                                incomingSMS.AIMemberId = aiMember.AIMemberId;
                                // reply to the message
                                incomingSMS.OutgoingMessage = string.Empty;
                                incomingSMS.OutgoingTo = aiMember.PhoneNumberSMSE163;
                                if (isNewSMSUser)
                                {
                                    // send a welcome message
                                    incomingSMS.OutgoingMessage += "Hello " + member.FirstName + ". This is your AI Pal.";
                                    incomingSMS.OutgoingMessage += "  Thank you for registering.  You can text me anytime from this number.";

                                    sendSMS(incomingSMS.OutgoingTo, incomingSMS.OutgoingMessage);
                                }
                                else
                                {
                                    // reply to the inquiry
                                    string knowledgeScope = string.Empty;
                                    knowledgeScope += aiMember.PersonaPrompt;
                                    knowledgeScope += "Your name is " + aiMember.AliasName + ".  You were born on the date of " + member.DateCreated.ToString("F") + " UTC timezone";
                                    knowledgeScope += "Your gender is " + aiMember.Gender + ".";
                                    knowledgeScope += "Keep your reply under 50 words.";

                                    OpenAIClient openAIClient = new OpenAIClient();
                                    openAIClient.SetKnowledgeScope(knowledgeScope);
                                    string userQuestion = string.Empty;
                                    userQuestion += lstIncomingSMSEventModel[0].data.message;
                                    userQuestion += "  The recipient name is " + member.FirstName + Environment.NewLine;
                                    openAIClient.SetUserQuestion(userQuestion);
                                    await openAIClient.SendMessage();
                                    List<Message> messages = openAIClient.Messages;
                                    if (messages != null && messages.Count > 0)
                                    {
                                        Message replyMessage = messages.LastOrDefault();
                                        openAIClient.ClearConversation();
                                        sendSMS(incomingSMS.OutgoingTo, replyMessage.content);

                                    }
                                }

                                // update the incoming SMS record for processed
                                incomingSMS.DateEndProcessing = DateTime.UtcNow;
                                TimeSpan span = incomingSMS.DateEndProcessing - incomingSMS.DateBeginProcessing;
                                incomingSMS.ProcessingDurationInMS = (int)span.TotalMilliseconds;
                                incomingSMS.IsProcessed = true;
                                incomingSMS.EventId = lstIncomingSMSEventModel[0].id;
                                incomingSMS.EventTopic = lstIncomingSMSEventModel[0].topic;
                                incomingSMS.EventSubject = lstIncomingSMSEventModel[0].subject;
                                incomingSMS.EventDataMessageId = lstIncomingSMSEventModel[0].data.messageId;
                                incomingSMS.EventDataFrom = lstIncomingSMSEventModel[0].data.from;
                                incomingSMS.EventDataTo = lstIncomingSMSEventModel[0].data.to;
                                incomingSMS.EventDataMessage = lstIncomingSMSEventModel[0].data.message;
                                incomingSMS.EventDataReceivedTimestamp = lstIncomingSMSEventModel[0].data.receivedTimestamp;
                                incomingSMS.EventEventType = lstIncomingSMSEventModel[0].eventType;
                                incomingSMS.EventDataVersion = lstIncomingSMSEventModel[0].dataVersion;
                                incomingSMS.EventMetadataVersion = lstIncomingSMSEventModel[0].metadataVersion;
                                incomingSMS.EventEventTime = lstIncomingSMSEventModel[0].eventTime;
                                incomingSMSORM.UpdateOutgoingData(incomingSMS);
                            }
                        }
                    }

                }
            }
        }

    }
}
