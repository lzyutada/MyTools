using C_WMS.Interface.Utility;
using MisModel;
using System.Collections.Generic;
using MangoMis.Frame.ThirdFrame;
using MangoMis.Frame.Helper;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城中的出库订单（通过购物车结算购买）
    /// </summary>
    public class MangoStockouOrder : Product_Warehouse_ProductMainOutput, IMangoOrderBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MangoStockouOrder()
        {
        }

            /// <summary>
            /// constructd by order id
            /// </summary>
            /// <param name="id"></param>
        public MangoStockouOrder(string id)
        {
            Copy(MangoFactory.NewOrder<Product_Warehouse_ProductMainOutput>(id));
        }

        /// <summary>
        /// 构造函数，通过srcObj创建和构造实例
        /// </summary>
        /// <param name="srcObj">源订单实例</param>
        public MangoStockouOrder(Product_Warehouse_ProductMainOutput srcObj)
        {
            Copy(srcObj);
        }

        /// <summary>
        /// 通过Product_Warehouse_ProductMainOutput实例拷贝创建MangoExwarehouseOrder实例
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string Copy(Product_Warehouse_ProductMainOutput srcObj)
        {
            if (null != srcObj)
            {
                WarehouseId = srcObj.WarehouseId;
                CompanyTypeId = srcObj.CompanyTypeId;
                isprint = srcObj.isprint;
                Applymonth = srcObj.Applymonth;
                Remark = srcObj.Remark;
                GatherListId = srcObj.GatherListId;
                isdel = srcObj.isdel;
                iscandel = srcObj.iscandel;
                MapId = srcObj.MapId;
                AddTime = srcObj.AddTime;
                AddUserId = srcObj.AddUserId;
                JieShouRen = srcObj.JieShouRen;
                ChuKuRen = srcObj.ChuKuRen;
                ProductOutputMainCode = srcObj.ProductOutputMainCode;
                ProductOutputMainId = srcObj.ProductOutputMainId;
                MapClassId = srcObj.MapClassId;
                return string.Empty;
            }
            else
            {
                string errMsg = string.Format("MangoStockouOrder.Copy(), invalid null input param.");
                C_WMS.Data.Utility.MyLog.Instance.Warning(errMsg);
                return errMsg;
            }
        }


        /// <summary>
        /// IMangoOrderBase.GetId(), return id of order
        /// </summary>
        /// <returns></returns>
        public string GetId() { return ProductOutputMainId.ToString(); }

        /// <summary>
        /// overrided ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType(), ProductOutputMainId);
        }
    }
}
