using C_WMS.Data.Mango.Data;
using C_WMS.Interface.CWms.CWmsEntity;
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
        CWmsWarehouseHandler _handler = new CWmsWarehouseHandler();

        /// <summary>
        /// get entity of warehouse in Mis
        /// </summary>
        public MangoWarehouse MangoEntity { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string WarehouseCode
        {
            get
            {
                return _handler.GetWarehouseCode(this);
            }
        }

        public override void Dispose()
        {
            // do nothing
        }
    }

    class CWmsWarehouseHandler
    {
        public string GetWarehouseCode(CWmsWarehouse pWarehouse)
        {

        }
    }
}
