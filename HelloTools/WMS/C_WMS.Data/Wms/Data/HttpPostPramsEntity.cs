using MangoMis.Frame.ThirdFrame;
using MangoMis.MisFrame.MisStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// 用于拼接url地址的实体类
    /// </summary>
    public class HttpPostPramsEntity
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public string method { get; set; }
        public string format { get; set; }
        public string app_key { get; set; }
        public string v { get; set; }
        public string sign { get; set; }
        public string sign_method { get; set; }
        public string customerId { get; set; }
        public string timestamp { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// Default constructor
        /// </summary>
        public HttpPostPramsEntity()
        {
            Load();
        }

        /// <summary>
        /// 加载配置信息，获取支持传输的数据格式、app_key、版本、签名方法、customerId等信息。
        /// </summary>
        /// <returns>如果成功则返回ErrorNone，否则返回其他值。</returns>
        public int Load()
        {
            method = string.Empty;
            format = SystemParamStore.FindParam("MgMall", "format")?.PValue ?? ""; // "xml"; // 
            app_key = SystemParamStore.FindParam("MgMall", "app_key")?.PValue ?? "";  // "201804261190"; // 
            v = SystemParamStore.FindParam("MgMall", "v")?.PValue ?? ""; // "2.0"; // 
            sign = string.Empty;
            sign_method = SystemParamStore.FindParam("MgMall", "sign_method")?.PValue ?? ""; // "md5"; //  
            customerId = SystemParamStore.FindParam("MgMall", "customerid")?.PValue ?? ""; // "lt"; // 
            timestamp = string.Empty;

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretkey"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string DoSign(string secretkey, string xml)
        {       
            timestamp = C_WMS.Interface.Utility.CWmsUtility.GetCurrentNowTime();
            var data = GetProperties();
            // 1. 确保参数已经排序
            var data_orderby_key = from objDic in data orderby objDic.Key where objDic.Key != "sign" select objDic;
            // 2.把所有参数名和参数值拼接在一起(包含body体)
            sign += secretkey;
            foreach (var i in data_orderby_key)
            {
                sign += i.Key + i.Value;
            }
            sign += xml;
            sign += secretkey;
            //// 3. 使用加密算法进行加密（目前仅支持md5算法）
            return sign = C_WMS.Interface.Utility.CWmsUtility.Md5(sign);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/29 15:08
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        private Dictionary<string, string> GetProperties()
        {
          

            Dictionary<string, string> tStr = new Dictionary<string, string>();
            System.Reflection.PropertyInfo[] properties = this.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
               
                return tStr;
            }
       
            foreach (var item in properties)
            {
              
                string name = item.Name;
                object value = item.GetValue(this, null);
                if ((item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String")) && name != "sign")
                {
                    tStr.Add(name, value.ToString());
                }
            }
         
            return tStr;
        }
    }
}
