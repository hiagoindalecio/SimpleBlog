using SimpleBlog.Domain.Interfaces;

namespace SimpleBlog.Domain.Services
{
    public class UserPasswordValidator : IUserPasswordValidator
    {
        public bool IsValid(string password, out List<string> errors)
        {
            errors = [];

            if (string.IsNullOrWhiteSpace(password))
                errors.Add("Password cannot be empty.");
            if (password.Length < 8)
                errors.Add("Password must be at least 8 characters long.");
            if (!password.Any(char.IsUpper))
                errors.Add("Password must contain at least one uppercase letter.");
            if (!password.Any(char.IsLower))
                errors.Add("Password must contain at least one lowercase letter.");
            if (!password.Any(char.IsDigit))
                errors.Add("Password must contain at least one digit.");
            if (!password.Any("!@#$%^&*()_-+=<>?/{}[]|\\~`".Contains))
                errors.Add("Password must contain at least one special character.");

            return errors.Count == 0;
        }
    }
}
