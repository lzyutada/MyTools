using System;
using System.Collections.Generic;
using System.Linq;
using MangoMis.Frame.ThirdFrame;
using TT.Common.Frame.Model;
using C_WMS.Data.Wms.Data;
using System.Reflection;
using MisModel;
using C_WMS.Data.Mango.MisModelPWI;
using MangoMis.Frame.Helper;
using System.Threading;
using C_WMS.Data.Mango;

namespace C_WMS.Data.CWms.CWmsEntity
{
    public enum TCWmsOrderType
    {
        EEntryOrder,
        EStockoutOrder,
        EReturnOrder,
        EUnknownOrder
    }

    /// <summary>
    /// 主单据基类
    /// </summary>
    abstract class CWmsOrderBase<TOrderType, TMangoType, TWmsType, TSubOrderType, THandlerType> : Interface.CWms.CWmsEntity.CWmsEntityBase
    {
        #region Properties
        public THandlerType Handler { get; protected set; }

        /// <summary>
        /// 获取芒果商城所对应的单据。
        /// </summary>
        public TMangoType MangoOrder { get; protected set; }

        /// <summary>
        /// 获取C-WMS系统所对应的单据。
        /// </summary>
        public TWmsType WmsOrder { get; protected set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public TCWmsOrderType OrderType { get; protected set; }
        /// <summary>
        /// 获取记录ID
        /// </summary>
        abstract public string Id { get; }

        /// <summary>
        /// 获取子单据列表。
        /// </summary>
        public Dictionary<string, TSubOrderType> SubOrders { get; protected set; }

        /// <summary>
        /// MapClassId of this
        /// </summary>
        virtual public TDict709_Value MapClassId { get; protected set; }
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsOrderBase()
        {
            SubOrders = new Dictionary<string, TSubOrderType>(1);
            Handler = NewHandler();
        }

        /// <summary>
        /// 释放占用资源
        /// </summary>
        public override void Dispose()
        {
            SubOrders.Clear();
        }

        abstract protected THandlerType NewHandler();
    }

    /// <summary>
    /// 主单据实体DataHandler类
    /// </summary>
    abstract class CWmsOrderBaseHandlerBase<TOrderType, TMangoType, TWmsType, TSubOrderType, THandlerType> : IDisposable // where TOrderType : class, new()
    {
        protected TCWmsOrderType OrderType = TCWmsOrderType.EUnknownOrder;

        /// <summary>
        /// Array of WaitHandle for async operations on WCF.
        /// </summary>
        protected AutoResetEvent[] AreArray = null;

        /// <summary>
        /// 声明委托，异步执行WCF操作
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pOperation"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected delegate int DefDlgt_RunWCF<TEntity>(TWCFOperation pOperation, params object[] args);

        /// <summary>
        /// default constructor
        /// </summary>
        protected CWmsOrderBaseHandlerBase(TCWmsOrderType pType)
        {
            OrderType = pType;
        }

        public void Dispose()
        {
            if (null != AreArray) Array.Clear(AreArray, 0, AreArray.Length);
        }

        public virtual CWmsWarehouse GetWarehouse(CWmsOrderBase<TOrderType, TMangoType, TWmsType, TSubOrderType, THandlerType> pOrder)
        {
            throw new NotImplementedException("");
        }
        /// <summary>
        /// get entity of WmsLogistics as the logistics of this entryorder represented by _order.
        /// -or- return null if failed in method executation.
        /// </summary>
        /// <returns></returns>
        public virtual WmsLogistics GetLogistics(CWmsOrderBase<TOrderType, TMangoType, TWmsType, TSubOrderType, THandlerType> pOrder)
        {
            Product_PeiSong_ProductMain deliveryOrder = null;
            CWmsSystemParam_LogisticsItem logistics = null;
            WmsLogistics retObj = null;

            if (null == pOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("Failed in getting entity of WmsLogistics by {0}, pOrder[{1}] is null", typeof(TOrderType), pOrder);
                return retObj;
            }

            // get entity of deliveryorder by id of entryorder.
            if (null == (deliveryOrder = MisModelFactory.GetMisEntity<Product_PeiSong_ProductMain>(pOrder.Id)))
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("Failed in getting entity of WmsLogistics by ID[{0}], CANNOT retrieve deliveryorder by {1}(typeof[{2}])", pOrder?.Id, pOrder, typeof(TOrderType));
            }
            else
            {
                // 根据主配送单中的‘配送人’判读使用第三方物流还是芒果物流
                if (null == (logistics = CWmsMisSystemParamCache.Cache.GetLogisticsByUserId(deliveryOrder.DeliveryUserId.Int().ToString())))
                {
                    retObj = new WmsLogistics(logistics.Code, logistics.Name);
                }
                else
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("Failed in getting entity of WmsLogistics by {0}.ID[{1}], CANNOT retrieve cached logistics by [{2}].DeliveryUserId[{3}]. return default logistics.", typeof(TOrderType), pOrder?.Id, deliveryOrder, deliveryOrder.DeliveryUserId);
                    retObj = CWmsDataFactory.GetDefaultLogistic();
                }
            }
            return retObj;
        } // WmsLogistics GetLogistics()

        protected int DlgtFunc_RunWCF<TEntity>(Mango.TWCFOperation pOperation, params object[] args)
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
                err = UpdateA709(orderId, pwiList[currentIndex].MapId2.Int().ToString(), isUpdateOK, isDel, out msg); // Dict709Handle.UpdateRowA_Order(TDict709_Value.EEntryOrder, orderId, pwiList[currentIndex].MapId2.Int().ToString(), isUpdateOK, isDel, out msg);
            }
            else
            {
                throw new NotSupportedException("");
            }
            return err;
        }

        virtual protected void Acb_RunWCF(IAsyncResult iar)
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
        /// <summary>
        /// 更新单据pMangoOrder在Dict709中对应的行的isUpdateOk和isDel. 当pAddOnNotFound为true时，如果Dict709中没有对应的行则插入新行；当pAddOnNotFound为false时，如果Dict709中没有对应的行则操作失败.
        /// 该方法返回WCF的执行结果或TError值
        /// </summary>
        /// <param name="pOrder">待更新的Mis实体对应的单据对象</param>
        /// <param name="pIsUpdateOk">Dict709.IsUpdateOK字段</param>
        /// <param name="pIsDel">Dict709.IsDel字段</param>
        /// <param name="pAddOnNotFound">当pAddOnNotFound为true时，如果Dict709中没有对应的行则插入新行；当pAddOnNotFound为false时，如果Dict709中没有对应的行则操作失败.</param>
        /// <returns></returns>
        virtual public int UpdateDict709(CWmsOrderBase<TOrderType, TMangoType, TWmsType, TSubOrderType, THandlerType> pOrder, TDict285_Values pIsUpdateOk, TDict285_Values pIsDel, bool pAddOnNotFound)
        {
            if (null != AreArray) Array.Clear(AreArray, 0, AreArray.Length);
            AreArray = pOrder.SubOrders.Select(x => new AutoResetEvent(false)).ToArray();

            var args = new object[]
            {
                0,  // 0st
                pAddOnNotFound, // 1st
                new DefDlgt_RunWCF<Product_WMS_Interface>(DlgtFunc_RunWCF<Product_WMS_Interface>), // 2nd
                MangoFactory.GetV_PwiList(pOrder.MapClassId, pOrder.Id, pOrder.SubOrders.Keys), // 3rd
                pOrder.Id,  // 4th
                pIsUpdateOk,    // 5th
                pIsDel  // 6th
            };

            var dlgt = args[2] as DefDlgt_RunWCF<Product_WMS_Interface>;//  (DlgtFunc_RunWCF<Product_WMS_Interface>);
            dlgt.BeginInvoke((pAddOnNotFound) ? TWCFOperation.EUpdateA : TWCFOperation.EUpdate, args, Acb_RunWCF, args);
            WaitHandle.WaitAll(AreArray);
            return -1;
        }

        virtual protected int Update709(string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            switch (OrderType)
            {
                case TCWmsOrderType.EEntryOrder:
                    return Dict709Handle.UpdateRow(TDict709_Value.EEntryOrder, pEid, pEsId, pUpdateOk, pDel, out pMsg);
                case TCWmsOrderType.EStockoutOrder:
                    return Dict709Handle.UpdateRow(TDict709_Value.EExwarehouseOrder, pEid, pEsId, pUpdateOk, pDel, out pMsg);
                case TCWmsOrderType.EReturnOrder:
                    return Dict709Handle.UpdateRow(TDict709_Value.EReturnOrder, pEid, pEsId, pUpdateOk, pDel, out pMsg);
                default:
                    {
                        pMsg = string.Format("{0}.Update709(), unknown order type={1}", GetType(), OrderType);
                        C_WMS.Data.Utility.MyLog.Instance.Warning(pMsg);
                        return TError.Post_NoChange.Int();
                    }
            }
        }

        virtual protected int UpdateA709(string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            switch (OrderType)
            {
                case TCWmsOrderType.EEntryOrder:
                    return Dict709Handle.UpdateRowA(TDict709_Value.EEntryOrder, pEid, pEsId, pUpdateOk, pDel, out pMsg);
                case TCWmsOrderType.EStockoutOrder:
                    return Dict709Handle.UpdateRowA(TDict709_Value.EExwarehouseOrder, pEid, pEsId, pUpdateOk, pDel, out pMsg);
                case TCWmsOrderType.EReturnOrder:
                    return Dict709Handle.UpdateRowA(TDict709_Value.EReturnOrder, pEid, pEsId, pUpdateOk, pDel, out pMsg);
                default:
                    {
                        pMsg = string.Format("{0}.Update709(), unknown order type={1}", GetType(), OrderType);
                        C_WMS.Data.Utility.MyLog.Instance.Warning(pMsg);
                        return TError.Post_NoChange.Int();
                    }
            }
        }
    } // class CWmsOrderBaseHandlerBase
}
