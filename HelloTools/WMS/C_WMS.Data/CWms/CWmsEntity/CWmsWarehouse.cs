using C_WMS.Data.Mango.Data;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Interface.CWms.CWmsEntity;
using MangoMis.Frame.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// entity of warehouse in C_WMS
    /// </summary>
    class CWmsWarehouse : CWmsEntityBase
    {
        /// <summary>
        /// get entity of warehouse in Mis
        /// </summary>
        public MangoWarehouse Mango { get; protected set; }

        /// <summary>
        /// return name/code of warehouse according to configuration in SystemParam.
        /// </summary>
        public string WmsCode
        {
            get
            {
                CWmsSystemParam_CustomerOwnerWarehouses warehouse = null;
                foreach (var owner in CWmsMisSystemParamCache.Cache.CustomerId.Owners)
                {
                    warehouse = owner.Warehouses.Find(w => w.Code.Equals(Mango?.WarehouseId.ToString()));
                    if (null != warehouse) break;
                }
                return warehouse?.Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        public CWmsWarehouse(string code, string name)
        {
            Mango.WarehouseId = code.Int();
            EntityName = name;
        }

        public override void Dispose()
        {
            if (null != Mango) Mango.Dispose();
        }
    }
}
