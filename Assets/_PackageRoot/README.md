# Unity + EFCore + SQLite = ❤️

![npm](https://img.shields.io/npm/v/extensions.unity.bundle.efcore.sqlite) [![openupm](https://img.shields.io/npm/v/extensions.unity.extensions.unity.bundle.efcore.sqlite?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/extensions.unity.extensions.unity.bundle.efcore.sqlite/) ![License](https://img.shields.io/github/license/IvanMurzak/Unity-EFCore-SQLite) [![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/badges/StandWithUkraine.svg)](https://stand-with-ukraine.pp.ua)

Ready to go bundle package that includes references on [EntityFrameworkCore](https://github.com/dotnet/efcore) and [SQLitePCLRaw](https://github.com/ericsink/SQLitePCL.raw) packages that just works in this combination for the next platforms:

- ✔️ Windows
- ✔️ Android
- ✔️ iOS
-  MacOS (need to test)

# Usage

Call the function once at app startup. Important to do that before opening SQLite connection.

```C#
SQLitePCLRaw.Startup.Setup();
```

# Installation

- [Install OpenUPM-CLI](https://github.com/openupm/openupm-cli#installation)
- Open command line in Unity project folder
- Run the command

``` CLI
openupm add extensions.unity.bundle.efcore.sqlite
```
