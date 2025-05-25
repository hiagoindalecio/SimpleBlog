using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<string?> GetNameByIdAsync(int userId);
        Task<User?> GetByEmailAsync(string email);
    }
}
