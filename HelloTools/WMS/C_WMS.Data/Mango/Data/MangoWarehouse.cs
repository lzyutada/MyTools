using MisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城中的仓库类
    /// </summary>
    public class MangoWarehouse : Product_Warehouse
    {
        /// <summary>
        ///  default constructor.
        /// </summary>
        public MangoWarehouse() { }

        /// <summary>
        /// Constructo by envaluate warehouse id
        /// </summary>
        /// <param name="wid"></param>
        public MangoWarehouse(string wid)
        {
            int tmpId = -1;
            WarehouseId = (int.TryParse(wid, out tmpId)) ? tmpId : -1;
        }
    }
}
