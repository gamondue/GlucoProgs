using gamon;
using System.Data;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        /// <summary>
        /// Get all containers from the database
        /// </summary>
        internal override List<Container> GetAllContainers()
        {
            List<Container> containers = new List<Container>();
            
            try
            {
                using (var conn = Connect())
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT IdContainer, Name, Weight, Notes, PhotoFileName FROM Containers ORDER BY Name";
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Container container = new Container
                            {
                                IdContainer = Safe.Int(reader["IdContainer"]),
                                Name = Safe.String(reader["Name"]),
                                Weight = new DoubleAndText { Double = Safe.Double(reader["Weight"]) },
                                Notes = Safe.String(reader["Notes"]),
                                PhotoFileName = Safe.String(reader["PhotoFileName"])
                            };
                            
                            containers.Add(container);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("DL_Sqlite - GetAllContainers", ex);
            }
            
            return containers;
        }
        
        /// <summary>
        /// Get one container by ID
        /// </summary>
        internal override Container GetOneContainer(int idContainer)
        {
            Container container = null;
            
            try
            {
                using (var conn = Connect())
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = $"SELECT IdContainer, Name, Weight, Notes, PhotoFileName FROM Containers WHERE IdContainer = {idContainer}";
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            container = new Container
                            {
                                IdContainer = Safe.Int(reader["IdContainer"]),
                                Name = Safe.String(reader["Name"]),
                                Weight = new DoubleAndText { Double = Safe.Double(reader["Weight"]) },
                                Notes = Safe.String(reader["Notes"]),
                                PhotoFileName = Safe.String(reader["PhotoFileName"])
                            };
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("DL_Sqlite - GetOneContainer", ex);
            }
            
            return container;
        }
        
        /// <summary>
        /// Save or update a container
        /// </summary>
        internal override int? SaveContainer(Container container)
        {
            try
            {
                if (container == null)
                    return null;
                
                using (var conn = Connect())
                {
                    var cmd = conn.CreateCommand();
                    
                    if (container.IdContainer.HasValue && container.IdContainer.Value > 0)
                    {
                        // Update existing container
                        cmd.CommandText = $@"UPDATE Containers SET 
                                Name = {SqliteSafe.String(container.Name)},
                                Weight = {SqliteSafe.Double(container.Weight?.Double)},
                                Notes = {SqliteSafe.String(container.Notes)},
                                PhotoFileName = {SqliteSafe.String(container.PhotoFileName)}
                                WHERE IdContainer = {container.IdContainer.Value}";
                        
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        return container.IdContainer.Value;
                    }
                    else
                    {
                        // Insert new container
                        int newId = GetTableNextPrimaryKey("Containers", "IdContainer");
                        
                        cmd.CommandText = $@"INSERT INTO Containers (IdContainer, Name, Weight, Notes, PhotoFileName) VALUES (
                                {newId},
                                {SqliteSafe.String(container.Name)},
                                {SqliteSafe.Double(container.Weight?.Double)},
                                {SqliteSafe.String(container.Notes)},
                                {SqliteSafe.String(container.PhotoFileName)})";
                        
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        container.IdContainer = newId;
                        return newId;
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("DL_Sqlite - SaveContainer", ex);
                return null;
            }
        }
        
        /// <summary>
        /// Delete a container
        /// </summary>
        internal override bool DeleteContainer(int idContainer)
        {
            try
            {
                using (var conn = Connect())
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = $"DELETE FROM Containers WHERE IdContainer = {idContainer}";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("DL_Sqlite - DeleteContainer", ex);
                return false;
            }
        }
        
        /// <summary>
        /// Search containers by name
        /// </summary>
        internal override List<Container> SearchContainers(string name)
        {
            List<Container> containers = new List<Container>();
            
            try
            {
                using (var conn = Connect())
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = $@"SELECT IdContainer, Name, Weight, Notes, PhotoFileName 
                                   FROM Containers 
                                   WHERE Name LIKE '%{SqliteSafe.String(name)}%' 
                                   ORDER BY Name";
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Container container = new Container
                            {
                                IdContainer = Safe.Int(reader["IdContainer"]),
                                Name = Safe.String(reader["Name"]),
                                Weight = new DoubleAndText { Double = Safe.Double(reader["Weight"]) },
                                Notes = Safe.String(reader["Notes"]),
                                PhotoFileName = Safe.String(reader["PhotoFileName"])
                            };
                            
                            containers.Add(container);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("DL_Sqlite - SearchContainers", ex);
            }
            
            return containers;
        }
    }
}
