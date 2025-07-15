# GlucoProg 
"Gluco Programs". Programs for Glucose manamegent.
## GlucoMan
GlucoMan is a Glucose Manager program for the diabetic person.  
Helps with the daily operations needed to manage carbs counting. Currently manages data related to: measured glucose, carbohydrates consumed during meals, foods carbohydrates, insulin injections.  
Calculates: the insulin bolus based on carbohydrates to be consumed during meals, the foods to eat to reach the desired carbohydrates and also a rough prediction of when blood sugar will reach a hypo value.  
The program has provisions to manage recipes, still not completed. Prototype functions for weighing foods are also included in the code, but disabled in the UI, since they are  still in very preliminary development.
The current version is developed in .Net MAUI, while earlier versions where written in Windows Forms and in Xamarin.
## ConciliateData
Program that will merge the data taken from the different sources of data that I use, tha are:
- my program "GlucoMan"
- the Android app "LibreView" that stores data about the glicemia measured by the Abbot sensor I wear. 
- the Android app "Diabetes:M" that I used extensively in the past 
- the Android app "FatSecret", that manages a big database od foods.
Still I am NOT using this program, so the code is preliminary and still not working at all. 

