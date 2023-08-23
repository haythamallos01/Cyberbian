using Cyberbian.Common.Logger;

namespace Cyberbian.Common.Worker
{
    public class BaseWorker
    {
        public bool HasError { get; set; }
        public Exception Exc { get; set; }

        protected string _connectionString { get; set; }
        protected string _SMSFromPhoneNumberE164 { get; set; }
        protected string _AzureCommunicationServiceEndpoint { get; set; }
        protected MyLogger _logger = null;
        
        public BaseWorker() { }
    }
}
