namespace SimpleBlog.Domain.Interfaces
{
    public interface IUserPasswordValidator
    {
        bool IsValid(string password, out List<string> errors);
    }
}
