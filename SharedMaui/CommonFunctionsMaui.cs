
namespace GlucoMan
{
    public static partial class Common
    {
        public static void SetCursorToStart(Entry entry)
        {
            if (entry != null && !string.IsNullOrEmpty(entry.Text))
            {
                entry.CursorPosition = 0;
                entry.SelectionLength = 0;
            }
        }
    }
}
