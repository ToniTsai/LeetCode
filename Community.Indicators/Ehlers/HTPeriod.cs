using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class HTPeriod : DataSeries
    {
        public HTPeriod(DataSeries ds, string description)
            : base(ds, description)
        {
            DataSeries InPhase = HTInPhase.Series(ds);
            DataSeries Quadrature = HTQuadrature.Series(ds);

            DataSeries Phase = new DataSeries(ds, "Phase");
            DataSeries Period = new DataSeries(ds, "Period");
            DataSeries InstPeriod = new DataSeries(ds, "InstPeriod");
            DataSeries DeltaPhase = new DataSeries(ds, "DeltaPhase");

            double Value = 0.0;
            DataSeries Result = new DataSeries(ds, "Result");

            for (int bar = 7; bar < ds.Count; bar++)
            {
                // Compute the Period of the Dominant Cycle:

                //Use ArcTan to compute the current Phase
                if (Math.Abs(InPhase[bar] + InPhase[bar - 1]) > 0)
                    Phase[bar] = Math.Atan(
                        Math.Abs((Quadrature[bar] + Quadrature[bar - 1]) / (InPhase[bar] + InPhase[bar - 1]))
                        ) * 180 / Math.PI; // Where WL4 required degrees, .NET uses radians!

                // Resolve the ArcTan ambiguity
                if ((InPhase[bar] < 0) & (Quadrature[bar] > 0))
                    Phase[bar] = 180 - Phase[bar];
                if ((InPhase[bar] < 0) & (Quadrature[bar] < 0))
                    Phase[bar] = 180 + Phase[bar];
                if ((InPhase[bar] > 0) & (Quadrature[bar] < 0))
                    Phase[bar] = 360 - Phase[bar];

                //Compute A Differential Phase
                //Resolve phase wraparound, and limit delta phase errors
                DeltaPhase[bar] = Phase[bar - 1] - Phase[bar];
                if ((Phase[bar - 1] < 90) & (Phase[bar] > 270))
                    DeltaPhase[bar] = 360 + Phase[bar - 1] - Phase[bar];
                if (DeltaPhase[bar] < 1)
                    DeltaPhase[bar] = 1;
                if (DeltaPhase[bar] > 60)
                    DeltaPhase[bar] = 60;

                //Sum DeltaPhases to reach 360 degrees
                //The sum is the instantaneous period
                InstPeriod[bar] = 0;
                Value = 0;

                for (int Count = bar; Count >= 0; Count--)
                {
                    Value += DeltaPhase[Count];
                    if ((Value > 360) & (InstPeriod[bar] == 0))
                    {
                        InstPeriod[bar] = bar - Count;
                        break;
                    }
                }

                //Resolve Instantaneous Period errors and smooth
                if (InstPeriod[bar] == 0)
                    InstPeriod[bar] = InstPeriod[bar - 1];
                Period[bar] = 0.25 * InstPeriod[bar] + 0.75 * Period[bar - 1];

                //Return the Hilbert Transform Period measured at the current bar:
                Result[bar] = Math.Truncate(Period[bar]);

                base[bar] = Result[bar];
            }
        }

        public static HTPeriod Series(DataSeries ds)
        {
            string description = string.Concat(new object[] { "HTPeriod(", ds.Description, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (HTPeriod)ds.Cache[description];
            }

            HTPeriod _HTPeriod = new HTPeriod(ds, description);
            ds.Cache[description] = _HTPeriod;
            return _HTPeriod;
        }
    }

    public class HTPeriodHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static HTPeriodHelper()
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
                return "The HTPeriod at a specific bar gives the current Hilbert Transform Period as instantaneously measured at that bar in the range of 10 to 40 and is often used to adjust other indicators to make them adaptive: for example, Stochastics and RSIs work best when using a half cycle period to peak their performance.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HTPeriod);
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
                return "HTPeriod";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/HTPeriod.ashx";
            }
        }
    }
        
}
