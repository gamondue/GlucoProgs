using System;
using System.ComponentModel;

namespace GlucoMan
{
    [Browsable(false)]
    public class DoubleAndText
    {
        private double? doubleVal;
        private string text;

        public DoubleAndText()
        {
            Format = "0.0";
            doubleVal = null;
            text = "NaN"; 
        }
        public string Format { get; set; }
        public double? Double
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
                    text = null;
                }
            }
        }
        public string Text
        {
            get
            {
                if (doubleVal == null)
                    return ""; 
                try
                {
                    return ((double)doubleVal).ToString(Format);
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                text = value;
                try
                {
                    doubleVal = double.Parse(value);
                }
                catch (Exception)
                {
                    doubleVal = null;
                }
            }
        }
        public override string ToString()
        {
            return text;
        }
    }
}