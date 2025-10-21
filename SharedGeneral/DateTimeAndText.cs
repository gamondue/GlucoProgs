namespace gamon
{
    public class DateTimeAndText
    {
        public string Format { get; set; }
        private DateTime? dateTimeVal;
        private string text;
        public DateTimeAndText()
        {
            // default Format
            //Format = "yyyy-MM-dd HH:mm:ss";
            // default values 
            dateTimeVal = General.DateNull;
            text = "";
        }
        public DateTime? DateTime
        {
            get => dateTimeVal;
            set
            {
                dateTimeVal = value;
                if (dateTimeVal == null || dateTimeVal is DBNull)
                {
                    text = "";
                    return;
                }
                try
                {
                    //text = ((DateTime)dateTimeVal).ToString(Format);
                    text = ((DateTime)dateTimeVal).ToString();
                }
                catch
                {
                    text = "";
                }
            }
        }
        public string Text
        {
            get => text;
            set
            {
                text = value;
                if (text == null || text is DBNull || text == "")
                {
                    dateTimeVal = General.DateNull;
                    return;
                }
                try
                {
                    dateTimeVal = System.DateTime.Parse(value);
                }
                catch
                {
                    dateTimeVal = General.DateNull;
                }
            }
        }
        public override string ToString()
        {
            return text;
        }
    }
}
