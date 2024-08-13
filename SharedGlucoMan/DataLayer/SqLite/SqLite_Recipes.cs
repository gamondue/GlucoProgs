using gamon;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        internal override List<Recipe> ReadSomeRecipes(string whereClause)
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
                    if (whereClause != null && whereClause != "")
                        query += whereClause;
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
        internal override int? SaveOneRecipe(Recipe recipe)
        {
            int? IdRecipe = null;
            try
            {
                if (recipe.IdRecipe == null || recipe.IdRecipe == 0)
                {
                    recipe.IdRecipe = GetNextTablePrimaryKey("Recipes", "IdRecipe");
                    //Recipe.TimeStart.DateTime = DateTime.Now;
                    // INSERT new record in the table
                    InsertOneRecipe(recipe);
                }
                else
                {
                    UpdateOneRecipe(recipe);
                }
                return recipe.IdRecipe;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite Datalayer | SaveOneRecipe", ex);
                return null;
            }
            return IdRecipe;
        }
        private void UpdateOneRecipe(Recipe recipe)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE Recipes" +
                    "(" +
                    "IdRecipe," +
                    "IsRepeated,IsEnabled";
                    query += ")VALUES(" +
                    SqliteSafe.Int(recipe.IdRecipe)
                    + "," + SqliteSafe.String(recipe.Name) + "," +
                    SqliteSafe.String(recipe.Description) + "," +
                    SqliteSafe.Double(recipe.ChoPercent) + "," +
                    ";";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | InsertOneRecipe", ex);
            }
        }
        private int? InsertOneRecipe(Recipe recipe)
        {
            recipe.IdRecipe = GetNextTablePrimaryKey("Recipes", "IdRecipe");
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Recipes" +
                    "(" +
                    "IdRecipe," +
                    "IsRepeated,IsEnabled";
                    query += ")VALUES(" +
                    SqliteSafe.Int(recipe.IdRecipe)
                    + "," + SqliteSafe.String(recipe.Name) + "," +
                    SqliteSafe.String(recipe.Description) + "," +
                    SqliteSafe.Double(recipe.ChoPercent) + "," +
                    ";";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return recipe.IdRecipe;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | InsertOneRecipe", ex);
                return null;
            }
        }
        private Recipe GetRecipeFromRow(DbDataReader row)
        {
            Recipe m = new Recipe();
            try
            {
                m.IdRecipe = (int)(row["IdRecipe"]);
                m.Name = SqlSafe.String(row["Name"]);
                m.Description = SqlSafe.String(row["Description"]);
                m.ChoPercent.Double = SqlSafe.Double(row["ChoPercent"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | GetRecipeFromRow", ex);
            }
            return m;
        }
        internal override List<RecipeIngredient> ReadAllIngredientsOfARecipe(int idRecipe)
        {
            List<RecipeIngredient> RecipeIngredients = new List<RecipeIngredient>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM RecipeIngredients";
                    query += " ORDER BY Name DESC ";
                    //if (whereClause != null && whereClause != "")
                    //    query += whereClause;
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        RecipeIngredient f = GetRecipeIngredientFromRow(dRead);
                        RecipeIngredients.Add(f);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_RecipeIngredients | ReadAllRecipeIngredients", ex);
            }
            return RecipeIngredients;
        }
        internal override int? SaveOneRecipeIngredient(RecipeIngredient ingredient)
        {
            try
            {
                if (ingredient.IdIngredient == null || ingredient.IdIngredient == 0)
                {
                    ingredient.IdIngredient = GetNextTablePrimaryKey("RecipeIngredients", "IdIngredient");
                    //RecipeIngredient.TimeStart.DateTime = DateTime.Now;
                    // INSERT new record in the table
                    InsertOneRecipeIngredient(ingredient);
                }
                else
                {   
                    UpdateOneRecipeIngredient(ingredient);
                }
                return ingredient.IdIngredient;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite Datalayer | SaveOneRecipeIngredient", ex);
                return null;
            }
            return ingredient.IdIngredient;
        }
        private void UpdateOneRecipeIngredient(RecipeIngredient ingredient)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE RecipeIngredient" +
                    "(" +
                    "IdIngredient,IdRecipe," +
                    "Name,Description,QuantityGrams,QuantityPercent,ChoPercent,IdFood";
                    query += ")VALUES(" +
                    SqliteSafe.Int(ingredient.IdIngredient) + "," +
                    SqliteSafe.Int(ingredient.IdRecipe) + "," +
                    SqliteSafe.String(ingredient.Name) + "," +
                    SqliteSafe.String(ingredient.Description) + "," +
                    SqliteSafe.Double(ingredient.QuantityGrams) + "," +
                    SqliteSafe.Double(ingredient.QuantityPercent) + "," +
                    SqliteSafe.Double(ingredient.ChoPercent) + "," +
                    SqliteSafe.Int(ingredient.IdFood) + "," +
                    ";";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Recipes | UpdateOneRecipeIngredient", ex);
            }
        }
        private int? InsertOneRecipeIngredient(RecipeIngredient ingredient)
        {
            ingredient.IdIngredient = GetNextTablePrimaryKey("Ingredients", "IdIngredient");
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO RecipeIngredients" +
                    "(" +
                    "IdIngredient, IdRecipe," +
                    "Name,Description,QuantityGrams,QuantityPercent,ChoPercent,IdFood";
                    query += ")VALUES(" +
                    SqliteSafe.Int(ingredient.IdIngredient) + "," +
                    SqliteSafe.Int(ingredient.IdRecipe) + "," +
                    SqliteSafe.String(ingredient.Name) + "," + 
                    SqliteSafe.String(ingredient.Description) + "," +
                    SqliteSafe.Double(ingredient.QuantityGrams) + "," +
                    SqliteSafe.Double(ingredient.QuantityPercent) + "," +
                    SqliteSafe.Double(ingredient.ChoPercent) + "," +
                    SqliteSafe.Double(ingredient.IdFood) + "," +
                    ";";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return ingredient.IdIngredient;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_RecipeIngredients | InsertOneRecipeIngredient", ex);
                return null;
            }
        }
        private RecipeIngredient GetRecipeIngredientFromRow(DbDataReader Row)
        {
            RecipeIngredient m = new RecipeIngredient();
            try
            {
                m.IdIngredient = (int)(Row["IdIngredient"]);
                m.IdRecipe = SqlSafe.Int(Row["IdRecipe"]);
                m.Name = SqlSafe.String(Row["Name"]);
                m.Description = SqlSafe.String(Row["Description"]);
                m.QuantityGrams.Double = SqlSafe.Double(Row["QuantityGrams"]);
                m.QuantityPercent.Double = SqlSafe.Double(Row["QuantityPercent"]);
                m.ChoPercent.Double = SqlSafe.Double(Row["ChoPercent"]);
                m.IdFood = SqlSafe.Int(Row["IdFood"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_RecipeIngredients | GetRecipeIngredientFromRow", ex);
            }
            return m;
        }
    }
}
