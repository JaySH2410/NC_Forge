using Forge.Features.MetaSchema.Contracts;
using Forge.Features.MetaSchema.DTOs;
using Forge.Shared.Exceptions;
using Forge.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forge.Features.MetaSchema.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MetaSchemaController : ControllerBase
{
    private readonly IMetaSchemaAuthoringService _metaSchemaAuthoringService;
    private readonly IApplicationAuthoringService _appAuthoringService;
    private readonly IMetaSchemaService _metaSchemaService;
    private readonly IApplicationService _appService;


    public MetaSchemaController(
        IMetaSchemaAuthoringService metaSchemaAuthoringService,
        IApplicationAuthoringService appAuthoringService,
        IMetaSchemaService metaSchemaService, 
        IApplicationService appService
        )
    {
        _metaSchemaAuthoringService = metaSchemaAuthoringService;
        _appAuthoringService = appAuthoringService;
        _metaSchemaService = metaSchemaService;
        _appService = appService;
    }

    [HttpGet("applications/{uuid}")]
    [ProducesResponseType(typeof(ApiResponse<ApplicationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplication(
        Guid uuid,
        CancellationToken cancellationToken)
    {
        var result = await _appService.GetApplicationAsync(
            uuid,
            cancellationToken);
        if (result is null)
        {
            throw new NotFoundException("Application not found.");
        }
        var resultDTO = new ApplicationResponse
        {
            Id = result.Id,
            Uuid = result.Uuid,
            Name = result.Name,
            DisplayName = result.DisplayName,
            Description = result.Description,
            Version = result.Version
        };
        return Ok(ApiResponse<ApplicationResponse>.Success(
            resultDTO,
            "Application retrieved successfully."));
    }

    [HttpGet("applications")]
    [ProducesResponseType(typeof(ApiResponse<ApplicationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationByName(
       string name,
       CancellationToken cancellationToken)
    {
        var result = await _appService.GetApplicationByNameAsync(
            name,
            cancellationToken);
        if (result is null)
        {
            throw new NotFoundException("Application not found.");
        }
        var resultDTO = new ApplicationResponse
        {
            Id = result.Id,
            Uuid = result.Uuid,
            Name = result.Name,
            DisplayName = result.DisplayName,
            Description = result.Description,
            Version = result.Version
        };
        return Ok(ApiResponse<ApplicationResponse>.Success(
            resultDTO,
            "Application retrieved successfully."));
    }

    [HttpPost("applications")]
    [ProducesResponseType(typeof(ApiResponse<ApplicationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateApplication(
        [FromBody] CreateApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _appAuthoringService.CreateApplicationAsync(
            request,
            cancellationToken);
        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<ApplicationResponse>.Success(
                result,
                "Application created successfully."));
    }
    [HttpPut("applications")]
    [ProducesResponseType(typeof(ApiResponse<ApplicationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> UpdateApplication(
       [FromBody] UpdateApplicationRequest request,
       CancellationToken cancellationToken)
    {
        var result = await _appAuthoringService.UpdateApplicationAsync(
            request,
            cancellationToken);

        return Ok(ApiResponse<ApplicationResponse>.Success(
            result,
            "Application updated successfully."));
    }

    [HttpGet("objects/{uuid}")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectResponse>), StatusCodes.Status200OK)]
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
            throw new NotFoundException("MetaObject not found.");
        }

        var resultDTO = new MetaObjectResponse
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

        return Ok(ApiResponse<MetaObjectResponse>.Success(
            resultDTO,
            "MetaObject retrieved successfully."));
    }

    [HttpGet("objects")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectResponse>), StatusCodes.Status200OK)]
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
            throw new NotFoundException("MetaObject not found.");
        }

        var resultDTO = new MetaObjectResponse
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

        return Ok(ApiResponse<MetaObjectResponse>.Success(
            resultDTO,
            "MetaObject retrieved successfully."));
    }

    [HttpPost("objects")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> CreateObject(
        [FromBody] CreateMetaObjectRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _metaSchemaAuthoringService.CreateObjectAsync(
            request,
            cancellationToken);

       return StatusCode(
        StatusCodes.Status201Created,
        ApiResponse<MetaObjectResponse>.Success(
            result,
            "MetaObject created successfully."));
    }

    [HttpPut("objects")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> UpdateObject(
       [FromBody] UpdateMetaObjectRequest request,
       CancellationToken cancellationToken)
    {
        var result = await _metaSchemaAuthoringService.UpdateObjectAsync(
            request,
            cancellationToken);

        return Ok(ApiResponse<MetaObjectResponse>.Success(
            result,
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
        await _metaSchemaAuthoringService.ActivateObjectAsync(
            request,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status200OK,
            ApiResponse.Success("MetaObject activated successfully.")
        );
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
        await _metaSchemaAuthoringService.DeactivateObjectAsync(
            request,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status200OK,
            ApiResponse.Success("MetaObject deactivated successfully.")
        );
    }

    [HttpGet("relationships/{uuid}")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationshipResponse>), StatusCodes.Status200OK)]
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
            throw new NotFoundException("MetaRelationship not found.");
        }

        var resultDTO = new MetaObjectRelationshipResponse
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

        return Ok(ApiResponse<MetaObjectRelationshipResponse>.Success(
            resultDTO,
            "MetaRelationship retrieved successfully."));
    }

    [HttpGet("relationships")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationshipResponse>), StatusCodes.Status200OK)]
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
            throw new NotFoundException("MetaRelationship not found.");
        }

        var resultDTO = new MetaObjectRelationshipResponse
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

        return Ok(ApiResponse<MetaObjectRelationshipResponse>.Success(
            resultDTO,
            "MetaRelationship retrieved successfully."));
    }

    [HttpPost("relationships")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationshipResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRelationship(
        [FromBody] CreateMetaObjectRelationshipRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _metaSchemaAuthoringService.CreateRelationshipAsync(
            request,
            cancellationToken);

        

        return StatusCode(
        StatusCodes.Status201Created,
        ApiResponse<MetaObjectRelationshipResponse>.Success(
            result,
            "MetaRelationship created successfully."));
    }

    [HttpPut("relationships")]
    [ProducesResponseType(typeof(ApiResponse<MetaObjectRelationshipResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRelationship(
       [FromBody] UpdateMetaObjectRelationshipRequest request,
       CancellationToken cancellationToken)
    {
        var result = await _metaSchemaAuthoringService.UpdateRelationshipAsync(
            request,
            cancellationToken);

        return Ok(ApiResponse<MetaObjectRelationshipResponse>.Success(
            result,
            "MetaRelationship updated successfully."));
    }

    [HttpPatch("relationships/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActivateRelationship(
        [FromBody] UuidRequest request,
        CancellationToken cancellationToken)
    {
        await _metaSchemaAuthoringService.ActivateRelationshipAsync(
            request,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status200OK,
            ApiResponse.Success("MetaRelationship activated successfully.")
        );
    }

    [HttpPatch("relationships/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeactivateRelationship(
        [FromBody] UuidRequest request,
        CancellationToken cancellationToken)
    {
        await _metaSchemaAuthoringService.DeactivateRelationshipAsync(
            request,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status200OK,
            ApiResponse.Success("MetaRelationship deactivated successfully.")
        );
    }

    [HttpPost("interface-implementations")]
    [ProducesResponseType(typeof(ApiResponse<InterfaceImplementationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateInterfaceImplementation(
        [FromBody] CreateInterfaceImplementationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _metaSchemaAuthoringService
            .CreateInterfaceImplementationAsync(request, cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<InterfaceImplementationResponse>.Success(
                result,
                "Interface implementation created successfully."));
    }
}
