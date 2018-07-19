using C_WMS.Data.Mango.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 退货订单类
    /// </summary>
    class CWmsReturnOrder: CWmsMcocOrder
    {
        /// <summary>
        /// overrided. 返回主退货订单的Id
        /// </summary>
        /// <returns></returns>
        public override string Id
        {
            get
            {
                var mangoOrder = MangoOrder as Mango.Data.MangoReturnOrder;
                if (null == mangoOrder)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("获取退货订单Id失败, MangoOrder={0}", MangoOrder);
                    return string.Empty;
                }
                else
                    return mangoOrder.TuiHuoMainID.Int().ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CWmsReturnOrder()
        {
            OrderType = TOrderType.EReturnOrder;
            MangoOrder = new MangoReturnOrder();
            WmsOrder = new Wms.Data.WmsReturnOrder();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class CWmsReturnOrderHandler : CWmsOrderBaseHandlerBase
    {
        /// <summary>
        /// 根据主出库单ID获取实体
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public override TEntity GetOrder<TEntity>(string pId)
        {
            throw new NotImplementedException("");
        }
    }
}
