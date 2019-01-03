using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class FastSMA : DataSeries
    {
        public FastSMA(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period - 1 + ds.FirstValidValue;
            if (period < 1 || period > ds.Count + 1)
            {
                period = ds.Count + 1;
            }

            double sum = 0;
            int max_count = ds.Count;
            if (period > max_count)
            {
                for (int bar = 0; bar < max_count; bar++)
                {
                    double cur_ds = ds[bar];
                    sum += cur_ds;
                    base[bar] = sum / (bar + 1);
                }
            }
            else
            {
                double[] ds_cache = new double[period];
                int cache_index = 0;
                int prev_cache_index = 0;
                int max_index = period - 1;
                double cur_ds = 0;
                for (int bar = 0; bar < period; bar++)
                {
                    cur_ds = ds[bar];
                    sum += cur_ds;
                    base[bar] = sum / (bar + 1);
                    ds_cache[cache_index] = cur_ds;
                    cache_index++;
                }

                double period_mul = 1d / period;
                for (int bar = period; bar < max_count; bar++)
                {
                    cur_ds = ds[bar];

                    sum += cur_ds;
                    sum -= ds_cache[prev_cache_index];
                    base[bar] = sum * period_mul;/// period;// 

                    cache_index++;
                    if (cache_index > max_index) cache_index = 0;

                    prev_cache_index++;
                    if (prev_cache_index > max_index) prev_cache_index = 0;
                    ds_cache[cache_index] = cur_ds;
                }
            }
        }

        public static double Value(int bar, DataSeries ds, int period)
        {
            double result;
            if (ds.Count < period)
            {
                result = 0.0;
            }
            else
            {
                double num = 0.0;
                for (int i = bar; i > bar - period; i--)
                {
                    num += ds[i];
                }
                result = num / (double)period;
            }
            return result;
        }

        public static FastSMA Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "Community.Indicators.FastSMA(", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (Community.Indicators.FastSMA)ds.Cache[description];
            }

            Community.Indicators.FastSMA _SMA = new Community.Indicators.FastSMA(ds, period, description);
            ds.Cache[description] = _SMA;
            return _SMA;
        }
    }

    public class FastSMAHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static FastSMAHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 300) };
            _paramNames = new string[] { "Data Series", "MA Period" };
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
                return "This the same SMA indicator but using an improved algorithm working faster by approximately 30%. Courtesy akuzn (Alexey Kuznetsov).";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(FastSMA);
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
                return @"http://www2.wealth-lab.com/WL5Wiki/SMA.ashx";
            }
        }
    }
}