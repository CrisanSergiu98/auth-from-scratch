using AuthFromScratch.Domain.Entities;

namespace AuthFromScratch.Application.Authentication;

public record AuthenticationResult(
    User User,
    string Token);