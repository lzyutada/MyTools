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
    interface ICWmsPostTransaction
    {
        /// <summary>
        /// 返回API接口的method
        /// </summary>
        /// <returns></returns>
        string GetApiMethod();

        /// <summary>
        /// 执行各接口的HTTP Transaction
        /// </summary>
        /// <returns></returns>
        HttpRespXmlBase DoTransaction();
    }
}
