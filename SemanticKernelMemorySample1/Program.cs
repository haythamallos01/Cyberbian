
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Reliability;
using RepoUtils;

namespace SemanticKernelMemorySample1
{
    public static class Program
    {
        public static async Task Main()
        {
            // Load configuration from environment variables or user secrets.
            LoadUserSecrets();

            // Execution canceled if the user presses Ctrl+C.
            using CancellationTokenSource cancellationTokenSource = new();
            CancellationToken cancelToken = cancellationTokenSource.ConsoleCancellationToken();

            // Run examples
            //await Example01_NativeFunctions.RunAsync().SafeWaitAsync(cancelToken);
            //await Example02_Pipeline.RunAsync().SafeWaitAsync(cancelToken);
            //await Example03_Variables.RunAsync().SafeWaitAsync(cancelToken);
            await Example04_CombineLLMPromptsAndNativeCode.RunAsync().SafeWaitAsync(cancelToken);


        }

        private static void LoadUserSecrets()
        {
            IConfigurationRoot configRoot = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<Env>()
                .Build();
            TestConfiguration.Initialize(configRoot);
        }

        private static CancellationToken ConsoleCancellationToken(this CancellationTokenSource tokenSource)
        {
            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("Canceling...");
                tokenSource.Cancel();
                e.Cancel = true;
            };

            return tokenSource.Token;
        }

        private static async Task SafeWaitAsync(this Task task,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await task.WaitAsync(cancellationToken);
                Console.WriteLine("== DONE ==");
            }
            catch (ConfigurationNotFoundException ex)
            {
                Console.WriteLine($"{ex.Message}. Skipping example.");
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}