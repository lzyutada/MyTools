using C_WMS.Data.CWms;
using C_WMS.Data.CWms.CWmsEntity;
using C_WMS.Data.CWms.Interfaces.Methods;
using C_WMS.Data.Mango;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.Wms.Data;
using C_WMS.Data.Wms.Transaction;
using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Transaction
{
    /// <summary>
    /// class of inventory monitoring.
    /// </summary>
    class CWmsInventoryMonitoring : MWmsTransactionBase<CWmsInventoryMonitoring, HttpReqXml_InventoryMonitoring, HttpRespXml_InventoryMonitoring>
    {
        /// <summary>
        /// init Impl for inventory monitoring
        /// </summary>
        /// <returns></returns>
        protected override IMWmsTransactionImpl<HttpReqXml_InventoryMonitoring, HttpRespXml_InventoryMonitoring> InitImpl()
        {
            return new CWmsInventoryMonitoringImpl();
        }

        /// <summary>
        /// <para>retrieve barcode from WMS service for product whose ProductId is pid.</para>
        /// <para>return TError.RunGood if succeed in retrieving; return others if WMS service response failure butnot product doesn't exist.</para>
        /// </summary>
        /// <param name="pid">id of product</param>
        /// <param name="barCode">return barcode</param>
        public int GetBarCode(string pid, out string barCode)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 库存监控接口的Impl类。
    /// 针对一个MIS中的商品，应监控其在WMS系统做所有货主及各货主下所有仓库中的库存
    /// </summary>
    class CWmsInventoryMonitoringImpl : CWmsTransactionImplBase<HttpReqXml_InventoryMonitoring, HttpRespXml_InventoryMonitoring>
    {
        /// <summary>
        /// get or set a list of product id to be monitoring for inventory
        /// </summary>
        List<string> ProductIdList { get { return _productIdList; } }
        List<string> _productIdList = null;

        /// <summary>
        /// get -or- set a list of owner codes to be monitoring for inventory
        /// </summary>
        List<CWmsOwner> OwnerList { get { return _ownerList; } }
        List<CWmsOwner> _ownerList = null;

        /// <summary>
        /// overriding void Dispose()
        /// </summary>
        public override void Dispose()
        {
            if (null != ProductIdList) ProductIdList.Clear();
            if (null != OwnerList) OwnerList.Clear();
        }

        /// <summary>
        /// 返回API接口的method
        /// </summary>
        /// <returns></returns>
        override public string GetApiMethod()
        {
            return CWmsMisSystemParamCache.Cache.ApiMethod_InventoryMonitoring?.PValue ?? string.Empty;
        }

        /// <summary>
        /// <para>针对int DoTransaction(out HttpRespXmlBase, out string, params object[])和</para>
        /// <para>TResponse DoTransaction(params object[])方法， 解析params object[]入参。</para>
        /// <para>参数支持一个string类型的商品ID（商城编码）或一个IEnumerable[string]类型的商品ID（商城编码）列表</para>
        /// <para>若解析成功则返回TError.RunGood，并将商品编码保存在ProductId中；否则返回其他值</para>
        /// </summary>
        /// <param name="args">待解析的参数。支持一个string类型的商品ID（商城编码）或一个List[string]类型的商品ID（商城编码）列表</param>
        /// <returns>若解析成功则返回TError.RunGood；否则返回其他值</returns>
        override public int ParseArguments(params object[] args)
        {
            try
            {
                int err = 0;
                if (0 == args?.Length)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.ParseArguments(), invalid empty args={1}", GetType(), args);
                    return err = TError.Post_ParamError.Int();
                }
                // parse 1st argument for product Id.
                if (TError.RunGood.Int() != (err = ParseArgument(args[0],true, out _productIdList)))
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.ParseArguments(), invalid arg[0]={1}", GetType(), args[0]);
                    return err;
                }
                // parse 2nd argument for owner
                List<string> ownerCodeList = null;
                if (TError.RunGood.Int() != (err = ParseArgument(args?[1], true, out ownerCodeList)))
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.ParseArguments(), invalid arg[1]={1}", GetType(), args?[1]);
                    return err;
                }
                return err;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "{0}.ParseArguments({0})发生异常.\r\nDEBUG INFO: {1}", GetType(), args, Utility.CWmsDataUtility.GetDebugInfo_Args(args));
                return TError.Post_ParamError.Int();
            }
        }

        /// <summary>
        /// 在执行同步操作之前，先重置709字典中对应的行，将IsUpdateOK的值置为同步失败。
        /// 库存监控接口属于获取类操作，无需操作709字典。
        /// </summary>
        /// <returns></returns>
        override public int Reset709()
        {
            return TError.RunGood.Int(); // do nothing
        }

        /// <summary>
        /// <para>根据同步通讯的结果更新709字典中对应的行的IsUpdateOK的值。</para>
        /// <para>若重置成功则影响的行数（重置多行）或影响行的主键（重置单行）；否则返回TError或0.</para>
        /// <para>库存监控接口属于获取类操作，无需操作709字典。</para>
        /// </summary>
        /// <param name="pUpdateOK">value of 709.isUpdateOK</param>
        /// <returns>若重置成功则影响的行数（重置多行）或影响行的主键（重置单行）；否则返回TError或0</returns>
        override public int Update709(bool pUpdateOK)
        {
            return TError.RunGood.Int(); // do nothing
        }

        /// <summary>
        /// 创建HttpReqXml_InventoryMonitoring对象
        /// </summary>
        /// <returns></returns>
        override public HttpReqXml_InventoryMonitoring NewRequestObj()
        {
            try
            {
                _productIdList = CWmsDataFactory.GetV_ProductIds((0 == ProductIdList?.Count) ? null : ProductIdList).ToList();
                if (0 == OwnerList?.Count) _ownerList = CWmsDataFactory.GetV_Owners().ToList();                
                if (0 == ProductIdList.Count || 0 == OwnerList.Count)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.NewRequestObject(), no productId or owner.", GetType());
                    return null;
                }

                var reqObj = new HttpReqXml_InventoryMonitoring();
                foreach (string id in ProductIdList)
                {
                    foreach (var owner in OwnerList)
                    {
                        var wList = CWmsDataFactory.GetV_Warehouse(owner);
                        if (!Utility.CWmsDataUtility.IEnumerableAny(wList))
                            C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.NewRequestObject(), owner[{1}] has no warehouse.", GetType(), owner);
                        else
                            reqObj.items.AddRange(wList.Select(w => new HttpReqXml_InventoryMonitoring_item(w.WmsCode, owner.WOwner.WmsID, id)));
                    }
                }
                return reqObj;
            }
            catch(Exception ex)
            {
                ProductIdList?.ForEach(id=>
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.NewRequestObject()  Exception: Id={1}", GetType(), id);
                });
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "{0}.NewRequestObject()发生异常", GetType());
                return null;
            }
        }

        /// <summary>
        /// 处理服务器响应
        /// </summary>
        /// <param name="respData">HTTP响应体</param>
        /// <returns>返回Response XML对应的数据实例</returns>
        override public HttpRespXml_InventoryMonitoring HandleResponse(byte[] respData)
        {
            try
            {
                return new HttpRespXml_InventoryMonitoring(Encoding.UTF8.GetString(respData));
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "HandleResponse(respData={0})发生异常", respData);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="canNull"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        protected int ParseArgument(object arg, bool canNull, out List<string> dest)
        {
            if (null == arg)
            {
                dest = (canNull) ? new List<string>(1) : null;
                return (canNull) ? TError.RunGood.Int() : TError.Post_NoParam.Int();
            } // doesn't sepecified the monited owner.

            try
            {
                int err = TError.RunGood.Int();
                if (arg is string)
                {
                    dest = (!string.IsNullOrEmpty(arg as string)) ? new List<string>(1) { arg as string } : new List<string>(1);
                }
                else if (arg is IEnumerable<string>)
                {
                    dest = (Utility.CWmsDataUtility.IEnumerableAny(arg as IEnumerable<string>)) ? (arg as IEnumerable<string>).ToList() : new List<string>(1);
                }
                else
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.ParseArgument(), unexpected arg={1}", GetType(), arg);
                    dest = null;
                    err = TError.Post_ParamError.Int();
                }
                return err;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "Exception in {0}.ParseArgument({1})", GetType(), arg);
                dest = null;
                return TError.Post_ParamError.Int();
            }
        }
    } // class CWmsInventoryMonitoringImpl
}
