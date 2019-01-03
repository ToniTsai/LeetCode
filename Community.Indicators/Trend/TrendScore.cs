using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class TrendScore : DataSeries
    {
        public TrendScore(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 20;

            DataSeries ScoreSeries = new DataSeries(bars, "TrendScore");
            for (int j = 11; j <= 20; j++)
            {
                Momentum Mj = Momentum.Series(bars.Close, j);
                DataSeries Sj = Mj / DataSeries.Abs(Mj);
                ScoreSeries += Sj;
            }

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = ScoreSeries[bar];
            }
        }

        public static TrendScore Series(Bars bars)
        {
            string description = string.Concat(new object[] { "TrendScore()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (TrendScore)bars.Cache[description];
            }

            TrendScore _TrendScore = new TrendScore(bars, description);
            bars.Cache[description] = _TrendScore;
            return _TrendScore;
        }
    }

    public class TrendScoreHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TrendScoreHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
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
                return "TrendScore by Tushar Chande is an indicator of both the trend strength and direction.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TrendScore);
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
                return "TrendScore";
            }
        }
    }
}