using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public class DateTimeAndText
    {
        private DateTime dateTimeVal;
        private string text;

        public DateTimeAndText()
        {
            Format = "yyyy.MM.dd hh:mm:ss";
            dateTimeVal = DateTime.MinValue;
            text = "";
        }

        public string Format { get; set; }
        public DateTime DateTime
        {
            get => dateTimeVal;
            set
            {
                dateTimeVal = value;
                try
                {
                    text = ((DateTime)dateTimeVal).ToString(Format);
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
                    dateTimeVal = DateTime.Parse(value);
                }
                catch
                {
                    dateTimeVal = DateTime.MinValue;
                }
            }
        }
    }
}
