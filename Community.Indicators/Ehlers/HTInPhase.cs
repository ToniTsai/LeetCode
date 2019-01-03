using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class HTInPhase : DataSeries
    {
        public HTInPhase(DataSeries ds, string description)
            : base(ds, description)
        {
            DataSeries Value1 = new DataSeries(ds, "Value1");
            DataSeries InPhase = new DataSeries(ds, "InPhase");
            double Value2 = 0.0;

            for (int bar = 6; bar < ds.Count; bar++)
            {
                Value1[bar] = ds[bar] - ds[bar - 6];
                Value2 = Value1[bar - 3];
                InPhase[bar] = 0.33 * Value2 + 0.67 * InPhase[bar - 1];
                base[bar] = InPhase[bar];
            }
        }

        public static HTInPhase Series(DataSeries ds)
        {
            string description = string.Concat(new object[] { "HTInPhase(", ds.Description, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (HTInPhase)ds.Cache[description];
            }

            HTInPhase _HTInPhase = new HTInPhase(ds, description);
            ds.Cache[description] = _HTInPhase;
            return _HTInPhase;
        }
    }

    public class HTInPhaseHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static HTInPhaseHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close };
            _paramNames = new string[] { "Data Series" };
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
                return 1;
            }
        }

        public override string Description
        {
            get
            {
                return "The InPhase component is used in conjunction with the Quadrature component to generate the phase of the analytic signal at a specific bar or for the entire data series.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HTInPhase);
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
                return "HTInPhase";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/HTInPhase.ashx";
            }
        }
    }
}
