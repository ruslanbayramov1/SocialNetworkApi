using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Zust.BL.Enums;
using Zust.BL.Exceptions.Common;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Options;
using Zust.Core.Entities;
using Zust.Core.Interfaces.Repositories;

namespace Zust.BL.ExternalServices.Implements;

public class AzureBlobCloudService : IAzureCloudBlobService
{
    private readonly BlobServiceClient _client;
    private readonly BlobContainerClient _containerClient;
    private readonly AzureOption _opt;
    private readonly IUserClaimService _userClaimService;
    private readonly IUserRepository _userRepo;
    public AzureBlobCloudService(IOptions<AzureOption> options, IUserClaimService userClaimsService, IUserRepository userRepo)
    {
        _userClaimService = userClaimsService;
        _userRepo = userRepo;
        
        _opt = options.Value;

        _client = new($"{_opt.Connection.Replace("ACCOUNT_KEY", _opt.AccountKey).Replace("ACCOUNT_NAME", _opt.AccountName)}");

        _containerClient = _client.GetBlobContainerClient(_opt.ContainerName);
    }

    public async Task<string> UploadImageAsync(IFormFile file, AzureFolderDestinations destFolderName)
    {
        await _containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var user = await _userRepo.GetByIdAsync(_userClaimService.GetId());
        if (user == null) throw new NotFoundException<User>();

        string userFolder = $"users/{user.UserName}/{destFolderName.ToString().ToLower()}/";
        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        BlobClient blobClient = _containerClient.GetBlobClient(userFolder + fileName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
        }

        return blobClient.Uri.ToString();
    }

    public async Task DeleteImageAsync(string path)
    {
        Uri uri = new Uri(path);
        string blobPath = string.Join("", uri.Segments.Skip(2));

        BlobClient blobClient = _containerClient.GetBlobClient(blobPath);
        await blobClient.DeleteIfExistsAsync();
    }
}
