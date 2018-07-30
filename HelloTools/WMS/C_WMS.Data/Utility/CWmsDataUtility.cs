using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TT.Common.Frame.Model;

namespace C_WMS.Data.Utility
{
    /// <summary>
    /// 
    /// </summary>
    class CWmsDataUtility
    {
        /// <summary>
        /// Return True if collection has any element, otherwise return False;
        /// </summary>
        /// <typeparam name="T"><![CDATA[T in IEnumerable]]></typeparam>
        /// <param name="collection">collection to be operated</param>
        /// <returns></returns>
        static public bool IEnumerableAny<T>(IEnumerable<T> collection)
        {
            try
            {
                if (!Enumerable.Any(collection)) return false;
                return (null != collection.First(item => null != item));
            }
            catch (Exception ex)
            {
                MyLog.Instance.Warning(ex, "Exception in CWmsDataUtility.IEnumerableAny<{0}>({1})", typeof(T), collection);
                return false;
            }
        }

        /// <summary>
        /// return count of items in collection
        /// </summary>
        /// <typeparam name="T"><![CDATA[T in IEnumerable]]></typeparam>
        /// <param name="collection">collection to be operated</param>
        /// <returns></returns>
        static public int IEnumerableCount<T>(IEnumerable<T> collection)
        {
            try
            {
                if (null == collection || !Enumerable.Any(collection)) return 0;
                return collection.Where(item => null != item).Count();
            }
            catch (Exception ex)
            {
                MyLog.Instance.Warning(ex, "Exception in CWmsDataUtility.IEnumerableCount<{0}>({1})", typeof(T), collection);
                return -1;
            }
        }

        /// <summary>
        /// remove null item(s) in collection
        /// </summary>
        /// <typeparam name="T"><![CDATA[T in IEnumerable]]></typeparam>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        static public void IEnumerableRemoveEmpty<T>(IEnumerable<T> source, out IEnumerable<T> dest)
        {
            dest = null;
            try
            {
                if (Enumerable.Any(source))
                {
                    dest = source.Where(item => null != item);
                }
            }
            catch (Exception ex)
            {
                MyLog.Instance.Warning(ex, "Exception in CWmsDataUtility.IEnumerableRemoveEmpty<{0}>({1})", typeof(T), source);
            }
        }

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
