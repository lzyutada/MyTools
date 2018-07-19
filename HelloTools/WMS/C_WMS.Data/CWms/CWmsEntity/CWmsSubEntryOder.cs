using C_WMS.Data.Mango.Data;
using C_WMS.Data.Wms.Data;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 子入库订单。
    /// </summary>
    public class CWmsSubEntryOder : CWmsSubOrderBase<CWmsSubEntryOder, MangoSubEntryOrder, WmsEntryOrderDetail, CWmsSubEntryOderHandler>
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsSubEntryOder()
        {
            MangoOrder = new MangoSubEntryOrder();
            WmsOrder = new WmsEntryOrderDetail();
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="pSrc"></param>
        public CWmsSubEntryOder(CWmsSubEntryOder pSrc)
        {
            if (null != pSrc)
            {
                mMangoOrder = pSrc.MangoOrder;
                mWmsOrder = pSrc.WmsOrderDetail;
            }
            else
            {
                mMangoOrder = new Mango.Data.MangoSubEntryOrder();
                mWmsOrder = new Wms.Data.WmsEntryOrderDetail();
            }
        }

        /// <summary>
        /// 获取芒果商城子入库订单的ID
        /// </summary>
        /// <returns></returns>
        public override string GetId()
        {
            return (MangoOrder as C_WMS.Data.Mango.Data.MangoSubEntryOrder).ProductInputId.ToString();
        }
    }

    /// <summary>
    /// 子单据实体DataHandler类
    /// </summary>
    class CWmsSubEntryOderHandler : CWmsSubOrderBaseHandlerBase<CWmsSubEntryOder, MangoSubEntryOrder, WmsEntryOrderDetail>
    {
    }
}
