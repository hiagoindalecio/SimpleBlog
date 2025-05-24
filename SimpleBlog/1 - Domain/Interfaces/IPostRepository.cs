using SimpleBlog.Application.DTOs;
using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Domain.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<List<PagedPostsDto>> GetAllPagedAsync(int take, int skip);
    }
}
