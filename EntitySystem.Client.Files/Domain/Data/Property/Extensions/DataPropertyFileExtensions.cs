using System;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EntitySystem.Client.Components.Data.Record.List.Feature.HideFilter;
using EntitySystem.Client.Domain.Data.Property.Extensions;
using EntitySystem.Client.Domain.Data.Property.Extensions.Settings;
using EntitySystem.Client.Domain.Data.Source;
using EntitySystem.Client.Domain.Data.Source.Extensions.Settings;
using EntitySystem.Client.Domain.Data.Target.Extensions.Settings;
using EntitySystem.Client.Files.Domain.Data.Property.Types;
using EntitySystem.Client.Files.Feature.DownloadLink;
using EntitySystem.Client.Files.Services;
using EntitySystem.Client.Services;
using EntitySystem.Shared.Domain;
using EntitySystem.Shared.Files.Domain;
using Microsoft.AspNetCore.Components.Forms;

namespace EntitySystem.Client.Files.Domain.Data.Property.Extensions;

public static class DataPropertyFileExtensions
{
    public static DataSource<TEntity, TEntityService> FileInfo<TEntity, TEntityService, TFileReference>(this DataSource<TEntity, TEntityService> source, Expression<Func<TEntity, TFileReference>> file)
        where TEntity : IEntity, new()
        where TEntityService : IEntityService<TEntity>
        where TFileReference : IFileReference, new()
    {
        source
            .LinkDownload(file.Compile())
            .FileTarget(file)
            .DisableEdit()
            .DisableDelete();

        return source;
    }

    private static DataSource<TEntity, TEntityService> FileTarget<TEntity, TEntityService, TOriginalFileReference>(this DataSource<TEntity, TEntityService> source, Expression<Func<TEntity, TOriginalFileReference>> file)
        where TEntity : IEntity, new()
        where TEntityService : IEntityService<TEntity>
        where TOriginalFileReference : IFileReference, new()
    {
        return source.Target<TOriginalFileReference, IFileReferenceService<TOriginalFileReference>>("File", file, f => f.Name, df => df.FileInfo(), t => t.ImplicitCreate());
    }

    public static DataSource<TFileReference, TFileReferenceService> FileInfo<TFileReference, TFileReferenceService>(this DataSource<TFileReference, TFileReferenceService> source)
        where TFileReference : IFileReference, new()
        where TFileReferenceService : IFileReferenceService<TFileReference>
    {
        return source
                .LinkDownload()
                .Upload("Upload file", o => o.HideInEditDialog())
                .Property("File name", f => f.Name, o => o.HideInEntityDialog())
            /*.Size("File size")
            .Property("Uploaded", f => f.TimeStamp, o => o.HidePropertyInEntityDialog())
            .Property("Author", f => f.Author, o => o.HidePropertyInEntityDialog())*/;
    }

    public static DataSource<TSourceFileReference, TSourceFileReferenceService> LinkDownload<TSourceFileReference, TSourceFileReferenceService>(this DataSource<TSourceFileReference, TSourceFileReferenceService> source)
        where TSourceFileReference : IFileReference, new()
        where TSourceFileReferenceService : IFileReferenceService<TSourceFileReference>
    {
        return source.LinkDownload(f => f);
    }

    public static DataSource<TEntity, TEntityService> LinkDownload<TEntity, TEntityService, TSourceFileReference>(this DataSource<TEntity, TEntityService> source, Func<TEntity, TSourceFileReference> fileSelector)
        where TEntity : IEntity, new()
        where TEntityService : IEntityService<TEntity>
        where TSourceFileReference : IFileReference, new()
    {
        var factory = source.RegistrationProvider.GetRegistration<IDataOutputEnableDownloadFactory>();

        var feature = factory.Build(fileSelector);

        source.Features.AddFeature(feature);

        return source;
    }

    public static DataSource<TFileReference, TFileReferenceService> Upload<TFileReference, TFileReferenceService>(this DataSource<TFileReference, TFileReferenceService> source, string name, Action<DataPropertyUpload<TFileReference, TFileReferenceService>> options = null)
        where TFileReference : IFileReference, new()
        where TFileReferenceService : IFileReferenceService<TFileReference>
    {
        var property = new DataPropertyUpload<TFileReference, TFileReferenceService>(source);

        property.Features.AddFeature<DataRecordListHidePropertyFilter>();

        options?.Invoke(property);

        DataPropertyExtensions.Property(source, property, name);

        return source;
    }

    public static async Task<TFileReference> UploadAsync<TFileReference>(this IFileReferenceService<TFileReference> service, TFileReference fileReference, IBrowserFile file)
        where TFileReference : IFileReference
    {
        return await UploadAsync(service, fileReference, file.Name, file.ContentType, file.OpenReadStream(1073741824L));
    }

    public static async Task<TFileReference> UploadAsync<TFileReference>(this IFileReferenceService<TFileReference> service, TFileReference fileReference, string name, string contentType, Stream stream)
        where TFileReference : IFileReference
    {
        var streamContent = new StreamContent(stream);

        streamContent.Headers.ContentType = new MediaTypeHeaderValue(string.IsNullOrEmpty(contentType) ? "application/octet-stream" : contentType);

        var content = new MultipartFormDataContent
        {
            { streamContent, "File", name }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, service.GetUploadUri(fileReference)) { Content = content };

        var response = await service.LoadAsync(nameof(UploadAsync), request);

        var result = await response.Content.ReadFromJsonAsync<TFileReference>();

        return result;
    }
}