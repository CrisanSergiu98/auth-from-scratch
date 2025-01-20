using AuthFromScratch.Application.Common.Interfaces.Authentication;
using AuthFromScratch.Application.Common.Interfaces.Persistence;
using AuthFromScratch.Domain.Common.Errors;
using AuthFromScratch.Domain.Entities;
using ErrorOr;

namespace AuthFromScratch.Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }    

    public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
    {
        // 1. Validate that the email does nto already exist
        if(_userRepository.GetUserByEmail(email) is not null)
        {
            return Errors.User.DuplicateEmail;
        }
            

        // 2. Create the user
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        // 3. Persist the user
        _userRepository.Add(user);

        // 4. Create Jwt token.
        var token = _jwtTokenGenerator.GenerateToken(user.Id, firstName, lastName);

        return new AuthenticationResult(
            user, 
            token);
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {       
        // 1. Validate that the user exists.
        if(_userRepository.GetUserByEmail(email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }
            

        // 2. Validate that the password is correct.
        if(user.Password != password)
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
