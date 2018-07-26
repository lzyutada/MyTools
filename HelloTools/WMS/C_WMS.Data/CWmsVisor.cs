using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C_WMS.Interface.CWms.Interfaces.Data;
using C_WMS.Data.Wms.Transaction;
using C_WMS.Data.Wms.Data;
using MangoMis.MisFrame.Frame;
using C_WMS.Interface;

namespace C_WMS.Data
{ //此cs放置的都是一些请求访问的基础方法
    /// <summary>
    /// WMS同步接口(ICWmsVisor)的实现类
    /// </summary>
    public class CWmsVisor : ICWmsVisor
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsVisor()
        {         
        }

        #region 调用接口
        /// <summary>
        /// 同步单个商品。返回WMS系统响应的XML内容反序列化后的实体， 若执行失败则返回null
        /// </summary>
        /// <param name="pId">商品编码</param>
        /// <param name="pRespXmlObj">返回WMS系统响应的XML内容反序列化后的实体， 若执行失败则返回null</param>
        public void SingleItemSync(int pId, out HttpRespXmlBase pRespXmlObj)
        {
            C_WMS.Data.Utility.MyLog.Instance.Info("同步单个商品{0}，开始", pId);
            var trans = new CWmsTransactionItemSync();
            pRespXmlObj = trans.DoTransaction(pId);
            C_WMS.Data.Utility.MyLog.Instance.Info("同步单个商品{0}，结束。返回：{1}", pId, pRespXmlObj);
        }

        /// <summary>
        /// 批量同步商品（同步操作）。返回WMS系统响应的XML内容反序列化后的实体， 若执行失败则返回null
        /// </summary>
        /// <param name="pRespXmlObj">返回WMS系统响应的XML内容反序列化后的实体， 若执行失败则返回null</param>
        [Obsolete("应使用批量同步商品（异步操作）方法，void ItemsSyncAsync(out int)")]
        public void ItemsSync(out HttpRespXmlBase pRespXmlObj)
        {
            C_WMS.Data.Utility.MyLog.Instance.Info("[弃用的]批量同步商品（同步操作），开始");
            var trans = new CWmsTransactionItemSync();
            pRespXmlObj = trans.DoTransaction();
        }

        /// <summary>
        /// 批量同步商品（异步操作）。若操作成功启动异步计时器则返回TError.RunGood;否则返回其他值
        /// </summary>
        /// <param name="pError">若操作成功启动异步计时器则返回TError.RunGood;否则返回其他值</param>
        public void ItemsSyncAsync(out int pError)
        {
            C_WMS.Data.Utility.MyLog.Instance.Info("批量同步商品（异步操作），开始");
            var trans = new CWmsTransactionItemSync();
            pError = trans.Activate();
            C_WMS.Data.Utility.MyLog.Instance.Info("批量同步商品（异步操作）结束。启动异步计时器结果{0}", pError);
        }

        /// <summary>
        /// 同步创建商品退货单。返回WMS系统响应的XML内容反序列化后的实体， 若执行失败则返回null
        /// </summary>
        /// <param name="pReturnOrderId">主退货订单Id</param>
        /// <param name="pRespXmlObj">返回WMS系统响应的XML内容反序列化后的实体， 若执行失败则返回null</param>    
        /// <returns></returns>
        public void ReturnOrderCreate(string pReturnOrderId, out HttpRespXmlBase pRespXmlObj)
        {
            C_WMS.Data.Utility.MyLog.Instance.Info("同步创建商品退货单{0}，开始", pReturnOrderId);
            string msg = string.Empty;
            HttpRespXml_ReturnOrderCreate tmpResp = null;
            using (var trans = new CWmsReturnOrderCreate())
            {
                trans.DoTransaction(out tmpResp, out msg, pReturnOrderId);
                pRespXmlObj = tmpResp;
            }
            C_WMS.Data.Utility.MyLog.Instance.Info("同步创建商品退货单{0}，结束。返回: \r\nRESPONSE={1}\r\nerrMsg={2}", pReturnOrderId, pRespXmlObj, msg);
        }

        /// <summary>
        /// 同步创建商品出库单。返回WMS系统响应的XML内容反序列化后的实体， 若执行失败则返回null
        /// </summary>
        /// <param name="pStockoutId">主出库单ID</param>    
        /// <param name="pRespXmlObj">返回WMS系统响应的XML内容反序列化后的实体， 若执行失败则返回null</param>
        public void StockoutOrderCreate(string pStockoutId, out HttpRespXmlBase pRespXmlObj)
        {
            C_WMS.Data.Utility.MyLog.Instance.Info("同步创建商品出库单{0}，开始", pStockoutId);
            using (var trans = new CWmsStockoutCreate())
            {
                pRespXmlObj = trans.DoTransaction(pStockoutId);
            }
            C_WMS.Data.Utility.MyLog.Instance.Info("同步创建商品出库单{0}，结束。返回: \r\nRESPONSE={1}", pStockoutId, pRespXmlObj);
        }

        /// <summary>
        /// 同步创建商品入库单。返回WMS系统响应的XML内容反序列化后的实体， 若执行失败则返回null
        /// </summary>
        /// <param name="pEntryOrderId">对应的主入库单ID</param>    
        /// <param name="pRespXmlObj">返回WMS系统响应的XML内容反序列化后的实体， 若执行失败则返回null</param>
        public void EntryOrderCreate(string pEntryOrderId, out HttpRespXmlBase pRespXmlObj)
        {
            C_WMS.Data.Utility.MyLog.Instance.Info("同步创建商品入库单{0}，开始", pEntryOrderId);
            string msg = string.Empty;
            HttpRespXml_EntryOrderCreate tmpResp = null;
            using (var trans = new CWmsEntryOrderCreate())
            {
                trans.DoTransaction(out tmpResp, out msg, pEntryOrderId, "");
                pRespXmlObj = tmpResp;
            }
            C_WMS.Data.Utility.MyLog.Instance.Info("同步创建商品入库单{0}，结束。返回: \r\nRESPONSE={1}\r\nerrMsg={2}", pEntryOrderId, pRespXmlObj, msg);
        }

        /// <summary>
        /// 批量同步采购入库单（异步操作）。若操作成功启动异步计时器则返回TError.RunGood;否则返回其他值
        /// </summary>
        /// <param name="pError">若操作成功启动异步计时器则返回TError.RunGood;否则返回其他值</param>
        public void OrdersSyncAsync_EntryOrder(out int pError)
        {
            C_WMS.Data.Utility.MyLog.Instance.Info("批量同步采购入库单（异步操作），开始");
            using (var trans = new CWmsOrdersSync_EntryOrder())
            {
                pError = trans.Activate();
            }
            C_WMS.Data.Utility.MyLog.Instance.Info("批量同步采购入库单（异步操作）结束。启动异步计时器结果{0}", pError);
        }

        /// <summary>
        /// 批量同步出库单（异步操作）。若操作成功启动异步计时器则返回TError.RunGood;否则返回其他值
        /// </summary>
        /// <param name="pError">若操作成功启动异步计时器则返回TError.RunGood;否则返回其他值</param>
        public void OrdersSyncAsync_StockoutOrder(out int pError)
        {
            C_WMS.Data.Utility.MyLog.Instance.Info("批量同步出库单（异步操作），开始");
            using (var trans = new CWmsOrdersSync_StockoutOrder())
            {
                pError = trans.Activate();
            }
            C_WMS.Data.Utility.MyLog.Instance.Info("批量同步出库单（异步操作）结束。启动异步计时器结果{0}", pError);
        }
#endregion
    }
}
