using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// Created by Michael Bytnar aka DartBoardTrader
    /// </summary>
    public class CorrelationXL : DataSeries
    {
        //private double AVERAGE(int start, DataSeries d, int period)
        //{
        //    return SMA.Series(d, period)[start];
        //}

        //private double CORREL(int start, DataSeries x, DataSeries y, int period)
        //{
        //    double pearson = 0;
        //    double covXY = 0;
        //    if (x.Count < period || y.Count < period)
        //    {
        //        // Too little data.
        //    }
        //    else
        //    {
        //        covXY = WealthLab.Indicators.Sum.Series((x - AVERAGE(start, x, period)) * (y - AVERAGE(start, y, period)), period)[start];
        //        covXY /= period;
        //        double stdx = StdDev.Series(x, period, StdDevCalculation.Population)[start];
        //        double stdy = StdDev.Series(y, period, StdDevCalculation.Population)[start];
        //        if (stdx * stdy != 0)
        //        {
        //            pearson = covXY / (stdx * stdy);
        //        }
        //    }
        //    return pearson;
        //}

        public CorrelationXL(DataSeries x, DataSeries y, int period, string description)
            : base(x, description)
        {
            //for (int i = period; i < x.Count; i++)
            //{
            //    base[i] = CORREL(i, x, y, period);
            //}

            Correlation c = Correlation.Series(x, y, period);
            for (int bar = period; bar < x.Count; bar++)
            {
                base[bar] = c[bar];
            }
        }

        public static double Value(int bar, DataSeries x, DataSeries y, int period)
        {
            if (x.Count < period || y.Count < period)
                return 0;

            double c = 0;
            double xmean = Community.Indicators.FastSMA.Value(bar, x, period);
            double ymean = Community.Indicators.FastSMA.Value(bar, y, period);

            double t1sq = 0;
            double t2sq = 0;
            double sxy = 0;
            for (int i = 0; i < period; i++)
            {
                double t1 = x[bar - i] - xmean;
                double t2 = y[bar - i] - ymean;
                t1sq += t1 * t1;
                t2sq += t2 * t2;
                sxy += t1 * t2;
            }

            if (t1sq == 0 || t2sq == 0)
                c = 0;
            else
                c = sxy / (Math.Sqrt(t1sq) * Math.Sqrt(t2sq));

            return c;
        }

        public static CorrelationXL Series(DataSeries x, DataSeries y, int period)
        {
            string description = string.Concat(new object[] { "Excel Correlation(", x.Description, ",", y.Description, ",", period.ToString(), ")" });
            if (x.Cache.ContainsKey(description))
            {
                return (CorrelationXL)x.Cache[description];
            }

            CorrelationXL _CorrelationXL = new CorrelationXL(x, y, period, description);
            x.Cache[description] = _CorrelationXL;
            return _CorrelationXL;
        }
    }

    public class CorrelationXLHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CorrelationXLHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, CoreDataSeries.Close, new RangeBoundInt32(20, 2, 300) };
            _paramNames = new string[] { "Data Series", "Data Series", "Lookback period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 1;
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return false;
            }
        }

        public override string Description
        {
            get
            {
                return "Correlation function that matches Excel's output.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CorrelationXL);
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
                return "Correlation";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5WIKI/CorrelationSeries.ashx";
            }
        }
    }

    /// <summary>
    /// Created by Steve Salemy aka ss161
    /// </summary>
    public class Correlation : DataSeries
    {
        public Correlation(DataSeries x, DataSeries y, int period, string description)
            : base(x, description)
        {
            if (x.Count < period || y.Count < period)
                return;

            DataSeries c = new DataSeries(x, x.Description + ":" + y.Description);
            DataSeries xmean = Community.Indicators.FastSMA.Series(x, period);
            DataSeries ymean = Community.Indicators.FastSMA.Series(y, period);

            DataSeries xv = new DataSeries(x, "xv");
            DataSeries yv = new DataSeries(y, "yv");
            DataSeries s = new DataSeries(x, "s");

            for (int bar = period - 1; bar < x.Count; bar++)
            {
                double t1sq = 0;
                double t2sq = 0;
                double sxy = 0;
                for (int i = 0; i < period; i++)
                {
                    double t1 = x[bar - i] - xmean[bar];
                    double t2 = y[bar - i] - ymean[bar];
                    t1sq += t1 * t1;
                    t2sq += t2 * t2;
                    sxy += t1 * t2;
                }
                xv[bar] = t1sq;
                yv[bar] = t2sq;
                s[bar] = sxy;

                if (xv[bar] == 0 | yv[bar] == 0)
                    c[bar] = 0;
                else
                    c[bar] = s[bar] / (Math.Sqrt(xv[bar]) * Math.Sqrt(yv[bar]));

                base[bar] = c[bar];

            }
        }

        public static double Value(int bar, DataSeries x, DataSeries y, int period)
        {
            if (x.Count < period || y.Count < period)
                return 0;

            double c = 0;
            double xmean = Community.Indicators.FastSMA.Value(bar, x, period);
            double ymean = Community.Indicators.FastSMA.Value(bar, y, period);

            double t1sq = 0;
            double t2sq = 0;
            double sxy = 0;
            for (int i = 0; i < period; i++)
            {
                double t1 = x[bar - i] - xmean;
                double t2 = y[bar - i] - ymean;
                t1sq += t1 * t1;
                t2sq += t2 * t2;
                sxy += t1 * t2;
            }

            if (t1sq == 0 || t2sq == 0)
                c = 0;
            else
                c = sxy / (Math.Sqrt(t1sq) * Math.Sqrt(t2sq));

            return c;
        }

        public static Correlation Series(DataSeries x, DataSeries y, int period)
        {
            string description = string.Concat(new object[] { "Correlation(", x.Description, ",", y.Description, ",", period.ToString(), ")" });
            if (x.Cache.ContainsKey(description))
            {
                return (Correlation)x.Cache[description];
            }

            Correlation _Correlation = new Correlation(x, y, period, description);
            x.Cache[description] = _Correlation;
            return _Correlation;
        }
    }

    public class CorrelationHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CorrelationHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, CoreDataSeries.Close, new RangeBoundInt32(20, 2, 300) };
            _paramNames = new string[] { "Data Series", "Data Series", "Lookback period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 1;
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return false;
            }
        }

        public override string Description
        {
            get
            {
                return "Pearson Correlation function created by Steve Salemy that uses a Community.Components function.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Correlation);
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
                return "Correlation";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5WIKI/CorrelationSeries.ashx";
            }
        }
    }

    /// <summary>
    /// Created by avishn
    /// </summary>
    public class KendallTauRankCorrelation : DataSeries
    {
        DataSeries ds1, ds2, zscore;
        int period;

        public KendallTauRankCorrelation(DataSeries ds1, DataSeries ds2, int period, string description)
            : base(ds1, description)
        {
            this.ds1 = ds1;
            this.ds2 = ds2;
            this.period = period;

            this.FirstValidValue = period;


            if (ds1.Count < period || ds2.Count < period)
                return;

            double[] r1 = new double[period];
            double[] r2 = new double[period];

            for (int bar = FirstValidValue; bar < ds1.Count; bar++)
            {

                for (int i = 0; i < period; i++)
                {
                    r1[i] = Rank(ref ds1, bar - period + 1, bar, ds1[bar - period + i + 1]);
                    r2[i] = Rank(ref ds2, bar - period + 1, bar, ds2[bar - period + i + 1]);
                }

                int conc_disc = 0;
                for (int i = 0; i < period; i++)
                {
                    for (int j = 0; j < (i - 1); j++)
                    {
                        conc_disc += Math.Sign(r1[i] - r1[j]) * Math.Sign(r2[i] - r2[j]);
                    }
                }

                base[bar] = conc_disc / (0.5 * period * (period - 1));
            }
        }

        double Rank(ref DataSeries ds, int start, int stop, double v)
        {
            int countLessThan = 0;
            for (int i = start; i <= stop; i++) if (v > ds[i]) countLessThan++;
            return (double)countLessThan / (stop - start);
        }

        public static KendallTauRankCorrelation Series(DataSeries ds1, DataSeries ds2, int period)
        {
            string description = string.Concat(new object[] { "KendallTauRankCorrelation(", ds1.Description, ",", ds2.Description, ",", period, ")" });
            if (ds1.Cache.ContainsKey(description))
            {
                return (KendallTauRankCorrelation)ds1.Cache[description];
            }
            KendallTauRankCorrelation _s = new KendallTauRankCorrelation(ds1, ds2, period, description);
            ds1.Cache[description] = _s;
            return _s;
        }

        public DataSeries ZScore()
        {
            if (zscore == null)
            {
                zscore = new DataSeries(this, "ZScore(" + this.Description + ")");
                for (int bar = this.FirstValidValue; bar < this.Count; bar++)
                {
                    zscore[bar] = this[bar] / Math.Sqrt(2d * (2d * this.period + 5d) / (9d * this.period * (this.period - 1)));
                }
            }
            return zscore;
        }
    }

    public class KendallTauRankCorrelationHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static KendallTauRankCorrelationHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, CoreDataSeries.Close, new RangeBoundInt32(2, 2, 300) };
            _paramNames = new string[] { "DataSeries 1", "DataSeries 2", "Period" };
        }

        public override Color DefaultColor { get { return Color.Black; } }
        public override int DefaultWidth { get { return 1; } }
        public override string Description { get { return "Kendall Tau Rank Correlation"; } }
        public override Type IndicatorType { get { return typeof(KendallTauRankCorrelation); } }
        public override IList<object> ParameterDefaultValues { get { return _paramDefaults; } }
        public override IList<string> ParameterDescriptions { get { return _paramNames; } }
        public override string TargetPane { get { return "KendallTauRankCorrelation"; } }
        public override string URL { get { return "http://en.wikipedia.org/wiki/Kendall_tau_rank_correlation_coefficient"; } }

    }
}