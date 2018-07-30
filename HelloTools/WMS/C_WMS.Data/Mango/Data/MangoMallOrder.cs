using MisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 芒果商城的商城订单类
    /// </summary>
    class MangoMallOrder : Product_User_DingDan, IMangoOrderBase
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public MangoMallOrder() { }

        /// <summary>
        /// construct by id
        /// </summary>
        /// <param name="id"></param>
        public MangoMallOrder(string id)
        {
            Copy(MangoFactory.NewOrder<Product_User_DingDan>(id));
        }

        /// <summary>
        /// 根据pSrcObj创建实体
        /// </summary>
        /// <param name="pSrcObj">源实体</param>
        public MangoMallOrder(Product_User_DingDan pSrcObj)
        {
            Copy(pSrcObj);
        }

        /// <summary>
        /// 从Product_User_DingDan实体中拷贝数据
        /// </summary>
        /// <param name="pSrc">源实体</param>
        /// <returns>若成功则返回string.Empty；否则返回错误描述</returns>
        public string Copy(Product_User_DingDan pSrc)
        {
            if (null == pSrc)
            {
                string errMsg = string.Format("MangoMallOrder.Copy(), invalid null input param.");
                C_WMS.Data.Utility.MyLog.Instance.Warning(errMsg);
                return errMsg;
            }
            UpdateTime = pSrc.UpdateTime;
            MapClassId = pSrc.MapClassId;
            MapId = pSrc.MapId;
            DeliveryAddr = pSrc.DeliveryAddr;
            WarehouseIdPoint = pSrc.WarehouseIdPoint;
            DictStairid = pSrc.DictStairid;
            LouCeng = pSrc.LouCeng;
            ShouHuoRen = pSrc.ShouHuoRen;
            ShouHuoRenTel = pSrc.ShouHuoRenTel;
            DDProcessInstanceId = pSrc.DDProcessInstanceId;
            IsGongYingShang = pSrc.IsGongYingShang;
            IsKuFang = pSrc.IsKuFang;
            IsErShou = pSrc.IsErShou;
            isInCar = pSrc.isInCar;
            DisOrder = pSrc.DisOrder;
            UpdateUserID = pSrc.UpdateUserID;
            OldDingDanID = pSrc.OldDingDanID;
            AddUserid = pSrc.AddUserid;
            DingDanID = pSrc.DingDanID;
            OrgID = pSrc.OrgID;
            IsShenPi = pSrc.IsShenPi;
            ShenPiUserId = pSrc.ShenPiUserId;
            ZhuanTai = pSrc.ZhuanTai;
            DingDanType = pSrc.DingDanType;
            ShenPiTime = pSrc.ShenPiTime;
            QueRenUserId = pSrc.QueRenUserId;
            ProductLingYongMainId = pSrc.ProductLingYongMainId;
            BeiZhu = pSrc.BeiZhu;
            DDmoney = pSrc.DDmoney;
            IsDel = pSrc.IsDel;
            AddTime = pSrc.AddTime;
            QueRenTime = pSrc.QueRenTime;
            return string.Empty;
        }

        /// <summary>
        /// IMangoOrderBase.GetId(), return id of order
        /// </summary>
        /// <returns></returns>
        public string GetId() { return DingDanID.ToString(); }

        /// <summary>
        /// overrided ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType(), DingDanID);
        }
    }
}
