using MangoMis.Frame.DataSource.Simple;
using MangoMis.Frame.Helper;
using MangoMis.Frame.Helper.ApiClient;
using MangoMis.Frame.ThirdFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TT.Common.Frame.Model;

namespace C_WMS.Data.Mango
{
    /// <summary>
    /// WCF操作类型
    /// </summary>
    public enum TWCFOperation
    {
        /// <summary>
        /// get count of entity
        /// </summary>
        EGetCount,

        /// <summary>
        /// get entity
        /// </summary>
        EGetEntity,

        /// <summary>
        /// get entity list
        /// </summary>
        EGetList,

        /// <summary>
        /// update entity.
        /// </summary>
        EUpdate,

        /// <summary>
        /// update entity, add a new entity if it's not found.
        /// </summary>
        EUpdateA,

        /// <summary>
        /// add a new entity
        /// </summary>
        EAdd
    }

    /// <summary>
    /// 封装WCF的类，优先调用V3，调用失败改为V2
    /// </summary>
    public class MangoWCF<TEntity> where TEntity : class,new()
    {
        static IMangoWCF_ClientBase<TEntity> _v2Client = new MangoWCF_V2<TEntity>();
        static IMangoWCF_ClientBase<TEntity> _v3Client = new MangoWCF_V3<TEntity>();

        /// <summary>
        /// 根据过滤器pFilters获取所有满足条件的实体，若执行成功则返回DefaultResult[list[TEntity]]的实体，否则返回其他
        /// </summary>
        /// <param name="pFilters"></param>
        /// <returns></returns>
        static public DefaultResult<List<TEntity>> GetList(List<CommonFilterModel> pFilters)
        {
            DefaultResult<List<TEntity>> wcfRslt = null;
            if (null == (wcfRslt = _v3Client.GetList(pFilters)))
                wcfRslt = _v2Client.GetList(pFilters);
            return wcfRslt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFilters"></param>
        /// <param name="pOrders"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        static public DefaultResult<List<TEntity>> GetList(List<CommonFilterModel> pFilters, List<CommonOrderModel> pOrders, int page = 1, int size = 20 )
        {
            DefaultResult<List<TEntity>> wcfRslt = null;
            if (null == (wcfRslt = _v3Client.GetList(pFilters, pOrders, page, size)))
                wcfRslt = _v2Client.GetList(pFilters);
            return wcfRslt;
        }

        /// <summary>
        /// </summary>
        /// <param name="id">The entity.</param>
        /// <returns></returns>
        /// <remarks>
        /// 查询实体
        /// </remarks>
        static public DefaultResult<TEntity> GetEntity(int id)
        {
            DefaultResult<TEntity> wcfRslt = null;
            if (null == (wcfRslt = _v3Client.GetEntity(id)))//pFilters, pOrders, page, size)))
                wcfRslt = _v2Client.GetEntity(id);
            return wcfRslt;
        }

        /// <summary>
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks>
        /// 更新实体
        /// </remarks>
        static public DefaultResult<int> Update(TEntity entity)
        {
            DefaultResult<int> wcfRslt = null;
            if (null == (wcfRslt = _v3Client.Update(entity)))//pFilters, pOrders, page, size)))
                wcfRslt = _v2Client.Update(entity);
            return wcfRslt;
        }
        /// <summary>
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks>
        /// 添加实体
        /// </remarks>
        static public DefaultResult<int> Add(TEntity entity)
        {
            DefaultResult<int> wcfRslt = null;
            if (null == (wcfRslt = _v3Client.Add(entity)))//pFilters, pOrders, page, size)))
                wcfRslt = _v2Client.Add(entity);
            return wcfRslt;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    interface IMangoWCF_ClientBase<TEntity>  where TEntity : class, new()
    {
        DefaultResult<List<TEntity>> GetList(List<CommonFilterModel> pFilters);
        DefaultResult<List<TEntity>> GetList(List<CommonFilterModel> filters, List<CommonOrderModel> orders, int page = 1, int size = 20);
        DefaultResult<TEntity> GetEntity(int id);
        DefaultResult<int> Update(TEntity entity);
        DefaultResult<int> Add(TEntity entity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    class MangoWCF_V3<TEntity> : IMangoWCF_ClientBase<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 根据过滤器pFilters获取实体列表，若执行成功则返回DefaultResult[List[TEntity]]，否则返回null
        /// </summary>
        /// <param name="pFilters"></param>
        /// <returns></returns>
        public DefaultResult<List<TEntity>> GetList(List<CommonFilterModel> pFilters)
        {
            try
            {
                var rslt = new ClientV3<TEntity>().GetList(pFilters);
                if (null == rslt || null == rslt.Data)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("WCF(V3)，根据过滤器Filter={0}获取实体列表失败", pFilters);
                    return null;
                }
                return rslt;
            }
            catch(Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "WCF(V3)，根据过滤器Filter={0}获取实体列表异常", pFilters);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFilters"></param>
        /// <param name="pOrders"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DefaultResult<List<TEntity>> GetList(List<CommonFilterModel> pFilters, List<CommonOrderModel> pOrders, int page, int size)
        {
            try
            {
                var rslt = new ClientV3<TEntity>().GetList(page, size, pFilters, pOrders);
                if (null == rslt || null == rslt.Data)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("WCF(V3)，根据（{0}, {1}, {2}, {3}）获取实体列表失败. WCF_RESULT(rslt={4}, RetInt={5}, RETData={6}, Debug={7}). \r\nFILTER DEBUG: {8}", pFilters, pOrders, page, size, rslt, rslt?.RetInt, rslt?.RETData, rslt?.Debug, MisModelPWI.MisModelFactory.GetDebugInfo_MisFilter( pFilters));
                    return null;
                }
                return rslt;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Warning(ex, "WCF(V3)，根据（{0}, {1}, {2}, {3}）获取实体列表异常", pFilters, pOrders, page, size);
                return null;
            }
        }

        /// <summary>
        /// 根据主键Id获取实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public DefaultResult<TEntity> GetEntity(int id)
        {
            try
            {
                var rslt = new ClientV3<TEntity>().GetModel(id);
                if (null == rslt || null == rslt.Data)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("WCF(V3)，根据Id[{0}]获取实体失败", id);
                    return null;
                }
                return rslt;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "WCF(V3)，根据Id[{0}]获取实体异常", id);
                return null;
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DefaultResult<int> Update(TEntity entity)
        {
            try
            {
                var rslt = new ClientV3<TEntity>().Update(entity);
                if (null == rslt)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("WCF(V3)，更新实体[{0}]失败", entity);
                    return null;
                }
                return rslt;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "WCF(V3)，更新实体[{0}]异常", entity);
                return null;
            }
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DefaultResult<int> Add(TEntity entity)
        {
            try
            {
                var rslt = new ClientV3<TEntity>().Add(entity);
                if (null == rslt)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("WCF(V3)，添加实体[{0}]失败", entity);
                    return null;
                }
                return rslt;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "WCF(V3)，添加实体[{0}]异常", entity);
                return null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    class MangoWCF_V2<TEntity> : IMangoWCF_ClientBase<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 根据过滤器pFilters获取实体列表，若执行成功则返回DefaultResult[List[TEntity]]，否则返回null
        /// </summary>
        /// <param name="pFilters"></param>
        /// <returns></returns>
        public DefaultResult<List<TEntity>> GetList(List<CommonFilterModel> pFilters)
        {
            try
            {
                var rslt = WCF<TEntity>.QueryAll(pFilters);
                if (null == rslt || null == rslt.Data)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("WCF(V2)，根据过滤器Filter={0}获取实体列表失败", pFilters);
                    return null;
                }
                return rslt;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "WCF(V2)，根据过滤器Filter={0}获取实体列表异常", pFilters);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFilters"></param>
        /// <param name="pOrders"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DefaultResult<List<TEntity>> GetList(List<CommonFilterModel> pFilters, List<CommonOrderModel> pOrders, int page, int size)
        {
            try
            {
                var rslt = WCF<TEntity>.Query(page, size, pFilters, pOrders);
                if (null == rslt || null == rslt.Data)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("WCF(V2)，根据（{0}, {1}, {2}, {3}）获取实体列表失败", pFilters, pOrders, page, size);
                    return null;
                }
                return rslt;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "WCF(V2)，根据（{0}, {1}, {2}, {3}）获取实体列表异常", pFilters, pOrders, page, size);
                return null;
            }
        }

        /// <summary>
        /// 根据主键Id获取实体
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public DefaultResult<TEntity> GetEntity(int id)
        {
            try
            {
                var rslt = WCF<TEntity>.Query(id);
                if (null == rslt || null == rslt.Data)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("WCF(V2)，根据Id[{0}]获取实体失败", id);
                    return null;
                }
                return rslt;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "WCF(V2)，根据Id[{0}]获取实体异常", id);
                return null;
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DefaultResult<int> Update(TEntity entity)
        {
            try
            {
                var rslt = WCF<TEntity>.Update(entity);
                if (null == rslt)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("WCF(V2)，更新实体[{0}]失败", entity);
                    return null;
                }
                return rslt;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "WCF(V2)，更新实体[{0}]异常", entity);
                return null;
            }
        }

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DefaultResult<int> Add(TEntity entity)
        {
            try
            {
                var rslt = WCF<TEntity>.Add(entity);
                if (null == rslt)
                {
                    C_WMS.Data.Utility.MyLog.Instance.Warning("WCF(V2)，添加实体[{0}]失败", entity);
                    return null;
                }
                return rslt;
            }
            catch (Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "WCF(V2)，添加实体[{0}]异常", entity);
                return null;
            }
        }
    }
}
