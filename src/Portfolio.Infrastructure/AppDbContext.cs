using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities;

namespace Portfolio.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<Work> Works { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<TagWork> TagWorks { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
