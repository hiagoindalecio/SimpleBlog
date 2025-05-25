namespace SimpleBlog.Domain.Interfaces
{
    public interface IEntityValidator<T>
    {
        bool IsValid(T entity, out List<string> errors);
    }
}
