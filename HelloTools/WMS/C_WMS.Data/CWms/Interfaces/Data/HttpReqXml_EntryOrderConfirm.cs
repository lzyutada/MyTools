using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 入库单确认接口的入参XML序列化对象
    /// </summary>
    [XmlRoot("request")]
    public class HttpReqXml_EntryOrderConfirm : HttpReqXmlBase
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public HttpReqXml_EntryOrderConfirm_EntryOrder entryOrder = null;

        [XmlArray("orderLines"), XmlArrayItem("orderLine")]
        public HttpReqXml_EntryOrderConfirm_OrderLine[] orderLines = null;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_EntryOrderConfirm()
        {
            entryOrder = new HttpReqXml_EntryOrderConfirm_EntryOrder();
            orderLines = new HttpReqXml_EntryOrderConfirm_OrderLine[1];
        }

        /// <summary>
        /// 根据pDesc创建实体
        /// </summary>
        /// <param name="pDesc"></param>
        public HttpReqXml_EntryOrderConfirm(string pDesc)
        {

            var tmpObj = Parse<HttpReqXml_EntryOrderConfirm>(pDesc) as HttpReqXml_EntryOrderConfirm;
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
            return typeof(HttpReqXml_EntryOrderConfirm);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("entryOrder")]
    public class HttpReqXml_EntryOrderConfirm_EntryOrder
    {
        #region Properties
        /// <summary>
        /// 单据总行数。
        /// </summary>
        public string totalOrderLines = string.Empty;

        /// <summary>
        /// 入库单编码
        /// </summary>
        public string entryOrderCode = string.Empty;

        /// <summary>
        /// 货主编码
        /// </summary>
        public string ownerCode = string.Empty;

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string warehouseCode = string.Empty;

        /// <summary>
        /// 仓储系统入库单ID
        /// </summary>
        public string entryOrderId = string.Empty;

        /// <summary>
        /// 入库单类型 
        /// </summary>
        public string entryOrderType = string.Empty;

        /// <summary>
        /// 外部业务编码
        /// </summary>
        public string outBizCode = string.Empty;

        /// <summary>
        /// 支持出入库单多次收货
        /// 多次收货后确认时
        /// 0 表示入库单最终状态确认；
        /// 1 表示入库单中间状态确认；
        /// 每次入库传入的数量为增量。
        /// </summary>
        public string confirmType = string.Empty;

        /// <summary>
        /// 入库单状态, 必填(NEW-未开始处理, ACCEPT-仓库接单 , PARTFULFILLED-部分收货完成, FULFILLED-收货完成
        /// , EXCEPTION-异常, CANCELED-取消, CLOSED-关闭, REJECT-拒单, CANCELEDFAIL-取消失败) ,  (只传英文编码)
        /// </summary>
        public string status = string.Empty;

        /// <summary>
        /// 操作时间,YYYY-MM-DD HH:MM:SS
        /// </summary>
        public string operateTime = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string remark = string.Empty;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_EntryOrderConfirm_EntryOrder()
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("orderLine")]
    public class HttpReqXml_EntryOrderConfirm_OrderLine
    {
        #region Properties
        /// <summary>
        /// 外部业务编码
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
        /// 仓储系统商品ID
        /// </summary>
        public string itemId = string.Empty;

        /// <summary>
        /// 商品名称
        /// </summary>
        public string itemName = string.Empty;

        /// <summary>
        /// 库存类型
        /// </summary>
        public string inventoryType = string.Empty;

        /// <summary>
        /// 应收数量
        /// </summary>
        public string planQty = string.Empty;

        /// <summary>
        /// 实收数量
        /// </summary>
        public string actualQty = string.Empty;

        /// <summary>
        /// 批次编码
        /// </summary>
        public string batchCode = string.Empty;

        /// <summary>
        /// 商品生产日期
        /// </summary>
        public string productDate = string.Empty;

        /// <summary>
        /// 商品过期日期
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
        /// 备注
        /// </summary>
        public string remark = string.Empty;
        #endregion Properties

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_EntryOrderConfirm_OrderLine()
        {
            batchs = new HttpReqXml_Batch[1];
        }
    }
}
