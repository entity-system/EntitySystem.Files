using System.Threading.Tasks;
using EntitySystem.Client.Abstract.Domain.Renderer;
using EntitySystem.Client.Components.Data.Input.Options;
using EntitySystem.Client.Domain.Data.Property;
using EntitySystem.Client.Domain.Data.Source;
using EntitySystem.Client.Files.Components.Data.Input.Upload.Parameters;
using EntitySystem.Client.Files.Components.Data.Input.Upload.Property;
using EntitySystem.Client.Files.Domain.Data.Property.Extensions;
using EntitySystem.Client.Files.Services;
using EntitySystem.Shared.Files.Domain;
using Microsoft.AspNetCore.Components.Forms;

namespace EntitySystem.Client.Files.Domain.Data.Property.Types;

public class DataPropertyUpload<TFileReference, TFileReferenceService> : DataProperty<TFileReference, TFileReference>, IDataInputUploadProperty<TFileReference>
    where TFileReference : IFileReference, new()
    where TFileReferenceService : IFileReferenceService<TFileReference>
{
    private readonly TFileReferenceService _fileReferenceService;

    public IBrowserFile BrowserFile { get; set; }

    public DataPropertyUpload(DataSource<TFileReference, TFileReferenceService> source) : base(source, f => f)
    {
        _fileReferenceService = source.EntityService;
    }

    public string GetLink(TFileReference fileReference)
    {
        return _fileReferenceService.GetDownloadUri(fileReference);
    }

    public override async Task OnAfterSaveOrUpdateAsync(TFileReference fileReference)
    {
        if (BrowserFile == null) return;

        var uploaded = await _fileReferenceService.UploadAsync(fileReference, BrowserFile);

        fileReference.Name = uploaded.Name;
    }

    public override void Reset()
    {
        BrowserFile = null;
    }

    public override IRenderer BuildInput(TFileReference entity, IDataInputOptions options)
    {
        var parameters = new DataInputUploadParameters<TFileReference>(RegistrationProvider, this, entity, options);

        return BuildInput(parameters);
    }
}