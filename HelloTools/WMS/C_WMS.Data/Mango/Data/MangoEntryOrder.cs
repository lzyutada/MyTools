using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using MisModel;
using System;
using System.Collections.Generic;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城主入库单类
    /// </summary>
    public class MangoEntryOrder : Product_Warehouse_ProductMainInput
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MangoEntryOrder()
        {
        }

        /// <summary>
        /// 从源主订单srcOrder复制，相当于重载operator=
        /// </summary>
        /// <param name="srcObj">源主订单</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string CopyFrom(Product_Warehouse_ProductMainInput srcObj)
        {
            if (null != srcObj)
            {
                ProductInputMainId = srcObj.ProductInputMainId; // ret.Append("srcObj.ProductInputMainId={0}", srcObj.ProductInputMainId);
                ProductInputMainCode = srcObj.ProductInputMainCode; // ret.Append("srcObj.ProductInputMainCode={0}", srcObj.ProductInputMainCode);
                YanShouRen = srcObj.YanShouRen; // ret.Append("srcObj.YanShouRen={0}", srcObj.YanShouRen);
                AddUserId = srcObj.AddUserId; // ret.Append("srcObj.AddUserId={0}", srcObj.AddUserId);
                AddTime = srcObj.AddTime; // ret.Append("srcObj.AddTime={0}", srcObj.AddTime);
                isdel = srcObj.isdel; // ret.Append("srcObj.isdel={0}", srcObj.isdel);
                WarehouseId = srcObj.WarehouseId; // ret.Append("srcObj.WarehouseId={0}", srcObj.WarehouseId);
                iscandel = srcObj.iscandel; // ret.Append("srcObj.iscandel={0}", srcObj.iscandel);
                Remark = srcObj.Remark; // ret.Append("srcObj.Remark={0}", srcObj.Remark);
                CompanyTypeId = srcObj.CompanyTypeId; // ret.Append("srcObj.CompanyTypeId={0}", srcObj.CompanyTypeId);
                isPrint = srcObj.isPrint; // ret.Append("srcObj.isPrint={0}", srcObj.isPrint);
                return string.Empty;
            }
            else
            {
                return "源实例srcObj为null。";
            }
        }
    }
}
