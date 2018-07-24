using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data
{
    /// <summary>
    /// Mis2014系统中的是否删除
    /// </summary>
    public enum TMis2014_IsDel
    {
        /// <summary>
        /// 正常
        /// </summary>
        ENormal = 0,

        /// <summary>
        /// 删除
        /// </summary>
        EDeleted = 1,

        /// <summary>
        /// 默认
        /// </summary>
        EDefault = ENormal
    }

    /// <summary>
    /// 单据类型，指代入库订单、出库订单、退货订单等。
    /// M.C.O.C: Mango Cancel Order Category，指代可已取消的单据类型
    /// </summary>
    public enum TCWmsOrderCategory
    {
        /// <summary>
        /// 入库单
        /// </summary>
        EEntryOrder,

        /// <summary>
        /// 采购订单
        /// </summary>
        EPurchaseOrder,

        /// <summary>
        /// 出库单
        /// </summary>
        EExwarehouseOrder,

        /// <summary>
        /// 退货单
        /// </summary>
        EReturnOrder,

        /// <summary>
        /// 商城订单
        /// </summary>
        EMallOrder,

        /// <summary>
        /// 退货订单可以取消
        /// </summary>
        EMcocReturnOrder,

        /// <summary>
        /// TODO: 网络新品库（货主为芒果网络）的出库订单（无订单出库）也应该是可以取消的（商城上没有取消的操作入口）。
        /// 目前阶段不处理，什么时候商城上完成了取消无订单出库的功能，再这里处理单据取消。 
        /// </summary>
        EMcocStockoutWithoutMallOrder,

        /// <summary>
        /// 单据类型的数量
        /// </summary>
        ECount,

        /// <summary>
        /// 未知类型 
        /// </summary>
        EUnknownCategory,

        /// <summary>
        /// 默认值
        /// </summary>
        EDefaultCategory = EUnknownCategory
    }
    
    /// <summary>
    /// C-WMS系统中的商品类型
    /// </summary>
    public enum TWmsItemType
    {
        EZhengChang,
        EFenXiao,
        EZuHe,
        EZengPin,
        EBaoCai,
        EHaoCai,
        EFuLiao,
        EXuNi,
        EFuShu,
        ECanCi,
        EOther,
        ETypeCount,
        EDefaultType = EOther
    }

    /// <summary>
    /// C-WMS系统的单据类型，包括入库订单、出库订单、退货入库订单等。
    /// </summary>
    public enum TWmsOrderType
    {
        /// <summary>
        /// 一般交易出库单
        /// </summary>
        JYCK,

        /// <summary>
        /// 换货出库,
        /// </summary>
        HHCK,

        /// <summary>
        /// 补发出库
        /// </summary>
        BFCK,

        /// <summary>
        /// 普通出库单,
        /// </summary>
        PTCK,

        /// <summary>
        /// 调拨出库,
        /// </summary>
        DBCK,

        /// <summary>
        /// B2B入库,
        /// </summary>
        B2BRK,

        /// <summary>
        /// B2B出库,
        /// </summary>
        B2BCK,

        /// <summary>
        /// 其他出库,
        /// </summary>
        QTCK,

        /// <summary>
        /// 生产入库,
        /// </summary>
        SCRK,

        /// <summary>
        /// 领用入库,
        /// </summary>
        LYRK,

        /// <summary>
        /// 残次品入库,
        /// </summary>
        CCRK,

        /// <summary>
        /// 采购入库,
        /// </summary>
        CGRK,

        /// <summary>
        /// 调拨入库,
        /// </summary>
        DBRK,

        /// <summary>
        /// 其他入库,
        /// </summary>
        QTRK,

        /// <summary>
        /// 销退入库
        /// </summary>
        XTRK,

        /// <summary>
        /// 换货入库
        /// </summary>
        HHRK,

        /// <summary>
        /// 仓内加工单
        /// </summary>
        CNJG,

        /// <summary>
        /// 未知单据类型
        /// </summary>
        EUnknown,

        /// <summary>
        /// 默认类型
        /// </summary>
        EDefaultType = EUnknown
    }

    /// <summary>
    /// 库存类型
    /// </summary>
    public enum TWmsInventoryType
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        ZP,    // 正品
        CC,    // 残次品
        JS,    // 机损
        XS,    // 箱损
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

    ///// <summary>
    ///// C-WMS系统中的出库订单类型， 废弃的，使用TWmsOrderType
    ///// </summary>
    //public enum TWmsStockoutType
    //{
    //    /// <summary>
    //    /// 普通出库单（退仓）
    //    /// </summary>
    //    PTCK,

    //    /// <summary>
    //    /// 调拨出库
    //    /// </summary>
    //    DBCK,

    //    /// <summary>
    //    /// B2B出库
    //    /// </summary>
    //    B2BCK,

    //    /// <summary>
    //    /// 其他出库
    //    /// </summary>
    //    QTCK,

    //    /// <summary>
    //    /// 采购退货出库单
    //    /// </summary>
    //    CGTH,
    //}

    /// <summary>
    /// 芒果商城订单类型
    /// </summary>
    public enum TMangoOrderType
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        EUnknown = 0,   // 未知
        EBMDD = 1,  // 部门订单
        EGRGM = 2,  // 个人购买
        EZSBD = 3,  // 装饰补单
        EBPSQ = 5,  // 备品申请
        EFLSQ = 6,  // 福利申请
        EWLCK = 7,  // 芒果网络为货主，无订单出库
        EDefaultType = EUnknown
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }
    
    /// <summary>
    /// C-WMS系统中的退货单据类型
    /// </summary>

    public enum TWmsReturnOrderType
    {
        THRK,
        HHRK,
    }
    
    /// <summary>
    /// 芒果商城中，子退货订单的退货类型。
    /// </summary>
    public enum TMangoReturnType
    {
        E2ndHandReturn,
        EReturn2Supplier,
        EQualityProblem,
        E2ndHandRecycle,
        ERepealStore,
        ENewReturn,
        ETypeCount,
        EUnknown,
        EDefaultType = EUnknown
    }

    /// <summary>
    /// 芒果商城中主退货订单中的‘退货物流’
    /// </summary>
    public enum T芒果商城退货物流
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        蓝江上门 = 1,
        自行返还 = 2,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

    /// <summary>
    /// 货主类型
    /// </summary>
    public enum TOwner
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        芒果网络,
        蓝江智家,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

    /// <summary>
    /// Mis2014系统中子采购订单的列
    /// </summary>
    public class Mis2014_SubPurchaseOrder_Column
    {
        /// <summary>
        /// 存放工号
        /// </summary>
        public const string QueryPricePerson = "QueryPricePerson";

        /// <summary>
        /// [ChangeRemark]如果购买的物品有变化，那么需要填写变化的原因。
        /// </summary>
        public const string ChangeRemark = "ChangeRemark";
        /// <summary>
        /// 1：购买中；2：部分到货；3：全部到货
        /// </summary>
        public const string ProductInputState = "ProductInputState";
        /// <summary>
        /// 供货商id
        /// </summary>
        public const string SupplierId = "SupplierId";
        /// <summary>
        /// 排序
        /// </summary>
        public const string DisOrder = "DisOrder";
        /// <summary>
        /// 添加人ID
        /// </summary>
        public const string AddUserid = "AddUserid";
        /// <summary>
        /// 添加时间
        /// </summary>
        public const string AddTime = "AddTime";
        /// <summary>
        /// 0:正常;1:删除.
        /// </summary>
        public const string IsDel = "IsDel";
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public const string UpdateTime = "UpdateTime";
        /// <summary>
        /// 最后修改人，备用
        /// </summary>
        public const string UpdateUserID = "UpdateUserID";
        /// <summary>
        /// 
        /// </summary>
        public const string PurchasingSupervisor = "PurchasingSupervisor";
        /// <summary>
        /// 
        /// </summary>
        public const string PurchasingManager = "PurchasingManager";
        /// <summary>
        /// 主采购单id
        /// </summary>
        public const string MainId = "MainId";
        /// <summary>
        /// 产品分类id
        /// </summary>
        public const string ProductCategoryId = "ProductCategoryId";
        /// <summary>
        /// 是否供应商直接配货 字典285
        /// </summary>
        public const string isSupplierPeiSong = "isSupplierPeiSong";
        /// <summary>
        /// 要购买的物品是否变化。1：没有变化；2：有变化
        /// </summary>
        public const string IsChange = "IsChange";
        /// <summary>
        /// 验收人。目前默认李权20817
        /// </summary>
        public const string YanShouRen = "YanShouRen";
        /// <summary>
        /// 
        /// </summary>
        public const string ProductGuiGeID = "ProductGuiGeID";
        /// <summary>
        /// 
        /// </summary>
        public const string Remark = "Remark";
        /// <summary>
        /// [主键][ProductBuyId]编号，自增
        /// </summary>
        public const string ProductBuyId = "ProductBuyId";
        /// <summary>
        /// 入库单自己的单号
        /// </summary>
        public const string ProductBuyCode = "ProductBuyCode";
        /// <summary>
        /// 入库的日期
        /// </summary>
        public const string ProductInputDate = "ProductInputDate";
        /// <summary>
        /// 外键，物品ID
        /// </summary>
        public const string ProductId_Plan = "ProductId_Plan";
        /// <summary>
        /// 本次入库的数量
        /// </summary>
        public const string ProductCount_Plan = "ProductCount_Plan";
        /// <summary>
        /// 主入库单ID
        /// </summary>
        public const string ProductInputMainId = "ProductInputMainId";
        /// <summary>
        /// 本次入库的单价
        /// </summary>
        public const string ProductPrice_Plan = "ProductPrice_Plan";
        /// <summary>
        /// 外键，物品ID
        /// </summary>
        public const string ProductId = "ProductId";
        /// <summary>
        /// 本次入库的数量
        /// </summary>
        public const string ProductCount = "ProductCount";
        /// <summary>
        /// 本次入库的单价
        /// </summary>
        public const string ProductPrice = "ProductPrice";
        /// <summary>
        /// 本物品的金额合计
        /// </summary>
        public const string ProductMoney = "ProductMoney";
        /// <summary>
        /// 已经到货多少
        /// </summary>
        public const string ProductCount_Already = "ProductCount_Already";
        /// <summary>
        /// 先随便写吧
        /// </summary>
        public const string UseKindId = "UseKindId";
        /// <summary>
        /// 本物品的金额合计
        /// </summary>
        public const string ProductMoney_Plan = "ProductMoney_Plan";

        /// <summary>
        /// 获取所有列明组成的List[string]
        /// </summary>
        /// <returns></returns>
        static public List<string> AllColumnNameList()
        {
            List<string> outList = new List<string>(3) {
                    QueryPricePerson, ChangeRemark, ProductInputState, SupplierId
                    ,DisOrder, AddUserid, AddTime, IsDel, UpdateTime, UpdateUserID
                    ,PurchasingSupervisor, PurchasingManager, MainId, ProductCategoryId
                    ,isSupplierPeiSong, IsChange, YanShouRen, ProductGuiGeID, Remark
                    ,ProductBuyId, ProductBuyCode, ProductInputDate, ProductId_Plan
                    ,ProductCount_Plan, ProductInputMainId, ProductPrice_Plan
                    ,ProductId, ProductCount, ProductPrice, ProductMoney
                    ,ProductCount_Already, UseKindId, ProductMoney_Plan
                    };
            return outList;
        }
    }

    /// <summary>
    /// Mis2014系统中子配送订单的列
    /// </summary>
    public class Mis2014_SubDeliveryOrder_Column
    {

        /// <summary>
        /// 子订单ID
        /// </summary>
        public const string ZiDingDanID = "ZiDingDanID";
        /// <summary>
        /// 子入库单ID(供应商直接发货用)
        /// </summary>
        public const string ProductInputId = "ProductInputId";
        /// <summary>
        /// 子出库单ID
        /// </summary>
        public const string ProductIOputId = "ProductIOputId";
        /// <summary>
        /// 是否删除
        /// </summary>
        public const string IsDel = "IsDel";
        /// <summary>
        /// 主配送单号
        /// </summary>
        public const string ProductLingYongMainId = "ProductLingYongMainId";
        /// <summary>
        /// [主键][ProductLingYongId]子配送单ID
        /// </summary>
        public const string ProductLingYongId = "ProductLingYongId";
    }

    /// <summary>
    /// Mis2014系统中子出库订单的列
    /// </summary>
    public class Mis2014_SubExwarehouseOrder_Column
    {
        #region Properties
        /// <summary>
        /// 来源库的类型
        /// </summary>
        public const string OutputKuKind = "OutputKuKind";

        /// <summary>
        /// 产品id
        /// </summary>
        public const string ProductId = "ProductId";

        /// <summary>
        /// 领用人id
        /// </summary>
        public const string ProductLingYongId = "ProductLingYongId";

        /// <summary>
        /// 使用寿命
        /// </summary>
        public const string ShiYongShuoMing = "ShiYongShuoMing";

        /// <summary>
        /// 主出库单id
        /// </summary>
        public const string MainId = "MainId";

        /// <summary>
        /// 出库状态
        /// </summary>
        public const string ProductOutputState = "ProductOutputState";

        /// <summary>
        /// 目的库
        /// </summary>
        public const string WarehouseIdPoint = "WarehouseIdPoint";

        /// <summary>
        /// 申请月份例如201501
        /// </summary>
        public const string Applymonth = "Applymonth";

        /// <summary>
        /// 出库原因
        /// </summary>
        public const string goal = "goal";

        /// <summary>
        /// 接收人
        /// </summary>
        public const string ReceiveUserid = "ReceiveUserid";

        /// <summary>
        /// 接收时间
        /// </summary>
        public const string ReceiveTime = "ReceiveTime";

        /// <summary>
        /// 接收状态
        /// </summary>
        public const string ReceiveState = "ReceiveState";

        /// <summary>
        /// 类型
        /// </summary>
        public const string MapClassId = "MapClassId";

        /// <summary>
        /// 类型id
        /// </summary>
        public const string MapId = "MapId";

        /// <summary>
        /// 生命周期ID
        /// </summary>
        public const string ProductLifeId = "ProductLifeId";

        /// <summary>
        /// 产品的出库数量
        /// </summary>
        public const string ProductCount = "ProductCount";

        /// <summary>
        /// 组织id
        /// </summary>
        public const string OrgId = "OrgId";

        /// <summary>
        /// 最后修改人，备用
        /// </summary>
        public const string ProductGuiGeID = "ProductGuiGeID";

        /// <summary>
        /// 最后修改人，备用
        /// </summary>
        public const string UpdateUserID = "UpdateUserID";

        /// <summary>
        /// 主键编号，自增
        /// </summary>
        public const string ProductOutputId = "ProductOutputId";

        /// <summary>
        /// 入库单自己的单号
        /// </summary>
        public const string ProductOutputCode = "ProductOutputCode";

        /// <summary>
        /// 入库的日期
        /// </summary>
        public const string ProductOutputDate = "ProductOutputDate";

        /// <summary>
        /// 外键，从哪个仓库出来的
        /// </summary>
        public const string WarehouseId = "WarehouseId";

        /// <summary>
        /// 发放人、出库人
        /// </summary>
        public const string FaFangRen = "FaFangRen";

        /// <summary>
        /// 是否二手 285
        /// </summary>
        public const string isErShou = "isErShou";

        /// <summary>
        /// 存放工号。0：不能确认使用人
        /// </summary>
        public const string FuZeRen = "FuZeRen";

        /// <summary>
        /// 备用。
        /// </summary>
        public const string KuaiJiID = "KuaiJiID";

        /// <summary>
        /// 排序
        /// </summary>
        public const string DisOrder = "DisOrder";

        /// <summary>
        /// 添加人ID
        /// </summary>
        public const string AddUserid = "AddUserid";

        /// <summary>
        /// 添加时间
        /// </summary>
        public const string AddTime = "AddTime";

        /// <summary>
        /// 0:正常;1:删除.
        /// </summary>
        public const string IsDel = "IsDel";

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public const string UpdateTime = "UpdateTime";

        /// <summary>
        /// 存放工号。接收、领用人
        /// </summary>
        public const string LingYongRen = "LingYongRen";
        #endregion

        /// <summary>
        /// 获取所有列明组成的List[string]
        /// </summary>
        /// <returns></returns>
        static public List<string> AllColumnNameList()
        {
            List<string> outList = new List<string>(3) { OutputKuKind, ProductId, ProductLingYongId, ShiYongShuoMing, MainId, ProductOutputState, WarehouseIdPoint, Applymonth, goal, ReceiveUserid, ReceiveTime, ReceiveState, MapClassId, MapId, ProductLifeId, ProductCount, OrgId, ProductGuiGeID, UpdateUserID, ProductOutputId, ProductOutputCode, ProductOutputDate, WarehouseId, FaFangRen, isErShou, FuZeRen, KuaiJiID, DisOrder, AddUserid, AddTime, IsDel, UpdateTime, LingYongRen };
            return outList;
        }
    }

    /// <summary>
    /// Mis2014系统中子退货订单的列
    /// </summary>
    public class Mis2014_SubReturnOrder_Column
    {
        /// <summary>
        /// 生命周期ID
        /// </summary>
        public const string ProductLifeId = "ProductLifeId";

        /// <summary>
        /// 
        /// </summary>
        public const string ChildZhuangTai = "ChildZhuangTai";

        /// <summary>
        /// 排序
        /// </summary>
        public const string DisOrder = "DisOrder";

        /// <summary>
        /// 最后更新人
        /// </summary>
        public const string UpdateUserID = "UpdateUserID";

        /// <summary>
        /// 添加人
        /// </summary>
        public const string AddUserid = "AddUserid";

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public const string UpdateTime = "UpdateTime";

        /// <summary>
        /// 添加时间
        /// </summary>
        public const string AddTime = "AddTime";

        /// <summary>
        /// 是否删除
        /// </summary>
        public const string IsDel = "IsDel";

        /// <summary>
        /// 购买时价格（单价）
        /// </summary>
        public const string DDMoney = "DDMoney";

        /// <summary>
        /// 子出库单ID
        /// </summary>
        public const string ProductIOputId = "ProductIOputId";

        /// <summary>
        /// 备注
        /// </summary>
        public const string memo = "memo";

        /// <summary>
        /// 子订单ID
        /// </summary>
        public const string ZiDingDanID = "ZiDingDanID";

        /// <summary>
        /// 物品回收价格(单价)
        /// </summary>
        public const string EerShouMoney = "EerShouMoney";

        /// <summary>
        /// 主退货订单ID
        /// </summary>
        public const string TuiHuoMainID = "TuiHuoMainID";

        /// <summary>
        /// 是否完成
        /// </summary>
        public const string isOver = "isOver";

        /// <summary>
        /// 物品数量
        /// </summary>
        public const string ProductCount = "ProductCount";

        /// <summary>
        /// 物品ID
        /// </summary>
        public const string ProductId = "ProductId";

        /// <summary>
        /// 子退货单ID
        /// </summary>
        public const string ZiTuihuoID = "ZiTuihuoID";
        /// <summary>
        /// 子退货单ID
        /// </summary>
        public const string ProductGuiGeID = "ProductGuiGeID";
    }

    /// <summary>
    /// Mis2014系统中商城子订单的列
    /// </summary>
    public class Mis2014_SubEntryOrder_Column
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public const string KuaiJiID="KuaiJiID";
        public const string MainId = "MainId";
        public const string Remark = "Remark";
        public const string YanShouJieGuo = "YanShouJieGuo";
        public const string ListType = "ListType";
        public const string PeiSongRenYuan = "PeiSongRenYuan";
        public const string UpdateUserID = "UpdateUserID";
        public const string UpdateTime = "UpdateTime";
        public const string IsDel = "IsDel";
        public const string AddTime = "AddTime";
        public const string AddUserid = "AddUserid";
        public const string DisOrder = "DisOrder";
        public const string InputKuKind = "InputKuKind";
        public const string ProductInputState = "ProductInputState";
        public const string ProductGuiGeID = "ProductGuiGeID";
        public const string QueryPricePerson = "QueryPricePerson";
        public const string YanShouRen = "YanShouRen";
        public const string ProductMoney = "ProductMoney";
        public const string ProductPrice = "ProductPrice";
        public const string ProductInputCount = "ProductInputCount";
        public const string WarehouseId = "WarehouseId";
        public const string ProductId = "ProductId";
        public const string ProductInputDate = "ProductInputDate";
        public const string ProductBuyId = "ProductBuyId";
        public const string ProductBuyCode = "ProductBuyCode";
        public const string ProductInputCode = "ProductInputCode";
        public const string ProductInputId = "ProductInputId";
        public const string SupplierId = "SupplierId";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

    /// <summary>
    /// Mis2014系统中的商品
    /// </summary>
    public class Mis2014_ProductInfo
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        //     [CanLingYong]1：可以领用；2：不可以领用
        public const string CanLingYong = "CanLingYong";
        //     [AddUserid]添加人ID
        public const string AddUserid = "AddUserid";
        //     [AddTime]添加时间
        public const string AddTime = "AddTime";
        //     [IsDel]0:正常;1:删除.
        public const string IsDel = "IsDel";
        //     [UpdateTime]最后修改时间
        public const string UpdateTime = "UpdateTime";
        //     [UpdateUserID]最后修改人，备用
        public const string UpdateUserID = "UpdateUserID";
        //     [KuCunCount]此产品的库存数量WarehouseType in（1,3,4）
        public const string KuCunCount = "KuCunCount";
        //     [TotalCount]此产品的总数量WarehouseType in（1,2,3,4）
        public const string TotalCount = "TotalCount";
        //     [SerialId]序列号id
        public const string SerialId = "SerialId";
        //     [isParent]是否套装
        public const string isParent = "isParent";
        //     [isPeiSongTime]是否可以要求指定时间配送 字典285
        public const string isPeiSongTime = "isPeiSongTime";
        //     [WuPinMoney]售价
        public const string WuPinMoney = "WuPinMoney";
        //     [ZhiBaotime]质保期至
        public const string ZhiBaotime = "ZhiBaotime";
        //     [isSupplierPeiSong]是否供应商直接配货 字典285
        public const string isSupplierPeiSong = "isSupplierPeiSong";
        //     [IsSale]是否上架 字典285
        public const string IsSale = "IsSale";
        //     [isTanXiao]dict 285
        public const string isTanXiao = "isTanXiao";
        //     [isPoint]是否充许小数点数量 285
        public const string isPoint = "isPoint";
        //     [xianZhiOrgs]限购部门
        public const string xianZhiOrgs = "xianZhiOrgs";
        //     [XianZhiType]限购类型 dic=555
        public const string XianZhiType = "XianZhiType";
        //     [CaiGoPrice]
        public const string CaiGoPrice = "CaiGoPrice";
        //     [YWY_Dingdan_type_2L]云物业安装订单二级项目
        public const string YWY_Dingdan_type_2L = "YWY_Dingdan_type_2L";
        //     [YWY_Dingdan_type_3L]云物业安装订单三级项目
        public const string YWY_Dingdan_type_3L = "YWY_Dingdan_type_3L";
        //     [DisOrder]排序
        public const string DisOrder = "DisOrder";
        //     [LuRuState]1：正式；2：采购员录入；2014-01-27新增
        public const string LuRuState = "LuRuState";
        //     [JDProudctID]京东商品编码
        public const string JDProudctID = "JDProudctID";
        //     [OrgID]外键，部门。
        public const string OrgID = "OrgID";
        //     [主键][ProductId]编号
        public const string ProductId = "ProductId";
        //     [Title]产品名称
        public const string Title = "Title";
        //     [BianMa]类似于拼音的编码
        public const string BianMa = "BianMa";
        //     [ProductTypeId]类型，外键，固定资产、备品、损耗品等
        public const string ProductTypeId = "ProductTypeId";
        //     [ProductCategoryIdBig]类别，外键，n级分类。
        public const string ProductCategoryIdBig = "ProductCategoryIdBig";
        //     [ProductLevel]1：总体；2：具体
        public const string ProductLevel = "ProductLevel";
        //     [ProductCategoryId]产品分类id
        public const string ProductCategoryId = "ProductCategoryId";
        //     [Brands]品牌，手填，依据类别提示
        public const string Brands = "Brands";
        //     [Model]型号，手填，依据类别提示
        public const string Model = "Model";
        //     [isJD]是否京东商品 dicClassid=285
        public const string isJD = "isJD";
        //     [MiniInventory]最低库存
        public const string MiniInventory = "MiniInventory";
        //     [InquiryCycle]询价周期，天
        public const string InquiryCycle = "InquiryCycle";
        //     [ProductImage]产品图片，多图
        public const string ProductImage = "ProductImage";
        //     [Remark]产品的描述。7.22新增
        public const string Remark = "Remark";
        //     [Unit]个、箱、瓶等
        public const string Unit = "Unit";
        //     [GuiGe]40ML等
        public const string GuiGe = "GuiGe";
        //     [PriceLow]产品的价格范围。8.23新增
        public const string PriceLow = "PriceLow";
        //     [PriceMax]产品的价格范围。8.23新增
        public const string PriceMax = "PriceMax";
        //     [PriceAve]根据已经成交的历史记录计算。8.23新增
        public const string PriceAve = "PriceAve";
        //     [DepreciationRate]折旧率
        public const string DepreciationRate = "DepreciationRate";
        //     [ResidualRate]残值率
        public const string ResidualRate = "ResidualRate";
        //     [MaxInventory]最高库存
        public const string MaxInventory = "MaxInventory";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

    /// <summary>
    /// Mis2014系统中的主入库单
    /// </summary>
    public class Mis2014_EntryOrder_Column
    {
        /// <summary>
        ///[主键][ProductInputMainId]主入库单id
        /// </summary>
        public const string ProductInputMainId = "ProductInputMainId";
        /// <summary>
        ///[ProductInputMainCode]入库单编号
        /// </summary>
        public const string ProductInputMainCode = "ProductInputMainCode";
        /// <summary>
        ///[YanShouRen]验收人id
        /// </summary>
        public const string YanShouRen = "YanShouRen";
        /// <summary>
        ///[AddUserId]
        /// </summary>
        public const string AddUserId = "AddUserId";
        /// <summary>
        ///[AddTime]
        /// </summary>
        public const string AddTime = "AddTime";
        /// <summary>
        ///[isdel]
        /// </summary>
        public const string isdel = "isdel";
        /// <summary>
        ///[WarehouseId]仓库id
        /// </summary>
        public const string WarehouseId = "WarehouseId";
        /// <summary> 
        ///[iscandel]是否可以删除
        /// </summary>
        public const string iscandel = "iscandel";
        /// <summary>
        ///[Remark]
        /// </summary>
        public const string Remark = "Remark";
        /// <summary>
        ///[CompanyTypeId]公司类型DictClassId=299
        /// </summary>
        public const string CompanyTypeId = "CompanyTypeId";
        /// <summary>
        ///[isPrint]字典285，入库单，是否打印
        /// </summary>
        public const string isPrint = "isPrint";
        /// <summary>
        ///[IsOver]是否入库完成 ,字典285
        /// </summary>
        public const string IsOver = "IsOver";
        /// <summary>
        ///[IsToWMS]是否发送至WMS,字典285
        /// </summary>
        public const string IsToWMS = "IsToWMS";
    }

    /// <summary>
    /// Mis2014系统中的主配送单
    /// </summary>
    public class Mis2014_DeliveryOrder_Column
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public const string AddUserid = "AddUserid";
        public const string DisOrder = "DisOrder";
        public const string isSupplierPeiSong = "isSupplierPeiSong";
        public const string PeiSongMoney = "PeiSongMoney";
        public const string DeliveryUserId = "DeliveryUserId";
        public const string WarehouseId = "WarehouseId";
        public const string ProductIOputMainId = "ProductIOputMainId";
        public const string LouCeng = "LouCeng";
        public const string ProductInputMainId = "ProductInputMainId";
        public const string PrintCount = "PrintCount";
        public const string PeiSongImage = "PeiSongImage";
        public const string PeiSongTime = "PeiSongTime";
        public const string IsJieSuan = "IsJieSuan";
        public const string CompanyClientId = "CompanyClientId";
        public const string KuaiDiCode = "KuaiDiCode";
        public const string DingDanType = "DingDanType";
        public const string ZhuangTai = "ZhuangTai";
        public const string MapClassID = "MapClassID";
        public const string UpdateUserID = "UpdateUserID";
        public const string UpdateTime = "UpdateTime";
        public const string ZhuangShi_JieSuan_Time = "ZhuangShi_JieSuan_Time";
        public const string AddTime = "AddTime";
        public const string ProductLingYongMainId = "ProductLingYongMainId";
        public const string ProductLingYongCode = "ProductLingYongCode";
        public const string OrgId = "OrgId";
        public const string DictStairid = "DictStairid";
        public const string DeliveryAddr = "DeliveryAddr";
        public const string WarehouseIdPoint = "WarehouseIdPoint";
        public const string ConsigneeId = "ConsigneeId";
        public const string MapID = "MapID";
        public const string Mobile = "Mobile";
        public const string IsPiZhun = "IsPiZhun";
        public const string PiZhunTime = "PiZhunTime";
        public const string Remark = "Remark";
        public const string DeliveryDriverUserId = "DeliveryDriverUserId";
        public const string DingDanID = "DingDanID";
        public const string ZhuisOver = "ZhuisOver";
        public const string IsDel = "IsDel";
        public const string KuFangZhuGuanId = "KuFangZhuGuanId";
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }
}
