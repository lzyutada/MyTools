using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using C_WMS.Data.Mango.Data;
using C_WMS.Data.Wms.Data;
using MisModel;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// entity of product
    /// </summary>
    /// <seealso cref="CWmsEntityBase" />
    class CWmsProduct : Interface.CWms.CWmsEntity.CWmsEntityBase
    {
        #region Members
#if false
        /// <summary>
        /// 有效期(天)
        /// </summary>
        public int ExpirationDay = 0;

        /// <summary>
        /// 有效期(小时)
        /// </summary>
        public int ExpirationHour = 0;
        
        private MangoProduct mMangoProduct = null;
        
        private WmsProduct mWmsProduct = null;
#endif

        /// <summary>
        /// 要传给wms的ItemCode
        /// </summary>
        /// <value>
        /// The item code.
        /// </value>
        public string ItemCode
        {
            get { return mItemCode; }
            set
            {
                mItemCode = value;  // set itemCode
                MangoProduct.SetId(CWmsProductHandler.GetItemIdFromCode(value));
                //// set ProductId
                //var idList = value.Split('-');
                //if (0 < idList.Length) MangoProduct.SetId(idList[0]);
                //else MangoProduct.SetId(string.Empty);
            }
        }
        private string mItemCode = string.Empty;

        /// <summary>
        /// 获取和设置芒果商城中的 商品
        /// </summary>
        public MangoProduct MangoProduct { get; protected set; }
        /// <summary>
        /// 获取和设置WMS系统中的商品
        /// </summary>
        public WmsProduct WmsProduct { get; protected set; }
        #endregion

        #region Methods

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsProduct() {
            MangoProduct = new MangoProduct();
            WmsProduct = new WmsProduct();
        }

        /// <summary>
        /// 构造方法，根据MangoProduct构造实体
        /// </summary>
        public CWmsProduct(MangoProduct pMango)
        {
            throw new NotImplementedException("");
            MangoProduct = pMango;// new MangoProduct();
            //if (null != pMango)
            //{
            //    MangoProduct.CopyFrom(pMango);
            //    ItemCode = pMango.ProductId.ToString();
            //}
            // TODO: set ItemCode
            WmsProduct = new WmsProduct();
        }

        /// <summary>
        /// 构造方法，根据MangoProduct、Product_ProductInfo_List_GuiGeList
        /// 和List[Product_ProductInfo_List_GuiGe]构造实体
        /// </summary>
        /// <param name="pMango"></param>
        /// <param name="pGgLink"></param>
        /// <param name="pGgList"></param>
        public CWmsProduct(MangoProduct pMango, Product_ProductInfo_List_GuiGeList pGgLink, List<Product_ProductInfo_List_GuiGe> pGgList)
        {
            throw new NotImplementedException("");
            WmsProduct = new WmsProduct();
            MangoProduct = new MangoProduct();
            if (null != pMango)
            {
                ItemCode = pMango.ProductId.ToString(); // TODO: set ItemCode by productid and linkId
                MangoProduct.CopyFrom(pMango);
            }
            //mMangoProduct = (null != pMango) ? pMango : new MangoProduct();
            if (null != pGgLink && null != pGgList)
            {
                ItemCode = pGgLink.ProductId + "-" + pGgLink.ProductGuiGeID;
                MangoProduct.GGDict.Id = pGgLink.ProductGuiGeID.ToString();
                MangoProduct.GGDict.GuiGeList.Clear();
                MangoProduct.GGDict.GuiGeList.AddRange(pGgList.Where(x => (-1 < pGgLink.GuiGeIDList.IndexOf(x.GuiGeID.ToString()))).Select(y => new GuiGeProp(y.GuiGeID.ToString(), y.GuiGeName)).ToList());
                //MangoProduct.GGDict.GetSpecification();
            }
        }
        #endregion
    }

    /// <summary>
    /// handler of entity of CWmsProduct
    /// </summary>
    class CWmsProductHandler
    {
        protected CWmsProduct _product = null;

        /// <summary>
        /// 根据商城中的商品ID获取数据实体
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public CWmsProduct GetProductByMisId(string pId)
        {
            throw new NotImplementedException("");
        }

        /// <summary>
        /// 根据WMS系统的商品编码获取数据实体
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public CWmsProduct GetProductByWmsCode(string pCode)
        {
            throw new NotImplementedException("");
        }

        /// <summary>
        /// 根据商品ID和商品/规格关联ID生成商品编码。若执行成功则返回商品编码([pProductId]-[pLinkId])，否则返回string.Empty
        /// </summary>
        /// <param name="pProductId">商品ID</param>
        /// <param name="pLinkId">商品/规格关联ID</param>
        /// <returns>若执行成功则返回商品编码([pProductId]-[pLinkId])，否则返回string.Empty</returns>
        static public string GetItemCode(string pProductId, string pLinkId)
        {
            if (string.IsNullOrEmpty(pProductId))
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}, 根据商品ID[{2}]和商品/规格关联ID[{3}]生成商品编码失败", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, pProductId, pLinkId);
                return string.Empty;
            }
            else return (string.IsNullOrEmpty(pLinkId)) ? pProductId : string.Format("{0}-{1}", pProductId, pLinkId);
        }

        /// <summary>
        /// 根据WMS系统中的商品编码获取商城中的商品ID。若执行成功则返回商品ID，否则返回string.Empty.
        /// </summary>
        /// <param name="pCode">WMS系统中的商品编码</param>
        /// <returns>若执行成功则返回商品ID，否则返回string.Empty.</returns>
        static public string GetItemIdFromCode(string pCode)
        {
            if (string.IsNullOrEmpty(pCode))
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}, 根据WMS系统中的商品编码[{2}]获取商城中的商品ID. 传入的商品编码为空", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, pCode);
                return string.Empty;
            }
            else
            {
                // set ProductId
                var idList = pCode.Split('-');
                if (0 < idList.Length) return idList[0]; // 返回商品ID
                else
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("{0}.{1}, 根据WMS系统中的商品编码[{2}]获取商城中的商品ID. 非法的商品编码", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, pCode);
                    return string.Empty;
                }
            }
        }
    }
}
