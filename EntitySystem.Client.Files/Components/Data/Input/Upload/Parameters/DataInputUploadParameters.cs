using EntitySystem.Client.Abstract.Domain.Renderer;
using EntitySystem.Client.Abstract.Providers;
using EntitySystem.Client.Components.Data.Input;
using EntitySystem.Client.Components.Data.Input.Options;
using EntitySystem.Client.Components.Data.Input.Parameters;
using EntitySystem.Client.Files.Components.Data.Input.Upload.Factory;
using EntitySystem.Client.Files.Components.Data.Input.Upload.Property;
using EntitySystem.Shared.Files.Domain;

namespace EntitySystem.Client.Files.Components.Data.Input.Upload.Parameters;

public class DataInputUploadParameters<TFileReference> : DataInputParameters<IDataInputUploadProperty<TFileReference>, TFileReference, TFileReference>
    where TFileReference : IFileReference
{
    private readonly IRegistrationProvider _registrationProvider;

    public DataInputUploadParameters(IRegistrationProvider registrationProvider, IDataInputUploadProperty<TFileReference> property, TFileReference entity, IDataInputOptions options) : base(property, entity, options)
    {
        _registrationProvider = registrationProvider;
    }

    public override IRenderer BuildInputValue(BaseDataInput<IDataInputUploadProperty<TFileReference>, TFileReference, TFileReference> input)
    {
        var factory = _registrationProvider.GetRegistration<IDataInputUploadFactory>();

        return factory.Build(input);
    }
}