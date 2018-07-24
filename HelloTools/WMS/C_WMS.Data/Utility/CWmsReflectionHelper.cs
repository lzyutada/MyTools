using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace C_WMS.Data.Utility
{
    class CWmsReflectionHelper
    {
        /// <summary>
        /// return the actural Type of pInfo according to MemberTypes.
        /// method will return null if the argument is null or it's MemberType wasn't the wanted.
        /// </summary>
        /// <param name="pInfo">a MemberInfo object</param>
        /// <returns></returns>
        static public Type GetMemberInfoType(MemberInfo pInfo)
        {
            if (null == pInfo)
                return null;

            Type rslt = null;
            switch (pInfo.MemberType)
            {
                case MemberTypes.Field:
                    {
                        rslt = (pInfo as FieldInfo).FieldType;
                        break;
                    }
                case MemberTypes.Property:
                    {
                        rslt = (pInfo as PropertyInfo).PropertyType;
                        break;
                    }
                default:
                    {
                        MyLog.Instance.Error("在{0}中，{1}不是期望的成员类型（MemberType={2}）", MethodBase.GetCurrentMethod().Name, pInfo, pInfo.MemberType);
                    break;
                    }
            }
            return rslt;
        }

        /// <summary>
        /// get the name of Assembly according to the fullname of a reference type who was defined in the destination assembly.
        /// </summary>
        /// <param name="pFullName">fullname of the declared type</param>
        /// <returns>return the name of the destination assembly</returns>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// startIndex plus length indicates a position not within this instance.
        /// -or- startIndex or length is less than zero.
        /// </exception>
        static public string GetAssemblyName(string pFullName)
        {
            int idx = pFullName.IndexOf('.');
            if (0 >= idx)
                throw new FormatException(string.Format("在{0}中，根据{1}获取程序集名称异常!", MethodBase.GetCurrentMethod().Name, pFullName));
            return pFullName.Substring(0, idx);
        }

        /// <summary>
        /// 根据Type的FullName获取其所在的程序集
        /// </summary>
        /// <param name="pFullName">Type的FullName</param>
        /// <returns></returns>
        static public Assembly GetAssemblyByTypeName(string pFullName)
        {
            try
            {
                string assemblyName = GetAssemblyName(pFullName);
                return Assembly.Load(assemblyName);
            }
            catch (Exception ex)
            {
                MyLog.Instance.Error(ex, "根据Type的FullName({0})获取其所在的程序集异常！", pFullName);
                return null;
            }
        }
    }
}
