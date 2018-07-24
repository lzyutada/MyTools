using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HelloTools
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Test.MyWaitHandle.TestVan_WaitHandle van = new Test.MyWaitHandle.TestVan_WaitHandle();
                van.Load();
                van.Run();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("[PID:{2}] !!EXCEPTION: {0}\r\nStackTrace: {1}", ex.Message, ex.StackTrace, Thread.CurrentThread.ManagedThreadId));
                if (null != ex.InnerException) System.Diagnostics.Trace.WriteLine(string.Format("[PID:{1}] !!INNER EXCEPTION: {0}", ex.InnerException.Message, Thread.CurrentThread.ManagedThreadId));
            }
        }
    }
}
