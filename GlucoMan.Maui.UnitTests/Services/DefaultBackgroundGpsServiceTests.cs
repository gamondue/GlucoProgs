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

    /// <summary>
    /// Tests that StartTrackingAsync returns false and maintains initial state when permission check throws an exception.
    /// Input: Permission API throws exception during CheckStatusAsync.
    /// Expected: Method returns false, IsTracking remains false, TrackingStartTime remains null.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions.CheckStatusAsync to force exception.
    /// This test documents expected behavior when the permission check fails with an exception.
    /// The catch block at line 59-63 should catch any exception, log it, and return false.
    /// Consider refactoring to inject IPermissions abstraction for testability.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class to simulate exception - requires code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionCheckThrowsException_ReturnsFalseAndMaintainsInitialState()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        // This would need Permissions.CheckStatusAsync to throw an exception
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected: result should be false, state should remain unchanged
        // Assert.That(result, Is.False);
        // Assert.That(service.IsTracking, Is.False);
        // Assert.That(service.TrackingStartTime, Is.Null);
    }

    /// <summary>
    /// Tests that StartTrackingAsync properly clears positions queue before starting new tracking session.
    /// Input: Service instance with positions queue.
    /// Expected: Positions count is 0 after StartTrackingAsync completes successfully.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify position clearing without mocking Permissions API.
    /// Expected behavior: Line 42 clears all positions before setting tracking state.
    /// The while loop should dequeue all existing positions before starting.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenSuccessful_ClearsPositionsQueue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        // Would need to populate positions first, then verify clearing

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected: positions queue should be empty after successful start
        // Assert.That(result, Is.True);
        // Assert.That(service.GetPositionsCount(), Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that StartTrackingAsync sets TrackingStartTime to approximately DateTime.Now when successful.
    /// Input: Service instance starting tracking successfully.
    /// Expected: TrackingStartTime is set to current time (within reasonable tolerance).
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify without mocking Permissions API.
    /// Expected behavior: Line 45 sets trackingStartTime = DateTime.Now.
    /// Should verify time is within a reasonable window (e.g., 5 seconds).
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenSuccessful_SetsTrackingStartTimeToCurrentTime()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        var beforeStart = DateTime.Now;

        // Act
        var result = await service.StartTrackingAsync();
        var afterStart = DateTime.Now;

        // Assert
        // Expected: TrackingStartTime should be between beforeStart and afterStart
        // Assert.That(result, Is.True);
        // Assert.That(service.TrackingStartTime, Is.Not.Null);
        // Assert.That(service.TrackingStartTime.Value, Is.GreaterThanOrEqualTo(beforeStart));
        // Assert.That(service.TrackingStartTime.Value, Is.LessThanOrEqualTo(afterStart));
    }

    /// <summary>
    /// Tests that StartTrackingAsync called multiple times consecutively handles the scenario correctly.
    /// Input: Calling StartTrackingAsync twice in succession without stopping.
    /// Expected: Both calls should succeed (if permissions granted), timer should be recreated.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions class.
    /// Expected behavior: Second call should clear positions again, reset trackingStartTime,
    /// and create a new timer (old timer may not be properly disposed - potential resource leak).
    /// Note: Current implementation doesn't stop/dispose existing timer before creating new one.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_CalledMultipleTimes_RestartsTracking()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var firstResult = await service.StartTrackingAsync();
        var firstStartTime = service.TrackingStartTime;

        await Task.Delay(100); // Small delay to ensure different timestamp

        var secondResult = await service.StartTrackingAsync();
        var secondStartTime = service.TrackingStartTime;

        // Assert
        // Expected: both calls succeed, tracking remains true, startTime is updated
        // Assert.That(firstResult, Is.True);
        // Assert.That(secondResult, Is.True);
        // Assert.That(service.IsTracking, Is.True);
        // Assert.That(secondStartTime, Is.GreaterThan(firstStartTime));
        // Note: This may cause timer resource leak as old timer isn't disposed
    }

    /// <summary>
    /// Tests that StartTrackingAsync returns false and logs error when permission is permanently denied.
    /// Input: Permission check returns Denied and request also returns Denied.
    /// Expected: Returns false, logs error message, IsTracking remains false.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions class.
    /// Expected behavior: Lines 34-38 handle permission denial.
    /// Should log "Permission denied, cannot start tracking" and return false.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionPermanentlyDenied_ReturnsFalseAndLogsError()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        // Would need to mock Permissions.CheckStatusAsync to return Denied
        // and RequestAsync to also return Denied

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected: returns false, state unchanged, error logged
        // Assert.That(result, Is.False);
        // Assert.That(service.IsTracking, Is.False);
        // Assert.That(service.TrackingStartTime, Is.Null);
        // Verify LogOfProgram.Error was called with appropriate message
    }

    /// <summary>
    /// Tests that StartTrackingAsync initializes timer with correct interval of 10000 milliseconds.
    /// Input: Successful StartTrackingAsync call.
    /// Expected: Timer is created with 10-second (10000ms) interval.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify timer configuration without mocking Permissions and accessing private field.
    /// Expected behavior: Line 48 creates timer with 10000ms interval.
    /// Timer should have AutoReset=true (line 50) and be started (line 51).
    /// Elapsed event handler should call RecordPosition (line 49).
    /// </remarks>
    [Test]
    [Ignore("Cannot verify private timer field without reflection and cannot mock Permissions class")]
    public async Task StartTrackingAsync_WhenSuccessful_CreatesTimerWith10SecondInterval()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected: gpsTimer field should be initialized with 10000ms interval
        // Would need reflection to verify:
        // var timerField = typeof(DefaultBackgroundGpsService).GetField("gpsTimer", BindingFlags.NonPublic | BindingFlags.Instance);
        // var timer = (Timer)timerField.GetValue(service);
        // Assert.That(timer, Is.Not.Null);
        // Assert.That(timer.Interval, Is.EqualTo(10000));
        // Assert.That(timer.AutoReset, Is.True);
        // Assert.That(timer.Enabled, Is.True);
    }

    /// <summary>
    /// Tests that StartTrackingAsync maintains isTracking as false when it returns false due to permission denial.
    /// Input: Permission denied scenario.
    /// Expected: IsTracking property remains false after method returns false.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions class.
    /// Expected behavior: When permission is denied (lines 34-38), the method returns false
    /// before setting isTracking = true (line 44), so IsTracking should remain false.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionDenied_KeepsIsTrackingFalse()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // If permission denied:
        // Assert.That(result, Is.False);
        // Assert.That(service.IsTracking, Is.False);
    }

    /// <summary>
    /// Tests that StartTrackingAsync does not set TrackingStartTime when permission is denied.
    /// Input: Permission denied scenario.
    /// Expected: TrackingStartTime remains null after method returns false.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions class.
    /// Expected behavior: When permission is denied, method returns at line 37 before
    /// setting trackingStartTime at line 45, so TrackingStartTime should remain null.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionDenied_KeepsTrackingStartTimeNull()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // If permission denied:
        // Assert.That(result, Is.False);
        // Assert.That(service.TrackingStartTime, Is.Null);
    }

    /// <summary>
    /// Tests that StartTrackingAsync calls RecordPosition immediately after starting timer.
    /// Input: Successful StartTrackingAsync call.
    /// Expected: RecordPosition is called once immediately (line 54), before timer triggers.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify RecordPosition call without mocking Permissions and Geolocation APIs.
    /// Expected behavior: Line 54 calls RecordPosition immediately after starting timer.
    /// This ensures first position is recorded without waiting for timer interval.
    /// Timer will then trigger RecordPosition every 10 seconds via the Elapsed event handler (line 49).
    /// </remarks>
    [Test]
    [Ignore("Cannot verify RecordPosition call without mocking static dependencies")]
    public async Task StartTrackingAsync_WhenSuccessful_CallsRecordPositionImmediatelyAfterTimerStart()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected: RecordPosition called once immediately at line 54
        // Would need to mock or spy on RecordPosition method
        // Assert.That(result, Is.True);
        // Verify RecordPosition was called exactly once during StartTrackingAsync
    }

    /// <summary>
    /// Tests that StartTrackingAsync returns true when location permissions are already granted.
    /// Input: Service instance with LocationWhenInUse permission already granted.
    /// Expected: Method returns true, IsTracking is true, TrackingStartTime is set, timer is started.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions.CheckStatusAsync method with Moq.
    /// This test documents expected behavior when CheckStatusAsync returns PermissionStatus.Granted.
    /// The method should proceed to clear positions, set tracking state, create timer, and call RecordPosition.
    /// Consider refactoring to inject IPermissions abstraction for testability.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionAlreadyGranted_ReturnsTrue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        // Expected: CheckStatusAsync returns Granted, method proceeds without RequestAsync call
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected behavior documented:
        // - result should be true
        // - service.IsTracking should be true
        // - service.TrackingStartTime should be approximately DateTime.Now
        // - positions queue should be empty
        // - timer should be created with 10000ms interval and started
        // - RecordPosition should be called once immediately
        // Assert.That(result, Is.True);
        // Assert.That(service.IsTracking, Is.True);
        // Assert.That(service.TrackingStartTime, Is.Not.Null);
        // Assert.That(service.TrackingStartTime.Value, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(2)));
    }

    /// <summary>
    /// Tests that StartTrackingAsync returns false when location permission check returns Denied and request also returns Denied.
    /// Input: Service instance where permission check and request both return Denied.
    /// Expected: Method returns false, IsTracking remains false, TrackingStartTime remains null, error is logged.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions.CheckStatusAsync and RequestAsync methods with Moq.
    /// Expected behavior per lines 34-38: When RequestAsync returns non-Granted status,
    /// method logs error "Permission denied, cannot start tracking" and returns false.
    /// IsTracking and TrackingStartTime should remain at their initial values.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionDeniedAfterRequest_ReturnsFalse()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        // Expected: CheckStatusAsync returns Denied, RequestAsync returns Denied
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected behavior:
        // - result should be false
        // - service.IsTracking should remain false
        // - service.TrackingStartTime should remain null
        // - Logger should record error with message "DefaultBackgroundGpsService - Permission denied, cannot start tracking"
        // Assert.That(result, Is.False);
        // Assert.That(service.IsTracking, Is.False);
        // Assert.That(service.TrackingStartTime, Is.Null);
    }

    /// <summary>
    /// Tests that StartTrackingAsync returns true when permission check returns Denied but request returns Granted.
    /// Input: Permission initially denied, then granted after RequestAsync.
    /// Expected: Method returns true and proceeds normally after successful permission request.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions class.
    /// Expected behavior per lines 30-38: When CheckStatusAsync returns non-Granted but RequestAsync
    /// returns Granted, method continues past the permission check and proceeds normally.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionGrantedAfterRequest_ReturnsTrue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        // Expected: CheckStatusAsync returns Denied, RequestAsync returns Granted
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected behavior:
        // - result should be true
        // - service.IsTracking should be true
        // - service.TrackingStartTime should be set
        // Assert.That(result, Is.True);
        // Assert.That(service.IsTracking, Is.True);
        // Assert.That(service.TrackingStartTime, Is.Not.Null);
    }

    /// <summary>
    /// Tests that StartTrackingAsync clears the positions queue before starting new tracking session.
    /// Input: Service instance (positions would need to be added via integration test).
    /// Expected: Positions queue is cleared (line 42) before setting tracking state.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot test position clearing without either:
    /// 1. Mocking Permissions to allow method to proceed, or
    /// 2. Adding positions via production code (requires StartTrackingAsync to succeed).
    /// Expected behavior per line 42: while loop dequeues all positions before setting isTracking.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class or populate positions without integration test")]
    public async Task StartTrackingAsync_WhenCalled_ClearsPositionsQueue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        // Would need to populate positions first via successful tracking session

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected: positions.Count should be 0 after StartTrackingAsync
        // Assert.That(service.GetPositionsCount(), Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that StartTrackingAsync sets IsTracking to true when successful.
    /// Input: Service instance with permissions granted.
    /// Expected: IsTracking property returns true after successful start.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify without mocking Permissions API.
    /// Expected behavior per line 44: isTracking = true is set after clearing positions and before timer creation.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenSuccessful_SetsIsTrackingToTrue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // Assert.That(service.IsTracking, Is.True);
    }

    /// <summary>
    /// Tests that StartTrackingAsync sets TrackingStartTime to current DateTime when successful.
    /// Input: Service instance with permissions granted.
    /// Expected: TrackingStartTime is set to DateTime.Now (within reasonable tolerance).
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify without mocking Permissions API.
    /// Expected behavior per line 45: trackingStartTime = DateTime.Now.
    /// Should verify timestamp is within a few seconds of test execution time.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenSuccessful_SetsTrackingStartTimeToNow()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        var beforeCall = DateTime.Now;

        // Act
        var result = await service.StartTrackingAsync();
        var afterCall = DateTime.Now;

        // Assert
        // Assert.That(service.TrackingStartTime, Is.Not.Null);
        // Assert.That(service.TrackingStartTime.Value, Is.GreaterThanOrEqualTo(beforeCall));
        // Assert.That(service.TrackingStartTime.Value, Is.LessThanOrEqualTo(afterCall));
    }

    /// <summary>
    /// Tests that StartTrackingAsync creates timer with 10-second (10000ms) interval.
    /// Input: Service instance with permissions granted.
    /// Expected: Timer is created with Interval = 10000, AutoReset = true, and is started.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify timer creation without mocking Permissions and accessing private gpsTimer field.
    /// Expected behavior per lines 48-51:
    /// - new Timer(10000) creates timer with 10-second interval
    /// - gpsTimer.AutoReset = true sets recurring behavior
    /// - gpsTimer.Start() begins timer
    /// - Elapsed event handler is attached that calls RecordPosition
    /// </remarks>
    [Test]
    [Ignore("Cannot verify private timer field without reflection and cannot mock Permissions class")]
    public async Task StartTrackingAsync_WhenSuccessful_CreatesTimerWithCorrectInterval()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // Would need reflection to access private gpsTimer field:
        // var timer = GetPrivateField<Timer>(service, "gpsTimer");
        // Assert.That(timer, Is.Not.Null);
        // Assert.That(timer.Interval, Is.EqualTo(10000));
        // Assert.That(timer.AutoReset, Is.True);
        // Assert.That(timer.Enabled, Is.True);
    }

    /// <summary>
    /// Tests that StartTrackingAsync calls RecordPosition immediately after starting timer.
    /// Input: Service instance with permissions granted.
    /// Expected: RecordPosition is invoked once immediately (line 54) before returning.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify RecordPosition call without mocking Permissions and Geolocation APIs.
    /// Expected behavior per line 54: await RecordPosition() is called immediately after timer.Start().
    /// This ensures first position is recorded without waiting for first timer interval.
    /// Subsequent positions recorded via timer.Elapsed event handler (line 49).
    /// </remarks>
    [Test]
    [Ignore("Cannot verify RecordPosition call without mocking static dependencies")]
    public async Task StartTrackingAsync_WhenSuccessful_CallsRecordPositionImmediately()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected: RecordPosition was called, which would add position to queue
        // (Cannot verify without mocking Geolocation API used by RecordPosition)
    }

    /// <summary>
    /// Tests that StartTrackingAsync returns false when an exception occurs during execution.
    /// Input: Exception thrown during method execution.
    /// Expected: Exception is caught, logged, and method returns false.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot force exception without mocking static dependencies.
    /// Expected behavior per lines 59-63: try-catch wraps entire method body.
    /// Any exception should be caught, logged via Logger.Error, and method returns false.
    /// State (IsTracking, TrackingStartTime) should remain unchanged when exception occurs.
    /// </remarks>
    [Test]
    [Ignore("Cannot force exception without mocking static Permissions class")]
    public async Task StartTrackingAsync_WhenExceptionOccurs_ReturnsFalseAndLogsError()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        // Would need to mock Permissions to throw exception
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected behavior when exception thrown:
        // - result should be false
        // - service.IsTracking should remain false
        // - service.TrackingStartTime should remain null
        // - Logger should record error with message "DefaultBackgroundGpsService - StartTrackingAsync" and exception
        // Assert.That(result, Is.False);
        // Assert.That(service.IsTracking, Is.False);
        // Assert.That(service.TrackingStartTime, Is.Null);
    }

    /// <summary>
    /// Tests that calling StartTrackingAsync multiple times consecutively handles state correctly.
    /// Input: Multiple consecutive calls to StartTrackingAsync.
    /// Expected: Each call clears positions, resets timer, updates TrackingStartTime.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify without mocking Permissions.
    /// Expected behavior: Second call should clear positions again, create new timer (potential resource leak
    /// if old timer not disposed), and update trackingStartTime.
    /// Current implementation does not dispose existing timer before creating new one (lines 48-51).
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_CalledMultipleTimes_RestartsTrackingEachTime()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result1 = await service.StartTrackingAsync();
        await Task.Delay(100); // Small delay
        var firstStartTime = service.TrackingStartTime;

        await Task.Delay(100); // Ensure different timestamp
        var result2 = await service.StartTrackingAsync();
        var secondStartTime = service.TrackingStartTime;

        // Assert
        // Expected behavior:
        // - Both calls return true
        // - secondStartTime is later than firstStartTime
        // - Positions cleared on second call
        // - New timer created (old one not explicitly disposed - potential issue)
        // Assert.That(result1, Is.True);
        // Assert.That(result2, Is.True);
        // Assert.That(secondStartTime, Is.GreaterThan(firstStartTime));
    }

    /// <summary>
    /// Tests that StartTrackingAsync maintains correct state when permission status is restricted.
    /// Input: Permission check returns Restricted status.
    /// Expected: Method treats Restricted as not granted, requests permission.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot mock static Permissions class.
    /// Expected behavior per line 30: Any status != Granted triggers RequestAsync.
    /// PermissionStatus enum includes: Unknown, Denied, Disabled, Granted, Restricted, Limited.
    /// Method should handle all non-Granted statuses consistently.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static Permissions class - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenPermissionRestricted_RequestsPermission()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        // Expected: CheckStatusAsync returns Restricted (or Disabled, Limited, etc.)
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected: RequestAsync should be called for any non-Granted status
        // Result depends on RequestAsync outcome
    }

    /// <summary>
    /// Tests that StartTrackingAsync logs success event when tracking starts successfully.
    /// Input: Service instance with permissions granted.
    /// Expected: Logger.Event is called with message "DefaultBackgroundGpsService - Started in-app tracking".
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot verify logging without mocking static General.LogOfProgram field.
    /// Expected behavior per line 56: Logger.Event logs success message before returning true.
    /// Actual logging behavior depends on Logger initialization and configuration.
    /// </remarks>
    [Test]
    [Ignore("Cannot mock static General.LogOfProgram - requires integration testing or code refactoring for DI")]
    public async Task StartTrackingAsync_WhenSuccessful_LogsSuccessEvent()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        var result = await service.StartTrackingAsync();

        // Assert
        // Expected: General.LogOfProgram?.Event("DefaultBackgroundGpsService - Started in-app tracking") was called
        // Cannot verify without mocking or capturing log output
    }

    /// <summary>
    /// Tests that StartTrackingAsync maintains initial state when not called.
    /// Input: Newly instantiated service without calling StartTrackingAsync.
    /// Expected: IsTracking is false, TrackingStartTime is null, positions count is 0.
    /// </summary>
    [Test]
    public void StartTrackingAsync_NotCalled_ServiceHasCorrectInitialState()
    {
        // Arrange & Act
        var service = new DefaultBackgroundGpsService();

        // Assert
        Assert.That(service.IsTracking, Is.False);
        Assert.That(service.TrackingStartTime, Is.Null);
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that StartTrackingAsync handles null logger gracefully (General.LogOfProgram is null).
    /// Input: Service running in environment where General.LogOfProgram is null.
    /// Expected: Method executes without NullReferenceException due to null-conditional operator.
    /// </summary>
    /// <remarks>
    /// Expected behavior: Lines 36 and 56 use null-conditional operator (?.) when calling Logger methods.
    /// If General.LogOfProgram is null, logging is skipped and method continues normally.
    /// This test documents defensive programming but cannot verify behavior without mocking Permissions.
    /// </remarks>
    [Test]
    [Ignore("Cannot verify without mocking Permissions class - documents expected null-safe behavior")]
    public async Task StartTrackingAsync_WhenLoggerIsNull_DoesNotThrowNullReferenceException()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        // In real scenario, General.LogOfProgram could be null

        // Act & Assert
        // Should not throw NullReferenceException due to ?. operator usage
        // var result = await service.StartTrackingAsync();
        // Assert.DoesNotThrow is implicit in successful execution
    }

    /// <summary>
    /// Tests that GetAndClearPositions correctly dequeues and returns a single position from the queue.
    /// Input: Queue with one GpsPositionRecord.
    /// Expected: Returns list with one item, queue is empty after call, returned item matches the enqueued position.
    /// </summary>
    /// <remarks>
    /// LIMITATION: This test cannot be executed without architectural changes to the production code.
    /// The positions field is private with no public API to populate it except via StartTrackingAsync,
    /// which depends on static Permissions and Geolocation APIs that cannot be mocked with Moq.
    /// 
    /// Lines 47-49 (while loop body with result.Add(pos)) cannot be covered without:
    /// 1. Adding a protected/internal AddPosition method for testing, OR
    /// 2. Accepting IGeolocation as a constructor dependency, OR
    /// 3. Using reflection (forbidden by test requirements), OR
    /// 4. Integration testing with actual GPS hardware
    /// 
    /// To enable this test, consider refactoring DefaultBackgroundGpsService to:
    /// - Add: internal void AddPositionForTesting(GpsPositionRecord position) { positions.Enqueue(position); }
    /// - Or: Extract position recording to a mockable service
    /// </remarks>
    [Test]
    [Ignore("Cannot populate positions queue without code refactoring - see remarks for details")]
    public void GetAndClearPositions_WithSinglePosition_ReturnsListWithOneItemAndClearsQueue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        var expectedPosition = new GpsPositionRecord
        {
            Latitude = 37.7749,
            Longitude = -122.4194,
            Altitude = 10.5,
            Accuracy = 5.0f,
            Speed = 0.0f,
            Timestamp = DateTime.Now
        };

        // NOTE: No way to execute this line without code changes:
        // service.AddPositionForTesting(expectedPosition);

        // Act
        var result = service.GetAndClearPositions();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Latitude, Is.EqualTo(expectedPosition.Latitude));
        Assert.That(result[0].Longitude, Is.EqualTo(expectedPosition.Longitude));
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0), "Queue should be empty after GetAndClearPositions");
    }

    /// <summary>
    /// Tests that GetAndClearPositions correctly dequeues and returns multiple positions in FIFO order.
    /// Input: Queue with three GpsPositionRecords added in specific order.
    /// Expected: Returns list with three items in FIFO order, queue is empty after call.
    /// </summary>
    /// <remarks>
    /// LIMITATION: This test cannot be executed without architectural changes.
    /// Lines 47-49 need to be executed multiple times to verify the while loop processes all items.
    /// This is the primary test needed to achieve coverage of the uncovered lines.
    /// See GetAndClearPositions_WithSinglePosition_ReturnsListWithOneItemAndClearsQueue remarks for details.
    /// </remarks>
    [Test]
    [Ignore("Cannot populate positions queue without code refactoring - see remarks for details")]
    public void GetAndClearPositions_WithMultiplePositions_ReturnsAllInFifoOrderAndClearsQueue()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        var position1 = new GpsPositionRecord
        {
            Latitude = 37.7749,
            Longitude = -122.4194,
            Timestamp = DateTime.Now.AddMinutes(-2)
        };
        var position2 = new GpsPositionRecord
        {
            Latitude = 37.7750,
            Longitude = -122.4195,
            Timestamp = DateTime.Now.AddMinutes(-1)
        };
        var position3 = new GpsPositionRecord
        {
            Latitude = 37.7751,
            Longitude = -122.4196,
            Timestamp = DateTime.Now
        };

        // NOTE: No way to execute these lines without code changes:
        // service.AddPositionForTesting(position1);
        // service.AddPositionForTesting(position2);
        // service.AddPositionForTesting(position3);

        // Act
        var result = service.GetAndClearPositions();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result[0].Latitude, Is.EqualTo(position1.Latitude), "First position should match FIFO order");
        Assert.That(result[1].Latitude, Is.EqualTo(position2.Latitude), "Second position should match FIFO order");
        Assert.That(result[2].Latitude, Is.EqualTo(position3.Latitude), "Third position should match FIFO order");
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0), "Queue should be completely empty");
    }

    /// <summary>
    /// Tests that GetAndClearPositions actually clears the queue by verifying subsequent calls return empty lists.
    /// Input: Queue with positions, call GetAndClearPositions twice.
    /// Expected: First call returns all positions, second call returns empty list.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot execute without code refactoring.
    /// This test verifies that lines 47-49 (the while loop) fully drains the queue.
    /// </remarks>
    [Test]
    [Ignore("Cannot populate positions queue without code refactoring - see remarks for details")]
    public void GetAndClearPositions_CalledTwice_SecondCallReturnsEmptyList()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        var position1 = new GpsPositionRecord
        {
            Latitude = 37.7749,
            Longitude = -122.4194,
            Timestamp = DateTime.Now
        };
        var position2 = new GpsPositionRecord
        {
            Latitude = 37.7750,
            Longitude = -122.4195,
            Timestamp = DateTime.Now
        };

        // NOTE: No way to execute these lines without code changes:
        // service.AddPositionForTesting(position1);
        // service.AddPositionForTesting(position2);

        // Act
        var firstResult = service.GetAndClearPositions();
        var secondResult = service.GetAndClearPositions();

        // Assert
        Assert.That(firstResult.Count, Is.EqualTo(2), "First call should return all positions");
        Assert.That(secondResult, Is.Empty, "Second call should return empty list");
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0), "Queue should remain empty");
    }

    /// <summary>
    /// Tests that GetAndClearPositions handles positions with various edge case values correctly.
    /// Input: GpsPositionRecords with extreme/boundary values (min/max coordinates, null optionals).
    /// Expected: All positions returned correctly including edge case values.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot execute without code refactoring.
    /// This test verifies that lines 47-49 handle edge case position data correctly.
    /// </remarks>
    [Test]
    [Ignore("Cannot populate positions queue without code refactoring - see remarks for details")]
    public void GetAndClearPositions_WithEdgeCasePositionValues_ReturnsAllPositionsCorrectly()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        var positionWithNulls = new GpsPositionRecord
        {
            Latitude = -90.0,
            Longitude = -180.0,
            Altitude = null,
            Accuracy = null,
            Speed = null,
            Timestamp = DateTime.MinValue
        };
        var positionWithMaxValues = new GpsPositionRecord
        {
            Latitude = 90.0,
            Longitude = 180.0,
            Altitude = double.MaxValue,
            Accuracy = float.MaxValue,
            Speed = float.MaxValue,
            Timestamp = DateTime.MaxValue
        };
        var positionWithZeros = new GpsPositionRecord
        {
            Latitude = 0.0,
            Longitude = 0.0,
            Altitude = 0.0,
            Accuracy = 0.0f,
            Speed = 0.0f,
            Timestamp = DateTime.UtcNow
        };

        // NOTE: No way to execute these lines without code changes:
        // service.AddPositionForTesting(positionWithNulls);
        // service.AddPositionForTesting(positionWithMaxValues);
        // service.AddPositionForTesting(positionWithZeros);

        // Act
        var result = service.GetAndClearPositions();

        // Assert
        Assert.That(result.Count, Is.EqualTo(3));

        Assert.That(result[0].Altitude, Is.Null);
        Assert.That(result[0].Latitude, Is.EqualTo(-90.0));

        Assert.That(result[1].Altitude, Is.EqualTo(double.MaxValue));
        Assert.That(result[1].Accuracy, Is.EqualTo(float.MaxValue));

        Assert.That(result[2].Latitude, Is.EqualTo(0.0));
        Assert.That(result[2].Longitude, Is.EqualTo(0.0));
    }

    /// <summary>
    /// Tests that GetAndClearPositions handles a large number of positions efficiently.
    /// Input: Queue with 1000 positions.
    /// Expected: Returns all 1000 positions, queue is empty, no performance degradation.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot execute without code refactoring.
    /// This test verifies that lines 47-49 (while loop with Add) can handle high volume efficiently.
    /// </remarks>
    [Test]
    [Ignore("Cannot populate positions queue without code refactoring - see remarks for details")]
    public void GetAndClearPositions_WithLargeNumberOfPositions_ReturnsAllPositionsEfficiently()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        const int positionCount = 1000;

        // NOTE: No way to execute this without code changes:
        // for (int i = 0; i < positionCount; i++)
        // {
        //     service.AddPositionForTesting(new GpsPositionRecord
        //     {
        //         Latitude = 37.0 + i * 0.001,
        //         Longitude = -122.0 + i * 0.001,
        //         Timestamp = DateTime.Now.AddSeconds(i)
        //     });
        // }

        // Act
        var startTime = DateTime.Now;
        var result = service.GetAndClearPositions();
        var duration = DateTime.Now - startTime;

        // Assert
        Assert.That(result.Count, Is.EqualTo(positionCount));
        Assert.That(service.GetPositionsCount(), Is.EqualTo(0));
        Assert.That(duration.TotalSeconds, Is.LessThan(1.0), "Should complete in under 1 second");
    }

    /// <summary>
    /// Tests that StopTrackingAsync does not throw when gpsTimer is null (new instance scenario).
    /// Input: New instance of DefaultBackgroundGpsService with null gpsTimer.
    /// Expected: Method completes successfully without throwing exceptions, returns completed task.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_WhenGpsTimerIsNull_DoesNotThrow()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act & Assert
        Assert.DoesNotThrowAsync(async () => await service.StopTrackingAsync());
    }

    /// <summary>
    /// Tests that StopTrackingAsync maintains positions count when called.
    /// Input: Service instance with no positions.
    /// Expected: Positions count remains zero after stopping.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_WhenCalled_MaintainsPositionsCount()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        int countBefore = service.GetPositionsCount();

        // Act
        await service.StopTrackingAsync();
        int countAfter = service.GetPositionsCount();

        // Assert
        Assert.That(countBefore, Is.EqualTo(0));
        Assert.That(countAfter, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that StopTrackingAsync is idempotent and can be called repeatedly without side effects.
    /// Input: Calling StopTrackingAsync five times consecutively.
    /// Expected: All calls complete successfully, IsTracking remains false, no exceptions thrown.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_CalledRepeatedlyWithoutStarting_RemainsIdempotent()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act & Assert
        for (int i = 0; i < 5; i++)
        {
            Assert.DoesNotThrowAsync(async () => await service.StopTrackingAsync());
            Assert.That(service.IsTracking, Is.False, $"IsTracking should be false after call {i + 1}");
        }
    }

    /// <summary>
    /// Tests that StopTrackingAsync does not modify TrackingStartTime when it was never set.
    /// Input: New service instance that was never started.
    /// Expected: TrackingStartTime remains null after StopTrackingAsync.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_WhenTrackingNeverStarted_LeavesTrackingStartTimeNull()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        DateTime? initialTime = service.TrackingStartTime;

        // Act
        await service.StopTrackingAsync();
        DateTime? finalTime = service.TrackingStartTime;

        // Assert
        Assert.That(initialTime, Is.Null);
        Assert.That(finalTime, Is.Null);
    }

    /// <summary>
    /// Tests that StopTrackingAsync returns a task that is already completed (not still running).
    /// Input: New service instance.
    /// Expected: Returned task has IsCompleted = true, no async continuation needed.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_WhenCalled_ReturnsCompletedTaskImmediately()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();

        // Act
        Task result = service.StopTrackingAsync();

        // Assert
        Assert.That(result.IsCompleted, Is.True);
        Assert.DoesNotThrowAsync(async () => await result);
    }

    /// <summary>
    /// Tests that StopTrackingAsync properly stops and disposes the timer when it exists.
    /// </summary>
    /// <remarks>
    /// LIMITATION: This test cannot be executed in a unit test environment because:
    /// 1. Creating a non-null gpsTimer requires calling StartTrackingAsync
    /// 2. StartTrackingAsync depends on static Permissions.CheckStatusAsync which cannot be mocked with Moq
    /// 3. Cannot use reflection to set private gpsTimer field (prohibited by testing requirements)
    /// 4. Cannot create a fake Timer implementation (prohibited by testing requirements)
    /// 
    /// Expected behavior (lines 34-38 in source):
    /// - If gpsTimer is not null, it should call Stop(), Dispose(), and set to null
    /// - This ensures proper resource cleanup and prevents timer from firing after stop
    /// 
    /// To enable full unit testing, consider refactoring to:
    /// - Inject IPermissions dependency for mocking permission checks
    /// - Extract timer lifecycle to a protected virtual method or injected service
    /// - Make gpsTimer accessible via internal/protected property for testing
    /// </remarks>
    [Test]
    [Ignore("Cannot test timer disposal without mocking Permissions API or using reflection - requires code refactoring for DI")]
    public async Task StopTrackingAsync_WhenTimerExists_StopsAndDisposesTimer()
    {
        // This test documents the expected behavior for lines 36-38 which are currently not covered.
        // 
        // Expected test flow:
        // 1. Create service and successfully start tracking (requires mocked Permissions)
        // 2. Verify gpsTimer is not null (requires access to private field or reflection)
        // 3. Call StopTrackingAsync
        // 4. Verify gpsTimer.Stop() was called (requires timer mock)
        // 5. Verify gpsTimer.Dispose() was called (requires timer mock)
        // 6. Verify gpsTimer is set to null (requires access to private field)
        //
        // Architectural changes needed:
        // - Accept IPermissions in constructor for mocking
        // - Accept ITimerFactory for creating mockable timers
        // - Or expose timer state through protected virtual methods
    }

    /// <summary>
    /// Tests that StopTrackingAsync handles exceptions gracefully without propagating them.
    /// </summary>
    /// <remarks>
    /// LIMITATION: This test cannot be executed because:
    /// 1. The method's try-catch block (lines 30-46) catches all exceptions
    /// 2. Exceptions could only be triggered by Timer.Stop() or Timer.Dispose() throwing
    /// 3. Cannot mock or inject a faulty timer without dependency injection support
    /// 4. Cannot use reflection to set a faulty timer (prohibited by requirements)
    /// 5. Cannot mock static Logger (General.LogOfProgram) to verify error logging
    /// 
    /// Expected behavior (lines 43-45 in source):
    /// - Any exception during stop/dispose should be caught
    /// - Exception should be logged via General.LogOfProgram?.Error()
    /// - Method should still return Task.CompletedTask
    /// - isTracking should still be set to false
    /// 
    /// To enable testing exception handling, consider:
    /// - Inject ILogger dependency instead of using static General.LogOfProgram
    /// - Inject ITimer or ITimerFactory for mockable timer instances
    /// - Extract timer disposal to a protected virtual method that can be overridden in tests
    /// </remarks>
    [Test]
    [Ignore("Cannot test exception handling without dependency injection for Timer and Logger - requires code refactoring")]
    public async Task StopTrackingAsync_WhenTimerDisposalThrows_CatchesExceptionAndLogsError()
    {
        // This test documents the expected behavior for lines 43-45 which are currently not covered.
        //
        // Expected test flow:
        // 1. Create service with a mocked timer that throws on Dispose()
        // 2. Mock the logger to capture error calls
        // 3. Call StopTrackingAsync
        // 4. Verify exception was caught (method doesn't throw)
        // 5. Verify logger.Error was called with exception details
        // 6. Verify method still returns completed task
        // 7. Verify isTracking is still set to false
        //
        // Current architectural barriers:
        // - Timer cannot be injected or mocked
        // - Logger is static and cannot be mocked with Moq
        // - No way to force Timer.Dispose() to throw without reflection
    }

    /// <summary>
    /// Tests that calling StopTrackingAsync after a failed StartTrackingAsync maintains correct state.
    /// </summary>
    /// <remarks>
    /// LIMITATION: Cannot execute because StartTrackingAsync requires MAUI runtime with Permissions API.
    /// This test documents expected behavior when stop is called after a failed start attempt.
    /// 
    /// Expected behavior:
    /// - If StartTrackingAsync fails (returns false), timer remains null
    /// - StopTrackingAsync should handle this gracefully (null check at line 34)
    /// - IsTracking should remain false
    /// - No exceptions should be thrown
    /// </remarks>
    [Test]
    [Ignore("Cannot test without mocking Permissions API - requires integration testing or code refactoring")]
    public async Task StopTrackingAsync_AfterFailedStart_HandlesNullTimerCorrectly()
    {
        // Expected test flow:
        // 1. Mock Permissions.CheckStatusAsync to return Denied
        // 2. Call StartTrackingAsync (should return false, timer stays null)
        // 3. Call StopTrackingAsync
        // 4. Verify no exception thrown
        // 5. Verify IsTracking is false
        // 6. Verify method returns completed task
    }

    /// <summary>
    /// Tests concurrent calls to StopTrackingAsync for thread safety.
    /// Input: Multiple simultaneous calls to StopTrackingAsync.
    /// Expected: All calls complete successfully without race conditions or exceptions.
    /// </summary>
    [Test]
    public async Task StopTrackingAsync_ConcurrentCalls_CompletesWithoutExceptions()
    {
        // Arrange
        var service = new DefaultBackgroundGpsService();
        var tasks = new List<Task>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(async () => await service.StopTrackingAsync()));
        }

        // Assert
        Assert.DoesNotThrowAsync(async () => await Task.WhenAll(tasks));
        Assert.That(service.IsTracking, Is.False);
    }
}