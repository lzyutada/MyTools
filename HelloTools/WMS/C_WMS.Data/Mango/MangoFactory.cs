using System;
using System.Collections.Generic;
using System.Linq;
using MisModel;
using MangoMis.Frame.DataSource.Simple;
using TT.Common.Frame.Model;
using C_WMS.Data.Mango.Data;
using MangoMis.Frame.ThirdFrame;
using MangoMis.Frame.Helper;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data;
using C_WMS.Data.CWms.CWmsEntity;
using C_WMS.Data.Wms.Data;
using MangoMis.Frame.Cache.StoreCache;
using C_WMS.Interface.Utility;
using System.Reflection;
using MangoMis.MisFrame.Frame;

namespace C_WMS.Data.Mango
{
    /// <summary>
    /// 商城实体工厂类
    /// </summary>
    class MangoFactory
    {
#if true
        static public List<Product_WMS_Interface> GetV_PwiList(TDict709_Value pMapClassId, string pId, IEnumerable<string> pSubIdList)
        {
            List<Product_WMS_Interface> rsltList = new List<Product_WMS_Interface>();
            try
            {
                rsltList = pSubIdList.Select(x => new Product_WMS_Interface()
                {
                    WMS_InterfaceId = 0,
                    MapCalssID = pMapClassId.Int(),
                    MapId1 = pId.Int(),
                    MapId2 = x.Int(),
                    IsUpdateOK = TDict285_Values.EUnknown.Int(),
                    IsDel = TDict285_Values.EUnknown.Int(),
                    //AddTime = DateTime.MinValue,
                    //AddUserid = 0,
                    LastTime = DateTime.Now,
                    UpdateUserID = CommonFrame.LoginUser.UserId.Int(),
                    DisOrder = CWmsConsts.cIntDefaultDisorder
                }).ToList();
            }
            catch (Exception ex)
            {
                rsltList.Clear();
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "MangoFactory.GetPwiListFromSubEntryOrders({0})发生异常", pId);
            }
            return rsltList;
        }

        static public int GetV_Order<T>(List<CommonFilterModel> pFilters, out T orderList, out string pErrMsg) where T : class, new()
        {
            throw new NotImplementedException(""); // TODO: 
        }

        /// <summary>
        /// <para>create and return a new object of TEntity by the indicated id of order.</para>
        /// <para>return the new created object of TEntity; return null if failed.</para>
        /// </summary>
        /// <typeparam name="TEntity">class type of order</typeparam>
        /// <param name="id">order id</param>
        /// <returns>return the new created object of TEntity; return null if failed.</returns>
        static public TEntity NewOrder<TEntity>(string id)
        {
            throw new NotImplementedException();
        }
#else
        static public List<Product_WMS_Interface> GetPwiListFromSubEntryOrders(CWmsEntryOrder pOrder)
        {
            List<Product_WMS_Interface> rsltList = new List<Product_WMS_Interface>();
            if (null == pOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("MangoFactory.GetPwiListFromSubEntryOrders(), 非法空入参pOrder.");
                return rsltList;
            }

            try
            {
                rsltList = pOrder.SubOrders.Select(x => new Product_WMS_Interface()
                    {
                        WMS_InterfaceId = 0,
                        MapCalssID = TDict709_Value.EEntryOrder.Int(),
                        MapId1 = pOrder.Id.Int(),
                        MapId2 = x.Value.Id.Int(),
                        IsUpdateOK = TDict285_Values.EUnknown.Int(),
                        IsDel = TDict285_Values.EUnknown.Int(),
                        AddTime = DateTime.MinValue,
                        AddUserid = 0,
                        LastTime = DateTime.Now,
                        UpdateUserID = CommonFrame.LoginUser.UserId.Int(),
                        DisOrder = 100
                    }).ToList();
            }
            catch (Exception ex)
            {
                rsltList.Clear();
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "MangoFactory.GetPwiListFromSubEntryOrders({0})发生异常", pOrder?.Id);
            }
            return rsltList;
        }

        static public List<Product_WMS_Interface> GetPwiListFromSubStockoutOrders(CWmsExWarehouseOrder pOrder)
        {
            List<Product_WMS_Interface> rsltList = new List<Product_WMS_Interface>();
            if (null == pOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("MangoFactory.GetPwiListFromSubEntryOrders(), 非法空入参pOrder.");
                return rsltList;
            }

            try
            {
                rsltList = pOrder.SubOrders.Select(x => new Product_WMS_Interface()
                {
                    WMS_InterfaceId = 0,
                    MapCalssID = TDict709_Value.EEntryOrder.Int(),
                    MapId1 = pOrder.Id.Int(),
                    MapId2 = x.Value.Id.Int(),
                    IsUpdateOK = TDict285_Values.EUnknown.Int(),
                    IsDel = TDict285_Values.EUnknown.Int(),
                    AddTime = DateTime.MinValue,
                    AddUserid = 0,
                    LastTime = DateTime.Now,
                    UpdateUserID = CommonFrame.LoginUser.UserId.Int(),
                    DisOrder = 100
                }).ToList();
            }
            catch (Exception ex)
            {
                rsltList.Clear();
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "MangoFactory.GetPwiListFromSubEntryOrders({0})发生异常", pOrder?.Id);
            }
            return rsltList;
        }
#endif
        /// <summary>
        /// 将int值订单类型转换为TMangoOrderType
        /// </summary>
        /// <param name="pOrderType"></param>
        /// <returns></returns>
        static public TMangoOrderType To_TMangoOrderType(int pOrderType)
        {
            switch (pOrderType)
            {
                case 0: return TMangoOrderType.EUnknown; // 未知
                case 1: return TMangoOrderType.EBMDD; // 部门订单
                case 2: return TMangoOrderType.EGRGM; // 个人购买
                case 3: return TMangoOrderType.EZSBD; // 装饰补单
                case 4: return TMangoOrderType.EBPSQ; // 备品申请
                case 5: return TMangoOrderType.EFLSQ; // 福利申请
                case 6: return TMangoOrderType.EWLCK; // 芒果网络为货主，无订单出库
                default: return TMangoOrderType.EDefaultType;
            }
        }

#if false // useless
        /// <summary>
        /// 获取芒果商城的单据
        /// </summary>
        /// <param name="pOrderType">单据类别，可以是商城订单、采购订单、退货订单等。</param>
        /// <param name="pOrderId">单据ID</param>
        /// <returns>返回芒果商城单据实例</returns>
        /// <exception cref="NotSupportedException"></exception>
        static public EntityBase GetMangoOrder(TCWmsOrderCategory pOrderType, string pOrderId)
        {
            switch (pOrderType)
            {
                case TCWmsOrderCategory.EEntryOrder: return GetMangoEntryOrder(pOrderId);
                case TCWmsOrderCategory.EPurchaseOrder: return GetMangoPurchaseOrder(pOrderId);
                case TCWmsOrderCategory.EExwarehouseOrder: return GetMangoExwarehouseOrder(pOrderId);
                case TCWmsOrderCategory.EReturnOrder: return GetMangoReturnOrder(pOrderId);
                case TCWmsOrderCategory.EMallOrder: return GetMangoMallOrder(pOrderId); // TODO: 不会被调用
                default: throw new NotSupportedException(string.Format("生成芒果商城的单据异常！不支持的单据类型:{0}", pOrderType.ToString()));
            }
        }
#endif
#if false // 由MisModel处理
        /// <summary>
        /// 根据过滤器获取满足条件的全部入库订单。若执行成功则返回获取的行数，否则返回TError.WCF_RunError
        /// </summary>
        /// <param name="pFilters">过滤器</param>
        /// <param name="pOutList">返回满足条件的全部入库订单，若执行失败则返回数量为0的列表实体</param>
        /// <param name="pErrMsg"></param>
        /// <returns></returns>
        static public int GetVMangoStockoutOrders(List<CommonFilterModel> pFilters, out List<MangoExwarehouseOrder> pOutList, out string pErrMsg)
        {
            // get all deliveryorders by filters
            int err = TError.WCF_RunError.Int();
            pOutList = new List<MangoExwarehouseOrder>(1);
            List<Product_PeiSong_ProductMain> deliveryList = null;
            List<int> stockIdList = null;

            err = MisModelFactory.GetMisOrderList(pFilters, out deliveryList, out pErrMsg);
            if (0 >= err || null == deliveryList || 0 == deliveryList.Count)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("在{0}中，根据过滤器获取主配送订单失败(err={1},orderList={2}), errMsg={3}", MethodBase.GetCurrentMethod().Name, err, deliveryList, pErrMsg);
                return err;
            }
            else
            {
                C_WMS.Data.Utility.MyLog.Instance.Info("在{0}中，根据过滤器获取主配送订单完成，err={1}, 配送单数量={2}", MethodBase.GetCurrentMethod().Name, err, deliveryList.Count);
            }

            // 根据主配送订单获取主出库单
            stockIdList = deliveryList.Select(x => x.ProductIOputMainId.Int())?.ToList();
            var wcfRslt = WCF<Product_Warehouse_ProductMainOutput>.Query(stockIdList);
            if (null != wcfRslt && null != wcfRslt.Data && 0 < wcfRslt.Data.Count)
            {
                foreach (var misModel in wcfRslt.Data)
                {
                    MangoExwarehouseOrder meo = new MangoExwarehouseOrder();
                    meo.CopyFrom(misModel);
                    pOutList.Add(meo);
                }
                {
                    // for debug
                    C_WMS.Data.Utility.MyLog.Instance.Info("在{0}中，根据过滤器获取主出库单完成，wcfRslt.RetInt={1}, 出库单数量={2}", MethodBase.GetCurrentMethod().Name, wcfRslt.RetInt, pOutList.Count);
                }
                return pOutList.Count;
            }
            else
            {
                pErrMsg = (null == wcfRslt) ? "WCF<TEntity>.Query(idList)返回空" : wcfRslt.Debug;
                C_WMS.Data.Utility.MyLog.Instance.Error("在{0}中，根据主配送订单列表获取主出库订单列表失败，err={1}, errMsg={2}", MethodBase.GetCurrentMethod().Name, err, pErrMsg);
                return err = (null == wcfRslt) ? TError.WCF_RunError.Int() : wcfRslt.RetInt;
            }
        }

        /// <summary>
        /// 获取芒果商城出库订单及其所有子出库单实例
        /// </summary>
        /// <param name="pOrderId">主出库订单ID</param>
        /// <returns>返回MangoExwarehouseOrder实例</returns>
        static public EntityBase GetMangoExwarehouseOrder(string pOrderId)
        {
            var ret = new ThirdResult<List<object>>(string.Format("获取主出库订单({0}) 开始", pOrderId));

            MangoExwarehouseOrder outOrder = null;
            EntityBase tmpOrder = null;

            try
            {
                // get order from Mis2014
                tmpOrder = MisModelFactory.GetMisOrder(TCWmsOrderCategory.EExwarehouseOrder, pOrderId);
                if (null == tmpOrder)
                {
                    ret.Append(string.Format("获取主出库订单({0}) 结束，获取主出库单失败", pOrderId));
                }
                else
                {
                    outOrder = new MangoExwarehouseOrder(tmpOrder as Product_Warehouse_ProductMainOutput);
                }
                ret.Append(string.Format("获取主出库订单({0}) 结束, outOrder={1}", pOrderId, outOrder));
                ret.End();
                return outOrder;
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return null;
            }
        }

        /// <summary>
        /// 根据主出库订单ID获取其所有子出库订单
        /// </summary>
        /// <param name="pId">主出库订单Id</param>
        /// <param name="pOutList">返回所有子出库订单实例的列表，若执行失败则返回Count=0的列表</param>
        /// <returns>若成功则返回String.Empty；否则返回错误描述</returns>
        static public string GetMangoExwarehouseSubOrderList(string pId, out List<MangoSubExwarehouseOrder> pOutList)
        {
            //C_WMS.Data.Utility.MyLog.Instance.Info("在{0}中, 根据主出库订单{1}获取其所有子出库单开始", System.Reflection.MethodBase.GetCurrentMethod().Name, pId);

            string errMsg = string.Empty;
            List<Product_Warehouse_ProductOutput> tmpList = null;
            pOutList = new List<MangoSubExwarehouseOrder>(1);

            try
            {
                // get sub orders from Mis2014
                errMsg = MisModelFactory.GetSubExwarehouseOrderList(pId, out tmpList);
                if (null != tmpList && 0 < tmpList.Count)
                {
                    foreach (var misOrder in tmpList)
                    {
                        MangoSubExwarehouseOrder subOrder = new MangoSubExwarehouseOrder(misOrder);
                        pOutList.Add(subOrder);
                    }
                    errMsg = string.Empty;
                }
                else
                {
                    errMsg += string.Format("\r\n获取{0}的子出库订单失败, tmpList={1}, Count={2}", pId, tmpList, tmpList?.Count);
                    C_WMS.Data.Utility.MyLog.Instance.Error("在{0}中, {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, errMsg);
                }

                //C_WMS.Data.Utility.MyLog.Instance.Info("{0}结束, message={1}", System.Reflection.MethodBase.GetCurrentMethod().Name, errMsg);
                return errMsg;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "{0}发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
                return errMsg;
            }
        }

        /// <summary>
        /// 获取芒果商城采购订单及其所有子单的实例
        /// </summary>
        /// <param name="pOrderId">主采购订单ID</param>
        /// <returns>返回EntityBase实例，若失败则返回null</returns>
        static protected EntityBase GetMangoPurchaseOrder(string pOrderId)
        {
            var ret = new ThirdResult<int>(string.Format("获取芒果商城采购订单{0}及其所有子单的实例开始", pOrderId));

            MangoPurchaseOrder outOrder = null;
            EntityBase tmpOrder = null;
            List<Product_Warehouse_ProductBuy> tmpList = null;

            try
            {
                // get order from Mis2014
                tmpOrder = MisModelFactory.GetMisOrder(TCWmsOrderCategory.EPurchaseOrder, pOrderId);
                if (null == tmpOrder)
                {
                    ret.Append(string.Format("获取芒果商城采购订单{0}及其所有子单的实例结束，获取主采购单失败", pOrderId));
                    ret.End();
                    return outOrder;
                }

                // get sub orders from Mis2014
                if (string.IsNullOrEmpty(MisModelFactory.GetSubPurchaseOrderList(pOrderId, out tmpList)))
                {
                    foreach (var misOrder in tmpList)
                    {
                        outOrder.SubOrders.Add(misOrder.ProductBuyId.ToString(), new MangoSubPurchaseOrder(misOrder));
                    }

                    ret.Append(string.Format("获取芒果商城采购订单{0}及其所有子单的实例结束", pOrderId));
                    ret.End();
                    return outOrder;
                }
                else
                {
                    ret.Append(string.Format("获取芒果商城采购订单{0}及其所有子单的实例结束，获取子采购单失败", pOrderId));
                    ret.End();
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return null;
            }
        }

        /// <summary>
        /// 根据过滤器获取满足条件的全部入库订单。若执行成功则返回获取的行数，否则返回TError.WCF_RunError
        /// </summary>
        /// <param name="pFilters">过滤器</param>
        /// <param name="pOutList">返回满足条件的全部入库订单，若执行失败则返回数量为0的列表实体</param>
        /// <param name="pErrMsg"></param>
        /// <returns></returns>
        static public int GetVMangoEntryOrders(List<CommonFilterModel> pFilters, out List<MangoEntryOrder> pOutList, out string pErrMsg)
        {
            int err = TError.WCF_RunError.Int();
            pOutList = new List<MangoEntryOrder>(1);
            List<Product_Warehouse_ProductMainInput> tmpList = null;

            err = MisModelFactory.GetMisOrderList(pFilters, out tmpList, out pErrMsg);
            if (0 >= err || null == tmpList || 0 == tmpList.Count)
            {
                pErrMsg = string.Format("在{0}中，根据过滤器获取满足条件的全部入库订单失败, err={1}， orderList={2}, errMsg={3}", System.Reflection.MethodBase.GetCurrentMethod().Name, err, tmpList, pErrMsg);
               C_WMS.Data.Utility.MyLog.Instance.Error(pErrMsg);
                return err;
            }
            else
            {
                foreach (var misModel in tmpList)
                {
                    MangoEntryOrder meo = new MangoEntryOrder();
                    meo.CopyFrom(misModel);
                    pOutList.Add(meo);
                }
                pErrMsg = string.Empty;
                return err;
            }
        }

        /// <summary>
        /// 获取芒果商城的主入库单及其所有子入库单
        /// </summary>
        /// <param name="pOrderId">主入库单ID</param>
        /// <returns>返回单据实例，若失败则返回null</returns>
        static protected EntityBase GetMangoEntryOrder(string pOrderId)
        {
            var ret = new ThirdResult<int>(string.Format("获取芒果商城的主入库单({0})及其所有子入库单 开始", pOrderId));

            MangoEntryOrder outOrder = null;
            EntityBase tmpOrder = null;
            //List<Product_Warehouse_ProductInput> tmpList = null;

            try
            {
                // get order from Mis2014
                tmpOrder = MisModelFactory.GetMisOrder(TCWmsOrderCategory.EEntryOrder, pOrderId);
                if (null == tmpOrder)
                {
                    ret.Append(string.Format("获取芒果商城的主入库单({0})及其所有子入库单结束，获取主出库单失败", pOrderId));
                    ret.End();
                    return outOrder;
                }

                outOrder = new MangoEntryOrder();
                outOrder.CopyFrom(tmpOrder as Product_Warehouse_ProductMainInput);
                ret.Append(string.Format("获取芒果商城的主入库单({0})及其所有子入库单结束, outOrder={1}", pOrderId, outOrder));
                ret.End();
                return outOrder;
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return null;
            }
        }

        /// <summary>
        /// 根据主订单Id获取子订单列表
        /// </summary>
        /// <param name="pId">主订单Id</param>
        /// <param name="pList">返回子订单列表，若操作失败则返回Count=0的列表</param>
        /// <returns>若操作成功则返回string.Empty；否则返回错误描述</returns>
        static public string GetVMangoSubEntryOrders(string pId, out List<MangoSubEntryOrder> pList)
        {
            //C_WMS.Data.Utility.MyLog.Instance.Info("在{0}中, 根据主订单({1})获取子订单列表, 开始", System.Reflection.MethodBase.GetCurrentMethod().Name, pId);

            string errMsg = string.Empty;
            List<Product_Warehouse_ProductInput> tmpList = null;
            pList = new List<MangoSubEntryOrder>(1);

            try
            {
                // get sub orders from Mis2014
                errMsg = MisModelFactory.GetSubEntryOrderList(pId, out tmpList);
                if (null == tmpList || 0 >= tmpList.Count)
                {
                    errMsg += string.Format("\r\n在{0}中, 获取主订单{1}的子订单异常（tmpList={2}）或子订单数量为0", System.Reflection.MethodBase.GetCurrentMethod().Name, pId, tmpList);
                    C_WMS.Data.Utility.MyLog.Instance.Error(errMsg);
                    return errMsg;
                }
                
                // copy to output list(pList).
                foreach (var t in tmpList)
                {
                    pList.Add(new MangoSubEntryOrder(t));
                }
                //C_WMS.Data.Utility.MyLog.Instance.Info("根据主订单({0})获取子订单列表 完成, 生成数量:{1}", pId, pList.Count);
            }
            catch (Exception ex)
            {
                pList.Clear();
                errMsg = ex.Message;
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "{0}发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            return errMsg;
        }


        /// <summary>
        /// 生成芒果商城退货订单实例
        /// </summary>
        /// <param name="pOrderId">主退货单ID</param>
        /// <returns>返回MangoReturnOrder实例</returns>
        static public EntityBase GetMangoReturnOrder(string pOrderId)
        {
            var ret = new ThirdResult<int>(string.Format("生成芒果商城退货订单{0}实例 开始", pOrderId));

            MangoReturnOrder outOrder = null;
            EntityBase tmpOrder = null;

            try
            {
                // get order from Mis2014
                tmpOrder = MisModelFactory.GetMisOrder(TCWmsOrderCategory.EReturnOrder, pOrderId);
                if (null != tmpOrder)
                {
                    outOrder = new MangoReturnOrder(tmpOrder as Product_TuiHuo_main);
                }
                ret.Append(string.Format("获取Mis2014系统中定义类型的实体完成:{0}，根据获取的实体创建MangoReturnOrder={1}", tmpOrder, outOrder));
                ret.End();
                return outOrder;
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return null;
            }
        }

        /// <summary>
        /// 获取芒果商城的商城订单及其子订单实体
        /// </summary>
        /// <param name="pOid">主订单Id</param>
        /// <returns>若成功则返回订单实体；否则返回null</returns>
        static public EntityBase GetMangoMallOrder(string pOid)
        {
            var ret = new ThirdResult<int>(string.Format("获取芒果商城的商城订单{0}及其子订单实体", pOid));

            MangoMallOrder outOrder = null;
            EntityBase tmpOrder = null;

            try
            {
                // get order from Mis2014
                tmpOrder = MisModelFactory.GetMisOrder(TCWmsOrderCategory.EMallOrder, pOid);
                if (null != tmpOrder)
                    outOrder = new MangoMallOrder(tmpOrder as Product_User_DingDan); // TODO: 暂不需获取子订单
                ret.Append(string.Format("获取芒果商城的商城订单{0}及其子订单实体完成:{1}，创建MangoReturnOrder={2}", pOid, tmpOrder, outOrder));
                ret.End();
                return outOrder;
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return null;
            }
        }

        /// <summary>
        /// 根据主退货单Id获取子退货单列表
        /// </summary>
        /// <param name="pOrderId">主退货单Id</param>
        /// <param name="pList">子退货单列表，MangoSubReturnOrder；若失败则返回Count=0的列表实体</param>
        /// <returns>若成功则返回string.Empty; 否则返回错我描述</returns>
        static public string GetVSubReturnOrders(string pOrderId, out List<MangoSubReturnOrder> pList)
        {
            var ret = new ThirdResult<int>(string.Format("根据主退货单{0}获取子退货单列表开始", pOrderId));

            string errMsg = string.Empty;
            List<Product_TuiHuo> tmpList = null;
            pList = new List<MangoSubReturnOrder>(1);

            try
            {
                // get sub orders from Mis2014
                errMsg = MisModelFactory.GetSubReturnOrderList(pOrderId, out tmpList);
                if (!string.IsNullOrEmpty(errMsg) || 0 >= tmpList.Count)
                {
                    ret.Append(string.Format("获取主退货单获取子退货单列表失败, errMsg={0}", errMsg));
                    ret.End();
                    return errMsg;
                }

                // copy to output list(pList).
                foreach (var t in tmpList)
                {
                    pList.Add(new MangoSubReturnOrder(t));
                }
                ret.Append(string.Format("根据主退货单Id获取子退货单列表(List<MangoSubReturnOrder>)结束: errMsg={0}", errMsg));
            }
            catch (Exception ex)
            {
                pList.Clear();
                errMsg = ex.Message;

                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
            }

            ret.End();
            return errMsg;
        }

        /// <summary>
        /// 根据子采购订单Id获取其应收商品数量。
        /// </summary>
        /// <param name="pId">子采购订单Id</param>
        /// <param name="pPlanQty">返回应收数量，若获取成功则返回应收数量；否则返回-1。</param>
        /// <returns>若获取成功则返回TError.RunGood；否则返回其他值</returns>
        static public int GetPlanQuantityByPid(string pId, out int pPlanQty)
        {
            var ret = new ThirdResult<int>(string.Format("根据子入库订单Id获取其应收商品数量，开始。"));
            try
            {
                pPlanQty = -1;
                if (0 >= pId.Int())
                {
                    ret.Append(string.Format("根据子入库订单Id获取其应收商品数量，失败。pId={0}", pId));
                    ret.End();
                    return TError.Post_ParamError.Int();
                }
                else
                {
                    var rslt = WCF<Product_Warehouse_ProductBuy>.Query(pId.Int());
                    if (null == rslt || null == rslt.Data)
                    {
                        ret.Append(string.Format("根据子入库订单Id获取其应收商品数量, WCF查询错误，null == rslt || null == rslt.Data"));
                        ret.End();
                        return TError.WCF_RunError.Int();
                    }
                    else
                    {
                        pPlanQty = CWmsUtility.Decimal2Int(rslt.Data.ProductCount.Decimal());
                        ret.Append(string.Format("根据子入库订单Id获取其应收商品数量，结束. rslt.Data.ProductCount={0}, pPlanQty={1}", rslt.Data.ProductCount, pPlanQty));
                        ret.End();
                        return TError.RunGood.Int();
                    }
                }
            }
            catch (Exception ex)
            {
                pPlanQty = -1;
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
                ret.End();
                return TError.WCF_RunError.Int();
            }
        }
#endif
#if false // 位置放错了
        /// <summary>
        /// 获取商品全部数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// </remarks>
        public static List<CWmsProduct> GetMangoProductList()
        {
            var ret = new ThirdResult<List<object>>("获取商品全部数据开始");

        #region temp variables definition
            List<Product_ProductInfo_List> mallProductList = null;
            List<Product_ProductInfo_List_GuiGeList> ggLinkList = null;
            List<Product_ProductInfo_List_GuiGe> ggList = null;
            List<CWmsProduct> outPList = new List<CWmsProduct>(1);
        #endregion
            mallProductList = GetMangoMallProductList(); // get list of all product info
            var filters = new List<CommonFilterModel>();
            filters.Add(new CommonFilterModel("ProductId", "in", mallProductList.Select(x=>x.ProductId).Cast<object>().ToList()));
            ggLinkList = GetGuiGeLinkList(filters); // get list of product_specification link
            ggList = GetGuiGeList(); // get list of specification
                        
        #region tranverse ggLinkList
            foreach (var tmpGgItem in ggLinkList)
            {
                CWmsProduct newMpItem = new CWmsProduct();
                newMpItem.MangoProduct.Copy(CopyFromProduct_ProductInfo_List(mallProductList.Find(x => x.ProductId == tmpGgItem.ProductId)));
                newMpItem.ItemCode = tmpGgItem.ProductId + "-" + tmpGgItem.ProductGuiGeID;
                newMpItem.MangoProduct.GGDict.Id = tmpGgItem.ProductGuiGeID.ToString();
                newMpItem.MangoProduct.GGDict.GuiGeList.Clear();
                newMpItem.MangoProduct.GGDict.GuiGeList.AddRange(ggList.Where(x => (-1 < tmpGgItem.GuiGeIDList.IndexOf(x.GuiGeID.ToString()))).Select(y => new GuiGeProp(y.GuiGeID.ToString(), y.GuiGeName)).ToList());
                //newMpItem.MangoProduct.GGDict.GetSpecification();
                outPList.Add(newMpItem);
            } // foreach (var tmpGgItem in ggLinkList)          
        #endregion/
           
            var toBeRemoveList = mallProductList.Where(
                             x => outPList.Select(
                                    plItem => plItem.MangoProduct.ProductId
                                    ).Cast<int>().ToList().Contains(x.ProductId.Int())
                             ).Select(y => y).ToList();

            foreach (var delItem in toBeRemoveList)
            {
                mallProductList.Remove(delItem);
            }

            foreach (var p in mallProductList)
            {
                CWmsProduct newMpItem = new CWmsProduct();
                newMpItem.MangoProduct.Copy(CopyFromProduct_ProductInfo_List(p));
                newMpItem.MangoProduct.ProductId = p.ProductId;
                newMpItem.ItemCode = p.ProductId.ToString();
                outPList.Add(newMpItem);
            }

            ret.Append("获取商品全部数据结束");
            ret.End();
            return outPList;
        }

        /// <summary>
        /// 获取单个商品数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// </remarks>

        public static List<CWmsProduct> GetMangoProduct(int ProductID)
        {
            var ret = new ThirdResult<List<object>>("获取单个商品数据开始");

        #region temp variables definition
            List<Product_ProductInfo_List> mallProductList = null;
            List<Product_ProductInfo_List_GuiGeList> ggLinkList = null;
            List<Product_ProductInfo_List_GuiGe> ggList = null;
            List<CWmsProduct> outPList = new List<CWmsProduct>(1);
        #endregion
            mallProductList = GetMangoMallProductEntity(ProductID); // get list of all product info
            var filters = new List<CommonFilterModel>();
            filters.Add(new CommonFilterModel("ProductId", "=", ProductID.ToString()));
            ggLinkList = GetGuiGeLinkList(filters); // get list of product_specification link
            ggList = GetGuiGeList(); // get list of specification

        #region tranverse ggLinkList
            foreach (var tmpGgItem in ggLinkList)
            {
                CWmsProduct newMpItem = new CWmsProduct();
                newMpItem.MangoProduct.Copy(CopyFromProduct_ProductInfo_List(mallProductList.Find(x => x.ProductId == tmpGgItem.ProductId)));
                newMpItem.ItemCode = tmpGgItem.ProductId + "-" + tmpGgItem.ProductGuiGeID;
                newMpItem.MangoProduct.GGDict.Id = tmpGgItem.ProductGuiGeID.ToString();
                newMpItem.MangoProduct.GGDict.GuiGeList.Clear();
                newMpItem.MangoProduct.GGDict.GuiGeList.AddRange(ggList.Where(x => (-1 < tmpGgItem.GuiGeIDList.IndexOf(x.GuiGeID.ToString()))).Select(y => new GuiGeProp(y.GuiGeID.ToString(), y.GuiGeName)).ToList());
                outPList.Add(newMpItem);
            } // foreach (var tmpGgItem in ggLinkList)
        #endregion/

            var toBeRemoveList = mallProductList.Where(
                             x => outPList.Select(
                                    plItem => plItem.MangoProduct.ProductId
                                    ).Cast<int>().ToList().Contains(x.ProductId.Int())
                             ).Select(y => y).ToList();
            foreach (var delItem in toBeRemoveList)
            {
                mallProductList.Remove(delItem);
            }

            foreach (var p in mallProductList)
            {
                CWmsProduct newMpItem = new CWmsProduct();
                newMpItem.MangoProduct.Copy(CopyFromProduct_ProductInfo_List(p));
                newMpItem.MangoProduct.ProductId = p.ProductId;
                newMpItem.ItemCode = p.ProductId.ToString();
                outPList.Add(newMpItem);
            }

            ret.Append("获取单个商品数据结束");
            ret.End();
            return outPList;
        }
#endif
#if false // 由MisModel处理
        /// <summary>
        /// 从商城中获取商品规格关联list
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/28 19:15
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        public static List<Product_ProductInfo_List_GuiGeList> GetGuiGeLinkList(List<CommonFilterModel> fliter)
        {
            var ret = new ThirdResult<List<object>>("取得商品规格关联表数据");

            var ggList = WCF<Product_ProductInfo_List_GuiGeList>.QueryAll(fliter);

            if (null == ggList || null == ggList.Data)
            {
                ret.Append("WCF运行未取得列表.判定为WCFAPI连接失败");
                ret.End();
                return new List<Product_ProductInfo_List_GuiGeList>(1);
            }
            else if (0 >= ggList.RetInt)
            {
                ret.Append("WCF运行取得的返回总数小于等于0,判定为WCF未取得数据");
                ret.End();
                return new List<Product_ProductInfo_List_GuiGeList>(1);
            }
            else
            {
                ret.Append(string.Format("WCF运行，成功获取数据，获取行数{0}", ggList.RetInt));
                ret.End();
                return ggList.Data;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/28 19:57
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        public static List<Product_ProductInfo_List_GuiGe> GetGuiGeList()
        {
            var ret = new ThirdResult<List<object>>("获取所有商品规格数据");

            var GuiGeDict = WCF<Product_ProductInfo_List_GuiGe>.QueryAll();
            if (null == GuiGeDict || null == GuiGeDict.Data)
            {
                ret.Append("WCF运行未取得列表.判定为WCFAPI连接失败");
                ret.End();
                return new List<Product_ProductInfo_List_GuiGe>(1);
            }
            else if (0 >= GuiGeDict.RetInt)
            {
                ret.Append("WCF运行取得的返回总数小于等于0,判定为WCF未取得数据");
                ret.End();
                return new List<Product_ProductInfo_List_GuiGe>(1);
            }
            else
            {
                ret.Append(string.Format("WCF运行，成功获取所有商品规格数据，获取行数{0}", GuiGeDict.RetInt));
                ret.End();
                return GuiGeDict.Data;
            }
        }
#endif
        /// <summary>
        /// 根据商品Id获取商城商品对象
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        static public MangoProduct GetProduct(string pId)
        {
            MangoProduct retObj = null;
            using (var tmp = MisModelFactory.GetMisEntity<Product_ProductInfo_List>(pId))
            {
                if (null != tmp)
                {
                    retObj = new MangoProduct();
                    retObj.CopyFrom(tmp);
                }
            }
            return retObj;
        }

        /// <summary>
        /// 从商城中获取商品信息list。若获取成功则返回获取的商品List[MangoProduct]；否则返回Count=0的列表。
        /// public static List[MangoProduct] GetMangoMallProductList(List[CommonFilterModel], out string))
        /// </summary>
        /// <param name="pFilters"></param>
        /// <param name="pMsg"></param>
        /// <returns>若获取成功则返回获取的商品List[MangoProduct]；否则返回Count=0的列表。</returns>
        public static List<MangoProduct> GetV_Product(List<CommonFilterModel> pFilters, out string pMsg)
        {
            var orders = new List<CommonOrderModel>();
            DefaultResult<List<Product_ProductInfo_List>> wcfRslt = null;

            if (null == pFilters)
                wcfRslt = WCF<Product_ProductInfo_List>.QueryAll();
            else
                wcfRslt = WCF<Product_ProductInfo_List>.QueryAll(pFilters);

            if (null == wcfRslt || wcfRslt.Data == null)
            {
                pMsg = "WCF运行未取得列表.判定为WCFAPI连接失败";
                return new List<MangoProduct>(1);
            }
            else if (wcfRslt.RetInt <= 0)
            {
                pMsg = "WCF运行取得的返回总数小于等于0,判定为WCF未取得数据";
                return new List<MangoProduct>(1);
            }
            else
            {
                var outList = wcfRslt.Data.Select(x => new MangoProduct
                {
                    #region Copy action
                    CanLingYong = x.CanLingYong,
                    AddUserid = x.AddUserid,
                    AddTime = x.AddTime,
                    IsDel = x.IsDel,
                    UpdateTime = x.UpdateTime,
                    UpdateUserID = x.UpdateUserID,
                    KuCunCount = x.KuCunCount,
                    TotalCount = x.TotalCount,
                    SerialId = x.SerialId,
                    isParent = x.isParent,
                    isPeiSongTime = x.isPeiSongTime,
                    WuPinMoney = x.WuPinMoney,
                    ZhiBaotime = x.ZhiBaotime,
                    isSupplierPeiSong = x.isSupplierPeiSong,
                    IsSale = x.IsSale,
                    isTanXiao = x.isTanXiao,
                    isPoint = x.isPoint,
                    xianZhiOrgs = x.xianZhiOrgs,
                    XianZhiType = x.XianZhiType,
                    CaiGoPrice = x.CaiGoPrice,
                    YWY_Dingdan_type_2L = x.YWY_Dingdan_type_2L,
                    YWY_Dingdan_type_3L = x.YWY_Dingdan_type_3L,
                    DisOrder = x.DisOrder,
                    LuRuState = x.LuRuState,
                    JDProudctID = x.JDProudctID,
                    OrgID = x.OrgID,
                    ProductId = x.ProductId,
                    Title = x.Title,
                    BianMa = x.BianMa,
                    ProductTypeId = x.ProductTypeId,
                    ProductCategoryIdBig = x.ProductCategoryIdBig,
                    ProductLevel = x.ProductLevel,
                    ProductCategoryId = x.ProductCategoryId,
                    Brands = x.Brands,
                    Model = x.Model,
                    isJD = x.isJD,
                    MiniInventory = x.MiniInventory,
                    InquiryCycle = x.InquiryCycle,
                    ProductImage = x.ProductImage,
                    Remark = x.Remark,
                    Unit = x.Unit,
                    GuiGe = x.GuiGe,
                    PriceLow = x.PriceLow,
                    PriceMax = x.PriceMax,
                    PriceAve = x.PriceAve,
                    DepreciationRate = x.DepreciationRate,
                    ResidualRate = x.ResidualRate,
                    MaxInventory = x.MaxInventory
                    #endregion
                }).ToList();
                pMsg = string.Empty;
                //ret.Append("从商城中获取商品list[{1}].Count={0}", outList.Count, outList);
                return outList;
            }
        }
#if false // 由MisModel处理
        /// <summary>
        /// 从商城中获取商品信息list。
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/28 19:09
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        public static List<Product_ProductInfo_List> GetMangoMallProductList()
        {
            var ret = new ThirdResult<List<object>>("取得商品数据");
            
            var orders = new List<CommonOrderModel>();
            var filters = new List<CommonFilterModel>();
            filters.Add(new CommonFilterModel("isJD", "!=", "1"));
            filters.Add(new CommonFilterModel("isSupplierPeiSong", "=", "2"));

            var productinfolistwcfret = WCF<Product_ProductInfo_List>.QueryAll(filters);
            if (productinfolistwcfret.Data == null)
            {
                ret.Append("WCF运行未取得列表.判定为WCFAPI连接失败");
                ret.End();
                return null;
            }
            else if (productinfolistwcfret.RetInt <= 0)
            {
                ret.Append("WCF运行取得的返回总数小于等于0,判定为WCF未取得数据");
                ret.End();
                return null;
            }
            else
            {
                ret.Append(string.Format("WCF运行，成功获取取得商品数据，获取行数{0}", productinfolistwcfret.RetInt));
                ret.End();
                return productinfolistwcfret.Data;
            }
        }

        /// <summary>
        /// 从商城中获取单条商品数据。
        /// </summary>
        /// <returns></returns>
        public static List<Product_ProductInfo_List> GetMangoMallProductEntity(int ProductID)
        {
            var ret = new ThirdResult<List<object>>("取得商品数据");

            var orders = new List<CommonOrderModel>();
            var filters = new List<CommonFilterModel>();
            filters.Add(new CommonFilterModel("ProductId","=", ProductID.ToString()));

            var productinfolistwcfret = WCF<Product_ProductInfo_List>.QueryAll(filters);
            if (productinfolistwcfret.Data == null)
            {
                ret.Append("WCF运行未取得列表.判定为WCFAPI连接失败");
                ret.End();
                return null;
            }
            else if (productinfolistwcfret.RetInt <= 0)
            {
                ret.Append("WCF运行取得的返回总数小于等于0,判定为WCF未取得数据");
                ret.End();
                return null;
            }
            else
            {
                ret.Append(string.Format("WCF运行，成功获取取得商品数据，获取行数{0}", productinfolistwcfret.RetInt));
                ret.End();
                return productinfolistwcfret.Data.ToList();
            }
        }
#endif
#if false // 由MisModel处理

        /// <summary>
        /// </summary>
        /// <param name="srcObj">The source object.</param>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/28 19:03
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        static public MangoProduct CopyFromProduct_ProductInfo_List(Product_ProductInfo_List srcObj)
        {
            MangoProduct p = new MangoProduct();
            if (null != srcObj)
            {
                p.CopyFrom(srcObj);
            }
            return p;
        }

        /// <summary>
        /// </summary>
        /// <param name="srcObj">拷贝的数据源，是MangoProduct的实例</param>
        /// <param name="mapCalssID">Dict=709</param>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/28 19:03
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        static public Product_WMS_Interface CopyFromMangoProduct(MangoProduct srcObj, int mapCalssID)
        {
           
            Product_WMS_Interface p = new Product_WMS_Interface();
            if (null != srcObj)
            {
                p.MapId1 = srcObj.ProductId;
                p.MapId2 = srcObj.GGDict.Id.Int();
                p.MapCalssID = mapCalssID;
            }
            
            return p;
        }

        /// <summary>
        /// </summary>
        /// <param name="srcObj">The source object.</param>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/6/6 15:27
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        static public Product_WMS_Interface CopyFromMangoProduct(MangoProduct srcObj)
        {

            Product_WMS_Interface p = new Product_WMS_Interface();
            if (null != srcObj)
            {
                p.MapId1 = srcObj.ProductId;
                p.MapId2 = srcObj.GGDict.Id.Int();
            }

            return p;
        }
#endif
        #region 生成或获取仓库
        /// <summary>
        /// 从芒果商城中获取指定仓库
        /// </summary>
        /// <param name="id">芒果商城中的仓库id</param>
        /// <returns>仓库实例</returns>
        static public MangoWarehouse GetWarehouse(string id)
        {
            MangoWarehouse w = null;
            var rslt = MangoWCF<Product_Warehouse>.GetList(new List<CommonFilterModel>() { new CommonFilterModel("WarehouseId", "=", id) }, new List<CommonOrderModel>());
            if (null != rslt && null != rslt.Data && 0 < rslt.Data.Count)
            {
                w = new MangoWarehouse();
                w.CompanyTypeId = rslt.Data[0].CompanyTypeId;
                w.WarehouseId = rslt.Data[0].WarehouseId;
                w.WarehouseName = rslt.Data[0].WarehouseName;
            }
            else
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("根据仓库Id({0})获取实体失败, WCF运行结果: WCFResult={1}, WCFResult.Data={2}", id, rslt, rslt?.Data);
            }

            return w;
        }

        /// <summary>
        /// 从芒果商城中获取所有仓库
        /// </summary>     
        static public List<MangoWarehouse> GetWarehouses()
        {
            var ret = new ThirdResult<List<object>>("从芒果商城中获取所有仓库");

            // declare temp variables
            List<MangoWarehouse> outList = new List<MangoWarehouse>(1);
            List<Product_Warehouse> wList = new List<Product_Warehouse>(3);

            try
            {
                // get all warehouses from mall.
                wList = WCF<Product_Warehouse>.ColumnsAll(
                    new List<CommonFilterModel>() {
                        new CommonFilterModel("CompanyTypeId", "in", new List<object>(){9,  1})
                    }, new List<string>() { "WarehouseId", "WarehouseName","CompanyTypeId"
                        }
                    ).Data;

                // copy warehouses to outList
                foreach (Product_Warehouse w in wList)
                {
                    MangoWarehouse mw = new MangoWarehouse();
                    mw.WarehouseId = w.WarehouseId;
                    mw.WarehouseName = w.WarehouseName;
                    mw.CompanyTypeId = w.CompanyTypeId;
                    outList.Add(mw);
                }
            }
            catch (Exception ex)
            {
                outList.Clear();
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            ret.Append(string.Format("从芒果商城中获取所有仓库，数量{0}", outList.Count));
            ret.End();
            return outList;
        }
        #endregion

        #region 生成或获取货主
        ///// <summary>
        ///// TODO: 根据仓库获取对应的所有货主
        ///// </summary>
        ///// <param name="pList"></param>
        ///// <returns></returns>
        //static public Dictionary<string, WmsOwner> GetV_Owners(List<MangoWarehouse> pList)
        //{
        //    Dictionary<string, WmsOwner> outList = new Dictionary<string, WmsOwner>(1);
        //    if (null == pList)
        //        return outList;

        //    try
        //    {
        //        foreach (var w in pList)
        //        {
        //            var o = GetOwner(w.WarehouseId.ToString());
        //            if (null != o && !outList.Keys.Contains(o.WmsID))
        //                outList.Add(o.WmsID, o);
        //        } //foreach (var w in pList)
        //    }
        //    catch (Exception ex)
        //    {
        //        outList.Clear();
        //        C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "MangoFactory.GetV_Owners()发生异常");
        //    }

        //    return outList;
        //}

#if false // 由WMS模块处理
        /// <summary>
        /// 根据仓库Id获取货主
        /// </summary>
        /// <param name="pWid">仓库编码</param>
        /// <returns>返回仓库实体。若操作失败则返回null。</returns>
        static public WmsOwner GetOwner(string pWid)
        {
            WmsOwner o = null;
            var w = GetWarehouse(pWid);

            if (null == w)
            {
                var ret = new ThirdResult<List<object>>(string.Format("根据仓库Id获取o货主"));
                ret.Append(string.Format("根据仓库Id[{0}]获取货主失败，没有找到仓库", pWid));
                ret.End();
                return o;
            }

            switch (w.CompanyTypeId)
            {
        #region 网络仓库
                case 1: // TODO: 用const代替
                    {
                        o = new WmsOwner(CWmsConsts.cStrWmsOwnerCode_MGWL, CWmsConsts.cStrWmsOwnerName_MGWL);
                        break;
                    }
        #endregion
        #region 蓝江仓库
                case 9: // TODO: 用const代替
                    {
                        o = new WmsOwner(CWmsConsts.cStrWmsOwnerCode_LJZJ, CWmsConsts.cStrWmsOwnerName_LJZJ);
                        break;
                    }
        #endregion
                default: break;
            } // switch (w.WarehouseId.ToString())
            return o;
        }
#endif
#if false //  不该有默认货主
        /// <summary>
        /// 根据仓库名称获取货主
        /// </summary>
        /// <returns>返回WmsOwner实例，货主为默认的‘芒果集团’</returns>
        static public WmsOwner GetDefaultOwner()
        {
            WmsOwner owner = new WmsOwner();
            owner.WmsID = CWmsConsts.cStrDefaultOwnerId;
            owner.name = CWmsConsts.cStrDefaultOwnerName;
            return owner;
        }
#endif
        #endregion
    }
    
    /// <summary>
    /// 商品分类缓存
    /// </summary>
    public class Simple_ProductCategory_Cache
    {
        private static IStoreCache<Product_ProductCategory> _ProductCategory_Cache_Store;

        /// <summary>
        /// 取得数据
        /// </summary>
        public static IStoreCache<Product_ProductCategory> ProductCategory_Cache_Store => _ProductCategory_Cache_Store != null ? _ProductCategory_Cache_Store : (_ProductCategory_Cache_Store =
            SimpleStore.Memory("ProductCategoryId", () =>
            {
                var wcfProductCategoryRet = WCF<Product_ProductCategory>.QueryAll(
                    new List<CommonFilterModel>(){
                        new CommonFilterModel("isdel", "=", "0")
                    }
                    , 400);
                return wcfProductCategoryRet.Data;
            }, "ProductCategory_Cache", "商品分类缓存"));
    }
}
