using System;
using System.Collections.Generic;
using System.Linq;
using EntitySystem.Client.Abstract.Domain.Renderer;
using EntitySystem.Client.Components.Data.Record.List;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Feature.ZipButton;

    public class DataRecordListZipButtonFeature<TKey, TFileReference> : IDataRecordListZipButtonFeature<TKey>
        where TFileReference : IFileReference
    {
        public const long Priority = 25;

        private readonly Func<TKey, TFileReference> _fileReferenceGetter;

        public DataRecordListZipButtonFeature(Func<TKey, TFileReference> fileReferenceGetter)
        {
            _fileReferenceGetter = fileReferenceGetter;
        }

        public IEnumerable<IRenderer> Build(BaseDataRecordList<TKey> recordList)
        {
            if (!recordList.Selected.Any()) yield break;

            var parameters = new DataRecordListZipButtonParameters<TKey, TFileReference>(recordList, _fileReferenceGetter);

            yield return new Renderer<DataRecordListZipButtonParameters<TKey, TFileReference>, DataRecordListZipButton<TKey, TFileReference>>(parameters, Priority);
        }
    }
