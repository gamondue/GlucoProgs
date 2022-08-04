using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        internal override int? SaveOneInjection(InsulinInjection Injection)
        {
            try
            {
                if (Injection.IdInsulinInjection == null || Injection.IdInsulinInjection == 0)
                {
                    Injection.IdInsulinInjection = GetNextTablePrimaryKey("InsulinInjections", "IdInsulinInjection");
                    // INSERT new record in the table
                    InsertInjection(Injection);
                }
                else
                {   // GlucoseMeasurement.IdGlucoseRecord exists
                    UpdateInjection(Injection);
                }
                return Injection.IdInsulinInjection;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusesAndInjections | SaveOneInjection", ex);
                return null;
            }
        }
        private int? UpdateInjection(InsulinInjection Injection)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE InsulinInjections SET " +
                    "Timestamp=" + SqlDate(Injection.Timestamp.DateTime) + "," +
                    "InsulinValue=" + SqlDouble(Injection.InsulinValue.Text) + "," +
                    "InsulinCalculated=" + SqlDouble(Injection.InsulinCalculated.Text) + "," +
                    "InjectionPositionX=" + SqlInt(Injection.InjectionPositionX.Int) + "," +
                    "InjectionPositionY=" + SqlInt(Injection.InjectionPositionY.Int) + "," +
                    "Notes=" + SqlString(Injection.Notes) + "," +
                    "IdTypeOfInjection=" + SqlInt(Injection.IdTypeOfInjection.Int) + "," +
                    "IdTypeOfInsulinSpeed=" + SqlInt(Injection.IdTypeOfInsulinSpeed.Int) + "," +
                    "IdTypeOfInsulinInjection=" + SqlInt(Injection.IdTypeOfInsulinInjection.Int) + "," +
                    "InsulinString=" + SqlString(Injection.InsulinString) + "" +
                    " WHERE IdInsulinInjection=" + SqlInt(Injection.IdInsulinInjection) +
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Injection.IdInsulinInjection;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusesAndInjections | UpdateInjection", ex);
                return null;
            }
        }
        private int? InsertInjection(InsulinInjection Injection)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO InsulinInjections" +
                    "(" +
                    "IdInsulinInjection,Timestamp,InsulinValue,InsulinCalculated," +
                    "InjectionPositionX,InjectionPositionY,Notes,IdTypeOfInjection," +
                    "IdTypeOfInsulinSpeed,IdTypeOfInsulinInjection,InsulinString";
                    query += ")VALUES(" +
                    SqlInt(Injection.IdInsulinInjection) + "," +
                    SqlDate(Injection.Timestamp.DateTime) + "," +
                    SqlDouble(Injection.InsulinValue.Text) + "," +
                    SqlDouble(Injection.InsulinCalculated.Text) + "," +
                    SqlInt(Injection.InjectionPositionX.Int) + "," +
                    SqlInt(Injection.InjectionPositionY.Int) + "," +
                    SqlString(Injection.Notes) + "," +
                    SqlInt(Injection.IdTypeOfInjection.Int) + "," +
                    SqlInt(Injection.IdTypeOfInsulinSpeed.Int) + "," +
                    SqlInt(Injection.IdTypeOfInsulinInjection.Int) + "," +
                    SqlString(Injection.InsulinString) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Injection.IdInsulinInjection;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusesAndInjections | InsertInjection", ex);
                return null;
            }
        }
        internal override void DeleteOneInjection(InsulinInjection Injection)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM InsulinInjections" +
                    " WHERE IdInsulinInjection=" + Injection.IdInsulinInjection;
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusesAndInjections | DeleteOneInjection", ex);
            }
        }
        internal override List<InsulinInjection> GetInjections(DateTime InitialInstant, 
            DateTime FinalInstant)
        {
            List<InsulinInjection> list = new List<InsulinInjection>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM InsulinInjections";
                    if (InitialInstant != null && FinalInstant != null)
                    {   // add WHERE clause
                        query += " WHERE Timestamp BETWEEN '" + ((DateTime)InitialInstant).ToString("yyyy-MM-dd") +
                            "' AND '" + ((DateTime)FinalInstant).ToString("yyyy-MM-dd 23:59:29") + "'";
                    }
                    query += " ORDER BY Timestamp DESC, IdInsulinInjection;";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        InsulinInjection g = GetInjectionFromRow(dRead);
                        list.Add(g);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusesAndInjections | GetInjections", ex);
            }
            return list;
        }
        internal InsulinInjection GetInjectionFromRow(DbDataReader Row)
        {
            InsulinInjection ii = new InsulinInjection();
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                ii.IdInsulinInjection = Safe.Int(Row["IdInsulinInjection"]);
                ii.Timestamp.DateTime = Safe.DateTime(Row["Timestamp"]);
                ii.InsulinValue.Double = Safe.Double(Row["InsulinValue"]);
                ii.InsulinCalculated.Double = Safe.Double(Row["InsulinCalculated"]);
                ii.InjectionPositionX.Int = Safe.Int(Row["InjectionPositionX"]);
                ii.InjectionPositionY.Int = Safe.Int(Row["InjectionPositionY"]);
                ii.Notes = Safe.String(Row["Notes"]);
                ii.IdTypeOfInjection.Int = Safe.Int(Row["IdTypeOfInjection"]);
                ii.IdTypeOfInsulinSpeed.Int = Safe.Int(Row["IdTypeOfInsulinSpeed"]);
                ii.IdTypeOfInsulinInjection.Int = Safe.Int(Row["IdTypeOfInsulinInjection"]);
                ii.InsulinString = Safe.String(Row["InsulinString"]);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusesAndInjections | GetInjectionFromRow", ex);
            }
            return ii;
        }
    }
}
 