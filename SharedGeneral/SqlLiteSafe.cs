using System;

namespace gamon
{
    public static class SqliteSafe
    {
        // Sql Helper functions that prepares values to be inserted in Sql queries  

        #region functions that prepare the value of a variable to be used in a SQL statement 
        public static string String(string String)
        {
            if (String == null) return "null";
            string temp;
            if (!(String == null))
            {
                temp = String;

                //temp = temp.Replace("\"", "\"\"");
                temp = temp.Replace("'", "''");
                //temp = "'" + temp + "'";
            }
            else
                temp = "";
            temp = "'" + temp + "'";
            return temp;
        }
        public static string String(string String, int MaxLenght)
        {
            if (String == null) return "null";
            string temp;
            if (!(String == null))
            {
                temp = String;

                //temp = temp.Replace("\"", "\"\"");
                temp = temp.Replace("'", "''");
                //temp = "'" + temp + "'";
            }
            else
                temp = "";
            if (MaxLenght > 0 && temp.Length > MaxLenght)
                temp = temp.Substring(0, MaxLenght);
            temp = "'" + temp + "'";
            return temp;
        }
        public static string SqlStringLike(string String)
        {
            if (String == null) return "null";
            string temp;
            if (!(String == null))
            {
                temp = String;

                //temp = temp.Replace("\"", "\"\"");
                temp = temp.Replace("'", "''");
                //temp = "'" + temp + "'";
            }
            else
                temp = "";
            temp = "LIKE '%" + temp + "%'";
            return temp;
        }
        public static string Bool(object Value)
        {
            if (Value == null)
                return "null";
            if ((bool)Value == false)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }
        public static string Double(string Number)
        {
            try
            {
                if (Number != null)
                {
                    double dummy = double.Parse(Number);
                    if (double.IsNaN(dummy) || double.IsInfinity(dummy))
                        return "null";
                    return double.Parse(Number).ToString().Replace(",", ".");
                }
                else
                    return "null";
            }
            catch
            {
                return "null";
            }
        }
        public static string Double(object Number)
        {
            if (Number == null)
                return "null";
            // restituisce null se dà errore, perché viene usato con double? 
            try
            {
                double dummy = (double)Number;
                if (double.IsNaN(dummy) || double.IsInfinity(dummy))
                    return "null";
                return Number.ToString().Replace(",", ".");
            }
            catch (Exception e)
            {
                return "null";
            }
        }
        public static string SqlFloat(float Number)
        {
            try
            {
                float dummy = (float)Number;
                if (double.IsNaN(dummy) || double.IsInfinity(dummy))
                    return "null";
                return Number.ToString().Replace(",", ".");
            }
            catch
            {
                return "null";
            }
        }
        public static string SqlFloat(string Number)
        {
            try
            {
                float dummy = float.Parse(Number);
                if (double.IsNaN(dummy) || double.IsInfinity(dummy))
                    return "null";
                return float.Parse(Number).ToString().Replace(",", ".");
            }
            catch
            {
                return "null";
            }
        }
        public static string Int(string Number)
        {
            try
            {
                if (Number != null)
                    return int.Parse(Number).ToString();
                else
                    return "null";
            }
            catch
            {
                return "null";
            }
        }
        public static string Int(int? Number)
        {
            if (Number == null) return "null";
            try
            {
                return Number.ToString();
            }
            catch
            {
                return "null";
            }
        }
        public static string Date(string Date)
        {
            if (Date is null)
                return "null";
            if (Date == "")
                return "null";

            DateTime? d = System.DateTime.Parse(Date);
            return ("datetime('" + ((DateTime)d).ToString("yyyy-MM-dd HH:mm:ss").Replace('.', ':') + "')");
        }
        public static string Date(DateTime? Date)
        {
            if (Date != null)
                return ("datetime('" + ((DateTime)Date).ToString("yyyy-MM-dd HH:mm:ss").Replace('.', ':') + "')");
            else
                return "null";
        }
        public static string CleanStringForQuery(string Query)
        {
            // pulisce la stringa dalle andate a capo e dai tab 
            Query = Query.Replace("\t", " ");
            Query = Query.Replace("\r\n", " ");
            Query = Query.Replace("  ", " ");
            Query = Query.Replace("  ", " ");

            while (Query.Contains("  "))
                Query = Query.Replace("  ", " ");
            return Query;
        }
        #endregion
    }
}