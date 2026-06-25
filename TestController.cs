using Microsoft.AspNetCore.Mvc;
using test.Shared.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace test;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        throw new NotFoundException(
            "Test not found exception");
    }
}
