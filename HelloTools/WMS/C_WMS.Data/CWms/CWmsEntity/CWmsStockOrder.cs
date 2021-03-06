﻿using C_WMS.Data.Mango.Data;
using C_WMS.Data.Mango.MisModelPWI;
using C_WMS.Data.Wms.Data;
using C_WMS.Data.Mango;
using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using MisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 出库订单
    /// </summary>
    class CWmsStockOrder : CWmsOrderBase<CWmsStockOrder, MangoStockouOrder, WmsStockoutOrder, CWmsSubStockoutOrder, CWmsStockoutOrderHandler>
    {
        /// <summary>
        /// overrided. 返回芒果商城中主入库订单的Id
        /// </summary>
        /// <returns></returns>
        public override string Id
        {
            get
            {
                return MangoOrder.ProductOutputMainId.Int().ToString();
            }
        }

        /// <summary>
        /// 获取C-WMS系统对应的出库单类型
        /// </summary>
        public TWmsOrderType WmsStockoutType
        {
            get
            {
                // 遍历所有子出库单所对应的订单类型
                int ptckCount = 0;
                int b2bckCount = 0;
                int qtckCount = 0;
                #region travers each sub order for ordertype
                foreach (var subOrder in SubOrders)
                {
                    switch (subOrder.Value?.MangoOrder.MallOrderType)
                    {
                        case TMangoOrderType.EBMDD: // 部门订单
                        case TMangoOrderType.EZSBD: { b2bckCount++; break; } // 装饰补单
                        case TMangoOrderType.EGRGM: { qtckCount++; break; }// 个人购买
                        case TMangoOrderType.EWLCK: { ptckCount++; break; }// 芒果网络为货主，无订单出库
                        default: { continue; } // 不支持其他订单类型
                    }
                }
                #endregion
                // 返回出库单类型
                if (SubOrders.Count == ptckCount) return TWmsOrderType.PTCK;
                else if (SubOrders.Count == b2bckCount) return TWmsOrderType.B2BCK;
                else if (SubOrders.Count == qtckCount) return TWmsOrderType.QTCK;
                else return TWmsOrderType.QTCK;
            }
        }

        /// <summary>
        /// 要求出库时间
        /// </summary>
        public string ScheculeDeliverDate
        {
            get
            {
                string ret = string.Empty;
                try
                {
                    ret = DateTime.MinValue.ToString();
                    // 取子出库单中最迟的要求配送时间
                    foreach (var o in SubOrders)
                    {
                        DateTime dtRet = DateTime.Parse(ret);
                        DateTime dtCur = DateTime.Parse((o.Value.MangoOrder as MangoSubStockoutOrder).ScheduleDate);
                        ret = (dtRet > dtCur) ? ret : (o.Value.MangoOrder as MangoSubStockoutOrder).ScheduleDate;
                    }
                }
                catch (Exception ex)
                {
                    var thirdRslt = new ThirdResult<List<object>>(string.Format(""));
                    if (null != ex.InnerException) thirdRslt.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                    thirdRslt.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                    thirdRslt.End();

                    ret = string.Empty;
                }
                return ret;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CWmsStockOrder()
        {
            OrderType = TCWmsOrderType.EStockoutOrder;
            MangoOrder = new MangoStockouOrder();
            WmsOrder = new WmsStockoutOrder();
        }

        protected override CWmsStockoutOrderHandler NewHandler()
        {
            return new CWmsStockoutOrderHandler();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class CWmsStockoutOrderHandler : CWmsOrderBaseHandlerBase<CWmsStockOrder, MangoStockouOrder, WmsStockoutOrder, CWmsSubStockoutOrder, CWmsStockoutOrderHandler>
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsStockoutOrderHandler() : base(TCWmsOrderType.EStockoutOrder)
        {
        }

        /// <summary>
        /// 根据主出库单ID获取实体
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        static public CWmsStockOrder NewOrder(string pId)
        {
            throw new NotImplementedException("");
        }

        /// <summary>
        /// get entity of WmsLogistics as the logistics of this stockout order represented by _order.
        /// -or- return null if failed in method executation.
        /// </summary>
        /// <returns></returns>
        public WmsLogistics GetLogistics(CWmsStockOrder pOrder)
        {
            return base.GetLogistics(pOrder);
        }
    } // class CWmsExWarehouseOrderHandler
}
