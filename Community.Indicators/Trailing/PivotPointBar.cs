using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class PivotPointBar : DataSeries
    {
        private Bars _bars;
        private bool _ppHigh;
        private bool _tradeable;
        private int _period;
        private double _lastPP = -1d;

        //Static Series method returns cached instance, or creates new instance
        public static PivotPointBar Series(Bars bars, int period, bool pivotPointHigh, bool tradeable)
        {
            //Build description
            string description = "PivotPointBar(" + period + "," + pivotPointHigh + "," + tradeable + ")";

            //Cached?
            if (bars.Cache.ContainsKey(description))
                return (PivotPointBar)bars.Cache[description];

            //Create instance
            PivotPointBar ppb = new PivotPointBar(bars, period, pivotPointHigh, tradeable, description);
            bars.Cache[description] = ppb;
            return ppb;
        }

        //Constructor
        public PivotPointBar(Bars bars, int period, bool pivotPointHigh, bool tradeable, string description)
            : base(bars, description)
        {
            _bars = bars;
            _ppHigh = pivotPointHigh;
            _period = period;
            _tradeable = tradeable;

            FirstValidValue += _period;
            if (bars.Count < _period) return;

            for (int bar = _period; bar < bars.Count; bar++)
            {
                this[bar] = -1d;        // returns -1 until a Valid Pivot Point is found
            }

            for (int bar = _period; bar < bars.Count; bar++)
            {
                if (_tradeable)
                {
                    if (_ppHigh)
                    {
                        if (bars.High[bar] >= Highest.Series(bars.High, _period)[bar - 1])
                            _lastPP = bar;
                    }
                    else if (bars.Low[bar] <= Lowest.Series(bars.Low, _period)[bar - 1])
                        _lastPP = bar;
                }
                else
                {
                    int periodFwd = _period;
                    if (bar + period >= bars.Count) periodFwd = bars.Count - bar - 1;
                    if (_ppHigh)
                    {
                        if (bars.High[bar] >= Highest.Series(bars.High, _period)[bar - 1]
                            && bars.High[bar] >= Highest.Value(bar + periodFwd, bars.High, periodFwd))  // use Value method since periodFwd is variable at the end 
                            _lastPP = bar;

                    }
                    else if (bars.Low[bar] <= Lowest.Series(bars.Low, _period)[bar - 1]
                            && bars.Low[bar] <= Lowest.Value(bar + periodFwd, bars.Low, periodFwd))
                        _lastPP = bar;
                }
                this[bar] = _lastPP;
            }
        }

        public override void CalculatePartialValue()
        {
            if (_bars.Count < _period || _bars.Low.PartialValue == Double.NaN || _bars.High.PartialValue == Double.NaN)
            {
                PartialValue = Double.NaN;
                return;
            }

            int bar = _bars.Count;          // bar - 1 is last bar number (prior to PartialBar)
            if (_tradeable)
            {
                if (_ppHigh)
                {
                    if (_bars.High.PartialValue >= Highest.Series(_bars.High, _period)[bar - 1])
                        _lastPP = bar;
                }
                else if (_bars.Low.PartialValue <= Lowest.Series(_bars.Low, _period)[bar - 1])
                    _lastPP = bar;
            }
            else
            {
                int periodFwd = _period;
                if (bar + _period >= _bars.Count) periodFwd = _bars.Count - bar - 1;
                if (_ppHigh)
                {
                    if (_bars.High.PartialValue >= Highest.Series(_bars.High, _period)[bar - 1]
                        && _bars.High.PartialValue >= Highest.Value(bar + periodFwd, _bars.High, periodFwd))  // use Value method since periodFwd is variable at the end 
                        _lastPP = bar;

                }
                else if (_bars.Low.PartialValue <= Lowest.Series(_bars.Low, _period)[bar - 1]
                        && _bars.Low.PartialValue <= Lowest.Value(bar + periodFwd, _bars.Low, periodFwd))
                    _lastPP = bar;
            }
            this.PartialValue = _lastPP;
        }
    }

    public class PivotPointBarHelper : IndicatorHelper
    {
        private static object[] _defaultValues = { BarDataType.Bars, new RangeBoundInt32(5, 2, 21), true, true };
        private static string[] _descriptions = { "Bars", "Period", "PP High", "Tradeable" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Brown;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Dots;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 3;
            }
        }

        public override string Description
        {
            get
            {
                return @"PivotPointBar returns the bar number of the most-recent Pivot Point.  Based on Fidelity Active Trader Pro® Pivot Points when Period = 5 and Tradeable = false";
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www2.wealth-lab.com/WL5Wiki/PivotPointBar.ashx";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PivotPointBar);
            }
        }

        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return _defaultValues;
            }
        }

        public override IList<string> ParameterDescriptions
        {
            get
            {
                return _descriptions;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "PivotPointBar";
            }
        }
    }
}