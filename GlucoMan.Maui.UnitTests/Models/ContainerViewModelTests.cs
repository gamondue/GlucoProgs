using System;
using System.ComponentModel;

using gamon;
using GlucoMan;
using GlucoMan.Maui.Models;
using Microsoft.Maui.Controls;
using Moq;
using NUnit.Framework;

namespace GlucoMan.Maui.Models.UnitTests;


/// <summary>
/// Unit tests for the <see cref="ContainerViewModel"/> class.
/// </summary>
public partial class ContainerViewModelTests
{
    /// <summary>
    /// Tests that the Name property returns an empty string when Container is null.
    /// </summary>
    [Test]
    public void Name_ContainerIsNull_ReturnsEmptyString()
    {
        // Arrange
        var viewModel = new ContainerViewModel(new Container());
        viewModel.Container = null!;

        // Act
        var result = viewModel.Name;

        // Assert
        Assert.That(result, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that the Name property returns an empty string when Container.Name is null.
    /// </summary>
    [Test]
    public void Name_ContainerNameIsNull_ReturnsEmptyString()
    {
        // Arrange
        var container = new Container();
        container.Name = null!;
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Name;

        // Assert
        Assert.That(result, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that the Name property returns an empty string when Container.Name is an empty string.
    /// </summary>
    [Test]
    public void Name_ContainerNameIsEmptyString_ReturnsEmptyString()
    {
        // Arrange
        var container = new Container();
        container.Name = "";
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Name;

        // Assert
        Assert.That(result, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that the Name property returns the value when Container.Name is whitespace.
    /// Input: Whitespace-only string.
    /// Expected: Returns the whitespace string.
    /// </summary>
    [TestCase("   ")]
    [TestCase("\t")]
    [TestCase("\n")]
    [TestCase("\r\n")]
    [TestCase(" \t \n ")]
    public void Name_ContainerNameIsWhitespace_ReturnsWhitespace(string whitespace)
    {
        // Arrange
        var container = new Container();
        container.Name = whitespace;
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Name;

        // Assert
        Assert.That(result, Is.EqualTo(whitespace));
    }

    /// <summary>
    /// Tests that the Name property returns the correct value when Container.Name has a normal value.
    /// Input: Various normal string values.
    /// Expected: Returns the same string value.
    /// </summary>
    [TestCase("Plate")]
    [TestCase("Large Bowl")]
    [TestCase("Small Container")]
    [TestCase("My Favorite Pot")]
    [TestCase("A")]
    public void Name_ContainerNameHasNormalValue_ReturnsValue(string name)
    {
        // Arrange
        var container = new Container();
        container.Name = name;
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Name;

        // Assert
        Assert.That(result, Is.EqualTo(name));
    }

    /// <summary>
    /// Tests that the Name property returns the correct value when Container.Name contains special characters.
    /// Input: Strings with special characters, Unicode, control characters.
    /// Expected: Returns the same string value.
    /// </summary>
    [TestCase("Container #1")]
    [TestCase("Bowl @ Home")]
    [TestCase("Plate & Fork")]
    [TestCase("Container's Name")]
    [TestCase("\"Quoted\" Name")]
    [TestCase("Name/With/Slashes")]
    [TestCase("Name\\With\\Backslashes")]
    [TestCase("Name<With>Brackets")]
    [TestCase("Émilie's Bowl")]
    [TestCase("日本語")]
    [TestCase("🍽️ Plate")]
    [TestCase("Tab\tIn\tName")]
    public void Name_ContainerNameHasSpecialCharacters_ReturnsValue(string name)
    {
        // Arrange
        var container = new Container();
        container.Name = name;
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Name;

        // Assert
        Assert.That(result, Is.EqualTo(name));
    }

    /// <summary>
    /// Tests that the Name property returns the correct value when Container.Name is a very long string.
    /// Input: Very long string (1000+ characters).
    /// Expected: Returns the same long string value.
    /// </summary>
    [Test]
    public void Name_ContainerNameIsVeryLong_ReturnsValue()
    {
        // Arrange
        var longName = new string('A', 10000);
        var container = new Container();
        container.Name = longName;
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Name;

        // Assert
        Assert.That(result, Is.EqualTo(longName));
    }

    /// <summary>
    /// Tests that the Name property uses the constructor-provided value correctly.
    /// Input: Container initialized via parameterized constructor.
    /// Expected: Returns the name provided to the constructor.
    /// </summary>
    [Test]
    public void Name_ContainerInitializedViaConstructor_ReturnsConstructorValue()
    {
        // Arrange
        var container = new Container("Test Container", 100.0);
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Name;

        // Assert
        Assert.That(result, Is.EqualTo("Test Container"));
    }

    /// <summary>
    /// Tests that the Name property reflects changes when Container.Name is modified.
    /// Input: Container.Name is changed after ContainerViewModel creation.
    /// Expected: Returns the updated name value.
    /// </summary>
    [Test]
    public void Name_ContainerNameIsModifiedAfterCreation_ReturnsUpdatedValue()
    {
        // Arrange
        var container = new Container();
        container.Name = "Initial Name";
        var viewModel = new ContainerViewModel(container);

        // Act - Modify the container name
        container.Name = "Updated Name";
        var result = viewModel.Name;

        // Assert
        Assert.That(result, Is.EqualTo("Updated Name"));
    }

    /// <summary>
    /// Tests that the Name property returns empty string when Container is replaced with null.
    /// Input: Container is set to null after initialization.
    /// Expected: Returns empty string.
    /// </summary>
    [Test]
    public void Name_ContainerReplacedWithNull_ReturnsEmptyString()
    {
        // Arrange
        var container = new Container("Initial Container", 50.0);
        var viewModel = new ContainerViewModel(container);

        // Act - Replace container with null
        viewModel.Container = null!;
        var result = viewModel.Name;

        // Assert
        Assert.That(result, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that PhotoFileName returns an empty string when Container is null.
    /// </summary>
    [Test]
    public void PhotoFileName_ContainerIsNull_ReturnsEmptyString()
    {
        // Arrange
        var viewModel = new ContainerViewModel(null!);
        viewModel.Container = null!;

        // Act
        var result = viewModel.PhotoFileName;

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that PhotoFileName returns the expected value for various PhotoFileName states.
    /// </summary>
    /// <param name="photoFileName">The photo file name to set on the container.</param>
    /// <param name="expected">The expected return value.</param>
    [TestCase(null, "", TestName = "PhotoFileName_ContainerPhotoFileNameIsNull_ReturnsEmptyString")]
    [TestCase("", "", TestName = "PhotoFileName_ContainerPhotoFileNameIsEmpty_ReturnsEmptyString")]
    [TestCase("photo.jpg", "photo.jpg", TestName = "PhotoFileName_ContainerPhotoFileNameHasValue_ReturnsValue")]
    [TestCase("container_123.png", "container_123.png", TestName = "PhotoFileName_ContainerPhotoFileNameHasValidFileName_ReturnsValue")]
    [TestCase("path/to/photo.jpg", "path/to/photo.jpg", TestName = "PhotoFileName_ContainerPhotoFileNameHasPathSeparators_ReturnsValue")]
    [TestCase("photo with spaces.jpg", "photo with spaces.jpg", TestName = "PhotoFileName_ContainerPhotoFileNameHasSpaces_ReturnsValue")]
    [TestCase(" ", " ", TestName = "PhotoFileName_ContainerPhotoFileNameIsWhitespace_ReturnsWhitespace")]
    [TestCase("  \t\n  ", "  \t\n  ", TestName = "PhotoFileName_ContainerPhotoFileNameIsWhitespaceWithTabs_ReturnsWhitespace")]
    [TestCase("photo_!@#$%^&()_+.jpg", "photo_!@#$%^&()_+.jpg", TestName = "PhotoFileName_ContainerPhotoFileNameHasSpecialCharacters_ReturnsValue")]
    [TestCase("очень_длинное_имя_файла_с_unicode_символами_и_специальными_знаками_1234567890.jpg", "очень_длинное_имя_файла_с_unicode_символами_и_специальными_знаками_1234567890.jpg", TestName = "PhotoFileName_ContainerPhotoFileNameHasUnicodeCharacters_ReturnsValue")]
    public void PhotoFileName_VariousPhotoFileNameValues_ReturnsExpectedValue(string? photoFileName, string expected)
    {
        // Arrange
        var container = new Container
        {
            PhotoFileName = photoFileName!
        };
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.PhotoFileName;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    /// <summary>
    /// Tests that PhotoFileName returns an empty string when Container.PhotoFileName is set to null after initialization.
    /// </summary>
    [Test]
    public void PhotoFileName_ContainerPhotoFileNameSetToNullAfterInitialization_ReturnsEmptyString()
    {
        // Arrange
        var container = new Container
        {
            PhotoFileName = "initial.jpg"
        };
        var viewModel = new ContainerViewModel(container);
        container.PhotoFileName = null!;

        // Act
        var result = viewModel.PhotoFileName;

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that PhotoFileName returns the updated value when Container.PhotoFileName is changed.
    /// </summary>
    [Test]
    public void PhotoFileName_ContainerPhotoFileNameChanged_ReturnsUpdatedValue()
    {
        // Arrange
        var container = new Container
        {
            PhotoFileName = "initial.jpg"
        };
        var viewModel = new ContainerViewModel(container);
        container.PhotoFileName = "updated.png";

        // Act
        var result = viewModel.PhotoFileName;

        // Assert
        Assert.That(result, Is.EqualTo("updated.png"));
    }

    /// <summary>
    /// Tests that PhotoFileName returns an empty string when Container is replaced with null.
    /// </summary>
    [Test]
    public void PhotoFileName_ContainerReplacedWithNull_ReturnsEmptyString()
    {
        // Arrange
        var container = new Container
        {
            PhotoFileName = "photo.jpg"
        };
        var viewModel = new ContainerViewModel(container);
        viewModel.Container = null!;

        // Act
        var result = viewModel.PhotoFileName;

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that PhotoFileName returns the new value when Container is replaced with a different container.
    /// </summary>
    [Test]
    public void PhotoFileName_ContainerReplacedWithDifferentContainer_ReturnsNewValue()
    {
        // Arrange
        var container1 = new Container
        {
            PhotoFileName = "photo1.jpg"
        };
        var container2 = new Container
        {
            PhotoFileName = "photo2.png"
        };
        var viewModel = new ContainerViewModel(container1);
        viewModel.Container = container2;

        // Act
        var result = viewModel.PhotoFileName;

        // Assert
        Assert.That(result, Is.EqualTo("photo2.png"));
    }

    /// <summary>
    /// Tests that PhotoFileName handles extremely long file names correctly.
    /// </summary>
    [Test]
    public void PhotoFileName_ContainerPhotoFileNameIsVeryLong_ReturnsLongValue()
    {
        // Arrange
        var longFileName = new string('a', 1000) + ".jpg";
        var container = new Container
        {
            PhotoFileName = longFileName
        };
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.PhotoFileName;

        // Assert
        Assert.That(result, Is.EqualTo(longFileName));
        Assert.That(result.Length, Is.EqualTo(1004));
    }

    /// <summary>
    /// Tests that the Weight property returns null when the Container is null.
    /// Input: Container is null.
    /// Expected: Weight returns null.
    /// </summary>
    [Test]
    public void Weight_WhenContainerIsNull_ReturnsNull()
    {
        // Arrange
        var viewModel = new ContainerViewModel(new Container());
        viewModel.Container = null!;

        // Act
        var result = viewModel.Weight;

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests that the Weight property returns the Container's Weight when Container is not null.
    /// Input: Container with default Weight (new DoubleAndText()).
    /// Expected: Weight returns the DoubleAndText instance from Container.
    /// </summary>
    [Test]
    public void Weight_WhenContainerHasDefaultWeight_ReturnsContainerWeight()
    {
        // Arrange
        var container = new Container();
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Weight;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.SameAs(container.Weight));
    }

    /// <summary>
    /// Tests that the Weight property returns the Container's Weight with a specific value.
    /// Input: Container with Weight set to a specific double value.
    /// Expected: Weight returns the DoubleAndText instance with the correct value.
    /// </summary>
    [Test]
    public void Weight_WhenContainerHasSpecificWeightValue_ReturnsCorrectWeight()
    {
        // Arrange
        var container = new Container("Test Container", 150.5);
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Weight;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Double, Is.EqualTo(150.5));
        Assert.That(result, Is.SameAs(container.Weight));
    }

    /// <summary>
    /// Tests that the Weight property returns null when Container's Weight is explicitly set to null.
    /// Input: Container with Weight property explicitly set to null.
    /// Expected: Weight returns null.
    /// </summary>
    [Test]
    public void Weight_WhenContainerWeightIsNull_ReturnsNull()
    {
        // Arrange
        var container = new Container();
        container.Weight = null!;
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Weight;

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests that the Weight property returns updated values when Container's Weight changes.
    /// Input: Container with initial Weight, then Weight is changed to a different value.
    /// Expected: Weight property reflects the updated Container.Weight value.
    /// </summary>
    [Test]
    public void Weight_WhenContainerWeightChanges_ReturnsUpdatedWeight()
    {
        // Arrange
        var container = new Container("Test", 100.0);
        var viewModel = new ContainerViewModel(container);
        var initialWeight = viewModel.Weight;
        var newWeight = new DoubleAndText { Double = 200.0 };

        // Act
        container.Weight = newWeight;
        var result = viewModel.Weight;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.SameAs(newWeight));
        Assert.That(result.Double, Is.EqualTo(200.0));
        Assert.That(result, Is.Not.SameAs(initialWeight));
    }

    /// <summary>
    /// Tests that the Notes property returns an empty string when the Container is null.
    /// </summary>
    [Test]
    public void Notes_ContainerIsNull_ReturnsEmptyString()
    {
        // Arrange
        var viewModel = new ContainerViewModel(null!);
        viewModel.Container = null!;

        // Act
        var result = viewModel.Notes;

        // Assert
        Assert.That(result, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that the Notes property returns an empty string when Container.Notes is null.
    /// Input: Container with Notes set to null.
    /// Expected: Empty string.
    /// </summary>
    [Test]
    public void Notes_ContainerNotesIsNull_ReturnsEmptyString()
    {
        // Arrange
        var container = new Container
        {
            Notes = null!
        };
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Notes;

        // Assert
        Assert.That(result, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that the Notes property returns an empty string when Container.Notes is already empty.
    /// Input: Container with Notes set to empty string.
    /// Expected: Empty string.
    /// </summary>
    [Test]
    public void Notes_ContainerNotesIsEmpty_ReturnsEmptyString()
    {
        // Arrange
        var container = new Container
        {
            Notes = ""
        };
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Notes;

        // Assert
        Assert.That(result, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that the Notes property returns whitespace when Container.Notes contains only whitespace.
    /// Input: Container with Notes set to whitespace string.
    /// Expected: The whitespace string is returned as-is.
    /// </summary>
    [TestCase("   ")]
    [TestCase("\t")]
    [TestCase("\n")]
    [TestCase("\r\n")]
    [TestCase("  \t  \n  ")]
    public void Notes_ContainerNotesIsWhitespace_ReturnsWhitespace(string whitespaceNotes)
    {
        // Arrange
        var container = new Container
        {
            Notes = whitespaceNotes
        };
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Notes;

        // Assert
        Assert.That(result, Is.EqualTo(whitespaceNotes));
    }

    /// <summary>
    /// Tests that the Notes property returns the correct value when Container.Notes contains normal text.
    /// Input: Container with Notes set to various valid text values.
    /// Expected: The exact Notes value is returned.
    /// </summary>
    [TestCase("Simple note")]
    [TestCase("A longer note with more details about the container")]
    [TestCase("Note with numbers 123456")]
    [TestCase("Single")]
    public void Notes_ContainerNotesHasNormalText_ReturnsNotes(string notes)
    {
        // Arrange
        var container = new Container
        {
            Notes = notes
        };
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Notes;

        // Assert
        Assert.That(result, Is.EqualTo(notes));
    }

    /// <summary>
    /// Tests that the Notes property correctly handles special characters in Container.Notes.
    /// Input: Container with Notes containing special characters.
    /// Expected: The exact Notes value with special characters is returned.
    /// </summary>
    [TestCase("Notes with !@#$%^&*()")]
    [TestCase("Notes with quotes: \"Hello\"")]
    [TestCase("Notes with apostrophe's")]
    [TestCase("Notes with unicode: ñ, ü, é, 中文")]
    [TestCase("Notes with symbols: €, £, ¥")]
    [TestCase("<xml>tags</xml>")]
    [TestCase("Line1\nLine2\rLine3")]
    [TestCase("Tab\tseparated")]
    public void Notes_ContainerNotesHasSpecialCharacters_ReturnsNotesWithSpecialCharacters(string notesWithSpecialChars)
    {
        // Arrange
        var container = new Container
        {
            Notes = notesWithSpecialChars
        };
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Notes;

        // Assert
        Assert.That(result, Is.EqualTo(notesWithSpecialChars));
    }

    /// <summary>
    /// Tests that the Notes property correctly handles very long strings in Container.Notes.
    /// Input: Container with Notes set to a very long string (10000 characters).
    /// Expected: The entire long string is returned correctly.
    /// </summary>
    [Test]
    public void Notes_ContainerNotesIsVeryLong_ReturnsLongNotes()
    {
        // Arrange
        var longNotes = new string('A', 10000);
        var container = new Container
        {
            Notes = longNotes
        };
        var viewModel = new ContainerViewModel(container);

        // Act
        var result = viewModel.Notes;

        // Assert
        Assert.That(result, Is.EqualTo(longNotes));
        Assert.That(result.Length, Is.EqualTo(10000));
    }

    /// <summary>
    /// Tests that the Notes property returns updated value when Container.Notes is changed.
    /// Input: Container with Notes initially set, then changed.
    /// Expected: The Notes property reflects the updated value.
    /// </summary>
    [Test]
    public void Notes_ContainerNotesChanged_ReturnsUpdatedNotes()
    {
        // Arrange
        var container = new Container
        {
            Notes = "Original notes"
        };
        var viewModel = new ContainerViewModel(container);

        // Act
        var initialNotes = viewModel.Notes;
        container.Notes = "Updated notes";
        var updatedNotes = viewModel.Notes;

        // Assert
        Assert.That(initialNotes, Is.EqualTo("Original notes"));
        Assert.That(updatedNotes, Is.EqualTo("Updated notes"));
    }

    /// <summary>
    /// Tests that the Notes property returns empty string when Container reference is changed to null.
    /// Input: Container initially set, then changed to null.
    /// Expected: Notes property returns empty string after Container is set to null.
    /// </summary>
    [Test]
    public void Notes_ContainerChangedToNull_ReturnsEmptyString()
    {
        // Arrange
        var container = new Container
        {
            Notes = "Some notes"
        };
        var viewModel = new ContainerViewModel(container);

        // Act
        var initialNotes = viewModel.Notes;
        viewModel.Container = null!;
        var notesAfterNull = viewModel.Notes;

        // Assert
        Assert.That(initialNotes, Is.EqualTo("Some notes"));
        Assert.That(notesAfterNull, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that the constructor correctly assigns a valid Container instance to the Container property.
    /// Input: Valid Container instance.
    /// Expected: Container property is set to the passed instance.
    /// </summary>
    [Test]
    public void Constructor_WithValidContainer_SetsContainerProperty()
    {
        // Arrange
        var container = new Container("Test Container", 100.0);

        // Act
        var viewModel = new ContainerViewModel(container);

        // Assert
        Assert.That(viewModel.Container, Is.Not.Null);
        Assert.That(viewModel.Container, Is.SameAs(container));
    }

    /// <summary>
    /// Tests that the constructor preserves the exact reference of the Container instance (reference equality).
    /// Input: Valid Container instance.
    /// Expected: Container property references the exact same instance.
    /// </summary>
    [Test]
    public void Constructor_WithValidContainer_PreservesContainerReference()
    {
        // Arrange
        var container = new Container
        {
            IdContainer = 42,
            Name = "Sample Container",
            Weight = new DoubleAndText { Double = 250.5 },
            Notes = "Test notes",
            PhotoFileName = "test.jpg"
        };

        // Act
        var viewModel = new ContainerViewModel(container);

        // Assert
        Assert.That(viewModel.Container, Is.SameAs(container));
        Assert.That(viewModel.IdContainer, Is.EqualTo(42));
        Assert.That(viewModel.Name, Is.EqualTo("Sample Container"));
        Assert.That(viewModel.Weight?.Double, Is.EqualTo(250.5));
        Assert.That(viewModel.Notes, Is.EqualTo("Test notes"));
        Assert.That(viewModel.PhotoFileName, Is.EqualTo("test.jpg"));
    }

    /// <summary>
    /// Tests that the constructor accepts a Container with default/empty values.
    /// Input: Container with default constructor (empty values).
    /// Expected: Container property is set and derived properties return default/empty values.
    /// </summary>
    [Test]
    public void Constructor_WithDefaultContainer_SetsContainerProperty()
    {
        // Arrange
        var container = new Container();

        // Act
        var viewModel = new ContainerViewModel(container);

        // Assert
        Assert.That(viewModel.Container, Is.SameAs(container));
        Assert.That(viewModel.IdContainer, Is.Null);
        Assert.That(viewModel.Name, Is.EqualTo(string.Empty));
        Assert.That(viewModel.Notes, Is.EqualTo(string.Empty));
        Assert.That(viewModel.PhotoFileName, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that the constructor accepts a Container with null IdContainer.
    /// Input: Container with null IdContainer.
    /// Expected: Container property is set and IdContainer returns null.
    /// </summary>
    [Test]
    public void Constructor_WithContainerHavingNullId_SetsContainerProperty()
    {
        // Arrange
        var container = new Container("Container without ID", 50.0)
        {
            IdContainer = null
        };

        // Act
        var viewModel = new ContainerViewModel(container);

        // Assert
        Assert.That(viewModel.Container, Is.SameAs(container));
        Assert.That(viewModel.IdContainer, Is.Null);
        Assert.That(viewModel.Name, Is.EqualTo("Container without ID"));
    }

    /// <summary>
    /// Tests constructor behavior when null is passed as the container parameter.
    /// Input: null.
    /// Expected: Container property is set to null (no validation in constructor).
    /// </summary>
    [Test]
    public void Constructor_WithNullContainer_SetsContainerToNull()
    {
        // Arrange
        Container? container = null;

        // Act
        var viewModel = new ContainerViewModel(container!);

        // Assert
        Assert.That(viewModel.Container, Is.Null);
        Assert.That(viewModel.IdContainer, Is.Null);
        Assert.That(viewModel.Name, Is.EqualTo(string.Empty));
        Assert.That(viewModel.Notes, Is.EqualTo(string.Empty));
        Assert.That(viewModel.PhotoFileName, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that IdContainer returns null when Container property is null.
    /// </summary>
    [Test]
    public void IdContainer_WhenContainerIsNull_ReturnsNull()
    {
        // Arrange
        var viewModel = new ContainerViewModel(new Container());
        viewModel.Container = null;

        // Act
        int? result = viewModel.IdContainer;

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests that IdContainer returns null when Container.IdContainer is null.
    /// </summary>
    [Test]
    public void IdContainer_WhenContainerIdContainerIsNull_ReturnsNull()
    {
        // Arrange
        var container = new Container { IdContainer = null };
        var viewModel = new ContainerViewModel(container);

        // Act
        int? result = viewModel.IdContainer;

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests that IdContainer returns the correct value when Container.IdContainer has various valid values.
    /// Includes boundary values (int.MinValue, int.MaxValue), zero, and typical positive/negative values.
    /// </summary>
    /// <param name="expectedId">The expected IdContainer value to test</param>
    [TestCase(int.MinValue)]
    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(100)]
    [TestCase(int.MaxValue)]
    public void IdContainer_WhenContainerIdContainerHasValue_ReturnsValue(int expectedId)
    {
        // Arrange
        var container = new Container { IdContainer = expectedId };
        var viewModel = new ContainerViewModel(container);

        // Act
        int? result = viewModel.IdContainer;

        // Assert
        Assert.That(result, Is.EqualTo(expectedId));
    }

    /// <summary>
    /// Tests that IdContainer property reflects changes when the Container property is reassigned.
    /// </summary>
    [Test]
    public void IdContainer_WhenContainerIsReassigned_ReflectsNewValue()
    {
        // Arrange
        var container1 = new Container { IdContainer = 1 };
        var container2 = new Container { IdContainer = 2 };
        var viewModel = new ContainerViewModel(container1);

        // Act
        int? firstResult = viewModel.IdContainer;
        viewModel.Container = container2;
        int? secondResult = viewModel.IdContainer;

        // Assert
        Assert.That(firstResult, Is.EqualTo(1));
        Assert.That(secondResult, Is.EqualTo(2));
    }

    /// <summary>
    /// Tests that IdContainer property reflects changes when Container.IdContainer is modified.
    /// </summary>
    [Test]
    public void IdContainer_WhenContainerIdContainerIsModified_ReflectsNewValue()
    {
        // Arrange
        var container = new Container { IdContainer = 1 };
        var viewModel = new ContainerViewModel(container);

        // Act
        int? firstResult = viewModel.IdContainer;
        container.IdContainer = 999;
        int? secondResult = viewModel.IdContainer;

        // Assert
        Assert.That(firstResult, Is.EqualTo(1));
        Assert.That(secondResult, Is.EqualTo(999));
    }

    /// <summary>
    /// Helper class to expose the protected OnPropertyChanged method for testing.
    /// </summary>
    private class TestableContainerViewModel : ContainerViewModel
    {
        public TestableContainerViewModel(Container container) : base(container)
        {
        }

        public void PublicOnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }

    /// <summary>
    /// Tests that OnPropertyChanged invokes the PropertyChanged event with the correct property name
    /// when there is at least one subscriber.
    /// </summary>
    /// <param name="propertyName">The property name to pass to OnPropertyChanged.</param>
    [TestCase("Name")]
    [TestCase("IdContainer")]
    [TestCase("Weight")]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase(null)]
    [TestCase("Property.With.Dots")]
    [TestCase("VeryLongPropertyNameThatExceedsNormalLengthToTestEdgeCaseHandlingInThePropertyChangedEventInvocationMechanism")]
    [TestCase("Property\nWith\nNewlines")]
    [TestCase("Property\tWith\tTabs")]
    public void OnPropertyChanged_WithSubscriber_InvokesEventWithCorrectPropertyName(string propertyName)
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new TestableContainerViewModel(mockContainer.Object);

        string? receivedPropertyName = null;
        object? receivedSender = null;
        viewModel.PropertyChanged += (sender, e) =>
        {
            receivedSender = sender;
            receivedPropertyName = e.PropertyName;
        };

        // Act
        viewModel.PublicOnPropertyChanged(propertyName);

        // Assert
        Assert.That(receivedPropertyName, Is.EqualTo(propertyName));
        Assert.That(receivedSender, Is.SameAs(viewModel));
    }

    /// <summary>
    /// Tests that OnPropertyChanged does not throw an exception when there are no subscribers
    /// to the PropertyChanged event (null event handler).
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithNoSubscriber_DoesNotThrowException()
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new TestableContainerViewModel(mockContainer.Object);

        // Act & Assert
        Assert.DoesNotThrow(() => viewModel.PublicOnPropertyChanged("SomeProperty"));
    }

    /// <summary>
    /// Tests that OnPropertyChanged can be called multiple times with the same property name
    /// and invokes the event each time.
    /// </summary>
    [Test]
    public void OnPropertyChanged_CalledMultipleTimes_InvokesEventEachTime()
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new TestableContainerViewModel(mockContainer.Object);

        int invocationCount = 0;
        viewModel.PropertyChanged += (sender, e) => invocationCount++;

        // Act
        viewModel.PublicOnPropertyChanged("TestProperty");
        viewModel.PublicOnPropertyChanged("TestProperty");
        viewModel.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(invocationCount, Is.EqualTo(3));
    }

    /// <summary>
    /// Tests that OnPropertyChanged invokes all subscribed event handlers when there are multiple subscribers.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithMultipleSubscribers_InvokesAllHandlers()
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new TestableContainerViewModel(mockContainer.Object);

        bool firstHandlerInvoked = false;
        bool secondHandlerInvoked = false;
        bool thirdHandlerInvoked = false;

        viewModel.PropertyChanged += (sender, e) => firstHandlerInvoked = true;
        viewModel.PropertyChanged += (sender, e) => secondHandlerInvoked = true;
        viewModel.PropertyChanged += (sender, e) => thirdHandlerInvoked = true;

        // Act
        viewModel.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(firstHandlerInvoked, Is.True);
        Assert.That(secondHandlerInvoked, Is.True);
        Assert.That(thirdHandlerInvoked, Is.True);
    }

    /// <summary>
    /// Tests that RowBorderColor returns "Orange" when IsSelectedInList is true,
    /// and "Transparent" when IsSelectedInList is false.
    /// </summary>
    /// <param name="isSelected">The value to set for IsSelectedInList.</param>
    /// <param name="expectedColor">The expected border color string.</param>
    [TestCase(true, "Orange")]
    [TestCase(false, "Transparent")]
    public void RowBorderColor_WhenIsSelectedInListIsSet_ReturnsExpectedColor(bool isSelected, string expectedColor)
    {
        // Arrange
        var containerMock = new Mock<Container>();
        var viewModel = new ContainerViewModel(containerMock.Object);
        viewModel.IsSelectedInList = isSelected;

        // Act
        var result = viewModel.RowBorderColor;

        // Assert
        Assert.That(result, Is.EqualTo(expectedColor));
    }

    /// <summary>
    /// Tests that RowBorderColor returns "Transparent" by default when IsSelectedInList has not been explicitly set.
    /// </summary>
    [Test]
    public void RowBorderColor_WhenIsSelectedInListIsDefault_ReturnsTransparent()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result = viewModel.RowBorderColor;

        // Assert
        Assert.That(result, Is.EqualTo("Transparent"));
    }

    /// <summary>
    /// Tests that ThumbnailSource returns null and caches the result when Container is null
    /// </summary>
    [Test]
    public void ThumbnailSource_ContainerIsNull_ReturnsNullAndCaches()
    {
        // Arrange
        var viewModel = new ContainerViewModel(null!);

        // Act
        var result1 = viewModel.ThumbnailSource;
        var result2 = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result1, Is.Null);
        Assert.That(result2, Is.Null);
    }

    /// <summary>
    /// Tests that ThumbnailSource returns null and caches the result when PhotoFileName is null
    /// </summary>
    [Test]
    public void ThumbnailSource_PhotoFileNameIsNull_ReturnsNullAndCaches()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        containerMock.SetupGet(c => c.PhotoFileName).Returns((string)null!);
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result1 = viewModel.ThumbnailSource;
        var result2 = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result1, Is.Null);
        Assert.That(result2, Is.Null);
    }

    /// <summary>
    /// Tests that ThumbnailSource returns null and caches the result when PhotoFileName is empty string
    /// </summary>
    [Test]
    public void ThumbnailSource_PhotoFileNameIsEmpty_ReturnsNullAndCaches()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        containerMock.SetupGet(c => c.PhotoFileName).Returns(string.Empty);
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result1 = viewModel.ThumbnailSource;
        var result2 = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result1, Is.Null);
        Assert.That(result2, Is.Null);
    }

    /// <summary>
    /// Tests that ThumbnailSource returns null and caches the result when PhotoFileName contains only whitespace
    /// </summary>
    [Test]
    public void ThumbnailSource_PhotoFileNameIsWhitespace_ReturnsNullAndCaches()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        containerMock.SetupGet(c => c.PhotoFileName).Returns("   ");
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result1 = viewModel.ThumbnailSource;
        var result2 = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result1, Is.Null);
        Assert.That(result2, Is.Null);
    }

    /// <summary>
    /// Tests that ThumbnailSource returns null and caches the result when GetPhotoFullPath returns null
    /// </summary>
    [Test]
    public void ThumbnailSource_GetPhotoFullPathReturnsNull_ReturnsNullAndCaches()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        containerMock.SetupGet(c => c.PhotoFileName).Returns("photo.jpg");
        containerMock.Setup(c => c.GetPhotoFullPath()).Returns((string)null!);
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result1 = viewModel.ThumbnailSource;
        var result2 = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result1, Is.Null);
        Assert.That(result2, Is.Null);
    }

    /// <summary>
    /// Tests that ThumbnailSource returns null and caches the result when GetPhotoFullPath returns empty string
    /// </summary>
    [Test]
    public void ThumbnailSource_GetPhotoFullPathReturnsEmpty_ReturnsNullAndCaches()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        containerMock.SetupGet(c => c.PhotoFileName).Returns("photo.jpg");
        containerMock.Setup(c => c.GetPhotoFullPath()).Returns(string.Empty);
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result1 = viewModel.ThumbnailSource;
        var result2 = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result1, Is.Null);
        Assert.That(result2, Is.Null);
    }

    /// <summary>
    /// Tests that ThumbnailSource returns null and caches the result when GetPhotoFullPath returns whitespace
    /// </summary>
    [Test]
    public void ThumbnailSource_GetPhotoFullPathReturnsWhitespace_ReturnsNullAndCaches()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        containerMock.SetupGet(c => c.PhotoFileName).Returns("photo.jpg");
        containerMock.Setup(c => c.GetPhotoFullPath()).Returns("  \t  ");
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result1 = viewModel.ThumbnailSource;
        var result2 = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result1, Is.Null);
        Assert.That(result2, Is.Null);
    }

    /// <summary>
    /// Tests that ThumbnailSource catches exceptions from GetPhotoFullPath and returns null
    /// </summary>
    [Test]
    public void ThumbnailSource_GetPhotoFullPathThrowsException_CatchesExceptionAndReturnsNull()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        containerMock.SetupGet(c => c.PhotoFileName).Returns("photo.jpg");
        containerMock.Setup(c => c.GetPhotoFullPath()).Throws(new InvalidOperationException("Test exception"));
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Tests that ThumbnailSource caches the result and subsequent accesses don't re-execute the logic
    /// </summary>
    [Test]
    public void ThumbnailSource_AccessedMultipleTimes_ReturnsCachedValue()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        containerMock.SetupGet(c => c.PhotoFileName).Returns("photo.jpg");
        containerMock.Setup(c => c.GetPhotoFullPath()).Returns((string)null!);
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result1 = viewModel.ThumbnailSource;
        var result2 = viewModel.ThumbnailSource;
        var result3 = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result1, Is.Null);
        Assert.That(result2, Is.Null);
        Assert.That(result3, Is.Null);

        // Verify GetPhotoFullPath was only called once (on first access, not on subsequent cached accesses)
        containerMock.Verify(c => c.GetPhotoFullPath(), Times.Once);
    }

    /// <summary>
    /// Tests that ThumbnailSource caches null when Container.PhotoFileName is null/whitespace and doesn't call GetPhotoFullPath on subsequent accesses
    /// </summary>
    [Test]
    public void ThumbnailSource_NullPhotoFileName_DoesNotCallGetPhotoFullPathOnSubsequentAccesses()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        containerMock.SetupGet(c => c.PhotoFileName).Returns((string)null!);
        containerMock.Setup(c => c.GetPhotoFullPath()).Returns("somepath.jpg");
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result1 = viewModel.ThumbnailSource;
        var result2 = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result1, Is.Null);
        Assert.That(result2, Is.Null);

        // GetPhotoFullPath should never be called since PhotoFileName is null
        containerMock.Verify(c => c.GetPhotoFullPath(), Times.Never);
    }

    /// <summary>
    /// Tests that ThumbnailSource handles UnauthorizedAccessException during photo loading
    /// </summary>
    [Test]
    public void ThumbnailSource_GetPhotoFullPathThrowsUnauthorizedAccessException_ReturnsNull()
    {
        // Arrange
        var containerMock = new Mock<Container>();
        containerMock.SetupGet(c => c.PhotoFileName).Returns("photo.jpg");
        containerMock.Setup(c => c.GetPhotoFullPath()).Throws(new UnauthorizedAccessException("Access denied"));
        var viewModel = new ContainerViewModel(containerMock.Object);

        // Act
        var result = viewModel.ThumbnailSource;

        // Assert
        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Partial test: ThumbnailSource should load ImageSource when file exists
    /// Note: This test cannot be fully automated because it depends on File.Exists() and ImageSource.FromFile(),
    /// which are static methods that cannot be mocked with Moq. To properly test this scenario:
    /// 1. Create a real test file in the file system
    /// 2. Configure Container.GetPhotoFullPath() to return the real file path
    /// 3. Verify that ThumbnailSource returns a non-null ImageSource
    /// 4. Clean up the test file after the test
    /// </summary>
    [Test]
    [Ignore("Requires file system access and cannot be mocked with current architecture")]
    public void ThumbnailSource_FileExists_LoadsImageSourceFromFile()
    {
        // TODO: Implement integration test with real file system
        // 1. Create temporary test image file
        // 2. Setup Container to return path to test file
        // 3. Access ThumbnailSource and verify it returns non-null ImageSource
        // 4. Delete temporary test file
        Assert.Inconclusive("This test requires file system access and ImageSource.FromFile() which cannot be mocked.");
    }

    /// <summary>
    /// Partial test: ThumbnailSource should return null when file path is valid but file doesn't exist
    /// Note: This test cannot be fully automated because it depends on File.Exists(), which is a static
    /// method that cannot be mocked with Moq. To properly test this scenario:
    /// 1. Configure Container.GetPhotoFullPath() to return a valid path format that doesn't exist
    /// 2. Ensure no file exists at that path
    /// 3. Verify that ThumbnailSource returns null
    /// </summary>
    [Test]
    [Ignore("Requires file system access and File.Exists() cannot be mocked with current architecture")]
    public void ThumbnailSource_ValidPathButFileDoesNotExist_ReturnsNull()
    {
        // TODO: Implement integration test with real file system
        // 1. Setup Container to return path to non-existent file
        // 2. Ensure file doesn't exist
        // 3. Access ThumbnailSource and verify it returns null
        Assert.Inconclusive("This test requires File.Exists() which cannot be mocked.");
    }

    /// <summary>
    /// Tests that ReloadThumbnail executes successfully without throwing exceptions
    /// when called on a ViewModel with a valid Container.
    /// Expected: Method completes without exceptions and resets internal thumbnail cache state.
    /// </summary>
    [Test]
    public void ReloadThumbnail_WithValidContainer_CompletesSuccessfully()
    {
        // Arrange
        var container = new Container { IdContainer = 1, Name = "Test Container" };
        var viewModel = new ContainerViewModel(container);

        // Act & Assert
        Assert.DoesNotThrow(() => viewModel.ReloadThumbnail());
    }

    /// <summary>
    /// Tests that ReloadThumbnail executes successfully without throwing exceptions
    /// when called on a ViewModel with a null Container.
    /// Expected: Method completes without exceptions even with null Container.
    /// </summary>
    [Test]
    public void ReloadThumbnail_WithNullContainer_CompletesSuccessfully()
    {
        // Arrange
        var container = new Container();
        var viewModel = new ContainerViewModel(container);
        viewModel.Container = null!;

        // Act & Assert
        Assert.DoesNotThrow(() => viewModel.ReloadThumbnail());
    }

    /// <summary>
    /// Tests that ReloadThumbnail can be called multiple times consecutively
    /// without throwing exceptions.
    /// Expected: Method can be called multiple times safely.
    /// </summary>
    [Test]
    public void ReloadThumbnail_CalledMultipleTimes_CompletesSuccessfully()
    {
        // Arrange
        var container = new Container { IdContainer = 1, Name = "Test Container" };
        var viewModel = new ContainerViewModel(container);

        // Act & Assert
        Assert.DoesNotThrow(() =>
        {
            viewModel.ReloadThumbnail();
            viewModel.ReloadThumbnail();
            viewModel.ReloadThumbnail();
        });
    }

    /// <summary>
    /// Tests that ReloadThumbnail executes successfully when Container has a PhotoFileName set.
    /// Expected: Method completes without exceptions regardless of PhotoFileName value.
    /// </summary>
    [Test]
    public void ReloadThumbnail_WithContainerHavingPhotoFileName_CompletesSuccessfully()
    {
        // Arrange
        var container = new Container
        {
            IdContainer = 1,
            Name = "Test Container",
            PhotoFileName = "test_photo.jpg"
        };
        var viewModel = new ContainerViewModel(container);

        // Act & Assert
        Assert.DoesNotThrow(() => viewModel.ReloadThumbnail());
    }

    /// <summary>
    /// Tests that ReloadThumbnail executes successfully when Container has an empty PhotoFileName.
    /// Expected: Method completes without exceptions with empty PhotoFileName.
    /// </summary>
    [Test]
    public void ReloadThumbnail_WithEmptyPhotoFileName_CompletesSuccessfully()
    {
        // Arrange
        var container = new Container
        {
            IdContainer = 1,
            Name = "Test Container",
            PhotoFileName = string.Empty
        };
        var viewModel = new ContainerViewModel(container);

        // Act & Assert
        Assert.DoesNotThrow(() => viewModel.ReloadThumbnail());
    }

    /// <summary>
    /// Tests that ReloadThumbnail executes successfully when Container has a whitespace-only PhotoFileName.
    /// Expected: Method completes without exceptions with whitespace PhotoFileName.
    /// </summary>
    [Test]
    public void ReloadThumbnail_WithWhitespacePhotoFileName_CompletesSuccessfully()
    {
        // Arrange
        var container = new Container
        {
            IdContainer = 1,
            Name = "Test Container",
            PhotoFileName = "   "
        };
        var viewModel = new ContainerViewModel(container);

        // Act & Assert
        Assert.DoesNotThrow(() => viewModel.ReloadThumbnail());
    }

    /// <summary>
    /// Tests that ReloadThumbnail executes successfully when Container has a null PhotoFileName.
    /// Expected: Method completes without exceptions with null PhotoFileName.
    /// </summary>
    [Test]
    public void ReloadThumbnail_WithNullPhotoFileName_CompletesSuccessfully()
    {
        // Arrange
        var container = new Container
        {
            IdContainer = 1,
            Name = "Test Container",
            PhotoFileName = null
        };
        var viewModel = new ContainerViewModel(container);

        // Act & Assert
        Assert.DoesNotThrow(() => viewModel.ReloadThumbnail());
    }

    /// <summary>
    /// Tests that IsSelectedInList getter returns the correct initial value (false by default).
    /// </summary>
    [Test]
    public void IsSelectedInList_GetInitialValue_ReturnsFalse()
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new ContainerViewModel(mockContainer.Object);

        // Act
        var result = viewModel.IsSelectedInList;

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Tests that IsSelectedInList getter returns the correct value after setting.
    /// </summary>
    /// <param name="setValue">The value to set on the property.</param>
    [TestCase(true)]
    [TestCase(false)]
    public void IsSelectedInList_GetAfterSet_ReturnsSetValue(bool setValue)
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new ContainerViewModel(mockContainer.Object);
        viewModel.IsSelectedInList = setValue;

        // Act
        var result = viewModel.IsSelectedInList;

        // Assert
        Assert.That(result, Is.EqualTo(setValue));
    }

    /// <summary>
    /// Tests that setting IsSelectedInList to a different value raises PropertyChanged events
    /// for both "IsSelectedInList" and "RowBorderColor".
    /// </summary>
    /// <param name="initialValue">The initial value of the property.</param>
    /// <param name="newValue">The new value to set.</param>
    [TestCase(false, true)]
    [TestCase(true, false)]
    public void IsSelectedInList_SetToDifferentValue_RaisesPropertyChangedEvents(bool initialValue, bool newValue)
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new ContainerViewModel(mockContainer.Object);
        viewModel.IsSelectedInList = initialValue;

        var propertyChangedEvents = new System.Collections.Generic.List<string>();
        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName != null)
            {
                propertyChangedEvents.Add(e.PropertyName);
            }
        };

        // Act
        viewModel.IsSelectedInList = newValue;

        // Assert
        Assert.That(propertyChangedEvents, Has.Count.EqualTo(2));
        Assert.That(propertyChangedEvents[0], Is.EqualTo(nameof(viewModel.IsSelectedInList)));
        Assert.That(propertyChangedEvents[1], Is.EqualTo(nameof(viewModel.RowBorderColor)));
    }

    /// <summary>
    /// Tests that setting IsSelectedInList to the same value does not raise PropertyChanged events.
    /// </summary>
    /// <param name="value">The value to set (both initial and new value).</param>
    [TestCase(false)]
    [TestCase(true)]
    public void IsSelectedInList_SetToSameValue_DoesNotRaisePropertyChanged(bool value)
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new ContainerViewModel(mockContainer.Object);
        viewModel.IsSelectedInList = value;

        var propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, e) =>
        {
            propertyChangedRaised = true;
        };

        // Act
        viewModel.IsSelectedInList = value;

        // Assert
        Assert.That(propertyChangedRaised, Is.False);
    }

    /// <summary>
    /// Tests that setting IsSelectedInList to a different value updates the property value correctly.
    /// </summary>
    /// <param name="initialValue">The initial value of the property.</param>
    /// <param name="newValue">The new value to set.</param>
    [TestCase(false, true)]
    [TestCase(true, false)]
    public void IsSelectedInList_SetToDifferentValue_UpdatesPropertyValue(bool initialValue, bool newValue)
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new ContainerViewModel(mockContainer.Object);
        viewModel.IsSelectedInList = initialValue;

        // Act
        viewModel.IsSelectedInList = newValue;

        // Assert
        Assert.That(viewModel.IsSelectedInList, Is.EqualTo(newValue));
    }

    /// <summary>
    /// Tests that PropertyChanged event sender is the viewmodel instance itself.
    /// </summary>
    [Test]
    public void IsSelectedInList_SetToDifferentValue_PropertyChangedSenderIsViewModel()
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new ContainerViewModel(mockContainer.Object);
        object? capturedSender = null;
        viewModel.PropertyChanged += (sender, e) =>
        {
            capturedSender = sender;
        };

        // Act
        viewModel.IsSelectedInList = true;

        // Assert
        Assert.That(capturedSender, Is.SameAs(viewModel));
    }

    /// <summary>
    /// Tests that multiple consecutive changes to IsSelectedInList raise PropertyChanged events each time.
    /// </summary>
    [Test]
    public void IsSelectedInList_MultipleConsecutiveChanges_RaisesPropertyChangedEachTime()
    {
        // Arrange
        var mockContainer = new Mock<Container>();
        var viewModel = new ContainerViewModel(mockContainer.Object);
        var eventCount = 0;
        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(viewModel.IsSelectedInList))
            {
                eventCount++;
            }
        };

        // Act
        viewModel.IsSelectedInList = true;
        viewModel.IsSelectedInList = false;
        viewModel.IsSelectedInList = true;

        // Assert
        Assert.That(eventCount, Is.EqualTo(3));
    }
}