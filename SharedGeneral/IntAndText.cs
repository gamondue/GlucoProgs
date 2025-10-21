using System;
using System.Collections.Generic;
using System.Text;

namespace gamon
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
                if (intVal == null)
                { 
                    text = "";
                    return;
                }
                try
                {
                    text = ((int)intVal).ToString(Format);
                }
                catch
                {
                    text = "";
                }
            }
        }
        public string Text
        {
            get => text; set
            {
                text = value;
                if (text == null)
                {
                    intVal = 0;
                    return;
                }
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
        public override string ToString()
        {
            return Text;
        }
    }
}
