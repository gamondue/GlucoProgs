# GlucoMan Quick Reference Guide

## Quick Start

### Recording a Glucose Measurement
1. Click **Blood Glucose** from main menu
2. Enter glucose value in mg/dL
3. Click **Now** button for current time (or manually enter date/time)
4. Add any notes
5. Click **Save**

### Recording a Meal
1. Click **Meal** from main menu
2. Click **Start** to set current time
3. Add foods and quantities
4. Click **Total CHO** to calculate carbohydrates
5. Click **Save**

### Calculating Insulin Bolus
1. Click **Insulin Calculation** from main menu
2. Select meal type (breakfast/lunch/dinner/snack)
3. Enter or click **Read glucose** for current glucose level
4. Enter or click **Read CHO** for carbohydrates to eat
5. Click **Calculate bolus**
6. Review the calculated bolus amount
7. Click **Injection** to record the injection

### Recording an Injection
1. Click **Injections** from main menu
2. Enter insulin amount
3. Select insulin type (short/slow acting)
4. Click **Now** for current time
5. Click **Save**

---

## Main Menu Quick Reference

| Icon | Function | Purpose |
|------|----------|---------|
| 🩸 Glucometer | Blood Glucose | Record glucose measurements |
| 💉 Syringe | Injections | Track insulin injections |
| 🧮 Calculator | Insulin Calculation | Calculate insulin doses |
| 🍽️ Meal | New Meal | Record current meal |
| 📋 Meals | Meals | View past meals |
| 🥗 Foods | Foods | Manage food database |
| 📖 Recipe | Recipes | Manage recipes |
| 📊 Chart | Charts & Statistics | View trends and statistics |
| ⏰ Clock | Prediction | Predict hypoglycemia timing |
| 🎯 Target | Food to Hit Target | Calculate food quantities |
| ⚙️ Gear | Configuration | Adjust settings |
| ❓ Question | About | Program information |

---

## Common Buttons and Icons

| Icon/Button | Meaning |
|-------------|---------|
| **+** (Plus) | Add new record |
| **-** (Minus) | Delete selected record |
| 💾 (Floppy Disk) | Save current data |
| 🔍 (Magnifier) | Search records |
| 🕐 (Clock) | Set to current time |
| ✓ (Choose) | Select this item |
| 🗑️ (Clear) | Clear all fields |
| ⬅️ (Back) | Return without saving |
| 📄 (Details) | View detailed information |

---

## Data Entry Shortcuts

### Time Entry
- **Now button**: Instantly sets current date and time
- **Add with Now as time**: Checkbox to automatically use current time when adding records

### Quick Data Reads
- **Read glucose**: Gets last glucose measurement
- **Read CHO**: Gets carbohydrates from current meal
- **Read Target CHO**: Gets target from insulin calculation
- **Read All**: Loads all relevant data at once

---

## Common Workflows

### Complete Meal Process
1. **Before Meal**: Record glucose (Blood Glucose page)
2. **Calculate**: Calculate insulin bolus (Insulin Calculation page)
3. **Record**: Log injection (Injections page)
4. **During Meal**: Record meal and foods (Meal page)
5. **Adjust**: Use "Food to Hit Target" if needed

### Adding a New Food
1. Foods page → **Clear Fields**
2. Enter name and description
3. Select unit
4. Enter CHO%
5. Click **Save**

### Checking for Hypoglycemia Risk
1. Hypoglycemia Prediction page
2. Click **Read** to load last two glucose measurements
3. Enter alarm glucose level
4. Click **Predict**
5. Review predicted time and alarm time

---

## Field Colors Meaning

| Color | Purpose |
|-------|---------|
| **Light Green** | User input fields (editable) |
| **Sky Blue/Aqua** | Calculated results (read-only) |
| **Pale Goldenrod** | Configuration parameters |
| **White** | System fields (ex. auto-generated IDs) |

---

## Important Parameters

### Insulin Calculation Parameters
- **CHO/Insulin Ratio**: From your dietitian (different for each meal type)
- **Insulin Sensitivity**: Glucose processed per unit of insulin
- **Target Glucose**: Desired glucose level
- **Embarked Insulin**: Previously injected insulin still active

### Meal Accuracy
- **Numerical**: Percentage accuracy (0-100%)
- **Qualitative**: Word description (e.g., "poor", "good", ..)

---

## Units Reference

| Measurement | Unit | Notes |
|-------------|------|-------|
| Glucose | mg/dL | Milligrams per deciliter |
| Carbohydrates | g | Grams |
| Insulin | UI | Units of insulin |
| CHO% | % | Percentage (grams in 100g of food) |
| Time | HH:mm:ss | 24-hour international format |

---

## Tips for Accurate Data

### Glucose Measurements
- Record immediately after measuring
- Note any unusual circumstances in Notes field
- Check for sensor malfunctions

### Meals
- Weigh foods when possible for accuracy
- Record all components of complex meals
- Use recipes for repeated meal combinations
- Update accuracy field based on confidence

### Insulin
- Always verify calculated dose before injecting
- Record actual injected amount (may differ from calculated)
- Track injection sites using Front/Back/Hands pages
- Note insulin brand and type

---

## Troubleshooting Quick Tips

**Insulin calculation seems wrong?**
- Verify CHO/Insulin ratios are correct
- Check sensitivity factor
- Ensure glucose reading is current
- Consider embarked insulin

**Can't find a food?**
- Use Search function
- Check spelling
- Add new food if needed

**Predictions seem inaccurate?**
- Ensure two recent glucose measurements
- Use only when glucose is changing steadily
- Remember: predictions are estimates only
- Consult healthcare provider

---

## Safety Reminders

⚠️ **Always:**
- Verify insulin calculations with your own judgment
- Consult your healthcare provider for dosing decisions
- Use predictions as estimates only, not definitive diagnoses
- Record actual injected amounts, not just calculated ones
- Keep your parameters updated with your doctor's guidance

⚠️ **Never:**
- Rely solely on app calculations for critical decisions
- Use hypoglycemia predictions as a substitute for regular monitoring
- Ignore symptoms even if app predictions differ
- Share your device without protecting your health data

---

## Data Management

### Regular Maintenance
- Review statistics weekly
- Update food database as you discover CHO values
- Clean up duplicate entries
- Export data periodically (if feature available)

### Before Healthcare Visits
1. Review Charts and Statistics
2. Note any patterns or concerns
3. Prepare questions about parameter adjustments
4. Export relevant data if requested

---

## Keyboard Navigation

- **Enter**: Confirm/Next field
- **Tab**: Move to next field
- **Escape**: Cancel/Go back

---

## Common Calculations

### Insulin Bolus
```
Bolus = CHO Insulin + Correction Insulin - Embarked Insulin

Where:
- CHO Insulin = Meal CHO / CHO-Insulin Ratio
- Correction Insulin = (Current Glucose - Target) / Sensitivity
```

### Food to Hit Target
```
Food to Eat = (Target CHO - Already Eaten) / (Food CHO% / 100)
```

### Meal Accuracy (Composed)
Calculated from individual food accuracy values when using **Total CHO** button

---

## Support Resources

- **In-app**: Click About button for version info
- **Documentation**: See full Operation Manual for detailed instructions
- **Healthcare**: Always consult your diabetes care team for medical decisions

---

*Quick Reference Guide - GlucoMan*
*For detailed information, see the complete Operation Manual*
