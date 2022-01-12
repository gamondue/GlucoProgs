using GlucoMan;
using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using static GlucoMan.DataLayer;

namespace SharedGlucoMan.BusinessLayer
{
    // Business Layer, general part
    internal class BL_General
    {
        DataLayer dl = Common.Database;
        internal long? SaveParameter(string FieldName, string FieldValue)
        {
            return dl.SaveParameter(FieldName, FieldValue);
        }
        internal string RestoreParameter(string FieldName)
        {
            return dl.RestoreParameter(FieldName);
        }
        internal void PurgeDatabase()
        {
            dl.PurgeDatabase();
        }
    }
}
