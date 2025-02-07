using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Zust.BL.Exceptions.Common;
using Zust.BL.ExternalServices.Interfaces;
using Zust.BL.Options;
using Zust.Core.Entities;

namespace Zust.BL.ExternalServices.Implements;

public class AzureBlobCloudService : IAzureCloudBlobService
{
    //private readonly BlobServiceClient _client;
    //private readonly BlobContainerClient _containerClient;
    //private readonly AzureOption _opt;
    //private readonly IUserClaimsService _userClaimsService;
    //private readonly UserManager<User> _userManager;
    //public AzureBlobCloudService(IOptions<AzureOption> options, IUserClaimsService userClaimsService)
    //{
    //    _userClaimsService = userClaimsService;
    //    _userManager = userManager;

    //    _opt = options.Value;

    //    _client = new($"{_opt.Connection.Replace("ACCOUNT_KEY", _opt.AccountKey).Replace("ACCOUNT_NAME", _opt.AccountName)}");

    //    _containerClient = _client.GetBlobContainerClient(_opt.ContainerName);
    //}

    //public async Task<string> UploadImageAsync(IFormFile file)
    //{
    //    await _containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

    //    var user = await _userManager.FindByIdAsync(_userClaimsService.GetUserId());
    //    if (user == null) throw new NotFoundException<User>();
        
    //    string userFolder = $"users/{user.UserName}/";
    //    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
    //    BlobClient blobClient = _containerClient.GetBlobClient(userFolder + fileName);

    //    using (var stream = file.OpenReadStream())
    //    {
    //        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
    //    }

    //    return blobClient.Uri.ToString();
    //}
}
