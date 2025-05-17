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

        // To define relationships between tables, configure navigation properties and use Fluent API.
        // Refer to the official EF Core documentation: https://learn.microsoft.com/en-us/ef/core/modeling/relationships
        // To generate code automatically, use the EF Core CLI tools. For example:
        //   dotnet ef migrations add <MigrationName>
        //   dotnet ef database update
    }
}