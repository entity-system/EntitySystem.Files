using EntitySystem.Shared.Domain;

namespace EntitySystem.Shared.Files.Domain;

public interface IFileReference : IEntity, IUnique
{
    string Mime { get; set; }

    string Name { get; set; }

    long Size { get; set; }
}