using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormClient.DictionaryService;
using FormClient.Helpers;

namespace FormClient.Forms
{
    public partial class ChangeDictionaryForm : Form
    {
        public ChangeDictionaryForm()
        {
            InitializeComponent();
            PrepareForm();
        }
        void FillingDictGrid(DictionaryData[] dictArray, CategoryData[] categoryArray, DataGridView dataGridView)
        {
            var result = (from d in dictArray
                          join c in categoryArray on d.Category_id equals c.Category_id
                          select new { d.Dictionary_id, d.Description, d.FriendlyName, CategoryName = c.Name, d.State, c.Category_id }).ToList();

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
        void PrepareForm()
        {
            using (var client = new DataClient())
            {
                var pack = client.GetDictionaryData(AccountHelper.GetAccount());
                FillingDictGrid(pack.dictionaryDatas, pack.Categories, dictGridView);
                FillingCategoryComboBox(categoryBox, pack.Categories);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public SelectedDictionary GetSelectedDictionary()
        {
            var row = dictGridView.SelectedRows[0].DataBoundItem;
            Type t = dictGridView.SelectedRows[0].DataBoundItem.GetType();
            SelectedDictionary selectedDictionary = new SelectedDictionary()
            {
                Category_id = (int) t.GetProperty("Category_id").GetValue(row),
                CategoryName = (string) t.GetProperty("CategoryName").GetValue(row),
                DictionaryName = (string) t.GetProperty("FriendlyName").GetValue(row),
                Dictionary_id = (int) t.GetProperty("Dictionary_id").GetValue(row)
            };
            return selectedDictionary;
        }

        private void categoryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cm = (CurrencyManager)BindingContext[dictGridView.DataSource];
            cm.SuspendBinding();
            if (((CategoryData)categoryBox.SelectedItem).Name == "Все")
            {
                dictGridView.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => row.Visible = true);
                return;
            }
            foreach (DataGridViewRow row in dictGridView.Rows)
            {
                if (row.Cells[dictGridView.Columns["CategoryName"].Index].Value != ((CategoryData)categoryBox.SelectedItem).Name)
                    row.Visible = false;
                else
                    row.Visible = true;
            }
            cm.ResumeBinding();
        }
    }
    public class SelectedDictionary
    {
        public int Dictionary_id { get; set; }
        public int Category_id { get; set; }
        public string DictionaryName { get; set; }
        public string CategoryName { get; set; }
    }
}
