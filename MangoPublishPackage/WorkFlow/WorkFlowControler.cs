using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MangoPublishPackage.WorkFlow
{
    public class WorkFlowControler : IDisposable
    {
        Queue<ITaskItem> _tasks = null;
        ManualResetEvent _event = new ManualResetEvent(false);

        public delegate void DlgtDef_OnWorkflowTaskBegin(ITaskItem Task, string Message);
        public delegate void DlgtDef_OnWorkflowTaskFinished(ITaskItem Task, TTaskResult Result);
        DlgtDef_OnWorkflowTaskBegin OnTakkBegin = null;
        DlgtDef_OnWorkflowTaskFinished OnTaksFinished = null;

        public WorkFlowControler(DlgtDef_OnWorkflowTaskBegin HandleBegin, DlgtDef_OnWorkflowTaskFinished HandleFinished)
        {
            _tasks = new Queue<ITaskItem>();
            OnTakkBegin = HandleBegin;
            OnTaksFinished = HandleFinished;
        }

        public void Dispose()
        {
            if (null != _tasks) _tasks.Clear();
            _event.Dispose();
        }

        public void Wait()
        {
            _event.WaitOne();
        }

        public int Start()
        {
            if (0 == _tasks.Count)
            {
                _event.Set();
                return -5;
            }
            else
            {
                Thread workTh = new Thread(ThreadMain_ExecuteTaskItem);
                workTh.Start(_tasks.Peek());
                return 1;
            }
        }

        public void AddTask(ITaskItem Item)
        {
            if (null != Item) _tasks.Enqueue(Item);
        }

        void ThreadMain_ExecuteTaskItem(object arg)
        {
            ITaskItem task = arg as ITaskItem;
            try
            {
                OnTakkBegin?.Invoke(task, "");
                // run current task
                task.Start();

                // notify current task running result
                OnTaksFinished?.Invoke(task, task.Finished());

                _tasks.Dequeue();

                // run next task if exist.
                Start();
            }
            catch (Exception ex)
            {
                OnTaksFinished?.Invoke(task, TTaskResult.EFailed); // TODO: notify exception.
            }
        }
    }
}
