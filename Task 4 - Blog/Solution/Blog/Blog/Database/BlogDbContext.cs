using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Database
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public BlogDbContext(): base(new DbContextOptions<BlogDbContext>())
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permisions> Permisions { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleRate> ArticleRates { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
