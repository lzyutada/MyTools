using C_WMS.Data.Mango.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 商城订单类
    /// </summary>
    class CWmsMallOrder:CWmsMcocOrder
    {
        /// <summary>
        /// 返回芒果商城的主商城订单的Id
        /// </summary>
        /// <returns></returns>
        public override string GetId()
        {
            return (MangoOrder as MangoMallOrder).DingDanID.ToString();
        }
    }
}
