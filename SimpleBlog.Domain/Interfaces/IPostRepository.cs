using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Domain.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        IEnumerable<(int Id, string Title, string Content, string AuthorName, DateTime UpdatedAt)> GetAllPaged(int take, int skip);
    }
}
