using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C_WMS.Data.Mango;
using MisModel;
using C_WMS.Data.Mango.MisModelPWI;
using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using MangoMis.MisFrame.Frame;
using C_WMS.Data.CWms.CWmsEntity;
//using MangoMis.Frame.DataSource.Simple;
using C_WMS.Data.Mango.Data;
using TT.Common.Frame.Model;
using C_WMS.Interface.Utility;
using C_WMS.Data.CWms.Interfaces.Data;

namespace C_WMS.Data.CWms.Interfaces.Methods
{    /// <summary>
     /// 库存同步接口
     /// </summary>
    public class CWmsMethod_ItemsSync : CWmsPostTransactionBase
    {
        CWmsMethod_ItemsSyncCtrl mCtrl = null;
        CWmsMethod_ItemsSyncHandler mHandler = null;

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsMethod_ItemsSync()
        {
            mCtrl = new CWmsMethod_ItemsSyncCtrl();
            mHandler = new CWmsMethod_ItemsSyncHandler();
        }

        /// <summary>
        /// 返回API接口的method
        /// </summary>
        /// <returns></returns>
        override public string GetApiMethod()
        {
            return "items.synchronize";
        }

        /// <summary>
        /// 执行接口items.synchronize的HTTP Transaction.
        /// 应使用异步操作来完成批量商品同步，请调用方法int Activate();
        /// </summary>
        override public Interface.CWms.Interfaces.Data.HttpRespXmlBase DoTransaction()
        {
            throw new NotImplementedException("应使用异步操作来完成批量商品同步，请调用方法int Activate();");
#if false
            var ret = new ThirdResult<List<object>>("同步商品（批量）开始");
            try
            {
#region temp variables
                HttpReqXml_ItemsSyncronize reqBody = null;//请求xml
                Interface.CWms.Interfaces.Data.HttpRespXmlBase respBody = null;// new HttpRespXmlBase();//响应xml
                List<CWmsProduct> pList = null;     //  mango product list
                List<string> syncFailedList = new List<string>(1);
                var updateSuccesslist = new List<CWmsProduct>() { };    // 所有获取条码成功的商品
                List<MangoWarehouse> wList = null;  // 仓库list
                List<Product_WMS_Interface> pwiList = null;    // PWI商品list
                List<Product_WMS_Interface> fPwiList = null;    // PWI更新失败list
                bool syncFlag = true;
                string errMsg = string.Empty;
                int errWcf = 0;
                int err = 0;
#endregion

                wList = MangoFactory.GetWarehouses();

                pList = MangoFactory.GetMangoProductList();

                err = GVPwiEntitiesFromCWmsProducts(pList, out pwiList);
                if (0 != err || 0 == pwiList.Count)
                {
                    ret.Append(string.Format("同步商品（批量）结束, 更新709时失败, err={0}", err));
                    ret.End();
                    return null;
                }

                // 拆分待添加list和待更新list
                var tmpList = Dict709Handle.GetProductList();
                var pwiUList = pwiList.Where(x1 => tmpList.Select(y1 => y1.MapId1).ToList().Contains(x1.MapId1))
                    .ToList();
                var pwiAList = new List<Product_WMS_Interface>(1);

                foreach (var pwi in pwiList)
                {
                    if (pwiUList.Select(x => x.MapId1).ToList().Contains(pwi.MapId1) && pwiUList.Select(x => x.MapId2).ToList().Contains(pwi.MapId2))
                        continue;
                    pwiAList.Add(pwi);
                    pwi.MapCalssID = TDict709_Value.EAddProduct.Int();
                    pwi.AddTime = DateTime.Now;
                    pwi.AddUserid = CommonFrame.LoginUser.UserId;
                }
                foreach (var pwi in pwiUList) pwi.MapCalssID = TDict709_Value.EUpdateProduct.Int();

#region 更新709，若没有商品则新增；若有则重置状态为更新失败
                err = Dict709Handle.UpdateRowVA_Order(pwiAList, out fPwiList, out errWcf, out errMsg);
                ret.Append(string.Format("插入709, err={0}, errWcf={1}, errMsg={2}, fList.Count={3}", err, errWcf, errMsg, fPwiList.Count));
                err = Dict709Handle.UpdateRowV_Order(pwiUList, out fPwiList, out errWcf, out errMsg);
                ret.Append(string.Format("更新709, err={0}, errWcf={1}, errMsg={2}, fList.Count={3}", err, errWcf, errMsg, fPwiList.Count));

#endregion
#region 用于获取商品条形码的http通讯
                foreach (var p in pList)
                {
                    CWmsInventory_Monitoring inventoryMonitTrans = new CWmsInventory_Monitoring();
                    respBody = inventoryMonitTrans.DoTransaction(p, CWmsConsts.cStrDefaultWarehouseName);//TODO: 联调时仓库用LTCK, 请求条形码的系列操作
                    if (null == respBody)
                    {
                        ret.Append(string.Format("同步商品（批量）结束, 获取商品{0}的条形码失败", p.MangoProduct.ProductId));
                        ret.End();
                        return respBody;
                    }
                    else if ("success" == respBody.flag)
                    {
                        p.WmsProduct.barCode = (respBody as HttpRespXml_InventoryMonitoring).items[0].barcodeNum;
                        updateSuccesslist.Add(p);
                    } // else if ("success" == respBody.flag)
                    else if ("failure" == respBody.flag)
                    {
                        if (respBody.message.Contains("商品不存在"))
                        {
                            updateSuccesslist.Add(p);
                        }
                        else
                        {
                            continue;
                        }
                    } // else if ("failure" == respBody.flag)
                } // foreach (var p in productList)
#endregion // 用于获取商品条形码的http通讯

                if (0 == updateSuccesslist.Count)
                {
                    if (null != pList) pList.Clear();
                    if (null != syncFailedList) syncFailedList.Clear();
                    if (null != updateSuccesslist) updateSuccesslist.Clear();
                    if (null != wList) wList.Clear();
                    return new Interface.CWms.Interfaces.Data.HttpRespXmlBase("<?xml version='1.0' encoding='UTF - 8' standalone='yes'?><response><flag>failure</flag><code>401</code><message>待同步商品个数为0，同步失败</message></response>");
                }
#region 根据货主进行http通讯
                var ownerList = MangoFactory.GetVOwners(wList);
                foreach (var o in ownerList)
                {
                    // 
                    reqBody = new HttpReqXml_ItemsSyncronize();
                    reqBody.actionType = CWmsConsts.sStrApiItemsSyncActionTypeUpdate;
                    reqBody.ownerCode = o.Value.WmsID;
                    reqBody.warehouseCode = "OTHER";    // 统仓统配，传OTHER

                    //  TODO: 应该是更新这个货主的所有商品（根据货主所辖仓库中的商品）向reqXml.items中添加所有更新709成功的商品
                    foreach (CWmsProduct p in updateSuccesslist)
                    {
                        HttpReqXml_ItemsSync_Item item = new HttpReqXml_ItemsSync_Item();
                        item.CopyMangoProductInfo(p);
                        (reqBody as HttpReqXml_ItemsSyncronize).items.Add(item);
                    } // foreach (CWmsProduct p in updateSuccesslist)

                    // 开始HTTP会话
                    respBody = Post(reqBody) as HttpRespXml_ItemsSynchronize;

                    // 处理response
                    if (null != respBody && "failure" != respBody.flag)
                    {
                        // do noting for now
                    }
                    else
                    {
                        syncFlag = false;
                        continue;
                    }
                } // foreach (var o in ownerList)
#endregion
#region 处理所有商品同步结果
                if (syncFlag)
                {
                    // 所有商品同步成功，更新709
                    foreach (var p in updateSuccesslist)
                    {
                        var dbgStr = string.Empty;
                        err = 0;
                        var pwi = MangoFactory.CopyFromMangoProduct(p.MangoProduct, TDict709_Value.EUpdateProduct.Int());
                        pwi.UpdateUserID = CommonFrame.LoginUser.UserId.Int();
                        pwi.LastTime = DateTime.Now;
                        pwi.IsUpdateOK = TDict285_Values.ENormal.Int();
                        err = Dict709Handle.UpdateRow_Order(pwi, out dbgStr);
                    }
                }
#endregion

                //TODO 叮叮通知
                ret.Append(string.Format("同步商品（批量）结束"));
                ret.End();
                return respBody;
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return new Interface.CWms.Interfaces.Data.HttpRespXmlBase(string.Format("<?xml version='1.0' encoding='UTF - 8' standalone='yes'?><response><flag>failure</flag><code>401</code><message>{0}</message></response>", ex.Message));
            }
#endif
        }

        /// <summary>
        /// 重载int DoTransaction(out HttpRespXmlBase, out string, params object[])方法
        /// 若成功则返回TError.RunGood; 否则返回其他值。
        /// </summary>
        /// <param name="pResp">返回响应体XML内容</param>
        /// <param name="pMsg">返回错误信息</param>
        /// <param name="args">输入参数</param>
        /// <returns></returns>
        public override int DoTransaction(out Interface.CWms.Interfaces.Data.HttpRespXmlBase pResp, out string pMsg, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 同步单个商品HTTP会话
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public Interface.CWms.Interfaces.Data.HttpRespXmlBase DoTransaction(int ProductId)
        {
            var ret = new ThirdResult<List<object>>("同步单个商品HTTP会话开始");
            try
            {
#region temp variables
                HttpReqXml_ItemsSyncronize reqBody = null;//请求xml
                Interface.CWms.Interfaces.Data.HttpRespXmlBase respBody = null;// new HttpRespXmlBase();//响应xml
                List<CWmsProduct> pList = null;     //  mango product list
                List<string> syncFailedList = new List<string>(1);
                var updateSuccesslist = new List<CWmsProduct>() { };    // 所有获取条码成功的商品
                List<MangoWarehouse> wList = null;  // 仓库list
                List<Product_WMS_Interface> pwiList = null;    // PWI商品list
                List<Product_WMS_Interface> fPwiList = null;    // PWI更新失败list
                bool syncFlag = true;
                string errMsg = string.Empty;
                int errWcf = 0;
                int err = 0;
#endregion

                wList = MangoFactory.GetWarehouses();
                ret.Append(string.Format("获取仓库列表完成: {0}", wList.Count));

                pList = MangoFactory.GetMangoProduct(ProductId);
                ret.Append(string.Format("获取商品列表完成: {0}", pList.Count));

                err = GVPwiEntitiesFromCWmsProducts(pList, out pwiList);
                if (0 != err || 0 == pwiList.Count)
                {
                   C_WMS.Data.Utility.MyLog.Instance.Error("同步单个商品HTTP会话结束, 更新709时失败, err={0}", err);
                    ret.Append(string.Format("同步单个商品HTTP会话结束, 更新709时失败, err={0}", err));
                    ret.End();
                    return null;
                }

                // 拆分待添加list和待更新list
                var tmpList = Dict709Handle.GetProductList();
                var pwiUList = pwiList.Where(x1 => tmpList.Select(y1 => y1.MapId1).ToList().Contains(x1.MapId1))
                    .ToList();

                var pwiAList = new List<Product_WMS_Interface>(1);
                foreach (var pwi in pwiList)
                {
                    if (pwiUList.Select(x => x.MapId1).ToList().Contains(pwi.MapId1) && pwiUList.Select(x => x.MapId2).ToList().Contains(pwi.MapId2))
                        continue;
                    pwiAList.Add(pwi);
                    pwi.MapCalssID = TDict709_Value.EAddProduct.Int();
                    pwi.AddTime = DateTime.Now;
                    pwi.AddUserid = CommonFrame.LoginUser.UserId;
                }
                foreach (var pwi in pwiUList) pwi.MapCalssID = TDict709_Value.EUpdateProduct.Int();

#region 更新709，若没有商品则新增；若有则重置状态为更新失败
                err = Dict709Handle.UpdateRowVA_Order(pwiAList, out fPwiList, out errWcf, out errMsg);
                ret.Append(string.Format("插入709, err={0}, errWcf={1}, errMsg={2}, fList.Count={3}", err, errWcf, errMsg, fPwiList.Count));
                err = Dict709Handle.UpdateRowV_Order(pwiUList, out fPwiList, out errWcf, out errMsg);
                ret.Append(string.Format("更新709, err={0}, errWcf={1}, errMsg={2}, fList.Count={3}", err, errWcf, errMsg, fPwiList.Count));
#endregion
                ret.Append(string.Format("操作709完成: err={0}", err));
#region 用于获取商品条形码的http通讯
                foreach (var p in pList)
                {
                    CWmsInventory_Monitoring inventoryMonitTrans = new CWmsInventory_Monitoring();
                    respBody = inventoryMonitTrans.DoTransaction(p, CWmsConsts.cStrDefaultWarehouseName);//TODO: 联调时仓库用LTCK, 请求条形码的系列操作
                    if (null == respBody)
                    {
                        ret.Append(string.Format("同步单个商品HTTP会话结束, 获取商品条码系统或服务器异常"));
                        ret.End();
                        return respBody;
                    }
                    else if ("success" == respBody.flag)
                    {
                        p.WmsProduct.barCode = (respBody as HttpRespXml_InventoryMonitoring).items[0].barcodeNum;
                        updateSuccesslist.Add(p);
                    } // else if ("success" == respBody.flag)
                    else if ("failure" == respBody.flag)
                    {
                        if (respBody.message.Contains("商品不存在"))
                        {
                            updateSuccesslist.Add(p);
                        }
                        else
                        {
                            continue;
                        }
                    } // else if ("failure" == respBody.flag)
                } // foreach (var p in productList)
#endregion // 用于获取商品条形码的http通讯
                ret.Append(string.Format("获取商品条码完成: 待同步的商品数量={0}", updateSuccesslist.Count));
                if (0 == updateSuccesslist.Count)
                {
                    if (null != pList) pList.Clear();
                    if (null != syncFailedList) syncFailedList.Clear();
                    if (null != updateSuccesslist) updateSuccesslist.Clear();
                    if (null != wList) wList.Clear();

                    ret.Append(string.Format("同步单个商品HTTP会话结束, 没有待同步商品"));
                    ret.End();
                    return new Interface.CWms.Interfaces.Data.HttpRespXmlBase("<?xml version='1.0' encoding='UTF - 8' standalone='yes'?><response><flag>failure</flag><code>401</code><message>待同步商品个数为0，同步失败</message></response>");
                }
#region 根据货主进行http通讯
                var ownerList = MangoFactory.GetVOwners(wList);
                ret.Append(string.Format("根据货主进行http通讯, 货主数量:{0}", ownerList.Count));
                foreach (var o in ownerList)
                {
                    // 
                    reqBody = new HttpReqXml_ItemsSyncronize();
                    reqBody.actionType = CWmsConsts.sStrApiItemsSyncActionTypeUpdate;
                    reqBody.ownerCode = o.Key; // CWmsConsts.cStrWmsOwnerCode_MGWL;
                    mTransParams.PostParams.customerId = o.Key; // 设置query参数中的customerId
                    reqBody.warehouseCode = CWmsConsts.cStrDefaultWarehouseName;    // 统仓统配，传OTHER

                    //  TODO: 应该是更新这个货主的所有商品（根据货主所辖仓库中的商品）向reqXml.items中添加所有更新709成功的商品
                    foreach (CWmsProduct p in updateSuccesslist)
                    {
                        HttpReqXml_ItemsSync_Item item = new HttpReqXml_ItemsSync_Item();
                        item.CopyMangoProductInfo(p);
                        (reqBody as HttpReqXml_ItemsSyncronize).items.Add(item);
                    } // foreach (CWmsProduct p in updateSuccesslist)

                    // 开始HTTP会话
                    respBody = Post(reqBody) as HttpRespXml_ItemsSynchronize;
                    // 处理response
                    if (null != respBody && "failure" != respBody.flag)
                    {
                        // 目前什么都不做
                    }
                    else
                    {
                        syncFlag = false;
                        continue;
                    }
                } // foreach (var o in ownerList)
#endregion
#region 处理所有商品同步结果
                if (syncFlag)
                {
                    // 所有商品同步成功，更新709
                    foreach (var p in updateSuccesslist)
                    {
                        var dbgStr = string.Empty;
                        err = 0;
                        var pwi = MangoFactory.CopyFromMangoProduct(p.MangoProduct, TDict709_Value.EUpdateProduct.Int());
                        pwi.UpdateUserID = CommonFrame.LoginUser.UserId.Int();
                        pwi.LastTime = DateTime.Now;
                        pwi.IsUpdateOK = TDict285_Values.ENormal.Int();
                        err = Dict709Handle.UpdateRow_Order(pwi, out dbgStr);
                    }
                }
#endregion
                //TODO 叮叮通知
               
                //叮叮通知
                ret.Append(string.Format("同步单个商品HTTP会话结束"));
                ret.End();
                return respBody;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
                ret.End();
                return new Interface.CWms.Interfaces.Data.HttpRespXmlBase(string.Format("<?xml version='1.0' encoding='UTF - 8' standalone='yes'?><response><flag>failure</flag><code>401</code><message>{0}</message></response>", ex.Message));
            }
        }

        /// <summary>
        /// 处理服务器响应
        /// </summary>
        /// <param name="encode"></param>
        /// <param name="respBody">HTTP响应体</param>
        /// <returns>返回Response XML对应的数据实例</returns>
        override public Interface.CWms.Interfaces.Data.HttpRespXmlBase HandleResponse(byte[] respBody, Encoding encode)
        {
            var ret = new ThirdResult<List<object>>("同步商品，处理服务器响应ResponseBody");

            if (null == encode) throw new ArgumentNullException("同步商品，处理服务器响应ResponseBody失败，非法的Encodindg对象");
            if (null == respBody)
            {
                ret.Append(string.Format("同步商品，处理服务器响应ResponseBody失败，请求体内容为空"));
                ret.End();
                return null;
            }

            HttpRespXml_ItemsSynchronize respObj = new HttpRespXml_ItemsSynchronize();
            string respXml = encode.GetString(respBody);

            respObj = CWmsUtility.ObjtoXml(respObj.GetType(), respXml) as HttpRespXml_ItemsSynchronize;

            ret.Append("同步商品，处理服务器响应ResponseBody结束");
            ret.End();
            return respObj;
        }

        /// <summary>
        /// 转换List[CWmsProduct] pSrcList成List[Product_WMS_Interface]。若成功则返回0，失败则返回其他值.
        /// </summary>
        /// <param name="pSrcList">The p source list.</param>
        /// <param name="pDestList">返回转换成功的List[Product_WMS_Interface], 若失败则返回Count=0的列表</param>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/6/6 15:23
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        public int GVPwiEntitiesFromCWmsProducts(List<CWmsProduct> pSrcList, out List<Product_WMS_Interface> pDestList)
        {
            pDestList = new List<Product_WMS_Interface>(1);
            if (null == pSrcList)
                return -1;
            foreach (var src in pSrcList)
            {
                var pwi = MangoFactory.CopyFromMangoProduct(src.MangoProduct);
                pwi.LastTime = DateTime.Now;
                pwi.UpdateUserID = CommonFrame.LoginUser.UserId.Int();
                pwi.IsDel = TDict285_Values.ENormal.Int();
                pwi.DisOrder = 100;
                pDestList.Add(pwi);
            }
            return 0;
        }

        /// <summary>
        /// 激活计时器，启动异步通讯。若操作成功则返回TError.RunGood；否则返回其他值
        /// </summary>
        /// <returns>若操作成功则返回TError.RunGood；否则返回其他值</returns>
        public override int Activate()
        {
            int err = TError.RunGood.Int();
            if (TError.RunGood.Int() == mCtrl.Activate(AsyncDoTransaction, DingDingMsgToUser))
            {
                err = base.Activate();
            }
            //ret.Append("CWmsMethod_ItemsSync.Activate, err={0}", err);
            return err;
        }

        /// <summary>
        /// 执行异步动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void RunL(object sender, EventArgs args)
        {
            if (System.Threading.Interlocked.Exchange(ref inTimer, 1) == 0)
            {
                int err = 0;
                if (TError.RunGood.Int() == (err = mCtrl.DoRunL(mHandler))
                    && CWmsMethod_ItemsSyncCtrl.TAsyncStatus.EStopped != mCtrl.AsyncStatus)
                {
                    StartTimer();   // 若控制器操作逻辑成功，并且控制器的状态不是EStopped，则再次激活计时器
                }
                else
                {
                    StopTimer();
                }
                System.Threading.Interlocked.Exchange(ref inTimer, 0);
                //ret.Append("CWmsMethod_ItemsSync.RunL, err={0}, AsyncStatus={1}", err, mCtrl.AsyncStatus);
            } // if (Interlocked.Exchange(ref inTimer, 1) == 0)
            else
            {
                //MyLog.Instance.Error("TimerHandler_GetVMisEntity被占用（当前尝试线程{0})", Thread.CurrentThread.ManagedThreadId);
            }
        } // protected override void RunL(object, EventArgs)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pReq"></param>
        /// <returns></returns>
        public Interface.CWms.Interfaces.Data.HttpRespXmlBase AsyncDoTransaction(HttpReqXml_ItemsSyncronize pReq)
        {
            var respBody = Post(pReq);
            return respBody;
        }
    } // class CWmsMethod_ItemsSync

    /// <summary>
    /// 控制器
    /// </summary>
    class CWmsMethod_ItemsSyncCtrl
    {
        /// <summary>
        /// 异步状态
        /// </summary>
        public enum TAsyncStatus
        {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
            EBegin,         // 开始异步会话
            EResetDict709Start,             // 开始重置Dict709
            EResetDict709Update,    // 继续更新Dict709中的行
            EResetDict709Add,       // 继续添加Dict709中的行
            EResetDict709Success,          // 重置Dict709完成
            EGetBarCode,    // 获取商品的条形码
            EReadyForSync,  // 开始准备同步商品
            EComposeReqXml, // 组装RequestXML内容
            EDoTrans,       // 执行HTTP会话
            EUpdateDict709, // 更新Dict709
            ESuccess,       // WMS同步操作成功
            EFailed,        // WMS同步操作失败
            EStopped        // 结束异步会话
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        }

#region Properties
        /// <summary>
        /// 当前异步状态
        /// </summary>
        public TAsyncStatus AsyncStatus { get { return mAsyncStatus; } }
        protected TAsyncStatus mAsyncStatus = TAsyncStatus.EStopped;
#endregion

#region delegates
        public delegate Interface.CWms.Interfaces.Data.HttpRespXmlBase DefDlgt_DoHttpTransaction(HttpReqXml_ItemsSyncronize pReq);
        public delegate void DefDlgt_NotifySyncResult(int pUid, string pMsg);
        DefDlgt_DoHttpTransaction mDlgtDoHttpTransaction = null;
        DefDlgt_NotifySyncResult mDlgtNotifySyncResult = null;
#endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsMethod_ItemsSyncCtrl()
        {
        }

        /// <summary>
        /// 激活控制器。若激活成功则返回TError.RunGood; 否则返回其他值.
        /// </summary>
        /// <returns></returns>
        public int Activate(DefDlgt_DoHttpTransaction pTransHandle, DefDlgt_NotifySyncResult pRsltHandle)
        {
            if (null == pTransHandle || null== pRsltHandle)
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
            //ret.Append("CWmsMethod_ItemsSyncCtrl.Activate. Try to activate, mAsyncStatus={0}, err={1}", mAsyncStatus, err);
            return err;
        }

        /// <summary>
        /// 执行处理逻辑。若仍要执行下一步一步操作，则应返回TError.RunGood；否则返回其他值
        /// </summary>
        /// <param name="pHandler">数据处理实体</param>
        /// <returns>若仍要执行下一步一步操作，则应返回TError.RunGood；否则返回其他值</returns>
        public int DoRunL(CWmsMethod_ItemsSyncHandler pHandler)
        {
            //ret.Append("DoRunL({0})开始", mAsyncStatus);
            int err = TError.WCF_RunError.Int();
            string errMsg = string.Empty;

            if (null == pHandler)
            {
                return (err = TError.Post_ParamError.Int());    // 非法入参
            }
            switch (mAsyncStatus)
            {
                case TAsyncStatus.EBegin:               // 开始异步会话
                    {
                        err = pHandler.GetProductList(out errMsg);
                        mAsyncStatus = (0 < err) ? TAsyncStatus.EResetDict709Start : TAsyncStatus.EFailed;
                        err = TError.RunGood.Int();
                        break;
                    }
                case TAsyncStatus.EResetDict709Start:   // 开始重置Dict709
                    {
                        pHandler.GetPwiProductList(); // 709中所有商品行(isDel!=1)
                        // 获取待更新的709item列表
                        err = pHandler.GetUpdatePwiList(pHandler.ProductList, pHandler.PwiList, TDict285_Values.EUnknown, out errMsg);
                        if (TError.RunGood.Int() == err)
                        {
                            err = pHandler.GetAddPwiList(pHandler.PwiList, out errMsg);  // 获取待插入的709item列表
                            pHandler.ResetCurrentPwiItem();
                        }
                        mAsyncStatus = (TError.RunGood.Int() == err) ? TAsyncStatus.EResetDict709Update : TAsyncStatus.EFailed;
                        break;
                    }
                case TAsyncStatus.EResetDict709Update:  // 继续更新Dict709中的行
                    {
                        err = pHandler.UpdatePwiItem(out errMsg);
                        if (1 == err)
                            mAsyncStatus = TAsyncStatus.EResetDict709Update;    // 更新成功，继续
                        else if (0 == err)
                            mAsyncStatus = TAsyncStatus.EResetDict709Add;       //　无待更新商品，开始插入新行
                        else
                        {
                            mAsyncStatus = TAsyncStatus.EFailed;                // 系统错误，执行失败
                        }
                        err = TError.RunGood.Int();
                        break;
                    }
                case TAsyncStatus.EResetDict709Add:     // 继续添加Dict709中的行
                    {
                        err = pHandler.AddPwiItem(out errMsg);
                        if (1 == err)
                            mAsyncStatus = TAsyncStatus.EResetDict709Add;       // 插入成功，继续
                        else if (0 == err)
                            mAsyncStatus = TAsyncStatus.EResetDict709Success;   //　无待插入商品，重置709完成
                        else
                        {
                            mAsyncStatus = TAsyncStatus.EFailed;                // 系统错误，执行失败
                        }
                        err = TError.RunGood.Int();
                        break;
                    }
                case TAsyncStatus.EResetDict709Success:        // 重置Dict709完成
                    {
                        pHandler.ResetCurrentProductItem();
                        err = TError.RunGood.Int();
                        mAsyncStatus = TAsyncStatus.EGetBarCode;
                        break;
                    }
                case TAsyncStatus.EGetBarCode:    // 获取商品的条形码
                    {
                        err = pHandler.GetBarCode(out errMsg);
                        if (1 == err)
                        {
                            mAsyncStatus = TAsyncStatus.EGetBarCode;       // 插入获取当前商品条码，继续
                            pHandler.CurrentProductItemGoNext();
                        }
                        else if (0 == err) // get owners & update asyncstatus
                            mAsyncStatus = (0 >= pHandler.GetOwners()) ? TAsyncStatus.EFailed : TAsyncStatus.EReadyForSync;   //　无待获取条码的商品，获取完成
                        else
                            mAsyncStatus = TAsyncStatus.EFailed;                // 系统错误，执行失败
                        err = TError.RunGood.Int();
                        break;
                    }
                case TAsyncStatus.EReadyForSync:    // 开始同步商品前需要做的事情
                    {
                        pHandler.ClearPwiUpdateList(); // 重置待更新的商品List
                        pHandler.SyncOkProducList.Clear();
                        pHandler.SyncFailedProducList.Clear();
                        // TODO: Add other oprations here
                        mAsyncStatus = TAsyncStatus.EComposeReqXml;
                        err = TError.RunGood.Int();
                        break;
                    }
                case TAsyncStatus.EComposeReqXml: // 针对当前货主组装RequestXML内容，对每个货主都同步所有的待同步商品
                    {
                        err = pHandler.GetReqBodyXml(out errMsg);
                        if (1 == err)
                        {
                            mAsyncStatus = TAsyncStatus.EDoTrans;       // 成功获取Request实体，继续
                            err = TError.RunGood.Int();
                        }
                        else if (0 == err)
                        {
                            // 刷新pHandler.PwiUpdateList，仅更新同步成功的商品
                            err = pHandler.GetUpdatePwiList(pHandler.SyncOkProducList, pHandler.PwiList, TDict285_Values.ENormal, out errMsg);
                            pHandler.ResetCurrentPwiItem();
                            //　无待待同步的Request，同步HTTP通讯完成
                            mAsyncStatus = (TError.RunGood.Int() == err)?TAsyncStatus.EUpdateDict709: TAsyncStatus.EFailed;
                        }
                        else
                        {
                            mAsyncStatus = TAsyncStatus.EFailed;                // 系统错误，执行失败
                            err = TError.RunGood.Int();
                        }
                        break;
                    }
                case TAsyncStatus.EDoTrans:       // 执行HTTP会话
                    {
                        var respBody = mDlgtDoHttpTransaction.Invoke(pHandler.CurrentRequest);
                        if (null == respBody)
                        {
                            errMsg = "通讯异常，同步商品失败";
                        }
                        else if (respBody.IsSuccess()) // 记录下所有同步成功的商品
                        {
                            err = pHandler.UpdateProductListBySyncResult(pHandler.ProductList, null);
                            errMsg = string.Empty;
                        } // 当前货主的商品全部同步成功了
                        else // 分别刷新缓存中同步成功和失败的商品
                        {
                            var respFList = (respBody as HttpRespXml_ItemsSynchronize).items;
                            var okList = pHandler.ProductList.Where(all => !(respFList.Select(f => f.itemCode).Contains(all.ItemCode))).ToList();
                            var failedList = pHandler.ProductList.Where(all => respFList.Select(f => f.itemCode).Contains(all.ItemCode)).ToList();

                            err = pHandler.UpdateProductListBySyncResult(okList, failedList);
                            errMsg = respBody.message;
                        } // 可能有部分商品同步成功，部分同步失败
                        
                        pHandler.RemoveCurrentOwner();  // 当前货主同步完成，继续下一个
                        mAsyncStatus = TAsyncStatus.EComposeReqXml;
                        err = TError.RunGood.Int();
                        break;
                    }
                case TAsyncStatus.EUpdateDict709: // 更新Dict709
                    {
                        err = pHandler.UpdatePwiItem(out errMsg);
                        if (1 == err)
                            mAsyncStatus = TAsyncStatus.EUpdateDict709;    // 更新成功，继续
                        else if (0 == err)
                            mAsyncStatus = TAsyncStatus.ESuccess;       //　无待更新商品，开始插入新行
                        else
                            mAsyncStatus = TAsyncStatus.EFailed;                // 系统错误，执行失败
                        err = TError.RunGood.Int();
                        break;
                    }
                case TAsyncStatus.ESuccess:     // WMS同步操作成功
                    {
                        string ddMsg = string.Format("商城提醒：向仓储管理系统同步商品信息（批量），同步结果：成功。");
                        mDlgtNotifySyncResult.BeginInvoke(CommonFrame.LoginUser.UserId.Int(), ddMsg, Acb_NotifySyncResult, mDlgtNotifySyncResult);
                        err = TError.RunGood.Int();
                        mAsyncStatus = TAsyncStatus.EStopped;
                        break;
                    }
                case TAsyncStatus.EFailed:      // WMS同步操作失败
                    {
                        string ddMsg = string.Format("商城提醒：向仓储管理系统同步商品信息（批量），同步结果：失败，请及时处理。");
                        mDlgtNotifySyncResult.BeginInvoke(CommonFrame.LoginUser.UserId.Int(), ddMsg, Acb_NotifySyncResult, mDlgtNotifySyncResult);
                        err = TError.RunGood.Int();
                        mAsyncStatus = TAsyncStatus.EStopped;
                        break;
                    }
                case TAsyncStatus.EStopped:     // 结束异步会话
                    {
                        // 释放所占资源
                        pHandler.Dispose();
                        err = TError.RunGood.Int();

                        break;
                    }
                default: // 不支持的状态
                    { err = TError.Pro_HaveNoData.Int(); break; }
            }   // switch (mAsyncStatus)
            return err;
        }   // int DoRunL(CWmsMethod_ItemsSyncHandler pHandler)

        ///<summary>
        /// 
        /// </summary>
        protected void Acb_NotifySyncResult(IAsyncResult iar)
        {
            try
            {
                //MyLog.Instance.Debug("异步回调(准备开始接受数据集)，开始");
                var dlgt = iar.AsyncState as DefDlgt_NotifySyncResult;
                dlgt.EndInvoke(iar);    // 结束回调阻塞
                //MyLog.Instance.Debug("异步回调(准备开始接受数据集)，结束");
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

    } // class CWmsMethod_ItemsSyncCtrl

    /// <summary>
    /// 处理器
    /// </summary>
    class CWmsMethod_ItemsSyncHandler : IDisposable
    {
#region Properties
        public List<CWmsProduct> ProductList { get { return mProductList; } }
        List<CWmsProduct> mProductList = null;

        public List<CWmsProduct> SyncOkProducList { get { return mSyncOkProducList; } }
        List<CWmsProduct> mSyncOkProducList = null;

        public List<CWmsProduct> SyncFailedProducList { get { return mSyncFailedProducList; } }
        List<CWmsProduct> mSyncFailedProducList = null;

        public List<Product_WMS_Interface> PwiList { get { return mPwiList; } }
        List<Product_WMS_Interface> mPwiList = null;  // 709里所有商品List

        List<Product_WMS_Interface> PwiUpdateList { get { return mPwiUpdateList; } }
        List<Product_WMS_Interface> mPwiUpdateList = null;  // PWI待更新商品list

        List<Product_WMS_Interface> PwiAddList { get { return mPwiAddList; } }
        List<Product_WMS_Interface> mPwiAddList = null;     // PWI待添加商品list
        Dictionary<string, Wms.Data.WmsOwner> OwnerList { get { return mOwnerList; } }
        Dictionary<string, Wms.Data.WmsOwner> mOwnerList = null;

        /// <summary>
        /// 当前待更新的PWI实体，若待更新PWI列表和待插入PWI列表均为空，则当前待更新的PWI实体为null
        /// </summary>
        Product_WMS_Interface CurrentPWI { get { return mCurrentPWI; } }
        Product_WMS_Interface mCurrentPWI = null;   // 当前待更新的PWI实体

        /// <summary>
        /// 当前待操作的商品实体。若ProductList为空，则当前待操作的商品实体体为null
        /// 若当前待操作的商品实体的索引超出范围，则返回ProductList中的首个实体或最末一个实体。
        /// </summary>
        CWmsProduct CurrentProduct
        {
            get
            {
                if (null == ProductList || 0 == ProductList.Count)
                    return null;
                if (ProductList.Count <= CurrentProductIndex)
                    return null;// CurrentProductIndex = ProductList.Count - 1;
                else if (0 > CurrentProductIndex)
                    CurrentProductIndex = 0;
                return ProductList[CurrentProductIndex];
            }
        }

        /// <summary>
        /// 当前操作的商品实体的索引
        /// </summary>
        int CurrentProductIndex = 0;

        /// <summary>
        /// 当前待同步商品所属的货主, 若待同步货主列表为空，则当前货主为null
        /// </summary>
        Wms.Data.WmsOwner CurrentOwner
        {
            get
            {
                if (null == OwnerList || 0 == OwnerList.Count)
                    return null;
                return OwnerList.FirstOrDefault().Value;
            }
        }

        public HttpReqXml_ItemsSyncronize CurrentRequest { get { return mCurrentRequest; } }
        HttpReqXml_ItemsSyncronize mCurrentRequest = null;
#endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsMethod_ItemsSyncHandler()
        {
            mProductList = new List<CWmsProduct>(5);
            mSyncOkProducList = new List<CWmsProduct>(1);
            mSyncFailedProducList = new List<CWmsProduct>(1);
            mPwiUpdateList = new List<Product_WMS_Interface>(5);
            mPwiAddList = new List<Product_WMS_Interface>(5);
            mOwnerList = new Dictionary<string, Wms.Data.WmsOwner>(1);
        }

        /// <summary>
        /// 回收资源
        /// </summary>
        public void Dispose()
        {
            if (null != mProductList) mProductList.Clear();
            if (null != mPwiList) mPwiList.Clear();
            if (null != mPwiUpdateList) mPwiUpdateList.Clear();
            if (null != mPwiAddList) mPwiAddList.Clear();
            if (null != mOwnerList) mOwnerList.Clear();
        }

#region Methods
    /// <summary>
    /// 重置当前待更新的PWI实体，若待更新PWI列表和待插入PWI列表均为空，则当前待更新的PWI实体为null
    /// </summary>
    public void ResetCurrentPwiItem() {
            mCurrentPWI = (0 < PwiUpdateList.Count) ? PwiUpdateList[0] : ((0 < PwiAddList.Count) ? PwiAddList[0] : null);
        }

        /// <summary>
        /// 重置待更新的PWI项目List
        /// </summary>
        public void ClearPwiUpdateList() {
            if (null != PwiUpdateList)
            {
                //ret.Append("before ClearPwiUpdateList, Count={0}", PwiUpdateList.Count);
                PwiUpdateList.Clear();
                //ret.Append("after ClearPwiUpdateList, Count={0}", PwiUpdateList.Count);
            }
        }

        public void CurrentProductItemGoNext() { CurrentProductIndex++; }
        public void ResetCurrentProductItem() { CurrentProductIndex = 0; }
        public void RemoveCurrentOwner() { if (null != OwnerList && 0 < OwnerList.Count) OwnerList.Remove(CurrentOwner.WmsID); }

        /// <summary>
        /// 获取待更新的商品列表。若成功则返回获取到的商品数量，否则返回TError.
        /// 方法执行后，将获取筛选掉京东商品和供应商直发商品后，并用商品+规格替换原商品后的List。
        /// </summary>
        /// <param name="pMsg">若成功则返回string.Empty; 否则返回错误描述</param>
        /// <returns>若成功则返回获取到的商品数量</returns>
        public int GetProductList(out string pMsg)
        {
            int err = TError.WCF_RunError.Int();
            List<Product_ProductInfo_List_GuiGeList> ggLinkList = null;
            List<Product_ProductInfo_List_GuiGe> ggList = null;
            ProductList.Clear();

            try
            {
                // set filter
                var filters = new List<CommonFilterModel> {
                   
                    };

                if (0 < (err = CWmsDataFactory.GetProductList(filters, ref mProductList, out pMsg)))
                {
                    // find ProductList[i].MangoProduct.ProductId in GuiGe[ProductId].ToList()
                    filters.Clear();
                    filters.Add(new CommonFilterModel(Mis2014_ProductInfo.ProductId, "in", ProductList.Select(x => x.MangoProduct.ProductId).Cast<object>().ToList()));
                    ggLinkList = MangoFactory.GetGuiGeLinkList(filters); // get list of product_specification link
                    ggList = MangoFactory.GetGuiGeList(); // get list of specification

                    // 替换：[商品Id-规格Id]替换[商品Id]
                    var subList = ggLinkList.Select(link => new CWmsProduct(ProductList.Find(x => x.MangoProduct.ProductId == link.ProductId).MangoProduct, link, ggList));
                    ProductList.AddRange(subList);

                    // 去重
                    ProductList.RemoveAll(rm => (ggLinkList.Select(x => x.ProductId).Contains(rm.MangoProduct.ProductId) && string.IsNullOrEmpty(rm.MangoProduct.GGDict.Id)));
                    
                    err = ProductList.Count;    // 到这，ProductList为待更新的List
                    pMsg = string.Empty;
                }
                else
                {
                   C_WMS.Data.Utility.MyLog.Instance.Error("获取待更新的商品列表失败, err={0}, pMsg={1}", err, pMsg);
                }
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
                err = TError.WCF_RunError.Int();
                pMsg = ex.Message;
                ProductList.Clear();
            }
            return err;
        }

        /// <summary>
        /// 获取709中所有的商品（isDel!=1），返回获取到的商品数量
        /// </summary>
        /// <returns></returns>
        public int GetPwiProductList()
        {
            if (null != mPwiList)
                mPwiList.Clear();
            mPwiList = Dict709Handle.GetProductList();   // 709中所有商品行(isDel!=1)
            return mPwiList.Count;
        }

        /// <summary>
        /// 根据商品同步的结果更新缓存
        /// </summary>
        /// <param name="okList"></param>
        /// <param name="failedList"></param>
        /// <returns></returns>
        public int UpdateProductListBySyncResult(List<CWmsProduct> okList, List<CWmsProduct> failedList)
        {
            int err = TError.Pro_HaveNoDataParam3.Int();

            try
            {
                // 把okList更新到SyncOkProductList(重复的不添加)
                if (null != okList && 0< okList.Count)
                {
                    var newOkList = okList.Where(ok => !(SyncOkProducList.Select(orgOk => orgOk.ItemCode).Contains(ok.ItemCode))).ToList();
                    SyncOkProducList.AddRange(newOkList);
                }

                // 把failedList更新到SyncFailedProductList
                if (null != failedList && 0 < failedList.Count)
                {
                    var newFailedList = failedList.Where(f => !(SyncFailedProducList.Select(orgF => orgF.ItemCode).Contains(f.ItemCode))).ToList();
                    SyncFailedProducList.AddRange(newFailedList);



                    // 把SyncOKProductList中同步失败的商品remove掉
                    // 找到SyncOkProducList中有，但SyncFailedProducList中没有的商品作为新的同步成功List
                    var sList = SyncOkProducList.Where(ok => !(SyncFailedProducList.Select(f => f.ItemCode).Contains(ok.ItemCode))).ToList();

                    SyncOkProducList.Clear();
                    SyncOkProducList.AddRange(sList);
                }

                err = TError.RunGood.Int();
            }
            catch(Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
                err = TError.Pro_HaveNoDataParam3.Int();
            }

            return err;
        }

        /// <summary>
        /// 在pPwiList找pCwmsList所对应的项目，作为待更新到Dict709中的行列表。若成功则返回TError.RunGood，否则返回其他值。
        /// 该方法内部会捕捉所有出现的异常
        /// </summary>
        /// <param name="pCwmsList">源CWmsProduct列表，其中的商品应更新到709中</param>
        /// <param name="pPwiList">查找池Product_WMS_Interface列表</param>
        /// <param name="pUpdateOk">isUpdateOk值</param>
        /// <param name="pMsg">若成功则返回TError.RunGood，否则返回其他值。</param>
        /// <returns>若成功则返回TError.RunGood，否则返回其他值。</returns>
        public int GetUpdatePwiList(List<CWmsProduct> pCwmsList, List<Product_WMS_Interface> pPwiList, TDict285_Values pUpdateOk, out string pMsg)
        {
            if (null == pCwmsList || null == PwiList)
            {
                pMsg = "获取待更新到Dict709中的行列表失败，异常参数";
               C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
                return TError.Pro_HaveNoData.Int();
            }

            try
            {
                var queryList = pCwmsList.Select(p => pPwiList.Find(pwi => ((p.MangoProduct.ProductId == pwi.MapId1) && (p.MangoProduct.GGDict.Id.Int() == pwi.MapId2))) ).ToList();
                
                queryList = queryList.Where(q => (null != q)).ToList(); // PWI里可能有null的实体

                var tmpList = queryList.Select(tmp => new Product_WMS_Interface
                {
#region
                    WMS_InterfaceId = tmp.WMS_InterfaceId,
                    MapCalssID = TDict709_Value.EUpdateProduct.Int(),
                    MapId1 = tmp.MapId1,
                    MapId2 = tmp.MapId2,
                    IsUpdateOK = pUpdateOk.Int(),
                    IsDel = TDict285_Values.ENormal.Int(),
                    AddTime = tmp.AddTime,
                    AddUserid = tmp.AddUserid,
                    LastTime = DateTime.Now,
                    UpdateUserID = CommonFrame.LoginUser.UserId.Int(),
                    DisOrder = tmp.DisOrder
#endregion
                });

                PwiUpdateList.Clear();
                PwiUpdateList.AddRange(tmpList);

                pMsg = string.Empty;
                return TError.RunGood.Int();
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
                PwiUpdateList.Clear();

                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);

                return TError.Pro_HaveNoDataParam3.Int();
            }
        }
       
        /// <summary>
        /// 获取待添加到Dict709中的行列表。若成功则返回TError.RunGood，否则返回其他值
        /// </summary>
        /// <param name="pPwiList"></param>
        /// <param name="pMsg">若成功则返回string.Empty; 否则返回错误描述</param>
        /// <returns>若成功则返回TError.RunGood，否则返回其他值</returns>
        public int GetAddPwiList(List<Product_WMS_Interface> pPwiList, out string pMsg)
        {
            int err = TError.RunGood.Int();
            if (null == pPwiList) //  || 0 == pPwiList.Count)
            {
                pMsg = "获取待添加到709中的商品列表失败，WCF获取709商品数据失败。";
                return TError.WCF_RunError.Int();
            }

            try
            {
                var rslt = ProductList.Where(cwms =>
                    !pPwiList.Exists(pwi => (cwms.MangoProduct.ProductId == pwi.MapId1 && cwms.MangoProduct.GGDict.Id.Int() == pwi.MapId2))
                    ).ToList();
                
                var tmpList = rslt.Select(x => new Product_WMS_Interface
                {
#region
                    MapCalssID = TDict709_Value.EAddProduct.Int(),
                    MapId1 = x.MangoProduct.ProductId,
                    MapId2 = x.MangoProduct.GGDict.Id.Int(),
                    IsUpdateOK = TDict285_Values.EUnknown.Int(),
                    IsDel = TDict285_Values.ENormal.Int(),
                    AddTime = DateTime.Now,
                    AddUserid = CommonFrame.LoginUser.UserId.Int(),
                    LastTime = DateTime.Now,
                    UpdateUserID = CommonFrame.LoginUser.UserId.Int(),
                    DisOrder = Product_WMS_Interface_Properties.cIntDisorderDefault
#endregion
                });

                PwiAddList.Clear();
                PwiAddList.AddRange(tmpList);
                pMsg = string.Empty;
                err = TError.RunGood.Int();
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
                pMsg = ex.Message;
                err = TError.WCF_RunError.Int();
                PwiAddList.Clear();
            }
            return err;
        }

        /// <summary>
        /// 更新当前待操作的709行，若执行成功则返回1(成功更新的行)；若没有待更新的行则返回0；其他情况返回TError
        /// </summary>
        /// <param name="pMsg">若成功则返回string.Empty; 否则返回错误描述</param>
        /// <returns>若执行成功则返回1(成功更新的行)；若没有待更新的行则返回0；其他情况返回TError</returns>
        public int UpdatePwiItem(out string pMsg)
        {
            int err = 0;
            if (null == CurrentPWI)
            {
                pMsg = "当前没有待更新的PWI实体";
                return err;
            }

            // 插入或更新
            if (null != CurrentPWI.WMS_InterfaceId)
            {
                err = Dict709Handle.UpdateRow_Product(CurrentPWI);
            }
            else
            {
                err = Dict709Handle.AddRow_Product(CurrentPWI);
            }

            // 处理WCF结果
            if (0 < err)
            {
                if (null != CurrentPWI.WMS_InterfaceId)
                    PwiUpdateList.Remove(CurrentPWI);   // remove掉成功更新的PWI实体
                else
                    PwiAddList.Remove(CurrentPWI);   // remove掉成功插入的PWI实体

                ResetCurrentPwiItem();
                pMsg = string.Empty;
                return 1;
            }
            else
            {
                pMsg = string.Format("WCF<PWI更新商品:{0}>, 执行失败[{1}]", CurrentPWI.MapId1, err);
               C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
                return err;
            }
        }

        /// <summary>
        /// 插入当前待操作的709行，若执行成功则返回1(成功插入的行)；若没有待插入的行则返回0；其他情况返回TError
        /// </summary>
        /// <param name="pMsg">若成功则返回string.Empty; 否则返回错误描述</param>
        /// <returns>若执行成功则返回1(成功插入的行)；若没有待插入的行则返回0；其他情况返回TError</returns>
        public int AddPwiItem(out string pMsg)
        {
            int err = 0;
            if (null == CurrentPWI)
            {
                pMsg = "当前没有待插入的PWI实体";
                return err;
            }

            if (0 < (err = Dict709Handle.AddRow_Product(CurrentPWI)))
            {
                PwiAddList.Remove(CurrentPWI);  // remove掉成功插入的PWI实体
                ResetCurrentPwiItem();
                pMsg = string.Empty;
                return 1;
            }
            else
            {
                pMsg = string.Format("WCF<PWI插入商品:{0}>, 执行失败", CurrentPWI.MapId1);
                return err;
            }
        }

        /// <summary>
        /// 获取当前待操作的商品的条码，若执行成功则返回1(成功获取数量)；若没有待获取的商品则返回0；其他情况返回TError
        /// </summary>
        /// <param name="pMsg">若成功则返回string.Empty; 否则返回错误描述</param>
        /// <returns>若执行成功则返回1(成功获取数量)；若没有待获取的商品则返回0；其他情况返回TError</returns>
        public int GetBarCode(out string pMsg)
        {
            //ret.Append("获取当前待操作的商品的条码，开始");   // for debug
            int err = 0;
            pMsg = string.Empty;
            if (null == CurrentProduct)
            {
                pMsg = "当前没有待操作的商品实体";
               C_WMS.Data.Utility.MyLog.Instance.Error("获取当前待操作的商品的条码，结束，{0}", pMsg);   // for debug
                return err;
            }

            CWmsInventory_Monitoring trans = new CWmsInventory_Monitoring();
            var respBody = trans.DoTransaction(CurrentProduct, CWmsConsts.cStrDefaultWarehouseName);  //TODO: 联调时仓库用LTCK, 请求条形码的系列操作

            // handle response
            if (null == respBody)   // 联网失败
            {
                pMsg = string.Format("获取商品[{0}]的条形码失败", CurrentProduct.MangoProduct.ProductId);

               C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);   // for debug

                err = TError.Pro_HaveNoData.Int();
            }
            else if (respBody.IsSuccess())  // 联网成功，服务器响应success
            {
                // 更新商品条码，若获取到的条码是string.Empty，那么用itemCode作为barCode
                var barCode = (respBody as HttpRespXml_InventoryMonitoring).items[0].barcodeNum;
                ProductList.Find(p => p.ItemCode == CurrentProduct.ItemCode).WmsProduct.barCode = (string.IsNullOrEmpty(barCode)) ? CurrentProduct.MangoProduct.ProductId.ToString() : barCode;
                err = 1;
            } // else if ("success" == respBody.flag)
            else  // 联网成功，服务器响应其他
            {
                if (!respBody.message.Contains("商品不存在"))
                {
                    ProductList.Remove(CurrentProduct); // CWMS系统里当条商品， 获取条形码失败了，本次不同步这个商品
                }
                pMsg = string.Format("服务器响应: {0}", respBody.message);
                err = 1;

               C_WMS.Data.Utility.MyLog.Instance.Error("联网成功，服务器响应其他，pMsg={0}", pMsg);   // for debug
            } // else if ("failure" == respBody.flag)

            return err;
        }

        /// <summary>
        /// 获取当前待同步的Request XML序列化类实体。若获取成功则返回1；若没有待同步的Request则返回0；其他情况返回TError
        /// </summary>
        /// <param name="pMsg">若成功则返回string.Empty; 否则返回错误描述</param>
        /// <returns>若获取成功则返回1；若没有待同步的Request则返回0；其他情况返回TError</returns>
        public int GetReqBodyXml(out string pMsg)
        {
            if (null == CurrentOwner)
            {
                pMsg = "当前没有剩余的货主的商品待同步";

                //ret.Append(pMsg);
                return 0;
            }

            try
            {
                mCurrentRequest = new HttpReqXml_ItemsSyncronize();
                CurrentRequest.actionType = CWmsConsts.sStrApiItemsSyncActionTypeUpdate;
                CurrentRequest.ownerCode = CurrentOwner.WmsID;
                CurrentRequest.warehouseCode = "OTHER";    // 统仓统配，传OTHER

                //  应该是更新这个货主的所有商品（根据货主所辖仓库中的商品）向reqXml.items中添加所有更新709成功的商品
                
                Product_ProductCategory cate = null;
                var itemsList = ProductList.Select(p => new HttpReqXml_ItemsSync_Item
                {
                    itemCode = p.ItemCode,
                    itemName = p.MangoProduct.Title,
                    barCode = p.WmsProduct.barCode,
                    skuProperty = p.MangoProduct.GGDict.Name,
                    stockUnit = p.MangoProduct.Unit,//计量单位//TODO
                    categoryId = p.MangoProduct.ProductCategoryId.ToString(),//类别id
                    categoryName = (null != (cate = Simple_ProductCategory_Cache.ProductCategory_Cache_Store.All().Find(e => e.ProductCategoryId == p.MangoProduct.ProductCategoryId.Int()))) ? cate.ProductCategory : "", //类别名称
                    retailPrice = p.MangoProduct.WuPinMoney.ToString(),
                    brandName = p.MangoProduct.Brands,
                    isShelfLifeMgmt = "N", // 0 < p.MangoProduct.ZhiBaotime ? "Y" : "N", // 默认不采集效期
                    remark = p.MangoProduct.Remark,
                    createTime = p.MangoProduct.AddTime.ToString(),
                    updateTime = p.MangoProduct.UpdateTime.ToString(),
                }).ToList();

                CurrentRequest.items = itemsList;

                //ret.Append("当前待同步的货主： {0}", CurrentRequest);

                pMsg = string.Empty;
                return 1;
            }
            catch(Exception ex)
            {
                pMsg = ex.Message;

                // for debug
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
                return TError.Ser_ErrorPost.Int();
            }
        }


        /// <summary>
        /// 获取货主，若操作成功则返回获取的货主数量，否则返回TError
        /// </summary>
        /// <returns>若操作成功则返回获取的货主数量，否则返回TError</returns>
        public int GetOwners()
        {
            List<MangoWarehouse> wList = MangoFactory.GetWarehouses();
            mOwnerList = MangoFactory.GetVOwners(wList);

            return (null == OwnerList || 0 == OwnerList.Count) ? TError.Pro_HaveNoData.Int() : OwnerList.Count;
        }
#endregion
    } // class CWmsMethod_ItemsSyncHandler
}
