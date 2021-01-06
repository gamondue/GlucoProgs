using System;
using System.Collections.Generic;
using System.Text;
using MSExcel = Excel;

/********************************************************************************
* Copyright : Alexander Sazonov 2009                                           //
*                                                                              //
* Email : sazon666@mail.ru                                                     //
*         sazon@freemail.ru                                                    // 
*                                                                              //
* This code may be used in any way you desire. This                            //
* file may be redistributed by any means PROVIDING it is                       //
* not sold for profit without the authors written consent, and                 //
* providing that this notice and the authors name is included.                 //
*                                                                              //
* This file is provided 'as is' with no expressed or implied warranty.         //
* The author accepts no liability if it causes any damage to your computer.    //
*                                                                              //
* Expect Bugs.                                                                 //
* Please let me know of any bugs/mods/improvements.                            //
* and I will try to fix/incorporate them into this file.                       //
* thx Amar Chaudhary for disclaimer text ;-)                                   //
* Enjoy!                                                                       //
*                                                                              //
*/
/////////////////////////////////////////////////////////////////////////////////

//An Excel sheet wrapper class. Used to manage document's pages

namespace SA.TblProc.Excel
{
    public class ExcelSheet : TableSheet
    {
        private MSExcel.Worksheet _sheet;

        public MSExcel.Worksheet Worksheet
        {
            get { return _sheet; }
        }
        public ExcelSheet(MSExcel.Worksheet sheet)
            : this(sheet, 1)
        {
        }
        public ExcelSheet(MSExcel.Worksheet sheet, int startRow)
            : base(startRow)
        {
            this._sheet = sheet;
        }
        public static ExcelSheet CreateExcelSheet(string name)
        {
            return CreateExcelSheet(name, 1);
        }
        public static ExcelSheet CreateExcelSheet(string name, int startRow)
        {
            ExcelApp app = new ExcelApp(name, startRow);
            return app.FirstExcelPage;
        }
        public override TableRange Range(CoordRange range)
        {
            return ExcelRange(range);
        }
        public ExcelRange ExcelRange(CoordRange range)
        {
            MSExcel.Range rng = _sheet.get_Range(_sheet.Cells[range.StartRow, range.StartCol],
                _sheet.Cells[range.EndRow, range.EndCol]);
            return new ExcelRange(range, rng);
        }

        public override TableRange BorderAround(CoordRange range, LineStyle lineStyle, BorderWeight borderWeight,
            object color)
        {
            ExcelRange cr = ExcelRange(range);
            cr.BorderAround(lineStyle, borderWeight, color);
            return cr;
        }

        public override void AutoFitColumns()
        {
            _sheet.Columns.AutoFit();
        }

        public override bool Visible
        {
            set { _sheet.Application.Visible = value; }
        }

        public override string Name
        {
            get { return _sheet.Name; }
            set { _sheet.Name = value; }
        }
        public override void AddPageNumbering()
        {
            if (_sheet.Application.LanguageSettings.get_LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDInstall) == 1049)
                _sheet.PageSetup.CenterFooter = "Страница &С из &К";
            else
                _sheet.PageSetup.CenterFooter = "Page &P of &N";
        }
        public override PageOrientation Orientation
        {
            set
            {
                _sheet.PageSetup.Orientation = ConstConvert.ToExcel(value);
            }
        }
        public override void PrintRowsOnEachPage(int startRow, int endRow)
        {
            _sheet.PageSetup.PrintTitleRows = "$" + startRow + ":$" + endRow;
        }

        public override int FitToPagesWide
        {
            set
            {
                _sheet.PageSetup.FitToPagesWide = value;
            }
        }
        public override int FitToPagesTall
        {
            set
            {
                _sheet.PageSetup.FitToPagesTall = value;
            }
        }

        public void Place(ExcelDiagram diagram)
        {
            
        }

        public override TableDiagram AddDiagram(TableRange dataRange, RowsCols rowsCols, DiagramType type, DrawRect rect)
        {
            MSExcel.Workbook wb = _sheet.Parent as MSExcel.Workbook;
            if (wb == null) return Null.TableDiagramNull.Get();
            ExcelRange rng = dataRange as ExcelRange;
            if (rng == null) return Null.TableDiagramNull.Get();
            MSExcel.Chart chart = wb.Sheets.Add(System.Type.Missing, _sheet, 1, MSExcel.XlSheetType.xlChart) as MSExcel.Chart;
            //MSExcel.Chart chart = wb.Charts.Add(System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing) as MSExcel.Chart;
            if (chart == null) return Null.TableDiagramNull.Get();
            chart.SetSourceData(rng.Range, ConstConvert.ToExcel(rowsCols));
            chart = chart.Location(MSExcel.XlChartLocation.xlLocationAsObject, _sheet.Name);
            chart.ChartType=ConstConvert.ToExcel(type);
            ExcelDiagram ret= new ExcelDiagram(chart, this);
            if (rect != null) ret.DrawRect = rect;
            return ret;
        }
    }
}
