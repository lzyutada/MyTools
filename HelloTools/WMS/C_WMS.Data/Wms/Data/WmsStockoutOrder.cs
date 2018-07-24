using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    ///  C-WMS系统中的出库单
    /// </summary>
    public class WmsStockoutOrder : WmsOrderBase
    {
        ///// <summary>
        ///// C-WMS系统中的出库订单类型
        ///// </summary>
        //public TWmsStockoutType stockoutType = TWmsStockoutType.EDefaultType;

        /// <summary>
        /// default constructor
        /// </summary>
        public WmsStockoutOrder()
        {
        }

        ///// <summary>
        ///// 返回枚举值的名称
        ///// </summary>
        ///// <returns></returns>
        //public string GetWmsStockoutTypeStr(TWmsStockoutType t)
        //{
        //    switch (t)
        //    {
        //        case TWmsStockoutType.EPTCK: return "PTCK";
        //        case TWmsStockoutType.EDBCK: return "DBCK";
        //        case TWmsStockoutType.EB2BCK: return "B2BCK";
        //        case TWmsStockoutType.ECGTH: return "CGTH";
        //        case TWmsStockoutType.EQTCK:
        //        default: return "QTCK";
        //    }
        //}
    }
}
