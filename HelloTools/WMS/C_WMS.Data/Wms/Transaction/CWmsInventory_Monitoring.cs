using C_WMS.Data.CWms;
using C_WMS.Data.CWms.Interfaces.Methods;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.Wms.Data;
using C_WMS.Data.Wms.Transaction;
using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using System;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Transaction
{
#if C_WMS_V1
    /// <summary>
    /// 库存监控接口
    /// </summary>
    class CWmsInventory_Monitoring : CWmsPostTransactionBase<CWmsInventory_Monitoring, HttpRespXml_InventoryMonitoring>
    {
        /// <summary>
        /// overided。执行获取库存信息。该方法将从所有仓库获取库存信息。
        /// 若通讯成功获取到库存信息则返回WMS响应内容对应的数据对象，否则返回null
        /// </summary>
        /// <param name="pResp"></param>
        /// <param name="pMsg"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        override public int DoTransaction(out HttpRespXml_InventoryMonitoring pResp, out string pMsg, params object[] args)
        {
            int err = TError.RunGood.Int();
            string productId = string.Empty;
            if (TError.RunGood.Int() != (err = ParseArguments(args)))
            {
                pMsg = string.Format("int DoTransaction(out HttpRespXmlBase, out string, params object[])失败，非法入参。\r\n{0}", Utility.CWmsDataUtility.GetDebugInfo_Args(args));
                pResp = null;
                C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
            }
            else
            {
                var reqBody = new HttpReqXml_InventoryMonitoring();
                var wList = CWmsDataFactory.GetV_Wareshouse(); // 获取所有仓库
                foreach (var warehouse in wList)
                {
                    var owner = CWmsDataFactory.GetOwner(warehouse);
                    var item = new HttpReqXml_InventoryMonitoring_item(warehouse.WarehouseCode, owner.WOwner.WmsID, productId);
                    reqBody.inventoryMonitoringList.Add(item);
                }

                if (null != (pResp = Post(reqBody)) || pResp.IsSuccess())
                {
                    pMsg = string.Empty;
                    err = TError.RunGood.Int();
                }
                else
                {
                    pMsg = string.Format("从WMS获取商品{0}的库存信息失败. \r\n请求URL： {1}\r\n请求内容：{2}\r\n 响应内容：{3}", productId, TransParams.URI, reqBody, pResp);
                    C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
                    err = TError.Pro_HaveNoData.Int();
                }
            }
            return err;
        }

        /// <summary>
        /// 执行获取库存信息。
        /// 该方法将从所有仓库获取库存信息。
        /// </summary>
        /// <param name="product">被监控的商品</param>
        /// <param name="warehouse">商品所在仓库</param>
        /// <returns></returns>
        public HttpRespXmlBase DoTransaction(CWmsProduct product, string warehouse)
        {
            C_WMS.Data.Utility.MyLog.Instance.Info("获取商品{0}库存信息开始", product?.ItemCode);
            Wms.Data.WmsOwner owner = CWmsDataFactory.GetDefaultOwner().WOwner;

            HttpReqXmlBase reqBody = new HttpReqXml_InventoryMonitoring();

            (reqBody as HttpReqXml_InventoryMonitoring).inventoryMonitoringList = new List<HttpReqXml_InventoryMonitoring_item>();
            (reqBody as HttpReqXml_InventoryMonitoring).inventoryMonitoringList.Add(new
                 HttpReqXml_InventoryMonitoring_item(warehouse, owner.WmsID, product.ItemCode));
            var resp = Post(reqBody);
            C_WMS.Data.Utility.MyLog.Instance.Info("获取商品{0}库存信息结束", product?.ItemCode);
            return resp;
        }

        /// <summary>
        /// 处理服务器响应
        /// </summary>
        /// <param name="respBody">HTTP响应体</param>
        /// <param name="encode">HTTP响应Header中ContentEncoding对应的编码格式</param>
        /// <returns>返回Response XML对应的数据实例</returns>
        override public HttpRespXmlBase HandleResponse(byte[] respBody, Encoding encode)
        {
            var ret = new ThirdResult<List<object>>("库存监控，处理服务器响应");

            if (null == encode) throw new ArgumentNullException("库存监控，处理服务器响应。无效的Encoding对象");
            if (null == respBody)
            {
                ret.Append("库存监控，处理服务器响应。ResponseBody为空。");
                ret.End();
                return null;
            }

            HttpRespXml_InventoryMonitoring respObj = new HttpRespXml_InventoryMonitoring();
            string respXml = encode.GetString(respBody);
            respObj = CWmsUtility.ObjtoXml(respObj.GetType(), respXml) as HttpRespXml_InventoryMonitoring;

            ret.Append("库存监控，处理服务器响应完成");
            ret.End();
            return respObj;
        }

        /// <summary>
        /// 执行接口inventory.monitoring的HTTP Transaction. 
        /// CWmsInventory_Monitoring不支持使用HttpRespXmlBase DoTransaction()方法
        /// </summary>
        /// <returns></returns>
        override public HttpRespXmlBase DoTransaction()
        {
            throw new NotImplementedException(string.Format("{0}不支持使用HttpRespXmlBase DoTransaction()方法", GetType()));
        }

        /// <summary>
        /// CWmsInventory_Monitoring不支持使用void RunL(object, EventArgs)方法
        /// </summary>
        /// <returns></returns>
        protected override void RunL(object sender, EventArgs args)
        {
            throw new NotImplementedException("CWmsInventory_Monitoring不支持使用void RunL(object, EventArgs)方法");
        }
    }
#else
    class CWmsInventoryMonitoring : MWmsTransactionBase<CWmsInventoryMonitoring, HttpReqXml_InventoryMonitoring, HttpRespXml_InventoryMonitoring>
    {
        protected override IMWmsTransactionImpl<HttpReqXml_InventoryMonitoring, HttpRespXml_InventoryMonitoring> InitImpl()
        {
            return new CWmsInventoryMonitoringImpl();
        }
    }

    /// <summary>
    /// 库存监控接口的Impl类。
    /// 针对一个MIS中的商品，应监控其在WMS系统做所有货主及各货主下所有仓库中的库存
    /// </summary>
    class CWmsInventoryMonitoringImpl : CWmsTransactionImplBase<HttpReqXml_InventoryMonitoring, HttpRespXml_InventoryMonitoring>
    {
        ///// <summary>
        ///// 获取和设置库存监控通讯的Request数据
        ///// </summary>
        //override public HttpReqXml_InventoryMonitoring RequestObject { get; set; }

        ///// <summary>
        ///// 获取和设置库存监控的Response数据
        ///// </summary>
        //override public HttpRespXml_InventoryMonitoring ResponseObject { get; set; }

        /// <summary>
        /// 判断是否从WMS系统响应成功
        /// </summary>
        /// <param name="pResp"></param>
        override public bool TransactionIsSuccess(HttpRespXml_InventoryMonitoring pResp)
        { return (null == pResp) ? false : pResp.IsSuccess(); }

    /// <summary>
    /// 获取或设置带查询的（WMS中的）商品编码
    /// </summary>
    string ProductId { get; set; }

        /// <summary>
        /// 返回API接口的method
        /// </summary>
        /// <returns></returns>
        override public string GetApiMethod()
        {
            return CWmsMisSystemParamCache.Cache.ApiMethod_InventoryMonitoring.PValue; // get method name from SystemParam
        }

        /// <summary>
        /// 针对int DoTransaction(out HttpRespXmlBase, out string, params object[])和TResponse DoTransaction(params object[])方法，解析params object[]入参。
        /// 参数应该是一个商品在WMS系统中的商品编码
        /// 若解析成功则返回TError.RunGood，并将商品编码保存在ProductId中；否则返回其他值
        /// </summary>
        /// <param name="args">带解析的参数</param>
        /// <returns>若解析成功则返回TError.RunGood；否则返回其他值</returns>
        override public int ParseArguments(params object[] args)
        {
            try
            {
                ProductId = args[0].ToString();
                return TError.RunGood.Int();
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "ParseArguments({0})发生异常.\r\nDEBUG INFO: {1}", args, Utility.CWmsDataUtility.GetDebugInfo_Args(args));
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
        /// 根据同步通讯的结果更新709字典中对应的行的IsUpdateOK的值。
        /// 若重置成功则影响的行数（重置多行）或影响行的主键（重置单行）；否则返回TError或0.
        /// 库存监控接口属于获取类操作，无需操作709字典。
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
            var owners = CWmsDataFactory.GetV_Owners();
            var tmpWList = owners.Select(owner => CWmsDataFactory.GetV_Warehouse(owner)); // IEnumerable<IEnumerable<CWmsWarehouse>>

            var retObj = new HttpReqXml_InventoryMonitoring();
            foreach (var owner_warehouses in tmpWList)
            {
                retObj.inventoryMonitoringList.AddRange(owner_warehouses.Select(w => new HttpReqXml_InventoryMonitoring_item(w.WmsCode, CWmsDataFactory.GetOwner(w).WOwner.WmsID, ProductId)));
            }
            return retObj;
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
    }
#endif
}
