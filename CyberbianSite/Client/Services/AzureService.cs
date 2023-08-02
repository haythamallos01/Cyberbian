using Azure.Storage.Blobs;
using CyberbianSite.Client.Config;
using CyberbianSite.Client.Models.Azure;
using CyberbianSite.Client.Models.DID.Clips;
using Microsoft.Extensions.Options;

namespace CyberbianSite.Client.Services
{
    public class AzureService
    {
        private readonly IOptions<AzureOptions> _options;

        public AzureService(IOptions<AzureOptions> options)
        {
            _options = options;
        }

        //public async Task<AzureStorageResponse> UploadBlob(MemoryStream ms, string fileName)
        //{
        //    AzureStorageResponse azureStorageResponse = null;

        //    BlobServiceClient blobServiceClient = new BlobServiceClient(_options.Value.StorageConnectionString);
        //    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(storageImagesContainerName);
        //    if (!containerClient.Exists())
        //    {
        //        containerClient.Create();
        //        Console.WriteLine("Container created successfully.");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Container already exists.");
        //    }
           


        //    return azureStorageResponse;
        //}

    }
}
