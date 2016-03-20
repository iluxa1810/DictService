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

namespace FormClient.Forms
{
    public partial class AddFieldsForm : Form
    {
        DataTable SerachFieldsDataTable;
        public AddFieldsForm()
        {
            InitializeComponent();
        }
        public AddFieldsForm(DataTable ds)
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            SerachFieldsDataTable = ds;
            dataGridView1.DataSource = SerachFieldsDataTable;
            dataGridView1.Columns[0].DataPropertyName = "id";
            dataGridView1.Columns[1].DataPropertyName = "XRef";
            dataGridView1.Columns[2].DataPropertyName = "DbFieldName";
            dataGridView1.Columns[3].DataPropertyName = "SearchRank";
            dataGridView1.Columns[4].DataPropertyName = "SearchDbFieldName";
            dataGridView1.Columns[5].DataPropertyName = "SearchSPName";
            dataGridView1.Columns[6].DataPropertyName = "Visible";
            dataGridView1.Columns[7].DataPropertyName = "ExactSearch";
            dataGridView1.Columns[8].DataPropertyName = "SearchFromStart";
            dataGridView1.Columns[9].DataPropertyName = "FilterColumn";
            dataGridView1.Columns[10].DataPropertyName = "CanBeEmpty";
        }

        public string GetSearchFields()
        {
            var sw=new StringWriter();
            SerachFieldsDataTable.WriteXml(sw);
            return sw.ToString();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult=DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
