// using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Features.Hobby.DTOs;
using test.Shared.Models;

namespace test.Features.Hobby.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(
        CreateHobbyRequest request)
    {
        return Ok(
            ApiResponse.Success(
                "Validation passed"));
    }
}
