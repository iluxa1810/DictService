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
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using FormEditor.Forms;


namespace FormEditor
{
    public partial class EditorMainForm : Form
    {
        XDocument XDoc { get; set; }
        public EditorMainForm()
        {
            InitializeComponent();
        }
        public EditorMainForm(string xmlConfg)
        {
            InitializeComponent();
            var reader = new StringReader(xmlConfg);
            XDoc=XDocument.Load(reader);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //var path = @"E:\Projects\Дипломная работа\ПО\DictService\DictService\Cfg.xml";
            //var reader = new StreamReader(path, Encoding.GetEncoding(1251));
            //XmlReader xmlReader = XmlReader.Create(reader);
            //XDoc = XDocument.Load(xmlReader);
            var ext =
                XDoc.XPathSelectElements("*//Extenders/Ext[(contains(@Id,'SimpleDictionary') and contains(@ConnectionString,'.mdb'))" +
                                         " or (contains(@Id,'ImportDictionary') and contains(@ImportDbConnectionString,'.mdb'))]").ToList();
            comboBox1.DataSource = ext;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using (var frm = new SimpleDictionaryForm((XElement)comboBox1.SelectedItem))
            {
                frm.ShowDialog();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using (var frm = new ImportDictionaryForm((XElement)comboBox1.SelectedItem))
            {


                frm.ShowDialog();

            }
        }
    }
}
