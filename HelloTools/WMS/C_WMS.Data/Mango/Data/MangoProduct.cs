using MangoMis.Frame.DataSource.Simple;
using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using MangoMis.MisFrame.Helper;
using MisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TT.Common.Frame.Model;

namespace C_WMS.Data.Mango.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class GuiGeProp
    {
        public string id = string.Empty;
        public string Description = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiGeProp"/> class.
        /// </summary>
        /// <param name="pId">The p identifier.</param>
        /// <param name="pName">Name of the p.</param>
        public GuiGeProp(string pId, string pName)
        {
            id = pId;
            Description = pName;
        }
    }

    /// <summary>
    /// 商品规格
    /// </summary>
    public class GuiGeDict
    {
        /// <summary>
        /// 规格ID
        /// </summary>
        public string Id = string.Empty;

        /// <summary>
        /// 规格描述
        /// </summary>
        public string Name
        {
            get
            {
                if (0 >= GuiGeList.Count)
                    return string.Empty;
                string spec = string.Empty;
                foreach (var i in GuiGeList) { spec += GuiGeList.Find(x => x.id == i.id).Description + ","; }
                return spec;
            }
        }
        //public string Name = string.Empty;

        /// <summary>
        /// The GUI ge list
        /// </summary>
        public List<GuiGeProp> GuiGeList { get { return mGuiGeList; } }
        private List<GuiGeProp> mGuiGeList = new List<GuiGeProp>(1);

        /// <summary>
        /// default constructor
        /// </summary>
        public GuiGeDict() { }
    }

    /// <summary>
    /// 商品类
    /// </summary>
    public class MangoProduct : Product_ProductInfo_List
    {
        /// <summary>
        /// 设置商品Id，同时更新其他商品信息
        /// </summary>
        virtual public string Id
        {
            get { return ProductId.ToString(); }
            set
            {
                int idInt = 0;
                if (int.TryParse(value, out idInt))
                    Copy(MangoFactory.GetProduct(value));
            }
        }

        /// <summary>
        /// 商品规格
        /// </summary>
        public GuiGeDict GGDict { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MangoProduct"/> class.
        /// </summary>
        public MangoProduct()
        {
            GGDict = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="mp">The mp.</param>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/29 21:25
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        public void Copy(MangoProduct mp)
        {
            CopyFrom(mp);
            if (null != mp?.GGDict) GGDict = mp.GGDict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSrc"></param>
        public void CopyFrom(Product_ProductInfo_List pSrc)
        {
            if (null == pSrc)
                return;
            
            #region Copy
            CanLingYong = pSrc.CanLingYong; // ret.Append("CanLingYong={0}", CanLingYong);
            AddUserid = pSrc.AddUserid; // ret.Append("AddUserid={0}", AddUserid);
            AddTime = pSrc.AddTime; // ret.Append("AddTime={0}", AddTime);
            IsDel = pSrc.IsDel; // ret.Append("IsDel={0}", IsDel);
            UpdateTime = pSrc.UpdateTime; // ret.Append("UpdateTime={0}", UpdateTime);
            UpdateUserID = pSrc.UpdateUserID; // ret.Append("UpdateUserID={0}", UpdateUserID);
            KuCunCount = pSrc.KuCunCount; // ret.Append("KuCunCount={0}", KuCunCount);
            TotalCount = pSrc.TotalCount; // ret.Append("TotalCount={0}", TotalCount);
            SerialId = pSrc.SerialId; // ret.Append("SerialId={0}", SerialId);
            isParent = pSrc.isParent; // ret.Append("isParent={0}", isParent);
            isPeiSongTime = pSrc.isPeiSongTime; // ret.Append("isPeiSongTime={0}", isPeiSongTime);
            WuPinMoney = pSrc.WuPinMoney; // ret.Append("WuPinMoney={0}", WuPinMoney);
            ZhiBaotime = pSrc.ZhiBaotime; // ret.Append("ZhiBaotime={0}", ZhiBaotime);
            isSupplierPeiSong = pSrc.isSupplierPeiSong; // ret.Append("isSupplierPeiSong={0}", isSupplierPeiSong);
            IsSale = pSrc.IsSale; // ret.Append("IsSale={0}", IsSale);
            isTanXiao = pSrc.isTanXiao; // ret.Append("isTanXiao={0}", isTanXiao);
            isPoint = pSrc.isPoint; // ret.Append("isPoint={0}", isPoint);
            xianZhiOrgs = pSrc.xianZhiOrgs; // ret.Append("xianZhiOrgs={0}", xianZhiOrgs);
            XianZhiType = pSrc.XianZhiType; // ret.Append("XianZhiType={0}", XianZhiType);
            CaiGoPrice = pSrc.CaiGoPrice; // ret.Append("CaiGoPrice={0}", CaiGoPrice);
            YWY_Dingdan_type_2L = pSrc.YWY_Dingdan_type_2L; // ret.Append("YWY_Dingdan_type_2L={0}", YWY_Dingdan_type_2L);
            YWY_Dingdan_type_3L = pSrc.YWY_Dingdan_type_3L; // ret.Append("YWY_Dingdan_type_3L={0}", YWY_Dingdan_type_3L);
            DisOrder = pSrc.DisOrder; // ret.Append("DisOrder={0}", DisOrder);
            LuRuState = pSrc.LuRuState; // ret.Append("LuRuState={0}", LuRuState);
            JDProudctID = pSrc.JDProudctID; // ret.Append("JDProudctID={0}", JDProudctID);
            OrgID = pSrc.OrgID; // ret.Append("OrgID={0}", OrgID);
            ProductId = pSrc.ProductId; // ret.Append("ProductId={0}", ProductId);
            Title = pSrc.Title; // ret.Append("Title={0}", Title);
            BianMa = pSrc.BianMa; // ret.Append("BianMa={0}", BianMa);
            ProductTypeId = pSrc.ProductTypeId; // ret.Append("ProductTypeId={0}", ProductTypeId);
            ProductCategoryIdBig = pSrc.ProductCategoryIdBig; // ret.Append("ProductCategoryIdBig={0}", ProductCategoryIdBig);
            ProductLevel = pSrc.ProductLevel; // ret.Append("ProductLevel={0}", ProductLevel);
            ProductCategoryId = pSrc.ProductCategoryId; // ret.Append("ProductCategoryId={0}", ProductCategoryId);
            Brands = pSrc.Brands; // ret.Append("Brands={0}", Brands);
            Model = pSrc.Model; // ret.Append("Model={0}", Model);
            isJD = pSrc.isJD; // ret.Append("isJD={0}", isJD);
            MiniInventory = pSrc.MiniInventory; // ret.Append("MiniInventory={0}", MiniInventory);
            InquiryCycle = pSrc.InquiryCycle; // ret.Append("InquiryCycle={0}", InquiryCycle);
            ProductImage = pSrc.ProductImage; // ret.Append("ProductImage={0}", ProductImage);
            Remark = pSrc.Remark; // ret.Append("Remark={0}", Remark);
            Unit = pSrc.Unit; // ret.Append("Unit={0}", Unit);
            GuiGe = pSrc.GuiGe; // ret.Append("GuiGe={0}", GuiGe);
            PriceLow = pSrc.PriceLow; // ret.Append("PriceLow={0}", PriceLow);
            PriceMax = pSrc.PriceMax; // ret.Append("PriceMax={0}", PriceMax);
            PriceAve = pSrc.PriceAve; // ret.Append("PriceAve={0}", PriceAve);
            DepreciationRate = pSrc.DepreciationRate; // ret.Append("DepreciationRate={0}", DepreciationRate);
            ResidualRate = pSrc.ResidualRate; // ret.Append("ResidualRate={0}", ResidualRate);
            MaxInventory = pSrc.MaxInventory; // ret.Append("MaxInventory={0}", MaxInventory);
            #endregion
        }
    }
}
