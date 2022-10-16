using EntitySystem.Server.Services;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Server.Files.Services;

public interface IPersistentInfoService<TFileReference, TPersistentFile, TPersistentInfo> : IEntityService<TPersistentInfo>
    where TFileReference : IFileReference
    where TPersistentFile : IPersistentFile
    where TPersistentInfo : class, IPersistentInfo<TFileReference, TPersistentFile>
{
}