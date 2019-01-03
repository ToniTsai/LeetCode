using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class IrwinCycle : DataSeries
    {
        public IrwinCycle( Bars bars, DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period * 3;

            Highest H1 = Highest.Series( bars.High, period );
            Lowest L1 = Lowest.Series( bars.Low, period );
            DataSeries C1 = H1 - L1;
            EMA R1 = EMA.Series( (( ds - L1 ) / C1) * 100, 3, EMACalculation.Modern );
            Highest H2 = Highest.Series( R1, period );
            Lowest L2 = Lowest.Series( R1, period );
            DataSeries C2 = H2 - L2;
            DataSeries Result = EMA.Series(  ( ( R1 - L2 ) / C2 ) * 100, 3, EMACalculation.Modern );

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = Result[bar];
            }
        }

        public static IrwinCycle Series(Bars bars, DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "Irwin Cycle(", ds.Description, ",", period.ToString(), ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (IrwinCycle)bars.Cache[description];
            }

            IrwinCycle _IrwinCycle = new IrwinCycle(bars, ds, period, description);
            bars.Cache[description] = _IrwinCycle;
            return _IrwinCycle;
        }
    }

    public class IrwinCycleHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static IrwinCycleHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, CoreDataSeries.Close, new RangeBoundInt32(10, 2, 300) };
            _paramNames = new string[] { "Bars", "Data Series", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
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
                return "The Irwin Cycle indicator, created by Bill Irwin, is a normalised double-smoothed Stochastic variation.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(IrwinCycle);
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

        public override string TargetPane
        {
            get
            {
                return "IrwinCycle";
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www2.wealth-lab.com/WL5Wiki/IrwinCycle.ashx";
            }
        }
    }
}
