using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class BBandUpper2 : DataSeries
    {
        public BBandUpper2(DataSeries ds, int period, double stdDevs, StdDevCalculation sd, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            StdDev sd_ = StdDev.Series(ds, period, sd);
            DataSeries sma = Community.Indicators.FastSMA.Series(ds, period);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = sma[bar] + (sd_[bar] * stdDevs);
            }
        }

        public static BBandUpper2 Series(DataSeries ds, int period, double stdDevs, StdDevCalculation sd)
        {
            string description = string.Concat(new object[] { "Upper Bollinger Band(", ds.Description, ",", period, ",", stdDevs, ",", sd, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (BBandUpper2)ds.Cache[description];
            }

            BBandUpper2 _BBandUpper2 = new BBandUpper2(ds, period, stdDevs, sd, description);
            ds.Cache[description] = _BBandUpper2;
            return _BBandUpper2;
        }
    }

    public class BBandUpper2Helper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static BBandUpper2Helper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 5, 300), 
                new RangeBoundDouble(2.0, 0.5, 10), StdDevCalculation.Sample };
            _paramNames = new string[] { "Data Series", "Period", "Deviations", "StdDev calculation" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
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
                return "Bollinger Bands are price envelopes based on a number of Standard Deviations above and below a moving average of the underlying data series." +
                    "This version allows to select a Standard Deviation calculation: Population or Sample.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(BBandUpper2);
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
                return typeof(BBandLower2);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/BBandUpper2.ashx";
            }
        }
    }

    public class BBandLower2 : DataSeries
    {
        public BBandLower2(DataSeries ds, int period, double stdDevs, StdDevCalculation sd, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            StdDev sd_ = StdDev.Series(ds, period, sd);
            DataSeries sma = Community.Indicators.FastSMA.Series(ds, period);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = sma[bar] - (sd_[bar] * stdDevs);
            }
        }

        public static BBandLower2 Series(DataSeries ds, int period, double stdDevs, StdDevCalculation sd)
        {
            string description = string.Concat(new object[] { "Lower Bollinger Band(", ds.Description, ",", period, ",", stdDevs, ",", sd, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (BBandLower2)ds.Cache[description];
            }

            BBandLower2 _BBandLower2 = new BBandLower2(ds, period, stdDevs, sd, description);
            ds.Cache[description] = _BBandLower2;
            return _BBandLower2;
        }
    }

    public class BBandLower2Helper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static BBandLower2Helper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 5, 300), 
                new RangeBoundDouble(2.0, 0.5, 10), StdDevCalculation.Sample };
            _paramNames = new string[] { "Data Series", "Period", "Deviations", "StdDev calculation" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
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
                return "Bollinger Bands are price envelopes based on a number of Standard Deviations above and below a moving average of the underlying data series." +
                    "This version allows to select a Standard Deviation calculation: Population or Sample.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(BBandLower2);
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
                return typeof(BBandUpper2);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/BBandLower2.ashx";
            }
        }
    }

    public class BBWidth : DataSeries
    {
        public BBWidth(DataSeries ds, int period, double SD, bool percent, string description)
            : base(ds, description)
        {
            DataSeries BBUp = BBandUpper2.Series(ds, period, SD, StdDevCalculation.Sample);
            DataSeries BBDown = BBandLower2.Series(ds, period, SD, StdDevCalculation.Sample);
            Community.Indicators.FastSMA sma = Community.Indicators.FastSMA.Series(ds, period);
            DataSeries bbWidth = (BBUp - BBDown) / sma;     //BandWidth = (upperBB − lowerBB) / middleBB

            base.FirstValidValue = ds.FirstValidValue;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                if (!percent)
                    base[bar] = bbWidth[bar];
                else
                    base[bar] = bbWidth[bar] * 100;
            }
        }

        public static BBWidth Series(DataSeries ds, int period, double SD, bool percent)
        {
            string description = string.Concat(new object[] { "Bollinger Band Width(", ds.Description, ",", period, ",", SD, ",", percent, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (BBWidth)ds.Cache[description];
            }

            BBWidth _BBWidth = new BBWidth(ds, period, SD, percent, description);
            ds.Cache[description] = _BBWidth;
            return _BBWidth;
        }
    }

    public class BBWidthHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static BBWidthHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 1, 300), new RangeBoundDouble(2.0, 0.1, 10), false };
            _paramNames = new string[] { "DataSeries", "Period", "Standard Deviations", "As percentage?" };
        }

        public override string Description
        {
            get
            {
                return "Bollinger BandWidth is an indicator derived from Bollinger Bands. Non-normalized BandWidth measures the distance between the upper band and the lower band.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(BBWidth);
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

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override string URL
        {
            get
            {
                return "http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:bollinger_band_width";
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 2;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "BBWidth";
            }
        }
    }

    public class BBPctB : DataSeries
    {
        public BBPctB(DataSeries ds, int period, double SD, string description)
            : base(ds, description)
        {
            DataSeries BBUp = BBandUpper2.Series(ds, period, SD, StdDevCalculation.Sample);
            DataSeries BBDown = BBandLower2.Series(ds, period, SD, StdDevCalculation.Sample);
            DataSeries PctBBW = ((ds - BBDown) / (BBUp - BBDown));  //%B = (Price - Lower Band)/(Upper Band - Lower Band)

            base.FirstValidValue = ds.FirstValidValue;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                double v = PctBBW[bar];
                base[bar] = (double.IsInfinity(v) || double.IsNaN(v)) ? 0 : v;
                //base[bar] = ((ds[bar] - BBDown[bar]) / (BBUp[bar] - BBDown[bar]));
            }
        }

        public static BBPctB Series(DataSeries ds, int period, double SD)
        {
            string description = string.Concat(new object[] { "Bollinger Band %b(", ds.Description, ",", period, ",", SD, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (BBPctB)ds.Cache[description];
            }

            BBPctB _BBPctB = new BBPctB(ds, period, SD, description);
            ds.Cache[description] = _BBPctB;
            return _BBPctB;
        }
    }

    public class BBPctBHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static BBPctBHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 1, 300), new RangeBoundDouble(2.0, 0.1, 10) };
            _paramNames = new string[] { "DataSeries", "Period", "Standard Deviations" };
        }

        public override string Description
        {
            get
            {
                return "%B is an indicator derived from Bollinger Bands that quantifies a security's price relative to its upper and lower Bollinger Bands.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(BBPctB);
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

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override string URL
        {
            get
            {
                return "http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:bollinger_band_perce";
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 2;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "BBPctB";
            }
        }
    }
}
