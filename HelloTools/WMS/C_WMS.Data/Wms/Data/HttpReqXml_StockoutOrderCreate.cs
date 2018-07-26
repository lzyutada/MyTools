using System;
using System.Xml.Serialization;
using C_WMS.Data.CWms.CWmsEntity;
using C_WMS.Data.Mango.Data;
using MangoMis.Frame.Helper;
using C_WMS.Interface;
using C_WMS.Interface.CWms.CWmsEntity;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// 出库单创建接口的HTTP请求XML对应的序列化类
    /// </summary>
    [XmlRoot("request")]
    class HttpReqXml_StockoutOrderCreate : HttpReqXmlBase
    {
        /// <summary>
        /// node 'deliveryOrder'
        /// </summary>
        public HttpReqXml_StockoutOrderCreate_DeliveryOrder deliveryOrder = null;

        /// <summary>
        /// node 'orderLines'
        /// </summary>
        [XmlArray("orderLines"), XmlArrayItem("orderLine")]
        public HttpReqXml_StockoutOrderCreate_OrderLine[] items = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderCreate()
        {
            deliveryOrder = new HttpReqXml_StockoutOrderCreate_DeliveryOrder();
            items = new HttpReqXml_StockoutOrderCreate_OrderLine[1];
        }

        public HttpReqXml_StockoutOrderCreate(CWmsStockOrder cWmsExWarehouseOrder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpReqXml_StockoutOrderCreate);
        }
    }

    /// <summary>
    /// node 'deliveryOrder' in Http request XML
    /// </summary>
    [XmlRoot("deliveryOrder")]
    class HttpReqXml_StockoutOrderCreate_DeliveryOrder : Interface.CWms.Interfaces.Data.HttpXmlBase
    {
        #region
        /// <summary>
        /// 单据总行数，int，当单据需要分多个请求发送时，发送方需要将totalOrderLines填入，接收方收到后，根据实际接收到的条数和totalOrderLines进行比对，如果小于，则继续等待接收请求。如果等于，则表示该单据的所有请求发送完成。
        /// </summary>
        public string totalOrderLines = string.Empty;

        /// <summary>
        /// 出库单号（ERP分配）, string (50) , 必填
        /// </summary>
        public string deliveryOrderCode = string.Empty;

        /// <summary>
        /// 出库单类型, string (50) , 必填, PTCK=普通出库单（退仓），DBCK=调拨出库，B2BCK=B2B出库，QTCK=其他出库，CGTH=采购退货出库单
        /// </summary>
        public string orderType = string.Empty;

        /// <summary>
        /// 仓库编码, string (50)，必填 ，统仓统配等无需ERP指定仓储编码的情况填OTHER
        /// </summary>
        public string warehouseCode = string.Empty;

        /// <summary>
        /// 出库单创建时间, string (19) , YYYY-MM-DD HH:MM:SS, 必填
        /// </summary>
        public string createTime = string.Empty;

        /// <summary>
        /// 要求出库时间, string (10) , YYYY-MM-DD
        /// </summary>
        public string scheduleDate = string.Empty;

        /// <summary>
        /// 物流公司编码, string (50) , SF=顺丰、EMS=标准快递、EYB=经济快件、ZJS=宅急送、YTO=圆通 、ZTO=中通 (ZTO) 、HTKY=百世汇通、UC=优速、STO=申通、TTKDEX=天天快递 、QFKD=全峰、FAST=快捷、POSTB=邮政小包 、GTO=国通、YUNDA=韵达、JD=京东配送、DD=当当宅配、OTHER=其他 ，(只传英文编码)
        /// </summary>
        public string logisticsCode = string.Empty;

        /// <summary>
        /// 物流公司名称（包括干线物流公司等）, string (200)
        /// </summary>
        public string logisticsName = string.Empty;

        /// <summary>
        /// 提货方式（到仓自提，快递，干线物流）
        /// </summary>
        public string transportMode = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string remark = string.Empty;

        /// <summary>
        /// 扩展1，string (50)
        /// </summary>
        public string ext1 = string.Empty;

        /// <summary>
        /// 扩展2，string (50)
        /// </summary>
        public string ext2 = string.Empty;

        /// <summary>
        /// Y 允许部分发货，N不允许
        /// </summary>
        public string partialShipment = string.Empty;

        /// <summary>
        /// 运单号, string (50)
        /// </summary>
        public string expressCode = string.Empty;
        #endregion

        /// <summary>
        /// 提货人信息
        /// </summary>
        public HttpReqXml_StockoutOrderCreate_PickerInfo pickerInfo = null;

        /// <summary>
        /// 发件人信息
        /// </summary>
        public HttpReqXml_StockoutOrderCreate_SenderInfo senderInfo = null;

        /// <summary>
        /// 收件人信息
        /// </summary>
        public HttpReqXml_StockoutOrderCreate_ReceiverInfo receiverInfo = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderCreate_DeliveryOrder()
        {
            pickerInfo = new HttpReqXml_StockoutOrderCreate_PickerInfo();
            senderInfo = new HttpReqXml_StockoutOrderCreate_SenderInfo();
            receiverInfo = new HttpReqXml_StockoutOrderCreate_ReceiverInfo();
        }


        /// <summary>
        /// 通过CWmsExWarehouseSubOrder实例拷贝HttpReqXml_StockoutOrderCreate_DeliveryOrder实例
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public string CopyFrom(CWmsStockOrder srcObj)
        {
            if (null == srcObj)
                return "HttpReqXml_StockoutOrderCreate_DeliveryOrder.CopyFrom异常，非法入参srcObj";

            var logistics = srcObj.Handler.GetLogistics(srcObj);
            totalOrderLines = srcObj.SubOrders.Count.ToString();
            deliveryOrderCode = (srcObj.MangoOrder as MangoStockouOrder).ProductOutputMainId.ToString();
            orderType = srcObj.WmsStockoutType.ToString(); //  ret.Append("wms出库单类型:{0}", srcObj.WmsStockoutType);
            warehouseCode = srcObj.Handler.GetWarehouse(srcObj).WmsCode;
            createTime = srcObj.MangoOrder.AddTime.ToString();
            scheduleDate = srcObj.ScheculeDeliverDate;  // ret.Append("要求配送时间:{0}", srcObj.ScheculeDeliverDate);
            remark = srcObj.MangoOrder.Remark;
            partialShipment = "N"; // CWmsConsts.cStrStockoutPartialShipmentDefault;
            logisticsCode = logistics?.logisticsName;   // 承运商Code
            logisticsName = logistics?.logisticsName;   // 承运商名称

            receiverInfo.CopyFrom(new CWmsAgentBase(TCwmsAgentType.ESHR, (srcObj.MangoOrder as MangoStockouOrder).JieShouRen.ToString()));
            senderInfo.CopyFrom(new CWmsAgentBase(TCwmsAgentType.EFHR, (srcObj.MangoOrder as MangoStockouOrder).ChuKuRen.ToString()));

            return string.Empty;
        }
    }

    /// <summary>
    /// node 'orderLines' in Http request XML
    /// </summary>
    [XmlRoot("orderLine")]
    class HttpReqXml_StockoutOrderCreate_OrderLine : Interface.CWms.Interfaces.Data.HttpXmlBase
    {
        #region
        /// <summary>
        /// 外部业务编码, 消息ID, 用于去重，当单据需要分批次发送时使用
        /// </summary>
        public string outBizCode = string.Empty;

        /// <summary>
        /// 单据行号
        /// </summary>
        public string orderLineNo = string.Empty;

        /// <summary>
        /// 货主编码
        /// </summary>
        public string ownerCode = string.Empty;

        /// <summary>
        /// 商品编码
        /// </summary>
        public string itemCode = string.Empty;

        /// <summary>
        /// 仓储系统商品编码
        /// </summary>
        public string itemId = string.Empty;

        /// <summary>
        /// 商品名称
        /// </summary>
        public string itemName = string.Empty;

        /// <summary>
        /// 库存类型，string (50) , ZP=正品, CC=残次,JS=机损, XS= 箱损，默认为ZP
        /// </summary>
        public string inventoryType = string.Empty;

        /// <summary>
        /// 应发商品数量
        /// </summary>
        public string planQty = string.Empty;

        /// <summary>
        /// 商品单价
        /// </summary>
        public string price = string.Empty;

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
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderCreate_OrderLine()
        {
        }

        /// <summary>
        /// 通过CWmsExWarehouseSubOrder实例拷贝HttpReqXml_StockoutOrderCreate_OrderLine实例
        /// </summary>
        /// <param name="pSrcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public string CopyFrom(CWmsSubStockoutOrder pSrcObj)
        {
            throw new NotImplementedException();

#if C_WMS_V1
            if (null == pSrcObj)
                return "HttpReqXml_StockoutOrderCreate_OrderLine.CopyFrom异常，非法入参srcObj";

            var mseo = pSrcObj.MangoOrder as MangoSubStockoutOrder;

            outBizCode = pSrcObj.WmsOrder.OutBizCode;
            orderLineNo = mseo.ProductOutputId.ToString();
            ownerCode = pSrcObj.WmsOrder.Owner.WmsID;    // TODO: 在获取子出库单时获取货主
            itemCode = (string.IsNullOrEmpty(mseo.ProductGuiGeID.ToString()) || "0".Equals(mseo.ProductGuiGeID.ToString())) ? 
                mseo.ProductId.ToString() : string.Format("{0}-{1}", mseo.ProductId, mseo.ProductGuiGeID);
            itemName = pSrcObj.Product.MangoProduct.Title; // ret.Append("商品名称:{0}", itemName);
            inventoryType = pSrcObj.WmsOrderDetail.InventoryType.ToString();
            planQty = Interface.Utility.CWmsUtility.Decimal2Int(mseo.PlanQuantity.Decimal()).ToString();  // TODO: 处理单位
            price = pSrcObj.Product.MangoProduct.PriceAve.ToString();  // TODO: 确认价格

            return string.Empty;
#endif
        }
    }

    /// <summary>
    /// node 'pickerInfo' of HTTP request XML
    /// </summary>
    [XmlRoot("pickerInfo")]
    class HttpReqXml_StockoutOrderCreate_PickerInfo : Interface.CWms.Interfaces.Data.HttpXmlBase_PickerInfoBase
    {
        /// <summary>
        /// 证件号
        /// </summary>
        public string id = string.Empty;

        /// <summary>
        /// 车牌号
        /// </summary>
        public string carNo = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderCreate_PickerInfo() { }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        /// <param name="pCompany"></param>
        /// <param name="pName"></param>
        /// <param name="pTel"></param>
        /// <param name="pMobile"></param>
        /// <param name="pId"></param>
        /// <param name="pCarNo"></param>
        public HttpReqXml_StockoutOrderCreate_PickerInfo(string pCompany, string pName, string pTel, string pMobile, string pId, string pCarNo)
            :base(pCompany, pName, pTel, pMobile)
        {
            id = pId;
            carNo = pCarNo;
        }
    }

    /// <summary>
    /// node 'senderInfo' of HTTP request XML
    /// </summary>
    [XmlRoot("senderInfo")]
    class HttpReqXml_StockoutOrderCreate_SenderInfo : Interface.CWms.Interfaces.Data.HttpXmlBase_SenderInfoBase
    {
        /// <summary>
        /// 证件号
        /// </summary>
        public string id = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderCreate_SenderInfo() { }
    }

    /// <summary>
    /// node 'receiverInfo' of HTTP request XML
    /// </summary>
    [XmlRoot("receiverInfo")]
    class HttpReqXml_StockoutOrderCreate_ReceiverInfo : Interface.CWms.Interfaces.Data.HttpXmlBase_SenderInfoBase
    {
        /// <summary>
        /// 客户编码
        /// </summary>
        public string receiverCode = string.Empty;

        /// <summary>
        /// 证件号
        /// </summary>
        public string id = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderCreate_ReceiverInfo() { }
    }
}
