using gamon;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Diagnostics.Metrics;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        internal override int GetNextPrimaryKey()
        {
            return GetTableNextPrimaryKey("GlucoseRecords", "IdGlucoseRecord");
        }
        internal override List<GlucoseRecord> GetGlucoseRecords(
            DateTime? InitialInstant = null, DateTime? FinalInstant = null)
        {
            List<GlucoseRecord> list = new List<GlucoseRecord>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM GlucoseRecords";
                    if (InitialInstant != null && FinalInstant != null)
                    {   // add WHERE clause
                        query += " WHERE TimeOfMeasurement BETWEEN " + SqliteSafe.Date(InitialInstant) +
                            " AND " + SqliteSafe.Date(FinalInstant);
                    }
                    query += " ORDER BY TimeOfMeasurement DESC";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        GlucoseRecord g = GetGlucoseRecordFromRow(dRead);
                        list.Add(g);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | ReadGlucoseMeasurements", ex);
            }
            return list;
        }
        internal override GlucoseRecord GetOneGlucoseRecord(int? IdGlucoseRecord)
        {
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM GlucoseRecords";
                    query += " WHERE idGlucoseRecord=" + IdGlucoseRecord;
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        gr = GetGlucoseRecordFromRow(dRead);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | ReadGlucoseMeasurements", ex);
            }
            return gr;
        }
        internal override List<GlucoseRecord> GetLastTwoGlucoseMeasurements()
        {
            List<GlucoseRecord> list = new List<GlucoseRecord>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM GlucoseRecords";
                    query += " ORDER BY TimeOfMeasurement DESC LIMIT 2";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        GlucoseRecord g = GetGlucoseRecordFromRow(dRead);
                        list.Add(g);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | ReadGlucoseMeasurements", ex);
            }
            return list;
        }
        internal GlucoseRecord GetGlucoseRecordFromRow(DbDataReader Row)
        {
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                int? value = Safe.Int(Row["IdGlucoseRecord"]);
                gr.IdGlucoseRecord = value;
                gr.EventTime.DateTime = Safe.DateTime(Row["TimeOfMeasurement"]);
                gr.GlucoseValue.Double = Safe.Double(Row["GlucoseValue"]);
                gr.GlucoseString = Safe.String(Row["GlucoseString"]);
                value = Safe.Int(Row["IdTypeOfGlucoseMeasurement"]);
                if (value != null)
                    gr.TypeOfGlucoseMeasurement = (Common.TypeOfGlucoseMeasurement)value;
                gr.IdOfDevice = Safe.Int(Row["IdOfDevice"]);
                gr.IdTypeOfDevice = Safe.String(Row["IdTypeOfDevice"]);
                gr.IdDeviceModel = Safe.Int(Row["IdDeviceModel"]);
                gr.Notes = Safe.String(Row["Notes"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | GetGlucoseRecordFromRow", ex);
            }
            return gr;
        }
        internal override void SaveGlucoseMeasurements(List<GlucoseRecord> List)
        {
            try
            {
                foreach (GlucoseRecord rec in List)
                {
                    SaveOneGlucoseMeasurement(rec);
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
            }
        }
        internal override long? SaveOneGlucoseMeasurement(GlucoseRecord GlucoseMeasurement)
        {
            try
            {
                if (GlucoseMeasurement.IdGlucoseRecord == null || GlucoseMeasurement.IdGlucoseRecord == 0)
                {
                    GlucoseMeasurement.IdGlucoseRecord = GetNextPrimaryKey();
                    // INSERT new record in the table
                    InsertGlucoseMeasurement(GlucoseMeasurement);
                }
                else
                {   // GlucoseMeasurement.IdGlucoseRecord exists
                    UpdateGlucoseMeasurement(GlucoseMeasurement);
                }
                return GlucoseMeasurement.IdGlucoseRecord;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
                return null;
            }
        }
        private long? UpdateGlucoseMeasurement(GlucoseRecord Measurement)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE GlucoseRecords SET " +
                    "GlucoseValue=" + SqliteSafe.Double(Measurement.GlucoseValue.Double) + "," +
                    "TimeOfMeasurement=" + SqliteSafe.Date(Measurement.EventTime.DateTime) + "," +
                    "GlucoseString=" + SqliteSafe.String(Measurement.GlucoseString) + "," +
                    "IdTypeOfGlucoseMeasurement=" + SqliteSafe.Int((int?)Measurement.TypeOfGlucoseMeasurement) + "," +
                    "IdOfDevice=" + SqliteSafe.Int(Measurement.IdOfDevice) + "," +
                    "IdTypeOfDevice=" + SqliteSafe.String(Measurement.IdTypeOfDevice) + "," +
                    "IdDeviceModel=" + SqliteSafe.Int(Measurement.IdDeviceModel) + "," +
                    "Notes=" + SqliteSafe.String(Measurement.Notes) + "";
                    query += " WHERE IdGlucoseRecord=" + SqliteSafe.Int(Measurement.IdGlucoseRecord);
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Measurement.IdGlucoseRecord;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
                return null;
            }
        }
        private long? InsertGlucoseMeasurement(GlucoseRecord Measurement)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO GlucoseRecords" +
                    "(" +
                    "IdGlucoseRecord,GlucoseValue,TimeOfMeasurement,GlucoseString," +
                    "IdTypeOfGlucoseMeasurement,IdOfDevice,IdTypeOfDevice,IdDeviceModel,Notes";
                    query += ")VALUES (" +
                    SqliteSafe.Int(Measurement.IdGlucoseRecord) + "," +
                    SqliteSafe.Double(Measurement.GlucoseValue.Double) + "," +
                    SqliteSafe.Date(Measurement.EventTime.DateTime) + "," +
                    SqliteSafe.String(Measurement.GlucoseString) + "," +
                    SqliteSafe.Int((int?)Measurement.TypeOfGlucoseMeasurement) + "," +
                    SqliteSafe.Int(Measurement.IdOfDevice) + "," +
                    SqliteSafe.String(Measurement.IdTypeOfDevice) + "," +
                    SqliteSafe.Int(Measurement.IdDeviceModel) + "," +
                    SqliteSafe.String(Measurement.Notes) + ")";
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Measurement.IdGlucoseRecord;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | InsertIntoGlucoseMeasurement", ex);
                return null;
            }
        }
        internal override void DeleteOneGlucoseMeasurement(GlucoseRecord gr)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM GlucoseRecords" +
                    " WHERE IdGlucoseRecord=" + gr.IdGlucoseRecord;
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | DeleteOneGlucoseMeasurement", ex);
            }
        }
        internal override List<GlucoseRecord> GetSensorsRecords(DateTime? InitialInstant = null, DateTime? FinalInstant = null)
        {
            List<GlucoseRecord> list = new List<GlucoseRecord>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM SensorsRecords";
                    if (InitialInstant != null && FinalInstant != null)
                    {   // add WHERE clause
                        query += " WHERE TimeOfMeasurement BETWEEN " + SqliteSafe.Date(InitialInstant) +
                            " AND " + SqliteSafe.Date(FinalInstant);
                    }
                    query += " ORDER BY TimeOfMeasurement DESC";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        GlucoseRecord g = GetGlucoseRecordFromRow(dRead);
                        list.Add(g);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | ReadGlucoseMeasurements", ex);
            }
            return list;
        }
        internal override void InsertSensorMeasurements(List<GlucoseRecord> List)
        {
            if (List == null || List.Count == 0)
                return;

            // get the first primary currentKey in the table
            int currentKey = GetTableNextPrimaryKey("SensorsRecords", "IdGlucoseRecord");
            try
            {
                using (DbConnection conn = Connect())
                {
                    // Begin a transaction to speed up bulk inserts
                    using (var tran = conn.BeginTransaction())
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = "INSERT INTO SensorsRecords (IdGlucoseRecord,GlucoseValue,TimeOfMeasurement,GlucoseString,IdTypeOfGlucoseMeasurement,IdOfDevice,IdTypeOfDevice,IdDeviceModel,Notes) VALUES (@id,@glucose,@ts,@gstr,@type,@idofdevice,@idtypeofdevice,@iddevicemodel,@notes);";

                        // create parameters once
                        var pId = cmd.CreateParameter(); pId.ParameterName = "@id"; pId.DbType = System.Data.DbType.Int32; cmd.Parameters.Add(pId);
                        var pGlucose = cmd.CreateParameter(); pGlucose.ParameterName = "@glucose"; pGlucose.DbType = System.Data.DbType.Double; cmd.Parameters.Add(pGlucose);
                        var pTs = cmd.CreateParameter(); pTs.ParameterName = "@ts"; pTs.DbType = System.Data.DbType.DateTime; cmd.Parameters.Add(pTs);
                        var pGStr = cmd.CreateParameter(); pGStr.ParameterName = "@gstr"; pGStr.DbType = System.Data.DbType.String; cmd.Parameters.Add(pGStr);
                        var pType = cmd.CreateParameter(); pType.ParameterName = "@type"; pType.DbType = System.Data.DbType.Int32; cmd.Parameters.Add(pType);
                        var pIdOfDevice = cmd.CreateParameter(); pIdOfDevice.ParameterName = "@idofdevice"; pIdOfDevice.DbType = System.Data.DbType.Int32; cmd.Parameters.Add(pIdOfDevice);
                        var pIdTypeOfDevice = cmd.CreateParameter(); pIdTypeOfDevice.ParameterName = "@idtypeofdevice"; pIdTypeOfDevice.DbType = System.Data.DbType.String; cmd.Parameters.Add(pIdTypeOfDevice);
                        var pIdDeviceModel = cmd.CreateParameter(); pIdDeviceModel.ParameterName = "@iddevicemodel"; pIdDeviceModel.DbType = System.Data.DbType.Int32; cmd.Parameters.Add(pIdDeviceModel);
                        var pNotes = cmd.CreateParameter(); pNotes.ParameterName = "@notes"; pNotes.DbType = System.Data.DbType.String; cmd.Parameters.Add(pNotes);

                        // Prepare the statement for repeated execution
                        try { cmd.Prepare(); } catch { /* Prepare may not be supported by all providers; ignore if fails */ }

                        foreach (GlucoseRecord Measurement in List)
                        {
                            // assign parameter values (use DBNull for nulls)
                            pId.Value = currentKey;
                            pGlucose.Value = Measurement?.GlucoseValue?.Double ?? (object)DBNull.Value;
                            pTs.Value = Measurement?.EventTime?.DateTime ?? (object)DBNull.Value;
                            pGStr.Value = string.IsNullOrEmpty(Measurement?.GlucoseString) ? (object)DBNull.Value : Measurement.GlucoseString;
                            pType.Value = (Measurement != null) ? (object?)(int)Measurement.TypeOfGlucoseMeasurement ?? (object)DBNull.Value : (object)DBNull.Value;
                            pIdOfDevice.Value = Measurement?.IdOfDevice ?? (object)DBNull.Value;
                            pIdTypeOfDevice.Value = string.IsNullOrEmpty(Measurement?.IdTypeOfDevice) ? (object)DBNull.Value : Measurement.IdTypeOfDevice;
                            pIdDeviceModel.Value = Measurement?.IdDeviceModel ?? (object)DBNull.Value;
                            pNotes.Value = string.IsNullOrEmpty(Measurement?.Notes) ? (object)DBNull.Value : Measurement.Notes;

                            cmd.ExecuteNonQuery();
                            currentKey++;
                        }

                        // commit transaction
                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | InsertSensorMeasurements", ex);
            }
        }
        // ModelsOfDevices CRUD
        internal override List<DeviceModel> GetSomeDeviceModels(string whereClause)
        {
            List<DeviceModel> list = new();
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "SELECT IdDeviceModel, Name, IdTypeOfDevice, Description FROM DevicesModels";
                    if (!string.IsNullOrWhiteSpace(whereClause))
                        query += " WHERE " + whereClause;
                    query += ";";
                    cmd.CommandText = query;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var m = new DeviceModel
                            {
                                IdDeviceModel = Safe.Int(reader["IdDeviceModel"]),
                                Name = Safe.String(reader["Name"]),
                                IdTypeOfDevice = Safe.Int(reader["IdTypeOfDevice"]),
                                Description = Safe.String(reader["Description"])
                            };
                            list.Add(m);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | GetSomeDeviceModels", ex);
            }
            return list;
        }
        internal override DeviceModel GetOneDeviceModel(int? idModel)
        {
            DeviceModel m = null;
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT IdDeviceModel, Name, IdTypeOfDevice, Description FROM DevicesModels WHERE IdDeviceModel=@id;";
                    cmd.Parameters.Add(new SqliteParameter("@id", idModel ?? (object)DBNull.Value));
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            m = new DeviceModel
                            {
                                IdDeviceModel = Safe.Int(reader["IdDeviceModel"]),
                                Name = Safe.String(reader["Name"]),
                                IdTypeOfDevice = Safe.Int(reader["IdTypeOfDevice"]),
                                Description = Safe.String(reader["Description"])
                            };
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | GetOneDeviceModel", ex);
            }
            return m;
        }
        internal override int? InsertOneDeviceModel(DeviceModel model)
        {
            try
            {
                int newId = GetTableNextPrimaryKey("DevicesModels", "IdDeviceModel");
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO DevicesModels (IdDeviceModel, Name, IdTypeOfDevice, Description) VALUES (@id,@name,@type,@desc);";
                    cmd.Parameters.Add(new SqliteParameter("@id", newId));
                    cmd.Parameters.Add(new SqliteParameter("@name", model.Name ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@type", model.IdTypeOfDevice ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@desc", model.Description ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                model.IdDeviceModel = newId;
                return newId;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | InsertOneDeviceModel", ex);
                return null;
            }
        }
        internal override int? SaveOneDeviceModel(DeviceModel model)
        {
            try
            {
                if (model.IdDeviceModel == null || model.IdDeviceModel ==0)
                {
                    return InsertOneDeviceModel(model);
                }
                else
                {
                    UpdateOneDeviceModel(model);
                    return model.IdDeviceModel;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveOneDeviceModel", ex);
                return null;
            }
        }
        internal override void UpdateOneDeviceModel(DeviceModel model)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE DevicesModels SET Name=@name, IdTypeOfDevice=@type, Description=@desc WHERE IdDeviceModel=@id;";
                    cmd.Parameters.Add(new SqliteParameter("@id", model.IdDeviceModel ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@name", model.Name ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@type", model.IdTypeOfDevice ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@desc", model.Description ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | UpdateOneDeviceModel", ex);
            }
        }
        internal override void DeleteOneDeviceModel(int? idModel)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "DELETE FROM DevicesModels WHERE IdDeviceModel=@id;";
                    cmd.Parameters.Add(new SqliteParameter("@id", idModel ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | DeleteOneDeviceModel", ex);
            }
        }
        // Devices CRUD
        internal override List<Device> GetAllDevices(string whereClause = null)
        {
            List<Device> list = new();
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "SELECT IdDevice, PhysicalCode, IdDeviceModel, StartTime, FinishTime FROM Devices";
                    if (!string.IsNullOrWhiteSpace(whereClause))
                        query += " WHERE " + whereClause;
                    query += ";";
                    cmd.CommandText = query;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var d = new Device
                            {
                                IdDevice = Safe.Int(reader["IdDevice"]),
                                PhysicalCode = Safe.String(reader["PhysicalCode"]),
                                IdDeviceModel = Safe.Int(reader["IdDeviceModel"]),
                                StartTime = Safe.DateTime(reader["StartTime"]),
                                FinishTime = Safe.DateTime(reader["FinishTime"])
                            };
                            list.Add(d);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | GetAllDevices", ex);
            }
            return list;
        }
        internal override Device GetOneDevice(int? idDevice)
        {
            Device d = null;
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT IdDevice, PhysicalCode, IdDeviceModel, StartTime, FinishTime FROM Devices WHERE IdDevice=@id;";
                    cmd.Parameters.Add(new SqliteParameter("@id", idDevice ?? (object)DBNull.Value));
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            d = new Device
                            {
                                IdDevice = Safe.Int(reader["IdDevice"]),
                                PhysicalCode = Safe.String(reader["PhysicalCode"]),
                                IdDeviceModel = Safe.Int(reader["IdDeviceModel"]),
                                StartTime = Safe.DateTime(reader["StartTime"]),
                                FinishTime = Safe.DateTime(reader["FinishTime"])
                            };
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | GetOneDevice", ex);
            }
            return d;
        }
        internal override int? InsertDevice(Device device)
        {
            try
            {
                int newId = GetTableNextPrimaryKey("Devices", "IdDevice");
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO Devices (IdDevice, PhysicalCode, IdDeviceModel, StartTime, FinishTime) VALUES (@id,@code,@model,@start,@finish);";
                    cmd.Parameters.Add(new SqliteParameter("@id", newId));
                    cmd.Parameters.Add(new SqliteParameter("@code", device.PhysicalCode ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@model", device.IdDeviceModel ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@start", device.StartTime ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@finish", device.FinishTime ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                device.IdDevice = newId;
                return newId;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | InsertDevice", ex);
                return null;
            }
        }
        internal override int? SaveDevice(Device device)
        {
            try
            {
                if (device.IdDevice == null || device.IdDevice ==0)
                    return InsertDevice(device);
                UpdateDevice(device);
                return device.IdDevice;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveDevice", ex);
                return null;
            }
        }
        internal override void UpdateDevice(Device device)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE Devices SET PhysicalCode=@code, IdDeviceModel=@model, StartTime=@start, FinishTime=@finish WHERE IdDevice=@id;";
                    cmd.Parameters.Add(new SqliteParameter("@id", device.IdDevice ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@code", device.PhysicalCode ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@model", device.IdDeviceModel ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@start", device.StartTime ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@finish", device.FinishTime ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | UpdateDevice", ex);
            }
        }
        internal override void DeleteDevice(int? idDevice)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "DELETE FROM Devices WHERE IdDevice=@id;";
                    cmd.Parameters.Add(new SqliteParameter("@id", idDevice ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | DeleteDevice", ex);
            }
        }
        internal override Device? GetDeviceBySerialNumber(int? IdDeviceModel, string PhysicalCode)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT IdDevice, PhysicalCode, IdDeviceModel, StartTime, FinishTime" +
                        " FROM Devices" +
                        " WHERE IdDeviceModel=@model AND PhysicalCode=@code;";
                    cmd.Parameters.Add(new SqliteParameter("@model", IdDeviceModel ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@code", PhysicalCode ?? (object)DBNull.Value));
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var d = new Device
                            {
                                IdDevice = Safe.Int(reader["IdDevice"]),
                                PhysicalCode = Safe.String(reader["PhysicalCode"]),
                                IdDeviceModel = Safe.Int(reader["IdDeviceModel"]),
                                StartTime = Safe.DateTime(reader["StartTime"]),
                                FinishTime = Safe.DateTime(reader["FinishTime"])
                            };
                            cmd.Dispose();
                            return d;
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | GetDeviceBySerialNumber", ex);
            }
            return null;
        }
        internal override Device? GetDeviceById(int? IdDevice)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT IdDevice, PhysicalCode, IdDeviceModel, StartTime, FinishTime" +
                        " FROM Devices" +
                        " WHERE IdDevice=@id;";
                    cmd.Parameters.Add(new SqliteParameter("@id", IdDevice ?? (object)DBNull.Value));
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var d = new Device
                            {
                                IdDevice = Safe.Int(reader["IdDevice"]),
                                PhysicalCode = Safe.String(reader["PhysicalCode"]),
                                IdDeviceModel = Safe.Int(reader["IdDeviceModel"]),
                                StartTime = Safe.DateTime(reader["StartTime"]),
                                FinishTime = Safe.DateTime(reader["FinishTime"])
                            };
                            cmd.Dispose();
                            return d;
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | GetDeviceBySerialNumber", ex);
            }
            return null;
        }
    }
}
