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

namespace FormClient.Forms
{
    public partial class AddCategoryForm : Form
    {
        public AddCategoryForm()
        {
            InitializeComponent();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)  
        {
            if (CheckFields())  
            {
                try
                {
                    using (var client = new DataClient()) 
                    { 
                        client.AddCategory(tbName.Text, rtbDiscription.Text); 
                    }
                    DialogResult = DialogResult.OK; 
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        bool CheckFields()
        {
            if (tbName.Text == String.Empty || rtbDiscription.Text == string.Empty)
            {
                MessageBox.Show("Не все поля заполнены");
                return false;
            }
            return true;
        }
    }
}
