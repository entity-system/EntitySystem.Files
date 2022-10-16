using EntitySystem.Server.Controllers;
using EntitySystem.Server.Files.Controllers;
using EntitySystem.Server.Files.Services;
using EntitySystem.Shared.Files.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace EntitySystem.Server.Files;

public static class EntitySystemServerFilesRegistrations
{
    public static IServiceCollection AddEntitySystemServerFiles<TFileController, TFileService, TFileReference, TFileReferenceService, TFileReferenceController, TPersistentFile, TPersistentFileService, TPersistentFileController, TPersistentInfo, TPersistentInfoService, TPersistentInfoController>(this IServiceCollection serviceCollection)
        where TFileController : FileController<TFileReference>
        where TFileService : class, IFileService<TFileReference>
        where TFileReference : class, IFileReference
        where TFileReferenceService : class, IFileReferenceService<TFileReference>
        where TFileReferenceController : EntityController<TFileReferenceService, TFileReference>
        where TPersistentFile : class, IPersistentFile
        where TPersistentFileService : class, IPersistentFileService<TPersistentFile>
        where TPersistentFileController : EntityController<TPersistentFileService, TPersistentFile>
        where TPersistentInfo : class, IPersistentInfo<TFileReference, TPersistentFile>
        where TPersistentInfoService : class, IPersistentInfoService<TFileReference, TPersistentFile, TPersistentInfo>
        where TPersistentInfoController : EntityController<TPersistentInfoService, TPersistentInfo>
    {
        return serviceCollection
            .AddScoped<IFileService<TFileReference>, TFileService>()
            .AddScoped<IFileReferenceService<TFileReference>, TFileReferenceService>()
            .AddScoped<IPersistentFileService<TPersistentFile>, TPersistentFileService>()
            .AddScoped<IPersistentInfoService<TFileReference, TPersistentFile, TPersistentInfo>, TPersistentInfoService>();
    }
}