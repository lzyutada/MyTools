using System;
using System.Collections.Generic;
using System.Linq;
using MisModel;
using MangoMis.Frame.ThirdFrame;
using MangoMis.Frame.Helper;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城中的采购单
    /// </summary>
    public class MangoPurchaseOrder : Product_Warehouse_ProductMainBuy
    {
        #region Properties
        //public CWmsAgentBase buyer;
        //public CWmsAgentBase purchaseMgr;
        //public CWmsAgentBase confirmer;
        //public bool approval;
        //public DateTime approvalTime;

        /// <summary>
        /// 获取子入库单列表
        /// </summary>
        public Dictionary<string, MangoSubPurchaseOrder> SubOrders { get { return mSubOrders; } }

        /// <summary>
        /// 子入库单列表
        /// </summary>
        private Dictionary<string, MangoSubPurchaseOrder> mSubOrders = null;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public MangoPurchaseOrder()
        {
            mSubOrders = new Dictionary<string, MangoSubPurchaseOrder>(1);
        }

        /// <summary>
        /// 从源主订单srcOrder复制，相当于重载operator=
        /// </summary>
        /// <param name="srcObj">源主订单</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string CopyFrom(Product_Warehouse_ProductMainBuy srcObj)
        {
            if (null != srcObj)
            {
                ProductBuyMainId = srcObj.ProductBuyMainId;
                ProductBuyMainCode = srcObj.ProductBuyMainCode;
                CaiGouRen = srcObj.CaiGouRen;
                CaiGouZhuGuan = srcObj.CaiGouZhuGuan;
                CaiGouJingLi = srcObj.CaiGouJingLi;
                AddUserId = srcObj.AddUserId;
                AddTime = srcObj.AddTime;
                isdel = srcObj.isdel;
                ispizhun = srcObj.ispizhun;
                PiZhunTime = srcObj.PiZhunTime;
                GatherListId = srcObj.GatherListId;
                Remark = srcObj.Remark;
                isSupplierPeiSong = srcObj.isSupplierPeiSong;
                return string.Empty;
            }
            else
            {
                return "源实例srcObj为null。";
            }
        }

        ///// <summary>
        ///// 将源子订单列表List[MangoSubEntryOrder]中的子订单项目复制到SubOrderList。
        ///// 该方法返回成功复制的子订单数目
        ///// </summary>
        ///// <param name="pList">源子订单列表</param>
        ///// <returns>返回成功复制的子订单数目</returns>
        //public int SetSubOrderList(List<MangoSubPurchaseOrder> pList)
        //{
        //    if (null == pList)
        //        return int.Parse(TError.Pro_HaveNoData.ToString());

        //    // clear current sub order list
        //    SubOrders.Clear();

        //    foreach (MangoSubPurchaseOrder o in pList)
        //    {
        //        SubOrders.Add(TypeHelper.IntConvert(o.ProductBuyId).ToString(), o);
        //    }
        //    return SubOrders.Count;
        //}
    }
}
