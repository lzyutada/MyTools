using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 所有C-WMS通讯接口中，HTTP request XML的基类
    /// </summary>
    public class HttpReqXmlBase : C_WMS.Interface.CWms.Interfaces.Data.HttpXmlBase
    {
        /// <summary>
        /// Construct instance by deserialize object of type from pDesc.
        /// </summary>
        /// <param name="pDesc">XML descriptor</param>
        public object Parse<T>(string pDesc)
        {
            if (string.IsNullOrEmpty(pDesc))
            {
                return null;
            }
            else
            {
                var outObj = Interface.Utility.CWmsUtility.ObjtoXml(typeof(T), pDesc);
                return outObj;
            }
        }
    }

    /// <summary>
    /// batch节点
    /// </summary>
    [XmlRoot("batch")]
    public class HttpReqXml_Batch
    {
        #region Properties
        /// <summary>
        /// 批次编号
        /// </summary>
        public string batchCode = string.Empty;

        /// <summary>
        /// 生产日期
        /// </summary>
        public string productDate = string.Empty;

        /// <summary>
        /// 过期日期
        /// </summary>
        public string expireDate = string.Empty;

        /// <summary>
        /// 生产批号
        /// </summary>
        public string produceCode = string.Empty;

        /// <summary>
        /// 库存类型
        /// </summary>
        public string inventoryType = string.Empty;

        /// <summary>
        /// 实收数量
        /// </summary>
        public string actualQty = string.Empty;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_Batch() { }
    }
}
