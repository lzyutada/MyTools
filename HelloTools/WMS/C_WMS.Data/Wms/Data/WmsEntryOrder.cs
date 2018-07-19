using C_WMS.Interface.CWms.CWmsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// C-WMS系统中的采购入库订单
    /// </summary>
    public class WmsEntryOrder : WmsOrderBase
    {
        ///// <summary>
        ///// 货主
        ///// </summary>
        //public WmsOwner owner = null;

        ///// <summary>
        ///// 业务类型 (SCRK=生产入库，LYRK=领用入库，CCRK=残次品入库，CGRK=采购入库，DBRK=调拨入库, QTRK=其他入库，B2BRK=B2B入库
        ///// </summary>
        //public TWmsOrderType orderType = TWmsOrderType.EDefaultType;

        ///// <summary>
        ///// 发件人
        ///// </summary>
        //public CWmsAgentBase sender = null;

        ///// <summary>
        ///// 收件人
        ///// </summary>
        //public CWmsAgentBase receiver = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WmsEntryOrder()
        {
        }

        /// <summary>
        /// 设置单据类型
        /// </summary>
        /// <param name="pType">芒果商城中的订单类型TMangoOrderType</param>
        override public void SetWmsOrderType(TWmsOrderType pType)
        {
            OrderType = pType;
        }
    }
}
