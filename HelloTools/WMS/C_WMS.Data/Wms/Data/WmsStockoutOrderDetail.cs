using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// C-WMS系统中的出库订单详细。
    /// </summary>
    class WmsStockoutOrderDetail : WmsOrderDetailBase
    {
        /// <summary>
        /// 
        /// </summary>
        public string purchaseOrderCode = string.Empty;
        public string entryOrderCode = string.Empty;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WmsStockoutOrderDetail()
        {
            mOwner = new WmsOwner();
        }
    }
}
