using Azure.Core.GeoJson;
using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.DTOs;
using Forge.Features.MetaSchema.Entities;
using Forge.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Features.MetaSchema.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MetaSchemaController : ControllerBase
{
    private readonly IMetaSchemaAuthoringService _authoringService;
    private readonly IMetaSchemaService _metaSchemaService;

    public MetaSchemaController(IMetaSchemaAuthoringService authoringService, IMetaSchemaService metaSchemaService)
    {
        _authoringService = authoringService;
        _metaSchemaService = metaSchemaService;
    }

    [HttpGet("objects/{uuid}")]
    [ProducesResponseType(typeof(ApiResponse<MetaObject>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetObject(
        Guid uuid,
        CancellationToken cancellationToken)
    {
        var result = await _metaSchemaService.GetObjectAsync(
            uuid,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(ApiResponse<MetaObject>.Success(
            result,
            "MetaObject retrieved successfully."));
    }

    [HttpGet("objects")]
    [ProducesResponseType(typeof(ApiResponse<MetaObject>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetObjectByName(
        string name,
        CancellationToken cancellationToken)
    {
        var result = await _metaSchemaService.GetObjectByNameAsync(
            name,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(ApiResponse<MetaObject>.Success(
            result,
            "MetaObject retrieved successfully."));
    }

    [HttpPost("objects")]
    [ProducesResponseType(typeof(ApiResponse<MetaObject>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateObject(
        [FromBody] MetaObject request,
        CancellationToken cancellationToken)
    {
        var result = await _authoringService.CreateObjectAsync(
            request,
            cancellationToken);

        return StatusCode(
        StatusCodes.Status201Created,
        ApiResponse<MetaObject>.Success(
            result,
            "MetaObject created successfully."));
    }

    [HttpPut("objects")]
    [ProducesResponseType(typeof(ApiResponse<MetaObject>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateObject(
       [FromBody] UpdateMetaObjectRequest request,
       CancellationToken cancellationToken)
    {
        var result = await _authoringService.UpdateObjectAsync(
            request,
            cancellationToken);

        return Ok(ApiResponse<MetaObject>.Success(
            result,
            "MetaObject updated successfully."));
    }

    [HttpPatch("objects/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ActivateObject(
        [FromBody] UuidRequest request,
        CancellationToken cancellationToken)
    {
        await _authoringService.ActivateObjectAsync(
            request,
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("objects/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeactivateObject(
        [FromBody] UuidRequest request,
        CancellationToken cancellationToken)
    {
        await _authoringService.DeactivateObjectAsync(
            request,
            cancellationToken);

        return NoContent();
    }

    [HttpGet("relationships/{uuid}")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationship>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRelationship(
       Guid uuid,
       CancellationToken cancellationToken)
    {
        var result = await _metaSchemaService.GetRelationshipAsync(
            uuid,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(ApiResponse<MetaObjectRelationship>.Success(
            result,
            "MetaRelationship retrieved successfully."));
    }

    [HttpGet("relationships")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationship>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRelationshipByName(
       string name,
       CancellationToken cancellationToken)
    {
        var result = await _metaSchemaService.GetRelationshipByNameAsync(
            name,
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(ApiResponse<MetaObjectRelationship>.Success(
            result,
            "MetaRelationship retrieved successfully."));
    }

    [HttpPost("relationships")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationship>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRelationship(
        [FromBody] MetaObjectRelationship request,
        CancellationToken cancellationToken)
    {
        var result = await _authoringService.CreateRelationshipAsync(
            request,
            cancellationToken);

        return StatusCode(
        StatusCodes.Status201Created,
        ApiResponse<MetaObjectRelationship>.Success(
            result,
            "MetaRelationship created successfully."));
    }

    [HttpPut("relationships")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationship>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRelationship(
       [FromBody] UpdateMetaObjectRelationshipRequest request,
       CancellationToken cancellationToken)
    {
        var result = await _authoringService.UpdateRelationshipAsync(
            request,
            cancellationToken);

        return Ok(ApiResponse<MetaObjectRelationship>.Success(
            result,
            "MetaRelationship updated successfully."));
    }

    [HttpPatch("relationships/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ActivateRelationship(
        [FromBody] UuidRequest request,
        CancellationToken cancellationToken)
    {
        await _authoringService.ActivateRelationshipAsync(
            request,
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("relationships/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeactivateRelationship(
        [FromBody] UuidRequest request,
        CancellationToken cancellationToken)
    {
        await _authoringService.DeactivateRelationshipAsync(
            request,
            cancellationToken);

        return NoContent();
    }
}
