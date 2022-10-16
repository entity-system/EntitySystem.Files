using System;
using EntitySystem.Client.Components.Data.Output.Feature.LinkFormatter;
using EntitySystem.Client.Files.Services;
using EntitySystem.Shared.Domain;
using EntitySystem.Shared.Files.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace EntitySystem.Client.Files.Feature.DownloadLink;

public class DataOutputEnableDownloadFactory : IDataOutputEnableDownloadFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DataOutputEnableDownloadFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public DataOutputEnableLink<TEntity> Build<TEntity, TFileReference>(Func<TEntity, TFileReference> fileGetter)
        where TEntity : IEntity, new()
        where TFileReference : IFileReference
    {
        var service = _serviceProvider.GetService<IFileReferenceService<TFileReference>>() ?? throw new InvalidOperationException("Unable to find file reference service.");

        return new DataOutputEnableDownload<TEntity, TFileReference>(fileGetter, service.GetDownloadUri);
    }
}