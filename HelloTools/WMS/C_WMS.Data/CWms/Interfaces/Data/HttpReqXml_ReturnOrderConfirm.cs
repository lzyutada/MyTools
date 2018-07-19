using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 退货单确认接口的入参XML序列化对象
    /// </summary>
    [XmlRoot("request")]
    public class HttpReqXml_ReturnOrderConfirm : HttpReqXmlBase
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public HttpReqXml_ReturnOrderConfirm_ReturnOrder entryOrder = null;

        [XmlArray("orderLines"), XmlArrayItem("orderLine")]
        public HttpReqXml_ReturnOrderConfirm_OrderLine[] orderLines = null;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_ReturnOrderConfirm()
        {
            entryOrder = new HttpReqXml_ReturnOrderConfirm_ReturnOrder();
            orderLines = new HttpReqXml_ReturnOrderConfirm_OrderLine[1];
        }

        /// <summary>
        /// 根据pDesc创建实体
        /// </summary>
        /// <param name="pDesc"></param>
        public HttpReqXml_ReturnOrderConfirm(string pDesc)
        {
            var tmpObj = Parse<HttpReqXml_ReturnOrderConfirm>(pDesc) as HttpReqXml_ReturnOrderConfirm;
            if (null != tmpObj)
            {
                entryOrder = tmpObj.entryOrder;
                orderLines = tmpObj.orderLines;
            }
        }

        /// <summary>
        /// overrided.
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpReqXml_ReturnOrderConfirm);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("returnOrder")]
    public class HttpReqXml_ReturnOrderConfirm_ReturnOrder
    {
        #region Properties
        /// <summary>
        /// ERP的退货入库单编码
        /// </summary>
        public string returnOrderCode = string.Empty;

        /// <summary>
        /// 仓库编码，统仓统配等无需ERP指定仓储编码的情况填OTHER
        /// </summary>
        public string warehouseCode = string.Empty;

        /// <summary>
        /// 单据类型, THRK=退货入库，HHRK=换货入库(只传英文编码)
        /// </summary>
        public string orderType = string.Empty;

        /// <summary>
        /// 用字符串格式来表示订单标记列表：比如VISIT^ SELLER_AFFORD^SYNC_RETURN_BILL 等, 中间用“^”来隔开.
        /// 订单标记list(所有字母全部大写) ： VISIT=上门；SELLER_AFFORD=是否卖家承担运费(默认是) ；SYNC_RETURN_BILL=同时退回发票；
        /// </summary>
        public string orderFlag = string.Empty;

        /// <summary>
        /// 原出库单号（ERP分配）
        /// </summary>
        public string preDeliveryOrderCode = string.Empty;

        /// <summary>
        /// 原出库单号（C-WMS分配）
        /// </summary>
        public string preDeliveryOrderId = string.Empty;

        /// <summary>
        /// 物流公司编码, SF=顺丰、EMS=标准快递、EYB=经济快件、ZJS=宅急送、YTO=圆通、
        /// ZTO=中通(ZTO) 、HTKY=百世汇通、UC=优速、STO=申通、TTKDEX=天天快递  、QFKD=全峰、
        /// FAST=快捷、POSTB=邮政小包  、GTO=国通、YUNDA=韵达、JD=京东配送、DD=当当宅配、OTHER=其他，必填,  (只传英文编码) 
        /// </summary>
        public string logisticsCode = string.Empty;

        /// <summary>
        /// 物流公司名称
        /// </summary>
        public string logisticsName = string.Empty;

        /// <summary>
        /// 运单号
        /// </summary>
        public string expressCode = string.Empty;

        /// <summary>
        /// 退货原因
        /// </summary>
        public string returnReason = string.Empty;

        /// <summary>
        /// 买家昵称
        /// </summary>
        public string buyerNick = string.Empty;

        /// <summary>
        /// 发件人信息
        /// </summary>
        public HttpReqXml_ReturnOrderConfirm_SenderInfo senderInfo = null;

        /// <summary>
        /// 备注
        /// </summary>
        public string remark = string.Empty;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_ReturnOrderConfirm_ReturnOrder()
        {
            senderInfo = new HttpReqXml_ReturnOrderConfirm_SenderInfo();
        }
    }

    /// <summary>
    /// 发货人信息类
    /// </summary>
    [XmlRoot("senderInfo")]
    public class HttpReqXml_ReturnOrderConfirm_SenderInfo : Interface.CWms.Interfaces.Data.HttpXmlBase_SenderInfoBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_ReturnOrderConfirm_SenderInfo() { }
    }

    /// <summary>
    /// 订单明细类
    /// </summary>
    [XmlRoot("orderLine")]
    public class HttpReqXml_ReturnOrderConfirm_OrderLine
    {
        #region Properties
        /// <summary>
        /// 单据行号
        /// </summary>
        public string orderLineNo = string.Empty;

        /// <summary>
        /// 交易平台订单
        /// </summary>
        public string sourceOrderCode = string.Empty;

        /// <summary>
        /// 交易平台子订单编码
        /// </summary>
        public string subSourceOrderCode = string.Empty;

        /// <summary>
        /// 货主编码
        /// </summary>
        public string ownerCode = string.Empty;

        /// <summary>
        /// 商品编码
        /// </summary>
        public string itemCode = string.Empty;

        /// <summary>
        /// 仓储系统商品编码, 条件必填，条件为提供后端（仓储系统）商品编码的仓储系统
        /// </summary>
        public string itemId = string.Empty;

        /// <summary>
        /// 库存类型, ZP=正品, CC=残次,JS=机损, XS= 箱损, 默认为ZP
        /// </summary>
        public string inventoryType = string.Empty;

        /// <summary>
        /// 应收商品数量
        /// </summary>
        public string planQty = string.Empty;

        /// <summary>
        /// 批次编码
        /// </summary>
        public string batchCode = string.Empty;

        /// <summary>
        /// 生产日期, YYYY-MM-DD
        /// </summary>
        public string productDate = string.Empty;

        /// <summary>
        /// 过期日期, YYYY-MM-DD
        /// </summary>
        public string expireDate = string.Empty;

        /// <summary>
        /// 生产批号
        /// </summary>
        public string produceCode = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        [XmlArray("batchs"), XmlArrayItem("batch")]
        public HttpReqXml_Batch[] batchs = null;

        /// <summary>
        /// 商品的二维码（类似电子产品的SN码），用来进行商品的溯源，多个二维码之间用分号（;）隔开
        /// </summary>
        public string qrCode = string.Empty;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_ReturnOrderConfirm_OrderLine()
        {
            batchs = new HttpReqXml_Batch[1];
        }
    }
}
