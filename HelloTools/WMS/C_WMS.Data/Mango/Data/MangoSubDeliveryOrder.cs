using MisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Mango.Data
{
    class MangoSubDeliveryOrder: Product_PeiSong_Product, IMangoOrderBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MangoSubDeliveryOrder()
        {
        }

        /// <summary>
        /// construct by srcObj
        /// </summary>
        /// <param name="srcObj"></param>
        public MangoSubDeliveryOrder(Product_PeiSong_Product srcObj)
        {
            Copy(srcObj);
        }

        /// <summary>
        /// construct by id
        /// </summary>
        /// <param name="id"></param>
        public MangoSubDeliveryOrder(string id)
        {
            Copy(MangoFactory.NewOrder<Product_PeiSong_Product>(id));
        }

        /// <summary>
        /// 从源主订单srcOrder复制，相当于重载operator=
        /// </summary>
        /// <param name="srcObj">源主订单</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string Copy(Product_PeiSong_Product srcObj)
        {
            if (null != srcObj)
            {
                #region 
                AddTime = srcObj.AddTime;
                AddUserid = srcObj.AddUserid;
                BeiZhu = srcObj.BeiZhu;
                DisOrder = srcObj.DisOrder;
                IsDel = srcObj.IsDel;
                isErShou = srcObj.isErShou;
                OrgID = srcObj.OrgID;
                PeiSongTime = srcObj.PeiSongTime;
                ProductCount = srcObj.ProductCount;
                ProductGuiGeID = srcObj.ProductGuiGeID;
                ProductId = srcObj.ProductId;
                ProductInputId = srcObj.ProductInputId;
                ProductIOputId = srcObj.ProductIOputId;
                ProductLifeId = srcObj.ProductLifeId;
                ProductLingYongId = srcObj.ProductLingYongId;
                ProductLingYongMainId = srcObj.ProductLingYongMainId;
                ProductMoney = srcObj.ProductMoney;
                SupplierId = srcObj.SupplierId;
                Supplier_JieSuan_Time = srcObj.Supplier_JieSuan_Time;
                Supplier_JieSuan_ZhuangTai = srcObj.Supplier_JieSuan_ZhuangTai;
                UpdateTime = srcObj.UpdateTime;
                UpdateUserID = srcObj.UpdateUserID;
                WarehouseId = srcObj.WarehouseId;
                WarehouseKeeper = srcObj.WarehouseKeeper;
                ZhuangTai = srcObj.ZhuangTai;
                ZiDingDanID = srcObj.ZiDingDanID;
                ZiIsOver = srcObj.ZiIsOver;
                #endregion
                return string.Empty;
            }
            else
            {
                string errMsg = string.Format("MangoSubDeliveryOrder.Copy(), invalid null input param.");
                C_WMS.Data.Utility.MyLog.Instance.Warning(errMsg);
                return errMsg;
            }
        }

        /// <summary>
        /// IMangoOrderBase.GetId(), return id of order
        /// </summary>
        /// <returns></returns>
        public string GetId() { return ProductLingYongId.ToString(); }

        /// <summary>
        /// overrided ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType(), ProductLingYongId);
        }
    }
}
