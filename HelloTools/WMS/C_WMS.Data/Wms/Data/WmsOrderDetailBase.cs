using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Wms.Data
{
    /// <summary>
    /// C-WMS系统中的单据明细基类
    /// </summary>
    public class WmsOrderDetailBase : WmsEntityBase
    {
        /// <summary>
        /// 货主
        /// </summary>
        public WmsOwner Owner { get { return mOwner; } }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        protected WmsOwner mOwner = null;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// 外部业务编码
        /// </summary>
        public string OutBizCode = string.Empty;

        /// <summary>
        /// 商品库存类型
        /// </summary>
        public TWmsInventoryType InventoryType = TWmsInventoryType.ZP;
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WmsOrderDetailBase()
        {
            GenerateOutBizCode();
            mOwner = new WmsOwner();
        }

        /// <summary>
        /// 生成接口通讯所需的外部业务编码
        /// </summary>
        /// <returns></returns>
        protected string GenerateOutBizCode()
        {
            return OutBizCode = Guid.NewGuid().ToString();
        }
    }
}
