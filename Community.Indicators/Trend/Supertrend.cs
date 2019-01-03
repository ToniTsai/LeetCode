using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class CMCSuperTrend : DataSeries
    {
        public CMCSuperTrend(Bars bars, double ATRMultiple, int ATRPeriod, string description)
            : base(bars, description)
        {
            base.FirstValidValue = ATRPeriod * 3;
            int state = 1;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                double Value = 0;

                if (state == 1)
                {
                    if (bars.Close[bar] < base[bar - 1])
                    {
                        state = -1;
                        Value = bars.High[bar] + ATRMultiple * ATR.Series(bars, ATRPeriod)[bar];
                    }
                    else
                        Value = Math.Max(this[bar - 1], bars.Low[bar] - ATRMultiple * ATR.Series(bars, ATRPeriod)[bar]);
                }
                else
                {
                    if (bars.Close[bar] > base[bar - 1])
                    {
                        state = 1;
                        Value = bars.Low[bar] - ATRMultiple * ATR.Series(bars, ATRPeriod)[bar];
                    }
                    else
                        Value = Math.Min(this[bar - 1], bars.High[bar] + ATRMultiple * ATR.Series(bars, ATRPeriod)[bar]);
                }

                base[bar] = Value;
            }
        }

        public static CMCSuperTrend Series(Bars bars, double ATRMultiple, int ATRPeriod)
        {
            string description = string.Concat(new object[] { "CMCSuperTrend(", ATRMultiple, ",", ATRPeriod, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (CMCSuperTrend)bars.Cache[description];
            }

            CMCSuperTrend _CMCSuperTrend = new CMCSuperTrend(bars, ATRMultiple, ATRPeriod, description);
            bars.Cache[description] = _CMCSuperTrend;
            return _CMCSuperTrend;
        }
    }

    public class CMCSuperTrendHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CMCSuperTrendHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundDouble(3, 0.1, 20), new RangeBoundInt32(10, 2, 300) };
            _paramNames = new string[] { "Bars", "ATR Multiple", "ATR Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
            }
        }

        public override string Description
        {
            get
            {
                return "CMCSuperTrend";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CMCSuperTrend);
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
                return "http://www.wealth-lab.com/Community/Forum/Posts.aspx?id=ySrfUxc3S1xfVgORzjWL4e82ABBsz8E/bqZgxykmGqmbDHXxGkzW/90PNIqmKjaSrszsZdo3da7OKYELTZciCCcpPCZni9QCVwkVqbf4lJg=#155843";
            }
        }
    }

    public class MTSuperTrendSeries : DataSeries
    {
        public MTSuperTrendSeries(Bars bars, int CCIPeriod, double ATRMultiple, int ATRPeriod, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(CCIPeriod, ATRPeriod * 3);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                double Value = 0;

                if (CCI.Series(bars, CCIPeriod)[bar] >= 0)
                {
                    Value = Math.Max(base[bar - 1], bars.Low[bar] - ATRMultiple * ATR.Series(bars, ATRPeriod)[bar]);
                }
                else
                {
                    Value = Math.Min(base[bar - 1], bars.High[bar] + ATRMultiple * ATR.Series(bars, ATRPeriod)[bar]);
                }

                base[bar] = Value;
            }
        }

        public static MTSuperTrendSeries Series(Bars bars, int CCIPeriod, double ATRMultiple, int ATRPeriod)
        {
            string description = string.Concat(new object[] { "MTSuperTrendSeries(", CCIPeriod, ",", ATRMultiple, ",", ATRPeriod, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (MTSuperTrendSeries)bars.Cache[description];
            }

            MTSuperTrendSeries _MTSuperTrendSeries = new MTSuperTrendSeries(bars, CCIPeriod, ATRMultiple, ATRPeriod, description);
            bars.Cache[description] = _MTSuperTrendSeries;
            return _MTSuperTrendSeries;
        }
    }

    public class MTSuperTrendSeriesHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MTSuperTrendSeriesHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars,  new RangeBoundInt32(50, 2, 300), new RangeBoundDouble(1, 0.1, 20), 
                new RangeBoundInt32(5, 2, 300) };
            _paramNames = new string[] { "Bars", "CCI Period", "ATR Multiple", "ATR Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
            }
        }

        public override string Description
        {
            get
            {
                return "MTSuperTrendSeries";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MTSuperTrendSeries);
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
                return "http://www.wealth-lab.com/Community/Forum/Posts.aspx?id=ySrfUxc3S1xfVgORzjWL4e82ABBsz8E/bqZgxykmGqmbDHXxGkzW/90PNIqmKjaSrszsZdo3da7OKYELTZciCCcpPCZni9QCVwkVqbf4lJg=#155843";
            }
        }
    }
}