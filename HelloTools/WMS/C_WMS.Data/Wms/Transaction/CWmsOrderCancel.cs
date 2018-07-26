using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C_WMS.Interface.CWms.Interfaces.Data;
using MangoMis.Frame.ThirdFrame;
using MangoMis.Frame.Helper;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.Wms.Data;

namespace C_WMS.Data.CWms.Interfaces.Methods
{
    /// <summary>
    /// 单据取消接口的HTTP请求XML对应的序列化类
    /// </summary>
    class CWmsOrderCancel : MWmsTransactionBase<CWmsOrderCancel, HttpReqXml_OrderCacnel, HttpRespXmlBase>
    {
#if C_WMS_V1
        /// <summary>
        /// 执行HTTP会话取消单据
        /// </summary>
        /// <param name="pResp">返回HTTP Response XML实体</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <param name="args">输入参数，应为主单据Id和单据类型</param>
        /// <returns>返回存储了HTTP response body的HttpRespXmlBase实例</returns>
        public override int DoTransaction(out HttpRespXmlBase pResp, out string pMsg, params object[] args)
        {
            var ret = new ThirdResult<List<object>>("同步取消单据HTTP会话，开始");

            // temp variables
            pMsg = string.Empty;
            pResp = null;
            int errCode = TError.RunGood.Int();
            string moid = string.Empty;          // 待取消的主单据Id
            TCWmsOrderCategory mcoc = TCWmsOrderCategory.EUnknownCategory;
            //TDict709_Value mapClassId = TDict709_Value.EUnknown;

            // handle arguments
            if (TError.RunGood.Int() != (errCode = ParseArguments(out moid, out mcoc, args)))
            {
                pMsg = "params object[] args，非法入参";
                ret.Append(pMsg);
                ret.End();
                return errCode;
            }
            
            // Do HTTP transaction
            errCode = DoTransaction(Dict709Handle.Dict709FromMoc(mcoc), moid, out pResp, out pMsg);
            
            ret.Append("同步取消单据HTTP会话，结束");
            ret.End();
            return errCode;
        }

        /// <summary>
        /// 执行HTTP会话。废弃的方法，使用DoTransaction(out HttpRespXmlBase, out string, params object[]
        /// </summary>
        /// <returns></returns>
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
            return "order.cancel";
        }

        /// <summary>
        /// inner method. 执行HTTP会话
        /// </summary>
        /// <param name="pMapClassId">Dict[709].MapClassId</param>
        /// <param name="moid">待取消的主单据Id</param>
        /// <param name="pResp">返回HTTP Response XML实体</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>返回存储了HTTP response body的HttpRespXmlBase实例</returns>
        /// <returns></returns>
        protected int DoTransaction(TDict709_Value pMapClassId, string moid, out HttpRespXmlBase pResp, out string pMsg)
        {
            var ret = new ThirdResult<List<object>>(string.Format("同步取消单据HTTP会话，开始, MapClassId={0}, modi{1}", pMapClassId, moid));

            try
            {
        #region temp variables
                pMsg = string.Empty;
                pResp = null;
                int errCode = TError.Post_ParamError.Int(); // 方法返回值
                int errDict709 = TError.WCF_RunError.Int();
                string errMsg = string.Empty;
                List<MisModel.Product_WMS_Interface> d709fList = null;  // 操作Dict709失败时的，缓存操作失败的行的列表
                HttpReqXmlBase reqBody = null;      // HTTP请求XML
                HttpRespXmlBase respBody = null;    // HTTP响应XML
        #endregion
        #region validate arguments
                if (TDict709_Value.ECancelReturnOrder != pMapClassId)
                {
                    pMsg = string.Format("不支持的单据类型, pMapClassId={0}", pMapClassId);
                    ret.Append(pMsg);
                    ret.End();
                    return errCode;
                }
                // TODO: other arguments validations
        #endregion
        #region Update Dict[709]
                errCode = Dict709Handle.UpdateRowVA_Order(pMapClassId, moid, TDict285_Values.EDeleted, TDict285_Values.ENormal, out d709fList, out errDict709, out pMsg);
                if (TError.WCF_RunError.Int() == errCode)
                {
                    errMsg = string.Format("更新Dict[709]失败:errCode={0}, errDict709={1}, pMsg={2}", errCode, errDict709, pMsg);
                    ret.Append(string.Format("同步取消单据HTTP会话失败， {0}", errMsg));
                    ret.End();
                    return errCode;
                }
        #endregion

                reqBody = CWmsDataFactory.GetReqXmlBody_CancelOrder(Dict709Handle.Dict709ToMoc(pMapClassId), moid) as HttpReqXml_OrderCacnel; // get HTTP request body
                respBody = Post(reqBody); // Do http transaction

        #region handle response
                if (null == pResp)
                {
                    pMsg = "HTTP会话异常，HTTP响应体为null";
                    errCode = TError.WCF_RunError.Int();
                    ret.Append(string.Format("同步取消单据HTTP会话失败，系统异常:errCode={0}， message={1}", errCode, pMsg));
                }
                else if (!pResp.IsSuccess())
                {
                    pMsg = pResp.ToString();
                    errCode = pResp.code.Int();
                    ret.Append(string.Format("同步取消单据HTTP会话失败，响应失败: errCode={0}， message={1}", errCode, pMsg));
                }
                else
                {
                    // update Dict[709]
                    errCode = Dict709Handle.UpdateRowVA_Order(pMapClassId, moid, TDict285_Values.EDeleted, TDict285_Values.ENormal, out d709fList, out errDict709, out pMsg);
                    ret.Append(string.Format("同步取消单据HTTP会话成功:{0}, {1}", errCode, pMsg));
                }
        #endregion

                ret.End();
                return errCode;
            }
            catch (Exception ex)
            {
                pResp = null;
                pMsg = ex.Message;

                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return TError.WCF_RunError.Int();
            }
        }

        /// <summary>
        /// 解析接口通讯会话方法DoTransaction的入参
        /// ！args中第一个为待取消的主单据Id、第二个为待取消的单据类型TMangoCancelOrderCategory
        /// </summary>
        /// <param name="pId">返回待取消的主单据Id</param>
        /// <param name="pMcoc">返回待取消的单据类型TMangoCancelOrderCategory</param>
        /// <param name="args">传递给DoTransaction方法的入参</param>
        /// <returns>若成功则返回TError.RunGood; 否则返回其他错误码</returns>
        int ParseArguments(out string pId, out TCWmsOrderCategory pMcoc, params object[] args)
        {
            pId = string.Empty;
            pMcoc = TCWmsOrderCategory.EUnknownCategory;

            if (null == args || 2 > args.Length)
            {
                return TError.Post_ParamError.Int();
            }
            pId = args[0] as string;
            pMcoc = (TCWmsOrderCategory)args[1];

            return TError.RunGood.Int();
        }

        protected override void RunL(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
#else
        public CWmsOrderCancel() { throw new NotImplementedException(); }
#endif
    }
}
