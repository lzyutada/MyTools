using System;
using System.Collections.Generic;
using System.Text;
using C_WMS.Interface.CWms.Interfaces.Data;
using MangoMis.Frame.ThirdFrame;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.CWms.CWmsEntity;
using C_WMS.Data.Wms.Data;
using C_WMS.Data.Wms.Transaction;
using C_WMS.Data.CWms.Interfaces.Methods;

namespace C_WMS.Data.Wms.Transaction
{
    /// <summary>
    /// 同步出库单接口通讯类
    /// </summary>
    class CWmsStockoutCreate : MWmsTransactionBase<CWmsStockoutCreate, HttpReqXml_StockoutOrderCreate, HttpRespXml_StockoutOrderCreate>
    {
#if C_WMS_V1
        /// <summary>
        /// 缓存当前创建的主出库订单ID
        /// </summary>
        private string mCachedOrderId = string.Empty;

        /// <summary>
        /// implement base.DoTransaction(). 
        /// 应该调用HttpRespXmlBase CWmsStockoutCreate.DoTransaction(params object[] args)来完成HTTP会话
        /// </summary>
        /// <returns>返回存储了HTTP response body的HttpRespXmlBase实例</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override HttpRespXmlBase DoTransaction()
        {
            throw new NotImplementedException("应该调用HttpRespXmlBase CWmsStockoutCreate.DoTransaction(params object[] args)");
        }

        /// <summary>
        /// 执行HTTP会话。
        /// </summary>
        /// <param name="args">重载方法应传入至少2个参数。且第一个输入参数为string类型的商城系统中的主出库单ID</param>
        /// <returns>返回存储了HTTP response body的HttpRespXmlBase实例</returns>
        public HttpRespXmlBase DoTransaction(params object[] args)
        {
            var ret = new ThirdResult<List<object>>("CWmsStockoutCreate.DoTransaction()开始");

            HttpReqXmlBase reqBody = null; // new HttpReqXml_InventoryMonitoring();请求
            HttpRespXmlBase respBody = null;//响应
            int addTo709Rows = 0;//添加到709中的数据
            string errMsg = string.Empty;//错误信息

        #region 从入参中获取单据ID
            try { mCachedOrderId = args[0] as string; }//传递值转换成string 失败就抛出异常
            catch (Exception ex)
            {
                ret.Append(string.Format("发生异常：{0}", ex.Message));
                ret.End();
                return null;
            }
        #endregion
            try
            {
                var cwmsOrder = CWmsStockoutOrderHandler.NewOrder(mCachedOrderId);
                cwmsOrder.Handler.UpdateDict709(cwmsOrder, TDict285_Values.EDeleted, TDict285_Values.ENormal, true);
                // update Dit[709]
                if (0 >= Dict709Handle.UpdateRow_StockoutCreate(mCachedOrderId, TDict285_Values.EDeleted, TDict285_Values.ENormal, out addTo709Rows, out errMsg))
                {
                    ret.Append(string.Format("CWmsStockoutCreate.DoTransaction()结束，向Dict[709]插入创建出库订单行失败:{0}, {1}", addTo709Rows, errMsg));
                    ret.End();
                    return null;
                }//提交709数据

                // generate Http request body
                if (null == (reqBody = CWmsDataFactory.GetReqXmlBody_ExWarehouseCreate(mCachedOrderId)))
                {
                    ret.Append(string.Format("CWmsStockoutCreate.DoTransaction()，创建请求体对象失败"));
                    ret.End();
                    return null;
                }

                respBody = Post(reqBody);   // Do Http transaction
                ret.Append(string.Format("ResponseXML={0}", respBody.ToString()));
                // 处理response body
                if (null == respBody)
                {
                    // update Dict[709]
                    Dict709Handle.UpdateRow_StockoutCreate(mCachedOrderId, TDict285_Values.ENormal, TDict285_Values.ENormal, out addTo709Rows, out errMsg);
                    ret.Append(string.Format("CWmsStockoutCreate.DoTransaction()结束"));
                    // TODO: 返回失败
                    ret.Append(string.Format("失败，ResponseXML对象为null"));
                    ret.End();

                    return null;
                }
                else if (!respBody.IsSuccess())
                {
                    // TODO: 返回失败
                    ret.Append(string.Format("失败，ResponseXML对象flag=failure"));
                    ret.End();
                    return respBody;
                }
                else
                {
                    Dict709Handle.UpdateRow_StockoutCreate(mCachedOrderId, TDict285_Values.ENormal, TDict285_Values.ENormal, out addTo709Rows, out errMsg);
                    ret.Append(string.Format("CWmsStockoutCreate.DoTransaction()结束"));
                    ret.End();
                    return respBody;
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
        /// 返回接口API名称
        /// </summary>
        /// <returns>返回表示接口API名称的字符串</returns>
        public override string GetApiMethod()
        {
            return "stockout.create";
        }

        /// <summary>
        /// 重载, 处理HTTP Response Body
        /// </summary>
        /// <param name="respBody"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public override HttpRespXmlBase HandleResponse(byte[] respBody, Encoding encode)
        {
            //int rslt = 0;
            string errMsg = string.Empty;

            // validate parameters
            if (null == encode || null == respBody)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsStockoutCreate.{0}, 非法的输入参数respBody={1}, encode={2}", System.Reflection.MethodBase.GetCurrentMethod().Name, respBody, encode);
                return null;
            }
            else
            {
                try
                {
                    var respXml = encode.GetString(respBody);
                    HttpRespXmlBase retObj = new HttpRespXml_StockoutOrderCreate(respXml);
                    return retObj;
                }
                catch (Exception ex)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error(ex, "CWmsStockoutCreate.{0}发生异常. respBody={1}, encode={2}", System.Reflection.MethodBase.GetCurrentMethod().Name, respBody, encode);
                    return null;
                }
            }
        }

        public override int DoTransaction(out HttpRespXmlBase pResp, out string pMsg, params object[] args)
        {
            throw new NotImplementedException();
        }

        protected override void RunL(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
#else
        protected override IMWmsTransactionImpl<HttpReqXml_StockoutOrderCreate, HttpRespXml_StockoutOrderCreate> InitImpl()
        {
            return new CWmsStockoutCreateImpl();
        }
#endif
    }

    class CWmsStockoutCreateImpl : CWmsOrderTransactionImpl<HttpReqXml_StockoutOrderCreate, HttpRespXml_StockoutOrderCreate>
    {
        /// <summary>
        /// 返回API接口的method
        /// </summary>
        /// <returns></returns>
        public override string GetApiMethod()
        {
            return CWmsMisSystemParamCache.Cache.ApiMethod_StockoutOrderCreate?.PValue;
        }

        /// <summary>
        /// 在执行同步操作之前，先重置709字典中对应的行，将IsUpdateOK的值置为同步失败。
        /// </summary>
        /// <returns></returns>
        public override int Reset709()
        {
            var order = CWmsStockoutOrderHandler.NewOrder(OrderId);
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
            var order = CWmsStockoutOrderHandler.NewOrder(OrderId);
            return order.Handler.UpdateDict709(order, (pUpdateOK) ? TDict285_Values.ENormal : TDict285_Values.EDeleted, TDict285_Values.EUnknown, false);
        }

        /// <summary>
        /// 创建HttpReqXml_StockoutOrderCreate对象
        /// </summary>
        /// <returns></returns>
        public override HttpReqXml_StockoutOrderCreate NewRequestObj()
        {
            return new HttpReqXml_StockoutOrderCreate(CWmsStockoutOrderHandler.NewOrder(OrderId));
        }

        /// <summary>
        /// 处理服务器响应
        /// </summary>
        /// <param name="respData">HTTP响应体</param>
        /// <returns>返回Response XML对应的数据实例</returns>
        public override HttpRespXml_StockoutOrderCreate HandleResponse(byte[] respData)
        {
            if (null == respData)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsStockoutCreateImpl.HandleResponse(), 传入空引用参数byte[]");
                return null;
            }
            else
            {
                try { return new HttpRespXml_StockoutOrderCreate(Encoding.UTF8.GetString(respData)); }
                catch (Exception ex)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error(ex, "CWmsStockoutCreateImpl.HandleResponse()发生异常. Response Length={0}", respData.Length);
                    return null;
                }
            }
        }
    }
}
