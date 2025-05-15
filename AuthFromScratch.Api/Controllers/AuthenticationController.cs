using Microsoft.AspNetCore.Mvc;
using AuthFromScratch.Contracts.Authentication;
using AuthFromScratch.Domain.Common.Errors;
using MediatR;
using AuthFromScratch.Application.Authentication.Commands.Register;
using AuthFromScratch.Application.Authentication.Queries.Login;
using MapsterMapper;

namespace AuthFromScratch.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{    
    private readonly ISender _sender;
    private readonly IMapper _mapper;
    public AuthenticationController(IMediator sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);

        var authResult = await _sender.Send(command);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));                
    }    

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);

        var authResult = await _sender.Send(query);

        if(authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized, 
                title: authResult.FirstError.Description);
        }

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }    
}