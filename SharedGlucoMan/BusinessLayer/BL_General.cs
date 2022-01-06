using GlucoMan;
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
        internal void SaveParameter(string FieldName, string FieldValue)
        {
            dl.SaveParameter(FieldName, FieldValue);
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
