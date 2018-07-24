using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城中的单据基类，包括商城订单、采购单等。
    /// </summary>
    interface IMangoOrderBase
    {
        string GetId();
    }
}
