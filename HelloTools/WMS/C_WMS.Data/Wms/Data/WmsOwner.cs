using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// WMS系统中的货主
    /// </summary>
    public class WmsOwner : WmsEntityBase
    {
        /// <summary>
        /// 货主名称
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public WmsOwner() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pName"></param>
        public WmsOwner(string id, string pName)
        {
            WmsID = id;
            name = pName;
        }

        /// <summary>
        /// 从src拷贝货主信息
        /// </summary>
        /// <param name="src">源数据</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string CopyFrom(WmsOwner src)
        {
            if (null == src)
            {
                return "WmsOwner.CopyFrom 非法入参";
            }

            WmsID = src.WmsID;
            name = src.name;

            return string.Empty;
        }
    }
}
