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
                    "Timestamp=" + SqliteHelper.Date(Injection.Timestamp.DateTime) + "," +
                    "InsulinValue=" + SqliteHelper.Double(Injection.InsulinValue.Text) + "," +
                    "InsulinCalculated=" + SqliteHelper.Double(Injection.InsulinCalculated.Text) + "," +
                    "InjectionPositionX=" + SqliteHelper.Int(Injection.InjectionPositionX.Int) + "," +
                    "InjectionPositionY=" + SqliteHelper.Int(Injection.InjectionPositionY.Int) + "," +
                    "Notes=" + SqliteHelper.String(Injection.Notes) + "," +
                    "IdTypeOfInsulinSpeed=" + SqliteHelper.Int(Injection.IdTypeOfInsulinSpeed.Int) + "," +
                    "IdTypeOfInsulinInjection=" + SqliteHelper.Int(Injection.IdTypeOfInsulinInjection.Int) + "," +
                    "InsulinString=" + SqliteHelper.String(Injection.InsulinString) + "" +
                    " WHERE IdInsulinInjection=" + SqliteHelper.Int(Injection.IdInsulinInjection) +
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
                    "InjectionPositionX,InjectionPositionY,Notes," +
                    "IdTypeOfInsulinSpeed,IdTypeOfInsulinInjection,InsulinString";
                    query += ")VALUES(" +
                    SqliteHelper.Int(Injection.IdInsulinInjection) + "," +
                    SqliteHelper.Date(Injection.Timestamp.DateTime) + "," +
                    SqliteHelper.Double(Injection.InsulinValue.Text) + "," +
                    SqliteHelper.Double(Injection.InsulinCalculated.Text) + "," +
                    SqliteHelper.Int(Injection.InjectionPositionX.Int) + "," +
                    SqliteHelper.Int(Injection.InjectionPositionY.Int) + "," +
                    SqliteHelper.String(Injection.Notes) + "," +
                    SqliteHelper.Int(Injection.IdTypeOfInsulinSpeed.Int) + "," +
                    SqliteHelper.Int(Injection.IdTypeOfInsulinInjection.Int) + "," +
                    SqliteHelper.String(Injection.InsulinString) + "";
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
                        " WHERE IdInsulinInjection=" + SqliteHelper.Int(IdInjection); 
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
                Common.LogOfProgram.Error("Sqlite_BolusesAndInjections | GetOneInjection", ex);
            }
            return g;
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
        internal List<InsulinInjection> GetInjectionsStillEffective(DateTime dateTime, object thresholdTime)
        {
            int maximumDurationInHoursOfTheInsulineInAnyKindOfMedicine = 24;
            DateTime now = DateTime.Now;
            DateTime minimumTimeToBeStillEffective =
                now.Subtract(new TimeSpan(maximumDurationInHoursOfTheInsulineInAnyKindOfMedicine,
                0, 0));
            List<InsulinInjection> ii = GetInjectionsAfterDate(minimumTimeToBeStillEffective);
            return ii;
        }
        internal List<InsulinInjection> GetInjectionsAfterDate(DateTime minimumTimeToBeStillEffective)
        {
            List<InsulinInjection> ii = new List<InsulinInjection>();
            foreach (InsulinInjection inj in ii)
            {

            }
            return ii;
        }
    }
}
 