using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using CWmsInterface.DataModel;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// C-WMS系统中的商品类
    /// </summary>
    public class WmsProduct : WmsEntityBase
    {
        /// <summary>
        /// 商品条码
        /// </summary>
        public string barCode;

        /// <summary>
        /// 商品类型
        /// </summary>
        public TWmsItemType itemType = TWmsItemType.EDefaultType;
    }
}
