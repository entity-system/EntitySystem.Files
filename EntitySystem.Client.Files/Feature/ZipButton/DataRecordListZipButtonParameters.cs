using System;
using EntitySystem.Client.Abstract.Domain.Feature;
using EntitySystem.Client.Abstract.Domain.Parameters;
using EntitySystem.Client.Components.Data.Record.List;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Feature.ZipButton;

    public class DataRecordListZipButtonParameters<TKey, TFileReference> : Featured, IParameters
        where TFileReference : IFileReference
    {
        public BaseDataRecordList<TKey> RecordList { get; }

        public Func<TKey, TFileReference> FileReferenceGetter { get; }

        public DataRecordListZipButtonParameters(BaseDataRecordList<TKey> recordList, Func<TKey, TFileReference> fileReferenceGetter)
        {
            RecordList = recordList;
            FileReferenceGetter = fileReferenceGetter;
        }
    }
