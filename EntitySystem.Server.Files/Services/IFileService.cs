using System;
using System.IO;
using System.Threading.Tasks;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Server.Files.Services;

public interface IFileService<TFileReference>
    where TFileReference : class, IFileReference
{
    Task<TFileReference> WriteFileAsync(Guid guid, string mime, string name, long size, Func<Stream> streamFactory, bool force = false);
    Task<(TFileReference file, Func<Stream> streamFactory)> ReadFileAsync(Guid guid);
    Task<Func<Stream>> ReadFileAsync(TFileReference fileReference);
}