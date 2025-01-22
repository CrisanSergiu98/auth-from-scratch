using AuthFromScratch.Application.Authentication.Common;
using AuthFromScratch.Application.Common.Interfaces.Authentication;
using AuthFromScratch.Application.Common.Interfaces.Persistence;
using AuthFromScratch.Domain.Entities;
using AuthFromScratch.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace AuthFromScratch.Application.Authentication.Queries.Login;

internal sealed class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginQueryHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    } 

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        // 1. Validate that the user exists.
        if(_userRepository.GetUserByEmail(request.Email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }
            

        // 2. Validate that the password is correct.
        if(user.Password != request.Password)
        {
            return Errors.Authentication.InvalidCredentials;
        }            

        // 3. Create Jwt token.
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.FirstName, user.LastName);

        return new AuthenticationResult(
            user,
            token);
    }
}
