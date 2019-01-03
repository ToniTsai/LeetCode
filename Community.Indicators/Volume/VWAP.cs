using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class VWAP : DataSeries
    {
        public VWAP(Bars bars, string description)
            : base(bars, description)
        {
            Helper.CompatibilityCheck();

            base.FirstValidValue = 1;

            //Can't work if data isn't intraday
            if (!bars.IsIntraday)
                return;

            double x = 0;
            double MVol = 0;

            //First, compute the typical price for the intraday period. This is the average of the high, low and close {(H+L+C)/3)}. 
            AveragePrice ap = AveragePrice.Series(bars);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                // daily initialization
                if (bars.IntradayBarNumber(bar) == 0)
                {
                    x = 0;
                    MVol = 0;

                    //20180413 fix for not taking the intraday bar 0 value, replacing it with the prior day VWAP closing value
                    //if (bar > 0)
                    //    base[bar] = base[bar - 1];
                }
                //else  //20180413 fix for not taking the intraday bar 0 value, replacing it with the prior day VWAP closing value
                {
                    //Second, multiply the typical price by the period's volume. 
                    //Third, create a running total of these values. This is also known as a cumulative total. 
                    x += ap[bar] * bars.Volume[bar];

                    //Fourth, create a running total of volume (cumulative volume).
                    MVol += bars.Volume[bar];

                    //Fifth, divide the running total of price-volume by the running total of volume. 
                    base[bar] = x / MVol;
                }

                // Old version (prior to 2012.04)
                /*x += ap[bar] * bars.Volume[bar];
                MVol += bars.Volume[bar];
                if (MVol == 0)
                    base[bar] = ap[bar];
                else
                    base[bar] = x / MVol;*/
            }
        }

        public static VWAP Series(Bars bars)
        {
            string description = string.Concat(new object[] { "VWAP()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (VWAP)bars.Cache[description];
            }

            VWAP _VWAP = new VWAP(bars, description);
            bars.Cache[description] = _VWAP;
            return _VWAP;
        }
    }

    public class VWAPHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static VWAPHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 2;
            }
        }

        public override string Description
        {
            get
            {
                return "The Volume-Weighted Average Price is the ratio of the value traded to total volume traded over a particular time horizon.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(VWAP);
            }
        }

        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return _paramDefaults;
            }
        }

        public override IList<string> ParameterDescriptions
        {
            get
            {
                return _paramNames;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/VWAP.ashx";
            }
        }
    }
}