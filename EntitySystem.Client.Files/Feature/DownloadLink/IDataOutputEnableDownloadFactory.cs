using System;
using EntitySystem.Client.Abstract.Domain.Registrations;
using EntitySystem.Client.Components.Data.Output.Feature.LinkFormatter;
using EntitySystem.Shared.Domain;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Feature.DownloadLink;

public interface IDataOutputEnableDownloadFactory : IRegistrable
{
    DataOutputEnableLink<TEntity> Build<TEntity, TSourceFileReference>(Func<TEntity, TSourceFileReference> fileGetter)
        where TEntity : IEntity, new()
        where TSourceFileReference : IFileReference;
}