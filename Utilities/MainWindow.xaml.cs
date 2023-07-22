using System;
using System.IO;
using System.Windows;
using System.Windows.Media.TextFormatting;

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
    }
}
