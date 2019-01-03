using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Windows.Forms;

namespace Community.Indicators
{
    public class ElderSafeStopLong : DataSeries
    {
        public ElderSafeStopLong(Bars bars, int period, int lookback, double coefficient, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(period * 3, lookback); // unstable: uses EMA
            DataSeries iEMA = EMA.Series(bars.Close, period, EMACalculation.Modern);
            DataSeries iDwnPen = new DataSeries(bars, "iDwnPen");
            DataSeries iIsDwn = new DataSeries(bars, "iIsDwn");
            int iDwnSum = 0; double fDwnSum = 0; double fDwnAvg = 0; double fDwnStop = 0;
            DataSeries Result = new DataSeries(bars, "Result");

            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < period || bars.Count < lookback)
                return;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                //Calc Dwn Side Penetration ---
                if (bars.Low[bar] < bars.Low[bar - 1])
                {
                    iIsDwn[bar] = 1;
                    iDwnPen[bar] = bars.Low[bar] - bars.Low[bar - 1];
                }
                else
                {
                    iIsDwn[bar] = 0;
                    iDwnPen[bar] = 0;
                }

                //Reset our Sumations ---
                iDwnSum = 0;
                fDwnSum = 0;

                //Summarize the Penetraions in our LookBack Period ---
                for (int x = 1; x <= lookback; x++)
                {
                    iDwnSum += (int)Math.Truncate(iIsDwn[bar - x]);
                    fDwnSum += iDwnPen[bar - x];
                }

                //Calc the Avg Penetration ---
                if (iDwnSum > 0)
                    fDwnAvg = fDwnSum / iDwnSum;
                else
                    fDwnAvg = 0;

                //Calc Todays Stops ---
                fDwnStop = bars.Low[bar - 1] + (coefficient * fDwnAvg);

                //Trend is Down --- Protect our Longs ---
                if (iEMA[bar - 1] > iEMA[bar])
                    if (fDwnStop < Result[bar - 1])
                        Result[bar] = fDwnStop;
                    else
                        Result[bar] = Result[bar - 1];
                else
                    Result[bar] = fDwnStop;

                base[bar] = Result[bar];
            }
        }

        public static ElderSafeStopLong Series(Bars bars, int period, int lookback, double coefficient)
        {
            string description = string.Concat(new object[] { "ElderSafeStopLong(", period, ",", lookback, ",", coefficient, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (ElderSafeStopLong)bars.Cache[description];
            }

            ElderSafeStopLong _ElderSafeStopLong = new ElderSafeStopLong(bars, period, lookback, coefficient, description);
            bars.Cache[description] = _ElderSafeStopLong;
            return _ElderSafeStopLong;
        }
    }

    public class ElderSafeStopLongHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ElderSafeStopLongHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(22, 2, 300), new RangeBoundInt32(10, 2, 300), new RangeBoundDouble(2.0, 0.5, 10.0) };
            _paramNames = new string[] { "Bars", "Period", "Lookback", "Coefficient" };
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
                return "Safe Stop Formula for calculating exit stops for Long Positions from Dr. Alexander Elder's Book 'Come into my Trading Room'.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ElderSafeStopLong);
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
                return "http://www2.wealth-lab.com/WL5Wiki/ElderSafeStopLong.ashx";
            }
        }
    }

    public class ElderSafeStopShort : DataSeries
    {
        public ElderSafeStopShort(Bars bars, int period, int lookback, double coefficient, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(period * 3, lookback); // unstable: uses EMA
            DataSeries iEMA = EMA.Series(bars.Close, period, EMACalculation.Modern);
            DataSeries iUpPen = new DataSeries(bars, "iUpPen");
            DataSeries iIsUp = new DataSeries(bars, "iIsUp");
            int iUpSum = 0; double fUpSum = 0; double fUpAvg = 0; double fUpStop = 0;
            DataSeries Result = new DataSeries(bars, "Result");

            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < period || bars.Count < lookback)
                return;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                //Calc Up Side Penetration ---
                if (bars.High[bar] > bars.High[bar - 1])
                {
                    iIsUp[bar] = 1;
                    iUpPen[bar] = bars.High[bar] - bars.High[bar - 1];
                }
                else
                {
                    iIsUp[bar] = 0;
                    iUpPen[bar] = 0;
                }

                //Reset our Sumations ---
                iUpSum = 0;
                fUpSum = 0;

                //Summarize the Penetraions in our LookBack Period ---
                for (int x = 1; x <= lookback; x++)
                {
                    iUpSum += (int)Math.Truncate(iIsUp[bar - x]);
                    fUpSum += iUpPen[bar - x];
                }

                //Calc the Avg Penetration ---
                if (iUpSum > 0)
                    fUpAvg = fUpSum / iUpSum;
                else
                    fUpAvg = 0;

                //Calc Todays Stops ---
                fUpStop = bars.High[bar - 1] + (coefficient * fUpAvg);

                //Trend is Up --- Protect our Shorts ---
                if (iEMA[bar - 1] < iEMA[bar])
                    if (fUpStop > Result[bar - 1])
                        Result[bar] = fUpStop;
                    else
                        Result[bar] = Result[bar - 1];
                else
                    Result[bar] = fUpStop;

                base[bar] = Result[bar];
            }
        }

        public static ElderSafeStopShort Series(Bars bars, int period, int lookback, double coefficient)
        {
            string description = string.Concat(new object[] { "ElderSafeStopShort(", period, ",", lookback, ",", coefficient, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (ElderSafeStopShort)bars.Cache[description];
            }

            ElderSafeStopShort _ElderSafeStopShort = new ElderSafeStopShort(bars, period, lookback, coefficient, description);
            bars.Cache[description] = _ElderSafeStopShort;
            return _ElderSafeStopShort;
        }
    }

    public class ElderSafeStopShortHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ElderSafeStopShortHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(22, 2, 300), new RangeBoundInt32(10, 2, 300), new RangeBoundDouble(2.0, 0.5, 10.0) };
            _paramNames = new string[] { "Bars", "Period", "Lookback", "Coefficient" };
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
                return "Safe Stop Formula for calculating exit stops for Short Positions from Dr. Alexander Elder's Book 'Come into my Trading Room'.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ElderSafeStopShort);
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
                return "http://www2.wealth-lab.com/WL5Wiki/ElderSafeStopShort.ashx";
            }
        }
    }
}