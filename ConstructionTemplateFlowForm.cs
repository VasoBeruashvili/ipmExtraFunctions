using System;
using System.Data;
using System.Windows.Forms;
using ipmPMBasic;

namespace ipmExtraFunctions
{
    public partial class ConstructionTemplateFlowForm : Form
    {
        ProgramManagerBasic pm;

        private bool wasClickedOnSaveAndNew;
        public bool WasClickedOnSaveAndNew
        {
            get { return wasClickedOnSaveAndNew; }
        }

        public ConstructionTemplateFlowForm(ProgramManagerBasic pm)
        {
            InitializeComponent();

            this.pm = pm;
            wasClickedOnSaveAndNew = false;
            FillComboGroupProduct();
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return pm;
        }

        private void FillComboGroupProduct()
        {
            string sql = "SELECT id, name FROM book.GroupProducts WHERE path IN(" + GetProductPaths() + ")";
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            comboGroupProduct.ValueMember = "id";
            comboGroupProduct.DisplayMember = "name";
            comboGroupProduct.DataSource = data;
        }

        private string GetProductPaths()
        {
            string paths = string.Empty;
            string sql = "SELECT path FROM book.GroupProducts WHERE path LIKE '0#1#10#11#%'";
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            foreach (DataRow row in data.Rows)
            {
                if (Convert.ToString(row["path"]).Split('#').Length == 5)
                    paths = paths + "'" + row["path"] + "'" + ",";
            }

            return paths.TrimEnd(',');
        }

        private bool ValidateData()
        {
            if (comboGroupProduct.SelectedValue == null)
            {
                MessageBox.Show("დასახელება არ არ ის მითითებული", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        public void SetData(int groupProdId)
        {
            btnSaveAndNew.Visible = false;
            comboGroupProduct.SelectedValue = groupProdId;
        }

        public void GetData(ref int groupProdId, ref string groupProdName)
        {
            groupProdId = Convert.ToInt32(comboGroupProduct.SelectedValue);
            groupProdName = comboGroupProduct.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;

            DialogResult = DialogResult.OK;
        }

        private void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            if (!ValidateData())
                return;

            wasClickedOnSaveAndNew = true;

            DialogResult = DialogResult.OK;
        }
    }
}
