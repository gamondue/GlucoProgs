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

//An Excel table processor wrapper class. represents an xls document

namespace SA.TblProc.Excel
{
    public class ExcelApp : TableProcessor
    {
        private MSExcel.Workbook _wbook;
        private List<ExcelSheet> _pages = new List<ExcelSheet>();

        public MSExcel.Workbook Workbook
        {
            get { return _wbook; }
        }

        public ExcelApp() : this(null, 1) { }
        public ExcelApp(string firstPageName) : this(firstPageName, 1) { }
        public ExcelApp(string firstPageName, int firstPageStartRow)
        {
            MSExcel.Application xls = new MSExcel.Application();
            _wbook = xls.Workbooks.Add(MSExcel.XlWBATemplate.xlWBATWorksheet);
            MSExcel.Worksheet sheet = (MSExcel.Worksheet)(_wbook.Worksheets.get_Item(1));
            if (firstPageName != null) sheet.Name = firstPageName;
            ExcelSheet page = new ExcelSheet(sheet, firstPageStartRow);
            _pages.Add(page);
        }
        public ExcelApp(MSExcel.Workbook wbook)
        {
            this._wbook = wbook;
            for (int i = 0; i < wbook.Worksheets.Count; i++)
            {
                ExcelSheet page = new ExcelSheet((MSExcel.Worksheet)wbook.Worksheets[i], 1);
                _pages.Add(page);
            }

        }
        public override TableSheet FirstPage
        {
            get
            {
                return FirstExcelPage;
            }
        }
        public ExcelSheet FirstExcelPage
        {
            get
            {
                return _pages[0];
            }
        }
        public override TableSheet LastPage
        {
            get
            {
                return LastExcelPage;
            }
        }
        public ExcelSheet LastExcelPage
        {
            get
            {
                return _pages[_pages.Count - 1];
            }
        }

        public override TableSheet CreateNextPage(string name, int startRow)
        {
            return CreateNextExcelPage(name, startRow);
        }

        public ExcelSheet CreateNextExcelPage(string name, int startRow)
        {
            MSExcel.Worksheet prevSheet = _pages[_pages.Count - 1].Worksheet;
            MSExcel.Sheets sheets = _wbook.Worksheets;
            MSExcel.Worksheet newSheet = (MSExcel.Worksheet)sheets.Add(System.Type.Missing,
                prevSheet, 1, prevSheet.Type);
            newSheet.Name = name;
            ExcelSheet ret = new ExcelSheet(newSheet, startRow);
            _pages.Add(ret);
            return ret;
        }
        public override TableSheet this[int idx]
        {
            get
            {
                return _pages[idx];
            }
        }

        public override bool Visible
        {
            set
            {
                _wbook.Application.Visible = value;
            }
        }
    }
}
