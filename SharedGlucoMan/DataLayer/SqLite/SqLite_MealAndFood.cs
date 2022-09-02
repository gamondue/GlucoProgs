using gamon;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using static GlucoMan.Common;

namespace GlucoMan
{
    internal  partial class DL_Sqlite : DataLayer
    {
        internal override List<Meal> GetMeals(DateTime? InitialInstant, DateTime? FinalInstant)
        {
            List<Meal> list = new List<Meal>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Meals";
                    if (InitialInstant != null && FinalInstant != null)
                    {   // add WHERE clause
                        query += " WHERE TimeBegin BETWEEN " + SqliteHelper.Date(((DateTime)InitialInstant).ToString("yyyy-MM-dd")) +
                            " AND " + SqliteHelper.Date(((DateTime)FinalInstant).ToString("yyyy-MM-dd 23:59:29")) + "";
                    }
                    query += " ORDER BY TimeBegin DESC, IdMeal;";
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
                Common.LogOfProgram.Error("Sqlite_MealAndFood | ReadMeals", ex);
            }
            return list;
        }
        internal override Meal GetOneMeal(int? IdMeal)
        {
            Meal food = new Meal();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Meals" +
                        " WHERE IdMeal=" + IdMeal;
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    dRead.Read();
                    food = GetMealFromRow(dRead);
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | GetOneMeal", ex);
            }
            return food;
        }
        internal override void SaveMeals(List<Meal> List)
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
        internal override int? SaveOneMeal(Meal Meal)
        {
            try
            {
                if (Meal.IdMeal == null || Meal.IdMeal == 0)
                {
                    Meal.IdMeal = GetNextTablePrimaryKey("Meals", "IdMeal"); 
                    // INSERT new record in the table
                    InsertMeal(Meal);
                }
                else
                {   // GlucoseMeasurement.IdGlucoseRecord exists
                    UpdateMeal(Meal);
                }
                return Meal.IdMeal;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | SaveOneMeal", ex);
                return null;
            }
        }
        internal override void DeleteOneMeal(Meal Meal)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM Meals" +
                    " WHERE IdMeal=" + Meal.IdMeal; 
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | DeleteOneMeal", ex);
            }
        }
        internal Meal GetMealFromRow(DbDataReader Row)
        {
            Meal m = new Meal();
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                m.IdMeal = Safe.Int(Row["IdMeal"]);
                if (Row["IdTypeOfMeal"] is DBNull)
                    m.IdTypeOfMeal = 0; 
                else
                    m.IdTypeOfMeal = (TypeOfMeal)Safe.Int(Row["IdTypeOfMeal"]);
                m.Carbohydrates.Double = Safe.Double(Row["Carbohydrates"]);
                m.TimeBegin.DateTime = Safe.DateTime(Row["TimeBegin"]);
                m.Notes = Safe.String(Row["Notes"]); 
                m.AccuracyOfChoEstimate.Double = Safe.Double(Row["AccuracyOfChoEstimate"]);
                m.IdBolusCalculation = Safe.Int(Row["IdBolusCalculation"]);
                m.IdGlucoseRecord = Safe.Int(Row["IdGlucoseRecord"]);
                m.IdInsulinInjection = Safe.Int(Row["IdInsulinInjection"]);
                m.TimeEnd.DateTime = Safe.DateTime(Row["TimeEnd"]);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | GetMealFromRow", ex);
            }
            return m;
        }
        internal int? UpdateMeal(Meal Meal)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE Meals SET " +
                    "IdTypeOfMeal=" + SqliteHelper.Int((int)Meal.IdTypeOfMeal) + "," +
                    "Carbohydrates=" + SqliteHelper.Double(Meal.Carbohydrates.Text) + "," +
                    "TimeBegin=" + SqliteHelper.Date(Meal.TimeBegin.DateTime) + "," +
                    "Notes=" + SqliteHelper.String(Meal.Notes) + "," + 
                    "AccuracyOfChoEstimate=" + SqliteHelper.Double(Meal.AccuracyOfChoEstimate.Double) + "," +
                    "IdBolusCalculation=" + SqliteHelper.Int(Meal.IdBolusCalculation) + "," +
                    "IdGlucoseRecord=" + SqliteHelper.Int(Meal.IdGlucoseRecord) + "," +
                    "IdInsulinInjection=" + SqliteHelper.Int(Meal.IdInsulinInjection) + "," +
                    "TimeEnd=" + SqliteHelper.Date(Meal.TimeEnd.DateTime) + "" +
                    " WHERE IdMeal=" + SqliteHelper.Int(Meal.IdMeal) + 
                    ";";
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
        internal int? InsertMeal(Meal Meal)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Meals" +
                    "(" +
                    "IdMeal,IdTypeOfMeal,Carbohydrates,TimeBegin,Notes,AccuracyOfChoEstimate," +
                    "IdBolusCalculation,IdGlucoseRecord,IdInsulinInjection,TimeEnd";
                    query += ")VALUES(" +
                    SqliteHelper.Int(Meal.IdMeal) + "," +
                    SqliteHelper.Int((int)Meal.IdTypeOfMeal) + "," +
                    SqliteHelper.Double(Meal.Carbohydrates.Double) + "," +
                    SqliteHelper.Date(Meal.TimeBegin.DateTime) + "," +
                    SqliteHelper.String(Meal.Notes) + "," +
                    SqliteHelper.Double(Meal.AccuracyOfChoEstimate.Double) + "," +
                    SqliteHelper.Double(Meal.IdBolusCalculation) + "," +
                    SqliteHelper.Int(Meal.IdGlucoseRecord) + "," +
                    SqliteHelper.Int(Meal.IdInsulinInjection) + "," +
                    SqliteHelper.Date(Meal.TimeEnd.DateTime) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Meal.IdMeal;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | InsertMeal", ex);
                return null;
            }
        }
        internal override void SaveFoodsInMeal(List<FoodInMeal> Meal)
        {
            throw new NotImplementedException();
        }
        internal override List<FoodInMeal> GetFoodsInMeal(int? IdMeal)
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
                            " WHERE FoodsInMeals.IdMeal=" + IdMeal +
                            " ORDER BY IdFoodInMeal DESC" +
                            ";";
                        cmd = new SqliteCommand(query);
                        cmd.Connection = conn;
                        dRead = cmd.ExecuteReader();
                        while (dRead.Read())
                        {
                            FoodInMeal f = GetFoodInMealFromRow(dRead);
                            FoodsInMeal.Add(f);
                        }
                        dRead.Dispose();
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Common.LogOfProgram.Error("Sqlite_MealAndFood | GetFoodsInMeal", ex);
                }
                return FoodsInMeal;
            }
            return null;
        }
        internal FoodInMeal GetFoodInMealFromRow(DbDataReader Row)
        {
            FoodInMeal f = new FoodInMeal();
            try
            {
                f.IdMeal = Safe.Int(Row["IdMeal"]);
                f.IdFoodInMeal = Safe.Int(Row["IdFoodInMeal"]);
                f.IdFood = Safe.Int(Row["IdFood"]);
                f.ChoGrams.Double = Safe.Double(Row["CarbohydratesGrams"]);
                f.ChoPercent.Double = Safe.Double(Row["CarbohydratesPercent"]);
                f.QuantityGrams.Double = Safe.Double(Row["Quantity"]);
                f.AccuracyOfChoEstimate.Double = Safe.Double(Row["AccuracyOfChoEstimate"]);
                f.Name = Safe.String(Row["Name"]);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | GetFoodInMealFromRow", ex);
            }
            return f;
        }
        internal override int? SaveOneFoodInMeal(FoodInMeal FoodToSave)
        {
            try
            {
                if (FoodToSave.IdMeal != null)
                {
                    if (FoodToSave.IdFoodInMeal == null || FoodToSave.IdFoodInMeal == 0)
                    {
                        FoodToSave.IdFoodInMeal = GetNextTablePrimaryKey("FoodsInMeals", "IdFoodInMeal");
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

        internal int? UpdateFoodInMeal(FoodInMeal FoodToSave)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE FoodsInMeals SET " +
                    "IdMeal=" + SqliteHelper.Int(FoodToSave.IdMeal) + "," +
                    "IdFood=" + SqliteHelper.Int(FoodToSave.IdFood) + "," +
                    "CarbohydratesGrams=" + SqliteHelper.Double(FoodToSave.ChoGrams.Double) + "," +
                    "Quantity=" + SqliteHelper.Double(FoodToSave.QuantityGrams.Double) + "," +
                    "CarbohydratesPercent=" + SqliteHelper.Double(FoodToSave.ChoPercent.Double) + "," +
                    "AccuracyOfChoEstimate=" + SqliteHelper.Double(FoodToSave.AccuracyOfChoEstimate.Double) + "," +
                    "Name=" + SqliteHelper.String(FoodToSave.Name) + "" +
                    " WHERE IdFoodInMeal=" + SqliteHelper.Int(FoodToSave.IdFoodInMeal) + 
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return FoodToSave.IdFoodInMeal;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | UpdateFoodInMeal", ex);
                return null;
            }
        }
        internal int? InsertFoodInMeal(FoodInMeal FoodToSave)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO FoodsInMeals" +
                    "(" +
                    "IdFoodInMeal,IdMeal,IdFood,Quantity,CarbohydratesGrams," +
                    "CarbohydratesPercent,AccuracyOfChoEstimate," +
                    "Name";
                    query += ")VALUES(" +
                    SqliteHelper.Int(FoodToSave.IdFoodInMeal) + "," +
                    SqliteHelper.Int(FoodToSave.IdMeal) + "," +
                    SqliteHelper.Int(FoodToSave.IdFood) + "," +
                    SqliteHelper.Double(FoodToSave.QuantityGrams.Double) + "," +
                    SqliteHelper.Double(FoodToSave.ChoGrams.Double) + "," +
                    SqliteHelper.Double(FoodToSave.ChoPercent.Double) + "," +
                    SqliteHelper.Double(FoodToSave.AccuracyOfChoEstimate.Double) + "," +
                    SqliteHelper.String(FoodToSave.Name) + ""; 

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
        internal Food GetFoodFromRow(DbDataReader Row)
        {
            Food f = new Food();
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                f.IdFood = Safe.Int(Row["IdFood"]);
                f.Name = Safe.String(Row["Name"]);
                f.Description = Safe.String(Row["Description"]);
                f.Energy.Double = Safe.Double(Row["Energy"]);
                f.TotalFats.Double = Safe.Double(Row["TotalFats"]);
                f.SaturatedFats.Double = Safe.Double(Row["SaturatedFats"]);
                f.Cho.Double = Safe.Double(Row["Carbohydrates"]);
                f.Sugar.Double = Safe.Double(Row["Sugar"]);
                f.Fibers.Double = Safe.Double(Row["Fibers"]);
                f.Proteins.Double = Safe.Double(Row["Proteins"]);
                f.Salt.Double = Safe.Double(Row["Salt"]);
                f.Potassium.Double = Safe.Double(Row["Potassium"]);
                f.GlycemicIndex.Double = Safe.Double(Row["GlycemicIndex"]);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | GetFoodFromRow", ex);
            }
            return f;
        }
        internal override void DeleteOneFoodInMeal(FoodInMeal Food)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM FoodsInMeals" +
                    " WHERE IdFoodInMeal=" + Food.IdFoodInMeal;
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | DeleteOneFoodInMeal", ex);
            }
        }
        internal override List<Food> SearchFoods(string Name, string Description)
        {
            List<Food> list = new List<Food>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Foods";
                    //if ((Name != "" && Name != null) || (Description != "" && Description != null) )
                    //{
                        query += " WHERE ((Name LIKE '%" + Name + "%'" +
                                " OR Name = null)" +
                                " AND (Description LIKE '%" + Description + "%'" + 
                                " OR Description = null))";
                    //}
                    query += " ORDER BY Name, Description;";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        Food f = GetFoodFromRow(dRead);
                        list.Add(f);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | SearchFood", ex);
            }
            return list;
        }
        internal override int? SaveOneFood(Food Food)
        {
            try
            {
                if (Food.IdFood == null || Food.IdFood == 0)
                {
                    Food.IdFood = GetNextTablePrimaryKey("Foods", "IdFood");
                    // INSERT new record in the table
                    InsertFood(Food);
                }
                else
                {   // GlucoseMeasurement.IdGlucoseRecord exists
                    UpdateFood(Food);
                }
                return Food.IdFood;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | SaveOneFood", ex);
                return null;
            }
        }
        internal override void DeleteOneFood(Food food)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM Foods" +
                    " WHERE IdFood=" + food.IdFood;
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | DeleteOneFoodInMeal", ex);
            }
        }
        private int? UpdateFood(Food food)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE Foods SET " +
                    "Name=" + SqliteHelper.String(food.Name) + "," +
                    "Description=" + SqliteHelper.String(food.Description) + "," +
                    "Energy=" + SqliteHelper.Double(food.Energy.Double) + "," +
                    "TotalFats=" + SqliteHelper.Double(food.TotalFats.Double) + "," +
                    "SaturatedFats=" + SqliteHelper.Double(food.SaturatedFats.Double) + "," +
                    "Carbohydrates=" + SqliteHelper.Double(food.Cho.Double) + "," +
                    "Sugar=" + SqliteHelper.Double(food.Sugar.Double) + "," +
                    "Fibers=" + SqliteHelper.Double(food.Fibers.Double) + "," +
                    "Proteins=" + SqliteHelper.Double(food.Proteins.Double) + "," +
                    "Salt=" + SqliteHelper.Double(food.Salt.Double) + "," +
                    "Potassium=" + SqliteHelper.Double(food.Potassium.Double) + "," +
                    "GlycemicIndex=" + SqliteHelper.Double(food.GlycemicIndex.Double) + "" +
                    " WHERE IdFood=" + SqliteHelper.Int(food.IdFood) +
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return food.IdFood;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | UpdateFoodInMeal", ex);
                return null;
            }
        }
        private void InsertFood(Food food)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Foods" +
                    "(" +
                    "IdFood,Name,Description,Energy,TotalFats,SaturatedFats,Carbohydrates," +
                    "Sugar,Fibers,Proteins,Salt,Potassium,GlycemicIndex"; 
                    query += ")VALUES(" +
                    SqliteHelper.Int(food.IdFood) + "," +
                    SqliteHelper.String(food.Name) + "," +
                    SqliteHelper.String(food.Description) + "," +
                    SqliteHelper.Double(food.Energy.Double) + "," +
                    SqliteHelper.Double(food.TotalFats.Double) + "," +
                    SqliteHelper.Double(food.SaturatedFats.Double) + "," +
                    SqliteHelper.Double(food.Cho.Double) + "," +
                    SqliteHelper.Double(food.Sugar.Double) + "," +
                    SqliteHelper.Double(food.Fibers.Double) + "," +
                    SqliteHelper.Double(food.Proteins.Double) + "," +
                    SqliteHelper.Double(food.Salt.Double) + "," +
                    SqliteHelper.Double(food.Potassium.Double) + "," +
                    SqliteHelper.Double(food.GlycemicIndex.Double) + ""; 
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | UpdateMeal", ex);
            }
        }
        internal override Food GetOneFood(int? IdFood)
        {
            Food food = new Food(); 
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Foods" + 
                        " WHERE IdFood=" + IdFood;
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    dRead.Read(); 
                    food = GetFoodFromRow(dRead);
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | ReadOneFood", ex);
            }
            return food;
        }
        internal override List<Food> GetFoods()
        {
            List<Food> list = new List<Food>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Foods";
                    //query += " WHERE XXXXX";
                    //query += " ORDER BY TimeBegin DESC"; 
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        Food g = GetFoodFromRow(dRead);
                        list.Add(g);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_MealAndFood | ReadOneFood", ex);
            }
            return list;
        }
    }
}

