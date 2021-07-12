using SharedData;

namespace Comuni.XOffice
{
    public class PopulateDummySpreadsheet : AbstractPopulateSpreadsheet
    {
        internal PopulateDummySpreadsheet(string TemplateFile, string ResultFile, string LogFile) : base(TemplateFile, ResultFile, LogFile)
        {
        }

        internal override string Close()
        {
            return CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|Close():\n", null);
        }

        internal override string ReplaceSquaredParenthesisTag(string Tag, string ReplacingString)
        {
            return CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|ReplaceString():\n" + Tag + "," + ReplacingString, null);
        }

        internal override string ShowWindow()
        {
            return CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|ShowWordWindow()\n", null);
        }

        internal override string Save()
        {
            return CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|Save():\n", null);
        }

        internal override string InsertTextInCell(int SheetNumber, int RowNumber, string ColumnName,
                string CellText)
        {
            return CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|InsertTextInCell()\n" + SheetNumber + "," +
                RowNumber + "," + ColumnName + "," + CellText, null);
        }

        internal override string InsertTextInCell(int SheetNumber, int RowNumber, int ColumnNumber,
                string CellText)
        {
            return CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|InsertTextInCell():\n" + SheetNumber + "," + RowNumber +
                "," + ColumnNumber + "," + CellText, null);
        }

        internal override void DeleteRow(int SheetNumber, int Row)
        {
            CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|DeleteRowInTable():\n" + SheetNumber + "," + Row, null);
        }

        internal override int CurrentRow()
        {
            CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|CurrentRow():\n", null);
            return -1;
        }

        internal override int CurrentColumn()
        {
            CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|CurrentRow():\n", null);
            return -1;
        }

        internal override int CurrentSheet()
        {
            CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|CurrentSheet():\n", null);
            return -1;
        }

        internal override string SaveAsPdf(int[] sheetsToBeSelected)
        {
            return CommonObjectsAndData.LogDelProgramma.Error("PopulateSpreadsheet|SaveAsPdf():\n", null);
        }

        internal override object GetCurrentCellValueAndCoordinates(ref int row, ref int column)
        {
            return null;
        }

        internal override string ReplaceSquaredParenthesisTagAndNearCell(string Tag, string ReplacingString, string Note)
        {
            return null;
        }

        internal override string InsertIntegerInCell(int SheetNumber, int RowNumber, int ColumnNumber, int value)
        {
            return null;
        }

        internal override string InsertDoubleInCell(int SheetNumber, int RowNumber, int ColumnNumber, double value)
        {
            return null;
        }

        internal override void SetCurrentSheet(int foglio)
        {
            //throw new NotImplementedException();
        }
    }
}
