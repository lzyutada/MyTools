using MisModel;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城的子入库单类
    /// </summary>
    public class MangoSubEntryOrder : Product_Warehouse_ProductInput
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MangoSubEntryOrder() { }

        /// <summary>
        /// 根据Product_Warehouse_ProductInput的实例进行构造
        /// </summary>
        /// <param name="srcObj">源实例</param>
        public MangoSubEntryOrder(Product_Warehouse_ProductInput srcObj)
        {
            CopyFrom(srcObj);
        }


        /// <summary>
        /// 通过Product_Warehouse_ProductInput实例拷贝创建MangoSubEntryOrder实例
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public string CopyFrom(Product_Warehouse_ProductInput srcObj)
        {
            if (null != srcObj)
            {
                KuaiJiID = srcObj.KuaiJiID; // ret.Append("srcObj.KuaiJiID={0}", srcObj.KuaiJiID);
                 MainId = srcObj.MainId; // ret.Append("srcObj.MainId={0}", srcObj.MainId);
                Remark = srcObj.Remark; // ret.Append("srcObj.Remark={0}", srcObj.Remark);
                YanShouJieGuo = srcObj.YanShouJieGuo; // ret.Append("srcObj.YanShouJieGuo={0}", srcObj.YanShouJieGuo);
                ListType = srcObj.ListType; // ret.Append("srcObj.ListType={0}", srcObj.ListType);
                PeiSongRenYuan = srcObj.PeiSongRenYuan; // ret.Append("srcObj.PeiSongRenYuan={0}", srcObj.PeiSongRenYuan);
                UpdateUserID = srcObj.UpdateUserID; // ret.Append("srcObj.UpdateUserID={0}", srcObj.UpdateUserID);
                UpdateTime = srcObj.UpdateTime; // ret.Append("srcObj.UpdateTime={0}", srcObj.UpdateTime);
                IsDel = srcObj.IsDel; // ret.Append("srcObj.IsDel={0}", srcObj.IsDel);
                AddTime = srcObj.AddTime; // ret.Append("srcObj.AddTime={0}", srcObj.AddTime);
                AddUserid = srcObj.AddUserid; // ret.Append("srcObj.AddUserid={0}", srcObj.AddUserid);
                DisOrder = srcObj.DisOrder; // ret.Append("srcObj.DisOrder={0}", srcObj.DisOrder);
                InputKuKind = srcObj.InputKuKind; // ret.Append("srcObj.InputKuKind={0}", srcObj.InputKuKind);
                ProductInputState = srcObj.ProductInputState; // ret.Append("srcObj.ProductInputState={0}", srcObj.ProductInputState);
                ProductGuiGeID = srcObj.ProductGuiGeID; // ret.Append("srcObj.ProductGuiGeID={0}", srcObj.ProductGuiGeID);
                QueryPricePerson = srcObj.QueryPricePerson; // ret.Append("srcObj.QueryPricePerson={0}", srcObj.QueryPricePerson);
                YanShouRen = srcObj.YanShouRen; // ret.Append("srcObj.YanShouRen={0}", srcObj.YanShouRen);
                ProductMoney = srcObj.ProductMoney; // ret.Append("srcObj.ProductMoney={0}", srcObj.ProductMoney);
                ProductPrice = srcObj.ProductPrice; // ret.Append("srcObj.ProductPrice={0}", srcObj.ProductPrice);
                ProductInputCount = srcObj.ProductInputCount; // ret.Append("srcObj.ProductInputCount={0}", srcObj.ProductInputCount);
                WarehouseId = srcObj.WarehouseId; // ret.Append("srcObj.WarehouseId={0}", srcObj.WarehouseId);
                ProductId = srcObj.ProductId; // ret.Append("ProductId={0}", ProductId);
                ProductInputDate = srcObj.ProductInputDate; // ret.Append("srcObj.ProductInputDate={0}", srcObj.ProductInputDate);
                ProductBuyId = srcObj.ProductBuyId; // ret.Append("srcObj.ProductBuyId={0}", srcObj.ProductBuyId);
                ProductBuyCode = srcObj.ProductBuyCode; // ret.Append("srcObj.ProductBuyCode={0}", srcObj.ProductBuyCode);
                ProductInputCode = srcObj.ProductInputCode; // ret.Append("srcObj.ProductInputCode={0}", srcObj.ProductInputCode);
                ProductInputId = srcObj.ProductInputId; // ret.Append("srcObj.ProductInputId={0}", srcObj.ProductInputId);
                SupplierId = srcObj.SupplierId; // ret.Append("srcObj.SupplierId={0}", srcObj.SupplierId);
                return string.Empty;
            }
            else
            {
                return "源实例srcObj为null。";
            }
        }
    }
}
