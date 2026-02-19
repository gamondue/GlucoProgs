using System;

using GlucoMan.Maui;
using GlucoMan.Maui.Services;
using Moq;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests
{
    /// <summary>
    /// Unit tests for the MainPage class.
    /// </summary>
    public partial class MainPageTests
    {
        /// <summary>
        /// Tests that OnSizeAllocated can be called with normal positive width and height values.
        /// This test verifies the method executes without throwing exceptions for typical size values.
        /// Note: This test requires MAUI application infrastructure to be initialized and is marked as Ignored.
        /// To run this test, it should be converted to a MAUI UI test with proper application context.
        /// </summary>
        [Test]
        [Ignore("Requires MAUI application infrastructure (Application.Current, MauiContext, XAML compilation). Convert to integration test with MAUI test host.")]
        public void OnSizeAllocated_WithNormalPositiveValues_CallsBaseWithoutException()
        {
            // Arrange
            // Note: MainPage constructor requires:
            // - Application.Current.Handler.MauiContext.Services to be initialized
            // - XAML InitializeComponent() to succeed
            // - Navigation context for async initialization
            // These dependencies make pure unit testing impractical without a MAUI test host.

            // To implement this test:
            // 1. Set up a MAUI application context with required services
            // 2. Register LocalizationService in the service provider
            // 3. Initialize Application.Current with a test handler and MauiContext
            // 4. Create a TestableMainPage instance that exposes OnSizeAllocated
            // 5. Call the exposed method with test values

            double width = 400.0;
            double height = 800.0;

            // Act & Assert
            Assert.Inconclusive("Test requires MAUI infrastructure setup. Implement as integration test.");
        }

        /// <summary>
        /// Tests that OnSizeAllocated handles zero width and height values without throwing.
        /// This represents the boundary case where the control has no allocated size.
        /// </summary>
        [Test]
        [Ignore("Requires MAUI application infrastructure. See OnSizeAllocated_WithNormalPositiveValues_CallsBaseWithoutException for setup details.")]
        public void OnSizeAllocated_WithZeroValues_CallsBaseWithoutException()
        {
            // Arrange
            double width = 0.0;
            double height = 0.0;

            // Act & Assert
            Assert.Inconclusive("Test requires MAUI infrastructure setup. Implement as integration test.");
        }

        /// <summary>
        /// Tests that OnSizeAllocated handles negative width and height values.
        /// While negative sizes are unusual, the method does not validate input and should handle them gracefully.
        /// </summary>
        [Test]
        [Ignore("Requires MAUI application infrastructure. See OnSizeAllocated_WithNormalPositiveValues_CallsBaseWithoutException for setup details.")]
        public void OnSizeAllocated_WithNegativeValues_CallsBaseWithoutException()
        {
            // Arrange
            double width = -100.0;
            double height = -200.0;

            // Act & Assert
            Assert.Inconclusive("Test requires MAUI infrastructure setup. Implement as integration test.");
        }

        /// <summary>
        /// Tests that OnSizeAllocated handles double.MaxValue for width and height.
        /// This tests the upper boundary of the double type.
        /// </summary>
        [Test]
        [Ignore("Requires MAUI application infrastructure. See OnSizeAllocated_WithNormalPositiveValues_CallsBaseWithoutException for setup details.")]
        public void OnSizeAllocated_WithMaxValues_CallsBaseWithoutException()
        {
            // Arrange
            double width = double.MaxValue;
            double height = double.MaxValue;

            // Act & Assert
            Assert.Inconclusive("Test requires MAUI infrastructure setup. Implement as integration test.");
        }

        /// <summary>
        /// Tests that OnSizeAllocated handles double.MinValue for width and height.
        /// This tests the lower boundary of the double type (large negative value).
        /// </summary>
        [Test]
        [Ignore("Requires MAUI application infrastructure. See OnSizeAllocated_WithNormalPositiveValues_CallsBaseWithoutException for setup details.")]
        public void OnSizeAllocated_WithMinValues_CallsBaseWithoutException()
        {
            // Arrange
            double width = double.MinValue;
            double height = double.MinValue;

            // Act & Assert
            Assert.Inconclusive("Test requires MAUI infrastructure setup. Implement as integration test.");
        }

        /// <summary>
        /// Tests that OnSizeAllocated handles double.NaN for width and height.
        /// NaN represents an undefined or unrepresentable value and is an important edge case for numeric operations.
        /// </summary>
        [Test]
        [Ignore("Requires MAUI application infrastructure. See OnSizeAllocated_WithNormalPositiveValues_CallsBaseWithoutException for setup details.")]
        public void OnSizeAllocated_WithNaNValues_CallsBaseWithoutException()
        {
            // Arrange
            double width = double.NaN;
            double height = double.NaN;

            // Act & Assert
            Assert.Inconclusive("Test requires MAUI infrastructure setup. Implement as integration test.");
        }

        /// <summary>
        /// Tests that OnSizeAllocated handles double.PositiveInfinity for width and height.
        /// Infinity values represent unbounded size and should be handled gracefully.
        /// </summary>
        [Test]
        [Ignore("Requires MAUI application infrastructure. See OnSizeAllocated_WithNormalPositiveValues_CallsBaseWithoutException for setup details.")]
        public void OnSizeAllocated_WithPositiveInfinityValues_CallsBaseWithoutException()
        {
            // Arrange
            double width = double.PositiveInfinity;
            double height = double.PositiveInfinity;

            // Act & Assert
            Assert.Inconclusive("Test requires MAUI infrastructure setup. Implement as integration test.");
        }

        /// <summary>
        /// Tests that OnSizeAllocated handles double.NegativeInfinity for width and height.
        /// Negative infinity is an edge case that tests numeric boundary handling.
        /// </summary>
        [Test]
        [Ignore("Requires MAUI application infrastructure. See OnSizeAllocated_WithNormalPositiveValues_CallsBaseWithoutException for setup details.")]
        public void OnSizeAllocated_WithNegativeInfinityValues_CallsBaseWithoutException()
        {
            // Arrange
            double width = double.NegativeInfinity;
            double height = double.NegativeInfinity;

            // Act & Assert
            Assert.Inconclusive("Test requires MAUI infrastructure setup. Implement as integration test.");
        }

        /// <summary>
        /// Tests that OnSizeAllocated handles mixed edge case values (one normal, one special).
        /// This tests combinations of normal and boundary values.
        /// </summary>
        [Test]
        [Ignore("Requires MAUI application infrastructure. See OnSizeAllocated_WithNormalPositiveValues_CallsBaseWithoutException for setup details.")]
        public void OnSizeAllocated_WithMixedValues_CallsBaseWithoutException()
        {
            // Arrange
            double width = 400.0;
            double height = double.NaN;

            // Act & Assert
            Assert.Inconclusive("Test requires MAUI infrastructure setup. Implement as integration test.");
        }

        /// <summary>
        /// Tests that OnDisappearing-like logic does not throw when the localization service is null.
        /// This verifies the null-check protection in the OnDisappearing method.
        /// </summary>
        [Test]
        public void OnDisappearing_WhenLocalizationServiceIsNull_DoesNotThrow()
        {
            // Arrange
            var pageSimulator = new PageWithEventUnsubscription(null);

            // Act & Assert
            Assert.DoesNotThrow(() => pageSimulator.SimulateOnDisappearing());
        }

        /// <summary>
        /// Tests that OnDisappearing-like logic properly unsubscribes from the CultureChanged event
        /// when the localization service is not null.
        /// </summary>
        [Test]
        public void OnDisappearing_WhenLocalizationServiceIsNotNull_UnsubscribesFromCultureChangedEvent()
        {
            // Arrange
            var mockService = new Mock<LocalizationService>();
            var eventRaised = false;
            EventHandler handler = (s, e) => eventRaised = true;

            // Subscribe to the event
            mockService.Object.CultureChanged += handler;

            var pageSimulator = new PageWithEventUnsubscription(mockService.Object);
            pageSimulator.SubscribeToEvent();

            // Act
            pageSimulator.SimulateOnDisappearing();

            // Raise the event after unsubscription - the handler should not be called
            mockService.Raise(m => m.CultureChanged += null, EventArgs.Empty);

            // Assert
            // The event should have been properly unsubscribed, so our flag should remain false
            Assert.That(eventRaised, Is.False, "Event handler should be unsubscribed and not invoked");
        }

        /// <summary>
        /// Tests that OnDisappearing-like logic can be called multiple times without throwing exceptions.
        /// This verifies idempotency of the event unsubscription logic.
        /// </summary>
        [Test]
        public void OnDisappearing_CalledMultipleTimes_DoesNotThrow()
        {
            // Arrange
            var mockService = new Mock<LocalizationService>();
            var pageSimulator = new PageWithEventUnsubscription(mockService.Object);
            pageSimulator.SubscribeToEvent();

            // Act & Assert
            Assert.DoesNotThrow(() => pageSimulator.SimulateOnDisappearing());
            Assert.DoesNotThrow(() => pageSimulator.SimulateOnDisappearing());
            Assert.DoesNotThrow(() => pageSimulator.SimulateOnDisappearing());
        }

        /// <summary>
        /// Tests that the event handler is properly unsubscribed by verifying
        /// it's no longer invoked after OnDisappearing is called.
        /// </summary>
        [Test]
        public void OnDisappearing_AfterUnsubscribing_EventHandlerIsNotInvoked()
        {
            // Arrange
            var mockService = new Mock<LocalizationService>();
            var pageSimulator = new PageWithEventUnsubscription(mockService.Object);
            var handlerInvokedCount = 0;

            // Override the handler to track invocations
            pageSimulator.OnEventRaised = () => handlerInvokedCount++;

            pageSimulator.SubscribeToEvent();

            // Verify handler works before unsubscribing
            mockService.Raise(m => m.CultureChanged += null, EventArgs.Empty);
            Assert.That(handlerInvokedCount, Is.EqualTo(1), "Handler should be invoked before unsubscribing");

            // Act
            pageSimulator.SimulateOnDisappearing();

            // Raise event again
            mockService.Raise(m => m.CultureChanged += null, EventArgs.Empty);

            // Assert
            Assert.That(handlerInvokedCount, Is.EqualTo(1), "Handler should not be invoked after unsubscribing");
        }

        /// <summary>
        /// Helper class that simulates the event subscription/unsubscription pattern
        /// used in MainPage.OnDisappearing. This is necessary because MainPage cannot
        /// be directly instantiated in unit tests due to MAUI framework dependencies.
        /// </summary>
        private class PageWithEventUnsubscription
        {
            private readonly LocalizationService? _localizationService;

            public Action? OnEventRaised { get; set; }

            public PageWithEventUnsubscription(LocalizationService? localizationService)
            {
                _localizationService = localizationService;
            }

            public void SubscribeToEvent()
            {
                if (_localizationService != null)
                {
                    _localizationService.CultureChanged += OnCultureChanged;
                }
            }

            public void SimulateOnDisappearing()
            {
                // Simulates: base.OnDisappearing(); (cannot test base call directly)

                // Simulates the actual OnDisappearing logic
                if (_localizationService != null)
                {
                    _localizationService.CultureChanged -= OnCultureChanged;
                }
            }

            private void OnCultureChanged(object? sender, EventArgs e)
            {
                OnEventRaised?.Invoke();
            }
        }

        /// <summary>
        /// Tests that the MainPage constructor successfully retrieves LocalizationService from DI,
        /// subscribes to the CultureChanged event, updates the Title with version, and calls InitializeAsync.
        /// </summary>
        /// <remarks>
        /// Expected behavior:
        /// 1. Retrieves LocalizationService from Application.Current.Handler.MauiContext.Services
        /// 2. If service is not null, subscribes to CultureChanged event with OnCultureChanged handler
        /// 3. Appends " " + Common.Version to the Title property
        /// 4. Calls InitializeAsync() without awaiting (fire-and-forget pattern)
        /// 
        /// This test cannot run in isolation because:
        /// - Application.Current is a static property that requires MAUI application initialization
        /// - InitializeComponent() is an auto-generated method requiring XAML compilation
        /// - Common.Version is a static property that cannot be mocked
        /// - ContentPage base class requires MAUI infrastructure
        /// 
        /// Recommendation: Implement this as an integration test or refactor constructor to accept
        /// IServiceProvider as a constructor parameter.
        /// </remarks>
        [Test]
        [Ignore("Cannot test constructor in isolation due to dependencies on static Application.Current, " +
                "auto-generated InitializeComponent(), and static Common.Version. " +
                "Requires integration test with MAUI test host or constructor refactoring.")]
        public void Constructor_WhenLocalizationServiceAvailable_InitializesCorrectly()
        {
            // Arrange
            // Cannot arrange - Application.Current.Handler.MauiContext.Services is static and cannot be mocked
            // Cannot mock InitializeComponent() as it's auto-generated and not virtual
            // Cannot mock Common.Version as it's a static property

            // Act
            // var mainPage = new MainPage();
            // This would throw NullReferenceException in unit test context

            // Assert
            // Would verify:
            // 1. _localizationService is not null
            // 2. CultureChanged event has OnCultureChanged handler attached
            // 3. Title contains Common.Version
            // 4. InitializeAsync was called

            Assert.Inconclusive("This test requires MAUI application context with proper DI setup. " +
                "Consider refactoring constructor to accept IServiceProvider parameter for testability.");
        }

        /// <summary>
        /// Tests that the MainPage constructor handles the case when LocalizationService is not registered in DI
        /// and returns null from GetService.
        /// </summary>
        /// <remarks>
        /// Expected behavior:
        /// 1. GetService returns null for LocalizationService
        /// 2. _localizationService field is set to null
        /// 3. CultureChanged event subscription is skipped (due to null check)
        /// 4. Title is still updated with version
        /// 5. InitializeAsync is still called
        /// 
        /// This test cannot run in isolation for the same reasons as Constructor_WhenLocalizationServiceAvailable_InitializesCorrectly.
        /// </remarks>
        [Test]
        [Ignore("Cannot test constructor in isolation due to dependencies on static Application.Current, " +
                "auto-generated InitializeComponent(), and static Common.Version. " +
                "Requires integration test with MAUI test host or constructor refactoring.")]
        public void Constructor_WhenLocalizationServiceNotAvailable_HandlesNullGracefully()
        {
            // Arrange
            // Cannot arrange - would need to mock IServiceProvider to return null for LocalizationService

            // Act
            // var mainPage = new MainPage();

            // Assert
            // Would verify:
            // 1. _localizationService is null
            // 2. CultureChanged event handler is NOT attached (null check prevents subscription)
            // 3. Title still contains Common.Version
            // 4. InitializeAsync was called
            // 5. No exception thrown despite null service

            Assert.Inconclusive("This test requires MAUI application context with controlled DI setup. " +
                "Consider refactoring constructor to accept IServiceProvider parameter for testability.");
        }

        /// <summary>
        /// Tests that the MainPage constructor throws NullReferenceException when Application.Current is null.
        /// </summary>
        /// <remarks>
        /// Expected behavior:
        /// When Application.Current is null at line 15, accessing Application.Current.Handler throws NullReferenceException.
        /// 
        /// This represents a critical failure scenario where the MAUI application has not been properly initialized.
        /// In production, this should never happen as MainPage is only created after Application.Current is set.
        /// 
        /// This test cannot run because we cannot control the Application.Current static property in unit tests.
        /// </remarks>
        [Test]
        [Ignore("Cannot test Application.Current null scenario as it's a static property that cannot be set in unit tests. " +
                "This scenario represents a critical application initialization failure that should be prevented " +
                "by proper application lifecycle management.")]
        public void Constructor_WhenApplicationCurrentIsNull_ThrowsNullReferenceException()
        {
            // Arrange
            // Cannot set Application.Current to null in unit test context

            // Act & Assert
            // Would expect NullReferenceException when accessing Application.Current.Handler.MauiContext.Services

            Assert.Inconclusive("Cannot control Application.Current static property. " +
                "This failure scenario is prevented by MAUI framework initialization guarantees.");
        }

        /// <summary>
        /// Tests that the MainPage constructor appends the version string to the Title property.
        /// </summary>
        /// <remarks>
        /// Expected behavior at line 33:
        /// Title += " " + Common.Version;
        /// 
        /// This concatenates the existing Title value (from XAML or default) with a space and the Common.Version string.
        /// 
        /// Cannot test because:
        /// - Common.Version is a static property that cannot be mocked
        /// - Title property requires ContentPage initialization
        /// - InitializeComponent() must be called first to set initial Title from XAML
        /// </remarks>
        [Test]
        [Ignore("Cannot test Title update in isolation due to dependency on static Common.Version and " +
                "requirement for ContentPage/XAML initialization.")]
        public void Constructor_UpdatesTitle_WithVersionString()
        {
            // Arrange
            // Cannot mock Common.Version (static property)
            // Cannot initialize ContentPage without MAUI infrastructure

            // Act
            // var mainPage = new MainPage();

            // Assert
            // Would verify: mainPage.Title.Contains(Common.Version)
            // Would verify: mainPage.Title format is correct (original title + " " + version)

            Assert.Inconclusive("Title update depends on static Common.Version and ContentPage initialization. " +
                "Consider extracting version retrieval to a testable service.");
        }

        /// <summary>
        /// Tests that the MainPage constructor subscribes to the CultureChanged event when LocalizationService is available.
        /// </summary>
        /// <remarks>
        /// Expected behavior at lines 27-30:
        /// if (_localizationService != null)
        /// {
        ///     _localizationService.CultureChanged += OnCultureChanged;
        /// }
        /// 
        /// Cannot test because we cannot mock the LocalizationService retrieval from static DI container.
        /// </remarks>
        [Test]
        [Ignore("Cannot test event subscription in isolation due to dependency on static DI container. " +
                "Event subscription testing requires integration test or constructor refactoring.")]
        public void Constructor_WhenLocalizationServiceExists_SubscribesToCultureChangedEvent()
        {
            // Arrange
            // Cannot arrange - would need to mock IServiceProvider and LocalizationService

            // Act
            // var mainPage = new MainPage();

            // Assert
            // Would verify CultureChanged event has OnCultureChanged handler
            // Could verify by raising the event and checking if OnCultureChanged was called

            Assert.Inconclusive("Event subscription testing requires MAUI context with mocked LocalizationService. " +
                "Consider refactoring to inject LocalizationService via constructor parameter.");
        }

        /// <summary>
        /// Tests that the MainPage constructor does not subscribe to CultureChanged event when LocalizationService is null.
        /// </summary>
        /// <remarks>
        /// Expected behavior:
        /// When GetService returns null, the null check at line 27 prevents event subscription.
        /// No exception should be thrown and the constructor should complete successfully.
        /// 
        /// Cannot test due to inability to control IServiceProvider behavior in static DI lookup.
        /// </remarks>
        [Test]
        [Ignore("Cannot test null LocalizationService scenario in isolation due to static DI dependencies.")]
        public void Constructor_WhenLocalizationServiceIsNull_DoesNotSubscribeToEvent()
        {
            // Arrange
            // Cannot arrange - would need IServiceProvider to return null

            // Act
            // var mainPage = new MainPage();

            // Assert
            // Would verify no exception thrown
            // Would verify CultureChanged event has no handlers attached

            Assert.Inconclusive("Null service handling requires controlled DI setup not available in unit tests.");
        }

        /// <summary>
        /// Tests that the MainPage constructor calls InitializeAsync without awaiting (fire-and-forget pattern).
        /// </summary>
        /// <remarks>
        /// Expected behavior at line 36:
        /// _ = InitializeAsync();
        /// 
        /// The discard operator (_) is used to suppress the compiler warning about not awaiting the Task.
        /// This is intentional fire-and-forget behavior to avoid blocking the UI thread during page construction.
        /// 
        /// Cannot test because:
        /// - InitializeAsync is a private method
        /// - Cannot verify async method invocation in constructor without integration testing
        /// - Would require hooking into Task scheduling or using reflection
        /// </remarks>
        [Test]
        [Ignore("Cannot verify fire-and-forget async method call in constructor without integration testing or reflection. " +
                "Consider making InitializeAsync internal/protected for testing or using integration tests.")]
        public void Constructor_CallsInitializeAsync_WithoutAwaiting()
        {
            // Arrange
            // Cannot arrange

            // Act
            // var mainPage = new MainPage();

            // Assert
            // Would need to verify InitializeAsync was called but not awaited
            // This would require monitoring Task creation or using reflection

            Assert.Inconclusive("Verifying fire-and-forget async call requires reflection or integration testing. " +
                "Consider exposing initialization state for testing or using integration tests.");
        }

        /// <summary>
        /// Tests that OnSizeAllocated handles various edge case values for width and height parameters.
        /// This parameterized test covers normal values, boundaries, and special double values (NaN, Infinity).
        /// Expected behavior: The method should call base.OnSizeAllocated without throwing exceptions for all input values.
        /// </summary>
        /// <param name="width">The width value to test</param>
        /// <param name="height">The height value to test</param>
        /// <param name="testDescription">Description of what this test case validates</param>
        [Test]
        [TestCase(100.0, 200.0, "Normal positive values")]
        [TestCase(0.0, 0.0, "Zero values (boundary case - no allocated size)")]
        [TestCase(-50.0, -100.0, "Negative values (invalid but handled gracefully)")]
        [TestCase(double.MaxValue, double.MaxValue, "Maximum double values (upper boundary)")]
        [TestCase(double.MinValue, double.MinValue, "Minimum double values (large negative boundary)")]
        [TestCase(double.NaN, double.NaN, "NaN values (undefined/unrepresentable)")]
        [TestCase(double.PositiveInfinity, double.PositiveInfinity, "Positive infinity (unbounded size)")]
        [TestCase(double.NegativeInfinity, double.NegativeInfinity, "Negative infinity (negative unbounded)")]
        [TestCase(100.0, double.NaN, "Mixed values (normal width, NaN height)")]
        [TestCase(double.PositiveInfinity, 0.0, "Mixed values (infinite width, zero height)")]
        [Ignore("Requires MAUI application infrastructure (Application.Current, MauiContext, XAML compilation). " +
                "MainPage constructor depends on Application.Current.Handler.MauiContext.Services for LocalizationService retrieval " +
                "and calls InitializeComponent() which requires compiled XAML resources. " +
                "Convert to integration test with MAUI test host or refactor constructor to accept IServiceProvider as parameter.")]
        public void OnSizeAllocated_WithVariousDoubleValues_CallsBaseWithoutException(double width, double height, string testDescription)
        {
            // Arrange
            // Cannot instantiate MainPage due to:
            // 1. Constructor accesses static Application.Current which is null in unit tests
            // 2. InitializeComponent() requires XAML compilation and MAUI infrastructure
            // 3. LocalizationService retrieval from DI container requires MauiContext
            // var mainPage = new MainPage();

            // Act
            // Would call: mainPage.OnSizeAllocated(width, height);
            // Expected: Calls base.OnSizeAllocated(width, height) at line 60

            // Assert
            // Would verify: Assert.DoesNotThrow(() => mainPage.OnSizeAllocated(width, height));
            // Expected: No exception thrown regardless of input values
            // Expected: Base ContentPage.OnSizeAllocated is invoked with the provided parameters

            Assert.Inconclusive($"Test case '{testDescription}': Cannot test OnSizeAllocated in isolation. " +
                              $"Requires MAUI application context with initialized Application.Current, " +
                              $"compiled XAML resources, and registered services. " +
                              $"Consider implementing as integration test with MAUI test host.");
        }

        /// <summary>
        /// Tests that OnSizeAllocated handles extreme combinations of special double values.
        /// These edge cases test the robustness of the method with unusual but valid double combinations.
        /// Expected behavior: The method should handle all combinations without throwing exceptions.
        /// </summary>
        /// <param name="width">The width value to test</param>
        /// <param name="height">The height value to test</param>
        /// <param name="testDescription">Description of what this test case validates</param>
        [Test]
        [TestCase(double.NaN, double.PositiveInfinity, "NaN width with infinite height")]
        [TestCase(double.NegativeInfinity, double.MaxValue, "Negative infinite width with max height")]
        [TestCase(double.Epsilon, double.Epsilon, "Smallest positive values (double.Epsilon)")]
        [TestCase(-double.Epsilon, -double.Epsilon, "Smallest negative values (-double.Epsilon)")]
        [TestCase(1.7976931348623157E+308, -1.7976931348623157E+308, "Near-max positive and near-min negative")]
        [Ignore("Requires MAUI application infrastructure. See OnSizeAllocated_WithVariousDoubleValues_CallsBaseWithoutException for details.")]
        public void OnSizeAllocated_WithExtremeValueCombinations_CallsBaseWithoutException(double width, double height, string testDescription)
        {
            // Arrange
            // Cannot instantiate MainPage - see OnSizeAllocated_WithVariousDoubleValues_CallsBaseWithoutException

            // Act
            // Would call: mainPage.OnSizeAllocated(width, height);

            // Assert
            // Would verify: Assert.DoesNotThrow(() => mainPage.OnSizeAllocated(width, height));

            Assert.Inconclusive($"Test case '{testDescription}': Requires MAUI infrastructure. " +
                              $"Width={width}, Height={height}");
        }
    }
}