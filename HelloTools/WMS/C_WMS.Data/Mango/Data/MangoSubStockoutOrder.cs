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
    class MangoSubStockoutOrder : Product_Warehouse_ProductOutput, IMangoOrderBase
    {
        #region Properties
        /// <summary>
        /// handler of MangoSubStockoutOrderHandler
        /// </summary>
        public MangoSubStockoutOrderHandler Handler = new MangoSubStockoutOrderHandler();

        /// <summary>
        /// 商城中子订单关联的子配送单ID
        /// </summary>
        public string SubDeliveryOrderId => (null != Handler) ? Handler.GetSubDeliveryOrder(this).GetId() : string.Empty;

        /// <summary>
        /// 商城中mSubDeliveryOrderId关联的商城子订单Id
        /// </summary>
        public string SubMallOrderId => (null != Handler) ? Handler.GetSubMallOrder(this).GetId() : string.Empty;

        /// <summary>
        /// 商城中mSubMallOrderId关联的商城主订单Id
        /// </summary>
        public string MallOrderId => (null != Handler) ? Handler.GetMallOrder(this).GetId() : string.Empty;

        /// <summary>
        /// 其对应的商城订单的订单类型
        /// </summary>
        public TMangoOrderType MallOrderType => (null != Handler) ? MangoFactory.To_TMangoOrderType((Handler.GetSubMallOrder(this) as MangoSubMallOrder).DingDanType.Int()) : TMangoOrderType.EDefaultType;

        /// <summary>
        /// 要求出库时间
        /// </summary>
        public string ScheduleDate => (null != Handler) ? Handler.GetScheduleDate(this) : DateTime.MaxValue.ToString("yyyy-MM-dd 00:00:00");

        /// <summary>
        /// 应收商品数量
        /// </summary>
        public decimal PlanQuantity => (null != Handler) ? Handler.GetPlanQuantity(this) : 0M;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public MangoSubStockoutOrder()
        {
        }

        /// <summary>
        /// construct by id
        /// </summary>
        /// <param name="id"></param>
        public MangoSubStockoutOrder(string id)
        {
            Copy(MangoFactory.NewOrder<Product_Warehouse_ProductOutput>(id));
        }

        /// <summary>
        /// 通过Product_Warehouse_ProductOutput实例拷贝创建MangoSubExwarehouseOrder实例
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public MangoSubStockoutOrder(Product_Warehouse_ProductOutput srcObj)
        {
            Copy(srcObj);
        }

        /// <summary>
        /// 根据Product_Warehouse_ProductOutput的实例进行构造
        /// </summary>
        /// <param name="srcObj">源实例</param>
        public string Copy(Product_Warehouse_ProductOutput srcObj)
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
                return string.Empty;
            }
            else
            {
                string errMsg = string.Format("MangoSubStockoutOrder.Copy(), invalid null input param.");
                C_WMS.Data.Utility.MyLog.Instance.Warning(errMsg);
                return errMsg;
            }
        }

        /// <summary>
        /// IMangoOrderBase.GetId(), return id of order
        /// </summary>
        /// <returns></returns>
        public string GetId() { return ProductOutputId.ToString(); }

        /// <summary>
        /// overrided ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType(), ProductOutputId);
        }        
    }

    /// <summary>
    /// handler of MangoSubExwarehouseOrder
    /// </summary>
    class MangoSubStockoutOrderHandler
    {
        /// <summary>
        /// init and return a list of filter for retrieving the sub-delivery order for pOrder.
        /// </summary>
        /// <param name="pOrder"></param>
        /// <returns></returns>
        public List<CommonFilterModel> NewFilter_GetDeliveryOrder(MangoSubStockoutOrder pOrder)
        {
            try
            {
                if (null == pOrder)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("MangoSubStockoutOrderHandler.NewFilter_GetDeliveryOrder(), invalid null param.");
                    return null;
                }
                List<CommonFilterModel> filters = new List<CommonFilterModel>(2);
                filters.Add(new CommonFilterModel(Mis2014_SubDeliveryOrder_Column.ProductIOputId, "=", pOrder.ProductOutputId.Int().ToString()));
                filters.Add(new CommonFilterModel(Mis2014_SubDeliveryOrder_Column.IsDel, "=", TMis2014_IsDel.ENormal.Int().ToString()));
                return filters;
            }
            catch(Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "Exception in MangoSubStockoutOrderHandler.NewFilter_GetDeliveryOrder(), pOrder={0}", pOrder);
                return null;
            }
        }

        /// <summary>
        /// return the sub-delivery order of pOrder, or null if failed.
        /// </summary>
        /// <param name="pOrder">object of sub-stockout order.</param>
        /// <returns></returns>
        public IMangoOrderBase GetSubDeliveryOrder(MangoSubStockoutOrder pOrder)
        {
            if (null == pOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("MangoSubStockoutOrderHandler.GetSubDeliveryOrder(), invalid null param.");
                return null;
            }
            try
            {
                string errMsg = string.Empty;
                MangoSubDeliveryOrder retOrder = null;
                List<MangoSubDeliveryOrder> sdList = null;
                int err = MangoFactory.GetV_Order(NewFilter_GetDeliveryOrder(pOrder), out sdList, out errMsg);
                if (1 == err)
                {
                    retOrder = sdList[0];
                }
                else
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("MangoSubStockoutOrderHandler.GetSubDeliveryOrder({0}), failed in retrieving sub-delivery order. err={1}, errMsg={2}", pOrder, err, errMsg);
                }
                if (null != sdList) sdList.Clear();
                return retOrder;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "Exception in MangoSubStockoutOrderHandler.GetSubDeliveryOrder({0})", pOrder);
                return null;
            }
        }

        /// <summary>
        /// 获取与子出库单pOrder关联的商城子订单，如果执行失败则返回null.
        /// </summary>
        /// <param name="pOrder">子出库单对象</param>
        public IMangoOrderBase GetSubMallOrder(MangoSubStockoutOrder pOrder)
        {
            if (null == pOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("MangoSubStockoutOrderHandler.GetSubMallOrder(), invalid null param.");
                return null;
            }

            try
            {
                var delivery = GetSubDeliveryOrder(pOrder);
                if (null == delivery as MangoSubDeliveryOrder)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("MangoSubStockoutOrderHandler.GetSubMallOrder({0}), COULDNOT retrieve sub-delivery order.", pOrder);
                    return null;
                }
                else
                {
                    var misEntity = MangoFactory.NewOrder<Product_User_DingDan_ProductList>((delivery as MangoSubDeliveryOrder).ZiDingDanID.ToString());
                    return (null == misEntity) ? null : new MangoSubMallOrder(misEntity);
                }
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "Exception in MangoSubStockoutOrderHandler.GetSubMallOrder({0})", pOrder);
                return null;
            }
        }
        
        /// <summary>
        /// 获取与pOrder关联的商城主订单Id
        /// </summary>
        public IMangoOrderBase GetMallOrder(MangoSubStockoutOrder pOrder)
        {
            if (null == pOrder)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("MangoSubStockoutOrderHandler.GetMallOrder(), invalid null param.");
                return null;
            }

            try
            {
                var subOrder = GetSubMallOrder(pOrder);
                if (null == subOrder as MangoSubMallOrder)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("MangoSubStockoutOrderHandler.GetMallOrder({0}), COULDNOT retrieve sub-delivery order.", pOrder);
                    return null;
                }
                else
                {
                    var misEntity = MangoFactory.NewOrder<Product_User_DingDan>((subOrder as MangoSubMallOrder).DingDanID.ToString());
                    return (null == misEntity) ? null : new MangoMallOrder(misEntity);
                }
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "Exception in MangoSubStockoutOrderHandler.GetMallOrder({0})", pOrder);
                return null;
            }
        }
        
        /// <summary>
        /// 要求出库时间
        /// </summary>
        public string GetScheduleDate(MangoSubStockoutOrder pOrder)
        {
            var subMallOrder = GetSubMallOrder(pOrder) as MangoSubMallOrder;
            return (null != subMallOrder) ? subMallOrder.PeiSongTime.ToString() : DateTime.MaxValue.ToString();
        }

        /// <summary>
        /// 应收商品数量
        /// </summary>
        public decimal GetPlanQuantity(MangoSubStockoutOrder pOrder)
        {
            var subMallOrder = GetSubMallOrder(pOrder) as MangoSubMallOrder;
            return (null != subMallOrder) ? subMallOrder.ProductCount.Decimal() : 0M;
        }
    }
}
