using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// 退货入库单创建接口的HTTP响应XML对应的序列化类
    /// </summary>
    [XmlRoot("response")]
    public class HttpRespXml_ReturnOrderCreate : Interface.CWms.Interfaces.Data.HttpRespXmlBase
    {
        /// <summary>
        /// 仓储系统退货单编码, string (50)
        /// </summary>
        public string returnOrderId = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpRespXml_ReturnOrderCreate()
        {
        }

        /// <summary>
        /// 根据输入XML字符串创建实例
        /// </summary>
        /// <param name="pXmlStr">与HttpRespXml_ReturnOrderCreate对应的XML字符串</param>
        public HttpRespXml_ReturnOrderCreate(string pXmlStr)
        {
            var tmpObj = Parse<HttpRespXml_ReturnOrderCreate>(pXmlStr) as HttpRespXml_ReturnOrderCreate;
            if (null != tmpObj)
            {
                flag = tmpObj.flag;
                code = tmpObj.code;
                message = tmpObj.message;
                returnOrderId = tmpObj.returnOrderId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpRespXml_ReturnOrderCreate);
        }
    }
}
