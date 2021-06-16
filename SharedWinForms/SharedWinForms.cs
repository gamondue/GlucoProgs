using GlucoMan;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SharedWinForms
{
    class Functions
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
        internal static void SaveCurrentValuesOfAllControls(Control ParentControl, ref string PathAndFile)
        {
            string fileContent = "";
            if (ParentControl.Controls.Count > 0)
            {
                SaveControlsValuesOfLevel(ParentControl, ref fileContent);
            }
            TextFile.StringToFile(PathAndFile, fileContent, false);
        }
        private static void SaveControlsValuesOfLevel(Control ParentControl, ref string NamesAndValues)
        {
            // recursive function! 
            foreach (Control c in ParentControl.Controls)
            {
                try
                {
                    // we don't save the values of the types of controls that we don't want to save
                    if (
                        // ADD here specific controls that you don't want to be saved
                        // make THE SAME in RestoreControlsValues()                            
                        c.Name != ""
                        && !(c is Label)
                        && !(c is Button)
                        && !(c is GroupBox)
                        && !(c is PictureBox)
                    )
                    {
                        NamesAndValues += c.Name + "\t";
                        if (c is TextBox)
                            NamesAndValues += ((TextBox)c).Text;
                        else if (c is RadioButton)
                            NamesAndValues += ((RadioButton)c).Checked.ToString();
                        else if (c is CheckBox)
                            NamesAndValues += ((CheckBox)c).Checked.ToString();
                        else
                            NamesAndValues += c.Text;
                        //if (c is DataGridView)
                        //    SaveTableContent((DataGridView)c);
                        NamesAndValues += "\n";
                    }
                }
                catch (Exception ex)
                {
                    Console.Beep(200, 300);
                }
                SaveControlsValuesOfLevel(c, ref NamesAndValues);
            }
        }
        internal static void RestoreCurrentValuesOfAllControls(Control ParentControl, string PathAndFile)
        {
            try
            {
                string[,] NamesAndValues = TextFile.FileToMatrix(PathAndFile, '\t');
                int index = 0;
                RestoreControlsValuesOfLevel(ParentControl, NamesAndValues, ref index);
            }
            catch (Exception ex)
            {
                Console.Beep(200, 300);
            }
        }
        private static void RestoreControlsValuesOfLevel(Control ParentControl, string[,] NamesAndValues, ref int Index)
        {
            // recursive function! 
            // !!!! to fix !!!! reads only a few of the controls !!!! 
            foreach (Control c in ParentControl.Controls)
            {
                // we don't restore the values of the types of controls that were not saved
                if (
                    // ADD here specific controls that you don't want to be saved
                    c.Name != ""
                    && !(c is Label)
                    && !(c is Button)
                    && !(c is GroupBox)
                    && !(c is PictureBox)
                )
                {
                    // if the name of the control is different from the one that has been saved,
                    // then the catch statement has fired when saving, so we skip this control
                    // by doing nothing 
                    if (NamesAndValues[Index, 0] != c.Name)
                    {

                    }
                    else
                    {
                        //if (c is DataGridView)
                        //    RestoreTableContent((DataGridView)c);
                        if (c is TextBox)
                            ((TextBox)c).Text = NamesAndValues[Index, 1];
                        else if (c is RadioButton)
                            ((RadioButton)c).Checked = bool.Parse(NamesAndValues[Index, 1]);
                        else if (c is CheckBox)
                            ((CheckBox)c).Checked = bool.Parse(NamesAndValues[Index, 1]);
                        else
                            c.Text = NamesAndValues[Index, 1];
                        Index++;
                        // if something went wrong 
                        // (e.g. something has changed in the U.I, controls since last execution)
                        // the Index could overflow the matrix
                        if (Index == NamesAndValues.Length)
                            break; // exits the loop 
                    }
                    RestoreControlsValuesOfLevel(c, NamesAndValues, ref Index);
                }
            }
        }
    }
}
