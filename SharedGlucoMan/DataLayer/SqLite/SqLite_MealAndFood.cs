using gamon;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using static GlucoMan.Common;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
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
                        query += " WHERE TimeBegin BETWEEN " + SqliteSafe.Date(((DateTime)InitialInstant).ToString("yyyy-MM-dd")) +
                            " AND " + SqliteSafe.Date(((DateTime)FinalInstant).ToString("yyyy-MM-dd 23:59:29")) + "";
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
                General.Log.Error("Sqlite_MealAndFood | ReadMeals", ex);
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
                General.Log.Error("Sqlite_MealAndFood | GetOneMeal", ex);
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
                General.Log.Error("Sqlite_MealAndFood | SaveMeals", ex);
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
                General.Log.Error("Sqlite_MealAndFood | SaveOneMeal", ex);
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
                General.Log.Error("Sqlite_MealAndFood | DeleteOneMeal", ex);
            }
        }
        internal Meal GetMealFromRow(DbDataReader Row)
        {
            Meal m = new Meal();
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                m.IdMeal = SqlSafe.Int(Row["IdMeal"]);
                if (Row["IdTypeOfMeal"] is DBNull)
                    m.IdTypeOfMeal = 0;
                else
                    m.IdTypeOfMeal = (TypeOfMeal)SqlSafe.Int(Row["IdTypeOfMeal"]);
                m.Carbohydrates.Double = SqlSafe.Double(Row["Carbohydrates"]);
                m.TimeBegin.DateTime = SqlSafe.DateTime(Row["TimeBegin"]);
                m.Notes = SqlSafe.String(Row["Notes"]);
                m.AccuracyOfChoEstimate.Double = SqlSafe.Double(Row["AccuracyOfChoEstimate"]);
                m.IdBolusCalculation = SqlSafe.Int(Row["IdBolusCalculation"]);
                m.IdGlucoseRecord = SqlSafe.Int(Row["IdGlucoseRecord"]);
                m.IdInsulinInjection = SqlSafe.Int(Row["IdInsulinInjection"]);
                m.TimeEnd.DateTime = SqlSafe.DateTime(Row["TimeEnd"]);
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_MealAndFood | GetMealFromRow", ex);
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
                    "IdTypeOfMeal=" + SqliteSafe.Int((int)Meal.IdTypeOfMeal) + "," +
                    "Carbohydrates=" + SqliteSafe.Double(Meal.Carbohydrates.Double) + "," +
                    "TimeBegin=" + SqliteSafe.Date(Meal.TimeBegin.DateTime) + "," +
                    "Notes=" + SqliteSafe.String(Meal.Notes) + "," +
                    "AccuracyOfChoEstimate=" + SqliteSafe.Double(Meal.AccuracyOfChoEstimate.Double) + "," +
                    "IdBolusCalculation=" + SqliteSafe.Int(Meal.IdBolusCalculation) + "," +
                    "IdGlucoseRecord=" + SqliteSafe.Int(Meal.IdGlucoseRecord) + "," +
                    "IdInsulinInjection=" + SqliteSafe.Int(Meal.IdInsulinInjection) + "," +
                    "TimeEnd=" + SqliteSafe.Date(Meal.TimeEnd.DateTime) + "" +
                    " WHERE IdMeal=" + SqliteSafe.Int(Meal.IdMeal) +
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Meal.IdMeal;
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_MealAndFood | UpdateMeal", ex);
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
                    SqliteSafe.Int(Meal.IdMeal) + "," +
                    SqliteSafe.Int((int)Meal.IdTypeOfMeal) + "," +
                    SqliteSafe.Double(Meal.Carbohydrates.Double) + "," +
                    SqliteSafe.Date(Meal.TimeBegin.DateTime) + "," +
                    SqliteSafe.String(Meal.Notes) + "," +
                    SqliteSafe.Double(Meal.AccuracyOfChoEstimate.Double) + "," +
                    SqliteSafe.Int(Meal.IdBolusCalculation) + "," +
                    SqliteSafe.Int(Meal.IdGlucoseRecord) + "," +
                    SqliteSafe.Int(Meal.IdInsulinInjection) + "," +
                    SqliteSafe.Date(Meal.TimeEnd.DateTime) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Meal.IdMeal;
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_MealAndFood | InsertMeal", ex);
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
                    General.Log.Error("Sqlite_MealAndFood | GetFoodsInMeal", ex);
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
                f.IdMeal = SqlSafe.Int(Row["IdMeal"]);
                f.IdFoodInMeal = SqlSafe.Int(Row["IdFoodInMeal"]);
                f.IdFood = SqlSafe.Int(Row["IdFood"]);
                f.ChoGrams.Double = SqlSafe.Double(Row["CarbohydratesGrams"]);
                f.ChoPercent.Double = SqlSafe.Double(Row["CarbohydratesPercent"]);
                f.QuantityGrams.Double = SqlSafe.Double(Row["Quantity"]);
                f.AccuracyOfChoEstimate.Double = SqlSafe.Double(Row["AccuracyOfChoEstimate"]);
                f.Name = SqlSafe.String(Row["Name"]);
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_MealAndFood | GetFoodInMealFromRow", ex);
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
                    General.Log.Error("Sqlite_MealAndFood | SaveOneFoodInMeal",
                        new Exception("FoodInMeal instance must have an IdMeal"));
                    return null;
                }
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_MealAndFood | SaveOneFoodInMeal", ex);
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
                    "IdMeal=" + SqliteSafe.Int(FoodToSave.IdMeal) + "," +
                    "IdFood=" + SqliteSafe.Int(FoodToSave.IdFood) + "," +
                    "CarbohydratesGrams=" + SqliteSafe.Double(FoodToSave.ChoGrams.Double) + "," +
                    "Quantity=" + SqliteSafe.Double(FoodToSave.QuantityGrams.Double) + "," +
                    "CarbohydratesPercent=" + SqliteSafe.Double(FoodToSave.ChoPercent.Double) + "," +
                    "AccuracyOfChoEstimate=" + SqliteSafe.Double(FoodToSave.AccuracyOfChoEstimate.Double) + "," +
                    "Name=" + SqliteSafe.String(FoodToSave.Name) + "" +
                    " WHERE IdFoodInMeal=" + SqliteSafe.Int(FoodToSave.IdFoodInMeal) +
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return FoodToSave.IdFoodInMeal;
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_MealAndFood | UpdateFoodInMeal", ex);
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
                    SqliteSafe.Int(FoodToSave.IdFoodInMeal) + "," +
                    SqliteSafe.Int(FoodToSave.IdMeal) + "," +
                    SqliteSafe.Int(FoodToSave.IdFood) + "," +
                    SqliteSafe.Double(FoodToSave.QuantityGrams.Double) + "," +
                    SqliteSafe.Double(FoodToSave.ChoGrams.Double) + "," +
                    SqliteSafe.Double(FoodToSave.ChoPercent.Double) + "," +
                    SqliteSafe.Double(FoodToSave.AccuracyOfChoEstimate.Double) + "," +
                    SqliteSafe.String(FoodToSave.Name) + "";

                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return FoodToSave.IdFoodInMeal;
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_MealAndFood | UpdateMeal", ex);
                return null;
            }
        }
        internal Food GetFoodFromRow(DbDataReader Row)
        {
            Food f = new Food();
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                f.IdFood = SqlSafe.Int(Row["IdFood"]);
                f.Name = SqlSafe.String(Row["Name"]);
                f.Description = SqlSafe.String(Row["Description"]);
                f.Energy.Double = SqlSafe.Double(Row["Energy"]);
                f.TotalFats.Double = SqlSafe.Double(Row["TotalFats"]);
                f.SaturatedFats.Double = SqlSafe.Double(Row["SaturatedFats"]);
                f.MonounsaturatedFats.Double = SqlSafe.Double(Row["MonoinsaturatedFats"]);
                f.PoliunsaturatedFats.Double = SqlSafe.Double(Row["PolinsaturatedFats"]);
                f.Cho.Double = SqlSafe.Double(Row["Carbohydrates"]);
                f.Sugar.Double = SqlSafe.Double(Row["Sugar"]);
                f.Fibers.Double = SqlSafe.Double(Row["Fibers"]);
                f.Proteins.Double = SqlSafe.Double(Row["Proteins"]);
                f.Salt.Double = SqlSafe.Double(Row["Salt"]);
                f.Potassium.Double = SqlSafe.Double(Row["Potassium"]);
                f.Cholesterol.Double = SqlSafe.Double(Row["Cholesterol"]);
                f.GlycemicIndex.Double = SqlSafe.Double(Row["GlycemicIndex"]);
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_MealAndFood | GetFoodFromRow", ex);
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
                General.Log.Error("Sqlite_MealAndFood | DeleteOneFoodInMeal", ex);
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
                    if ((Name != "" && Name != null) || (Description != "" && Description != null))
                    {
                        query += " WHERE ((Name LIKE '%" + Name + "%'" +
                            " OR Name = null)" +
                            " AND (Description LIKE '%" + Description + "%'" +
                            " OR Description = null))";
                    }
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
                General.Log.Error("Sqlite_MealAndFood | SearchFood", ex);
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
                General.Log.Error("Sqlite_MealAndFood | SaveOneFood", ex);
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
                General.Log.Error("Sqlite_MealAndFood | DeleteOneFoodInMeal", ex);
            }
        }
        private int? UpdateFood(Food food)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    SqliteSafe.Double(food.Cholesterol.Double);
                    string query = "UPDATE Foods SET " +
                    "Name=" + SqliteSafe.String(food.Name) + "," +
                    "Description=" + SqliteSafe.String(food.Description) + "," +
                    "Energy=" + SqliteSafe.Double(food.Energy.Double) + "," +
                    "TotalFats=" + SqliteSafe.Double(food.TotalFats.Double) + "," +
                    "SaturatedFats=" + SqliteSafe.Double(food.SaturatedFats.Double) + "," +
                    "MonoinsaturatedFats=" + SqliteSafe.Double(food.MonounsaturatedFats.Double) + "," +
                    "PolinsaturatedFats=" + SqliteSafe.Double(food.PoliunsaturatedFats.Double) + "," +
                    "Carbohydrates=" + SqliteSafe.Double(food.Cho.Double) + "," +
                    "Sugar=" + SqliteSafe.Double(food.Sugar.Double) + "," +
                    "Fibers=" + SqliteSafe.Double(food.Fibers.Double) + "," +
                    "Proteins=" + SqliteSafe.Double(food.Proteins.Double) + "," +
                    "Salt=" + SqliteSafe.Double(food.Salt.Double) + "," +
                    "Potassium=" + SqliteSafe.Double(food.Potassium.Double) + "," +
                    "Cholesterol=" + SqliteSafe.Double(food.Cholesterol.Double) + "" +
                    "GlycemicIndex=" + SqliteSafe.Double(food.GlycemicIndex.Double) + "," +
                    " WHERE IdFood=" + SqliteSafe.Int(food.IdFood) +
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return food.IdFood;
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_MealAndFood | UpdateFoodInMeal", ex);
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
                    "IdFood,Name,Description,Energy,TotalFats,SaturatedFats,MonoinsaturatedFats,PolinsaturatedFats" +
                    ",Carbohydrates,Sugar,Fibers,Proteins,Salt,Potassium,Cholesterol,GlycemicIndex";
                    query += ")VALUES(" +
                    SqliteSafe.Int(food.IdFood) + "," +
                    SqliteSafe.String(food.Name) + "," +
                    SqliteSafe.String(food.Description) + "," +
                    SqliteSafe.Double(food.Energy.Double) + "," +
                    SqliteSafe.Double(food.TotalFats.Double) + "," +
                    SqliteSafe.Double(food.SaturatedFats.Double) + "," +
                    SqliteSafe.Double(food.MonounsaturatedFats.Double) + "," +
                    SqliteSafe.Double(food.PoliunsaturatedFats.Double) + "," +
                    SqliteSafe.Double(food.Cho.Double) + "," +
                    SqliteSafe.Double(food.Sugar.Double) + "," +
                    SqliteSafe.Double(food.Fibers.Double) + "," +
                    SqliteSafe.Double(food.Proteins.Double) + "," +
                    SqliteSafe.Double(food.Salt.Double) + "," +
                    SqliteSafe.Double(food.Potassium.Double) + "," +
                    SqliteSafe.Double(food.Cholesterol.Double) + "," +
                    SqliteSafe.Double(food.GlycemicIndex.Double) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_MealAndFood | UpdateMeal", ex);
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
                General.Log.Error("Sqlite_MealAndFood | ReadOneFood", ex);
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
                General.Log.Error("Sqlite_MealAndFood | ReadOneFood", ex);
            }
            return list;
        }
    }
}

