using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// WMS系统中的承运商类
    /// </summary>
    public class WmsLogistics: WmsEntityBase
    {
        public string logisticsCode
        {
            get { return WmsID; }
            set { WmsID = value; }
        }

        public string logisticsName
        {
            get { return EntityName; }
            set { EntityName = value; }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public WmsLogistics()
        {
            WmsID = string.Empty;
            logisticsName = string.Empty;
        }

        /// <summary>
        /// construct by code and name
        /// </summary>
        /// <param name="pCode">code of logistics</param>
        /// <param name="pName">name of logistics</param>
        public WmsLogistics(string pCode, string pName)
        {
            WmsID = pCode;
            logisticsName = pName;
        }
    }
}
