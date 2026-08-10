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
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRequest>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

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

        var resultDTO = new MetaObjectRequest
        {
            Id = result.Id,
            Uuid = result.Uuid,
            Name = result.Name,
            DisplayName = result.DisplayName,
            Description = result.Description,
            ObjTypeUid = result.ObjTypeUid,
            ApplicationUid = result.ApplicationUid,
            Version = result.Version
        };

        return Ok(ApiResponse<MetaObjectRequest>.Success(
            resultDTO,
            "MetaObject retrieved successfully."));
    }

    [HttpGet("objects")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRequest>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

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

        var resultDTO = new MetaObjectRequest
        {
            Id = result.Id,
            Uuid = result.Uuid,
            Name = result.Name,
            DisplayName = result.DisplayName,
            Description = result.Description,
            ObjTypeUid = result.ObjTypeUid,
            ApplicationUid = result.ApplicationUid,
            Version = result.Version
        };

        return Ok(ApiResponse<MetaObjectRequest>.Success(
            resultDTO,
            "MetaObject retrieved successfully."));
    }

    [HttpPost("objects")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRequest>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRequest>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> CreateObject(
        [FromBody] MetaObject request,
        CancellationToken cancellationToken)
    {
        var result = await _authoringService.CreateObjectAsync(
            request,
            cancellationToken);

        var resultDTO = new MetaObjectRequest
        {
            Id = result.Id,
            Uuid = result.Uuid,
            Name = result.Name,
            DisplayName = result.DisplayName,
            Description = result.Description,
            ObjTypeUid = result.ObjTypeUid,
            ApplicationUid = result.ApplicationUid,
            Version = result.Version
        };

        return StatusCode(
        StatusCodes.Status201Created,
        ApiResponse<MetaObjectRequest>.Success(
            resultDTO,
            "MetaObject created successfully."));
    }

    [HttpPut("objects")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRequest>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> UpdateObject(
       [FromBody] UpdateMetaObjectRequest request,
       CancellationToken cancellationToken)
    {
        var result = await _authoringService.UpdateObjectAsync(
            request,
            cancellationToken);

        var resultDTO = new MetaObjectRequest
        {
            Id = result.Id,
            Uuid = result.Uuid,
            Name = result.Name,
            DisplayName = result.DisplayName,
            Description = result.Description,
            ObjTypeUid = result.ObjTypeUid,
            ApplicationUid = result.ApplicationUid,
            Version = result.Version
        };

        return Ok(ApiResponse<MetaObjectRequest>.Success(
            resultDTO,
            "MetaObject updated successfully."));
    }

    [HttpPatch("objects/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationshipRequest>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

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

        var resultDTO = new MetaObjectRelationshipRequest
        {
            Id = result.Id,
            Uuid = result.Uuid,
            Name = result.Name,
            DisplayName = result.DisplayName,
            Description = result.Description,
            End1Uid = result.End1Uid,
            End2Uid = result.End2Uid,
            RelTypeUid = result.RelTypeUid,
            Ordinal = result.Ordinal
        };

        return Ok(ApiResponse<MetaObjectRelationshipRequest>.Success(
            resultDTO,
            "MetaRelationship retrieved successfully."));
    }

    [HttpGet("relationships")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationshipRequest>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

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

        var resultDTO = new MetaObjectRelationshipRequest
        {
            Id = result.Id,
            Uuid = result.Uuid,
            Name = result.Name,
            DisplayName = result.DisplayName,
            Description = result.Description,
            End1Uid = result.End1Uid,
            End2Uid = result.End2Uid,
            RelTypeUid = result.RelTypeUid,
            Ordinal = result.Ordinal
        };

        return Ok(ApiResponse<MetaObjectRelationshipRequest>.Success(
            resultDTO,
            "MetaRelationship retrieved successfully."));
    }

    [HttpPost("relationships")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationshipRequest>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRequest>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRelationship(
        [FromBody] MetaObjectRelationship request,
        CancellationToken cancellationToken)
    {
        var result = await _authoringService.CreateRelationshipAsync(
            request,
            cancellationToken);

        var resultDTO = new MetaObjectRelationshipRequest
        {
            Id = result.Id,
            Uuid = result.Uuid,
            Name = result.Name,
            DisplayName = result.DisplayName,
            Description = result.Description,
            End1Uid = result.End1Uid,
            End2Uid = result.End2Uid,
            RelTypeUid = result.RelTypeUid,
            Ordinal = result.Ordinal
        };

        return StatusCode(
        StatusCodes.Status201Created,
        ApiResponse<MetaObjectRelationshipRequest>.Success(
            resultDTO,
            "MetaRelationship created successfully."));
    }

    [HttpPut("relationships")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationshipRequest>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRelationship(
       [FromBody] UpdateMetaObjectRelationshipRequest request,
       CancellationToken cancellationToken)
    {
        var result = await _authoringService.UpdateRelationshipAsync(
            request,
            cancellationToken);

        var resultDTO = new MetaObjectRelationshipRequest
        {
            Id = result.Id,
            Uuid = result.Uuid,
            Name = result.Name,
            DisplayName = result.DisplayName,
            Description = result.Description,
            End1Uid = result.End1Uid,
            End2Uid = result.End2Uid,
            RelTypeUid = result.RelTypeUid,
            Ordinal = result.Ordinal
        };

        return Ok(ApiResponse<MetaObjectRelationshipRequest>.Success(
            resultDTO,
            "MetaRelationship updated successfully."));
    }

    [HttpPatch("relationships/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
