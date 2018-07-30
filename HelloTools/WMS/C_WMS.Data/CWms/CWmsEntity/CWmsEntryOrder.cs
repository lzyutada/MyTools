using C_WMS.Data.Mango.Data;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.Wms.Data;
using MangoMis.Frame.Helper;
using MisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C_WMS.Data.Mango;
using MangoMis.MisFrame.Frame;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 入库订单
    /// </summary>
    class CWmsEntryOrder : CWmsOrderBase<CWmsEntryOrder, MangoEntryOrder, WmsEntryOrder, CWmsSubEntryOder, CWmsEntryOrderHandler>
    {
        /// <summary>
        /// overrided. 返回芒果商城中主入库订单的Id
        /// </summary>
        /// <returns></returns>
        public override string Id { get { return MangoOrder.ProductInputMainId.Int().ToString();} }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CWmsEntryOrder()
        {
            OrderType = TCWmsOrderType.EEntryOrder;
            MangoOrder = new MangoEntryOrder();
            WmsOrder = new WmsEntryOrder();
        }

        /// <summary>
        /// new handle for CWmsEntryOrder
        /// </summary>
        /// <returns></returns>
        protected override CWmsEntryOrderHandler NewHandler()
        {
            return new CWmsEntryOrderHandler();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class CWmsEntryOrderHandler : CWmsOrderBaseHandlerBase<CWmsEntryOrder, MangoEntryOrder, WmsEntryOrder, CWmsSubEntryOder, CWmsEntryOrderHandler>
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsEntryOrderHandler() : base(TCWmsOrderType.EEntryOrder)
        {
        }

        /// <summary>
        /// 根据主入库单ID获取实体
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        static public CWmsEntryOrder NewOrder(string pId)
        {
            throw new NotImplementedException("");
        }
        
        /// <summary>
        /// get entity of WmsLogistics as the logistics of this entryorder represented by _order.
        /// -or- return null if failed in method executation.
        /// </summary>
        /// <returns></returns>
        public WmsLogistics GetLogistics(CWmsEntryOrder pOrder)
        {
            return base.GetLogistics(pOrder);
        }
    }
}
