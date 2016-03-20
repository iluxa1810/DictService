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
using System.Xml.Linq;
using FormClient.Enums;

namespace FormClient.Forms
{
    public partial class ImportDictionaryForm : Form
    {
        enum ShowDictEnum
        {
            OnChanges, Manual
        }
        public ImportDictionaryForm()
        {
            InitializeComponent();
        }

        public ImportDictionaryForm(XElement dict)
        {
            InitializeComponent();
            SetDataSetToComboBoxes();
            tbShowDictShortcut.Text = dict.Attribute("ShowDictShortcut")?.Value ?? "";
            tbClearValuesShortcut.Text = dict.Attribute("ClearValuesShortcut")?.Value ?? "";
            tbInsertDictValuesShortcut.Text = dict.Attribute("InsertDictValuesShortcut")?.Value ?? "";
            tbImportDbConnectionString.Text = dict.Attribute("ImportDbConnectionString")?.Value ?? "";
            tbImportTableName.Text = dict.Attribute("ImportTableName")?.Value ?? "";
            tbLinkGroupXRef.Text = dict.Attribute("LinkGroupXRef")?.Value ?? "";
            tbResultMaxRowCount.Text = dict.Attribute("ResultMaxRowCount")?.Value ?? "10";
            tbSearchFields.Text = dict.Attribute("SearchFields")?.Value ?? "<DocumentElement/>";
            tbShowFormMaxCount.Text = dict.Attribute("ShowFormMaxCount")?.Value ?? "10";
            tbId.Text = dict.Attribute("Id")?.Value ?? "";
            tbType.Text = dict.Attribute("Type")?.Value ?? "";
            cbShowDict.Text = dict.Attribute("ShowDict")?.Value ?? ShowDictEnum.OnChanges.ToString();
            cbShowErrors.Text = dict.Attribute("ShowErrors")?.Value ?? BoolEnum.False.ToString();
            cbApplyAndStep2NextField.Text = dict.Attribute("ApplyAndStep2NextField")?.Value ?? BoolEnum.False.ToString();
        }
        void SetDataSetToComboBoxes()
        {
            cbShowDict.DataSource = Enum.GetValues(typeof(ShowDictEnum));
            cbApplyAndStep2NextField.DataSource = Enum.GetValues(typeof(BoolEnum));
            cbShowErrors.DataSource = Enum.GetValues(typeof(BoolEnum));
        }

        private void btnAddSearchFields_Click(object sender, EventArgs e)
        {
            var dt = CreateSerachFieldsDataTable();
            DataSet fieldsDs = new DataSet();
            var reader = new StringReader(tbSearchFields.Text);
            dt.ReadXml(reader);

            using (var frm = new AddFieldsForm(dt))
            {
                frm.ShowDialog();
            }
        }
        static DataTable CreateSerachFieldsDataTable()
        {
            var dt = new DataTable("Fields");
            DataColumn column;
            column = dt.Columns.Add("id", typeof(Int32));
            column.AutoIncrement = true;
            column = dt.Columns.Add("XRef", typeof(string));
            column.DefaultValue = "";
            column = dt.Columns.Add("DbFieldName", typeof(string));
            column.DefaultValue = "";
            column = dt.Columns.Add("SearchRank", typeof(Int32));
            column.DefaultValue = 0;
            column = dt.Columns.Add("SearchDbFieldName", typeof(string));
            column.DefaultValue = "";
            column = dt.Columns.Add("SearchSPName", typeof(string));
            column.DefaultValue = "";
            column = dt.Columns.Add("Visible", typeof(bool));
            column.DefaultValue = true;
            column = dt.Columns.Add("ExactSearch", typeof(bool));
            column.DefaultValue = false;
            column = dt.Columns.Add("SearchFromStart", typeof(bool));
            column.DefaultValue = true;
            column = dt.Columns.Add("FilterColumn", typeof(bool));
            column.DefaultValue = false;
            column = dt.Columns.Add("CanBeEmpty", typeof(bool));
            column.DefaultValue = false;
            return dt;
        }
    }
}
