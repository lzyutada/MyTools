using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 库存异动通知接口的入参XML序列化对象
    /// </summary>
    [XmlRoot("request")]
    public class HttpReqXml_StockChange : HttpReqXmlBase
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        [XmlArray("items"), XmlArrayItem("item")]
        public HttpReqXml_StockChange_Item[] items = null;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockChange()
        {
            items = new HttpReqXml_StockChange_Item[1];
        }

        /// <summary>
        /// 根据pDesc创建实体
        /// </summary>
        /// <param name="pDesc"></param>
        public HttpReqXml_StockChange(string pDesc)
        {
            var tmpObj = Parse<HttpReqXml_StockChange>(pDesc) as HttpReqXml_StockChange;
            if (null != tmpObj)
            {
                items = tmpObj.items;
            }
        }

        /// <summary>
        /// overrided.
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpReqXml_StockChange);
        }
    }

    /// <summary>
    /// 库存异动通知接口的入参XML中[items]的序列化对象
    /// </summary>
    [XmlRoot("item")]
    public class HttpReqXml_StockChange_Item
    {
        #region Properties
        /// <summary>
        /// 货主编码
        /// </summary>
        public string ownerCode = string.Empty;

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string warehouseCode = string.Empty;

        /// <summary>
        /// 引起异动的单据编码
        /// </summary>
        public string orderCode = string.Empty;

        /// <summary>
        /// 单据类型 ，JYCK= 一般交易出库单，HHCK= 换货出库 ，BFCK= 补发出库 PTCK=普通出库单，
        /// DBCK=调拨出库 ，QTCK=其他出库， SCRK=生产入库，LYRK=领用入库，CCRK=残次品入库，
        /// CGRK=采购入库 ，DBRK= 调拨入库 ，QTRK= 其他入库 ，XTRK= 销退入库 HHRK= 换货入库,
        /// CNJG= 仓内加工单 ZTTZ=状态调整单
        /// </summary>
        public string orderType = string.Empty;

        /// <summary>
        /// 外部业务编码, 消息ID, 用于去重，用来保证因为网络等原因导致重复传输，请求不会被重复处理
        /// </summary>
        public string outBizCode = string.Empty;

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
        /// 批次
        /// </summary>
        [XmlArray("batchs"), XmlArrayItem("batch")]
        public HttpReqXml_Batch[] batchs = null;

        /// <summary>
        /// 备注
        /// </summary>
        public string remark = string.Empty;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockChange_Item()
        {
            batchs = new HttpReqXml_Batch[1];
        }
    }
}
