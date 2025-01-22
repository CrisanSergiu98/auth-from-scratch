using AuthFromScratch.Domain.Entities;

namespace AuthFromScratch.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);