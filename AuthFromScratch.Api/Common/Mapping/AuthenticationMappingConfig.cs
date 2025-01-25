using AuthFromScratch.Application.Authentication.Commands.Register;
using AuthFromScratch.Application.Authentication.Common;
using AuthFromScratch.Application.Authentication.Queries.Login;
using AuthFromScratch.Contracts.Authentication;
using Mapster;

namespace AuthFromScratch.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
        .Map(dest => dest, src => src.User);
    }
}