# GlucoMan

## Data
- Change the time zone of past data (already produced in the past) when I was travelling in different time zones (Turkey, Uzbekistan, ?? Bulgaria ??, Portugal). The UtcOffset field must be adjusted in the tables: GlucoseRecords, Meals and SensorsRecords.

- Adjust the time zones from 25 January 2026 from UTC+2 to UTC+1.

- Analyze the 2022 data rows (and a few from 2024) that appeared in the latest import from LibreView (Injections and Meals tables).

# Database
- Try database operations on the SensorsRecords table.

### Import from FreeStyle Libre
- Investigate why strange records are created in the Injections table with dates in 2022, without an insulin bolus value and with insulin type 0.

- Done, TO BE VERIFIED. Also import UtcOffset into SensorRecords. The value is not provided in Abbott's data, so in the method:
internal override void InsertSensorMeasurements(List<GlucoseRecord> List)
we will derive the UTC from the closest record in the tables: Meals, GlucoseRecords and Injections.

## MealPage
???? DONE ???? (check)
On Android, the txtFoodCarbohydratesPerUnit, txtFoodQuantityInUnits and txtFoodCarbohydratesGrams entries do not accept the comma when the food is not "attached" to a row of the CollectionView below (when a new food is being entered in the entries). After the food has been passed to the CollectionView rows (with the + button), digits after the comma can be entered. On Windows this does not happen.


## FoodPage


## Charts


## Units of measure


# FatSecret
- Done. FatSecret no longer used for results under the free license: they are almost always in English and of little relevance, compared to the App, which has a very complete database of Italian foods.
