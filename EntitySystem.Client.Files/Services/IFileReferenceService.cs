using EntitySystem.Client.Services;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Services;

public interface IFileReferenceService<TFileReference> : IEntityService<TFileReference>
    where TFileReference : IFileReference
{
    string GetUploadUri(TFileReference file);

    string GetDownloadUri(TFileReference file);
}