using SimpleBlog.Application.DTOs;
using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(CreateUserDto dto);
        Task DeleteUserAsync(int userId);
        Task UpdateUserAsync(UpdateUserDto dto);
        Task<string> GetUserNameByIdAsync(int userId);
        Task<User> AuthenticateAsync(string email, string password);
        Task UpdatePasswordAsync(UpdatePasswordDto dto);
    }
}
