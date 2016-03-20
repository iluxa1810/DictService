using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using FormClient.DictionaryService;
using FormClient.Forms;


namespace FormClient
{

    public partial class EditorMainForm : Form
    {
        XDocument XDoc { get; set; }
        DictionaryOnTaskPackage pack;
        BindingList<ExtensionData> ExtensionDatas;
        public EditorMainForm()
        {
            InitializeComponent();
        }
        public EditorMainForm(string xmlConfg, DictionaryOnTaskPackage pack)
        {
            InitializeComponent();
            cbDictionaryType.DataSource = Enum.GetValues(typeof (DictionaryTypeEnum));
            var reader = new StringReader(xmlConfg);
            XDoc = XDocument.Load(reader);
            this.pack = pack;
            FillDataGrid();
        }
        void FillDataGrid()
        {
            var ext =
               XDoc.XPathSelectElements("*//Extenders/Ext[((contains(@Id,'SimpleDictionary') and contains(@ConnectionString,'.mdb'))" +
                                        " or (contains(@Id,'ImportDictionary') and contains(@ImportDbConnectionString,'.mdb')) and not(@" +
                                        "Event='isDeleted'))]").Select(x => new
                                        {
                                            DictType = x.Attribute("Id").Value,
                                            DictionaryOnTask_id = x.Attribute("DictionaryOnTask_id")?.Value ?? "0",
                                            Ext = x
                                        }).ToList();
            var result = (from t in ext
                          join t2 in pack.DictionaryOnTaskDatas on Convert.ToInt32(t.DictionaryOnTask_id) equals
                              t2.DictionaryOnTask_id
                              into t3
                          from t4 in t3.DefaultIfEmpty(new DictionaryOnTaskData())
                          join t5 in pack.dictionaryDatas on t4.Dictionary_id equals t5.Dictionary_id
                          into t6
                          from t7 in t6.DefaultIfEmpty(new DictionaryData() { FriendlyName = "Неизвестно" })
                          join t8 in pack.Categories on t7.Category_id equals t8.Category_id
                          into t9
                          from t10 in t9.DefaultIfEmpty(new CategoryData() { Name = "Неизвестно" })
                          select new ExtensionData()
                          {
                              DictionaryType = t.DictType,
                              Dictionary_id = t4.Dictionary_id==0?1:default(int?),
                              DictionaryOnTask_id = t4.DictionaryOnTask_id == 0 ? 1 : default(int?),
                              Category_id = t10.Category_id,
                              DictionaryName = t7.FriendlyName,
                              CategoryName = t10.Name,
                              Extension = t.Ext
                          }).ToList();
            ExtensionDatas = new BindingList<ExtensionData>(result);
            dgvDictionary.AutoGenerateColumns = false;
            dgvDictionary.DataSource = ExtensionDatas;
            dgvDictionary.Columns["DictionaryType"].DataPropertyName = "DictionaryType";
            dgvDictionary.Columns["FriendlyName"].DataPropertyName = "DictionaryName";
            dgvDictionary.Columns["CategoryName"].DataPropertyName = "CategoryName";
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            var ext = (ExtensionData)dgvDictionary.SelectedRows[0].DataBoundItem;
            if (ext.DictionaryType.Contains("Import"))
            {
                using (var frm = new ImportDictionaryForm(ext))
                {
                    frm.ShowDialog();
                    RefreshDataSource();
                }
            }
            else if (ext.DictionaryType.Contains("Simple"))
            {
                using (var frm = new SimpleDictionaryForm(ext.Extension))
                {
                    frm.ShowDialog();
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if ((DictionaryTypeEnum)cbDictionaryType.SelectedItem==DictionaryTypeEnum.ImportDictionary)
            using (var frm = new ImportDictionaryForm())
            {
                frm.ShowDialog();
                XDoc.XPathSelectElement("*//Extenders").Add(frm.GetNewExtElement().Extension);
                ExtensionDatas.Add(frm.GetNewExtElement());
                RefreshDataSource();
            }
            if ((DictionaryTypeEnum)cbDictionaryType.SelectedItem == DictionaryTypeEnum.SimpleDictionary)
                using (var frm = new SimpleDictionaryForm())
                {
                    frm.ShowDialog();
                    //XDoc.XPathSelectElement("*//Extenders").Add(frm.GetNewExtElement().Extension);
                    //ExtensionDatas.Add(frm.GetNewExtElement());
                    RefreshDataSource();
                }
        }
        void RefreshDataSource()
        {
            dgvDictionary.DataSource = null;
            dgvDictionary.DataSource = ExtensionDatas;
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
        public XDocument GetXmlConfig()
        {
            return XDoc;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var ext = (ExtensionData)dgvDictionary.SelectedRows[0].DataBoundItem;
            if (ext.Extension.Attribute("Event") == null)
            {
                ext.Extension.Add(new XAttribute("Event", "Remove"));
            }
            else
            {
                ext.Extension.Attribute("Event").Value = "Remove";
            }
            ExtensionDatas.Remove(ext);
            RefreshDataSource();
        }
    }

    public class ExtensionData
    {
        public string DictionaryType { get; set; }
        public int? Dictionary_id { get; set; }
        public int? DictionaryOnTask_id { get; set; }
        public int Category_id { get; set; }
        public string DictionaryName { get; set; }
        public string CategoryName { get; set; }
        public XElement Extension { get; set; }
    }

    enum DictionaryTypeEnum
    {
        ImportDictionary,SimpleDictionary
    }
}
