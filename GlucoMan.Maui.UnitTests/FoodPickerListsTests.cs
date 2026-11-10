using gamon;
using GlucoMan;
using Microsoft.Data.Sqlite;

namespace GlucoMan.Maui.UnitTests;

/// <summary>
/// Verifies the rules of the lists shown by the pickers of manufacturers and categories
/// of FoodPage. The tables Manufacturers and CategoriesOfFood are "light" lists of suggestions:
/// - the Manufacturer and Category fields of a food are free strings:
///   saving a food never adds them to the lists;
/// - a name is added to a list only with the "+" button (AddManufacturerToList / AddCategoryToList),
///   and adding it does not change any food;
/// - the "-" button (DeleteManufacturerFromList / DeleteCategoryFromList) deletes the row of the list
///   and leaves all the foods untouched;
/// - the first item of the pickers is blank, and it is the item shown when the page opens.
/// Each test works on a new database, created by the program in a temporary file.
/// </summary>
[TestFixture]
public class FoodPickerListsTests
{
    private string _dbFile = null!;
    private BL_MealAndFood _bl = null!;

    [SetUp]
    public void SetUp()
    {
        if (General.LogOfProgram == null)
        {
            string tempLogPath = Path.Combine(Path.GetTempPath(), "GlucoManTests");
            Directory.CreateDirectory(tempLogPath);
            General.LogOfProgram = new Logger(tempLogPath, true,
                "Test_Log.txt", "Test_Errors.txt", "Test_Debug.txt",
                "Test_Prompts.txt", "Test_Data.txt");
        }
        _dbFile = Path.Combine(Path.GetTempPath(), "GlucoManTest_" + Guid.NewGuid().ToString("N") + ".sqlite");
        _bl = new BL_MealAndFood(new DL_Sqlite(_dbFile));
    }

    [TearDown]
    public void TearDown()
    {
        SqliteConnection.ClearAllPools();
        try { File.Delete(_dbFile); } catch (IOException) { }
    }

    #region helpers
    private int SaveFood(string name, string manufacturer, string category)
    {
        Food food = new Food(new UnitOfFood("g", 1))
        {
            Name = name,
            Manufacturer = manufacturer,
            Category = category
        };
        return _bl.SaveOneFood(food)!.Value;
    }
    private List<string> ManufacturerNames() =>
        _bl.GetManufacturersForPicker().Where(m => !string.IsNullOrEmpty(m.Name)).Select(m => m.Name).ToList();
    private List<string> CategoryNames() =>
        _bl.GetCategoriesForPicker().Where(c => !string.IsNullOrEmpty(c.Name)).Select(c => c.Name).ToList();
    private Manufacturer ManufacturerInList(string name) =>
        _bl.GetManufacturersForPicker().Single(m => m.Name == name);
    private CategoryOfFood CategoryInList(string name) =>
        _bl.GetCategoriesForPicker().Single(c => c.Name == name);
    #endregion

    #region Manufacturers
    [Test]
    public void SaveOneFood_DoesNotAddItsManufacturerToTheList()
    {
        SaveFood("Pasta", "Barilla", "Cereals");

        Assert.That(ManufacturerNames(), Is.Empty);
    }

    [Test]
    public void AddManufacturerToList_AddsTheTrimmedTextOfTheEntry()
    {
        bool added = _bl.AddManufacturerToList("  Barilla  ");

        Assert.That(added, Is.True);
        Assert.That(ManufacturerNames(), Is.EqualTo(new[] { "Barilla" }));
    }

    [Test]
    public void AddManufacturerToList_DoesNotChangeAnyFood()
    {
        int idFood = SaveFood("Pasta", "Barilla", "Cereals");

        _bl.AddManufacturerToList("De Cecco");

        Assert.That(_bl.GetOneFood(idFood).Manufacturer, Is.EqualTo("Barilla"));
    }

    [Test]
    public void AddManufacturerToList_IgnoresEmptyText()
    {
        Assert.That(_bl.AddManufacturerToList(""), Is.False);
        Assert.That(_bl.AddManufacturerToList("   "), Is.False);
        Assert.That(_bl.AddManufacturerToList(null!), Is.False);
        Assert.That(ManufacturerNames(), Is.Empty);
    }

    [Test]
    public void AddManufacturerToList_DoesNotAddDuplicates_IgnoringCase()
    {
        _bl.AddManufacturerToList("Barilla");

        bool addedAgain = _bl.AddManufacturerToList("BARILLA");

        Assert.That(addedAgain, Is.False);
        Assert.That(ManufacturerNames(), Is.EqualTo(new[] { "Barilla" }));
    }

    [Test]
    public void DeleteManufacturerFromList_RemovesOnlyTheRowOfTheList()
    {
        _bl.AddManufacturerToList("Barilla");
        _bl.AddManufacturerToList("De Cecco");

        _bl.DeleteManufacturerFromList(ManufacturerInList("Barilla"));

        Assert.That(ManufacturerNames(), Is.EqualTo(new[] { "De Cecco" }));
    }

    [Test]
    public void DeleteManufacturerFromList_LeavesTheFoodsThatUseItUntouched()
    {
        int idPasta = SaveFood("Pasta", "Barilla", "Cereals");
        int idBiscuits = SaveFood("Biscuits", "Barilla", "Sweets");
        _bl.AddManufacturerToList("Barilla");

        _bl.DeleteManufacturerFromList(ManufacturerInList("Barilla"));

        Assert.That(_bl.GetOneFood(idPasta).Manufacturer, Is.EqualTo("Barilla"));
        Assert.That(_bl.GetOneFood(idBiscuits).Manufacturer, Is.EqualTo("Barilla"));
    }

    [Test]
    public void GetManufacturersForPicker_FirstItemIsBlank()
    {
        _bl.AddManufacturerToList("Barilla");

        List<Manufacturer> list = _bl.GetManufacturersForPicker();

        Assert.That(list, Has.Count.EqualTo(2));
        Assert.That(list[0].Name, Is.Null.Or.Empty);
    }

    [Test]
    public void GetManufacturersForPicker_WithEmptyTable_ContainsOnlyTheBlankItem()
    {
        List<Manufacturer> list = _bl.GetManufacturersForPicker();

        Assert.That(list, Has.Count.EqualTo(1));
        Assert.That(list[0].Name, Is.Null.Or.Empty);
    }
    #endregion

    #region Categories
    [Test]
    public void SaveOneFood_DoesNotAddItsCategoryToTheList()
    {
        SaveFood("Pasta", "Barilla", "Cereals");

        Assert.That(CategoryNames(), Is.Empty);
    }

    [Test]
    public void AddCategoryToList_AddsTheTrimmedTextOfTheEntry()
    {
        bool added = _bl.AddCategoryToList("  Cereals  ");

        Assert.That(added, Is.True);
        Assert.That(CategoryNames(), Is.EqualTo(new[] { "Cereals" }));
    }

    [Test]
    public void AddCategoryToList_DoesNotChangeAnyFood()
    {
        int idFood = SaveFood("Pasta", "Barilla", "Cereals");

        _bl.AddCategoryToList("Sweets");

        Assert.That(_bl.GetOneFood(idFood).Category, Is.EqualTo("Cereals"));
    }

    [Test]
    public void AddCategoryToList_IgnoresEmptyText()
    {
        Assert.That(_bl.AddCategoryToList(""), Is.False);
        Assert.That(_bl.AddCategoryToList("   "), Is.False);
        Assert.That(_bl.AddCategoryToList(null!), Is.False);
        Assert.That(CategoryNames(), Is.Empty);
    }

    [Test]
    public void AddCategoryToList_DoesNotAddDuplicates_IgnoringCase()
    {
        _bl.AddCategoryToList("Cereals");

        bool addedAgain = _bl.AddCategoryToList("cereals");

        Assert.That(addedAgain, Is.False);
        Assert.That(CategoryNames(), Is.EqualTo(new[] { "Cereals" }));
    }

    [Test]
    public void DeleteCategoryFromList_RemovesOnlyTheRowOfTheList()
    {
        _bl.AddCategoryToList("Cereals");
        _bl.AddCategoryToList("Sweets");

        _bl.DeleteCategoryFromList(CategoryInList("Cereals"));

        Assert.That(CategoryNames(), Is.EqualTo(new[] { "Sweets" }));
    }

    [Test]
    public void DeleteCategoryFromList_LeavesTheFoodsThatUseItUntouched()
    {
        int idPasta = SaveFood("Pasta", "Barilla", "Cereals");
        int idRice = SaveFood("Rice", "Scotti", "Cereals");
        _bl.AddCategoryToList("Cereals");

        _bl.DeleteCategoryFromList(CategoryInList("Cereals"));

        Assert.That(_bl.GetOneFood(idPasta).Category, Is.EqualTo("Cereals"));
        Assert.That(_bl.GetOneFood(idRice).Category, Is.EqualTo("Cereals"));
    }

    [Test]
    public void GetCategoriesForPicker_FirstItemIsBlank()
    {
        _bl.AddCategoryToList("Cereals");

        List<CategoryOfFood> list = _bl.GetCategoriesForPicker();

        Assert.That(list, Has.Count.EqualTo(2));
        Assert.That(list[0].Name, Is.Null.Or.Empty);
    }
    #endregion
}
