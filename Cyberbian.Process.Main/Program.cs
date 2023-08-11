using Cyberbian.Process.Main.Lib;

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

            Log("Starting main service");
            MainWorker worker = new MainWorker();

            Console.WriteLine("Processing ... create empty stop file to quit gracefully");
            while (!_bStop)
            {
                worker.Run();
                await Task.Delay(_nCheckIntervalInMs);
                if (File.Exists(stopFilepath))
                {
                    Log("Gracefully stoppng main service");
                    Console.WriteLine("Detected stop file");
                    _bStop = true;
                    File.Delete(stopFilepath);
                    Console.WriteLine("Gracefully stopped main service");
                }
            }
        }

        static void Log(string logMessage)
        {
            MainWorker mainWorker = new MainWorker();
            mainWorker.Log("Main Service", logMessage);
        }
    }
}