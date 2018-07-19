using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C_WMS.Interface.CWms.Interfaces.Data;
using MangoMis.Frame.ThirdFrame;
using C_WMS.Data.Mango.MisModelPWI;
using MangoMis.Frame.Helper;
using C_WMS.Data.CWms.Interfaces.Data;

namespace C_WMS.Data.CWms.Interfaces.Methods
{
    /// <summary>
    /// 退货入库单创建接口的HTTP请求XML对应的序列化类
    /// </summary>
    public class CWmsReturnOrderCreate : CWmsPostTransactionBase
    {
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
    }
}
