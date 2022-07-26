using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan
{
    internal partial class Accuracy
    {
        internal QualitativeAccuracy GetQualitativeAccuracyGivenQuantitavive(double? NumericalAccuracy)
        {
            if (NumericalAccuracy < 0)
            {
                return QualitativeAccuracy.NotSet;
            }
            else if (NumericalAccuracy == 0)
            {
                return QualitativeAccuracy.Null;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull)
            {
                return QualitativeAccuracy.AlmostNull;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad)
            {
                return QualitativeAccuracy.VeryBad;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad)
            {
                return QualitativeAccuracy.Bad;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor)
            {
                return QualitativeAccuracy.Poor;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient)
            {
                return QualitativeAccuracy.AlmostSufficient;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient)
            {
                return QualitativeAccuracy.Sufficient;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory)
            {
                return QualitativeAccuracy.Satisfactory;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good)
            {
                return QualitativeAccuracy.Good;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding)
            {
                return QualitativeAccuracy.Outstanding;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect)
            {
                return QualitativeAccuracy.Perfect;
            }
            else
            {
                return QualitativeAccuracy.NotSet;
            }
        }
        internal Color AccuracyBackColor(double NumericalAccuracy)
        {
            Color c;
            if (NumericalAccuracy <= 0)
            {
                c = Color.Red;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull)
            {
                c = Color.DarkRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad)
            {
                c = Color.MediumVioletRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad)
            {
                c = Color.OrangeRed;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor)
            {
                c = Color.Orange;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient)
            {
                c = Color.Yellow;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient)
            {
                c = Color.YellowGreen;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory)
            {
                c = Color.GreenYellow;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good)
            {
                c = Color.Lime;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding)
            {
                c = Color.DarkSeaGreen;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect)
            {
                c = Color.Green;
            }
            else
            {
                c = Color.MintCream;
            }
            return c;
        }
        internal Color AccuracyForeColor(double NumericalAccuracy)
        {
            Color c;
            if (NumericalAccuracy <= 0)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostNull)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.VeryBad)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Bad)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Poor)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.AlmostSufficient)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Sufficient)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Satisfactory)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Good)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Outstanding)
            {
                c = Color.Black;
            }
            else if (NumericalAccuracy <= (double)QualitativeAccuracy.Perfect)
            {
                c = Color.White;
            }
            else
            {
                c = Color.White;
            }
            return c;
        }
    }
}