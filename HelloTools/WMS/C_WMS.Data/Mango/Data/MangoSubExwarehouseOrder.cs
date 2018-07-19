using System;
using System.Collections.Generic;
using System.Text;
using MisModel;
using MangoMis.Frame.DataSource.Simple;
using TT.Common.Frame.Model;
using MangoMis.Frame.ThirdFrame;
using C_WMS.Interface.Utility;
using MangoMis.Frame.Helper;
using C_WMS.Interface;
using System.Reflection;
using C_WMS.Data.Mango.MisModelPWI;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城中的子出库订单（通过购物车结算购买）
    /// </summary>
    class MangoSubExwarehouseOrder : Product_Warehouse_ProductOutput
    {
        #region Properties
        /// <summary>
        /// handler of MangoSubExwarehouseOrder
        /// </summary>
        public MangoSubExwarehouseOrderHandler Handler = new MangoSubExwarehouseOrderHandler();

        /// <summary>
        /// 商城中子订单关联的子配送单ID
        /// </summary>
        public string SubDeliveryOrderId { get; protected set; }

        /// <summary>
        /// 商城中mSubDeliveryOrderId关联的商城子订单Id
        /// </summary>
        public string SubMallOrderId { get; protected set; }

        /// <summary>
        /// 商城中mSubMallOrderId关联的商城主订单Id
        /// </summary>
        public string MallOrderId { get; protected set; }

        /// <summary>
        /// 其对应的商城订单的订单类型
        /// </summary>
        public TMangoOrderType MallOrderType { get; protected set; }

        /// <summary>
        /// 要求出库时间
        /// </summary>
        public string ScheduleDate { get; protected set; }

        /// <summary>
        /// 应收商品数量
        /// </summary>
        public decimal PlanQuantity { get; protected set; }

        ///// <summary>
        ///// 售价
        ///// </summary>
        //public double price = 0.00;

        ///// <summary>
        ///// 实收商品数量, 默认为-1（非法)
        ///// </summary>
        //public int actualQty = -1;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public MangoSubExwarehouseOrder()
        {
            Reset();
        }

        /// <summary>
        /// 通过Product_Warehouse_ProductOutput实例拷贝创建MangoSubExwarehouseOrder实例
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public MangoSubExwarehouseOrder(Product_Warehouse_ProductOutput srcObj)
        {
            CopyFrom(srcObj);
        }

        /// <summary>
        /// 根据Product_Warehouse_ProductOutput的实例进行构造
        /// </summary>
        /// <param name="srcObj">源实例</param>
        public string CopyFrom(Product_Warehouse_ProductOutput srcObj)
        {
            if (null != srcObj)
            {
                #region from Product_Warehouse_ProductOutput
                OutputKuKind = srcObj.OutputKuKind;             // 来源库的类型
                ProductId = srcObj.ProductId;                   // 产品id
                ProductLingYongId = srcObj.ProductLingYongId;   // 领用人id
                ShiYongShuoMing = srcObj.ShiYongShuoMing;       // 使用寿命
                MainId = srcObj.MainId;                         // 主出库单id
                ProductOutputState = srcObj.ProductOutputState; // 出库状态
                WarehouseIdPoint = srcObj.WarehouseIdPoint;     // 目的库
                Applymonth = srcObj.Applymonth;                 // 申请月份例如201501
                goal = srcObj.goal;                             // 出库原因
                ReceiveUserid = srcObj.ReceiveUserid;           // 接收人
                ReceiveTime = srcObj.ReceiveTime;               // 接收时间
                ReceiveState = srcObj.ReceiveState;             // 接收状态 字典157
                MapClassId = srcObj.MapClassId;                 // 类型
                MapId = srcObj.MapId;                           // 类型id
                ProductLifeId = srcObj.ProductLifeId;           // 生命周期ID
                ProductCount = srcObj.ProductCount;             // 产品的出库数量
                OrgId = srcObj.OrgId;                           // 组织id
                ProductGuiGeID = srcObj.ProductGuiGeID;
                UpdateUserID = srcObj.UpdateUserID;             // 最后修改人，备用
                ProductOutputId = srcObj.ProductOutputId;       // 编号，自增
                ProductOutputCode = srcObj.ProductOutputCode;   // 入库单自己的单号
                ProductOutputDate = srcObj.ProductOutputDate;   // 入库的日期
                WarehouseId = srcObj.WarehouseId;               // 外键，从哪个仓库出来的
                FaFangRen = srcObj.FaFangRen;                   // 发放人、出库人
                isErShou = srcObj.isErShou;                     // 是否二手 285
                FuZeRen = srcObj.FuZeRen;                       // 存放工号。0：不能确认使用人
                KuaiJiID = srcObj.KuaiJiID;                     // 备用
                DisOrder = srcObj.DisOrder;                     // 排序
                AddUserid = srcObj.AddUserid;                   // 添加人ID
                AddTime = srcObj.AddTime;                       // 添加时间
                IsDel = srcObj.IsDel;                           // 0:正常;1:删除
                UpdateTime = srcObj.UpdateTime;                 // 最后修改时间
                LingYongRen = srcObj.LingYongRen;               // 存放工号。接收、领用人
                #endregion
                Reset(ProductOutputId.Int().ToString());
                return string.Empty;
            }
            else
            {
                string errMsg = "源实例srcObj为null。";
                C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}, {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, errMsg);
                return errMsg;
            }
        }

        /// <summary>
        /// 单据被重新复制，其相关联的配送单、商城订单被重置
        /// </summary>
        protected void Reset()
        {
            SubDeliveryOrderId = string.Empty;
            SubMallOrderId = string.Empty;
            MallOrderId = string.Empty;
            MallOrderType = TMangoOrderType.EUnknown;
            ScheduleDate = DateTime.MinValue.ToString();
            PlanQuantity = -1;
        }

        /// <summary>
        /// 根据传入的子出库单号更新相关联的配送单编号、商城订单编号
        /// </summary>
        /// <param name="pOrderId"></param>
        protected void Reset(string pOrderId)
        {
            if (string.IsNullOrEmpty(pOrderId))
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}, invalid null param(pOrderId)", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                return;
            }
            try
            {
                // 获取子配送单Id和子订单ID
                var filters = Handler.GetFilter_GetDeliveryOrder(this);
                var deliveryOder = MisModelFactory.GetMisEntity<Product_PeiSong_Product>(filters);
                //var queryRslt = WCF<Product_PeiSong_Product>.Query(1, CWmsConsts.cIntDefaultWcfQueryPageSize, Handler.GetFilter_GetDeliveryOrder(this), new List<CommonOrderModel>());
                if (null != deliveryOder)
                {
                    SubMallOrderId = deliveryOder.ZiDingDanID.Int().ToString();
                    SubDeliveryOrderId = deliveryOder.ProductLingYongId.Int().ToString();

                    var subMallOrder = MisModelFactory.GetMisEntity<Product_User_DingDan_ProductList>(SubMallOrderId); // 获取子订单Id
                    //var subOrder = WCF<Product_User_DingDan_ProductList>.Query(SubMallOrderId.Int()).Data;
                    if (null == subMallOrder)
                    {
                        C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}({2}), failed in retrieving sub-mallorder({3})", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pOrderId, SubMallOrderId);
                        Reset();
                        return;
                    }
                    MallOrderId = subMallOrder.DingDanID.ToString(); // ID of main-mallorder
                    MallOrderType = MangoFactory.ConvertToMangoType(subMallOrder.DingDanType.Int()); // (TMangoOrderType)subMallOrder.DingDanType.Int();
                    ScheduleDate = subMallOrder.PeiSongTime.ToString();
                    PlanQuantity = subMallOrder.ProductCount.Decimal();
                }
                else
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}({2}), failed in retrieving sub-deliveryorder by FILTER:{3}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pOrderId, MisModelFactory.GetDebugInfo_MisFilter(filters));
                    Reset();
                }
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "根据子出库单号[{0}]更新相关联的配送单编号、商城订单编号异常", pOrderId);
                Reset();
            }
        }
    }

    /// <summary>
    /// handler of MangoSubExwarehouseOrder
    /// </summary>
    class MangoSubExwarehouseOrderHandler
    {
        public List<CommonFilterModel> GetFilter_GetDeliveryOrder(MangoSubExwarehouseOrder pOrder)
        {
            try
            {
                if (null == pOrder)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}, invalid null param.", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                    return null;
                }
                List<CommonFilterModel> filters = new List<CommonFilterModel>(2);
                filters.Add(new CommonFilterModel(Mis2014_SubDeliveryOrder_Column.ProductIOputId, "=", pOrder.ProductOutputId.Int().ToString()));
                filters.Add(new CommonFilterModel(Mis2014_SubDeliveryOrder_Column.IsDel, "=", TMis2014_IsDel.ENormal.Int().ToString()));
                return filters;
            }
            catch(Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "!!Exception in getting filter for retrieving sub-delivery-order by sub-stockout-order.");
                return null;
            }
        }
    }
}
