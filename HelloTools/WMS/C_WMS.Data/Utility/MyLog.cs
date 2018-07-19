using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using MangoMis.Frame.Helper;
using System.Management;

namespace C_WMS.Data.Utility
{
    #region usefull tip
    // StackTrace st = new StackTrace(new StackFrame(true));
    // StackFrame sf = st.GetFrame(0);
    // Console.WriteLine(" File: {0}", sf.GetFileName());   //文件名
    // Console.WriteLine(" Method: {0}", sf.GetMethod().Name);  //函数名
    // Console.WriteLine(" Line Number: {0}", sf.GetFileLineNumber());  //文件行号
    // Console.WriteLine(" Column Number: {0}", sf.GetFileColumnNumber());
    #endregion

    /// <summary>
    /// common local log file
    /// </summary>
    public class MyLog : TraceListener
    {
        /// <summary>
        /// log level
        /// </summary>
        protected enum TLogLevel
        {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
            LOG,
            INFO,
            DEBUG,
            WARNING,
            ERROR,
            FATAL
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        }

        #region Members
#if  CWMS_LOGLOCAL
        /// <summary>
        /// path of log
        /// </summary>
        static protected string LogPath = @"\\dll.517.jiali\dll\mango-misnet2014\"; // AppDomain.CurrentDomain.BaseDirectory + "\\logs\\";

        /// <summary>
        /// file name of log.
        /// </summary>
        static protected string LogFileName { get { return "mylog.log"; } }

        /// <summary>
        /// get a full path name of log.
        /// </summary>
        static public string LogFile { get { return LogPath + LogFileName; } }
#endif
        /// <summary>
        /// 
        /// </summary>
        static protected MyLog _instance = new MyLog();

        /// <summary>
        /// 
        /// </summary>
        static public MyLog Instance { get { return _instance; } }
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        protected MyLog()
        {
        }

        #region overrided Methods
        /// <summary>
        /// override method. 向侦听器写入指定消息
        /// </summary>
        /// <param name="lstr">context to be writen.</param>
        public override void Write(string lstr)
        {
            lstr += Environment.NewLine;
            WriteLine(lstr);
        }

        /// <summary>
        /// override method. write a line of log with trace info.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category">allways be String.Empty.</param>
        public override void Write(string message, string category)
        {
            message += (Environment.StackTrace + Environment.NewLine); // append stack trace.
            WriteLine(message);
        }

        /// <summary>
        /// override method. write a line of log with exception message.
        /// </summary>
        /// <param name="o">should be an inherranted object from Exception.</param>
        /// <param name="message"></param>
        public override void Write(object o, string message)
        {
            string s = string.Empty;

            if (null != o)
            {
                if (o is Exception)
                {
                    s = string.Format("{0}\r\n\t\tInnerException: {1}\r\n\t\t异常信息:{2}\r\n\t\t调用堆栈:{3}\r\n", message
                        , (null != (o as Exception).InnerException) ? (o as Exception).InnerException.Message : string.Empty
                        , (o as Exception).Message
                        , (o as Exception).StackTrace);
                }
                else
                {
                    s = string.Format("{0}\r\n\t\t未知类型o:{1}", message, o);
                }
            }
            else
            {
                s = message;
            }

            WriteLine(s);
        }

        /// <summary>
        /// override TraceListener. 向侦听器写入消息，后跟行结束符。
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
#if CWMS_LOGLOCAL
            try { File.AppendAllText(LogFile, message); } catch (Exception) { }
#endif
#if CWMS_LOGFRAME
            try { var ret = new MangoMis.Frame.ThirdFrame.ThirdResult<List<object>>(message); ret.End(); } catch (Exception) { }
#endif
#if CWMS_LOGHELPER
            try { LogHelper.WriteCustom("CWMS_log", message); } catch (Exception) { }
#endif
        }
#endregion

#region log methods
        /// <summary>
        /// write a line of log.
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="args">logs to be written.</param>
        public void Log(string fmt, params object[] args)
        {
            try
            {
                string lstr = string.Format(fmt, args);
                lstr = Formatted(TLogLevel.LOG, lstr);
                Write(lstr);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// write a line of info.
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="args">infors to be written.</param>
        public void Info(string fmt, params object[] args)
        {
            try
            {
                string lstr = string.Format(fmt, args);
                lstr = Formatted(TLogLevel.INFO, lstr);
                Write(lstr);
            }
            catch (Exception) { }
        }
        
        /// <summary>
        /// write a line of Debug
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="args"></param>
        public void Debug(string fmt, params object[] args)
        {
            try
            {
                string lstr = string.Format(fmt, args);
                lstr = Formatted(TLogLevel.DEBUG, lstr);
                Write(lstr);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// write a line of debug, and it contains informations of exception.
        /// </summary>
        /// <param name="exp">exception to be logged.</param>
        /// <param name="fmt"></param>
        /// <param name="args">debug infos to be written.</param>
        public void Debug(Exception exp, string fmt, params object[] args)
        {
            try
            {
                string lstr = string.Format(fmt, args);
                lstr = Formatted(TLogLevel.DEBUG, lstr);
                Write(exp, lstr);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// write a line of ERROR
        /// </summary>
        /// <param name="fmt">format of arguments, use it like string.Format(string fmt, params object[] args)</param>
        /// <param name="args">message be written.</param>
        public void Warning(string fmt, params object[] args)
        {
            try
            {
                string lstr = string.Format(fmt, args);
                lstr = Formatted(TLogLevel.WARNING, lstr);
                Write(lstr);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// write a line of ERROR, and it contains informations of exception.
        /// </summary>
        /// <param name="exp">exception to be logged.</param>
        /// <param name="fmt">format of arguments, use it like string.Format(string fmt, params object[] args)</param>
        /// <param name="args">ERROR infos to be written.</param>
        public void Warning(Exception exp, string fmt, params object[] args)
        {
            try
            {
                string lstr = string.Format(fmt, args);
                lstr = Formatted(TLogLevel.WARNING, lstr);
                Write(exp, lstr);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="args"></param>
        public void Error(string fmt, params object[] args)
        {
            try
            {
                string lstr = string.Format(fmt, args);
                lstr = Formatted(TLogLevel.ERROR, lstr);
                Write(lstr);
            }
            catch (Exception) { }
        }
        
        /// <summary>
        /// write a line of ERROR, and it contains informations of exception.
        /// </summary>
        /// <param name="exp">exception to be logged.</param>
        /// <param name="fmt"></param>
        /// <param name="args">ERROR infos to be written.</param>
        public void Error(Exception exp, string fmt, params object[] args)
        {
            try
            {
                string lstr = string.Format(fmt, args);
                lstr = Formatted(TLogLevel.ERROR, lstr);
                Write(exp, lstr);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// write a line of Fatal, and it contains informations of exception.
        /// </summary>
        /// <param name="exp">exception to be logged.</param>
        /// <param name="args">Fatal infos to be written.</param>
        public void Fatal(Exception exp, string fmt, params object[] args)
        {
            try
            {
                string lstr = string.Format(fmt, args);
                lstr = Formatted(TLogLevel.FATAL, lstr);
                Write(exp, lstr);
            }
            catch (Exception) { }
        }
#endregion

#region inner methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="l"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected string Formatted(TLogLevel l, string msg)
        {
            DateTime dt = DateTime.Now;
            string s = string.Format("{0} [PID({3}), ALC({4}), AVL({5})] {1}: {2}", l.ToString(), dt.ToString("yyyy/MM/dd HH:mm:ss.fff"), msg, System.Threading.Thread.CurrentThread.ManagedThreadId, GetRomCommittedMB(), GetRomAvaliableMB());
            return s;
        }
#endregion

        /// <summary>
        /// 获取公共信息，若执行成功则返回内存字节数；否则返回TError.ErrorNotFound
        /// </summary>
        /// <param name="mgntClass">ManagementClass名称</param>
        /// <param name="prop">属性名称</param>
        /// <returns></returns>
        public List<object> GetCIM(string mgntClass, string prop)
        {
            List<Object> list = new List<object>(1);
            try
            {
                using (ManagementClass cimobject = new ManagementClass(mgntClass))
                {
                    using (ManagementObjectCollection moc = cimobject.GetInstances())
                    {
                        foreach (ManagementObject mo1 in moc)
                        {
                            list.Add(mo1.Properties[prop].Value);
                        }
                    } // moc1.Dispose();
                }   // cimobject1.Dispose();

                return list;
            }
            catch (Exception /*ex*/)
            {
                list.Clear();
                return list;
            }
        }

        /// <summary>
        /// 获取总系统内存，若执行成功则返回总系统内存字节数；否则返回TError.ErrorNotFound
        /// </summary>
        /// <returns></returns>
        public double GetRomTotalMB()
        {
            double rslt = 0;
            var list = GetCIM("Win32_PhysicalMemory", "Capacity");
            foreach (var o in list)
            {
                rslt += double.Parse(o.ToString());
            }

            rslt = rslt / 1024 / 1024;
            return rslt;
        }

        /// <summary>
        /// 获取可用系统内存，若执行成功则返回可用系统内存字节数；否则返回TError.ErrorNotFound
        /// </summary>
        /// <returns></returns>
        public double GetRomAvaliableMB()
        {
            double rslt = 0;
            var list = GetCIM("Win32_PerfFormattedData_PerfOS_Memory", "AvailableMBytes");
            foreach (var o in list)
            {
                rslt += double.Parse(o.ToString());
            }
            return rslt;
        }

        /// <summary>
        /// 获取可用系统内存，若执行成功则返回可用系统内存字节数；否则返回TError.ErrorNotFound
        /// </summary>
        /// <returns></returns>
        public long GetRomCommittedMB()
        {
            long rslt = Process.GetCurrentProcess().PrivateMemorySize64;
            rslt = rslt / 1024 / 1024;
            return rslt;
        }
    }
}
