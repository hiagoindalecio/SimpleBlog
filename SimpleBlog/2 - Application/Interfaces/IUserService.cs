using SimpleBlog.Application.DTOs;

namespace SimpleBlog.Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(CreateUserDto dto);
        Task DeleteUserAsync(int userId);
        Task UpdateUserAsync(UpdateUserDto dto);
        Task<bool> AuthenticateAsync(string email, string password);
        Task UpdatePasswordAsync(UpdatePasswordDto dto);
    }
}
