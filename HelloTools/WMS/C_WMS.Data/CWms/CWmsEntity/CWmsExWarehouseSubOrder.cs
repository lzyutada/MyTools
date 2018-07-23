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
    class CWmsExWarehouseSubOrder : CWmsSubOrderBase<CWmsExWarehouseSubOrder, MangoSubExwarehouseOrder, WmsStockoutOrderDetail, CWmsExWarehouseSubOrderHandler>
    {
#if false
        /// <summary>
        /// 获取商品的货主
        /// </summary>
        public WmsEntity.WmsOwner Owner { get { return mOwner; } }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        protected WmsEntity.WmsOwner mOwner = null;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// Dict[709]中的主键
        /// </summary>
        protected string WMS_InterfaceId = string.Empty;
#endif
        /// <summary>
        /// 
        /// </summary>
        public override string Id
        {
            get
            {
                return MangoOrder.ProductOutputId.Int().ToString();
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsExWarehouseSubOrder()
        {
            MangoOrder = new MangoSubExwarehouseOrder();
            WmsOrder = new WmsStockoutOrderDetail();
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="pSrc"></param>
        public CWmsExWarehouseSubOrder(CWmsExWarehouseSubOrder pSrc)
        {
            if (null != pSrc)
            {
                MangoOrder = pSrc.MangoOrder;
                WmsOrder = pSrc.WmsOrder;
            }
            else
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}, Invalid null param", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                MangoOrder = new MangoSubExwarehouseOrder();
                WmsOrder = new WmsStockoutOrderDetail();
            }
        }        
    }

    /// <summary>
    /// 
    /// </summary>
    class CWmsExWarehouseSubOrderHandler : CWmsSubOrderBaseHandlerBase<CWmsExWarehouseSubOrder, MangoSubExwarehouseOrder, WmsStockoutOrderDetail>
    {
    }
}
