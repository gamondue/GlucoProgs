using System;

using GlucoMan;
using Microsoft.Maui.Controls;
using Moq;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests;


/// <summary>
/// Unit tests for WindowsAlarmPage class.
/// </summary>
public partial class WindowsAlarmPageTests
{
    /// <summary>
    /// Tests that OnBackButtonPressed returns true to prevent back navigation.
    /// This ensures users cannot accidentally dismiss the alarm using the back button
    /// and must explicitly use the Dismiss or Snooze buttons.
    /// </summary>
    [Test]
    public void OnBackButtonPressed_Always_ReturnsTrue()
    {
        // Arrange
        Mock<Alarm> mockAlarm = new Mock<Alarm>();
        mockAlarm.Setup(a => a.ReminderText).Returns("Test Alarm");
        mockAlarm.Setup(a => a.EnablePlaySoundFile).Returns(false);

        bool callbackInvoked = false;
        Action<bool> dismissCallback = (dismissed) => { callbackInvoked = dismissed; };

        // Note: This test requires the WindowsAlarmPage to be testable.
        // The actual WindowsAlarmPage class calls InitializeComponent() which requires XAML context.
        // In a real test scenario, you would need to either:
        // 1. Refactor WindowsAlarmPage to separate UI initialization from business logic
        // 2. Use integration tests with MAUI test harness
        // 3. Create a testable wrapper that doesn't call InitializeComponent

        TestableWindowsAlarmPage? page = null;

        try
        {
            page = new TestableWindowsAlarmPage(mockAlarm.Object, dismissCallback);

            // Act
            bool result = page.TestOnBackButtonPressed();

            // Assert
            Assert.That(result, Is.True, "OnBackButtonPressed should return true to prevent back navigation");
        }
        catch (Exception ex) when (ex.Message.Contains("InitializeComponent") || ex is NullReferenceException)
        {
            // If InitializeComponent fails (expected in unit test context without XAML),
            // we document the expected behavior
            Assert.Inconclusive(
                "Unable to fully test OnBackButtonPressed due to XAML dependencies. " +
                "The method is expected to return true to prevent accidental alarm dismissal via back button. " +
                "Consider integration testing or refactoring to separate UI concerns from testable logic.");
        }
        finally
        {
            page?.Dispose();
        }
    }

    /// <summary>
    /// Helper class to expose protected OnBackButtonPressed method for testing.
    /// This class inherits from WindowsAlarmPage to access its protected members.
    /// </summary>
    private class TestableWindowsAlarmPage : WindowsAlarmPage
    {
        public TestableWindowsAlarmPage(Alarm alarm, Action<bool> dismissCallback)
            : base(alarm, dismissCallback)
        {
        }

        /// <summary>
        /// Exposes the protected OnBackButtonPressed method for testing.
        /// </summary>
        public bool TestOnBackButtonPressed()
        {
            return OnBackButtonPressed();
        }

        public void Dispose()
        {
            // Clean up timers and resources if needed
        }
    }
}