namespace CyberbianSite.Client.Config
{
    public class AzureOptions
    {
        public string StorageConnectionString { get; set; }
        public string StorageRootContainerName { get; set; }
        public string StorageImagesContainerName { get; set; }
        public string CDNEndpointHostname { get; set; }

    }
}
