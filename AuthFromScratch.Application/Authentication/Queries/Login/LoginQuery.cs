using AuthFromScratch.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace AuthFromScratch.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
): IRequest<ErrorOr<AuthenticationResult>>;