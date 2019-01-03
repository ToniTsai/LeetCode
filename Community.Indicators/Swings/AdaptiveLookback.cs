using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    // Both Adaptive Lookback and Wealth-Lab Moving Average were created by Eugene

    public class AdaptiveLookback : DataSeries
    {
        public AdaptiveLookback(Bars bars, int howManySwings, bool UseAll, string description)
            : base(bars, description)
        {
            bool SwingLo, SwingHi;
			double lastSL = bars.Low[1];
            double lastSH = bars.High[1];
			int firstSwingBarOnChart = 0;
			int lastSwingInCalc = 0;
			int swingCount = 0;
            double so = 0;
            List<int> SwingBarArray = new List<int>();

            for (int bar = 5; bar < bars.Count; bar++)
            {
                SwingLo = (bars.Low[bar - 2] < bars.Low[bar - 3]) & (bars.Low[bar - 3] < bars.Low[bar - 4]) &
                    (bars.Low[bar - 1] > bars.Low[bar - 2]) & (bars.Low[bar] > bars.Low[bar - 1]);
                SwingHi = (bars.High[bar - 2] > bars.High[bar - 3]) & (bars.High[bar - 3] > bars.High[bar - 4]) &
                    (bars.High[bar - 1] < bars.High[bar - 2]) & (bars.High[bar] < bars.High[bar - 1]);

                so = SwingLo ? -1 : SwingHi ? 1 : 0;

                if( (so != 0) & (swingCount == 0))
                {
                    firstSwingBarOnChart = bar;
                    swingCount++;
                    SwingBarArray.Add(bar);
                }
                else
                    if (swingCount > 0)
                    {
                        if( so != 0.0)
                        {
                            swingCount++;
                            SwingBarArray.Add(bar);
                        }

                        if (swingCount == howManySwings)
                            base.FirstValidValue = bar;
                    }

                lastSwingInCalc = (SwingBarArray.Count - howManySwings);

                if (lastSwingInCalc >= 0)
                {
                    base[bar] = UseAll ? (int)(bars.Count / SwingBarArray.Count) :
                        (bar - (int)SwingBarArray[lastSwingInCalc]) / howManySwings;
                }
           }
        }

        public AdaptiveLookback(Bars bars, int howManySwings, bool UseAll, bool fastSwing, string description)
            : base(bars, description)
        {
            bool SwingLo, SwingHi;
            double lastSL = bars.Low[1];
            double lastSH = bars.High[1];
            int firstSwingBarOnChart = 0;
            int lastSwingInCalc = 0;
            int swingCount = 0;
            double so = 0;
            List<int> SwingBarArray = new List<int>();

            for (int bar = 5; bar < bars.Count; bar++)
            {
                SwingLo = (bars.Low[bar - 2] < bars.Low[bar - 3]) & (bars.Low[bar - 3] < bars.Low[bar - 4]) &
                    (bars.Low[bar - 1] > bars.Low[bar - 2]) & (bars.Low[bar] > bars.Low[bar - 1]);
                SwingHi = (bars.High[bar - 2] > bars.High[bar - 3]) & (bars.High[bar - 3] > bars.High[bar - 4]) &
                    (bars.High[bar - 1] < bars.High[bar - 2]) & (bars.High[bar] < bars.High[bar - 1]);

                if (fastSwing)
                {
                    SwingLo = (CumDown.Series(bars.Low, 1)[bar - 2] >= 1) && (CumUp.Series(bars.High, 1)[bar] == 1);
                    SwingHi = (CumUp.Series(bars.High, 1)[bar - 2] >= 1) && (CumDown.Series(bars.Low, 1)[bar] == 1);
                }

                so = SwingLo ? -1 : SwingHi ? 1 : 0;

                if ((so != 0) & (swingCount == 0))
                {
                    firstSwingBarOnChart = bar;
                    swingCount++;
                    SwingBarArray.Add(bar);
                }
                else
                    if (swingCount > 0)
                    {
                        if (so != 0.0)
                        {
                            swingCount++;
                            SwingBarArray.Add(bar);
                        }

                        if (swingCount == howManySwings)
                            base.FirstValidValue = bar;
                    }

                lastSwingInCalc = (SwingBarArray.Count - howManySwings);

                if (lastSwingInCalc >= 0)
                {
                    base[bar] = UseAll ? (int)(bars.Count / SwingBarArray.Count) :
                        (bar - (int)SwingBarArray[lastSwingInCalc]) / howManySwings;
                }
            }
        }

        public AdaptiveLookback(Bars bars, int howManySwings, bool UseAll, bool fastSwing, bool preciseDetection, string description)
            : base(bars, description)
        {
            bool SwingLo, SwingHi;
            double lastSL = bars.Low[1];
            double lastSH = bars.High[1];
            int firstSwingBarOnChart = 0;
            int lastSwingInCalc = 0;
            int swingCount = 0;
            double so = 0;
            List<int> SwingBarArray = new List<int>();

            for (int bar = 5; bar < bars.Count; bar++)
            {
                SwingLo = (bars.Low[bar - 2] < bars.Low[bar - 3]) & (bars.Low[bar - 3] < bars.Low[bar - 4]) &
                    (bars.Low[bar - 1] > bars.Low[bar - 2]) & (bars.Low[bar] > bars.Low[bar - 1]);
                SwingHi = (bars.High[bar - 2] > bars.High[bar - 3]) & (bars.High[bar - 3] > bars.High[bar - 4]) &
                    (bars.High[bar - 1] < bars.High[bar - 2]) & (bars.High[bar] < bars.High[bar - 1]);

                if (fastSwing)
                {
                    SwingLo = (CumDown.Series(bars.Low, 1)[bar - 2] >= 1) && (CumUp.Series(bars.High, 1)[bar] == 1);
                    SwingHi = (CumUp.Series(bars.High, 1)[bar - 2] >= 1) && (CumDown.Series(bars.Low, 1)[bar] == 1);

                    if (preciseDetection)
                    {
                        SwingLo = (CumDown.Series(bars.Low, 1)[bar - 1] >= 2) && (CumUp.Series(bars.Low, 1)[bar] == 1);
                        SwingHi = (CumUp.Series(bars.High, 1)[bar - 1] >= 2) && (CumDown.Series(bars.High, 1)[bar] == 1);
                    }
                }

                so = SwingLo ? -1 : SwingHi ? 1 : 0;

                if ((so != 0) & (swingCount == 0))
                {
                    firstSwingBarOnChart = bar;
                    swingCount++;
                    SwingBarArray.Add(bar);
                }
                else
                    if (swingCount > 0)
                    {
                        if (so != 0.0)
                        {
                            swingCount++;
                            SwingBarArray.Add(bar);
                        }

                        if (swingCount == howManySwings)
                            base.FirstValidValue = bar;
                    }

                lastSwingInCalc = (SwingBarArray.Count - howManySwings);

                if (lastSwingInCalc >= 0)
                {
                    base[bar] = UseAll ? (int)(bars.Count / SwingBarArray.Count) :
                        (bar - (int)SwingBarArray[lastSwingInCalc]) / howManySwings;
                }
            }
        }

        /// <summary>
        /// Parameterless Adaptive Lookback
        /// Determined as the distance between 2 last same-sign swings (last SwingHi to penultimate SwingHi and vice versa)
        /// </summary>
        /// <param name="bars">A Bars object</param>
        /// <param name="fastSwing">Use faster swing detection method</param>
        /// <param name="preciseDetection">Applicable only when fastSwing is true. When true, the high of the bar preceding a swing high or the low of the bar preceding a swing low can not be equal to the swing bar's high/low, respectively.</param>
        /// <param name="description">Series description</param>
        public AdaptiveLookback(Bars bars, bool fastSwing, bool preciseDetection, string description)
            : base(bars, description)
        {
            bool SwingLo, SwingHi;
            double lastSL = bars.Low[1];
            double lastSH = bars.High[1];
            int swingCount = 0;
            double swingDirection = 0;
            List<Tuple<int, double>> lstSwingBars = new List<Tuple<int, double>>();

            for (int bar = 5; bar < bars.Count; bar++)
            {
                SwingLo = (bars.Low[bar - 2] < bars.Low[bar - 3]) & (bars.Low[bar - 3] < bars.Low[bar - 4]) &
                    (bars.Low[bar - 1] > bars.Low[bar - 2]) & (bars.Low[bar] > bars.Low[bar - 1]);
                SwingHi = (bars.High[bar - 2] > bars.High[bar - 3]) & (bars.High[bar - 3] > bars.High[bar - 4]) &
                    (bars.High[bar - 1] < bars.High[bar - 2]) & (bars.High[bar] < bars.High[bar - 1]);

                if (fastSwing)
                {
                    SwingLo = (CumDown.Series(bars.Low, 1)[bar - 2] >= 1) && (CumUp.Series(bars.High, 1)[bar] == 1);
                    SwingHi = (CumUp.Series(bars.High, 1)[bar - 2] >= 1) && (CumDown.Series(bars.Low, 1)[bar] == 1);

                    if (preciseDetection)
                    {
                        SwingLo = (CumDown.Series(bars.Low, 1)[bar - 1] >= 2) && (CumUp.Series(bars.Low, 1)[bar] == 1);
                        SwingHi = (CumUp.Series(bars.High, 1)[bar - 1] >= 2) && (CumDown.Series(bars.High, 1)[bar] == 1);
                    }
                }
                
                swingDirection = SwingLo ? -1 : SwingHi ? 1 : 0;

                if ((swingDirection != 0) & (swingCount == 0))
                {
                    swingCount++;
                    lstSwingBars.Add(new Tuple<int, double>(bar, swingDirection));
                }
                else
                    if (swingCount > 0)
                    {
                        if (swingDirection != 0.0)
                        {
                            swingCount++;

                            double prevValue = lstSwingBars[lstSwingBars.Count - 1].Item1;
                            if (prevValue != swingDirection)
                            {
                                lstSwingBars.Add(new Tuple<int, double>(bar, swingDirection));
                            }
                        }

                        if (swingCount == 3)
                            base.FirstValidValue = bar;
                    }

                if (lstSwingBars.Count >= 3)
                {
                    int lastDistance = (int)(lstSwingBars[lstSwingBars.Count - 1].Item1 - lstSwingBars[lstSwingBars.Count - 2].Item1);
                    int penultimateDistance = (int)(lstSwingBars[lstSwingBars.Count - 2].Item1 - lstSwingBars[lstSwingBars.Count - 3].Item1);
                    int distanceBetweenTwo = (int)(lastDistance + penultimateDistance);

                    base[bar] = distanceBetweenTwo;
                }
            }
        }

        /// <summary>
        /// Parameterless Adaptive Lookback
        /// Determined as the distance between 2 last same-sign swings (last SwingHi to penultimate SwingHi and vice versa)
        /// </summary>
        /// <param name="bars">A Bars object</param>
        /// <param name="fastSwing">Use faster swing detection method</param>
        /// <param name="description">Series description</param>
        public AdaptiveLookback(Bars bars, bool fastSwing, string description)
            : base(bars, description)
        {
            bool SwingLo, SwingHi;
            double lastSL = bars.Low[1];
            double lastSH = bars.High[1];
            int swingCount = 0;
            double swingDirection = 0;
            List<Tuple<int, double>> lstSwingBars = new List<Tuple<int, double>>();

            for (int bar = 5; bar < bars.Count; bar++)
            {
                SwingLo = (bars.Low[bar - 2] < bars.Low[bar - 3]) & (bars.Low[bar - 3] < bars.Low[bar - 4]) &
                    (bars.Low[bar - 1] > bars.Low[bar - 2]) & (bars.Low[bar] > bars.Low[bar - 1]);
                SwingHi = (bars.High[bar - 2] > bars.High[bar - 3]) & (bars.High[bar - 3] > bars.High[bar - 4]) &
                    (bars.High[bar - 1] < bars.High[bar - 2]) & (bars.High[bar] < bars.High[bar - 1]);

                if (fastSwing)
                {
                    SwingLo = (CumDown.Series(bars.Low, 1)[bar - 2] >= 1) && (CumUp.Series(bars.High, 1)[bar] == 1);
                    SwingHi = (CumUp.Series(bars.High, 1)[bar - 2] >= 1) && (CumDown.Series(bars.Low, 1)[bar] == 1);
                }

                swingDirection = SwingLo ? -1 : SwingHi ? 1 : 0;

                if ((swingDirection != 0) & (swingCount == 0))
                {
                    swingCount++;
                    lstSwingBars.Add(new Tuple<int, double>(bar, swingDirection));
                }
                else
                    if (swingCount > 0)
                    {
                        if (swingDirection != 0.0)
                        {
                            swingCount++;

                            double prevValue = lstSwingBars[lstSwingBars.Count - 1].Item1;
                            if (prevValue != swingDirection)
                            {
                                lstSwingBars.Add(new Tuple<int, double>(bar, swingDirection));
                            }
                        }

                        if (swingCount == 3)
                            base.FirstValidValue = bar;
                    }

                if (lstSwingBars.Count >= 3)
                {
                    int lastDistance = (int)(lstSwingBars[lstSwingBars.Count - 1].Item1 - lstSwingBars[lstSwingBars.Count - 2].Item1);
                    int penultimateDistance = (int)(lstSwingBars[lstSwingBars.Count - 2].Item1 - lstSwingBars[lstSwingBars.Count - 3].Item1);
                    int distanceBetweenTwo = (int)(lastDistance + penultimateDistance);

                    base[bar] = distanceBetweenTwo;
                }
            }
        }

        public static AdaptiveLookback Series(Bars bars, int howManySwings, bool UseAll)
        {
            string description = string.Concat(new object[] { "Adaptive Lookback(", howManySwings.ToString(), "," , UseAll.ToString(), ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (AdaptiveLookback)bars.Cache[description];
            }

            AdaptiveLookback _AdaptiveLookback = new AdaptiveLookback(bars, howManySwings, UseAll, description);
            bars.Cache[description] = _AdaptiveLookback;
            return _AdaptiveLookback;
        }
    }

    public class AdaptiveLookbackHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AdaptiveLookbackHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(6, 1, 50), new Boolean() };
            _paramNames = new string[] { "Bars", "Number of Swings", "Use all" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkRed;
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
                return "The Adaptive Lookback indicator by Eugene. is a parameterless, universal tool that helps find an optimum period for short-term indicators, " +
                    "turning them into responsive and adaptive instruments.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AdaptiveLookback);
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
                return "AdaptiveLookback";
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www2.wealth-lab.com/WL5Wiki/AdaptiveLookback.ashx";
            }
        }
    }

    
}
