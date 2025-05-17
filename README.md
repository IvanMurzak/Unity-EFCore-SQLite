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

Then use EFCore as usual.

# Installation

- [Install OpenUPM-CLI](https://github.com/openupm/openupm-cli#installation)
- Open command line in Unity project folder
- Run the command

``` CLI
openupm add extensions.unity.bundle.efcore.sqlite
```

# Usage

## Option 1: Explicit (Recommended)

Use this approach to setup your database and establish connection.

### 1. Create data model `LevelData.cs`

```csharp
using System.ComponentModel.DataAnnotations;

public class LevelData
{
    [Key]
    public int Id { get; set; }
    [StringLength(100)]
    public string Name { get; set; }
    [Range(1, 10)]
    public int Difficulty { get; set; }
    public string Description { get; set; }
}
```

### 2. Create `SQLiteContext.cs`

```csharp
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

        // To create connection between tables read more about Code First EFCore approach
        // Highly recommended to use command line to generate the code automatically.
    }
}
```

### 3. Create `SQLiteContextFactory.cs`

```csharp
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
```

The `EFCoreSQLiteBundle.SQLiteContextFactory` class under the hood executes `SQLitePCLRaw.Startup.Setup();` for proper SQLite setup depends on the current platform.

### 4. Create database context

```csharp
using (var dbContext = new SQLiteContextFactory().CreateDbContext())
{
    // use it for data manipulations
    // sample:
    var level_1 = dbContext.Levels.FirstOrDefault(level => level.Id == 1);
}
```

There is full usage sample in this source code:

```csharp
using System.Linq;
using UnityEngine;

public class DatabaseOperations : MonoBehaviour
{
    void Awake()
    {
        AddLevel("Level 1", 1, "Easy level");
        AddLevel("Level 2", 2, "Medium level");
        AddLevel("Level 3", 3, "Hard level");

        PrintAllLevels();
    }

    void AddLevel(string name, int difficulty, string description)
    {
        using (var dbContext = new SQLiteContextFactory().CreateDbContext())
        {
            var level = new LevelData
            {
                Name = name,
                Difficulty = difficulty,
                Description = description
            };
            dbContext.Levels.Add(level);
            dbContext.SaveChanges();
        }
        Debug.Log($"Added Level: {name}, Difficulty: {difficulty}");
    }
    void PrintAllLevels()
    {
        using (var dbContext = new SQLiteContextFactory().CreateDbContext())
        {
            var levels = dbContext.Levels.ToList();
            foreach (var level in levels)
                Debug.Log($"Level ID: {level.Id}, Name: {level.Name}, Difficulty: {level.Difficulty}");
        }
    }
}
```

---

## Option 2: Minimalistic

Use this option if you well understand how to operate with EFCore on your own.

Call the function once at app startup. Important to do that before opening SQLite connection. This method call prepares SQLite.

```csharp
SQLitePCLRaw.Startup.Setup();
```

---

# Helpful information

Read more how to use [EntityFrameworkCore](https://learn.microsoft.com/en-us/ef/ef6/get-started?redirectedfrom=MSDN). My favorite approach is `Code First`.
Please keep in mind. Because of Unity's .NET Standard 2.1 restrictions we are only limited to use the old version of EntityFrameworkCore 5.0.17. Newer versions require newer .NET version which Unity doesn't support yet. Anyway the version 5.0.17 is a good one for sure!
