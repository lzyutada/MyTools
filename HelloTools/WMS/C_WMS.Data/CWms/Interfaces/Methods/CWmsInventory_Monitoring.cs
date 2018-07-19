using C_WMS.Data.CWms.CWmsEntity;
using C_WMS.Data.CWms.Interfaces.Data;
using C_WMS.Data.Mango;
using C_WMS.Interface.CWms.Interfaces.Data;
using C_WMS.Interface.Utility;
using MangoMis.Frame.ThirdFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.Interfaces.Methods
{
    /// <summary>
    /// 库存监控接口
    /// </summary>
    public class CWmsInventory_Monitoring : CWmsPostTransactionBase
    {
        /// <summary>
        /// 返回API接口的method
        /// </summary>
        /// <returns></returns>
        override public string GetApiMethod()
        {
            return "inventory.monitoring";
        }
        /// <summary>
        /// 执行接口inventory.monitoring的HTTP Transaction
        /// </summary>
        /// <returns></returns>
        override public HttpRespXmlBase DoTransaction()
        {
            try
            {
                HttpReqXmlBase reqBody = new HttpReqXml_ItemsSyncronize();              
                var plist = MangoFactory.GetMangoProductList();       
                return Post(reqBody);
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
        /// 执行HTTP会话
        /// </summary>
        /// <param name="plist"></param>
        /// <param name="wname"></param>
        /// <returns></returns>
        public HttpRespXmlBase DoTransaction(CWmsProduct plist, string wname)
        {
            var ret = new ThirdResult<List<object>>("库存监控，执行HTTP会话开始");
            Wms.Data.WmsOwner owner = MangoFactory.GetDefaultOwner();   // 取默认货主

            HttpReqXmlBase reqBody = new HttpReqXml_InventoryMonitoring();

            (reqBody as HttpReqXml_InventoryMonitoring).inventoryMonitoringList = new List<HttpReqXml_InventoryMonitoring_item>();
            (reqBody as HttpReqXml_InventoryMonitoring).inventoryMonitoringList.Add(new
                 HttpReqXml_InventoryMonitoring_item(wname, owner.WmsID, plist.ItemCode));
            var resp = Post(reqBody);
            ret.Append(string.Format("库存监控，执行HTTP会话结束, Response={0}", resp));
            ret.End();
            return resp;
        }

        /// <summary>
        /// 处理服务器响应
        /// </summary>
        /// <param name="respBody">HTTP响应体</param>
        /// <param name="encode">HTTP响应Header中ContentEncoding对应的编码格式</param>
        /// <returns>返回Response XML对应的数据实例</returns>
        override public HttpRespXmlBase HandleResponse(byte[] respBody, Encoding encode)
        {
            var ret = new ThirdResult<List<object>>("库存监控，处理服务器响应");

            if (null == encode) throw new ArgumentNullException("库存监控，处理服务器响应。无效的Encoding对象");
            if (null == respBody)
            {
                ret.Append("库存监控，处理服务器响应。ResponseBody为空。");
                ret.End();
                return null;
            }

            HttpRespXml_InventoryMonitoring respObj = new HttpRespXml_InventoryMonitoring();
            string respXml = encode.GetString(respBody);
            respObj = CWmsUtility.ObjtoXml(respObj.GetType(), respXml) as HttpRespXml_InventoryMonitoring;

            ret.Append("库存监控，处理服务器响应完成");
            ret.End();
            return respObj;
        }

        public override int DoTransaction(out HttpRespXmlBase pResp, out string pMsg, params object[] args)
        {
            throw new NotImplementedException();
        }

        protected override void RunL(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
