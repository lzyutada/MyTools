using C_WMS.Data.Mango.Data;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.Wms.Data;
using C_WMS.Data.Mango;
using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using MisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 出库订单
    /// </summary>
    class CWmsExWarehouseOrder : CWmsOrderBase<CWmsExWarehouseOrder, MangoExwarehouseOrder, WmsStockoutOrder, CWmsExWarehouseSubOrder, CWmsStockoutOrderHandler>
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
                var mangoOrder = MangoOrder as Mango.Data.MangoExwarehouseOrder;
                if (null == mangoOrder)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("获取主出库单Id失败, MangoOrder={0}", MangoOrder);
                    return string.Empty;
                }
                else
#endif
                return MangoOrder.ProductOutputMainId.Int().ToString();
            }
        }

#if false
        /// <summary>
        /// 
        /// </summary>
        public DateTime lastModifiedTime = DateTime.MinValue;
#endif

        /// <summary>
        /// 获取C-WMS系统对应的出库单类型
        /// </summary>
        public TWmsOrderType WmsStockoutType
        {
            get
            {
                // 遍历所有子出库单所对应的订单类型
                int ptckCount = 0;
                int b2bckCount = 0;
                int qtckCount = 0;
                #region travers each sub order for ordertype
                foreach (var subOrder in SubOrders)
                {
                    switch (subOrder.Value?.MangoOrder.MallOrderType)
                    {
                        case TMangoOrderType.EBMDD: // 部门订单
                        case TMangoOrderType.EZSBD: { b2bckCount++; break; } // 装饰补单
                        case TMangoOrderType.EGRGM: { qtckCount++; break; }// 个人购买
                        case TMangoOrderType.EWLCK: { ptckCount++; break; }// 芒果网络为货主，无订单出库
                        default: { continue; } // 不支持其他订单类型
                    }
                }
                #endregion
                // 返回出库单类型
                if (SubOrders.Count == ptckCount) return TWmsOrderType.PTCK;
                else if (SubOrders.Count == b2bckCount) return TWmsOrderType.B2BCK;
                else if (SubOrders.Count == qtckCount) return TWmsOrderType.QTCK;
                else return TWmsOrderType.QTCK;
            }
        }

        /// <summary>
        /// 要求出库时间
        /// </summary>
        public string ScheculeDeliverDate
        {
            get
            {
                string ret = string.Empty;
                try
                {
                    ret = DateTime.MinValue.ToString();
                    // 取子出库单中最迟的要求配送时间
                    foreach (var o in SubOrders)
                    {
                        DateTime dtRet = DateTime.Parse(ret);
                        DateTime dtCur = DateTime.Parse((o.Value.MangoOrder as MangoSubExwarehouseOrder).ScheduleDate);
                        ret = (dtRet > dtCur) ? ret : (o.Value.MangoOrder as MangoSubExwarehouseOrder).ScheduleDate;
                    }
                }
                catch (Exception ex)
                {
                    var thirdRslt = new ThirdResult<List<object>>(string.Format(""));
                    if (null != ex.InnerException) thirdRslt.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                    thirdRslt.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                    thirdRslt.End();

                    ret = string.Empty;
                }
                return ret;
            }
        }

#if false
        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsExWarehouseOrder()
        {
            mMangoOrder = new MangoExwarehouseOrder();
            mWmsOrder = new WmsStockoutOrder();
        }

        /// <summary>
        /// 获取单据Id
        /// </summary>
        /// <returns></returns>
        public override string GetId()
        {
            return (null == (MangoOrder as MangoExwarehouseOrder)) ? string.Empty : (MangoOrder as MangoExwarehouseOrder).ProductOutputMainId.ToString();
        }
#endif

        /// <summary>
        /// Default constructor
        /// </summary>
        public CWmsExWarehouseOrder()
        {
            OrderType = TCWmsOrderType.EStockoutOrder;
            MangoOrder = new MangoExwarehouseOrder();
            WmsOrder = new WmsStockoutOrder();
        }

        protected override CWmsStockoutOrderHandler NewHandler()
        {
            return new CWmsStockoutOrderHandler();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class CWmsStockoutOrderHandler : CWmsOrderBaseHandlerBase<CWmsExWarehouseOrder, MangoExwarehouseOrder, WmsStockoutOrder, CWmsExWarehouseSubOrder, CWmsStockoutOrderHandler>
    {
#if false
        class TArgs_UpdateDict709
        {
            public CWmsExWarehouseOrder Order;// = null;
            public DefDlgt_RunWCF<Product_WMS_Interface> dlgt;// = new DefDlgt_RunWCF<Product_WMS_Interface>(Dlgt_RunWCF);
            public TDict285_Values IsUpdateOk;// = TDict285_Values.EUnknown;
            public TDict285_Values IsDel;// = TDict285_Values.EUnknown;
            public bool AddOnNotFound;// = false;
            public int CurrentAreIndex;
            public TDict709_Value MapClassId;

            public TArgs_UpdateDict709(DefDlgt_RunWCF<Product_WMS_Interface> pDlgt, TDict709_Value pMapClassId, CWmsExWarehouseOrder pOrder, TDict285_Values pIsUpdateOk, TDict285_Values pIsDel, int pIndex, bool pAddOnNotFound)
            {
                MapClassId = pMapClassId;
                Order = pOrder;
                dlgt = pDlgt;
                IsUpdateOk = pIsUpdateOk;
                IsDel = pIsDel;
                AddOnNotFound = pAddOnNotFound;
                CurrentAreIndex = pIndex;
            }
        }
#endif
        /// <summary>
        /// 根据主出库单ID获取实体
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        static public CWmsExWarehouseOrder NewOrder(string pId)
        {
            throw new NotImplementedException("");
        }
        override protected int Update709(string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            return Dict709Handle.UpdateRow_Order(TDict709_Value.EExwarehouseOrder, pEid, pEid, pUpdateOk, pDel, out pMsg);
        }
        override protected int UpdateA709(string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            return Dict709Handle.UpdateRowA_Order(TDict709_Value.EExwarehouseOrder, pEid, pEid, pUpdateOk, pDel, out pMsg);
        }
        //protected override List<Product_WMS_Interface> GetPwiListFromOrder(object pOrder)
        //{
        //    return MangoFactory.GetPwiListFromSubStockoutOrders(pOrder as CWmsExWarehouseOrder);
        //}

#if false
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pOperation"></param>
        /// <param name="pEntity"></param>
        /// <returns></returns>
        protected override int DlgtFunc_RunWCF<TEntity>(TWCFOperation pOperation, params object[] args)
        {
            int err = 0;
            string msg = "";
            Dict709Handle.UpdateRowA(pEntity as Product_WMS_Interface, out err, out msg);
            return err;
        }
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
#if true

        /// <summary>
        /// 更新单据pMangoOrder在Dict709中对应的行的isUpdateOk和isDel. 当pAddOnNotFound为true时，如果Dict709中没有对应的行则插入新行；当pAddOnNotFound为false时，如果Dict709中没有对应的行则操作失败.
        /// 该方法返回WCF的执行结果或TError值
        /// </summary>
        /// <param name="pOrder">待更新的Mis实体对应的单据对象</param>
        /// <param name="pIsUpdateOk">Dict709.IsUpdateOK字段</param>
        /// <param name="pIsDel">Dict709.IsDel字段</param>
        /// <param name="pAddOnNotFound">当pAddOnNotFound为true时，如果Dict709中没有对应的行则插入新行；当pAddOnNotFound为false时，如果Dict709中没有对应的行则操作失败.</param>

        protected override List<Product_WMS_Interface> GetPwiListFromOrder(object pOrder)
        {
            return MangoFactory.GetPwiListFromSubEntryOrders(pOrder as CWmsEntryOrder);
        }
#else
        public override int UpdateDict709(CWmsOrderBase<CWmsExWarehouseOrder, MangoExwarehouseOrder, WmsStockoutOrder, CWmsExWarehouseSubOrder, CWmsStockoutOrderHandler> pOrder, TDict285_Values pIsUpdateOk, TDict285_Values pIsDel, bool pAddOnNotFound)
        {
            if (null != AreArray) Array.Clear(AreArray, 0, AreArray.Length);
            AreArray = pOrder.SubOrders.Select(x => new AutoResetEvent(false)).ToArray();

            var args = new object[]
            {
                0,  // 0st
                pAddOnNotFound, // 1st
                new DefDlgt_RunWCF<Product_WMS_Interface>(DlgtFunc_RunWCF<Product_WMS_Interface>), // 2nd
                MangoFactory.GetPwiListFromSubStockoutOrders(pOrder),  // 3rd
                pOrder.Id,  // 4th
                pIsUpdateOk,    // 5th
                pIsDel  // 6th
            };

            var dlgt = args[2] as DefDlgt_RunWCF<Product_WMS_Interface>;
            dlgt.BeginInvoke((pAddOnNotFound) ? TWCFOperation.EUpdateA : TWCFOperation.EUpdate, args, Acb_RunWCF, args);
            WaitHandle.WaitAll(AreArray);
            return -1;
        }

        //void Acb_UpdateDict709Item(IAsyncResult iar)
        //{
        //    TArgs_UpdateDict709 arg = iar.AsyncState as TArgs_UpdateDict709;
        //    _ares[arg.CurrentAreIndex].Reset();
        //    arg.CurrentAreIndex += 1;
        //    var entity = Dict709Handle.NewPwiEntity(arg.MapClassId, arg.Order.Id, arg.Order.SubOrders.Values.ToList()[arg.CurrentAreIndex].Id, arg.IsUpdateOk, arg.IsDel);
        //    arg.dlgt.BeginInvoke((arg.AddOnNotFound) ? Mango.TWCFOperation.EUpdateA : Mango.TWCFOperation.EUpdate, entity, Acb_UpdateDict709Item, arg);
        //}
#endif

        /// <summary>
        /// get entity of WmsLogistics as the logistics of this stockout order represented by _order.
        /// -or- return null if failed in method executation.
        /// </summary>
        /// <returns></returns>
        public WmsLogistics GetLogistics(CWmsExWarehouseOrder pOrder)
        {
            return base.GetLogistics(pOrder);
        }
    } // class CWmsExWarehouseOrderHandler
}
