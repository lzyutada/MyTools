using C_WMS.Data.CWms.Interfaces.Methods;
using C_WMS.Data.Wms.Data;
using C_WMS.Interface.CWms.Interfaces.Data;
using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace C_WMS.Data.Wms.Transaction
{
    /// <summary>
    /// C-WMS通讯中用到的参数
    /// </summary>
    class CWmsTransactionParams : MWmsTransactionParamsBase<CWmsTransactionParams>
    {
        /// <summary>
        /// Request URI
        /// </summary>
        public string RequestUri { get { return string.Empty; } }

        /// <summary>
        /// 密钥
        /// </summary>
        protected string SecretKey = null;
        /// <summary>
        /// The request XML
        /// </summary>
        public string RequestXml = string.Empty;

        /// <summary>
        /// 获取POST Transaction中URI中的参数
        /// </summary>
        public Data.HttpPostPramsEntity PostParams { get; protected set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsTransactionParams() : base()
        {
        }
    }

    class CWmsTransactionImplBase<TRequest, TResponse> : MWmsTransactionImplBase<TRequest, TResponse, CWmsTransactionParams>
    {
        /// <summary>
        /// Do POST transaction with HTTP request. return TError.RunGood if transaction is success,
        /// otherwise return others.
        /// </summary>
        /// <param name="pRequest">request object</param>
        /// <param name="pResp">return with response data from WMS service.</param>
        /// <param name="pMsg">return error message.</param>
        /// <returns></returns>
        protected override int Post(TRequest pRequest, out byte[] pResp, out string pMsg)
        {
            try
            {
                string respEncodingStr = string.Empty;

                Params.RequestXml = pRequest.ToString();
                pMsg = C_WMS.Interface.Utility.CWmsUtility.HttpPostTransaction(Params.RequestUri, "POST", "text/xml", Encoding.UTF8.GetBytes(pRequest.ToString()), out respEncodingStr, out pResp);
                return TError.RunGood.Int();
            }
            catch (Exception ex)
            {
                pResp = null;
                pMsg = string.Format("在<{0}>.Post()中发生异常", GetType());
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, pMsg);
                return TError.Post_NoChange.Int();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    class CWmsOrderTransactionImpl<TRequest, TResponse>: CWmsTransactionImplBase<TRequest, TResponse>
    {
        /// <summary>
        /// id of the synchronizing order
        /// </summary>
        public string OrderId { get; protected set; }

        /// <summary>
        /// 判断是否从WMS系统响应成功
        /// </summary>
        /// <param name="pResp"></param>
        /// <returns></returns>
        public override bool TransactionIsSuccess(TResponse pResp)
        {
            return (null == ((pResp as HttpRespXmlBase))) ? false : (pResp as HttpRespXmlBase).IsSuccess();
        }

        /// <summary>
        /// parse args for sychronizing.
        /// </summary>
        /// <param name="args">args needs at least one item that stores the id of sychronizing order</param>
        /// <returns>return TError.RunGood if parsing is successful, otherwise return other.</returns>
        public override int ParseArguments(params object[] args)
        {
            if (null == args || 1 > args.Length)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("{0}.ParseArguments() failed, arguments DEBUG:\r\n{1}", GetType(), Utility.CWmsDataUtility.GetDebugInfo_Args(args));
                return TError.Post_ParamError.Int();
            }
            else
            {
                OrderId = args[0] as string;
                return TError.RunGood.Int();
            }
        }
    }
}
