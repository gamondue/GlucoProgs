using GlucoMan;
using GlucoMan.Maui;
using Microsoft.Maui.Controls;
using NUnit.Framework;

namespace GlucoMan.Maui.UnitTests;



/// <summary>
/// Unit tests for the <see cref="AlarmDialogPage"/> class.
/// </summary>
[TestFixture]
public partial class AlarmDialogPageTests
{
    /// <summary>
    /// Tests that the AlarmDialogPage constructor successfully creates an instance
    /// without throwing exceptions when XAML resources are available.
    /// </summary>
    /// <remarks>
    /// Note: This test depends on XAML resources being available through InitializeComponent(),
    /// which is auto-generated code that cannot be mocked. If this test fails due to missing
    /// XAML resources in the test context, it may need to be converted to an integration test
    /// or marked as inconclusive.
    /// </remarks>
    [Test]
    public void AlarmDialogPage_Constructor_CreatesInstanceSuccessfully()
    {
        // Arrange & Act
        AlarmDialogPage? page = null;
        TestDelegate constructorCall = () => page = new AlarmDialogPage();

        // Assert
        Assert.DoesNotThrow(constructorCall, "Constructor should not throw exceptions");
        Assert.That(page, Is.Not.Null, "Created instance should not be null");
        Assert.That(page, Is.InstanceOf<ContentPage>(), "Instance should be a ContentPage");
        Assert.That(page, Is.InstanceOf<AlarmDialogPage>(), "Instance should be an AlarmDialogPage");
    }

    /// <summary>
    /// Tests that multiple instances of AlarmDialogPage can be created independently
    /// without interference.
    /// </summary>
    [Test]
    public void AlarmDialogPage_Constructor_CreatesMultipleIndependentInstances()
    {
        // Arrange & Act
        var page1 = new AlarmDialogPage();
        var page2 = new AlarmDialogPage();

        // Assert
        Assert.That(page1, Is.Not.Null, "First instance should not be null");
        Assert.That(page2, Is.Not.Null, "Second instance should not be null");
        Assert.That(page1, Is.Not.SameAs(page2), "Instances should be different objects");
    }
}