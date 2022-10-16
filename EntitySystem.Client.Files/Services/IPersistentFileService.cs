using EntitySystem.Client.Services;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Services;

public interface IPersistentFileService<TPersistentFile> : IEntityService<TPersistentFile>
    where TPersistentFile : IPersistentFile
{
}