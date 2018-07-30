using MisModel;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城中的退货子订单
    /// </summary>
    public class MangoSubReturnOrder : Product_TuiHuo, IMangoOrderBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MangoSubReturnOrder() { }

        /// <summary>
        /// construct by id
        /// </summary>
        /// <param name="id"></param>
        public MangoSubReturnOrder(string id)
        {
            Copy(MangoFactory.NewOrder<Product_TuiHuo>(id));
        }

        /// <summary>
        /// 通过Product_TuiHuo实例拷贝创建MangoSubExwarehouseOrder实例
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public MangoSubReturnOrder(Product_TuiHuo srcObj)
        {
            Copy(srcObj);
        }

        /// <summary>
        /// MangoSubReturnOrder
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public string Copy(Product_TuiHuo srcObj)
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
                string errMsg = string.Format("MangoSubReturnOrder.Copy(), invalid null input param.");
                C_WMS.Data.Utility.MyLog.Instance.Warning(errMsg);
                return errMsg;
            }
        }

        /// <summary>
        /// IMangoOrderBase.GetId(), return id of order
        /// </summary>
        /// <returns></returns>
        public string GetId() { return ZiTuihuoID.ToString(); }

        /// <summary>
        /// overrided ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType(), ZiTuihuoID);
        }
    }
}
