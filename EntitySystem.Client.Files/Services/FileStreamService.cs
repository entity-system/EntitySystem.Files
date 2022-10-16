using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Services;

public class FileStreamService<TFileReference> : IFileStreamService<TFileReference>
    where TFileReference : IFileReference
{
    private readonly IFileReferenceService<TFileReference> _fileReferenceService;
    private readonly HttpClient _httpClient;

    public FileStreamService(IFileReferenceService<TFileReference> fileReferenceService, HttpClient httpClient)
    {
        _fileReferenceService = fileReferenceService;
        _httpClient = httpClient;
    }

    public async Task<Stream> GetFileStreamAsync(TFileReference fileReference)
    {
        var link = _fileReferenceService.GetDownloadUri(fileReference);

        return await _httpClient.GetStreamAsync(link);
    }
}
