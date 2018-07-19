using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 入库单创建接口的HTTP响应XML对应的序列化类
    /// </summary>
    [XmlRoot("response")]
    public class HttpRespXml_EntryOrderCreate : Interface.CWms.Interfaces.Data.HttpRespXmlBase
    {
        /// <summary>
        /// 仓储系统入库单编码
        /// </summary>
        public string entryOrderId = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpRespXml_EntryOrderCreate() { }

        /// <summary>
        /// override constructor, constructed from HTTP response XML.
        /// </summary>
        /// <param name="pRespXmlStr">HTTP response XML.</param>
        public HttpRespXml_EntryOrderCreate(string pRespXmlStr)
            : base(pRespXmlStr)
        {
            HttpRespXml_EntryOrderCreate tmpObj = Parse<HttpRespXml_EntryOrderCreate>(pRespXmlStr) as HttpRespXml_EntryOrderCreate;
            if (null != tmpObj)
            {
                flag = tmpObj.flag;
                code = tmpObj.code;
                message = tmpObj.message;
                entryOrderId = tmpObj.entryOrderId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpRespXml_EntryOrderCreate);
        }
    }
}
