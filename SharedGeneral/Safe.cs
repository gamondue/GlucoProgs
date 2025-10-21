using System.Data.Common;

namespace gamon
{
    /// <summary>
    /// Safe read of a database field to a string, managing nulls without stopping
    /// </summary>
    internal static class Safe 
    {
        internal static int? Int(object Value)
        {
            if (Value == null || Value is DBNull)
                return null;
            
            string strValue = Value.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(strValue))
                return null;
            
            if (int.TryParse(strValue, out int result))
                return result;
            
            return null;
        }
        
        internal static int? Int(string Value)
        {
            if (string.IsNullOrWhiteSpace(Value))
                return null;
            
            if (int.TryParse(Value.Trim(), out int result))
                return result;
            
            return null;
        }
        internal static string String(DbDataReader r, int FieldNumber)
        {
            if (r == null || FieldNumber < 0 || FieldNumber >= r.FieldCount)
                return null;
            
            try
            {
                if (r.IsDBNull(FieldNumber))
                    return null;
                
                return r.GetString(FieldNumber).Trim();
            }
            catch
            {
                return null;
            }
        }
        
        internal static string String(object Field)
        {
            if (Field == null || Field is DBNull)
                return null;
            
            string result = Field.ToString();
            return string.IsNullOrWhiteSpace(result) ? null : result.Trim();
        }
        
        internal static string String(object Field, bool NullOnError)
        {
            if (Field == null || Field is DBNull)
                return NullOnError ? null : "";
            
            try
            {
                string result = Field.ToString();
                return string.IsNullOrWhiteSpace(result) ? (NullOnError ? null : "") : result.Trim();
            }
            catch
            {
                return NullOnError ? null : "";
            }
        }
        internal static Nullable<DateTime> DateTime(object Field)
        {
            if (Field == null || Field is DBNull)
                return General.DateNull;
            
            if (Field is DateTime dt)
                return dt;
            
            string strValue = Field.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(strValue))
                return General.DateNull;
            
            if (System.DateTime.TryParse(strValue, out DateTime result))
                return result;
            
            return General.DateNull;
        }
        
        internal static Nullable<DateTime> DateTime(string Date)
        {
            if (string.IsNullOrWhiteSpace(Date))
                return null;
            
            if (System.DateTime.TryParse(Date, out DateTime result))
                return result;
            
            return null;
        }
        internal static Nullable<double> Double(string d)
        {
            if (string.IsNullOrWhiteSpace(d) || d == "\r")
                return null;
            
            string trimmed = d.Trim();
            if (double.TryParse(trimmed, out double result))
                return result;
            
            return null;
        }
        
        internal static double? Double(object Value)
        {
            if (Value == null || Value is DBNull)
                return null;
            
            if (Value is double dbl)
                return dbl;
            
            string strValue = Value.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(strValue))
                return null;
            
            if (double.TryParse(strValue, out double result))
                return result;
            
            return null;
        }
        internal static bool Bool(string field)
        {
            if (string.IsNullOrWhiteSpace(field) || field == "0")
                return false;
            
            return true;
        }
        
        internal static bool? Bool(object field)
        {
            if (field == null || field is DBNull)
                return null;
            
            if (field is bool b)
                return b;
            
            string strValue = field.ToString()?.Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(strValue))
                return null;
            
            // Riconosci valori comuni per true/false
            if (strValue == "1" || strValue == "true" || strValue == "yes" || strValue == "si" || strValue == "sì")
                return true;
            
            if (strValue == "0" || strValue == "false" || strValue == "no")
                return false;
            
            if (bool.TryParse(strValue, out bool result))
                return result;
            
            return null;
        }
        internal static TimeSpan? TimeSpanFromSeconds(object field)
        {
            if (field == null || field is DBNull)
                return null;
            
            double? seconds = Double(field);
            if (!seconds.HasValue)
                return null;
            
            try
            {
                return TimeSpan.FromSeconds(seconds.Value);
            }
            catch
            {
                return null;
            }
        }
        
        internal static TimeSpan? TimeSpanFromMinutes(object field)
        {
            if (field == null || field is DBNull)
                return null;
            
            double? minutes = Double(field);
            if (!minutes.HasValue)
                return null;
            
            try
            {
                return TimeSpan.FromMinutes(minutes.Value);
            }
            catch
            {
                return null;
            }
        }
    }
}
