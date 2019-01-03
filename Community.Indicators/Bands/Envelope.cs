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
    public class EnvelopeUpper : DataSeries
    {
        public EnvelopeUpper(DataSeries ds, int period, double pct, ChoiceOfMA ma, string description)
            : base(ds, description)
        {
            if (ma == ChoiceOfMA.SMA)
                base.FirstValidValue = period;
            else
                base.FirstValidValue = (period * 3);

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            var rangePartitioner = Partitioner.Create(0, ds.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    if (ma == ChoiceOfMA.EMA)
                        base[bar] = EMA.Series(ds, period, EMACalculation.Modern)[bar] * (1 + (pct / 100));
                    else
                        if (ma == ChoiceOfMA.SMA)
                            base[bar] = Community.Indicators.FastSMA.Series(ds, period)[bar] * (1 + (pct / 100));
                        else
                            if (ma == ChoiceOfMA.WMA)
                                base[bar] = WMA.Series(ds, period)[bar] * (1 + (pct / 100));
                            else
                                if (ma == ChoiceOfMA.SMMA)
                                    base[bar] = SMMA.Series(ds, period)[bar] * (1 + (pct / 100));
                }
            });
        }

        public static EnvelopeUpper Series(DataSeries ds, int period, double pct, ChoiceOfMA ma)
        {
            string description = string.Concat(new object[] { "MA Envelope Upper(", ds.Description, ",", period, ",", pct, ",", ma, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (EnvelopeUpper)ds.Cache[description];
            }

            EnvelopeUpper _EnvelopeUpper = new EnvelopeUpper(ds, period, pct, ma, description);
            ds.Cache[description] = _EnvelopeUpper;
            return _EnvelopeUpper;
        }
    }

    public class EnvelopeUpperHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static EnvelopeUpperHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 5, 300), 
                new RangeBoundDouble(5, 0.5, 100), ChoiceOfMA.SMA };
            _paramNames = new string[] { "Data Series", "MA Period", "Percent", "MA Type" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override Color DefaultBandColor
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
                return "Moving average envelopes are lines plotted a certain percentage above and below a moving average of price. " +
                    "They are also known as trading bands, moving average bands, price envelopes and percentage envelopes.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(EnvelopeUpper);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(EnvelopeLower);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MAEnvelope.ashx";
            }
        }
    }

    public class EnvelopeLower : DataSeries
    {
        public EnvelopeLower(DataSeries ds, int period, double pct, ChoiceOfMA ma, string description)
            : base(ds, description)
        {
            if (ma == ChoiceOfMA.SMA)
                base.FirstValidValue = period;
            else
                base.FirstValidValue = (period * 3);

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            var rangePartitioner = Partitioner.Create(0, ds.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    if (ma == ChoiceOfMA.EMA)
                        base[bar] = EMA.Series(ds, period, EMACalculation.Modern)[bar] * (1 - (pct / 100));
                    else
                        if (ma == ChoiceOfMA.SMA)
                            base[bar] = Community.Indicators.FastSMA.Series(ds, period)[bar] * (1 - (pct / 100));
                        else
                            if (ma == ChoiceOfMA.WMA)
                                base[bar] = WMA.Series(ds, period)[bar] * (1 - (pct / 100));
                            else
                                if (ma == ChoiceOfMA.SMMA)
                                    base[bar] = SMMA.Series(ds, period)[bar] * (1 - (pct / 100));
                }
            });
        }

        public static EnvelopeLower Series(DataSeries ds, int period, double pct, ChoiceOfMA ma)
        {
            string description = string.Concat(new object[] { "MA Envelope Lower(", ds.Description, ",", period, ",", pct, ",", ma, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (EnvelopeLower)ds.Cache[description];
            }

            EnvelopeLower _EnvelopeLower = new EnvelopeLower(ds, period, pct, ma, description);
            ds.Cache[description] = _EnvelopeLower;
            return _EnvelopeLower;
        }
    }

    public class EnvelopeLowerHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static EnvelopeLowerHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 5, 300), 
                new RangeBoundDouble(5, 0.5, 100), ChoiceOfMA.SMA };
            _paramNames = new string[] { "Data Series", "MA Period", "Percent", "MA Type" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override Color DefaultBandColor
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
                return "Moving average envelopes are lines plotted a certain percentage above and below a moving average of price. " +
                    "They are also known as trading bands, moving average bands, price envelopes and percentage envelopes.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(EnvelopeLower);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(EnvelopeUpper);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MAEnvelope.ashx";
            }
        }
    }
}
