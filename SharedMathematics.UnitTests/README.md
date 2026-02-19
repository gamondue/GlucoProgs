# Identification Tests - GlucoMan.Maui.UnitTests

## Overview
Tests for **SharedMathematics** `Identification` classes that require GlucoMan business objects (`Meal`, `Injection`, `GlucoseRecord`).

These tests use **mock data** (no database access) to verify mathematical identification algorithms.

## Test Files Created

### ✅ Identification1Tests.cs (18 tests)
Tests for time-series conversion methods in `Mathematics.Identification1.Identification`.

#### **FromMeals Tests** (6 tests)
- ✅ Null input handling
- ✅ Empty list handling
- ✅ Valid meal conversion (EventTime → CarbohydratesGrams)
- ✅ Skipping meals without timestamp
- ✅ Skipping meals without carbohydrate value
- ✅ Chronological sorting of unordered meals

#### **FromInjections Tests** (4 tests)
- ✅ Null input handling
- ✅ Conversion using `InsulinValue`
- ✅ Preference for `InsulinCalculated` over `InsulinValue`
- ✅ Skipping injections without insulin values

#### **FromGlucoseRecords Tests** (4 tests)
- ✅ Null input handling
- ✅ Valid glucose record conversion
- ✅ Skipping records without glucose values
- ✅ Chronological sorting

#### **Integration Tests** (1 test)
- ✅ Real-world breakfast scenario (meal + insulin + glucose)

## Test Strategy

### Mock Data Pattern
All tests use **in-memory mock objects** without database access:

```csharp
var meal = new Meal
{
    EventTime = new DateTimeAndText { DateTime = new DateTime(2024, 1, 15, 12, 0, 0) },
    CarbohydratesGrams = new DoubleAndText { Double = 50.0 }
};
```

### No Dependencies On:
- ❌ DataLayer (no database queries)
- ❌ BusinessLayer (no BL_* classes)
- ❌ UI (no MAUI pages/controls)

### Only Dependencies:
- ✅ Business Objects (`Meal`, `Injection`, `GlucoseRecord`)
- ✅ Mathematics namespace (`Identification1`, `Identification2`, `Identification3`)
- ✅ MathNet.Numerics (for linear algebra)

## Running Tests

### Visual Studio Test Explorer
1. Open **Test Explorer** (`Test` → `Test Explorer`)
2. Filter by namespace: `Mathematics`
3. Run all Identification tests

### Command Line
```bash
# Run all Identification tests
dotnet test GlucoMan.Maui.UnitTests --filter "FullyQualifiedName~Identification"

# Run only Identification1 tests
dotnet test GlucoMan.Maui.UnitTests --filter "FullyQualifiedName~Identification1Tests"
```

## Planned Tests

### 🔜 Identification2Tests.cs (To be created)
Tests for system identification algorithms:
- Parameter estimation for glucose-insulin dynamics
- Time-series segmentation
- Isolated segment analysis

### 🔜 Identification3Tests.cs (To be created)
Tests for advanced identification methods:
- Multi-input identification (CHO + Insulin → Glucose)
- Model validation
- Prediction error analysis

## Test Coverage Goals

| Class | Methods | Tests Created | Coverage |
|-------|---------|---------------|----------|
| `Identification1` | 3 | 18 | ✅ 100% |
| `Identification2` | TBD | 0 | 🔜 Planned |
| `Identification3` | TBD | 0 | 🔜 Planned |

## Example Test

```csharp
[Test]
public void FromMeals_WithValidMeals_ReturnsCorrectTimePoints()
{
    // Arrange - Create mock meal data
    var baseTime = new DateTime(2024, 1, 15, 12, 0, 0);
    var meals = new List<Meal>
    {
        new Meal
        {
            EventTime = new DateTimeAndText { DateTime = baseTime },
            CarbohydratesGrams = new DoubleAndText { Double = 50.0 }
        }
    };

    // Act - Convert to time points
    var result = Identification.FromMeals(meals);

    // Assert - Verify conversion
    Assert.That(result.Length, Is.EqualTo(1));
    Assert.That(result[0].Time, Is.EqualTo(baseTime));
    Assert.That(result[0].Value, Is.EqualTo(50.0));
}
```

## Known Issues

⚠️ **GlucoMan.Maui.UnitTests currently has compilation errors** due to:
1. Missing enum definitions (`ZoneOfPosition` moved to `CommonEnums.cs`)
2. Duplicate class definitions in some test files

These are pre-existing issues not related to Identification tests.

## Next Steps

1. ✅ **Identification1Tests.cs** - CREATED (18 tests)
2. 🔜 Fix compilation errors in GlucoMan.Maui.UnitTests
3. 🔜 Create **Identification2Tests.cs**
4. 🔜 Create **Identification3Tests.cs**
5. 🔜 Verify all tests pass in Test Explorer

## Mathematical Background

### Identification1
Converts time-stamped diabetes data into uniform time series for mathematical analysis:
- **Meals** → Carbohydrate input signal
- **Injections** → Insulin input signal
- **Glucose Records** → Output signal

### Use Case
These time series are used in system identification algorithms to estimate parameters like:
- Insulin sensitivity factor
- Carbohydrate-to-insulin ratio
- Glucose response time constants
