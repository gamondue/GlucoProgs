using GlucoMan;
using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using static GlucoMan.DataLayer;

namespace SharedGlucoMan.BusinessLayer
{
    // Business Layer, general part
    public  class BL_General
    {
        DataLayer dl = Common.Database;
        public  long? SaveParameter(string FieldName, string FieldValue)
        {
            return dl.SaveParameter(FieldName, FieldValue);
        }
        public  string RestoreParameter(string FieldName)
        {
            return dl.RestoreParameter(FieldName);
        }
        public  void DeleteDatabase()
        {
            dl.DeleteDatabase();
        }
    }
}
