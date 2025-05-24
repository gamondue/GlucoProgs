using gamon;
using GlucoMan.BusinessObjects;
using Microsoft.Data.Sqlite;
using System.Data.Common;

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
                    "IdTypeOfInsulinSpeed=" + SqliteSafe.Int(Injection.IdTypeOfInsulinSpeed) + "," +
                    "IdTypeOfInsulinInjection=" + SqliteSafe.Int(Injection.IdTypeOfInsulinInjection) + "," +
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
                    "IdTypeOfInsulinSpeed,IdTypeOfInsulinInjection,InsulinString";
                    query += ")VALUES(" +
                    SqliteSafe.Int(Injection.IdInjection) + "," +
                    SqliteSafe.Date(Injection.Timestamp.DateTime) + "," +
                    SqliteSafe.Double(Injection.InsulinValue.Double) + "," +
                    SqliteSafe.Double(Injection.InsulinCalculated.Double) + "," +
                    (int)Injection.Zone + "," +
                    SqliteSafe.Double(Injection.PositionX) + "," +
                    SqliteSafe.Double(Injection.PositionY) + "," +
                    SqliteSafe.String(Injection.Notes) + "," +
                    SqliteSafe.Int(Injection.IdTypeOfInsulinSpeed) + "," +
                    SqliteSafe.Int(Injection.IdTypeOfInsulinInjection) + "," +
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
            Common.TypeOfInsulinSpeed TypeOfInsulinSpeed = Common.TypeOfInsulinSpeed.NotSet, 
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
    }
}
