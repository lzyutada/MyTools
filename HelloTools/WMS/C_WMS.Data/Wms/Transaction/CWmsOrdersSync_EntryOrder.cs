using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MangoMis.Frame.ThirdFrame;
using MangoMis.Frame.Helper;
using C_WMS.Data.CWms.Interfaces.Data;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.CWms.CWmsEntity;
using C_WMS.Interface.CWms.Interfaces.Data;
using MisModel;
using TT.Common.Frame.Model;
using C_WMS.Data.Wms.Data;
using C_WMS.Data.Wms.Transaction;
using C_WMS.Data.CWms.Interfaces.Methods;

namespace C_WMS.Data.Wms.Transaction
{
    /// <summary>
    /// 批量从商城向WMS同步入库订单
    /// </summary>
    class CWmsOrdersSync_EntryOrder : MWmsTransactionBase<CWmsOrdersSync_EntryOrder, HttpReqXml_EntryOrderCreate, HttpRespXml_EntryOrderCreate>
    {
#if C_WMS_V1
        CWmsOrdersSync_EntryOrderCtrl mCtrl = new CWmsOrdersSync_EntryOrderCtrl();
        CWmsOrdersSync_EntryOrderImpl mImpl = new CWmsOrdersSync_EntryOrderImpl();

        #region 废弃的，同步操作执行单据同步
        /// <summary>
        /// overrided method. 执行各接口的HTTP Transaction.
        /// 不要使用同步通讯操作，该方法将抛出异常。需调用int Activate()方法启动异步同步操作。
        /// </summary>
        /// <exception cref="NotImplementedException">不要使用同步通讯操作，该方法将抛出异常。需调用int Activate()方法启动异步同步操作。</exception>
        /// <returns></returns>
        public override HttpRespXmlBase DoTransaction()
        {
            throw new NotImplementedException("不要使用同步通讯操作，该方法将抛出异常。需调用int Activate()方法启动异步同步操作。");
        }
        /// <summary>
        /// overrided method. 执行各接口的HTTP Transaction.
        /// 不要使用同步通讯操作，该方法将抛出异常。需调用int Activate()方法启动异步同步操作。
        /// </summary>
        /// <exception cref="NotImplementedException">不要使用同步通讯操作，该方法将抛出异常。需调用int Activate()方法启动异步同步操作。</exception>
        /// <returns></returns>
        public override int DoTransaction(out HttpRespXmlBase pResp, out string pMsg, params object[] args)
        {
            throw new NotImplementedException("不要使用同步通讯操作，该方法将抛出异常。需调用int Activate()方法启动异步同步操作。");
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pReq"></param>
        /// <returns></returns>
        public HttpRespXml_EntryOrderCreate AsyncDoTransaction(HttpReqXml_EntryOrderCreate pReq)
        {
            HttpRespXml_EntryOrderCreate retObj = null;
            var respObj = Post(pReq);
            retObj = respObj as HttpRespXml_EntryOrderCreate;
            if (null == retObj)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("在CWmsOrdersSync_EntryOrder.{0}中，执行与WMS系统的接口通讯完成，但得到的反序列化后的响应实体为空。原响应内容为：{1}", MethodBase.GetCurrentMethod().Name, respObj);
            }
            return retObj;
        }

        public override int Activate()
        {
            int err = TError.RunGood.Int();
            if (TError.RunGood.Int() == mCtrl.Activate(AsyncDoTransaction, DingDingMsgToUser))
            {
                err = base.Activate();
            }
            return err;
        }

        /// <summary>
        /// 异步执行批量同步操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void RunL(object sender, EventArgs args)
        {
            if (System.Threading.Interlocked.Exchange(ref inTimer, 1) == 0)
            {
                try
                {
                    int err = 0;
                    if (TError.RunGood.Int() == (err = mCtrl.DoRunL(mImpl))
                        && CWmsOrdersSync_EntryOrderCtrl.TAsyncStatus.EStopped != mCtrl.AsyncStatus)
                    {
                        StartTimer();   // 若控制器操作逻辑成功，并且控制器的状态不是EStopped，则再次激活计时器
                    }
                    else
                    {
                        StopTimer();
                        C_WMS.Data.Utility.MyLog.Instance.Info("CWmsOrdersSync_EntryOrder.{0}({2})中，停止批量同步的异步控制器, err={1}", MethodBase.GetCurrentMethod().Name, err, mCtrl.AsyncStatus);
                    }
                    System.Threading.Interlocked.Exchange(ref inTimer, 0);
                }
                catch (Exception ex)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "CWmsOrdersSync_EntryOrder.在{0}中，异步执行批量同步操作发生异常。尝试恢复标志位、尝试结束计时器", MethodBase.GetCurrentMethod().Name);
                    System.Threading.Interlocked.Exchange(ref inTimer, 0);
                    StopTimer();
                }
            } // if (Interlocked.Exchange(ref inTimer, 1) == 0)
        }
#else
        protected override IMWmsTransactionImpl<HttpReqXml_EntryOrderCreate, HttpRespXml_EntryOrderCreate> InitImpl()
        {
            return new CWmsOrdersSync_EntryOrderImpl();
        }
#endif
    } // class CWmsOrdersSync_EntryOrder

#if !C_WMS_V1
    class CWmsOrdersSync_EntryOrderImpl : CWmsEntryOrderCreateImpl
    {
        public override int ActivateImpl(params object[] args)
        {
            throw new NotImplementedException();
        }

        public override int RunImpl(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
#else
        /// <summary>
        /// 控制器
        /// </summary>
        class CWmsOrdersSync_EntryOrderCtrl
    {
        /// <summary>
        /// 异步状态
        /// </summary>
        public enum TAsyncStatus
        {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
            EBegin,         // 开始异步会话
            EGetOrders,     // 获取所有待同步的主入库订单
            ESyncOrderResetSyncFlag,    // 同步单条主入库订单，重置所有子单据的709和主单据的IsToWMS
            ESyncOrderHttpTrans,        // 同步单条主入库订单，向WMS同步
            ESyncOrderUpdateSyncFlag,   // 同步单条主入库订单，根据同步结果更新所有子单据的709和主单据的IsToWMS
            ESuccess,       // WMS同步操作成功
            EFailed,        // WMS同步操作失败
            EStopped        // 结束异步会话
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        } // enum TAsyncStatus

        /// <summary>
        /// 获取或设置当前正在处理的主入库单的WMS同步结果，true:成功 -or- false:失败
        /// </summary>
        bool _currentOrderSyncOk { get; set; }

        /// <summary>
        /// 当前异步状态
        /// </summary>
        public TAsyncStatus AsyncStatus { get { return mAsyncStatus; } }
        protected TAsyncStatus mAsyncStatus = TAsyncStatus.EStopped;

        public delegate HttpRespXml_EntryOrderCreate DefDlgt_DoHttpTransaction(HttpReqXml_EntryOrderCreate pReq);
        public delegate void DefDlgt_NotifySyncResult(int pUid, string pMsg);
        DefDlgt_DoHttpTransaction mDlgtDoHttpTransaction = null;
        DefDlgt_NotifySyncResult mDlgtNotifySyncResult = null;

        /// <summary>
        /// 激活控制器。若激活成功则返回TError.RunGood; 否则返回其他值.
        /// </summary>
        /// <returns></returns>
        public int Activate(DefDlgt_DoHttpTransaction pTransHandle, DefDlgt_NotifySyncResult pRsltHandle)
        {
            if (null == pTransHandle || null == pRsltHandle)
                return TError.Post_NoParam.Int();

            // 仅当控制器为空闲（即EStopped）时才能被激活
            int err = TError.Post_NoChange.Int();
            switch (mAsyncStatus)
            {
                case TAsyncStatus.EStopped:
                    {
                        mDlgtDoHttpTransaction = pTransHandle;
                        mDlgtNotifySyncResult = pRsltHandle;
                        mAsyncStatus = TAsyncStatus.EBegin;
                        err = TError.RunGood.Int();
                        break;
                    }
                default: { break; }
            }
            return err;
        }

        /// <summary>
        /// 执行处理逻辑。若仍要执行下一步一步操作，则应返回TError.RunGood；否则返回其他值
        /// </summary>
        /// <param name="pImpl">implementor of CWmsOrdersSync_EntryOrder</param>
        /// <returns>若仍要执行下一步一步操作，则应返回TError.RunGood；否则返回其他值</returns>
        public int DoRunL(CWmsOrdersSync_EntryOrderImpl pImpl)
        {
            if (null == pImpl)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsOrdersSync_EntryOrder.在{0}中，输入参数pHandler为空", MethodBase.GetCurrentMethod().Name);
                return TError.Post_NoParam.Int();
            }

            // 仅当控制器为空闲（即EStopped）时才能被激活
            int err = TError.Post_NoChange.Int();
            switch (AsyncStatus)
            {
                case TAsyncStatus.EBegin:
                    {
                        err = TError.RunGood.Int();
                        mAsyncStatus = TAsyncStatus.EGetOrders;
                        break;
                    }
                case TAsyncStatus.EGetOrders:
                    {
                        err = pImpl.DoGetVEntryOrders();
                        C_WMS.Data.Utility.MyLog.Instance.Debug("CWmsOrdersSync_EntryOrder.{0}({1}), err={2}", MethodBase.GetCurrentMethod().Name, AsyncStatus, err);
                        if (TError.RunGood.Int() == err)
                        {
                            pImpl.CurrentEntryOrder_SeekToFront();
                            mAsyncStatus = TAsyncStatus.ESyncOrderResetSyncFlag;
                        }
                        else
                        {
                            C_WMS.Data.Utility.MyLog.Instance.Error("获取所有待同步的主入库订单失败, err={0}", err);
                            mAsyncStatus = TAsyncStatus.EFailed;
                        }
                        break;
                    }
                case TAsyncStatus.ESyncOrderHttpTrans:
                    {
                        pImpl.ReloadSubOrders();    // 重新加载一次子入库订单，因为在同步前的重置flag操作结束后，子入库订单列表就被清空了
                        if (TError.RunGood.Int() == (err = DoSyncSingleOrder(pImpl)))
                        {
                            mAsyncStatus = TAsyncStatus.ESyncOrderUpdateSyncFlag;
                        } // 当前单据同步成功且还有待同步的单据，将待同步的单据置为下一条
                        else if (TError.Pro_HaveNoData.Int() == err)
                        {
                            mAsyncStatus = TAsyncStatus.ESyncOrderUpdateSyncFlag;
                        }  // 没有待同步的单据（即所有单据都同步完成）
                        else
                        {
                            mAsyncStatus = TAsyncStatus.EFailed;
                            C_WMS.Data.Utility.MyLog.Instance.Debug("CWmsOrdersSync_EntryOrder.{0}({ESyncOrderHttpTrans}), err={1}", MethodBase.GetCurrentMethod().Name, err);
                        }

                        break;
                    }
                case TAsyncStatus.ESyncOrderResetSyncFlag:
                    {
                        //C_WMS.Data.Utility.MyLog.Instance.Debug("{0}({1}), CurrentOrder={2}", MethodBase.GetCurrentMethod().Name, AsyncStatus, pImpl.CurrentOrder?.GetId());

                        if (null == pImpl.CurrentOrder)
                        {
                            mAsyncStatus = TAsyncStatus.ESuccess;
                        }
                        else
                        {
                            err = pImpl.UpdateCurrentEntryOrderSyncStatus(TDict285_Values.EUnknown, true);
                            if (TError.RunGood.Int() == err)
                            {
                                if (-1 == pImpl.CurrentSubEntryOrder_MoveNext())
                                {
                                    mAsyncStatus = TAsyncStatus.ESyncOrderHttpTrans;
                                } // 没有待更新结果的子入库订单了，即当前主入库单在商城和709中的同步状态更新完成
                            } // 更新主入库单及其子入库单的状态成功完成
                            else
                            {
                                C_WMS.Data.Utility.MyLog.Instance.Error("在CWmsOrdersSync_EntryOrder.{0}中，重置主入库单{1}和子入库单{2}的同步状态失败，结束同步操作", pImpl.CurrentOrder?.GetId(), pImpl.CurrentSubOrder?.GetId());
                                mAsyncStatus = TAsyncStatus.EFailed;
                            } // 更新主入库单及其子入库单的状态失败，结束本次批量同步
                        }

                        //C_WMS.Data.Utility.MyLog.Instance.Debug("{0}(ESyncOrderResetSyncFlag), err={1}, CurrentOrder={2}", MethodBase.GetCurrentMethod().Name, err, pImpl.CurrentOrder?.GetId());
                        break;
                    }
                case TAsyncStatus.ESyncOrderUpdateSyncFlag:
                    {
                        //C_WMS.Data.Utility.MyLog.Instance.Debug("{0}({1})开始, CurrentOrder={2}, CurrentSubOrder={3}", MethodBase.GetCurrentMethod().Name, AsyncStatus, pImpl.CurrentOrder?.GetId(), pImpl.CurrentSubOrder?.GetId());

                        if (TError.RunGood.Int() == (err = pImpl.UpdateCurrentEntryOrderSyncStatus((_currentOrderSyncOk) ? TDict285_Values.EDeleted : TDict285_Values.EUnknown, false)))
                        {
                            if (-1 == pImpl.CurrentSubEntryOrder_MoveNext())
                            {
                                pImpl.CurrentEntryOrder_MoveNext();
                                _currentOrderSyncOk = false;
                                mAsyncStatus = TAsyncStatus.ESyncOrderResetSyncFlag;  // 开始下一个主入库单的同步
                            } // 没有待更新结果的子入库订单了
                        } // 更新主入库单及其子入库单的状态成功完成
                        else
                        {
                            C_WMS.Data.Utility.MyLog.Instance.Error("CWmsOrdersSync_EntryOrder.在{0}中，重置主入库单{1}和子入库单{2}的同步状态失败，结束同步操作", pImpl.CurrentOrder?.GetId(), pImpl.CurrentSubOrder?.GetId());
                            mAsyncStatus = TAsyncStatus.EFailed;
                        }// 更新主入库单及其子入库单的状态失败，结束本次批量同步

                        //C_WMS.Data.Utility.MyLog.Instance.Debug("{0}(ESyncOrderUpdateSyncFlag)结束, err={1}, CurrentOrder={2}, CurrentSubOrder={3}", MethodBase.GetCurrentMethod().Name, err, pImpl.CurrentOrder?.GetId(), pImpl.CurrentSubOrder?.GetId());
                        break;
                    }
                case TAsyncStatus.ESuccess:
                case TAsyncStatus.EFailed:
                    {
                        mAsyncStatus = TAsyncStatus.EStopped;
                        err = TError.RunGood.Int();
                        break;
                    }
                case TAsyncStatus.EStopped:
                    {
                        pImpl.Dispose();
                        break;
                    }
            } // switch (AsyncStatus)

            return err;
        } // int DoRunL(CWmsOrdersSync_EntryOrderImpl)

        /// <summary>
        /// 同步当前主入库订单。若执行成功则返回TError.RunGood，否则返回其他
        /// </summary>
        /// <param name="pImpl">implementor of CWmsOrdersSync_EntryOrder</param>
        /// <returns></returns>
        int DoSyncSingleOrder(CWmsOrdersSync_EntryOrderImpl pImpl)
        {
            if (null == pImpl)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsOrdersSync_EntryOrder.在{0}中，输入参数pHandler为空", MethodBase.GetCurrentMethod().Name);
                return TError.Post_ParamError.Int();
            }

            int err = TError.RunGood.Int();
            HttpRespXml_EntryOrderCreate respObj = null;
            HttpReqXml_EntryOrderCreate reqObj = pImpl.GetCurrentEntryOrder_ReqXmlObj();

    #region 根据当前待处理的主入库订单创建RequestXMLObject
            if (null == reqObj)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsOrdersSync_EntryOrder.在{0}中，根据当前待处理的主入库订单[{1}]创建RequestXMLObject失败。", MethodBase.GetCurrentMethod().Name, pImpl.CurrentOrder?.GetId());
                return err = TError.Pro_HaveNoData.Int();
            }
    #endregion

    #region 执行同步 -and- handle response
            if (null != (respObj = mDlgtDoHttpTransaction.Invoke(reqObj)) && respObj.IsSuccess())
            {
                _currentOrderSyncOk = true; // 更新商城中该单据isToWms为‘同步成功’和Dict709中该行的状态为'同步成功'
            } // if ( null  == respObj && respObj.IsSuccess())
            else
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsOrdersSync_EntryOrder.{0}中，同步单据{3}的HTTP通讯失败.\r\nREQUEST={2}\r\nRESPONSE={1}", MethodBase.GetCurrentMethod().Name, respObj, reqObj, pImpl.CurrentOrder?.GetId());
            } // else
    #endregion
            return err;
        }
    }

    /// <summary>
    /// implementor of CWmsOrdersSync_EntryOrder
    /// </summary>
    class CWmsOrdersSync_EntryOrderImpl : IDisposable
    {
    #region Members -and- Properties
        /// <summary>
        /// 待同步的主入库单列表
        /// </summary>
        List<CWmsEntity.CWmsEntryOrder> OrderList { get { return _orderList; } }
        List<CWmsEntity.CWmsEntryOrder> _orderList = new List<CWmsEntity.CWmsEntryOrder>(1);

        /// <summary>
        /// 获取当前待处理的主入库订单
        /// </summary>
        public CWmsEntity.CWmsEntryOrder CurrentOrder { get { return _currentOrder; } }
        public CWmsEntity.CWmsEntryOrder _currentOrder = null;

        /// <summary>
        /// 获取当前待处理的主入库订单的子入库单列表
        /// </summary>
        List<CWmsEntity.CWmsSubEntryOder> SubOrderList { get { return _subOrderList; } }
        List<CWmsEntity.CWmsSubEntryOder> _subOrderList = new List<CWmsEntity.CWmsSubEntryOder>(1);

        /// <summary>
        /// 获取当前待处理的主入库订单的子入库单列表
        /// </summary>
        public CWmsEntity.CWmsSubEntryOder CurrentSubOrder { get { return _currentSubOrder; } }
        CWmsEntity.CWmsSubEntryOder _currentSubOrder = null;
    #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsOrdersSync_EntryOrderImpl()
        {
        }

        /// <summary>
        /// overrided from IDisposable.Dispose()
        /// </summary>
        public void Dispose()
        {
            OrderList.Clear();
            SubOrderList.Clear();
        }

        /// <summary>
        /// 将CurrentOrder移至首个待更新的主入库订单。
        /// 若OrderList中没有待操作的主入库单（即Count=0），则返回null。
        /// </summary>
        /// <returns></returns>
        public void CurrentEntryOrder_SeekToFront()
        {
            _currentOrder = (0 == OrderList.Count) ? null : OrderList.First();
            if (null != CurrentOrder)
            {
                ReloadSubOrders(); // get all sub orders for current stockout order
            }
            //C_WMS.Data.Utility.MyLog.Instance.Debug("{0}, OrderList.Count={1}, CurrentOrder={2}, SubOrderList.Count={3}, CurrentSubOrder={4}", MethodBase.GetCurrentMethod().Name, OrderList.Count, CurrentOrder?.GetId(), SubOrderList.Count, CurrentSubOrder?.GetId());
        }

        /// <summary>
        /// 将CurrentOrder移至下一个待更新的主入库订单.
        /// 该方法执行成功后，OrderList会Remove掉其中的首个单据，并将CurrentOrder重置为当前的首个单据。
        /// 若OrderList中没有待操作的主入库单（即Count=0），则返回null。
        /// </summary>
        /// <returns></returns>
        public void CurrentEntryOrder_MoveNext()
        {
            if (null == OrderList || 0 == OrderList.Count)
            {
                _currentOrder = null;
            }
            else
            {
                OrderList.Remove(OrderList[0]);
                CurrentEntryOrder_SeekToFront();
            }
        }

        /// <summary>
        /// get all sub orders for current stockout order
        /// </summary>
        public void ReloadSubOrders()
        {
            SubOrderList.Clear();
            if (null != CurrentOrder)
            {
                foreach (var so in CurrentOrder.SubOrders.Values)
                {
                    CWms.CWmsEntity.CWmsSubEntryOder seo = new CWmsEntity.CWmsSubEntryOder(so as CWmsEntity.CWmsSubEntryOder);
                    SubOrderList.Add(seo);
                }
                _currentSubOrder = (0 == SubOrderList.Count) ? null : SubOrderList.First();
            }
        }

        /// <summary>
        /// 从子订单列表中删除当前项，将当前子入库单移到列表的下一个，并返回新子入库单的主键。若当前的子入库单是最后一个，则将CurrentSubOrder置为null，且方法返回-1。
        /// </summary>
        /// <returns></returns>
        public int CurrentSubEntryOrder_MoveNext()
        {
            if (null == SubOrderList || 0 == SubOrderList.Count)
            {
                _currentOrder = null;
                return -1;
            }
            else
            {
                SubOrderList.Remove(SubOrderList[0]);
                _currentSubOrder = (0 == SubOrderList.Count) ? null : SubOrderList.First();
                return (null == CurrentSubOrder) ? -1 : CurrentSubOrder.GetId().Int();
            }
        }

        /// <summary>
        /// 获取所有待更新的主入库订单。若执行成功则返回TError.RunGood，否则返回其他
        /// </summary>
        /// <returns></returns>
        public int DoGetVEntryOrders()
        {
            int err = TError.RunGood.Int();
            string errMsg = string.Empty;
            IEnumerable<CWmsEntryOrder> tmpList = null;
            List<CommonFilterModel> filters = new List<CommonFilterModel>()
            {
                new CommonFilterModel(Mis2014_EntryOrder_Column.iscandel, "=", TDict285_Values.EUnknown.Int().ToString()),
                new CommonFilterModel(Mis2014_EntryOrder_Column.WarehouseId, "=", "541")   // 蓝讲智家新品库ID, TODO：使用系统参数
            };


            err = CWmsDataFactory.GetV_Order<CWmsEntryOrder, Mango.Data.MangoEntryOrder, CWmsEntryOrderHandler>(filters, out tmpList, out errMsg); // CWmsDataFactory.GetVEntryOrders(filters, out tmpList, out errMsg);

            if (0 >= err || null == tmpList)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsOrdersSync_EntryOrder.在{0}中，获取所有待更新的主入库订单失败，err={1}, errMsg={2}", MethodBase.GetCurrentMethod().Name, err, errMsg);
                return err;
            }
            else
            {
                _orderList.Clear();
                _orderList = tmpList.ToList();
                C_WMS.Data.Utility.MyLog.Instance.Debug("CWmsOrdersSync_EntryOrder.在{0}中，获取所有待更新的主入库订单完成，订单数量为{1}", MethodBase.GetCurrentMethod().Name, OrderList.Count);
                err = TError.RunGood.Int();
            }
            return err;
        }

        /// <summary>
        /// 获取当前处理的待更新的主入库订单。若执行成功则返回HttpReqXml_EntryOrderCreate的实体，否则返回null
        /// </summary>
        /// <returns></returns>
        public HttpReqXml_EntryOrderCreate GetCurrentEntryOrder_ReqXmlObj()
        {
            return CWmsDataFactory.GetReqXmlBody_EntryOrderCreate(CurrentOrder?.GetId(), string.Empty) as HttpReqXml_EntryOrderCreate;
        }

        /// <summary>
        /// 更新CurrentOrder在商城中([TB_商城主入库订单表].isToWms)和Dict709(isUpdateOK)中的状态。若执行成功则返回TError.RunGood，否则返回TError.WCF_RunError。
        /// </summary>
        /// <param name="pStatus"></param>
        /// <param name="pAddOnNotfoud">true: 若Dict709中没有指定行则新插入. -or- false: 若Dict709中没有指定行则执行失败</param>
        /// <returns></returns>
        public int UpdateCurrentEntryOrderSyncStatus(TDict285_Values pStatus, bool pAddOnNotfoud)
        {
            if (null == CurrentOrder || null == CurrentSubOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Info("CWmsOrdersSync_EntryOrder.在{0}中，没有待更新的主入库订单或子入库单, CurrentOrder={1}, CurrentSubOrder={2}", MethodBase.GetCurrentMethod().Name, CurrentOrder?.GetId(), CurrentSubOrder?.GetId());
                return TError.Pro_HaveNoData.Int();
            }

            int err = TError.RunGood.Int();
            string errMsg = string.Empty;
            //List<Product_WMS_Interface> fList = null;   // 返回WCF操作失败的List

    #region 更新Dict709该单据的isUpdateOK
            err = (pAddOnNotfoud) ?
                Dict709Handle.UpdateRowA_Order(TDict709_Value.EEntryOrder, CurrentOrder.GetId(), CurrentSubOrder.GetId(), pStatus, TDict285_Values.EUnknown, out errMsg) :
                Dict709Handle.UpdateRow_Order(TDict709_Value.EEntryOrder, CurrentOrder.GetId(), CurrentSubOrder.GetId(), pStatus, TDict285_Values.EUnknown, out errMsg);
            if (TError.RunGood.Int() != err)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsOrdersSync_EntryOrder.在{0}中，更新Dict709中主入库单{1} -and- 子入库单{2}的isUpdateOK({3}失败, err={4}, errMsg={5}"
                    , MethodBase.GetCurrentMethod().Name, CurrentOrder.GetId(), CurrentSubOrder.GetId(), pStatus, err, errMsg);
                return err;
            }
    #endregion

    #region 更新商城中该单据的([TB_商城主入库订单表].isToWms)
            Product_Warehouse_ProductMainInput entity = Mango.MisModelFactory.GetMisOrder(TCWmsOrderCategory.EEntryOrder, CurrentOrder.GetId()) as Product_Warehouse_ProductMainInput;
            if (null == entity)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsOrdersSync_EntryOrder.在{0}中，从商城中获取单据({1})的实体失败", MethodBase.GetCurrentMethod().Name, CurrentOrder.GetId());
                return err = TError.Pro_HaveNoData.Int();
            }
            else
                entity.IsToWMS = pStatus.Int();

            if (0 >= (err = CWmsMisHandler.UpdateEntity(entity)))
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsOrdersSync_EntryOrder.在{0}中，更新商城中单据({1})的isToWMS失败, err={2}", MethodBase.GetCurrentMethod().Name, CurrentOrder.GetId(), err);
            }
    #endregion
            return (0 >= err) ? TError.WCF_RunError.Int() : TError.RunGood.Int();
        }
    } // class CWmsOrdersSync_EntryOrderImpl
#endif
} // namespace C_WMS.Data.CWms.Interfaces.Methods
