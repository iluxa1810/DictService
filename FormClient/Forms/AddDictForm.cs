using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormClient.DictionaryService;
using FormClient.Forms;
using FormClient.Helpers;

namespace FormClient
{
    public partial class AddDictForm : Form
    {
        public AddDictForm()
        {
            InitializeComponent();
        }
        public AddDictForm(ComboBox comboBox)
        {
            InitializeComponent();
            InitCategoryCB();
        }

        void InitCategoryCB()
        {
            using (var client = new DataClient())
            {
                var category = client.GetCategories();
                cbCategory.DataSource = category;
                cbCategory.DisplayMember = "Name";
                cbCategory.ValueMember = "Category_id";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Access Data Base (.mdb)|*.mdb";
            fileDialog.ShowDialog();
            tbFilePath.Text = fileDialog.FileName;
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            if (CheckFields())
            {
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        bool CheckFields()
        {
            if (tbFilePath.Text == String.Empty ||tbFrendlyName.Text == string.Empty|| 
                cbCategory.Text == String.Empty ||rtbDescription.Text == String.Empty)
            {
                MessageBox.Show("Не все поля заполнены");
                return false;
            }
            return true;
        }
        public DictionaryInfo GetUploadData()
        {
            DictionaryInfo dictInfo = new DictionaryInfo()
            {
                Category_id = ((CategoryData)cbCategory.SelectedItem).Category_id,
                FileName = Path.GetFileName(tbFilePath.Text).Replace(".mdb",".zip"),
                FriendlyName = tbFrendlyName.Text,
                Action = ActionEnum.AddDict,
                SenderLogin = AccountHelper.GetAccount(),
                Description = rtbDescription.Text
            };
            return dictInfo;
        }

        public string GetFilePath()
        {
            return tbFilePath.Text;
        }

        private void addCategoryBtn_Click_1(object sender, EventArgs e)
        {
            using (var categoryAddForm = new AddCategoryForm())
            {
                categoryAddForm.ShowDialog(this);
                if (categoryAddForm.DialogResult == DialogResult.OK)
                {
                    InitCategoryCB();
                }
            }
        }
    }
}
