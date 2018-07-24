using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 库存盘点通知接口的入参XML序列化对象
    /// </summary>
    [XmlRoot("request")]
    public class HttpReqXml_InventoryReport : HttpReqXmlBase
    {
        #region Properties
        /// <summary>
        /// 总页数
        /// </summary>
        public string totalPage = string.Empty;

        /// <summary>
        /// 当前页，从1开始
        /// </summary>
        public string currentPage = string.Empty;

        /// <summary>
        /// 每页记录的条数
        /// </summary>
        public string pageSize = string.Empty;

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string warehouseCode = string.Empty;

        /// <summary>
        /// 盘点单编码
        /// </summary>
        public string checkOrderCode = string.Empty;

        /// <summary>
        /// 仓储系统的盘点单编码
        /// </summary>
        public string checkOrderId = string.Empty;

        /// <summary>
        /// 货主编码
        /// </summary>
        public string ownerCode = string.Empty;

        /// <summary>
        /// 盘点时间, YYYY-MM-DD HH:MM:SS
        /// </summary>
        public string checkTime = string.Empty;

        /// <summary>
        /// 外部业务编码, 消息ID, 用于去重, ISV对于同一请求，分配一个唯一性的编码。用来保证因为网络等原因导致重复传输，请求不会被重复处理
        /// </summary>
        public string outBizCode = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string remark = string.Empty;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        [XmlArray("items"), XmlArrayItem("item")]
        public HttpReqXml_InventoryReport_Item[] items = null;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_InventoryReport()
        {
            items = new HttpReqXml_InventoryReport_Item[1];
        }

        /// <summary>
        /// 根据pDesc创建实体
        /// </summary>
        /// <param name="pDesc"></param>
        public HttpReqXml_InventoryReport(string pDesc)
        {
            var tmpObj = Parse<HttpReqXml_InventoryReport>(pDesc) as HttpReqXml_InventoryReport;
            if (null != tmpObj)
            {
                // TODO: others
                items = tmpObj.items;
            }
        }

        /// <summary>
        /// overrided.
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpReqXml_InventoryReport);
        }
    }

    /// <summary>
    /// 库存盘点通知接口的入参XML中[items]的序列化对象
    /// </summary>
    [XmlRoot("item")]
    public class HttpReqXml_InventoryReport_Item
    {
        #region Properties
        /// <summary>
        /// 商品编码
        /// </summary>
        public string itemCode = string.Empty;

        /// <summary>
        /// 仓储系统商品ID
        /// </summary>
        public string itemId = string.Empty;

        /// <summary>
        /// 库存类型, ZP=正品, CC=残次,JS=机损, XS= 箱损, ZT=在途库存，默认为ZP
        /// </summary>
        public string inventoryType = string.Empty;

        /// <summary>
        /// 盘盈盘亏商品变化量，盘盈为正数，盘亏为负数public string 
        /// </summary>
        public string quantity = string.Empty;

        /// <summary>
        /// 批次编码
        /// </summary>
        public string batchCode = string.Empty;

        /// <summary>
        /// 商品生产日期 YYYY-MM-DD
        /// </summary>
        public string productDate = string.Empty;

        /// <summary>
        /// 商品过期日期YYYY-MM-DD
        /// </summary>
        public string expireDate = string.Empty;

        /// <summary>
        /// 生产批号
        /// </summary>
        public string produceCode = string.Empty;

        /// <summary>
        /// 商品序列号
        /// </summary>
        public string snCode = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string remark = string.Empty;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_InventoryReport_Item()
        {
        }
    }
}
