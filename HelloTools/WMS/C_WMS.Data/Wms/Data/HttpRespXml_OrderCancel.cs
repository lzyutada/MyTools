using System;
using System.Xml.Serialization;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// 单据取消接口的HTTP响应XML对应的序列化类
    /// </summary>
    [XmlRoot("response")]
    public class HttpRespXml_OrderCancel : Interface.CWms.Interfaces.Data.HttpRespXmlBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpRespXml_OrderCancel);
        }
    }
}
