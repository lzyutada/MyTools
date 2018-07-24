using C_WMS.Interface.CWms.CWmsEntity;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// 子入库订单。
    /// </summary>
    public class WmsEntryOrderDetail : WmsOrderDetailBase
    {
        /// <summary>
        /// 实际正品接收数量
        /// </summary>
        public int actualZpQty = -1;

        /// <summary>
        /// 计划接收数量
        /// </summary>
        public int planQty = -1;

        /// <summary>
        /// 实际机损收货数量
        /// </summary>
        public int actualJsQty = -1;

        /// <summary>
        /// 实际箱损收货数量
        /// </summary>
        public int actualXsQty = -1;

        /// <summary>
        /// 实际残次品收货数量
        /// </summary>
        public int actualCcQty = -1;

        /// <summary>
        /// default constructor
        /// </summary>
        public WmsEntryOrderDetail(){  }

        /// <summary>
        /// 拷贝构造
        /// </summary>
        /// <param name="pSrcObj"></param>
        public WmsEntryOrderDetail(WmsEntryOrderDetail pSrcObj) { CopyFrom(pSrcObj); }

        /// <summary>
        /// 拷贝数据
        /// </summary>
        /// <param name="pSrcObj">源实体</param>
        public string CopyFrom(WmsEntryOrderDetail pSrcObj)
        {
            if (null == pSrcObj)
                return "非法入参，源实体pSrcObj为null";
            actualZpQty = pSrcObj.actualZpQty;
            planQty = pSrcObj.planQty;
            actualJsQty = pSrcObj.actualJsQty;
            actualXsQty = pSrcObj.actualXsQty;
            actualCcQty = pSrcObj.actualCcQty;
            return string.Empty;
        }    
    }
}
