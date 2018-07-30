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
            var tmp = MangoFactory.GetWarehouse(wid);
            if (null != tmp)
            {
                WarehouseId = tmp.WarehouseId;
                WarehouseName = tmp.WarehouseName;
                CompanyTypeId = tmp.CompanyTypeId;
            }
            else
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("MangoWarehouse._ctor({0}), failed in retrieving entity of Product_Warehouse", wid);
            }
        }
    }
}
