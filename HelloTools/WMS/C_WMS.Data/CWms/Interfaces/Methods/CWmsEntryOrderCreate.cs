using System;
using System.Collections.Generic;
using System.Text;
using C_WMS.Interface.CWms.Interfaces.Data;
using MangoMis.Frame.ThirdFrame;
using MangoMis.Frame.Helper;
using C_WMS.Data.Mango.MisModelPWI;
using MangoMis.MisFrame.Frame;
using C_WMS.Interface.Utility;
using C_WMS.Data.CWms.Interfaces.Data;

namespace C_WMS.Data.CWms.Interfaces.Methods
{
    /// <summary>
    /// 入库单创建接口通讯类
    /// </summary>
    public class CWmsEntryOrderCreate : CWmsPostTransactionBase
    {
        const string DingDingMsgFmtSuccess = "商城提醒：有一条新的入库申请，入库订单编号为：[{0}]。已在仓储管理系统中成功同步创建采购入库订单[客户订单编号为：{0}]，请及时处理。来自后台。";
        const string DingDingMsgFmtFailed = "商城提醒：有一条新的入库申请，入库订单编号为：[{0}]。在向仓储管理系统中同步创建采购入库订单时失败，请及时处理,来自钉钉审批。";

        /// <summary>
        /// 重载CWmsPostTransactionBase.DoTransaction()。
        /// 不要调用该方法，使用HttpRespXmlBase CWmsPostTransactionBase.DoTransaction(params object[] args)
        /// </summary>
        /// <returns></returns>
        public override HttpRespXmlBase DoTransaction()
        {
            throw new NotImplementedException("不要调用该方法，使用Hint DoTransaction(out HttpRespXmlBase pResp, out string pMsg, params object[] args)");
        }

        /// <summary>
        /// 执行接口的HTTP Transaction，该方法可以传入参数，返回HTTP Response Body、错误码和错误描述。需要在子类中实现
        /// </summary>
        /// <param name="pResp">返回HTTP Response Body</param>
        /// <param name="pMsg">错误描述，若返回成功则返回String.Empty</param>
        /// <param name="args">传入参数</param>
        /// <returns>返回错误码TError</returns>
        public override int DoTransaction(out HttpRespXmlBase pResp, out string pMsg, params object[] args)
        {
            try
            {
                #region temp variables
                pMsg = string.Empty;
                pResp = null;
                int errCode = TError.RunGood.Int();
                int errDict709 = TError.WCF_RunError.Int();
                string errMsg = string.Empty;
                string ddMsg = string.Empty;
                List<MisModel.Product_WMS_Interface> d709fList = null;
                string eid = string.Empty;  // 主入库订单ID
                string pid = string.Empty;  // 主采购单Id
                HttpReqXml_EntryOrderCreate reqBody = null;//请求xml
                #endregion
                #region handle arguments
                if (TError.RunGood.Int() != (errCode = ParseArguments(out eid, out pid, args)))
                {
                    errMsg = "params object[] args，非法入参";
                    C_WMS.Data.Utility.MyLog.Instance.Error("执行同步入库订单的HTTP会话失败 errCode={0}, msg={1}", errMsg, errMsg);
                    return errCode;
                }
                #endregion
                #region Update Dict[709]
                var order = CWmsEntity.CWmsEntryOrderHandler.NewOrder(eid);
                order.Handler.UpdateDict709(order, TDict285_Values.EDeleted, TDict285_Values.EUnknown, true);
#if false
                if (TError.WCF_RunError.Int() == 
                    (errCode = Dict709Handle.UpdateRowVA_Order(TDict709_Value.EEntryOrder, eid, TDict285_Values.EDeleted, TDict285_Values.ENormal, out d709fList, out errCode, out pMsg)))
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("执行同步入库订单的HTTP会话，更新709失败: 入库单Id={0}, errCode={1}, errDict709={2}, errMsg={3}", eid, errCode, errDict709, errMsg);
                    return errCode;
                }
#endif
                #endregion

                reqBody = CWmsDataFactory.GetReqXmlBody_EntryOrderCreate(eid, pid) as HttpReqXml_EntryOrderCreate; // get HTTP request body
                pResp = Post(reqBody); // Do http transaction

                #region handle response
                if (null == pResp)
                {
                    pMsg = "HTTP会话异常，HTTP响应体为null";
                    errCode = TError.WCF_RunError.Int();
                    C_WMS.Data.Utility.MyLog.Instance.Error("执行同步入库订单的HTTP会话，服务器响应异常, message={0}", pMsg);
                }
                else if (!pResp.IsSuccess())
                {
                    pMsg = pResp.message;
                    errCode = TError.Ser_ErrorPost.Int();
                    C_WMS.Data.Utility.MyLog.Instance.Error("执行同步入库订单的HTTP会话，服务器响应失败，message={0}", pMsg);

                }
                else
                {
                    errCode = Dict709Handle.UpdateRowVA_Order(TDict709_Value.EEntryOrder, eid, TDict285_Values.ENormal
                        , TDict285_Values.ENormal, out d709fList, out errCode, out pMsg);
                    C_WMS.Data.Utility.MyLog.Instance.Info("执行同步入库订单的HTTP会话完成，更新[709]:errCode={0}, errCode={1}, pMsg={2}", errCode, errCode, pMsg);
                }
                #endregion

                #region 推送钉钉通知
                ddMsg = (0 < errCode) ? ddMsg = string.Format(DingDingMsgFmtSuccess, eid) : ddMsg = string.Format(DingDingMsgFmtFailed, eid);
                mDlgtNotifySyncResult.BeginInvoke(CommonFrame.LoginUser.UserId.Int(), ddMsg, Acb_NotifySyncResult, mDlgtNotifySyncResult);
                #endregion

                return errCode;
            }
            #region Handle exception
            catch (Exception ex)
            {
                pResp = null;
                pMsg = ex.Message;
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
                return TError.WCF_RunError.Int();
            }
            #endregion
        }

        /// <summary>
        /// 处理服务器响应
        /// </summary>
        /// <param name="respBody">HTTP响应体</param>
        /// <param name="encode">符合HTTP response header中ContentEncoding多对应的编码类型实例</param>
        /// <returns>返回Response XML对应的数据实例，如果处理失败则返回null。</returns>
        public override HttpRespXmlBase HandleResponse(byte[] respBody, Encoding encode)
        {
            if (null == respBody || null ==encode)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("CWmsEntryOrderCreate.{0}, 非法的输入参数respBody={1}, encode={2}", System.Reflection.MethodBase.GetCurrentMethod().Name, respBody, encode);
                return null;
            }
            else
            {
                try
                {
                    var respXml = encode.GetString(respBody);
                    HttpRespXmlBase retObj = new HttpRespXml_EntryOrderCreate(respXml);
                    return retObj;
                }
                catch (Exception ex)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error(ex, "CWmsEntryOrderCreate.{0}发生异常. respBody={1}, encode={2}", System.Reflection.MethodBase.GetCurrentMethod().Name, respBody, encode);
                    return null;
                }
            }
        }

        /// <summary>
        /// 返回接口API名称
        /// </summary>
        /// <returns>返回表示接口API名称的字符串</returns>
        public override string GetApiMethod()
        {
            return "entryorder.create"; // TODO: 从系统配置里获取接口方法名称
        }

        /// <summary>
        /// 解析DoTransaction中的输入参数params object[] args。
        /// ！args中第一个为主入库订单Id、第二个位主采购订单Id
        /// </summary>
        /// <param name="args">DoTransaction中的输入参数params object[] args</param>
        /// <param name="pEid">返回主入库订单Id</param>
        /// <param name="pPid">返回主采购订单Id</param>
        /// <returns>返回错误码，若执行成功则返回TError.RunGood；否则返回其他值</returns>
        int ParseArguments(out string pEid, out string pPid, params object[] args)
        {

            pEid = string.Empty;
            pPid = string.Empty;

            if (null == args || 1 > args.Length)
            {
                var ret = new ThirdResult<List<object>>("同步创建入库订单接口，解析输入参数，结束，输入参数为null");
                ret.End();
                return TError.Post_ParamError.Int();
            }
            pEid = args[0] as string;
            pPid = args[1] as string;

            return TError.RunGood.Int();
        }

        protected override void RunL(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
