using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;


namespace AzureExtended.MyDemos;

internal class SasTokenDemo
{

    public static void SasDemo()
    {
        BlobContainerClient container = new BlobContainerClient(
            AzureConstansts._storageConnectionString, 
            AzureConstansts._containerName);
        foreach (BlobItem blobItem in container.GetBlobs())
        {
            BlobClient blob = container.GetBlobClient(blobItem.Name);
            BlobSasBuilder sas = new BlobSasBuilder
            {
                BlobContainerName = blob.BlobContainerName,
                BlobName = blob.Name,
                Resource = "b",  //blob
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            // Allow read access
            sas.SetPermissions(BlobSasPermissions.Read);
            var storageSharedKeyCredential = new StorageSharedKeyCredential(
                AzureConstansts._accountName,  AzureConstansts._accountKey);
            string sasToken = sas.ToSasQueryParameters(
                storageSharedKeyCredential).ToString();
            Console.WriteLine($"SAS created {sasToken}");

            string totalUrl = "https://"
                + AzureConstansts._accountName
                + ".blob.core.windows.net/"
                + AzureConstansts._containerName
                + "/"
                + AzureConstansts._fileNameImage
                + "?"
                + sasToken;
            Console.WriteLine($"URI with SAS created {totalUrl}");
        }
    }
}

