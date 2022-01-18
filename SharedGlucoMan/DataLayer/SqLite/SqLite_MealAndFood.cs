using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan
{
    public  partial class DL_Sqlite : DataLayer
    {
        public  override List<Meal> ReadMeals(DateTime? InitialInstant, DateTime? FinalInstant)
        {
            List<Meal> list = new List<Meal>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Meals ";
                    if (InitialInstant != null && FinalInstant != null)
                    {   // add WHERE clause
                        query += " WHERE TimeBegin BETWEEN " + ((DateTime)InitialInstant).ToString("YYYY-MM-DD") +
                            " AND " + ((DateTime)FinalInstant).ToString("YYYY-MM-DD");
                    }
                    query += " ORDER BY TimeBegin DESC";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        Meal g = GetMealFromRow(dRead);
                        list.Add(g);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | RestoreMeals", ex);
            }
            return list;
        }
        public  override void SaveMeals(List<Meal> List)
        {
            try
            {
                foreach (Meal rec in List)
                {
                    SaveOneMeal(rec);
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | SaveMeals", ex);
            }
        }
        public  override long? SaveOneMeal(Meal Meal)
        {
            try
            {
                if (Meal.IdMeal == null || Meal.IdMeal == 0)
                {
                    Meal.IdMeal = GetNextTablePrimaryKey("Meals", "IdMeal"); 
                    Meal.TimeStart.DateTime = DateTime.Now;
                    //Meal.TimeEnd.DateTime =  DateTime.Now;
                    // INSERT new record in the table
                    InsertMeal(Meal);
                }
                else
                {   // GlucoseMeasurement.IdGlucoseRecord exists
                    UpdateMeal(Meal);
                }
                return Meal.IdGlucoseRecord;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | SaveOneMeal", ex);
                return null;
            }
        }
        private Meal GetMealFromRow(DbDataReader Row)
        {
            Meal m = new Meal();
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                m.IdMeal = Safe.Int(Row["IdMeal"]);
                m.TypeOfMeal = (TypeOfMeal) Safe.Int(Row["TypeOfMeal"]);
                m.Carbohydrates.Double = Safe.Double(Row["Carbohydrates"]);
                m.TimeStart.DateTime = (DateTime) Safe.DateTime(Row["TimeBegin"]);
                m.TimeEnd.Text = Safe.String(Row["TimeEnd"]);
                m.AccuracyOfChoEstimate = (double)Safe.Double(Row["AccuracyOfChoEstimate"]);
                m.IdBolusCalculation = Safe.Int(Row["IdBolusCalculation"]);
                m.IdGlucoseRecord = Safe.Int(Row["IdGlucoseRecord"]);
                m.QualitativeAccuracyOfChoEstimate = (QualitativeAccuracy) Safe.Int(Row["IdQualitativeAccuracyCHO"]);
                m.IdInsulineInjection = Safe.Int(Row["IdInsulineInjection"]);
                //m = Safe(Row["xxxx"]);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | GetMealFromRow", ex);
            }
            return m;
        }
        private long? UpdateMeal(Meal Meal)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE Meals SET " +
                    "Carbohydrates=" + SqlDouble(Meal.Carbohydrates.Text) + "," +
                    "TimeBegin=" + SqlDate(Meal.TimeStart.DateTime) + "," +
                    "TimeEnd=" + SqlDate(Meal.TimeEnd.DateTime) + "," +
                    "AccuracyOfChoEstimate=" + SqlDouble(Meal.AccuracyOfChoEstimate) + "," +
                    "IdBolusCalculation=" + SqlInt(Meal.IdBolusCalculation) + "," +
                    "IdGlucoseRecord=" + SqlInt(Meal.IdGlucoseRecord) + "," +
                    //"IdQualitativeAccuracyCho=" + SqlString(Meal.IdQualitativeAccuracyCho) + "," +
                    "IdInsulineInjection=" + SqlInt(Meal.IdInsulineInjection) + "" +
                    // "XXXXX=" + SqlString(Meal.) + "," +
                    " WHERE IdMeal=" + SqlInt(Meal.IdMeal) + 
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Meal.IdGlucoseRecord;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | UpdateMeal", ex);
                return null;
            }
        }
        private long? InsertMeal(Meal Meal)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Meals" +
                    "(" +
                    "IdMeal,Carbohydrates,TimeBegin,TimeEnd,AccuracyOfChoEstimate," +
                    "IdBolusCalculation,IdGlucoseRecord," +
                    "IdInsulineInjection";
                    //"XXXX,XXXX,XXXX" +
                    query += ")VALUES(" +
                    SqlInt(Meal.IdMeal) + "," +
                    SqlDouble(Meal.Carbohydrates) + "," +
                    SqlDate(Meal.TimeStart.DateTime) + "," +
                    SqlDate(Meal.TimeEnd.DateTime) + "," +
                    SqlDouble(Meal.AccuracyOfChoEstimate) + "," +
                    SqlDouble(Meal.IdBolusCalculation) + "," +
                    SqlInt(Meal.IdGlucoseRecord) + "," +
                    SqlInt(Meal.IdInsulineInjection);
                    // SqlString(Meal.) + "," +
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Meal.IdMeal;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | UpdateMeal", ex);
                return null;
            }
        }
        public  override void SaveFoodsInMeal(List<FoodInMeal> Meal)
        {

        }
        public  override List<FoodInMeal> ReadFoodsInMeal(int? IdMeal)
        {
            if (IdMeal != null)
            {
                List<FoodInMeal> FoodsInMeal = new List<FoodInMeal>();
                try
                {
                    DbDataReader dRead;
                    DbCommand cmd;
                    using (DbConnection conn = Connect())
                    {
                        string query = "SELECT *" +
                            " FROM FoodsInMeals" +
                            //" JOIN Meals ON Meals.IdMeal=FoodsInMeals.IdMeal" +
                            " WHERE FoodsInMeals.IdMeal=" + IdMeal +
                            ";";
                        // query += " ORDER BY Timestamp DESC";
                        cmd = new SqliteCommand(query);
                        cmd.Connection = conn;
                        dRead = cmd.ExecuteReader();
                        while (dRead.Read())
                        {
                            FoodInMeal f= new FoodInMeal();
                            f = GetFoodInMealFromRow(dRead);
                            FoodsInMeal.Add(f);
                        }
                        dRead.Dispose();
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Common.LogOfProgram.Error("Sqlite_MealAndFood | RestoreFoodsInMeal", ex);
                }
                return FoodsInMeal;
            }
            return null;
        }
        private FoodInMeal GetFoodInMealFromRow(DbDataReader Row)
        {
            FoodInMeal f = new FoodInMeal();
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                f.IdFoodInMeal = Safe.Int(Row["IdFoodInMeal"]);
                f.IdMeal = Safe.Int(Row["IdMeal"]);
                f.IdFood = Safe.Int(Row["IdFood"]);
                f.Quantity.Double = Safe.Double(Row["Quantity"]);
                f.CarbohydratesGrams.Double = Safe.Double(Row["CarbohydratesGrams"]);
                f.CarbohydratesPercent.Double = Safe.Double(Row["CarbohydratesPercent"]);
                f.AccuracyOfChoEstimate.Double = Safe.Double(Row["AccuracyOfChoEstimate"]);
                f.SugarPercent.Double = Safe.Double(Row["SugarPercent"]);
                f.FibersPercent.Double = Safe.Double(Row["FibersPercent"]);
                f.Name = Safe.String(Row["Name"]);
                f.Description = Safe.String(Row["Description"]);
                //f. = Safe(Row["XXXXX"]);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | GetFoodFromRow", ex);
            }
            return f;
        }
        public  override int? SaveOneFoodInMeal(FoodInMeal FoodToSave)
        {
            try
            {
                if (FoodToSave.IdMeal != null)
                {
                    if (FoodToSave.IdFoodInMeal == null || FoodToSave.IdFoodInMeal == 0)
                    {
                        FoodToSave.IdFoodInMeal = GetNextPrimaryKey();
                        // INSERT new record in the table
                        InsertFoodInMeal(FoodToSave);
                    }
                    else
                    {   // GlucoseMeasurement.IdGlucoseRecord exists
                        UpdateFoodInMeal(FoodToSave);
                    }
                    return FoodToSave.IdFoodInMeal;
                }
                else
                {
                    Common.LogOfProgram.Error("Sqlite_MealAndFood | SaveOneFoodInMeal", 
                        new Exception("FoodInMeal instance must have an IdMeal"));
                    return null; 
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | SaveOneFoodInMeal", ex);
                return null;
            }
        }
        private int? UpdateFoodInMeal(FoodInMeal FoodToSave)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE Meals SET " +
                    "IdFood=" + SqlDouble(FoodToSave.IdFood) + "," +
                    "Quantity=" + SqlDouble(FoodToSave.Quantity) + "," +
                    "CarbohydratesGrams=" + SqlDouble(FoodToSave.CarbohydratesGrams) + "," +
                    "CarbohydratesPercent=" + SqlDouble(FoodToSave.CarbohydratesPercent) + "," +
                    "AccuracyOfChoEstimate=" + SqlDouble(FoodToSave.AccuracyOfChoEstimate) + "," +
                    "SugarPercent=" + SqlDouble(FoodToSave.SugarPercent) + "," +
                    "FibersPercent=" + SqlDouble(FoodToSave.FibersPercent) + "," +
                    "Name=" + SqlString(FoodToSave.Name) + "," +
                    "Description=" + SqlString(FoodToSave.Description) + "," +
                    // "XXXXX=" + SqlString(FoodToSave) + "," +
                    " WHERE IdMeal=" + SqlInt(FoodToSave.IdFoodInMeal) + 
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return FoodToSave.IdFoodInMeal;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | UpdateMeal", ex);
                return null;
            }
        }
        private int? InsertFoodInMeal(FoodInMeal FoodToSave)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO FoodsInMeals" +
                    "(" +
                    "IdFoodInMeal,IdMeal,IdFood,Quantity,CarbohydratesGrams,CarbohydratesPercent," +
                    "AccuracyOfChoEstimate,SugarPercent,FibersPercent," +
                    "Name,Description";
                    query += ")VALUES(" +
                    SqlInt(FoodToSave.IdFoodInMeal) + "," +
                    SqlInt(FoodToSave.IdMeal) + "," +
                    SqlInt(FoodToSave.IdFood) + "," +
                    SqlDouble(FoodToSave.Quantity.Double) + "," +
                    SqlDouble(FoodToSave.CarbohydratesGrams.Double) + "," +
                    SqlDouble(FoodToSave.CarbohydratesPercent.Double) + "," +
                    SqlDouble(FoodToSave.AccuracyOfChoEstimate.Double) + "," +
                    SqlDouble(FoodToSave.SugarPercent.Double) + "," +
                    SqlDouble(FoodToSave.FibersPercent.Double) + "," +
                    SqlString(FoodToSave.Name) + "," +
                    SqlString(FoodToSave.Description);

                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return FoodToSave.IdFoodInMeal;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | UpdateMeal", ex);
                return null;
            }
        }
        private Food GetFoodFromRow(DbDataReader Row)
        {
            Food f = new Food();
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                f.IdFood = Safe.Int(Row["IdFood "]);
                //f.TypeOfMeal = (TypeOfMeal)Safe.Int(Row["TypeOfMeal"]);
                //f.Carbohydrates.Double = Safe.Double(Row["Carbohydrates"]);
                //f.TimeBegin.DateTime = (DateTime)Safe.DateTime(Row["TimeBegin"]);
                //f.TimeEnd.Text = Safe.String(Row["TimeEnd"]);
                //f.AccuracyOfChoEstimate = (double)Safe.Double(Row["AccuracyOfChoEstimate"]);
                //f.IdBolusCalculation = Safe.Int(Row["IdBolusCalculation"]);
                //f.IdGlucoseRecord = Safe.Int(Row["IdGlucoseRecord"]);
                //f.QualitativeAccuracyOfChoEstimate = (QualitativeAccuracy)Safe.Int(Row["IdQualitativeAccuracyCHO"]);
                //f.IdInsulineInjection = Safe.Int(Row["IdInsulineInjection"]);
                //m = Safe(Row["xxxx"]);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | GetFoodFromRow", ex);
            }
            return f;
        }
    }
}
