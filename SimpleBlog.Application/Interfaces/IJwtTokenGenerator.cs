using SimpleBlog.Application.DTOs;
using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        AuthResponseDto GenerateToken(User user);
    }
}
