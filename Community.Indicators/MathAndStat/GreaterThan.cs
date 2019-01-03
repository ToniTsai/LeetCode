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
    /// <summary>
    /// Courtesy DartBoardTrader
    /// </summary> 
    public class GreaterThan : DataSeries
    {
        public GreaterThan(DataSeries ds, DataSeries comp, double greaterThanWeight, double lessThanWeight, string description)
            : base(ds, description)
        {
            base.FirstValidValue = 1;

            var rangePartitioner = Partitioner.Create(0, ds.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    base[i] = ds[i] > comp[i] ? greaterThanWeight : lessThanWeight;
                }
            });
        }

        public static GreaterThan Series(DataSeries ds, DataSeries comp, double greaterThanWeight, double lessThanWeight)
        {
            string description = string.Concat(new object[] { "GreaterThan(", ds.Description, ",", comp.Description, "," , 
                greaterThanWeight, "," , lessThanWeight, "," , ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (GreaterThan)ds.Cache[description];
            }

            GreaterThan _GreaterThan = new GreaterThan(ds, comp, greaterThanWeight, lessThanWeight, description);
            ds.Cache[description] = _GreaterThan;
            return _GreaterThan;
        }
    }
}