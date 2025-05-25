using SimpleBlog.Application.DTOs;

namespace SimpleBlog.Application.Interfaces
{
    public interface IPostService
    {
        Task CreatePostAsync(CreatePostDto dto);
        Task DeletePostAsync(int postId);
        Task UpdatePostAsync(UpdatePostDto dto);
        List<PagedPostsDto> GetAllPaged(int take, int skip);
    }
}
