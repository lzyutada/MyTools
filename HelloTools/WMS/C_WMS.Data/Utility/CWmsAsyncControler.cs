using MangoMis.Frame.Helper;
using MangoMis.Frame.ThirdFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C_WMS.Data.Utility
{
    /// <summary>
    /// 异步通讯控制器
    /// </summary>
    abstract public class CWmsAsyncControler : System.Timers.Timer
    {
        /// <summary>
        /// 最小时间间隔
        /// </summary>
        public static int MinInterval { get { return 500; } }

        /// <summary>
        /// 标志位，解决重入问题
        /// </summary>
        protected int inTimer = 0;

        /// <summary>
        /// default constructor
        /// </summary>
        public CWmsAsyncControler()
        {
            Interval = MinInterval;
            AutoReset = false;
            StopTimer();
        }

        /// <summary>
        /// constructor by setting interval
        /// </summary>
        /// <param name="pInterval"></param>
        public CWmsAsyncControler(UInt16 pInterval)
        {
            Interval = pInterval;
            AutoReset = false;
            StopTimer();
        }

        /// <summary>
        /// 启动计时器
        /// </summary>
        protected void StartTimer()
        {
            Start();
        }

        /// <summary>
        /// 停止计时器
        /// </summary>
        protected void StopTimer()
        {
            Stop();
        }

        /// <summary>
        /// 关闭计时器
        /// </summary>
        public new void Close()
        {
            StopTimer();
            base.Close();
            base.Dispose(true);
        }

        /// <summary>
        /// 激活计时器，启动异步通讯。若操作成功则返回TError.RunGood；否则返回其他值
        /// </summary>
        /// <returns>若操作成功则返回TError.RunGood；否则返回其他值</returns>
        virtual public int Activate()
        {
            int err = TError.RunGood.Int();
            try
            {
                Elapsed += RunL;
                StartTimer();
            }
            catch(Exception ex)
            {
                C_WMS.Data.Utility.MyLog.Instance.Fatal(ex, "{0}激活计时器异常", System.Reflection.MethodBase.GetCurrentMethod().Name);
                err = TError.WCF_RunError.Int();
            }
            return err;
        }

        /// <summary>
        /// 执行异步通讯的每一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        abstract protected void RunL(object sender, EventArgs args);
    }
}
