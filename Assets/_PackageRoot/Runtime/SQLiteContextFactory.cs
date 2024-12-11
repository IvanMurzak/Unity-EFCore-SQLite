using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Microsoft.EntityFrameworkCore
{
    public abstract class SQLiteContextFactory<T> : IDbContextFactory<T>, IDesignTimeDbContextFactory<T> where T : DbContext
    {
        protected abstract string FolderPath_runtime { get; }
        protected abstract string FolderPath_designTime { get; }
        protected virtual string DatabasePath_designTime => $"Data Source={FolderPath_designTime}/data.db";
        protected virtual string DatabasePath_runtime => $"Data Source={FolderPath_runtime}/data.db";

        public SQLiteContextFactory()
        {
            SQLitePCLRaw.Startup.Setup();
        }
        public SQLiteContextFactory(string folderPath) : this()
        {
            EnsureDatabaseFolderExists(folderPath);
        }
        private void EnsureDatabaseFolderExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }

        protected abstract T InternalCreateDbContext(DbContextOptions<T> optionsBuilder);

        // Runtime usage
        public T CreateDbContext()
        {
            EnsureDatabaseFolderExists(FolderPath_runtime);

            var optionsBuilder = new DbContextOptionsBuilder<T>()
                .UseSqlite(DatabasePath_runtime);

            var dbContext = InternalCreateDbContext(optionsBuilder.Options);

            dbContext.Database.Migrate();

            return dbContext;
        }

        // Only design time usage
        T IDesignTimeDbContextFactory<T>.CreateDbContext(string[] args)
        {
            EnsureDatabaseFolderExists(FolderPath_designTime);

            var optionsBuilder = new DbContextOptionsBuilder<T>()
                .UseSqlite(DatabasePath_designTime);

            return InternalCreateDbContext(optionsBuilder.Options);
        }
    }
}