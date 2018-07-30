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
    class CWmsSubReturnOrder:CWmsSubOrderBase<CWmsSubReturnOrder, MangoSubReturnOrder, WmsReturnOrderDetail, CWmsSubReturnOrderHandler>
    {
        /// <summary>
        /// overrided，返回子退货订单Id
        /// </summary>
        /// <returns></returns>
        public override string Id
        {
            get { return (MangoOrder as MangoSubReturnOrder).ZiTuihuoID.ToString(); }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsSubReturnOrder() {
            MangoOrder = new MangoSubReturnOrder();
            WmsOrder = new WmsReturnOrderDetail();
        }
        
        /// <summary>
        /// constructor, construct from pMangoOrder and pWmsOrder
        /// </summary>
        /// <param name="pMangoOrder">source object of MangoSubReturnOrder</param>
        /// <param name="pWmsOrder">source object of WmsReturnOrderDetail</param>
        public void CopyFrom(MangoSubReturnOrder pMangoOrder, WmsReturnOrderDetail pWmsOrder)
        {
            MangoOrder.Copy(pMangoOrder);
            WmsOrder.CopyFrom(pWmsOrder);
        }
    }

    /// <summary>
    /// 子单据实体DataHandler类
    /// </summary>
    class CWmsSubReturnOrderHandler : CWmsSubOrderBaseHandlerBase<CWmsSubReturnOrder, MangoSubReturnOrder, CWmsSubReturnOrder>
    {
    }
}
