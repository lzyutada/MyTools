using MisModel;
using System;
using System.Collections.Generic;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城中的主退货单
    /// </summary>
    public class MangoReturnOrder : Product_TuiHuo_main, IMangoOrderBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MangoReturnOrder()
        {
        }

        /// <summary>
        /// constructed by pRid
        /// </summary>
        /// <param name="pRid">order ID</param>
        public MangoReturnOrder(string pRid)
        {
            Copy(MangoFactory.NewOrder<Product_TuiHuo_main>(pRid));
        }

        /// <summary>
        /// 根据srcObj构造
        /// </summary>
        /// <param name="srcObj">源实体</param>
        public MangoReturnOrder(Product_TuiHuo_main srcObj)
        {
            Copy(srcObj);
        }

        /// <summary>
        /// 从源主订单srcOrder复制，相当于重载operator=
        /// </summary>
        /// <param name="srcObj">源主订单</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string Copy(Product_TuiHuo_main srcObj)
        {
            if (null != srcObj)
            {
                MapClassId = srcObj.MapClassId;
                WarehouseId = srcObj.WarehouseId;
                DictStairid = srcObj.DictStairid;
                LouCeng = srcObj.LouCeng;
                DuiJieRen = srcObj.DuiJieRen;
                DuiJieRenTel = srcObj.DuiJieRenTel;
                QuXiaoYuanYin = srcObj.QuXiaoYuanYin;
                ProductIOputMainId = srcObj.ProductIOputMainId;
                PrintCount = srcObj.PrintCount;
                PeiSongMoney = srcObj.PeiSongMoney;
                TuiHuoImage = srcObj.TuiHuoImage;
                IsDel = srcObj.IsDel;
                AddTime = srcObj.AddTime;
                UpdateTime = srcObj.UpdateTime;
                AddUserid = srcObj.AddUserid;
                QuHuoAddr = srcObj.QuHuoAddr;
                MapId = srcObj.MapId;
                DisOrder = srcObj.DisOrder;
                THMoney = srcObj.THMoney;
                TuiHuoMainID = srcObj.TuiHuoMainID;
                OrgID = srcObj.OrgID;
                ShenPiUserId = srcObj.ShenPiUserId;
                ZhuanTai = srcObj.ZhuanTai;
                DDProcessInstanceId = srcObj.DDProcessInstanceId;
                THYuanYin = srcObj.THYuanYin;
                UpdateUserID = srcObj.UpdateUserID;
                TuiHuoType = srcObj.TuiHuoType;
                DingDanID = srcObj.DingDanID;
                ShenPiTime = srcObj.ShenPiTime;
                QueRenTime = srcObj.QueRenTime;
                QueRenUserId = srcObj.QueRenUserId;
                ProductLingYongMainId = srcObj.ProductLingYongMainId;
                BeiZhu = srcObj.BeiZhu;
                THwuLiu = srcObj.THwuLiu;
                return string.Empty;
            }
            else
            {
                string errMsg = string.Format("MangoReturnOrder.Copy(), invalid null input param.");
                C_WMS.Data.Utility.MyLog.Instance.Warning(errMsg);
                return errMsg;
            }
        }

        /// <summary>
        /// get id of order
        /// </summary>
        /// <returns></returns>
        public string GetId() { return ProductLingYongMainId.ToString(); }

        /// <summary>
        /// overrided ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType(), ProductLingYongMainId);
        }
    }
}
