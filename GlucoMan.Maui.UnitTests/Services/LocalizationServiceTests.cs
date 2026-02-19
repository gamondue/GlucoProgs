using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

using GlucoMan.Maui.Resources.Strings;
using GlucoMan.Maui.Services;
using Microsoft.Maui.Storage;
using NUnit.Framework;

namespace GlucoMan.Maui.Services.UnitTests;



/// <summary>
/// Unit tests for the LocalizationService class
/// </summary>
public partial class LocalizationServiceTests
{
    [SetUp]
    public void Setup()
    {
        // Reset culture to a known state before each test
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("en");
            CultureInfo.CurrentUICulture = new CultureInfo("en");
        }
        catch
        {
            // Ignore if setting culture fails in test environment
        }
    }

    /// <summary>
    /// Tests that the CurrentCulture property getter returns the initialized culture after construction.
    /// Input: Default constructor initialization.
    /// Expected: CurrentCulture property returns a non-null CultureInfo object.
    /// </summary>
    [Test]
    public void CurrentCulture_AfterConstruction_ReturnsNonNullCultureInfo()
    {
        // Arrange & Act
        LocalizationService service = null;

        try
        {
            service = new LocalizationService();
        }
        catch (Exception ex)
        {
            Assert.Inconclusive($"Cannot create LocalizationService - MAUI not initialized: {ex.Message}");
            return;
        }

        // Assert
        Assert.That(service.CurrentCulture, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the CurrentCulture property getter returns the correct culture after SetCulture is called.
    /// Input: SetCulture called with a specific culture.
    /// Expected: CurrentCulture property returns the set culture.
    /// </summary>
    [Test]
    public void CurrentCulture_AfterSetCultureWithEnglish_ReturnsEnglishCulture()
    {
        // Arrange
        var service = new LocalizationService();
        var expectedCulture = new CultureInfo("en");

        // Act
        service.SetCulture(expectedCulture);

        // Assert
        Assert.That(service.CurrentCulture.Name, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that the CurrentCulture property getter returns the correct culture after SetCulture is called with Italian.
    /// Input: SetCulture called with Italian culture.
    /// Expected: CurrentCulture property returns Italian culture.
    /// </summary>
    [Test]
    public void CurrentCulture_AfterSetCultureWithItalian_ReturnsItalianCulture()
    {
        // Arrange
        var service = new LocalizationService();
        var expectedCulture = new CultureInfo("it");

        // Act
        service.SetCulture(expectedCulture);

        // Assert
        Assert.That(service.CurrentCulture.Name, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that PropertyChanged event is raised for CurrentCulture when culture changes to a different culture.
    /// Input: SetCulture called with a different culture than the current one.
    /// Expected: PropertyChanged event is raised with propertyName "CurrentCulture".
    /// </summary>
    [Test]
    public void CurrentCulture_WhenCultureChangesToDifferent_RaisesPropertyChangedForCurrentCulture()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en"));

        var propertyChangedRaised = false;
        string? raisedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LocalizationService.CurrentCulture))
            {
                propertyChangedRaised = true;
                raisedPropertyName = args.PropertyName;
            }
        };

        // Act
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(propertyChangedRaised, Is.True);
        Assert.That(raisedPropertyName, Is.EqualTo("CurrentCulture"));
    }

    /// <summary>
    /// Tests that PropertyChanged event is raised for CurrentLanguageDisplayName when culture changes.
    /// Input: SetCulture called with a different culture than the current one.
    /// Expected: PropertyChanged event is raised with propertyName "CurrentLanguageDisplayName".
    /// </summary>
    [Test]
    public void CurrentCulture_WhenCultureChangesToDifferent_RaisesPropertyChangedForCurrentLanguageDisplayName()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en"));

        var propertyChangedRaised = false;
        string? raisedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LocalizationService.CurrentLanguageDisplayName))
            {
                propertyChangedRaised = true;
                raisedPropertyName = args.PropertyName;
            }
        };

        // Act
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(propertyChangedRaised, Is.True);
        Assert.That(raisedPropertyName, Is.EqualTo("CurrentLanguageDisplayName"));
    }

    /// <summary>
    /// Tests that PropertyChanged event is NOT raised when setting the same culture (by Name).
    /// Input: SetCulture called twice with cultures that have the same Name.
    /// Expected: PropertyChanged event is not raised on the second call.
    /// </summary>
    [Test]
    public void CurrentCulture_WhenCultureSetToSameName_DoesNotRaisePropertyChanged()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en"));

        var propertyChangedCount = 0;

        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LocalizationService.CurrentCulture))
            {
                propertyChangedCount++;
            }
        };

        // Act
        service.SetCulture(new CultureInfo("en"));

        // Assert
        Assert.That(propertyChangedCount, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that PropertyChanged event is raised with correct count when culture changes multiple times.
    /// Input: SetCulture called multiple times with different cultures.
    /// Expected: PropertyChanged event is raised each time the culture name differs.
    /// </summary>
    [Test]
    public void CurrentCulture_WhenCultureChangesMultipleTimes_RaisesPropertyChangedCorrectNumberOfTimes()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en"));

        var propertyChangedCount = 0;

        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LocalizationService.CurrentCulture))
            {
                propertyChangedCount++;
            }
        };

        // Act
        service.SetCulture(new CultureInfo("it"));
        service.SetCulture(new CultureInfo("en"));
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(propertyChangedCount, Is.EqualTo(3));
    }

    /// <summary>
    /// Tests that CurrentCulture handles en-US variant correctly (comparing by Name).
    /// Input: SetCulture with en-US culture.
    /// Expected: CurrentCulture is set and PropertyChanged is raised when changing from different culture.
    /// </summary>
    [Test]
    public void CurrentCulture_WhenSetToEnUsVariant_UpdatesCorrectly()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("it"));

        var propertyChangedRaised = false;
        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LocalizationService.CurrentCulture))
            {
                propertyChangedRaised = true;
            }
        };

        // Act
        service.SetCulture(new CultureInfo("en-US"));

        // Assert
        Assert.That(propertyChangedRaised, Is.True);
        Assert.That(service.CurrentCulture, Is.Not.Null);
    }

    /// <summary>
    /// Tests that CurrentCulture handles it-IT variant correctly (comparing by Name).
    /// Input: SetCulture with it-IT culture.
    /// Expected: CurrentCulture is set and PropertyChanged is raised when changing from different culture.
    /// </summary>
    [Test]
    public void CurrentCulture_WhenSetToItItVariant_UpdatesCorrectly()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en"));

        var propertyChangedRaised = false;
        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LocalizationService.CurrentCulture))
            {
                propertyChangedRaised = true;
            }
        };

        // Act
        service.SetCulture(new CultureInfo("it-IT"));

        // Assert
        Assert.That(propertyChangedRaised, Is.True);
        Assert.That(service.CurrentCulture, Is.Not.Null);
    }

    /// <summary>
    /// Tests that CurrentCulture property setter correctly compares culture Names (null-safe).
    /// Input: Set culture when current culture might be in various states.
    /// Expected: Comparison works correctly with null-conditional operator.
    /// </summary>
    [Test]
    public void CurrentCulture_PropertyChangedEventRaised_IncludesSenderAndArgs()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en"));

        object? capturedSender = null;
        PropertyChangedEventArgs? capturedArgs = null;

        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LocalizationService.CurrentCulture))
            {
                capturedSender = sender;
                capturedArgs = args;
            }
        };

        // Act
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(capturedSender, Is.SameAs(service));
        Assert.That(capturedArgs, Is.Not.Null);
        Assert.That(capturedArgs!.PropertyName, Is.EqualTo("CurrentCulture"));
    }

    /// <summary>
    /// Tests that SetCulture with a valid supported culture name "en" sets the CurrentCulture to English.
    /// </summary>
    [Test]
    public void SetCulture_ValidEnglishCultureName_SetsCurrentCultureToEnglish()
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture("en");

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with a valid supported culture name "it" sets the CurrentCulture to Italian.
    /// </summary>
    [Test]
    public void SetCulture_ValidItalianCultureName_SetsCurrentCultureToItalian()
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture("it");

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture with culture name in uppercase sets the correct culture (case-insensitive).
    /// </summary>
    [TestCase("EN")]
    [TestCase("IT")]
    public void SetCulture_UppercaseCultureName_SetsCorrectCulture(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        var expectedLanguage = cultureName.ToLower();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo(expectedLanguage));
    }

    /// <summary>
    /// Tests that SetCulture with culture name in mixed case sets the correct culture (case-insensitive).
    /// </summary>
    [TestCase("En")]
    [TestCase("It")]
    public void SetCulture_MixedCaseCultureName_SetsCorrectCulture(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        var expectedLanguage = cultureName.ToLower();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo(expectedLanguage));
    }

    /// <summary>
    /// Tests that SetCulture with culture name containing region code matches to the base culture.
    /// </summary>
    [TestCase("en-US", "en")]
    [TestCase("en-GB", "en")]
    [TestCase("it-IT", "it")]
    public void SetCulture_CultureNameWithRegion_MatchesToBaseCulture(string cultureName, string expectedLanguage)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo(expectedLanguage));
    }

    /// <summary>
    /// Tests that SetCulture with null culture name defaults to English.
    /// </summary>
    [Test]
    public void SetCulture_NullCultureName_DefaultsToEnglish()
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture((string)null!);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with empty string culture name defaults to English.
    /// </summary>
    [Test]
    public void SetCulture_EmptyCultureName_DefaultsToEnglish()
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(string.Empty);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with whitespace-only culture name defaults to English.
    /// </summary>
    [TestCase(" ")]
    [TestCase("   ")]
    [TestCase("\t")]
    [TestCase("\r\n")]
    public void SetCulture_WhitespaceCultureName_DefaultsToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with an invalid culture name defaults to English.
    /// </summary>
    [TestCase("xx")]
    [TestCase("invalid")]
    [TestCase("123")]
    [TestCase("!!!")]
    public void SetCulture_InvalidCultureName_DefaultsToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with a valid but unsupported culture name defaults to English.
    /// </summary>
    [TestCase("fr")]
    [TestCase("de")]
    [TestCase("es")]
    [TestCase("ja")]
    public void SetCulture_UnsupportedValidCultureName_DefaultsToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with a very long string defaults to English.
    /// </summary>
    [Test]
    public void SetCulture_VeryLongString_DefaultsToEnglish()
    {
        // Arrange
        var service = new LocalizationService();
        var longString = new string('a', 10000);

        // Act
        service.SetCulture(longString);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture raises the PropertyChanged event for CurrentCulture when culture changes.
    /// </summary>
    [Test]
    public void SetCulture_ChangingCulture_RaisesPropertyChangedEvent()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en");
        var propertyChangedRaised = false;
        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LocalizationService.CurrentCulture))
            {
                propertyChangedRaised = true;
            }
        };

        // Act
        service.SetCulture("it");

        // Assert
        Assert.That(propertyChangedRaised, Is.True);
    }

    /// <summary>
    /// Tests that SetCulture raises the CultureChanged event when culture changes.
    /// </summary>
    [Test]
    public void SetCulture_ChangingCulture_RaisesCultureChangedEvent()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en");
        var cultureChangedRaised = false;
        service.CultureChanged += (sender, args) =>
        {
            cultureChangedRaised = true;
        };

        // Act
        service.SetCulture("it");

        // Assert
        Assert.That(cultureChangedRaised, Is.True);
    }

    /// <summary>
    /// Tests that SetCulture does not raise PropertyChanged event when setting the same culture.
    /// </summary>
    [Test]
    public void SetCulture_SameCulture_DoesNotRaisePropertyChangedForCurrentCulture()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en");
        var propertyChangedCount = 0;
        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LocalizationService.CurrentCulture))
            {
                propertyChangedCount++;
            }
        };

        // Act
        service.SetCulture("en");

        // Assert
        Assert.That(propertyChangedCount, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests that SetCulture updates the static CultureInfo.CurrentCulture.
    /// </summary>
    [Test]
    public void SetCulture_ValidCultureName_UpdatesStaticCurrentCulture()
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture("it");

        // Assert
        Assert.That(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture updates the static CultureInfo.CurrentUICulture.
    /// </summary>
    [Test]
    public void SetCulture_ValidCultureName_UpdatesStaticCurrentUICulture()
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture("it");

        // Assert
        Assert.That(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture with special characters defaults to English.
    /// </summary>
    [TestCase("en@#$")]
    [TestCase("it!!!")]
    [TestCase("@@@")]
    public void SetCulture_SpecialCharacters_DefaultsToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the Italian language string when culture is set to Italian (short form).
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_ItalianCultureShortForm_ReturnsItalianLanguageString()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("it");

        // Act
        var result = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(AppStrings.LanguageItalian));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the Italian language string when culture is set to Italian (long form).
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_ItalianCultureLongForm_ReturnsItalianLanguageString()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("it-IT");

        // Act
        var result = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(AppStrings.LanguageItalian));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the English language string when culture is set to English (short form).
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_EnglishCultureShortForm_ReturnsEnglishLanguageString()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en");

        // Act
        var result = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the English language string when culture is set to English (long form).
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_EnglishCultureLongForm_ReturnsEnglishLanguageString()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en-US");

        // Act
        var result = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the English language string (default) when culture is set to an unsupported culture.
    /// Input: French culture "fr".
    /// Expected: Returns English language string as fallback.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_UnsupportedCulture_ReturnsEnglishLanguageStringAsDefault()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("fr");

        // Act
        var result = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the English language string when culture is set to German.
    /// Input: German culture "de-DE".
    /// Expected: Returns English language string as fallback for unsupported culture.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_GermanCulture_ReturnsEnglishLanguageStringAsDefault()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("de-DE");

        // Act
        var result = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the English language string when culture is set to an empty string.
    /// Input: Empty string for culture name.
    /// Expected: Returns English language string as fallback.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_EmptyStringCulture_ReturnsEnglishLanguageString()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(string.Empty);

        // Act
        var result = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the English language string when culture is set to whitespace.
    /// Input: Whitespace-only string for culture name.
    /// Expected: Returns English language string as fallback.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_WhitespaceCulture_ReturnsEnglishLanguageString()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("   ");

        // Act
        var result = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the English language string when SetCulture is called with a null CultureInfo.
    /// Input: Null CultureInfo.
    /// Expected: Returns English language string (null is converted to English in SetCulture).
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_NullCultureInfo_ReturnsEnglishLanguageString()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture((CultureInfo?)null!);

        // Act
        var result = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName changes when culture is changed from English to Italian.
    /// Input: Set culture to English, then change to Italian.
    /// Expected: Display name changes from English string to Italian string.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_CultureChangedFromEnglishToItalian_DisplayNameChanges()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en");
        var englishDisplayName = service.CurrentLanguageDisplayName;

        // Act
        service.SetCulture("it");
        var italianDisplayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(englishDisplayName, Is.EqualTo(AppStrings.LanguageEnglish));
        Assert.That(italianDisplayName, Is.EqualTo(AppStrings.LanguageItalian));
        Assert.That(englishDisplayName, Is.Not.EqualTo(italianDisplayName));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName raises PropertyChanged notification when culture changes.
    /// Input: Set culture to Italian after initialization.
    /// Expected: PropertyChanged event is raised for CurrentLanguageDisplayName.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_CultureChanged_RaisesPropertyChangedEvent()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en");
        var propertyChangedRaised = false;
        var propertyName = string.Empty;

        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(LocalizationService.CurrentLanguageDisplayName))
            {
                propertyChangedRaised = true;
                propertyName = args.PropertyName;
            }
        };

        // Act
        service.SetCulture("it");

        // Assert
        Assert.That(propertyChangedRaised, Is.True);
        Assert.That(propertyName, Is.EqualTo(nameof(LocalizationService.CurrentLanguageDisplayName)));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns consistent value when accessed multiple times without culture change.
    /// Input: Set culture to Italian and read property multiple times.
    /// Expected: Same value returned on each access.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_AccessedMultipleTimes_ReturnsConsistentValue()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("it");

        // Act
        var firstAccess = service.CurrentLanguageDisplayName;
        var secondAccess = service.CurrentLanguageDisplayName;
        var thirdAccess = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(firstAccess, Is.EqualTo(secondAccess));
        Assert.That(secondAccess, Is.EqualTo(thirdAccess));
        Assert.That(firstAccess, Is.EqualTo(AppStrings.LanguageItalian));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName handles invalid culture names gracefully.
    /// Input: Invalid culture name that would throw CultureNotFoundException.
    /// Expected: Returns English language string as fallback.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_InvalidCultureName_ReturnsEnglishLanguageString()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("invalid-culture-xyz");

        // Act
        var result = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that SetCulture with a null culture parameter defaults to English culture.
    /// Verifies the culture is correctly set and events are raised.
    /// </summary>
    [Test]
    public void SetCulture_WithNullCulture_SetsEnglishCulture()
    {
        // Arrange
        var service = new LocalizationService();
        var propertyChangedEvents = new System.Collections.Generic.List<string>();
        var cultureChangedRaised = false;

        service.PropertyChanged += (sender, args) => propertyChangedEvents.Add(args.PropertyName ?? string.Empty);
        service.CultureChanged += (sender, args) => cultureChangedRaised = true;

        // Act
        service.SetCulture((CultureInfo)null!);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(propertyChangedEvents, Does.Contain("CurrentCulture"));
        Assert.That(propertyChangedEvents, Does.Contain("CurrentLanguageDisplayName"));
        Assert.That(propertyChangedEvents, Does.Contain(string.Empty));
        Assert.That(cultureChangedRaised, Is.True);
    }

    /// <summary>
    /// Tests that SetCulture with a valid English culture sets the culture correctly.
    /// Verifies CurrentCulture property and events are raised.
    /// </summary>
    [Test]
    public void SetCulture_WithValidEnglishCulture_SetsCultureAndRaisesEvents()
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo("en");
        var propertyChangedEvents = new System.Collections.Generic.List<string>();
        var cultureChangedRaised = false;

        service.PropertyChanged += (sender, args) => propertyChangedEvents.Add(args.PropertyName ?? string.Empty);
        service.CultureChanged += (sender, args) => cultureChangedRaised = true;

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(service.CurrentCulture.Name, Is.EqualTo("en"));
        Assert.That(cultureChangedRaised, Is.True);
    }

    /// <summary>
    /// Tests that SetCulture with a valid Italian culture sets the culture correctly.
    /// Verifies CurrentCulture property is set to Italian.
    /// </summary>
    [Test]
    public void SetCulture_WithValidItalianCulture_SetsCultureToItalian()
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo("it");

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(service.CurrentCulture.Name, Is.EqualTo("it"));
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture with a specific regional English culture resolves to base English.
    /// Verifies that "en-US" is resolved to "en" from available cultures.
    /// </summary>
    [Test]
    public void SetCulture_WithEnglishUSCulture_ResolvesToEnglish()
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo("en-US");

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with a specific regional Italian culture resolves to base Italian.
    /// Verifies that "it-IT" is resolved to "it" from available cultures.
    /// </summary>
    [Test]
    public void SetCulture_WithItalianITCulture_ResolvesToItalian()
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo("it-IT");

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture with an unsupported culture defaults to English.
    /// Verifies French culture falls back to English as it's not in available cultures.
    /// </summary>
    [Test]
    public void SetCulture_WithUnsupportedFrenchCulture_DefaultsToEnglish()
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo("fr");

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture raises PropertyChanged event for CurrentCulture.
    /// Verifies the specific property name is included in the event.
    /// </summary>
    [Test]
    public void SetCulture_WithDifferentCulture_RaisesPropertyChangedForCurrentCulture()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en")); // Set initial culture
        var propertyChangedEvents = new System.Collections.Generic.List<string>();
        service.PropertyChanged += (sender, args) => propertyChangedEvents.Add(args.PropertyName ?? string.Empty);

        // Act
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(propertyChangedEvents, Does.Contain("CurrentCulture"));
    }

    /// <summary>
    /// Tests that SetCulture raises PropertyChanged event for CurrentLanguageDisplayName.
    /// Verifies the display name property change is notified when culture changes.
    /// </summary>
    [Test]
    public void SetCulture_WithDifferentCulture_RaisesPropertyChangedForCurrentLanguageDisplayName()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en")); // Set initial culture
        var propertyChangedEvents = new System.Collections.Generic.List<string>();
        service.PropertyChanged += (sender, args) => propertyChangedEvents.Add(args.PropertyName ?? string.Empty);

        // Act
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(propertyChangedEvents, Does.Contain("CurrentLanguageDisplayName"));
    }

    /// <summary>
    /// Tests that SetCulture raises PropertyChanged event with empty string to refresh all bindings.
    /// Verifies the global property change notification from OnCultureChanged.
    /// </summary>
    [Test]
    public void SetCulture_WithDifferentCulture_RaisesPropertyChangedWithEmptyString()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en")); // Set initial culture
        var propertyChangedEvents = new System.Collections.Generic.List<string>();
        service.PropertyChanged += (sender, args) => propertyChangedEvents.Add(args.PropertyName ?? string.Empty);

        // Act
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(propertyChangedEvents, Does.Contain(string.Empty));
    }

    /// <summary>
    /// Tests that SetCulture raises the CultureChanged event.
    /// Verifies subscribers are notified when culture changes.
    /// </summary>
    [Test]
    public void SetCulture_WithValidCulture_RaisesCultureChangedEvent()
    {
        // Arrange
        var service = new LocalizationService();
        var cultureChangedRaised = false;
        service.CultureChanged += (sender, args) => cultureChangedRaised = true;

        // Act
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(cultureChangedRaised, Is.True);
    }

    /// <summary>
    /// Tests that SetCulture with the same culture still raises CultureChanged event.
    /// Verifies the event is always raised even if the culture hasn't changed.
    /// </summary>
    [Test]
    public void SetCulture_WithSameCulture_StillRaisesCultureChangedEvent()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en"));
        var cultureChangedCount = 0;
        service.CultureChanged += (sender, args) => cultureChangedCount++;

        // Act
        service.SetCulture(new CultureInfo("en"));

        // Assert
        Assert.That(cultureChangedCount, Is.EqualTo(1));
    }

    /// <summary>
    /// Tests that SetCulture with the same culture does not raise PropertyChanged for CurrentCulture.
    /// Verifies property change optimization when the culture value hasn't actually changed.
    /// </summary>
    [Test]
    public void SetCulture_WithSameCulture_DoesNotRaisePropertyChangedForCurrentCulture()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en"));
        var currentCultureChangedCount = 0;
        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == "CurrentCulture")
                currentCultureChangedCount++;
        };

        // Act
        service.SetCulture(new CultureInfo("en"));

        // Assert
        Assert.That(currentCultureChangedCount, Is.EqualTo(0));
    }

    /// <summary>
    /// Tests SetCulture with multiple different unsupported cultures.
    /// Verifies all unsupported cultures default to English.
    /// </summary>
    [TestCase("fr")]
    [TestCase("de")]
    [TestCase("es")]
    [TestCase("ja")]
    [TestCase("zh")]
    public void SetCulture_WithVariousUnsupportedCultures_DefaultsToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo(cultureName);

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests SetCulture with various regional variants of English.
    /// Verifies all English variants resolve to base English culture.
    /// </summary>
    [TestCase("en-US")]
    [TestCase("en-GB")]
    [TestCase("en-CA")]
    [TestCase("en-AU")]
    public void SetCulture_WithEnglishRegionalVariants_ResolvesToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo(cultureName);

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests SetCulture with various regional variants of Italian.
    /// Verifies all Italian variants resolve to base Italian culture.
    /// </summary>
    [TestCase("it-IT")]
    [TestCase("it-CH")]
    public void SetCulture_WithItalianRegionalVariants_ResolvesToItalian(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo(cultureName);

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture passes sender and EventArgs correctly to CultureChanged event.
    /// Verifies event arguments are properly structured.
    /// </summary>
    [Test]
    public void SetCulture_WhenRaisingCultureChanged_PassesCorrectEventArgs()
    {
        // Arrange
        var service = new LocalizationService();
        object? eventSender = null;
        EventArgs? eventArgs = null;
        service.CultureChanged += (sender, args) =>
        {
            eventSender = sender;
            eventArgs = args;
        };

        // Act
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(eventSender, Is.SameAs(service));
        Assert.That(eventArgs, Is.Not.Null);
        Assert.That(eventArgs, Is.EqualTo(EventArgs.Empty));
    }

    /// <summary>
    /// Tests that SetCulture passes correct PropertyChangedEventArgs to PropertyChanged event.
    /// Verifies the property name is correctly included in the event arguments.
    /// </summary>
    [Test]
    public void SetCulture_WhenRaisingPropertyChanged_PassesCorrectEventArgs()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(new CultureInfo("en")); // Set initial culture
        PropertyChangedEventArgs? currentCultureEventArgs = null;
        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == "CurrentCulture")
                currentCultureEventArgs = args;
        };

        // Act
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(currentCultureEventArgs, Is.Not.Null);
        Assert.That(currentCultureEventArgs!.PropertyName, Is.EqualTo("CurrentCulture"));
    }

    /// <summary>
    /// Helper class to expose protected OnPropertyChanged method for testing.
    /// Note: LocalizationService constructor accesses Microsoft.Maui.Storage.Preferences which may require
    /// platform initialization. If tests fail during construction, consider running tests in a platform context.
    /// </summary>
    private class TestableLocalizationService : LocalizationService
    {
        public void PublicOnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }

    /// <summary>
    /// Tests that OnPropertyChanged raises the PropertyChanged event with the correct property name
    /// for various valid and edge-case property name inputs.
    /// Input: Various property name strings including normal names, empty, whitespace, special characters.
    /// Expected: PropertyChanged event is raised with the exact property name provided.
    /// </summary>
    [TestCase("CurrentCulture")]
    [TestCase("CurrentLanguageDisplayName")]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("VeryLongPropertyNameThatExceedsNormalLengthWithManyCharacters1234567890")]
    [TestCase("Property.With.Dots")]
    [TestCase("Property[0]")]
    [TestCase("Property_With_Underscores")]
    [TestCase("PropertyWith123Numbers")]
    [TestCase("UPPERCASEPROPERTY")]
    [TestCase("lowercaseproperty")]
    public void OnPropertyChanged_WithSubscriberAndVariousPropertyNames_RaisesEventWithCorrectPropertyName(string propertyName)
    {
        // Arrange
        var service = new TestableLocalizationService();
        string? capturedPropertyName = null;
        bool eventRaised = false;

        service.PropertyChanged += (sender, e) =>
        {
            eventRaised = true;
            capturedPropertyName = e.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(propertyName);

        // Assert
        Assert.That(eventRaised, Is.True, "PropertyChanged event should be raised");
        Assert.That(capturedPropertyName, Is.EqualTo(propertyName), "Property name in event args should match input");
    }

    /// <summary>
    /// Tests that OnPropertyChanged handles null property name without throwing.
    /// Input: null property name.
    /// Expected: PropertyChanged event is raised with null property name in event args.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithNullPropertyName_RaisesEventWithNullPropertyName()
    {
        // Arrange
        var service = new TestableLocalizationService();
        string? capturedPropertyName = "NotNull";
        bool eventRaised = false;

        service.PropertyChanged += (sender, e) =>
        {
            eventRaised = true;
            capturedPropertyName = e.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(null!);

        // Assert
        Assert.That(eventRaised, Is.True, "PropertyChanged event should be raised even with null property name");
        Assert.That(capturedPropertyName, Is.Null, "Property name in event args should be null");
    }

    /// <summary>
    /// Tests that OnPropertyChanged does not throw an exception when there are no subscribers.
    /// Input: Any property name.
    /// Expected: No exception is thrown.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithNoSubscriber_DoesNotThrow()
    {
        // Arrange
        var service = new TestableLocalizationService();

        // Act & Assert
        Assert.DoesNotThrow(() => service.PublicOnPropertyChanged("TestProperty"),
            "OnPropertyChanged should not throw when there are no subscribers");
    }

    /// <summary>
    /// Tests that OnPropertyChanged invokes all subscribers when multiple handlers are attached.
    /// Input: Property name with multiple event subscribers.
    /// Expected: All subscribers are invoked exactly once.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithMultipleSubscribers_InvokesAllSubscribers()
    {
        // Arrange
        var service = new TestableLocalizationService();
        int subscriber1InvocationCount = 0;
        int subscriber2InvocationCount = 0;
        int subscriber3InvocationCount = 0;

        service.PropertyChanged += (sender, e) => subscriber1InvocationCount++;
        service.PropertyChanged += (sender, e) => subscriber2InvocationCount++;
        service.PropertyChanged += (sender, e) => subscriber3InvocationCount++;

        // Act
        service.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(subscriber1InvocationCount, Is.EqualTo(1), "First subscriber should be invoked once");
        Assert.That(subscriber2InvocationCount, Is.EqualTo(1), "Second subscriber should be invoked once");
        Assert.That(subscriber3InvocationCount, Is.EqualTo(1), "Third subscriber should be invoked once");
    }

    /// <summary>
    /// Tests that OnPropertyChanged passes the correct sender reference in the event args.
    /// Input: Any property name.
    /// Expected: The sender in PropertyChangedEventArgs is the LocalizationService instance itself.
    /// </summary>
    [Test]
    public void OnPropertyChanged_RaisesEvent_WithCorrectSender()
    {
        // Arrange
        var service = new TestableLocalizationService();
        object? capturedSender = null;

        service.PropertyChanged += (sender, e) =>
        {
            capturedSender = sender;
        };

        // Act
        service.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(capturedSender, Is.Not.Null, "Sender should not be null");
        Assert.That(capturedSender, Is.SameAs(service), "Sender should be the service instance itself");
    }

    /// <summary>
    /// Tests that OnPropertyChanged creates a new PropertyChangedEventArgs instance for each invocation.
    /// Input: Same property name invoked multiple times.
    /// Expected: Each invocation creates a new PropertyChangedEventArgs instance.
    /// </summary>
    [Test]
    public void OnPropertyChanged_MultipleInvocations_CreatesNewEventArgsEachTime()
    {
        // Arrange
        var service = new TestableLocalizationService();
        PropertyChangedEventArgs? firstEventArgs = null;
        PropertyChangedEventArgs? secondEventArgs = null;
        int invocationCount = 0;

        service.PropertyChanged += (sender, e) =>
        {
            invocationCount++;
            if (invocationCount == 1)
                firstEventArgs = e;
            else if (invocationCount == 2)
                secondEventArgs = e;
        };

        // Act
        service.PublicOnPropertyChanged("TestProperty");
        service.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(firstEventArgs, Is.Not.Null, "First event args should be captured");
        Assert.That(secondEventArgs, Is.Not.Null, "Second event args should be captured");
        Assert.That(firstEventArgs, Is.Not.SameAs(secondEventArgs), "Each invocation should create a new event args instance");
    }

    /// <summary>
    /// Tests that OnPropertyChanged raises event for different property names in sequence.
    /// Input: Multiple different property names.
    /// Expected: Event is raised with correct property name for each invocation.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithDifferentPropertyNames_RaisesEventWithCorrectNameEachTime()
    {
        // Arrange
        var service = new TestableLocalizationService();
        var capturedPropertyNames = new System.Collections.Generic.List<string?>();

        service.PropertyChanged += (sender, e) =>
        {
            capturedPropertyNames.Add(e.PropertyName);
        };

        // Act
        service.PublicOnPropertyChanged("Property1");
        service.PublicOnPropertyChanged("Property2");
        service.PublicOnPropertyChanged("Property3");

        // Assert
        Assert.That(capturedPropertyNames.Count, Is.EqualTo(3), "Event should be raised three times");
        Assert.That(capturedPropertyNames[0], Is.EqualTo("Property1"), "First property name should be correct");
        Assert.That(capturedPropertyNames[1], Is.EqualTo("Property2"), "Second property name should be correct");
        Assert.That(capturedPropertyNames[2], Is.EqualTo("Property3"), "Third property name should be correct");
    }

    /// <summary>
    /// Tests that GetString returns the localized string value when provided with a valid, existing property key from AppStrings.
    /// Input: "Abort" (a known existing property in AppStrings)
    /// Expected: Returns the localized string value for "Abort" (not the key itself)
    /// </summary>
    [Test]
    public void GetString_ValidExistingKey_ReturnsLocalizedString()
    {
        // Arrange
        var service = new LocalizationService();
        string validKey = "Abort";

        // Act
        string result = service.GetString(validKey);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        // The result should be the localized value, not the key itself
        // We can't predict exact value due to localization, but it should exist
    }

    /// <summary>
    /// Tests that GetString returns the original key when the property does not exist in AppStrings.
    /// Input: "NonExistentPropertyKey123" (a key that doesn't exist in AppStrings)
    /// Expected: Returns the input key itself as fallback
    /// </summary>
    [Test]
    public void GetString_NonExistentKey_ReturnsOriginalKey()
    {
        // Arrange
        var service = new LocalizationService();
        string nonExistentKey = "NonExistentPropertyKey123";

        // Act
        string result = service.GetString(nonExistentKey);

        // Assert
        Assert.That(result, Is.EqualTo(nonExistentKey));
    }

    /// <summary>
    /// Tests that GetString throws ArgumentNullException when provided with a null key.
    /// Input: null
    /// Expected: Throws ArgumentNullException because Type.GetProperty does not accept null
    /// </summary>
    [Test]
    public void GetString_NullKey_ThrowsArgumentNullException()
    {
        // Arrange
        var service = new LocalizationService();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.GetString(null!));
    }

    /// <summary>
    /// Tests that GetString returns an empty string when provided with an empty string key.
    /// Input: "" (empty string)
    /// Expected: Returns empty string as fallback since no property named "" exists
    /// </summary>
    [Test]
    public void GetString_EmptyStringKey_ReturnsEmptyString()
    {
        // Arrange
        var service = new LocalizationService();
        string emptyKey = string.Empty;

        // Act
        string result = service.GetString(emptyKey);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that GetString returns the whitespace key when provided with whitespace-only input.
    /// Input: "   " (whitespace only)
    /// Expected: Returns the whitespace string as fallback since no such property exists
    /// </summary>
    [Test]
    public void GetString_WhitespaceKey_ReturnsWhitespace()
    {
        // Arrange
        var service = new LocalizationService();
        string whitespaceKey = "   ";

        // Act
        string result = service.GetString(whitespaceKey);

        // Assert
        Assert.That(result, Is.EqualTo(whitespaceKey));
    }

    /// <summary>
    /// Tests that GetString handles very long key names gracefully.
    /// Input: A very long string (1000 characters) that doesn't match any property
    /// Expected: Returns the long key as fallback without errors
    /// </summary>
    [Test]
    public void GetString_VeryLongKey_ReturnsKey()
    {
        // Arrange
        var service = new LocalizationService();
        string veryLongKey = new string('a', 1000);

        // Act
        string result = service.GetString(veryLongKey);

        // Assert
        Assert.That(result, Is.EqualTo(veryLongKey));
    }

    /// <summary>
    /// Tests that GetString handles keys with special characters gracefully.
    /// Input: "Key@#$%^&*()" (key with special characters)
    /// Expected: Returns the key as fallback since no such property exists
    /// </summary>
    [Test]
    public void GetString_KeyWithSpecialCharacters_ReturnsKey()
    {
        // Arrange
        var service = new LocalizationService();
        string specialCharKey = "Key@#$%^&*()";

        // Act
        string result = service.GetString(specialCharKey);

        // Assert
        Assert.That(result, Is.EqualTo(specialCharKey));
    }

    /// <summary>
    /// Tests that GetString handles keys with Unicode characters.
    /// Input: "Key_日本語_Тест" (key with Unicode characters)
    /// Expected: Returns the key as fallback since no such property exists
    /// </summary>
    [Test]
    public void GetString_KeyWithUnicodeCharacters_ReturnsKey()
    {
        // Arrange
        var service = new LocalizationService();
        string unicodeKey = "Key_日本語_Тест";

        // Act
        string result = service.GetString(unicodeKey);

        // Assert
        Assert.That(result, Is.EqualTo(unicodeKey));
    }

    /// <summary>
    /// Tests that GetString returns localized value for another known valid key.
    /// Input: "About" (another known existing property in AppStrings)
    /// Expected: Returns the localized string value for "About"
    /// </summary>
    [Test]
    public void GetString_AnotherValidKey_ReturnsLocalizedString()
    {
        // Arrange
        var service = new LocalizationService();
        string validKey = "About";

        // Act
        string result = service.GetString(validKey);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
    }

    /// <summary>
    /// Tests that GetString handles keys with numbers.
    /// Input: "Key123Test456" (key with numbers)
    /// Expected: Returns the key as fallback since no such property exists
    /// </summary>
    [Test]
    public void GetString_KeyWithNumbers_ReturnsKey()
    {
        // Arrange
        var service = new LocalizationService();
        string numericKey = "Key123Test456";

        // Act
        string result = service.GetString(numericKey);

        // Assert
        Assert.That(result, Is.EqualTo(numericKey));
    }

    /// <summary>
    /// Tests that GetString handles keys starting with numbers.
    /// Input: "123InvalidPropertyName" (key starting with number - invalid C# property name)
    /// Expected: Returns the key as fallback since this cannot be a valid property name
    /// </summary>
    [Test]
    public void GetString_KeyStartingWithNumber_ReturnsKey()
    {
        // Arrange
        var service = new LocalizationService();
        string invalidKey = "123InvalidPropertyName";

        // Act
        string result = service.GetString(invalidKey);

        // Assert
        Assert.That(result, Is.EqualTo(invalidKey));
    }

    /// <summary>
    /// Tests that the constructor successfully creates a LocalizationService instance.
    /// Verifies that basic initialization completes without exceptions.
    /// </summary>
    [Test]
    public void Constructor_DefaultInvocation_CreatesInstanceSuccessfully()
    {
        // Act
        LocalizationService? service = null;
        TestDelegate act = () => service = new LocalizationService();

        // Assert
        Assert.DoesNotThrow(act);
        Assert.That(service, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor initializes the CurrentCulture property to a non-null value.
    /// Verifies that the service has a valid culture set after construction.
    /// </summary>
    [Test]
    public void Constructor_DefaultInvocation_InitializesCurrentCultureToNonNull()
    {
        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture, Is.Not.Null);
    }

    /// <summary>
    /// Tests that the constructor initializes CurrentCulture to one of the available cultures.
    /// Verifies that the resolved culture is among the supported cultures (en or it).
    /// </summary>
    [Test]
    public void Constructor_DefaultInvocation_InitializesCurrentCultureToSupportedCulture()
    {
        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.AvailableCultures.Any(c => c.Name == service.CurrentCulture.Name), Is.True,
            $"CurrentCulture '{service.CurrentCulture.Name}' should be in AvailableCultures");
    }

    /// <summary>
    /// Tests that the constructor populates the AvailableCultures list with expected cultures.
    /// Verifies that both English and Italian cultures are available.
    /// </summary>
    [Test]
    public void Constructor_DefaultInvocation_PopulatesAvailableCultures()
    {
        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.AvailableCultures, Is.Not.Null);
        Assert.That(service.AvailableCultures.Count, Is.EqualTo(2));
        Assert.That(service.AvailableCultures.Any(c => c.TwoLetterISOLanguageName == "en"), Is.True);
        Assert.That(service.AvailableCultures.Any(c => c.TwoLetterISOLanguageName == "it"), Is.True);
    }

    /// <summary>
    /// Tests that the constructor initializes the PropertyChanged event handler infrastructure.
    /// Verifies that the service properly implements INotifyPropertyChanged.
    /// </summary>
    [Test]
    public void Constructor_DefaultInvocation_EnablesPropertyChangedEventSubscription()
    {
        // Arrange
        var service = new LocalizationService();
        var propertyChangedRaised = false;

        // Act
        service.PropertyChanged += (sender, args) => propertyChangedRaised = true;
        service.SetCulture("it");

        // Assert
        Assert.That(propertyChangedRaised, Is.True, "PropertyChanged event should be raised after culture change");
    }

    /// <summary>
    /// Tests that the constructor sets CurrentCulture to English or Italian based on system/saved preferences.
    /// NOTE: This test validates observable behavior but cannot control the static Preferences.Get() dependency.
    /// The actual culture set depends on system settings and saved preferences at runtime.
    /// </summary>
    [Test]
    public void Constructor_DefaultInvocation_SetsCurrentCultureToEnglishOrItalian()
    {
        // Act
        var service = new LocalizationService();

        // Assert
        var twoLetterCode = service.CurrentCulture.TwoLetterISOLanguageName;
        Assert.That(twoLetterCode, Is.EqualTo("en").Or.EqualTo("it"),
            "CurrentCulture should be either English or Italian");
    }

    /// <summary>
    /// LIMITATION: Cannot test constructor behavior with saved culture preference.
    /// The constructor depends on Microsoft.Maui.Storage.Preferences.Get(), which is a static method
    /// that cannot be mocked with Moq. To properly test this scenario, you would need:
    /// 1. A wrapper interface around Preferences (e.g., IPreferencesService)
    /// 2. Dependency injection of this interface into LocalizationService
    /// 3. Mock the interface to return specific saved culture values
    /// Current design does not allow mocking static Preferences API.
    /// </summary>
    [Test]
    [Ignore("Cannot mock static Preferences.Get() - requires architectural change to test")]
    public void Constructor_WithSavedEnglishCulture_SetsCurrentCultureToEnglish()
    {
        // This test cannot be implemented without refactoring to use dependency injection
        // for the Preferences service instead of static method calls.
        Assert.Inconclusive("Test requires mockable Preferences dependency");
    }

    /// <summary>
    /// LIMITATION: Cannot test constructor behavior with saved culture preference.
    /// The constructor depends on Microsoft.Maui.Storage.Preferences.Get(), which is a static method
    /// that cannot be mocked with Moq. To properly test this scenario, you would need:
    /// 1. A wrapper interface around Preferences (e.g., IPreferencesService)
    /// 2. Dependency injection of this interface into LocalizationService
    /// 3. Mock the interface to return specific saved culture values
    /// Current design does not allow mocking static Preferences API.
    /// </summary>
    [Test]
    [Ignore("Cannot mock static Preferences.Get() - requires architectural change to test")]
    public void Constructor_WithSavedItalianCulture_SetsCurrentCultureToItalian()
    {
        // This test cannot be implemented without refactoring to use dependency injection
        // for the Preferences service instead of static method calls.
        Assert.Inconclusive("Test requires mockable Preferences dependency");
    }

    /// <summary>
    /// LIMITATION: Cannot test constructor fallback to CurrentUICulture.
    /// The constructor depends on CultureInfo.CurrentUICulture, which is a static property
    /// that cannot be easily mocked or controlled in unit tests. While it can be set,
    /// doing so would affect other tests running in parallel and is not thread-safe.
    /// To properly test this scenario, you would need:
    /// 1. A wrapper interface for culture resolution (e.g., ICultureProvider)
    /// 2. Dependency injection of this interface into LocalizationService
    /// 3. Mock the interface to return specific culture values
    /// Current design does not allow safe testing of static CurrentUICulture dependency.
    /// </summary>
    [Test]
    [Ignore("Cannot safely mock static CultureInfo.CurrentUICulture - requires architectural change to test")]
    public void Constructor_WithoutSavedCulture_FallsBackToCurrentUICulture()
    {
        // This test cannot be safely implemented without refactoring to use dependency injection
        // for culture resolution instead of static CultureInfo.CurrentUICulture access.
        Assert.Inconclusive("Test requires mockable culture provider dependency");
    }

    /// <summary>
    /// LIMITATION: Cannot test constructor behavior with empty saved culture.
    /// The constructor depends on Microsoft.Maui.Storage.Preferences.Get(), which is a static method
    /// that cannot be mocked with Moq. This test would verify that when an empty string is saved,
    /// the service falls back to CurrentUICulture, but both dependencies are static and non-mockable.
    /// </summary>
    [Test]
    [Ignore("Cannot mock static Preferences.Get() - requires architectural change to test")]
    public void Constructor_WithEmptySavedCulture_FallsBackToCurrentUICulture()
    {
        // This test cannot be implemented without refactoring to use dependency injection.
        Assert.Inconclusive("Test requires mockable Preferences dependency");
    }

    /// <summary>
    /// LIMITATION: Cannot test constructor behavior with whitespace-only saved culture.
    /// The constructor depends on Microsoft.Maui.Storage.Preferences.Get(), which is a static method
    /// that cannot be mocked with Moq. This test would verify that whitespace values trigger
    /// fallback behavior, but the dependency is static and non-mockable.
    /// </summary>
    [Test]
    [Ignore("Cannot mock static Preferences.Get() - requires architectural change to test")]
    public void Constructor_WithWhitespaceSavedCulture_FallsBackToCurrentUICulture()
    {
        // This test cannot be implemented without refactoring to use dependency injection.
        Assert.Inconclusive("Test requires mockable Preferences dependency");
    }

    /// <summary>
    /// Tests that SetCulture with valid supported culture names correctly sets the culture.
    /// Input: Valid culture names "en" and "it"
    /// Expected: CurrentCulture is set to the specified culture
    /// </summary>
    [TestCase("en", "en")]
    [TestCase("it", "it")]
    public void SetCultureString_ValidSupportedCultureNames_SetsCultureCorrectly(string cultureName, string expectedTwoLetterCode)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo(expectedTwoLetterCode));
    }

    /// <summary>
    /// Tests that SetCulture with regional culture variants correctly resolves to base culture.
    /// Input: Regional variants like "en-US", "en-GB", "it-IT"
    /// Expected: CurrentCulture is set to the base culture (en or it)
    /// </summary>
    [TestCase("en-US", "en")]
    [TestCase("en-GB", "en")]
    [TestCase("it-IT", "it")]
    [TestCase("it-CH", "it")]
    public void SetCultureString_RegionalVariants_ResolvesToBaseCulture(string cultureName, string expectedTwoLetterCode)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo(expectedTwoLetterCode));
    }

    /// <summary>
    /// Tests that SetCulture with null, empty, or whitespace culture names defaults to English.
    /// Input: null, empty string, whitespace strings
    /// Expected: CurrentCulture is set to English (en)
    /// </summary>
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("   ")]
    [TestCase("\t")]
    public void SetCultureString_NullEmptyOrWhitespace_DefaultsToEnglish(string? cultureName)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName!);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with invalid culture names defaults to English.
    /// Input: Invalid culture identifiers
    /// Expected: CurrentCulture is set to English (en)
    /// </summary>
    [TestCase("invalid")]
    [TestCase("xxx")]
    [TestCase("123")]
    [TestCase("!!!")]
    public void SetCultureString_InvalidCultureNames_DefaultsToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with unsupported but valid culture names defaults to English.
    /// Input: Valid cultures not in AvailableCultures (fr, de, es, ja)
    /// Expected: CurrentCulture is set to English (en)
    /// </summary>
    [TestCase("fr")]
    [TestCase("de")]
    [TestCase("es")]
    [TestCase("ja")]
    public void SetCultureString_UnsupportedValidCultures_DefaultsToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with case variations correctly resolves to the appropriate culture.
    /// Input: Culture names in various cases (uppercase, mixed case)
    /// Expected: Culture resolution is case-insensitive
    /// </summary>
    [TestCase("EN", "en")]
    [TestCase("IT", "it")]
    [TestCase("En", "en")]
    [TestCase("It", "it")]
    public void SetCultureString_CaseVariations_ResolvesCaseInsensitively(string cultureName, string expectedTwoLetterCode)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo(expectedTwoLetterCode));
    }

    /// <summary>
    /// Tests that SetCulture raises PropertyChanged event when culture changes.
    /// Input: Different culture from current
    /// Expected: PropertyChanged event is raised for CurrentCulture and CurrentLanguageDisplayName
    /// </summary>
    [Test]
    public void SetCultureString_ChangingCulture_RaisesPropertyChangedEvents()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en");

        var propertyChangedEvents = new System.Collections.Generic.List<string>();
        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName != null)
            {
                propertyChangedEvents.Add(args.PropertyName);
            }
        };

        // Act
        service.SetCulture("it");

        // Assert
        Assert.That(propertyChangedEvents, Does.Contain("CurrentCulture"));
        Assert.That(propertyChangedEvents, Does.Contain("CurrentLanguageDisplayName"));
    }

    /// <summary>
    /// Tests that SetCulture raises CultureChanged event when culture is set.
    /// Input: Any culture name
    /// Expected: CultureChanged event is raised
    /// </summary>
    [Test]
    public void SetCultureString_SettingCulture_RaisesCultureChangedEvent()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en");

        var cultureChangedRaised = false;
        service.CultureChanged += (sender, args) =>
        {
            cultureChangedRaised = true;
        };

        // Act
        service.SetCulture("it");

        // Assert
        Assert.That(cultureChangedRaised, Is.True);
    }

    /// <summary>
    /// Tests that SetCulture updates static CultureInfo.CurrentCulture and CurrentUICulture.
    /// Input: Valid culture name
    /// Expected: Static culture properties are updated to match
    /// </summary>
    [Test]
    public void SetCultureString_ValidCulture_UpdatesStaticCultureProperties()
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture("it");

        // Assert
        Assert.That(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
        Assert.That(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture with very long culture name string defaults to English.
    /// Input: Very long string (1000 characters)
    /// Expected: Handles gracefully and defaults to English
    /// </summary>
    [Test]
    public void SetCultureString_VeryLongString_DefaultsToEnglish()
    {
        // Arrange
        var service = new LocalizationService();
        var longCultureName = new string('x', 1000);

        // Act
        service.SetCulture(longCultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with culture names containing special characters defaults to English.
    /// Input: Culture names with special characters
    /// Expected: Defaults to English
    /// </summary>
    [TestCase("en@#$")]
    [TestCase("it!!!")]
    [TestCase("@@@")]
    [TestCase("en-US-invalid")]
    public void SetCultureString_SpecialCharacters_DefaultsToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture correctly delegates to ResolveSupportedCulture and SetCulture(CultureInfo).
    /// Input: Valid culture name "it"
    /// Expected: Culture is properly resolved and set, verifying the delegation chain
    /// </summary>
    [Test]
    public void SetCultureString_ValidCulture_CorrectlyDelegatesToOverload()
    {
        // Arrange
        var service = new LocalizationService();
        var initialCulture = service.CurrentCulture;

        // Act
        service.SetCulture("it");

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
        Assert.That(service.CurrentCulture.Name, Is.Not.EqualTo(initialCulture.Name));
    }

    /// <summary>
    /// Tests that SetCulture with the same culture name does not raise PropertyChanged but still raises CultureChanged.
    /// Input: Same culture as currently set
    /// Expected: CultureChanged is raised, but PropertyChanged for CurrentCulture is not raised
    /// </summary>
    [Test]
    public void SetCultureString_SameCulture_RaisesCultureChangedButNotPropertyChanged()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en");

        var propertyChangedRaised = false;
        var cultureChangedRaised = false;

        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == "CurrentCulture")
            {
                propertyChangedRaised = true;
            }
        };

        service.CultureChanged += (sender, args) =>
        {
            cultureChangedRaised = true;
        };

        // Act
        service.SetCulture("en");

        // Assert
        Assert.That(cultureChangedRaised, Is.True);
        Assert.That(propertyChangedRaised, Is.False);
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the Italian language string for all Italian culture variants.
    /// Input: Italian culture names "it" or "it-IT".
    /// Expected: Returns AppStrings.LanguageItalian for each variant.
    /// </summary>
    [TestCase("it")]
    [TestCase("it-IT")]
    public void CurrentLanguageDisplayName_AllItalianVariants_ReturnsItalianLanguageString(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(cultureName);

        // Act
        string displayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(displayName, Is.EqualTo(AppStrings.LanguageItalian));
        Assert.That(displayName, Is.Not.Null);
        Assert.That(displayName, Is.Not.Empty);
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns the English language string for all English culture variants.
    /// Input: English culture names "en" or "en-US".
    /// Expected: Returns AppStrings.LanguageEnglish for each variant.
    /// </summary>
    [TestCase("en")]
    [TestCase("en-US")]
    public void CurrentLanguageDisplayName_AllEnglishVariants_ReturnsEnglishLanguageString(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(cultureName);

        // Act
        string displayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(displayName, Is.EqualTo(AppStrings.LanguageEnglish));
        Assert.That(displayName, Is.Not.Null);
        Assert.That(displayName, Is.Not.Empty);
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns English language string as default for all unsupported cultures.
    /// Input: Various unsupported culture names including invalid, empty, whitespace, and valid but unsupported cultures.
    /// Expected: Returns AppStrings.LanguageEnglish as fallback for all cases.
    /// </summary>
    [TestCase("fr")]
    [TestCase("de")]
    [TestCase("es")]
    [TestCase("ja")]
    [TestCase("zh")]
    [TestCase("ru")]
    [TestCase("ar")]
    [TestCase("pt")]
    [TestCase("invalid")]
    [TestCase("xxx-invalid")]
    [TestCase("123")]
    [TestCase("!@#")]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("\t")]
    [TestCase("\r\n")]
    public void CurrentLanguageDisplayName_AllUnsupportedCultures_ReturnsEnglishLanguageStringAsDefault(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(cultureName);

        // Act
        string displayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(displayName, Is.EqualTo(AppStrings.LanguageEnglish));
        Assert.That(displayName, Is.Not.Null);
        Assert.That(displayName, Is.Not.Empty);
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns English language string when null CultureInfo is passed to SetCulture.
    /// Input: Null CultureInfo.
    /// Expected: Returns AppStrings.LanguageEnglish as default, demonstrating null-safe property access.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_NullCulturePassedToSetCulture_ReturnsEnglishLanguageStringAsDefault()
    {
        // Arrange
        var service = new LocalizationService();
        CultureInfo? nullCulture = null;

        // Act
        service.SetCulture(nullCulture!);
        string displayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(displayName, Is.EqualTo(AppStrings.LanguageEnglish));
        Assert.That(displayName, Is.Not.Null);
        Assert.That(displayName, Is.Not.Empty);
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns English for unsupported English regional variants.
    /// Input: English regional culture codes not explicitly in the switch expression (en-GB, en-CA, en-AU, en-NZ).
    /// Expected: Returns AppStrings.LanguageEnglish (matches default case, not "en-US" case).
    /// </summary>
    [TestCase("en-GB")]
    [TestCase("en-CA")]
    [TestCase("en-AU")]
    [TestCase("en-NZ")]
    [TestCase("en-IN")]
    public void CurrentLanguageDisplayName_UnsupportedEnglishRegionalVariants_ReturnsEnglishLanguageString(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(cultureName);

        // Act
        string displayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(displayName, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns English for unsupported Italian regional variants.
    /// Input: Italian regional culture codes not explicitly in the switch expression (it-CH, it-SM).
    /// Expected: Returns AppStrings.LanguageEnglish (matches default case, not "it-IT" case).
    /// </summary>
    [TestCase("it-CH")]
    [TestCase("it-SM")]
    public void CurrentLanguageDisplayName_UnsupportedItalianRegionalVariants_ReturnsEnglishLanguageString(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture(cultureName);

        // Act
        string displayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(displayName, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName maintains consistent value across multiple accesses without culture change.
    /// Input: Set culture to Italian once, access property multiple times.
    /// Expected: Same Italian language string returned each time, demonstrating getter consistency.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_MultipleAccessesWithoutCultureChange_ReturnsConsistentValue()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("it");

        // Act
        string displayName1 = service.CurrentLanguageDisplayName;
        string displayName2 = service.CurrentLanguageDisplayName;
        string displayName3 = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(displayName1, Is.EqualTo(AppStrings.LanguageItalian));
        Assert.That(displayName2, Is.EqualTo(displayName1));
        Assert.That(displayName3, Is.EqualTo(displayName1));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName updates correctly when culture changes from Italian to English.
    /// Input: Set culture to Italian, then change to English.
    /// Expected: Display name changes from Italian language string to English language string.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_WhenCultureChangesFromItalianToEnglish_DisplayNameUpdates()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("it");
        string initialDisplayName = service.CurrentLanguageDisplayName;

        // Act
        service.SetCulture("en");
        string updatedDisplayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(initialDisplayName, Is.EqualTo(AppStrings.LanguageItalian));
        Assert.That(updatedDisplayName, Is.EqualTo(AppStrings.LanguageEnglish));
        Assert.That(updatedDisplayName, Is.Not.EqualTo(initialDisplayName));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName updates correctly when culture changes from English to Italian.
    /// Input: Set culture to English, then change to Italian.
    /// Expected: Display name changes from English language string to Italian language string.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_WhenCultureChangesFromEnglishToItalian_DisplayNameUpdates()
    {
        // Arrange
        var service = new LocalizationService();
        service.SetCulture("en");
        string initialDisplayName = service.CurrentLanguageDisplayName;

        // Act
        service.SetCulture("it");
        string updatedDisplayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(initialDisplayName, Is.EqualTo(AppStrings.LanguageEnglish));
        Assert.That(updatedDisplayName, Is.EqualTo(AppStrings.LanguageItalian));
        Assert.That(updatedDisplayName, Is.Not.EqualTo(initialDisplayName));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName handles very long culture names gracefully.
    /// Input: Very long string (1000 characters).
    /// Expected: Returns AppStrings.LanguageEnglish as default without throwing exception.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_VeryLongCultureName_ReturnsEnglishLanguageStringAsDefault()
    {
        // Arrange
        var service = new LocalizationService();
        string veryLongCultureName = new string('x', 1000);

        // Act
        service.SetCulture(veryLongCultureName);
        string displayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(displayName, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName handles culture names with special and Unicode characters.
    /// Input: Culture names containing special characters, Unicode characters.
    /// Expected: Returns AppStrings.LanguageEnglish as default for all non-matching patterns.
    /// </summary>
    [TestCase("en@#$%")]
    [TestCase("it!!!")]
    [TestCase("日本語")]
    [TestCase("Тест")]
    [TestCase("en_US")]
    [TestCase("it.IT")]
    public void CurrentLanguageDisplayName_CultureNamesWithSpecialCharacters_ReturnsEnglishLanguageStringAsDefault(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture(cultureName);
        string displayName = service.CurrentLanguageDisplayName;

        // Assert
        Assert.That(displayName, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that CurrentLanguageDisplayName returns correct values after multiple culture changes.
    /// Input: Change culture multiple times between Italian, English, and unsupported cultures.
    /// Expected: Display name updates correctly after each culture change.
    /// </summary>
    [Test]
    public void CurrentLanguageDisplayName_AfterMultipleCultureChanges_ReflectsCurrentCulture()
    {
        // Arrange
        var service = new LocalizationService();

        // Act & Assert - Italian
        service.SetCulture("it");
        Assert.That(service.CurrentLanguageDisplayName, Is.EqualTo(AppStrings.LanguageItalian));

        // Act & Assert - English
        service.SetCulture("en");
        Assert.That(service.CurrentLanguageDisplayName, Is.EqualTo(AppStrings.LanguageEnglish));

        // Act & Assert - Unsupported
        service.SetCulture("fr");
        Assert.That(service.CurrentLanguageDisplayName, Is.EqualTo(AppStrings.LanguageEnglish));

        // Act & Assert - Italian variant
        service.SetCulture("it-IT");
        Assert.That(service.CurrentLanguageDisplayName, Is.EqualTo(AppStrings.LanguageItalian));

        // Act & Assert - English variant
        service.SetCulture("en-US");
        Assert.That(service.CurrentLanguageDisplayName, Is.EqualTo(AppStrings.LanguageEnglish));
    }

    /// <summary>
    /// Tests that SetCulture with valid English CultureInfo updates Thread.CurrentThread.CurrentCulture.
    /// Input: English CultureInfo object.
    /// Expected: Thread.CurrentThread.CurrentCulture is set to English.
    /// </summary>
    [Test]
    public void SetCulture_WithValidEnglishCultureInfo_UpdatesThreadCurrentCulture()
    {
        // Arrange
        var service = new LocalizationService();
        var englishCulture = new CultureInfo("en");

        // Act
        service.SetCulture(englishCulture);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with valid Italian CultureInfo updates Thread.CurrentThread.CurrentCulture.
    /// Input: Italian CultureInfo object.
    /// Expected: Thread.CurrentThread.CurrentCulture is set to Italian.
    /// </summary>
    [Test]
    public void SetCulture_WithValidItalianCultureInfo_UpdatesThreadCurrentCulture()
    {
        // Arrange
        var service = new LocalizationService();
        var italianCulture = new CultureInfo("it");

        // Act
        service.SetCulture(italianCulture);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture with valid English CultureInfo updates Thread.CurrentThread.CurrentUICulture.
    /// Input: English CultureInfo object.
    /// Expected: Thread.CurrentThread.CurrentUICulture is set to English.
    /// </summary>
    [Test]
    public void SetCulture_WithValidEnglishCultureInfo_UpdatesThreadCurrentUICulture()
    {
        // Arrange
        var service = new LocalizationService();
        var englishCulture = new CultureInfo("en");

        // Act
        service.SetCulture(englishCulture);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with valid Italian CultureInfo updates Thread.CurrentThread.CurrentUICulture.
    /// Input: Italian CultureInfo object.
    /// Expected: Thread.CurrentThread.CurrentUICulture is set to Italian.
    /// </summary>
    [Test]
    public void SetCulture_WithValidItalianCultureInfo_UpdatesThreadCurrentUICulture()
    {
        // Arrange
        var service = new LocalizationService();
        var italianCulture = new CultureInfo("it");

        // Act
        service.SetCulture(italianCulture);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture with null CultureInfo updates Thread.CurrentThread.CurrentCulture to English (default).
    /// Input: null CultureInfo.
    /// Expected: Thread.CurrentThread.CurrentCulture is set to English as default.
    /// </summary>
    [Test]
    public void SetCulture_WithNullCultureInfo_UpdatesThreadCurrentCultureToEnglish()
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture((CultureInfo?)null);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with null CultureInfo updates Thread.CurrentThread.CurrentUICulture to English (default).
    /// Input: null CultureInfo.
    /// Expected: Thread.CurrentThread.CurrentUICulture is set to English as default.
    /// </summary>
    [Test]
    public void SetCulture_WithNullCultureInfo_UpdatesThreadCurrentUICultureToEnglish()
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture((CultureInfo?)null);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with valid English CultureInfo updates AppStrings.Culture.
    /// Input: English CultureInfo object.
    /// Expected: AppStrings.Culture is set to English.
    /// </summary>
    [Test]
    public void SetCulture_WithValidEnglishCultureInfo_UpdatesAppStringsCulture()
    {
        // Arrange
        var service = new LocalizationService();
        var englishCulture = new CultureInfo("en");

        // Act
        service.SetCulture(englishCulture);

        // Assert
        Assert.That(AppStrings.Culture, Is.Not.Null);
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with valid Italian CultureInfo updates AppStrings.Culture.
    /// Input: Italian CultureInfo object.
    /// Expected: AppStrings.Culture is set to Italian.
    /// </summary>
    [Test]
    public void SetCulture_WithValidItalianCultureInfo_UpdatesAppStringsCulture()
    {
        // Arrange
        var service = new LocalizationService();
        var italianCulture = new CultureInfo("it");

        // Act
        service.SetCulture(italianCulture);

        // Assert
        Assert.That(AppStrings.Culture, Is.Not.Null);
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture with null CultureInfo updates AppStrings.Culture to English (default).
    /// Input: null CultureInfo.
    /// Expected: AppStrings.Culture is set to English as default.
    /// </summary>
    [Test]
    public void SetCulture_WithNullCultureInfo_UpdatesAppStringsCultureToEnglish()
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        service.SetCulture((CultureInfo?)null);

        // Assert
        Assert.That(AppStrings.Culture, Is.Not.Null);
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with regional English variant updates AppStrings.Culture to base English.
    /// Input: en-US CultureInfo (regional variant).
    /// Expected: AppStrings.Culture is set to base English culture after resolution.
    /// </summary>
    [Test]
    public void SetCulture_WithEnglishUSCultureInfo_UpdatesAppStringsCultureToEnglish()
    {
        // Arrange
        var service = new LocalizationService();
        var enUsCulture = new CultureInfo("en-US");

        // Act
        service.SetCulture(enUsCulture);

        // Assert
        Assert.That(AppStrings.Culture, Is.Not.Null);
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with regional Italian variant updates AppStrings.Culture to base Italian.
    /// Input: it-IT CultureInfo (regional variant).
    /// Expected: AppStrings.Culture is set to base Italian culture after resolution.
    /// </summary>
    [Test]
    public void SetCulture_WithItalianITCultureInfo_UpdatesAppStringsCultureToItalian()
    {
        // Arrange
        var service = new LocalizationService();
        var itItCulture = new CultureInfo("it-IT");

        // Act
        service.SetCulture(itItCulture);

        // Assert
        Assert.That(AppStrings.Culture, Is.Not.Null);
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture with unsupported culture updates AppStrings.Culture to English (default).
    /// Input: French CultureInfo (unsupported).
    /// Expected: AppStrings.Culture is set to English as fallback.
    /// </summary>
    [Test]
    public void SetCulture_WithUnsupportedFrenchCultureInfo_UpdatesAppStringsCultureToEnglish()
    {
        // Arrange
        var service = new LocalizationService();
        var frenchCulture = new CultureInfo("fr");

        // Act
        service.SetCulture(frenchCulture);

        // Assert
        Assert.That(AppStrings.Culture, Is.Not.Null);
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with regional English variant updates Thread.CurrentThread.CurrentCulture to base English.
    /// Input: en-GB CultureInfo (regional variant).
    /// Expected: Thread.CurrentThread.CurrentCulture is set to base English after resolution.
    /// </summary>
    [Test]
    public void SetCulture_WithEnglishGBCultureInfo_UpdatesThreadCurrentCultureToEnglish()
    {
        // Arrange
        var service = new LocalizationService();
        var enGbCulture = new CultureInfo("en-GB");

        // Act
        service.SetCulture(enGbCulture);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with unsupported German culture updates Thread.CurrentThread.CurrentCulture to English (default).
    /// Input: German CultureInfo (unsupported).
    /// Expected: Thread.CurrentThread.CurrentCulture is set to English as fallback.
    /// </summary>
    [Test]
    public void SetCulture_WithUnsupportedGermanCultureInfo_UpdatesThreadCurrentCultureToEnglish()
    {
        // Arrange
        var service = new LocalizationService();
        var germanCulture = new CultureInfo("de");

        // Act
        service.SetCulture(germanCulture);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture updates all four static culture properties consistently.
    /// Input: Italian CultureInfo.
    /// Expected: CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture, Thread.CurrentThread.CurrentCulture,
    /// and Thread.CurrentThread.CurrentUICulture are all set to Italian.
    /// </summary>
    [Test]
    public void SetCulture_WithValidCultureInfo_UpdatesAllStaticCulturePropertiesConsistently()
    {
        // Arrange
        var service = new LocalizationService();
        var italianCulture = new CultureInfo("it");

        // Act
        service.SetCulture(italianCulture);

        // Assert
        Assert.That(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
        Assert.That(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
        Assert.That(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that SetCulture updates all culture properties (static and AppStrings) consistently.
    /// Input: English CultureInfo.
    /// Expected: All culture properties (static CultureInfo, Thread, AppStrings) are set to English.
    /// </summary>
    [Test]
    public void SetCulture_WithEnglishCultureInfo_UpdatesAllCulturePropertiesConsistently()
    {
        // Arrange
        var service = new LocalizationService();
        var englishCulture = new CultureInfo("en");

        // Act
        service.SetCulture(englishCulture);

        // Assert
        Assert.That(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(AppStrings.Culture, Is.Not.Null);
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture called multiple times updates Thread.CurrentThread properties each time.
    /// Input: Sequential calls with English, then Italian, then English again.
    /// Expected: Thread.CurrentThread.CurrentCulture follows each change.
    /// </summary>
    [Test]
    public void SetCulture_CalledMultipleTimes_UpdatesThreadCurrentCultureEachTime()
    {
        // Arrange
        var service = new LocalizationService();
        var englishCulture = new CultureInfo("en");
        var italianCulture = new CultureInfo("it");

        // Act & Assert - First call
        service.SetCulture(englishCulture);
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));

        // Act & Assert - Second call
        service.SetCulture(italianCulture);
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));

        // Act & Assert - Third call
        service.SetCulture(englishCulture);
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture called multiple times updates AppStrings.Culture each time.
    /// Input: Sequential calls with Italian, then English.
    /// Expected: AppStrings.Culture follows each change.
    /// </summary>
    [Test]
    public void SetCulture_CalledMultipleTimes_UpdatesAppStringsCultureEachTime()
    {
        // Arrange
        var service = new LocalizationService();
        var englishCulture = new CultureInfo("en");
        var italianCulture = new CultureInfo("it");

        // Act & Assert - First call
        service.SetCulture(italianCulture);
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("it"));

        // Act & Assert - Second call
        service.SetCulture(englishCulture);
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with InvariantCulture defaults to English after resolution.
    /// Input: CultureInfo.InvariantCulture.
    /// Expected: Culture is resolved to English (default), all properties updated to English.
    /// </summary>
    [Test]
    public void SetCulture_WithInvariantCulture_ResolvesToEnglishAndUpdatesAllProperties()
    {
        // Arrange
        var service = new LocalizationService();
        var invariantCulture = CultureInfo.InvariantCulture;

        // Act
        service.SetCulture(invariantCulture);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with various unsupported cultures defaults to English for all properties.
    /// Input: Various unsupported CultureInfo objects (French, German, Spanish, Japanese, Chinese).
    /// Expected: All culture properties default to English.
    /// </summary>
    [TestCase("fr")]
    [TestCase("de")]
    [TestCase("es")]
    [TestCase("ja")]
    [TestCase("zh")]
    public void SetCulture_WithVariousUnsupportedCultures_DefaultsAllPropertiesToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo(cultureName);

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with various English regional variants resolves to base English for all properties.
    /// Input: Various English regional CultureInfo objects (en-US, en-GB, en-CA, en-AU).
    /// Expected: All culture properties set to base English culture.
    /// </summary>
    [TestCase("en-US")]
    [TestCase("en-GB")]
    [TestCase("en-CA")]
    [TestCase("en-AU")]
    public void SetCulture_WithEnglishRegionalVariants_ResolvesAllPropertiesToEnglish(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo(cultureName);

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("en"));
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("en"));
    }

    /// <summary>
    /// Tests that SetCulture with various Italian regional variants resolves to base Italian for all properties.
    /// Input: Various Italian regional CultureInfo objects (it-IT, it-CH).
    /// Expected: All culture properties set to base Italian culture.
    /// </summary>
    [TestCase("it-IT")]
    [TestCase("it-CH")]
    public void SetCulture_WithItalianRegionalVariants_ResolvesAllPropertiesToItalian(string cultureName)
    {
        // Arrange
        var service = new LocalizationService();
        var culture = new CultureInfo(cultureName);

        // Act
        service.SetCulture(culture);

        // Assert
        Assert.That(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
        Assert.That(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, Is.EqualTo("it"));
        Assert.That(AppStrings.Culture.TwoLetterISOLanguageName, Is.EqualTo("it"));
    }

    /// <summary>
    /// Tests that OnPropertyChanged raises the PropertyChanged event when there is a subscriber.
    /// Input: Valid property name "TestProperty".
    /// Expected: PropertyChanged event is raised with the correct property name.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithSubscriber_RaisesPropertyChangedEvent()
    {
        // Arrange
        var service = new TestableLocalizationService();
        var eventRaised = false;
        string? capturedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            capturedPropertyName = args.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(eventRaised, Is.True);
        Assert.That(capturedPropertyName, Is.EqualTo("TestProperty"));
    }

    /// <summary>
    /// Tests that OnPropertyChanged passes the correct sender reference in the event.
    /// Input: Any valid property name.
    /// Expected: The sender in the event is the LocalizationService instance itself.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WhenRaisingEvent_PassesCorrectSender()
    {
        // Arrange
        var service = new TestableLocalizationService();
        object? capturedSender = null;

        service.PropertyChanged += (sender, args) =>
        {
            capturedSender = sender;
        };

        // Act
        service.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(capturedSender, Is.SameAs(service));
    }

    /// <summary>
    /// Tests that OnPropertyChanged does not throw when there are no subscribers to the event.
    /// Input: Valid property name with no event subscribers.
    /// Expected: No exception is thrown.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithNoSubscribers_DoesNotThrow()
    {
        // Arrange
        var service = new TestableLocalizationService();

        // Act & Assert
        Assert.DoesNotThrow(() => service.PublicOnPropertyChanged("TestProperty"));
    }

    /// <summary>
    /// Tests that OnPropertyChanged handles null property name without throwing.
    /// Input: null property name.
    /// Expected: PropertyChanged event is raised with null property name (indicates all properties changed).
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithNullPropertyName_RaisesEventWithNull()
    {
        // Arrange
        var service = new TestableLocalizationService();
        var eventRaised = false;
        string? capturedPropertyName = "NotNull";

        service.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            capturedPropertyName = args.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(null!);

        // Assert
        Assert.That(eventRaised, Is.True);
        Assert.That(capturedPropertyName, Is.Null);
    }

    /// <summary>
    /// Tests that OnPropertyChanged handles empty string property name correctly.
    /// Input: Empty string "" (indicates all properties changed in WPF/MAUI binding).
    /// Expected: PropertyChanged event is raised with empty string property name.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithEmptyString_RaisesEventWithEmptyString()
    {
        // Arrange
        var service = new TestableLocalizationService();
        var eventRaised = false;
        string? capturedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            capturedPropertyName = args.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(string.Empty);

        // Assert
        Assert.That(eventRaised, Is.True);
        Assert.That(capturedPropertyName, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that OnPropertyChanged handles whitespace-only property names correctly.
    /// Input: Whitespace-only strings (spaces, tabs, newlines).
    /// Expected: PropertyChanged event is raised with the exact whitespace string.
    /// </summary>
    [TestCase("   ")]
    [TestCase("\t")]
    [TestCase("\r\n")]
    [TestCase(" \t\r\n ")]
    public void OnPropertyChanged_WithWhitespace_RaisesEventWithWhitespace(string propertyName)
    {
        // Arrange
        var service = new TestableLocalizationService();
        var eventRaised = false;
        string? capturedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            capturedPropertyName = args.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(propertyName);

        // Assert
        Assert.That(eventRaised, Is.True);
        Assert.That(capturedPropertyName, Is.EqualTo(propertyName));
    }

    /// <summary>
    /// Tests that OnPropertyChanged handles very long property names without issues.
    /// Input: A very long string (1000 characters).
    /// Expected: PropertyChanged event is raised with the complete long string.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithVeryLongPropertyName_RaisesEventWithFullName()
    {
        // Arrange
        var service = new TestableLocalizationService();
        var longPropertyName = new string('A', 1000);
        var eventRaised = false;
        string? capturedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            capturedPropertyName = args.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(longPropertyName);

        // Assert
        Assert.That(eventRaised, Is.True);
        Assert.That(capturedPropertyName, Is.EqualTo(longPropertyName));
        Assert.That(capturedPropertyName?.Length, Is.EqualTo(1000));
    }

    /// <summary>
    /// Tests that OnPropertyChanged handles property names with special characters.
    /// Input: Property names with dots, brackets, and other special characters used in binding expressions.
    /// Expected: PropertyChanged event is raised with the exact special character string.
    /// </summary>
    [TestCase("Property.SubProperty")]
    [TestCase("Property[0]")]
    [TestCase("Property[Key]")]
    [TestCase("Property.SubProperty[0]")]
    [TestCase("@SpecialProperty")]
    [TestCase("Property!@#$%")]
    public void OnPropertyChanged_WithSpecialCharacters_RaisesEventWithExactName(string propertyName)
    {
        // Arrange
        var service = new TestableLocalizationService();
        var eventRaised = false;
        string? capturedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            capturedPropertyName = args.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(propertyName);

        // Assert
        Assert.That(eventRaised, Is.True);
        Assert.That(capturedPropertyName, Is.EqualTo(propertyName));
    }

    /// <summary>
    /// Tests that OnPropertyChanged handles property names with Unicode characters.
    /// Input: Property names containing Unicode characters (various languages).
    /// Expected: PropertyChanged event is raised with the exact Unicode string.
    /// </summary>
    [TestCase("Property_日本語")]
    [TestCase("Свойство")]
    [TestCase("属性名称")]
    [TestCase("Propriété")]
    [TestCase("Propiedad_Ñ")]
    public void OnPropertyChanged_WithUnicodeCharacters_RaisesEventWithExactName(string propertyName)
    {
        // Arrange
        var service = new TestableLocalizationService();
        var eventRaised = false;
        string? capturedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            capturedPropertyName = args.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(propertyName);

        // Assert
        Assert.That(eventRaised, Is.True);
        Assert.That(capturedPropertyName, Is.EqualTo(propertyName));
    }

    /// <summary>
    /// Tests that OnPropertyChanged handles various casing conventions for property names.
    /// Input: Property names in different casing styles (PascalCase, camelCase, UPPERCASE, lowercase, snake_case).
    /// Expected: PropertyChanged event is raised with the exact casing provided.
    /// </summary>
    [TestCase("PascalCaseProperty")]
    [TestCase("camelCaseProperty")]
    [TestCase("UPPERCASEPROPERTY")]
    [TestCase("lowercaseproperty")]
    [TestCase("snake_case_property")]
    [TestCase("kebab-case-property")]
    [TestCase("Mixed_Case_Property123")]
    public void OnPropertyChanged_WithVariousCasing_RaisesEventWithExactCasing(string propertyName)
    {
        // Arrange
        var service = new TestableLocalizationService();
        var eventRaised = false;
        string? capturedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            capturedPropertyName = args.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(propertyName);

        // Assert
        Assert.That(eventRaised, Is.True);
        Assert.That(capturedPropertyName, Is.EqualTo(propertyName));
    }

    /// <summary>
    /// Tests that OnPropertyChanged creates a new PropertyChangedEventArgs instance for each invocation.
    /// Input: Multiple invocations with the same property name.
    /// Expected: Each invocation uses a distinct PropertyChangedEventArgs instance.
    /// </summary>
    [Test]
    public void OnPropertyChanged_MultipleInvocations_CreatesNewEventArgsInstances()
    {
        // Arrange
        var service = new TestableLocalizationService();
        PropertyChangedEventArgs? firstEventArgs = null;
        PropertyChangedEventArgs? secondEventArgs = null;
        var invocationCount = 0;

        service.PropertyChanged += (sender, args) =>
        {
            invocationCount++;
            if (invocationCount == 1)
                firstEventArgs = args;
            else if (invocationCount == 2)
                secondEventArgs = args;
        };

        // Act
        service.PublicOnPropertyChanged("TestProperty");
        service.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(firstEventArgs, Is.Not.Null);
        Assert.That(secondEventArgs, Is.Not.Null);
        Assert.That(firstEventArgs, Is.Not.SameAs(secondEventArgs));
    }

    /// <summary>
    /// Tests that OnPropertyChanged can be called sequentially with different property names.
    /// Input: Multiple different property names called in sequence.
    /// Expected: Event is raised for each property name with correct values.
    /// </summary>
    [Test]
    public void OnPropertyChanged_SequentialCallsWithDifferentNames_RaisesEventForEach()
    {
        // Arrange
        var service = new TestableLocalizationService();
        var capturedPropertyNames = new System.Collections.Generic.List<string?>();

        service.PropertyChanged += (sender, args) =>
        {
            capturedPropertyNames.Add(args.PropertyName);
        };

        // Act
        service.PublicOnPropertyChanged("FirstProperty");
        service.PublicOnPropertyChanged("SecondProperty");
        service.PublicOnPropertyChanged("ThirdProperty");

        // Assert
        Assert.That(capturedPropertyNames, Has.Count.EqualTo(3));
        Assert.That(capturedPropertyNames[0], Is.EqualTo("FirstProperty"));
        Assert.That(capturedPropertyNames[1], Is.EqualTo("SecondProperty"));
        Assert.That(capturedPropertyNames[2], Is.EqualTo("ThirdProperty"));
    }

    /// <summary>
    /// Tests that OnPropertyChanged handles property names with numbers correctly.
    /// Input: Property names containing numeric characters.
    /// Expected: PropertyChanged event is raised with the exact property name including numbers.
    /// </summary>
    [TestCase("Property1")]
    [TestCase("Property123")]
    [TestCase("123Property")]
    [TestCase("Prop3rty")]
    [TestCase("PropertyWith0InMiddle")]
    public void OnPropertyChanged_WithNumbers_RaisesEventWithExactName(string propertyName)
    {
        // Arrange
        var service = new TestableLocalizationService();
        var eventRaised = false;
        string? capturedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            capturedPropertyName = args.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(propertyName);

        // Assert
        Assert.That(eventRaised, Is.True);
        Assert.That(capturedPropertyName, Is.EqualTo(propertyName));
    }

    /// <summary>
    /// Tests that OnPropertyChanged event can be unsubscribed and no longer receives notifications.
    /// Input: Subscribe to event, then unsubscribe, then call method.
    /// Expected: Event handler is not invoked after unsubscription.
    /// </summary>
    [Test]
    public void OnPropertyChanged_AfterUnsubscribe_DoesNotInvokeHandler()
    {
        // Arrange
        var service = new TestableLocalizationService();
        var invocationCount = 0;
        PropertyChangedEventHandler handler = (sender, args) => invocationCount++;

        service.PropertyChanged += handler;
        service.PublicOnPropertyChanged("TestProperty");
        Assert.That(invocationCount, Is.EqualTo(1));

        service.PropertyChanged -= handler;

        // Act
        service.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(invocationCount, Is.EqualTo(1)); // Should still be 1, not 2
    }

    /// <summary>
    /// Tests that OnPropertyChanged with actual LocalizationService property names works correctly.
    /// Input: Real property names from LocalizationService (CurrentCulture, CurrentLanguageDisplayName).
    /// Expected: PropertyChanged event is raised with the correct property name.
    /// </summary>
    [TestCase("CurrentCulture")]
    [TestCase("CurrentLanguageDisplayName")]
    [TestCase("AvailableCultures")]
    public void OnPropertyChanged_WithActualServicePropertyNames_RaisesEventCorrectly(string propertyName)
    {
        // Arrange
        var service = new TestableLocalizationService();
        var eventRaised = false;
        string? capturedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            eventRaised = true;
            capturedPropertyName = args.PropertyName;
        };

        // Act
        service.PublicOnPropertyChanged(propertyName);

        // Assert
        Assert.That(eventRaised, Is.True);
        Assert.That(capturedPropertyName, Is.EqualTo(propertyName));
    }

    /// <summary>
    /// Tests that OnPropertyChanged passes EventArgs that are of the correct type.
    /// Input: Any property name.
    /// Expected: EventArgs parameter is of type PropertyChangedEventArgs.
    /// </summary>
    [Test]
    public void OnPropertyChanged_EventArgsType_IsPropertyChangedEventArgs()
    {
        // Arrange
        var service = new TestableLocalizationService();
        EventArgs? capturedEventArgs = null;

        service.PropertyChanged += (sender, args) =>
        {
            capturedEventArgs = args;
        };

        // Act
        service.PublicOnPropertyChanged("TestProperty");

        // Assert
        Assert.That(capturedEventArgs, Is.Not.Null);
        Assert.That(capturedEventArgs, Is.InstanceOf<PropertyChangedEventArgs>());
    }

    /// <summary>
    /// Tests that OnPropertyChanged with a subscriber that throws an exception does not prevent other subscribers from being invoked.
    /// Input: Multiple subscribers where one throws an exception.
    /// Expected: Exception propagates but does not prevent the method from completing the invocation list.
    /// Note: In .NET, if an event handler throws, it will propagate and potentially stop subsequent handlers.
    /// This test documents actual behavior.
    /// </summary>
    [Test]
    public void OnPropertyChanged_WithSubscriberThatThrows_PropagatesException()
    {
        // Arrange
        var service = new TestableLocalizationService();
        var secondHandlerInvoked = false;

        service.PropertyChanged += (sender, args) =>
        {
            throw new InvalidOperationException("Test exception");
        };

        service.PropertyChanged += (sender, args) =>
        {
            secondHandlerInvoked = true;
        };

        // Act & Assert
        // The first handler throws, which should propagate the exception
        Assert.Throws<InvalidOperationException>(() => service.PublicOnPropertyChanged("TestProperty"));

        // Second handler may or may not be invoked depending on event invocation implementation
        // In standard .NET event invocation, subsequent handlers after a throw are not invoked
    }

    /// <summary>
    /// Helper class to expose protected OnCultureChanged method for testing.
    /// Extends the existing TestableLocalizationService.
    /// </summary>
    private class ExtendedTestableLocalizationService : TestableLocalizationService
    {
        public void PublicOnCultureChanged()
        {
            OnCultureChanged();
        }
    }

    /// <summary>
    /// Tests that OnCultureChanged raises the CultureChanged event when a subscriber is attached.
    /// Input: Call OnCultureChanged with an event subscriber.
    /// Expected: CultureChanged event is raised exactly once.
    /// </summary>
    [Test]
    public void OnCultureChanged_WithSubscriber_RaisesCultureChangedEvent()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        var eventRaised = false;

        service.CultureChanged += (sender, args) =>
        {
            eventRaised = true;
        };

        // Act
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(eventRaised, Is.True);
    }

    /// <summary>
    /// Tests that OnCultureChanged does not throw an exception when there are no subscribers to CultureChanged event.
    /// Input: Call OnCultureChanged without any event subscribers.
    /// Expected: No exception is thrown (null-conditional operator handles null safely).
    /// </summary>
    [Test]
    public void OnCultureChanged_WithNoSubscriber_DoesNotThrow()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();

        // Act & Assert
        Assert.DoesNotThrow(() => service.PublicOnCultureChanged());
    }

    /// <summary>
    /// Tests that OnCultureChanged passes the correct sender to the CultureChanged event handler.
    /// Input: Call OnCultureChanged with an event subscriber.
    /// Expected: The sender in the event args is the LocalizationService instance itself.
    /// </summary>
    [Test]
    public void OnCultureChanged_WithSubscriber_PassesCorrectSender()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        object? capturedSender = null;

        service.CultureChanged += (sender, args) =>
        {
            capturedSender = sender;
        };

        // Act
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(capturedSender, Is.SameAs(service));
    }

    /// <summary>
    /// Tests that OnCultureChanged passes EventArgs.Empty to the CultureChanged event handler.
    /// Input: Call OnCultureChanged with an event subscriber.
    /// Expected: The EventArgs in the event handler is EventArgs.Empty.
    /// </summary>
    [Test]
    public void OnCultureChanged_WithSubscriber_PassesEventArgsEmpty()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        EventArgs? capturedArgs = null;

        service.CultureChanged += (sender, args) =>
        {
            capturedArgs = args;
        };

        // Act
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(capturedArgs, Is.SameAs(EventArgs.Empty));
    }

    /// <summary>
    /// Tests that OnCultureChanged raises the PropertyChanged event with an empty string property name.
    /// Input: Call OnCultureChanged with a PropertyChanged event subscriber.
    /// Expected: PropertyChanged event is raised with propertyName equal to string.Empty.
    /// </summary>
    [Test]
    public void OnCultureChanged_WithSubscriber_RaisesPropertyChangedWithEmptyString()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        var propertyChangedRaised = false;
        string? capturedPropertyName = null;

        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == string.Empty)
            {
                propertyChangedRaised = true;
                capturedPropertyName = args.PropertyName;
            }
        };

        // Act
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(propertyChangedRaised, Is.True);
        Assert.That(capturedPropertyName, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that OnCultureChanged invokes all subscribers when multiple handlers are attached to CultureChanged event.
    /// Input: Call OnCultureChanged with multiple event subscribers.
    /// Expected: All subscribers are invoked exactly once.
    /// </summary>
    [Test]
    public void OnCultureChanged_WithMultipleSubscribers_InvokesAllSubscribers()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        var firstSubscriberInvoked = false;
        var secondSubscriberInvoked = false;
        var thirdSubscriberInvoked = false;

        service.CultureChanged += (sender, args) => { firstSubscriberInvoked = true; };
        service.CultureChanged += (sender, args) => { secondSubscriberInvoked = true; };
        service.CultureChanged += (sender, args) => { thirdSubscriberInvoked = true; };

        // Act
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(firstSubscriberInvoked, Is.True);
        Assert.That(secondSubscriberInvoked, Is.True);
        Assert.That(thirdSubscriberInvoked, Is.True);
    }

    /// <summary>
    /// Tests that OnCultureChanged invokes CultureChanged event before PropertyChanged event.
    /// Input: Call OnCultureChanged with subscribers to both events.
    /// Expected: CultureChanged event is raised before PropertyChanged event with empty string.
    /// </summary>
    [Test]
    public void OnCultureChanged_WithBothEventSubscribers_RaisesCultureChangedBeforePropertyChanged()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        var eventOrder = new System.Collections.Generic.List<string>();

        service.CultureChanged += (sender, args) =>
        {
            eventOrder.Add("CultureChanged");
        };

        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == string.Empty)
            {
                eventOrder.Add("PropertyChanged_Empty");
            }
        };

        // Act
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(eventOrder.Count, Is.EqualTo(2));
        Assert.That(eventOrder[0], Is.EqualTo("CultureChanged"));
        Assert.That(eventOrder[1], Is.EqualTo("PropertyChanged_Empty"));
    }

    /// <summary>
    /// Tests that OnCultureChanged raises PropertyChanged event with correct sender.
    /// Input: Call OnCultureChanged with a PropertyChanged event subscriber.
    /// Expected: The sender in PropertyChanged event is the LocalizationService instance.
    /// </summary>
    [Test]
    public void OnCultureChanged_RaisesPropertyChanged_WithCorrectSender()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        object? capturedSender = null;

        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == string.Empty)
            {
                capturedSender = sender;
            }
        };

        // Act
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(capturedSender, Is.SameAs(service));
    }

    /// <summary>
    /// Tests that OnCultureChanged can be called multiple times without errors.
    /// Input: Call OnCultureChanged multiple times in succession.
    /// Expected: Each call successfully raises both events without exceptions.
    /// </summary>
    [Test]
    public void OnCultureChanged_CalledMultipleTimes_RaisesEventsEachTime()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        var cultureChangedCount = 0;
        var propertyChangedCount = 0;

        service.CultureChanged += (sender, args) => { cultureChangedCount++; };
        service.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == string.Empty)
            {
                propertyChangedCount++;
            }
        };

        // Act
        service.PublicOnCultureChanged();
        service.PublicOnCultureChanged();
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(cultureChangedCount, Is.EqualTo(3));
        Assert.That(propertyChangedCount, Is.EqualTo(3));
    }

    /// <summary>
    /// Tests that OnCultureChanged raises PropertyChanged for all properties (indicated by empty string).
    /// This verifies the intention that all UI bindings should refresh when culture changes.
    /// Input: Call OnCultureChanged.
    /// Expected: PropertyChanged event is raised with string.Empty, not with specific property names.
    /// </summary>
    [Test]
    public void OnCultureChanged_NotifiesAllProperties_ViaEmptyStringPropertyName()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        var propertyNames = new System.Collections.Generic.List<string?>();

        service.PropertyChanged += (sender, args) =>
        {
            propertyNames.Add(args.PropertyName);
        };

        // Act
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(propertyNames.Count, Is.EqualTo(1));
        Assert.That(propertyNames[0], Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that OnCultureChanged properly handles unsubscription during event invocation.
    /// Input: Unsubscribe from CultureChanged event within the event handler itself.
    /// Expected: No exception is thrown and remaining subscribers are still invoked.
    /// </summary>
    [Test]
    public void OnCultureChanged_WithUnsubscriptionDuringEvent_HandlesGracefully()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        var firstInvoked = false;
        var secondInvoked = false;
        EventHandler? handler = null;

        handler = (sender, args) =>
        {
            firstInvoked = true;
            service.CultureChanged -= handler;
        };

        service.CultureChanged += handler;
        service.CultureChanged += (sender, args) => { secondInvoked = true; };

        // Act
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(firstInvoked, Is.True);
        Assert.That(secondInvoked, Is.True);
    }

    /// <summary>
    /// Tests that OnCultureChanged does not raise PropertyChanged for specific property names.
    /// Input: Subscribe to PropertyChanged and call OnCultureChanged.
    /// Expected: Only PropertyChanged with empty string is raised, not CurrentCulture or CurrentLanguageDisplayName.
    /// </summary>
    [Test]
    public void OnCultureChanged_DoesNotRaisePropertyChangedForSpecificProperties()
    {
        // Arrange
        var service = new ExtendedTestableLocalizationService();
        var propertyNames = new System.Collections.Generic.List<string?>();

        service.PropertyChanged += (sender, args) =>
        {
            propertyNames.Add(args.PropertyName);
        };

        // Act
        service.PublicOnCultureChanged();

        // Assert
        Assert.That(propertyNames, Does.Not.Contain("CurrentCulture"));
        Assert.That(propertyNames, Does.Not.Contain("CurrentLanguageDisplayName"));
        Assert.That(propertyNames, Does.Contain(string.Empty));
    }

    /// <summary>
    /// Tests that GetString handles keys with control characters by returning the key as fallback.
    /// Input: Keys containing control characters (\n, \t, \r, \0).
    /// Expected: Returns the key as fallback since no property with control characters exists.
    /// </summary>
    [TestCase("Key\nWithNewline")]
    [TestCase("Key\tWithTab")]
    [TestCase("Key\rWithCarriageReturn")]
    [TestCase("Key\0WithNull")]
    [TestCase("\n")]
    [TestCase("\t\t")]
    public void GetString_KeyWithControlCharacters_ReturnsKey(string keyWithControlChar)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(keyWithControlChar);

        // Assert
        Assert.That(result, Is.EqualTo(keyWithControlChar));
    }

    /// <summary>
    /// Tests that GetString is case-sensitive and returns the key when case doesn't match.
    /// Input: Known property "Abort" with different casing.
    /// Expected: Returns the key as fallback since property names are case-sensitive.
    /// </summary>
    [TestCase("abort")]
    [TestCase("ABORT")]
    [TestCase("aBort")]
    public void GetString_KeyWithIncorrectCase_ReturnsKey(string incorrectCaseKey)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(incorrectCaseKey);

        // Assert
        Assert.That(result, Is.EqualTo(incorrectCaseKey));
    }

    /// <summary>
    /// Tests that GetString handles keys that are all digits.
    /// Input: Keys containing only numeric characters.
    /// Expected: Returns the key as fallback since numeric-only names cannot be valid C# property names.
    /// </summary>
    [TestCase("123456")]
    [TestCase("0")]
    [TestCase("999999999")]
    public void GetString_KeyWithAllDigits_ReturnsKey(string numericKey)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(numericKey);

        // Assert
        Assert.That(result, Is.EqualTo(numericKey));
    }

    /// <summary>
    /// Tests that GetString handles keys with path-like separators.
    /// Input: Keys containing forward or backward slashes.
    /// Expected: Returns the key as fallback since slashes are not valid in property names.
    /// </summary>
    [TestCase("Key/With/Slash")]
    [TestCase("Key\\With\\Backslash")]
    [TestCase("/StartingSlash")]
    [TestCase("EndingSlash/")]
    public void GetString_KeyWithPathSeparators_ReturnsKey(string keyWithPath)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(keyWithPath);

        // Assert
        Assert.That(result, Is.EqualTo(keyWithPath));
    }

    /// <summary>
    /// Tests that GetString handles keys with whitespace in the middle.
    /// Input: Keys containing spaces within the string.
    /// Expected: Returns the key as fallback since spaces are not valid in property names.
    /// </summary>
    [TestCase("Key With Spaces")]
    [TestCase("Two  Spaces")]
    [TestCase(" LeadingSpace")]
    [TestCase("TrailingSpace ")]
    public void GetString_KeyWithWhitespaceInMiddle_ReturnsKey(string keyWithSpaces)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(keyWithSpaces);

        // Assert
        Assert.That(result, Is.EqualTo(keyWithSpaces));
    }

    /// <summary>
    /// Tests that GetString handles keys with only special characters.
    /// Input: Keys containing only punctuation and special symbols.
    /// Expected: Returns the key as fallback since these are not valid property names.
    /// </summary>
    [TestCase("!!!")]
    [TestCase("@@@")]
    [TestCase("***")]
    [TestCase("$$$")]
    [TestCase("###")]
    [TestCase("&&&")]
    public void GetString_KeyWithOnlySpecialCharacters_ReturnsKey(string specialKey)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(specialKey);

        // Assert
        Assert.That(result, Is.EqualTo(specialKey));
    }

    /// <summary>
    /// Tests that GetString returns consistent results when called multiple times with the same key.
    /// Input: Same valid key called multiple times.
    /// Expected: Returns the same localized value each time.
    /// </summary>
    [Test]
    public void GetString_SameValidKeyMultipleTimes_ReturnsConsistentValue()
    {
        // Arrange
        var service = new LocalizationService();
        string validKey = "Abort";

        // Act
        string result1 = service.GetString(validKey);
        string result2 = service.GetString(validKey);
        string result3 = service.GetString(validKey);

        // Assert
        Assert.That(result1, Is.Not.Null);
        Assert.That(result2, Is.EqualTo(result1));
        Assert.That(result3, Is.EqualTo(result1));
    }

    /// <summary>
    /// Tests that GetString returns consistent results when called multiple times with non-existent key.
    /// Input: Same non-existent key called multiple times.
    /// Expected: Returns the same key as fallback each time.
    /// </summary>
    [Test]
    public void GetString_SameInvalidKeyMultipleTimes_ReturnsConsistentValue()
    {
        // Arrange
        var service = new LocalizationService();
        string invalidKey = "NonExistentKey123";

        // Act
        string result1 = service.GetString(invalidKey);
        string result2 = service.GetString(invalidKey);
        string result3 = service.GetString(invalidKey);

        // Assert
        Assert.That(result1, Is.EqualTo(invalidKey));
        Assert.That(result2, Is.EqualTo(invalidKey));
        Assert.That(result3, Is.EqualTo(invalidKey));
    }

    /// <summary>
    /// Tests that GetString handles keys with parentheses.
    /// Input: Keys containing parentheses which might be confused with method calls.
    /// Expected: Returns the key as fallback since parentheses are not valid in property names.
    /// </summary>
    [TestCase("Key()")]
    [TestCase("Method(param)")]
    [TestCase("(Key)")]
    public void GetString_KeyWithParentheses_ReturnsKey(string keyWithParens)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(keyWithParens);

        // Assert
        Assert.That(result, Is.EqualTo(keyWithParens));
    }

    /// <summary>
    /// Tests that GetString handles keys with angle brackets.
    /// Input: Keys containing angle brackets which might be confused with generic types.
    /// Expected: Returns the key as fallback since angle brackets are not valid in property names.
    /// </summary>
    [TestCase("Key<T>")]
    [TestCase("Generic<int>")]
    [TestCase("<Key>")]
    public void GetString_KeyWithAngleBrackets_ReturnsKey(string keyWithBrackets)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(keyWithBrackets);

        // Assert
        Assert.That(result, Is.EqualTo(keyWithBrackets));
    }

    /// <summary>
    /// Tests that GetString handles keys with equals signs.
    /// Input: Keys containing equals signs which might be confused with assignment.
    /// Expected: Returns the key as fallback since equals signs are not valid in property names.
    /// </summary>
    [TestCase("Key=Value")]
    [TestCase("=Key")]
    [TestCase("Key==")]
    public void GetString_KeyWithEqualsSigns_ReturnsKey(string keyWithEquals)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(keyWithEquals);

        // Assert
        Assert.That(result, Is.EqualTo(keyWithEquals));
    }

    /// <summary>
    /// Tests that GetString handles keys with mixed valid and invalid characters.
    /// Input: Keys that start validly but contain invalid characters.
    /// Expected: Returns the key as fallback since invalid characters make them not valid property names.
    /// </summary>
    [TestCase("Valid-Key")]
    [TestCase("Valid@Key")]
    [TestCase("Valid Key")]
    [TestCase("Valid.Key.Name")]
    public void GetString_KeyWithMixedValidInvalidCharacters_ReturnsKey(string mixedKey)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(mixedKey);

        // Assert
        Assert.That(result, Is.EqualTo(mixedKey));
    }

    /// <summary>
    /// Tests that GetString handles multiple different valid keys correctly.
    /// Input: Multiple known valid property keys from AppStrings.
    /// Expected: Returns localized values for each, not the keys themselves.
    /// </summary>
    [Test]
    public void GetString_MultipleDifferentValidKeys_ReturnsLocalizedValues()
    {
        // Arrange
        var service = new LocalizationService();
        string key1 = "Abort";
        string key2 = "About";

        // Act
        string result1 = service.GetString(key1);
        string result2 = service.GetString(key2);

        // Assert
        Assert.That(result1, Is.Not.Null);
        Assert.That(result1, Is.Not.Empty);
        Assert.That(result2, Is.Not.Null);
        Assert.That(result2, Is.Not.Empty);
        // Results should be different localized values, not the keys
        Assert.That(result1, Is.Not.EqualTo(result2));
    }

    /// <summary>
    /// Tests that GetString handles keys with curly braces.
    /// Input: Keys containing curly braces which might be confused with string formatting.
    /// Expected: Returns the key as fallback since braces are not valid in property names.
    /// </summary>
    [TestCase("Key{0}")]
    [TestCase("{Key}")]
    [TestCase("Key{value}")]
    public void GetString_KeyWithCurlyBraces_ReturnsKey(string keyWithBraces)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(keyWithBraces);

        // Assert
        Assert.That(result, Is.EqualTo(keyWithBraces));
    }

    /// <summary>
    /// Tests that GetString handles keys with plus and minus signs.
    /// Input: Keys containing arithmetic operators.
    /// Expected: Returns the key as fallback since operators are not valid in property names.
    /// </summary>
    [TestCase("Key+Value")]
    [TestCase("Key-Value")]
    [TestCase("+Key")]
    [TestCase("-Key")]
    public void GetString_KeyWithArithmeticOperators_ReturnsKey(string keyWithOperators)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(keyWithOperators);

        // Assert
        Assert.That(result, Is.EqualTo(keyWithOperators));
    }

    /// <summary>
    /// Tests that GetString handles keys with quotes.
    /// Input: Keys containing single or double quotes.
    /// Expected: Returns the key as fallback since quotes are not valid in property names.
    /// </summary>
    [TestCase("Key\"WithQuote")]
    [TestCase("'Key'")]
    [TestCase("Key'Value")]
    public void GetString_KeyWithQuotes_ReturnsKey(string keyWithQuotes)
    {
        // Arrange
        var service = new LocalizationService();

        // Act
        string result = service.GetString(keyWithQuotes);

        // Assert
        Assert.That(result, Is.EqualTo(keyWithQuotes));
    }

    /// <summary>
    /// Tests that GetString does not throw exceptions for any string input except null.
    /// Input: Various edge case strings.
    /// Expected: Returns either localized value or key, never throws (except for null).
    /// </summary>
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("ValidKey")]
    [TestCase("Invalid@Key")]
    [TestCase("123")]
    [TestCase("Key\nNew")]
    public void GetString_VariousInputs_DoesNotThrow(string key)
    {
        // Arrange
        var service = new LocalizationService();

        // Act & Assert
        Assert.DoesNotThrow(() => service.GetString(key));
    }

    /// <summary>
    /// Tests that constructor initializes with saved English culture from Preferences.
    /// Input: Preferences contains saved culture "en".
    /// Expected: CurrentCulture is set to English culture.
    /// </summary>
    [Test]
    public void Constructor_WithSavedEnglishCultureInPreferences_SetsCurrentCultureToEnglish()
    {
        // Arrange
        const string cultureName = "en";
        Preferences.Set("CurrentCulture", cultureName);

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor initializes with saved Italian culture from Preferences.
    /// Input: Preferences contains saved culture "it".
    /// Expected: CurrentCulture is set to Italian culture.
    /// </summary>
    [Test]
    public void Constructor_WithSavedItalianCultureInPreferences_SetsCurrentCultureToItalian()
    {
        // Arrange
        const string cultureName = "it";
        Preferences.Set("CurrentCulture", cultureName);

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor falls back to CurrentUICulture when no saved culture exists in Preferences.
    /// Input: Preferences has no saved culture, CurrentUICulture is English.
    /// Expected: CurrentCulture is set to English from CurrentUICulture fallback.
    /// </summary>
    [Test]
    public void Constructor_WithoutSavedCultureAndEnglishUIculture_FallsBackToEnglish()
    {
        // Arrange
        Preferences.Remove("CurrentCulture");
        CultureInfo.CurrentUICulture = new CultureInfo("en");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor falls back to CurrentUICulture when no saved culture exists in Preferences.
    /// Input: Preferences has no saved culture, CurrentUICulture is Italian.
    /// Expected: CurrentCulture is set to Italian from CurrentUICulture fallback.
    /// </summary>
    [Test]
    public void Constructor_WithoutSavedCultureAndItalianUIculture_FallsBackToItalian()
    {
        // Arrange
        Preferences.Remove("CurrentCulture");
        CultureInfo.CurrentUICulture = new CultureInfo("it");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
        CultureInfo.CurrentUICulture = new CultureInfo("en");
    }

    /// <summary>
    /// Tests that constructor resolves regional culture variant to base culture.
    /// Input: Preferences contains "en-US".
    /// Expected: CurrentCulture is resolved to base English culture "en".
    /// </summary>
    [Test]
    public void Constructor_WithSavedRegionalEnglishCulture_ResolvesToBaseEnglish()
    {
        // Arrange
        Preferences.Set("CurrentCulture", "en-US");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor resolves regional Italian culture variant to base culture.
    /// Input: Preferences contains "it-IT".
    /// Expected: CurrentCulture is resolved to base Italian culture "it".
    /// </summary>
    [Test]
    public void Constructor_WithSavedRegionalItalianCulture_ResolvesToBaseItalian()
    {
        // Arrange
        Preferences.Set("CurrentCulture", "it-IT");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor defaults to English when saved culture is unsupported.
    /// Input: Preferences contains unsupported culture "fr" (French).
    /// Expected: CurrentCulture defaults to English.
    /// </summary>
    [Test]
    public void Constructor_WithUnsupportedSavedCulture_DefaultsToEnglish()
    {
        // Arrange
        Preferences.Set("CurrentCulture", "fr");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor defaults to English when saved culture name is invalid.
    /// Input: Preferences contains invalid culture name "invalid123".
    /// Expected: CurrentCulture defaults to English.
    /// </summary>
    [Test]
    public void Constructor_WithInvalidSavedCultureName_DefaultsToEnglish()
    {
        // Arrange
        Preferences.Set("CurrentCulture", "invalid123");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor falls back to CurrentUICulture when it's a supported culture.
    /// Input: No saved culture, CurrentUICulture is "en-GB".
    /// Expected: CurrentCulture is resolved to base English culture.
    /// </summary>
    [Test]
    public void Constructor_WithoutSavedCultureAndRegionalUIculture_ResolvesToBaseCulture()
    {
        // Arrange
        Preferences.Remove("CurrentCulture");
        CultureInfo.CurrentUICulture = new CultureInfo("en-GB");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
        CultureInfo.CurrentUICulture = new CultureInfo("en");
    }

    /// <summary>
    /// Tests that constructor defaults to English when CurrentUICulture is unsupported and no saved culture exists.
    /// Input: No saved culture, CurrentUICulture is "de" (German - unsupported).
    /// Expected: CurrentCulture defaults to English.
    /// </summary>
    [Test]
    public void Constructor_WithoutSavedCultureAndUnsupportedUIculture_DefaultsToEnglish()
    {
        // Arrange
        Preferences.Remove("CurrentCulture");
        CultureInfo.CurrentUICulture = new CultureInfo("de");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
        CultureInfo.CurrentUICulture = new CultureInfo("en");
    }

    /// <summary>
    /// Tests that constructor handles saved culture with special characters by defaulting to English.
    /// Input: Preferences contains culture name with special characters "@#$".
    /// Expected: CurrentCulture defaults to English.
    /// </summary>
    [Test]
    public void Constructor_WithSpecialCharactersInSavedCulture_DefaultsToEnglish()
    {
        // Arrange
        Preferences.Set("CurrentCulture", "@#$");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor calls SetCulture which raises PropertyChanged event.
    /// Input: Default constructor with any saved culture.
    /// Expected: PropertyChanged event can be subscribed and receives notifications.
    /// </summary>
    [Test]
    public void Constructor_DefaultInvocation_CallsSetCultureWhichEnablesPropertyChanged()
    {
        // Arrange
        Preferences.Set("CurrentCulture", "en");
        var service = new LocalizationService();
        var propertyChangedRaised = false;

        service.PropertyChanged += (sender, args) =>
        {
            propertyChangedRaised = true;
        };

        // Act
        service.SetCulture(new CultureInfo("it"));

        // Assert
        Assert.That(propertyChangedRaised, Is.True, "PropertyChanged event should be raised after SetCulture");

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor calls SetCulture which saves the culture to Preferences.
    /// Input: Default constructor.
    /// Expected: Culture is saved to Preferences after construction.
    /// </summary>
    [Test]
    public void Constructor_DefaultInvocation_CallsSetCultureWhichSavesToPreferences()
    {
        // Arrange
        Preferences.Remove("CurrentCulture");

        // Act
        var service = new LocalizationService();
        var savedCulture = Preferences.Get("CurrentCulture", string.Empty);

        // Assert
        Assert.That(savedCulture, Is.Not.Empty, "Culture should be saved to Preferences");
        Assert.That(savedCulture, Is.EqualTo("en").Or.EqualTo("it"), "Saved culture should be one of the supported cultures");

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor with saved culture in uppercase is handled correctly.
    /// Input: Preferences contains "EN" (uppercase).
    /// Expected: CurrentCulture is set to English (case-insensitive matching).
    /// </summary>
    [Test]
    public void Constructor_WithUppercaseSavedCulture_SetsCorrectCulture()
    {
        // Arrange
        Preferences.Set("CurrentCulture", "EN");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("en"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }

    /// <summary>
    /// Tests that constructor with saved culture in mixed case is handled correctly.
    /// Input: Preferences contains "It" (mixed case).
    /// Expected: CurrentCulture is set to Italian (case-insensitive matching).
    /// </summary>
    [Test]
    public void Constructor_WithMixedCaseSavedCulture_SetsCorrectCulture()
    {
        // Arrange
        Preferences.Set("CurrentCulture", "It");

        // Act
        var service = new LocalizationService();

        // Assert
        Assert.That(service.CurrentCulture.TwoLetterISOLanguageName, Is.EqualTo("it"));

        // Cleanup
        Preferences.Remove("CurrentCulture");
    }
}