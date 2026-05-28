using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFCoreSQLiteBundle
{
    public abstract class SQLiteContextFactory<T> : IDbContextFactory<T>, IDesignTimeDbContextFactory<T> where T : DbContext
    {
        /// <summary>
        /// The "design time" is only needed in the case if you would like to use the EF Core tools
        /// for creating, migrating or updating the database schema.
        /// If you don't need to use the EF Core tools, you can return null here.
        ///
        /// Provide the connection string for design time usage.
        /// It should be something like "Data Source=database.db"
        /// </summary>
        protected virtual string DesignTimeDataSource => DataSource;
        protected virtual string DataSource { get; set; }

        private readonly string _databasePath;

        /// <summary>
        /// Default constructor needed for Design Time.
        /// Don't use it in runtime.
        /// </summary>
        public SQLiteContextFactory()
        {
            SQLitePCLRaw.Startup.Setup();
        }
        public SQLiteContextFactory(string path, string filename)
        {
            _databasePath = path;
            DataSource = BuildSource(path, filename);
            EnsureDatabaseFolderExists();

            SQLitePCLRaw.Startup.Setup();
        }
        private void EnsureDatabaseFolderExists()
        {
            if (!Directory.Exists(_databasePath))
                Directory.CreateDirectory(_databasePath);
        }

        /// <summary>
        /// This method is used to build the full path to the database file.
        /// You can override it to change the way the path is built.
        /// </summary>
        protected virtual string BuildSource(string path, string filename)
        {
            return $"Data Source=" + Path.GetFullPath(Path.Combine(path, filename));
        }

        protected abstract T InternalCreateDbContext(DbContextOptions<T> optionsBuilder);

        /// <summary>
        /// This method is called when the DbContext is created.
        /// You can override it to configure the DbContext options.
        /// </summary>
        protected virtual void OnConfiguring(DbContextOptionsBuilder optionsBuilder, string dataSource)
        {
            // This is the default connection string for SQLite
            optionsBuilder.UseSqlite(dataSource);
        }

        /// <summary>
        /// This method is called when the DbContext is created.
        /// You can override it to configure the DbContext options.
        /// </summary>
        protected virtual void OnConfiguring(T dbContext)
        {
            // Create the database if it doesn't exist
            // and apply any pending migrations to the database
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
        }

        /// <summary>
        /// This method is called when the DbContext is created.
        /// You can override it to configure the DbContext options.
        /// </summary>
        public T CreateDbContext()
        {
            EnsureDatabaseFolderExists();

            var optionsBuilder = new DbContextOptionsBuilder<T>();
            OnConfiguring(optionsBuilder, DataSource);

            var dbContext = InternalCreateDbContext(optionsBuilder.Options);
            OnConfiguring(dbContext);

            return dbContext;
        }

        /// <summary>
        /// !!! DO NOT USE THIS METHOD IN RUNTIME !!!
        /// This method is called when the DbContext is created during DESIGN TIME (code first approach).
        /// You can override it to configure the DbContext options.
        /// </summary>
        T IDesignTimeDbContextFactory<T>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            OnConfiguring(optionsBuilder, DesignTimeDataSource);

            return InternalCreateDbContext(optionsBuilder.Options);
        }
    }
}