using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WealthLab;
using System.Drawing;

namespace Community.Indicators
{
    public class ExternalSymbol : DataSeries
    {
        public ExternalSymbol(Bars source, string dataSet, string extSym, DataSeries ds, string description)
            : base(ds, description)
        {
            base.FirstValidValue = 0;

            Bars b = new Bars(extSym, ds.DataScale.Scale, ds.DataScale.BarInterval);
            try
            {
                b = MainModuleInstance.LoadExternalSymbol(dataSet, extSym);
                b = BarScaleConverter.Synchronize(b, source);
                for (int bar = FirstValidValue; bar < b.Count; bar++)
                {
                    if (ds.Description == "Open")
                        base[bar] = b.Open[bar];
                    else
                        if (ds.Description == "High")
                            base[bar] = b.High[bar];
                        else
                            if (ds.Description == "Low")
                                base[bar] = b.Low[bar];
                            else
                                base[bar] = b.Close[bar];
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static ExternalSymbol Series(Bars source, string dataSet, string extSym, DataSeries ds)
        {
            string description = string.Concat(new object[] { "External Symbol (", dataSet, ",", extSym, ",", ds.Description, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (ExternalSymbol)ds.Cache[description];
            }

            ExternalSymbol _ExtSymHelper = new ExternalSymbol(source, dataSet, extSym, ds, description);
            ds.Cache[description] = _ExtSymHelper;
            return _ExtSymHelper;
        }
    }

    public class ExternalSymbolHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ExternalSymbolHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, "Dow 30", "AA", CoreDataSeries.Close };
            _paramNames = new string[] { "Source Bars", "DataSet", "External symbol", "DataSeries" };
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
                return "Plots external symbol's OHLC. For use in charts and Rule Builder.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ExternalSymbol);
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
                return "ExternalSymbolPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ExternalSymbolPlotter.ashx";
            }
        }
    }
}
