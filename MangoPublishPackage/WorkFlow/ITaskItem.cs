using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoPublishPackage.WorkFlow
{
    public interface ITaskItem
    {
        int Start();
        TTaskResult Finished();
    }
}
