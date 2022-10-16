using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntitySystem.Client.Abstract.Services;
using EntitySystem.Shared.Files.Domain;
using EntitySystem.Shared.Files.Services;

namespace EntitySystem.Client.Files.Services;

    public class FileZipDownloadService<TFileReference> : IFileZipDownloadService<TFileReference> where TFileReference : IFileReference
    {
        private readonly IFileStreamService<TFileReference> _fileStreamService;
        private readonly IFileZipService _fileZipService;
        private readonly IDownloadService _downloadService;

        public FileZipDownloadService(IFileStreamService<TFileReference> fileStreamService, IFileZipService fileZipService, IDownloadService downloadService)
        {
            _fileStreamService = fileStreamService;
            _fileZipService = fileZipService;
            _downloadService = downloadService;
        }

        public async Task DownloadZipAsync(IEnumerable<TFileReference> fileReferences)
        {
            var download = fileReferences.Select(async r => (r.Name, await _fileStreamService.GetFileStreamAsync(r)));

            var files = await Task.WhenAll(download);

            var zip = await _fileZipService.CreateZipAsync(files);

            var name = _downloadService.CreateFileName(".zip");

            await _downloadService.DownloadStreamAsync(name, zip);
        }
    }
