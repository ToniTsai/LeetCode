using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// Highest2 - the highest of two series
    /// </summary>
    public class Highest2 : DataSeries
    {
        public Highest2(DataSeries ds1, DataSeries ds2, int period)
            : base(ds2, "Highest Of Two")
        {
            DataSeries tmp = ds1 - ds1;
            for (int bar = tmp.FirstValidValue; bar < ds2.Count; bar++)
            {
                tmp[bar] = Math.Max(ds1[bar], ds2[bar]);
            }

            for (int bar = tmp.FirstValidValue; bar < ds2.Count; bar++)
            {
                this[bar] = Highest.Series(tmp, period)[bar];
            }
        }

        public static Highest2 Series(DataSeries ds1, DataSeries ds2, int period)
        {
            /*Highest2 _Highest2 = new Highest2( ds1, ds2, period );
            return _Highest2;*/

            string description = string.Concat(new object[] { "_Highest2(", ds1.Description, ",", ds2.Description, ",", period, ")" });
            if (ds1.Cache.ContainsKey(description))
            {
                return (Highest2)ds1.Cache[description];
            }

            Highest2 _Highest2 = new Highest2(ds1, ds2, period);
            ds1.Cache[description] = _Highest2;
            return _Highest2;

        }
    }

    /// <summary>
    /// Lowest2 - the lowest of two series
    /// </summary>
    public class Lowest2 : DataSeries
    {
        public Lowest2(DataSeries ds1, DataSeries ds2, int period)
            : base(ds2, "Lowest Of Two")
        {
            DataSeries tmp = ds1 - ds1;
            for (int bar = tmp.FirstValidValue; bar < ds2.Count; bar++)
            {
                tmp[bar] = Math.Min(ds1[bar], ds2[bar]);
            }

            for (int bar = tmp.FirstValidValue; bar < ds2.Count; bar++)
            {
                this[bar] = Lowest.Series(tmp, period)[bar];
            }
        }

        public static Lowest2 Series(DataSeries ds1, DataSeries ds2, int period)
        {
            /*Lowest2 _Lowest2 = new Lowest2( ds1, ds2, period );
            return _Lowest2;*/

            string description = string.Concat(new object[] { "Lowest2(", ds1.Description, ",", ds2.Description, ",", period, ")" });
            if (ds1.Cache.ContainsKey(description))
            {
                return (Lowest2)ds1.Cache[description];
            }

            Lowest2 _Lowest2 = new Lowest2(ds1, ds2, period);
            ds1.Cache[description] = _Lowest2;
            return _Lowest2;
        }
    }
}
