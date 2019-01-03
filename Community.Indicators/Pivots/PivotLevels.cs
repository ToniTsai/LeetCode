using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WealthLab;
using System.Drawing;
using WealthLab.Indicators;
using System.Windows.Forms;

namespace Community.Indicators
{
    public enum PivotType { Pivot, R1, R2, R3, S1, S2, S3 }

    public class PivotLevels : DataSeries
    {
        public PivotLevels(Bars bars, PivotType type, string description)
            : base(bars, description)
        {
            DataSeries Pivot = new DataSeries(bars, "Daily Pivot Level");
            DataSeries DayHigh = new DataSeries(bars, "Previous Day's High Level");
            DataSeries DayClose = new DataSeries(bars, "Previous Day's Close Level");
            DataSeries DayLow = new DataSeries(bars, "Previous Day's Low Level");
            DataSeries R1 = new DataSeries(bars, "Resistance Pivot Level 1");
            DataSeries S1 = new DataSeries(bars, "Support Pivot Level 1");
            DataSeries R2 = new DataSeries(bars, "Resistance Pivot Level 2");
            DataSeries S2 = new DataSeries(bars, "Support Pivot Level 2");
            DataSeries R3 = new DataSeries(bars, "Resistance Pivot Level 3");
            DataSeries S3 = new DataSeries(bars, "Support Pivot Level 3");
            DataSeries result = new DataSeries(bars, "Result");

            base.FirstValidValue = 1;

            if (!bars.IsIntraday)   // Daily data
            {
                //base.FirstValidValue = bars.FirstActualBar;

                Pivot = AveragePriceC.Series(bars);
                DayHigh = bars.High;
                DayClose = bars.Close;
                DayLow = bars.Low;

                CalcPivotLevel(type, Pivot, DayHigh, DayLow, ref R1, ref S1, ref R2, ref S2, ref R3, ref S3, ref result);
            }
            else    // Special logic for intraday data
            {
                try
                {
                    Bars eodBars = BarScaleConverter.ToDaily(bars);
                    Pivot = AveragePriceC.Series(eodBars);
                    DayHigh = eodBars.High;
                    DayClose = eodBars.Close;
                    DayLow = eodBars.Low;

                    R1 = BarScaleConverter.Synchronize(R1, eodBars);
                    S1 = BarScaleConverter.Synchronize(S1, eodBars);
                    R2 = BarScaleConverter.Synchronize(R2, eodBars);
                    S2 = BarScaleConverter.Synchronize(S2, eodBars);
                    R3 = BarScaleConverter.Synchronize(R3, eodBars);
                    S3 = BarScaleConverter.Synchronize(S3, eodBars);

                    CalcPivotLevel(type, Pivot, DayHigh, DayLow, ref R1, ref S1, ref R2, ref S2, ref R3, ref S3, ref result);
                    result = BarScaleConverter.Synchronize(result, bars);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = result[bar];
            }
        }

        /// <summary>
        /// Calculate Main Pivots Series
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Pivot"></param>
        /// <param name="DayHigh"></param>
        /// <param name="DayLow"></param>
        /// <param name="R1"></param>
        /// <param name="S1"></param>
        /// <param name="R2"></param>
        /// <param name="S2"></param>
        /// <param name="R3"></param>
        /// <param name="S3"></param>
        /// <param name="result"></param>
        private static void CalcPivotLevel(PivotType type, DataSeries Pivot, DataSeries DayHigh, DataSeries DayLow, ref DataSeries R1, ref DataSeries S1, ref DataSeries R2, ref DataSeries S2, ref DataSeries R3, ref DataSeries S3, ref DataSeries result)
        {
            R1 = 2 * Pivot - DayLow;
            S1 = 2 * Pivot - DayHigh;
            R2 = Pivot + (R1 - S1);
            S2 = Pivot - (R1 - S1);
            R3 = R1 + (DayHigh - DayLow);
            S3 = S1 - (DayHigh - DayLow);

            switch (type)
            {
                case PivotType.Pivot:
                    result = Pivot;
                    break;
                case PivotType.R1:
                    result = R1;
                    break;
                case PivotType.R2:
                    result = R2;
                    break;
                case PivotType.R3:
                    result = R3;
                    break;
                case PivotType.S1:
                    result = S1;
                    break;
                case PivotType.S2:
                    result = S2;
                    break;
                case PivotType.S3:
                    result = S3;
                    break;
                default:
                    break;
            }
        }

        public static PivotLevels Series(Bars bars, PivotType type)
        {
            string description = string.Concat(new object[] { "PivotLevels(", type.ToString(), ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (PivotLevels)bars.Cache[description];
            }

            PivotLevels _PivotLevels = new PivotLevels(bars, type, description);
            bars.Cache[description] = _PivotLevels;
            return _PivotLevels;
        }
    }

    public class PivotLevelsHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PivotLevelsHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, PivotType.Pivot };
            _paramNames = new string[] { "Bars", "Pivot type" };
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
                return 2;
            }
        }

        public override string Description
        {
            get
            {
                return "Plots selected floor trader's pivot line. On intraday data, scales it to Daily first.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PivotLevels);
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
                return @"http://www2.wealth-lab.com/WL5Wiki/PivotLevels.ashx";
            }
        }
    }
}