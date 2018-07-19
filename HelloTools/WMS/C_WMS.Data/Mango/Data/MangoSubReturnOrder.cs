using MisModel;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城中的退货子订单
    /// </summary>
    public class MangoSubReturnOrder : Product_TuiHuo
    {
        #region
        ///// <summary>
        ///// 原交易平台订单，即芒果商城主订单
        ///// </summary>
        //protected MangoExwarehouseOrder order = null;

        ///// <summary>
        ///// 获取原交易平台订单
        ///// </summary>
        //public MangoExwarehouseOrder Order { get { return order; } }

        ///// <summary>
        ///// 交易平台子订单，即芒果商城子订单
        ///// </summary>
        //protected MangoSubExwarehouseOrder subOrder = null;

        ///// <summary>
        ///// 获取交易平台子订单
        ///// </summary>
        //public MangoSubExwarehouseOrder SubOrder { get { return subOrder; } }

        ///// <summary>
        ///// 应收商品数量
        ///// </summary>
        //public int planQty = 0;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public MangoSubReturnOrder() { }

        /// <summary>
        /// 通过Product_TuiHuo实例拷贝创建MangoSubExwarehouseOrder实例
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public MangoSubReturnOrder(Product_TuiHuo srcObj)
        {
            CopyFrom(srcObj);
        }

        /// <summary>
        /// MangoSubReturnOrder
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public string CopyFrom(Product_TuiHuo srcObj)
        {
            if (null != srcObj)
            {
                ProductLifeId = srcObj.ProductLifeId;
                ChildZhuangTai = srcObj.ChildZhuangTai;
                DisOrder = srcObj.DisOrder;
                UpdateUserID = srcObj.UpdateUserID;
                AddUserid = srcObj.AddUserid;
                UpdateTime = srcObj.UpdateTime;
                AddTime = srcObj.AddTime;
                IsDel = srcObj.IsDel;
                DDMoney = srcObj.DDMoney;
                ProductIOputId = srcObj.ProductIOputId;
                memo = srcObj.memo;
                ZiDingDanID = srcObj.ZiDingDanID;
                EerShouMoney = srcObj.EerShouMoney;
                TuiHuoMainID = srcObj.TuiHuoMainID;
                isOver = srcObj.isOver;
                ProductCount = srcObj.ProductCount;
                ProductId = srcObj.ProductId;
                ZiTuihuoID = srcObj.ZiTuihuoID;
                ProductGuiGeID = srcObj.ProductGuiGeID;
                return string.Empty;
            }
            else
            {
                return "源实例srcObj为null。";
            }
        }
    }
}
