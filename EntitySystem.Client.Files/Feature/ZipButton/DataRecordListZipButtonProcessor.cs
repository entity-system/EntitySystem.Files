using EntitySystem.Client.Components.Data.Record.List.Parameters;
using EntitySystem.Client.Files.Feature.DownloadLink;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Feature.ZipButton;

    public class DataRecordListZipButtonProcessor<TFileReference> : IDataRecordListZipButtonProcessor
    where TFileReference : IFileReference
    {
        public void Process<TEntity>(IDataRecordListParameters<TEntity> parameters)
        {
            var source = parameters.Options.GetRecordListSource();

            if (source.Features.GetFeature<DataOutputEnableDownload<TEntity, TFileReference>>() is not { } enableDownloadLink) return;

            if (source.Features.HasFeature<DataRecordListDisableZipFeature>()) return;

            var feature = new DataRecordListZipButtonFeature<TEntity, TFileReference>(enableDownloadLink.FileReferenceGetter);

            parameters.Features.AddFeature(feature);
        }
    }
