using System;

using GlucoMan.Maui.Helpers;
using NUnit.Framework;

namespace GlucoMan.Maui.Helpers.UnitTests;


/// <summary>
/// Unit tests for <see cref="DisplayOrientationHelper"/> class.
/// </summary>
[TestFixture]
public class DisplayOrientationHelperTests
{
    /// <summary>
    /// Tests that LockToPortrait can be called without throwing an exception.
    /// Note: This is a limited smoke test due to static dependencies.
    /// Full behavioral verification requires integration testing on actual devices.
    /// </summary>
    [Test]
    public void LockToPortrait_WhenCalled_DoesNotThrow()
    {
        // Arrange
        // No arrangement needed - method has no parameters

        // Act & Assert
        Assert.DoesNotThrow(() => DisplayOrientationHelper.LockToPortrait());
    }

    /// <summary>
    /// Tests that AllowAllOrientations executes without throwing an exception.
    /// Note: This test verifies the method can be called successfully but cannot verify
    /// the actual orientation change behavior due to platform-specific dependencies
    /// that cannot be mocked in a unit test environment.
    /// </summary>
    [Test]
    public void AllowAllOrientations_WhenCalled_DoesNotThrow()
    {
        // Arrange
        // No arrangement needed - static method with no parameters

        // Act & Assert
        Assert.DoesNotThrow(() => DisplayOrientationHelper.AllowAllOrientations());
    }

#if ANDROID
    /// <summary>
    /// Tests that AllowAllOrientations executes on Android platform.
    /// Note: This test requires Android platform context. If Platform.CurrentActivity is null,
    /// the internal implementation handles it gracefully and logs an error instead of throwing.
    /// </summary>
    [Test]
    public static void AllowAllOrientations_OnAndroid_ExecutesWithoutException()
    {
        // Arrange
        // Platform-specific behavior - requires Android runtime context

        // Act & Assert
        Assert.DoesNotThrow(() => DisplayOrientationHelper.AllowAllOrientations());
    }
#endif

#if WINDOWS
    /// <summary>
    /// Tests that AllowAllOrientations executes on Windows platform.
    /// On Windows, the method is a no-op and should always succeed.
    /// </summary>
    [Test]
    public static void AllowAllOrientations_OnWindows_ExecutesWithoutException()
    {
        // Arrange
        // Windows implementation is a no-op (no actual code execution)

        // Act & Assert
        Assert.DoesNotThrow(() => DisplayOrientationHelper.AllowAllOrientations());
    }
#endif
}