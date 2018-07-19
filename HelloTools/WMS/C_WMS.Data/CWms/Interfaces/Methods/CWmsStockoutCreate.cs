using System;
using System.Collections.Generic;
using System.Text;
using C_WMS.Interface.CWms.Interfaces.Data;
using MangoMis.Frame.ThirdFrame;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.CWms.Interfaces.Data;
//using MisModel;
//using MangoMis.Frame.Helper;

namespace C_WMS.Data.CWms.Interfaces.Methods
{
    /// <summary>
    /// 出库单创建接口的HTTP请求XML对应的序列化类
    /// </summary>
    public class CWmsStockoutCreate : CWmsPostTransactionBase
    {
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
                var cwmsOrder = CWmsEntity.CWmsStockoutOrderHandler.NewOrder(mCachedOrderId);
                cwmsOrder.Handler.UpdateDict709(cwmsOrder, TDict285_Values.EDeleted, TDict285_Values.ENormal, true);
#if false
                foreach (var subOrder in cwmsOrder.SubOrders)
                {
#if false
                    var pwiEntity = Dict709Handle.NewPwiEntity(TDict709_Value.EExwarehouseOrder, cwmsOrder.Id, subOrder.Value.Id, TDict285_Values.EDeleted, TDict285_Values.ENormal);
                    int err = Dict709Handle.UpdateRowA(pwiEntity, out addTo709Rows, out errMsg);
#else
                    DefDlgt_WcfExcecution<Product_WMS_Interface>
#endif
                }
#endif
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
    }
}
