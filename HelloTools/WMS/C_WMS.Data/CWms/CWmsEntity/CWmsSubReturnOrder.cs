using C_WMS.Data.Mango.Data;
using C_WMS.Data.Wms.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 子退货订单类
    /// </summary>
    public class CWmsSubReturnOrder:CWmsSubOrderBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsSubReturnOrder() {
            mMangoOrder = new MangoSubReturnOrder();
            mWmsOrder = new Wms.Data.WmsReturnOrderDetail();
        }
        
        /// <summary>
        /// constructor, construct from pMangoOrder and pWmsOrder
        /// </summary>
        /// <param name="pMangoOrder">source object of MangoSubReturnOrder</param>
        /// <param name="pWmsOrder">source object of WmsReturnOrderDetail</param>
        public void CopyFrom(MangoSubReturnOrder pMangoOrder, WmsReturnOrderDetail pWmsOrder)
        {
            (MangoOrder as MangoSubReturnOrder).CopyFrom(pMangoOrder);
            (WmsOrderDetail as WmsReturnOrderDetail).CopyFrom(pWmsOrder);
        }

        /// <summary>
        /// overrided，返回子退货订单Id
        /// </summary>
        /// <returns></returns>
        public override string GetId()
        {
            return (MangoOrder as MangoSubReturnOrder).ZiTuihuoID.ToString();
        }
    }
}
