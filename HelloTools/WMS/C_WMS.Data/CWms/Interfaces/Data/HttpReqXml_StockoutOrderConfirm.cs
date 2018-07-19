using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 出库单确认接口的入参XML序列化对象
    /// </summary>
    [XmlRoot("request")]
    public class HttpReqXml_StockoutOrderConfirm: HttpReqXmlBase
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public HttpReqXml_StockoutOrderConfirm_DeliveryOrder deliveryOrder = null;
        public HttpReqXml_StockoutOrderConfirm_Package packages = null;
        [XmlArray("orderLines"), XmlArrayItem("orderLine")]
        public HttpReqXml_StockoutOrderConfirm_OrderLine[] orderLines = null;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderConfirm() {
            deliveryOrder = new HttpReqXml_StockoutOrderConfirm_DeliveryOrder();
            packages = new HttpReqXml_StockoutOrderConfirm_Package();
            orderLines = new HttpReqXml_StockoutOrderConfirm_OrderLine[1];
        }

        /// <summary>
        /// 根据pDesc创建实体
        /// </summary>
        /// <param name="pDesc"></param>
        public HttpReqXml_StockoutOrderConfirm(string pDesc)
        {
            var tmpObj = Parse<HttpReqXml_StockoutOrderConfirm>(pDesc) as HttpReqXml_StockoutOrderConfirm;
            if (null != tmpObj)
            {
                deliveryOrder = tmpObj.deliveryOrder;
                packages = tmpObj.packages;
                orderLines = tmpObj.orderLines;
            }
        }

        /// <summary>
        /// overrided.
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpReqXml_StockoutOrderConfirm);
        }
    }

    /// <summary>
    /// 出库单确认接口的入参XML序列化对象
    /// </summary>
    [XmlRoot("deliveryOrder")]
    public class HttpReqXml_StockoutOrderConfirm_DeliveryOrder
    {
        #region Properties
        /// <summary>
        /// 单据总行数，当单据需要分多个请求发送时，发送方需要将totalOrderLines填入.
        /// 接收方收到后，根据实际接收到的条数和totalOrderLines进行比对，如果小于，则继续等待接收请求。如果等于，则表示该单据的所有请求发送完成。
        /// </summary>
        public string totalOrderLines = string.Empty;

        /// <summary>
        /// 出库单号
        /// </summary>
        public string deliveryOrderCode = string.Empty;

        /// <summary>
        /// 仓储系统出库单号
        /// </summary>
        public string deliveryOrderId = string.Empty;

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string warehouseCode = string.Empty;

        /// <summary>
        /// 出库单类型, string (50)，PTCK=普通出库单（退仓），DBCK=调拨出库 ，B2BCK=B2B出库，QTCK=其他出库，，CGTH=采购退货出库单，必填
        /// </summary>
        public string orderType = string.Empty;

        /// <summary>
        /// 出库单状态, (NEW-未开始处理, ACCEPT-仓库接单 , PARTDELIVERED-部分发货完成, DELIVERED-发货完成,
        /// EXCEPTION-异常, CANCELED-取消, CLOSED-关闭, REJECT-拒单, CANCELEDFAIL-取消失败) , (只传英文编码)
        /// </summary>
        public string status = string.Empty;

        /// <summary>
        /// 外部业务编码, 外部业务编码, 同一请求的唯一性标示编码。ISV对于同一请求，分配一个唯一性的编码。用来保证因为网络等原因导致重复传输，请求不会被重复处理，条件必填，条件为一单需要多次确认时
        /// </summary>
        public string outBizCode = string.Empty;

        /// <summary>
        /// 支持出库单多次发货, int。多次发货后确认时：0-表示发货单最终状态确认；1-表示发货单中间状态确认。
        /// </summary>
        public string confirmType = string.Empty;

        /// <summary>
        /// 物流公司编码 SF=顺丰、EMS=标准快递、EYB=经济快件、ZJS=宅急送、YTO=圆通 、ZTO=中通(ZTO) 、
        /// HTKY=百世汇通、UC=优速、STO=申通、TTKDEX=天天快递 、QFKD=全峰、FAST=快捷、POSTB=邮政小包 、
        /// GTO=国通、YUNDA=韵达、JD=京东配送、DD=当当宅配、OTHER=其他，(只传英文编码)
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
        /// 订单完成时间
        /// </summary>
        public string orderConfirmTime = string.Empty;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderConfirm_DeliveryOrder() { }
    }

    /// <summary>
    /// 包装类
    /// </summary>
    [XmlRoot("packages")]
    public class HttpReqXml_StockoutOrderConfirm_Package
    {
        #region Properties
        /// <summary>
        /// 物流公司名称
        /// </summary>
        public string logisticsName = string.Empty;

        /// <summary>
        /// 运单号
        /// </summary>
        public string expressCode = string.Empty;

        /// <summary>
        /// 包裹编号
        /// </summary>
        public string packageCode = string.Empty;

        /// <summary>
        /// 包裹长度(厘米)
        /// </summary>
        public string length = string.Empty;

        /// <summary>
        /// 包裹宽度(厘米)
        /// </summary>
        public string width = string.Empty;

        /// <summary>
        /// 包裹高度(厘米)
        /// </summary>
        public string height = string.Empty;

        /// <summary>
        /// 包裹重量
        /// </summary>
        public string weight = string.Empty;

        /// <summary>
        /// 包裹体积(升, L)
        /// </summary>
        public string volume = string.Empty;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public HttpReqXml_StockoutOrderConfirm_Package_PackageMaterial[] packageMaterialList = null;
        public HttpReqXml_StockoutOrderConfirm_Package_Item[] items = null;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderConfirm_Package() {
            packageMaterialList = new HttpReqXml_StockoutOrderConfirm_Package_PackageMaterial[1];
            items = new HttpReqXml_StockoutOrderConfirm_Package_Item[1];
        }
    }

    /// <summary>
    /// 包材类
    /// </summary>
    [XmlRoot("packageMaterial")]
    public class HttpReqXml_StockoutOrderConfirm_Package_PackageMaterial
    {
        /// <summary>
        /// 包材型号
        /// </summary>
        public string type = string.Empty;

        /// <summary>
        /// 包材的数量
        /// </summary>
        public string quantity = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderConfirm_Package_PackageMaterial() { }
    }

    /// <summary>
    /// 包装下商品项目类
    /// </summary>
    [XmlRoot("item")]
    public class HttpReqXml_StockoutOrderConfirm_Package_Item
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        public string itemCode = string.Empty;

        /// <summary>
        /// 商品仓储系统编码
        /// </summary>
        public string itemId = string.Empty;

        /// <summary>
        /// 包裹内该商品的数量
        /// </summary>
        public string quantity = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderConfirm_Package_Item() { }
    }

    /// <summary>
    /// 出库单确认接口的入参XML序列化对象
    /// </summary>
    [XmlRoot("orderLine")]
    public class HttpReqXml_StockoutOrderConfirm_OrderLine
    {
        #region Properties
        /// <summary>
        /// 外部业务编码, 消息ID, 用于去重，当单据需要分批次发送时使用
        /// </summary>
        public string outBizCode = string.Empty;
        
        /// <summary>
        /// 单据行号
        /// </summary>
        public string orderLineNo = string.Empty;
        
        /// <summary>
        /// 商品编码
        /// </summary>
        public string itemCode = string.Empty;
        
        /// <summary>
        /// 商品仓储系统编码
        /// </summary>
        public string itemId = string.Empty;

                    /// <summary>
        /// 商品名称
        /// </summary>
        public string itemName = string.Empty;
            
        /// <summary>
        /// 库存类型，ZP=正品, CC=残次,JS=机损, XS= 箱损，默认为ZP
        /// </summary>
        public string inventoryType = string.Empty;

        /// <summary>
        /// 实发商品数量
        /// </summary>
        public string actualQty = string.Empty;

        /// <summary>
        /// 批次编号
        /// </summary>
        public string batchCode = string.Empty;

        /// <summary>
        /// 生产日期
        /// </summary>
        public string productDate = string.Empty;

        /// <summary>
        /// 过期日期
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
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_StockoutOrderConfirm_OrderLine()
        {
            batchs = new HttpReqXml_Batch[1];
        }
    }
}
