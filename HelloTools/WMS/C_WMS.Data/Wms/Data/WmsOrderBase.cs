using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// C-WMS系统中的单据基类
    /// </summary>
    public class WmsOrderBase : WmsEntityBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public WmsOrderBase() { }

        /// <summary>
        /// order category
        /// </summary>
        public TWmsOrderType OrderType = TWmsOrderType.EDefaultType;

        /// <summary>
        /// 设置单据类型
        /// </summary>
        /// <param name="pType">芒果商城中的订单类型TMangoOrderType</param>
        virtual public void SetWmsStockoutOrderType(TMangoOrderType pType)
        {
            switch (pType)
            {
                case TMangoOrderType.EBMDD: OrderType = TWmsOrderType.B2BCK; break; // 部门订单
                case TMangoOrderType.EZSBD: // 装饰补单
                case TMangoOrderType.EWLCK: // 芒果网络为货主，无订单出库
                case TMangoOrderType.EGRGM: OrderType = TWmsOrderType.QTCK; break; // 个人购买
                case TMangoOrderType.EBPSQ: // 备品申请
                case TMangoOrderType.EFLSQ: // 福利申请
                case TMangoOrderType.EUnknown:   // 未知
                default: OrderType = TWmsOrderType.EUnknown; break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pType"></param>
        virtual public void SetWmsOrderType(TWmsOrderType pType) { OrderType = pType; }
    }
}
