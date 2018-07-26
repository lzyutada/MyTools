using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.Wms.Data;
using C_WMS.Data.CWms.Interfaces.Methods;
using C_WMS.Data.CWms.CWmsEntity;

namespace C_WMS.Data.Wms.Transaction
{
    /// <summary>
    /// 同步退货入库单的接口通讯类
    /// </summary>
    class CWmsReturnOrderCreate : MWmsTransactionBase<CWmsReturnOrderCreate, HttpReqXml_ReturnOrderCreate, HttpRespXml_ReturnOrderCreate>
    {
#if C_WMS_V1
        /// <summary>
        /// 执行接口通讯会话
        /// </summary>
        /// <param name="pResp">返回HTTP Response XML实体</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <param name="args">输入参数，应为主退货订单Id</param>
        /// <returns></returns>
        public override int DoTransaction(out HttpRespXmlBase pResp, out string pMsg, params object[] args)
        {
            var ret = new ThirdResult<List<object>>("同步创建退货订单HTTP会话，开始");
            try
            {

        #region temp variables
                pMsg = string.Empty;
                pResp = null;
                int errCode = TError.RunGood.Int();
                int errDict709 = TError.WCF_RunError.Int();
                string errMsg = string.Empty;
                List<MisModel.Product_WMS_Interface> d709fList = null;  // 操作Dict709失败时的，缓存操作失败的行的列表
                string rid = string.Empty;          // 主退货订单ID
                HttpReqXmlBase reqBody = null;      // HTTP请求XML
                //HttpRespXmlBase respBody = null;    // HTTP响应XML
        #endregion
        #region handle arguments
                if (TError.RunGood.Int() != (errCode = ParseArguments(out rid, args)))
                {
                    errMsg = "params object[] args，非法入参";
                    ret.Append(errMsg);
                    ret.End();
                    return errCode;
                }
        #endregion
        #region Update Dict[709]
                errCode = Dict709Handle.UpdateRowVA_Order(TDict709_Value.EReturnOrder, rid, TDict285_Values.EDeleted, TDict285_Values.ENormal, out d709fList, out errCode, out pMsg);
                if (TError.WCF_RunError.Int() == errCode)
                {
                    errMsg = string.Format("CWmsStockoutCreate.DoTransaction()结束，更新Dict[709]失败:errCode={0}, errDict709={1}, errMsg={2}", errCode, errDict709, errMsg);
                    ret.Append(errMsg);
                    ret.End();
                    return errCode;
                }
        #endregion
        #region get HTTP request body
                reqBody = CWmsDataFactory.GetReqXmlBody_ReturnOrderCreate(rid) as HttpReqXml_ReturnOrderCreate;
        #endregion
        #region Do http transaction
                pResp = Post(reqBody);
        #endregion
        #region handle response
                if (null == pResp)
                {
                    pMsg = "HTTP会话异常，HTTP响应体为null";
                    errCode = TError.WCF_RunError.Int();
                    ret.Append(string.Format("响应失败:errCode={0}， message={1}", errCode, pMsg));
                }
                else if (!pResp.IsSuccess())
                {
                    pMsg = pResp.ToString();
                    errCode = pResp.code.Int();
                    ret.Append(string.Format("响应失败:errCode={0}， message={1}", errCode, pMsg));
                }
                else {
                    // update Dict[709]
                    errCode = Dict709Handle.UpdateRowVA_Order(TDict709_Value.EReturnOrder, rid, TDict285_Values.ENormal, TDict285_Values.ENormal, out d709fList, out errCode, out pMsg);
                    ret.Append(string.Format("响应成功:{0}, {1}", errCode, pMsg));
                }
        #endregion
                ret.End();
                return errCode;
            }
        #region Handle exception
            catch (Exception ex)
            {
                pResp = null;
                pMsg = ex.Message;

                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return TError.WCF_RunError.Int();
            }
        #endregion
        }

        /// <summary>
        /// 执行HTTP会话
        /// </summary>
        /// <returns>返回存储了HTTP response body的HttpRespXmlBase实例</returns>
        public override HttpRespXmlBase DoTransaction()
        {
            throw new NotImplementedException("不要调用该方法，使用DoTransaction(out HttpRespXmlBase, out string, params object[]");
        }


        /// <summary>
        /// 返回接口API名称
        /// </summary>
        /// <returns>返回表示接口API名称的字符串</returns>
        public override string GetApiMethod()
        {
            return "returnorder.create";
        }

        /// <summary>
        /// 处理服务器响应
        /// </summary>
        /// <param name="respBody">HTTP响应体</param>
        /// <param name="encode">服务器响应回来的编码格式所对应的Encoding</param>
        /// <returns>返回Response XML对应的数据实例</returns>
        public override HttpRespXmlBase HandleResponse(byte[] respBody, Encoding encode)
        {
            if (null == respBody)
            {
                return null;
            }
            else
            {
                HttpRespXmlBase retObj = new HttpRespXml_ReturnOrderCreate(encode.GetString(respBody));
                return retObj;
            }
        }

        /// <summary>
        /// 解析接口通讯会话方法DoTransaction的入参
        /// ！args中第一个为主退货订单
        /// </summary>
        /// <param name="pId">返回主退货订单id</param>
        /// <param name="args">传递给DoTransaction方法的入参</param>
        /// <returns>若成功则返回TError.RunGood; 否则返回其他错误码</returns>
        int ParseArguments(out string pId, params object[] args)
        {
            pId = string.Empty;

            if (null == args || 1 > args.Length)
            {
                return TError.Post_ParamError.Int();
            }
            pId= args[0] as string;

            return TError.RunGood.Int();
        }

        protected override void RunL(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
#else
        protected override IMWmsTransactionImpl<HttpReqXml_ReturnOrderCreate, HttpRespXml_ReturnOrderCreate> InitImpl()
        {
            return new CWmsReturnOrderCreateImpl();
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    class CWmsReturnOrderCreateImpl : CWmsOrderTransactionImpl<HttpReqXml_ReturnOrderCreate, HttpRespXml_ReturnOrderCreate>
    {
        /// <summary>
        /// id of the sychronizing returnorder
        /// </summary>
        public string ReturnOrderId { get; protected set; }

        /// <summary>
        /// 返回API接口的method
        /// </summary>
        /// <returns></returns>
        public override string GetApiMethod()
        {
            return CWmsMisSystemParamCache.Cache.ApiMethod_ReturnOrderCreate?.PValue;
        }
        
        /// <summary>
        /// 在执行同步操作之前，先重置709字典中对应的行，将IsUpdateOK的值置为同步失败。
        /// </summary>
        /// <returns></returns>
        public override int Reset709()
        {
            var order = CWmsReturnOrderHandler.NewOrder(OrderId);
            return order.Handler.UpdateDict709(order, TDict285_Values.EDeleted, TDict285_Values.EUnknown, true);
        }

        /// <summary>
        /// 根据同步通讯的结果更新709字典中对应的行的IsUpdateOK的值。
        /// 若重置成功则影响的行数（重置多行）或影响行的主键（重置单行）；否则返回TError或0.
        /// </summary>
        /// <param name="pUpdateOK">value of 709.isUpdateOK</param>
        /// <returns></returns>
        public override int Update709(bool pUpdateOK)
        {
            var order = CWmsReturnOrderHandler.NewOrder(OrderId);
            return order.Handler.UpdateDict709(order, (pUpdateOK) ? TDict285_Values.ENormal : TDict285_Values.EDeleted, TDict285_Values.EUnknown, false);
        }

        /// <summary>
        /// 创建HttpReqXml_InventoryMonitoring对象
        /// </summary>
        /// <returns></returns>
        public override HttpReqXml_ReturnOrderCreate NewRequestObj()
        {
            return new HttpReqXml_ReturnOrderCreate(CWmsReturnOrderHandler.NewOrder(OrderId));
        }

        /// <summary>
        /// 处理服务器响应
        /// </summary>
        /// <param name="respData">HTTP响应体</param>
        /// <returns>返回Response XML对应的数据实例</returns>
        public override HttpRespXml_ReturnOrderCreate HandleResponse(byte[] respData)
        {
            if (null == respData)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsReturnOrderCreateImpl.HandleResponse(), 传入空引用参数byte[]");
                return null;
            }
            else
            {
                try { return new HttpRespXml_ReturnOrderCreate(Encoding.UTF8.GetString(respData)); }
                catch (Exception ex)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error(ex, "CWmsReturnOrderCreateImpl.HandleResponse()发生异常. Response Length={0}", respData.Length);
                    return null;
                }
            }
        }
    }
}
