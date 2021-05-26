using Microsoft.EntityFrameworkCore;
using PostService.Models;

namespace PostService.Database
{
    public class PostDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
