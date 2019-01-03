using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class PositiveCloses : DataSeries
    {
        public PositiveCloses(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            DataSeries pc = new DataSeries(ds, "PositiveCloses(" + period + ")");
            pc = CumUp.Series(ds, 1);
            pc /= pc;
            pc = Sum.Series(pc, period);

            for (int bar = period; bar < ds.Count; bar++)
            {
                base[bar] = pc[bar];
            }
        }

        public static PositiveCloses Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "Positive Closes(", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (PositiveCloses)ds.Cache[description];
            }

            PositiveCloses _PositiveCloses = new PositiveCloses(ds, period, description);
            ds.Cache[description] = _PositiveCloses;
            return _PositiveCloses;
        }
    }

    public class PositiveClosesHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PositiveClosesHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(21, 2, 300) };
            _paramNames = new string[] { "Data Series", "Period" };
        }

        public override Color DefaultColor
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
                return "The Positive Closes indicator returns the number of positive closes over the specified lookback period.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PositiveCloses);
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
                return "NegPosCloses";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/PositiveCloses.ashx";
            }
        }
    }

    public class NegativeCloses : DataSeries
    {
        public NegativeCloses(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            DataSeries nc = new DataSeries(ds, "NegativeCloses(" + period + ")");
            nc = CumDown.Series(ds, 1);
            nc /= nc;
            nc = Sum.Series(nc, period);

            for (int bar = period; bar < ds.Count; bar++)
            {
                base[bar] = nc[bar];
            }
        }

        public static NegativeCloses Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "Negative Closes(", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (NegativeCloses)ds.Cache[description];
            }

            NegativeCloses _NegativeCloses = new NegativeCloses(ds, period, description);
            ds.Cache[description] = _NegativeCloses;
            return _NegativeCloses;
        }
    }

    public class NegativeClosesHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static NegativeClosesHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(21, 2, 300) };
            _paramNames = new string[] { "Data Series", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override string Description
        {
            get
            {
                return "The Negative Closes indicator returns the number of negative closes over the specified lookback period.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(NegativeCloses);
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
                return "NegPosCloses";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/NegativeCloses.ashx";
            }
        }
    }
}
