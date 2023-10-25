using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage.Blobs.Specialized;

namespace AzureExtended.MyDemos;

internal class SasTokenDemo
{
    private static string containerName = "images75886e7f-2711-45b3-a675-d88e2d16cf2d";

    public static void SasDemo()
    {
        BlobContainerClient container = new BlobContainerClient(
            AzureConstansts._storageConnectionString,
            containerName);

        foreach (BlobItem blobItem in container.GetBlobs())
        {
            BlobClient blob = container.GetBlobClient(blobItem.Name);
            Uri? blobSASURI = CreateServiceSASBlob(blob);
            Console.WriteLine($"URI with SAS created {blobSASURI}");
        }

    }

    public static Uri? CreateServiceSASBlob(
    BlobClient blobClient,
    string? storedPolicyName = null)
    {
        // Check if BlobContainerClient object has been authorized with Shared Key
        if (blobClient.CanGenerateSasUri)
        {
            // Create a SAS token that's valid for one day
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                BlobName = blobClient.Name,
                Resource = "b"
            };

            if (storedPolicyName == null)
            {
                sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddDays(1);
                sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
            }
            else
            {
                sasBuilder.Identifier = storedPolicyName;
            }

            Uri sasURI = blobClient.GenerateSasUri(sasBuilder);

            return sasURI;
        }
        else
        {
            // Client object is not authorized via Shared Key
            return null;
        }
    }

}

