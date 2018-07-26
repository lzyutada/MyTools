using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using MangoMis.MisFrame.Frame;
using MangoMis.MisFrame.Helper;
using MangoMis.MisFrame.MisStore;
using Send.Interface;
using C_WMS.Interface.CWms.Interfaces.Data;
using C_WMS.Interface.Utility;
using C_WMS.Data.CWms.Interfaces.Data;

namespace C_WMS.Data.CWms.Interfaces.Methods
{
#if C_WMS_V1
    /// <summary>
    /// C-WMS通讯中用到的参数
    /// </summary>
    public class CWmsTransactionParams
    {
    #region Properties
        /// <summary>
        /// 通信url //TODO配置之一
        /// </summary>
        protected string HostUri = SystemParamStore.FindParam("MgMall", "CWmsHostUri")?.PValue ?? ""; // "http://c-wms.iask.in:8081/BH_CLIS/qimen?"; //  
        /// <summary>
        /// 密钥
        /// </summary>
        protected string SecretKey = SystemParamStore.FindParam("MgMall", "secret")?.PValue ?? ""; // "RA8wjgCNocNo99IAd5wFFW93Wll1TuRC"; // 
        /// <summary>
        /// The request XML
        /// </summary>
        public string RequestXml = string.Empty;

        /// <summary>
        /// 获取POST Transaction中URI中的参数
        /// </summary>
        public HttpPostPramsEntity PostParams { get; protected set; }

        /// <summary>
        /// HTTP Transaction中的Host URI
        /// </summary>
        public string URI
        {
            get
            {
                string tStr = "";
                if (PostParams == null)
                {
                    return tStr;
                }

                //生成md5签名
                PostParams.sign = string.Empty;
                PostParams.DoSign(SecretKey, RequestXml);
                PropertyInfo[] properties = PostParams.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                
                if (properties.Length <= 0)
                {
                    return tStr;
                }

                var data_orderby_key = from objDic in properties orderby objDic.Name select objDic;
                foreach (PropertyInfo item in properties)
                {
                    string name = item.Name;
                    object value = item.GetValue(PostParams, null);
                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    {
                        tStr += name + "=" + System.Web.HttpUtility.UrlEncode(value.ToString()) + "&";
                    }
                }
                var uri = CWmsUtility.get_uft8(HostUri + (tStr.Remove(tStr.Length - 1, 1)));
                return uri;
            }
        }

    #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsTransactionParams()
        {
            PostParams = new HttpPostPramsEntity();
        }
    }
#endif

    /// <summary>
    /// C-WMS接口调用通讯基类
    /// </summary>
    /// <typeparam name="TTransaction">接口通讯的继承类</typeparam>
    /// <typeparam name="TRequest">实现具体接口通讯的Impl类型</typeparam>
    /// <typeparam name="TResponse"></typeparam>
    class MWmsTransactionBase<TTransaction, TRequest, TResponse> : Utility.CWmsAsyncControler, IMWmsTransaction<TResponse> where TResponse : class, new()
    {
        /// <summary>
        /// 获取和设置同步通讯的Request数据
        /// </summary>
        public TRequest RequestObject { get; protected set; }

        /// <summary>
        /// 获取和设置同步通讯的Response数据
        /// </summary>
        public TResponse ResponseObject { get; protected set; }

        /// <summary>
        /// 判断是否从WMS系统响应同步成功
        /// </summary>
        virtual public bool TransactionIsSuccess { get { return Impl.TransactionIsSuccess(ResponseObject); } }

        protected IMWmsTransactionImpl<TRequest, TResponse> Impl { get; set; }

#if C_WMS_V1
        /// <summary>
        /// HTTP会话需要使用的参数
        /// </summary>
        public CWmsTransactionParams TransParams { get; private set; }
        /// <summary>
        /// 声明委托，WMS同步结束后的通知操作
        /// </summary>
        /// <param name="pUid"></param>
        /// <param name="pMsg"></param>
        public delegate void DefDlgt_NotifySyncResult(int pUid, string pMsg);
        
        /// <summary>
        /// DefDlgt_NotifySyncResult委托对象
        /// </summary>
        protected DefDlgt_NotifySyncResult mDlgtNotifySyncResult = null;
#endif

        /// <summary>
        /// Default constructor
        /// </summary>
        protected MWmsTransactionBase()
        {
#if C_WMS_V1
            LoadTransactionParams();
            mDlgtNotifySyncResult = DingDingMsgToUser;
#else
            Impl = InitImpl();
#endif
        }

#if C_WMS_V1

        /// <summary>
        /// 加载C-WMS HTTP会话的各项参数
        /// </summary>
        virtual protected void LoadTransactionParams()
        {
            TransParams = new CWmsTransactionParams();
            TransParams.PostParams.method = GetApiMethod();
        }

        /// <summary>
        /// 处理服务器响应
        /// </summary>
        /// <param name="respBody">HTTP响应体</param>
        /// <param name="encode">服务器响应回来的编码格式所对应的Encoding</param>
        /// <returns>返回Response XML对应的数据实例</returns>
        virtual public HttpRespXmlBase HandleResponse(byte[] respBody, Encoding encode)
        {
            if (null == encode || null == respBody)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.HandleResponse(respBody={1}, encoding={2})，非法空入参.", GetType(), respBody, encode);
                return null;
            }
            else
            {
                return new HttpRespXmlBase(encode.GetString(respBody));
            }
        }

        /// <summary>
        /// 发送HTTP POST请求
        /// </summary>
        /// <param name="reqXmlBody">request XML实例</param>
        /// <returns>Response XML实例</returns>
        protected HttpRespXmlBase Post<T>(T reqXmlBody)
        {
            try
            {
                if (null == reqXmlBody)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.Post<{1}>(reqXmlBody={2})，非法空入参.", GetType(), typeof(T), reqXmlBody);
                    return null;
                }

                string errMsg = string.Empty;
                byte[] respBody = null;
                string respEncodingStr = string.Empty;

                TransParams.RequestXml = reqXmlBody.ToString();
                errMsg = CWmsUtility.HttpPostTransaction(TransParams.URI, "POST", "text/xml", Encoding.UTF8.GetBytes(reqXmlBody.ToString()), out respEncodingStr, out respBody);

                if (null == respBody)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("HTTP通讯响应异常:\r\n请求URI: {0}\r\n请求内容: {1}\r\n响应内容为空", TransParams.URI, reqXmlBody);
                    return null;
                }
                else
                {
                    var ret = HandleResponse(respBody, Encoding.GetEncoding(respEncodingStr));
                    if (null == ret)
                    {
                        C_WMS.Data.Utility.MyLog.Instance.Error("HTTP通讯响应完成，处理服务器响应返回空:\r\n请求URI: {0}\r\n请求内容: {1}\r\n服务器响应CDATA: {2}\r\n服务器响应Encoding: {3}", TransParams.URI, reqXmlBody, respBody, respEncodingStr);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}.{1}({2})中发生异常", GetType(), MethodBase.GetCurrentMethod().Name, reqXmlBody);
                return null;
            }
        }

        /// <summary>
        /// 发送钉钉推送给指定用户
        /// </summary>
        /// <param name="pUid">用户Id</param>
        /// <param name="pMsg"></param>
        virtual protected void DingDingMsgToUser(int pUid, string pMsg)
        {
            // send to user
            var rslt = Add_DingDingplan.send_dingding_message_to_user(pUid.Int(), CWmsConsts.cInt芒果钉秘Id, pMsg);

            // send to 芒果商城订单群
            rslt = Add_DingDingplan.send_dingding_message_to_user(CWmsConsts.cInt芒果商城订单群Id, CWmsConsts.cInt芒果钉秘Id, pMsg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iar"></param>
        protected void Acb_NotifySyncResult(IAsyncResult iar)
        {
            try
            {
                (iar.AsyncState as DefDlgt_NotifySyncResult).EndInvoke(iar);    // 结束回调阻塞
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}.{1}({2})中发生异常, AsyncState={3}", GetType(), MethodBase.GetCurrentMethod().Name, iar, iar?.AsyncState);
            }
        }

#if false
        /// <summary>
        /// 返回API接口的method，需要在其子类中实现该方法
        /// </summary>
        /// <returns></returns>
        abstract public string GetApiMethod();
#endif
        /// <summary>
        /// 执行各接口的HTTP Transaction，需要在其子类中实现该方法
        /// </summary>
        /// <returns></returns>
        abstract public HttpRespXmlBase DoTransaction();

        /// <summary>
        /// overided。执行接口的HTTP Transaction，该方法可以传入参数，返回HTTP Response Body、错误码和错误描述。需要在子类中实现
        /// </summary>
        /// <param name="pResp">返回HTTP Response Body</param>
        /// <param name="pMsg">错误描述，若返回成功则返回String.Empty</param>
        /// <param name="args">传入参数</param>
        /// <returns>返回错误码</returns>
        public virtual int DoTransaction(out HttpRespXmlBase pResp, out string pMsg, params object[] args)
        {
            throw new NotImplementedException("int DoTransaction(out HttpRespXmlBase, out string, params object[])需要在子类中实现");
        }

        /// <summary>
        /// 针对int DoTransaction(out HttpRespXmlBase, out string, params object[])方法，解析params object[]入参。
        /// 若解析成功则返回TError.RunGood；否则返回其他值
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual int ParseArguments(params object[] args)
        {
            throw new NotImplementedException("int ParseArguments(params object[])需要在子类中实现");
        }
#else
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        virtual protected IMWmsTransactionImpl<TRequest, TResponse> InitImpl()
        {
            throw new NotImplementedException("");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        virtual public TResponse DoTransaction()
        {
            try
            {
                if (null == Impl)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction()失败，未初始化Impl。", MethodBase.GetCurrentMethod().DeclaringType.Name);
                    return null;
                }
                int err = TError.RunGood.Int();
                string errMsg = string.Empty;
                TResponse rsltResp = null;

                if (TError.RunGood.Int() != Impl.ParseArguments())
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(), 解析参数args失败, error={1}。", MethodBase.GetCurrentMethod().DeclaringType.Name, err);
                    return null;
                }// parse argument
                else if (0 < (err = Impl.Reset709()))
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(), 重置709字典失败, error={1}", MethodBase.GetCurrentMethod().DeclaringType.Name, err);
                    return null;
                }// operations on 709
                else if (null == (RequestObject = Impl.NewRequestObj()))
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(), 创建请求体对象失败, error={1}", MethodBase.GetCurrentMethod().DeclaringType.Name, err);
                    return null;
                } // init request data
                else if (null != (rsltResp = Impl.DoTransaction(RequestObject)) && TransactionIsSuccess)
                {
                    if (0 >= (err = Impl.Update709(true)))
                    {
                        C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(), WMS系统响应成功，更新709失败。", MethodBase.GetCurrentMethod().DeclaringType.Name, err);
                    }
                }
                else
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(), WMS系统响应null或响应失败({1}).", MethodBase.GetCurrentMethod().DeclaringType.Name, TransactionIsSuccess);
                }
                return rsltResp;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "{0}.DoTransaction()发生异常", MethodBase.GetCurrentMethod().DeclaringType.Name);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        virtual public TResponse DoTransaction(params object[] args)
        {
            try
            {
                if (null == Impl)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(params object[])失败，未初始化Impl。", MethodBase.GetCurrentMethod().DeclaringType.Name);
                    return null;
                }

                int err = TError.RunGood.Int();
                string errMsg = string.Empty;
                //TResponse respObj = null;
                if (TError.RunGood.Int() != Impl.ParseArguments(args))
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(params object[]), 解析参数args失败, error={1}。\r\nArguments Debug:{2}", MethodBase.GetCurrentMethod().DeclaringType.Name, err, Utility.CWmsDataUtility.GetDebugInfo_Args(args));
                    return null;
                }// parse argument
                else if (TError.RunGood.Int() != (err = Impl.Reset709()))
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(params object[]), 重置709字典失败, error={1}", MethodBase.GetCurrentMethod().DeclaringType.Name, err);
                    return null;
                }// operations on 709
                else if (null == (RequestObject = Impl.NewRequestObj()))
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(params object[]), 创建请求体对象失败, error={1}", MethodBase.GetCurrentMethod().DeclaringType.Name, err);
                    return null;
                } // init request data
                else if (null != (ResponseObject = Impl.DoTransaction(RequestObject)) && TransactionIsSuccess)
                {
                    if (0 >= (err = Impl.Update709(true)))
                    {
                        C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(out TResponse, out string, params object[]), WMS系统响应成功，更新709失败。", MethodBase.GetCurrentMethod().DeclaringType.Name, err);
                        err = TError.Pro_HaveNoData.Int();

                    }
                }
                else
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(out TResponse, out string, params object[]), WMS系统响应null或响应失败({1}).", MethodBase.GetCurrentMethod().DeclaringType.Name, TransactionIsSuccess);
                }
                return ResponseObject;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "{0}.DoTransaction(params object[])发生异常", MethodBase.GetCurrentMethod().DeclaringType.Name);
                return null;
            }
        }

        /// <summary>
        /// overided。执行接口的HTTP Transaction，该方法可以传入参数，返回HTTP Response Body、错误码和错误描述。需要在子类中实现
        /// </summary>
        /// <param name="pResp">返回HTTP Response Body</param>
        /// <param name="pMsg">错误描述，若返回成功则返回String.Empty</param>
        /// <param name="args">传入参数</param>
        /// <returns>返回错误码</returns>
        virtual public int DoTransaction(out TResponse pResp, out string pMsg, params object[] args)
        {
            try
            {
                int err = TError.RunGood.Int();
                pResp = null;
                if (null == Impl)
                {
                    pMsg = string.Format("{0}.DoTransaction(out TResponse, out string, params object[])失败，未初始化Impl。", MethodBase.GetCurrentMethod().DeclaringType.Name);
                    C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
                    return err = TError.Post_ParamError.Int();
                }
                if (TError.RunGood.Int() != Impl.ParseArguments(args))
                {
                    pMsg = string.Format("{0}.DoTransaction(out TResponse, out string, params object[]), 解析参数args失败, error={1}。\r\nArguments Debug:{2}", MethodBase.GetCurrentMethod().DeclaringType.Name, err, Utility.CWmsDataUtility.GetDebugInfo_Args(args));
                    C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
                    return err = TError.Post_ParamError.Int();
                }// parse argument
                else if (TError.RunGood.Int() != (err = Impl.Reset709()))
                {
                    pMsg = string.Format("{0}.DoTransaction(out TResponse, out string, params object[]), 重置709字典失败, error={1}", MethodBase.GetCurrentMethod().DeclaringType.Name, err);
                    C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
                    return TError.WCF_RunError.Int();
                }// operations on 709
                else if (null == (RequestObject = Impl.NewRequestObj()))
                {
                    pMsg = string.Format("{0}.DoTransaction(out TResponse, out string, params object[]), 创建请求体对象失败, error={1}", MethodBase.GetCurrentMethod().DeclaringType.Name, err);
                    C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
                    return TError.Pro_HaveNoData.Int();
                } // init request data
                else if (null != (pResp = Impl.DoTransaction(RequestObject)) && TransactionIsSuccess)
                {
                    if (0 < (err = Impl.Update709(true)))
                    {
                        pMsg = string.Empty;
                        err = TError.RunGood.Int();
                    }
                    else
                    {
                        pMsg = string.Format("{0}.DoTransaction(out TResponse, out string, params object[]), WMS系统响应成功，更新709失败。", MethodBase.GetCurrentMethod().DeclaringType.Name, err);
                        C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
                        err = TError.Pro_HaveNoData.Int();
                    }
                }
                else
                {
                    pMsg = string.Format("{0}.DoTransaction(out TResponse, out string, params object[]), WMS系统响应null或响应失败({1}).", MethodBase.GetCurrentMethod().DeclaringType.Name, TransactionIsSuccess);
                    C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
                    err = TError.Post_NoChange.Int();
                }
                return err;
            }
            catch (Exception ex)
            {
                pResp = null;
                pMsg = string.Format("{0}.DoTransaction(out TResponse, out string, params object[]发生异常, {1}", MethodBase.GetCurrentMethod().DeclaringType.Name, ex.Message);
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, pMsg);
                return TError.Post_NoChange.Int();
            }
        }

        protected override void RunL(object sender, EventArgs args)
        {
            try
            {
                if (System.Threading.Interlocked.Exchange(ref inTimer, 1) == 0)
                {
                    // 若控制器操作逻辑成功，并且控制器的状态不是EStopped，则再次激活计时器
                    if (TError.RunGood.Int() == Impl.RunImpl(sender, args)) StartTimer();
                    else StopTimer();
                    System.Threading.Interlocked.Exchange(ref inTimer, 0);
                } // if (Interlocked.Exchange(ref inTimer, 1) == 0)
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "MWmsTransactionBase.Run发生异常。");
                StopTimer();
            }
        }

        /// <summary>
        /// 激活计时器，启动异步通讯。若操作成功则返回TError.RunGood；否则返回其他值
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override int Activate(params object[] args)
        {
            if (TError.RunGood.Int() == Impl.ActivateImpl(args)) return base.Activate();
            else return TError.RunGood.Int();
        }
#endif
    }
}
