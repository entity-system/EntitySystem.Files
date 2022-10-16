using System.IO;
using System.Threading.Tasks;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Services;

public interface IFileStreamService<in TFileReference>
    where TFileReference : IFileReference
{
    Task<Stream> GetFileStreamAsync(TFileReference fileReference);
}