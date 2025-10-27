using System;

namespace GlucoMan
{
 // Represents a row in DevicesModels table
 public class DeviceModel
 {
 // Primary key
 public int? IdDeviceModel { get; set; }

 // Name of the device model (e.g. "Abbot Freestyle")
 public string Name { get; set; }

 // Id of the type of device (stored as integer in DB)
 public int? IdTypeOfDevice { get; set; }

 // Optional description
 public string Description { get; set; }

 public DeviceModel()
 {
 Name = string.Empty;
 Description = string.Empty;
 }

 public override string ToString()
 {
 return $"{IdDeviceModel?.ToString() ?? "null"}: {Name} (Type {IdTypeOfDevice?.ToString() ?? "null"})";
 }
 }
}
