using C_WMS.Data.Wms.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.CWmsEntity
{
    /// <summary>
    /// 可取消的单据类型的基类
    /// </summary>
    abstract public class CWmsMcocOrder : CWmsOrderBase
    {
        /// <summary>
        /// 单据取消原因
        /// </summary>
        public string CancelReason = string.Empty;

        /// <summary>
        /// 获取货主
        /// </summary>
        public WmsOwner Owner { get; protected set; }
//#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
//        protected WmsOwner mOwner = new WmsOwner();
//#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }
}
