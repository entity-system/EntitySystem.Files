using EntitySystem.Shared.Domain;

namespace EntitySystem.Shared.Files.Domain;

public interface IPersistentFile : IEntity
{
    string Mime { get; set; }

    string Name { get; set; }

    string Storage { get; set; }

    long Size { get; set; }

    string Hash { get; set; }
}