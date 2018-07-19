using System;
//using System.Xml;
using System.Xml.Serialization;
//using System.Text;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 出库单创建接口的HTTP响应XML对应的序列化类
    /// </summary>
    [XmlRoot("response")]
    public class HttpRespXml_StockoutOrderCreate : Interface.CWms.Interfaces.Data.HttpRespXmlBase
    {
        /// <summary>
        /// 出库单仓储系统编码
        /// </summary>
        public string deliveryOrderId = string.Empty;

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string createTime = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpRespXml_StockoutOrderCreate() { }

        /// <summary>
        /// overrided constructor. Construct instance by deserializing object from respXmlStr.
        /// </summary>
        /// <param name="pRespXmlStr"></param>
        public HttpRespXml_StockoutOrderCreate(string pRespXmlStr)
            :base(pRespXmlStr)
        {
            HttpRespXml_StockoutOrderCreate tmpObj = Parse<HttpRespXml_StockoutOrderCreate>(pRespXmlStr) as HttpRespXml_StockoutOrderCreate;
            if (null != tmpObj)
            {
                deliveryOrderId = tmpObj.deliveryOrderId;
                createTime = tmpObj.createTime;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpRespXml_StockoutOrderCreate);
        }
    }
}
