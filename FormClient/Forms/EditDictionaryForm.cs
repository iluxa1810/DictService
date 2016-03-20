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
using Common.Helpers;
using FormClient.DictionaryService;
using FormClient.Helpers;

namespace FormClient.Forms
{
    public partial class EditDictionaryForm : Form
    {
        public EditDictionaryForm()
        {
            InitializeComponent();
        }
        public EditDictionaryForm(string dictId, string dictName, string categoryName, string description)
        {
            InitializeComponent();
            tbDictionaryId.Text = dictId;
            DescriptionRtb.Text = description;
            FrendlyNameTb.Text = dictName;
            categoryCb.Text = categoryName;
            using (var client = new DataClient())
            {
                var categoryes = client.GetCategories();
                categoryCb.DataSource = categoryes;
                categoryCb.DisplayMember = "Name";
                categoryCb.ValueMember = "Category_id";
                categoryCb.Text = categoryName;
            }
        }

        private void uploadChkBx_CheckStateChanged(object sender, EventArgs e)
        {
            if (uploadChkBx.Checked)
            {
                buttonFileDialog.Enabled = true;
                return;
            }
            else
            {
                buttonFileDialog.Enabled = false;
            }
        }

        private async void resfreshBtn_Click(object sender, EventArgs e)
        {
            if (uploadChkBx.Checked)
            {
                using (var client = new FileUploadClient())
                {
                    try
                    {
                        string path;
                        path = ZipHelper.CreateZipDictionary(tbFilePath.Text);
                        DictionaryInfo dictInfo = new DictionaryInfo()
                        {
                            Dictionary_id = Convert.ToInt32(tbDictionaryId.Text)
                            ,SenderLogin = AccountHelper.GetAccount(),Action = ActionEnum.EditDict
                        };
                        Stream file = new FileStream(path, FileMode.Open);
                        await client.UploadAsync(dictInfo, file);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
                using (var client = new DataClient())
                {
                    var dictInfo = new DictionaryData()
                    {
                        Dictionary_id = Convert.ToInt32(tbDictionaryId.Text),
                        Category_id = ((CategoryData)categoryCb.SelectedItem).Category_id,
                        FriendlyName = FrendlyNameTb.Text,
                        Description = DescriptionRtb.Text
                    };
                    client.ChangeDictionaryInfo(dictInfo);
                    client.ChangeDictionaryStatus(Convert.ToInt32(tbDictionaryId.Text), DictionaryStateEnum.Available);
                }
        }

        private void buttonFileDialog_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Access Data Base (.mdb)|*.mdb";
            fileDialog.ShowDialog();
            tbFilePath.Text = fileDialog.FileName;
        }
    }
}
