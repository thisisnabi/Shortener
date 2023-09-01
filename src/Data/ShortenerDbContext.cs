namespace Devblogs.Shortener.Data;

public class ShortenerDbContext : DbContext
{
    public const string DefaultSchema = "shortener";
    public const string ConnectionStringName = "SvcDbContext";

    public ShortenerDbContext(DbContextOptions<ShortenerDbContext> dbContextOptions)
        : base(dbContextOptions)
    {

    }

    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tag>(link =>
        {
            link.ToTable(Tag.TableName, DefaultSchema);
            link.HasKey(x => x.Id);

            link.Property(x => x.ShortCode)
                .HasMaxLength(20)
                .IsRequired();

            link.Property(x => x.LongUrl)
                .HasMaxLength(2083)
                .IsRequired();

            link.HasIndex(x => x.ShortCode)
                    .IsUnique(true);

            link.HasIndex(x => x.LongUrl)
                    .IsUnique(true);
        });
    }
}
