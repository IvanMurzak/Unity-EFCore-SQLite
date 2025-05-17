using Microsoft.EntityFrameworkCore;

public class SQLiteContextFactory : EFCoreSQLiteBundle.SQLiteContextFactory<SQLiteContext>
{
    public SQLiteContextFactory() : base(UnityEngine.Application.persistentDataPath, "data.db")
    {
        // Optional logging
        UnityEngine.Debug.Log($"Using database: {DataSource}");
    }

    protected override SQLiteContext InternalCreateDbContext(DbContextOptions<SQLiteContext> optionsBuilder)
    {
        return new SQLiteContext(optionsBuilder);
    }
}