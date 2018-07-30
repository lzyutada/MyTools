using C_WMS.Data.Mango.MisModelPWI;
using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.Interfaces.Methods
{
    /// <summary>
    /// C-WMS通讯中用到的参数
    /// </summary>
    class MWmsTransactionParamsBase<TParam> where TParam :class, new()
    {
        /// <summary>
        /// 通信url
        /// </summary>
        virtual protected string Host { get; set; }

#if C_WMS_V1
        /// <summary>
        /// HTTP Transaction中的Host URI
        /// </summary>
        virtual public string RequestUri { get { return string.Empty; } }
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
        /// default constructor
        /// </summary>
        public CWmsTransactionParams()
        {
            PostParams = new HttpPostPramsEntity();
        }
#else
        /// <summary>
        /// default constructor
        /// </summary>
        public MWmsTransactionParamsBase()
        {
            bool debugMode = (null != CWmsMisSystemParamCache.Cache.EnableDebug) ?
                CWmsMisSystemParamKeys.cStrIsEnabled.Equals(CWmsMisSystemParamCache.Cache.EnableDebug.PValue ?? string.Empty) :
                false;

            Host = (debugMode) ?
                CWmsMisSystemParamCache.Cache.CWMSOfflineHostUri?.PValue ?? string.Empty :
                CWmsMisSystemParamCache.Cache.CWMSHostUri?.PValue ?? string.Empty;
        }
#endif
    }

    /// <summary>
    /// base implementation for MWMS transaction.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TParam"></typeparam>
    class MWmsTransactionImplBase<TRequest, TResponse, TParam> : IMWmsTransactionImpl<TRequest, TResponse> where TParam : class, new()
    {
        /// <summary>
        /// get or set transaction parameters.
        /// </summary>
        public TParam Params { get; protected set; }

        /// <summary>
        /// default constructor
        /// </summary>
        protected MWmsTransactionImplBase()
        {
            Initialize();
        }
        
        /// <summary>
        /// disponse sotrage.
        /// </summary>
        virtual public void Dispose()
        {
            // do nothing
        }

        /// <summary>
        /// 判断是否从WMS系统响应同步成功
        /// </summary>
        /// <param name="pResp"></param>
        virtual public bool TransactionIsSuccess(TResponse pResp)
        {
            throw new NotImplementedException(string.Format("{0}.TransactionIsSuccess(), should be implemented with it's inherits.", GetType()));
        }

        virtual protected void Initialize()
        {
            Params = new TParam();
        }

        /// <summary>
        /// do POST transaction. return TError.RunGood if transaction is success
        /// , otherwise return other value.
        /// </summary>
        /// <param name="pRequest">request object</param>
        /// <param name="pResp">return with response data from WMS server.</param>
        /// <param name="pMsg">return error message</param>
        /// <returns>Response XML实例</returns>
        virtual protected int Post(TRequest pRequest, out byte[] pResp, out string pMsg)
        {
            throw new NotImplementedException(string.Format("{0}.Post(), should be implemented with it's inherits.", GetType()));
        }

        /// <summary>
        /// do transaction for synchronizing. return response object 
        /// from WMS service or default(TResponse) if internal error.
        /// </summary>
        /// <param name="pReqObj">request object.</param>
        /// <returns></returns>
        /// <exception cref="Exception">method may throw exception in some unexpected cases.</exception>
        virtual public TResponse DoTransaction(TRequest pReqObj)
        {
            if (null == pReqObj)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error("{0}.DoTransaction(), invalid null input param.", GetType());
                return default(TResponse);
            }
            else
            {
                byte[] respData = null;
                string errMsg = string.Empty;
                TResponse rsltObj = default(TResponse);
                int err = Post(pReqObj, out respData, out errMsg); // 
                rsltObj = HandleResponse(respData);
                return rsltObj;
            }
        }

        /// <summary>
        /// return name of the synchronizing method.
        /// </summary>
        /// <returns></returns>
        virtual public string GetApiMethod()
        {
            throw new NotImplementedException(string.Format("{0}.GetApiMethod(), should be implemented with it's inherits.", GetType()));
        }

        /// <summary>
        /// handle response data retrieved from WMS service.
        /// return an object representing response data 
        /// or null if failed or invalid response data.
        /// </summary>
        /// <param name="respData">response data.</param>
        /// <returns></returns>
        virtual public TResponse HandleResponse(byte[] respData)
        {
            throw new NotImplementedException(string.Format("{0}.HandleResponse(), should be implemented with it's inherits.", GetType()));
        }

        /// <summary>
        /// new an object representing request data and return.
        /// return null if failure.
        /// </summary>
        /// <returns></returns>
        virtual public TRequest NewRequestObj()
        {
            throw new NotImplementedException(string.Format("{0}.NewRequestObj(), should be implemented with it's inherits.", GetType()));
        }

        /// <summary>
        /// Parsing arguments for this transaction. Return TError.RunGood if success, 
        /// otherwise return others in TError.
        /// </summary>
        /// <param name="args">arguments for transaction</param>
        /// <returns></returns>
        virtual public int ParseArguments(params object[] args)
        {
            throw new NotImplementedException(string.Format("{0}.ParseArguments(), should be implemented with it's inherits.", GetType()));
        }

        /// <summary>
        /// reset IsUpdateOK of row(s) in Dict709 before transaction.
        /// Insert new row if doesn't exist in 709.
        /// Return count of affected rows -or- -1 / TError if failure.
        /// </summary>
        /// <returns></returns>
        virtual public int Reset709()
        {
            throw new NotImplementedException(string.Format("{0}.Reset709(), should be implemented with it's inherits.", GetType()));
        }

        /// <summary>
        /// Update IsUpdateOK of row(s) in Dict709 according to result of transaction.
        /// return count of affected rows -or- -1/TError if failure.
        /// </summary>
        /// <param name="pMapClassId">MapClassId</param>
        /// <param name="pMapIds">a set of MapId1</param>
        /// <param name="pIsUpdateOK">value to be updated for IsUpdateOK</param>
        /// <param name="pIsDel">value to be updated for IsDel</param>
        /// <returns></returns>
        virtual protected int Update709(TDict709_Value pMapClassId, List<string> pMapIds, TDict285_Values pIsUpdateOK, TDict285_Values pIsDel) {
            throw new NotImplementedException(string.Format("{0}.Update709(TDict709_Value, List<string>, TDict285, TDict285), should be implemented with it's inherits.", GetType()));
        }

        /// <summary>
        /// Update IsUpdateOK of row(s) in Dict709 according to pUpdateOK.
        /// return the count of affected rows -or- -1/TError if failure.
        /// </summary>
        /// <param name="pUpdateOK">flag indicating value for IsUpdateOK</param>
        /// <returns></returns>
        virtual public int Update709(bool pUpdateOK)
        {
            throw new NotImplementedException(string.Format("{0}.Update709(bool), should be implemented with it's inherits.", GetType()));
        }

        /// <summary>
        /// 激活计时器，启动异步通讯。若操作成功则返回TError.RunGood；否则返回其他值。
        /// 需要在继承类中实现该方法
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        virtual public int ActivateImpl(params object[] args)
        {
            return TError.Post_NoChange.Int(); // do nothing
        }

        /// <summary>
        /// 需要在继承类中实现该方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        virtual public int RunImpl(object sender, EventArgs args)
        {
            return TError.Post_NoChange.Int(); // do nothing
        }
    }
}
