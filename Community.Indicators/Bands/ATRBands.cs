using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Community.Indicators
{
    public class ATRBandUpper : DataSeries
    {
        public ATRBandUpper(Bars bars, DataSeries ds, int atrPeriod, double atrMult, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(ds.FirstValidValue, atrPeriod * 3);

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < atrPeriod)
                return;

            var rangePartitioner = Partitioner.Create(FirstValidValue, ds.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    base[bar] = ds[bar] + atrMult * ATR.Series(bars, atrPeriod)[bar];
                }
            });
        }

        public static ATRBandUpper Series(Bars bars, DataSeries ds, int atrPeriod, double atrMult)
        {
            string description = string.Concat(new object[] { "ATR Upper Band(", ds.Description, ",", atrPeriod, ",", atrMult, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (ATRBandUpper)ds.Cache[description];
            }

            ATRBandUpper _ATRBandUpper = new ATRBandUpper(bars, ds, atrPeriod, atrMult, description);
            ds.Cache[description] = _ATRBandUpper;
            return _ATRBandUpper;
        }
    }

    public class ATRBandUpperHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ATRBandUpperHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, CoreDataSeries.Close, new RangeBoundInt32(20, 5, 300), new RangeBoundDouble(2.0, 0.5, 10) };
            _paramNames = new string[] { "Bars", "Data Series", "ATR Period", "ATR Multiple" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override Color DefaultBandColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override string Description
        {
            get
            {
                return "The ATR Bands indicator creates an envelope of a selected ATR multiple around a user-defined data series (e.g. a simple moving average).";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ATRBandUpper);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(ATRBandLower);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ATRBandUpper.ashx";
            }
        }
    }

    public class ATRBandLower : DataSeries
    {
        public ATRBandLower(Bars bars, DataSeries ds, int atrPeriod, double atrMult, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(ds.FirstValidValue, atrPeriod * 3);

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < atrPeriod)
                return;

            var rangePartitioner = Partitioner.Create(FirstValidValue, ds.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    base[bar] = ds[bar] - atrMult * ATR.Series(bars, atrPeriod)[bar];
                }
            });
        }

        public static ATRBandLower Series(Bars bars, DataSeries ds, int atrPeriod, double atrMult)
        {
            string description = string.Concat(new object[] { "ATR Lower Band(", ds.Description, ",", atrPeriod, ",", atrMult, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (ATRBandLower)ds.Cache[description];
            }

            ATRBandLower _ATRBandLower = new ATRBandLower(bars, ds, atrPeriod, atrMult, description);
            bars.Cache[description] = _ATRBandLower;
            return _ATRBandLower;
        }
    }

    public class ATRBandLowerHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ATRBandLowerHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, CoreDataSeries.Close, new RangeBoundInt32(20, 5, 300),
                new RangeBoundDouble( 2.0, 0.5, 10 ) };
            _paramNames = new string[] { "Bars", "Data Series", "ATR Period", "ATR Multiple" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override Color DefaultBandColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override string Description
        {
            get
            {
                return "The ATR Bands indicator creates an envelope of a selected ATR multiple around a user-defined data series (e.g. a simple moving average).";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ATRBandLower);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(ATRBandUpper);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ATRBandLower.ashx";
            }
        }
    }
}
