using GlucoMan.Maui;
using Microsoft.Maui.Controls;
using NUnit.Framework;


namespace GlucoMan.Maui.UnitTests;

/// <summary>
/// Unit tests for the <see cref="ConfigurationPage"/> class.
/// </summary>
public partial class ConfigurationPageTests
{
    /// <summary>
    /// Tests that the ConfigurationPage constructor successfully creates an instance.
    /// This test verifies that the parameterless constructor completes without throwing
    /// and returns a valid ConfigurationPage object that inherits from ContentPage.
    /// Note: This test requires MAUI infrastructure to be initialized as it calls
    /// the generated InitializeComponent() method which loads XAML resources.
    /// </summary>
    [Test]
    public void ConfigurationPage_Constructor_CreatesValidInstance()
    {
        // Arrange & Act
        ConfigurationPage? page = null;
        TestDelegate act = () => page = new ConfigurationPage();

        // Assert
        Assert.DoesNotThrow(act, "Constructor should not throw an exception");
        Assert.That(page, Is.Not.Null, "Constructor should create a non-null instance");
        Assert.That(page, Is.InstanceOf<ContentPage>(), "ConfigurationPage should inherit from ContentPage");
    }

    /// <summary>
    /// Tests that the ConfigurationPage constructor properly initializes the page
    /// as a ContentPage with expected base properties accessible.
    /// This verifies that the XAML initialization completes successfully.
    /// </summary>
    [Test]
    public void ConfigurationPage_Constructor_InitializesAsContentPage()
    {
        // Arrange & Act
        var page = new ConfigurationPage();

        // Assert
        Assert.That(page, Is.Not.Null, "Page should be created successfully");
        Assert.That(page.GetType().Name, Is.EqualTo("ConfigurationPage"), "Type name should be ConfigurationPage");
        Assert.That(page.GetType().BaseType, Is.EqualTo(typeof(ContentPage)), "Should directly inherit from ContentPage");
    }
}