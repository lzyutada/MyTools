using MangoMis.Frame.ThirdFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using C_WMS.Interface.CWms.Interfaces.Data;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 接口‘商品同步接口 (批量) ’中响应XML的数据类
    /// </summary>
    [XmlRoot("response")]
    public class HttpRespXml_ItemsSynchronize : HttpRespXmlBase
    {
        [XmlArray("items"), XmlArrayItem("item")]
        public List<HttpRespXml_ItemsSync_Item> items = null;

        public HttpRespXml_ItemsSynchronize()
        {
            items = new List<HttpRespXml_ItemsSync_Item>(1);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpRespXml_ItemsSynchronize);
        }
    }

    /// <summary>
    /// 接口‘商品同步接口 (批量) ’中响应XML中的'item'数据类
    /// </summary>
    [XmlRoot("response")]
    public class HttpRespXml_ItemsSync_Item
    {
        /// <summary>
        /// 没有同步成功的商品的编码，必填
        /// </summary>
        public string itemCode = string.Empty;
        /// <summary>
        /// 出错信息
        /// </summary>
        public string message = string.Empty;

    }
}
