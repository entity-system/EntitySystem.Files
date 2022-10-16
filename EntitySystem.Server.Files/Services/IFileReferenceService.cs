using EntitySystem.Server.Services;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Server.Files.Services;

    public interface IFileReferenceService<TFileReference> : IEntityService<TFileReference>, IUniqueService<TFileReference>
        where TFileReference : class, IFileReference
    {
    }

