using gamon;
using GlucoMan.BusinessObjects;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;
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
                    "Timestamp=" + SqliteSafe.Date(Injection.EventTime.DateTime) + "," +
                    "InsulinValue=" + SqliteSafe.Double(Injection.InsulinValue.Double) + "," +
                    "InsulinCalculated=" + SqliteSafe.Double(Injection.InsulinCalculated.Double) + "," +
                    "Zone=" + (int)Injection.Zone + "," +
                    "InjectionPositionX=" + SqliteSafe.Double(Injection.PositionX) + "," +
                    "InjectionPositionY=" + SqliteSafe.Double(Injection.PositionY) + "," +
                    "Notes=" + SqliteSafe.String(Injection.Notes) + "," +
                    "IdTypeOfInjection=" + SqliteSafe.Int(Injection.IdTypeOfInjection) + "," +
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
                    "IdTypeOfInjection,IdTypeOfInsulinAction,IdInsulinDrug,InsulinString";
                    query += ")VALUES(" +
                    SqliteSafe.Int(Injection.IdInjection) + "," +
                    SqliteSafe.Date(Injection.EventTime.DateTime) + "," +
                    SqliteSafe.Double(Injection.InsulinValue.Double) + "," +
                    SqliteSafe.Double(Injection.InsulinCalculated.Double) + "," +
                    (int)Injection.Zone + "," +
                    SqliteSafe.Double(Injection.PositionX) + "," +
                    SqliteSafe.Double(Injection.PositionY) + "," +
                    SqliteSafe.String(Injection.Notes) + "," +
                    SqliteSafe.Int(Injection.IdTypeOfInjection) + "," +
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

                    if (dRead.Read())
                    {
                        g = GetInjectionFromRow(dRead);
                    }
                    
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
        // get all the injections of type Short or Rapid between InitialInstant and FinalInstant
        internal override List<Injection> GetQuickInjections(DateTime InitialInstant,
            DateTime FinalInstant)
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
                    if (InitialInstant != null && FinalInstant != null)
                    {   // add WHERE clause
                        query += " WHERE Timestamp BETWEEN '" + ((DateTime)InitialInstant).ToString("yyyy-MM-dd HH:mm:ss") +
                            "' AND '" + ((DateTime)FinalInstant).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        query += " AND (IdTypeOfInsulinAction=" + (int)Common.TypeOfInsulinAction.Short;
                        query += " OR IdTypeOfInsulinAction=" + (int)Common.TypeOfInsulinAction.Rapid;
                        query += ")";
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
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | GetQuickInjections", ex);
            }
            return list;
        }
        internal override List<Injection> GetInjections(DateTime InitialInstant, DateTime FinalInstant, 
            Common.TypeOfInsulinAction TypeOfInsulinAction = Common.TypeOfInsulinAction.NotSet, 
            Common.ZoneOfPosition Zone = Common.ZoneOfPosition.NotSet,
            bool getFront = true, bool getBack = true, bool getHands = true, bool getSensors = true)
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
                    if (InitialInstant != null && FinalInstant != null)
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
                    if (!getFront && !getBack && !getHands && !getSensors)
                    {
                        // no zone is selected, so we return all the injections
                    }
                    else if (getFront && getBack && getHands && getSensors)
                    {
                        // every zone is selected, so we return all the injections
                    }
                    else
                    {
                        // at least one zone is selected, so we add the conditions
                        query += " AND (";
                        List<string> zones = new List<string>();
                        if (getFront) zones.Add("Zone=" + (int)Common.ZoneOfPosition.Front);
                        if (getBack) zones.Add("Zone=" + (int)Common.ZoneOfPosition.Back);
                        if (getHands) zones.Add("Zone=" + (int)Common.ZoneOfPosition.Hands);
                        if (getSensors) zones.Add("Zone=" + (int)Common.ZoneOfPosition.Sensor);
                        query += string.Join(" OR ", zones);
                        query += ")";
                    }
                    query += " ORDER BY Timestamp DESC, IdInjection DESC;";
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
                ii.EventTime.DateTime = Safe.DateTime(Row["Timestamp"]);
                ii.InsulinValue.Double = Safe.Double(Row["InsulinValue"]);
                ii.InsulinCalculated.Double = Safe.Double(Row["InsulinCalculated"]);
                ii.PositionX = Safe.Double(Row["InjectionPositionX"]);
                ii.PositionY = Safe.Double(Row["InjectionPositionY"]);
                ii.Notes = Safe.String(Row["Notes"]);
                ii.IdTypeOfInjection = Safe.Int(Row["IdTypeOfInjection"]);
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

        // Append InsertInjections at the end (bulk insert)
        internal override void InsertInjections(List<Injection> List)
        {
 
            if (List == null || List.Count ==0)
                return;

            int currentKey = GetTableNextPrimaryKey("Injections", "IdInjection");
            try
            {
                using (DbConnection conn = Connect())
                {
                    using (var tran = conn.BeginTransaction())
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = "INSERT INTO Injections (IdInjection, Timestamp, InsulinValue, InsulinCalculated, InjectionPositionX, InjectionPositionY, Notes, IdTypeOfInjection, IdTypeOfInsulinAction, IdInsulinDrug, InsulinString, Zone) VALUES (@id,@ts,@insval,@inscalc,@posx,@posy,@notes,@type,@action,@drug,@istr,@zone);";

                        var pId = cmd.CreateParameter(); pId.ParameterName = "@id"; pId.DbType = DbType.Int32; cmd.Parameters.Add(pId);
                        var pTs = cmd.CreateParameter(); pTs.ParameterName = "@ts"; pTs.DbType = DbType.DateTime; cmd.Parameters.Add(pTs);
                        var pInsVal = cmd.CreateParameter(); pInsVal.ParameterName = "@insval"; pInsVal.DbType = DbType.Double; cmd.Parameters.Add(pInsVal);
                        var pInsCalc = cmd.CreateParameter(); pInsCalc.ParameterName = "@inscalc"; pInsCalc.DbType = DbType.Double; cmd.Parameters.Add(pInsCalc);
                        var pPosX = cmd.CreateParameter(); pPosX.ParameterName = "@posx"; pPosX.DbType = DbType.Double; cmd.Parameters.Add(pPosX);
                        var pPosY = cmd.CreateParameter(); pPosY.ParameterName = "@posy"; pPosY.DbType = DbType.Double; cmd.Parameters.Add(pPosY);
                        var pNotes = cmd.CreateParameter(); pNotes.ParameterName = "@notes"; pNotes.DbType = DbType.String; cmd.Parameters.Add(pNotes);
                        var pType = cmd.CreateParameter(); pType.ParameterName = "@type"; pType.DbType = DbType.Int32; cmd.Parameters.Add(pType);
                        var pAction = cmd.CreateParameter(); pAction.ParameterName = "@action"; pAction.DbType = DbType.Int32; cmd.Parameters.Add(pAction);
                        var pDrug = cmd.CreateParameter(); pDrug.ParameterName = "@drug"; pDrug.DbType = DbType.Int32; cmd.Parameters.Add(pDrug);
                        var pIstr = cmd.CreateParameter(); pIstr.ParameterName = "@istr"; pIstr.DbType = DbType.String; cmd.Parameters.Add(pIstr);
                        var pZone = cmd.CreateParameter(); pZone.ParameterName = "@zone"; pZone.DbType = DbType.Int32; cmd.Parameters.Add(pZone);

                        try { cmd.Prepare(); } catch { /* ignore */ }

                        foreach (var inj in List)
                        {
                            pId.Value = currentKey;
                            pTs.Value = inj?.EventTime?.DateTime ?? (object)DBNull.Value;
                            pInsVal.Value = inj?.InsulinValue?.Double ?? (object)DBNull.Value;
                            pInsCalc.Value = inj?.InsulinCalculated?.Double ?? (object)DBNull.Value;
                            pPosX.Value = inj?.PositionX ?? (object)DBNull.Value;
                            pPosY.Value = inj?.PositionY ?? (object)DBNull.Value;
                            pNotes.Value = string.IsNullOrEmpty(inj?.Notes) ? (object)DBNull.Value : inj.Notes;
                            pType.Value = inj?.IdTypeOfInjection ?? (object)DBNull.Value;
                            pAction.Value = inj?.IdTypeOfInsulinAction ?? (object)DBNull.Value;
                            pDrug.Value = inj?.IdInsulinDrug ?? (object)DBNull.Value;
                            pIstr.Value = string.IsNullOrEmpty(inj?.InsulinString) ? (object)DBNull.Value : inj.InsulinString;
                            pZone.Value = inj != null ? (object?)(int)inj.Zone ?? (object)DBNull.Value : (object)DBNull.Value;

                            cmd.ExecuteNonQuery();
                            inj.IdInjection = currentKey; // set generated id back
                            currentKey++;
                        }

                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_BolusesAndInjections | InsertInjections", ex);
            }
        }
    }
}
