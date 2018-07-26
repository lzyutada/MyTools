using C_WMS.Data.Mango.Data;
using C_WMS.Data.Wms.Data;
using MangoMis.Frame.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 出库子订单
    /// </summary>
    class CWmsSubStockoutOrder : CWmsSubOrderBase<CWmsSubStockoutOrder, MangoSubStockoutOrder, WmsStockoutOrderDetail, CWmsExWarehouseSubOrderHandler>
    {
        /// <summary>
        /// get id of order
        /// </summary>
        public override string Id { get { return MangoOrder.ProductOutputId.ToString(); } }

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsSubStockoutOrder()
        {
            MangoOrder = new MangoSubStockoutOrder();
            WmsOrder = new WmsStockoutOrderDetail();
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="pSrc"></param>
        public CWmsSubStockoutOrder(CWmsSubStockoutOrder pSrc)
        {
            if (null != pSrc)
            {
                MangoOrder = pSrc.MangoOrder;
                WmsOrder = pSrc.WmsOrder;
            }
            else
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}, Invalid null param", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                MangoOrder = new MangoSubStockoutOrder();
                WmsOrder = new WmsStockoutOrderDetail();
            }
        }        
    }

    /// <summary>
    /// 
    /// </summary>
    class CWmsExWarehouseSubOrderHandler : CWmsSubOrderBaseHandlerBase<CWmsSubStockoutOrder, MangoSubStockoutOrder, WmsStockoutOrderDetail>
    {
    }
}
