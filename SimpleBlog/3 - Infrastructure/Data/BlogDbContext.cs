using Microsoft.EntityFrameworkCore;
using SimpleBlog.Domain.Entities;

namespace SimpleBlog.Infrastructure.Data
{
    public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // TOD: Configure the entities here
        }
    }
}
