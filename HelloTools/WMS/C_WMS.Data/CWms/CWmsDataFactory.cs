using System;
using System.Collections.Generic;
using MangoMis.Frame.ThirdFrame;
using C_WMS.Data.CWms.CWmsEntity;
using C_WMS.Data.CWms.Interfaces.Data;
using C_WMS.Data.Mango.Data;
using C_WMS.Data.Mango;
using C_WMS.Interface.Utility;
using MangoMis.Frame.Helper;
using TT.Common.Frame.Model;
using System.Linq;
using MisModel;
using MangoMis.Frame.DataSource.Simple;
using C_WMS.Data.Mango.MisModelPWI;
using System.Reflection;
using C_WMS.Data.Wms.Data;

namespace C_WMS.Data.CWms
{
    /// <summary>
    /// 数据工厂类
    /// </summary>
    class CWmsDataFactory
    {
#if C_WMS_V2
        static public int GetV_SpecificationLink(out Dictionary<string, string> pGgLinks)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// new an object of CWmsProduct by itemCode.
        /// return the new object if success -or- return null if failed.
        /// </summary>
        /// <param name="itemCode">itemCode of product, in a format of [productId]-[specLinkId]</param>
        /// <returns>return the new object if success -or- return null if failed.</returns>
        static public CWmsProduct NewProduct(string itemCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据过滤器获取商品，并返回成功获取的商品数量。若操作失败则返回TError.WCFRunnError
        /// static public int GetProductList(List[CommonFilterModel], ref List[CWmsProduct], out string pMsg)
        /// </summary>
        /// <param name="pList">返回成功获取的商品List，若操作失败则返回Count为0的列表实体</param>
        /// <param name="pFilters">过滤条件</param>
        /// <param name="pMsg">返回错我描述</param>
        /// <returns>返回成功获取的商品数量。若操作失败则返回TError.WCFRunnError</returns>
        static public int GetV_Product(List<CommonFilterModel> pFilters, ref List<CWmsProduct> pList, out string pMsg)
        {
            int err = TError.WCF_RunError.Int();

            try
            {
                if (null == pList) pList = new List<CWmsProduct>(10);
                else pList.Clear();
                List<MangoProduct> mangoList = null;
                if (null != (mangoList = MangoFactory.GetV_Product(pFilters, out pMsg)) && 0 < mangoList.Count)
                {
                    pList.AddRange(mangoList.Select(x => new CWmsProduct(x)).ToList());
                    err = pList.Count;
                }
                else
                {
                    err = TError.Pro_HaveNoData.Int();
                    pMsg = string.Format("CWmsDataFactory.GetV_Product(), 查询返回空(mangoList={0})或商品数量为0, err={1}, \r\nFilterDebug={2}", mangoList, err, Utility.CWmsDataUtility.GetDebugInfo_MisFilter(pFilters));
                    C_WMS.Data.Utility.MyLog.Instance.Warning(pMsg);
                }
            }
            catch (Exception ex)
            {
                if (null != pList) pList.Clear();
                pMsg = ex.Message;
                err = TError.WCF_RunError.Int();
                Data.Utility.MyLog.Instance.Warning(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            return err;
        }

        static public List<string> GetV_ProductIds(List<string> pIdList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取默认货主
        /// </summary>
        /// <returns></returns>
        static public CWmsOwner GetDefaultOwner()
        {
            var cacheOwner = CWmsMisSystemParamCache.Cache.GetDefaultOwner();
            if (null != cacheOwner)
            {
                CWmsOwner o = new CWmsOwner(cacheOwner.Code, cacheOwner.Name);
                return o;
            }
            else
            {
                return null;
            }
        }

        static public CWmsOwner GetOwner(CWmsWarehouse warehouse)
        {
            var owner = CWmsMisSystemParamCache.Cache.GetV_Owners()?.First(cache => cache.Warehouses.Select(w => w.Code).Contains(warehouse.Mango.WarehouseId.Int().ToString()));
            return new CWmsOwner(owner.Code, owner.Name);
        }

        static public IEnumerable<CWmsOwner> GetV_Owners()
        {
            return CWmsMisSystemParamCache.Cache.GetV_Owners()?.Select(cache => new CWmsOwner(cache.Code, cache.Name));
        }

        static public IEnumerable<CWmsWarehouse> GetV_Warehouse()
        {
            return CWmsMisSystemParamCache.Cache.GetV_Warehouses()?.Select(cache => new CWmsWarehouse(cache.Code, cache.Name) );
        }

        static public IEnumerable<CWmsWarehouse> GetV_Warehouse(string pOwnerCode)
        {
            throw new NotImplementedException();
        }

        static public IEnumerable<CWmsWarehouse> GetV_Warehouse(CWmsOwner pOwnerCode)
        {
            throw new NotImplementedException();
        }

        static public int GetV_Order<TOrderType, TMangoOrderType, THandler>(List<CommonFilterModel> pFilters, out IEnumerable<TOrderType> pOutList, out string pErrMsg) where TOrderType: class, new()
        {
            int err = TError.WCF_RunError.Int();
            List<TMangoOrderType> orderList = null;

            try
            {
                if (0 >= (err = MangoFactory.GetV_Order(pFilters, out orderList, out pErrMsg)) || null == orderList)
                {
                    pOutList = null;
                    pErrMsg = string.Format("CWmsDataFactory.GetV_Order<{0}, {1}, {2}>列表失败, err={3}， orderList={4}, errMsg={5}", typeof(TOrderType), typeof(TMangoOrderType), typeof(THandler), err, orderList, pErrMsg);
                    C_WMS.Data.Utility.MyLog.Instance.Warning(pErrMsg);
                    return err;
                }
                else
                {
                    MethodInfo methodInfo = typeof(THandler).GetMethod("NewOrder", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
                    pOutList = orderList.Select(order => methodInfo.Invoke(null, new object[] { (order as IMangoOrderBase)?.GetId() }) as TOrderType);
                    err = Enumerable.Count(pOutList);
                    pErrMsg = (0 < err) ? string.Empty : string.Format("");
                    return err;
                }
            }
            catch (Exception ex)
            {
                if (null != orderList) orderList.Clear();
                pOutList = null;
                pErrMsg = string.Format("CWmsDataFactory.GetV_Order<{0}, {1}, {2}>发生异常，FilterDebug={3}", typeof(TOrderType), typeof(TMangoOrderType), typeof(THandler), Utility.CWmsDataUtility.GetDebugInfo_MisFilter(pFilters));
                Data.Utility.MyLog.Instance.Error(ex, pErrMsg);
                return TError.Post_NoChange.Int();
            }
        }
#endif

#if false
        /// <summary>
        /// 根据过滤器获取满足条件的全部入库订单。若执行成功则返回获取的行数，否则返回小于或等于0
        /// static public int GetVEntryOrders(List[CommonFilterModel], out List[CWmsEntryOrder], out string)
        /// </summary>
        /// <param name="pFilters">过滤器</param>
        /// <param name="pOutList">返回满足条件的全部入库订单，若执行失败则返回数量为0的列表实体</param>
        /// <param name="pErrMsg"></param>
        /// <returns></returns>
        static public int GetV_EntryOrders(List<CommonFilterModel> pFilters, out IEnumerable<CWmsEntryOrder> pOutList, out string pErrMsg)
        {
            int err = TError.WCF_RunError.Int();
            List<MangoEntryOrder> orderList = null;

            try
            {
                if (0 >= (err = MangoFactory.GetVMangoEntryOrders(pFilters, out orderList, out pErrMsg)) || null == orderList)
                {
                    pOutList = null;
                    pErrMsg = string.Format("根据过滤器获取入库订单失败, err={0}， orderList={2}, errMsg={1}", err, pErrMsg, orderList);
                    C_WMS.Data.Utility.MyLog.Instance.Warning(pErrMsg);
                    return err;
                }
                else
                {
                    foreach (MangoEntryOrder smo in orderList)
                    {
#region 获取子入库单
                        List<MangoSubEntryOrder> subList = null;
                        errMsg = MangoFactory.GetVMangoSubEntryOrders(smo?.ProductInputMainId.ToString(), out subList);
                        if (null == subList || 0 == subList.Count)
                        {
                            C_WMS.Data.Utility.MyLog.Instance.Error("在{0}中，获取主入库单({1})的子入库单失败, err={2}, errMsg={3}", System.Reflection.MethodBase.GetCurrentMethod().Name, smo?.ProductInputMainId, err, errMsg);
                            continue;
                        }
#endregion
#region 处理子入库单
                        CWmsEntryOrder ceo = new CWmsEntryOrder();
                        foreach (var mso in subList)
                        {
                            if (null == mso)
                            {
                                C_WMS.Data.Utility.MyLog.Instance.Error("在{0}中，主入库单({1})中有空引用的子入库订单实体", System.Reflection.MethodBase.GetCurrentMethod().Name, smo?.ProductInputMainId);
                                ceo.SubOrders.Clear();
                                break;
                            }
                            CWmsSubEntryOder cso = new CWmsSubEntryOder();
                            (cso.MangoOrder as MangoSubEntryOrder).CopyFrom(mso);
                            ceo.SubOrders.Add(mso.ProductInputId.ToString(), cso);
                        }
#endregion

                        if (0 < ceo.SubOrders.Count)
                        {
                            (ceo.MangoOrder as MangoEntryOrder).CopyFrom(smo);
                            pOutList.Add(ceo);
                        }
                        else
                        {
                            C_WMS.Data.Utility.MyLog.Instance.Error("在{0}中，主入库单({1})没有获取到任何子入库订单实体", System.Reflection.MethodBase.GetCurrentMethod().Name, smo?.ProductInputMainId);
                        }
                    } // foreach (MangoEntryOrder smo in orderList)
                    err = Enumerable.Count(pOutList);
                    pErrMsg = (0 < err) ? string.Empty : string.Format("");
                    return err;
                }
            }
            catch(Exception ex)
            {
                pOutList = null;
                pErrMsg = ex.Message;
                Data.Utility.MyLog.Instance.Error(ex, "根据过滤条件获取主入库订单发生异常");
                return TError.Post_NoChange.Int();
            }
        }

        /// <summary>
        /// 根据主入库订单ID获取主入库订单及其所有子订单的实例
        /// </summary>
        /// <param name="pOrderId">芒果商城主入库单ID</param>
        /// <returns>CWmsEntryOrder对象</returns>
        static public CWmsEntryOrder GetCWmsEntryOrder(string pOrderId)
        {
            try
            {
                CWmsEntryOrder outObj = null;
                List<MangoSubEntryOrder> tmpList = null;
                string errMsg = string.Empty;

                // get mango order
                MangoEntryOrder mangoOrder = MangoFactory.GetMangoOrder(TCWmsOrderCategory.EEntryOrder, pOrderId) as MangoEntryOrder;

                // get sub orders
                errMsg = MangoFactory.GetVMangoSubEntryOrders(pOrderId, out tmpList);

                // create CWmsEntryOrder if successful
                if (null != mangoOrder && string.IsNullOrEmpty(errMsg) && 0 < tmpList?.Count)
                {
                    outObj = new CWmsEntryOrder();
                    (outObj.MangoOrder as MangoEntryOrder).CopyFrom(mangoOrder);
                    outObj.WmsOrder.SetWmsOrderType(TWmsOrderType.CGRK);// 默认采购入库
                    foreach (var s in tmpList)
                    {
                        int tmpPlanQty = -1;
                        CWmsSubEntryOder cso = new CWmsSubEntryOder();
                        (cso.MangoOrder as MangoSubEntryOrder).CopyFrom(s);

#region // 获取应收数量
                        if (0 >= s.ProductBuyId.Int())
                        {
                            // 无采购入库Id，判断为芒果网络商品入库（补），实收数量即为应收数量
                            (cso.WmsOrderDetail as Wms.Data.WmsEntryOrderDetail).planQty = CWmsUtility.Decimal2Int(s.ProductInputCount.Decimal());
                        }
                        else
                        {
                            // 有采购入库Id，取采购入库单中的应收数量
                            (cso.WmsOrderDetail as Wms.Data.WmsEntryOrderDetail).planQty =
                                (TError.RunGood.Int() == MangoFactory.GetPlanQuantityByPid(s.ProductBuyId.ToString(), out tmpPlanQty)) ? tmpPlanQty : -1;
                        }
#endregion

                        // 设置商品Id
                        cso.Product.ItemCode = (string.IsNullOrEmpty(s.ProductGuiGeID.ToString()) || "0".Equals(s.ProductGuiGeID.ToString())) ?
                            s.ProductId.ToString() : string.Format("{0}-{1}", s.ProductId, s.ProductGuiGeID);

                        outObj.SubOrders.Add(s.ProductInputId.ToString(), cso);
                    }
                    // TODO: other operations
                }
                else
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("根据主入库订单ID({0})获取主入库订单及其所有子订单的实例失败, mangoOrder=(1}, 子单据数量={2}", pOrderId, mangoOrder, tmpList?.Count);
                }
                
                return outObj;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "根据主入库订单ID({0})获取主入库订单及其所有子订单的实例异常", pOrderId);
                return null;
            }
        }
#endif

#if false // 由WMS模块处理
        /// <summary>
        /// 根据芒果商城的入库订单生成入库订单接口的HTTP request XML实例。
        /// </summary>
        /// <param name="pEntryOrderId">芒果商城主入库订单ID</param>
        /// <param name="pPurchaseOrderId">芒果商城主采购订单ID</param>
        /// <returns>若成功则返回被创建的实例；否则返回null</returns>
        static public HttpReqXmlBase GetReqXmlBody_EntryOrderCreate(string pEntryOrderId, string pPurchaseOrderId)
        {
            HttpReqXml_EntryOrderCreate reqObj = null;
            List<HttpReqXml_EntryOrderCreate_Order> reqSubOrderList = new List<HttpReqXml_EntryOrderCreate_Order>(1);

#region validate arguments
            if (string.IsNullOrEmpty(pEntryOrderId))
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("根据芒果商城的入库订单[{0}]生成入库订单接口的HTTP request XML实例结束，非法入参", pEntryOrderId);
                return reqObj;
            }
#endregion

            // get main entry order from ‘主入库单表’
            CWmsEntryOrder cwmsOrder = GetCWmsEntryOrder(pEntryOrderId);
            if (null == cwmsOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("根据入库订单[{0}]获取主入库订单实体失败.", pEntryOrderId);
                return reqObj;
            }

            // TODO: 暂不处理采购订单，目前我们的一个入库单可能对应多个主采购单，但在仓储系统里一个入库单对应一个采购单（采购单编号可以传入空值）

            // sub entry order list
            reqObj = new HttpReqXml_EntryOrderCreate();
            reqObj.entryOrder.CopyFrom(cwmsOrder);
            foreach (var cwmsSubOrder in cwmsOrder.SubOrders)
            {
                HttpReqXml_EntryOrderCreate_Order orderLine = new HttpReqXml_EntryOrderCreate_Order();
                orderLine.CopyFrom(cwmsSubOrder.Value as CWmsSubEntryOder);
                reqSubOrderList.Add(orderLine);
            }

            reqObj.orders = reqSubOrderList.ToArray();
            
            return reqObj;
        }
#endif

#if false // 使用static public int GetV_Order
        /// <summary>
        /// 获取所有符合条件的主出库订单，若执行成功则返回获取的行数，否则返回TError.WCF_RunError和Count=0的主出库订单列表
        /// </summary>
        /// <param name="pFilters"></param>
        /// <param name="pOutList">返回所有符合条件的主出库订单</param>
        /// <param name="pErrMsg"></param>
        /// <returns></returns>
        static public int GetVStockoutOrders(List<CommonFilterModel> pFilters, out List<CWmsExWarehouseOrder> pOutList, out string pErrMsg)
        {
            int err = TError.WCF_RunError.Int();
            string errMsg = string.Empty;
            pOutList = new List<CWmsExWarehouseOrder>(1);
            List<MangoExwarehouseOrder> orderList = null;

            try
            {
                if (0 >= (err = MangoFactory.GetVMangoStockoutOrders(pFilters, out orderList, out pErrMsg)) || null == orderList)
                {
                    pErrMsg = string.Format("根据过滤器获取满足条件的全部出库订单失败, err={0}， orderList={2}, errMsg={1}", err, pErrMsg, orderList);
                    Data.Utility.MyLog.Instance.Error(pErrMsg);
                    return err;
                }
                else
                {
                    foreach (MangoExwarehouseOrder smo in orderList)
                    {
#region 获取子出库单
                        List<MangoSubExwarehouseOrder> subList = null;
                        errMsg = MangoFactory.GetMangoExwarehouseSubOrderList(smo.ProductOutputMainId.ToString(), out subList);
                        if (null == subList || 0 == subList.Count)
                        {
                            C_WMS.Data.Utility.MyLog.Instance.Error("在{0}中，获取主出库单({1})的子出库单失败, err={2}, errMsg={3}", System.Reflection.MethodBase.GetCurrentMethod().Name, smo?.ProductOutputMainId, err, errMsg);
                            continue;
                        }
#endregion
#region 处理子出库单
                        CWmsExWarehouseOrder ceo = new CWmsExWarehouseOrder();
                        foreach (var mso in subList)
                        {
                            if (null == mso)
                            {
                                C_WMS.Data.Utility.MyLog.Instance.Error("在{0}中，主出库单({1})中有空引用的子出库订单实体"
                                    , System.Reflection.MethodBase.GetCurrentMethod().Name, smo.ProductOutputMainId);
                                ceo.SubOrders.Clear();
                                break;
                            }
                            CWmsExWarehouseSubOrder cso = new CWmsExWarehouseSubOrder();
                            (cso.MangoOrder as MangoSubExwarehouseOrder).CopyFrom(mso);
                            ceo.SubOrders.Add(mso.ProductOutputId.ToString(), cso);
                        }
#endregion
                        if (0 < ceo.SubOrders.Count)
                        {
                            (ceo.MangoOrder as MangoExwarehouseOrder).CopyFrom(smo);
                            pOutList.Add(ceo);
                        }
                        else
                        {
                            C_WMS.Data.Utility.MyLog.Instance.Error("在{0}中，主出库单({1})没有获取到任何子出库订单实体"
                                    , System.Reflection.MethodBase.GetCurrentMethod().Name, smo.ProductOutputMainId);
                        }
                    } // foreach (MangoExwarehouseOrder smo in orderList)
                    pErrMsg = string.Empty;
                    return err;
                }
            }
            catch (Exception ex)
            {
                pErrMsg = ex.Message;
                pOutList.Clear();
                Data.Utility.MyLog.Instance.Error(ex, "根据过滤条件获取主出库订单发生异常");
                return TError.Post_NoChange.Int();
            }
        }
#endif
#if false // 使用Handler.NewOrder
        /// <summary>
        /// 根据主订单Id获取芒果商城的订单实体及其子订单明细的实体
        /// </summary>
        /// <param name="pOid">主订单Id</param>
        /// <returns>若成功则返回CWmsMallOrder实体；否则返回null</returns>
        static public CWmsMallOrder GetCWmsMallOrder(string pOid)
        {
            try
            {
                string errMsg = string.Empty;
                CWmsMallOrder outObj = null;

                // get mango order, TODO:　传入了错误的TCWmsOrderCategory类型? 固定传入了出库单类型
                MangoMallOrder mangoOrder = MangoFactory.GetMangoOrder(TCWmsOrderCategory.EExwarehouseOrder, pOid) as MangoMallOrder;

                if (null == mangoOrder)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("在{0}中，根据单据ID[{0}]获取商城主订单实体及其子订单明细失败", System.Reflection.MethodBase.GetCurrentMethod().Name, pOid);
                    return outObj;
                }

                // 创建CWmsExWarehouseOrder实例及其子出库订单实例列表
                outObj = new CWmsMallOrder();
                (outObj.MangoOrder as MangoMallOrder).CopyFrom(mangoOrder);
                
                outObj.Owner.CopyFrom(new Wms.Data.WmsOwner(Mango.MisModelPWI.CWmsMisSystemParamCache.Cache.GetDefaultOwner().Code, Mango.MisModelPWI.CWmsMisSystemParamCache.Cache.GetDefaultOwner().Name));   // 对于商城下的订单，货主一定是蓝江智家
                outObj.CancelReason = (outObj.MangoOrder as MangoMallOrder).BeiZhu;
                outObj.WmsOrder.SetWmsStockoutOrderType((TMangoOrderType)(outObj.MangoOrder as MangoMallOrder).DingDanType.Int());
                // TODO: 目前阶段不需要处理子订单
                return outObj;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中，根据单据ID[{0}]获取商城主订单实体及其子订单明细发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name, pOid);
                return null;
            }
        }

        /// <summary>
        /// 根据主出库订单ID获取主出库订单及其所有子订单的实例
        /// </summary>
        /// <param name="pOrderId">芒果商城主出库订单ID</param>
        /// <returns>CWmsExWarehouseOrder</returns>
        static public CWmsExWarehouseOrder GetCWmsStockoutOrder(string pOrderId)
        {
            var ret = new ThirdResult<List<object>>(string.Format("根据主出库订单[{0}]获取主出库订单及其所有子订单的实例,开始", pOrderId));

            try
            {
                string errMsg = string.Empty;
                CWmsExWarehouseOrder outObj = null;
                List<MangoSubExwarehouseOrder> tmpList = null;

                // get mango order
                MangoExwarehouseOrder mangoOrder = MangoFactory.GetMangoOrder(TCWmsOrderCategory.EExwarehouseOrder, pOrderId) as MangoExwarehouseOrder;
                // create CWmsEntryOrder if successful
                if (null == mangoOrder)
                {
                    ret.Append(string.Format("根据主出库订单[{0}]获取主出库订单及其所有子订单的实例结束, 获取主出库订单失败", pOrderId));
                    ret.End();
                    return outObj;
                }

                // 获取子出库订单列表
                errMsg = MangoFactory.GetMangoExwarehouseSubOrderList(pOrderId, out tmpList);
                if (!string.IsNullOrEmpty(errMsg) || null == tmpList || 0 >= tmpList.Count)
                {
                    ret.Append(string.Format("根据主出库订单[{0}]获取主出库订单及其所有子订单的实例结束, 获取主出库订单失败。errMsg: {1}", pOrderId, errMsg));
                    ret.End();
                    return outObj;
                }

                // 创建CWmsExWarehouseOrder实例及其子出库订单实例列表
                outObj = new CWmsExWarehouseOrder();
                (outObj.MangoOrder as MangoExwarehouseOrder).CopyFrom(mangoOrder);

                foreach (var t in tmpList)
                {
                    CWmsExWarehouseSubOrder subOrder = new CWmsExWarehouseSubOrder();
                    (subOrder.MangoOrder as MangoSubExwarehouseOrder).CopyFrom(t);
                    subOrder.Product.MangoProduct.ProductId = t.ProductId;
                    (subOrder.WmsOrderDetail).Owner.CopyFrom(MangoFactory.GetOwner((subOrder.MangoOrder as MangoSubExwarehouseOrder).WarehouseId.ToString())); // 获取货主
                                                                                                                                                               // 设置商品Id
                    subOrder.Product.ItemCode = (string.IsNullOrEmpty(t.ProductGuiGeID.ToString()) || "0".Equals(t.ProductGuiGeID.ToString())) ?
                        t.ProductId.ToString() : string.Format("{0}-{1}", t.ProductId, t.ProductGuiGeID);

                    outObj.SubOrders.Add((subOrder.MangoOrder as MangoSubExwarehouseOrder).ProductOutputId.ToString(), subOrder);
                }

                ret.Append(string.Format("根据主出库订单[{0}]获取主出库订单及其所有子订单的实例结束, 返回{1}", pOrderId, outObj));
                ret.End();
                return outObj;
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return null;
            }
        }
#endif
#if false // 由WMS模块处理
        /// <summary>
        /// 根据芒果商城的出库订单生成出库订单接口的HTTP request XML实例。
        /// </summary>
        /// <param name="pEntryOrderId">芒果商城主出库订单ID</param>
        /// <returns>若成功则返回被创建的实例；否则返回null</returns>
        static public HttpReqXmlBase GetReqXmlBody_ExWarehouseCreate(string pEntryOrderId)
        {
            //
            HttpReqXml_StockoutOrderCreate reqObj = null;
            List<HttpReqXml_StockoutOrderCreate_OrderLine> reqSubOrderList = null;

#region validate parameters
            if (string.IsNullOrEmpty(pEntryOrderId))
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("根据主出库订单[{0}]获取主出库订单及其所有子订单的实例结束，非法入参", pEntryOrderId);
                return reqObj;
            }
#endregion

            // get main order from ‘主出库单表’
            CWmsExWarehouseOrder cwmsOrder = GetCWmsStockoutOrder(pEntryOrderId);
            if (null == cwmsOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("根据主出库订单[{0}]获取主出库订单及其所有子订单的实例结束，获取主出库订单失败", pEntryOrderId);
                return reqObj;
            }

            // sub 出库单 list
            reqObj = new HttpReqXml_StockoutOrderCreate();
            reqSubOrderList = new List<HttpReqXml_StockoutOrderCreate_OrderLine>(1);
            reqObj.deliveryOrder.CopyFrom(cwmsOrder);
            foreach (var cwmsSubOrder in cwmsOrder.SubOrders)
            {
                HttpReqXml_StockoutOrderCreate_OrderLine orderLine = new HttpReqXml_StockoutOrderCreate_OrderLine();
                orderLine.CopyFrom(cwmsSubOrder.Value as CWmsExWarehouseSubOrder);
                reqSubOrderList.Add(orderLine);
            }

            reqObj.items = reqSubOrderList.ToArray();
            
            return reqObj;
        }
#endif

#if false // 由WMS模块处理
        /// <summary>
        /// 根据主退货订单Id获取主退货订单实体及其所有子退货订单的实体
        /// </summary>
        /// <param name="pRid">主退货订单Id</param>
        /// <returns>返回HttpRespXml_ReturnOrderCreate对象实体，若失败则返回Null</returns>
        static public HttpReqXmlBase GetReqXmlBody_ReturnOrderCreate(string pRid)
        {
            var ret = new ThirdResult<List<object>>(string.Format("根据主退货订单[{0}]获取主退货订单实体及其所有子退货订单的实体, 开始", pRid));

            HttpReqXmlBase retObj = null;
            HttpReqXml_ReturnOrderCreate tmpRespObj = null;
            List<HttpReqXml_ReturnOrderCreate_Orders> orderLinesList = null;
            CWmsReturnOrder order = null;

            try
            {
#region 从商城里获取主退货订单及其所有子单据
                order = GetCWmsReturnOrder(pRid);
#endregion
#region 根据商城的退货订单创建HttpRespXml_ReturnOrderCreate实体
                if (null != order)
                {
                    orderLinesList = new List<HttpReqXml_ReturnOrderCreate_Orders>(1);
                    foreach (var so in order.SubOrders)
                    {
                        HttpReqXml_ReturnOrderCreate_Orders orderLine = new HttpReqXml_ReturnOrderCreate_Orders();
                        orderLine.CopyFrom(so.Value as CWmsSubReturnOrder);
                        ret.Append(string.Format("itemcode={0}", (so.Value.MangoOrder as MangoSubReturnOrder).ProductId.ToString()));
                        orderLine.itemCode = (so.Value.MangoOrder as MangoSubReturnOrder).ProductId.ToString();
                        orderLinesList.Add(orderLine);
                    }

                    tmpRespObj = new HttpReqXml_ReturnOrderCreate();
                    tmpRespObj.returnOrder.CopyFrom(order);
                    tmpRespObj.orders = orderLinesList.ToArray();
                    retObj = tmpRespObj;
                    ret.Append(string.Format("根据商城的退货订单创建HttpRespXml实体: {0}， 子单据数量: {1}", retObj, (retObj as HttpReqXml_ReturnOrderCreate).orders.Length));
                }
                else
                {
                    ret.Append(string.Format("从商城里获取主退货订单及其所有子单据失败，order={0}", order));
                }
#endregion
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
            }
            ret.End();
            return retObj;
        }
#endif
#if false // Handler.NewOrder
        /// <summary>
        /// 根据主退货订单ID获取主退货订单及其所有子订单的实例
        /// </summary>
        /// <param name="pOrderId">芒果商城主退货订单ID</param>
        /// <returns>CWmsReturnOrder实体，若失败则返回null</returns>
        static public CWmsReturnOrder GetCWmsReturnOrder(string pOrderId)
        {
            var ret = new ThirdResult<List<object>>(string.Format("根据主退货订单{0}获取主退货订单及其所有子订单的实例, 开始", pOrderId));
            string errMsg = string.Empty;
            CWmsReturnOrder retObj = null;
            MangoReturnOrder mangoOrder = null;
            List<MangoSubReturnOrder> tmpList = new List<MangoSubReturnOrder>(1); ;   // 缓存子退货订单实体列表

#region validate parameters
            if (string.IsNullOrEmpty(pOrderId)) {
                ret.Append(string.Format("根据主退货订单{0}获取主退货订单及其所有子订单的实例失败， 非法入参", pOrderId));
                ret.End();
                return retObj;
            }
#endregion
            try
            {
#region Handle MangoOrder
#region  获取主退货订单 MangoReturnOrder
                if (null == (mangoOrder = MangoFactory.GetMangoOrder(TCWmsOrderCategory.EReturnOrder, pOrderId) as MangoReturnOrder))
                {
                    ret.Append(string.Format("根据主退货订单{0}获取主退货订单及其所有子订单的实例失败， 获取主退货订单失败", pOrderId));
                    ret.End();
                    return retObj;
                }
#endregion
#region 获取子退货订单列表 List<MangoSubReturnOrder>
                errMsg = MangoFactory.GetVSubReturnOrders(pOrderId, out tmpList);
                ret.Append(string.Format("tmplist.count={0}",tmpList.Count));
                ret.Append(string.Format("errMsg={0}", errMsg));

                if (!string.IsNullOrEmpty(errMsg) || null == tmpList || 0 >= tmpList.Count)
                {
                    ret.Append(string.Format("1根据主退货订单{0}获取主退货订单及其所有子订单的实例失败， 获取子退货订单失败, message={1}", pOrderId, errMsg));
                    ret.End();
                    return retObj;
                }
#endregion
#endregion // Handle MangoOrder
#region 创建CWmsReturnOrder实例及其子出库订单实例列表
                retObj = new CWmsReturnOrder();
                (retObj.MangoOrder as MangoReturnOrder).CopyFrom(mangoOrder);
                foreach (var t in tmpList)
                {
                    CWmsSubReturnOrder tmpSo = new CWmsSubReturnOrder();
                    tmpSo.CopyFrom(t, null);
                    // TODO: 根据子退货订单Id找原子订单Id, 移到Mis的Factory里面！！
                    var wcfSo = WCF<Product_Warehouse_ProductOutput>.Query((tmpSo.MangoOrder as MangoSubReturnOrder).ProductIOputId.Int()).Data;
                    // 根据原子订单Id取仓库Id, 根据仓库Id取货主
                    tmpSo.WmsOrderDetail.Owner.CopyFrom(MangoFactory.GetOwner(wcfSo.WarehouseId.ToString()));

                    // 设置商品Id
                    tmpSo.Product.ItemCode = (string.IsNullOrEmpty(t.ProductGuiGeID.ToString()) || "0".Equals(t.ProductGuiGeID.ToString())) ?
                            t.ProductId.ToString() : string.Format("{0}-{1}", t.ProductId, t.ProductGuiGeID);
                    retObj.SubOrders.Add(t.ZiTuihuoID.ToString(), tmpSo);// new CWmsSubReturnOrder(t, null));
                }
#region Handle WMS order
                switch ((T芒果商城退货物流)(retObj.MangoOrder as MangoReturnOrder).THwuLiu)
                {
                    case T芒果商城退货物流.自行返还:
                        {
                            (retObj.WmsOrder as Wms.Data.WmsReturnOrder).SetOrderFlag(false, true, false);
                            break;
                        }
                    case T芒果商城退货物流.蓝江上门:
                        {
                            (retObj.WmsOrder as Wms.Data.WmsReturnOrder).SetOrderFlag(true, false, false);
                            break;
                        }
                    default:
                        {
                            (retObj.WmsOrder as Wms.Data.WmsReturnOrder).SetOrderFlag(false, false, false);
                            break;
                        }
                }
#region TuiHuoType
                switch ((retObj.MangoOrder as MangoReturnOrder).TuiHuoType)
                {
                    case 2:
                        (retObj.MangoOrder as MangoReturnOrder).TuiHuoType = TWmsReturnOrderType.HHRK.Int();
                        break;
                    case 3:
                        (retObj.MangoOrder as MangoReturnOrder).TuiHuoType = TWmsReturnOrderType.HHRK.Int();
                        break;
                    case 6:
                        (retObj.MangoOrder as MangoReturnOrder).TuiHuoType = TWmsReturnOrderType.HHRK.Int();
                        break;
                    default:
                        {
                             (retObj.MangoOrder as MangoReturnOrder).TuiHuoType = TWmsReturnOrderType.HHRK.Int();
                            break;
                        }
                }
#endregion
#endregion // Handle WMS order
#endregion // 创建CWmsReturnOrder实例及其子出库订单实例列表
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
            }
            ret.Append(string.Format("根据主退货订单{0}获取主退货订单及其所有子订单的实例失败， 获取子退货订单失败, message={1}", pOrderId, errMsg));
            ret.End();
            return retObj;
        }
#endif
#if false // 暂不支持
#region 单据取消
        /// <summary>
        /// 根据主单据ID获取单据及其所有子单据的实例.
        /// </summary>
        /// <param name="pCate">单据类型（支持的类型包括取消商城订单和取消退货订单）</param>
        /// <param name="pOid">返回HttpReqXml_OrderCacnel对象实体，若失败则返回Null</param>
        /// <returns></returns>
        static public HttpReqXmlBase GetReqXmlBody_CancelOrder(TCWmsOrderCategory pCate, string pOid)
        {
            var ret = new ThirdResult<List<object>>(string.Format("单据[{0}]取消, 根据主单据{1}获取单据及其所有子单据的实例, 开始", pCate, pOid));

            HttpReqXmlBase retObj = null;
            HttpReqXml_OrderCacnel tmpReqObj = null;
            CWmsMcocOrder order = null;

            try
            {
                // 从获取单据实体
                if (null != (order = GetCWmsOrder(pCate, pOid) as CWmsMcocOrder))
                {
                    tmpReqObj = new HttpReqXml_OrderCacnel(order);
                    retObj = tmpReqObj;
                }
            }
            catch (Exception ex)
            {
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
            }
            ret.Append(string.Format("单据[{0}]取消, 根据主单据{1}获取单据及其所有子单据的实例完成, 返回{2}", pCate, pOid, retObj));
            ret.End();
            return retObj;
        }
#endregion
#endif
#if false // Handler.NewOrder
        /// <summary>
        /// 获取CWmsOrderBase实体
        /// </summary>
        /// <param name="pCate">单据类型</param>
        /// <param name="pId">单据Id</param>
        /// <returns>若成功则返回CWmsOrderBase实体；否则返回null</returns>
        static public CWmsOrderBase GetCWmsOrder(TCWmsOrderCategory pCate, string pId)
        {
            switch (pCate)
            {
                case TCWmsOrderCategory.EEntryOrder: return GetCWmsEntryOrder(pId);
                case TCWmsOrderCategory.EExwarehouseOrder: return GetCWmsStockoutOrder(pId);
                case TCWmsOrderCategory.EMcocReturnOrder:
                case TCWmsOrderCategory.EReturnOrder: return GetCWmsReturnOrder(pId);
                case TCWmsOrderCategory.EMallOrder: return GetCWmsMallOrder(pId);
                default: return null;
            }
        }
#endif

#region 供应商
#if false
        /// <summary>
        /// 根据??获取单据对应的承运商实体。若成功则返回对应实体；否则返回默认
        /// </summary>
        /// <param name="pCate">单据类别</param>
        /// <param name="pId">??</param>
        /// <returns>若成功则返回对应实体；否则返回默认</returns>
        static public Wms.Data.WmsLogistics GetLogistics(TCWmsOrderCategory pCate, string pId)
        {
            switch (pCate)
            {
                case TCWmsOrderCategory.EEntryOrder:
                    {
#if true
                        return GetLogisticsByEntryOrderId(pId);
#else
                        if (T芒果商城退货物流.自行返还 == (T芒果商城退货物流)pId.Int())
                            return new Wms.Data.WmsLogistics(CWmsConsts.cStrWmsLogisticsZTCode, CWmsConsts.cStrWmsLogisticsZTName);
                        else if ((T芒果商城退货物流.蓝江上门 == (T芒果商城退货物流)pId.Int()))
                            return new Wms.Data.WmsLogistics(CWmsConsts.cStrWmsLogisticsZYWLCode, CWmsConsts.cStrWmsLogisticsZYWLName);
                        else return new Wms.Data.WmsLogistics(CWmsConsts.cStrWmsLogisticsQTCode, CWmsConsts.cStrWmsLogisticsQTName);
#endif
                    }
                case TCWmsOrderCategory.EExwarehouseOrder:  // 出库订单
                    {
                        return GetLogisticsByStockoutOrderId(pId);
                    }
                case TCWmsOrderCategory.EReturnOrder:
                    {
                        return GetLogisticsByReturnOrderId(pId);
                    }
                default:
                    // 获取默认承运商
                    return GetDefaultLogistic();
            }
        }
#endif

        /// <summary>
        /// get default logistics.
        /// </summary>
        /// <returns>return entity of Wms.Data.WmsLogistics as default logistics</returns>
        static public Wms.Data.WmsLogistics GetDefaultLogistic()
        {
            var defaultLogistics = CWmsMisSystemParamCache.Cache.GetDefaultLogistics();
            if (null == defaultLogistics)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("Failed in get cached default logistics. Wms.Data.WmsLogistics CWmsDataFactory.GetDefaultLogistic() return null");
                return null;
            }
            else
            {
                return new Wms.Data.WmsLogistics(defaultLogistics.Code, defaultLogistics.Name);
            }
        }

#if false
        /// <summary>
        /// get logistics by entry order. return entity of Wms.Data.WmsLogistics if success -or- return null if failed.
        /// </summary>
        /// <param name="pId">ID of the entry order</param>
        /// <returns>return entity of Wms.Data.WmsLogistics if success -or- return null if failed.</returns>
        static public Wms.Data.WmsLogistics GetLogisticsByEntryOrderId(string pId)
        {
            throw new NotImplementedException("");
        }

        /// <summary>
        /// get logistics from stockout order, return entity of Wms.Data.WmsLogistics if success -or- return null if failed.
        /// </summary>
        /// <param name="pId">ID of the stockout order.</param>
        /// <returns>return entity of Wms.Data.WmsLogistics if success -or- return null if failed.</returns>
        static public Wms.Data.WmsLogistics GetLogisticsByStockoutOrderId(string pId)
        {
            Product_Warehouse_ProductMainOutput stockoutOrder = null; // TODO: 根据主出库单Id找主出库单实体
            Product_PeiSong_ProductMain deliveryOrder = null; // TODO: 根据主出库单Id找主配送单实体
            if (null == stockoutOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("根据主出库订单[{0}]获取承运商对象失败, 无法根据主出库单编码获取主出库单实体", pId);
                return null;
            }
            else if (null == deliveryOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("根据主出库订单[{0}]获取承运商对象失败, 无法根据主出库单编码获取主配送单实体", pId);
                return null;
            }
            else
            {
                // TODO: 根据主配送单中的‘配送人’判读使用第三方物流还是芒果物流
                var logistics = CWmsMisSystemParamCache.Cache.GetLogisticsByUserId(deliveryOrder.DeliveryUserId.Int().ToString());
                return new Wms.Data.WmsLogistics(logistics?.Code, logistics?.Name); // TODO： logistics为null怎么办？
            }
        }

        /// <summary>
        /// get logistics by return order. return entity of Wms.Data.WmsLogistics if success -or- return null if failed.
        /// </summary>
        /// <param name="pId">ID of the return order</param>
        /// <returns>return entity of Wms.Data.WmsLogistics if success -or- return null if failed.</returns>
        static public Wms.Data.WmsLogistics GetLogisticsByReturnOrderId(string pId)
        {
            throw new NotImplementedException("");
        }
#endif
#endregion
    }
}
