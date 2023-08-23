using Microsoft.Extensions.Configuration;

namespace Cyberbian.Process.Main
{
    public class Config
    {
        private int _nClientID = 0;
        private int _nCheckIntervalInMs = 5000;
        private string _strConnectionString = null;
        private string _strSendEmailAPIToken = null;
        private string _strSendEmailUrl = null;
        private string _strSMSFromPhoneNumberE164 = null;
        private string _strAzureCommunicationServiceEndpoint = null;

        private static readonly string KEY_CLIENT_ID = "ClientID";
        private static readonly string KEY_CHECK_INTERVAL_IN_MS = "CheckIntervalInMs";
        private static readonly string KEY_CONNECTION_STRING = "ConnectionString";
        private static readonly string KEY_SEND_EMAIL_API_TOKEN = "SendEmailAPIToken";
        private static readonly string KEY_SEND_EMAIL_URL = "SendEmailUrl";
        private static readonly string KEY_SMS_FROM_PHONE_NUMBER_E164 = "SMSFromPhoneNumberE164";
        private static readonly string KEY_AZURE_COMMUNICATION_SERVICE_ENDPOINT = "AzureCommunicationServiceEndpoint";

        public string AzureCommunicationServiceEndpoint
        {
            get { return _strAzureCommunicationServiceEndpoint; }
            set { _strAzureCommunicationServiceEndpoint = value; }
        }

        public string SMSFromPhoneNumberE164
        {
            get { return _strSMSFromPhoneNumberE164; }
            set { _strSMSFromPhoneNumberE164 = value; }
        }

        public string ConnectionString
        {
            get { return _strConnectionString; }
            set { _strConnectionString = value; }
        }

        public int ClientID
        {
            get { return _nClientID; }
            set { _nClientID = value; }
        }

        public int CheckIntervalInMs
        {
            get { return _nCheckIntervalInMs; }
            set { _nCheckIntervalInMs = value; }
        }

        public string SendEmailAPIToken
        {
            get { return _strSendEmailAPIToken; }
            set { _strSendEmailAPIToken = value; }
        }

        public string SendEmailUrl
        {
            get { return _strSendEmailUrl; }
            set { _strSendEmailUrl = value; }
        }

        public Config()
        {
            IConfiguration configuration = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
              .Build();

            try
            {
                _nCheckIntervalInMs = Convert.ToInt32(configuration["AppSettings:" + KEY_CHECK_INTERVAL_IN_MS]);
            }
            catch { _strConnectionString = null; }

            try
            {
                _strConnectionString = configuration["AppSettings:" + KEY_CONNECTION_STRING];
            }
            catch { _strConnectionString = null; }

            try
            {
                _nClientID = Convert.ToInt32(configuration["AppSettings:" + KEY_CLIENT_ID]);
            }
            catch { _nClientID = 0; }

            try
            {
                _strSendEmailAPIToken = configuration["AppSettings:" + KEY_SEND_EMAIL_API_TOKEN];
            }
            catch { }

            try
            {
                _strSendEmailUrl = configuration["AppSettings:" + KEY_SEND_EMAIL_URL];
            }
            catch { }

            try
            {
                _strSMSFromPhoneNumberE164 = configuration["AppSettings:" + KEY_SMS_FROM_PHONE_NUMBER_E164];
            }
            catch { }

            try
            {
                _strAzureCommunicationServiceEndpoint = configuration["AppSettings:" + KEY_AZURE_COMMUNICATION_SERVICE_ENDPOINT];
            }
            catch { }

        }
    }
}
