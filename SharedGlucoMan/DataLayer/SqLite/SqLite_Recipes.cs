using gamon;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        internal override int? SaveOneRecipe(Recipe Recipe)
        {
            int? IdRecipe = null;
            try
            {
                if (Recipe.IdRecipe == null || Recipe.IdRecipe == 0)
                {
                    Recipe.IdRecipe = GetNextTablePrimaryKey("Recipes", "IdRecipe");
                    //Recipe.TimeStart.DateTime = DateTime.Now;
                    // INSERT new record in the table
                    InsertOneRecipe(Recipe);
                }
                else
                {   // update existing record 
                    UpdateOneRecipe(Recipe);
                }
                return Recipe.IdRecipe;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite Datalayer | SaveOneRecipe", ex);
                return null;
            }
            return IdRecipe;
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
                Common.LogOfProgram.Error("Sqlite_Recipes | ReadAllRecipes", ex);
            }
            return Recipes;
        }
        private void UpdateOneRecipe(Recipe Recipe)
        {
            throw new NotImplementedException();
        }
        private int? InsertOneRecipe(Recipe Recipe)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    //int? secondsOfInterval = (int?)((TimeSpan)Recipe.Interval).TotalSeconds;
                    //int? secondsOfDuration = (int?)((TimeSpan)Recipe.Duration).TotalSeconds;

                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Recipes" +
                    "(" +
                    "IdRecipe," +
                    "IsRepeated,IsEnabled";
                    query += ")VALUES(" +
                    SqliteSafe.Int(Recipe.IdRecipe)
                    //+ "," + SqliteSafe.Date(Recipe.TimeStart.DateTime) + "," +
                    //SqliteSafe.Date(Recipe.TimeRecipe.DateTime) + "," +
                    //SqliteSafe.Int(secondsOfInterval) + "," +
                    //SqliteSafe.Int(secondsOfDuration) + "," +
                    //SqliteSafe.Bool(Recipe.IsRepeated) + "," +
                    //SqliteSafe.Bool(Recipe.IsEnabled) +
                    ;
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Recipe.IdRecipe;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_Recipes | InsertRecipe", ex);
                return null;
            }
        }
        private Recipe GetRecipeFromRow(DbDataReader Row)
        {
            Recipe m = new Recipe();
            try
            {
                m.IdRecipe = SqlSafe.Int(Row["IdRecipe"]);
                //m.TimeStart.DateTime = SqlSafe.DateTime(Row["TimeStart"]);
                //m.TimeRecipe.DateTime = SqlSafe.DateTime(Row["TimeRecipe"]);
                //m.Interval = SqlSafe.TimeSpanFromSeconds(Row["Interval"]);
                //m.Duration = SqlSafe.TimeSpanFromMinutes(Row["Duration"]);
                //m.IsEnabled = SqlSafe.Bool(Row["IsEnabled"]);
                //m.IsRepeated = SqlSafe.Bool(Row["IsRepeated"]);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_Recipes | GetRecipeFromRow", ex);
            }
            return m;
        }
    }
}
