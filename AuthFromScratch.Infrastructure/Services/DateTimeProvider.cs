using AuthFromScratch.Application.Common.Interfaces.Services;

namespace AuthFromScratch.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
