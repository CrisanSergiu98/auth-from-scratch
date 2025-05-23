using AuthFromScratch.Application.Common.Interfaces.Persistence;
using AuthFromScratch.Domain.Entities;

namespace AuthFromScratch.Infrastructure.Persistence;

public class InMemoryUserRepository : IUserRepository
{
    private static readonly List<User> _users = new();
    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetUserByEmail(string email)
    {
        return _users.SingleOrDefault(x => x.Email == email);
    }
}
