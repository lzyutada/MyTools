using MangoMis.Frame.DataSource.Simple;
using MangoMis.Frame.ThirdFrame;
using MisModel;
using System.Collections.Generic;
using System;
using TT.Common.Frame.Model;
using MangoMis.Frame.Helper;
using C_WMS.Data.Mango.Data;
using C_WMS.Interface.CWms.CWmsEntity;
using C_WMS.Data.CWms.CWmsEntity;
using System.Reflection;

namespace C_WMS.Data.Mango.MisModelPWI
{
    /// <summary>
    /// 表Product_WMS_Interface中的列名
    /// </summary>
    class Product_WMS_Interface_Properties
    {
        #region
        public const string PropName_AddTime = "AddTime";
        public const string PropName_AddUserid = "AddUserid";
        public const string PropName_DisOrder = "DisOrder";
        public const string PropName_IsDel = "IsDel";
        public const string PropName_IsUpdateOK = "IsUpdateOK";
        public const string PropName_LastTime = "LastTime";
        public const string PropName_MapCalssID = "MapCalssID";
        public const string PropName_MapId1 = "MapId1";
        public const string PropName_MapId2 = "MapId2";
        public const string PropName_UpdateUserID = "UpdateUserID";
        public const string PropName_WMS_InterfaceId = "WMS_InterfaceId";
        #endregion
    }

    /// <summary>
    /// 字典285中的值（是否删除）
    /// </summary>
    public enum TDict285_Values
    {
        /// <summary>
        /// 不详。
        /// 对于isDel，代表未删除
        /// </summary>
        EUnknown = 0,

        /// <summary>
        /// 若对应Dict[709]，则表示更新失败。
        /// 对于isDel，代表已删除。
        /// 对于IsToWMS等商城中字段的定义，代表是，如已批准、已向WMS同步、可以删除等。
        /// </summary>
        EDeleted = 1,

        /// <summary>
        /// 若对应Dict[709]，则表示更新成功。
        /// 对于isDel，不存在2，如果传入，则直接被置为1.
        /// </summary>
        ENormal = 2
    }

    /// <summary>
    /// 字典709中的值
    /// </summary>
    public enum TDict709_Value
    {
        /// <summary>
        /// 未知
        /// </summary>
        EUnknown = 0,

        /// <summary>
        /// 添加商品
        /// </summary>
        EAddProduct = 1,

        /// <summary>
        /// 更新商品
        /// </summary>
        EUpdateProduct = 2,

        /// <summary>
        /// 入库订单
        /// </summary>
        EEntryOrder = 3,

        /// <summary>
        /// 出库订单
        /// </summary>
        EExwarehouseOrder = 4,

        /// <summary>
        /// 退货订单
        /// </summary>
        EReturnOrder = 5,

        /// <summary>
        /// 取消出库订单
        /// </summary>
        ECancelExwarehouseOrder = 6,

        /// <summary>
        /// 取消退货订单
        /// </summary>
        ECancelReturnOrder = 7
    }


    /// <summary>
    /// 表Product_WMS_Interface的操作类
    /// </summary>
    class Dict709Handle
    {
#if !C_WMS_V1
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMapClassId"></param>
        /// <param name="pMapId1"></param>
        /// <param name="pMapId2"></param>
        /// <param name="pUpdateOk"></param>
        /// <param name="pDel"></param>
        /// <param name="pMsg"></param>
        /// <returns></returns>
        static public int UpdateRow(TDict709_Value pMapClassId, string pMapId1, string pMapId2, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            throw new NotFiniteNumberException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMapClassId"></param>
        /// <param name="pMapId1"></param>
        /// <param name="pMapId2"></param>
        /// <param name="pUpdateOk"></param>
        /// <param name="pDel"></param>
        /// <param name="pMsg"></param>
        /// <returns></returns>
        static public int UpdateRowA(TDict709_Value pMapClassId, string pMapId1, string pMapId2, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            throw new NotFiniteNumberException();
        }
#else
        /// <summary>
        /// 获取所有商品。若操作失败则返回Count=0的List
        /// </summary>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/29 16:01
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        public static List<Product_WMS_Interface> GetProductList()
        {
            var ret= new ThirdResult<List<object>>("取得表Product_WMS_Interface所有商品数据，包含添加商品or更新商品");

            // Set filter
            var filter = new List<CommonFilterModel>() { };
            filter.Add(new CommonFilterModel(Product_WMS_Interface_Properties.PropName_MapCalssID, "in", new List<object>() { 1, 2 }));
            filter.Add(new CommonFilterModel(Product_WMS_Interface_Properties.PropName_IsDel, "!=", TDict285_Values.EDeleted.Int().ToString()));

            // query
            var wcfPWI = WCF<Product_WMS_Interface>.QueryAll(filter);

            // query finished.
            InnerDebug(ret, wcfPWI, "获取所有商品");
            ret.End();
            return (null == wcfPWI || null == wcfPWI.Data || 0 >= wcfPWI.RetInt)? new List<Product_WMS_Interface>(): wcfPWI.Data;
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/29 16:25
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        static public int AddRow_Product(Product_WMS_Interface row)
        {
            var ret = new ThirdResult<List<object>>("向表Product_WMS_Interface插入一行商品");
            
            row.IsDel = 1;
            row.AddTime = DateTime.Now;
            row.AddUserid = MangoMis.Frame.Frame.CommonFrame.userid;
            var wcfPWI = WCF<Product_WMS_Interface>.Add(row);
            
            InnerDebug(ret, wcfPWI, "添加商品");
            ret.End();
            return wcfPWI.RetInt;
        }

        /// <summary>
        /// 向709批量插入数据，废弃
        /// </summary>
        /// <param name="rowlist"></param>
        /// <returns></returns>
        static public int AddRow_Product(List<Product_WMS_Interface> rowlist)
        {
            var ret = new ThirdResult<List<object>>("向表Product_WMS_Interface插入多条商品");
            var renInt = 0;
            foreach (var row in rowlist)
            {
                row.IsDel = 2;
                row.AddTime = DateTime.Now;
                row.AddUserid = MangoMis.Frame.Frame.CommonFrame.userid;
                var wcfPWI = WCF<Product_WMS_Interface>.Add(row); // TODO: 不能add整个list
                InnerDebug(ret, wcfPWI, "向709批量插入数据，废弃");
                renInt += wcfPWI.RetInt;
            }

            ret.End();
            return renInt;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/29 16:49
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        static public int UpdateRow_Product(Product_WMS_Interface row)
        {
            var ret = new ThirdResult<List<object>>("向表Product_WMS_Interface更新一行商品");
            
            var wcfPWI = WCF<Product_WMS_Interface>.Update(row);

            InnerDebug(ret, wcfPWI,"更新商品");
            ret.End();
            return wcfPWI.RetInt;
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        static public int UpdateRow_Product(List<Product_WMS_Interface> row)
        {
            var ret = new ThirdResult<List<object>>("向表Product_WMS_Interface更新一行商品");
            var wcfPWI = WCF<Product_WMS_Interface>.Update(row);
            InnerDebug(ret, wcfPWI, "更新商品");
            ret.End();
            return wcfPWI.RetInt;
        }
        
        /// <summary>
        /// 新增一条创建出库订单的记录
        /// </summary>
        /// <param name="pData">待插入的记录</param>
        /// <param name="pMsg">返回错误描述信息</param>
        /// <returns>返回成功操作的记录行数，若失败则返回0</returns>
        static protected int AddRow_StockoutCreate(Product_WMS_Interface pData, out string pMsg)
        {
            if (null == pData)
            {
                pMsg = "非法入参, pMsg=null";
                return 0;
            }

            int rslt = 0;
            var retRslt = WCF<Product_WMS_Interface>.Add(pData);
            if (null == retRslt)
            {
                pMsg = "WCF插入记录行失败，返回null对象";
                return 0;
            }
            else
            {
                rslt = retRslt.RetInt;
                pMsg = retRslt.RETData;
                return rslt;
            }
        }
        /// <summary>
        /// 新增主出库订单的记录
        /// </summary>
        /// <param name="pOrderId">主出库订单Id</param>
        /// <param name="pCount">返回成功新增的行数</param>
        /// <param name="pMsg">返回错误描述信息</param>
        /// <returns>返回成功操作的记录行数，若失败则返回0</returns>
        static public int AddRow_StockoutCreate(string pOrderId, out int pCount, out string pMsg)
        {
            int rslt = TError.Ser_ErrorPost.Int();
            pCount = 0; pMsg = string.Empty;
            CWmsStockOrder order = null; // 主出库订单实例

            // validate parameters
            if (string.IsNullOrEmpty(pOrderId))
            {
                return rslt = TError.Post_ParamError.Int();
            }

            try
            {
                // get order instance
                if (null == (order = CWms.CWmsDataFactory.GetCWmsStockoutOrder(pOrderId)))
                {
                    pMsg = "获取子出库订单列表失败";
                    return 0;
                }
// prepare updated rows
                //rowList = new List<Product_WMS_Interface>(1);
                foreach (var subOrder in order.SubOrders)
                {
                    Product_WMS_Interface r = new Product_WMS_Interface();
// 实例赋值
                    r.MapCalssID = (int)TDict709_Value.EExwarehouseOrder;
                    r.MapId1 = (order.MangoOrder as MangoStockouOrder).ProductOutputMainId;
                    r.MapId2 = (subOrder.Value.MangoOrder as MangoSubStockoutOrder).ProductOutputId;
                    r.IsUpdateOK = (int)TDict285_Values.EDeleted;
                    r.IsDel = (int)TDict285_Values.ENormal;
                    r.AddTime = DateTime.Now;
                    r.AddUserid = MangoMis.Frame.Frame.CommonFrame.userid;
                    r.LastTime = r.AddTime;
                    r.UpdateUserID = r.AddUserid;
                    r.DisOrder = Product_WMS_Interface_Properties.cIntDisorderDefault;
                    if (0 == (rslt = AddRow_StockoutCreate(r, out pMsg)))
                    {
                        rslt = 0;
                        break;
                    }
                    else
                    {
                        pCount += rslt;
                    }
                }
                return rslt;
            }
            catch (Exception ex)
            {
                var ret = new ThirdResult<List<object>>("");
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return rslt = TError.WCF_RunError.Int();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMapClassId"></param>
        /// <param name="pMapId1"></param>
        /// <param name="pMapId2"></param>
        /// <param name="pIsUpdateOk"></param>
        /// <param name="pIsDel"></param>
        /// <param name="wcfError"></param>
        /// <param name="pMsg"></param>
        /// <returns></returns>
        static public Product_WMS_Interface NewPwiEntity(TDict709_Value pMapClassId, string pMapId1, string pMapId2, TDict285_Values pIsUpdateOk, TDict285_Values pIsDel)
        {
            try
            {
                return new Product_WMS_Interface()
                {
                    MapCalssID = pMapClassId.Int(),
                    MapId1 = pMapClassId.Int(),
                    MapId2 = pMapClassId.Int(),
                    IsUpdateOK = pIsUpdateOk.Int(),
                    IsDel = pIsDel.Int(),
                    WMS_InterfaceId = 0,
                    AddTime = DateTime.Now,
                    AddUserid = 0,
                    LastTime = DateTime.Now,
                    UpdateUserID = MangoMis.Frame.Frame.CommonFrame.userid,
                    DisOrder = Product_WMS_Interface_Properties.cIntDisorderDefault
                };
            }
            catch(Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "{0}.{1}(pMapClassId={2}, pMapId1={3}, pMapId2={4}, pIsUpdateOk={5}, pIsDel={6})发生异常", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pMapClassId, pMapId1, pMapId2, pIsUpdateOk, pIsDel);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pEntity"></param>
        /// <param name="pWcfError"></param>
        /// <param name="pMsg"></param>
        /// <returns></returns>
        static public int UpdateRowA(Product_WMS_Interface pEntity, out int pWcfError, out string pMsg)
        {
            if (null == pEntity)
            {
                pMsg  =string.Format("{0}.{1}, 非法空入参", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                C_WMS.Data.Utility.MyLog.Instance.Warning(pMsg);
                return pWcfError=TError.Post_ParamError.Int();
            }
            else
            {
                try
                {
                    DefaultResult<int> retRslt = null;
                    var wcfRslt = MangoWCF<Product_WMS_Interface>.GetEntity(pEntity.WMS_InterfaceId.Int());
// update/add row in Dict709
                    if (null == wcfRslt)
                    {
                        pMsg = string.Format("{0}.{1}, WCF查找({2}, {3}, {4})返回null", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pEntity.WMS_InterfaceId, pEntity.MapId1, pEntity.MapId2);
                        C_WMS.Data.Utility.MyLog.Instance.Warning(pMsg);
                        return pWcfError = TError.Pro_HaveNoData.Int();
                    } // if (null == wcfRslt), return error
                    else if (null == wcfRslt.Data)
                    {
                        pEntity.AddUserid = MangoMis.Frame.Frame.CommonFrame.userid;
                        pEntity.AddTime = DateTime.Now;
                        retRslt = MangoWCF<Product_WMS_Interface>.Add(pEntity);
                    } // else if (null == wcfRslt.Data), insert new row when notfound
                    else
                    {
                        pEntity.AddUserid = wcfRslt.Data.AddUserid;
                        pEntity.AddTime = wcfRslt.Data.AddTime;
                        retRslt = MangoWCF<Product_WMS_Interface>.Update(pEntity);
                    } // else, update the existing row

        // handle updating/adding WCF result.
                    if (null == retRslt)
                    {
                        pMsg = string.Format("{0}.{1}, WCF(wcfRslt.Data={2})返回null [WCF({3}, {4}, {5})查找结果: {6}, {7}]", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, wcfRslt.Data, pEntity.WMS_InterfaceId, pEntity.MapId1, pEntity.MapId2, wcfRslt.RetInt, wcfRslt.Debug);
                        C_WMS.Data.Utility.MyLog.Instance.Warning(pMsg);
                        return pWcfError = TError.WCF_RunError.Int();
                    } // if (null == retRslt), WCF ERROR
                    else if (0 >= retRslt.Data)
                    {
                        pMsg = string.Format("{0}.{1}, WCF(wcfRslt.Data={2})返回结果: {3}, {4}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, wcfRslt.Data, wcfRslt.RetInt, wcfRslt.Debug);
                        C_WMS.Data.Utility.MyLog.Instance.Warning(pMsg);
                        return pWcfError = retRslt.RetInt;
                    } // else if (0 >= retRslt.Data), failed in updating/adding
                    else
                    {
                        pMsg = string.Empty;
                        pWcfError = retRslt.Data;
                        return retRslt.RetInt;
                    } // else succeed in updating/adding
                }
                catch(Exception ex)
                {
                    pMsg = ex.Message;
                    C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "{0}.{1}({2})发生异常", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, pEntity);
                    return pWcfError = TError.Post_NoChange.Int();
                }
            }
        }
        
        /// <summary>
        /// 更新子出库订单的记录, 若没找到记录则新增一条
        /// </summary>
        /// <param name="pOrderId">主出库单Id</param>
        /// <param name="pUpdateOk">记录的更新状态，遵照Dict[285]</param>
        /// <param name="pDel">Dict[285]</param>
        /// <param name="pCount">返回成功更新的行数</param>
        /// <param name="pMsg">返回错误描述信息</param>
        /// <returns>若更新成功则返回操作的行数；其他错误返回TError.WCF_RunError</returns>
        static public int UpdateRow_StockoutCreate(string pOrderId, TDict285_Values pUpdateOk, TDict285_Values pDel, out int pCount, out string pMsg)
        {
            pCount = 0; pMsg = string.Empty;
            CWmsStockOrder order = null;
            Product_WMS_Interface tmpEntity = null;

            try
            {
                // get order instance
                if (null == (order = CWms.CWmsDataFactory.GetCWmsStockoutOrder(pOrderId)))
                {
                    return TError.WCF_RunError.Int();
                }

        #region prepare updated rows
                foreach (var subOrder in order.SubOrders)
                {
                    // 根据主单据Id和子单据Id找到行
                    var filter = new List<CommonFilterModel>() { }; // query filter
                    filter.Add(new CommonFilterModel(Product_WMS_Interface_Properties.PropName_MapId1, "=", order.GetId()));
                    filter.Add(new CommonFilterModel(Product_WMS_Interface_Properties.PropName_MapId2, "=", subOrder.Value.GetId()));
                    var wcfPWI = WCF<Product_WMS_Interface>.QueryAll(filter); // query，只取第一页第一条（同一个子单据不应该有2条数据）

                    if (null == wcfPWI || null == wcfPWI.Data)
                    {
                        pCount = TError.WCF_RunError.Int(); pMsg = "WCF返回null异常";
                        break;
                    } // TODO: 系统异常
                    else if (0 >= wcfPWI.RetInt) { tmpEntity = new Product_WMS_Interface(); } // 没有找到
                    else { tmpEntity = wcfPWI.Data[0]; } // 找到了一条

        #region 实例赋值并更新
                    tmpEntity.MapCalssID = TDict709_Value.EExwarehouseOrder.Int();
                    tmpEntity.MapId1 = (order.MangoOrder as MangoStockouOrder).ProductOutputMainId;
                    tmpEntity.MapId2 = (subOrder.Value.MangoOrder as MangoSubStockoutOrder).ProductOutputId;
                    tmpEntity.IsUpdateOK = pUpdateOk.Int();
                    tmpEntity.IsDel = pDel.Int();
                    tmpEntity.LastTime = DateTime.Now;
                    tmpEntity.UpdateUserID = MangoMis.Frame.Frame.CommonFrame.userid;
                    var updateRslt = (0 >= wcfPWI.RetInt) ? WCF<Product_WMS_Interface>.Add(tmpEntity) : WCF<Product_WMS_Interface>.Update(tmpEntity);
                    if (null == updateRslt) { pCount = TError.WCF_RunError.Int(); pMsg = "WCF返回null异常"; break; } // TODO: 系统异常
                    else { pCount++; pMsg = updateRslt.RETData; }
        #endregion
                }
        #endregion

                return pCount;
            }
            catch (Exception ex)
            {
                var ret = new ThirdResult<List<object>>("");
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return pCount = TError.WCF_RunError.Int();
            }
        }

        #region 单据操作
        /// <summary>
        /// 更新Dict[709]中MapId1对应的行。若Dict【709】没有对应行则插入
        /// </summary>
        /// <param name="pMapClassId">接口类型</param>
        /// <param name="pEid">主入库订单Id</param>
        /// <param name="pEsId">子单据Id</param>
        /// <param name="pUpdateOk">是否同步成功</param>
        /// <param name="pDel">是否删除</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>若成功则返回成功操作的行数；否则返回TError.Wcf_RunError</returns>
        static public int UpdateRowA_Order(TDict709_Value pMapClassId, string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            Product_WMS_Interface entity = new Product_WMS_Interface();
            entity.MapCalssID = pMapClassId.Int();
            entity.MapId1 = pEid.Int();
            entity.MapId2 = pEsId.Int();
            entity.IsUpdateOK = pUpdateOk.Int();
            entity.IsDel = pDel.Int();
            entity.LastTime = DateTime.Now;
            entity.UpdateUserID = MangoMis.Frame.Frame.CommonFrame.userid;
            entity.DisOrder = Product_WMS_Interface_Properties.cIntDisorderDefault;

            int ret = UpdateRowA_Order(entity, out pMsg);
            return ret;
        }

        /// <summary>
        /// 更新Dict[709]中pEntity对应的行。若Dict【709】没有对应行则插入
        /// </summary>
        /// <param name="pEntity">被插入的实体</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>若成功则返回成功操作的行数；否则返回TError</returns>
        static public int UpdateRowA_Order(Product_WMS_Interface pEntity, out string pMsg)
        {
        #region  validate parameters
            //int ret = TError.WCF_RunError.Int();
            pMsg = string.Empty;
            DefaultResult<List<Product_WMS_Interface>> queryRslt = null;
            DefaultResult<int> rslt = null;
            if (null == pEntity){return TError.Post_ParamError.Int();}
        #endregion
        #region 在Dict[709]中找pEntity, var wcfPWI 
            var filter = new List<CommonFilterModel>() { }; // query filter
            filter.Add(new CommonFilterModel(Product_WMS_Interface_Properties.PropName_MapId1, "=", pEntity.MapId1.ToString()));
            filter.Add(new CommonFilterModel(Product_WMS_Interface_Properties.PropName_MapId2, "=", pEntity.MapId2.ToString()));
            queryRslt = WCF<Product_WMS_Interface>.QueryAll(filter); // query，只取第一页第一条（同一个子单据不应该有2条数据）
            if (null == queryRslt)
            {
                pMsg = "尝试获取符合条件的实体，WCF操作失败，返回null对象";
                return TError.WCF_RunError.Int();
            }
            else if (null == queryRslt.Data)
            {
                pMsg = string.Format("WCF操作失败, queryRslt.RetInt={0}, queryRslt.RetData={1}, queryRslt.Data={2}", queryRslt.RetInt, queryRslt.RETData, queryRslt.Data);
                return queryRslt.RetInt;
            }
        #endregion
        #region WCF操作，更新或插入
            if (0 == queryRslt.Data.Count) {
                rslt = WCF<Product_WMS_Interface>.Add(pEntity);
            } // Dict[709]中没有符合条件的行，添加
            else {
                pEntity.WMS_InterfaceId = queryRslt.Data[0].WMS_InterfaceId;
                rslt = WCF<Product_WMS_Interface>.Update(pEntity);
            } // Dict[709]中有符合条件的行，更新
        #endregion
        #region Handle result
            if (null == rslt)
            {
                pMsg = "更新或插入，WCF操作记录行失败，返回null对象";
                return TError.WCF_RunError.Int();
            }
            else
            {
                pMsg = string.Format("UpdateRowA(Product_WMS_Interface pEntity)结束, rslt.RetInt={0}, rslt.RetData={1}, rslt.Data={2}", rslt.RetInt, rslt.RETData, rslt.Data);
                return 1;
            }
        #endregion
        }

        /// <summary>
        /// 更新Dict[709]中MapId1对应的行
        /// </summary>
        /// <param name="pMapClassId">接口类型</param>
        /// <param name="pEid">主单据Id</param>
        /// <param name="pEsId">子单据Id</param>
        /// <param name="pUpdateOk">是否同步成功</param>
        /// <param name="pDel">是否删除</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>若成功则返回成功操作的行数；否则返回TError.Wcf_RunError</returns>
        static public int UpdateRow_Order(TDict709_Value pMapClassId, string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            Product_WMS_Interface entity = new Product_WMS_Interface();
            entity.MapCalssID = pMapClassId.Int();
            entity.MapId1 = pEid.Int();
            entity.MapId2 = pEsId.Int();
            entity.IsUpdateOK = pUpdateOk.Int();
            entity.IsDel = pDel.Int();
            entity.LastTime = DateTime.Now;
            entity.UpdateUserID = MangoMis.Frame.Frame.CommonFrame.userid;
            entity.DisOrder = Product_WMS_Interface_Properties.cIntDisorderDefault;

            int ret = UpdateRow_Order(entity, out pMsg);
            return ret;
        }

        /// <summary>
        /// 更新Dict[709]中pEntity对应的行。
        /// </summary>
        /// <param name="pEntity">被插入的实体</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>若成功则返回成功操作的行数；否则返回TError.WCF_RunError</returns>
        static public int UpdateRow_Order(Product_WMS_Interface pEntity, out string pMsg)
        {
        #region  validate parameters
            pMsg = string.Empty;
            DefaultResult<List<Product_WMS_Interface>> queryRslt = null;
            DefaultResult<int> rslt = null;
            if (null == pEntity) { return TError.Post_ParamError.Int(); }
        #endregion
        #region 在Dict[709]中找pEntity, var wcfPWI 
            var filter = new List<CommonFilterModel>() { }; // query filter
            filter.Add(new CommonFilterModel(Product_WMS_Interface_Properties.PropName_MapId1, "=", pEntity.MapId1.ToString()));
            filter.Add(new CommonFilterModel(Product_WMS_Interface_Properties.PropName_MapId2, "=", pEntity.MapId2.ToString()));
            filter.Add(new CommonFilterModel(Product_WMS_Interface_Properties.PropName_IsDel, "!=", TDict285_Values.EDeleted.Int().ToString()));
            queryRslt = WCF<Product_WMS_Interface>.Query(1, 1, filter, new List<CommonOrderModel>() { }); // query，只取第一页第一条（同一个子单据不应该有2条数据）
            if (null == queryRslt)
            {
                pMsg = "尝试获取符合条件的实体，WCF操作失败，返回null对象";
                return TError.WCF_RunError.Int();
            }
            else if (null == queryRslt.Data)
            {
                pMsg = queryRslt.RETData;
                return queryRslt.RetInt;
            }
        #endregion
        #region WCF操作，更新
            if (0 == queryRslt.Data.Count)
            {
                pMsg = string.Format("Dict[709]中没有符合条件的行, MapId1={0}, MapId2={1}", pEntity.MapId1.ToString(), pEntity.MapId2.ToString());
                return TError.WCF_RunError.Int();
            } // Dict[709]中没有符合条件的行
            else
            {
                pEntity.WMS_InterfaceId = queryRslt.Data[0].WMS_InterfaceId;

                rslt = WCF<Product_WMS_Interface>.Update(pEntity);
        #region Handle result
                if (null == rslt)
                {
                    pMsg = "更新或插入，WCF操作记录行失败，返回null对象";
                    return TError.WCF_RunError.Int();
                }
                else
                {
                    pMsg = string.Format("UpdateRowA(Product_WMS_Interface pEntity)结束, rslt.RetInt={0}, rslt.RetData={1}, rslt.Data={2}", rslt.RetInt, rslt.RETData, rslt.Data);
                    return 1;
                }
        #endregion
            } // Dict[709]中有符合条件的行，更新
        #endregion
        }

        /// <summary>
        /// 更新Dict[709]中MapId1对应的行。若Dict【709】没有对应行则返回
        /// </summary>
        /// <param name="pMapClassId">接口类型</param>
        /// <param name="pEid">主单据Id</param>
        /// <param name="pUpdateOk">是否同步成功</param>
        /// <param name="pDel">是否删除</param>
        /// <param name="pfList">返回操作失败的实体列表，若失败则返回Count=0的列表实体</param>
        /// <param name="pWcfError">返回WCF操作结果</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>若成功则返回成功操作的行数；否则返回TError.Wcf_RunError</returns>
        static public int UpdateRowV_Order(TDict709_Value pMapClassId, string pEid, TDict285_Values pUpdateOk, TDict285_Values pDel
            , out List<Product_WMS_Interface> pfList, out int pWcfError, out string pMsg)
        {
            try
            {
                int rslt = 0;
                List<Product_WMS_Interface> eList = null;
                // 取出主单据Id所包含的所有子单据
                var order = CWms.CWmsDataFactory.GetCWmsOrder(Dict709ToMoc(pMapClassId), pEid);
                // 获取待更新的实体列表
                eList = GetVPwiEntities(pMapClassId, Dict709ToMoc(pMapClassId), order, pUpdateOk, pDel);
                // 更新
                rslt = UpdateRowV_Order(eList, out pfList, out pWcfError, out pMsg);

                return rslt;
            }
            catch (Exception ex)
            {
                pfList = new List<Product_WMS_Interface>(1);
                pWcfError = TError.WCF_RunError.Int();
                pMsg = ex.Message;

                var ret = new ThirdResult<List<object>>("");
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return TError.WCF_RunError.Int();
            }
        }

        /// <summary>
        /// 更新Dict[709]中MapId1对应的行。若Dict【709】没有对应行则返回
        /// </summary>
        /// <param name="pList">待更新的实体列表</param>
        /// <param name="pfList">返回更新失败的实体列表</param>
        /// <param name="pWcfError">返回WCF操作结果</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns></returns>
        static public int UpdateRowV_Order(List<Product_WMS_Interface> pList, out List<Product_WMS_Interface> pfList, out int pWcfError, out string pMsg)
        {
        #region template parameters
            int rslt = 0;
            pfList = new List<Product_WMS_Interface>(1);
            pWcfError = TError.RunGood.Int();
            pMsg = string.Empty;
        #endregion
        #region validate parameters
            if (null == pList)
            {
                pWcfError = TError.Post_NoParam.Int(); pMsg = "UpdateRowV(List<Product_WMS_Interface>)错误，非法入参";
                return rslt;
            }
        #endregion
        #region UpdateList
            foreach (var e in pList)
            {
                string tmpMsg = string.Empty;
                int tmpRslt = UpdateRow_Order(e, out tmpMsg);    // Update row
                pMsg += string.Format("\r\n{0}", tmpMsg);
                if (0 == tmpRslt) { pfList.Add(e); pWcfError = TError.WCF_RunError.Int(); }
                else rslt += tmpRslt;
            }
        #endregion
            return rslt;
        }

        /// <summary>
        /// 更新Dict[709]中MapId1对应的行。若Dict【709】没有对应行则添加。
        /// 若成功则返回成功操作的行数；否则返回TError.Wcf_RunError
        /// </summary>
        /// <param name="pMapClassId">接口类型</param>
        /// <param name="pEid">主单据Id</param>
        /// <param name="pUpdateOk">是否同步成功</param>
        /// <param name="pDel">是否删除</param>
        /// <param name="pfList">返回操作失败的实体列表，若失败则返回Count=0的列表实体</param>
        /// <param name="pWcfError">返回WCF操作结果</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>若成功则返回成功操作的行数；否则返回TError.Wcf_RunError</returns>
        static public int UpdateRowVA_Order(TDict709_Value pMapClassId, string pEid, TDict285_Values pUpdateOk, TDict285_Values pDel
            , out List<Product_WMS_Interface> pfList, out int pWcfError, out string pMsg)
        {
            try
            {
                int rslt = 0;
                List<Product_WMS_Interface> eList = null;
                // 取出主单据Id所包含的所有子单据
                var order = CWms.CWmsDataFactory.GetCWmsOrder(Dict709ToMoc(pMapClassId), pEid);
                // 获取待更新的实体列表
                eList = GetVPwiEntities(pMapClassId, Dict709ToMoc(pMapClassId), order, pUpdateOk, pDel);
                // 更新
                rslt = UpdateRowVA_Order(eList, out pfList, out pWcfError, out pMsg);

                return rslt;
            }
            catch (Exception ex)
            {
                pfList = new List<Product_WMS_Interface>(1);
                pWcfError = TError.WCF_RunError.Int();
                pMsg = ex.Message;

                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "{0}发生异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
                return TError.WCF_RunError.Int();
            }
        }

        /// <summary>
        /// 更新Dict[709]中MapId1对应的行。若Dict【709】没有对应行则添加
        /// </summary>
        /// <param name="pList">待更新的实体列表</param>
        /// <param name="pfList">返回更新失败的实体列表</param>
        /// <param name="pWcfError">返回WCF操作结果</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>若成功则返回成功操作的行数；否则返回TError.Wcf_RunError</returns>
        static public int UpdateRowVA_Order(List<Product_WMS_Interface> pList, out List<Product_WMS_Interface> pfList, out int pWcfError, out string pMsg)
        {
        #region template parameters
            int rslt = 0; // 
            pfList = new List<Product_WMS_Interface>(1);
            pWcfError = TError.RunGood.Int();
            pMsg = string.Empty;
        #endregion
        #region validate parameters
            if (null == pList)
            {
                pWcfError = TError.Post_NoParam.Int(); pMsg = "UpdateRowVA(List<Product_WMS_Interface>)错误，非法入参";
                return rslt;
            }
        #endregion
        #region UpdateList
            foreach (var e in pList)
            {
                string tmpMsg = string.Empty;
                int tmpRslt = UpdateRowA_Order(e, out tmpMsg);    // Update row
                pMsg += string.Format("\r\n{0}", tmpMsg);
                if (0 == tmpRslt) { pfList.Add(e); pWcfError = TError.WCF_RunError.Int(); }
                else rslt += tmpRslt;
            }
        #endregion
            return rslt;
        }

        /// <summary>
        /// 向Dict[709]中插入一行
        /// </summary>
        /// <param name="pEid">主单据Id</param>
        /// <param name="pMapClassId">接口类型</param>
        /// <param name="pEsId">子单据Id</param>
        /// <param name="pUpdateOk">是否同步成功</param>
        /// <param name="pDel">是否删除</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>返回操作成功的行数，若失败则返回TErrr.WCF_RunError</returns>
        static public int AddRow_Order(TDict709_Value pMapClassId, string pEid, string pEsId, TDict285_Values pUpdateOk, TDict285_Values pDel, out string pMsg)
        {
            try
            {
        #region Product_WMS_Interface entity = new Product_WMS_Interface();
                Product_WMS_Interface entity = new Product_WMS_Interface();
                entity.MapCalssID = pMapClassId.Int();
                entity.MapId1 = pEid.Int();
                entity.MapId2 = pEsId.Int();
                entity.IsUpdateOK = pUpdateOk.Int();
                entity.IsDel = pDel.Int();
                entity.AddTime = entity.LastTime = DateTime.Now;
                entity.AddUserid = entity.UpdateUserID = MangoMis.Frame.Frame.CommonFrame.userid;
                entity.DisOrder = Product_WMS_Interface_Properties.cIntDisorderDefault;
        #endregion
                return AddRow_Order(entity, out pMsg);
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;

                var ret = new ThirdResult<List<object>>("");
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return TError.WCF_RunError.Int();
            }
        }

        /// <summary>
        /// 向Dict[709]中插入一行
        /// </summary>
        /// <param name="pEntity">带插入的实体</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>返回操作成功的行数，若失败则返回DefaultResult[int].RetInt</returns>
        static public int AddRow_Order(Product_WMS_Interface pEntity, out string pMsg)
        {
        #region  validate parameters
            if (null == pEntity)
            {
                pMsg = "AddRow结束，非法入参";
                return TError.Post_ParamError.Int();
            }
        #endregion
            // WCF操作
            var retRslt = WCF<Product_WMS_Interface>.Add(pEntity);
        #region Handle result
            if (null == retRslt)
            {
                pMsg = "WCF插入记录行失败，返回null对象";
                return TError.WCF_RunError.Int();
            }
            else
            {
                pMsg = string.Format("WCF插入记录行结束, retRslt.RetInt={0}, retRslt.RETData={1}, retRslt.Data={2}", retRslt.RetInt, retRslt.RETData, retRslt.Data);
                return retRslt.RetInt;
            }
        #endregion
        }

        /// <summary>
        /// 向Dict[709]中插入多行
        /// </summary>
        /// <param name="pList">带插入的实体列表</param>
        /// <param name="pfList">返回操作失败的实体列表，若失败则返回Count=0的列表实体</param>
        /// <param name="pWcfError">返回WCF操作结果</param>
        /// <param name="pMsg">返回错误描述</param>
        /// <returns>返回操作成功的行数，若失败则返回0</returns>
        static public int AddRowV_Order(List<Product_WMS_Interface> pList, out List<Product_WMS_Interface> pfList, out int pWcfError, out string pMsg)
        {
        #region template parameters
            int rslt = 0;
            pfList = new List<Product_WMS_Interface>(1);
            pWcfError = TError.RunGood.Int();
            pMsg = string.Empty;
        #endregion
        #region validate parameters
            if (null == pList)
            {
                pWcfError = TError.Post_NoParam.Int(); pMsg = "AddRowV(List<Product_WMS_Interface>, out List<Product_WMS_Interface>, out int, out string)错误，非法入参";
                return rslt;
            }
        #endregion
            foreach (var e in pList)
            {
                string tmpMsg = string.Empty;
                int tmpRslt = AddRow_Order(e, out tmpMsg);
                pMsg += string.Format("\r\n{0}", tmpMsg);
                if (0 == tmpRslt) { pfList.Add(e); pWcfError = TError.WCF_RunError.Int(); }
                else rslt += tmpRslt;
                //(0 == tmpRslt) ? pfList.Add(e) : rslt += tmpRslt;
            }
            return rslt;
        }
        #endregion

        /// <summary>
        /// 公司内部通用debug
        /// </summary>
        /// <param name="dbgObj">The debug object.</param>
        /// <param name="arg">The argument.</param>
        /// <param name="flag"></param>
        /// <remarks>
        /// ------+++++++++-------
        /// --姓名:dongtianyi时间:2018/5/29 16:43
        /// --操作简单描述:
        /// --本次操作需求来源URL:
        ///  --当前设备：DONGTIANYI
        /// ------+++++++++-------
        /// </remarks>
        static protected void InnerDebug<T>(ThirdResult<List<object>> dbgObj, DefaultResult<T> arg, string flag)
        {
            if (null == dbgObj)
                return;

            if (null == arg || null == arg.Data)
            {
                dbgObj.Append("WCF运行未取得列表.判定为WCFAPI连接失败");
            }
            else if (0 >= arg.RetInt)
            {
                dbgObj.Append("WCF运行取得的返回总数小于等于0,判定为WCF未取得数据");
            }
            else
            {
                dbgObj.Append(string.Format("WCF运行，成功获取商品规格关联表中的数据，获取行数{0}", arg.RetInt));
            }
        }

        /// <summary>
        /// 根据单据类实体创建并返回Product_WMS_Interface实体
        /// </summary>
        /// <param name="pMapClassId">Dict[709].pMapClassId</param>
        /// <param name="pCate">单据类型</param>
        /// <param name="pOrder">单据实体</param>
        /// <param name="pUpdateOk">是否同步成功</param>
        /// <param name="pDel">是否删除</param>
        /// <returns>若成功则返回Product_WMS_Interface实体；否则返回null</returns>
        static protected List<Product_WMS_Interface> GetVPwiEntities(TDict709_Value pMapClassId, TCWmsOrderCategory pCate, CWmsOrderBase pOrder, TDict285_Values pUpdateOk, TDict285_Values pDel)
        {
            List<Product_WMS_Interface> eList = null;
            if (null == pOrder)
            {
                return eList;
            }

            try
            {
                eList = new List<Product_WMS_Interface>(1);
        #region 遍历子单据，创建并添加Product_WMS_Interface实体
                foreach (var so in pOrder.SubOrders)
                {
                    Product_WMS_Interface entity = new Product_WMS_Interface();
                    entity.MapCalssID = pMapClassId.Int();
                    entity.IsUpdateOK = pUpdateOk.Int();
                    entity.IsDel = pDel.Int();
                    entity.LastTime = DateTime.Now;
                    entity.UpdateUserID = MangoMis.Frame.Frame.CommonFrame.userid;
                    entity.DisOrder = Product_WMS_Interface_Properties.cIntDisorderDefault;
                    switch (pCate)
                    {
                        case TCWmsOrderCategory.EEntryOrder:
                            {
                                entity.MapId1 = (pOrder.MangoOrder as MangoEntryOrder).ProductInputMainId;
                                entity.MapId2 = (so.Value.MangoOrder as MangoSubEntryOrder).ProductInputId;
                                entity.AddTime = (so.Value.MangoOrder as MangoSubEntryOrder).AddTime;
                                entity.AddUserid = (so.Value.MangoOrder as MangoSubEntryOrder).AddUserid;
                                eList.Add(entity);
                                break;
                            }
                        case TCWmsOrderCategory.EExwarehouseOrder:
                            {
                                entity.MapId1 = (pOrder.MangoOrder as MangoStockouOrder).ProductOutputMainId;
                                entity.MapId2 = (so.Value.MangoOrder as MangoSubStockoutOrder).ProductOutputId;
                                entity.AddTime = (so.Value.MangoOrder as MangoSubStockoutOrder).AddTime;
                                entity.AddUserid = (so.Value.MangoOrder as MangoSubStockoutOrder).AddUserid;
                                eList.Add(entity);
                                break;
                            }
                        case TCWmsOrderCategory.EReturnOrder:
                            {
                                entity.MapId1 = (pOrder.MangoOrder as MangoReturnOrder).TuiHuoMainID;
                                entity.MapId2 = (so.Value.MangoOrder as MangoSubReturnOrder).ZiTuihuoID;
                                entity.AddTime = (so.Value.MangoOrder as MangoSubReturnOrder).AddTime;
                                entity.AddUserid = (so.Value.MangoOrder as MangoSubReturnOrder).AddUserid;
                                eList.Add(entity);
                                break;
                            }
                        default: { break; }
                    }
                }
        #endregion

                return eList;
            }
            catch (Exception ex)
            {
                var ret = new ThirdResult<List<object>>("");
                if (null != ex.InnerException) ret.Append(string.Format("InnerException: {0}", ex.InnerException.Message));
                ret.Append(string.Format("发生异常: {0}\r\n调用堆栈：{1}", ex.Message, ex.StackTrace));
                ret.End();
                return null;
            }
        }

        /// <summary>
        /// 根据Dict[709]的值返回单据类型(TCWmsOrderCategory)
        /// </summary>
        /// <param name="pMapClassId">Dict[709].MapClassId</param>
        /// <returns>返回TCWmsOrderCategory中的值</returns>
        static public TCWmsOrderCategory Dict709ToMoc(TDict709_Value pMapClassId)
        {
            switch (pMapClassId)
            {
                case TDict709_Value.EEntryOrder: { return TCWmsOrderCategory.EEntryOrder; }
                case TDict709_Value.EExwarehouseOrder: { return TCWmsOrderCategory.EExwarehouseOrder; }
                case TDict709_Value.ECancelReturnOrder: { return TCWmsOrderCategory.EMcocReturnOrder; }
                case TDict709_Value.EReturnOrder: { return TCWmsOrderCategory.EReturnOrder; }
                default: { return TCWmsOrderCategory.EUnknownCategory; }
            }
        }

        /// <summary>
        /// 根据单据类型(TCWmsOrderCategory)的值返回Dict[709]
        /// </summary>
        /// <param name="pMoc">TCWmsOrderCategory</param>
        /// <returns>返回TDict709_Value</returns>
        static public TDict709_Value Dict709FromMoc(TCWmsOrderCategory pMoc)
        {
            switch (pMoc)
            {
                case TCWmsOrderCategory.EEntryOrder: return TDict709_Value.EEntryOrder;
                case TCWmsOrderCategory.EExwarehouseOrder: return TDict709_Value.EExwarehouseOrder;
                case TCWmsOrderCategory.EReturnOrder: return TDict709_Value.EReturnOrder;
                case TCWmsOrderCategory.EMcocReturnOrder: return TDict709_Value.ECancelReturnOrder;
                default: return TDict709_Value.EUnknown;
            }
        }
    }

    /// <summary>
    /// MIS实体数据操作类
    /// </summary>
    public class CWmsMisHandler
    {
        /// <summary>
        /// 更新Mis实体。若执行成功则返回WCF.RetInt，否则返回其他
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">待更新实体</param>
        /// <returns></returns>
        static public int UpdateEntity<T>(T entity) where T : class, new()
        {
            try
            {
                if (null == entity)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Error("在{0}中，待更新实体为空", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    return TError.Pro_HaveNoData.Int();
                }
                var rslt = WCF<T>.Update(entity);
                return (null == rslt) ? TError.WCF_RunError.Int() : rslt.RetInt.Int();
            }
            catch(Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Error(ex, "更新实体[{0}]发生异常", entity);
                return TError.WCF_RunError.Int();
            }
        }
#endif
    }
}
