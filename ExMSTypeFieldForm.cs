using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ipmExtraFunctions
{
    public partial class ExMSTypeFieldForm : Form
    {
        public string FieldName { get; set; }
        public string FieldValues { get; set; }
        public ExMSTypeFieldForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            m_GridValues.CurrentCell = null;
            m_GridValues.EndEdit();
            this.FieldName = txtFieldName.Text;
            if (comboFieldType.SelectedIndex == 1)
                this.FieldValues = String.Join("@", m_GridValues.Rows.Cast<DataGridViewRow>().Select(c => c.Cells["name"].Value.ToString()).ToArray());
            else if(comboFieldType.SelectedIndex == 0)
                this.FieldValues = "";
            else
                this.FieldValues = "date";
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void ExMSTypeFieldForm_Load(object sender, EventArgs e)
        {
            txtFieldName.Text = this.FieldName;
            string[] values = this.FieldValues.Split('@');
            if (values.Count() > 0 && values[0] != "")
            {
                if (values[0] != "date")
                {
                    comboFieldType.SelectedIndex = 1;
                    foreach (string val in values)
                    {
                        m_GridValues.Rows.Add(val);
                    }
                }
                else
                {
                    comboFieldType.SelectedIndex = 2;
                }
            }
            else
            {
                comboFieldType.SelectedIndex = 0;
            }
            
        }

        private void comboFieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel_field_1.Visible = comboFieldType.SelectedIndex == 1;
            if (comboFieldType.SelectedIndex != 1)
                m_GridValues.Rows.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            m_GridValues.Rows.Add(1);
            var newRow = m_GridValues.Rows.Cast<DataGridViewRow>().Last();
            if (newRow != null)
            {
                m_GridValues.Columns["name"].ReadOnly = false;
                m_GridValues.CurrentCell = newRow.Cells["name"];
                m_GridValues.BeginEdit(false);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (m_GridValues.SelectedRows.Count > 0)
            {
                var SelectedRow = m_GridValues.SelectedRows.Cast<DataGridViewRow>().ToList().OrderBy(p => p.Index).FirstOrDefault();
                changeRowReadOnlyStatus(SelectedRow, false);
            }
        }

        private void changeRowReadOnlyStatus(DataGridViewRow dgr, bool status)
        {
            m_GridValues.Columns["name"].ReadOnly = status;
            m_GridValues.CurrentCell = dgr.Cells["name"];
            if (!status) m_GridValues.BeginEdit(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (m_GridValues.SelectedRows.Count == 0) return;

            if (MessageBox.Show("გინათ ჩანაწერის წაშლა?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m_GridValues.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(p => m_GridValues.Rows.Remove(p));
            }
        }

    }
}
