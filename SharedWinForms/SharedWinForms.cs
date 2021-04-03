using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SharedWinForms
{
    class Methods
    {
        internal static Control FindFocusedControl(Control ContainingControl)
        {
            ContainerControl container = ContainingControl as ContainerControl;
            while (container != null)
            {
                ContainingControl = container.ActiveControl;
                container = ContainingControl as ContainerControl;
            }
            return ContainingControl;
        }
    }
}
