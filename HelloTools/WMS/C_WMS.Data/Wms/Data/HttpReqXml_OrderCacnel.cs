using C_WMS.Data.CWms.CWmsEntity;
using C_WMS.Data.Mango.Data;
using C_WMS.Interface.Utility;
using System;
using System.Xml.Serialization;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// 单据取消接口的HTTP请求XML对应的序列化类
    /// </summary>
    [XmlRoot("request")]
    public class HttpReqXml_OrderCacnel : HttpReqXmlBase
    {
        #region properties
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string warehouseCode = string.Empty;

        /// <summary>
        /// 货主编码
        /// </summary>
        public string ownerCode = string.Empty;

        /// <summary>
        /// 单据编码
        /// </summary>
        public string orderCode = string.Empty;

        /// <summary>
        /// 仓储系统单据编码
        /// </summary>
        public string orderId = string.Empty;

        /// <summary>
        /// 单据类型,  JYCK= 一般交易出库单，HHCK= 换货出库 ，BFCK= 补发出库 PTCK = 普通出库单，DBCK=调拨出库, 
        /// B2BRK=B2B入库，B2BCK=B2B出库，QTCK=其他出库， SCRK=生产入库，LYRK=领用入库，CCRK=残次品入库,
        /// CGRK=采购入库 ，DBRK= 调拨入库 ，QTRK= 其他入库 ，XTRK= 销退入库 HHRK = 换货入库 CNJG= 仓内加工单
        /// </summary>
        public string orderType = string.Empty;

        /// <summary>
        /// 取消原因
        /// </summary>
        public string cancelReason = string.Empty;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_OrderCacnel() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpReqXml_OrderCacnel);
        }        
    }
}
