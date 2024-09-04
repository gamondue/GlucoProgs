using gamon;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        internal override int? InsertOneRecipe(Recipe Recipe)
        {
            Recipe.IdRecipe = GetNextTablePrimaryKey("Recipes", "IdRecipe");
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Recipes" +
                    "(" +
                    "IdRecipe," +
                    "Name,Description,CarbohydratesPercent," +
                    "AccuracyOfChoEstimate,IsCooked,RawToCookedRatio";
                    query += ")VALUES(" +
                    SqliteSafe.Int(Recipe.IdRecipe)
                    + "," + SqliteSafe.String(Recipe.Name) + "," +
                    SqliteSafe.String(Recipe.Description) + "," +
                    SqliteSafe.Double(Recipe.CarbohydratesPercent) + "," +
                    SqliteSafe.Double(Recipe.AccuracyOfChoEstimate) + "," +
                    SqliteSafe.Bool(Recipe.IsCooked) + "," +
                    SqliteSafe.Double(Recipe.RawToCookedRatio) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Recipe.IdRecipe;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | InsertOneRecipe", ex);
                return null;
            }
        }
        internal override List<Recipe> ReadSomeRecipes(string WhereClause)
        {
            List<Recipe> Recipes = new List<Recipe>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Recipes";
                    query += " ORDER BY IdRecipe DESC ";
                    if (WhereClause != null && WhereClause != "")
                        query += WhereClause;
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        Recipe f = GetRecipeFromRow(dRead);
                        Recipes.Add(f);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | ReadSomeRecipes", ex);
            }
            return Recipes;
        }
        internal override Recipe GetOneRecipe(int? IdRecipe)
        {
            Recipe recipe = new Recipe();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Recipes" +
                        " WHERE IdRecipe=" + IdRecipe;
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    dRead.Read();
                    recipe = GetRecipeFromRow(dRead);
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipe | ReadOneRecipe", ex);
            }
            return recipe;
        }
        internal override int? SaveOneRecipe(Recipe Recipe)
        {
            int? IdRecipe = null;
            try
            {
                if (Recipe.IdRecipe == null || Recipe.IdRecipe == 0)
                {
                    Recipe.IdRecipe = GetNextTablePrimaryKey("Recipes", "IdRecipe");
                    // INSERT new record in the table
                    InsertOneRecipe(Recipe);
                }
                else
                {
                    UpdateOneRecipe(Recipe);
                }
                return Recipe.IdRecipe;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite Datalayer | SaveOneRecipe", ex);
                return null;
            }
            return IdRecipe;
        }
        internal override void UpdateOneRecipe(Recipe RecipeToSave)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE Recipes SET " +
                    "IdRecipe=" + SqliteSafe.Int(RecipeToSave.IdRecipe) + "," +
                    "Name=" + SqliteSafe.String(RecipeToSave.Name) + "," +
                    "Description=" + SqliteSafe.String(RecipeToSave.Description) + "," +
                    "CarbohydratesPercent=" + SqliteSafe.Double(RecipeToSave.CarbohydratesPercent.Double) + "," +
                    "AccuracyOfChoEstimate=" + SqliteSafe.Double(RecipeToSave.AccuracyOfChoEstimate.Double) + "," +
                    "IsCooked=" + SqliteSafe.Bool(RecipeToSave.IsCooked) + "," +
                    "RawToCookedRatio=" + SqliteSafe.Double(RecipeToSave.RawToCookedRatio.Double) +
                    " WHERE IdRecipe=" + SqliteSafe.Int(RecipeToSave.IdRecipe);
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | UpdateOneRecipe", ex);
            }
        }
        internal override void DeleteOneRecipe(Recipe Recipe)
        {
            if (Recipe.IdRecipe == null || Recipe.IdRecipe == 0)
                return;
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM Recipes" +
                    " WHERE IdRecipe=" + Recipe.IdRecipe;
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipe | DeleteOneRecipe", ex);
            }
        }
        internal Recipe GetRecipeFromRow(DbDataReader row)
        {
            Recipe m = new Recipe();
            try
            {
                m.IdRecipe = SqlSafe.Int(row["IdRecipe"]);
                m.Name = SqlSafe.String(row["Name"]);
                m.Description = SqlSafe.String(row["Description"]);
                m.CarbohydratesPercent.Double = SqlSafe.Double(row["CarbohydratesPercent"]);
                m.AccuracyOfChoEstimate.Double = SqlSafe.Double(row["AccuracyOfChoEstimate"]);
                m.IsCooked = (bool)SqlSafe.Bool(row["IsCooked"]);
                m.RawToCookedRatio.Double = SqlSafe.Double(row["RawToCookedRatio"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | GetRecipeFromRow", ex);
            }
            return m;
        }
        internal override List<Ingredient> ReadAllIngredientsInARecipe(int? idRecipe)
        {
            List<Ingredient> Ingredients = new List<Ingredient>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Ingredients";
                    query += " WHERE IdRecipe=" + idRecipe.ToString();
                    //query += " ORDER BY Name DESC ";
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        Ingredient f = GetIngredientFromRow(dRead);
                        Ingredients.Add(f);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | ReadAllIngredientsOfARecipe", ex);
            }
            return Ingredients;
        }
        internal override int? SaveOneIngredient(Ingredient ingredient)
        {
            try
            {
                if (ingredient.IdIngredient == null || ingredient.IdIngredient == 0)
                {
                    ingredient.IdIngredient = GetNextTablePrimaryKey("Ingredients", "IdIngredient");
                    // INSERT new record in the table
                    InsertOneIngredient(ingredient);
                }
                else
                {
                    UpdateOneIngredient(ingredient);
                }
                return ingredient.IdIngredient;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | SaveOneIngredient", ex);
                return null;
            }
            return ingredient.IdIngredient;
        }
        internal override void SaveAllIngredientsInARecipe(List<Ingredient> IngredientsList)
        {
            try
            {
                // if performance is an issue, we could use e unique connection
                // and DbCommand for all the inserts
                foreach (Ingredient i in IngredientsList)
                {
                    SaveOneIngredient(i);
                }
            }
            catch (DbException ex)
            {
                Console.WriteLine("Error saving ingredients: " + ex.Message);
            }
        }
        internal override void UpdateOneIngredient(Ingredient ingredient)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        string query = "UPDATE Ingredient" +
                            "(" +
                            "IdIngredient,IdRecipe," +
                            "Name,Description,QuantityGrams,QuantityPercent,CarbohydratesPercent,IdFood";
                        query += ")VALUES(" +
                        SqliteSafe.Int(ingredient.IdIngredient) + "," +
                        SqliteSafe.Int(ingredient.IdRecipe) + "," +
                        SqliteSafe.String(ingredient.Name) + "," +
                        SqliteSafe.String(ingredient.Description) + "," +
                        SqliteSafe.Double(ingredient.QuantityGrams) + "," +
                        SqliteSafe.Double(ingredient.QuantityPercent) + "," +
                        SqliteSafe.Double(ingredient.CarbohydratesPercent) + "," +
                        SqliteSafe.Int(ingredient.IdFood) + "," +
                        ";";
                        query += ");";
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | UpdateOneIngredient", ex);
            }
        }
        internal override int? InsertOneIngredient(Ingredient ingredient)
        {
            ingredient.IdIngredient = GetNextTablePrimaryKey("Ingredients", "IdIngredient");
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Ingredients" +
                    "(" +
                    "IdIngredient, IdRecipe," +
                    "Name,Description,QuantityGrams,QuantityPercent," +
                    "CarbohydratesPercent,AccuracyOfChoEstimate,IdFood";
                    query += ")VALUES(" +
                    SqliteSafe.Int(ingredient.IdIngredient) + "," +
                    SqliteSafe.Int(ingredient.IdRecipe) + "," +
                    SqliteSafe.String(ingredient.Name) + "," +
                    SqliteSafe.String(ingredient.Description) + "," +
                    SqliteSafe.Double(ingredient.QuantityGrams) + "," +
                    SqliteSafe.Double(ingredient.QuantityPercent) + "," +
                    SqliteSafe.Double(ingredient.CarbohydratesPercent) + "," +
                    SqliteSafe.Double(ingredient.AccuracyOfChoEstimate) + "," +
                    SqliteSafe.Double(ingredient.IdFood) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return ingredient.IdIngredient;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Ingredient | InsertOneIngredient", ex);
                return null;
            }
        }
        internal override Ingredient GetIngredientFromRow(DbDataReader Row)
        {
            Ingredient m = new Ingredient();
            try
            {
                m.IdIngredient = SqlSafe.Int(Row["IdIngredient"]);
                m.IdRecipe = SqlSafe.Int(Row["IdRecipe"]);
                m.Name = SqlSafe.String(Row["Name"]);
                m.Description = SqlSafe.String(Row["Description"]);
                m.QuantityGrams.Double = SqlSafe.Double(Row["QuantityGrams"]);
                m.QuantityPercent.Double = SqlSafe.Double(Row["QuantityPercent"]);
                m.CarbohydratesPercent.Double = SqlSafe.Double(Row["CarbohydratesPercent"]);
                m.AccuracyOfChoEstimate.Double = SqlSafe.Double(Row["AccuracyOfChoEstimate"]);
                m.IdFood = SqlSafe.Int(Row["IdFood"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | GetIngredientFromRow", ex);
            }
            return m;
        }
        internal override void DeleteOneIngredient(Ingredient Ingredient)
        {
            if (Ingredient.IdIngredient == null || Ingredient.IdIngredient == 0)
                return;
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM Ingredients" +
                    " WHERE IdIngredient=" + Ingredient.IdIngredient;
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipe | DeleteOneRecipe", ex);
            }
        }

        internal override List<Recipe> SearchRecipes(string Name, string Description)
        {
            List<Recipe> list = new List<Recipe>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Recipes";
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
                        Recipe f = GetRecipeFromRow(dRead);
                        list.Add(f);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | SearchRecipes", ex);
            }
            return list;
        }
    }
}
