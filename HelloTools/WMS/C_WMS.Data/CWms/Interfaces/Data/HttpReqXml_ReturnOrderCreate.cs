using C_WMS.Data.Mango;
using C_WMS.Interface.Utility;
using MangoMis.Frame.Helper;
using System;
using System.Xml.Serialization;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 退货入库单创建接口的HTTP请求XML对应的序列化类
    /// </summary>
    [XmlRoot("request")]
    public class HttpReqXml_ReturnOrderCreate : HttpReqXmlBase
    {
        /// <summary>
        /// node 'returnOrder'
        /// </summary>
        public HttpReqXml_ReturnOrderCreate_ReturnOrder returnOrder = null;

        /// <summary>
        /// node 'orderLines'
        /// </summary>
        [XmlArray("orderLines"), XmlArrayItem("orderLine")]
        public HttpReqXml_ReturnOrderCreate_Orders[] orders = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_ReturnOrderCreate()
        {
            returnOrder = new HttpReqXml_ReturnOrderCreate_ReturnOrder();
            orders = new HttpReqXml_ReturnOrderCreate_Orders[1];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpReqXml_ReturnOrderCreate);
        }
    }

    /// <summary>
    /// node 'returnOrder' of HTTP request XML
    /// </summary>
    [XmlRoot("returnOrder")]
    public class HttpReqXml_ReturnOrderCreate_ReturnOrder
    {
        #region properties
        /// <summary>
        /// ERP的退货入库单编码
        /// </summary>
        public string returnOrderCode = string.Empty;
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string warehouseCode = string.Empty;
        /// <summary>
        /// 单据类型
        /// </summary>
        public string orderType = string.Empty;
        
        /// <summary>
        /// 用字符串格式来表示订单标记列表：比如VISIT^ SELLER_AFFORD^SYNC_RETURN_BILL 等
        /// , 中间用“^”来隔开 订单标记list (所有字母全部大写) ：
        /// VISIT=上门；SELLER_AFFORD=是否卖家承担运费 (默认是)；SYNC_RETURN_BILL=同时退回发票
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
        /// 物流公司编码, string (50) , SF=顺丰、EMS=标准快递、EYB=经济快件、ZJS=宅急送、YTO=圆通, 
        /// ZTO=中通 (ZTO) 、HTKY=百世汇通、UC=优速、STO=申通、TTKDEX=天天快递  、QFKD=全峰、
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
        public HttpReqXml_ReturnOrderCreate_SenderInfo senderInfo = null;// <senderInfo> <!---->

        /// <summary>
        /// 备注
        /// </summary>
        public string remark = string.Empty;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_ReturnOrderCreate_ReturnOrder()
        {
            senderInfo = new HttpReqXml_ReturnOrderCreate_SenderInfo();
        }

        /// <summary>
        /// 从CWmsReturnOrder对象实体拷贝数据
        /// </summary>
        /// <param name="srcObj">源对象实体</param>
        /// <returns>若拷贝成功则返回string.Empty; 否则返回错误描述</returns>
        public string CopyFrom(CWmsEntity.CWmsReturnOrder srcObj)
        {
            if (null == srcObj)
                return "源实例srcObj为null";
            var logistics = CWmsDataFactory.GetLogisticsBy(TCWmsOrderCategory.EReturnOrder
                , (srcObj.MangoOrder as Mango.Data.MangoReturnOrder).THwuLiu.ToString());
            returnOrderCode = (srcObj.MangoOrder as Mango.Data.MangoReturnOrder).TuiHuoMainID.ToString();
            warehouseCode = CWmsConsts.cStrDefaultWarehouseId;

            orderType = 1>(srcObj.MangoOrder as Mango.Data.MangoReturnOrder).TuiHuoType.Int()? TWmsReturnOrderType.THRK.ToString(): TWmsReturnOrderType.HHRK.ToString();

            orderFlag = (srcObj.WmsOrder as Wms.Data.WmsReturnOrder).OrderFlag.ToString();
            preDeliveryOrderCode = (srcObj.MangoOrder as Mango.Data.MangoReturnOrder).ProductIOputMainId.ToString();
            preDeliveryOrderId = string.Empty;
            logisticsCode = logistics.WmsID;
            logisticsName = logistics.logisticsName;
            returnReason = (srcObj.MangoOrder as Mango.Data.MangoReturnOrder).THYuanYin;
            remark = (srcObj.MangoOrder as Mango.Data.MangoReturnOrder).BeiZhu;
            return string.Empty;
        }
}

    /// <summary>
    /// node 'senderInfo' of HTTP request XML
    /// </summary>
    [XmlRoot("senderInfo")]
    public class HttpReqXml_ReturnOrderCreate_SenderInfo : Interface.CWms.Interfaces.Data.HttpXmlBase_SenderInfoBase
    {
    }

    /// <summary>
    /// node 'orderLine' of HTTP request XML
    /// </summary>
    [XmlRoot("orderLine")]
    public class HttpReqXml_ReturnOrderCreate_Orders
    {
        #region Properties
        /// <summary>
        /// 入库单的行号
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
        /// 仓储系统商品ID
        /// </summary>
        public string itemId = string.Empty;

        /// <summary>
        /// 库存类型, ZP=正品, CC=残次,JS=机损, XS= 箱损，默认为ZP
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
        /// 商品生产日期YYYY-MM-DD
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
        public HttpReqXml_ReturnOrderCreate_Orders()
        {
        }

        /// <summary>
        /// 从CWmsSubReturnOrder对象实体拷贝数据
        /// </summary>
        /// <param name="srcObj">源对象实体</param>
        /// <returns>若拷贝成功则返回string.Empty; 否则返回错误描述</returns>
        public string CopyFrom(CWmsEntity.CWmsSubReturnOrder srcObj)
        {
            if (null == srcObj)
                return "源实例srcObj为null";

            orderLineNo = (srcObj.MangoOrder as Mango.Data.MangoSubReturnOrder).ZiTuihuoID.ToString();
            sourceOrderCode = string.Empty; // TODO: 获取主订单Id
            subSourceOrderCode = (srcObj.MangoOrder as Mango.Data.MangoSubReturnOrder).ZiDingDanID.ToString();
            ownerCode = srcObj.WmsOrderDetail.Owner.WmsID;; // TODO: 获取货主
            itemCode = (srcObj.MangoOrder as Mango.Data.MangoSubReturnOrder).ProductId.ToString();
            inventoryType = srcObj.WmsOrderDetail.InventoryType.ToString();
            planQty = Interface.Utility.CWmsUtility.Decimal2Int((srcObj.MangoOrder as Mango.Data.MangoSubReturnOrder).ProductCount.Decimal()).ToString();
            return string.Empty;
        }
    }
}
