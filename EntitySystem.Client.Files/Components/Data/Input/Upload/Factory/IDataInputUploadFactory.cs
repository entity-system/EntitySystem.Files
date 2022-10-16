using EntitySystem.Client.Abstract.Domain.Registrations;
using EntitySystem.Client.Abstract.Domain.Renderer;
using EntitySystem.Client.Components.Data.Input;
using EntitySystem.Client.Files.Components.Data.Input.Upload.Property;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Components.Data.Input.Upload.Factory;

public interface IDataInputUploadFactory : IRegistrable
{
    IRenderer Build<TFileReference>(BaseDataInput<IDataInputUploadProperty<TFileReference>, TFileReference, TFileReference> input) where TFileReference : IFileReference;
}