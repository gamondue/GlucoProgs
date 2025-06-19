using gamon;
using GlucoMan.BusinessObjects;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        internal override int? SaveOneInjection(Injection Injection)
        {
            try
            {
                if (Injection.IdInjection == null || Injection.IdInjection == 0)
                {
                    Injection.IdInjection = GetTableNextPrimaryKey("Injections", "IdInjection");
                    // INSERT new record in the table
                    InsertInjection(Injection);
                }
                else
                {   // GlucoseMeasurement.IdGlucoseRecord exists
                    UpdateInjection(Injection);
                }
                return Injection.IdInjection;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | SaveOneInjection", ex);
                return null;
            }
        }
        private int? UpdateInjection(Injection Injection)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE Injections SET " +
                    "Timestamp=" + SqliteSafe.Date(Injection.Timestamp.DateTime) + "," +
                    "InsulinValue=" + SqliteSafe.Double(Injection.InsulinValue.Double) + "," +
                    "InsulinCalculated=" + SqliteSafe.Double(Injection.InsulinCalculated.Double) + "," +
                    "Zone=" + (int)Injection.Zone + "," +
                    "InjectionPositionX=" + SqliteSafe.Double(Injection.PositionX) + "," +
                    "InjectionPositionY=" + SqliteSafe.Double(Injection.PositionY) + "," +
                    "Notes=" + SqliteSafe.String(Injection.Notes) + "," +
                    "IdTypeOfInsulinAction=" + SqliteSafe.Int(Injection.IdTypeOfInsulinAction) + "," +
                    "IdInsulinDrug=" + SqliteSafe.Int(Injection.IdInsulinDrug) + "," +
                    "InsulinString=" + SqliteSafe.String(Injection.InsulinString) + "" +
                    " WHERE IdInjection=" + SqliteSafe.Int(Injection.IdInjection) +
                    ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Injection.IdInjection;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | UpdateInjection", ex);
                return null;
            }
        }
        private int? InsertInjection(Injection Injection)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Injections" +
                    "(" +
                    "IdInjection,Timestamp,InsulinValue,InsulinCalculated," +
                    "Zone,InjectionPositionX,InjectionPositionY,Notes," +
                    "IdTypeOfInsulinAction,IdInsulinDrug,InsulinString";
                    query += ")VALUES(" +
                    SqliteSafe.Int(Injection.IdInjection) + "," +
                    SqliteSafe.Date(Injection.Timestamp.DateTime) + "," +
                    SqliteSafe.Double(Injection.InsulinValue.Double) + "," +
                    SqliteSafe.Double(Injection.InsulinCalculated.Double) + "," +
                    (int)Injection.Zone + "," +
                    SqliteSafe.Double(Injection.PositionX) + "," +
                    SqliteSafe.Double(Injection.PositionY) + "," +
                    SqliteSafe.String(Injection.Notes) + "," +
                    SqliteSafe.Int(Injection.IdTypeOfInsulinAction) + "," +
                    SqliteSafe.Int(Injection.IdInsulinDrug) + "," +
                    SqliteSafe.String(Injection.InsulinString) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Injection.IdInjection;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | InsertInjection", ex);
                return null;
            }
        }
        internal override void DeleteOneInjection(Injection Injection)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM Injections" +
                    " WHERE IdInjection=" + Injection.IdInjection;
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
        internal override Injection GetOneInjection(int? IdInjection)
        {
            Injection g = null;
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Injections" +
                        " WHERE IdInjection=" + SqliteSafe.Int(IdInjection);
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
        internal override List<Injection> GetInjections(DateTime InitialInstant, DateTime FinalInstant, 
            Common.TypeOfInsulinAction TypeOfInsulinAction = Common.TypeOfInsulinAction.NotSet, 
            Common.ZoneOfPosition Zone = Common.ZoneOfPosition.NotSet)
        {
            List<Injection> list = new List<Injection>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Injections";
                    if (FinalInstant != null)
                    {   // add WHERE clause
                        query += " WHERE Timestamp BETWEEN '" + ((DateTime)InitialInstant).ToString("yyyy-MM-dd HH:mm:ss") +
                            "' AND '" + ((DateTime)FinalInstant).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        if (TypeOfInsulinAction != Common.TypeOfInsulinAction.NotSet)
                        {
                            query += " AND IdTypeOfInsulinAction=" + (int)TypeOfInsulinAction;
                        }
                        if (Zone != Common.ZoneOfPosition.NotSet)
                        {
                            query += " AND Zone=" + (int)Zone;
                        }
                    }
                    else
                    {
                        if (TypeOfInsulinAction != Common.TypeOfInsulinAction.NotSet)
                        {
                            query += " WHERE IdTypeOfInsulinAction=" + (int)TypeOfInsulinAction;
                        }
                    }
                    query += " ORDER BY Timestamp DESC, IdInjection;";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        Injection g = GetInjectionFromRow(dRead);
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
        private Injection GetInjectionFromRow(DbDataReader Row)
        {
            Injection ii = new Injection();
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                ii.IdInjection = Safe.Int(Row["IdInjection"]);
                ii.Timestamp.DateTime = Safe.DateTime(Row["Timestamp"]);
                ii.InsulinValue.Double = Safe.Double(Row["InsulinValue"]);
                ii.InsulinCalculated.Double = Safe.Double(Row["InsulinCalculated"]);
                ii.PositionX = Safe.Double(Row["InjectionPositionX"]);
                ii.PositionY = Safe.Double(Row["InjectionPositionY"]);
                ii.Notes = Safe.String(Row["Notes"]);
                ii.IdTypeOfInsulinAction = Safe.Int(Row["IdTypeOfInsulinAction"]);
                ii.IdInsulinDrug = Safe.Int(Row["IdInsulinDrug"]);
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
        internal override void SaveOneReferenceCoordinate(PositionOfInjection position)
        {
            // save in table PositionsOfReferences one record with the data contained in position
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    int newId = GetTableNextPrimaryKey("PositionsOfReferences", "IdPosition");
                    string query = "INSERT INTO PositionsOfReferences " +
                                   "(IdPosition, Timestamp, Zone, PositionX, PositionY, Notes) " +
                                   "VALUES (" +
                                   newId + "," +
                                   SqliteSafe.Date(DateTime.Now) + "," +
                                   SqliteSafe.Int((int)position.Zone) + "," +
                                   SqliteSafe.Double(position.PositionX) + ", " +
                                   SqliteSafe.Double(position.PositionY) + "," +
                                   SqliteSafe.String(position.Notes) + ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | SaveOneReferenceCoordinate", ex);
            }
        }
        internal override void DeleteAllReferenceCoordinates(Common.ZoneOfPosition zone)
        {
            // deletes all the records of table PositionsOfReferences in which
            // the Zone field has value Zone (paramater)
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM PositionsOfReferences WHERE Zone = " + (int)zone + ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | DeleteAllReferenceCoordinates", ex);
            }
        }
        internal override List<PositionOfInjection> GetReferencePositions(Common.ZoneOfPosition Zone)
        {
            // read all the records in Table PositionsOfReferences into a list PositionOfInjection
            List<PositionOfInjection> positions = new List<PositionOfInjection>();
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "SELECT * FROM PositionsOfReferences";
                    if (Zone != Common.ZoneOfPosition.NotSet)
                    {
                        query += " WHERE Zone = " + (int)Zone;
                    }
                    query += ";";
                    cmd.CommandText = query;

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PositionOfInjection position = new PositionOfInjection
                            {
                                IdPosition = Safe.Int(reader["IdPosition"]),
                                Timestamp = Safe.DateTime(reader["Timestamp"]),
                                Zone = (Common.ZoneOfPosition)Safe.Int(reader["Zone"]),
                                PositionX = Safe.Double(reader["PositionX"]),
                                PositionY = Safe.Double(reader["PositionY"]),
                                Notes = Safe.String(reader["Notes"])
                            };
                            positions.Add(position);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | GetReferencePositions", ex);
            }
            return positions;
        }
        internal override InsulinDrug? GetOneInsulinDrug(int? idInsulinDrug)
        {
            InsulinDrug insulinDrug = null;
            using (DbConnection conn = Connect())
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours " +
                    "FROM InsulinDrugs WHERE IdInsulinDrug=@IdInsulinDrug;";
                cmd.Parameters.Add(new SqliteParameter("@IdInsulinDrug", idInsulinDrug ?? (object)DBNull.Value));
                DbDataReader dRead = cmd.ExecuteReader();
                while (dRead.Read()) // should make zero or one turns
                {
                    insulinDrug = GetInsulinDrugFromRow(dRead);
                }
            }
            return insulinDrug;
        }
        internal override List<InsulinDrug>? GetAllInsulinDrugs
            (Common.TypeOfInsulinAction InsulineActingType = Common.TypeOfInsulinAction.NotSet)
        {
            // the parameter in ot passed the query gets all the insulins in the database
            // if the parameter is passed, only the insulins of that type are returned
            List<InsulinDrug> lidr = new();
            using (DbConnection conn = Connect())
            {
                DbCommand cmd = conn.CreateCommand();
                if (InsulineActingType == Common.TypeOfInsulinAction.NotSet)
                    cmd.CommandText = "SELECT * FROM InsulinDrugs;";
                else
                {
                    int idTypeOfInsulinAction = (int)InsulineActingType;
                    cmd.CommandText = "SELECT * FROM InsulinDrugs WHERE TypeOfInsulinAction=@IdTypeOfInsulinAction;";
                    cmd.Parameters.Add(new SqliteParameter("@IdTypeOfInsulinAction", idTypeOfInsulinAction));
                }
                DbDataReader dRead = cmd.ExecuteReader();
                while (dRead.Read())
                {
                    InsulinDrug idr = GetInsulinDrugFromRow(dRead);
                    lidr.Add(idr);
                }
            }
            return lidr;
        }
        private InsulinDrug GetInsulinDrugFromRow(DbDataReader row)
        {
            InsulinDrug idr = new InsulinDrug();
            idr.IdInsulinDrug = Safe.Int(row["IdInsulinDrug"]);
            idr.Name = Safe.String(row["Name"]);
            idr.Manufacturer = Safe.String(row["Manufacturer"]);
            idr.TypeOfInsulinAction = (Common.TypeOfInsulinAction)Safe.Int(row["TypeOfInsulinAction"]);
            idr.DurationInHours = Safe.Double(row["DurationInHours"]);
            idr.OnsetTimeTimeInHours = Safe.Double(row["OnsetTimeInHours"]);
            idr.PeakTimeInHours = Safe.Double(row["PeakTimeInHours"]);
            return idr;
        }
        internal override int? SaveInsulinDrug(InsulinDrug insulinDrug)
        {
            try
            {
                if (insulinDrug.IdInsulinDrug == null || insulinDrug.IdInsulinDrug == 0)
                {
                    insulinDrug.IdInsulinDrug = GetTableNextPrimaryKey("InsulinDrugs", "IdInsulinDrug");
                    InsertInsulinDrug(insulinDrug);
                }
                else
                {
                    UpdateInsulinDrug(insulinDrug);
                }
                return insulinDrug.IdInsulinDrug;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_DataLayerConstructorsAndGeneral | SaveInsulinDrug", ex);
                return null;
            }
            return null;
        }
        private void InsertInsulinDrug(InsulinDrug insulinDrug)
        {
            using (DbConnection conn = Connect())
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"
            INSERT INTO InsulinDrugs (IdInsulinDrug, Name, Manufacturer, TypeOfInsulinAction, DurationInHours, OnsetTimeInHours, PeakTimeInHours)
            VALUES (@IdInsulinDrug, @Name, @Manufacturer, @TypeOfInsulinAction, @DurationInHours, @OnsetTimeInHours, @PeakTimeInHours);";

                cmd.Parameters.Add(new SqliteParameter("@IdInsulinDrug", insulinDrug.IdInsulinDrug ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqliteParameter("@Name", insulinDrug.Name ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqliteParameter("@Manufacturer", insulinDrug.Manufacturer ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqliteParameter("@TypeOfInsulinAction", (int)insulinDrug.TypeOfInsulinAction));
                cmd.Parameters.Add(new SqliteParameter("@DurationInHours", insulinDrug.DurationInHours ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqliteParameter("@OnsetTimeInHours", insulinDrug.OnsetTimeTimeInHours ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqliteParameter("@PeakTimeInHours", insulinDrug.PeakTimeInHours ?? (object)DBNull.Value));

                cmd.ExecuteNonQuery();
            }
        }
        private void UpdateInsulinDrug(InsulinDrug insulinDrug)
        {
            using (DbConnection conn = Connect())
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"
            UPDATE InsulinDrugs
            SET Name = @Name,
                Manufacturer = @Manufacturer,
                TypeOfInsulinAction = @TypeOfInsulinAction,
                DurationInHours = @DurationInHours,
                OnsetTimeInHours = @OnsetTimeInHours,
                PeakTimeInHours = @PeakTimeInHours
            WHERE IdInsulinDrug = @IdInsulinDrug;";

                cmd.Parameters.Add(new SqliteParameter("@IdInsulinDrug", insulinDrug.IdInsulinDrug ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqliteParameter("@Name", insulinDrug.Name ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqliteParameter("@Manufacturer", insulinDrug.Manufacturer ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqliteParameter("@TypeOfInsulinAction", (int)insulinDrug.TypeOfInsulinAction));
                cmd.Parameters.Add(new SqliteParameter("@DurationInHours", insulinDrug.DurationInHours ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqliteParameter("@OnsetTimeInHours", insulinDrug.OnsetTimeTimeInHours ?? (object)DBNull.Value));
                cmd.Parameters.Add(new SqliteParameter("@PeakTimeInHours", insulinDrug.PeakTimeInHours ?? (object)DBNull.Value));

                cmd.ExecuteNonQuery();
            }
        }
    }
}
