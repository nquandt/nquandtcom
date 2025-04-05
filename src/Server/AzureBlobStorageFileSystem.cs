

using Femur.FileSystem;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Server;

public class AzureBlobStorageFileSystem : IFileSystem
{
    private readonly BlobContainerClient _blobContainerClient;

    public AzureBlobStorageFileSystem(BlobContainerClient blobContainerClient)
    {
        _blobContainerClient = blobContainerClient;
        _blobContainerClient.CreateIfNotExists();
    }

    public Task CreateDirectoryAsync(string directoryPath, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task DeleteDirectoryAsync(string directoryPath, bool recursive = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DirectoryExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> FileExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        var blobClient = _blobContainerClient.GetBlobClient(path);

        var exists = await blobClient.ExistsAsync(cancellationToken);

        return exists.Value;
    }

    public Task<IEnumerable<string>> GetDirectoriesAsync(string directoryPath, bool recursive = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> GetFilesAsync(string directoryPath, bool recursive = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Stream> OpenReadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var blobClient = _blobContainerClient.GetBlobClient(filePath);

        return await blobClient.OpenReadAsync(new BlobOpenReadOptions(false), cancellationToken);
    }

    public async Task WriteAsync(string filePath, Stream data, bool overwrite = true, CancellationToken cancellationToken = default)
    {
        var blobClient = _blobContainerClient.GetBlobClient(filePath);

        await blobClient.UploadAsync(data, true, cancellationToken);
    }
}
