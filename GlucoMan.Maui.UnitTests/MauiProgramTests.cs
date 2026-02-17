using System;
using System.Threading.Tasks;

using CommunityToolkit.Maui;
using gamon;
using GlucoMan;
using GlucoMan.Maui;
using GlucoMan.Maui.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using NUnit.Framework;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace GlucoMan.Maui.UnitTests
{
    /// <summary>
    /// Unit tests for the <see cref="DummyAlarmScheduler"/> class.
    /// </summary>
    public class DummyAlarmSchedulerTests
    {
        /// <summary>
        /// Tests that ScheduleAsync returns a completed task when called with a null alarm.
        /// This verifies the dummy implementation handles null input gracefully without throwing exceptions.
        /// </summary>
        [Test]
        public async Task ScheduleAsync_NullAlarm_ReturnsCompletedTask()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();
            Alarm? alarm = null;

            // Act
            Task result = scheduler.ScheduleAsync(alarm!);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsCompleted, Is.True);
            Assert.That(result.Status, Is.EqualTo(TaskStatus.RanToCompletion));
            await result; // Verify it can be awaited without exception
        }

        /// <summary>
        /// Tests that ScheduleAsync returns a completed task when called with a valid alarm object.
        /// This verifies the dummy implementation completes immediately without performing any actual scheduling.
        /// </summary>
        [Test]
        public async Task ScheduleAsync_ValidAlarm_ReturnsCompletedTask()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();
            var alarm = new Alarm
            {
                IdAlarm = 1,
                ReminderText = "Test Reminder",
                TimeStart = new DateTimeAndText { DateTime = DateTime.Now },
                IsDisabled = false
            };

            // Act
            Task result = scheduler.ScheduleAsync(alarm);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsCompleted, Is.True);
            Assert.That(result.Status, Is.EqualTo(TaskStatus.RanToCompletion));
            await result; // Verify it can be awaited without exception
        }

        /// <summary>
        /// Tests that ScheduleAsync can be called multiple times consecutively without issues.
        /// This verifies the dummy implementation is stateless and can handle repeated calls.
        /// </summary>
        [Test]
        public async Task ScheduleAsync_MultipleCalls_ReturnsCompletedTaskEachTime()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();
            var alarm1 = new Alarm { IdAlarm = 1, ReminderText = "First Alarm" };
            var alarm2 = new Alarm { IdAlarm = 2, ReminderText = "Second Alarm" };

            // Act
            Task result1 = scheduler.ScheduleAsync(alarm1);
            Task result2 = scheduler.ScheduleAsync(alarm2);

            // Assert
            Assert.That(result1.IsCompleted, Is.True);
            Assert.That(result2.IsCompleted, Is.True);
            await result1;
            await result2;
        }

        /// <summary>
        /// Tests that ScheduleAsync returns a completed task when called with an alarm with all nullable properties set to null.
        /// This verifies the dummy implementation handles minimal alarm objects without throwing exceptions.
        /// </summary>
        [Test]
        public async Task ScheduleAsync_AlarmWithNullProperties_ReturnsCompletedTask()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();
            var alarm = new Alarm
            {
                IdAlarm = null,
                ReminderText = null,
                NextTriggerTime = null,
                IsDisabled = null,
                ValidTimeAfterStart = null,
                Duration = null,
                RepetitionTime = null,
                Interval = null,
                IsPlaying = null,
                EnablePlaySoundFile = null,
                SoundFilePath = null,
                RepeatCount = null
            };

            // Act
            Task result = scheduler.ScheduleAsync(alarm);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsCompleted, Is.True);
            Assert.That(result.Status, Is.EqualTo(TaskStatus.RanToCompletion));
            await result;
        }

        /// <summary>
        /// Tests that ScheduleAsync with various alarm ID values returns a completed task.
        /// This parameterized test verifies the dummy implementation handles different ID edge cases.
        /// </summary>
        /// <param name="alarmId">The alarm ID to test with.</param>
        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(int.MaxValue)]
        [TestCase(null)]
        public async Task ScheduleAsync_VariousAlarmIds_ReturnsCompletedTask(int? alarmId)
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();
            var alarm = new Alarm { IdAlarm = alarmId };

            // Act
            Task result = scheduler.ScheduleAsync(alarm);

            // Assert
            Assert.That(result.IsCompleted, Is.True);
            await result;
        }

        /// <summary>
        /// Tests that CancelAsync returns a completed task for various alarm ID values,
        /// including boundary values and common scenarios.
        /// </summary>
        /// <param name="idAlarm">The alarm ID to test.</param>
        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(100)]
        [TestCase(int.MaxValue)]
        public async Task CancelAsync_VariousAlarmIds_ReturnsCompletedTask(int idAlarm)
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();

            // Act
            Task result = scheduler.CancelAsync(idAlarm);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsCompleted, Is.True);
            await result; // Should not throw
        }

        /// <summary>
        /// Tests that CancelAsync with a negative alarm ID does not throw an exception.
        /// </summary>
        [Test]
        public void CancelAsync_NegativeAlarmId_DoesNotThrow()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();
            int negativeId = -999;

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await scheduler.CancelAsync(negativeId));
        }

        /// <summary>
        /// Tests that CancelAsync with zero alarm ID returns a completed task.
        /// </summary>
        [Test]
        public async Task CancelAsync_ZeroAlarmId_ReturnsCompletedTask()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();

            // Act
            Task result = scheduler.CancelAsync(0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsCompleted, Is.True);
            Assert.That(result.IsCompletedSuccessfully, Is.True);
            await result;
        }

        /// <summary>
        /// Tests that CancelAsync with maximum integer value returns a completed task.
        /// </summary>
        [Test]
        public async Task CancelAsync_MaxIntValue_ReturnsCompletedTask()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();

            // Act
            Task result = scheduler.CancelAsync(int.MaxValue);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsCompleted, Is.True);
            Assert.That(result.IsCompletedSuccessfully, Is.True);
            await result;
        }

        /// <summary>
        /// Tests that CancelAsync with minimum integer value returns a completed task.
        /// </summary>
        [Test]
        public async Task CancelAsync_MinIntValue_ReturnsCompletedTask()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();

            // Act
            Task result = scheduler.CancelAsync(int.MinValue);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsCompleted, Is.True);
            Assert.That(result.IsCompletedSuccessfully, Is.True);
            await result;
        }

        /// <summary>
        /// Tests that CancelAllAsync returns a non-null Task instance.
        /// Input: None (method has no parameters).
        /// Expected: A non-null Task is returned.
        /// </summary>
        [Test]
        public void CancelAllAsync_WhenCalled_ReturnsNonNullTask()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();

            // Act
            var result = scheduler.CancelAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that CancelAllAsync returns a completed task.
        /// Input: None (method has no parameters).
        /// Expected: The returned task has IsCompleted property set to true.
        /// </summary>
        [Test]
        public void CancelAllAsync_WhenCalled_ReturnsCompletedTask()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();

            // Act
            var result = scheduler.CancelAllAsync();

            // Assert
            Assert.That(result.IsCompleted, Is.True);
        }

        /// <summary>
        /// Tests that CancelAllAsync returns a successfully completed task.
        /// Input: None (method has no parameters).
        /// Expected: The returned task has IsCompletedSuccessfully property set to true.
        /// </summary>
        [Test]
        public void CancelAllAsync_WhenCalled_ReturnsSuccessfullyCompletedTask()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();

            // Act
            var result = scheduler.CancelAllAsync();

            // Assert
            Assert.That(result.IsCompletedSuccessfully, Is.True);
        }

        /// <summary>
        /// Tests that CancelAllAsync does not throw any exceptions.
        /// Input: None (method has no parameters).
        /// Expected: No exception is thrown during method execution.
        /// </summary>
        [Test]
        public void CancelAllAsync_WhenCalled_DoesNotThrow()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();

            // Act & Assert
            Assert.DoesNotThrow(() => scheduler.CancelAllAsync());
        }

        /// <summary>
        /// Tests that multiple calls to CancelAllAsync work correctly.
        /// Input: Multiple sequential calls to the method.
        /// Expected: Each call returns a completed task without errors.
        /// </summary>
        [Test]
        public void CancelAllAsync_WhenCalledMultipleTimes_ReturnsCompletedTaskEachTime()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();

            // Act
            var result1 = scheduler.CancelAllAsync();
            var result2 = scheduler.CancelAllAsync();
            var result3 = scheduler.CancelAllAsync();

            // Assert
            Assert.That(result1.IsCompleted, Is.True);
            Assert.That(result2.IsCompleted, Is.True);
            Assert.That(result3.IsCompleted, Is.True);
        }

        /// <summary>
        /// Tests that awaiting CancelAllAsync completes successfully.
        /// Input: None (method has no parameters).
        /// Expected: The method can be awaited without throwing exceptions or blocking.
        /// </summary>
        [Test]
        public async Task CancelAllAsync_WhenAwaited_CompletesSuccessfully()
        {
            // Arrange
            var scheduler = new DummyAlarmScheduler();

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await scheduler.CancelAllAsync());
        }
    }

    /// <summary>
    /// Tests for the MauiProgram class.
    /// Note: These are integration-style tests as MauiProgram.CreateMauiApp is a composition root
    /// that creates real objects, calls static methods with side effects, and cannot be properly unit tested in isolation.
    /// </summary>
    [TestFixture]
    public class MauiProgramTests
    {
        /// <summary>
        /// Tests that CreateMauiApp returns a non-null MauiApp instance.
        /// This test verifies the basic functionality of the composition root.
        /// Note: This test executes real code including static method calls (SetGlobalParameters, GeneralInitializationsAsync)
        /// and creates a Logger instance that writes to the file system.
        /// </summary>
        [Test]
        public void CreateMauiApp_WhenCalled_ReturnsNonNullMauiApp()
        {
            // Arrange & Act
            MauiApp? result = MauiProgram.CreateMauiApp();

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that CreateMauiApp registers LocalizationService as a singleton.
        /// Verifies that the service can be resolved from the built application's service provider.
        /// Note: This is an integration test that builds the real application and resolves services.
        /// </summary>
        [Test]
        public void CreateMauiApp_WhenCalled_RegistersLocalizationService()
        {
            // Arrange & Act
            MauiApp app = MauiProgram.CreateMauiApp();
            LocalizationService? service = app.Services.GetService<LocalizationService>();

            // Assert
            Assert.That(service, Is.Not.Null);
        }

        /// <summary>
        /// Tests that CreateMauiApp registers IBackgroundGpsService.
        /// Verifies that the interface can be resolved from the built application's service provider.
        /// The actual implementation depends on the platform (Android, Windows, or default).
        /// Note: This is an integration test that builds the real application and resolves services.
        /// </summary>
        [Test]
        public void CreateMauiApp_WhenCalled_RegistersBackgroundGpsService()
        {
            // Arrange & Act
            MauiApp app = MauiProgram.CreateMauiApp();
            IBackgroundGpsService? service = app.Services.GetService<IBackgroundGpsService>();

            // Assert
            Assert.That(service, Is.Not.Null);
        }

        /// <summary>
        /// Tests that CreateMauiApp registers ISystemAlarmScheduler.
        /// Verifies that the interface can be resolved from the built application's service provider.
        /// The actual implementation depends on the platform (Android, Windows, or DummyAlarmScheduler).
        /// Note: This is an integration test that builds the real application and resolves services.
        /// </summary>
        [Test]
        public void CreateMauiApp_WhenCalled_RegistersSystemAlarmScheduler()
        {
            // Arrange & Act
            MauiApp app = MauiProgram.CreateMauiApp();
            ISystemAlarmScheduler? service = app.Services.GetService<ISystemAlarmScheduler>();

            // Assert
            Assert.That(service, Is.Not.Null);
        }

#if WINDOWS
        /// <summary>
        /// Tests that CreateMauiApp registers the Windows-specific SystemAlarmScheduler implementation.
        /// Verifies that the resolved service is of the correct platform-specific type.
        /// Note: This test only runs on Windows builds.
        /// </summary>
        [Test]
        public void CreateMauiApp_OnWindows_RegistersWindowsSystemAlarmScheduler()
        {
            // Arrange & Act
            MauiApp app = MauiProgram.CreateMauiApp();
            ISystemAlarmScheduler? service = app.Services.GetService<ISystemAlarmScheduler>();

            // Assert
            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.InstanceOf<GlucoMan.Maui.Platforms.Windows.SystemAlarmScheduler>());
        }

        /// <summary>
        /// Tests that CreateMauiApp registers the Windows-specific DefaultBackgroundGpsService implementation.
        /// Verifies that the resolved service is of the correct platform-specific type.
        /// Note: This test only runs on Windows builds.
        /// </summary>
        [Test]
        public void CreateMauiApp_OnWindows_RegistersDefaultBackgroundGpsService()
        {
            // Arrange & Act
            MauiApp app = MauiProgram.CreateMauiApp();
            IBackgroundGpsService? service = app.Services.GetService<IBackgroundGpsService>();

            // Assert
            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.InstanceOf<DefaultBackgroundGpsService>());
        }
#endif

#if ANDROID
        /// <summary>
        /// Tests that CreateMauiApp registers the Android-specific SystemAlarmScheduler implementation.
        /// Verifies that the resolved service is of the correct platform-specific type.
        /// Note: This test only runs on Android builds.
        /// </summary>
        [Test]
        public void CreateMauiApp_OnAndroid_RegistersAndroidSystemAlarmScheduler()
        {
            // Arrange & Act
            MauiApp app = MauiProgram.CreateMauiApp();
            ISystemAlarmScheduler? service = app.Services.GetService<ISystemAlarmScheduler>();

            // Assert
            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.InstanceOf<GlucoMan.Maui.Platforms.Android.SystemAlarmScheduler>());
        }

        /// <summary>
        /// Tests that CreateMauiApp registers the Android-specific BackgroundGpsServiceAndroid implementation.
        /// Verifies that the resolved service is of the correct platform-specific type.
        /// Note: This test only runs on Android builds.
        /// </summary>
        [Test]
        public void CreateMauiApp_OnAndroid_RegistersBackgroundGpsServiceAndroid()
        {
            // Arrange & Act
            MauiApp app = MauiProgram.CreateMauiApp();
            IBackgroundGpsService? service = app.Services.GetService<IBackgroundGpsService>();

            // Assert
            Assert.That(service, Is.Not.Null);
            Assert.That(service, Is.InstanceOf<BackgroundGpsServiceAndroid>());
        }
#endif

#if DEBUG
        /// <summary>
        /// Tests that CreateMauiApp registers logging services in DEBUG builds.
        /// Verifies that ILoggerFactory can be resolved from the service provider.
        /// Note: This test only runs in DEBUG builds.
        /// </summary>
        [Test]
        public void CreateMauiApp_InDebugMode_RegistersLoggingServices()
        {
            // Arrange & Act
            MauiApp app = MauiProgram.CreateMauiApp();
            ILoggerFactory? loggerFactory = app.Services.GetService<ILoggerFactory>();

            // Assert
            Assert.That(loggerFactory, Is.Not.Null);
        }
#endif

        /// <summary>
        /// Tests that CreateMauiApp can be called multiple times without throwing exceptions.
        /// Verifies that the method is idempotent in terms of not crashing on repeated calls.
        /// Note: Multiple calls will have side effects including overwriting log files and resetting static state.
        /// </summary>
        [Test]
        public void CreateMauiApp_WhenCalledMultipleTimes_DoesNotThrow()
        {
            // Arrange & Act & Assert
            Assert.DoesNotThrow(() =>
            {
                MauiApp app1 = MauiProgram.CreateMauiApp();
                MauiApp app2 = MauiProgram.CreateMauiApp();

                Assert.That(app1, Is.Not.Null);
                Assert.That(app2, Is.Not.Null);
            });
        }
    }
}