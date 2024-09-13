using System.ComponentModel;

namespace gamon
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
                if (value == null || value is DBNull)
                {
                    text = null;
                    return;
                }
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
                if (doubleVal == null || doubleVal is DBNull)
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
                if (value == null || value is DBNull)
                {
                    text = null;
                }
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