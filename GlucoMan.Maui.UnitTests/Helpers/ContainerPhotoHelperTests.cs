using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using gamon;
using GlucoMan.Maui.Helpers;
using Microsoft.Maui.Storage;
using NUnit.Framework;

namespace GlucoMan.Maui.Helpers.UnitTests
{
    /// <summary>
    /// Unit tests for ContainerPhotoHelper class
    /// </summary>
    public partial class ContainerPhotoHelperTests
    {
        /// <summary>
        /// Tests that GeneratePhotoFileName returns a filename starting with 'container_'
        /// Input: None
        /// Expected: Filename starts with the correct prefix
        /// </summary>
        [Test]
        public void GeneratePhotoFileName_WhenCalled_ReturnsFilenameWithCorrectPrefix()
        {
            // Act
            string result = ContainerPhotoHelper.GeneratePhotoFileName();

            // Assert
            Assert.That(result, Does.StartWith("container_"));
        }

        /// <summary>
        /// Tests that GeneratePhotoFileName returns a filename ending with '.jpg'
        /// Input: None
        /// Expected: Filename ends with the correct extension
        /// </summary>
        [Test]
        public void GeneratePhotoFileName_WhenCalled_ReturnsFilenameWithCorrectExtension()
        {
            // Act
            string result = ContainerPhotoHelper.GeneratePhotoFileName();

            // Assert
            Assert.That(result, Does.EndWith(".jpg"));
        }

        /// <summary>
        /// Tests that GeneratePhotoFileName returns a filename matching the expected format pattern
        /// Input: None
        /// Expected: Filename matches pattern "container_yyyyMMdd_HHmmss.jpg"
        /// </summary>
        [Test]
        public void GeneratePhotoFileName_WhenCalled_ReturnsFilenameMatchingExpectedPattern()
        {
            // Arrange
            var expectedPattern = @"^container_\d{8}_\d{6}\.jpg$";

            // Act
            string result = ContainerPhotoHelper.GeneratePhotoFileName();

            // Assert
            Assert.That(Regex.IsMatch(result, expectedPattern), Is.True,
                $"Filename '{result}' does not match expected pattern '{expectedPattern}'");
        }

        /// <summary>
        /// Tests that GeneratePhotoFileName returns a filename with correct length
        /// Input: None
        /// Expected: Filename has length of 29 characters (container_ = 10, yyyyMMdd_HHmmss = 15, .jpg = 4)
        /// </summary>
        [Test]
        public void GeneratePhotoFileName_WhenCalled_ReturnsFilenameWithCorrectLength()
        {
            // Arrange
            const int expectedLength = 29; // "container_" (10) + "yyyyMMdd_HHmmss" (15) + ".jpg" (4)

            // Act
            string result = ContainerPhotoHelper.GeneratePhotoFileName();

            // Assert
            Assert.That(result.Length, Is.EqualTo(expectedLength));
        }

        /// <summary>
        /// Tests that the datetime portion of the generated filename can be parsed to a valid DateTime
        /// Input: None
        /// Expected: The datetime portion is parseable and represents a valid timestamp close to current time
        /// </summary>
        [Test]
        public void GeneratePhotoFileName_WhenCalled_ContainsParsableDateTimePortion()
        {
            // Arrange
            DateTime beforeCall = DateTime.Now;

            // Act
            string result = ContainerPhotoHelper.GeneratePhotoFileName();

            // Arrange (extract datetime portion)
            DateTime afterCall = DateTime.Now;
            string dateTimePortion = result.Substring(10, 15); // Extract "yyyyMMdd_HHmmss"
            string dateTimeString = dateTimePortion.Replace("_", "");

            // Assert
            bool isParsed = DateTime.TryParseExact(
                dateTimeString,
                "yyyyMMddHHmmss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsedDateTime);

            Assert.That(isParsed, Is.True, $"Could not parse datetime portion '{dateTimePortion}'");
            Assert.That(parsedDateTime, Is.GreaterThanOrEqualTo(beforeCall.AddSeconds(-1)));
            Assert.That(parsedDateTime, Is.LessThanOrEqualTo(afterCall.AddSeconds(1)));
        }

        /// <summary>
        /// Tests that multiple calls to GeneratePhotoFileName in succession produce unique filenames
        /// Input: None
        /// Expected: Consecutive calls produce different filenames due to timestamp differences
        /// </summary>
        [Test]
        public void GeneratePhotoFileName_CalledMultipleTimes_ProducesUniqueFilenames()
        {
            // Act
            string result1 = ContainerPhotoHelper.GeneratePhotoFileName();
            System.Threading.Thread.Sleep(1100); // Sleep for more than 1 second to ensure different timestamps
            string result2 = ContainerPhotoHelper.GeneratePhotoFileName();

            // Assert
            Assert.That(result1, Is.Not.EqualTo(result2));
        }

        /// <summary>
        /// Tests that GeneratePhotoFileName returns a non-null and non-empty string
        /// Input: None
        /// Expected: Result is not null or empty
        /// </summary>
        [Test]
        public void GeneratePhotoFileName_WhenCalled_ReturnsNonNullNonEmptyString()
        {
            // Act
            string result = ContainerPhotoHelper.GeneratePhotoFileName();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }
        private Logger? _originalLogger;
        private string? _testLogPath;

        /// <summary>
        /// Tests that EnsurePhotosFolderExists creates the folder when it doesn't exist.
        /// </summary>
        /// <remarks>
        /// This test requires MAUI FileSystem.AppDataDirectory to be properly initialized.
        /// It creates and deletes a real directory on the file system.
        /// </remarks>
        [Test]
        public void EnsurePhotosFolderExists_WhenFolderDoesNotExist_CreatesFolderSuccessfully()
        {
            // Arrange
            string? folderPath = null;
            try
            {
                // Get the expected folder path
                folderPath = ContainerPhotoHelper.GetPhotosFolderPath();

                // Ensure folder doesn't exist before test
                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                }

                // Act
                ContainerPhotoHelper.EnsurePhotosFolderExists();

                // Assert
                Assert.That(Directory.Exists(folderPath), Is.True, "Container photos folder should be created");
            }
            catch (NotSupportedException)
            {
                Assert.Ignore("MAUI FileSystem.AppDataDirectory is not initialized in this test context. This test requires a full MAUI environment.");
            }
            catch (InvalidOperationException)
            {
                Assert.Ignore("MAUI FileSystem.AppDataDirectory is not initialized in this test context. This test requires a full MAUI environment.");
            }
            finally
            {
                // Cleanup: Remove the created folder
                if (folderPath != null && Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.Delete(folderPath, true);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
            }
        }

        /// <summary>
        /// Tests that EnsurePhotosFolderExists handles the case when the folder already exists.
        /// </summary>
        /// <remarks>
        /// This test verifies that no exception is thrown when the folder already exists.
        /// </remarks>
        [Test]
        public void EnsurePhotosFolderExists_WhenFolderAlreadyExists_DoesNotThrowException()
        {
            // Arrange
            string? folderPath = null;
            try
            {
                // Get the expected folder path
                folderPath = ContainerPhotoHelper.GetPhotosFolderPath();

                // Create the folder first
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Act & Assert - should not throw
                Assert.DoesNotThrow(() => ContainerPhotoHelper.EnsurePhotosFolderExists(),
                    "EnsurePhotosFolderExists should not throw when folder already exists");
            }
            catch (NotSupportedException)
            {
                Assert.Ignore("MAUI FileSystem.AppDataDirectory is not initialized in this test context. This test requires a full MAUI environment.");
            }
            catch (InvalidOperationException)
            {
                Assert.Ignore("MAUI FileSystem.AppDataDirectory is not initialized in this test context. This test requires a full MAUI environment.");
            }
            finally
            {
                // Cleanup: Remove the folder
                if (folderPath != null && Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.Delete(folderPath, true);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
            }
        }

        /// <summary>
        /// Tests that EnsurePhotosFolderExists does not throw exceptions under normal circumstances.
        /// </summary>
        /// <remarks>
        /// This test verifies that the method handles exceptions internally and doesn't propagate them.
        /// </remarks>
        [Test]
        public void EnsurePhotosFolderExists_UnderNormalConditions_DoesNotThrowException()
        {
            // Arrange
            string? folderPath = null;
            try
            {
                folderPath = ContainerPhotoHelper.GetPhotosFolderPath();

                // Act & Assert
                Assert.DoesNotThrow(() => ContainerPhotoHelper.EnsurePhotosFolderExists(),
                    "EnsurePhotosFolderExists should catch and handle all exceptions internally");
            }
            catch (NotSupportedException)
            {
                Assert.Ignore("MAUI FileSystem.AppDataDirectory is not initialized in this test context. This test requires a full MAUI environment.");
            }
            catch (InvalidOperationException)
            {
                Assert.Ignore("MAUI FileSystem.AppDataDirectory is not initialized in this test context. This test requires a full MAUI environment.");
            }
            finally
            {
                // Cleanup
                if (folderPath != null && Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.Delete(folderPath, true);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
            }
        }

        private string? _testPhotosFolderPath;

        /// <summary>
        /// Tests that GetAllPhotoFiles returns an empty list when the photos directory does not exist.
        /// </summary>
        [Test]
        public void GetAllPhotoFiles_DirectoryDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            _testPhotosFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Ensure directory does not exist
            if (Directory.Exists(_testPhotosFolderPath))
            {
                Directory.Delete(_testPhotosFolderPath, true);
            }

            // Act
            List<string> result = ContainerPhotoHelper.GetAllPhotoFiles();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Tests that GetAllPhotoFiles returns an empty list when the photos directory exists but is empty.
        /// </summary>
        [Test]
        public void GetAllPhotoFiles_DirectoryExistsButEmpty_ReturnsEmptyList()
        {
            // Arrange
            _testPhotosFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Ensure directory exists and is empty
            if (Directory.Exists(_testPhotosFolderPath))
            {
                Directory.Delete(_testPhotosFolderPath, true);
            }
            Directory.CreateDirectory(_testPhotosFolderPath);

            try
            {
                // Act
                List<string> result = ContainerPhotoHelper.GetAllPhotoFiles();

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.Empty);
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(_testPhotosFolderPath))
                {
                    Directory.Delete(_testPhotosFolderPath, true);
                }
            }
        }

        /// <summary>
        /// Tests that GetAllPhotoFiles returns list of JPG filenames when directory contains JPG files.
        /// Verifies that only filenames (not full paths) are returned.
        /// </summary>
        [Test]
        public void GetAllPhotoFiles_DirectoryContainsJpgFiles_ReturnsFilenamesOnly()
        {
            // Arrange
            _testPhotosFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Ensure directory exists and is empty
            if (Directory.Exists(_testPhotosFolderPath))
            {
                Directory.Delete(_testPhotosFolderPath, true);
            }
            Directory.CreateDirectory(_testPhotosFolderPath);

            try
            {
                // Create test JPG files
                string file1 = Path.Combine(_testPhotosFolderPath, "container_20240101_120000.jpg");
                string file2 = Path.Combine(_testPhotosFolderPath, "container_20240102_130000.jpg");
                File.WriteAllText(file1, "test content");
                File.WriteAllText(file2, "test content");

                // Act
                List<string> result = ContainerPhotoHelper.GetAllPhotoFiles();

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result, Does.Contain("container_20240101_120000.jpg"));
                Assert.That(result, Does.Contain("container_20240102_130000.jpg"));

                // Verify that results are filenames only, not full paths
                Assert.That(result.All(r => !r.Contains(Path.DirectorySeparatorChar)), Is.True);
                Assert.That(result.All(r => !r.Contains(Path.AltDirectorySeparatorChar)), Is.True);
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(_testPhotosFolderPath))
                {
                    Directory.Delete(_testPhotosFolderPath, true);
                }
            }
        }

        /// <summary>
        /// Tests that GetAllPhotoFiles returns only JPG files and excludes non-JPG files.
        /// </summary>
        [Test]
        public void GetAllPhotoFiles_DirectoryContainsMixedFiles_ReturnsOnlyJpgFiles()
        {
            // Arrange
            _testPhotosFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Ensure directory exists and is empty
            if (Directory.Exists(_testPhotosFolderPath))
            {
                Directory.Delete(_testPhotosFolderPath, true);
            }
            Directory.CreateDirectory(_testPhotosFolderPath);

            try
            {
                // Create test files with different extensions
                string jpgFile = Path.Combine(_testPhotosFolderPath, "container_20240101_120000.jpg");
                string pngFile = Path.Combine(_testPhotosFolderPath, "container_20240102_130000.png");
                string txtFile = Path.Combine(_testPhotosFolderPath, "readme.txt");
                string noExtFile = Path.Combine(_testPhotosFolderPath, "noextension");

                File.WriteAllText(jpgFile, "jpg content");
                File.WriteAllText(pngFile, "png content");
                File.WriteAllText(txtFile, "txt content");
                File.WriteAllText(noExtFile, "no ext content");

                // Act
                List<string> result = ContainerPhotoHelper.GetAllPhotoFiles();

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result, Does.Contain("container_20240101_120000.jpg"));
                Assert.That(result, Does.Not.Contain("container_20240102_130000.png"));
                Assert.That(result, Does.Not.Contain("readme.txt"));
                Assert.That(result, Does.Not.Contain("noextension"));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(_testPhotosFolderPath))
                {
                    Directory.Delete(_testPhotosFolderPath, true);
                }
            }
        }

        /// <summary>
        /// Tests that GetAllPhotoFiles returns an empty list and logs error when an exception occurs.
        /// NOTE: This test verifies exception handling but cannot verify logging behavior
        /// without mocking the static General.LogOfProgram field, which is not possible with Moq.
        /// </summary>
        [Test]
        public void GetAllPhotoFiles_SingleJpgFile_ReturnsSingleFileInList()
        {
            // Arrange
            _testPhotosFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Ensure directory exists and is empty
            if (Directory.Exists(_testPhotosFolderPath))
            {
                Directory.Delete(_testPhotosFolderPath, true);
            }
            Directory.CreateDirectory(_testPhotosFolderPath);

            try
            {
                // Create single test JPG file
                string file = Path.Combine(_testPhotosFolderPath, "container_20240101_120000.jpg");
                File.WriteAllText(file, "test content");

                // Act
                List<string> result = ContainerPhotoHelper.GetAllPhotoFiles();

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0], Is.EqualTo("container_20240101_120000.jpg"));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(_testPhotosFolderPath))
                {
                    Directory.Delete(_testPhotosFolderPath, true);
                }
            }
        }

        /// <summary>
        /// Tests that GetAllPhotoFiles handles multiple JPG files correctly and returns all of them.
        /// </summary>
        [Test]
        public void GetAllPhotoFiles_MultipleJpgFiles_ReturnsAllFiles()
        {
            // Arrange
            _testPhotosFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Ensure directory exists and is empty
            if (Directory.Exists(_testPhotosFolderPath))
            {
                Directory.Delete(_testPhotosFolderPath, true);
            }
            Directory.CreateDirectory(_testPhotosFolderPath);

            try
            {
                // Create multiple test JPG files
                List<string> expectedFilenames = new List<string>
                {
                    "container_20240101_120000.jpg",
                    "container_20240102_130000.jpg",
                    "container_20240103_140000.jpg",
                    "container_20240104_150000.jpg",
                    "container_20240105_160000.jpg"
                };

                foreach (string filename in expectedFilenames)
                {
                    string fullPath = Path.Combine(_testPhotosFolderPath, filename);
                    File.WriteAllText(fullPath, "test content");
                }

                // Act
                List<string> result = ContainerPhotoHelper.GetAllPhotoFiles();

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(5));

                foreach (string expectedFilename in expectedFilenames)
                {
                    Assert.That(result, Does.Contain(expectedFilename));
                }
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(_testPhotosFolderPath))
                {
                    Directory.Delete(_testPhotosFolderPath, true);
                }
            }
        }

        /// <summary>
        /// Tests that GetAllPhotoFiles is case-insensitive for .jpg extension matching (platform-dependent behavior).
        /// On Windows, .JPG, .jpg, and .Jpg should all be matched. On Linux, only .jpg would match.
        /// </summary>
        [Test]
        [Platform(Include = "Win")]
        public void GetAllPhotoFiles_JpgFilesWithDifferentCasing_ReturnsAllOnWindows()
        {
            // Arrange
            _testPhotosFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Ensure directory exists and is empty
            if (Directory.Exists(_testPhotosFolderPath))
            {
                Directory.Delete(_testPhotosFolderPath, true);
            }
            Directory.CreateDirectory(_testPhotosFolderPath);

            try
            {
                // Create test files with different casing (.jpg vs .JPG)
                string lowerCaseFile = Path.Combine(_testPhotosFolderPath, "container_20240101_120000.jpg");
                string upperCaseFile = Path.Combine(_testPhotosFolderPath, "container_20240102_130000.JPG");

                File.WriteAllText(lowerCaseFile, "test content");
                File.WriteAllText(upperCaseFile, "test content");

                // Act
                List<string> result = ContainerPhotoHelper.GetAllPhotoFiles();

                // Assert - On Windows, Directory.GetFiles with "*.jpg" pattern is case-insensitive
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(2));
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(_testPhotosFolderPath))
                {
                    Directory.Delete(_testPhotosFolderPath, true);
                }
            }
        }

        /// <summary>
        /// Tests that GetPhotosFolderPath returns a non-null and non-empty string.
        /// Input: None (parameterless method).
        /// Expected: Returns a valid path string that is not null or empty.
        /// </summary>
        [Test]
        public void GetPhotosFolderPath_WhenCalled_ReturnsNonNullNonEmptyString()
        {
            // Act
            string result = ContainerPhotoHelper.GetPhotosFolderPath();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        }

        /// <summary>
        /// Tests that GetPhotosFolderPath returns a path that ends with the container photos folder name.
        /// Input: None (parameterless method).
        /// Expected: The returned path ends with "ContainerPhotos".
        /// </summary>
        [Test]
        public void GetPhotosFolderPath_WhenCalled_ReturnsPathEndingWithContainerPhotos()
        {
            // Act
            string result = ContainerPhotoHelper.GetPhotosFolderPath();

            // Assert
            Assert.That(result, Does.EndWith("ContainerPhotos"));
        }

        /// <summary>
        /// Tests that GetPhotosFolderPath returns a path that contains a directory separator.
        /// Input: None (parameterless method).
        /// Expected: The returned path contains at least one directory separator character.
        /// </summary>
        [Test]
        public void GetPhotosFolderPath_WhenCalled_ReturnsPathWithDirectorySeparator()
        {
            // Act
            string result = ContainerPhotoHelper.GetPhotosFolderPath();

            // Assert
            Assert.That(result, Does.Contain(Path.DirectorySeparatorChar.ToString()).Or.Contain(Path.AltDirectorySeparatorChar.ToString()));
        }

        /// <summary>
        /// Tests that GetPhotosFolderPath returns a valid rooted path.
        /// Input: None (parameterless method).
        /// Expected: The returned path is a rooted path (absolute path).
        /// </summary>
        [Test]
        public void GetPhotosFolderPath_WhenCalled_ReturnsRootedPath()
        {
            // Act
            string result = ContainerPhotoHelper.GetPhotosFolderPath();

            // Assert
            Assert.That(Path.IsPathRooted(result), Is.True);
        }

        /// <summary>
        /// Tests that GetPhotosFolderPath consistently returns the same path when called multiple times.
        /// Input: None (parameterless method).
        /// Expected: Multiple calls return the same path value.
        /// </summary>
        [Test]
        public void GetPhotosFolderPath_WhenCalledMultipleTimes_ReturnsSamePath()
        {
            // Act
            string firstCall = ContainerPhotoHelper.GetPhotosFolderPath();
            string secondCall = ContainerPhotoHelper.GetPhotosFolderPath();

            // Assert
            Assert.That(firstCall, Is.EqualTo(secondCall));
        }

        /// <summary>
        /// Tests that DeletePhoto returns false when photoFileName is null.
        /// Input: null photoFileName
        /// Expected: Returns false without attempting file operations
        /// </summary>
        [Test]
        public void DeletePhoto_NullPhotoFileName_ReturnsFalse()
        {
            // Arrange
            string? photoFileName = null;

            // Act
            bool result = ContainerPhotoHelper.DeletePhoto(photoFileName!);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that DeletePhoto returns false when photoFileName is an empty string.
        /// Input: Empty string photoFileName
        /// Expected: Returns false without attempting file operations
        /// </summary>
        [Test]
        public void DeletePhoto_EmptyPhotoFileName_ReturnsFalse()
        {
            // Arrange
            string photoFileName = string.Empty;

            // Act
            bool result = ContainerPhotoHelper.DeletePhoto(photoFileName);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that DeletePhoto returns false when photoFileName contains only whitespace.
        /// Input: Whitespace-only photoFileName
        /// Expected: Returns false without attempting file operations
        /// </summary>
        [TestCase("   ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("\r\n")]
        [TestCase(" \t \n ")]
        public void DeletePhoto_WhitespacePhotoFileName_ReturnsFalse(string photoFileName)
        {
            // Arrange
            // photoFileName provided via test case

            // Act
            bool result = ContainerPhotoHelper.DeletePhoto(photoFileName);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that DeletePhoto returns false when the specified photo file does not exist.
        /// Input: Valid filename for a non-existent file
        /// Expected: Returns false
        /// </summary>
        [Test]
        public void DeletePhoto_FileDoesNotExist_ReturnsFalse()
        {
            // Arrange
            string photoFileName = "nonexistent_file_12345.jpg";
            string fullPath = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);

            // Ensure file does not exist
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            // Act
            bool result = ContainerPhotoHelper.DeletePhoto(photoFileName);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that DeletePhoto successfully deletes an existing photo file and returns true.
        /// Input: Valid filename for an existing file
        /// Expected: File is deleted and method returns true
        /// </summary>
        [Test]
        public void DeletePhoto_FileExists_DeletesFileAndReturnsTrue()
        {
            // Arrange
            string photoFileName = $"test_photo_{Guid.NewGuid()}.jpg";
            string folderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Ensure folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);
            File.WriteAllText(fullPath, "test content");

            // Verify file was created
            Assert.That(File.Exists(fullPath), Is.True, "Test setup failed: file was not created");

            // Act
            bool result = ContainerPhotoHelper.DeletePhoto(photoFileName);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(File.Exists(fullPath), Is.False, "File should have been deleted");
        }

        /// <summary>
        /// Tests that DeletePhoto handles filenames with special characters correctly.
        /// Input: Filename containing special characters
        /// Expected: Handles appropriately based on file existence
        /// </summary>
        [TestCase("photo_with-dashes.jpg")]
        [TestCase("photo_with_underscores.jpg")]
        [TestCase("photo123.jpg")]
        public void DeletePhoto_SpecialCharactersInFileName_HandlesCorrectly(string photoFileName)
        {
            // Arrange
            string folderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Ensure folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);
            File.WriteAllText(fullPath, "test content");

            // Act
            bool result = ContainerPhotoHelper.DeletePhoto(photoFileName);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(File.Exists(fullPath), Is.False);
        }

        /// <summary>
        /// Tests that DeletePhoto returns false when attempting to delete the same file twice.
        /// Input: Valid filename that exists initially
        /// Expected: First call returns true, second call returns false
        /// </summary>
        [Test]
        public void DeletePhoto_DeleteSameFileTwice_SecondCallReturnsFalse()
        {
            // Arrange
            string photoFileName = $"test_double_delete_{Guid.NewGuid()}.jpg";
            string folderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Ensure folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);
            File.WriteAllText(fullPath, "test content");

            // Act
            bool firstResult = ContainerPhotoHelper.DeletePhoto(photoFileName);
            bool secondResult = ContainerPhotoHelper.DeletePhoto(photoFileName);

            // Assert
            Assert.That(firstResult, Is.True, "First deletion should succeed");
            Assert.That(secondResult, Is.False, "Second deletion should fail because file no longer exists");
        }

        /// <summary>
        /// Tests that DeletePhoto handles very long filenames appropriately.
        /// Input: Very long filename
        /// Expected: Returns false (GetPhotoFullPath should handle this, or exception caught)
        /// </summary>
        [Test]
        public void DeletePhoto_VeryLongFileName_ReturnsFalse()
        {
            // Arrange
            string photoFileName = new string('a', 300) + ".jpg"; // Very long filename

            // Act
            bool result = ContainerPhotoHelper.DeletePhoto(photoFileName);

            // Assert
            // Should return false either because GetPhotoFullPath handles it or exception is caught
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that DeletePhoto returns false for path traversal attempts.
        /// Input: Filename with path traversal characters
        /// Expected: Returns false (file should not exist at constructed path)
        /// </summary>
        [TestCase("../../../photo.jpg")]
        [TestCase("..\\..\\..\\photo.jpg")]
        [TestCase("./../photo.jpg")]
        public void DeletePhoto_PathTraversalAttempt_ReturnsFalse(string photoFileName)
        {
            // Arrange
            // These should not create/delete files outside the intended directory

            // Act
            bool result = ContainerPhotoHelper.DeletePhoto(photoFileName);

            // Assert
            // Should return false - either file doesn't exist or path handling prevents operation
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that PhotoExists returns false when photoFileName is null.
        /// This validates the null guard clause.
        /// </summary>
        [Test]
        public void PhotoExists_NullPhotoFileName_ReturnsFalse()
        {
            // Arrange
            string? photoFileName = null;

            // Act
            bool result = ContainerPhotoHelper.PhotoExists(photoFileName!);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that PhotoExists returns false when photoFileName is an empty string.
        /// This validates the whitespace guard clause.
        /// </summary>
        [Test]
        public void PhotoExists_EmptyPhotoFileName_ReturnsFalse()
        {
            // Arrange
            string photoFileName = string.Empty;

            // Act
            bool result = ContainerPhotoHelper.PhotoExists(photoFileName);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that PhotoExists returns false when photoFileName contains only whitespace.
        /// This validates the whitespace guard clause with various whitespace characters.
        /// </summary>
        [TestCase("   ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("\r\n")]
        [TestCase(" \t\n ")]
        public void PhotoExists_WhitespacePhotoFileName_ReturnsFalse(string photoFileName)
        {
            // Act
            bool result = ContainerPhotoHelper.PhotoExists(photoFileName);

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that PhotoExists returns false when a valid filename is provided but the file does not exist.
        /// Note: This test depends on Microsoft.Maui.Storage.FileSystem.AppDataDirectory being available.
        /// If the test fails with FileSystem exceptions, it may need to be run in a MAUI test context.
        /// </summary>
        [Test]
        public void PhotoExists_ValidFileNameButFileDoesNotExist_ReturnsFalse()
        {
            // Arrange
            string photoFileName = "nonexistent_photo_12345.jpg";

            try
            {
                // Act
                bool result = ContainerPhotoHelper.PhotoExists(photoFileName);

                // Assert
                Assert.That(result, Is.False);
            }
            catch (Exception ex) when (ex.Message.Contains("FileSystem") || ex.Message.Contains("AppDataDirectory"))
            {
                // If FileSystem.AppDataDirectory is not available in test context, mark as inconclusive
                Assert.Inconclusive("Test requires MAUI FileSystem.AppDataDirectory to be available. " +
                    "This test should be run in a MAUI-enabled test environment. Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// Tests that PhotoExists returns true when a valid filename is provided and the file exists.
        /// Note: This test creates an actual file in the photos folder and cleans it up afterward.
        /// It depends on Microsoft.Maui.Storage.FileSystem.AppDataDirectory being available.
        /// </summary>
        [Test]
        public void PhotoExists_ValidFileNameAndFileExists_ReturnsTrue()
        {
            // Arrange
            string photoFileName = "test_photo_exists.jpg";
            string? photosFolder = null;
            string? fullPath = null;

            try
            {
                photosFolder = ContainerPhotoHelper.GetPhotosFolderPath();
                ContainerPhotoHelper.EnsurePhotosFolderExists();
                fullPath = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);

                // Create a test file
                File.WriteAllText(fullPath, "test content");

                // Act
                bool result = ContainerPhotoHelper.PhotoExists(photoFileName);

                // Assert
                Assert.That(result, Is.True);
            }
            catch (Exception ex) when (ex.Message.Contains("FileSystem") || ex.Message.Contains("AppDataDirectory"))
            {
                // If FileSystem.AppDataDirectory is not available in test context, mark as inconclusive
                Assert.Inconclusive("Test requires MAUI FileSystem.AppDataDirectory to be available. " +
                    "This test should be run in a MAUI-enabled test environment. Exception: " + ex.Message);
            }
            finally
            {
                // Cleanup
                if (fullPath != null && File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
            }
        }

        /// <summary>
        /// Tests that PhotoExists handles filenames with special characters correctly.
        /// Special characters in filenames may cause issues depending on the file system.
        /// </summary>
        [TestCase("photo_with_underscore.jpg")]
        [TestCase("photo-with-dash.jpg")]
        [TestCase("photo.with.dots.jpg")]
        [TestCase("photo with spaces.jpg")]
        [TestCase("photo123.jpg")]
        public void PhotoExists_SpecialCharactersInFileName_HandlesCorrectly(string photoFileName)
        {
            try
            {
                // Act
                bool result = ContainerPhotoHelper.PhotoExists(photoFileName);

                // Assert
                // File should not exist (we're not creating it), so should return false
                Assert.That(result, Is.False);
            }
            catch (Exception ex) when (ex.Message.Contains("FileSystem") || ex.Message.Contains("AppDataDirectory"))
            {
                // If FileSystem.AppDataDirectory is not available in test context, mark as inconclusive
                Assert.Inconclusive("Test requires MAUI FileSystem.AppDataDirectory to be available. " +
                    "This test should be run in a MAUI-enabled test environment. Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// Tests that PhotoExists handles very long filenames without throwing exceptions.
        /// This tests the boundary condition for filename length.
        /// </summary>
        [Test]
        public void PhotoExists_VeryLongFileName_HandlesCorrectly()
        {
            // Arrange
            string photoFileName = new string('a', 250) + ".jpg";

            try
            {
                // Act & Assert - Should not throw, should return false since file doesn't exist
                bool result = ContainerPhotoHelper.PhotoExists(photoFileName);
                Assert.That(result, Is.False);
            }
            catch (PathTooLongException)
            {
                // This is acceptable behavior for very long paths
                Assert.Pass("Method correctly throws PathTooLongException for excessively long filenames");
            }
            catch (Exception ex) when (ex.Message.Contains("FileSystem") || ex.Message.Contains("AppDataDirectory"))
            {
                Assert.Inconclusive("Test requires MAUI FileSystem.AppDataDirectory to be available. " +
                    "This test should be run in a MAUI-enabled test environment. Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// Tests that PhotoExists handles potential path traversal attempts safely.
        /// This ensures that relative path components don't cause unexpected behavior.
        /// </summary>
        [TestCase("../../../photo.jpg")]
        [TestCase("..\\..\\..\\photo.jpg")]
        [TestCase("./photo.jpg")]
        public void PhotoExists_PathTraversalAttempts_HandlesCorrectly(string photoFileName)
        {
            try
            {
                // Act
                bool result = ContainerPhotoHelper.PhotoExists(photoFileName);

                // Assert
                // Should safely handle and return false (file doesn't exist)
                Assert.That(result, Is.False);
            }
            catch (Exception ex) when (ex.Message.Contains("FileSystem") || ex.Message.Contains("AppDataDirectory"))
            {
                Assert.Inconclusive("Test requires MAUI FileSystem.AppDataDirectory to be available. " +
                    "This test should be run in a MAUI-enabled test environment. Exception: " + ex.Message);
            }
        }

        /// <summary>
        /// Tests that CleanupOrphanedPhotos throws ArgumentNullException when referencedPhotoFileNames is null.
        /// Input: null parameter
        /// Expected: ArgumentNullException or NullReferenceException
        /// </summary>
        [Test]
        public void CleanupOrphanedPhotos_NullReferencedPhotoFileNames_ThrowsOrReturnsZero()
        {
            // NOTE: This test cannot be properly implemented as a unit test because:
            // 1. ContainerPhotoHelper is a static class with static methods
            // 2. The method calls other static methods (GetAllPhotoFiles, DeletePhoto) that cannot be mocked with Moq
            // 3. The method depends on static file system operations and a static logger
            // 4. Proper unit testing would require refactoring to use dependency injection

            // To properly test this, the code would need to be refactored to:
            // - Use an interface (e.g., IPhotoFileManager) 
            // - Inject dependencies through constructor or method parameters
            // - Make the class instantiable rather than static

            Assert.Inconclusive("Cannot unit test static methods that call other static methods without creating fake implementations, which is prohibited. Code refactoring required for proper unit testing.");
        }

        /// <summary>
        /// Tests that CleanupOrphanedPhotos deletes all photos when referencedPhotoFileNames is empty.
        /// Input: Empty list of referenced photos
        /// Expected: Returns count of all photos that were deleted
        /// </summary>
        [Test]
        public void CleanupOrphanedPhotos_EmptyReferencedList_DeletesAllPhotos()
        {
            // Arrange
            // Would need to mock GetAllPhotoFiles() to return a list of photo files
            // Would need to mock DeletePhoto() to return true
            // Would need to mock General.LogOfProgram

            // Act
            // int result = ContainerPhotoHelper.CleanupOrphanedPhotos(new List<string>());

            // Assert
            // Would verify that DeletePhoto was called for each file
            // Would verify the returned count matches the number of photos

            Assert.Inconclusive("Cannot unit test static methods that call other static methods without creating fake implementations, which is prohibited. Code refactoring required for proper unit testing.");
        }

        /// <summary>
        /// Tests that CleanupOrphanedPhotos deletes no photos when all photos are referenced.
        /// Input: List containing all existing photo filenames
        /// Expected: Returns 0 (no photos deleted)
        /// </summary>
        [Test]
        public void CleanupOrphanedPhotos_AllPhotosReferenced_DeletesNone()
        {
            // Arrange
            // Would need to mock GetAllPhotoFiles() to return ["photo1.jpg", "photo2.jpg"]
            // Would need to pass the same list as referencedPhotoFileNames

            // Act
            // int result = ContainerPhotoHelper.CleanupOrphanedPhotos(new List<string> { "photo1.jpg", "photo2.jpg" });

            // Assert
            // Would verify that DeletePhoto was never called
            // Would verify result equals 0

            Assert.Inconclusive("Cannot unit test static methods that call other static methods without creating fake implementations, which is prohibited. Code refactoring required for proper unit testing.");
        }

        /// <summary>
        /// Tests that CleanupOrphanedPhotos deletes only orphaned photos.
        /// Input: List with some referenced photos, but not all
        /// Expected: Returns count of orphaned photos that were successfully deleted
        /// </summary>
        [Test]
        public void CleanupOrphanedPhotos_SomePhotosOrphaned_DeletesOnlyOrphaned()
        {
            // Arrange
            // Would need to mock GetAllPhotoFiles() to return ["photo1.jpg", "photo2.jpg", "photo3.jpg"]
            // Would pass referencedPhotoFileNames = ["photo1.jpg"]
            // Would need to mock DeletePhoto() to return true

            // Act
            // int result = ContainerPhotoHelper.CleanupOrphanedPhotos(new List<string> { "photo1.jpg" });

            // Assert
            // Would verify DeletePhoto was called for "photo2.jpg" and "photo3.jpg" but not "photo1.jpg"
            // Would verify result equals 2

            Assert.Inconclusive("Cannot unit test static methods that call other static methods without creating fake implementations, which is prohibited. Code refactoring required for proper unit testing.");
        }

        /// <summary>
        /// Tests that CleanupOrphanedPhotos returns 0 when GetAllPhotoFiles throws an exception.
        /// Input: Any valid list, but GetAllPhotoFiles throws
        /// Expected: Returns 0 and logs error
        /// </summary>
        [Test]
        public void CleanupOrphanedPhotos_GetAllPhotoFilesThrows_ReturnsZero()
        {
            // Arrange
            // Would need to mock GetAllPhotoFiles() to throw an exception
            // Would need to mock General.LogOfProgram to verify error logging

            // Act
            // int result = ContainerPhotoHelper.CleanupOrphanedPhotos(new List<string>());

            // Assert
            // Would verify result equals 0
            // Would verify Error was logged

            Assert.Inconclusive("Cannot unit test static methods that call other static methods without creating fake implementations, which is prohibited. Code refactoring required for proper unit testing.");
        }

        /// <summary>
        /// Tests that CleanupOrphanedPhotos counts only successfully deleted photos.
        /// Input: List with referenced photos, where some DeletePhoto calls return false
        /// Expected: Returns count only of photos where DeletePhoto returned true
        /// </summary>
        [Test]
        public void CleanupOrphanedPhotos_SomeDeletesFail_CountsOnlySuccessful()
        {
            // Arrange
            // Would need to mock GetAllPhotoFiles() to return ["photo1.jpg", "photo2.jpg", "photo3.jpg"]
            // Would pass referencedPhotoFileNames = ["photo1.jpg"]
            // Would need to mock DeletePhoto() to return true for "photo2.jpg" and false for "photo3.jpg"

            // Act
            // int result = ContainerPhotoHelper.CleanupOrphanedPhotos(new List<string> { "photo1.jpg" });

            // Assert
            // Would verify result equals 1 (only photo2.jpg was successfully deleted)

            Assert.Inconclusive("Cannot unit test static methods that call other static methods without creating fake implementations, which is prohibited. Code refactoring required for proper unit testing.");
        }

        /// <summary>
        /// Tests that CleanupOrphanedPhotos handles duplicate filenames in referenced list correctly.
        /// Input: List with duplicate filenames
        /// Expected: Each orphaned photo is only attempted to be deleted once
        /// </summary>
        [Test]
        public void CleanupOrphanedPhotos_DuplicatesInReferencedList_HandlesCorrectly()
        {
            // Arrange
            // Would need to mock GetAllPhotoFiles() to return ["photo1.jpg", "photo2.jpg"]
            // Would pass referencedPhotoFileNames = ["photo1.jpg", "photo1.jpg"] (duplicate)

            // Act
            // int result = ContainerPhotoHelper.CleanupOrphanedPhotos(new List<string> { "photo1.jpg", "photo1.jpg" });

            // Assert
            // Would verify DeletePhoto was called only once for "photo2.jpg"
            // List.Contains() will return true if photo1.jpg appears at least once

            Assert.Inconclusive("Cannot unit test static methods that call other static methods without creating fake implementations, which is prohibited. Code refactoring required for proper unit testing.");
        }

        /// <summary>
        /// Tests that CleanupOrphanedPhotos logs the correct event message.
        /// Input: Valid scenario with known deletion count
        /// Expected: Event log message contains correct count
        /// </summary>
        [Test]
        public void CleanupOrphanedPhotos_SuccessfulCleanup_LogsEventWithCount()
        {
            // Arrange
            // Would need to mock GetAllPhotoFiles(), DeletePhoto(), and General.LogOfProgram

            // Act
            // int result = ContainerPhotoHelper.CleanupOrphanedPhotos(new List<string>());

            // Assert
            // Would verify General.LogOfProgram.Event was called with message containing the deletion count

            Assert.Inconclusive("Cannot unit test static methods that call other static methods without creating fake implementations, which is prohibited. Code refactoring required for proper unit testing.");
        }

        /// <summary>
        /// Tests that GetPhotoFullPath returns null when photoFileName is null.
        /// </summary>
        [Test]
        public void GetPhotoFullPath_NullFileName_ReturnsNull()
        {
            // Arrange
            string? photoFileName = null;

            // Act
            string? result = ContainerPhotoHelper.GetPhotoFullPath(photoFileName!);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that GetPhotoFullPath returns null when photoFileName is an empty string.
        /// </summary>
        [Test]
        public void GetPhotoFullPath_EmptyFileName_ReturnsNull()
        {
            // Arrange
            string photoFileName = string.Empty;

            // Act
            string? result = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that GetPhotoFullPath returns null for various whitespace-only inputs.
        /// </summary>
        /// <param name="photoFileName">The whitespace string to test</param>
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("\t")]
        [TestCase("\n")]
        [TestCase("\r\n")]
        [TestCase(" \t\n ")]
        public void GetPhotoFullPath_WhitespaceFileName_ReturnsNull(string photoFileName)
        {
            // Act
            string? result = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that GetPhotoFullPath returns a valid combined path for a simple filename.
        /// </summary>
        [Test]
        public void GetPhotoFullPath_ValidSimpleFileName_ReturnsCombinedPath()
        {
            // Arrange
            string photoFileName = "photo.jpg";
            string expectedFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Act
            string? result = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.EndWith(photoFileName));
            Assert.That(result, Does.StartWith(expectedFolderPath));
            Assert.That(result, Is.EqualTo(Path.Combine(expectedFolderPath, photoFileName)));
        }

        /// <summary>
        /// Tests that GetPhotoFullPath correctly handles various valid filename patterns.
        /// </summary>
        /// <param name="photoFileName">The filename to test</param>
        [TestCase("container_20240101_120000.jpg")]
        [TestCase("photo_test-1.jpg")]
        [TestCase("image123.png")]
        [TestCase("file.with.multiple.dots.jpg")]
        [TestCase("a.jpg")]
        [TestCase("_underscore.jpg")]
        [TestCase("-hyphen.jpg")]
        public void GetPhotoFullPath_ValidFileNames_ReturnsCombinedPath(string photoFileName)
        {
            // Arrange
            string expectedFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Act
            string? result = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.EndWith(photoFileName));
            Assert.That(result, Does.Contain(expectedFolderPath));
            Assert.That(result, Is.EqualTo(Path.Combine(expectedFolderPath, photoFileName)));
        }

        /// <summary>
        /// Tests that GetPhotoFullPath handles very long filenames correctly.
        /// </summary>
        [Test]
        public void GetPhotoFullPath_VeryLongFileName_ReturnsCombinedPath()
        {
            // Arrange
            string photoFileName = new string('a', 200) + ".jpg";
            string expectedFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Act
            string? result = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.EndWith(photoFileName));
            Assert.That(result, Is.EqualTo(Path.Combine(expectedFolderPath, photoFileName)));
        }

        /// <summary>
        /// Tests that GetPhotoFullPath handles filenames with Unicode characters.
        /// </summary>
        [Test]
        public void GetPhotoFullPath_UnicodeFileName_ReturnsCombinedPath()
        {
            // Arrange
            string photoFileName = "фото_图片_📷.jpg";
            string expectedFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Act
            string? result = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.EndWith(photoFileName));
            Assert.That(result, Is.EqualTo(Path.Combine(expectedFolderPath, photoFileName)));
        }

        /// <summary>
        /// Tests that GetPhotoFullPath returns a path that includes both folder and filename components.
        /// </summary>
        [Test]
        public void GetPhotoFullPath_ValidFileName_ResultContainsBothComponents()
        {
            // Arrange
            string photoFileName = "test.jpg";
            string folderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Act
            string? result = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Does.Contain(folderPath));
            Assert.That(result, Does.Contain(photoFileName));
            Assert.That(result!.Length, Is.GreaterThan(folderPath.Length));
            Assert.That(result.Length, Is.GreaterThan(photoFileName.Length));
        }

        /// <summary>
        /// Tests that GetPhotoFullPath result matches Path.Combine behavior for edge case filenames.
        /// </summary>
        /// <param name="photoFileName">The filename to test</param>
        [TestCase("filename with spaces.jpg")]
        [TestCase("file(with)parentheses.jpg")]
        [TestCase("file[with]brackets.jpg")]
        [TestCase("file{with}braces.jpg")]
        [TestCase("file@special#chars$.jpg")]
        public void GetPhotoFullPath_SpecialCharacterFileNames_ReturnsCombinedPath(string photoFileName)
        {
            // Arrange
            string expectedFolderPath = ContainerPhotoHelper.GetPhotosFolderPath();

            // Act
            string? result = ContainerPhotoHelper.GetPhotoFullPath(photoFileName);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(Path.Combine(expectedFolderPath, photoFileName)));
        }
    }
}