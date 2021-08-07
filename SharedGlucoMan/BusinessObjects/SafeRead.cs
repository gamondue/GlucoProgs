using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace GlucoMan
{
    /// <summary>
    /// funzioni che prendono un campo di database e lo mettono nella destinazione senza inchiodarsi
    /// </summary>
    internal static class SafeRead
    {
        internal static int? SafeInt(object Value)
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

        internal static string SafeString(DbDataReader r, int FieldNumber)
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

        internal static string SafeString(object Field)
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

        internal static string SafeString(object Field, bool NullOnError)
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

        internal static Nullable<DateTime> SafeDateTime(object Field)
        {
            try
            {
                return Convert.ToDateTime(Field);
                //return DateTime.ParseExact(Campo.ToString(), "yyyy-MM-dd HH:mm:ss",
                //    System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                //return Comuni.DateNull;
                return null;
            }
        }

        internal static Nullable<DateTime> SafeDateTime(string Date)
        {
            try
            {
                return DateTime.Parse(Date);
            }
            catch
            {
                return null;
            }
        }

        internal static Nullable<double> SafeDouble(string d)
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

        internal static double? SafeDouble(object Value)
        {
            if (Value == null)
                return null;
            try
            {
                return Double.Parse(Value.ToString());
            }
            catch
            {
                return null;
            }
        }

        internal static bool? SafeBool(string field)
        {
            if (field == "" || field == "0")
                return true;
            else
                return false;
        }
    }
}
