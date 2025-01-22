using Microsoft.AspNetCore.Mvc;
using AuthFromScratch.Contracts.Authentication;
using ErrorOr;
using AuthFromScratch.Domain.Common.Errors;
using AuthFromScratch.Application.Authentication.Common;
using MediatR;
using AuthFromScratch.Application.Authentication.Commands.Register;
using System.Threading.Tasks;
using AuthFromScratch.Application.Authentication.Queries.Login;

namespace AuthFromScratch.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{    
    private readonly ISender _sender;
    public AuthenticationController(IMediator sender)
    {
        _sender = sender;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.FirstName, 
            request.LastName, 
            request.Email, 
            request.Password);

        var authResult = await _sender.Send(command);

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors));                
    }    

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(
            request.Email,
            request.Password);

        var authResult = await _sender.Send(query);

        if(authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized, 
                title: authResult.FirstError.Description);
        }

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(errors));
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token
        );
    }
}