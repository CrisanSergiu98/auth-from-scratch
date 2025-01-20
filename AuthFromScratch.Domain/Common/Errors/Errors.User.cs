using ErrorOr;

namespace AuthFromScratch.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DupicateEmail",
            description: "Email is already in use."
        );
    }
}