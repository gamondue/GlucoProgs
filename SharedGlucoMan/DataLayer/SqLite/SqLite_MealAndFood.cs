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
                f.Name = Safe.String(Row["Name"]);
                f.IdMeal = Safe.Int(Row["IdMeal"]);
                f.IdFood = Safe.Int(Row["IdFood"]);
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
            Food f = new Food(new Unit("g", 1));
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
                    Unit unit = new Unit("g", 1);
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
            Food food = new Food(new Unit("g", 1));
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
        internal override int? AddUnit(Unit Unit)
        {
            // if the IdFood in thos Unit is null, the UnitSymbol will be shown in any food
            int? key = null;
            try
            {
                key = GetTableNextPrimaryKey("Units", "IdUnit");
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Units " +
                                "(IdUnit,Symbol,Name,Description,IdFood,GramsInOneUnit";
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
                    string query = "DELETE FROM Units" +
                                " WHERE IdUnit=" + SqliteSafe.Int(Food.UnitSymbol) +
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
        internal override List<Unit> GetAllUnitsOfOneFood(Food Food)
        {
            if (Food.IdFood != null)
            {
                List<Unit> UnitsInFood = new();
                try
                {
                    DbDataReader dRead;
                    DbCommand cmd;
                    using (DbConnection conn = Connect())
                    {
                        string query = "SELECT *" +
                            " FROM Units" +
                            " WHERE IdFood=" + SqliteSafe.Int(Food.IdFood) +
                            " OR IdFood IS null OR IdFood=0" +
                            ";";
                        cmd = new SqliteCommand(query);
                        cmd.Connection = conn;
                        dRead = cmd.ExecuteReader();
                        while (dRead.Read())
                        {
                            Unit u = new();
                            u.IdUnit = Safe.Int(dRead["IdUnit"]);
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
                            " FROM Categories" +
                            " JOIN Foods ON Foods.IdCategory=Categories.IdCategory" +
                            " WHERE Categories.IdFood=" + SqliteSafe.Int(food.IdFood) +
                            " OR IdFood IS null OR IdFood=0" +
                            ";";
                        cmd = new SqliteCommand(query);
                        cmd.Connection = conn;
                        dRead = cmd.ExecuteReader();
                        while (dRead.Read())
                        {
                            CategoryOfFood c = new CategoryOfFood();
                            c.IdCategory = (int)Safe.Int(dRead["IdCategory"]);
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
        internal override bool CheckIfUnitSymbolExists(Unit unit, int? idFood)
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
                    string query = "SELECT idUnit FROM Units" +
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
            throw new NotImplementedException();
        }
        internal override int? AddCategoryOfFood(CategoryOfFood category, Food food)
        {
            throw new NotImplementedException();
        }
        internal override void RemoveCategoryFromFood(Food currentFood)
        {
            throw new NotImplementedException();
        }
        internal override void RemoveUnitFromFood(Unit unit, Food food)
        {
            throw new NotImplementedException();
        }
    }
}


