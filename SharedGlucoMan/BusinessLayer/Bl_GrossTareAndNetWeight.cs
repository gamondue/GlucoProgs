using GlucoMan;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan.BusinessLayer
{
    internal class BL_GrossTareAndNetWeight
    {
        DataLayer dl = new DataLayer();

        private DoubleAndText gross;
        private DoubleAndText container;
        private DoubleAndText net;

        private DoubleAndText oldGross = new DoubleAndText();
        private DoubleAndText oldContainer = new DoubleAndText();
        private DoubleAndText oldNet = new DoubleAndText();

        internal BL_GrossTareAndNetWeight
            (DoubleAndText GrossWeight, DoubleAndText ContainerWeight, DoubleAndText NetWeight)
        {
            gross = GrossWeight;
            container = ContainerWeight; 
            net = NetWeight;

            oldGross.Double = gross.Double;
            oldContainer.Double = container.Double;
            oldNet.Double = net.Double; 
        }

        internal void GrossOrTareChanged()
        {
            // variations in gross and container, that change the net weight 
            // if gross or container have changed and container and gross are numbers
            // sets the new value for net weight
            if ((!EqualWithNaN(gross.Double, oldGross.Double) || !EqualWithNaN(container.Double, oldContainer.Double)) 
                && !Double.IsNaN(container.Double) && !Double.IsNaN(gross.Double))
               net.Double = gross.Double - container.Double;
            else
               net.Double = double.NaN;

            oldGross.Double = gross.Double;
            oldContainer.Double = container.Double;
            oldNet.Double = net.Double;
        }
        internal void NetWeightChanged()
        {
            // if one of the other two in a number, the other is calculated
            if (gross.Double > 0)
            {
                container.Double = gross.Double - net.Double;
            }
            else if (container.Double > 0)
                gross.Double = container.Double + net.Double;

            // if net has changed and is a number, the other two must be different from a number
            if (!EqualWithNaN(net.Double, oldNet.Double)
                && !Double.IsNaN(net.Double))
            {
                gross.Double = double.NaN;
                container.Double = double.NaN;
            }
            oldGross.Double = gross.Double;
            oldContainer.Double = container.Double;
            oldNet.Double = net.Double;
        }

        private bool EqualWithNaN(Double A, Double B)
        {
            // overcomes the "problem" that NaN is always != NaN 
            return A == B || (Double.IsNaN(A) && Double.IsNaN(B));
        }
    }
}
