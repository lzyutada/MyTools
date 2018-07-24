using C_WMS.Interface.CWms.CWmsEntity;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// WMS系统数据基类
    /// </summary>
    public class WmsEntityBase : CWmsEntityBase
    {
        /// <summary>
        /// WMS系统Id
        /// </summary>
        public string WmsID = string.Empty;

        /// <summary>
        /// override Dispose
        /// </summary>
        public override void Dispose()
        {
            // not implement
        }
    }
}
