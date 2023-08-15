using Cyberbian.Data.ORM;
using Cyberbian.Process.Main.Lib.Models;
using CyberbianSite.Shared;
using Newtonsoft.Json;
using System.Dynamic;

namespace Cyberbian.Web.Webhook.Email.Services
{
    public class StorageHelper
    {
        private string? _connectionString { get; set; }

        public StorageHelper(string connectionString) {
            _connectionString = connectionString;
        }

        public void SaveEmailInbound(string data, string msgSource, bool isTest=false)
        {
            try
            {
                MemberORM memberORM = new MemberORM(_connectionString);

                IncomingEmailPayload incomingEmailPayload = JsonConvert.DeserializeObject<IncomingEmailPayload>(data);
                // get the handle id from to address
                int bInd = incomingEmailPayload.To.IndexOf('.');
                int eInd = incomingEmailPayload.To.IndexOf('@');
                string handleId = incomingEmailPayload.To.Substring(bInd + 1, eInd - bInd - 1);
                Member member = memberORM.GetByHandleId(handleId);

                IncomingEmail incomingEmail = new()
                {
                    RawData = data,
                    IsTest = isTest,
                    MsgSource = msgSource,
                    HandleId = handleId,
                    IncomingTo = incomingEmailPayload.To,
                    IncomingFrom = incomingEmailPayload.From,
                    IncomingFromName = incomingEmailPayload.FromName,
                    IncomingSubject = incomingEmailPayload.Subject,
                    MemberId = member.MemberId
                };

                IncomingEmailORM incomingEmailORM = new IncomingEmailORM(_connectionString);
                incomingEmail = incomingEmailORM.Create(incomingEmail);
            }
            catch (Exception ex)
            {
                // save syslog record
                try
                {
                    dynamic jsonPayloadObject = new ExpandoObject();
                    jsonPayloadObject.MsgSource = msgSource;
                    jsonPayloadObject.Payload = data;
                    string jsonPayloadString = JsonConvert.SerializeObject(jsonPayloadObject);

                    dynamic jsonErrorObject = new ExpandoObject();
                    jsonErrorObject.ErrorMessage = ex.Message;
                    jsonErrorObject.StackTrace = ex.StackTrace;
                    string jsonErrorString = JsonConvert.SerializeObject(jsonErrorObject);

                    Syslog syslog = new Syslog();
                    syslog.MsgSource = "SaveEmailInbound";
                    syslog.MsgText = jsonErrorString;
                    syslog.Payload = jsonPayloadString;

                    SyslogORM syslogORM = new SyslogORM(_connectionString);
                    syslog = syslogORM.Create(syslog);
                }
                catch (Exception ex2) { }
            }
        }
    }
}
