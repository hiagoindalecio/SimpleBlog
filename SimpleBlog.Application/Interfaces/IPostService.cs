using SimpleBlog.Application.DTOs;

namespace SimpleBlog.Application.Interfaces
{
    public interface IPostService
    {
        Task CreatePostAsync(CreatePostDto dto);
        Task DeletePostAsync(int postId, int userId);
        Task UpdatePostAsync(UpdatePostDto dto, int userId);
        List<PagedPostsDto> GetAllPaged(int take, int skip);
    }
}
