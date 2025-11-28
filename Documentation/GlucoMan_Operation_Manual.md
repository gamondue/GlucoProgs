# GlucoMan Operation Manual

## Table of Contents
1. [Introduction](#introduction)
2. [Main Menu](#main-menu)
3. [Glucose Management](#glucose-management)
4. [Insulin Management](#insulin-management)
5. [Meals and Foods](#meals-and-foods)
6. [Recipes](#recipes)
7. [Predictions and Analysis](#predictions-and-analysis)
8. [Configuration and Settings](#configuration-and-settings)

---

## Introduction

GlucoMan is a comprehensive diabetes management application designed to help users track glucose levels, calculate insulin doses, manage meals, and monitor carbohydrate intake. This manual provides detailed instructions on how to use each feature of the application based on the tooltips and interface elements.

---

## Main Menu

The main menu provides access to all major features of GlucoMan. Each button opens a specific page for managing different aspects of diabetes care.

### Navigation Buttons

- **Blood Glucose** - Opens the page of glucose measurement events where you can record and view blood glucose readings
- **Insulin Calculation** - Opens a page that calculates the quantity of insulin to inject before a meal
- **Injections** - Opens the page of past and present insulin injections
- **Food to Hit Target** - Opens the page that calculates the food you need to eat to hit a carbohydrates target
- **Meals** - Opens the page of meals done in the past
- **Meal** - Opens a new meal page for recording a current meal
- **Foods** - Opens the page for foods management
- **Recipes** - Opens the page of recipes management
- **Charts and Statistics** - Opens the page that searches past data, makes statistics and charts
- **Prediction** - Opens the page that makes a simple prediction of the time of a hypoglycemia episode
- **Miscellaneous** - Opens the page of miscellaneous functions
- **Alarms** - Opens the page of the alarms settings
- **Configurations** - Opens the page that assigns the program's parameters
- **About** - Opens the page of information about this program

---

## Glucose Management

### Glucose Measurements Page

This page allows you to record and manage blood glucose measurements.

#### Data Entry Fields

- **Glucose [mg/dL]** - The glucose measurement taken from your glucometer or sensor
- **Measurement Date** - Date when the glucose measurement was taken
- **Measurement Time** - Time when the glucose measurement was taken
- **Notes** - Add any notes about the measurement (such as context, symptoms, or special circumstances)
- **Id** - System-generated code for this measurement

#### Action Buttons

- **Now** - Sets the date and time to the current instant
- **Plus (+)** - Adds a new measurement with the data taken from the UI
- **Minus (-)** - Deletes the selected measurement
- **Clear Data** - Deletes the fields in the UI
- **Save** - Saves the modifications made on the current measurement

#### Special Features

- **Add with Now as time** checkbox - When checked, automatically uses the current date and time when adding a new measurement

#### Measurements List

The lower section displays a list of past glucose measurements with their timestamps and values.

---

## Insulin Management

### Insulin Calculation Page

This page helps calculate the correct insulin bolus for meals.

#### Input Parameters

**CHO/Insulin Ratios (from dietitian)**
- **Morning CHO/insulin** - Carbohydrates per unit of insulin for breakfast time [CHO/UI]
- **Midday CHO/insulin** - Carbohydrates per unit of insulin for lunch time [CHO/UI]
- **Evening CHO/insulin** - Carbohydrates per unit of insulin for dinner time [CHO/UI]

**Glucose Values**
- **Current glucose** - Glucose measured before the meal [mg/dL]
- **Target** - Glucose level to reach with calculated bolus [mg/dL]
- **To correct** - Calculated glucose difference to be corrected
- **Target CHO** - Carbohydrates amount to eat to reach target glucose level [g]

**Sensitivity Parameters**
- **Blood glucose/insulin** - Glucose processed by one unit of insulin [mg/(dl×UI)]
- **Embarked insulin** - Amount of insulin taken previously and still effective

#### Meal Type Selection

Choose the type of meal using radio buttons:
- **Breakfast (b.fast)** - For morning meals
- **Lunch** - For midday meals
- **Dinner** - For evening meals
- **Snack** - For meals outside main meal times

#### Results Display

The page displays four calculated insulin values:
- **Correction insulin** - Insulin that brings blood glucose level to target glucose
- **CHO insulin** - Insulin that reduces the amount of carbohydrates ingested during the meal
- **Total excluding embarked** - Insulin of the bolus when no insulin is left in the body
- **Bolus** - Final calculated insulin bolus to inject (displayed prominently)

#### Action Buttons

- **Calculate bolus** - Calculates the insulin bolus for this meal
- **Parameters** - Opens the page that calculates or sets sensitivity parameters
- **Round** - Calculates the CHO to eat to inject an integer value of the insulin bolus
- **Read glucose** - Reads the last value of blood glucose stored in the database
- **Measure Glucose** - Opens the page that stores measured glucose in the database
- **Injection** - Opens the page of injections to make a new entry
- **Read CHO** - Reads the value of carbohydrates of the current meal

### Injections Page

This page manages all insulin and other injections.

#### Injection Data Entry

- **Bolus** - Insulin actually injected [UI]
- **Calculated** - Insulin bolus calculated by this program (currently not used)
- **Code of this injection** - System-generated identifier
- **Notes** - Notes about this injection

#### Injection Type

Radio buttons to select insulin type:
- **Short insulin** - Short-acting insulin
- **Slow insulin** - Long-acting insulin

#### Timing

- **Date** - Date of this injection
- **Time** - Time of this injection

#### Action Buttons

- **Front** - Opens a new Front injections picture page
- **Back** - Opens a new Back injections picture page
- **Hands** - Opens a new Hands blood samples positions picture page
- **Sensors** - Opens a new Sensors positions picture page
- **Now** - Sets the time to the current instant
- **Clear fields** - Clears all data in the UI
- **Plus (+)** - Adds a new injection
- **Minus (-)** - Removes this injection
- **Save** - Saves this injection to the database

#### Filter Options

Checkboxes to filter the injection list:
- **Filter front** - Shows injections made in the Front zone
- **Filter back** - Shows injections made in the Back zone
- **Filter hands** - Shows blood sampling punctures
- **Filter sensor** - Shows sensors installed

#### Special Features

- **Manual/Automatic time** checkbox - When checked, sets 'now' as the time of a new event; when unchecked, uses the time shown in the UI

#### Injections List

The lower section displays past injections with columns for:
- Time
- Insulin quantity (Ins.) [UI]
- Type of injection done (Inj.)
- Type of insulin action (Act.)
- Brand of insulin injected

---

## Meals and Foods

### Meals Page

This page allows you to record and review past meals.

#### Meal Type Selection

Radio buttons to specify meal type:
- **Breakfast (b.fast)** - Breakfast meal
- **Lunch** - Lunch meal
- **Dinner (dinn.)** - Dinner meal
- **Snack** - Snack, out of main meals

#### Meal Data

- **CHO** - Total carbohydrates of this meal [g]
- **Accuracy** (numerical) - Numerical accuracy of meal in percent
- **Accuracy** (qualitative) - Qualitative accuracy of meal as a word
- **Code of this meal** - System identifier
- **Begin date** - Date of the beginning of the meal
- **Begin time** - Time of the beginning of the meal
- **Notes** - Notes about the meal

#### Action Buttons

- **Save** - Saves the meal with the current data of the UI
- **Now** - Sets the date and time at the current instant
- **Plus (+)** - Adds a new meal with data from UI
- **Minus (-)** - Deletes the selected meal
- **Details** - Opens the page with additional data about the meal
- **Clear fields** - Clears all data in the UI

#### Special Features

- **Add with Now as time** checkbox - When checked, uses the current instant (now) when adding a new meal; when unchecked, uses the value in the UI

#### Past Meals List

Displays previous meals with:
- Date and Time
- CHO [g]
- Type (meal type code)
- Accuracy %

### Meal Page (Single Meal Detail)

This page provides detailed management of a single meal with its components.

#### Meal Section

**Meal Summary Data**
- **CHO** - Total carbohydrates of this meal [g]
- **Accuracy** (percent) - Percent numerical accuracy of the meal
- **Accuracy** (qualitative) - Qualitative accuracy of the meal, as a word
- **Code of the meal** - System identifier
- **Notes** - Notes about the meal

#### Quick Action Buttons

- **Save** - Saves in the current meal the data currently in the UI
- **Start** - Saves the current time as the starting time of the meal
- **Gluc.value** - Opens the page of glucose measurements
- **Bolus Calc** - Opens the page that calculates insulin bolus for the meal
- **Injection** - Opens the page of insulin injections
- **Total CHO** - Calculates the summary data of the meal: total carbohydrates and composed accuracy
- **Food to target** - Opens the page that calculates the food to eat to reach target carbohydrates

### Foods Page

This page manages the food database used for meal planning.

#### Food Information

- **Name** - Short name of this food
- **Units** - List of units used for this food (selected from dropdown)
- **Code of this food** - System identifier
- **Description** - Longer description of this food
- **Percent Carbohydrates** - Carbohydrates of this food in 100 g [%]

#### Action Buttons

- **Back** - Returns to the calling page without saving changes
- **Choose** - Chooses this food to be used by the calling page
- **Plus (+)** - Adds a new food with data from UI
- **Minus (-)** - Deletes the selected food
- **Save** - Saves the food with the current data of the UI
- **Details** - Opens the page with additional data about the food
- **Clear Fields** - Clears all data in the UI
- **Search** - Searches the foods matching the filter that is in the UI

#### Foods List

Displays available foods with:
- Food name
- CHO% (carbohydrate percentage)

### Food to Hit Target Page

This page calculates how much of a specific food you need to eat to reach your target carbohydrates.

#### Input Fields

- **Carbohydrates we want to reach** - Target CHO [g]
- **Carbohydrates already eaten** - CHO already taken [g]
- **Carbohydrates % of the food** - CHO food to eat [%]
- **Short name of the food** - The food you want to eat

#### Results

- **CHO left** - Calculated carbohydrates to be eaten to reach the target [g]
- **Food to eat** - Grams of the chosen food one has to eat to reach the target carbohydrates [g]

#### Action Buttons

- **Food Calc** - Calculates the grams of chosen food you need to eat to reach the target carbohydrates
- **Read Target CHO** - Reads from the bolus calculation page the CHO to eat
- **Read eaten CHO** - Reads from the meal page the quantity of carbohydrates already eaten
- **Read food CHO** - Reads from the meal page the CHO% of the food that is selected
- **Read all** - Reads all the parameters useful to calculate the CHO that has still to be eaten
- **Insulin** - Opens the insulin injections page

---

## Recipes

### Recipes Page

This page manages recipes for complex meals.

#### Recipe Information

- **Name** - Short name of this recipe
- **Code of this recipe** - System identifier
- **Description** - Longer description of this recipe
- **CHO%** - Total carbohydrates % of this recipe [%]
- **Cooked recipe** checkbox - If checked, the carbohydrates regard a cooked recipe; if not checked, a raw recipe
- **Cooked to raw ratio** - Ratio between the cooked weight and the raw weight of the recipe

#### Action Buttons

- **Chooses this recipe for the meal** - Selects this recipe to use in the current meal
- **Plus (+)** - Adds a new recipe
- **Minus (-)** - Deletes the selected recipe
- **Save** - Saves the recipe
- **Details** - Opens detailed recipe information
- **Clear Fields** - Clears all data fields
- **Search** - Searches recipes

---

## Predictions and Analysis

### Hypoglycemia Prediction Page

This page predicts when your glucose levels might reach hypoglycemic levels based on current trends.

#### WARNING
⚠️ **This feature uses linear extrapolation and should only be used under VERY STRICT conditions. Always consult with your healthcare provider and use clinical judgment.**

#### Input Parameters

**Alarm Settings**
- **Alarm value** - Low glucose at which you want to take action against hypoglycemia [mg/dL]
- **Time advance** - Time of anticipated alarm, with respect to the alarm value time [minutes]

**Current Measurement**
- **Glucose measured the last time** - Most recent glucose reading [mg/dL]
- **Hours of the last measurement**
- **Minutes of the last measurement**

**Previous Measurement**
- **Glucose measured the previous time** - Previous glucose reading [mg/dL]
- **Hours of the previous measurement**
- **Minutes of the previous measurement**

#### Results

- **Slope** - Rate of change of glucose [glucose units/hour]
- **Expected time** - Hours and minutes of the predicted 'low glucose' event
- **Alarm time** - Hours and minutes of the alarm event for low glucose

#### Future Glucose Calculation

This section allows calculation of glucose at a future time point:
- **Difference in minutes for prediction** - Time from current measurement [min]
- **Hour and time of prediction** - Calculated future time
- **Predicted glucose** - Predicted glucose at 'future time' (USE WITH CAUTION!)

#### Action Buttons

- **Enter Now time** - Copies the current day and time to the date of the current glucose measurement
- **Enter the next glucose measurement** - Moves the current glucose measurement into the previous slot and copies time now in latest glucose measurement
- **Read** - Reads the last two glucose measurements from the database
- **Predict** - Predicts the time at which the glucose will reach the value for alarm (prediction by linear extrapolation)
- **Calculates the future glucose** - Calculates future glucose after 'Time diff' minutes from 'current measurement' time using the shown 'slope'

### Charts and Statistics Page

Opens the page that allows you to:
- Search past data
- Generate statistics
- View charts of glucose trends, insulin usage, and carbohydrate intake

---

## Configuration and Settings

### Configuration Page

Opens the page where you can assign the program's parameters, including:
- Meal timing preferences
- Default values
- User preferences

### Alarms Page

Opens the page where you can configure alarm settings for:
- Hypoglycemia predictions
- Medication reminders
- Other diabetes management notifications

---

## Calculator

GlucoMan includes a built-in calculator for quick calculations. It supports:
- Basic arithmetic operations (+, -, ×, ÷)
- Decimal numbers
- Clear (CE) and Clear All (C) functions
- Backspace for corrections
- OK to accept and return the result
- Escape to cancel

---

## General Tips

### Common Interface Elements

**Standard Buttons**
- **Plus (+)** - Adds new records
- **Minus (-)** - Deletes selected records
- **Save (floppy disk icon)** - Saves current data
- **Clear Fields** - Resets form to default values
- **Now (clock icon)** - Sets current date/time
- **Back/Escape** - Returns to previous page

**Color Coding**
- **Light Green / Pale Green** - Editable input fields
- **Sky Blue / Aqua** - Calculated results
- **Pale Goldenrod** - Configuration/parameter fields
- **White** - Read-only system fields

### Data Entry Tips

1. Always save your data using the Save button before navigating away
2. Use the "Now" button for quick timestamp entry
3. The "Add with Now as time" checkbox helps speed up data entry for current events
4. Review calculated values before saving
5. Use notes fields to add context that might be useful later

### Best Practices

1. **Record glucose measurements regularly** - This helps with accurate predictions and trend analysis
2. **Log meals and carbohydrates accurately** - Essential for proper insulin calculations
3. **Keep injection records up to date** - Important for tracking embarked insulin
4. **Review statistics regularly** - Helps identify patterns and improve diabetes management
5. **Update your sensitivity parameters** - Work with your healthcare provider to keep these current

---

## Support and Additional Information

For more information about GlucoMan, use the **About** button from the main menu. This provides version information and additional details about the application.

---

*Last Updated: 2024*
*This manual is based on GlucoMan's interface tooltips and may be updated as the application evolves.*
