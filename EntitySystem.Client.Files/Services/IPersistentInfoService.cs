using EntitySystem.Client.Services;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Services;

public interface IPersistentInfoService<TFileReference, TPersistentFile, TPersistentInfo> : IEntityService<TPersistentInfo>
    where TFileReference : IFileReference
    where TPersistentFile : IPersistentFile
    where TPersistentInfo : IPersistentInfo<TFileReference, TPersistentFile>
{
}