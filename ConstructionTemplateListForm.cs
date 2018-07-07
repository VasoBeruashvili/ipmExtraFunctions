using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ipmPMBasic;

namespace ipmExtraFunctions
{
    public partial class ConstructionTemplateListForm : Form
    {
        ProgramManagerBasic pm;

        public int TemplateID
        {
            get
            {
                if (m_GridTemplates.SelectedRows.Count == 0)
                    return -1;
                return (int)m_GridTemplates.SelectedRows[0].Cells["ColTemplateID"].Value; 
            }
        }

        public ConstructionTemplateListForm(ProgramManagerBasic pm)
        {
            InitializeComponent();

            this.pm = pm;
            m_GridTemplates.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
            FillGrid();
        }

        private void FillGrid()
        {
            string sql = "SELECT * FROM book.ConstructionTemplates";

            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            if (data == null)
                return;

            foreach (DataRow rw in data.Rows)
                m_GridTemplates.Rows.Add(rw["id"], rw["name"]);
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return pm;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnChooseTemplate_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #region Template

        private int SaveTemplate(string templateName, DataGridViewRowCollection templatesFlow)
        {
            int id = -1;
            string sql;

            sql = "INSERT INTO book.ConstructionTemplates (name) VALUES (N'" + templateName + "') SELECT  SCOPE_IDENTITY()";
            if ((id = GetProgramManager().GetDataManager().GetIntegerValue(sql)) <= 0)
                return -1;

            foreach (DataGridViewRow row in templatesFlow)
            {
                sql = @"INSERT INTO book.ConstructionTemplatesFlow (template_id, product_group_id) " +
                    "VALUES (" + id + ", " + row.Cells["ColProducGrouptID"].Value + ")";

                if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
                    return -1;
            }

            return id;
        }

        private bool UpdateTemplate(int templateId, string templateName, string deletedIds, DataGridViewRowCollection templatesFlow)
        {
            string sql;

            sql = "UPDATE book.ConstructionTemplates set name = N'" + templateName + "'";
            if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
                return false;

            if (!string.IsNullOrEmpty(deletedIds))
            {
                sql = "DELETE FROM book.ConstructionTemplatesFlow WHERE id IN (" + deletedIds + ")";
                if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
                    return false;
            }

            foreach (DataGridViewRow row in templatesFlow)
            {
                if (Convert.ToInt32(row.Cells["ColID"].Value) == -1)
                {
                    sql = @"INSERT INTO book.ConstructionTemplatesFlow (template_id, product_group_id) " +
                        "VALUES (" + templateId + ", " + row.Cells["ColProducGrouptID"].Value + ")";

                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
                        return false;
                }
                else
                {
                    sql = "UPDATE book.ConstructionTemplatesFlow SET" +
                          " product_group_id = " + row.Cells["ColProducGrouptID"].Value +
                          " WHERE id = " + row.Cells["ColID"].Value;

                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
                        return false;
                }
            }

            return true;
        }

        private bool DeleteTemplate(int templateId)
        {
            string sql;

            sql = "DELETE FROM book.ConstructionTemplatesFlow WHERE template_id = " + templateId;
            if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
                return false;

            sql = "DELETE FROM book.ConstructionTemplates WHERE id = " + templateId;
            if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
                return false;

            return true;
        }

        private void OnAddTemplate()
        {
            ConstructionTemplateForm form = new ConstructionTemplateForm(GetProgramManager());

            if (form.ShowDialog() != DialogResult.OK)
                return;

            int id = SaveTemplate(form.TemplateName, form.TemplatesFlow);

            if (id > 0)
                m_GridTemplates.Rows.Add(id, form.TemplateName);

            if (form.WasClickedOnSaveAndNew)
                OnAddTemplate();
        }

        private void OnEditTemplate()
        {
            if (m_GridTemplates.SelectedRows.Count == 0)
                return;

            int index = m_GridTemplates.SelectedRows[0].Index;
            int templateId = (int)m_GridTemplates.Rows[index].Cells["ColTemplateID"].Value;
            string templateName = (string)m_GridTemplates.Rows[index].Cells["ColTemplateName"].Value;

            ConstructionTemplateForm form = new ConstructionTemplateForm(GetProgramManager(), templateId, templateName);

            if (form.ShowDialog() != DialogResult.OK)
                return;

            if (UpdateTemplate(templateId, templateName, form.DeletedTemplatesFlowIds, form.TemplatesFlow))
                m_GridTemplates.Rows[index].Cells["ColTemplateName"].Value = form.TemplateName;
        }

        private void OnDeleteTemplate()
        {
            if (m_GridTemplates.SelectedRows.Count == 0)
                return;

            int index = m_GridTemplates.SelectedRows[0].Index;
            int templateId = (int)m_GridTemplates.Rows[index].Cells["ColTemplateID"].Value;

            if (MessageBox.Show("წაიშალოს აღნიშნული ჩანაწერი?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                m_GridTemplates.Rows.RemoveAt(index);
                DeleteTemplate(templateId);
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
