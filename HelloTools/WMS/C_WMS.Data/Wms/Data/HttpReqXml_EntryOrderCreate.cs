using C_WMS.Interface.CWms.CWmsEntity;
using C_WMS.Data.Mango;
using C_WMS.Data.Mango.Data;
using C_WMS.Interface.Utility;
using MangoMis.Frame.ThirdFrame;
using MangoMis.MisFrame.Helper;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Reflection;
using C_WMS.Data.CWms.CWmsEntity;
using C_WMS.Data.CWms;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// 入库单创建接口的HTTP请求XML对应的序列化类
    /// </summary>
    [XmlRoot("request")]
    class HttpReqXml_EntryOrderCreate : HttpReqXmlBase
    {
        /// <summary>
        /// node 'entryOrder' in request XML body
        /// </summary>
        public HttpReqXml_EntryOrderCreate_EntryOrder entryOrder = null;

        /// <summary>
        /// node 'orderLines' and 'orderLine' in request XML body
        /// </summary>
        [XmlArray("orderLines"), XmlArrayItem("orderLine")]
        public HttpReqXml_EntryOrderCreate_Order[] orders = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_EntryOrderCreate()
        {
            entryOrder = new HttpReqXml_EntryOrderCreate_EntryOrder();
            orders = new HttpReqXml_EntryOrderCreate_Order[1];
        }

        /// <summary>
        /// construct by object of CWmsEntryOrder
        /// </summary>
        /// <param name="pOrder">source order.</param>
        public HttpReqXml_EntryOrderCreate(CWmsEntryOrder pOrder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// overrided.
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpReqXml_EntryOrderCreate);
        }
    }

    /// <summary>
    /// node 'entryOrder' of HTTP request XML
    /// </summary>
    [XmlRoot("entryOrder")]
    class HttpReqXml_EntryOrderCreate_EntryOrder
    {
        #region Properties
        /// <summary>
        /// 单据总行数
        /// </summary>
        public string totalOrderLines = string.Empty;

        /// <summary>
        /// 入库单号, string (50) , 必填
        /// </summary>
        public string entryOrderCode = string.Empty; // 

        /// <summary>
        /// 货主编码, string (50) , 必填
        /// </summary>
        public string ownerCode = string.Empty; // 

        /// <summary>
        /// 采购单号，string(50)，当orderType=CGRK时，使用。
        /// </summary>
        public string purchaseOrderCode = string.Empty; // 

        /// <summary>
        /// 仓库编码, string(50)，必填，统仓统配等无需ERP指定仓储编码的情况填OTHER
        /// </summary>
        public string warehouseCode = string.Empty; // 

        /// <summary>
        /// 订单创建时间，string(19)，YYYY-MM-DD HH:MM:SS
        /// </summary>
        public string orderCreateTime = string.Empty; // 

        /// <summary>
        /// 业务类型
        /// </summary>
        public string orderType = string.Empty; // 

        /// <summary>
        /// 预期到货时间, string (19) , YYYY-MM-DD HH:MM:SS
        /// </summary>
        public string expectStartTime = string.Empty; // 


        /// <summary>
        /// 最迟预期到货时间, string (19) , YYYY-MM-DD HH:MM:SS
        /// </summary>
        public string expectEndTime = string.Empty; // 

        /// <summary>
        /// 物流公司编码, string (50)
        /// </summary>
        public string logisticsCode = string.Empty; // 

        /// <summary>
        /// 物流公司名称, string(200) 
        /// </summary>
        public string logisticsName = string.Empty; // 

        /// <summary>
        /// 运单号, string(50) 
        /// </summary>
        public string expressCode = string.Empty; // 

        /// <summary>
        /// 供应商编码 string(50)
        /// </summary>
        public string supplierCode = string.Empty; // 

        /// <summary>
        /// 供应商名称 string(200) 
        /// </summary>
        public string supplierName = string.Empty; // 

        /// <summary>
        /// 操作员编码, string(50) 
        /// </summary>
        public string operatorCode = string.Empty; // 

        /// <summary>
        /// 操作员名称, string(50) 
        /// </summary>
        public string operatorName = string.Empty; // 

        /// <summary>
        /// 操作时间,  string (19) , YYYY-MM-DD HH:MM:SS
        /// </summary>
        public string operateTime = string.Empty; // 

        /// <summary>
        /// 发件人信息
        /// </summary>
        public HttpReqXml_EntryOrderCreate_SenderInfo senderInfo = null;// 

        /// <summary>
        /// 收件人信息
        /// </summary>
        public HttpReqXml_EntryOrderCreate_ReceiverInfo receiverInfo = null; //    <receiverInfo> <!---->

        /// <summary>
        /// 备注
        /// </summary>
        public string remark = string.Empty; // , string (500) </remark>  

        /// <summary>
        /// 扩展属性 
        /// </summary>
        public HttpReqXml_EntryOrderCreate_ExtendProps extendProps = null; //      <extendProps>
        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public HttpReqXml_EntryOrderCreate_EntryOrder()
        {
            senderInfo = new HttpReqXml_EntryOrderCreate_SenderInfo();
            receiverInfo = new HttpReqXml_EntryOrderCreate_ReceiverInfo();
            extendProps = new HttpReqXml_EntryOrderCreate_ExtendProps();
        }

        /// <summary>
        /// 从CWmsEntryOrder实例中拷贝订单信息
        /// </summary>
        /// <param name="pSrc">源订单实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public string CopyFrom(CWmsEntryOrder pSrc)
        {
#if !C_WMS_V1
            throw new NotFiniteNumberException();
#else
            if (null == pSrc)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("Failed, {0}.{1}(pSrc={2})", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pSrc);
                return "非法参数，pSrc为null";
            }
            try
            {
                // 获取承运商信息
                var logistics = pSrc.Handler.GetLogistics(pSrc);// CWmsDataFactory.GetLogisticsBy(TCWmsOrderCategory.EEntryOrder, string.Empty);

                totalOrderLines = pSrc.SubOrders.Count.ToString();
                entryOrderCode = pSrc.Id;
                ownerCode = CWmsDataFactory.GetOwner(pSrc.MangoOrder.WarehouseId.ToString()).Code; // TODO: 根据仓库Id判断货主
                purchaseOrderCode = string.Empty; // 无法确定采购订单Id（多个主采购订单的子单据都可以加到一个主入库单中）
                warehouseCode = CWmsDataFactory.GetWarehouse(pSrc.MangoOrder.WarehouseId.ToString()).Name; // CWmsConsts.cStrDefaultWarehouseId;  // TODO: get warehous ID
                orderCreateTime = pSrc.MangoOrder.AddTime.ToString();
                orderType = TWmsOrderType.QTRK.ToString(); // TODO: need to identify purchase entry and no-purchase entry for inventory adding from nowhere
                operatorCode = MangoMis.Frame.Frame.CommonFrame.userid.ToString();  // 操作人Id为当前登录用户
                operatorName = MangoMis.Frame.Frame.CommonFrame.userid.User().UserName2; // 获取人员名称
                logisticsCode = logistics.WmsID;
                logisticsName = logistics.logisticsName;
                operateTime = DateTime.Now.ToString();  // 操作时间为当前时间
                remark = pSrc.MangoOrder.Remark;
                return string.Empty;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "!!Exception in {0}.{1}({2})", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pSrc);
                return ex.Message;
            }
#endif
        }
    }

    /// <summary>
    /// node 'senderInfo' of HTTP request XML
    /// </summary>
    [XmlRoot("senderInfo")]
    class HttpReqXml_EntryOrderCreate_SenderInfo : Interface.CWms.Interfaces.Data.HttpXmlBase_SenderInfoBase
    {
    }

    /// <summary>
    /// node 'receiverInfo' of HTTP request XML
    /// </summary>
    [XmlRoot("receiverInfo")]
    class HttpReqXml_EntryOrderCreate_ReceiverInfo : Interface.CWms.Interfaces.Data.HttpXmlBase_SenderInfoBase
    {
    }

    /// <summary>
    /// node 'extendProps' of HTTP request XML
    /// </summary>
    [XmlRoot("extendProps")]
    class HttpReqXml_EntryOrderCreate_ExtendProps : Interface.CWms.Interfaces.Data.HttpXmlBase_ExtendProps
    {
    }

    /// <summary>
    /// node 'orderLine' of HTTP request XML
    /// </summary>
    [XmlRoot("orderLine")]
    class HttpReqXml_EntryOrderCreate_Order
    {
#region properties
        /// <summary>
        /// 外部业务编码, 消息ID, 用于去重，当单据需要分批次发送时使用
        /// </summary>
        public string outBizCode = string.Empty; //  </outBizCode>

        /// <summary>
        /// 入库单的行号
        /// </summary>
        public string orderLineNo = string.Empty; // ，string（50）</orderLineNo>

        /// <summary>
        /// 货主编码
        /// </summary>
        public string ownerCode = string.Empty; // , string (50) , 必填</ownerCode>

        /// <summary>
        /// 商品编码
        /// </summary>
        public string itemCode = string.Empty; // , string (50) , 必填</itemCode>

        /// <summary>
        /// 仓储系统商品ID
        /// </summary>
        public string itemId = string.Empty; // ,string(50)，条件必填</itemId>

        /// <summary>
        /// 商品名称
        /// </summary>
        public string itemName = string.Empty; // , string (200) </itemName>

        /// <summary>
        /// 应收商品数量
        /// </summary>
        public string planQty = string.Empty; // , int, 必填</planQty>  

        /// <summary>
        /// 商品属性
        /// </summary>
        public string skuProperty = string.Empty; // , string (200) </skuProperty>

        /// <summary>
        /// 采购价
        /// </summary>
        public string purchasePrice = string.Empty; // , double (18, 2) </purchasePrice>

        /// <summary>
        /// 零售价
        /// </summary>
        public string retailPrice = string.Empty; // , double (18, 2) </retailPrice>

        /// <summary>
        /// 库存类型
        /// </summary>
        public string inventoryType = string.Empty; // ，string (50) , ZP=正品, CC=残次,JS=机损, XS= 箱损，默认为ZP</inventoryType>

        /// <summary>
        /// 商品生产日期 YYYY-MM-DD
        /// </summary>
        public string productDate = string.Empty; // </productDate>

        /// <summary>
        /// 商品过期日期YYYY-MM-DD
        /// </summary>
        public string expireDate = string.Empty; // </expireDate>

        /// <summary>
        /// 生产批号
        /// </summary>
        public string produceCode = string.Empty; // , string (50) </produceCode>

        /// <summary>
        /// 批次编码
        /// </summary>
        public string batchCode = string.Empty; // , string (50) </batchCode>

        /// <summary>
        /// 扩展属性 
        /// </summary>
        public HttpReqXml_EntryOrderCreate_ExtendProps extendProps = null; //      <extendProps>
#endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public HttpReqXml_EntryOrderCreate_Order()
        {
            extendProps = new HttpReqXml_EntryOrderCreate_ExtendProps();
        }

        /// <summary>
        /// 从CWmsSubEntryOder实例中拷贝订单信息
        /// </summary>
        /// <param name="srcOrder">源订单实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public string CopyFrom(CWmsSubEntryOder srcOrder)
        {
#if !C_WMS_V1
            throw new NotFiniteNumberException();
#else
            if (null == srcOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("Failed in {0}.{1}(pSrc={2})", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, srcOrder);
                return "非法入参，srcOrder为null";
            }
            try
            {
                outBizCode = srcOrder.WmsOrder.OutBizCode; // 外部业务编码, 消息ID, 用于去重，当单据需要分批次发送时使用
                orderLineNo = srcOrder.Id; // 入库单的行号
                ownerCode = CWmsDataFactory.GetOwner(srcOrder.MangoOrder.WarehouseId.ToString()).Code; // 根据仓库Id判断货主
                itemCode = srcOrder.Product.ItemCode; // ret.Append("itemCode={0}", itemCode); // 商品编码
                itemName = srcOrder.Product.MangoProduct.Title; // 商品名称
                planQty = srcOrder.WmsOrder.planQty.ToString(); // 获取应收数量
                inventoryType = srcOrder.WmsOrder.InventoryType.ToString();
                skuProperty = srcOrder.Product.MangoProduct.GuiGe;
                purchasePrice = srcOrder.MangoOrder.ProductPrice.ToString(); // 采购价
                return string.Empty;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "!!Exception in {0}.{1}({2})", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, srcOrder);
                return ex.Message;
            }
#endif
        }
    }
}
