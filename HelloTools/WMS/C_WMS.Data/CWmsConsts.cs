using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data
{
    /// <summary>
    /// 常量定义
    /// </summary>
    public class CWmsConsts
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
#if false
        /// <summary>
        /// 默认C-WMS货主ID //TODO配置之一
        /// </summary>
        public const string cStrDefaultOwnerId = cStrWmsOwnerCode_MGWL;

        /// <summary>
        /// 默认C-WMS货主名称
        /// </summary>
        public const string cStrDefaultOwnerName = cStrWmsOwnerName_MGWL;
#endif
        /// <summary>
        /// 允许部分发货
        /// </summary>
        public const string cStrStockoutPartialShipmentY = "Y";

        /// <summary>
        /// 不允许部分发货
        /// </summary>
        public const string cStrStockoutPartialShipmentN = "N";
        public const string cStrStockoutPartialShipmentDefault = cStrStockoutPartialShipmentN;

        /// <summary>
        /// 通过WCF获取数据时，默认的page size.
        /// </summary>
        public const int cIntDefaultWcfQueryPageSize = 399;


#if false
        /// <summary>
        /// 商品同步接口默认操作类型
        /// </summary>
        public const string sStrApiItemsSyncActionTypeUpdate = "update";
        public const int cInt芒果商城订单群Id = 18140;
        public const int cInt芒果钉秘Id = 11007;
        public const string cStrHttpMethodPost = "POST";
        public const string cStrHttpMethodGet = "GET";
        public const string cStrHttpConentType_TxtXml = "text/xml";

        /// <summary>
        /// 货主编码-芒果网络
        /// </summary>
        public const string cStrWmsOwnerCode_MGWL = "LNMGWL";//"蓝江";

        /// <summary>
        /// 货主名称-芒果网络
        /// </summary>
        public const string cStrWmsOwnerName_MGWL = "辽宁芒果网络股份有限公司";//"辽宁芒果网络股份有限公司";

        /// <summary>
        /// 货主编码-蓝江智家
        /// </summary>
        public const string cStrWmsOwnerCode_LJZJ = "LJZJKJ";//"WMSOWNER_LJZJ";

        /// <summary>
        /// 货主名称-蓝江智家
        /// </summary>
        public const string cStrWmsOwnerName_LJZJ = "沈阳市蓝江智家科技有限公司";//"沈阳市蓝江智家科技有限公司";

        /// <summary>
        /// 默认承运商Code，联调用
        /// </summary>
        public const string cStrWmsLogisticsDefaultCode = cStrWmsLogisticsZYWLCode;

        /// <summary>
        /// 默认承运商名称，联调用
        /// </summary>
        public const string cStrWmsLogisticsDefaultName = cStrWmsLogisticsZYWLName;

        /// <summary>
        /// 承运商Id-自提
        /// </summary>
        public const string cStrWmsLogisticsZTCode = cStrWmsLogisticsDefaultCode; // "MG_LOGISTICSCODE_ZITI";

        /// <summary>
        /// 承运商name-自提
        /// </summary>
        public const string cStrWmsLogisticsZTName = cStrWmsLogisticsDefaultCode; // "自提";

        /// <summary>
        /// 承运商Id-自营物流
        /// </summary>
        public const string cStrWmsLogisticsZYWLCode = "MGWL"; // "MG_LOGISTICSCODE_ZYWUL";

        /// <summary>
        /// 承运商name-自营物流
        /// </summary>
        public const string cStrWmsLogisticsZYWLName = "芒果物流"; // "自营物流";

        /// <summary>
        /// 承运商Id-其他
        /// </summary>
        public const string cStrWmsLogisticsQTCode = "DSF"; // "MG_LOGISTICSCODE_QT";

        /// <summary>
        /// 承运商name-其他
        /// </summary>
        public const string cStrWmsLogisticsQTName = "第三方物流"; // "其他物流";



        /// <summary>
        /// 承运商Id-其他
        /// </summary>
        public const string cStrDefaultWarehouseName = "芒果网络沈阳仓"; // "MG_LOGISTICSCODE_QT";

        /// <summary>
        /// 承运商Id-其他
        /// </summary>
        public const string cStrDefaultWarehouseId = "芒果网络沈阳仓"; // "MG_LOGISTICSCODE_QT";
#endif
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }
}
