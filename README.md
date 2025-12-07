# GlucoProg 
"Gluco Programs". Programs for Glucose manamegent.

## GlucoMan
GlucoMan is a Glucose Manager program for the diabetic person.  
It helps with the daily operations needed to manage carbs counting. Currently it manages data related to: 
- measured glucose
- carbohydrates consumed during meals
- meals and foods carbohydrates
- insulin injections.  

It calculates: 
- the insulin bolus based on carbohydrates to be consumed during meals
- the foods to eat to reach the desired carbohydrates 
- a rough prediction of when blood sugar will reach a hypo value (to USE WITH CAUTION).  
- it shows statistics and graphs of glucose values and carbs consumed.

A page of the program helps the weighing of foods for carbs counting.  

The program has provisions to manage recipes of food, that are finished but still need to be tested and refined.  
Also the management of units of measurement of food is still to be refined.

The next field of action will be managing physical activities and alarms, but the code in this two cases is still not existing and we now have just two icons, that are usually kept disabled.  

The current version is developed in .Net MAUI, while earlier versions where written in Windows Forms and in Xamarin.

### Documentation
- **[Operation Manual](Documentation/GlucoMan_Operation_Manual.md)** - Complete guide to using GlucoMan with detailed descriptions of all features
- **[Quick Reference Guide](Documentation/GlucoMan_Quick_Reference.md)** - Quick reference for common tasks and workflows
## ConciliateData
CURRENTLY OUT OF THE SOLUTION  
This is a program that should have merged the data taken from the different sources of data that I use, that are:
- my program "GlucoMan", included in this solution
- the Android app "LibreView" that stores data about the glicemia measured by the Abbot sensor I wear.
- the Android app "Diabetes:M" that I used extensively in the past 
- the Android app "FatSecret", that manages a big database of foods.
Currently I excluded this program from the solution because I started implementing its functionalities in the UI program GlucoMan itself.

