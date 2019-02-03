using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace MangoPublishPackage
{
    public partial class FormTfsManage : Form
    {
        Dictionary<string, Project> _projectDict = null;
        TfsTeamProjectCollection _tfsServer = null;

        public FormTfsManage()
        {
            InitializeComponent();
        }

        private void FormTfsManage_Load(object sender, EventArgs e)
        {
            tbTfsUri.Text = "http://192.168.3.202:8080/tfs/MangoMis2014";
            _projectDict = new Dictionary<string, Project>();
        }

        private void btnConnectTfs_Click(object sender, EventArgs e)
        {
            _projectDict.Clear();
            Uri tfsUri = new Uri(tbTfsUri.Text);
            if (null != _tfsServer) { _tfsServer.Dispose(); }
            _tfsServer = new TfsTeamProjectCollection(tfsUri);
            WorkItemStore workstore = _tfsServer.GetService<WorkItemStore>();
            foreach (Project project in workstore.Projects)
            {
                _projectDict.Add(project.Name, project);
                listBoxProjects.Items.Add(project.Name);
            }
        }

        private void FormTfsManage_Closed(object sender, FormClosedEventArgs e)
        {
            if (null != _tfsServer)
            {
                //_tfsServer.
                _tfsServer.Dispose();
            }
            if (null != _projectDict) _projectDict.Clear();
        }

        private void listBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
#if true
            VersionControlServer version = _tfsServer.GetService<VersionControlServer>();
#else
            listBoxTfsProjInfo.Items.Clear();
            Project item = _projectDict[listBoxProjects.Items[listBoxProjects.SelectedIndex].ToString()];
            //listBoxTfsProjInfo.Items.Add(item.)
            listBoxTfsProjInfo.Items.Add("Id: " + item.Id);
            listBoxTfsProjInfo.Items.Add("HasWorkItemWriteRights: " + item.HasWorkItemWriteRights);
            listBoxTfsProjInfo.Items.Add("HasWorkItemReadRights: " + item.HasWorkItemReadRights);
            listBoxTfsProjInfo.Items.Add("QueryHierarchy: " + item.QueryHierarchy);
            listBoxTfsProjInfo.Items.Add("StoredQueries: " + item.StoredQueries);
            listBoxTfsProjInfo.Items.Add("Guid: " + item.Guid);
            listBoxTfsProjInfo.Items.Add("Uri: " + item.Uri);
            listBoxTfsProjInfo.Items.Add("Name: " + item.Name);
            listBoxTfsProjInfo.Items.Add("HasWorkItemWriteRightsRecursive: " + item.HasWorkItemWriteRightsRecursive);
            listBoxTfsProjInfo.Items.Add("IterationRootNodes: " + item.IterationRootNodes);
            listBoxTfsProjInfo.Items.Add("AreaRootNodeUri: " + item.AreaRootNodeUri);
            listBoxTfsProjInfo.Items.Add("AreaRootNodes: " + item.AreaRootNodes);
            listBoxTfsProjInfo.Items.Add("Store: " + item.Store);
            listBoxTfsProjInfo.Items.Add("Categories: " + item.Categories);
            listBoxTfsProjInfo.Items.Add("WorkItemTypes: " + item.WorkItemTypes);
            listBoxTfsProjInfo.Items.Add("HasWorkItemReadRightsRecursive: " + item.HasWorkItemReadRightsRecursive);
#endif
        }
    }

    //class MyProject : Microsoft.TeamFoundation.WorkItemTracking.Client.Project
    //{
    //    public override string ToString()
    //    {
    //        return base.ToString();
    //    }
    //}
}
