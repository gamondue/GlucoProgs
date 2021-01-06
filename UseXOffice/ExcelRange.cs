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

//An Excel range wrapper class. Used to manipulate data and it's appearance within a page

namespace SA.TblProc.Excel
{
    public class ExcelRange : TableRange
    {
        private MSExcel.Range _range;
        private object[,] _data;

        public MSExcel.Range Range
        {
            get { return _range; }
        }
        public ExcelRange(CoordRange coordRange, MSExcel.Range xlRange)
            : base(coordRange)
        {
            _range = xlRange;
        }
        public override void Merge()
        {
            _range.Merge(System.Type.Missing);
        }
        public override void AutoFitColumns()
        {
            _range.Columns.AutoFit();
        }

        public override object Value
        {
            get
            {
                return _range.get_Value(System.Type.Missing);
            }
            set
            {
                _range.set_Value(System.Type.Missing, value);
            }
        }

        public override bool FontBold
        {
            set
            {
                _range.Font.Bold = value;
            }
        }

        public override bool FontItalic
        {
            set
            {
                _range.Font.Italic = value;
            }
        }


        public override int BackgroundColor
        {
            set
            {
                _range.Interior.ColorIndex = ConstConvert.FindClosestColorIndex(value);
            }
        }

        public override HAlign HAlign
        {
            set
            {
                _range.HorizontalAlignment = ConstConvert.ToExcel(value);
            }
        }

        public override VAlign VAlign
        {
            set
            {
                _range.VerticalAlignment = ConstConvert.ToExcel(value);
            }
        }

        public override int FontSize
        {
            get
            {
                return Convert.ToInt32(_range.Font.Size);
            }
            set
            {
                _range.Font.Size = value;
            }
        }

        public override string FontName
        {
            get
            {
                object name = _range.Font.Name;
                if (name == null) return null;
                return name.ToString();
            }
            set
            {
                _range.Font.Name = value;
            }
        }

        public override int FontColor
        {
            get
            {
                return ConstConvert.ColorIndexToRGB(FontColorIndex);
            }
            set
            {
                FontColorIndex = ConstConvert.FindClosestColorIndex(value);
            }
        }

        public override int FontColorIndex
        {
            get
            {
                return (int)_range.Font.ColorIndex;
            }
            set
            {
                _range.Font.ColorIndex = value;
            }
        }


        public override string NumberFormat
        {
            set
            {
                _range.NumberFormat = value;
            }
        }

        public override double ColumnWidth
        {
            get
            {
                return Convert.ToDouble(_range.ColumnWidth ?? 0);
            }
            set
            {
                _range.ColumnWidth = value;
            }
        }
        public override bool WrapText
        {
            set
            {
                _range.WrapText = value;
            }
        }


        public override void BorderAround(LineStyle lineStyle, BorderWeight borderWeight, object color)
        {
            _range.BorderAround(ConstConvert.ToExcel(lineStyle),
                ConstConvert.ToExcel(borderWeight), MSExcel.XlColorIndex.xlColorIndexAutomatic,
                color);
        }

        public override int Orientation
        {
            set
            {
                _range.Orientation = value;
            }
        }

        public override int BackgroundColorIndex
        {
            set
            {
                _range.Interior.ColorIndex = value;
            }
        }

        public override void SetBorderLineStyle(BordersIndex index, LineStyle style)
        {
            _range.Borders[ConstConvert.ToExcel(index)].LineStyle = ConstConvert.ToExcel(style);
        }
        public override void SetBorderWeight(BordersIndex index, BorderWeight weight)
        {
            _range.Borders[ConstConvert.ToExcel(index)].Weight = ConstConvert.ToExcel(weight);
        }

        public override void SetCharFontBold(int start, int end, bool set)
        {
            _range.get_Characters(start, end).Font.Bold = set;
        }
        public override void CreateArray()
        {
            _data = new object[_coords.Height, _coords.Width];
        }
        public override object this[int row, int col]
        {
            get { return _data[row, col]; }
            set { _data[row, col] = value; }
        }
        public override void FlushArray()
        {
            Value = _data;
        }

        public override DrawRect DrawRect 
        { 
            get 
            {
                double left=Convert.ToDouble(_range.Left);
                double top = Convert.ToDouble(_range.Top);
                return new DrawRect(left, top, 
                    left+Convert.ToDouble(_range.Width), top+Convert.ToDouble( _range.Height));

            } 
        }

    }
}
