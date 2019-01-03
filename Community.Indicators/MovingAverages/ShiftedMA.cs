using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public enum ChoiceOfMA
    {
        SMA, EMA, WMA, SMMA
    }

    public class ShiftedMA : DataSeries
    {
        public ShiftedMA(DataSeries ds, int period, int shift, ChoiceOfMA option, string description)
            : base(ds, description)
        {
            if (ds.Count < period + shift)
                return;

            if (option == ChoiceOfMA.SMA)
                base.FirstValidValue = period + shift;
            else
                base.FirstValidValue = (period * 3) + shift;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                if (option == ChoiceOfMA.EMA)
                    base[bar] = EMA.Series(ds, period, EMACalculation.Modern)[bar - shift];
                else
                    if (option == ChoiceOfMA.SMA)
                        base[bar] = Community.Indicators.FastSMA.Series(ds, period)[bar - shift];
                    else
                        if (option == ChoiceOfMA.WMA)
                            base[bar] = WMA.Series(ds, period)[bar - shift];
                        else
                            if (option == ChoiceOfMA.SMMA)
                                base[bar] = SMMA.Series(ds, period)[bar - shift];
            }
        }

        public static ShiftedMA Series(DataSeries ds, int period, int shift, ChoiceOfMA option)
        {
            string description = string.Concat(new object[] { "ShiftedMA(", ds.Description, ",", period, ",", shift, ",", option, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (ShiftedMA)ds.Cache[description];
            }

            ShiftedMA _ShiftedMA = new ShiftedMA(ds, period, shift, option, description);
            ds.Cache[description] = _ShiftedMA;
            return _ShiftedMA;
        }
    }

    public class ShiftedMAHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ShiftedMAHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 300), 
                new RangeBoundInt32(2, 1, 100), ChoiceOfMA.SMA };
            _paramNames = new string[] { "Data Series", "MA Period", "Shift (delay)", "MA Type" };
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
                return "The delayed moving average is a helper tool for the users of rule-based strategies. " +
                    "It is simply a moving average - SMA, EMA, WMA or SMMA - delayed by user-selected number of periods.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ShiftedMA);
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
                return @"http://www2.wealth-lab.com/WL5Wiki/ShiftedMA.ashx";
            }
        }
    }
}