using C_WMS.Data.Mango.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 采购订单类
    /// </summary>
    class CWmsPurchaseOrder:CWmsOrderBase
    {
        /// <summary>
        /// overrided。返回芒果商城采购订单Id
        /// </summary>
        /// <returns></returns>
        public override string GetId()
        {
            return (MangoOrder as MangoPurchaseOrder).ProductBuyMainId.ToString();
        }
    }
}
