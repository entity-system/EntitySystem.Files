using EntitySystem.Client.Files.Components.Data.Input.Upload.Factory;
using EntitySystem.Client.Files.Feature.DownloadLink;
using EntitySystem.Client.Files.Feature.ZipButton;
using EntitySystem.Client.Files.Services;
using EntitySystem.Shared.Files.Domain;
using EntitySystem.Shared.Files.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EntitySystem.Client.Files;

public static class EntitySystemClientFilesRegistrations
{
    public static IServiceCollection AddEntitySystemClientFiles<TFileReference, TFileReferenceService, TPersistentFile, TPersistentFileService, TPersistentInfo, TPersistentInfoService>(this IServiceCollection serviceCollection)
        where TFileReference : IFileReference, new()
        where TFileReferenceService : class, IFileReferenceService<TFileReference>
        where TPersistentFile : IPersistentFile, new()
        where TPersistentFileService : class, IPersistentFileService<TPersistentFile>
        where TPersistentInfo : IPersistentInfo<TFileReference, TPersistentFile>, new()
        where TPersistentInfoService : class, IPersistentInfoService<TFileReference, TPersistentFile, TPersistentInfo>
    {
        return serviceCollection
            .AddSingleton<IFileStreamService<TFileReference>, FileStreamService<TFileReference>>()
            .AddSingleton<IFileZipService, FileZipService>()
            .AddSingleton<IFileZipDownloadService<TFileReference>, FileZipDownloadService<TFileReference>>()
            .AddSingleton<IFileReferenceService<TFileReference>, TFileReferenceService>()
            .AddSingleton<IPersistentFileService<TPersistentFile>, TPersistentFileService>()
            .AddSingleton<IPersistentInfoService<TFileReference, TPersistentFile, TPersistentInfo>, TPersistentInfoService>()
            .AddSingleton<IDataInputUploadFactory, DataInputUploadFactory>()
            .AddSingleton<IDataOutputEnableDownloadFactory, DataOutputEnableDownloadFactory>()
            .AddSingleton<IDataRecordListZipButtonProcessor, DataRecordListZipButtonProcessor<TFileReference>>();
    }
}