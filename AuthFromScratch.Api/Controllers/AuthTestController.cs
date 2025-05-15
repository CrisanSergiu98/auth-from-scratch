using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthFromScratch.Api.Controllers;

[Route("authTest")]
[Authorize]
public class AuthTestController: ApiController
{
    [HttpGet]    
    public IActionResult TestIfAuthenticated()
    {
        return Ok("Authentication success");
    }
    
}