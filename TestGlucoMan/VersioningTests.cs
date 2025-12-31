using NUnit.Framework;
using System;
using System.Reflection;

namespace TestGlucoMan
{
    [TestFixture]
    public class VersioningTests
    {
        [Test]
        public void Version_ShouldBeInCorrectFormat()
        {
            // Arrange & Act
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0";
            
            // Assert
            Assert.That(version, Is.Not.Null.And.Not.Empty, "Version should not be null or empty");
            
            var parts = version.Split('.');
            Assert.That(parts.Length, Is.EqualTo(4).Or.EqualTo(5), 
                "Version should have 4 or 5 parts (Major.Minor.Patch.BuildDate[.BuildTime])");
        }

        [Test]
        public void Version_MajorMinorPatch_ShouldBe_0_9_6()
        {
            // Arrange & Act
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            
            // Assert
            Assert.That(version, Is.Not.Null, "Version should not be null");
            Assert.That(version.Major, Is.EqualTo(0), "Major version should be 0");
            Assert.That(version.Minor, Is.EqualTo(9), "Minor version should be 9");
            Assert.That(version.Build, Is.EqualTo(6), "Patch/Build version should be 6");
        }

        [Test]
        public void Version_BuildDate_ShouldBeValidFormat()
        {
            // Arrange & Act
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0.0";
            var parts = version.Split('.');
            
            // Skip if version doesn't have build date part
            if (parts.Length < 4)
            {
                Assert.Inconclusive("Version doesn't include build date");
                return;
            }
            
            string buildDateStr = parts.Length >= 4 ? parts[3] : "000000";
            
            // Assert
            Assert.That(buildDateStr.Length, Is.EqualTo(6).Or.EqualTo(1), 
                "Build date should be 6 digits (YYMMDD) or single digit (legacy)");
            
            if (buildDateStr.Length == 6)
            {
                // Parse YYMMDD
                bool canParseYear = int.TryParse(buildDateStr.Substring(0, 2), out int year);
                bool canParseMonth = int.TryParse(buildDateStr.Substring(2, 2), out int month);
                bool canParseDay = int.TryParse(buildDateStr.Substring(4, 2), out int day);
                
                Assert.That(canParseYear, Is.True, "Year part should be numeric");
                Assert.That(canParseMonth, Is.True, "Month part should be numeric");
                Assert.That(canParseDay, Is.True, "Day part should be numeric");
                
                Assert.That(year, Is.GreaterThanOrEqualTo(25), "Year should be >= 25 (2025+)");
                Assert.That(month, Is.InRange(1, 12), "Month should be between 1 and 12");
                Assert.That(day, Is.InRange(1, 31), "Day should be between 1 and 31");
            }
        }

        [Test]
        public void Version_BuildTime_ShouldBeValidFormat()
        {
            // Arrange & Act
            string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0.0";
            var parts = version.Split('.');
            
            // Skip if version doesn't have build time part
            if (parts.Length < 5)
            {
                Assert.Inconclusive("Version doesn't include build time");
                return;
            }
            
            string buildTimeStr = parts[4];
            
            // Assert
            Assert.That(buildTimeStr.Length, Is.EqualTo(4).Or.EqualTo(1), 
                "Build time should be 4 digits (HHmm) or single digit (legacy)");
            
            if (buildTimeStr.Length == 4)
            {
                // Parse HHmm
                bool canParseHour = int.TryParse(buildTimeStr.Substring(0, 2), out int hour);
                bool canParseMinute = int.TryParse(buildTimeStr.Substring(2, 2), out int minute);
                
                Assert.That(canParseHour, Is.True, "Hour part should be numeric");
                Assert.That(canParseMinute, Is.True, "Minute part should be numeric");
                
                Assert.That(hour, Is.InRange(0, 23), "Hour should be between 0 and 23");
                Assert.That(minute, Is.InRange(0, 59), "Minute should be between 0 and 59");
            }
        }

        [Test]
        public void VersionHelper_ParseVersion_ShouldParseCorrectly()
        {
            // Arrange
            string testVersion = "0.9.6.250129.1445";
            
            // Act
            var (major, minor, patch, buildDateTime) = VersionHelper.ParseVersion(testVersion);
            
            // Assert
            Assert.That(major, Is.EqualTo(0), "Major version should be 0");
            Assert.That(minor, Is.EqualTo(9), "Minor version should be 9");
            Assert.That(patch, Is.EqualTo(6), "Patch version should be 6");
            Assert.That(buildDateTime.Year, Is.EqualTo(2025), "Year should be 2025");
            Assert.That(buildDateTime.Month, Is.EqualTo(1), "Month should be January");
            Assert.That(buildDateTime.Day, Is.EqualTo(29), "Day should be 29");
            Assert.That(buildDateTime.Hour, Is.EqualTo(14), "Hour should be 14 (2 PM)");
            Assert.That(buildDateTime.Minute, Is.EqualTo(45), "Minute should be 45");
        }

        [Test]
        public void VersionHelper_ParseVersion_InvalidFormat_ShouldThrow()
        {
            // Arrange
            string invalidVersion = "0.9.6";
            
            // Act & Assert
            Assert.Throws<ArgumentException>(() => VersionHelper.ParseVersion(invalidVersion));
        }

        [Test]
        public void Version_ShouldBeGreaterThan_PreviousVersion()
        {
            // Arrange
            string currentVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0";
            string previousVersion = "0.9.5.0.0";
            
            // Act
            var current = new Version(currentVersion);
            var previous = new Version(previousVersion);
            
            // Assert
            Assert.That(current, Is.GreaterThan(previous), 
                "Current version should be greater than previous version 0.9.5");
        }
    }

    /// <summary>
    /// Helper class for parsing GlucoMan version strings
    /// </summary>
    public static class VersionHelper
    {
        /// <summary>
        /// Parses a version string in format: Major.Minor.Patch.YYMMDD.HHmm
        /// </summary>
        /// <param name="version">Version string to parse</param>
        /// <returns>Tuple with version components and build DateTime</returns>
        /// <exception cref="ArgumentException">If version format is invalid</exception>
        public static (int major, int minor, int patch, DateTime buildDateTime) ParseVersion(string version)
        {
            var parts = version.Split('.');
            if (parts.Length != 5)
                throw new ArgumentException($"Invalid version format. Expected 5 parts, got {parts.Length}");
            
            int major = int.Parse(parts[0]);
            int minor = int.Parse(parts[1]);
            int patch = int.Parse(parts[2]);
            
            // Parse build date: YYMMDD
            string dateStr = parts[3];
            if (dateStr.Length != 6)
                throw new ArgumentException($"Invalid build date format. Expected 6 digits, got {dateStr.Length}");
                
            int year = 2000 + int.Parse(dateStr.Substring(0, 2));
            int month = int.Parse(dateStr.Substring(2, 2));
            int day = int.Parse(dateStr.Substring(4, 2));
            
            // Parse build time: HHmm
            string timeStr = parts[4];
            if (timeStr.Length != 4)
                throw new ArgumentException($"Invalid build time format. Expected 4 digits, got {timeStr.Length}");
                
            int hour = int.Parse(timeStr.Substring(0, 2));
            int minute = int.Parse(timeStr.Substring(2, 2));
            
            DateTime buildDateTime = new DateTime(year, month, day, hour, minute, 0);
            
            return (major, minor, patch, buildDateTime);
        }

        /// <summary>
        /// Formats a version for display to users
        /// </summary>
        /// <param name="version">Version string</param>
        /// <returns>User-friendly version string</returns>
        public static string FormatForDisplay(string version)
        {
            try
            {
                var (major, minor, patch, buildDateTime) = ParseVersion(version);
                return $"v{major}.{minor}.{patch} (Build: {buildDateTime:yyyy-MM-dd HH:mm})";
            }
            catch
            {
                return version; // Return as-is if parsing fails
            }
        }
    }
}
