using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AuthFromScratch.Api.Controllers;

[ApiController]
public class ErrorsController: ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        return Problem(title: exception?.Message, statusCode: 400);
    }
}