using MisModel;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城的子入库单类
    /// </summary>
    public class MangoSubEntryOrder : Product_Warehouse_ProductInput, IMangoOrderBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MangoSubEntryOrder() { }


        /// <summary>
        /// construct by srcObj
        /// </summary>
        /// <param name="id"></param>
        public MangoSubEntryOrder(string id)
        {
            Copy(MangoFactory.NewOrder<Product_Warehouse_ProductInput>(id));
        }


        /// <summary>
        /// 根据Product_Warehouse_ProductInput的实例进行构造
        /// </summary>
        /// <param name="srcObj">源实例</param>
        public MangoSubEntryOrder(Product_Warehouse_ProductInput srcObj)
        {
            Copy(srcObj);
        }

        /// <summary>
        /// 通过Product_Warehouse_ProductInput实例拷贝创建MangoSubEntryOrder实例
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public string Copy(Product_Warehouse_ProductInput srcObj)
        {
            if (null != srcObj)
            {
                KuaiJiID = srcObj.KuaiJiID;
                MainId = srcObj.MainId;
                Remark = srcObj.Remark;
                YanShouJieGuo = srcObj.YanShouJieGuo;
                ListType = srcObj.ListType;
                PeiSongRenYuan = srcObj.PeiSongRenYuan;
                UpdateUserID = srcObj.UpdateUserID;
                UpdateTime = srcObj.UpdateTime;
                IsDel = srcObj.IsDel;
                AddTime = srcObj.AddTime;
                AddUserid = srcObj.AddUserid;
                DisOrder = srcObj.DisOrder;
                InputKuKind = srcObj.InputKuKind;
                ProductInputState = srcObj.ProductInputState;
                ProductGuiGeID = srcObj.ProductGuiGeID;
                QueryPricePerson = srcObj.QueryPricePerson;
                YanShouRen = srcObj.YanShouRen;
                ProductMoney = srcObj.ProductMoney;
                ProductPrice = srcObj.ProductPrice;
                ProductInputCount = srcObj.ProductInputCount;
                WarehouseId = srcObj.WarehouseId;
                ProductId = srcObj.ProductId;
                ProductInputDate = srcObj.ProductInputDate;
                ProductBuyId = srcObj.ProductBuyId;
                ProductBuyCode = srcObj.ProductBuyCode;
                ProductInputCode = srcObj.ProductInputCode;
                ProductInputId = srcObj.ProductInputId;
                SupplierId = srcObj.SupplierId;
                return string.Empty;
            }
            else
            {
                string errMsg = string.Format("MangoSubEntryOrder.Copy(), invalid null input param.");
                C_WMS.Data.Utility.MyLog.Instance.Warning(errMsg);
                return errMsg;
            }
        }

        /// <summary>
        /// IMangoOrderBase.GetId(), return id of order
        /// </summary>
        /// <returns></returns>
        public string GetId() { return ProductInputId.ToString(); }

        /// <summary>
        /// overrided ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType(), ProductInputId);
        }
    }
}
