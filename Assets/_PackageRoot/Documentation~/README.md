# Unity + EFCore + SQLite = ❤️

![npm](https://img.shields.io/npm/v/extensions.unity.bundle.efcore.sqlite) [![openupm](https://img.shields.io/npm/v/extensions.unity.bundle.efcore.sqlite?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/extensions.unity.bundle.efcore.sqlite/) ![License](https://img.shields.io/github/license/IvanMurzak/Unity-EFCore-SQLite) [![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/badges/StandWithUkraine.svg)](https://stand-with-ukraine.pp.ua)

Ready to go bundle package that includes references on [EntityFrameworkCore](https://github.com/dotnet/efcore) and [SQLitePCLRaw](https://github.com/ericsink/SQLitePCL.raw) packages that just works in this combination for the next platforms:

Supports AOT an JIT compilation. For AOT it uses nested `link.xml` file to exclude required classes from stripping.

## Supported project settings

### Platform

- ✔️ Windows
- ✔️ Android
- ✔️ iOS
- ✔️ MacOS
- Others not yet tested

### Scripting backend

- ❌ `Mono`
- ✔️ `IL2CPP`

### API Compatibility

- ❌ `.NET Framework`
- ✔️ `.NET Standard 2.0`
- ✔️ `.NET Standard 2.1`

# Usage

Call the function once at app startup. Important to do that before opening SQLite connection.

```C#
SQLitePCLRaw.Startup.Setup();
```

Then use EFCore as usual.

# Installation

- [Install OpenUPM-CLI](https://github.com/openupm/openupm-cli#installation)
- Open command line in Unity project folder
- Run the command

``` CLI
openupm add org.nuget.castle.core@5.1.1 --registry https://unitynuget-registry.openupm.com/
openupm add org.nuget.sqlitepclraw.bundle_e_sqlite3@2.1.10 --registry https://unitynuget-registry.openupm.com/
openupm add org.nuget.sqlitepclraw.provider.e_sqlite3@2.1.10 --registry https://unitynuget-registry.openupm.com/
openupm add org.nuget.sqlitepclraw.provider.sqlite3@2.1.10 --registry https://unitynuget-registry.openupm.com/
openupm add org.nuget.sqlitepclraw.lib.e_sqlite3@2.1.10 --registry https://unitynuget-registry.openupm.com/

```

- Modify `/Packages/manifest.json` file by adding required `dependencies` and `scopedRegistries`

```json
{
  "dependencies": {
    "extensions.unity.bundle.efcore.sqlite": "0.0.7"
  },
  "scopedRegistries": [
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.openupm"
      ]
    }
  ]
}
```

# Alternative usage

## 1. Create `SQLiteContext` class

```C#
using Microsoft.EntityFrameworkCore;

public class SQLiteContext : DbContext
{
    // sample table of levels in your database
    public DbSet<LevelData> Levels { get; set; }

    public SQLiteContext() : base() { }
    public SQLiteContext(DbContextOptions<SQLiteContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<LevelData>();
    }
}
```

## 2. Create `SQLiteContextFactory` class

```C#
using Microsoft.EntityFrameworkCore;

public class SQLiteContextFactory : EFCoreSQLiteBundle.SQLiteContextFactory<SQLiteContext>
{
    protected override string DesignTimeDataSource => "replace it with path to design time database";

    public SQLiteContextFactory() : base(UnityEngine.Application.persistentDataPath, "data.db") { }

    protected override SQLiteContext InternalCreateDbContext(DbContextOptions<SQLiteContext> optionsBuilder)
        => new SQLiteContext(optionsBuilder);
}
```

The `EFCoreSQLiteBundle.SQLiteContextFactory` class under the hood executes `SQLitePCLRaw.Startup.Setup();` for proper SQLite setup depends on the current platform.

## 3. Create database context

```C#
var contextFactory = new SQLiteContextFactory();
var dbContext = contextFactory.CreateDbContext();

// use it for data manipulations
// sample:
var level_1 = dbContext.Levels.FirstOrDefault(level => level.id == 1);
```

# Helpful information

Read more how to use [EntityFrameworkCore](https://learn.microsoft.com/en-us/ef/ef6/get-started?redirectedfrom=MSDN). My favorite approach is `Code First`.
Please keep in mind. Because of Unity's .NET Standard 2.1 restrictions we are only limited to use the old version of EntityFrameworkCore 5.0.17. Newer versions require newer .NET version which Unity doesn't support yet. Anyway the version 5.0.17 is a good one for sure!
