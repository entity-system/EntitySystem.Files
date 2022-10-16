using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace EntitySystem.Shared.Files.Services;

    public class FileZipService : IFileZipService
    {
        public async Task<Stream> CreateZipAsync(IEnumerable<(string, Stream)> files)
        {
            var output = new MemoryStream();

            using (var archive = new ZipArchive(output, ZipArchiveMode.Create, true))
            {
                foreach (var (name, stream) in files)
                {
                    await using var entry = archive.CreateEntry(name).Open();

                    await stream.CopyToAsync(entry);

                }
            }

            output.Seek(0, SeekOrigin.Begin);

            return output;
        }
    }

