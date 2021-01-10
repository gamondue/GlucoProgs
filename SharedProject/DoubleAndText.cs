using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public class DoubleAndText
    {
        private double doubleVal;
        private string text;

        public DoubleAndText()
        {
            Format = "0.0";
            doubleVal = 0;
            text = ""; 
        }

        public string Format { get; set; }
        public double Double
        {
            get => doubleVal;
            set
            {
                doubleVal = value;
                try
                {
                    text = ((double)doubleVal).ToString(Format);
                }
                catch
                {
                    text = "NaN";
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
                    doubleVal = double.Parse(value);
                }
                catch
                {
                    doubleVal = double.NaN;
                }
            }
        }
    }
}