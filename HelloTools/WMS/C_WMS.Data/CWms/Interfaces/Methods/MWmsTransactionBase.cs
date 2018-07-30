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
        /// enumerate steps for asynchrous transaction
        /// </summary>
        protected enum TAsyncSteps
        {
            EStart,
            EReset709,
            ENewRequestObject,
            EDoTransaction,
            EUpdate709,
            ESuccess,
            EFailed,
            EStopped
        }

        protected TAsyncSteps _asyncStep = TAsyncSteps.EStopped;

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

        /// <summary>
        /// Impl of transaction.
        /// </summary>
        protected IMWmsTransactionImpl<TRequest, TResponse> Impl { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected MWmsTransactionBase()
        {
            Impl = InitImpl();
        }

        /// <summary>
        /// initializing Impl
        /// </summary>
        /// <returns></returns>
        virtual protected IMWmsTransactionImpl<TRequest, TResponse> InitImpl()
        {
            throw new NotImplementedException(string.Format("{0}.InitImpl(), should be implemented with it's inherits.", GetType()));
        }
        
        /// <summary>
        /// do transaction for synchronizing.
        /// return the response from WMS service -or- null if internal error.
        /// </summary>
        /// <returns></returns>
        virtual public TResponse DoTransaction()
        {
            try
            {
                if (null == Impl)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction()失败，未初始化Impl。", GetType());
                    return null;
                }

                int err = TError.RunGood.Int();
                string errMsg = string.Empty;
                if (0 < (err = Impl.Reset709()))
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(), 重置709字典失败, error={1}", GetType(), err);
                    return null;
                }// operations on 709
                else if (null == (RequestObject = Impl.NewRequestObj()))
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(), 创建请求体对象失败", GetType());
                    return null;
                } // init request data
                else if (null != (ResponseObject = Impl.DoTransaction(RequestObject)))
                {
                    if (TransactionIsSuccess)
                    {
                        err = Impl.Update709(true);
                    }
                    if (0 >= err)
                    {
                        C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(), WMS系统响应成功，更新709失败, error={1}。", GetType(), err);
                    }
                }

                if (!TransactionIsSuccess)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.DoTransaction()失败, WMS系统响应: \r\n{1}.", GetType(), ResponseObject);
                }
                return ResponseObject;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "{0}.DoTransaction()发生异常", GetType());
                return null;
            }
        }

        /// <summary>
        /// overrided. do transaction for synchronizing.
        /// return the response from WMS service -or- null if internal error.
        /// </summary>
        /// <param name="args">arguments for transaction.</param>
        /// <returns></returns>
        virtual public TResponse DoTransaction(params object[] args)
        {
            try
            {
                if (null == Impl)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(params object[])失败，未初始化Impl。", GetType());
                    return null;
                }
                else
                    return (TError.RunGood.Int() == Impl.ParseArguments(args)) ? DoTransaction() : null;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "{0}.DoTransaction(params object[])发生异常", GetType());
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

                if (null == Impl)
                {
                    pResp = null;
                    pMsg = string.Format("{0}.DoTransaction(out TResponse, out string, params object[])失败，未初始化Impl。", GetType());
                    C_WMS.Data.Utility.MyLog.Instance.Error(pMsg);
                    return TError.Post_ParamError.Int();
                }
                else
                {
                    pMsg = string.Empty;
                    pResp = DoTransaction(args);
                    return (TransactionIsSuccess) ? TError.RunGood.Int() : TError.Post_NoChange.Int();
                }
            }
            catch (Exception ex)
            {
                pResp = null;
                pMsg = string.Format("{0}.DoTransaction(out TResponse, out string, params object[]发生异常, {1}", GetType(), ex.Message);
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, pMsg);
                return TError.Post_NoChange.Int();
            }
        }

        /// <summary>
        /// 激活计时器，启动异步通讯。若操作成功则返回TError.RunGood；否则返回其他值
        /// </summary>
        /// <param name="args">arguments for transaction.</param>
        /// <returns></returns>
        public override int Activate(params object[] args)
        {
            if (TAsyncSteps.EStopped != _asyncStep)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("Failed in {0}.Activate(), async-transaction is busy[{1}].", GetType(), _asyncStep);
                return TError.Ser_EvenHaveData.Int();
            }
            else
            {
                int err = (TError.RunGood.Int() == Impl.ActivateImpl(args)) ? base.Activate() : TError.WCF_ConnError.Int();
                if (TError.RunGood.Int() == err)
                {
                    _asyncStep = TAsyncSteps.EStart;
                }
                return err;
            }
        }

        /// <summary>
        /// Run steps for asynchrous transaction with WMS service.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void RunL(object sender, EventArgs args)
        {
            try
            {
                if (System.Threading.Interlocked.Exchange(ref inTimer, 1) == 0)
                {
                    // 若控制器操作逻辑成功，并且控制器的状态不是EStopped，则再次激活计时器
                    DoRunL();
                    if (TAsyncSteps.EStopped != _asyncStep) StartTimer();
                    else StopTimer();
                    System.Threading.Interlocked.Exchange(ref inTimer, 0);
                } // if (Interlocked.Exchange(ref inTimer, 1) == 0)
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "{0}.Run()发生异常。", GetType());
                StopTimer();
            }
        }

        /// <summary>
        /// execute running step. return TError.RunGood if execution of current async step is success
        /// -or- return others in TError if execution failed.
        /// </summary>
        protected virtual int DoRunL()
        {
            int err = TError.RunGood.Int();
            TAsyncSteps tmpStatus = _asyncStep;

            switch (_asyncStep)
            {
                case TAsyncSteps.EStart:
                    {
                        _asyncStep = TAsyncSteps.EReset709;
                        break;
                    }
                case TAsyncSteps.EReset709:
                    {
                        _asyncStep = (0 < (err = Impl.Reset709())) ? TAsyncSteps.ENewRequestObject : TAsyncSteps.EFailed;
                        break;
                    }
                case TAsyncSteps.ENewRequestObject:
                    {
                        _asyncStep = (null != (RequestObject = Impl.NewRequestObj())) ? TAsyncSteps.EDoTransaction : TAsyncSteps.EFailed;
                        break;
                    }
                case TAsyncSteps.EDoTransaction:
                    {
                        ResponseObject = Impl.DoTransaction(RequestObject);
                        _asyncStep = (TransactionIsSuccess) ? TAsyncSteps.EUpdate709 : TAsyncSteps.EFailed;
                        break;
                    }
                case TAsyncSteps.EUpdate709:
                    {
                        _asyncStep = (0 < (err = Impl.Update709(true))) ? TAsyncSteps.ESuccess : TAsyncSteps.EFailed;
                        break;
                    }
                case TAsyncSteps.ESuccess: { _asyncStep = TAsyncSteps.EStopped; break; }
                case TAsyncSteps.EFailed:
                    {
                        C_WMS.Data.Utility.MyLog.Instance.Error("Failed in {0}.DoRunL() debugging.....\r\nRquestObject={1}\r\nResponseObject={2}", GetType(), RequestObject, ResponseObject);
                        _asyncStep = TAsyncSteps.EStopped;
                        break;
                    }
                case TAsyncSteps.EStopped:
                    {
                        break;
                    }
            } // switch(_asyncStep)

            if (TError.RunGood.Int() != err)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("Failed in {0}.DoRunL(), err={1}. AsyncStep was EFailed to EStopped.", GetType(), tmpStatus, err);
            }
            return err;
        } // DoRunL
    } // MWmsTransactionBase
}
