using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.CWms.Interfaces.Methods
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    interface IMWmsTransactionImpl<TRequest, TResponse> : IDisposable, IMWmsAsyncTransactionImpl
    {
        /// <summary>
        /// 判断是否从WMS系统响应同步成功
        /// </summary>
        /// <param name="pResp"></param>
        bool TransactionIsSuccess(TResponse pResp);

        /// <summary>
        /// 返回API接口的method
        /// </summary>
        /// <returns></returns>
        string GetApiMethod();

        /// <summary>
        /// 针对int DoTransaction(out HttpRespXmlBase, out string, params object[])和TResponse DoTransaction(params object[])方法，解析params object[]入参。
        /// 若解析成功则返回TError.RunGood；否则返回其他值
        /// </summary>
        /// <param name="args">带解析的参数</param>
        /// <returns>若解析成功则返回TError.RunGood；否则返回其他值</returns>
        int ParseArguments(params object[] args);

        /// <summary>
        /// 在执行同步操作之前，先重置709字典中对应的行，将IsUpdateOK的值置为同步失败。
        /// 若重置成功则影响的行数（重置多行）或影响行的主键（重置单行）；否则返回TError或0
        /// </summary>
        /// <returns>若重置成功则影响的行数（重置多行）或影响行的主键（重置单行）；否则返回TError或0</returns>
        int Reset709();

        /// <summary>
        /// 根据同步通讯的结果更新709字典中对应的行的IsUpdateOK的值。
        /// 若重置成功则影响的行数（重置多行）或影响行的主键（重置单行）；否则返回TError或0
        /// </summary>
        /// <param name="pUpdateOK">flag - 是否同步成功</param>
        /// <returns>若重置成功则影响的行数（重置多行）或影响行的主键（重置单行）；否则返回TError或0</returns>
        int Update709(bool pUpdateOK);

        /// <summary>
        /// 创建同步通讯的Request数据对象（存储在RequestObject中）。若成功则返回TError.RunGood; 否则返回其他
        /// </summary>
        /// <returns>若成功则返回TError.RunGood; 否则返回其他</returns>
        TRequest NewRequestObj();

        /// <summary>
        /// 执行同步通讯。若通讯成功则返回TError.RunGood; 否则返回其他
        /// </summary>
        /// <param name="pReqObj"></param>
        /// <returns>若通讯成功则返回TError.RunGood; 否则返回其他</returns>
        TResponse DoTransaction(TRequest pReqObj);
    }

    /// <summary>
    /// 
    /// </summary>
    interface IMWmsAsyncTransactionImpl
    {
        /// <summary>
        /// 激活计时器，启动异步通讯。若操作成功则返回TError.RunGood；否则返回其他值
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        int ActivateImpl(params object[] args);

        /// <summary>
        /// 执行异步通讯逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        int RunImpl(object sender, EventArgs args);
    }
}
