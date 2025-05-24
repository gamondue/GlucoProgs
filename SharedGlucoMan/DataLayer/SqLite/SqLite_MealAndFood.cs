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
                m.Carbohydrates.Double = Safe.Double(Row["Carbohydrates"]);
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
                    "Carbohydrates=" + SqliteSafe.Double(Meal.Carbohydrates.Double) + "," +
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
                    SqliteSafe.Double(Meal.Carbohydrates.Double) + "," +
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
                f.IdMeal = Safe.Int(Row["IdMeal"]);
                f.IdFoodInMeal = Safe.Int(Row["IdFoodInMeal"]);
                f.IdFood = Safe.Int(Row["IdFood"]);
                f.CarbohydratesGrams.Double = Safe.Double(Row["CarbohydratesGrams"]);
                f.CarbohydratesPerUnit.Double = Safe.Double(Row["CarbohydratesPercent"]);
                f.QuantityGrams.Double = Safe.Double(Row["Quantity"]);
                f.AccuracyOfChoEstimate.Double = Safe.Double(Row["AccuracyOfChoEstimate"]);
                f.Name = Safe.String(Row["Name"]);
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
                    "CarbohydratesGrams=" + SqliteSafe.Double(FoodToSave.CarbohydratesGrams.Double) + "," +
                    "Quantity=" + SqliteSafe.Double(FoodToSave.QuantityGrams.Double) + "," +
                    "CarbohydratesPercent=" + SqliteSafe.Double(FoodToSave.CarbohydratesPerUnit.Double) + "," +
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
                    "IdFoodInMeal,IdMeal,IdFood,Quantity,CarbohydratesGrams," +
                    "CarbohydratesPercent,AccuracyOfChoEstimate," +
                    "Name";
                    query += ")VALUES(" +
                    SqliteSafe.Int(FoodToSave.IdFoodInMeal) + "," +
                    SqliteSafe.Int(FoodToSave.IdMeal) + "," +
                    SqliteSafe.Int(FoodToSave.IdFood) + "," +
                    SqliteSafe.Double(FoodToSave.QuantityGrams.Double) + "," +
                    SqliteSafe.Double(FoodToSave.CarbohydratesGrams.Double) + "," +
                    SqliteSafe.Double(FoodToSave.CarbohydratesPerUnit.Double) + "," +
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
                f.TotalFatsPercent.Double = Safe.Double(Row["TotalFats"]);
                f.SaturatedFatsPercent.Double = Safe.Double(Row["SaturatedFats"]);
                f.MonounsaturatedFatsPercent.Double = Safe.Double(Row["MonounsaturatedFats"]);
                f.PolyunsaturatedFatsPercent.Double = Safe.Double(Row["PolyunsaturatedFats"]);
                f.CarbohydratesPercent.Double = Safe.Double(Row["Carbohydrates"]);
                f.SugarPercent.Double = Safe.Double(Row["Sugar"]);
                f.FibersPercent.Double = Safe.Double(Row["Fibers"]);
                f.ProteinsPercent.Double = Safe.Double(Row["Proteins"]);
                f.SaltPercent.Double = Safe.Double(Row["Salt"]);
                f.PotassiumPercent.Double = Safe.Double(Row["Potassium"]);
                f.CholesterolPercent.Double = Safe.Double(Row["Cholesterol"]);
                f.GlycemicIndex.Double = Safe.Double(Row["GlycemicIndex"]);
                UnitOfFood unit = new UnitOfFood();
                unit.Name = Safe.String(Row["Unit"]);
                unit.GramsInOneUnit.Double = Safe.Double(Row["GramsInOneUnit"]);
                f.Unit = unit;
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
                    // default unit is grams, add an entry in UnitsOfFood table
                    UnitOfFood unit = new UnitOfFood("g", 1);
                    AddUnitToFood(Food, unit);
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
                    "TotalFats=" + SqliteSafe.Double(food.TotalFatsPercent.Double) + "," +
                    "SaturatedFats=" + SqliteSafe.Double(food.SaturatedFatsPercent.Double) + "," +
                    "MonounsaturatedFats=" + SqliteSafe.Double(food.MonounsaturatedFatsPercent.Double) + "," +
                    "PolyunsaturatedFats=" + SqliteSafe.Double(food.PolyunsaturatedFatsPercent.Double) + "," +
                    "Carbohydrates=" + SqliteSafe.Double(food.CarbohydratesPercent.Double) + "," +
                    "Sugar=" + SqliteSafe.Double(food.SugarPercent.Double) + "," +
                    "Fibers=" + SqliteSafe.Double(food.FibersPercent.Double) + "," +
                    "Proteins=" + SqliteSafe.Double(food.ProteinsPercent.Double) + "," +
                    "Salt=" + SqliteSafe.Double(food.SaltPercent.Double) + "," +
                    "Potassium=" + SqliteSafe.Double(food.PotassiumPercent.Double) + "," +
                    "Cholesterol=" + SqliteSafe.Double(food.CholesterolPercent.Double) + "," +
                    "GlycemicIndex=" + SqliteSafe.Double(food.GlycemicIndex.Double) + "," +
                    "Unit=" + SqliteSafe.String(food.Unit.Name) + "," +
                    "GramsInOneUnit=" + SqliteSafe.Double(food.Unit.GramsInOneUnit.Double) + "," +
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
                    "IdFood,Name,Description,Energy,TotalFats,SaturatedFats,MonounsaturatedFats,PolyunsaturatedFats" +
                    ",Carbohydrates,Sugar,Fibers,Proteins,Salt,Potassium,Cholesterol,GlycemicIndex" +
                    ",Unit,GramsInOneUnit,Manufacturer,Category";
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
                    SqliteSafe.Double(food.CholesterolPercent.Double) + "," +
                    SqliteSafe.Double(food.GlycemicIndex.Double) + "," +
                    SqliteSafe.Double(food.Unit.Name) + "," +
                    SqliteSafe.Double(food.Unit.GramsInOneUnit.Double) + "," +
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
        internal override void AddUnitToFood(Food Food, UnitOfFood Unit)
        {
            if (Food == null)
                return;
            try
            {
                int key = GetTableNextPrimaryKey("UnitsOfFood", "IdUnitOfFood");
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO UnitsOfFood " +
                                "(IdUnitOfFood,IdFood,Name,GramsInOneUnit";
                    query += ")VALUES(" + key + "," +
                    SqliteSafe.Int(Food.IdFood) + "," +
                    SqliteSafe.String(Unit.Name) + "," +
                    SqliteSafe.Double(Unit.GramsInOneUnit.Double) + "" +
                    ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_MealAndFood | AddUnitToFoodsUnits", ex);
            }
        }
        internal override void RemoveUnitFromFoodsUnits(Food Food)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM UnitsOfFood SET " +
                                "WHERE IdUnit=" + SqliteSafe.Int(Food.Unit.IdUnit) +
                                " AND IdFood=" + SqliteSafe.Int(Food.IdFood) +
                    ");";
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
                            ";";
                        cmd = new SqliteCommand(query);
                        cmd.Connection = conn;
                        dRead = cmd.ExecuteReader();
                        while (dRead.Read())
                        {
                            UnitOfFood u = new(Safe.String(dRead["Name"]), (double)Safe.Double(dRead["GramsInOneUnit"]));
                            u.IdUnit = Safe.Int(dRead["IdUnitOfFood"]);
                            u.IdFood = Safe.Int(dRead["IdFood"]);
                            u.Description = Safe.String(dRead["Description"]);
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
    }
}


