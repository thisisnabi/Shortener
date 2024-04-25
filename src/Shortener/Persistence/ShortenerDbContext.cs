using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using Shortener.Models;
namespace Shortener.Persistence;

public class ShortenerDbContext : DbContext
{
    public DbSet<Link> Links { get; set; }

    public ShortenerDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Link>().ToCollection("links");
    }
}


