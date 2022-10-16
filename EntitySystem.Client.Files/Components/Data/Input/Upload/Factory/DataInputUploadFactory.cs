using EntitySystem.Client.Abstract.Domain.Renderer;
using EntitySystem.Client.Components.Data.Input;
using EntitySystem.Client.Files.Components.Data.Input.Upload.Property;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Components.Data.Input.Upload.Factory;

public class DataInputUploadFactory : IDataInputUploadFactory
{
    public IRenderer Build<TFileReference>(BaseDataInput<IDataInputUploadProperty<TFileReference>, TFileReference, TFileReference> input)
        where TFileReference : IFileReference
    {
        return new Renderer<BaseDataInput<IDataInputUploadProperty<TFileReference>, TFileReference, TFileReference>, DataInputUpload<TFileReference>>(input);
    }
}