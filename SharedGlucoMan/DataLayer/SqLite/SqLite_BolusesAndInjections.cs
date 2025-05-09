using gamon;
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
                    Injection.IdInsulinInjection = GetTableNextPrimaryKey("InsulinInjections", "IdInsulinInjection");
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
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | SaveOneInjection", ex);
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
                    "Timestamp=" + SqliteSafe.Date(Injection.Timestamp.DateTime) + "," +
                    "InsulinValue=" + SqliteSafe.Double(Injection.InsulinValue.Double) + "," +
                    "InsulinCalculated=" + SqliteSafe.Double(Injection.InsulinCalculated.Double) + "," +
                    "Zone=" + (int)Injection.Zone + "," +
                    "InjectionPositionX=" + SqliteSafe.Double(Injection.InjectionPositionX) + "," +
                    "InjectionPositionY=" + SqliteSafe.Double(Injection.InjectionPositionY) + "," +
                    "Notes=" + SqliteSafe.String(Injection.Notes) + "," +
                    "IdTypeOfInsulinSpeed=" + SqliteSafe.Int(Injection.IdTypeOfInsulinSpeed) + "," +
                    "IdTypeOfInsulinInjection=" + SqliteSafe.Int(Injection.IdTypeOfInsulinInjection) + "," +
                    "InsulinString=" + SqliteSafe.String(Injection.InsulinString) + "" +
                    " WHERE IdInsulinInjection=" + SqliteSafe.Int(Injection.IdInsulinInjection) +
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Injection.IdInsulinInjection;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | UpdateInjection", ex);
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
                    "Zone,InjectionPositionX,InjectionPositionY,Notes," +
                    "IdTypeOfInsulinSpeed,IdTypeOfInsulinInjection,InsulinString";
                    query += ")VALUES(" +
                    SqliteSafe.Int(Injection.IdInsulinInjection) + "," +
                    SqliteSafe.Date(Injection.Timestamp.DateTime) + "," +
                    SqliteSafe.Double(Injection.InsulinValue.Double) + "," +
                    SqliteSafe.Double(Injection.InsulinCalculated.Double) + "," +
                    (int)Injection.Zone + "," +
                    SqliteSafe.Double(Injection.InjectionPositionX) + "," +
                    SqliteSafe.Double(Injection.InjectionPositionY) + "," +
                    SqliteSafe.String(Injection.Notes) + "," +
                    SqliteSafe.Int(Injection.IdTypeOfInsulinSpeed) + "," +
                    SqliteSafe.Int(Injection.IdTypeOfInsulinInjection) + "," +
                    SqliteSafe.String(Injection.InsulinString) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Injection.IdInsulinInjection;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | InsertInjection", ex);
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
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | DeleteOneInjection", ex);
            }
        }
        internal override InsulinInjection GetOneInjection(int? IdInjection)
        {
            InsulinInjection g = null;
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM InsulinInjections" +
                        " WHERE IdInsulinInjection=" + SqliteSafe.Int(IdInjection);
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    g = GetInjectionFromRow(dRead);
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | GetOneInjection", ex);
            }
            return g;
        }
        internal override List<InsulinInjection> GetInjections(DateTime InitialInstant, DateTime FinalInstant, 
            Common.TypeOfInsulinSpeed TypeOfInsulinSpeed = Common.TypeOfInsulinSpeed.NotSet, 
            Common.ZoneOfPosition Zone = Common.ZoneOfPosition.NotSet)
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
                    if (FinalInstant != null)
                    {   // add WHERE clause
                        query += " WHERE Timestamp BETWEEN '" + ((DateTime)InitialInstant).ToString("yyyy-MM-dd HH:mm:ss") +
                            "' AND '" + ((DateTime)FinalInstant).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        if (TypeOfInsulinSpeed != Common.TypeOfInsulinSpeed.NotSet)
                        {
                            query += " AND IdTypeOfInsulinSpeed=" + (int)TypeOfInsulinSpeed;
                        }
                        if (Zone != Common.ZoneOfPosition.NotSet)
                        {
                            query += " AND Zone=" + (int)Zone;
                        }
                    }
                    else
                    {
                        if (TypeOfInsulinSpeed != Common.TypeOfInsulinSpeed.NotSet)
                        {
                            query += " WHERE IdTypeOfInsulinSpeed=" + (int)TypeOfInsulinSpeed;
                        }
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
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | GetInjections", ex);
            }
            return list;
        }
        private InsulinInjection GetInjectionFromRow(DbDataReader Row)
        {
            InsulinInjection ii = new InsulinInjection();
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                ii.IdInsulinInjection = Safe.Int(Row["IdInsulinInjection"]);
                ii.Timestamp.DateTime = Safe.DateTime(Row["Timestamp"]);
                ii.InsulinValue.Double = Safe.Double(Row["InsulinValue"]);
                ii.InsulinCalculated.Double = Safe.Double(Row["InsulinCalculated"]);
                ii.InjectionPositionX = Safe.Double(Row["InjectionPositionX"]);
                ii.InjectionPositionY = Safe.Double(Row["InjectionPositionY"]);
                ii.Notes = Safe.String(Row["Notes"]);
                ii.IdTypeOfInsulinSpeed = Safe.Int(Row["IdTypeOfInsulinSpeed"]);
                ii.IdTypeOfInsulinInjection = Safe.Int(Row["IdTypeOfInsulinInjection"]);
                ii.InsulinString = Safe.String(Row["InsulinString"]);
                if (Row["Zone"] != DBNull.Value)
                    ii.Zone = (Common.ZoneOfPosition)(Safe.Int(Row["Zone"]));
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | GetInjectionFromRow", ex);
            }
            return ii;
        }
    }
}
