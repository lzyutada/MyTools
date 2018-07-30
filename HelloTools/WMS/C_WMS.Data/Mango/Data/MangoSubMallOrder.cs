using MisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Mango.Data
{
    class MangoSubMallOrder : Product_User_DingDan_ProductList, IMangoOrderBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MangoSubMallOrder()
        {
        }

        /// <summary>
        /// construct by srcObj
        /// </summary>
        /// <param name="srcObj"></param>
        public MangoSubMallOrder(Product_User_DingDan_ProductList srcObj)
        {
            Copy(srcObj);
        }

        /// <summary>
        /// construct by id
        /// </summary>
        /// <param name="id"></param>
        public MangoSubMallOrder(string id)
        {
            Copy(MangoFactory.NewOrder<Product_User_DingDan_ProductList>(id));
        }

        /// <summary>
        /// 从源主订单srcOrder复制，相当于重载operator=
        /// </summary>
        /// <param name="srcObj">源主订单</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string Copy(Product_User_DingDan_ProductList srcObj)
        {
            if (null != srcObj)
            {
                #region 
                AddTime = srcObj.AddTime;
                AddUserid = srcObj.AddUserid;
                AnZhuangMapID = srcObj.AnZhuangMapID;
                AnzhuanTime = srcObj.AnzhuanTime;
                BeiZhu = srcObj.BeiZhu;
                DingDanID = srcObj.DingDanID;
                DingDanType = srcObj.DingDanType;
                DisOrder = srcObj.DisOrder;
                isAnZhuang = srcObj.isAnZhuang;
                IsDel = srcObj.IsDel;
                isErShou = srcObj.isErShou;
                IsGongYingShang = srcObj.IsGongYingShang;
                IsJD = srcObj.IsJD;
                IsKuFang = srcObj.IsKuFang;
                isOver = srcObj.isOver;
                LiangChiTime = srcObj.LiangChiTime;
                OldZiDingDanID = srcObj.OldZiDingDanID;
                OrgID = srcObj.OrgID;
                PeiSongCount = srcObj.PeiSongCount;
                PeiSongEndTime = srcObj.PeiSongEndTime;
                PeiSongTime = srcObj.PeiSongTime;
                ProductCount = srcObj.ProductCount;
                ProductGuiGeID = srcObj.ProductGuiGeID;
                ProductId = srcObj.ProductId;
                ProductLifeId = srcObj.ProductLifeId;
                ProductMoney = srcObj.ProductMoney;
                UpdateTime = srcObj.UpdateTime;
                UpdateUserID = srcObj.UpdateUserID;
                ZhuangTai = srcObj.ZhuangTai;
                ZiDingDanID = srcObj.ZiDingDanID;
                #endregion
                return string.Empty;
            }
            else
            {
                string errMsg = string.Format("MangoSubMallOrder.Copy(), invalid null input param.");
                C_WMS.Data.Utility.MyLog.Instance.Warning(errMsg);
                return errMsg;
            }
        }

        /// <summary>
        /// IMangoOrderBase.GetId(), return id of order
        /// </summary>
        /// <returns></returns>
        public string GetId() { return ZiDingDanID.ToString(); }

        /// <summary>
        /// overrided ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType(), ZiDingDanID);
        }
    }
}
