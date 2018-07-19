using C_WMS.Interface.CWms.CWmsEntity;
using C_WMS.Data.Mango;
using C_WMS.Data.Mango.Data;
using MangoMis.Frame.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using C_WMS.Data.CWms.CWmsEntity;

namespace C_WMS.Data.CWms.Interfaces.Data
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot("request")]
    public class HttpReqXml_ItemsSyncronize : HttpReqXmlBase
    {
        /// <summary>
        /// 赋值详见https://wiki.517cdn.com/index.php/CWMS/product/%E9%A1%B9%E7%9B%AE%E5%AE%9E%E6%96%BD#.E8.B0.83.E7.A0.94
        /// </summary>
        public string warehouseCode { get; set; }

        /// <summary>
        /// 同步方法 add/update
        /// </summary>
        public string actionType { get; set; }

        /// <summary>
        /// 赋值详见https://wiki.517cdn.com/index.php/Mango-WMS#.E8.B0.83.E7.A0.94
        /// </summary>
        public string ownerCode { get; set; }

        /// <summary>
        /// 商品的信息
        /// </summary>
        [XmlArray("items"), XmlArrayItem("item")]
        public List<HttpReqXml_ItemsSync_Item> items { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public HttpReqXml_ItemsSyncronize()
        {
            items = new List<HttpReqXml_ItemsSync_Item>(1);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Type GetType()
        {
            return typeof(HttpReqXml_ItemsSyncronize);
        }
    }

    /// <summary>
    /// 商品序列化所需实体类
    /// </summary>
    [XmlRoot("item")]
    public class HttpReqXml_ItemsSync_Item
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string barCode { get; set; } 
        public string skuProperty { get; set; }
        public string stockUnit { get; set; }
        public string categoryId { get; set; }
        public string categoryName { get; set; }
        public string itemType { get; set; } = "ZC";
        public string retailPrice { get; set; }
        public string purchasePrice { get; set; }
        public string brandName { get; set; }
        public string isShelfLifeMgmt { get; set; }
        public string remark { get; set; }
        public string createTime { get; set; }
        public string updateTime { get; set; }
        public string isValid { get; set; }
        public string isSku { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// default constructor
        /// </summary>
        public HttpReqXml_ItemsSync_Item() { }

        /// <summary>
        /// construct by source object
        /// </summary>
        /// <param name="pSrc"></param>
        public HttpReqXml_ItemsSync_Item(CWmsProduct pSrc)
        {
            CopyMangoProductInfo(pSrc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcObj"></param>
        public void CopyMangoProductInfo(CWmsProduct srcObj)
        {
            if (null == srcObj)
            {
                return;
            }
            itemCode = srcObj.ItemCode;
            itemName = srcObj.MangoProduct.Title;
            barCode = srcObj.WmsProduct.barCode;
            skuProperty = srcObj.MangoProduct.GGDict.Name;
            stockUnit = srcObj.MangoProduct.Unit;
            categoryId = srcObj.MangoProduct.ProductCategoryId.ToString();
            categoryName = Simple_ProductCategory_Cache.ProductCategory_Cache_Store.All().Find(e => e.ProductCategoryId == srcObj.MangoProduct.ProductCategoryId.Int()).ProductCategory ?? "";
            retailPrice = srcObj.MangoProduct.WuPinMoney.ToString(); // retailPrice.ToString();
            brandName = srcObj.MangoProduct.Brands;
            isShelfLifeMgmt = "N"; //  0 < srcObj.MangoProduct.ZhiBaotime ? "Y" : "N"; // 默认不采集效期
            remark = srcObj.MangoProduct.Remark;
            createTime = srcObj.MangoProduct.AddTime.ToString();
            updateTime = srcObj.MangoProduct.UpdateTime.ToString();
        }

    }


}
