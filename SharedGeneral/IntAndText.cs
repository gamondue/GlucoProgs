using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public class IntAndText
    {
        private int? intVal;
        private string text;

        public IntAndText()
        {
            Format = "";
            intVal = 0;
            text = "";
        }

        public string Format { get; set; }
        public int? Int
        {
            get => intVal;
            set
            {
                intVal = value;
                try
                {
                    text = ((int)intVal).ToString(Format);
                }
                catch
                {
                    text = null;
                }
            }
        }
        public string Text
        {
            get => text; set
            {
                text = value;
                try
                {
                    intVal = int.Parse(value);
                }
                catch
                {
                    intVal = 0;
                }
            }
        }
    }
}
