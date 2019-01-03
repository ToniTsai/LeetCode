using System;
using System.Reflection;

namespace Community.Indicators
{
    class Helper
    {
        static internal void CompatibilityCheck()
        {
            if (!Helper.Compatibility())
            {
                throw new TypeInitializationException("WealthLab.Indicators.Community", new Exception());
                //Environment.Exit(666);
            }
        }

        static internal bool Compatibility()
        {
            var v = Assembly.GetEntryAssembly().GetName().Version;
            var maj = v.Major;
            var min = v.Minor;
            var build = v.Build;
            bool supportedVer = ((maj > 6) || (maj == 6 && min >= 9 && build >= 15));
            if (!supportedVer)
            {
                return false;
            }

            return true;
        }
    }    
}
