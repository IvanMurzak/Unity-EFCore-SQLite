using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCoreSQLiteBundle
{
    public abstract class SQLiteContextFactory<T> : IDbContextFactory<T>, IDesignTimeDbContextFactory<T> where T : DbContext
    {
        /// <summary>
        /// The "design time" is only needed in the case if you would like to use the EF Core tools
        /// For example, to create a migration or update the database schema.
        /// If you don't need to use the EF Core tools, you can return null here.
        ///
        /// Provide the connection string for design time usage.
        /// It should be something like "Data Source=database.db"
        /// </summary>
        protected abstract string DesignTimeDataSource { get; }
        protected virtual string DataSource => $"Data Source={_dataSource}";

        private readonly string _dataSource;
        private readonly string _directoryPath;

        /// <summary>
        /// Default constructor needed for Design Time.
        /// Don't use it in runtime.
        /// </summary>
        public SQLiteContextFactory()
        {
            SQLitePCLRaw.Startup.Setup();
        }
        public SQLiteContextFactory(string directoryPath, string databaseFilename)
        {
            _directoryPath = directoryPath;
            _dataSource = Path.Combine(directoryPath, databaseFilename);
            EnsureDatabaseFolderExists();

            SQLitePCLRaw.Startup.Setup();
        }
        private void EnsureDatabaseFolderExists()
        {
            if (!Directory.Exists(_directoryPath))
                Directory.CreateDirectory(_directoryPath);
        }

        protected abstract T InternalCreateDbContext(DbContextOptions<T> optionsBuilder);

        // Runtime usage
        public T CreateDbContext()
        {
            EnsureDatabaseFolderExists();

            var optionsBuilder = new DbContextOptionsBuilder<T>()
                .UseSqlite(DataSource);

            var dbContext = InternalCreateDbContext(optionsBuilder.Options);

            dbContext.Database.Migrate();

            return dbContext;
        }

        // Only design time usage
        T IDesignTimeDbContextFactory<T>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>()
                .UseSqlite(DesignTimeDataSource);

            return InternalCreateDbContext(optionsBuilder.Options);
        }
    }
}