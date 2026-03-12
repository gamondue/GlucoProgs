using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public static partial class Common
    {
        #if !MATH_TESTS_ONLY
        public static void SetCursorToStart(Entry entry)
        {
            if (entry != null && !string.IsNullOrEmpty(entry.Text))
            {
                entry.CursorPosition = 0;
                entry.SelectionLength = 0;
            }
        }
#endif
    }
}
