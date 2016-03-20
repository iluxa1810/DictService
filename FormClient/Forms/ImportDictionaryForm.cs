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
using FormClient.DictionaryService;
using FormClient.Enums;

namespace FormClient.Forms
{
    public partial class ImportDictionaryForm : Form
    {
        enum ShowDictEnum
        {
            OnChanges, Manual
        }
        ExtensionData Ext { get; set; }
        SelectedDictionary newDictionary { get; set; }
        public ImportDictionaryForm()
        {
            InitializeComponent();
            Ext=new ExtensionData();
            FillingComponents();
        }

        public ImportDictionaryForm(ExtensionData ext)
        {
            InitializeComponent();
            Ext = ext;
            FillingComponents();
        }

        void FillingComponents()
        {
            SetDataSetToComboBoxes();
            tbShowDictShortcut.Text = Ext.Extension?.Attribute("ShowDictShortcut")?.Value ?? "";
            tbClearValuesShortcut.Text = Ext.Extension?.Attribute("ClearValuesShortcut")?.Value ?? "";
            tbInsertDictValuesShortcut.Text = Ext.Extension?.Attribute("InsertDictValuesShortcut")?.Value ?? "";
            tbImportDbConnectionString.Text = Ext.Extension?.Attribute("ImportDbConnectionString")?.Value ?? "";
            tbImportTableName.Text = Ext.Extension?.Attribute("ImportTableName")?.Value ?? "";
            tbLinkGroupXRef.Text = Ext.Extension?.Attribute("LinkGroupXRef")?.Value ?? "";
            tbResultMaxRowCount.Text = Ext.Extension?.Attribute("ResultMaxRowCount")?.Value ?? "10";
            tbSearchFields.Text = Ext.Extension?.Attribute("SearchFields")?.Value ?? "<DocumentElement/>";
            tbShowFormMaxCount.Text = Ext.Extension?.Attribute("ShowFormMaxCount")?.Value ?? "10";
            tbId.Text = Ext.Extension?.Attribute("Id")?.Value ?? "ImportDictionary";
            tbType.Text = Ext.Extension?.Attribute("Type")?.Value ?? "";
            cbShowDict.Text = Ext.Extension?.Attribute("ShowDict")?.Value ?? ShowDictEnum.OnChanges.ToString();
            cbShowErrors.Text = Ext.Extension?.Attribute("ShowErrors")?.Value ?? BoolEnum.False.ToString();
            cbApplyAndStep2NextField.Text = Ext.Extension?.Attribute("ApplyAndStep2NextField")?.Value ?? BoolEnum.False.ToString();
            tbDictionaryName.Text = Ext.DictionaryName;
            tbCategoryName.Text = Ext.CategoryName;
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
            var reader = new StringReader(tbSearchFields.Text);
            dt.ReadXml(reader);

            using (var frm = new AddFieldsForm(dt))
            {
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.OK)
                {
                    tbSearchFields.Text = frm.GetSearchFields();
                }
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
        void CheckDictAttr()
        {
            if (Ext.Extension.Attribute("ShowDictShortcut") == null)
            {
                Ext.Extension.Add(new XAttribute("ShowDictShortcut", ""));
            }
            if (Ext.Extension.Attribute("ClearValuesShortcut") == null)
            {
                Ext.Extension.Add(new XAttribute("ClearValuesShortcut", ""));
            }
            if (Ext.Extension.Attribute("InsertDictValuesShortcut") == null)
            {
                Ext.Extension.Add(new XAttribute("InsertDictValuesShortcut", ""));
            }
            if (Ext.Extension.Attribute("ImportDbConnectionString") == null)
            {
                Ext.Extension.Add(new XAttribute("ImportDbConnectionString", ""));
            }
            if (Ext.Extension.Attribute("ImportTableName") == null)
            {
                Ext.Extension.Add(new XAttribute("ImportTableName", ""));
            }
            if (Ext.Extension.Attribute("LinkGroupXRef") == null)
            {
                Ext.Extension.Add(new XAttribute("LinkGroupXRef", ""));
            }
            if (Ext.Extension.Attribute("ResultMaxRowCount") == null)
            {
                Ext.Extension.Add(new XAttribute("ResultMaxRowCount", ""));
            }
            if (Ext.Extension.Attribute("SearchFields") == null)
            {
                Ext.Extension.Add(new XAttribute("SearchFields", ""));
            }
            if (Ext.Extension.Attribute("ShowFormMaxCount") == null)
            {
                Ext.Extension.Add(new XAttribute("ShowFormMaxCount", ""));
            }
            if (Ext.Extension.Attribute("Id") == null)
            {
                Ext.Extension.Add(new XAttribute("Id", ""));
            }
            if (Ext.Extension.Attribute("Type") == null)
            {
                Ext.Extension.Add(new XAttribute("Type", ""));
            }
            if (Ext.Extension.Attribute("ShowDict") == null)
            {
                Ext.Extension.Add(new XAttribute("ShowDict", ""));
            }
            if (Ext.Extension.Attribute("ShowErrors") == null)
            {
                Ext.Extension.Add(new XAttribute("ShowErrors", ""));
            }
            if (Ext.Extension.Attribute("ApplyAndStep2NextField") == null)
            {
                Ext.Extension.Add(new XAttribute("ApplyAndStep2NextField", ""));
            }
            if (Ext.Extension.Attribute("DictionaryOnTask_id") == null)
            {
                Ext.Extension.Add(new XAttribute("DictionaryOnTask_id", ""));
            }
            if (Ext.Extension.Attribute("Dictionary_id") == null)
            {
                Ext.Extension.Add(new XAttribute("Dictionary_id", ""));
            }
        }

        public ExtensionData GetNewExtElement()
        {
            return Ext;
        }
        XElement CreateDictNode()
        {
            var ext= new XElement("Ext");
            ext.Add(new XAttribute("ShowDictShortcut", ""));
            ext.Add(new XAttribute("ClearValuesShortcut", ""));
            ext.Add(new XAttribute("InsertDictValuesShortcut", ""));
            ext.Add(new XAttribute("ImportDbConnectionString", ""));
            ext.Add(new XAttribute("ImportTableName", ""));
            ext.Add(new XAttribute("LinkGroupXRef", ""));
            ext.Add(new XAttribute("ResultMaxRowCount", ""));
            ext.Add(new XAttribute("SearchFields", ""));
            ext.Add(new XAttribute("ShowFormMaxCount", ""));
            ext.Add(new XAttribute("Id", ""));
            ext.Add(new XAttribute("Type", ""));
            ext.Add(new XAttribute("ShowDict", ""));
            ext.Add(new XAttribute("ShowErrors", ""));
            ext.Add(new XAttribute("ApplyAndStep2NextField", ""));
            ext.Add(new XAttribute("DictionaryOnTask_id", ""));
            ext.Add(new XAttribute("Dictionary_id", ""));
            return ext;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!CheckFieldsValues())
            {
                return;
            }
            if (newDictionary != null)
            {
                Ext.CategoryName = newDictionary.CategoryName;
                Ext.DictionaryName = newDictionary.DictionaryName;
                Ext.Category_id = newDictionary.Category_id;
                Ext.Dictionary_id = newDictionary.Dictionary_id;
                Ext.DictionaryType = "ImportDictionary";
            }
            if (Ext.Extension != null)
            {
                CheckDictAttr();
            }
            else
            {
                Ext.Extension=CreateDictNode();
            }
            DialogResult = DialogResult.OK;
            AssignValueToXelement();
            this.Close();
        }

        bool CheckFieldsValues()
        {
            if (tbDictionaryName.Text == String.Empty || tbCategoryName.Text == String.Empty)
            {
                MessageBox.Show("Не выбран словарь");
                return false;
            }
            if (tbSearchFields.Text == "<DocumentElement/>")
            {
                MessageBox.Show("Не выбраны поля");
                return false;
            }
            return true;
        }
        private void AssignValueToXelement()
        {
            Ext.Extension.Attribute("ShowDictShortcut").Value = tbShowDictShortcut.Text;
            Ext.Extension.Attribute("ClearValuesShortcut").Value = tbClearValuesShortcut.Text;
            Ext.Extension.Attribute("InsertDictValuesShortcut").Value = tbInsertDictValuesShortcut.Text;
            Ext.Extension.Attribute("ImportDbConnectionString").Value = tbImportDbConnectionString.Text;
            Ext.Extension.Attribute("ImportTableName").Value = tbImportTableName.Text;
            Ext.Extension.Attribute("LinkGroupXRef").Value = tbLinkGroupXRef.Text;
            Ext.Extension.Attribute("ResultMaxRowCount").Value = tbResultMaxRowCount.Text;
            Ext.Extension.Attribute("SearchFields").Value = tbSearchFields.Text;
            Ext.Extension.Attribute("ShowFormMaxCount").Value = tbShowFormMaxCount.Text;
            Ext.Extension.Attribute("Id").Value = tbId.Text;
            Ext.Extension.Attribute("Type").Value = tbType.Text;
            Ext.Extension.Attribute("ShowDict").Value = cbShowDict.Text;
            Ext.Extension.Attribute("ShowErrors").Value = cbShowErrors.Text;
            Ext.Extension.Attribute("ApplyAndStep2NextField").Value = cbApplyAndStep2NextField.Text;
            if (newDictionary != null)
            {
                Ext.Extension.Attribute("Dictionary_id").Value = newDictionary.Dictionary_id.ToString();
            }
            AddEventToXElement(Ext.Extension);
            if (Ext.DictionaryOnTask_id == null)
            {
                Ext.Extension.Attribute("DictionaryOnTask_id").Value = "";
            }
        }

        void AddEventToXElement(XElement xElement)
        {
            if (xElement.Attribute("Event") == null)
            {
                xElement.Add(new XAttribute("Event", "Add"));
            }
            else if (xElement.Attribute("Event").Value != "Add")
            {
                xElement.Attribute("Event").Value = "Change";
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnChangeDictionary_Click(object sender, EventArgs e)
        {
            using (var frm = new ChangeDictionaryForm())
            {
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.OK)
                {
                    newDictionary = frm.GetSelectedDictionary();
                    tbDictionaryName.Text = newDictionary.DictionaryName;
                    tbCategoryName.Text = newDictionary.CategoryName;
                }
            }
        }
    }
}
