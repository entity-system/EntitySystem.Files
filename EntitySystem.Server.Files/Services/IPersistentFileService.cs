using EntitySystem.Server.Services;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Server.Files.Services;

    public interface IPersistentFileService<TPersistentFile> : IEntityService<TPersistentFile>
        where TPersistentFile : class, IPersistentFile
    {
    }

