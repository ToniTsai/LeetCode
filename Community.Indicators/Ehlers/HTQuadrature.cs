using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class HTQuadrature : DataSeries
    {
        public HTQuadrature(DataSeries ds, string description)
            : base(ds, description)
        {
            DataSeries Value1 = new DataSeries(ds, "Value1");
            DataSeries Quadrature = new DataSeries(ds, "Quadrature");
            double Value3 = 0.0;

            for (int bar = 6; bar < ds.Count; bar++)
            {
                Value1[bar] = ds[bar] - ds[bar - 6];
                Value3 = 0.75 * (Value1[bar] - Value1[bar - 6]) + 0.25 * (Value1[bar - 2] - Value1[bar - 4]);
                Quadrature[bar] = 0.2 * Value3 + 0.8 * Quadrature[bar - 1];
                base[bar] = Quadrature[bar];
            }
        }

        public static HTQuadrature Series(DataSeries ds)
        {
            string description = string.Concat(new object[] { "HTQuadrature(", ds.Description, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (HTQuadrature)ds.Cache[description];
            }

            HTQuadrature _HTQuadrature = new HTQuadrature(ds, description);
            ds.Cache[description] = _HTQuadrature;
            return _HTQuadrature;
        }
    }

    public class HTQuadratureHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static HTQuadratureHelper()
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
                return "The Quadrature component is used in conjunction with the InPhase component to generate the phase of the analytic signal at a specific bar or for the entire data series.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HTQuadrature);
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
                return "http://www2.wealth-lab.com/WL5Wiki/HTQuadrature.ashx";
            }
        }
    }
}