using MangoMis.Frame.Helper;
using MangoMis.MisFrame.MisStore;
using PubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace C_WMS.Data.Mango.MisModelPWI
{
    /// <summary>
    /// constances of Mis System Parameters
    /// </summary>
    class CWmsMisSystemParamKeys
    {
        public const string cStrParamClassName = "MgMall";
        public const string cStrEnableCWMS = "EnableCWMS";
        public const string cStrCWMSOfflineHostUri = "CWMSOfflineHostUri";
        public const string cStrCWMSHostUri = "CWMSHostUri";
        public const string cStrSecret = "secret";
        public const string cStrFormat = "format";
        public const string cStrAppKey = "app_key";
        public const string cStrVersion = "v";
        public const string cStrSignMethod = "sign_method";
        public const string cStrCustomerId = "customerid";
        public const string cStrLogisticsId = "logistics";
        #region name of API methods
        public const string cStrApiMethod_ItemsSync = "ApiMethod_ItemsSync";
        public const string cStrApiMethod_InventoryMonitoring = "ApiMethod_InventoryMonitoring";
        #endregion
        public const int cIntIsDefault = 0;
    }

    /// <summary>
    /// CWMS相关的Mis系统参数缓存
    /// </summary>
    class CWmsMisSystemParamCache
    {
        /// <summary>
        /// 获取CWMS相关的Mis系统参数缓存实例
        /// </summary>
        static public CWmsMisSystemParamCache Cache { get { return (null == _cache) ? _cache = new CWmsMisSystemParamCache() : _cache; } }
        static CWmsMisSystemParamCache _cache = null;

        #region Properties of params
        public pub_SystemParam EnableCWms { get; private set; }
        public pub_SystemParam CWMSOfflineHostUri { get; private set; }
        public pub_SystemParam CWMSHostUri { get; private set; }
        public pub_SystemParam Secret { get; private set; }
        public pub_SystemParam Format { get; private set; }
        public pub_SystemParam AppKey { get; private set; }
        public pub_SystemParam Version { get; private set; }
        public pub_SystemParam SignMethod { get; private set; }
        public pub_SystemParam ApiMethod_ItemsSync { get; private set; }
        public pub_SystemParam ApiMethod_InventoryMonitoring { get; private set; }
        public pub_SystemParam ApiMethod_ItemSync { get; private set; }
        protected pub_SystemParam SysParamCustomerId { get; private set; }
        protected pub_SystemParam SysParamLogistics { get; private set; }
        public CWmsSystemParam_Customer CustomerId { get; private set; }
        public CWmsSystemParam_Logistics Logistics { get; private set; }
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        CWmsMisSystemParamCache()
        {
            try
            {
                EnableCWms = SystemParamStore.FindParam(CWmsMisSystemParamKeys.cStrParamClassName, CWmsMisSystemParamKeys.cStrEnableCWMS);
                CWMSOfflineHostUri = SystemParamStore.FindParam(CWmsMisSystemParamKeys.cStrParamClassName, CWmsMisSystemParamKeys.cStrCWMSOfflineHostUri);
                CWMSHostUri = SystemParamStore.FindParam(CWmsMisSystemParamKeys.cStrParamClassName, CWmsMisSystemParamKeys.cStrCWMSHostUri);
                Secret = SystemParamStore.FindParam(CWmsMisSystemParamKeys.cStrParamClassName, CWmsMisSystemParamKeys.cStrSecret);
                Format = SystemParamStore.FindParam(CWmsMisSystemParamKeys.cStrParamClassName, CWmsMisSystemParamKeys.cStrFormat);
                AppKey = SystemParamStore.FindParam(CWmsMisSystemParamKeys.cStrParamClassName, CWmsMisSystemParamKeys.cStrAppKey);
                Version = SystemParamStore.FindParam(CWmsMisSystemParamKeys.cStrParamClassName, CWmsMisSystemParamKeys.cStrVersion);
                SignMethod = SystemParamStore.FindParam(CWmsMisSystemParamKeys.cStrParamClassName, CWmsMisSystemParamKeys.cStrSignMethod);
                {
                    SysParamCustomerId = SystemParamStore.FindParam(CWmsMisSystemParamKeys.cStrParamClassName, CWmsMisSystemParamKeys.cStrCustomerId);
                    Utility.CWmsXmlSerializer mxs = new Utility.CWmsXmlSerializer(false, typeof(CWmsSystemParam_Customer), SysParamCustomerId?.PValue);
                    CustomerId = mxs.Deserialize() as CWmsSystemParam_Customer;
                }
                {
                    SysParamLogistics = SystemParamStore.FindParam(CWmsMisSystemParamKeys.cStrParamClassName, CWmsMisSystemParamKeys.cStrLogisticsId);
                    Utility.CWmsXmlSerializer mxs = new Utility.CWmsXmlSerializer(false, typeof(CWmsSystemParam_Logistics), SysParamLogistics?.PValue);
                    Logistics = mxs.Deserialize() as CWmsSystemParam_Logistics;
                }
            }
            catch (Exception ex)
            {
                #region for Error INFO
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, @"创建CWMS相关的Mis系统参数缓存异常.\r\n
                    , EnableCWms({0}) [{1}: {2}]\r\n
                    , CWMSOfflineHostUri({3}) [{4}: {5}]\r\n
                    , CWMSHostUri({6}) [{7}: {8}]\r\n
                    , Secret({9}) [{10}: {11}]\r\n
                    , Format({12}) [{13}: {14}]\r\n
                    , AppKey({15}) [{16}: {17}]\r\n
                    , Version({18}) [{19}: {20}]\r\n
                    , SignMethod({21}) [{22}: {23}]\r\n
                    , SysParamCustomerId({24}) [{25}: {26}]\r\n
                    , SysParamLogistics({27}) [{28}: {29}]\r\n
                    , CustomerId={30}\r\n
                    , Logistic{31}"
                    , EnableCWms, EnableCWms?.PName, EnableCWms?.PValue
                    , CWMSOfflineHostUri, CWMSOfflineHostUri?.PName, CWMSOfflineHostUri?.PValue
                    , CWMSHostUri, CWMSHostUri?.PName, CWMSHostUri?.PValue
                    , Secret, Secret?.PName, Secret?.PValue
                    , Format, Format?.PName, Format?.PValue
                    , AppKey, AppKey?.PName, AppKey?.PValue
                    , Version, Version?.PName, Version?.PValue
                    , SignMethod, SignMethod?.PName, SignMethod?.PValue
                    , SysParamCustomerId, SysParamCustomerId?.PName, SysParamCustomerId?.PValue
                    , SysParamLogistics, SysParamLogistics?.PName, SysParamLogistics?.PValue
                    , CustomerId, Logistics);
                #endregion
            }
        }

        public IEnumerable<CWmsSystemParam_CustomerOwner> GetV_Owners()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取MIS系统参数中配置的默认货主
        /// </summary>
        /// <returns></returns>
        public CWmsSystemParam_CustomerOwner GetDefaultOwner()
        {
            if (null == CustomerId)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("获取MIS系统参数中配置的默认货主失败，系统参数异常: CustomerId={0}", SysParamCustomerId?.PValue);
                return null;
            }
            else if (0 == CustomerId.Owners.Count)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning("获取MIS系统参数中配置的默认货主失败，没有货主:CustomerId={0}", SysParamCustomerId?.PValue);
                return null;
            }
            else
            {
                try { return CustomerId.Owners.First(o => CWmsMisSystemParamKeys.cIntIsDefault == o.IsDefault.Int()); }
                catch (Exception ex)
                {
                    foreach (var o in CustomerId.Owners)
                    {
                        C_WMS.Data.Utility.MyLog.Instance.Warning("获取MIS系统参数中配置的默认货主发生异常. 货主[Owner={3}][{0} | {1} | {2}]", o?.Code, o?.Name, o?.IsDefault, o);
                    }
                    C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "获取MIS系统参数中配置的默认货主发生异常");
                    return null;
                }
            }
        }

        /// <summary>
        /// Get logistics in Mis.System.Parameters(sort of MSP) by id of delievery user. -and-
        ///  User whose id with value equals to 'pUserId' COULDNOT be found in MSP is a stuff of MGWL or LJZJ,
        ///  and logistics of Mango will be returned; one logistics in MSP will be returned if User was found
        ///  in MSP; null will be returned if something wrong.
        /// </summary>
        /// <returns>null or an entity of CWmsSystemParam_LogisticsItem</returns>
        public CWmsSystemParam_LogisticsItem GetLogisticsByUserId(string pUserId)
        {
            try
            {
                CWmsSystemParam_LogisticsItem rslt = null;
                foreach (var logistics in Logistics.Logisticses)
                {
                    if (null != logistics.DeliveryUsers.Find(u => u.UserCode.Equals(pUserId)))
                    {
                        rslt = logistics;
                        break;
                    }
                }

                if (null != rslt) { return rslt; } // one logistics has been founded.

                // user hasn't been found in MSP, return the default 
                rslt = Logistics?.Logisticses.Find(l => CWmsMisSystemParamKeys.cIntIsDefault.ToString().Equals(l.IsDefault));

                if (null != rslt)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("CWmsMisSystemParamCache.GetLogisticsByUserId({0}) FAILED, COULDNOT retrieve the default logistics({1}) of MIS system parameters", pUserId, Logistics);
                }

                return rslt;
            }
            catch(Exception ex)
            {
                foreach (var l in Logistics.Logisticses)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("!!Exception in CWmsMisSystemParamCache.GetLogisticsByUserId({0}). [Logistics={1}][{2} | {3} | {4}]", pUserId, l, l?.Code, l?.Name, l?.IsDefault);
                }
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "Exception in CWmsMisSystemParamCache.GetLogisticsByUserId({0})", pUserId);
                return null;
            }
        }

        /// <summary>
        /// get default logistics according to Mis System Parameters.
        /// </summary>
        /// <returns></returns>
        public CWmsSystemParam_LogisticsItem GetDefaultLogistics()
        {
            try
            {
                var rslt = Logistics.Logisticses.Find(x => CWmsMisSystemParamKeys.cIntIsDefault.ToString().Equals(x.IsDefault));
                if (null == rslt)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("Failed in finding default logistics, return null.\r\n Logistics={0}, Logistics.Logisticses.Count={1}", Logistics, Logistics?.Logisticses.Count);
                }
                return rslt;
            }
            catch(Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "Exception in CWmsMisSystemParamCache.GetDefaultLogistics, return null.");
                return null;
            }
        }

        /// <summary>
        /// 获取全部仓库
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CWmsSystemParam_CustomerOwnerWarehouses> GetV_Warehouses()
        {
            throw new NotImplementedException("");
        }
    }

    #region 货主
    /// <summary>
    /// 货主参数对应的可序列化类型
    /// </summary>
    [XmlRoot("CWMS_Owner_SysParam")]
    class CWmsSystemParam_Customer
    {
        [XmlArray("owners"), XmlArrayItem("owner")]
        public List<CWmsSystemParam_CustomerOwner> Owners = new List<CWmsSystemParam_CustomerOwner>(1);
        public CWmsSystemParam_Customer()
        {
        }
    }

    /// <summary>
    /// owner节点
    /// </summary>
    [XmlRoot("owner")]
    public class CWmsSystemParam_CustomerOwner
    {
        /// <summary>
        /// ownerCode节点
        /// </summary>
        [XmlElement("ownerCode")]
        public string Code = string.Empty;

        /// <summary>
        /// ownerName节点
        /// </summary>
        [XmlElement("ownerName")]
        public string Name = string.Empty;

        /// <summary>
        /// isDefault节点
        /// int, 0 - 是默认货主；其他值 - 不是默认货主
        /// </summary>
        [XmlElement("isDefault")]
        public string IsDefault = string.Empty;

        /// <summary>
        /// ownerCode节点
        /// </summary>
        [XmlArray("warehouses"), XmlArrayItem("warehouse")]
        public List<CWmsSystemParam_CustomerOwnerWarehouses> Warehouses = new List<CWmsSystemParam_CustomerOwnerWarehouses>();

        /// <summary>
        /// default construcotr
        /// </summary>
        public CWmsSystemParam_CustomerOwner()
        {
        }
    }

    /// <summary>
    /// warehouse节点
    /// </summary>
    [XmlRoot("warehouse")]
    public class CWmsSystemParam_CustomerOwnerWarehouses
    {
        /// <summary>
        /// warehouseCode节点
        /// </summary>
        [XmlElement("warehouseCode")]
        public string Code = string.Empty;

        /// <summary>
        /// warehouseName节点
        /// </summary>
        [XmlElement("warehouseName")]
        public string Name = string.Empty;

        /// <summary>
        /// default construcotr
        /// </summary>
        public CWmsSystemParam_CustomerOwnerWarehouses()
        {
        }
    }
    #endregion

    #region 承运商
    /// <summary>
    /// 承运商参数对应的可序列化类型
    /// </summary>
    [XmlRoot("CWMS_Logistics_SysParam")]
    class CWmsSystemParam_Logistics
    {
        [XmlArray("logisticses"), XmlArrayItem("logistics")]
        public List<CWmsSystemParam_LogisticsItem> Logisticses = new List<CWmsSystemParam_LogisticsItem>(1);
        public CWmsSystemParam_Logistics()
        {
        }
    }

    /// <summary>
    /// owner节点
    /// </summary>
    [XmlRoot("logistics")]
    public class CWmsSystemParam_LogisticsItem
    {
        /// <summary>
        /// ownerCode节点
        /// </summary>
        [XmlElement("logisticsCode")]
        public string Code = string.Empty;

        /// <summary>
        /// ownerName节点
        /// </summary>
        [XmlElement("logisticsName")]
        public string Name = string.Empty;

        /// <summary>
        /// isDefault节点
        /// </summary>
        [XmlElement("isDefault")]
        public string IsDefault = string.Empty;

        /// <summary>
        /// ownerName节点
        /// </summary>
        [XmlArray("DeliveryUsers"), XmlArrayItem("user")]
        public List<CWmsSystemParam_DeliveryUser> DeliveryUsers = new List<CWmsSystemParam_DeliveryUser>(1);

        /// <summary>
        /// default construcotr
        /// </summary>
        public CWmsSystemParam_LogisticsItem()
        {
        }
    }

    /// <summary>
    /// user节点。商城系统中的配送人
    /// </summary>
    [XmlRoot("user")]
    public class CWmsSystemParam_DeliveryUser
    {
        /// <summary>
        /// userName节点
        /// </summary>
        [XmlElement("userCode")]
        public string UserCode = string.Empty;

        /// <summary>
        /// userName节点
        /// </summary>
        [XmlElement("userName")]
        public string UserName = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsSystemParam_DeliveryUser()
        {
        }
    }
    #endregion
}
