using C_WMS.Data.Mango.Data;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.Wms.Data;
using MangoMis.Frame.Helper;
using System;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 退货订单类
    /// </summary>
    class CWmsReturnOrder: CWmsOrderBase<CWmsReturnOrder, MangoReturnOrder, WmsReturnOrder, CWmsSubReturnOrder, CWmsReturnOrderHandler>
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
#if false
            OrderType = TOrderType.EReturnOrder;
#endif
            MangoOrder = new MangoReturnOrder();
            WmsOrder = new WmsReturnOrder();
        }

        protected override CWmsReturnOrderHandler NewHandler()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class CWmsReturnOrderHandler : CWmsOrderBaseHandlerBase<CWmsReturnOrder, MangoReturnOrder, WmsReturnOrder, CWmsSubReturnOrder, CWmsReturnOrderHandler>
    {
        /// <summary>
        /// 根据主入库单ID获取实体
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        static public CWmsReturnOrder NewOrder(string pId)
        {
            throw new NotImplementedException("");
        }

        override protected int Update709(string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            return Dict709Handle.UpdateRow_Order(TDict709_Value.EReturnOrder, pEid, pEsId, pUpdateOk, pDel, out pMsg);
        }

        override protected int UpdateA709(string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            return Dict709Handle.UpdateRowA_Order(TDict709_Value.EReturnOrder, pEid, pEsId, pUpdateOk, pDel, out pMsg);
        }

        /// <summary>
        /// get entity of WmsLogistics as the logistics of this entryorder represented by _order.
        /// -or- return null if failed in method executation.
        /// </summary>
        /// <returns></returns>
        public WmsLogistics GetLogistics(CWmsReturnOrder pOrder)
        {
            return base.GetLogistics(pOrder);
        }
    }
}
