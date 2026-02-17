using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

using GlucoMan.Maui.Resources.Strings;
using GlucoMan.Maui.Services;
using NUnit.Framework;

namespace GlucoMan.Maui.Services.UnitTests;


/// <summary>
/// Unit tests for the LocalizationService class
/// </summary>
public partial class LocalizationServiceTests
{
    /// <summary>
    /// Tests that the CurrentCulture property getter returns the initialized culture after construction.
    /// Input: Default constructor initialization.
    /// Expected: CurrentCulture property returns a non-null CultureInfo object.
    /// </summary>
    [Test]
    public void CurrentCulture_AfterConstruction_ReturnsNonNullCultureInfo()
    {
        // Arrange & Act
        var service = new LocalizationService();

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
}