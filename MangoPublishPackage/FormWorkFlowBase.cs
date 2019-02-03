using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MangoPublishPackage.WorkFlow;

namespace MangoPublishPackage
{
    public partial class FormWorkFlowBase : Form
    {
        protected WorkFlowControler _controler;
        protected delegate void DlgtDef_HandleWorkTaskBegin(object sender, ITaskItem task);
        protected delegate void DlgtDef_HandleWorkTaskFinished(object sender, ITaskItem task, TTaskResult Result);
        protected DlgtDef_HandleWorkTaskBegin _hTaskBegin = null;
        protected DlgtDef_HandleWorkTaskFinished _hTaskFinished = null;

        public FormWorkFlowBase()
        {
            InitializeComponent();
        }

        virtual protected void StartWorkFlow()
        {
            Task.Factory.StartNew(() =>
            {
                _controler.Start();
            });
        }

        virtual public void OnWorkflowTaskBegin(ITaskItem Task, string Result)
        {
            if (null != _hTaskBegin)
                BeginInvoke(_hTaskBegin, null, Task);
        }

        virtual public void OnWorkflowTaskFinished(ITaskItem Task, TTaskResult Result)
        {
            if (null != _hTaskFinished)
                BeginInvoke(_hTaskFinished, null, Task, Result);
        }

        virtual protected void SetHandleWorkTaskBegin(DlgtDef_HandleWorkTaskBegin Handle) { _hTaskBegin = Handle; }
        virtual protected void SetHandleWorkTaskFinished(DlgtDef_HandleWorkTaskFinished Handle) { _hTaskFinished = Handle; }

        protected void AddTask(ITaskItem Item)
        {
            if (null != Item) _controler.AddTask(Item);
        }

        private void FormWorkflowBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            _controler.Dispose();
        }

        private void FormWorkFlowBase_Load(object sender, EventArgs e)
        {
            _controler = new WorkFlowControler(OnWorkflowTaskBegin, OnWorkflowTaskFinished);
        }
    }
}
