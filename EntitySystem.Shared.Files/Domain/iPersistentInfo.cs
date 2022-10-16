using System.ComponentModel.DataAnnotations;
using EntitySystem.Shared.Domain;

namespace EntitySystem.Shared.Files.Domain;

public interface IPersistentInfo<TFileReference, TPersistentFile> : IEntity
    where TFileReference : IFileReference
    where TPersistentFile : IPersistentFile
{
    [Required]
    [ValidateComplexType]
    TFileReference Reference { get; set; }

    [Required]
    [ValidateComplexType]
    TPersistentFile File { get; set; }
}