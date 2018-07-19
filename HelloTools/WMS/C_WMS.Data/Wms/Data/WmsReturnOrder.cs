using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// C-WMS系统中的退货入库订单
    /// </summary>
    class WmsReturnOrder : WmsOrderBase
    {
        /// <summary>
        /// 订单标记列表[orderFlag]
        /// </summary>
        protected WmsReturnOrderFlag orderFlag = null;

        /// <summary>
        /// 获取订单标记列表。
        /// </summary>
        public WmsReturnOrderFlag OrderFlag { get { return orderFlag; } }

        /// <summary>
        /// 退货单据类型
        /// </summary>
        public TWmsReturnOrderType orderType = TWmsReturnOrderType.THRK;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WmsReturnOrder()
        {
            orderFlag = new WmsReturnOrderFlag();
        }

        /// <summary>
        /// 设置退货物流标志
        /// </summary>
        /// <param name="pVisit"></param>
        /// <param name="pSellerAfford"></param>
        /// <param name="pSyncReturnBill"></param>
        public void SetOrderFlag(bool pVisit, bool pSellerAfford, bool pSyncReturnBill)
        {
            orderFlag.isVisit = pVisit;
            orderFlag.isSellerAfford = pSellerAfford;
            orderFlag.isSyncReturnBill = pSyncReturnBill;
        }
    }

    /// <summary>
    /// C-WMS系统中退货入库订单的单据标识。
    /// 参考C-WMS接口文档：用字符串格式来表示订单标记列表：比如VISIT^ SELLER_AFFORD^SYNC_RETURN_BILL 等, 中间用“^”来隔开
    /// 订单标记list (所有字母全部大写) ： VISIT=上门；SELLER_AFFORD=是否卖家承担运费 (默认是) ；SYNC_RETURN_BILL=同时退回发票
    /// </summary>
    class WmsReturnOrderFlag
    {
        /// <summary>
        /// 是否上门
        /// </summary>
        public bool isVisit = false;

        /// <summary>
        /// 是否卖家承担运费 (默认是) 
        /// </summary>
        public bool isSellerAfford = true;

        /// <summary>
        /// 是否同时退回发票
        /// </summary>
        public bool isSyncReturnBill = false;

        private const string mVisit = "VISIT";
        private const string mSellerAfford = "SELLER_AFFORD";
        private const string mSyncReturnBill = "SYNC_RETURN_BILL";
        private const string mJointChar = "^";

        /// <summary>
        /// overrided method. 获取用字符串格式来表示的订单标记列表
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret = string.Empty;
            ret += (isVisit) ? mVisit : string.Empty;
            ret += (isSellerAfford) ? (mJointChar + mSellerAfford) : string.Empty;
            ret += (isSyncReturnBill) ? (mJointChar + mSyncReturnBill) : string.Empty;
            return ret;

        }
    }
}
