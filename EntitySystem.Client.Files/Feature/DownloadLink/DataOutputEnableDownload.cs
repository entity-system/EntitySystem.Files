using System;
using EntitySystem.Client.Components.Data.Output.Feature.LinkFormatter;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Feature.DownloadLink;

    public class DataOutputEnableDownload<TEntity, TFileReference> : DataOutputEnableLink<TEntity>
        where TFileReference : IFileReference
    {
        public Func<TEntity, TFileReference> FileReferenceGetter { get; }

        public DataOutputEnableDownload(Func<TEntity, TFileReference> fileReferenceGetter, Func<TFileReference, string> linkFactory) : base(e => linkFactory(fileReferenceGetter(e)), "_blank")
        {
            FileReferenceGetter = fileReferenceGetter;
        }
    }
