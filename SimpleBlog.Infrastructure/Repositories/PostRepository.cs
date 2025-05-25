using Microsoft.EntityFrameworkCore;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Infrastructure.Data;

namespace SimpleBlog.Infrastructure.Repositories
{
    public class PostRepository(SimpleBlogDbContext context) : IPostRepository
    {
        private readonly SimpleBlogDbContext _context = context;

        public Task AddAsync(Post post)
        {
            _context.Posts.Add(post);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Post post)
        {
            _context.Posts.Remove(post);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            return _context.SaveChangesAsync();
        }

        public Task<Post> GetByIdAsync(int postId)
            => _context.Posts.AsNoTracking().FirstAsync(p => p.Id == postId);

        public IEnumerable<(int Id, string Title, string Content, string AuthorName, DateTime UpdatedAt)> GetAllPaged(int take, int skip)
            => _context.Posts
                .OrderByDescending(p => p.UpdatedAt)
                .Skip(skip)
                .Take(take)
                .Select(p => new ValueTuple<int, string, string, string, DateTime>(
                    p.Id,
                    p.Title,
                    p.Content,
                    p.Author != null ? p.Author.Name : string.Empty,
                    p.UpdatedAt
                ))
                .AsEnumerable();
    }
}
