using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ipmPMBasic;

namespace ipmExtraFunctions
{
    public partial class ConstructionTemplateForm : Form
    {
        ProgramManagerBasic pm;

        private bool wasClickedOnSaveAndNew;
        public bool WasClickedOnSaveAndNew
        {
            get { return wasClickedOnSaveAndNew; }
        }

        private string deletedTemplatesFlowIds;
        public string DeletedTemplatesFlowIds
        {
            get
            {
                if (deletedTemplatesFlowIds == null)
                    return string.Empty;

                return deletedTemplatesFlowIds.TrimEnd(',');
            }
            set
            {
                if (value.Equals("-1"))
                    return;

                deletedTemplatesFlowIds += value + ",";
            }
        }

        public string TemplateName
        {
            get { return txtName.Text; }
        }

        public DataGridViewRowCollection TemplatesFlow
        {
            get { return m_GridGroupProducts.Rows; }
        }

        public ConstructionTemplateForm(ProgramManagerBasic pm)
        {
            InitializeComponent();

            this.pm = pm;
            wasClickedOnSaveAndNew = false;
            m_GridGroupProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
        }

        public ConstructionTemplateForm(ProgramManagerBasic pm, int templateId, string templateName)
        {
            InitializeComponent();

            this.pm = pm;
            btnSaveAndNew.Visible = false;
            wasClickedOnSaveAndNew = false;
            txtName.Text = templateName;
            m_GridGroupProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
            FillGrid(templateId);
        }

        private void FillGrid(int templateId)
        {
            string sql = "SELECT gp.name, " +
                        "ctf.id, " +
                        "ctf.product_group_id " +
                        "FROM book.ConstructionTemplates ct " +
                        "INNER JOIN book.ConstructionTemplatesFlow ctf ON ctf.template_id = ct.id " +
                        "INNER JOIN book.GroupProducts gp ON gp.id = ctf.product_group_id " +
                        "WHERE ct.id = " + templateId;

            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            if (data == null)
                return;

            foreach (DataRow rw in data.Rows)
            {
                m_GridGroupProducts.Rows.Add(
                    rw["id"],
                    rw["product_group_id"],
                    rw["name"]
                    );
            }
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return pm;
        }

        private bool ValidateData()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("დასახელება არ არ ის მითითებული", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
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

        #region Template

        private void OnAddTemplate()
        {
            int groupProdId = 0;
            string groupProdName = string.Empty;

            ConstructionTemplateFlowForm form = new ConstructionTemplateFlowForm(GetProgramManager());

            if (form.ShowDialog() != DialogResult.OK)
                return;

            form.GetData(ref groupProdId, ref groupProdName);

            m_GridGroupProducts.Rows.Add(-1, groupProdId, groupProdName);

            if (form.WasClickedOnSaveAndNew)
                OnAddTemplate();
        }

        private void OnEditTemplate()
        {
            if (m_GridGroupProducts.SelectedRows.Count == 0)
                return;

            int index = m_GridGroupProducts.SelectedRows[0].Index;

            ConstructionTemplateFlowForm form = new ConstructionTemplateFlowForm(GetProgramManager());

            form.SetData(Convert.ToInt32(m_GridGroupProducts.Rows[index].Cells["ColProducGrouptID"].Value));

            if (form.ShowDialog() != DialogResult.OK)
                return;

            int groupProdId = 0;
            string groupProdName = string.Empty;

            form.GetData(ref groupProdId, ref groupProdName);

            m_GridGroupProducts.Rows[index].Cells["ColProducGrouptID"].Value = groupProdId;
            m_GridGroupProducts.Rows[index].Cells["ColProductGroupName"].Value = groupProdName;
        }

        private void OnDeleteTemplate()
        {
            if (m_GridGroupProducts.SelectedRows.Count == 0)
                return;

            int index = m_GridGroupProducts.SelectedRows[0].Index;

            if (MessageBox.Show("წაიშალოს აღნიშნული ჩანაწერი?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                DeletedTemplatesFlowIds = Convert.ToString(m_GridGroupProducts.Rows[index].Cells["ColID"].Value);
                m_GridGroupProducts.Rows.RemoveAt(index);
            }
        }

        private void btnAddTemplate_Click(object sender, EventArgs e)
        {
            OnAddTemplate();
        }

        private void btnEditTemplate_Click(object sender, EventArgs e)
        {
            OnEditTemplate();
        }

        private void m_GridGroupProducts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnEditTemplate();
        }

        private void btnDeleteTemplate_Click(object sender, EventArgs e)
        {
            OnDeleteTemplate();
        }

        #endregion
    }
}
