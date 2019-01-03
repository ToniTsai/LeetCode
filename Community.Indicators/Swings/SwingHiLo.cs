using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using WealthLab;

namespace Community.Indicators
{
    public class SwingHiLo : DataSeries
    {
        private Bars _bars;
        private DataSeries _dsH;
        private DataSeries _dsL;
        private int _leftBars;
        private double _leftThreshold;
        private int _rightBars;
        private double _rightThreshold;
        private double _equalPriceFloat;
        private bool _percentMode;
        private bool _returnLeftSwing;
        private bool _returnOuterSwings;
        private bool _steppedSeries;

        public static SwingHiLo Series(Bars bars, int leftBars, double leftThreshold, int rightBars, double rightThreshold, double equalPriceFloat, bool percentMode, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries)
        {
            string description = "SwingHiLo(" + bars + "," + leftBars + "," + leftThreshold + "," + rightBars + "," + rightThreshold + "," + equalPriceFloat + "," + percentMode + "," + returnLeftSwing + "," + returnOuterSwings + "," + steppedSeries + ")";
            if (bars.Cache.ContainsKey(description))
                return (SwingHiLo)bars.Cache[description];
            SwingHiLo swingHiLo = new SwingHiLo(bars, leftBars, leftThreshold, rightBars, rightThreshold, equalPriceFloat, percentMode, returnLeftSwing, returnOuterSwings, steppedSeries, description);
            bars.Cache[description] = swingHiLo;
            return swingHiLo;
        }

        public SwingHiLo(DataSeries ds, int leftBars, int rightBars, double equalPriceFloat, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries, string description)
            : base(ds, description)
        {
            _dsH = ds;
            _dsL = ds;
            _leftBars = leftBars;
            _rightBars = rightBars;
            _equalPriceFloat = equalPriceFloat;
            _returnLeftSwing = returnLeftSwing;
            _returnOuterSwings = returnOuterSwings;
            _steppedSeries = steppedSeries;
            this.FirstValidValue = leftBars + rightBars + 1;
            int lastBar = leftBars + rightBars + 1;
            int lastSwg = -1;
            int lastSwgHighBar = -1;
            int lastSwgLowBar = -1;
            int padL = 0;
            int inBtwn = 0;
            int stepNum = 0;
            int maxPadL = Math.Max(leftBars * 2, 100);
            double lastSwgPrice = double.NaN;
            double stepSH = double.NaN;
            double stepSL = double.NaN;
            double stepAccum = double.NaN;
            bool haveSL = false;
            bool haveSH = false;
            for (int bar = 0; bar < ds.Count; bar++)
            {
                if (bar < leftBars + rightBars + 1)
                {
                    this[bar] = ds[bar];
                    lastBar = bar;
                }
                else
                {
                    haveSL = false;
                    haveSH = false;
                    for (int rbar = bar; rbar > bar - rightBars; rbar--)
                    {
                        if (ds[bar - rightBars] > ds[rbar])
                            haveSH = true;
                        else
                        {
                            haveSH = false;
                            break;
                        }
                    }
                    for (int rbar = bar; rbar > bar - rightBars; rbar--)
                    {
                        if (ds[bar - rightBars] < ds[rbar])
                            haveSL = true;
                        else
                        {
                            haveSL = false;
                            break;
                        }
                    }
                    if (haveSH == true)
                    {
                        padL = 0;
                        inBtwn = 0;
                        for (int Lbar = bar - rightBars - 1; Lbar >= bar - rightBars - leftBars - padL + inBtwn; Lbar--)
                        {
                            if (Math.Abs(ds[bar - rightBars] - ds[Lbar]) < equalPriceFloat)
                            {
                                if (Lbar - 1 > leftBars && padL + 1 < maxPadL)
                                {
                                    if (Lbar < bar - rightBars - 2 && ds[Lbar + 1] > ds[Lbar]) inBtwn++;
                                    padL = (bar - rightBars) - Lbar;
                                    continue;
                                }
                            }
                            if (ds[bar - rightBars] < ds[Lbar])
                            {
                                haveSH = false;
                                break;
                            }
                        }
                    }
                    if (haveSL == true)
                    {
                        padL = 0;
                        inBtwn = 0;
                        for (int Lbar = bar - rightBars - 1; Lbar >= bar - rightBars - leftBars - padL + inBtwn; Lbar--)
                        {
                            if (Math.Abs(ds[bar - rightBars] - ds[Lbar]) < equalPriceFloat)
                            {
                                if (Lbar - 1 > leftBars && padL + 1 < maxPadL)
                                {
                                    if (Lbar < bar - rightBars - 2 && ds[Lbar + 1] > ds[Lbar]) inBtwn++;
                                    padL = (bar - rightBars) - Lbar;
                                    continue;
                                }
                            }
                            if (ds[bar - rightBars] > ds[Lbar])
                            {
                                haveSL = false;
                                break;
                            }
                        }
                    }
                    if (haveSH == true)
                    {
                        lastSwgPrice = lastSwg == 1 ? ds[lastBar] : ds[lastBar];
                        int offset = 0;
                        if (returnOuterSwings == true && lastSwgHighBar > 0)
                        {
                            if (ds[lastSwgHighBar] < ds[bar - rightBars])
                                offset = padL;
                            else if (ds[lastSwgHighBar] > ds[bar - rightBars])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padL : 0;
                        this[bar - rightBars - offset] = ds[bar - rightBars - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = bar - rightBars - offset - lastBar;
                            stepSH = (lastSwgPrice - ds[bar - rightBars - offset]) / stepNum;
                            stepAccum = ds[bar - rightBars - offset];
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSH;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                                this[i] = lastSwgPrice;
                        }
                        lastBar = bar - rightBars - offset;
                        lastSwg = 1;
                        lastSwgHighBar = bar - rightBars - offset;
                    }
                    if (haveSL == true)
                    {
                        lastSwgPrice = lastSwg == 1 ? ds[lastBar] : ds[lastBar];
                        int offset = 0;
                        if (returnOuterSwings == true && lastSwgLowBar > 0)
                        {
                            if (ds[lastSwgLowBar] > ds[bar - rightBars])
                                offset = padL;
                            else if (ds[lastSwgLowBar] < ds[bar - rightBars])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padL : 0;
                        this[bar - rightBars - offset] = ds[bar - rightBars - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = bar - rightBars - offset - lastBar;
                            stepSL = (lastSwgPrice - ds[bar - rightBars - offset]) / stepNum;
                            stepAccum = ds[bar - rightBars - offset];
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSL;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                            {
                                this[i] = lastSwgPrice;
                            }
                        }
                        lastBar = bar - rightBars - offset;
                        lastSwg = 0;
                        lastSwgLowBar = bar - rightBars - offset;
                    }
                    if (bar == ds.Count - 1)
                        for (int i = bar; i > lastBar; i--) this[i] = this[lastBar];
                }
            }
        }

        public SwingHiLo(DataSeries ds, int leftBars, double leftThreshold, int rightBars, double rightThreshold, double equalPriceFloat, bool percentMode, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries, string description)
            : base(ds, description)
        {
            _dsH = ds;
            _dsL = ds;
            _leftBars = leftBars;
            _rightBars = rightBars;
            _equalPriceFloat = equalPriceFloat;
            _returnLeftSwing = returnLeftSwing;
            _returnOuterSwings = returnOuterSwings;
            _steppedSeries = steppedSeries;
            _leftThreshold = leftThreshold;
            _rightThreshold = rightThreshold;
            _percentMode = percentMode;
            this.FirstValidValue = leftBars + rightBars + 1;
            int lastBar = leftBars + rightBars + 1;
            int lastSwg = -1;
            int lastSwgHighBar = -1;
            int lastSwgLowBar = -1;
            int padHL = 0;
            int padLL = 0;
            int inBtwn = 0;
            int stepNum = 0;
            int targetBar = 0;
            int stepBk = 0;
            int maxPadL = Math.Max(leftBars * 2, 100);
            double lastSwgPrice = double.NaN;
            double stepSH = double.NaN;
            double stepSL = double.NaN;
            double stepAccum = double.NaN;
            double maxChng = double.NaN;
            double minChng = double.NaN;
            double stepChng = double.NaN;
            bool haveSL = false;
            bool haveSH = false;
            bool requireLeftThreshold = leftThreshold <= 0 ? false : true;
            bool requireRightThreshold = rightThreshold <= 0 ? false : true;
            for (int bar = 0; bar < ds.Count; bar++)
            {
                if (bar < leftBars + rightBars + 1)
                {
                    this[bar] = ds[bar];
                    lastBar = bar;
                }
                else
                {
                    haveSL = false;
                    haveSH = false;
                    maxChng = requireRightThreshold == false ? 999999 : 0;
                    minChng = requireLeftThreshold == false ? 0 : 999999;
                    targetBar = bar - rightBars + stepBk;
                    if (requireRightThreshold == true)
                    {
                        for (int rbar = targetBar + 1; rbar <= bar; rbar++)
                        {
                            if (ds[rbar] >= ds[targetBar])
                            {
                                haveSH = false;
                                break;
                            }
                            minChng = Math.Min(minChng, ds[rbar]);
                            stepChng = percentMode == true ? (ds[targetBar] - minChng) / ds[targetBar] * 100 : ds[targetBar] - minChng;
                            if (stepChng >= rightThreshold - equalPriceFloat)
                            {
                                haveSH = true;
                                break;
                            }
                        }
                    }
                    if (haveSH == false)
                    {
                        for (int rbar = bar - rightBars + 1; rbar <= bar; rbar++)
                        {
                            if (ds[bar - rightBars] <= ds[rbar])
                            {
                                haveSH = false;
                                break;
                            }
                            if (rbar == bar)
                                haveSH = true;
                        }
                    }
                    if (requireRightThreshold == true)
                    {
                        for (int rbar = targetBar + 1; rbar <= bar; rbar++)
                        {
                            if (ds[rbar] <= ds[targetBar])
                            {
                                haveSL = false;
                                break;
                            }
                            maxChng = Math.Max(maxChng, ds[rbar]);
                            stepChng = percentMode == true ? (maxChng - ds[targetBar]) / ds[targetBar] * 100 : maxChng - ds[targetBar];
                            if (stepChng >= rightThreshold - equalPriceFloat)
                            {
                                haveSL = true;
                                break;
                            }
                        }
                    }
                    if (haveSL == false)
                    {
                        for (int rbar = bar - rightBars + 1; rbar <= bar; rbar++)
                        {
                            if (ds[bar - rightBars] >= ds[rbar])
                            {
                                haveSL = false;
                                break;
                            }
                            if (rbar == bar)
                                haveSL = true;
                        }
                    }
                    if (haveSH == true)
                    {
                        padHL = 0;
                        inBtwn = 0;
                        minChng = requireLeftThreshold == false ? 0 : 999999;
                        if (leftBars != 0)
                        {
                            int Lbar = targetBar;
                            while (inBtwn < leftBars && Lbar >= targetBar - leftBars - padHL)
                            {
                                Lbar--;
                                if (ds[targetBar] < ds[Lbar])
                                {
                                    haveSH = false;
                                    break;
                                }
                                if (Math.Abs(ds[targetBar] - ds[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar - 1 > leftBars && padHL + 1 < maxPadL)
                                    {
                                        padHL = (targetBar) - Lbar;
                                        continue;
                                    }
                                }
                                else
                                {
                                    inBtwn++;
                                    if (requireLeftThreshold == true)
                                        minChng = Math.Min(minChng, ds[Lbar]);
                                }
                            }
                            if (requireLeftThreshold == true)
                            {
                                stepChng = percentMode == true ? (ds[targetBar] - minChng) / minChng * 100 : ds[targetBar] - minChng;
                                if (stepChng < leftThreshold - equalPriceFloat)
                                    haveSH = false;
                            }
                        }
                        else
                        {
                            int Lbar = targetBar - 1;
                            while (Lbar > 0)
                            {
                                if (ds[Lbar] > ds[targetBar] + equalPriceFloat)
                                {
                                    haveSH = false;
                                    break;
                                }
                                else if (Math.Abs(ds[targetBar] - ds[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar < targetBar - 2 && ds[Lbar + 1] > ds[Lbar]) inBtwn++;
                                    padHL = targetBar - Lbar;
                                }
                                else
                                {
                                    minChng = Math.Min(minChng, ds[Lbar]);
                                    stepChng = percentMode == true ? (ds[targetBar] - minChng) / minChng * 100 : ds[targetBar] - minChng;
                                    if (stepChng >= leftThreshold - equalPriceFloat)
                                        break;
                                }
                                Lbar--;
                            }
                        }
                    }
                    if (haveSL == true)
                    {
                        padLL = 0;
                        inBtwn = 0;
                        maxChng = requireLeftThreshold == false ? 999999 : 0;
                        if (leftBars != 0)
                        {
                            int Lbar = targetBar;
                            while (inBtwn < leftBars && Lbar >= targetBar - leftBars - padLL)
                            {
                                Lbar--;
                                if (ds[targetBar] > ds[Lbar])
                                {
                                    haveSL = false;
                                    break;
                                }
                                if (Math.Abs(ds[targetBar] - ds[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar - 1 > leftBars && padLL + 1 < maxPadL)
                                    {
                                        padLL = targetBar - Lbar;
                                        continue;
                                    }
                                }
                                else
                                {
                                    inBtwn++;
                                    if (requireLeftThreshold == true)
                                        maxChng = Math.Max(maxChng, ds[Lbar]);
                                }
                            }
                            if (requireLeftThreshold == true)
                            {
                                stepChng = percentMode == true ? (maxChng - ds[targetBar]) / maxChng * 100 : maxChng - ds[targetBar];
                                if (stepChng < leftThreshold - equalPriceFloat)
                                    haveSL = false;
                            }
                        }
                        else
                        {
                            int Lbar = targetBar - 1;
                            while (Lbar > 0)
                            {
                                if (ds[targetBar] > ds[Lbar] + equalPriceFloat)
                                {
                                    haveSL = false;
                                    break;
                                }
                                else if (Math.Abs(ds[targetBar] - ds[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar < targetBar - 2 && ds[Lbar + 1] > ds[Lbar]) inBtwn++;
                                    padLL = (targetBar) - Lbar;
                                }
                                else
                                {
                                    maxChng = Math.Max(maxChng, ds[Lbar]);
                                    stepChng = percentMode == true ? (maxChng - ds[targetBar]) / maxChng * 100 : maxChng - ds[targetBar];
                                    if (stepChng >= leftThreshold - equalPriceFloat)
                                        break;
                                }
                                Lbar--;
                            }
                        }
                    }
                    if (haveSH == true)
                    {
                        lastSwgPrice = lastSwg == 1 ? ds[lastBar] : ds[lastBar];
                        int offset = 0;
                        if (returnOuterSwings == true && lastSwgHighBar > 0)
                        {
                            if (ds[lastSwgHighBar] < ds[targetBar])
                                offset = padHL;
                            else if (ds[lastSwgHighBar] > ds[targetBar])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padHL : 0;
                        this[targetBar - offset] = ds[targetBar - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = Math.Max(1, targetBar - offset - lastBar);
                            stepSH = (lastSwgPrice - ds[targetBar - offset]) / stepNum;
                            stepAccum = ds[targetBar - offset];
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSH;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                                this[i] = lastSwgPrice;
                        }
                        lastBar = targetBar - offset;
                        lastSwg = 1;
                        lastSwgHighBar = targetBar - offset;
                    }
                    if (haveSL == true)
                    {
                        lastSwgPrice = lastSwg == 1 ? ds[lastBar] : ds[lastBar];
                        int offset = 0;
                        if (returnOuterSwings == true && lastSwgLowBar > 0)
                        {
                            if (ds[lastSwgLowBar] > ds[targetBar])
                                offset = padLL;
                            else if (ds[lastSwgLowBar] < ds[targetBar])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padLL : 0;

                        this[targetBar - offset] = ds[targetBar - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = Math.Max(1, targetBar - offset - lastBar);
                            stepSL = (lastSwgPrice - ds[targetBar - offset]) / stepNum;
                            stepAccum = ds[targetBar - offset];
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSL;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                                this[i] = lastSwgPrice;
                        }
                        lastBar = targetBar - offset;
                        lastSwg = 0;
                        lastSwgLowBar = targetBar - offset;
                    }
                }
                if (bar == ds.Count - 1)
                {
                    stepBk++;
                    bar--;
                }
                if (rightBars - stepBk < 1)
                    break;
            }
            for (int i = ds.Count - 1; i > lastBar; i--) this[i] = this[lastBar];
        }

        public SwingHiLo(Bars bars, int leftBars, int rightBars, double equalPriceFloat, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries, string description)
            : base(bars, description)
        {
            _bars = bars;
            _dsH = bars.High;
            _dsL = bars.Low;
            _leftBars = leftBars;
            _rightBars = rightBars;
            _equalPriceFloat = equalPriceFloat;
            _returnLeftSwing = returnLeftSwing;
            _returnOuterSwings = returnOuterSwings;
            _steppedSeries = steppedSeries;
            this.FirstValidValue = leftBars + rightBars + 1;
            int lastBar = leftBars + rightBars + 1;
            int lastSwg = -1;
            int lastSwgHighBar = -1;
            int lastSwgLowBar = -1;
            int padL = 0;
            int inBtwn = 0;
            int stepNum = 0;
            int maxPadL = Math.Max(leftBars * 2, 100);
            double lastSwgPrice = double.NaN;
            double stepSH = double.NaN;
            double stepSL = double.NaN;
            double stepAccum = double.NaN;
            bool haveSL = false;
            bool haveSH = false;
            for (int bar = 0; bar < bars.Count; bar++)
            {
                if (bar < leftBars + rightBars + 1)
                {
                    this[bar] = bars.Close[bar];
                    lastBar = bar;
                }
                else
                {
                    haveSL = false;
                    haveSH = false;
                    for (int rbar = bar; rbar > bar - rightBars; rbar--)
                    {
                        if (bars.High[bar - rightBars] > bars.High[rbar])
                            haveSH = true;
                        else
                        {
                            haveSH = false;
                            break;
                        }
                    }
                    for (int rbar = bar; rbar > bar - rightBars; rbar--)
                    {
                        if (bars.Low[bar - rightBars] < bars.Low[rbar])
                            haveSL = true;
                        else
                        {
                            haveSL = false;
                            break;
                        }
                    }
                    if (haveSH == true)
                    {
                        padL = 0;
                        inBtwn = 0;
                        for (int Lbar = bar - rightBars - 1; Lbar >= bar - rightBars - leftBars - padL + inBtwn; Lbar--)
                        {
                            if (Math.Abs(bars.High[bar - rightBars] - bars.High[Lbar]) < equalPriceFloat)
                            {
                                if (Lbar - 1 > leftBars && padL + 1 < maxPadL)
                                {
                                    if (Lbar < bar - rightBars - 2 && bars.High[Lbar + 1] > bars.High[Lbar]) inBtwn++;
                                    padL = (bar - rightBars) - Lbar;
                                    continue;
                                }
                            }
                            if (bars.High[bar - rightBars] < bars.High[Lbar])
                            {
                                haveSH = false;
                                break;
                            }
                        }
                    }
                    if (haveSL == true)
                    {
                        padL = 0;
                        inBtwn = 0;
                        for (int Lbar = bar - rightBars - 1; Lbar >= bar - rightBars - leftBars - padL + inBtwn; Lbar--)
                        {
                            if (Math.Abs(bars.Low[bar - rightBars] - bars.Low[Lbar]) < equalPriceFloat)
                            {
                                if (Lbar - 1 > leftBars && padL + 1 < maxPadL)
                                {
                                    if (Lbar < bar - rightBars - 2 && bars.Low[Lbar + 1] > bars.Low[Lbar]) inBtwn++;
                                    padL = (bar - rightBars) - Lbar;
                                    continue;
                                }
                            }
                            if (bars.Low[bar - rightBars] > bars.Low[Lbar])
                            {
                                haveSL = false;
                                break;
                            }
                        }
                    }
                    if (haveSH == true)
                    {
                        lastSwgPrice = lastSwg == 1 ? bars.High[lastBar] : bars.Low[lastBar];
                        int offset = 0;
                        if (returnOuterSwings == true && lastSwgHighBar > 0)
                        {
                            if (bars.High[lastSwgHighBar] < bars.High[bar - rightBars])
                                offset = padL;
                            else if (bars.High[lastSwgHighBar] > bars.High[bar - rightBars])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padL : 0;
                        this[bar - rightBars - offset] = bars.High[bar - rightBars - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = bar - rightBars - offset - lastBar;
                            stepSH = (lastSwgPrice - bars.High[bar - rightBars - offset]) / stepNum;
                            stepAccum = bars.High[bar - rightBars - offset];
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSH;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                                this[i] = lastSwgPrice;
                        }
                        lastBar = bar - rightBars - offset;
                        lastSwg = 1;
                        lastSwgHighBar = bar - rightBars - offset;
                    }
                    if (haveSL == true)
                    {
                        lastSwgPrice = lastSwg == 1 ? bars.High[lastBar] : bars.Low[lastBar];
                        int offset = 0;
                        if (returnOuterSwings == true && lastSwgLowBar > 0)
                        {
                            if (bars.Low[lastSwgLowBar] > bars.Low[bar - rightBars])
                                offset = padL;
                            else if (bars.Low[lastSwgLowBar] < bars.Low[bar - rightBars])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padL : 0;
                        this[bar - rightBars - offset] = bars.Low[bar - rightBars - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = bar - rightBars - offset - lastBar;
                            stepSL = (lastSwgPrice - bars.Low[bar - rightBars - offset]) / stepNum;
                            stepAccum = bars.Low[bar - rightBars - offset];
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSL;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                            {
                                this[i] = lastSwgPrice;
                            }
                        }
                        lastBar = bar - rightBars - offset;
                        lastSwg = 0;
                        lastSwgLowBar = bar - rightBars - offset;
                    }
                    if (bar == bars.Count - 1)
                        for (int i = bar; i > lastBar; i--) this[i] = this[lastBar];
                }
            }
        }

        public SwingHiLo(Bars bars, int leftBars, double leftThreshold, int rightBars, double rightThreshold, double equalPriceFloat, bool percentMode, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries, string description)
            : base(bars, description)
        {
            _bars = bars;
            _dsH = bars.High;
            _dsL = bars.Low;
            _leftBars = leftBars;
            _rightBars = rightBars;
            _equalPriceFloat = equalPriceFloat;
            _returnLeftSwing = returnLeftSwing;
            _returnOuterSwings = returnOuterSwings;
            _steppedSeries = steppedSeries;
            _leftThreshold = leftThreshold;
            _rightThreshold = rightThreshold;
            _percentMode = percentMode;
            this.FirstValidValue = leftBars + rightBars + 1;
            int lastBar = leftBars + rightBars + 1;
            int lastSwg = -1;
            int lastSwgHighBar = -1;
            int lastSwgLowBar = -1;
            int padHL = 0;
            int padLL = 0;
            int inBtwn = 0;
            int stepNum = 0;
            int targetBar = 0;
            int stepBk = 0;
            int maxPadL = Math.Max(leftBars * 2, 100);
            double lastSwgPrice = double.NaN;
            double stepSH = double.NaN;
            double stepSL = double.NaN;
            double stepAccum = double.NaN;
            double maxChng = double.NaN;
            double minChng = double.NaN;
            double stepChng = double.NaN;
            bool haveSL = false;
            bool haveSH = false;
            bool requireLeftThreshold = leftThreshold <= 0 ? false : true;
            bool requireRightThreshold = rightThreshold <= 0 ? false : true;
            for (int bar = 0; bar < bars.Count; bar++)
            {
                if (bar < leftBars + rightBars + 1)
                {
                    this[bar] = bars.Close[bar];
                    lastBar = bar;
                }
                else
                {
                    haveSL = false;
                    haveSH = false;
                    maxChng = requireRightThreshold == false ? 999999 : 0;
                    minChng = requireLeftThreshold == false ? 0 : 999999;
                    targetBar = bar - rightBars + stepBk;
                    if (requireRightThreshold == true)
                    {
                        for (int rbar = targetBar + 1; rbar <= bar; rbar++)
                        {
                            if (bars.High[rbar] >= bars.High[targetBar])
                            {
                                haveSH = false;
                                break;
                            }
                            minChng = Math.Min(minChng, bars.High[rbar]);
                            stepChng = percentMode == true ? (bars.High[targetBar] - minChng) / bars.High[targetBar] * 100 : bars.High[targetBar] - minChng;
                            if (stepChng >= rightThreshold - equalPriceFloat)
                            {
                                haveSH = true;
                                break;
                            }
                        }
                    }
                    if (haveSH == false)
                    {
                        for (int rbar = bar - rightBars + 1; rbar <= bar; rbar++)
                        {
                            if (bars.High[bar - rightBars] <= bars.High[rbar])
                            {
                                haveSH = false;
                                break;
                            }
                            if (rbar == bar)
                                haveSH = true;
                        }
                    }
                    if (requireRightThreshold == true)
                    {
                        for (int rbar = targetBar + 1; rbar <= bar; rbar++)
                        {
                            if (bars.Low[rbar] <= bars.Low[targetBar])
                            {
                                haveSL = false;
                                break;
                            }
                            maxChng = Math.Max(maxChng, bars.Low[rbar]);
                            stepChng = percentMode == true ? (maxChng - bars.Low[targetBar]) / bars.Low[targetBar] * 100 : maxChng - bars.Low[targetBar];
                            if (stepChng >= rightThreshold - equalPriceFloat)
                            {
                                haveSL = true;
                                break;
                            }
                        }
                    }
                    if (haveSL == false)
                    {
                        for (int rbar = bar - rightBars + 1; rbar <= bar; rbar++)
                        {
                            if (bars.Low[bar - rightBars] >= bars.Low[rbar])
                            {
                                haveSL = false;
                                break;
                            }
                            if (rbar == bar)
                                haveSL = true;
                        }
                    }
                    if (haveSH == true)
                    {
                        padHL = 0;
                        inBtwn = 0;
                        minChng = requireLeftThreshold == false ? 0 : 999999;
                        if (leftBars != 0)
                        {
                            int Lbar = targetBar;
                            while (inBtwn < leftBars && Lbar >= targetBar - leftBars - padHL)
                            {
                                Lbar--;
                                if (bars.High[targetBar] < bars.High[Lbar])
                                {
                                    haveSH = false;
                                    break;
                                }
                                if (Math.Abs(bars.High[targetBar] - bars.High[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar - 1 > leftBars && padHL + 1 < maxPadL)
                                    {
                                        padHL = (targetBar) - Lbar;
                                        continue;
                                    }
                                }
                                else
                                {
                                    inBtwn++;
                                    if (requireLeftThreshold == true)
                                        minChng = Math.Min(minChng, bars.High[Lbar]);
                                }
                            }
                            if (requireLeftThreshold == true)
                            {
                                stepChng = percentMode == true ? (bars.High[targetBar] - minChng) / minChng * 100 : bars.High[targetBar] - minChng;
                                if (stepChng < leftThreshold - equalPriceFloat)
                                    haveSH = false;
                            }
                        }
                        else
                        {
                            int Lbar = targetBar - 1;
                            while (Lbar > 0)
                            {
                                if (bars.High[Lbar] > bars.High[targetBar] + equalPriceFloat)
                                {
                                    haveSH = false;
                                    break;
                                }
                                else if (Math.Abs(bars.High[targetBar] - bars.High[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar < targetBar - 2 && bars.High[Lbar + 1] > bars.High[Lbar]) inBtwn++;
                                    padHL = targetBar - Lbar;
                                }
                                else
                                {
                                    minChng = Math.Min(minChng, bars.High[Lbar]);
                                    stepChng = percentMode == true ? (bars.High[targetBar] - minChng) / minChng * 100 : bars.High[targetBar] - minChng;
                                    if (stepChng >= leftThreshold - equalPriceFloat)
                                        break;
                                }
                                Lbar--;
                            }
                        }
                    }
                    if (haveSL == true)
                    {
                        padLL = 0;
                        inBtwn = 0;
                        maxChng = requireLeftThreshold == false ? 999999 : 0;
                        if (leftBars != 0)
                        {
                            int Lbar = targetBar;
                            while (inBtwn < leftBars && Lbar >= targetBar - leftBars - padLL)
                            {
                                Lbar--;
                                if (bars.Low[targetBar] > bars.Low[Lbar])
                                {
                                    haveSL = false;
                                    break;
                                }
                                if (Math.Abs(bars.Low[targetBar] - bars.Low[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar - 1 > leftBars && padLL + 1 < maxPadL)
                                    {
                                        padLL = targetBar - Lbar;
                                        continue;
                                    }
                                }
                                else
                                {
                                    inBtwn++;
                                    if (requireLeftThreshold == true)
                                        maxChng = Math.Max(maxChng, bars.Low[Lbar]);
                                }
                            }
                            if (requireLeftThreshold == true)
                            {
                                stepChng = percentMode == true ? (maxChng - bars.Low[targetBar]) / maxChng * 100 : maxChng - bars.Low[targetBar];
                                if (stepChng < leftThreshold - equalPriceFloat)
                                    haveSL = false;
                            }
                        }
                        else
                        {
                            int Lbar = targetBar - 1;
                            while (Lbar > 0)
                            {
                                if (bars.Low[targetBar] > bars.Low[Lbar] + equalPriceFloat)
                                {
                                    haveSL = false;
                                    break;
                                }
                                else if (Math.Abs(bars.Low[targetBar] - bars.Low[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar < targetBar - 2 && bars.Low[Lbar + 1] > bars.Low[Lbar]) inBtwn++;
                                    padLL = (targetBar) - Lbar;
                                }
                                else
                                {
                                    maxChng = Math.Max(maxChng, bars.Low[Lbar]);
                                    stepChng = percentMode == true ? (maxChng - bars.Low[targetBar]) / maxChng * 100 : maxChng - bars.Low[targetBar];
                                    if (stepChng >= leftThreshold - equalPriceFloat)
                                        break;
                                }
                                Lbar--;
                            }
                        }
                    }
                    if (haveSH == true)
                    {
                        lastSwgPrice = lastSwg == 1 ? bars.High[lastBar] : bars.Low[lastBar];
                        int offset = 0;
                        if (returnOuterSwings == true && lastSwgHighBar > 0)
                        {
                            if (bars.High[lastSwgHighBar] < bars.High[targetBar])
                                offset = padHL;
                            else if (bars.High[lastSwgHighBar] > bars.High[targetBar])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padHL : 0;
                        this[targetBar - offset] = bars.High[targetBar - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = Math.Max(1, targetBar - offset - lastBar);
                            stepSH = (lastSwgPrice - bars.High[targetBar - offset]) / stepNum;
                            stepAccum = bars.High[targetBar - offset];
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSH;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                                this[i] = lastSwgPrice;
                        }
                        lastBar = targetBar - offset;
                        lastSwg = 1;
                        lastSwgHighBar = targetBar - offset;
                    }
                    if (haveSL == true)
                    {
                        lastSwgPrice = lastSwg == 1 ? bars.High[lastBar] : bars.Low[lastBar];
                        int offset = 0;
                        if (returnOuterSwings == true && lastSwgLowBar > 0)
                        {
                            if (bars.Low[lastSwgLowBar] > bars.Low[targetBar])
                                offset = padLL;
                            else if (bars.Low[lastSwgLowBar] < bars.Low[targetBar])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padLL : 0;

                        this[targetBar - offset] = bars.Low[targetBar - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = Math.Max(1, targetBar - offset - lastBar);
                            stepSL = (lastSwgPrice - bars.Low[targetBar - offset]) / stepNum;
                            stepAccum = bars.Low[targetBar - offset];
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSL;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                                this[i] = lastSwgPrice;
                        }
                        lastBar = targetBar - offset;
                        lastSwg = 0;
                        lastSwgLowBar = targetBar - offset;
                    }
                }
                if (bar == bars.Count - 1)
                {
                    stepBk++;
                    bar--;
                }
                if (rightBars - stepBk < 1)
                    break;
            }
            for (int i = bars.Count - 1; i > lastBar; i--) this[i] = this[lastBar];
        }

    }

    public class SwingHiLoHelper : IndicatorHelper
    {
        private static string[] _swingParameterNames = { "bars", "LeftBars", "LeftReversalAmount", "RightBars", "RightReversalAmount", "EqualPriceThreshold", "PercentMode", "SetLeftSwings", "SetOuterSwings", "SetSteppedSeries" };
        private static object[] _swingParameterDefaults = { BarDataType.Bars, new RangeBoundInt32(13, 0, 30), new RangeBoundDouble(3.5, 0, 100.0), new RangeBoundInt32(5, 1, 30), new RangeBoundDouble(1.5, 0, 100.0), new RangeBoundDouble(0.000001, 0.0000001, 1.0), new bool(), new bool(), new bool(), new bool() };

        public override IList<object> ParameterDefaultValues
        {
            get { return _swingParameterDefaults; }
        }

        public override IList<string> ParameterDescriptions
        {
            get { return _swingParameterNames; }
        }

        public override string Description
        {
            get
            {
                return @"Controls both high & low swings, with variable left and right side number of bars and "
                + "price or percent as reversal amount." + Environment.NewLine + Environment.NewLine
                + "Each side of a swing can use either/or or both bars and price to define a swing. When both bar quota "
                + "and price are used the early(left) history will require a match from both parameters, whereas the "
                + "recent(right) history will be matched when either parameter is fulfilled." + Environment.NewLine + Environment.NewLine
                + "To drop price-percent filtering set their parameter(s) to zero. Alternatively, to drop the bars quota "
                + "set the left side parameter to zero, but on the right side set its parameter to a minimum of one." + Environment.NewLine + Environment.NewLine
                + "For cases of multi-bar swings the default bar returned is the right side. This can be overridden with "
                + "the SetLeftSwings parameter. Alternatively the parameter SetOutSideSwings will force the outside bar "
                + "of multi-bar swings to be returned (in cases of consecutively higher(lower) swings. "
                + "Set the parameter EqualPriceThreshold to the maximum tolerable value to return equal prices." + Environment.NewLine + Environment.NewLine
                + "The SetSteppedSeries parameter will force only the last swings values to be plotted box style. The "
                + "default plot increments the series values between swings." + Environment.NewLine + Environment.NewLine
                + "The parameter types are; bars-Bars, Left&RightBars-int, L&RPrice-double, EqualPriceThreshold-double, "
                + "all others-bool. The main method parameters are ordered;" + Environment.NewLine
                + " SwingHiLo.Series( bars, LeftBars, LeftReversalAmount, RightBars, RightReversalAmount, EqualPriceThreshold, "
                + " PercentMode, SetLeftSwings, SetOuterSwings, SetSteppedSeries )" + Environment.NewLine + Environment.NewLine
                + "Alternative overload :-" + Environment.NewLine
                + " new SwingHiLo( bars, LeftBars, RightBars, EqualPriceThreshold, SetLeftSwings, SetOuterSwings, SetSteppedSeries, \"mySwingHighLow\" )" + Environment.NewLine + Environment.NewLine
                + "While the series 'peaks', it can be avoided if used inconjunction with associated methods IsSwingHi() and IsSwingLo()." + Environment.NewLine
                + "The methods are bool with 3 additional parameters to their series counterparts:- " + Environment.NewLine
                + "- 'farBack' controls how many past bars(from 'bar') to search." + Environment.NewLine
                + "- 'occur' controls which of the 1st, 2nd, 3rd and so on, most recent swings to return" + Environment.NewLine
                + "- 'out swingLowBar' assigns an int variable with the bar number of 'occur'." + Environment.NewLine + Environment.NewLine
                + " IsSwingHi( bar, dataSeries, LeftBars, LeftReversalAmount, RightBars, RightReversalAmount, farback, occur, EqualPriceThreshold, PercentMode, SetLeftSwings, out swingHighBar ) " + Environment.NewLine
                + " IsSwingHi( bar, dataSeries, LeftBars, RightBars, farback, occur, EqualPriceThreshold, SetLeftSwings, out swingHighBar ) " + Environment.NewLine
                + " IsSwingLo( bar, dataSeries, LeftBars, LeftReversalAmount, RightBars, RightReversalAmount, farback, occur, EqualPriceThreshold, PercentMode, SetLeftSwings, out swingLowBar ) " + Environment.NewLine
                + " IsSwingLo( bar, dataSeries, LeftBars, RightBars, farback, occur, EqualPriceThreshold, SetLeftSwings, out swingLowBar ) " + Environment.NewLine;
            }
        }

        public override Type IndicatorType
        {
            get { return typeof(SwingHiLo); }
        }

        public override Color DefaultColor
        {
            get { return Color.LightGray; }
        }

        public override int DefaultWidth
        {
            get { return 2; }
        }

        public override bool IsOscillator
        {
            get { return false; }
        }
    }

    public class SwingHi : DataSeries
    {
        private DataSeries _dsH;
        private int _leftBars;
        private double _leftThreshold;
        private int _rightBars;
        private double _rightThreshold;
        private double _equalPriceFloat;
        private bool _percentMode;
        private bool _returnLeftSwing;
        private bool _returnOuterSwings;
        private bool _steppedSeries;

        public static SwingHi Series(DataSeries ds, int leftBars, double leftThreshold, int rightBars, double rightThreshold, double equalPriceFloat, bool percentMode, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries)
        {
            string description = string.Concat(new object[] { "SwingHi(", ds.Description, ",", leftBars, ",", leftThreshold, ",", rightBars, ",", rightThreshold, ",", percentMode, ",", equalPriceFloat, ",", returnLeftSwing, ",", returnOuterSwings, ",", steppedSeries, ")" });
            if (ds.Cache.ContainsKey(description))
                return (SwingHi)ds.Cache[description];
            SwingHi swingHi = new SwingHi(ds, leftBars, leftThreshold, rightBars, rightThreshold, equalPriceFloat, percentMode, returnLeftSwing, returnOuterSwings, steppedSeries, description);
            ds.Cache[description] = swingHi;
            return swingHi;
        }

        public SwingHi(DataSeries ds, int leftBars, int rightBars, double equalPriceFloat, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries, string description)
            : base(ds, description)
        {
            _dsH = ds;
            _leftBars = leftBars;
            _rightBars = rightBars;
            _equalPriceFloat = equalPriceFloat;
            _returnLeftSwing = returnLeftSwing;
            _returnOuterSwings = returnOuterSwings;
            _steppedSeries = steppedSeries;
            this.FirstValidValue = leftBars + rightBars + 1;
            int lastBar = leftBars + rightBars + 1;
            int padL = 0;
            int stepNum = 0;
            int maxPadL = Math.Max(leftBars * 2, 100);
            double stepSH = double.NaN;
            double stepAccum = double.NaN;
            bool haveSH = false;
            for (int bar = 0; bar < ds.Count; bar++)
            {
                if (bar < leftBars + rightBars + 1)
                {
                    this[bar] = ds[bar];
                    lastBar = bar;
                }
                else
                {
                    haveSH = false;
                    for (int rbar = bar; rbar > bar - rightBars; rbar--)
                    {
                        if (ds[bar - rightBars] > ds[rbar])
                            haveSH = true;
                        else
                        {
                            haveSH = false;
                            break;
                        }
                    }
                    if (haveSH == true)
                    {
                        padL = 0;
                        int inBtwn = 0;
                        for (int Lbar = bar - rightBars - 1; Lbar >= bar - rightBars - leftBars - padL + inBtwn; Lbar--)
                        {
                            if (Math.Abs(ds[bar - rightBars] - ds[Lbar]) < equalPriceFloat)
                            {
                                if (Lbar - 1 > leftBars && padL + 1 < maxPadL)
                                {
                                    if (Lbar < bar - rightBars - 2 && ds[Lbar + 1] > ds[Lbar]) inBtwn++;
                                    padL = (bar - rightBars) - Lbar;
                                    continue;
                                }
                            }
                            if (ds[bar - rightBars] < ds[Lbar])
                            {
                                haveSH = false;
                                break;
                            }
                        }
                    }
                    if (haveSH == true)
                    {
                        int offset = 0;
                        if (returnOuterSwings == true)
                        {
                            if (ds[lastBar] < ds[bar - rightBars])
                                offset = padL;
                            else if (ds[lastBar] > ds[bar - rightBars])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padL : 0;
                        this[bar - rightBars - offset] = ds[bar - rightBars - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = bar - rightBars - offset - lastBar;
                            stepSH = (ds[lastBar] - ds[bar - rightBars - offset]) / stepNum;
                            stepAccum = ds[bar - rightBars - offset];
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSH;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                                this[i] = ds[lastBar];
                        }
                        lastBar = bar - rightBars - offset;
                    }
                    if (bar == ds.Count - 1)
                        for (int i = bar; i > lastBar; i--) this[i] = this[lastBar];
                }
            }


        }

        public SwingHi(DataSeries ds, int leftBars, double leftThreshold, int rightBars, double rightThreshold, double equalPriceFloat, bool percentMode, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries, string description)
            : base(ds, description)
        {
            _dsH = ds;
            _leftBars = leftBars;
            _rightBars = rightBars;
            _equalPriceFloat = equalPriceFloat;
            _returnLeftSwing = returnLeftSwing;
            _returnOuterSwings = returnOuterSwings;
            _steppedSeries = steppedSeries;
            _leftThreshold = leftThreshold;
            _rightThreshold = rightThreshold;
            _percentMode = percentMode;
            this.FirstValidValue = leftBars + rightBars + 1;
            int lastBar = leftBars + rightBars + 1;
            int padL = 0;
            int stepNum = 0;
            int targetBar = 0;
            int stepBk = 0;
            int maxPadL = Math.Max(leftBars * 2, 100);
            double stepSH = double.NaN;
            double stepAccum = double.NaN;
            double minChng = double.NaN;
            double stepChng = double.NaN;
            bool haveSH = false;
            bool requireLeftThreshold = leftThreshold <= 0 ? false : true;
            bool requireRightThreshold = rightThreshold <= 0 ? false : true;
            for (int bar = 0; bar < ds.Count; bar++)
            {
                if (bar < leftBars + rightBars + 1)
                {
                    this[bar] = ds[bar];
                    lastBar = bar;
                }
                else
                {
                    targetBar = bar - rightBars + stepBk;
                    haveSH = false;
                    minChng = requireRightThreshold == false ? 0 : 999999;
                    if (requireRightThreshold == true)
                    {
                        for (int rbar = targetBar + 1; rbar <= bar; rbar++)
                        {
                            if (ds[rbar] >= ds[targetBar])
                            {
                                haveSH = false;
                                break;
                            }
                            minChng = Math.Min(minChng, ds[rbar]);
                            stepChng = percentMode == true ? (ds[targetBar] - minChng) / ds[targetBar] * 100 : ds[targetBar] - minChng;
                            if (stepChng >= rightThreshold - equalPriceFloat)
                            {
                                haveSH = true;
                                break;
                            }
                        }
                    }
                    if (haveSH == false)
                    {
                        for (int rbar = bar - rightBars + 1; rbar <= bar; rbar++)
                        {
                            if (ds[bar - rightBars] <= ds[rbar])
                            {
                                haveSH = false;
                                break;
                            }
                            if (rbar == bar)
                                haveSH = true;
                        }
                    }
                    if (haveSH == true)
                    {
                        padL = 0;
                        int inBtwn = 0;
                        minChng = requireLeftThreshold == false ? 0 : 999999;
                        if (leftBars != 0)
                        {
                            int Lbar = targetBar;
                            while (inBtwn < leftBars && Lbar >= targetBar - leftBars - padL)
                            {
                                Lbar--;
                                if (ds[targetBar] < ds[Lbar])
                                {
                                    haveSH = false;
                                    break;
                                }
                                if (Math.Abs(ds[targetBar] - ds[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar - 1 > leftBars && padL + 1 < maxPadL)
                                    {
                                        padL = (targetBar) - Lbar;
                                        continue;
                                    }
                                }
                                else
                                {
                                    inBtwn++;
                                    if (requireLeftThreshold == true)
                                        minChng = Math.Min(minChng, ds[Lbar]);
                                }
                            }
                            if (requireLeftThreshold == true)
                            {
                                stepChng = percentMode == true ? (ds[targetBar] - minChng) / minChng * 100 : (ds[targetBar] - minChng);
                                if (stepChng < leftThreshold - equalPriceFloat)
                                    haveSH = false;
                            }
                        }
                        else
                        {
                            int Lbar = targetBar - 1;
                            while (Lbar > 0)
                            {
                                if (ds[Lbar] > ds[targetBar] + equalPriceFloat)
                                {
                                    haveSH = false;
                                    break;
                                }
                                else if (Math.Abs(ds[targetBar] - ds[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar < targetBar - 2 && ds[Lbar + 1] > ds[Lbar]) inBtwn++;
                                    padL = (targetBar) - Lbar;
                                }
                                else
                                {
                                    minChng = Math.Min(minChng, ds[Lbar]);
                                    stepChng = percentMode == true ? (ds[targetBar] - minChng) / minChng * 100 : ds[targetBar] - minChng;
                                    if (stepChng >= leftThreshold - equalPriceFloat)
                                        break;
                                }
                                Lbar--;
                            }
                        }
                    }
                    if (haveSH == true)
                    {
                        int offset = 0;
                        if (returnOuterSwings == true)
                        {
                            if (ds[lastBar] < ds[targetBar])
                                offset = padL;
                            else if (ds[lastBar] > ds[targetBar])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padL : 0;
                        this[targetBar - offset] = ds[targetBar - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = Math.Max(1, targetBar - offset - lastBar);
                            stepSH = (ds[targetBar - offset] - ds[lastBar]) / stepNum;
                            stepAccum = ds[targetBar - offset];
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum -= stepSH;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                                this[i] = ds[lastBar];
                        }
                        lastBar = targetBar - offset;
                    }
                    if (bar == ds.Count - 1)
                    {
                        stepBk++;
                        bar--;
                    }
                    if (rightBars - stepBk < 1)
                        break;
                }
            }
            for (int i = ds.Count - 1; i > lastBar; i--) this[i] = this[lastBar];
        }
    }

    public class SwingHiHelper : IndicatorHelper
    {
        private static string[] _swingParameterNames = { "dataSeries", "LeftBars", "LeftReversalAmount", "RightBars", "RightReversalAmount", "EqualPriceThreshold", "PercentMode", "SetLeftSwings", "SetOuterSwings", "SetSteppedSeries" };
        private static object[] _swingParameterDefaults = { CoreDataSeries.High, new RangeBoundInt32(13, 0, 30), new RangeBoundDouble(3.5, 0, 100.0), new RangeBoundInt32(5, 1, 30), new RangeBoundDouble(1.5, 0, 100.0), new RangeBoundDouble(0.000001, 0.0000001, 1.0), new bool(), new bool(), new bool(), new bool() };

        public override IList<object> ParameterDefaultValues
        {
            get { return _swingParameterDefaults; }
        }

        public override IList<string> ParameterDescriptions
        {
            get { return _swingParameterNames; }
        }

        public override string Description
        {
            get
            {
                return @"Control swings with variable left and right side number of bars and "
                + "price or percent as reversal amount." + Environment.NewLine + Environment.NewLine
                + "Each side of a swing can use either/or or both bars and price to define a swing. When both bar quota "
                + "and price are used the early(left) history will require a match from both parameters, whereas the "
                + "recent(right) history will be matched when either parameter is fulfilled." + Environment.NewLine + Environment.NewLine
                + "To drop price-percent filtering set their parameter(s) to zero. Alternatively, to drop the bars quota "
                + "set the left side parameter to zero, but on the right side set its parameter to a minimum of one." + Environment.NewLine + Environment.NewLine
                + "For cases of multi-bar swings the default bar returned is the right side. This can be overridden with "
                + "the SetLeftSwings parameter. Alternatively the parameter SetOutSideSwings will force the outside bar "
                + "of multi-bar swings to be returned (in cases of consecutively higher swings). "
                + "Set the parameter EqualPriceThreshold to the maximum tolerable value to return equal prices." + Environment.NewLine + Environment.NewLine
                + "The SetSteppedSeries parameter will force only the last swings values to be plotted box style. The "
                + "default plot increments the series values between swings." + Environment.NewLine + Environment.NewLine
                + "The parameter types are; bars-Bars, Left&RightBars-int, L&RPrice-double, EqualPriceThreshold-double, "
                + "all others-bool. The main method parameters are ordered;" + Environment.NewLine
                + " SwingHi.Series( dataSeries, LeftBars, LeftReversalAmount, RightBars, RightReversalAmount, EqualPriceThreshold, "
                + " PercentMode, SetLeftSwings, SetOuterSwings, SetSteppedSeries )" + Environment.NewLine + Environment.NewLine
                + "Alternative overload :-" + Environment.NewLine
                + " new SwingHi( dataSeries, LeftBars, RightBars, EqualPriceThreshold, SetLeftSwings, SetOuterSwings, SetSteppedSeries, \"mySwingHigh\" )" + Environment.NewLine + Environment.NewLine
                + "While the series 'peaks', it can be avoided if used inconjunction with associated methods IsSwingHi() and IsSwingLo()." + Environment.NewLine
                + "The methods are bool with 3 additional parameters to their series counterparts:- " + Environment.NewLine
                + "- 'farBack' controls how many past bars(from 'bar') to search." + Environment.NewLine
                + "- 'occur' controls which of the 1st, 2nd, 3rd and so on, most recent swings to return" + Environment.NewLine
                + "- 'out swingLowBar' assigns an int variable with the bar number of 'occur'." + Environment.NewLine + Environment.NewLine
                + " IsSwingHi( bar, dataSeries, LeftBars, LeftReversalAmount, RightBars, RightReversalAmount, farback, occur, EqualPriceThreshold, PercentMode, SetLeftSwings, out swingHighBar ) " + Environment.NewLine
                + " IsSwingHi( bar, dataSeries, LeftBars, RightBars, farback, occur, EqualPriceThreshold, SetLeftSwings, out swingHighBar ) " + Environment.NewLine;
            }
        }

        public override Type IndicatorType
        {
            get { return typeof(SwingHi); }
        }

        public override Color DefaultColor
        {
            get { return Color.Green; }
        }

        public override int DefaultWidth
        {
            get { return 1; }
        }

        public override bool IsOscillator
        {
            get { return false; }
        }
    }

    public class SwingLo : DataSeries
    {
        private DataSeries _dsL;
        private int _leftBars;
        private double _leftThreshold;
        private int _rightBars;
        private double _rightThreshold;
        private double _equalPriceFloat;
        private bool _percentMode;
        private bool _returnLeftSwing;
        private bool _returnOuterSwings;
        private bool _steppedSeries;

        public static SwingLo Series(DataSeries ds, int leftBars, double leftThreshold, int rightBars, double rightThreshold, double equalPriceFloat, bool percentMode, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries)
        {
            string description = string.Concat(new object[] { "SwingLo(", ds.Description, ",", leftBars, ",", leftThreshold, ",", rightBars, ",", rightThreshold, ",", percentMode, ",", equalPriceFloat, ",", returnLeftSwing, ",", returnOuterSwings, ",", steppedSeries, ")" });
            if (ds.Cache.ContainsKey(description))
                return (SwingLo)ds.Cache[description];
            SwingLo swingLo = new SwingLo(ds, leftBars, leftThreshold, rightBars, rightThreshold, equalPriceFloat, percentMode, returnLeftSwing, returnOuterSwings, steppedSeries, description);
            ds.Cache[description] = swingLo;
            return swingLo;
        }

        public SwingLo(DataSeries ds, int leftBars, int rightBars, double equalPriceFloat, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries, string description)
            : base(ds, description)
        {
            _dsL = ds;
            _leftBars = leftBars;
            _rightBars = rightBars;
            _equalPriceFloat = equalPriceFloat;
            _returnLeftSwing = returnLeftSwing;
            _returnOuterSwings = returnOuterSwings;
            _steppedSeries = steppedSeries;
            this.FirstValidValue = leftBars + rightBars + 1;
            int lastBar = leftBars + rightBars + 1;
            int padL = 0;
            int stepNum = 0;
            int maxPadL = Math.Max(leftBars * 2, 100);
            double stepSL = double.NaN;
            double stepAccum = double.NaN;
            bool haveSL = false;
            for (int bar = 0; bar < ds.Count; bar++)
            {
                if (bar < leftBars + rightBars + 1)
                {
                    this[bar] = ds[bar];
                    lastBar = bar;
                }
                else
                {
                    haveSL = false;
                    for (int rbar = bar; rbar > bar - rightBars; rbar--)
                    {
                        if (ds[bar - rightBars] < ds[rbar])
                            haveSL = true;
                        else
                        {
                            haveSL = false;
                            break;
                        }
                    }
                    if (haveSL == true)
                    {
                        padL = 0;
                        int inBtwn = 0;
                        for (int Lbar = bar - rightBars - 1; Lbar >= bar - rightBars - leftBars - padL + inBtwn; Lbar--)
                        {
                            if (Math.Abs(ds[bar - rightBars] - ds[Lbar]) < equalPriceFloat)
                            {
                                if (Lbar - 1 > leftBars && padL + 1 < maxPadL)
                                {
                                    if (Lbar < bar - rightBars - 2 && ds[Lbar + 1] > ds[Lbar]) inBtwn++;
                                    padL = (bar - rightBars) - Lbar;
                                    continue;
                                }
                            }
                            if (ds[bar - rightBars] > ds[Lbar])
                            {
                                haveSL = false;
                                break;
                            }
                        }
                    }
                    if (haveSL == true)
                    {
                        int offset = 0;
                        if (returnOuterSwings == true)
                        {
                            if (ds[lastBar] > ds[bar - rightBars])
                                offset = padL;
                            else if (ds[lastBar] < ds[bar - rightBars])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padL : 0;
                        this[bar - rightBars - offset] = ds[bar - rightBars - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = bar - rightBars - offset - lastBar;
                            stepSL = (ds[lastBar] - ds[bar - rightBars - offset]) / stepNum;
                            stepAccum = ds[bar - rightBars - offset];
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSL;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = bar - rightBars - 1 - offset; i > lastBar; i--)
                                this[i] = ds[lastBar];
                        }
                        lastBar = bar - rightBars - offset;
                    }
                    if (bar == ds.Count - 1)
                        for (int i = bar; i > lastBar; i--) this[i] = this[lastBar];
                }
            }

        }

        public SwingLo(DataSeries ds, int leftBars, double leftThreshold, int rightBars, double rightThreshold, double equalPriceFloat, bool percentMode, bool returnLeftSwing, bool returnOuterSwings, bool steppedSeries, string description)
            : base(ds, description)
        {
            _dsL = ds;
            _leftBars = leftBars;
            _rightBars = rightBars;
            _equalPriceFloat = equalPriceFloat;
            _returnLeftSwing = returnLeftSwing;
            _returnOuterSwings = returnOuterSwings;
            _steppedSeries = steppedSeries;
            _leftThreshold = leftThreshold;
            _rightThreshold = rightThreshold;
            _percentMode = percentMode;
            this.FirstValidValue = leftBars + rightBars + 1;
            int lastBar = leftBars + rightBars + 1;
            int padL = 0;
            int stepNum = 0;
            int targetBar = 0;
            int stepBk = 0;
            int maxPadL = Math.Max(leftBars * 2, 100);
            double stepSL = double.NaN;
            double stepAccum = double.NaN;
            double maxChng = double.NaN;
            double stepChng = double.NaN;
            bool haveSL = false;
            bool requireLeftThreshold = leftThreshold <= 0 ? false : true;
            bool requireRightThreshold = rightThreshold <= 0 ? false : true;
            for (int bar = 0; bar < ds.Count; bar++)
            {
                if (bar < leftBars + rightBars + 1)
                {
                    this[bar] = ds[bar];
                    lastBar = bar;
                }
                else
                {
                    targetBar = bar - rightBars + stepBk;
                    haveSL = false;
                    maxChng = requireRightThreshold == false ? 999999 : 0;
                    if (requireRightThreshold == true)
                    {
                        for (int rbar = targetBar + 1; rbar <= bar; rbar++)
                        {
                            if (ds[rbar] <= ds[targetBar])
                            {
                                haveSL = false;
                                break;
                            }
                            maxChng = Math.Max(maxChng, ds[rbar]);
                            stepChng = percentMode == true ? (maxChng - ds[targetBar]) / ds[targetBar] * 100 : maxChng - ds[targetBar];
                            if (stepChng >= rightThreshold - equalPriceFloat)
                            {
                                haveSL = true;
                                break;
                            }
                        }
                    }
                    if (haveSL == false)
                    {
                        for (int rbar = bar - rightBars + 1; rbar <= bar; rbar++)
                        {
                            if (ds[bar - rightBars] >= ds[rbar])
                            {
                                haveSL = false;
                                break;
                            }
                            if (rbar == bar)
                                haveSL = true;
                        }
                    }
                    if (haveSL == true)
                    {
                        padL = 0;
                        int inBtwn = 0;
                        maxChng = requireLeftThreshold == false ? 999999 : 0;
                        if (leftBars != 0)
                        {
                            int Lbar = targetBar;
                            while (inBtwn < leftBars && Lbar >= targetBar - leftBars - padL)
                            {
                                Lbar--;
                                if (ds[targetBar] > ds[Lbar])
                                {
                                    haveSL = false;
                                    break;
                                }
                                if (Math.Abs(ds[targetBar] - ds[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar - 1 > leftBars && padL + 1 < maxPadL)
                                    {
                                        padL = (targetBar) - Lbar;
                                        continue;
                                    }
                                }
                                else
                                {
                                    inBtwn++;
                                    if (requireLeftThreshold == true)
                                        maxChng = Math.Max(maxChng, ds[Lbar]);
                                }
                            }
                            if (requireLeftThreshold == true)
                            {
                                stepChng = percentMode == true ? (maxChng - ds[targetBar]) / maxChng * 100 : maxChng - ds[targetBar];
                                if (stepChng < leftThreshold - equalPriceFloat)
                                    haveSL = false;
                            }
                        }
                        else
                        {
                            int Lbar = targetBar - 1;
                            while (Lbar > 0)
                            {
                                if (ds[targetBar] > ds[Lbar] + equalPriceFloat)
                                {
                                    haveSL = false;
                                    break;
                                }
                                else if (Math.Abs(ds[targetBar] - ds[Lbar]) < equalPriceFloat)
                                {
                                    if (Lbar < targetBar - 2 && ds[Lbar + 1] > ds[Lbar]) inBtwn++;
                                    padL = (targetBar) - Lbar;
                                }
                                else
                                {
                                    maxChng = Math.Max(maxChng, ds[Lbar]);
                                    stepChng = percentMode == true ? (maxChng - ds[targetBar]) / maxChng * 100 : maxChng - ds[targetBar];
                                    if (stepChng >= leftThreshold - equalPriceFloat)
                                        break;
                                }
                                Lbar--;
                            }
                        }
                    }
                    if (haveSL == true)
                    {
                        int offset = 0;
                        if (returnOuterSwings == true)
                        {
                            if (ds[lastBar] > ds[targetBar])
                                offset = padL;
                            else if (ds[lastBar] < ds[targetBar])
                                offset = 0;
                        }
                        else
                            offset = returnLeftSwing == true ? padL : 0;
                        this[targetBar - offset] = ds[targetBar - offset];
                        if (steppedSeries == false)
                        {
                            stepNum = Math.Max(1, targetBar - offset - lastBar);
                            stepSL = (ds[lastBar] - ds[targetBar - offset]) / stepNum;
                            stepAccum = ds[targetBar - offset];
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                            {
                                stepAccum += stepSL;
                                this[i] = stepAccum;
                            }
                        }
                        else
                        {
                            for (int i = targetBar - 1 - offset; i > lastBar; i--)
                                this[i] = ds[lastBar];
                        }
                        lastBar = targetBar - offset;
                    }
                    if (bar == ds.Count - 1)
                    {
                        stepBk++;
                        bar--;
                    }
                    if (rightBars - stepBk < 1) break;
                }
            }
            for (int i = ds.Count - 1; i > lastBar; i--) this[i] = this[lastBar];
        }

    }

    public class SwingLoHelper : IndicatorHelper
    {
        private static string[] _swingParameterNames = { "dataSeries", "LeftBars", "LeftReversalAmount", "RightBars", "RightReversalAmount", "EqualPriceThreshold", "PercentMode", "SetLeftSwings", "SetOuterSwings", "SetSteppedSeries" };
        private static object[] _swingParameterDefaults = { CoreDataSeries.Low, new RangeBoundInt32(13, 0, 30), new RangeBoundDouble(3.5, 0, 100.0), new RangeBoundInt32(5, 1, 30), new RangeBoundDouble(1.5, 0, 100.0), new RangeBoundDouble(0.000001, 0.0000001, 1.0), new bool(), new bool(), new bool(), new bool() };

        public override IList<object> ParameterDefaultValues
        {
            get { return _swingParameterDefaults; }
        }

        public override IList<string> ParameterDescriptions
        {
            get { return _swingParameterNames; }
        }

        public override string Description
        {
            get
            {
                return @"Control swings with variable left and right side number of bars and "
                + "price or percent as reversal amount." + Environment.NewLine + Environment.NewLine
                + "Each side of a swing can use either/or or both bars and price to define a swing. When both bar quota "
                + "and price are used the early(left) history will require a match from both parameters, whereas the "
                + "recent(right) history will be matched when either parameter is fulfilled." + Environment.NewLine + Environment.NewLine
                + "To drop price-percent filtering set their parameter(s) to zero. Alternatively, to drop the bars quota "
                + "set the left side parameter to zero, but on the right side set its parameter to a minimum of one." + Environment.NewLine + Environment.NewLine
                + "For cases of multi-bar swings the default bar returned is the right side. This can be overridden with "
                + "the SetLeftSwings parameter. Alternatively the parameter SetOutSideSwings will force the outside bar "
                + "of multi-bar swings to be returned (in cases of consecutively lower swings). "
                + "Set the parameter EqualPriceThreshold to the maximum tolerable value to return equal prices." + Environment.NewLine + Environment.NewLine
                + "The SetSteppedSeries parameter will force only the last swings values to be plotted box style. The "
                + "default plot increments the series values between swings." + Environment.NewLine + Environment.NewLine
                + "The parameter types are; bars-Bars, Left&RightBars-int, L&RPrice-double, EqualPriceThreshold-double, "
                + "all others-bool. The main method parameters are ordered;" + Environment.NewLine
                + " SwingLo.Series( dataSeries, LeftBars, LeftReversalAmount, RightBars, RightReversalAmount, EqualPriceThreshold, "
                + " PercentMode, SetLeftSwings, SetOuterSwings, SetSteppedSeries )" + Environment.NewLine + Environment.NewLine
                + "Alternative overload :-" + Environment.NewLine
                + " new SwingLo( dataSeries, LeftBars, RightBars, EqualPriceThreshold, SetLeftSwings, SetOuterSwings, SetSteppedSeries, \"mySwingLow\" )" + Environment.NewLine + Environment.NewLine
                + "While the series 'peaks', it can be avoided if used inconjunction with associated methods IsSwingHi() and IsSwingLo()." + Environment.NewLine
                + "The methods are bool with 3 additional parameters to their series counterparts:- " + Environment.NewLine
                + "- 'farBack' controls how many past bars(from 'bar') to search." + Environment.NewLine
                + "- 'occur' controls which of the 1st, 2nd, 3rd and so on, most recent swings to return" + Environment.NewLine
                + "- 'out swingLowBar' assigns an int variable with the bar number of 'occur'." + Environment.NewLine + Environment.NewLine
                + "IsSwingLo( bar, dataSeries, LeftBars, LeftReversalAmount, RightBars, RightReversalAmount, farback, occur, EqualPriceThreshold, PercentMode, SetLeftSwings, out swingLowBar ) " + Environment.NewLine
                + "IsSwingLo( bar, dataSeries, LeftBars, RightBars, farback, occur, EqualPriceThreshold, SetLeftSwings, out swingLowBar ) " + Environment.NewLine;
            }
        }

        public override Type IndicatorType
        {
            get { return typeof(SwingLo); }
        }

        public override Color DefaultColor
        {
            get { return Color.Red; }
        }

        public override int DefaultWidth
        {
            get { return 1; }
        }

        public override bool IsOscillator
        {
            get { return false; }
        }
    }
}