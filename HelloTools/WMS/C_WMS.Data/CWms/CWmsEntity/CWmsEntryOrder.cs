using C_WMS.Data.Mango.Data;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.Wms.Data;
using MangoMis.Frame.Helper;
using MisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C_WMS.Data.Mango;
using MangoMis.MisFrame.Frame;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 入库订单
    /// </summary>
    class CWmsEntryOrder : CWmsOrderBase<CWmsEntryOrder, MangoEntryOrder, WmsEntryOrder, CWmsSubEntryOder, CWmsEntryOrderHandler>
    {
        /// <summary>
        /// overrided. 返回芒果商城中主入库订单的Id
        /// </summary>
        /// <returns></returns>
        public override string Id
        {
            get
            {
#if false
                //var mangoOrder = MangoOrder as Mango.Data.MangoEntryOrder;
                if (null == MangoOrder)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("获取主入库单Id失败, MangoOrder={0}", MangoOrder);
                    return string.Empty;
                }
                else
#endif
                    return MangoOrder.ProductInputMainId.Int().ToString();
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CWmsEntryOrder()
        {
            OrderType = TCWmsOrderType.EEntryOrder;
            MangoOrder = new MangoEntryOrder();
            WmsOrder = new WmsEntryOrder();
        }

        /// <summary>
        /// new handle for CWmsEntryOrder
        /// </summary>
        /// <returns></returns>
        protected override CWmsEntryOrderHandler NewHandler()
        {
            return new CWmsEntryOrderHandler();
        }
        //protected override CWmsOrderBaseHandlerBase<CWmsEntryOrder, MangoEntryOrder, WmsEntryOrder, CWmsEntryOrderHandler> NewHandler()
        //{
        //    return new CWmsEntryOrderHandler();
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    class CWmsEntryOrderHandler : CWmsOrderBaseHandlerBase<CWmsEntryOrder, MangoEntryOrder, WmsEntryOrder, CWmsSubEntryOder, CWmsEntryOrderHandler>
    {
        /// <summary>
        /// 根据主入库单ID获取实体
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        static public CWmsEntryOrder NewOrder(string pId)
        {
            throw new NotImplementedException("");
        }

#if false
        protected override int DlgtFunc_RunWCF<TEntity>(TWCFOperation pOperation, params object[] args)
        {
            int err = 0;
            string msg = string.Empty;
            int currentIndex = args[0].Int();
            bool addOnNotFound = bool.Parse(args[1].ToString());
            List<Product_WMS_Interface> pwiList = args[3] as List<Product_WMS_Interface>;
            string orderId = args[4].ToString();
            TDict285_Values isUpdateOK = (TDict285_Values)args[5];
            TDict285_Values isDel = (TDict285_Values)args[6];
            var acb_args = new object[] {
                        currentIndex,
                        addOnNotFound,
                        new DefDlgt_RunWCF<Product_WMS_Interface>(DlgtFunc_RunWCF<Product_WMS_Interface>),
                        pwiList.ToList()
                    };
            if (!addOnNotFound && TWCFOperation.EUpdate == pOperation)
            {
                err = Update709(orderId, pwiList[currentIndex].MapId2.Int().ToString(), isUpdateOK, isDel, out msg);// Dict709Handle.UpdateRow_Order(TDict709_Value.EEntryOrder, orderId, pwiList[currentIndex].MapId2.Int().ToString(), isUpdateOK, isDel, out msg);
            }
            else if (addOnNotFound && TWCFOperation.EUpdateA == pOperation)
            {
                err = err = UpdateA(orderId, pwiList[currentIndex].MapId2.Int().ToString(), isUpdateOK, isDel, out msg); // Dict709Handle.UpdateRowA_Order(TDict709_Value.EEntryOrder, orderId, pwiList[currentIndex].MapId2.Int().ToString(), isUpdateOK, isDel, out msg);
            }
            else
            {
                throw new NotSupportedException("");
            }
            return err;
        }
#endif
#if false
        protected override void Acb_RunWCF(IAsyncResult iar)
        {
            object[] args = iar.AsyncState as object[];
            int currentIndex = args[0].Int();
            bool addOnNotFound = bool.Parse(args[1].ToString());
            DefDlgt_RunWCF<Product_WMS_Interface> dlgt = args[2] as DefDlgt_RunWCF<Product_WMS_Interface>;
            List<Product_WMS_Interface> pwiList = args[3] as List<Product_WMS_Interface>;

            if (pwiList.Count > currentIndex)
            {
                AreArray[currentIndex].Set();
                currentIndex++;
                if (pwiList.Count > currentIndex)
                {
                    var acb_args = new object[] {
                        currentIndex,
                        addOnNotFound,
                        new DefDlgt_RunWCF<Product_WMS_Interface>(DlgtFunc_RunWCF<Product_WMS_Interface>),
                        pwiList.ToList(),
                        args[4],
                        args[5],
                        args[6]
                    };
                    dlgt.BeginInvoke((addOnNotFound) ? TWCFOperation.EUpdateA : TWCFOperation.EUpdate, acb_args, Acb_RunWCF, acb_args);
                }
            }

            dlgt.EndInvoke(iar);
        }

#endif
        override protected int Update709(string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            return Dict709Handle.UpdateRow_Order( TDict709_Value.EEntryOrder, pEid, pEid, pUpdateOk, pDel, out pMsg);
        }
        override protected int UpdateA709(string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            return Dict709Handle.UpdateRowA_Order(TDict709_Value.EEntryOrder, pEid, pEid, pUpdateOk, pDel, out pMsg);
        }
        //protected override List<Product_WMS_Interface> GetPwiListFromOrder(object pOrder)
        //{
        //    var order  = pOrder as CWmsEntryOrder;
        //    return MangoFactory.GetPwiListFromOrder(order.Id, order.SubOrders.Values.Select(x=>x.Id));
        //}

        /// <summary>
        /// get entity of WmsLogistics as the logistics of this entryorder represented by _order.
        /// -or- return null if failed in method executation.
        /// </summary>
        /// <returns></returns>
        public WmsLogistics GetLogistics(CWmsEntryOrder pOrder)
        {
            return base.GetLogistics(pOrder);
        }
    }
}
