using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal partial class DL_FlatText : DataLayer
    {
        internal override void PurgeDatabase()
        {
            throw new NotImplementedException();
        }
        internal override int? SaveParameter(string FieldName, string FieldValue, int? Key = null)
        {
            throw new NotImplementedException();
        }
        internal override string RestoreParameter(string FieldName, int? Key = null)
        {
            throw new NotImplementedException();
        }
    }
}
