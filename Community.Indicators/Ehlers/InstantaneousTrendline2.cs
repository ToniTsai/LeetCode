using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    //ITrend Indicator class
    public class InstantaneousTrendline2 : DataSeries
    {
        public static InstantaneousTrendline2 Series(DataSeries ds, int period)
        {
            //Build description
            string description = "InstantaneousTrendline2(" + ds.Description + "," + period + ")";
            //See if it exists in the cache
            if (ds.Cache.ContainsKey(description))
                return (InstantaneousTrendline2)ds.Cache[description];
            //Create ITrend, cache it, return it
            InstantaneousTrendline2 itrend = new InstantaneousTrendline2(ds, period, description);
            ds.Cache[description] = itrend;
            return itrend;
        }

        //Constructor
        public InstantaneousTrendline2(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            // if period=0 then make period the default from Ehlers book
            // otherwise choose your own period
            if (period == 0.0)
                period = 39; // from Figure 3.5, p25

            if (period < 2)
                period = 2;

            //Remember parameters
            _sourceSeries = ds;
            _period = period;
            //Assign first bar that contains indicator data
            FirstValidValue = 3 * period - 1 + ds.FirstValidValue;

            // Defensive
            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            //Setup coefficients
            this[0] = ds[0];
            this[1] = ds[1];

            double alpha = 2.0d / (period + 1.0d);

            double t2 = alpha * alpha / 2.0d;
            double t1 = alpha - (t2 / 2.0d);
            double t3 = -(alpha - 3.0d * t2 / 2.0d);
            double t4 = 2.0d * (1.0d - alpha);
            double t5 = -(1.0d - alpha) * (1.0d - alpha);

            double v1 = ds[0];
            double v0 = ds[1];
            double v2 = 0;

            //Calculate ITrend values
            for (int bar = 2; bar < ds.Count; bar++)
            {
                v2 = v1;
                v1 = v0;

                v0 = t1 * ds[bar] + t2 * ds[bar - 1] + t3 * ds[bar - 2]
                   + t4 * v1 + t5 * v2;

                this[bar] = v0;
            }
        }
        //Calculate a value for a partial bar
        public override void CalculatePartialValue()
        {
            if (_sourceSeries.Count < _period - 1 || _sourceSeries.PartialValue == Double.NaN)
                PartialValue = Double.NaN;
            else
            {
                this[_sourceSeries.Count - 3 * _period + 1] = _sourceSeries[_sourceSeries.Count - 3 * _period + 1];
                this[_sourceSeries.Count - 3 * _period + 2] = _sourceSeries[_sourceSeries.Count - 3 * _period + 2];

                double alpha = 2.0d / (_period + 1.0d);

                double t2 = alpha * alpha / 2.0d;
                double t1 = alpha - (t2 / 2.0d);
                double t3 = -(alpha - 3.0d * t2 / 2.0d);
                double t4 = 2.0d * (1.0d - alpha);
                double t5 = -(1.0d - alpha) * (1.0d - alpha);

                double v1 = _sourceSeries[_sourceSeries.Count - 3 * _period + 1];
                double v0 = _sourceSeries[_sourceSeries.Count - 3 * _period + 2];
                double v2 = 0;

                for (int bar = _sourceSeries.Count - 3 * _period + 3; bar < _sourceSeries.Count; bar++)
                {
                    v2 = v1;
                    v1 = v0;

                    v0 = t1 * _sourceSeries[bar] + t2 * _sourceSeries[bar - 1] + t3 * _sourceSeries[bar - 2]
                       + t4 * v1 + t5 * v2;
                }

                PartialValue = v0;
            }
        }
        //---*** Private members ***---
        DataSeries _sourceSeries;
        int _period;
    }

    public class InstantaneousTrendline2Helper : IndicatorHelper
    {
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

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }
        public override Type IndicatorType
        {
            get
            {
                return typeof(InstantaneousTrendline2);
            }
        }

        public override string Description
        {
            get
            {
                return @"The low-pass Instantaneous Trendline filter from book 'Cybernetic Analysis for Stocks and Futures' (p.16) by John Ehlers.";
            }
        }
        public override string URL
        {
            get
            {
                return @"http://www2.wealth-lab.com/WL5WIKI/InstantaneousTrendLine2.ashx";
            }
        }

        private static object[] _paramDefaults = { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 300) }; 
        private static string[] _paramNames = { "Source", "Period" };
    }
}
