using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public HttpPostPramsEntity PostParams { get { return mPostParams; } }
        private HttpPostPramsEntity mPostParams = null;

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
                System.Reflection.PropertyInfo[] properties = mPostParams.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                
                if (properties.Length <= 0)
                {
                    return tStr;
                }

                var data_orderby_key = from objDic in properties orderby objDic.Name select objDic;
                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    string name = item.Name;
                    object value = item.GetValue(mPostParams, null);
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
            mPostParams = new HttpPostPramsEntity();
        }
    }

    /// <summary>
    /// C-WMS接口调用通讯基类
    /// </summary>
    public abstract class CWmsPostTransactionBase : Utility.CWmsAsyncControler, ICWmsPostTransaction
    {
        /// <summary>
        /// HTTP会话需要使用的参数
        /// </summary>
        public CWmsTransactionParams mTransParams = null;

        /// <summary>
        /// 声明委托，WMS同步结束后的通知操作
        /// </summary>
        /// <param name="pUid"></param>
        /// <param name="pMsg"></param>
        public delegate void DefDlgt_NotifySyncResult(int pUid, string pMsg);

#if false
        /// <summary>
        /// 声明委托，异步执行WCF操作
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pEntity"></param>
        /// <param name="pOperation"></param>
        /// <returns></returns>
        protected delegate int DefDlgt_WcfExcecution<TEntity>(Mango.TWCFOperation pOperation, TEntity pEntity);
#endif
        /// <summary>
        /// DefDlgt_NotifySyncResult委托对象
        /// </summary>
        protected DefDlgt_NotifySyncResult mDlgtNotifySyncResult = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        protected CWmsPostTransactionBase()
        {
            LoadTransactionParams();
            mDlgtNotifySyncResult = DingDingMsgToUser;
        }

        /// <summary>
        /// 加载C-WMS HTTP会话的各项参数
        /// </summary>
        virtual protected void LoadTransactionParams()
        {
            mTransParams = new CWmsTransactionParams();
            mTransParams.PostParams.method = GetApiMethod();
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
                return null;
            }
            else
            {
                HttpRespXmlBase retObj =  new HttpRespXmlBase(encode.GetString(respBody));
                return retObj;
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
                if (null == reqXmlBody) { return null; }

                string errMsg = string.Empty;
                byte[] respBody = null;
                string respEncodingStr = string.Empty;

                mTransParams.RequestXml = reqXmlBody.ToString();
                errMsg = CWmsUtility.HttpPostTransaction(mTransParams.URI, "POST", "text/xml", Encoding.UTF8.GetBytes(reqXmlBody.ToString()), out respEncodingStr, out respBody);

                if (null == respBody)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("HTTP通讯响应异常:\r\n请求URI: {0}\r\n请求内容: {1}\r\n响应内容为空", mTransParams.URI, reqXmlBody);
                    return null;
                }
                else
                {
                    var ret = HandleResponse(respBody, Encoding.GetEncoding(respEncodingStr));
                    if (null == ret)
                    {
                        C_WMS.Data.Utility.MyLog.Instance.Error("HTTP通讯响应完成，处理服务器响应内容返回空:\r\n请求URI: {0}\r\n请求内容: {1}\r\n服务器响应CDATA: {2}\r\n服务器响应Encoding: {3}", mTransParams.URI, reqXmlBody, respBody, respEncodingStr);
                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
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
                var dlgt = iar.AsyncState as DefDlgt_NotifySyncResult;
                dlgt.EndInvoke(iar);    // 结束回调阻塞
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "在{0}中发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pOperation"></param>
        /// <param name="pEntity"></param>
        /// <returns></returns>
        protected int Dlgt_WcfExecution<TEntity>(Mango.TWCFOperation pOperation, TEntity pEntity)
        {
        }

        /// <summary>
        /// 返回API接口的method，需要在其子类中实现该方法
        /// </summary>
        /// <returns></returns>
        abstract public string GetApiMethod();

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
        abstract public int DoTransaction(out HttpRespXmlBase pResp, out string pMsg, params object[] args);
    }
}
