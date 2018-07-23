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
    class WmsProduct : WmsEntityBase
    {
        /// <summary>
        /// 生成商品编码(itemCode)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="ggLinkId"></param>
        /// <returns></returns>
        static public string NewItemCode(string productId, string ggLinkId)
        {
            return (string.IsNullOrEmpty(productId)) ? string.Empty : ((string.IsNullOrEmpty(ggLinkId)) ? productId : string.Format("{0}-{1}", productId, ggLinkId));
        }

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
