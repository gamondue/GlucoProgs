using gamon;
using Microsoft.Data.Sqlite;
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | ReadMeals", ex);
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | GetOneMeal", ex);
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | SaveMeals", ex);
            }
        }
        internal override int? SaveOneMeal(Meal Meal)
        {
            try
            {
                if (Meal.IdMeal == null || Meal.IdMeal == 0)
                {
                    Meal.IdMeal = GetTableNextPrimaryKey("Meals", "IdMeal");
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | SaveOneMeal", ex);
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | DeleteOneMeal", ex);
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
                m.CarbohydratesGrams.Double = Safe.Double(Row["Carbohydrates"]);
                m.TimeBegin.DateTime = Safe.DateTime(Row["TimeBegin"]);
                m.Notes = Safe.String(Row["Notes"]);
                m.AccuracyOfChoEstimate.Double = Safe.Double(Row["AccuracyOfChoEstimate"]);
                m.IdBolusCalculation = Safe.Int(Row["IdBolusCalculation"]);
                m.IdGlucoseRecord = Safe.Int(Row["IdGlucoseRecord"]);
                m.IdInjection = Safe.Int(Row["IdInjection"]);
                m.TimeEnd.DateTime = Safe.DateTime(Row["TimeEnd"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | GetMealFromRow", ex);
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
                    "Carbohydrates=" + SqliteSafe.Double(Meal.CarbohydratesGrams.Double) + "," +
                    "TimeBegin=" + SqliteSafe.Date(Meal.TimeBegin.DateTime) + "," +
                    "Notes=" + SqliteSafe.String(Meal.Notes) + "," +
                    "AccuracyOfChoEstimate=" + SqliteSafe.Double(Meal.AccuracyOfChoEstimate.Double) + "," +
                    "IdBolusCalculation=" + SqliteSafe.Int(Meal.IdBolusCalculation) + "," +
                    "IdGlucoseRecord=" + SqliteSafe.Int(Meal.IdGlucoseRecord) + "," +
                    "IdInjection=" + SqliteSafe.Int(Meal.IdInjection) + "," +
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | UpdateMeal", ex);
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
                    "IdBolusCalculation,IdGlucoseRecord,IdInjection,TimeEnd";
                    query += ")VALUES(" +
                    SqliteSafe.Int(Meal.IdMeal) + "," +
                    SqliteSafe.Int((int)Meal.IdTypeOfMeal) + "," +
                    SqliteSafe.Double(Meal.CarbohydratesGrams.Double) + "," +
                    SqliteSafe.Date(Meal.TimeBegin.DateTime) + "," +
                    SqliteSafe.String(Meal.Notes) + "," +
                    SqliteSafe.Double(Meal.AccuracyOfChoEstimate.Double) + "," +
                    SqliteSafe.Int(Meal.IdBolusCalculation) + "," +
                    SqliteSafe.Int(Meal.IdGlucoseRecord) + "," +
                    SqliteSafe.Int(Meal.IdInjection) + "," +
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | InsertMeal", ex);
                return null;
            }
        }
        internal override bool SaveFoodsInMeal(List<FoodInMeal> Meal)
        {
            bool error = false;
            foreach (FoodInMeal f in Meal)
            {
                int? id = SaveOneFoodInMeal(f);
                if (id == null)
                {
                    error = true;
                }
            }
            return !error;
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
                    General.LogOfProgram.Error("Sqlite_MealAndFood | GetFoodsInMeal", ex);
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
                f.IdFoodInMeal = Safe.Int(Row["IdFoodInMeal"]);
                f.IdMeal = Safe.Int(Row["IdMeal"]);
                f.IdFood = Safe.Int(Row["IdFood"]);
                f.Name = Safe.String(Row["Name"]);
                f.CarbohydratesPercent.Double = Safe.Double(Row["CarbohydratesPercent"]);
                f.UnitSymbol = Safe.String(Row["UnitSymbol"]);
                f.GramsInOneUnit.Double = Safe.Double(Row["GramsInOneUnit"]);
                f.QuantityInUnits.Double = Safe.Double(Row["QuantityInUnits"]);
                f.CarbohydratesGrams.Double = Safe.Double(Row["CarbohydratesGrams"]);
                f.AccuracyOfChoEstimate.Double = Safe.Double(Row["AccuracyOfChoEstimate"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | GetFoodInMealFromRow", ex);
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
                        FoodToSave.IdFoodInMeal = GetTableNextPrimaryKey("FoodsInMeals", "IdFoodInMeal");
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
                    General.LogOfProgram.Error("Sqlite_MealAndFood | SaveOneFoodInMeal",
                        new Exception("FoodInMeal instance must have an IdMeal"));
                    return null;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | SaveOneFoodInMeal", ex);
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
                    "Name=" + SqliteSafe.String(FoodToSave.Name) + "," +
                    "CarbohydratesPercent=" + SqliteSafe.Double(FoodToSave.CarbohydratesPercent.Double) + "," +
                    "UnitSymbol=" + SqliteSafe.String(FoodToSave.UnitSymbol) + "," +
                    "GramsInOneUnit=" + SqliteSafe.Double(FoodToSave.GramsInOneUnit.Double) + "," +
                    "QuantityInUnits=" + SqliteSafe.Double(FoodToSave.QuantityInUnits.Double) + "," +
                    "CarbohydratesGrams=" + SqliteSafe.Double(FoodToSave.CarbohydratesGrams.Double) + "," +
                    "AccuracyOfChoEstimate=" + SqliteSafe.Double(FoodToSave.AccuracyOfChoEstimate.Double) + "" +
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | UpdateFoodInMeal", ex);
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
                    "IdFoodInMeal,IdMeal,IdFood,Name,CarbohydratesPercent," +
                    "UnitSymbol,GramsInOneUnit,QuantityInUnits," +
                    "CarbohydratesGrams,AccuracyOfChoEstimate" +
                    "";
                    query += ")VALUES(" +
                    SqliteSafe.Int(FoodToSave.IdFoodInMeal) + "," +
                    SqliteSafe.Int(FoodToSave.IdMeal) + "," +
                    SqliteSafe.Int(FoodToSave.IdFood) + "," +
                    SqliteSafe.String(FoodToSave.Name) + "," +
                    SqliteSafe.Double(FoodToSave.CarbohydratesPercent.Double) + "," +
                    SqliteSafe.String(FoodToSave.UnitSymbol) + "," +
                    SqliteSafe.Double(FoodToSave.GramsInOneUnit.Double) + "," +
                    SqliteSafe.Double(FoodToSave.QuantityInUnits.Double) + "," +
                    SqliteSafe.Double(FoodToSave.CarbohydratesGrams.Double) + "," +
                    SqliteSafe.Double(FoodToSave.AccuracyOfChoEstimate.Double);

                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return FoodToSave.IdFoodInMeal;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | UpdateMeal", ex);
                return null;
            }
        }
        internal Food GetFoodFromRow(DbDataReader Row)
        {
            Food f = new Food(new UnitOfFood("g", 1));
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                f.IdFood = Safe.Int(Row["IdFood"]);
                f.Name = Safe.String(Row["Name"]);
                f.Description = Safe.String(Row["Description"]);
                f.Energy.Double = Safe.Double(Row["Energy"]);
                f.TotalFatsPercent.Double = Safe.Double(Row["TotalFatsPercent"]);
                f.SaturatedFatsPercent.Double = Safe.Double(Row["SaturatedFatsPercent"]);
                f.MonounsaturatedFatsPercent.Double = Safe.Double(Row["MonounsaturatedFatsPercent"]);
                f.PolyunsaturatedFatsPercent.Double = Safe.Double(Row["PolyunsaturatedFatsPercent"]);
                f.CarbohydratesPercent.Double = Safe.Double(Row["CarbohydratesPercent"]);
                f.SugarPercent.Double = Safe.Double(Row["SugarPercent"]);
                f.FibersPercent.Double = Safe.Double(Row["FibersPercent"]);
                f.ProteinsPercent.Double = Safe.Double(Row["ProteinsPercent"]);
                f.SaltPercent.Double = Safe.Double(Row["SaltPercent"]);
                f.PotassiumPercent.Double = Safe.Double(Row["PotassiumPercent"]);
                f.Cholesterol.Double = Safe.Double(Row["Cholesterol"]);
                f.GlycemicIndex.Double = Safe.Double(Row["GlycemicIndex"]);
                f.GramsInOneUnit.Double = Safe.Double(Row["GramsInOneUnit"]);
                f.UnitSymbol = Safe.String(Row["UnitSymbol"]);
                f.Manufacturer = Safe.String(Row["Manufacturer"]);
                f.Category = Safe.String(Row["Category"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | GetFoodFromRow", ex);
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | DeleteOneFoodInMeal", ex);
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | SearchFood", ex);
            }
            return list;
        }
        internal override int? SaveOneFood(Food Food)
        {
            try
            {
                if (Food.IdFood == null || Food.IdFood == 0)
                {
                    Food.IdFood = GetTableNextPrimaryKey("Foods", "IdFood");
                    // INSERT new record in the table
                    InsertFood(Food);
                    // default unit is grams, add an entry in Units table
                    UnitOfFood unit = new UnitOfFood("g", 1);
                    AddUnit(unit);
                }
                else
                {   // GlucoseMeasurement.IdGlucoseRecord exists
                    UpdateFood(Food);
                }
                return Food.IdFood;
            }

            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | SaveOneFood", ex);
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | DeleteOneFoodInMeal", ex);
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
                    "Name=" + SqliteSafe.String(food.Name) + "," +
                    "Description=" + SqliteSafe.String(food.Description) + "," +
                    "Energy=" + SqliteSafe.Double(food.Energy.Double) + "," +
                    "TotalFatsPercent=" + SqliteSafe.Double(food.TotalFatsPercent.Double) + "," +
                    "SaturatedFatsPercent=" + SqliteSafe.Double(food.SaturatedFatsPercent.Double) + "," +
                    "MonounsaturatedFatsPercent=" + SqliteSafe.Double(food.MonounsaturatedFatsPercent.Double) + "," +
                    "PolyunsaturatedFatsPercent=" + SqliteSafe.Double(food.PolyunsaturatedFatsPercent.Double) + "," +
                    "CarbohydratesPercent=" + SqliteSafe.Double(food.CarbohydratesPercent.Double) + "," +
                    "SugarPercent=" + SqliteSafe.Double(food.SugarPercent.Double) + "," +
                    "FibersPercent=" + SqliteSafe.Double(food.FibersPercent.Double) + "," +
                    "ProteinsPercent=" + SqliteSafe.Double(food.ProteinsPercent.Double) + "," +
                    "SaltPercent=" + SqliteSafe.Double(food.SaltPercent.Double) + "," +
                    "PotassiumPercent=" + SqliteSafe.Double(food.PotassiumPercent.Double) + "," +
                    "Cholesterol=" + SqliteSafe.Double(food.Cholesterol.Double) + "," +
                    "GlycemicIndex=" + SqliteSafe.Double(food.GlycemicIndex.Double) + "," +
                    "UnitSymbol=" + SqliteSafe.String(food.UnitSymbol) + "," +
                    "GramsInOneUnit=" + SqliteSafe.Double(food.GramsInOneUnit.Double) + "," +
                    "Manufacturer=" + SqliteSafe.String(food.Manufacturer) + "," +
                    "Category=" + SqliteSafe.String(food.Category) + "" +
                    " WHERE IdFood=" + SqliteSafe.Int(food.IdFood) + "" +
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return food.IdFood;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | UpdateFoodInMeal", ex);
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
                    "IdFood,Name,Description,Energy,TotalFatsPercent,SaturatedFatsPercent," +
                    "MonounsaturatedFatsPercent,PolyunsaturatedFatsPercent" +
                    ",CarbohydratesPercent,SugarPercent,FibersPercent,ProteinsPercent" +
                    ",SaltPercent,PotassiumPercent,Cholesterol,GlycemicIndex" +
                    ",UnitSymbol,GramsInOneUnit,Manufacturer,Category";
                    query += ")VALUES(" +
                    SqliteSafe.Int(food.IdFood) + "," +
                    SqliteSafe.String(food.Name) + "," +
                    SqliteSafe.String(food.Description) + "," +
                    SqliteSafe.Double(food.Energy.Double) + "," +
                    SqliteSafe.Double(food.TotalFatsPercent.Double) + "," +
                    SqliteSafe.Double(food.SaturatedFatsPercent.Double) + "," +
                    SqliteSafe.Double(food.MonounsaturatedFatsPercent.Double) + "," +
                    SqliteSafe.Double(food.PolyunsaturatedFatsPercent.Double) + "," +
                    SqliteSafe.Double(food.CarbohydratesPercent.Double) + "," +
                    SqliteSafe.Double(food.SugarPercent.Double) + "," +
                    SqliteSafe.Double(food.FibersPercent.Double) + "," +
                    SqliteSafe.Double(food.ProteinsPercent.Double) + "," +
                    SqliteSafe.Double(food.SaltPercent.Double) + "," +
                    SqliteSafe.Double(food.PotassiumPercent.Double) + "," +
                    SqliteSafe.Double(food.Cholesterol.Double) + "," +
                    SqliteSafe.Double(food.GlycemicIndex.Double) + "," +
                    SqliteSafe.Double(food.UnitSymbol) + "," +
                    SqliteSafe.Double(food.GramsInOneUnit.Double) + "," +
                    SqliteSafe.Double(food.Manufacturer) + "," +
                    SqliteSafe.Double(food.Category) + "" +
                    ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | UpdateMeal", ex);
            }
        }
        internal override Food GetOneFood(int? IdFood)
        {
            Food food = new Food(new UnitOfFood("g", 1));
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | ReadOneFood", ex);
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
                General.LogOfProgram.Error("Sqlite_MealAndFood | ReadOneFood", ex);
            }
            return list;
        }
        internal override int? AddUnit(UnitOfFood Unit)
        {
            // if the IdFood in thos UnitOfFood is null, the UnitSymbol will be shown in any food
            int? key = null;
            try
            {
                key = GetTableNextPrimaryKey("UnitsOfFood", "IdUnitOfFood");
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO UnitsOfFood " +
                                "(IdUnitOfFood,Symbol,Name,Description,IdFood,GramsInOneUnit";
                    query += ")VALUES(" + key + "," +
                    SqliteSafe.String(Unit.Symbol) + "," +
                    SqliteSafe.String(Unit.Name) + "," +
                    SqliteSafe.String(Unit.Description) + "," +
                    SqliteSafe.Int(Unit.IdFood) + "," +
                    SqliteSafe.Double(Unit.GramsInOneUnit.Double) + "" +
                    ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | AddUnit", ex);
            }
            return key;
        }
        internal override void RemoveManufacturerFromFood(Food Food)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM UnitsOfFood" +
                                " WHERE IdUnitOfFood=" + SqliteSafe.Int(Food.UnitSymbol) +
                                " AND IdFood=" + SqliteSafe.Int(Food.IdFood) +
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | RemoveUnitFromFoodsUnits", ex);
            }
        }
        internal override List<UnitOfFood> GetAllUnitsOfOneFood(Food Food)
        {
            if (Food.IdFood != null)
            {
                List<UnitOfFood> UnitsInFood = new();
                try
                {
                    DbDataReader dRead;
                    DbCommand cmd;
                    using (DbConnection conn = Connect())
                    {
                        string query = "SELECT *" +
                            " FROM UnitsOfFood" +
                            " WHERE IdFood=" + SqliteSafe.Int(Food.IdFood) +
                            " OR IdFood IS null OR IdFood=0" +
                            ";"; 
                        cmd = new SqliteCommand(query);
                        cmd.Connection = conn;
                          dRead = cmd.ExecuteReader();
                        while (dRead.Read())
                        {
                            UnitOfFood u = new();
                            u.IdUnitOfFood = Safe.Int(dRead["IdUnitOfFood"]);
                            u.Symbol = Safe.String(dRead["Symbol"]);
                            u.Name = Safe.String(dRead["Name"]);
                            u.Description = Safe.String(dRead["Description"]);
                            u.IdFood = Safe.Int(dRead["IdFood"]);
                            u.GramsInOneUnit.Double = Safe.Double(dRead["GramsInOneUnit"]);
                            UnitsInFood.Add(u);
                        }
                        dRead.Dispose();
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error("Sqlite_MealAndFood | GetAllUnitsOfOneFood", ex);
                }
                return UnitsInFood;
            }
            return null;
        }
        internal override List<Manufacturer> GetAllManufacturersOfOneFood(Food food)
        {
            if (food.IdFood != null)
            {
                List<Manufacturer> manufacturers = new();
                try
                {
                    DbDataReader dRead;
                    DbCommand cmd;
                    using (DbConnection conn = Connect())
                    {
                        string query = "SELECT *" +
                            " FROM Manufacturers" +
                            " JOIN Foods ON Foods.IdManufacturer=Manufacturers.IdManufacturer" +
                            " WHERE Manufacturers.IdFood=" + SqliteSafe.Int(food.IdFood) +
                            " OR IdFood IS null OR IdFood=0" +
                            ";";
                        cmd = new SqliteCommand(query);
                        cmd.Connection = conn;
                        dRead = cmd.ExecuteReader();
                        while (dRead.Read())
                        {
                            Manufacturer m = new Manufacturer();
                            m.IdManufacturer = (int)Safe.Int(dRead["IdManufacturer"]);
                            m.Name = Safe.String(dRead["Name"]);
                            manufacturers.Add(m);
                        }
                        dRead.Dispose();
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error("Sqlite_MealAndFood | GetAllManufacturersOfOneFood", ex);
                }
                return manufacturers;
            }
            return null;
        }
        internal override List<CategoryOfFood> GetAllCategoriesOfOneFood(Food food)
        {
            if (food.IdFood != null)
            {
                List<CategoryOfFood> categories = new();
                try
                {
                    DbDataReader dRead;
                    DbCommand cmd;
                    using (DbConnection conn = Connect())
                    {
                        string query = "SELECT *" +
                            " FROM CategoriesOfFood" +
                            ";";
                        cmd = new SqliteCommand(query);
                        cmd.Connection = conn;
                        dRead = cmd.ExecuteReader();
                        while (dRead.Read())
                        {
                            CategoryOfFood c = new CategoryOfFood();
                            c.IdCategory = (int)Safe.Int(dRead["IdCategoryOfFood"]);
                            c.Description = Safe.String(dRead["Description"]);
                            c.Name = Safe.String(dRead["Name"]);
                            categories.Add(c);
                        }
                        dRead.Dispose();
                        cmd.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error("Sqlite_MealAndFood | GetAllCategoriesOfOneFood", ex);
                }
                return categories;
            }
            return null;
        }
        internal override bool CheckIfUnitSymbolExists(UnitOfFood unit, int? idFood)
        {
            // Check if the symbol of the unit already exists in the database
            // if idFood == null search all over the units
            // if idFood != null search only in the units of this food
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT IdUnitOfFood FROM UnitsOfFood" +
                        " WHERE Symbol=" + SqliteSafe.String(unit.Symbol);
                    if (idFood != null)
                        query += " AND (idFood=" + idFood + " OR idFood IS NULL)";
                    else
                        query += " AND idFood IS NULL";
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    bool exists = dRead.HasRows;
                    dRead.Dispose();
                    cmd.Dispose();
                    return exists;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | CheckIfUnitExists", ex);
                return false;
            }
        }
        internal override int? AddManufacturer(Manufacturer manufacturer, Food food)
        {
            try
            {
                int? idManufacturer = null;
                using (DbConnection conn = Connect())
                {
                    // Check if manufacturer already exists (by name)
                    DbCommand checkCmd = conn.CreateCommand();
                    checkCmd.CommandText = "SELECT IdManufacturer FROM Manufacturers WHERE Name = @Name;";
                    checkCmd.Parameters.Add(new SqliteParameter("@Name", manufacturer.Name ?? (object)DBNull.Value));
                    var result = checkCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idManufacturer = Safe.Int(result);
                    }
                    else
                    {
                        // Insert new manufacturer
                        idManufacturer = GetTableNextPrimaryKey("Manufacturers", "IdManufacturer");
                        DbCommand insertCmd = conn.CreateCommand();
                        insertCmd.CommandText = "INSERT INTO Manufacturers (IdManufacturer, Name) VALUES (@IdManufacturer, @Name);";
                        insertCmd.Parameters.Add(new SqliteParameter("@IdManufacturer", idManufacturer));
                        insertCmd.Parameters.Add(new SqliteParameter("@Name", manufacturer.Name ?? (object)DBNull.Value));
                        insertCmd.ExecuteNonQuery();
                        insertCmd.Dispose();
                    }
                    checkCmd.Dispose();

                    // Optionally, associate the manufacturer with the food if needed (if there is a relation table)
                    // If Foods table has IdManufacturer, update it
                    if (food != null && food.IdFood != null)
                    {
                        DbCommand updateCmd = conn.CreateCommand();
                        updateCmd.CommandText = "UPDATE Foods SET Manufacturer = @Manufacturer WHERE IdFood = @IdFood;";
                        updateCmd.Parameters.Add(new SqliteParameter("@Manufacturer", manufacturer.Name ?? (object)DBNull.Value));
                        updateCmd.Parameters.Add(new SqliteParameter("@IdFood", food.IdFood));
                        updateCmd.ExecuteNonQuery();
                        updateCmd.Dispose();
                    }
                }
                return idManufacturer;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | AddManufacturer", ex);
                return null;
            }
        }
        internal override int? AddCategoryOfFood(CategoryOfFood category, Food food)
        {
            try
            {
                int? idCategory = null;
                using (DbConnection conn = Connect())
                {
                    // Check if category already exists (by name)
                    DbCommand checkCmd = conn.CreateCommand();
                    checkCmd.CommandText = "SELECT IdCategory FROM Categories WHERE Name = @Name;";
                    checkCmd.Parameters.Add(new SqliteParameter("@Name", category.Name ?? (object)DBNull.Value));
                    var result = checkCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        idCategory = Safe.Int(result);
                    }
                    else
                    {
                        // Insert new category
                        idCategory = GetTableNextPrimaryKey("Categories", "IdCategory");
                        DbCommand insertCmd = conn.CreateCommand();
                        insertCmd.CommandText = "INSERT INTO Categories (IdCategory, Name) VALUES (@IdCategory, @Name);";
                        insertCmd.Parameters.Add(new SqliteParameter("@IdCategory", idCategory));
                        insertCmd.Parameters.Add(new SqliteParameter("@Name", category.Name ?? (object)DBNull.Value));
                        insertCmd.ExecuteNonQuery();
                        insertCmd.Dispose();
                    }
                    checkCmd.Dispose();

                    // Optionally, associate the category with the food if needed (if there is a relation table)
                    // If Foods table has Category, update it
                    if (food != null && food.IdFood != null)
                    {
                        DbCommand updateCmd = conn.CreateCommand();
                        updateCmd.CommandText = "UPDATE Foods SET Category = @Category WHERE IdFood = @IdFood;";
                        updateCmd.Parameters.Add(new SqliteParameter("@Category", category.Name ?? (object)DBNull.Value));
                        updateCmd.Parameters.Add(new SqliteParameter("@IdFood", food.IdFood));
                        updateCmd.ExecuteNonQuery();
                        updateCmd.Dispose();
                    }
                }
                return idCategory;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | AddCategoryOfFood", ex);
                return null;
            }
        }
        internal override void RemoveCategoryFromFood(Food currentFood)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    if (currentFood != null && currentFood.IdFood != null)
                    {
                        DbCommand updateCmd = conn.CreateCommand();
                        updateCmd.CommandText = "UPDATE Foods SET Category = NULL WHERE IdFood = @IdFood;";
                        updateCmd.Parameters.Add(new SqliteParameter("@IdFood", currentFood.IdFood));
                        updateCmd.ExecuteNonQuery();
                        updateCmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | RemoveCategoryFromFood", ex);
            }
        }
        internal override void RemoveUnitFromFood(UnitOfFood unit, Food food)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    if (unit != null && food != null && food.IdFood != null)
                    {
                        DbCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "DELETE FROM UnitsOfFood WHERE Symbol = @Symbol AND IdFood = @IdFood;";
                        cmd.Parameters.Add(new SqliteParameter("@Symbol", unit.Symbol ?? (object)DBNull.Value));
                        cmd.Parameters.Add(new SqliteParameter("@IdFood", food.IdFood));
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | RemoveUnitFromFood", ex);
            }
        }
        internal override int? AddManufacturerToFood(Manufacturer m, Food currentFood)
        {
            try
            {
                if (currentFood != null && currentFood.IdFood != null)
                {
                    using (DbConnection conn = Connect())
                    {
                        DbCommand updateCmd = conn.CreateCommand();
                        updateCmd.CommandText = "UPDATE Foods SET Manufacturer = @Manufacturer WHERE IdFood = @IdFood;";
                        updateCmd.Parameters.Add(new SqliteParameter("@Manufacturer", m.Name ?? (object)DBNull.Value));
                        updateCmd.Parameters.Add(new SqliteParameter("@IdFood", currentFood.IdFood));
                        updateCmd.ExecuteNonQuery();
                        updateCmd.Dispose();
                    }
                }
                return currentFood?.IdFood;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | AddManufacturerToFood", ex);
                return null;
            }
        }
        internal override int? AddCategoryToFood(CategoryOfFood c, Food currentFood)
        {
            try
            {
                if (currentFood != null && currentFood.IdFood != null)
                {
                    using (DbConnection conn = Connect())
                    {
                        DbCommand updateCmd = conn.CreateCommand();
                        updateCmd.CommandText = "UPDATE Foods SET Category = @Category WHERE IdFood = @IdFood;";
                        updateCmd.Parameters.Add(new SqliteParameter("@Category", c.Name ?? (object)DBNull.Value));
                        updateCmd.Parameters.Add(new SqliteParameter("@IdFood", currentFood.IdFood));
                        updateCmd.ExecuteNonQuery();
                        updateCmd.Dispose();
                    }
                }
                return currentFood?.IdFood;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | AddCategoryToFood", ex);
                return null;
            }
        }
        internal override void RemoveUnitFromFoodsUnits(Food currentFood)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    if (currentFood != null && currentFood.IdFood != null)
                    {
// ???????????????????????????? control here
                        DbCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "DELETE FROM UnitsOfFood WHERE IdFood = @IdFood;";
                        cmd.Parameters.Add(new SqliteParameter("@IdFood", currentFood.IdFood));
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | RemoveUnitFromFoodsUnits", ex);
            }
        }
        internal override int? UpdateManufacturer(Manufacturer m)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                        UPDATE Manufacturers
                        SET Name = @Name
                        WHERE IdManufacturer = @IdManufacturer;";
                    cmd.Parameters.Add(new SqliteParameter("@Name", m.Name ?? (object)DBNull.Value));
                    //////////cmd.Parameters.Add(new SqliteParameter("@IdManufacturer", m.IdManufacturer ?? (object)DBNull.Value));
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return rows > 0 ? m.IdManufacturer : null;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | UpdateManufacturer", ex);
                return null;
            }
        }

        internal override bool CheckIfManufacturerExists(Manufacturer m)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(*) FROM Manufacturers WHERE Name = @Name;";
                    cmd.Parameters.Add(new SqliteParameter("@Name", m.Name ?? (object)DBNull.Value));
                    var result = cmd.ExecuteScalar();
                    cmd.Dispose();
                    return Convert.ToInt32(result) > 0;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | CheckIfManufacturerExists", ex);
                return false;
            }
        }
        internal override int? AddManufacturer(Manufacturer m)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    int newId = GetTableNextPrimaryKey("Manufacturers", "IdManufacturer");
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                        INSERT INTO Manufacturers (IdManufacturer, Name)
                        VALUES (@IdManufacturer, @Name);";
                    cmd.Parameters.Add(new SqliteParameter("@IdManufacturer", newId));
                    cmd.Parameters.Add(new SqliteParameter("@Name", m.Name ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return newId;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | AddManufacturer", ex);
                return null;
            }
        }
        internal override bool CheckIfCategoryExists(CategoryOfFood categoryOfFood)
        {
            bool exists = false;
            try
            {
                using (DbConnection conn = Connect())
                {
                    var checkCmd = conn.CreateCommand();
                    checkCmd.CommandText = "SELECT IdCategoryOfFood FROM CategoriesOfFood WHERE Name = @Name;";
                    checkCmd.Parameters.Add(new SqliteParameter("@Name", categoryOfFood.Name));
                    var result = checkCmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        exists = true;
                    }
                    checkCmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | CheckIfCategoryExists", ex);
                return false;
            }
            return exists;
        }
        internal override int? AddCategoryOfFood(CategoryOfFood categoryOfFood)
        {
            try
            {
                int? idCategory = null;
                using (DbConnection conn = Connect())
                {
                    idCategory = GetTableNextPrimaryKey("CategoriesOfFood", "IdCategoryOfFood");
                    DbCommand insertCmd = conn.CreateCommand();
                    insertCmd.CommandText = "INSERT INTO CategoriesOfFood (IdCategoryOfFood, Name, Description) VALUES (@IdCategory, @Name, @Description);";
                    insertCmd.Parameters.Add(new SqliteParameter("@IdCategory", idCategory));
                    insertCmd.Parameters.Add(new SqliteParameter("@Name", categoryOfFood.Name ?? (object)DBNull.Value));
                    insertCmd.Parameters.Add(new SqliteParameter("@Description", categoryOfFood.Description ?? (object)DBNull.Value));
                    insertCmd.ExecuteNonQuery();
                    insertCmd.Dispose();
                }
                return idCategory;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | AddCategoryOfFood (single)", ex);
                return null;
            }
        }
        internal override int? UpdateCategoryOfFood(CategoryOfFood categoryOfFood)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE CategoriesOfFood SET Name = @Name, Description = @Description WHERE IdCategoryOfFood = @IdCategory;";
                    cmd.Parameters.Add(new SqliteParameter("@Name", categoryOfFood.Name ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Description", categoryOfFood.Description ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@IdCategory", categoryOfFood.IdCategory));
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    return rows > 0 ? categoryOfFood.IdCategory : null;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | UpdateCategoryOfFood", ex);
                return null;
            }
        }
    }
}


