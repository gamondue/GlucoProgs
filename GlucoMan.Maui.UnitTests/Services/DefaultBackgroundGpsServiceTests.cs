using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

using GlucoMan.Maui.Services;
using Microsoft.Maui.ApplicationModel;
using NUnit.Framework;

namespace GlucoMan.Maui.Services.UnitTests;


/// <summary>
/// Unit tests for the DefaultBackgroundGpsService class.
/// </summary>
public partial class DefaultBackgroundGpsServiceTests
{
    /// <summary>
    /// Tests that GetAndClearPositions returns an empty list when no positions have been recorded.
    /// </summary>
    [Test]
    public void GetAndClearPositions_WhenNoPositionsRecorded_ReturnsEmptyList()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = service.GetAndClearPositions();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that GetAndClearPositions clears the positions queue when no positions exist.
    /// </summary>
    [Test]
    public void GetAndClearPositions_WhenNoPositionsRecorded_LeavesQueueEmpty()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        service.GetAndClearPositions();

        // Assert
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that multiple calls to GetAndClearPositions on an empty queue return empty lists.
    /// </summary>
    [Test]
    public void GetAndClearPositions_WhenCalledMultipleTimesOnEmptyQueue_ReturnsEmptyListsEachTime()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result1 = service.GetAndClearPositions();
        var result2 = service.GetAndClearPositions();
        var result3 = service.GetAndClearPositions();

        // Assert
        Assert.That(result1, Is.Empty);
        Assert.That(result2, Is.Empty);
        Assert.That(result3, Is.Empty);
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that GetAndClearPositions returns a distinct list instance on each call.
    /// </summary>
    [Test]
    public void GetAndClearPositions_WhenCalledMultipleTimes_ReturnsDifferentListInstances()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result1 = service.GetAndClearPositions();
        var result2 = service.GetAndClearPositions();

        // Assert
        Assert.That(result1, Is.Not.SameAs(result2));
    }

    /// <summary>
    /// Tests that GetAndClearPositions does not throw an exception when called on a newly created service.
    /// </summary>
    [Test]
    public void GetAndClearPositions_WhenCalledOnNewInstance_DoesNotThrow()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act & Assert
        Assert.DoesNotThrow(() => service.GetAndClearPositions());
    }

    // NOTE: Comprehensive testing of GetAndClearPositions with populated positions queue requires
    // either integration testing (using StartTrackingAsync with actual GPS hardware/emulator)
    // or production code changes to improve testability (e.g., adding a protected AddPosition method
    // or accepting an IGeolocation dependency for mocking).
    //
    // The following test scenarios cannot be fully implemented without such changes:
    // - GetAndClearPositions with a single position in queue
    // - GetAndClearPositions with multiple positions in queue
    // - Verification that positions are returned in FIFO order
    // - Verification that the queue is fully cleared after calling GetAndClearPositions
    // - Thread safety testing with concurrent access to the positions queue
    //
    // To enable full unit testing, consider refactoring DefaultBackgroundGpsService to:
    // 1. Accept IGeolocation as a constructor dependency
    // 2. Add an internal/protected AddPosition method for testing
    // 3. Extract position recording logic to a mockable service

    /// <summary>
    /// Tests that the TrackingStartTime property returns null when the service is newly instantiated.
    /// </summary>
    [Test]
    public void TrackingStartTime_WhenNewlyInstantiated_ReturnsNull()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        DateTime? result = service.TrackingStartTime;

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests that the TrackingStartTime property returns a non-null DateTime value after StartTrackingAsync is called successfully.
    /// Note: This test requires the MAUI Permissions API to be available and cannot be executed in a standard unit test environment.
    /// The test is marked as Ignore because the Permissions class is static and cannot be mocked with Moq.
    /// Consider using integration tests or refactoring the service to accept an IPermissions abstraction for better testability.
    /// </summary>
    [Test]
    [Ignore("Requires MAUI framework with Permissions API - use integration testing instead")]
    public async Task TrackingStartTime_AfterSuccessfulStartTracking_ReturnsDateTime()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        // Note: This will fail in unit tests because Permissions.CheckStatusAsync requires MAUI runtime
        bool started = await service.StartTrackingAsync();
        DateTime? result = service.TrackingStartTime;

        // Assert
        // If StartTrackingAsync succeeds, trackingStartTime should be set to DateTime.Now
        // Assert.That(started, Is.True);
        // Assert.That(result, Is.Not.Null);
        // Assert.That(result.Value, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
    }

    /// <summary>
    /// Tests that the TrackingStartTime property retains its value after StopTrackingAsync is called.
    /// Note: This test requires the MAUI Permissions API to be available and cannot be executed in a standard unit test environment.
    /// The test is marked as Ignore because the Permissions class is static and cannot be mocked with Moq.
    /// Consider using integration tests or refactoring the service to accept an IPermissions abstraction for better testability.
    /// </summary>
    [Test]
    [Ignore("Requires MAUI framework with Permissions API - use integration testing instead")]
    public async Task TrackingStartTime_AfterStopTracking_RetainsValue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        // Note: This will fail in unit tests because Permissions.CheckStatusAsync requires MAUI runtime
        bool started = await service.StartTrackingAsync();
        DateTime? valueAfterStart = service.TrackingStartTime;
        await service.StopTrackingAsync();
        DateTime? valueAfterStop = service.TrackingStartTime;

        // Assert
        // StopTrackingAsync does not reset trackingStartTime, so it should retain the same value
        // Assert.That(started, Is.True);
        // Assert.That(valueAfterStart, Is.Not.Null);
        // Assert.That(valueAfterStop, Is.EqualTo(valueAfterStart));
    }

    /// <summary>
    /// Tests that GetPositionsCount returns 0 for a newly instantiated service.
    /// </summary>
    [Test]
    public void GetPositionsCount_NewInstance_ReturnsZero()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        int count = service.GetPositionsCount();

        // Assert
        Assert.That(count, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that GetPositionsCount returns 0 after clearing an empty positions queue.
    /// </summary>
    [Test]
    public void GetPositionsCount_AfterClearingEmptyQueue_ReturnsZero()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        service.ClearPositions();

        // Act
        int count = service.GetPositionsCount();

        // Assert
        Assert.That(count, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that GetRecordedPositions returns an empty list when no positions have been recorded.
    /// Input: Newly instantiated service with no positions.
    /// Expected: Returns an empty list (not null).
    /// </summary>
    [Test]
    public void GetRecordedPositions_WhenNoPositionsRecorded_ReturnsEmptyList()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = service.GetRecordedPositions();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
        Assert.That(result.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that GetRecordedPositions returns a new list instance on each call.
    /// Input: Service instance called multiple times.
    /// Expected: Each call returns a different list instance (not the same reference).
    /// </summary>
    [Test]
    public void GetRecordedPositions_WhenCalledMultipleTimes_ReturnsNewListInstance()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result1 = service.GetRecordedPositions();
        var result2 = service.GetRecordedPositions();

        // Assert
        Assert.That(result1, Is.Not.SameAs(result2));
    }

    /// <summary>
    /// Tests that GetRecordedPositions does not modify the internal state.
    /// Input: Service instance with positions count checked before and after.
    /// Expected: Position count remains unchanged after calling GetRecordedPositions.
    /// </summary>
    [Test]
    public void GetRecordedPositions_WhenCalled_DoesNotModifyInternalState()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        var initialCount = service.GetPositionsCount();

        // Act
        var result = service.GetRecordedPositions();

        // Assert
        Assert.That(service.GetPositionsCount(), Is.EqualTo(initialCount));
    }

    /// <summary>
    /// Tests that GetRecordedPositions returns an empty list after positions are cleared.
    /// Input: Service with cleared positions.
    /// Expected: Returns an empty list.
    /// </summary>
    [Test]
    public void GetRecordedPositions_AfterClearingPositions_ReturnsEmptyList()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        service.ClearPositions();

        // Act
        var result = service.GetRecordedPositions();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    /// <summary>
    /// Tests that modifications to the returned list do not affect the internal positions.
    /// Input: Service instance where returned list is modified.
    /// Expected: Internal state (position count) remains unchanged.
    /// </summary>
    [Test]
    public void GetRecordedPositions_WhenReturnedListModified_DoesNotAffectInternalPositions()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        var initialCount = service.GetPositionsCount();

        // Act
        var result = service.GetRecordedPositions();
        result.Add(new GpsPositionRecord
        {
            Latitude = 40.7128,
            Longitude = -74.0060,
            Timestamp = DateTime.UtcNow
        });

        // Assert
        Assert.That(service.GetPositionsCount(), Is.EqualTo(initialCount));
    }

    /// <summary>
    /// Tests that ClearPositions does not throw an exception when called on a newly instantiated service with an empty queue.
    /// Expected: Method completes without throwing.
    /// </summary>
    [Test]
    public void ClearPositions_WhenQueueIsEmpty_DoesNotThrow()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act & Assert
        Assert.DoesNotThrow(() => service.ClearPositions());
    }

    /// <summary>
    /// Tests that ClearPositions results in an empty positions collection as verified by GetPositionsCount.
    /// Expected: GetPositionsCount returns 0 after clearing.
    /// </summary>
    [Test]
    public void ClearPositions_AfterClearing_GetPositionsCountReturnsZero()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        service.ClearPositions();

        // Assert
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that ClearPositions results in an empty positions collection as verified by GetRecordedPositions.
    /// Expected: GetRecordedPositions returns an empty list after clearing.
    /// </summary>
    [Test]
    public void ClearPositions_AfterClearing_GetRecordedPositionsReturnsEmptyList()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        service.ClearPositions();

        // Assert
        var positions = service.GetRecordedPositions();
        Assert.That(positions, Is.Not.Null);
        Assert.That(positions, Is.Empty);
    }

    /// <summary>
    /// Tests that ClearPositions can be called multiple times consecutively without throwing exceptions (idempotent behavior).
    /// Expected: Multiple calls complete successfully and queue remains empty.
    /// </summary>
    [Test]
    public void ClearPositions_CalledMultipleTimes_DoesNotThrowAndRemainsEmpty()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        service.ClearPositions();
        service.ClearPositions();
        service.ClearPositions();

        // Assert
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0));
        Assert.That(service.GetRecordedPositions(), Is.Empty);
    }

    /// <summary>
    /// Tests that IsTracking property returns false when a new instance is created.
    /// Expected: IsTracking should be false by default.
    /// </summary>
    [Test]
    public void IsTracking_InitialState_ReturnsFalse()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = service.IsTracking;

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Tests that IsTracking property returns false after StopTrackingAsync is called.
    /// Expected: IsTracking should be false after stopping tracking.
    /// </summary>
    [Test]
    public async System.Threading.Tasks.Task IsTracking_AfterStopTracking_ReturnsFalse()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        await service.StopTrackingAsync();
        var result = service.IsTracking;

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Tests that IsTracking property correctly reflects tracking state after multiple stop calls.
    /// Expected: IsTracking should remain false even after multiple stop operations.
    /// </summary>
    [Test]
    public async System.Threading.Tasks.Task IsTracking_AfterMultipleStopCalls_RemainsFalse()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        await service.StopTrackingAsync();
        await service.StopTrackingAsync();
        await service.StopTrackingAsync();
        var result = service.IsTracking;

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Tests that IsTracking property returns true after successful tracking start.
    /// This test requires actual MAUI runtime with location permissions or refactoring for dependency injection.
    /// NOTE: StartTrackingAsync depends on static Permissions API which cannot be mocked with Moq.
    /// Consider refactoring to use dependency injection for IPermissions to enable proper unit testing.
    /// </summary>
    [Test]
    [Ignore("Requires MAUI runtime environment with location permissions. Cannot mock static Permissions API.")]
    public async System.Threading.Tasks.Task IsTracking_AfterStartTracking_ReturnsTrue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        // This will fail in unit test environment due to Permissions.CheckStatusAsync being unavailable
        var started = await service.StartTrackingAsync();
        var result = service.IsTracking;

        // Assert
        // This assertion would be valid if permissions were granted
        // Assert.That(started, Is.True);
        // Assert.That(result, Is.True);
    }

    /// <summary>
    /// Tests that IsTracking property returns false after start-stop cycle.
    /// This test requires actual MAUI runtime with location permissions or refactoring for dependency injection.
    /// NOTE: StartTrackingAsync depends on static Permissions API which cannot be mocked with Moq.
    /// </summary>
    [Test]
    [Ignore("Requires MAUI runtime environment with location permissions. Cannot mock static Permissions API.")]
    public async System.Threading.Tasks.Task IsTracking_AfterStartThenStop_ReturnsFalse()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        await service.StartTrackingAsync();
        await service.StopTrackingAsync();
        var result = service.IsTracking;

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Tests that StartTrackingAsync can be called without throwing when permissions are available.
    /// </summary>
    /// <remarks>
    /// This test will attempt to call the real Permissions API and will behave based on the actual
    /// runtime environment. This is more of an integration test than a unit test.
    /// LIMITATION: Cannot mock Permissions static class with Moq.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionsGranted_ShouldReturnTrue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        // NOTE: This will call the real Permissions API which cannot be mocked
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected behavior: Returns true when permissions are granted
        // Expected behavior: isTracking should be true
        // Expected behavior: TrackingStartTime should be set
        // Expected behavior: Timer should be created and started
        // Expected behavior: RecordPosition should be called immediately
        Assert.That(result, Is.True);
        Assert.That(service.IsTracking, Is.True);
        Assert.That(service.TrackingStartTime, Is.Not.Null);
    }

    /// <summary>
    /// Tests that StartTrackingAsync returns false when location permission is denied.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions.CheckStatusAsync and RequestAsync methods with Moq.
    /// This test documents the expected behavior but cannot be executed as a unit test.
    /// Recommendation: Refactor to inject IPermissions service for testability.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionDenied_ShouldReturnFalse()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Expected behavior when permission is denied:
        // 1. Permissions.CheckStatusAsync returns PermissionStatus.Denied
        // 2. Permissions.RequestAsync returns PermissionStatus.Denied
        // 3. General.LogOfProgram.Error is called with "Permission denied" message
        // 4. Method returns false
        // 5. isTracking remains false
        // 6. trackingStartTime remains null

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        Assert.That(result, Is.False);
        Assert.That(service.IsTracking, Is.False);
        Assert.That(service.TrackingStartTime, Is.Null);
    }

    /// <summary>
    /// Tests that StartTrackingAsync returns false when permission is initially not granted but granted after request.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions class with Moq.
    /// Expected behavior: CheckStatusAsync returns Denied, RequestAsync returns Granted, method proceeds successfully.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionGrantedAfterRequest_ShouldReturnTrue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Expected behavior:
        // 1. Permissions.CheckStatusAsync returns PermissionStatus.Denied (or any non-Granted status)
        // 2. Permissions.RequestAsync is called and returns PermissionStatus.Granted
        // 3. Method continues to start tracking
        // 4. Returns true

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        Assert.That(result, Is.True);
        Assert.That(service.IsTracking, Is.True);
        Assert.That(service.TrackingStartTime, Is.Not.Null);
    }

    /// <summary>
    /// Tests that StartTrackingAsync clears previous positions before starting new tracking session.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot fully test without mocking Permissions API.
    /// This test verifies the positions clearing behavior in isolation.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenCalled_ShouldClearPreviousPositions()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Expected behavior:
        // After permissions are granted, the method should clear all positions from the queue
        // This is done via: while (positions.TryDequeue(out _)) { }
        // Positions count should be 0 after clearing

        // Act
        await service.StartTrackingAsync();

        // Assert
        // Positions should be cleared (count should be 0 after successful start)
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that StartTrackingAsync sets tracking state correctly.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions class, so this test cannot verify state changes
    /// without permission being actually granted in the test environment.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenSuccessful_ShouldSetTrackingState()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        await service.StartTrackingAsync();

        // Assert
        // Expected behavior:
        // isTracking should be set to true (line 44)
        // trackingStartTime should be set to DateTime.Now (line 45)
        Assert.That(service.IsTracking, Is.True);
        Assert.That(service.TrackingStartTime, Is.Not.Null);
        Assert.That(service.TrackingStartTime.Value, Is.LessThanOrEqualTo(DateTime.Now));
    }

    /// <summary>
    /// Tests that StartTrackingAsync handles exceptions gracefully and returns false.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot force an exception without mocking static dependencies.
    /// Expected behavior: Any exception during execution should be caught, logged, and return false.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class to force exceptions - requires code refactoring for DI")]
    public async Task StartTrackingAsync_WhenExceptionThrown_ShouldReturnFalse()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Expected behavior when exception occurs:
        // 1. Exception is caught in catch block (line 59)
        // 2. General.LogOfProgram.Error is called with exception details
        // 3. Method returns false (line 62)

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Tests basic instantiation of DefaultBackgroundGpsService.
    /// </summary>
    /// <remarks>
    /// This test verifies basic object creation and initial state without requiring any mocking.
    /// </remarks>
    [Test]
    public void DefaultBackgroundGpsService_WhenInstantiated_ShouldHaveCorrectInitialState()
    {
        // Act
        var service = new DefaultBackgroundGpsService();

        // Assert
        Assert.That(service, Is.Not.Null);
        Assert.That(service.IsTracking, Is.False);
        Assert.That(service.TrackingStartTime, Is.Null);
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that StartTrackingAsync creates and configures timer correctly.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify timer creation without mocking Permissions and accessing private timer field.
    /// Expected behavior: Timer should be created with 10000ms interval, AutoReset=true, and event handler attached.
    /// Timer.Start() should be called.
    /// </remarks>
    [Test]
    [Ignore("Cannot verify timer behavior without mocking Permissions and accessing private timer field")]
    public async Task StartTrackingAsync_WhenSuccessful_ShouldConfigureTimerCorrectly()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Expected behavior:
        // 1. Timer created with 10000ms interval (line 48)
        // 2. Event handler attached to Elapsed event (line 49)
        // 3. AutoReset set to true (line 50)
        // 4. Timer.Start() called (line 51)

        // Act
        await service.StartTrackingAsync();

        // Assert
        // Cannot verify timer without accessing private field or waiting for timer events
        // This would require refactoring to inject ITimer or expose timer for testing
    }

    /// <summary>
    /// Tests that StartTrackingAsync calls RecordPosition immediately after starting.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify RecordPosition call without mocking Permissions, Geolocation, and other static dependencies.
    /// Expected behavior: RecordPosition should be called once immediately (line 54) and then periodically via timer.
    /// </remarks>
    [Test]
    [Ignore("Cannot verify RecordPosition call without mocking static dependencies")]
    public async Task StartTrackingAsync_WhenSuccessful_ShouldCallRecordPositionImmediately()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Expected behavior:
        // After timer is configured and started, RecordPosition is called immediately (line 54)
        // This should add a position to the queue if geolocation succeeds

        // Act
        await service.StartTrackingAsync();

        // Assert
        // Would need to verify RecordPosition was called and position was recorded
        // Requires mocking Geolocation.Default.GetLocationAsync which is static
    }

    /// <summary>
    /// Tests that StopTrackingAsync sets IsTracking to false when called on a new instance.
    /// Input: New instance of DefaultBackgroundGpsService with no tracking started.
    /// Expected: IsTracking property should be false after calling StopTrackingAsync.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_WhenNotTracking_SetsIsTrackingToFalse()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        await service.StopTrackingAsync();

        // Assert
        Assert.That(service.IsTracking, Is.False);
    }

    /// <summary>
    /// Tests that StopTrackingAsync returns a completed task.
    /// Input: New instance of DefaultBackgroundGpsService.
    /// Expected: Method should return a completed task without throwing exceptions.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_WhenCalled_ReturnsCompletedTask()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var task = service.StopTrackingAsync();

        // Assert
        Assert.That(task, Is.Not.Null);
        Assert.That(task.IsCompleted, Is.True);
        await task; // Ensure no exception is thrown
    }

    /// <summary>
    /// Tests that StopTrackingAsync can be called multiple times without throwing exceptions.
    /// Input: Calling StopTrackingAsync multiple times consecutively.
    /// Expected: All calls should complete successfully without exceptions, IsTracking remains false.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_CalledMultipleTimes_CompletesSuccessfully()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        await service.StopTrackingAsync();
        await service.StopTrackingAsync();
        await service.StopTrackingAsync();

        // Assert
        Assert.That(service.IsTracking, Is.False);
    }

    /// <summary>
    /// Tests that StopTrackingAsync sets IsTracking to false regardless of initial state.
    /// Input: Service in various states before calling StopTrackingAsync.
    /// Expected: IsTracking should be false after method execution.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_WithDifferentInitialStates_SetsIsTrackingToFalse()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act & Assert - First call
        await service.StopTrackingAsync();
        Assert.That(service.IsTracking, Is.False);

        // Act & Assert - Second call while already stopped
        await service.StopTrackingAsync();
        Assert.That(service.IsTracking, Is.False);
    }

    /// <summary>
    /// Tests that StopTrackingAsync does not affect the positions collection.
    /// Input: Service with no positions recorded.
    /// Expected: Positions count should remain unchanged after StopTrackingAsync.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_WhenCalled_DoesNotClearPositions()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        var initialCount = service.GetPositionsCount();

        // Act
        await service.StopTrackingAsync();

        // Assert
        Assert.That(service.GetPositionsCount(), Is.EqualTo(initialCount));
    }

    /// <summary>
    /// Tests that TrackingStartTime remains null when StopTrackingAsync is called without prior tracking.
    /// Input: New service instance that was never started.
    /// Expected: TrackingStartTime should remain null.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_WhenNeverStarted_TrackingStartTimeRemainsNull()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        await service.StopTrackingAsync();

        // Assert
        Assert.That(service.TrackingStartTime, Is.Null);
    }

    // NOTE: The following scenarios cannot be fully tested due to architectural limitations:
    // 1. Verification of Timer.Stop() and Timer.Dispose() calls requires access to private gpsTimer field
    // 2. Verification of logging calls requires mocking static gamon.General.LogOfProgram field, which is not possible with Moq
    // 3. Testing exception handling during timer disposal would require injecting a faulty timer, which cannot be done without DI
    // 4. Testing behavior after StartTrackingAsync requires the implementation of that method which was not provided
    // 
    // To enable comprehensive testing of this class, consider the following refactoring:
    // - Inject ILogger dependency instead of using static General.LogOfProgram
    // - Extract timer creation to a protected virtual method for testability
    // - Consider making gpsTimer lifecycle observable through events or protected virtual methods
}