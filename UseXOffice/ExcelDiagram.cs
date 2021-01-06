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

//An Excel chart wrapper class.


namespace SA.TblProc.Excel
{
    public class ExcelDiagram: TableDiagram
    {
        private MSExcel.Chart _chart;
        private Excel.ExcelSheet _sheet;

        public MSExcel.Chart Chart
        {
            get { return _chart; }
        }
        public ExcelDiagram(MSExcel.Chart chart, Excel.ExcelSheet sheet)
        {
            this._chart = chart;
            this._sheet = sheet;
        }
        public void DeleteLegend()
        {
            if (_chart.Legend != null)
                _chart.Legend.Delete();
        }

        private MSExcel.Axis XAxis
        {
            get
            {
                MSExcel.Axis axis = _chart.Axes(MSExcel.XlAxisType.xlCategory, MSExcel.XlAxisGroup.xlPrimary) as MSExcel.Axis;
                if (axis == null) throw new AxisNotFoundException("X axis not found");
                return axis;
            }
        }

        private MSExcel.Axis YAxis
        {
            get
            {
                MSExcel.Axis axis = _chart.Axes(MSExcel.XlAxisType.xlValue, MSExcel.XlAxisGroup.xlPrimary) as MSExcel.Axis;
                if (axis == null) throw new AxisNotFoundException("Y axis not found");
                return axis;
            }
        }

        public override string XAxisName
        {
            get 
            {
                MSExcel.Axis axis = XAxis;
                if (!axis.HasTitle) return null;
                return axis.AxisTitle.Text;
            }
            set 
            {
                MSExcel.Axis axis = XAxis;
                if (value == null||value.Length==0)
                {
                    axis.HasTitle = false;
                }
                else
                {
                    axis.HasTitle = true;
                    axis.AxisTitle.Text = value;
                }
            }
        }
        public override string YAxisName
        {
            get
            {
                MSExcel.Axis axis = YAxis;
                if (!axis.HasTitle) return null;
                return axis.AxisTitle.Text;
            }
            set 
            {
                MSExcel.Axis axis = YAxis;
                if (value == null||value.Length==0)
                {
                    axis.HasTitle = false;
                }
                else
                {
                    axis.HasTitle = true;
                    axis.AxisTitle.Text = value;
                }
            }
        }
        public override string Name
        {
            get 
            {
                if (!_chart.HasTitle) return null;
                return _chart.ChartTitle.Text;
            }
            set 
            {
                if (value == null || value.Length == 0)
                {
                    _chart.HasTitle = false;
                }
                else
                {
                    _chart.HasTitle = true;
                    _chart.ChartTitle.Text = value;
                }
            }
        }

        public override TableRange XAxisNamesRange
        {
            set
            {
                ExcelRange rng = value as ExcelRange;
                if (rng == null)
                    throw new InvalidTableProcessorException("ExcelDiagram requires an ExcelRange for XAxisNamesRange");
                MSExcel.SeriesCollection sc = _chart.SeriesCollection(System.Type.Missing) as MSExcel.SeriesCollection;
                if (sc == null)
                    throw new ChartDataNotFoundException("Chart X axis data can't be accessed");
                int count = sc.Count;
                for (int i = 1; i <= count; i++)
                {
                    try
                    {
                        MSExcel.Series ser = _chart.SeriesCollection(i) as MSExcel.Series;
                        if (ser == null) continue;
                        ser.XValues = rng.Range;
                        return;
                    }
                    catch (Exception) { continue; }
                }
            }
        }

        public override void SetMainAxisNamesRange(TableRange range, TableSheet rangeSheet)
        {
            MSExcel.SeriesCollection sc = _chart.SeriesCollection(System.Type.Missing) as MSExcel.SeriesCollection;
            if (sc == null)
                throw new ChartDataNotFoundException("Chart legend data can't be accessed");
            int serCount = sc.Count;

            string[] names = GetOneDimensionRangeValues(range, serCount, rangeSheet);
            MainAxisNames = names;
        }

        public override string[] MainAxisNames
        {
            set
            {
                if (value == null)
                    throw new InvalidParameterException("Null array passed to MainAxisNames");
                MSExcel.SeriesCollection sc = _chart.SeriesCollection(System.Type.Missing) as MSExcel.SeriesCollection;
                if (sc == null)
                    throw new ChartDataNotFoundException("Chart legend data can't be accessed");
                int serCount = sc.Count;

                for (int i = 1; i <= value.Length && i <= serCount; i++)
                {
                    MSExcel.Series ser = _chart.SeriesCollection(i) as MSExcel.Series;
                    if (ser == null) continue;
                    if (value[i - 1] != null) ser.Name = value[i - 1];
                }
            }
        }

        public override DrawRect DrawRect
        {
            get 
            {
                MSExcel.ChartObject co = _chart.Parent as MSExcel.ChartObject;
                return new DrawRect(co.Left, co.Top,
                    co.Left + co.Width,
                    co.Top + co.Height);
            }
            set
            {
                MSExcel.ChartObject co = _chart.Parent as MSExcel.ChartObject;
                if (co == null)
                    throw new DiagramException("Unable to get chart coordinates");
                co.Top = (float)value.Top;
                co.Left = (float)value.Left;
                co.Width = (float)value.Width;
                co.Height = (float)value.Height;
            }
        }
    }
}
