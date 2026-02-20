namespace AZ_204_Exercises;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;

public class BlobExercise
{
    private readonly DefaultAzureCredentialOptions _defaultOptions = new()
    {
        ExcludeEnvironmentCredential = true,
        ExcludeManagedIdentityCredential = true
    };
    
    public void ProcessAsync()
    {
        var blobStorageClient = CreateBlobStorageClient(_defaultOptions);
        Console.WriteLine("Created blob storage client");
        var blobContainerClient = CreateBlobContainerClient(blobStorageClient);
        Console.WriteLine("Created blob container client");
        var filePath = CreateFileToUpload();
        Console.WriteLine("Created file to upload");
        
        var blobClient = blobContainerClient.GetBlobClient(filePath);
        var uploaded = UploadFileToBlobClient(blobClient, filePath).GetAwaiter().GetResult();
    }
    
    private static BlobServiceClient CreateBlobStorageClient(DefaultAzureCredentialOptions options)
    {
        const string accountName = "replace_with_your_storage_account_name";
        
        var blobServiceUri = new Uri($"https://{accountName}.blob.core.windows.net");
        var credential = new DefaultAzureCredential(options);
        
        var blobServiceClient = new BlobServiceClient(blobServiceUri, credential);
        
        return blobServiceClient;
    }

    private static BlobContainerClient CreateBlobContainerClient(BlobServiceClient blobServiceClient)
    {
        var containerName = "wtblob" + Guid.NewGuid();
        var containerClient = blobServiceClient.CreateBlobContainerAsync(containerName)
            .GetAwaiter()
            .GetResult()
            .Value;

        return containerClient ?? throw new InvalidOperationException($"Blob container '{containerName}' not found");
    }

    private static string CreateFileToUpload()
    {
        const string localPath = "./data/";
        var fileName = "wtfile" + Guid.NewGuid() + ".txt";
        var localFilePath = Path.Combine(localPath, fileName);

        File.WriteAllText(localFilePath, "Hello, World!");
        
        return localFilePath;
    }
    
    private static async Task<bool> UploadFileToBlobClient(BlobClient blobClient, string localFilePath)
    {
        
        Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}", blobClient.Uri);
        await using (var uploadFileStream = File.OpenRead(localFilePath))
        {
            await blobClient.UploadAsync(uploadFileStream);
            uploadFileStream.Close();
        }

        bool blobExists = await blobClient.ExistsAsync();
        if (blobExists)
        {
            Console.WriteLine("File uploaded successfully");
            return blobExists;
        }
        
        Console.WriteLine("File upload failed, exiting program..");
        return blobExists;
        
    }
}