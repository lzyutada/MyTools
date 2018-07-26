using C_WMS.Data.CWms.Interfaces.Data;
using C_WMS.Interface.CWms.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.Interfaces.Methods
{
    //此cs放置的都是一些请求访问的基础方法
    /// <summary>
    /// C-WMS接口调用通讯接口
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    interface IMWmsTransaction<TResponse> : IDisposable
    {
        /// <summary>
        /// 执行各接口的HTTP Transaction
        /// </summary>
        /// <returns></returns>
        TResponse DoTransaction();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        TResponse DoTransaction(params object[] args);

        /// <summary>
        /// overided。执行接口的HTTP Transaction，该方法可以传入参数，返回HTTP Response Body、错误码和错误描述。需要在子类中实现
        /// </summary>
        /// <param name="pResp">返回HTTP Response Body</param>
        /// <param name="pMsg">错误描述，若返回成功则返回String.Empty</param>
        /// <param name="args">传入参数</param>
        /// <returns>返回错误码</returns>
        int DoTransaction(out TResponse pResp, out string pMsg, params object[] args);        
    }
}
