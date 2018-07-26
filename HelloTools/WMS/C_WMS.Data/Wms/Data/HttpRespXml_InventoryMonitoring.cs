using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using C_WMS.Interface.CWms.Interfaces.Data;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// 接口‘库存监控接口 ’中响应XML的数据类
    /// </summary>
    [XmlRoot("response")]
    public class HttpRespXml_InventoryMonitoring : HttpRespXmlBase
    {
        /// <summary>
        /// 商品的信息
        /// </summary>
        [XmlArray("items"), XmlArrayItem("item")]
        public List<HttpRespXml_InventoryMonitoring_Item> items;

        public HttpRespXml_InventoryMonitoring()
        {
            items = new List<HttpRespXml_InventoryMonitoring_Item>(1);
        }

        public HttpRespXml_InventoryMonitoring(string xmlDescriptor)
        {
            Utility.CWmsXmlSerializer xs = new Utility.CWmsXmlSerializer(false, typeof(HttpRespXml_InventoryMonitoring), xmlDescriptor);
            items = (xs.Deserialize() as HttpRespXml_InventoryMonitoring)?.items.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpRespXml_InventoryMonitoring);
        }
    }

    [XmlRoot("item")]
    public class HttpRespXml_InventoryMonitoring_Item
    {
        public string warehouseName { get; set; }
        public string customerCode { get; set; }
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string barcodeNum { get; set; }
        public string normalFlag { get; set; }
        public string lockQuantity { get; set; }
        public string allQuantity { get; set; }
        public string diffQuantity { get; set; }
    }
}
