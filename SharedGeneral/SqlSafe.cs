using System;
using System.Data.Common;

namespace gamon
{
    /// <summary>
    /// SqlSafe read of a database field to a string, managing nulls without stopping
    /// </summary>
    internal static class SqlSafe
    {
        internal static int? Int(object Value)
        {
            if (Value == null || Value is DBNull)
                return null;
            try
            {
                return int.Parse(Value.ToString().Trim());
            }
            catch
            {
                return null;
            }
        }
        internal static int? Int(string Value)
        {
            if (Value == "" || Value == null)
                return null;
            try
            {
                return int.Parse(Value.Trim());
            }
            catch
            {
                return null;
            }
        }
        internal static string String(DbDataReader r, int FieldNumber)
        {
            try
            {
                return r.GetString(FieldNumber).Trim();
            }
            catch
            {
                return null;
            }
        }
        internal static string String(object Field)
        {
            if (Field == null)
                return null;
            try
            {
                return Field.ToString().Trim();
            }
            catch
            {
                return null;
            }
        }
        internal static string String(object Field, bool NullOnError)
        {
            try
            {
                return Field.ToString().Trim();
            }
            catch
            {
                if (NullOnError)
                    return null;
                else
                    return "";
            }
        }
        internal static Nullable<DateTime> DateTime(object Field)
        {
            try
            {
                return Convert.ToDateTime(Field);
                //return DateTime.ParseExact(Campo.ToString(), "yyyy-MM-dd HH:mm:ss",
                //    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return General.DateNull;
                //return null;
            }
        }
        internal static Nullable<DateTime> DateTime(string Date)
        {
            try
            {
                return System.DateTime.Parse(Date);
            }
            catch
            {
                return null;
            }
        }
        internal static Nullable<double> Double(string d)
        {
            if (d == "")
                return null;
            try
            {
                return Convert.ToDouble(d);
            }
            catch
            {
                return null;
            }
        }
        internal static double? Double(object Value)
        {
            if (Value == null)
                return null;
            try
            {
                return double.Parse(Value.ToString());
            }
            catch
            {
                return null;
            }
        }
        internal static bool? Bool(string field)
        {
            if (field == "" || field == "0")
                return true;
            else
                return false;
        }
        internal static bool? Bool(object field)
        {
            if (field == null)
                return null;
            else
                try
                {
                    return Convert.ToBoolean(field);
                }
                catch
                {
                    return null;
                }
        }
        internal static TimeSpan? TimeSpanFromSeconds(object field)
        {
            try
            {
                return TimeSpan.FromSeconds((double)field);
            }
            catch
            {
                return null;
            }
        }
        internal static TimeSpan? TimeSpanFromMinutes(object field)
        {
            try
            {
                return TimeSpan.FromMinutes((double)field);
            }
            catch
            {
                return null;
            }
        }
    }
}
