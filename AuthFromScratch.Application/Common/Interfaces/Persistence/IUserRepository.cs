using AuthFromScratch.Domain.Entities;

namespace AuthFromScratch.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetUserByEmail(string email);
    void Add(User user);
}