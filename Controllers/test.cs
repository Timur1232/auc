using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace App.Controllers;

[ApiController]
[Route("test")]
[Authorize]
public class TestController : Controller
{
    [HttpGet]
    public IActionResult Test()
    {
        return Content("Hi");
    }
}
