using Microsoft.EntityFrameworkCore;
using SimpleBlog.Domain.Entities;
using SimpleBlog.Domain.Interfaces;
using SimpleBlog.Infrastructure.Data;

namespace SimpleBlog.Infrastructure.Repositories
{
    public class UserRepository(BlogDbContext context) : IUserRepository
    {
        private readonly BlogDbContext _context = context;

        public Task AddAsync(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return _context.SaveChangesAsync();
        }

        public Task<User> GetByIdAsync(int userId)
            => _context.Users.AsNoTracking().FirstAsync(u => u.Id == userId);

        public Task<User> GetByEmailAsync(string email)
            => _context.Users.AsNoTracking().FirstAsync(u => u.Email == email);
    }
}
