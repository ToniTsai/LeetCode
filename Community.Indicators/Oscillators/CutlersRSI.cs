using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class CutlersRSI : DataSeries
    {
        public CutlersRSI(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;
            if (FirstValidValue <= 1) return;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            if (ds.Count >= period)
            {
                DataSeries U = new DataSeries(ds, "cutler.U");
                DataSeries D = new DataSeries(ds, "cutler.D");
                DataSeries smaU = new DataSeries(ds, "cutler.smaU");
                DataSeries smaD = new DataSeries(ds, "cutler.smaD");
                DataSeries RS = new DataSeries(ds, "cutler.RS");

                for (int i = period; i < ds.Count; i++)
                {
                    double u = 0.0;
                    double d = 0.0;

                    if (ds[i] > ds[i - 1])
                    {
                        u = ds[i] - ds[i - 1];
                    }
                    else
                    {
                        d = ds[i - 1] - ds[i];
                    }

                    U[i] = u;
                    D[i] = d;
                }

                smaU = Community.Indicators.FastSMA.Series(U, period);
                smaD = Community.Indicators.FastSMA.Series(D, period);
                RS = smaU / smaD;

                //for (int j = period + 1; j < ds.Count; j++)
                for (int bar = base.FirstValidValue; bar < ds.Count; bar++)
                {
                    base[bar] = 100.0 - 100.0 / (1.0 + RS[bar]);
                }
            }
        }

        public static CutlersRSI Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "Cutlers RSI (", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (CutlersRSI)ds.Cache[description];
            }

            CutlersRSI _CutlersRSI = new CutlersRSI(ds, period, description);
            ds.Cache[description] = _CutlersRSI;
            return _CutlersRSI;
        }
    }

    public class CutlersRSIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CutlersRSIHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(14, 2, 300) };
            _paramNames = new string[] { "Data Series", "Cutler's RSI Period" };
        }

        public override string TargetPane
        {
            get
            {
                return "CutlersRSIPane";
            }
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Violet;
            }
        }

        public override string Description
        {
            get
            {
                return "Cutler's RSI is a variation of Wilder's RSI based on a SMA. Unlike the RSI, this makes it a \"stable\" indicator. The oscillator's interpretation is similar to RSI.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CutlersRSI);
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
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

        public override double OscillatorOversoldValue
        {
            get
            {
                return 30;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 70;
            }
        }

        public override Color OscillatorOversoldColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override Color OscillatorOverboughtColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CutlersRSI.ashx";
            }
        }
    }
}
