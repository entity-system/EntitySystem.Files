using System;
using System.Net;
using System.Threading.Tasks;
using EntitySystem.Server.Attributes;
using EntitySystem.Server.Files.Services;
using EntitySystem.Shared.Files.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace EntitySystem.Server.Files.Controllers;

[ApiController]
public abstract class FileController<TFileReference> : ControllerBase
    where TFileReference : class, IFileReference
{
    private readonly IFileService<TFileReference> _fileService;

    protected FileController(IServiceProvider serviceProvider)
    {
        _fileService = serviceProvider.GetService<IFileService<TFileReference>>();
    }

    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Consumes("multipart/form-data")]
    [RequestFormLimits(MultipartBodyLengthLimit = 1073741824L)]
    [RequestSizeLimit(1073741824L)]
    [Produces("application/json")]
    [HttpPost("{guid:Guid}")]
    [Transaction]
    public virtual async Task<IActionResult> Upload(Guid guid, [FromForm] IFormFile file)
    {
        var result = await _fileService.WriteFileAsync(guid, file.ContentType, file.FileName, file.Length, file.OpenReadStream);

        return new ObjectResult(result);
    }

    [AllowAnonymous]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpGet("{guid:Guid}")]
    [Session]
    public virtual async Task<IActionResult> Download(Guid guid)
    {
        var (file, streamFactory) = await _fileService.ReadFileAsync(guid);

        return File(streamFactory(), file.Mime, file.Name);
    }
}