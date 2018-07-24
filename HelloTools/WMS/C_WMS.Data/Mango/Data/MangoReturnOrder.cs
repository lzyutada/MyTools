using MisModel;
using System;
using System.Collections.Generic;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城中的主退货单
    /// </summary>
    public class MangoReturnOrder : Product_TuiHuo_main
    {
        #region Properties
        //public CWmsExWarehouseOrder orgExwarehouseOrder;
        //public int logistics;
        //public CWmsAgentBase sender;
        //public string reason;
        //public string remark;
        //public TMangoReturnType returnType = TMangoReturnType.EDefaultType;
        //public TMangoReturnLogisticsType logisticsType = TMangoReturnLogisticsType.EDefaultType;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        public MangoReturnOrder()
        {
        }

        /// <summary>
        /// 根据srcObj构造
        /// </summary>
        /// <param name="srcObj">源实体</param>
        public MangoReturnOrder(Product_TuiHuo_main srcObj)
        {
            CopyFrom(srcObj);
        }

        /// <summary>
        /// 从源主订单srcOrder复制，相当于重载operator=
        /// </summary>
        /// <param name="srcObj">源主订单</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string CopyFrom(Product_TuiHuo_main srcObj)
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
                return "源实例srcObj为null。";
            }
        }

    }
}
