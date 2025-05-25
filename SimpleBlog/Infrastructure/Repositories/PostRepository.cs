using Microsoft.EntityFrameworkCore;
using SimpleBlog.Application.DTOs;
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

        public Task<List<PagedPostsDto>> GetAllPagedAsync(int take, int skip)
            => _context.Posts
                .OrderByDescending(p => p.UpdatedAt)
                .Skip(skip)
                .Take(take)
                .Select(p => new PagedPostsDto(
                    p.Id,
                    p.Title,
                    p.Content,
                    p.Author != null ? p.Author.Name : "",
                    p.UpdatedAt
                ))
                .ToListAsync();
    }
}
