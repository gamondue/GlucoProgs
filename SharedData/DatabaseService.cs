namespace GlucoMan
{
    /// <summary>
    /// Singleton service providing access to the DataLayer.
    /// This class has minimal dependencies to allow use without coupling to Common class.
    /// </summary>
    public sealed class DatabaseService
    {
        private static readonly object _lock = new object();
        private static DatabaseService? _instance;
        private DataLayer? _database;
        private bool _isInitialized;
        private string? _databasePath;

        private DatabaseService() { }

        /// <summary>
        /// Gets the singleton instance of DatabaseService.
        /// </summary>
        public static DatabaseService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new DatabaseService();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Gets the DataLayer instance. Returns null if not initialized.
        /// </summary>
        internal DataLayer? Database => _database;

        /// <summary>
        /// Gets whether the DatabaseService has been initialized.
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// Initializes the DatabaseService with a SQLite database.
        /// This method should be called once at application startup.
        /// </summary>
        /// <param name="pathAndFileDatabase">Full path to the SQLite database file.</param>
        /// <exception cref="InvalidOperationException">Thrown if already initialized.</exception>
        public void Initialize(string pathAndFileDatabase)
        {
            lock (_lock)
            {
                if (_isInitialized)
                {
                    throw new InvalidOperationException("DatabaseService has already been initialized.");
                }

                _databasePath = pathAndFileDatabase;
                _database = new DL_Sqlite(pathAndFileDatabase);
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Closes the current database connection to release file locks.
        /// Call this before any FILE operation on the database file.
        /// </summary>
        public void CloseConnection()
        {
            lock (_lock)
            {
                _database = null;

                // Force garbage collection to release any lingering handles
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // SQLite specific: clear the connection pool
                Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            }
        }

        /// <summary>
        /// Re-opens the database connection after a file operation.
        /// Uses the same path that was used during Initialize().
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if Initialize() was never called.</exception>
        public void ReopenConnection()
        {
            lock (_lock)
            {
                if (string.IsNullOrEmpty(_databasePath))
                {
                    throw new InvalidOperationException("DatabaseService was never initialized.");
                }

                _database = new DL_Sqlite(_databasePath);
            }
        }

        /// <summary>
        /// Resets the singleton instance. Use only for testing purposes.
        /// </summary>
        internal static void ResetForTesting()
        {
            lock (_lock)
            {
                _instance?._database = null;
                if (_instance != null)
                {
                    _instance._isInitialized = false;
                    _instance._databasePath = null;
                }
                _instance = null;
            }
        }
    }
}
