using System;
using C_WMS.Data.Mango.Data;
using C_WMS.Data.Wms.Data;
using MangoMis.Frame.Helper;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 子入库订单。
    /// </summary>
    class CWmsSubEntryOder : CWmsSubOrderBase<CWmsSubEntryOder, MangoSubEntryOrder, WmsEntryOrderDetail, CWmsSubEntryOderHandler>
    {
        public override string Id { get { return MangoOrder.ProductInputId.Int().ToString(); } }

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
                MangoOrder = pSrc.MangoOrder;
                WmsOrder = pSrc.WmsOrder;
            }
            else
            {
                MangoOrder = new MangoSubEntryOrder();
                WmsOrder = new WmsEntryOrderDetail();
            }
        }
    }

    /// <summary>
    /// 子单据实体DataHandler类
    /// </summary>
    class CWmsSubEntryOderHandler : CWmsSubOrderBaseHandlerBase<CWmsSubEntryOder, MangoSubEntryOrder, WmsEntryOrderDetail>
    {
    }
}
