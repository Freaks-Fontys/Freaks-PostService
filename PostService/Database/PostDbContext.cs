using Microsoft.EntityFrameworkCore;
using PostService.Models;
using System;
namespace PostService.Database
{
    public class PostDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=localhost,7000;Database=PlayerService;User Id=sa;Password=HelloStudents!");
        }
    }
}
