using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EntitySystem.Shared.Files.Services;

public interface IFileZipService
{
    Task<Stream> CreateZipAsync(IEnumerable<(string, Stream)> files);
}