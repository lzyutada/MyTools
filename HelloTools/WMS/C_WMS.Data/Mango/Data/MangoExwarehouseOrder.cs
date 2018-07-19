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
    public class MangoExwarehouseOrder : Product_Warehouse_ProductMainOutput
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MangoExwarehouseOrder()
        {
        }

        /// <summary>
        /// 构造函数，通过srcObj创建和构造实例
        /// </summary>
        /// <param name="srcObj">源订单实例</param>
        public MangoExwarehouseOrder(Product_Warehouse_ProductMainOutput srcObj)
        {
            if (null == srcObj)
                return;

            CopyFrom(srcObj);
        }

        /// <summary>
        /// 通过Product_Warehouse_ProductMainOutput实例拷贝创建MangoExwarehouseOrder实例
        /// </summary>
        /// <param name="srcObj">源实例</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string CopyFrom(Product_Warehouse_ProductMainOutput srcObj)
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
                return "源实例srcObj为null。";
            }
        }
    }
}
