using System.Collections.Generic;
using System.Threading.Tasks;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Services;

public interface IFileZipDownloadService<in TFileReference> where TFileReference : IFileReference
{
    Task DownloadZipAsync(IEnumerable<TFileReference> fileReferences);
}