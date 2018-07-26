using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TT.Common.Frame.Model;

namespace C_WMS.Data.Utility
{
    class CWmsDataUtility
    {
        /// <summary>
        /// get debug info descriptor of MIS entity filters.
        /// this method should not throw exception.
        /// </summary>
        /// <param name="pFilters">filter to be debugged.</param>
        /// <returns></returns>
        static public string GetDebugInfo_MisFilter(List<CommonFilterModel> pFilters)
        {
            string dbgInfo = "FILTER DEBUG INFO: ";
            try
            {
                if (null == pFilters || 0 == pFilters.Count)
                {
                    return dbgInfo += string.Format("empty pFilters.Count={0}", pFilters?.Count);
                }
                else
                {
                    foreach (CommonFilterModel f in pFilters)
                    {
                        dbgInfo += string.Format("\r\nFilter({0})[Name={1}, Filter={2}, Value={3}]", f, f?.Name, f?.Filter, f?.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                dbgInfo += string.Format("!!Exception int {0}.{1}({2}), message={3}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pFilters, ex.Message);
            }
            return dbgInfo;
        }

        internal static string GetDebugInfo_Args(object[] args)
        {
            string dbgInfo = "PARAMS object[] ARGS DEBUG INFO: ";
            try
            {
                if (null == args || 0 == args.Length)
                {
                    return dbgInfo += string.Format("empty args or args.Count={0}", args?.Length);
                }
                else
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        dbgInfo += string.Format("\r\nargs[{0}]={1}", i, args[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                dbgInfo += string.Format("!!EXCEPTION in {0}.{1}({2}). message={3{", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, args, ex.Message);
            }
            return dbgInfo;
        }
    }
}
