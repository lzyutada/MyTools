using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// WMS系统退货订单明细
    /// </summary>
    public class WmsReturnOrderDetail : WmsOrderDetailBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public WmsReturnOrderDetail()
        {
        }

        /// <summary>
        /// 拷贝数据，相当于operator=
        /// </summary>
        /// <param name="srcObj">数据源实体</param>
        public void CopyFrom(WmsReturnOrderDetail srcObj)
        {
            if (null != srcObj)
            {
                mOwner = srcObj.mOwner;
                OutBizCode = srcObj.OutBizCode;
                WmsID = srcObj.WmsID;
            }
        }
    }
}
