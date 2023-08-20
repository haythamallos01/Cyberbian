using Cyberbian.Common.Logger;
using Cyberbian.Common.Worker;
using Microsoft.Extensions.Configuration;

namespace Cyberbian.Process.Main
{
    class Program
    {
        static volatile bool _bStop = false;
        static int _nCheckIntervalInMs = 60000;
        static string stopFilepath = Directory.GetCurrentDirectory() + @"/stop";
        static async Task Main(string[] args)
        {
            Config config = new Config();
            _nCheckIntervalInMs = config.CheckIntervalInMs;
            MyLogger logger = new MyLogger(config.ConnectionString);

            logger.Log("Main Service", "Starting main service");
            EmailWorker emailWorker = new EmailWorker(config.ConnectionString, config.SendEmailAPIToken, config.SendEmailUrl);

            Console.WriteLine("Processing ... create empty stop file to quit gracefully");
            while (!_bStop)
            {
                await emailWorker.Run();
                await Task.Delay(_nCheckIntervalInMs);
                if (File.Exists(stopFilepath))
                {
                    logger.Log("Main Service", "Gracefully stoppng main service");
                    Console.WriteLine("Detected stop file");
                    _bStop = true;
                    File.Delete(stopFilepath);
                    Console.WriteLine("Gracefully stopped main service");
                }
            }
        }
    }
}