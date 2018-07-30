using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("request")]
    public class HttpReqXml_InventoryMonitoring : HttpReqXmlBase
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlArray("inventoryMonitoringList"), XmlArrayItem("inventoryMonitoring")]
        public List<HttpReqXml_InventoryMonitoring_item> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpReqXml_InventoryMonitoring"/> class.
        /// </summary>
        public HttpReqXml_InventoryMonitoring()
        {
            items = new List<HttpReqXml_InventoryMonitoring_item>(1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpReqXml_InventoryMonitoring);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("inventoryMonitoring")]
    public class HttpReqXml_InventoryMonitoring_item
    {
        /// <summary>
        /// 
        /// </summary>
        public string warehouseName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string customerCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string itemCode { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public HttpReqXml_InventoryMonitoring_item()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="c"></param>
        /// <param name="i"></param>
        public HttpReqXml_InventoryMonitoring_item(string w, string c, string i)
        {
            this.warehouseName = w;
            this.customerCode = c;
            this.itemCode = i;
        }
    }
}
