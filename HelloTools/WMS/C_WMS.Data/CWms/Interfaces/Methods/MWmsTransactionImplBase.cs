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
        /// 通信url //TODO配置之一
        /// </summary>
        virtual protected string HostUri { get; set; }

        /// <summary>
        /// HTTP Transaction中的Host URI
        /// </summary>
        virtual public string RequestUri { get { return string.Empty; } }
#if C_WMS_V1
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
        public MWmsTransactionParamsBase()
        {
            HostUri = CWmsMisSystemParamCache.Cache.CWMSHostUri.PValue;
        }
#endif
    }

    class MWmsTransactionImplBase<TRequest, TResponse, TParam> : IMWmsTransactionImpl<TRequest, TResponse> where TParam : class, new()
    {
        public TParam Params { get; protected set; }

        //virtual public TRequest RequestObject { get; set; }

        //virtual public TResponse ResponseObject { get; set; }

        protected MWmsTransactionImplBase()
        {
            Initialize();
        }
        
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
            throw new NotImplementedException();
        }

        virtual protected void Initialize() // where TParam : class, new()
        {
            Params = new TParam();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <param name="pResp"></param>
        /// <param name="pMsg"></param>
        /// <returns>Response XML实例</returns>
        virtual protected int Post(TRequest pRequest, out byte[] pResp, out string pMsg)
        {
            throw new NotImplementedException("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pReqObj"></param>
        /// <returns></returns>
        virtual public TResponse DoTransaction(TRequest pReqObj)
        {
            if (null == pReqObj)
            {
                // TODO: error info
                return default(TResponse);
            }

            byte[] respData = null;
            string errMsg = string.Empty;
            TResponse rsltObj = default(TResponse);
            int err = Post(pReqObj, out respData, out errMsg); // 
            rsltObj = HandleResponse(respData);
            return rsltObj;
        }

        virtual public string GetApiMethod()
        {
            throw new NotImplementedException();
        }

        virtual public TResponse HandleResponse(byte[] respData)
        {
            throw new NotImplementedException();
        }

        virtual public TRequest NewRequestObj()
        {
            throw new NotImplementedException();
        }

        virtual public int ParseArguments(params object[] args)
        {
            throw new NotImplementedException();
        }

        virtual public int Reset709()
        {
            throw new NotImplementedException();
        }

        virtual protected int Update709(TDict709_Value pMapClassId, List<string> pMapIds, TDict285_Values pIsUpdateOK, TDict285_Values pIsDel) {
            throw new NotImplementedException();
        }

        virtual public int Update709(bool pUpdateOK)
        {
            throw new NotImplementedException();
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
