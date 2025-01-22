using AuthFromScratch.Application.Authentication.Common;
using AuthFromScratch.Application.Common.Interfaces.Authentication;
using AuthFromScratch.Application.Common.Interfaces.Persistence;
using AuthFromScratch.Domain.Common.Errors;
using AuthFromScratch.Domain.Entities;
using ErrorOr;
using MediatR;

namespace AuthFromScratch.Application.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    } 

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate that the email does nto already exist
        if(_userRepository.GetUserByEmail(request.Email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }            

        // 2. Create the user
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };

        // 3. Persist the user
        _userRepository.Add(user);

        // 4. Create Jwt token.
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);

        return new AuthenticationResult(
            user, 
            token);
    }
}
