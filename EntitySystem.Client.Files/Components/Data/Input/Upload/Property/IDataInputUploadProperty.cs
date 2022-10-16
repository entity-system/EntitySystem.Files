using EntitySystem.Client.Components.Data.Input.Property;
using EntitySystem.Shared.Files.Domain;
using Microsoft.AspNetCore.Components.Forms;

namespace EntitySystem.Client.Files.Components.Data.Input.Upload.Property;

public interface IDataInputUploadProperty<TFileReference> : IDataInputProperty<TFileReference, TFileReference>
    where TFileReference : IFileReference
{
    IBrowserFile BrowserFile { get; set; }

    string GetLink(TFileReference fileReference);
}