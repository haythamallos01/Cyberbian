using System;
using System.IO;
using System.Windows;
using System.Windows.Media.TextFormatting;

using Azure;
using Azure.Communication.Email;
using Cyberbian.Common;

namespace Utilities
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentDir;
        private string outputDir;
        private string dataDir;

        public MainWindow()
        {
            InitializeComponent();
            currentDir = AppContext.BaseDirectory;
            outputDir = Path.Combine(currentDir, "Output");
            dataDir = Path.Combine(currentDir, "Data");

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

        }

        private void parseLinksButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filePath = Path.Combine(dataDir, "MrBeastWikiReferences.html");
                string text = File.ReadAllText(filePath);
                int ind = text.IndexOf("href=\"http");
                string links = string.Empty;
                while (ind > -1)
                {
                    //text = text.Substring(0, ind);
                    int tmpInd = text.IndexOf("http", ind);
                    for (int i = tmpInd; i < text.Length; i++)
                    {
                        if (text[i] == '"')
                        {
                            links += Environment.NewLine;
                            break;
                        }
                        links += text[i];
                    }
                    ind = text.IndexOf("href=\"https", ind+1);
                }
                var resultPath = Path.Combine(outputDir, "links.txt");
                System.IO.File.WriteAllText(resultPath, links);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void talkingAIButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void sendEmailAzureButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // This code demonstrates how to fetch your connection string
                // from an environment variable.
                string connectionString = "endpoint=https://cyberbian-communication-services-main.unitedstates.communication.azure.com/;accesskey=nn4HLpUjL6PY55A5l6cMIzbDBHPc/6hdEA5MjDjJjMCNsZ8DuoumtMgu+lygv6bw/eITwWlfB3nm9+IrAev/KA==";
                EmailClient emailClient = new EmailClient(connectionString);

                //Replace with your domain and modify the content, recipient details as required

                var subject = "Welcome to Azure Communication Service Email APIs.";
                var htmlContent = "<html><body><h1>Quick send email test</h1><br/><h4>This email message is sent from Azure Communication Service Email.</h4><p>This mail was sent using .NET SDK!!</p></body></html>";
                var senderEmail = "donotreply@9a452696-680b-4fd4-989a-32ec7cf7605e.azurecomm.net";
                var recipient = "haytham.allos@gmail.com";

                try
                {
                    Console.WriteLine("Sending email...");
                    EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                        Azure.WaitUntil.Completed,
                        senderEmail,
                        recipient,
                        subject,
                        htmlContent);
                    EmailSendResult statusMonitor = emailSendOperation.Value;

                    Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                    /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                    string operationId = emailSendOperation.Id;
                    Console.WriteLine($"Email operation id = {operationId}");
                }
                catch (RequestFailedException ex)
                {
                    /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                    Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void stringHelperButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string randomText = StringHelper.RandomString(9);
                MessageBox.Show(randomText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
