using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;

namespace SimpleBlog.Domain.Services
{
    public class UserValidator : IEntityValidator<User>
    {
        public bool IsValid(User user, out List<string> errors)
        {
            errors = [];

            if (string.IsNullOrWhiteSpace(user.Name))
                errors.Add("The user name can't be empty!");
            if (string.IsNullOrWhiteSpace(user.Email))
                errors.Add("The user email can't be empty!");
            else if (!user.Email.Contains('@') || !user.Email.Contains('.'))
                errors.Add("The user email is invalid!");
            
            return errors.Count == 0;
        }
    }
}
