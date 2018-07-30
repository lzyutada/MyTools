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
    public class MangoEntryOrder : Product_Warehouse_ProductMainInput, IMangoOrderBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MangoEntryOrder()
        {
        }

            /// <summary>
            /// construct by srcObj
            /// </summary>
            /// <param name="srcObj"></param>
        public MangoEntryOrder(Product_Warehouse_ProductMainInput srcObj)
        {
            Copy(srcObj);
        }

        /// <summary>
        /// construct by id
        /// </summary>
        /// <param name="id"></param>
        public MangoEntryOrder(string id)
        {
            Copy(MangoFactory.NewOrder<Product_Warehouse_ProductMainInput>(id));
        }

        /// <summary>
        /// 从源主订单srcOrder复制，相当于重载operator=
        /// </summary>
        /// <param name="srcObj">源主订单</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string Copy(Product_Warehouse_ProductMainInput srcObj)
        {
            if (null != srcObj)
            {
                ProductInputMainId = srcObj.ProductInputMainId;
                ProductInputMainCode = srcObj.ProductInputMainCode;
                YanShouRen = srcObj.YanShouRen;
                AddUserId = srcObj.AddUserId;
                AddTime = srcObj.AddTime;
                isdel = srcObj.isdel;
                WarehouseId = srcObj.WarehouseId;
                iscandel = srcObj.iscandel;
                Remark = srcObj.Remark;
                CompanyTypeId = srcObj.CompanyTypeId;
                isPrint = srcObj.isPrint;
                return string.Empty;
            }
            else
            {
                string errMsg = string.Format("MangoEntryOrder.Copy(), invalid null input param.");
                C_WMS.Data.Utility.MyLog.Instance.Warning(errMsg);
                return errMsg;
            }
        }

        /// <summary>
        /// IMangoOrderBase.GetId(), return id of order
        /// </summary>
        /// <returns></returns>
        public string GetId() { return ProductInputMainId.ToString(); }

        /// <summary>
        /// overrided ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType(), ProductInputMainId);
        }
    }
}
