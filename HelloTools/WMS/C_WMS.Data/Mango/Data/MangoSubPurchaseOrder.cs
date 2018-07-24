using System;
using System.Collections.Generic;
using System.Linq;
using MisModel;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城中的采购子订单
    /// </summary>
    public class MangoSubPurchaseOrder : Product_Warehouse_ProductBuy
    {
        #region Properties
        //public double lowestPrice = 0.0;
        //public bool approval = false;
        //public string status = string.Empty;
        //public string remark = string.Empty;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public MangoSubPurchaseOrder() { }

        /// <summary>
        /// 根据Product_Warehouse_ProductBuy的实例进行构造
        /// </summary>
        /// <param name="srcObj">源实例</param>
        public MangoSubPurchaseOrder(Product_Warehouse_ProductBuy srcObj)
        {
            if (null != srcObj)
            {
                QueryPricePerson = srcObj.QueryPricePerson;
                ChangeRemark = srcObj.ChangeRemark;
                ProductInputState = srcObj.ProductInputState;
                SupplierId = srcObj.SupplierId;
                DisOrder = srcObj.DisOrder;
                AddUserid = srcObj.AddUserid;
                AddTime = srcObj.AddTime;
                IsDel = srcObj.IsDel;
                UpdateTime = srcObj.UpdateTime;
                UpdateUserID = srcObj.UpdateUserID;
                PurchasingSupervisor = srcObj.PurchasingSupervisor;
                PurchasingManager = srcObj.PurchasingManager;
                MainId = srcObj.MainId;
                ProductCategoryId = srcObj.ProductCategoryId;
                isSupplierPeiSong = srcObj.isSupplierPeiSong;
                IsChange = srcObj.IsChange;
                YanShouRen = srcObj.YanShouRen;
                ProductGuiGeID = srcObj.ProductGuiGeID;
                Remark = srcObj.Remark;
                ProductBuyId = srcObj.ProductBuyId;
                ProductBuyCode = srcObj.ProductBuyCode;
                ProductInputDate = srcObj.ProductInputDate;
                ProductId_Plan = srcObj.ProductId_Plan;
                ProductCount_Plan = srcObj.ProductId_Plan;
                ProductInputMainId = srcObj.ProductInputMainId;
                ProductPrice_Plan = srcObj.ProductPrice_Plan;
                ProductId = srcObj.ProductId;
                ProductCount = srcObj.ProductCount;
                ProductPrice = srcObj.ProductPrice;
                ProductMoney = srcObj.ProductMoney;
                ProductCount_Already = srcObj.ProductCount_Already;
                UseKindId = srcObj.UseKindId;
                ProductMoney_Plan = srcObj.ProductMoney_Plan;
            }
        }

        /// <summary>
        /// 通过Product_Warehouse_ProductInput实例拷贝创建MangoSubEntryOrder实例
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty; 否则返回错误描述</returns>
        public string CreateFrom(Product_Warehouse_ProductBuy srcObj)
        {
            if (null != srcObj)
            {
                QueryPricePerson = srcObj.QueryPricePerson;
                ChangeRemark = srcObj.ChangeRemark;
                ProductInputState = srcObj.ProductInputState;
                SupplierId = srcObj.SupplierId;
                DisOrder = srcObj.DisOrder;
                AddUserid = srcObj.AddUserid;
                AddTime = srcObj.AddTime;
                IsDel = srcObj.IsDel;
                UpdateTime = srcObj.UpdateTime;
                UpdateUserID = srcObj.UpdateUserID;
                PurchasingSupervisor = srcObj.PurchasingSupervisor;
                PurchasingManager = srcObj.PurchasingManager;
                MainId = srcObj.MainId;
                ProductCategoryId = srcObj.ProductCategoryId;
                isSupplierPeiSong = srcObj.isSupplierPeiSong;
                IsChange = srcObj.IsChange;
                YanShouRen = srcObj.YanShouRen;
                ProductGuiGeID = srcObj.ProductGuiGeID;
                Remark = srcObj.Remark;
                ProductBuyId = srcObj.ProductBuyId;
                ProductBuyCode = srcObj.ProductBuyCode;
                ProductInputDate = srcObj.ProductInputDate;
                ProductId_Plan = srcObj.ProductId_Plan;
                ProductCount_Plan = srcObj.ProductId_Plan;
                ProductInputMainId = srcObj.ProductInputMainId;
                ProductPrice_Plan = srcObj.ProductPrice_Plan;
                ProductId = srcObj.ProductId;
                ProductCount = srcObj.ProductCount;
                ProductPrice = srcObj.ProductPrice;
                ProductMoney = srcObj.ProductMoney;
                ProductCount_Already = srcObj.ProductCount_Already;
                UseKindId = srcObj.UseKindId;
                ProductMoney_Plan = srcObj.ProductMoney_Plan;
                return string.Empty;
            }
            else
            {
                return "源实例srcObj为null。";
            }
        }
    }
}
