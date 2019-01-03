using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using System.Reflection;

namespace Community.Indicators
{
    /// <summary>
    /// MainModule.Instance
    /// </summary>
    static class MainModuleInstance
    {
        private static object _instance;
        private static MethodInfo _loadExternalSymbolWith2Args;
        private static MethodInfo _loadExternalSymbolWith4Args;

        /// <summary>
        /// Ctor
        /// </summary>
        static MainModuleInstance()
        {
            Initialize();
        }

        /// <summary>
        /// Get Wealth-Lab Dev/Pro exe assembly
        /// </summary>
        /// <returns></returns>
        private static Assembly GetWLAssembly()
        {
            Assembly wl = null;

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.FullName.Contains("WealthLabDev") || a.FullName.Contains("WealthLabPro"))
                {
                    wl = a;
                    break;
                }
            }

            if (wl == null) throw new Exception("Wealth-Lab not found.");

            return wl;
        }

        /// <summary>
        /// Set Main Module Instance
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static void SetMainModuleInstance()
        {
            Type mainModuleType = null;

            foreach (Type t in GetWLAssembly().GetTypes())
            {
                Type[] interfaces = t.GetInterfaces();

                bool wlpDetected = false;
                int c = 0;
                foreach (Type i in interfaces)
                {
                    if (i == typeof(WealthLab.IAuthenticationHost) ||
                        (i == typeof(WealthLab.IConnectionStatus)) ||
                        (i == typeof(WealthLab.IMenuItemAdder)))
                        c++;

                    if (c >= 3)
                        wlpDetected = true;
                }
                if (t.FullName == "WealthLabPro.MainModule" || wlpDetected)
                {
                    mainModuleType = t;
                    break;
                }
            }

            if (mainModuleType == null) throw new Exception("MainModule not found.");

            FieldInfo fiInstance = null;

            foreach (FieldInfo field in mainModuleType.GetFields())
            {
                if (field.FieldType == mainModuleType)
                {
                    fiInstance = field;
                    break;
                }
            }

            if (fiInstance == null) throw new Exception("MainModule.Instance not found.");

            _instance = fiInstance.GetValue(null);
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private static void Initialize()
        {
            SetMainModuleInstance();

            //

            SetLoadExternalSymbolInfos();
        }

        /// <summary>
        /// Set Load External Symbol Infos.
        /// </summary>
        private static void SetLoadExternalSymbolInfos()
        {
            foreach (MethodInfo method in _instance.GetType().GetMethods())
            {
                if (method.IsPublic && (method.ReturnType == typeof(Bars)))
                {
                    ParameterInfo[] parameters = method.GetParameters();

                    if ((parameters.Length == 2) && 
                        (parameters[0].ParameterType == typeof(string)) && 
                        (parameters[1].ParameterType == typeof(string)))
                    {
                        _loadExternalSymbolWith2Args = method;
                    }

                    if ((parameters.Length == 4) &&
                        (parameters[0].ParameterType == typeof(string)) &&
                        (parameters[1].ParameterType == typeof(BarScale)) &&
                        (parameters[2].ParameterType == typeof(int)) &&
                        (parameters[3].ParameterType == typeof(bool)))
                    {
                        _loadExternalSymbolWith4Args = method;
                    }
                }
            }

            if ((_loadExternalSymbolWith2Args == null) ||
                (_loadExternalSymbolWith4Args == null)) throw new Exception("LoadExternalSymbol methods not found.");
        }

        /// <summary>
        /// Load External Symbol
        /// </summary>
        /// <param name="dataSetName"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static Bars LoadExternalSymbol(string dataSetName, string symbol)
        {
            return (Bars)_loadExternalSymbolWith2Args.Invoke(_instance, new object[] { dataSetName, symbol });
        }

        /// <summary>
        /// Load External Symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="scale"></param>
        /// <param name="barInterval"></param>
        /// <param name="includePartialBar"></param>
        /// <returns></returns>
        public static Bars LoadExternalSymbol(string symbol, BarScale scale, int barInterval, bool includePartialBar)
        {
            return (Bars)_loadExternalSymbolWith4Args.Invoke(_instance, new object[] { symbol, scale, barInterval, includePartialBar });
        }
    }
}
