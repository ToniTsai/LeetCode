using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class HTDCPhase : DataSeries
    {
        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public HTDCPhase(DataSeries ds, string description)
            : base(ds, description)
        {
            DataSeries Result = new DataSeries(ds, "Result");
            DataSeries Value = HTPeriod.Series(ds);
            double RealPart = 0; double ImagPart = 0;

            for (int bar = 1; bar < ds.Count; bar++)
            {
                RealPart = 0;
                ImagPart = 0;

                for (int Count = 0; Count < Math.Truncate(Value[bar]); Count++)
                {
                    if ((bar - Count) >= 0)
                    {
                        RealPart += Math.Sin(DegreeToRadian(360 * Count / Value[bar])) * ds[bar - Count];
                        ImagPart += Math.Cos(DegreeToRadian(360 * Count / Value[bar])) * ds[bar - Count];
                    }
                }

                if (Math.Abs(ImagPart) > 0.001)
                    Result[bar] = Math.Atan(RealPart / ImagPart)
                        * 180 / Math.PI; // Where WL4 required degrees, .NET uses radians!
                if (Math.Abs(ImagPart) <= 0.001)
                    Result[bar] = 90 * Math.Sign(RealPart);

                if ((Value[bar] < 30) & (Value[bar] > 0))
                    @Result[bar] += (6.818 / Value[bar] - 0.227) * 360;

                @Result[bar] += 90;

                if (ImagPart < 0)
                    Result[bar] += 180;
                if (Result[bar] > 315)
                    Result[bar] -= 360;

                base[bar] = Result[bar];
            }
        }

        public static HTDCPhase Series(DataSeries ds)
        {
            string description = string.Concat(new object[] { "HTDCPhase(", ds.Description, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (HTDCPhase)ds.Cache[description];
            }

            HTDCPhase _HTDCPhase = new HTDCPhase(ds, description);
            ds.Cache[description] = _HTDCPhase;
            return _HTDCPhase;
        }
    }

    public class HTDCPhaseHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static HTDCPhaseHelper()
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
                return "The DC Phase at a specific bar gives the phase position from 0 to 360 degrees within the current Hilbert Transform Period instantaneously measured at that bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HTDCPhase);
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
                return "HTDCPhase";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5WIKI/HTDCPhase.ashx";
            }
        }
    }
}
