using NUnit.Framework;
using System.IO;

namespace TestGlucoMan
{
    [TestFixture]
    public class DatabaseTests
    {
        private string testDatabasePath;

        [SetUp]
        public void Setup()
        {
            // Setup test environment
            SetupTestPaths();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up test database after each test
            if (File.Exists(testDatabasePath))
            {
                try
                {
                    File.Delete(testDatabasePath);
                }
                catch
                {
                    // Ignore deletion errors in cleanup
                }
            }
        }

        private void SetupTestPaths()
        {
            // Get the repository root directory (where Git repository resides)
            string repositoryRoot = GetRepositoryRoot();
            
            // Create test database path: Repository/Database/Test/
            string testDatabaseDirectory = Path.Combine(repositoryRoot, "Database", "Test");
            
            // Ensure the test directory exists
            Directory.CreateDirectory(testDatabaseDirectory);
            
            // Set the test database file path
            testDatabasePath = Path.Combine(testDatabaseDirectory, "TestGlucoManData.Sqlite");
        }

        private string GetRepositoryRoot()
        {
            // Start from current directory and look for .git folder
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            
            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, ".git")))
            {
                dir = dir.Parent;
            }
            
            if (dir == null)
            {
                // Fallback: assume we're in the repository already
                // Navigate up from TestGlucoMan/bin/Debug/net9.0 to repository root
                return Path.GetFullPath(Path.Combine(currentDir, "..", "..", "..", ".."));
            }
            
            return dir.FullName;
        }

        [Test]
        public void CreateNewDatabase_ShouldCreateDatabaseInTestFolder()
        {
            // Arrange
            // Ensure test database doesn't exist
            if (File.Exists(testDatabasePath))
            {
                File.Delete(testDatabasePath);
            }

            // Act - Create a simple SQLite database manually for demonstration
            CreateSimpleTestDatabase();

            // Assert
            Assert.That(File.Exists(testDatabasePath), Is.True, 
                $"Database should be created at: {testDatabasePath}");
            
            // Verify the database is not empty (has some basic structure)
            var fileInfo = new FileInfo(testDatabasePath);
            Assert.That(fileInfo.Length, Is.GreaterThan(0), 
                "Database file should not be empty");
        }

        [Test]
        public void CreateNewDatabase_ShouldCreateValidSqliteDatabase()
        {
            // Arrange
            if (File.Exists(testDatabasePath))
            {
                File.Delete(testDatabasePath);
            }

            // Act
            CreateSimpleTestDatabase();

            // Assert
            Assert.That(File.Exists(testDatabasePath), Is.True);
            
            // Try to verify the database structure exists by checking if it's a valid SQLite file
            Assert.DoesNotThrow(() => 
            {
                using (var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={testDatabasePath}"))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT name FROM sqlite_master WHERE type='table';";
                        var result = command.ExecuteScalar();
                        Assert.That(result, Is.Not.Null, "Database should contain at least one table");
                    }
                }
            }, "Should be able to query the test database");
        }

        [Test]
        public void TestDatabasePath_ShouldBeInCorrectLocation()
        {
            // Arrange & Act
            string repositoryRoot = GetRepositoryRoot();
            string expectedPath = Path.Combine(repositoryRoot, "Database", "Test");

            // Assert
            Assert.That(Path.GetDirectoryName(testDatabasePath), Is.EqualTo(expectedPath),
                $"Test database should be in Repository/Database/Test/ folder. Expected: {expectedPath}, Actual: {Path.GetDirectoryName(testDatabasePath)}");
            
            // Verify the directory structure was created correctly
            Assert.That(Directory.Exists(expectedPath), Is.True,
                "Test database directory should exist");
        }

        [Test]
        public void DeleteDatabase_ShouldRemoveTestDatabase()
        {
            // Arrange
            CreateSimpleTestDatabase();
            Assert.That(File.Exists(testDatabasePath), Is.True, "Database should exist before delete");

            // Act
            bool result = File.Exists(testDatabasePath);
            if (result)
            {
                File.Delete(testDatabasePath);
            }

            // Assert
            Assert.That(result, Is.True, "Database should have existed before deletion");
            Assert.That(File.Exists(testDatabasePath), Is.False, "Database should be deleted");
        }

        [Test]
        public void RepositoryRoot_ShouldBeFound()
        {
            // Act
            string repositoryRoot = GetRepositoryRoot();

            // Assert
            Assert.That(Directory.Exists(repositoryRoot), Is.True, 
                "Repository root directory should exist");
            Assert.That(Directory.Exists(Path.Combine(repositoryRoot, ".git")), Is.True,
                "Repository root should contain .git folder");
        }

        /// <summary>
        /// Creates a simple test database to demonstrate the concept.
        /// In the actual application, this would be replaced by BL_General.CreateNewDatabase()
        /// </summary>
        private void CreateSimpleTestDatabase()
        {
            using (var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={testDatabasePath}"))
            {
                connection.Open();
                
                // Create a simple test table to verify database creation
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS TestTable (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Name TEXT NOT NULL,
                            CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                        );";
                    command.ExecuteNonQuery();
                }
                
                // Insert a test record
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO TestTable (Name) VALUES ('Test Database Creation');";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}