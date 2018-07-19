using System;
using System.Collections.Generic;
using System.Linq;
using MisModel;
using TT.Common.Frame.Model;
using MangoMis.Frame.DataSource.Simple;
using MangoMis.Frame.Helper;
using C_WMS.Data.Mango.Data;
using C_WMS.Data.CWms.CWmsEntity;
//using CWmsInterface.DataModel.MangoEntity;
using MangoMis.Frame.ThirdFrame;
using System.Reflection;

namespace C_WMS.Data.Mango.MisModelPWI
{
    /// <summary>
    /// Mis2014系统中相关数据类型的操作类
    /// </summary>
    class MisModelFactory
    {
        #region 入库订单
        /*
        /// <summary>
        /// 返回Mis2014系统的主入库单实例
        /// </summary>
        /// <param name="pOrderId">主入库单ID</param>
        /// <returns>若成功则返回Product_Warehouse_ProductMainInput实例；否则返回null。</returns>
        static protected Product_Warehouse_ProductMainInput GetMisEntryOrder(string pOrderId)
        {
            // validate argument
            int orderId = -1;   // orderId in integer
            if (!int.TryParse(pOrderId, out orderId))
            {
                return null;
            }

            DefaultResult<Product_Warehouse_ProductMainInput> entityRslt = null;    // WCF<Product_Warehouse_ProductMainInput> query result
            Product_Warehouse_ProductMainInput entity = new Product_Warehouse_ProductMainInput();   // main entry order entity

            // get the speicifid main entry order
            entity.ProductInputMainId = orderId;
            entityRslt = WCF<Product_Warehouse_ProductMainInput>.Query(TypeHelper.IntConvert(entity.ProductInputMainId));

            if (null != entityRslt && null != entityRslt.Data) // get order successful
            {
                return entityRslt.Data;
            }
            else // failed in getting main entry order
            {
                return null;
            }
        }
        */

        /// <summary>
        /// 返回Mis2014系统定义的子入库单实例
        /// </summary>
        /// <param name="pOrderId">子入库单ID</param>
        /// <returns>若成功则返回MangoSubEntryOrder实例；否则返回null。</returns>
        static protected Product_Warehouse_ProductInput GetMisSubEntryOrder(string pOrderId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回当前项目定义的子入库单实例
        /// </summary>
        /// <param name="pOrderId">子入库单ID</param>
        /// <returns>若成功则返回MangoSubEntryOrder实例；否则返回null。</returns>
        static public CWmsSubEntryOder GetSubEntryOrder(string pOrderId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回Mis2014系统定义的子入库单实例列表
        /// </summary>
        /// <param name="pOrderId">子入库单ID</param>
        /// <param name="pSubOrderList">返回子入库单实例列表，若失败则返回Count=0的列表</param>
        /// <returns>若添加成功则返回string.Empty; 否则返回添加失败的错误描述</returns>
        static protected string GetMisSubEntryOrderList(string pOrderId, out List<Product_Warehouse_ProductInput> pSubOrderList)
        {
            string retStr = string.Empty;
            pSubOrderList = null; // reset pSubOrderList

            // validate argument
            int orderId = -1;   // stores an integer value of pOrderId
            if (!int.TryParse(pOrderId, out orderId))
            {
                retStr= string.Format("ERROR!! 异常的输入参数pOrderId={0}", pOrderId);
                return retStr;
            }

            try
            {
                // temp variables definition
                // fileter for query sub entry order list
                List<CommonFilterModel> filters = new List<CommonFilterModel>(1) { new CommonFilterModel("MainId", "=", orderId.ToString()) }; // TODO: donot use MainId
                Product_Warehouse_ProductMainInput entity = new Product_Warehouse_ProductMainInput();   // main entry order entity

                var wcfSubListRet = WCF<Product_Warehouse_ProductInput>.Query(1, 1000, filters, null); // TODO: donot use 1 and 1000

                if (null != wcfSubListRet && null != wcfSubListRet.Data)
                {
                    pSubOrderList = wcfSubListRet.Data;
                    retStr = string.Empty;
                }
                else
                {
                    retStr = string.Format("ERROR!! 获取子入库单列表失败, pOrderId={0}, wcfSubListRet={1}", pOrderId,wcfSubListRet);
                }
            }
            catch (Exception ex)
            {
                retStr = string.Format("Exception ERROR!! 获取子入库单列表失败, pOrderId={0}, Message={1}", pOrderId, ex.Message);
            }

            return retStr;
        }
        
        #endregion // 入库订单

        #region 采购订单
        /*
        /// <summary>
        /// 返回Mis2014系统定义的主采购订单实例
        /// </summary>
        /// <param name="pOrderId">单据ID</param>
        /// <returns>>若成功则返回Product_Warehouse_ProductMainBuy实例；否则返回null</returns>
        static protected Product_Warehouse_ProductMainBuy GetMisPurchaseOrder(string pOrderId)
        {
            int orderId = -1;   // orderId in integer
            Product_Warehouse_ProductMainBuy outOrder = null;
            DefaultResult<Product_Warehouse_ProductMainBuy> entityRslt = null;

            // validate argument
            if (!int.TryParse(pOrderId, out orderId))
            {
                return outOrder;
            }

            // 
            entityRslt = WCF<Product_Warehouse_ProductMainBuy>.Query(orderId);
            if (null != entityRslt && 0 < entityRslt.RetInt)
            {
                outOrder = entityRslt.Data;
            }

            return outOrder;
        }
        */
        /// <summary>
        /// 返回Mis2014系统定义的子采购单实例
        /// </summary>
        /// <param name="pOrderId">子采购单ID</param>
        /// <returns>若成功则返回MangoSubEntryOrder实例；否则返回null。</returns>
        static protected Product_Warehouse_ProductBuy GetMisSubPurchaseOrder(string pOrderId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回当前项目定义的子采购单实例
        /// </summary>
        /// <param name="pOrderId">子采购单ID</param>
        /// <returns>若成功则返回MangoSubEntryOrder实例；否则返回null。</returns>
        static public MangoSubPurchaseOrder GetSubPurchaseOrder(string pOrderId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回Mis2014系统定义的子采购单实例列表
        /// </summary>
        /// <param name="pOrderId">主采购单ID</param>
        /// <param name="pSubOrderList">返回子采购单实例列表，若失败则返回Count=0的列表</param>
        /// <returns>若添加成功则返回string.Empty; 否则返回添加失败的错误描述</returns>
        static protected string GetMisSubPurchaseOrderList(string pOrderId, out List<Product_Warehouse_ProductBuy> pSubOrderList)
        {
            var ret = new ThirdResult<int>("MisModelFactory.GetMisSubPurchaseOrderList() 开始");

            string retStr = string.Empty;
            pSubOrderList = null;

            ret.Append(string.Format("开始设置查询Filter"));
            try
            {
                List<CommonFilterModel> filter = new List<CommonFilterModel>(2);
                filter.Add(new CommonFilterModel(Mis2014_SubPurchaseOrder_Column.MainId, "=", pOrderId));
                filter.Add(new CommonFilterModel(Mis2014_SubPurchaseOrder_Column.IsDel, "=", TMis2014_IsDel.EDeleted.ToString()));

                ret.Append(string.Format("开始查询所有子订单"));
                var wcfSubPurchaseOrder = WCF<Product_Warehouse_ProductBuy>.ColumnsAll(filter, Mis2014_SubPurchaseOrder_Column.AllColumnNameList());

                if (null == wcfSubPurchaseOrder || null == wcfSubPurchaseOrder.Data)
                {
                    retStr = "WCF取得列表返回null,判定为WCF连接失败";
                    ret.Append(retStr);
                }
                else
                {
                    ret.Append(string.Format("WCF取得列表，item数量={0}", wcfSubPurchaseOrder.Data.Count));
                    pSubOrderList = wcfSubPurchaseOrder.Data; // reset pSubOrderList
                    retStr = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ret.Append(string.Format("发生异常: {0}\r\n{1}", ex.Message, ex.StackTrace));
                if (null != ex.InnerException)
                {
                    ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                }
            }

            ret.End(TError.RunGood, string.Format("MisModelFactory.GetMisSubPurchaseOrderList() 开始结束:{0}", retStr));
            return retStr;
        }

        /// <summary>
        /// 返回当前项目定义的子采购单实例列表
        /// </summary>
        /// <param name="pOrderId">子采购单ID</param>
        /// <param name="pSubOrderList">返回子采购单实例列表，若失败则返回Count=0的列表</param>
        /// <returns>若成功则返回string.Empty; 否则返回添加失败的错误描述</returns>
        static public string GetSubPurchaseOrderList(string pOrderId, out List<MangoSubPurchaseOrder> pSubOrderList)
        {
            var ret = new ThirdResult<int>("MisModelFactory.GetSubPurchaseOrderList() 开始");

            string retStr = string.Empty;
            pSubOrderList = new List<MangoSubPurchaseOrder>(1); // reset out parameter
            List<Product_Warehouse_ProductBuy> tmpList = null;

            try
            {
                ret.Append(string.Format("开始查询所有子订单"));

                retStr = GetMisSubPurchaseOrderList(pOrderId, out tmpList); // get sub order list from Mis2014

                if (string.IsNullOrEmpty(retStr) && null != tmpList)
                {
                    foreach (Product_Warehouse_ProductBuy p in tmpList)
                    {
                        MangoSubPurchaseOrder tmpSubOrder = new MangoSubPurchaseOrder();
                        tmpSubOrder.CreateFrom(p);
                        pSubOrderList.Add(tmpSubOrder);
                    }
                    ret.Append(string.Format("子订单数量:{0}", pSubOrderList.Count));
                }
                else
                {
                    ret.Append(string.Format("!string.IsNullOrEmpty(retStr) || null == tmpList)"));
                }
            }
            catch (Exception ex)
            {
                retStr = ex.Message;

                ret.Append(string.Format("发生异常: {0}\r\n{1}", ex.Message, ex.StackTrace));
                if (null != ex.InnerException)
                {
                    ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                }
                ret.End(TError.WCF_RunError, string.Format("MisModelFactory.GetSubPurchaseOrderList() 异常结束"));
            }
            return retStr;
        }
        #endregion // 采购订单

#region 出库订单
#if false
        /// <summary>
        /// 返回Mis2014系统定义的主采购订单实例
        /// </summary>
        /// <param name="pOrderCate">单据类型</param>
        /// <param name="pOrderId">单据ID</param>
        /// <returns>>若成功则返回Product_Warehouse_ProductMainBuy实例；否则返回null</returns>
        static protected EntityBase GetMisOrder(TCWmsOrderCategory pOrderCate, string pOrderId)
        {
            switch (pOrderCate)
            {
                case TCWmsOrderCategory.EEntryOrder:
                    {
                        return WCF<Product_Warehouse_ProductInput>.Query(int.Parse(pOrderId)).Data;
                    }
                case TCWmsOrderCategory.EPurchaseOrder:
                    {
                        return WCF<Product_Warehouse_ProductMainBuy>.Query(int.Parse(pOrderId)).Data;
                    }
                case TCWmsOrderCategory.EExwarehouseOrder:
                    {
                        return WCF<Product_Warehouse_ProductMainOutput>.Query(int.Parse(pOrderId)).Data;
                    }
                case TCWmsOrderCategory.EReturnOrder:
                    {
                        return WCF<Product_TuiHuo_main>.Query(int.Parse(pOrderId)).Data;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
#endif
#endregion

#region 商品      
#endregion
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pFilters"></param>
        /// <returns></returns>
        static public TEntity GetMisEntity<TEntity>(List<CommonFilterModel> pFilters) where TEntity : class, new()
        {
            try
            {
                var wcfRslt = MangoWCF<TEntity>.GetList(pFilters);
                if (null != wcfRslt && null != wcfRslt.Data && 0 < wcfRslt.Data.Count)
                {
                    return wcfRslt.Data.First();
                }
                else
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}<{2}>({3})，没有获取到实体(wcfRslt={4}, wcfRslt.RetInt={5}, wcfRslt.RETData={6}, wcfRslt.Debug={7}), Filter debug:\r\n{8}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, typeof(TEntity), pFilters, wcfRslt, wcfRslt?.RetInt, wcfRslt?.RETData, wcfRslt?.Debug, GetDebugInfo_MisFilter(pFilters));
                    return null;
                }
            }
            catch(Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "!!Exception in {0}.{1}<{2}>({3})，Filters debug:\r\n{4}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, typeof(TEntity), pFilters, GetDebugInfo_MisFilter(pFilters));
                return null;
            }
        }

        /// <summary>
        /// get entity of TEntity by primary_key, return null if failed.
        /// </summary>
        /// <typeparam name="TEntity">indicate the type of entity to be retrieved.</typeparam>
        /// <param name="pPk">primary key of entity</param>
        /// <returns></returns>
        static public TEntity GetMisEntity<TEntity>(string pPk) where TEntity : class, new()
        {
            int orderId = -1;
            TEntity rslt = null;
            try
            {
                orderId = int.Parse(pPk);
                var wcfRslt = MangoWCF<TEntity>.GetEntity(orderId);
                if (null == wcfRslt || null == wcfRslt.Data)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("failed in TEntity MisModelFactory.GetMisOrder<{0}>({1}), WCF run error: WCF={2}, WCF.RetInt={3}, WCF.DEBUG={4}"
                        , typeof(TEntity), pPk, wcfRslt, wcfRslt?.RetInt, wcfRslt?.Debug);
                }
                else
                    rslt = wcfRslt.Data;
                return rslt;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "!!Exception in TEntity MisModelFactory.GetMisOrder<{0}>({1}), return {2}", typeof(TEntity), pPk, rslt);
                return rslt;
            }
        }

#region 入库订单
        /// <summary>
        /// 返回Mis2014系统定义的子入库单实例列表
        /// </summary>
        /// <param name="pOrderId">子入库单ID</param>
        /// <param name="pSubOrderList">返回子入库单实例列表，若失败则返回Count=0的列表</param>
        /// <returns>若添加成功则返回string.Empty; 否则返回添加失败的错误描述</returns>
        static public string GetSubEntryOrderList(string pOrderId, out List<Product_Warehouse_ProductInput> pSubOrderList)
        {
            var ret = new ThirdResult<List<object>>(string.Format("返回Mis2014系统定义的子入库单({0})实例列表开始", pOrderId));

            // temp variables definition
            string retStr = string.Empty;
            pSubOrderList = new List<Product_Warehouse_ProductInput>(1);

            try
            {
                // fileter for query sub entry order list
                List<CommonFilterModel> filters = new List<CommonFilterModel>(1) {
                    new CommonFilterModel(Mis2014_SubEntryOrder_Column.MainId, "=", pOrderId)
                    , new CommonFilterModel(Mis2014_SubEntryOrder_Column.IsDel, "<>", TDict285_Values.EDeleted.Int().ToString())
                };

                var wcfSubListRet = WCF<Product_Warehouse_ProductInput>.Query(1, CWmsConsts.cIntDefaultWcfQueryPageSize, filters, null);

                if (null != wcfSubListRet && null != wcfSubListRet.Data)
                {
                    pSubOrderList = wcfSubListRet.Data;
                    retStr = string.Empty;
                    ret.Append(string.Format("WCF查询完成, ret={0}, ret.Data={1}, SubListCount={2}", wcfSubListRet, wcfSubListRet.Data, wcfSubListRet.Data.Count));
                }
                else
                {
                    retStr = string.Format("ERROR!! 获取子入库单列表失败, pOrderId={0}, wcfSubListRet={1}", pOrderId, wcfSubListRet);
                    ret.Append(retStr);
                }
            }
            catch (Exception ex)
            {
                pSubOrderList.Clear();
                retStr = string.Format("Exception!! 获取子入库单列表失败, pOrderId={0}, Message={1}", pOrderId, ex.Message);

                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
            }

            ret.End();
            return retStr;
        }

        /// <summary>
        /// 根据过滤条件查询Mis实体，若执行成功则返回实体数量，否则返回TError.WCF_RunError或-1。
        /// </summary>
        /// <param name="pFilters"></param>
        /// <param name="pOutList">返回查询到Mis实体列表，若执行失败则返回Count为0的List</param>
        /// <param name="pErrMsg"></param>
        /// <returns></returns>
        static public int GetMisOrderList<T>(List<CommonFilterModel> pFilters, out List<T> pOutList, out string pErrMsg) where T : class, new()
        {
            int err = TError.WCF_RunError.Int();
            var wcfRslt = MangoWCF<T>.GetList(pFilters);
            if (null != wcfRslt && null != wcfRslt.Data)
            {
                C_WMS.Data.Utility.MyLog.Instance.Info("在{0}中，根据过滤条件获取的{2}实体数量为{1}", MethodBase.GetCurrentMethod().Name, wcfRslt.Data.Count, typeof(T));
                pErrMsg = (0 == wcfRslt.Data.Count) ? string.Format("在{0}中，根据过滤条件获取的Mis实体数量为0，DEBUG: {1}", MethodBase.GetCurrentMethod().Name, wcfRslt.Debug) : string.Empty;
            }
            else
            {
                pErrMsg = string.Format("在{0}中，根据过滤条件获取{2}实体失败， DEBUG: {1}", MethodBase.GetCurrentMethod().Name, wcfRslt?.Debug, typeof(T));
            }

            pOutList = (null != wcfRslt?.Data) ? wcfRslt.Data.ToList() : new List<T>(1);
            C_WMS.Data.Utility.MyLog.Instance.Info("在{0}中，根据过滤条件获取的{3}实体完成, pOutList={1}, Count={2}", MethodBase.GetCurrentMethod().Name, pOutList, pOutList?.Count, typeof(T));
            err = (0 == pOutList.Count) ? -1 : pOutList.Count;
            if (!string.IsNullOrEmpty(pErrMsg)) C_WMS.Data.Utility.MyLog.Instance.Error(pErrMsg);
            return (null == wcfRslt) ? TError.WCF_RunError.Int() : wcfRslt.RetInt.Int();
        }
#endregion // 入库订单

#region 采购订单
        /// <summary>
        /// 返回Mis2014系统定义的子采购单实例列表
        /// </summary>
        /// <param name="pOrderId">主采购单ID</param>
        /// <param name="pSubOrderList">返回子采购单实例列表，若失败则返回Count=0的列表</param>
        /// <returns>若添加成功则返回string.Empty; 否则返回添加失败的错误描述</returns>
        static public string GetSubPurchaseOrderList(string pOrderId, out List<Product_Warehouse_ProductBuy> pSubOrderList)
        {
            var ret = new ThirdResult<int>("返回Mis2014系统定义的子采购单实例列表 开始");

            string retStr = string.Empty;
            pSubOrderList = null;

            try
            {
                List<CommonFilterModel> filter = new List<CommonFilterModel>(2);
                filter.Add(new CommonFilterModel(Mis2014_SubPurchaseOrder_Column.MainId, "=", pOrderId));
                filter.Add(new CommonFilterModel(Mis2014_SubPurchaseOrder_Column.IsDel, "=", TMis2014_IsDel.EDeleted.ToString()));

                var wcfSubPurchaseOrder = WCF<Product_Warehouse_ProductBuy>.ColumnsAll(filter, Mis2014_SubPurchaseOrder_Column.AllColumnNameList());

                if (null == wcfSubPurchaseOrder || null == wcfSubPurchaseOrder.Data)
                {
                    retStr = "WCF取得列表返回null,判定为WCF连接失败";
                    ret.Append(retStr);
                }
                else
                {
                    ret.Append(string.Format("WCF取得列表，item数量={0}", wcfSubPurchaseOrder.Data.Count));
                    pSubOrderList = wcfSubPurchaseOrder.Data; // reset pSubOrderList
                    retStr = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ret.Append(string.Format("发生异常: {0}\r\n{1}", ex.Message, ex.StackTrace));
                if (null != ex.InnerException)
                {
                    ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                }
            }

            ret.Append(string.Format("MisModelFactory.GetMisSubPurchaseOrderList() 开始结束:{0}", retStr));
            ret.End();
            return retStr;
        }
#endregion // 采购订单

#region 出库订单
        /// <summary>
        /// 返回Mis2014系统定义的主采购订单实例
        /// </summary>
        /// <param name="pOrderCate">单据类型</param>
        /// <param name="pOrderId">单据ID</param>
        /// <returns>>若成功则返回Product_Warehouse_ProductMainBuy实例；否则返回null</returns>
        static public EntityBase GetMisOrder(TCWmsOrderCategory pOrderCate, string pOrderId)
        {
            try
            {
                EntityBase retObj = null;
                switch (pOrderCate)
                {
                    case TCWmsOrderCategory.EEntryOrder: { retObj = MangoWCF<Product_Warehouse_ProductMainInput>.GetEntity(int.Parse(pOrderId))?.Data; break; }
                    case TCWmsOrderCategory.EPurchaseOrder: { retObj = MangoWCF<Product_Warehouse_ProductMainBuy>.GetEntity(int.Parse(pOrderId))?.Data; break; }
                    case TCWmsOrderCategory.EExwarehouseOrder: { retObj = MangoWCF<Product_Warehouse_ProductMainOutput>.GetEntity(int.Parse(pOrderId))?.Data; break; }
                    case TCWmsOrderCategory.EReturnOrder: { retObj = MangoWCF<Product_TuiHuo_main>.GetEntity(int.Parse(pOrderId))?.Data; break; }
                    case TCWmsOrderCategory.EMallOrder: { retObj = MangoWCF<Product_User_DingDan>.GetEntity(int.Parse(pOrderId))?.Data; break; }
                    default: break;
                }

                if (null == retObj)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}({2}, {3})，没有获取到实体", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pOrderCate, pOrderId);
                }
                return retObj;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "!!Exception in {0}.{1}({2}, {3})", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pOrderCate, pOrderId);
                return null;
            }
        }

        /// <summary>
        /// 返回当前项目定义的子出库单实例列表
        /// </summary>
        /// <param name="pOrderId">主出库单ID</param>
        /// <param name="pSubOrderList">返回子出库单实例列表，若失败则返回Count=0的列表</param>
        /// <returns>若成功则返回string.Empty; 否则返回添加失败的错误描述</returns>
        static public string GetSubExwarehouseOrderList(string pOrderId, out List<Product_Warehouse_ProductOutput> pSubOrderList)
        {
            string retStr = string.Empty;
            pSubOrderList = null;

            try
            {
                List<CommonFilterModel> filter = new List<CommonFilterModel>(2);
                filter.Add(new CommonFilterModel(Mis2014_SubExwarehouseOrder_Column.MainId, "=", pOrderId));
                filter.Add(new CommonFilterModel(Mis2014_SubExwarehouseOrder_Column.IsDel, "<>", TMis2014_IsDel.EDeleted.Int().ToString()));

                var wcfSubOrder = WCF<Product_Warehouse_ProductOutput>.ColumnsAll(filter, Mis2014_SubExwarehouseOrder_Column.AllColumnNameList());

                if (null == wcfSubOrder || null == wcfSubOrder.Data)
                {
                    retStr = string.Format("根据主出库订单Id{0}获取其所有子订单失败, WCF={1}, WCF.Data={2}, \r\nWCF.DEBUG={3}", wcfSubOrder, wcfSubOrder?.Data, wcfSubOrder?.Debug);
                    C_WMS.Data.Utility.MyLog.Instance.Error(retStr);
                }
                else
                {
                    pSubOrderList = wcfSubOrder.Data.ToList(); // reset pSubOrderList
                    retStr = string.Empty;
                }
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "根据主出库订单Id{0}获取其所有子订单异常", pOrderId);
                retStr = string.Format("根据主出库订单Id{0}获取其所有子订单异常", pOrderId);
            }

            return retStr;
        }
#endregion

#region 退货订单
        /// <summary>
        /// 返回Mis2014系统定义的子退货入库单实例列表
        /// </summary>
        /// <param name="pOrderId">子退货入库单ID</param>
        /// <param name="pSubOrderList">返回子退货入库单实例列表，若失败则返回Count=0的列表</param>
        /// <returns>若添加成功则返回string.Empty; 否则返回添加失败的错误描述</returns>
        static public string GetSubReturnOrderList(string pOrderId, out List<Product_TuiHuo> pSubOrderList)
        {
            var ret = new ThirdResult<int>(string.Format("获取子退货订单列表List(Product_TuiHuo)开始, pOrderId={0}", pOrderId));

            string retStr = string.Empty;
            pSubOrderList = new List<Product_TuiHuo>(1);

            // validate argument
            if (string.IsNullOrEmpty(pOrderId))
            {
                retStr = string.Format("获取子退货订单列表List(Product_TuiHuo)失败，非法入参");
                ret.Append(retStr);
                ret.End();
                return retStr;
            }

            try
            {
                // temp variables definition
#region 设置过滤器
                List<CommonFilterModel> filters = new List<CommonFilterModel>(1) {
                    new CommonFilterModel(Mis2014_SubReturnOrder_Column.TuiHuoMainID, "=", pOrderId)

                };
#endregion

                // WCF查询
                var wcfVRslt = WCF<Product_TuiHuo>.Query(1, CWmsConsts.cIntDefaultWcfQueryPageSize, filters, null);
                if (null == wcfVRslt)
                {
                    retStr = "获取子退货订单列表List(Product_TuiHuo)结束，WCF获取失败，返回对象null";
                }
                else if (null == wcfVRslt.Data || 0 >= wcfVRslt.Data.Count)
                {
                    retStr = string.Format("获取子退货订单列表List(Product_TuiHuo)结束，WCF获取失败，RETData={0}, Debug={1}", wcfVRslt.RETData, wcfVRslt.Debug);
                }
                else
                {
                    pSubOrderList = wcfVRslt.Data;
                    retStr = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ret.Append(string.Format("发生异常: {0}\r\n{1}", ex.Message, ex.StackTrace));
                if (null != ex.InnerException) { ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message)); }
            }
            ret.Append(string.Format("获取子退货订单列表List(Product_TuiHuo)结束, 子退货订单数量={0}, retStr={1}", pSubOrderList.Count, retStr));
            ret.End();
            return retStr;
        }
#endregion

        /// <summary>
        /// get debug info descriptor of MIS entity filters.
        /// this method should not throw exception.
        /// </summary>
        /// <param name="pFilters">filter to be debugged.</param>
        /// <returns></returns>
        static public string GetDebugInfo_MisFilter(List<CommonFilterModel> pFilters)
        {
            string dbgInfo = "FILTER DEBUG INFO: ";
            try
            {
                if (null == pFilters || 0 == pFilters.Count)
                {
                    return dbgInfo += string.Format("empty pFilters.Count={0}", pFilters?.Count);
                }
                else
                {
                    foreach (CommonFilterModel f in pFilters)
                    {
                        dbgInfo += string.Format("\r\nFilter({0})[Name={1}, Filter={2}, Value={3}]", f, f?.Name, f?.Filter, f?.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                dbgInfo += string.Format("!!Exception, {0}.{1}({2})", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pFilters);
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, dbgInfo);
            }
            return dbgInfo;
        }
    }
}
