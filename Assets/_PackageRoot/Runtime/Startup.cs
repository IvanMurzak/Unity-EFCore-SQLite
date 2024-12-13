
namespace SQLitePCLRaw
{
    public static class Startup
    {
        private static bool isInitialized = false;

        public static void Setup()
        {
            if (isInitialized)
                return;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN // ---- Windows
            SQLitePCL.Batteries_V2.Init();
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX // -- MacOS
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());
#elif UNITY_ANDROID // ----------------------------- Android
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
#elif UNITY_IOS // --------------------------------- iOS
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());
#else
            SQLitePCL.Batteries_V2.Init();
#endif
            isInitialized = true;
        }
    }
}