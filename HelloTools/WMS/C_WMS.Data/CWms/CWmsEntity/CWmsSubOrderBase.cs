using C_WMS.Data.Wms.Data;
using System;
using TT.Common.Frame.Model;


namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 子单据基类
    /// </summary>
    abstract class CWmsSubOrderBase<TOrderType, TMangoType, TWmsType, THandlerType>
    {
        #region Propeties
        public THandlerType Handler { get; protected set; }

        /// <summary>
        /// 获取芒果商城订单实例
        /// </summary>
        public TMangoType MangoOrder { get; protected set; }

#if false
        /// <summary>
        /// 芒果商城订单实例
        /// </summary>
        protected EntityBase mMangoOrder = null;
#endif
        /// <summary>
        /// 获取WMS系统的单据实例
        /// </summary>
        /// <returns></returns>
        public TWmsType WmsOrder { get; protected set; }
#if false
        /// <summary>
        /// WMS系统的单据实例
        /// </summary>
        protected WmsOrderDetailBase mWmsOrder = null;
#endif
#if false
        /// <summary>
        /// 商品实例
        /// </summary>
        private CWmsProduct mProduct = null;
#endif
        /// <summary>
        /// 获取商品实例
        /// </summary>
        /// <returns></returns>
        public CWmsProduct Product { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string Id { get; }
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        protected CWmsSubOrderBase()
        {
            Product = new CWmsProduct();
        }

#if false
        /// <summary>
        /// 获取子单据的ID
        /// </summary>
        /// <returns></returns>
        abstract public string GetId();
#endif
    }

    /// <summary>
    /// 子单据实体DataHandler类
    /// </summary>
    abstract class CWmsSubOrderBaseHandlerBase<TOrderType, TMangoType, TWmsType>
    {
#if false
        public virtual CWmsWarehouse GetWarehouse(CWmsOrderBase<TOrderType, TMangoType, TWmsType, TSubOrderType, THandlerType> pOrder)
        {

        }
#endif
#if false
        /// <summary>
        /// get entity of WmsLogistics as the logistics of this entryorder represented by _order.
        /// -or- return null if failed in method executation.
        /// </summary>
        /// <returns></returns>
        public virtual WmsLogistics GetLogistics(CWmsOrderBase<TOrderType, TMangoType, TWmsType, TSubOrderType, THandlerType> pOrder)
        {
            Product_PeiSong_ProductMain deliveryOrder = null;
            CWmsSystemParam_LogisticsItem logistics = null;
            WmsLogistics retObj = null;

            if (null == pOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("Failed in getting entity of WmsLogistics by {0}, pOrder[{1}] is null", typeof(TOrderType), pOrder);
                return retObj;
            }

            // get entity of deliveryorder by id of entryorder.
            if (null == (deliveryOrder = Mango.MisModelFactory.GetMisEntity<Product_PeiSong_ProductMain>(pOrder.Id)))
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("Failed in getting entity of WmsLogistics by ID[{0}], CANNOT retrieve deliveryorder by {1}(typeof[{2}])", pOrder?.Id, pOrder, typeof(TOrderType));
            }
            else
            {
                // 根据主配送单中的‘配送人’判读使用第三方物流还是芒果物流
                if (null == (logistics = CWmsMisSystemParamCache.Cache.GetLogisticsByUserId(deliveryOrder.DeliveryUserId.Int().ToString())))
                {
                    retObj = new WmsLogistics(logistics.Code, logistics.Name);
                }
                else
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("Failed in getting entity of WmsLogistics by {0}.ID[{1}], CANNOT retrieve cached logistics by [{2}].DeliveryUserId[{3}]. return default logistics.", typeof(TOrderType), pOrder?.Id, deliveryOrder, deliveryOrder.DeliveryUserId);
                    retObj = CWmsDataFactory.GetDefaultLogistic();
                }
            }
            return retObj;
        } // WmsLogistics GetLogistics()
#endif
    } // class CWmsSubOrderBaseHandlerBase
}
