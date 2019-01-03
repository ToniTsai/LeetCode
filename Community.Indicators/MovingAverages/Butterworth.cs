using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Butterworth2 : DataSeries
    {
        /// <summary>
        /// Where WL4 required degrees, .NET uses radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public double Deg2Rad(double degrees)
        {
            return (Math.PI * degrees) / 180;
        }

        public Butterworth2(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            double x, x1, x2; 	// Input values
            double y, y1, y2; 	// Output series
            double a = Math.Exp(-1.414 * Math.PI / period);
            double b = 2 * a * Math.Cos(Deg2Rad(1.414 * 180 / period));
            double c = Math.Pow(a, 2);

            y1 = ds[0];
            y2 = y1;

            for (int bar = 2; bar < ds.Count; bar++)
            {
                x = ds[bar];
                x1 = ds[bar - 1];
                x2 = ds[bar - 2];

                y = b * y1 - c * y2 + ((1 - b + c) / 4) * (x + 2 * x1 + x2);
                base[bar] = y;
                y2 = y1; // delayed by two bars
                y1 = y;  // delayed by one bar
            }
        }

        public static Butterworth2 Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "Butterworth2(", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (Butterworth2)ds.Cache[description];
            }

            Butterworth2 _Butterworth2 = new Butterworth2(ds, period, description);
            ds.Cache[description] = _Butterworth2;
            return _Butterworth2;
        }
    }

    public class Butterworth2Helper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static Butterworth2Helper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(14, 2, 300) };
            _paramNames = new string[] { "Source", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override string Description
        {
            get
            {
                return "Butterworth2 implements a two pole Butterworth filter that can be used for smoothing.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Butterworth2);
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
                return "http://www2.wealth-lab.com/WL5Wiki/Butterworth.ashx";
            }
        }
    }

    public class Butterworth3 : DataSeries
    {
        /// <summary>
        /// Where WL4 required degrees, .NET uses radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public double Deg2Rad(double degrees)
        {
            return (Math.PI * degrees) / 180;
        }

        public Butterworth3(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            double x, x1, x2, x3;
            double y, y1, y2, y3;
            double a = Math.Exp(-Math.PI / period);
            double b = 2 * a * Math.Cos(Deg2Rad(1.738 * 180 / period));
            double c = Math.Pow(a, 2);

            y1 = ds[0];
            y2 = y1;
            y3 = y2;

            for (int bar = 3; bar < ds.Count; bar++)
            {
                x = ds[bar];
                x1 = ds[bar - 1];
                x2 = ds[bar - 2];
                x3 = ds[bar - 3];

                y = (b + c) * y1 - (c + b * c) * y2 + c * c * y3 + ((1 - b + c) * (1 - c) / 8)
                    * (x + 3 * x1 + 3 * x2 + x3);
                base[bar] = y;

                y3 = y2; // delayed by three bars
                y2 = y1; // delayed by two bars
                y1 = y;  // delayed by one bar
            }
        }

        public static Butterworth3 Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "Butterworth3(", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (Butterworth3)ds.Cache[description];
            }

            Butterworth3 _Butterworth3 = new Butterworth3(ds, period, description);
            ds.Cache[description] = _Butterworth3;
            return _Butterworth3;
        }
    }

    public class Butterworth3Helper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static Butterworth3Helper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(14, 2, 300) };
            _paramNames = new string[] { "Source", "Period" };
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
                return "Butterworth3 implements a three pole Butterworth filter that can be used for smoothing.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Butterworth3);
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
                return "http://www2.wealth-lab.com/WL5Wiki/Butterworth.ashx";
            }
        }
    }
}
