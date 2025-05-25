using SimpleBlog.Application.DTOs;

namespace SimpleBlog.Application.Interfaces
{
    public interface IPostService
    {
        Task CreatePostAsync(CreatePostDto dto);
        Task DeletePostAsync(int postId);
        Task UpdatePostAsync(UpdatePostDto dto);
        Task<List<PagedPostsDto>> GetAllPagedAsync(int take, int skip);
    }
}
