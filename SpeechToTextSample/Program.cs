using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace SpeechToTextSample
{
    class Program
    {
        // This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
        //static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        //static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");
        static string speechKey = "19dc69d8572e4eab88a6059f0b87a30e";
        static string speechRegion = "eastus";

        static void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
        {
            switch (speechRecognitionResult.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                    break;
                case ResultReason.NoMatch:
                    Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    break;
                case ResultReason.Canceled:
                    var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
            }
        }

        async static Task Main(string[] args)
        {
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechRecognitionLanguage = "en-US";

            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            while(true)
            {
                Console.WriteLine("Speak into your microphone.");
                var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
                OutputSpeechRecognitionResult(speechRecognitionResult);
                if (!string.IsNullOrEmpty(speechRecognitionResult.Text) &&
                    speechRecognitionResult.Text.IndexOf("end", StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    break;
                }
            }
        }
    }
}