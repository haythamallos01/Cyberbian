using Microsoft.Extensions.Configuration;

namespace Cyberbian.Process.Main
{
    public class Config
    {
        private int _nClientID = 0;
        private int _nCheckIntervalInMs = 5000;
        private string _strConnectionString = null;

        private static readonly string KEY_CLIENT_ID = "ClientID";
        private static readonly string KEY_CHECK_INTERVAL_IN_MS = "CheckIntervalInMs";
        private static readonly string KEY_CONNECTION_STRING = "ConnectionString";

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
        }
    }
}
