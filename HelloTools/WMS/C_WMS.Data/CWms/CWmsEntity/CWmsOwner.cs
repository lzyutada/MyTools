using C_WMS.Data.Wms.Data;
using C_WMS.Interface.CWms.CWmsEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.CWmsEntity
{
    class CWmsOwner : CWmsEntityBase
    {
        public WmsOwner WOwner { get; protected set; }

        public CWmsOwner()
        {
            EntityName = "Owner";
            WOwner = new WmsOwner();
        }

        public CWmsOwner(string code, string name)
        {
            EntityName = "Owner";
            WOwner = new WmsOwner(code, name);
        }

        public override void Dispose()
        {
            // do nothing
        }

        public override string ToString()
        {
            return string.Format("[{0}]: {1}", EntityName, WOwner);
        }
    }
}
