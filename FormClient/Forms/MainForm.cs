using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormClient.DictionaryService;
using Common.Helpers;
using FormClient.Forms;
using FormClient.Helpers;
using FormClient;
using Common.Helpers;
namespace FormClient
{
    public partial class MainForm : Form
    {
        public class TaskEventArgs : EventArgs
        {
            public string Name { get; set; }
            public string Message { get; set; }
            public string Status { get; set; }
            public int Taskid { get; set; }
        }

        public delegate int TaskDelegate(object sender, TaskEventArgs e);
        public delegate void RefreshStatusDelegate(object sender, TaskEventArgs e);
        public event TaskDelegate StartTask;
        public event RefreshStatusDelegate RefreshStatus;
        public MainForm()
        {
            Thread.Sleep(2000);
            InitializeComponent();
            PrepareForm();
            StartTask += AddNewTask;
            RefreshStatus += RefreshTaskStatus;
        }

        private async void btnAddDict_Click(object sender, EventArgs e)
        {

            DictionaryInfo dictInfo;
            string path;
            using (var form = new AddDictForm(cbCategory))
            {
                form.ShowDialog(this);
                if (form.DialogResult != DialogResult.OK)
                {
                    return;
                }
                dictInfo = form.GetUploadData();
                var mdbPath = form.GetFilePath();
                try
                {
                    AccessHelper.CheckExistPrimaryKey(AccessHelper.CreateMdbConnectionString(mdbPath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                path = ZipHelper.CreateZipDictionary(mdbPath, FileHelper.GetTemporaryDirectory());
            }
            var id = StartTask(this,
                new TaskEventArgs() { Name = $"Добавление {dictInfo.FriendlyName}", Status = "Started" });
            try
            {
                using (var client = new FileUploadClient())
                {
                    using (Stream file = new FileStream(path, FileMode.Open))
                    {
                        await client.UploadAsync(dictInfo, file);
                    }
                }
                RefreshTaskStatus(this, new TaskEventArgs() { Status = "Complete", Taskid = id });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                FileHelper.DeleteFolder(Path.GetDirectoryName(path));
            }
        }

        void PrepareForm()
        {
            using (var client = new DataClient())
            {
                var pack = client.GetDictionaryData(AccountHelper.GetAccount());
                // DisableFunctions(pack.ActionEnums);
                FillingDictGrid(pack.dictionaryDatas, pack.Categories, dgvDict);
                FillingCategoryComboBox(cbCategory, pack.Categories);
            }
        }
        void DisableFunctions(ActionEnum[] actions)
        {
            if (!actions.Contains(ActionEnum.AddDict))
            {
                btnAddDict.Enabled = false;
            }
            if (!actions.Contains(ActionEnum.EditDict))
            {
                btnEditDict.Enabled = false;
            }

        }
        void FillingDictGrid(DictionaryData[] dictArray, CategoryData[] categoryArray, DataGridView dataGridView)
        {
            var result = (from d in dictArray
                          join c in categoryArray on d.Category_id equals c.Category_id
                          select new { d.Dictionary_id, d.Description, d.FriendlyName, CategoryName = c.Name, d.State }).ToList();
            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = result;
            dataGridView.Columns["Dictionary_id"].DataPropertyName = "Dictionary_id";
            dataGridView.Columns["FriendlyName"].DataPropertyName = "FriendlyName";
            dataGridView.Columns["CategoryName"].DataPropertyName = "CategoryName";
            dataGridView.Columns["Description"].DataPropertyName = "Description";
            dataGridView.Columns["State"].DataPropertyName = "State";

        }
        void FillingCategoryComboBox(ComboBox comboBox, CategoryData[] categoryArray)
        {
            List<CategoryData> categories = categoryArray.ToList();
            categories.Insert(0, new CategoryData { Name = "Все" });
            comboBox.DataSource = categories;
            comboBox.DisplayMember = "Name";
            comboBox.ValueMember = "Category_id";
            comboBox.Text = "Все";
        }

        private async void bDownload_Click(object sender, EventArgs e)
        {
            using (var client = new FileDownloadClient())
            {
                var dictId = (int)dgvDict.SelectedCells[0].Value;
                var fileName = await client.DownloadAsync("", dictId);
                var id = StartTask(this, new TaskEventArgs() { Name = $"Скачивание {fileName.FileName}", Status = "Started" });
                string dstDir;
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.ShowDialog();
                    dstDir = folderDialog.SelectedPath;
                    if (dstDir == string.Empty)
                    {
                        return;
                    }
                }
                string tempDir=FileHelper.GetTemporaryDirectory();
                string dstFile= Path.Combine(tempDir, fileName.FileName);
                await FileHelper.LoadFileFromStreamAsync(fileName.stream, dstFile);
                ZipHelper.UnZip(dstFile, dstDir);
                FileHelper.DeleteFolder(tempDir);
                RefreshTaskStatus(this, new TaskEventArgs() { Status = "Complete", Taskid = id });
            }
        }

        private void categoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cm = (CurrencyManager)BindingContext[dgvDict.DataSource];
            cm.SuspendBinding();
            if (cbCategory.Text == "Все")
            {
                dgvDict.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => row.Visible = true);
                return;
            }
            foreach (DataGridViewRow row in dgvDict.Rows)
            {
                if (row.Cells[dgvDict.Columns["CategoryName"].Index].Value != cbCategory.Text)
                    row.Visible = false;
                else
                    row.Visible = true;
            }
            cm.ResumeBinding();
        }

        private void editDictButton_Click(object sender, EventArgs e)
        {
            if (dgvDict.SelectedCells[dgvDict.Columns["State"].Index].Value.ToString() != DictionaryStateEnum.Available.ToString())
            {
                MessageBox.Show("Сейчас словарь нельзя редактировать");
                return;
            }
            var dictId = dgvDict.SelectedCells[dgvDict.Columns["Dictionary_id"].Index].Value.ToString();
            var dictName = dgvDict.SelectedCells[dgvDict.Columns["FriendlyName"].Index].Value.ToString();
            var categoryName = dgvDict.SelectedCells[dgvDict.Columns["CategoryName"].Index].Value.ToString();
            var description = dgvDict.SelectedCells[dgvDict.Columns["Description"].Index].Value.ToString();
            using (var form = new EditDictionaryForm(dictId, dictName, categoryName, description))
            {
                using (var client = new DataClient())
                {
                    client.ChangeDictionaryStatus(Convert.ToInt32(dictId), DictionaryStateEnum.Locked);
                }
                form.Owner = this;
                form.ShowDialog();
            }
        }

        private int AddNewTask(object sender, TaskEventArgs e)
        {
            int id = dgvTask.Rows.Add();
            dgvTask.Rows[id].Cells["Id"].Value = id;
            dgvTask.Rows[id].Cells["TaskName"].Value = e.Name;
            dgvTask.Rows[id].Cells["Status"].Value = e.Status;
            return id;
        }
        private void RefreshTaskStatus(object sender, TaskEventArgs e)
        {
            dgvTask.Rows[e.Taskid].Cells["Status"].Value = e.Status;
        }

        private void RefreshMainForm()
        {
            dgvDict.DataSource = null;
            dgvDict.Rows.Clear();
            PrepareForm();
        }

        private void refreshBtnData_Click(object sender, EventArgs e)
        {
            RefreshMainForm();
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "Links" && dgvProjects.RowCount == 0)
            {
                RefreshLinkTab();
            }

        }

        private void RefreshLinkTab()
        {
            using (var client = new DataClient())
            {
                var pack = client.GetProjectsData();
                var projectList = (from f1 in pack.ProjectDatas
                                   join f2 in pack.OctopusServerssDatas on f1.Server_id equals f2.Server_id
                                   select new { f1.Project_id, f1.Server_id, ProjectName = f1.Name, f2.ServerName }).ToList();
                dgvProjects.AutoGenerateColumns = false;
                dgvProjects.DataSource = projectList;
                dgvProjects.Columns["Project_id"].DataPropertyName = "Project_id";
                dgvProjects.Columns["ProjectName"].DataPropertyName = "ProjectName";
                dgvProjects.Columns["ServerName"].DataPropertyName = "ServerName";
                dgvProjects.Columns["ProjectServerId"].DataPropertyName = "Server_id";
                var taskList = pack.TaskDatas.ToList();
                dgvTasks.AutoGenerateColumns = false;
                dgvTasks.DataSource = taskList;
                dgvTasks.Columns["LinkTaskId"].DataPropertyName = "Task_id";
                dgvTasks.Columns["LinkTaskProjectId"].DataPropertyName = "Project_id";
                dgvTasks.Columns["LinkTaskServerId"].DataPropertyName = "Server_id";
                dgvTasks.Columns["LinkTaskName"].DataPropertyName = "Name";
                dgvTasks.Columns["LinkTaskState"].DataPropertyName = "State";
                HideDataGridRows();
            }

        }
        private void HideDataGridRows()
        {
            var cm = (CurrencyManager)BindingContext[dgvTasks.DataSource];
            cm.SuspendBinding();
            dgvTasks.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => row.Visible = false);
            cm.ResumeBinding();
        }

        private void dgvProjects_SelectionChanged(object sender, EventArgs e)
        {
            dgvTasks.ClearSelection();
            if (dgvTasks.DataSource != null)
            {
                ((CurrencyManager)BindingContext[dgvTasks.DataSource]).SuspendBinding();

            }
            foreach (DataGridViewRow row in dgvTasks.Rows)
            {
                if (
                    (row.Cells["LinkTaskProjectId"].Value.ToString() == dgvProjects.SelectedCells[dgvProjects.Columns["Project_id"].Index].Value.ToString()) &&
                    (row.Cells["LinkTaskServerId"].Value.ToString() == dgvProjects.SelectedCells[dgvProjects.Columns["ProjectServerId"].Index].Value.ToString()))
                {
                    row.Visible = true;
                }
                else
                    row.Visible = false;
            }
            if (dgvTasks.DataSource != null)
            {
                ((CurrencyManager)BindingContext[dgvTasks.DataSource]).ResumeBinding();
            }
        }

        private void btnLinkDict_Click(object sender, EventArgs e)
        {
            if (dgvTasks.SelectedCells.Count == 0)
            {
                return;
            }
            int task_id = (int)dgvTasks.SelectedCells[dgvTasks.Columns["LinkTaskId"].Index].Value;
            int project_id = (int)dgvTasks.SelectedCells[dgvTasks.Columns["LinkTaskProjectId"].Index].Value;
            int server_id = (int)dgvTasks.SelectedCells[dgvTasks.Columns["LinkTaskServerId"].Index].Value;
            using (var client = new DataClient())
            {
                var xmlConfig = client.GetXmlFormConfig(server_id, project_id, task_id);
                var pack = client.GetDictionariesOnTask(server_id, project_id, task_id);
                using (var frm = new EditorMainForm(xmlConfig, pack))
                {
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        try
                        {
                            client.UpdateXmlFormConfig(server_id, project_id, task_id, frm.GetXmlConfig().ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(),"Не удалось обновить конфигурацию",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
