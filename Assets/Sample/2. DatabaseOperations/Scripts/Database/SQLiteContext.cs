using Microsoft.EntityFrameworkCore;

public class SQLiteContext : DbContext
{
    public DbSet<LevelData> Levels { get; set; }

    public SQLiteContext() : base() { }
    public SQLiteContext(DbContextOptions<SQLiteContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<LevelData>();

        // To create connection between tables read more about Code First EFCore approach
        // Highly recommended to use command line to generate the code automatically.
    }
}