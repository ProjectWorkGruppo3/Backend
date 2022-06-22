using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Serendipity.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TestController : Controller
{
    [Authorize]
    [Route("test")]
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Sei autorizzato");
    }
}