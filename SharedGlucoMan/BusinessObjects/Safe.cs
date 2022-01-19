using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GlucoMan
{
    /// <summary>
    /// Safe read of a database field to a string, managing nulls without stopping
    /// </summary>
    internal static class Safe
    {
        internal static int? Int(object Value)
        {
            if (Value == null)
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
                return Common.DateNull;
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
    }
}
