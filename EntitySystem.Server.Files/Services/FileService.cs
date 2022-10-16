using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EntitySystem.Server.Exceptions;
using EntitySystem.Server.Extensions;
using EntitySystem.Shared.Abstract.Services;
using EntitySystem.Shared.Files.Domain;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;

namespace EntitySystem.Server.Files.Services;

    public class FileService<TFileReference, TPersistentFile, TPersistentInfo> : IFileService<TFileReference> where TFileReference : class, IFileReference
        where TPersistentFile : class, IPersistentFile, new()
        where TPersistentInfo : class, IPersistentInfo<TFileReference, TPersistentFile>, new()
    {
        private readonly ITimeService _timeService;
        private readonly IFileReferenceService<TFileReference> _fileReferenceService;
        private readonly IPersistentFileService<TPersistentFile> _persistentFileService;
        private readonly IPersistentInfoService<TFileReference, TPersistentFile, TPersistentInfo> _persistentInfoService;

        public FileService(IServiceProvider serviceProvider)
        {
            _timeService = serviceProvider.GetService<ITimeService>();

            _fileReferenceService = serviceProvider.GetService<IFileReferenceService<TFileReference>>();

            _persistentFileService = serviceProvider.GetService<IPersistentFileService<TPersistentFile>>();

            _persistentInfoService = serviceProvider.GetService<IPersistentInfoService<TFileReference, TPersistentFile, TPersistentInfo>>();
        }

        public async Task<TFileReference> WriteFileAsync(Guid guid, string mime, string name, long size, Func<Stream> streamFactory, bool force = false)
        {
            var fileReference = await _fileReferenceService.GetByGuidAsync(guid);

            if (fileReference.Size > 0) throw new EntityBadFriendlyException<TFileReference>("WriteFile", "Your file was not uploaded correctly. Please contact support.", "file is already saved");

            fileReference.Mime = mime ?? "application/octet-stream";

            fileReference.Name = name;

            fileReference.Size = size;

            await _fileReferenceService.SaveOrUpdateAsync(fileReference, force);

            var persistentFile = await WritePersistentFileAsync(mime, Path.GetExtension(name), size, streamFactory, force);

            await SetPersistentInfoAsync(fileReference, persistentFile, force);

            return fileReference;
        }

        public async Task<(TFileReference file, Func<Stream> streamFactory)> ReadFileAsync(Guid guid)
        {
            var fileReference = await _fileReferenceService.GetByGuidAsync(guid);

            return (fileReference, await ReadFileAsync(fileReference));
        }

        public async Task<Func<Stream>> ReadFileAsync(TFileReference fileReference)
        {
            var info = await _persistentInfoService.Query(i => i.Reference.Id, fileReference.Id).FirstOrDefaultAsync();

            if (info == null) throw new EntityBadFriendlyException<TFileReference>("ReadFile", "Problem occurred while processing your files. Please contact support.", "file is not persisted");

            var streamFactory = ReadPersistentFile(info.File);

            return streamFactory;
        }

        public async Task<TPersistentFile> WritePersistentFileAsync(string mime, string extension, long size, Func<Stream> streamFactory, bool force = false)
        {
            var stamp = $"{_timeService.GetTimeNow():yyyy-MM-dd-HH-mm-ss-fff}";

            var name = $"{stamp}{extension}";

            var storage = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var path = Path.Combine(storage, name);

            var hash = await WriteFileAndReturnHashAsync(path, streamFactory);

            if (await GetPersistentFileByHashAsync(hash) is { } existing)
            {
                File.Delete(path);

                return existing;
            }

            var file = new TPersistentFile
            {
                Mime = mime,
                Name = name,
                Storage = storage,
                Size = size,
                Hash = hash
            };

            await _persistentFileService.SaveOrUpdateAsync(file, force);

            return file;
        }

        public Func<Stream> ReadPersistentFile(TPersistentFile file)
        {
            return () => File.OpenRead(Path.Combine(file.Storage, file.Name));
        }

        public async Task<TPersistentFile> GetPersistentFileByHashAsync(string hash)
        {
            var file = await _persistentFileService.Query(f => f.Hash, hash).FirstOrDefaultAsync();

            return file;
        }

        public async Task<TPersistentInfo> SetPersistentInfoAsync(TFileReference fileReference, TPersistentFile persistentFile, bool force = false)
        {
            var info = new TPersistentInfo
            {
                Reference = fileReference,
                File = persistentFile
            };

            await _persistentInfoService.DeleteAsync(i => i.Reference.Id, true, fileReference.Id);

            await _persistentInfoService.SaveOrUpdateAsync(info, force);

            return info;
        }

        private static async Task<string> WriteFileAndReturnHashAsync(string path, Func<Stream> streamFactory)
        {
            await using var sourceStream = streamFactory();

            await using var targetStream = File.Open(path, FileMode.CreateNew, FileAccess.ReadWrite);

            await sourceStream.CopyToAsync(targetStream);

            using var sha = SHA256.Create();

            targetStream.Seek(0, SeekOrigin.Begin);

            return Convert.ToBase64String(await sha.ComputeHashAsync(targetStream));
        }
    }
