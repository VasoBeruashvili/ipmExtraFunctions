using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using System.Collections;

namespace ipmExtraFunctions
{
    public partial class ExMSTypeColumnsForm : Form
    {
        ProgramManagerBasic ProgramManager;
        bool isNew;
        int typeId;
        Hashtable sqlParams = new Hashtable();
        bool isFirstLoad = true;
        public ExMSTypeColumnsForm(ProgramManagerBasic PM, bool is_new, int type_id, string type_name, int type)
        {
            InitializeComponent();
            ProgramManager = PM;

            comboTypeCategory.DataSource = PM.GetDataManager().GetTableData("SELECT 0 AS id,N'' AS name UNION ALL SELECT id,name FROM book.GroupProducts where path LIKE '0#1#10#11#%'");
            comboTypeCategory.ValueMember = "id";
            comboTypeCategory.DisplayMember = "name";
            comboTypeCategory.SelectedValue = type;

            List<string> items = ProgramManager.GetDataManager().GetMultiStringVal("SELECT name FROM book.Products WHERE group_id=" + type);
            foreach (string item in items)
                txtTypeName.Items.Add(item);
            txtTypeName.Text = type_name;

            typeId = type_id;
            isNew = is_new;
            setColumns();
            m_Grid.Focus();

            OnFillTypeFields();
            isFirstLoad = false;
        }
        private void setColumns()
        {
            string sql = @"SELECT col.id as col_id, ISNULL(type.type_id, 0) AS type_id, 
                           CASE WHEN (ISNULL(type.col_id, 0)=0) THEN 0 ELSE 1 END as col_enable, col.name as col_name, 
                           ISNULL(type.order_by, 0) AS order_by from book.ExMSColumns as col 
                           LEFT JOIN (select col_id, ISNULL(type_id, 0) AS type_id,  CASE WHEN (ISNULL( col_id, 0)=0) THEN 0 ELSE 1 END as col_enable,
                           ISNULL(order_by, 0) AS order_by FROM book.ExMSGeneralTypeColumns WHERE type_id =@type_id ) as type 
                           ON col.id=type.col_id order by col.id";
            sqlParams.Clear();
            sqlParams.Add(@"type_id", typeId);

            DataTable data = new DataTable();
            data = ProgramManager.GetDataManager().GetTableData(sql, sqlParams);

            foreach (DataRow dr in data.Rows)
            {
                m_Grid.Rows.Add(dr.Field<int>("col_id"), dr.Field<int>("type_id"), dr.Field<int>("col_enable"), dr.Field<string>("col_name"), dr.Field<int>("order_by"));
               
            }
            if (isNew) 
                 m_Grid.Rows.Cast<DataGridViewRow>().ToList().ForEach(p => p.Cells["order_by"].Value = p.Index);
        }
        private bool insertOrUpdateGeneralTypeColumns()
        {
            string sql = "DELETE FROM book.ExMSGeneralTypeColumns WHERE type_id=" + typeId;
            if (!ProgramManager.GetDataManager().ExecuteSql(sql)) return false;

            m_Grid.EndEdit();

            var checkedColumns = m_Grid.Rows.Cast<DataGridViewRow>().Where(p => Convert.ToBoolean(p.Cells["col_enable"].Value)).ToList();

            if (checkedColumns == null) return false;

            foreach (DataGridViewRow dgr in checkedColumns)
            {
                if (typeId == 0) return false;
                int col_id=0;
                if (!int.TryParse(dgr.Cells["col_id"].Value.ToString(), out col_id)) return false;

                int order_by = 0;
                if (!int.TryParse(dgr.Cells["order_by"].Value.ToString(), out order_by)) return false;

                sql="INSERT INTO book.ExMSGeneralTypeColumns (col_id, type_id, order_by) VALUES (@col_id, @type_id, @order_by)";
                sqlParams.Clear();
                sqlParams.Add(@"col_id", col_id);
                sqlParams.Add(@"type_id", typeId);
                sqlParams.Add(@"order_by", order_by);

                if (!ProgramManager.GetDataManager().ExecuteSql(sql, sqlParams)) return false;

            }


            return true;
        }
        private bool insertGeneralType()
        {
            string sql = "INSERT INTO book.ExMSGeneralTypes (name, type_id) VALUES (@name, @type_id)";

            sqlParams.Clear();
            sqlParams.Add(@"name", txtTypeName.Text);
            sqlParams.Add(@"type_id", comboTypeCategory.SelectedValue);
            if (!ProgramManager.GetDataManager().ExecuteSql(sql, sqlParams)) return false;

            typeId = ProgramManager.GetDataManager().GetIntegerValue("SELECT MAX(id) FROM book.ExMSGeneralTypes");

            if (!insertOrUpdateGeneralTypeColumns()) return false;

            return true;
        }
        private bool updateGeneralTypes()
        {
            if (typeId == 0) return false;

            string sql = "UPDATE book.ExMSGeneralTypes SET name=@name, type_id=@type_id WHERE id=@id";

            sqlParams.Clear();
            sqlParams.Add(@"id", typeId);
            sqlParams.Add(@"type_id", comboTypeCategory.SelectedValue);
            sqlParams.Add(@"name", txtTypeName.Text);

            if (!ProgramManager.GetDataManager().ExecuteSql(sql, sqlParams)) return false;

            if (!insertOrUpdateGeneralTypeColumns()) return false;

            return true;
        }
        private bool onSave()
        {           
            if (isNew)
            {
                if (!insertGeneralType()) return false;
            }
            else
            {
                if (!updateGeneralTypes()) return false;
            }

            if (!InsertOrUpdateGeneralTypeFields()) return false;

            return true;
        }

        private bool InsertOrUpdateGeneralTypeFields()
        {
            string res = ProgramManager.GetDataManager().GetStringValue("SELECT TOP(1) field_name FROM book.ExMSGeneralTypeFields ORDER BY id DESC");
            int col_index = res == string.Empty ? 1 : int.Parse(res.Split('_')[1]) + 1;
            string sql;

            if (DeletedId != string.Empty)
            {
                if (!ProgramManager.GetDataManager().ExecuteSql("DELETE FROM book.ExMSGeneralTypeFields WHERE id IN (" + DeletedId + ")"))
                    return false;
            }

            foreach (DataGridViewRow row in mGridFields.Rows)
            {
                sqlParams.Clear();
                sqlParams.Add("@general_type_id", typeId);
                sqlParams.Add("@field_desc", row.Cells[mgridFieldColFieldName.Index].Value);
                sqlParams.Add("@field_name", "col_" + col_index);
                sqlParams.Add("@field_values", row.Cells[mgridFieldColdFieldValues.Index].Value);
                sqlParams.Add("@order_by", row.Index + 1);

                if (row.Cells[mgridFieldsColId.Index].Value.Equals(0))
                {
                    sql = "INSERt INTO book.ExMSGeneralTypeFields (general_type_id,field_desc,field_name, field_values,order_by) VALUES(@general_type_id,@field_desc,@field_name,@field_values,@order_by)";
                    col_index++;
                }
                else
                {
                    sqlParams.Add("@id", row.Cells[mgridFieldsColId.Index].Value);
                    sql = "UPDATE book.ExMSGeneralTypeFields SET field_desc = @field_desc, field_values=@field_values,order_by=@order_by WHERE general_type_id=@general_type_id AND id=@id";
                }
                if (!ProgramManager.GetDataManager().ExecuteSql(sql, sqlParams))
                    return false;
            }

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            m_Grid.EndEdit();
            if (txtTypeName.Text == string.Empty)
            {
                MessageBox.Show("შეიყვანეთ დასახელება!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }
            if (!onSave())
            {
                MessageBox.Show("შენახვა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void OnFillTypeFields()
        {
            DataTable data = ProgramManager.GetDataManager().GetTableData("SELECT id,field_desc,field_values,order_by FROM book.ExMSGeneralTypeFields WHERE general_type_id=" + typeId + " ORDER BY order_by");
            if (data != null)
            {
                int index;
                foreach (DataRow row in data.Rows)
                {
                    index = mGridFields.Rows.Add();
                    mGridFields.Rows[index].Cells[mgridFieldsColId.Index].Value = row["id"];
                    mGridFields.Rows[index].Cells[mgridFieldColFieldName.Index].Value = row["field_desc"];
                    mGridFields.Rows[index].Cells[mgridFieldColdFieldValues.Index].Value = row["field_values"];
                    mGridFields.Rows[index].Cells[mgridFieldsOrderBy.Index].Value = row["order_by"];
                }
            }
        }

        private void btnAddField_Click(object sender, EventArgs e)
        {
            onAddUpdateTypeField(true);
        }

        private void btnEditField_Click(object sender, EventArgs e)
        {
            if (mGridFields.SelectedRows.Count > 0)
            {
                onAddUpdateTypeField(false);
            }
        }

       // private string mDeletedId = ""; 
        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            if (mGridFields.SelectedRows.Count > 0)
            {
                DeletedId = Convert.ToString(mGridFields.SelectedRows[0].Cells[mgridFieldsColId.Index].Value);
                mGridFields.Rows.Remove(mGridFields.SelectedRows[0]);
                mGridFields.Rows.Cast<DataGridViewRow>().ToList().ForEach(p => p.Cells[mgridFieldsOrderBy.Index].Value = p.Index);
            }
        }

        private void onAddUpdateTypeField(bool isNew)
        {
            ExMSTypeFieldForm _form = new ExMSTypeFieldForm() 
            { 
                FieldName = isNew ? "" : mGridFields.SelectedRows[0].Cells[mgridFieldColFieldName.Index].Value.ToString(),
                FieldValues = isNew ? "" : mGridFields.SelectedRows[0].Cells[mgridFieldColdFieldValues.Index].Value.ToString()
            };
            if (_form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (isNew)
                {
                    if (!mGridFields.Rows
                        .Cast<DataGridViewRow>()
                        .Where(r => r.Cells[mgridFieldColFieldName.Index].Value.ToString() == _form.FieldName)
                        .Any())
                    {
                        int index = mGridFields.Rows.Add();
                        mGridFields.Rows[index].Cells[mgridFieldsColId.Index].Value = 0;
                        mGridFields.Rows[index].Cells[mgridFieldColFieldName.Index].Value = _form.FieldName;
                        mGridFields.Rows[index].Cells[mgridFieldColdFieldValues.Index].Value = _form.FieldValues;
                        mGridFields.Rows[index].Cells[mgridFieldsOrderBy.Index].Value = index + 1;
                    }
                }
                else
                {
                    mGridFields.SelectedRows[0].Cells[mgridFieldColFieldName.Index].Value = _form.FieldName;
                    mGridFields.SelectedRows[0].Cells[mgridFieldColdFieldValues.Index].Value = _form.FieldValues;
                }
            }
        }

        private void mGridFields_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            onAddUpdateTypeField(false);
        }

        private void comboTypeCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            if (isFirstLoad)
                return;

            txtTypeName.Text = "";
            txtTypeName.Items.Clear();
            List<string> items = ProgramManager.GetDataManager().GetMultiStringVal("SELECT name FROM book.Products WHERE group_id=" + comboTypeCategory.SelectedValue);
            foreach (string item in items)
                txtTypeName.Items.Add(item);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            onUp();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            onDown();
        }

        private void onUp()
        {
            if (mGridFields.SelectedRows.Count == 0) return;

            var selectedRow = mGridFields.SelectedRows[0];
            if (selectedRow == null) return;

            int selectedIndex = selectedRow.Index;
            if (selectedIndex == 0) return;

            mGridFields.Rows.RemoveAt(selectedIndex);
            mGridFields.Rows.Insert(selectedIndex - 1, selectedRow);

            mGridFields.Rows.Cast<DataGridViewRow>().ToList().ForEach(p => p.Cells[mgridFieldsOrderBy.Index].Value = p.Index);

            mGridFields.ClearSelection();

            selectedRow.Selected = true;

        }
        private void onDown()
        {
            if (mGridFields.SelectedRows.Count == 0) return;

            var selectedRow = mGridFields.SelectedRows[0];
            if (selectedRow == null) return;

            int selectedIndex = selectedRow.Index;
            if (selectedIndex == mGridFields.Rows.Count - 1) return;

            mGridFields.Rows.RemoveAt(selectedIndex);
            mGridFields.Rows.Insert(selectedIndex + 1, selectedRow);

            mGridFields.Rows.Cast<DataGridViewRow>().ToList().ForEach(p => p.Cells[mgridFieldsOrderBy.Index].Value = p.Index);

            mGridFields.ClearSelection();

            selectedRow.Selected = true;
        }

        private string deletedId;
        private string DeletedId
        {
            get
            {
                if (deletedId == null)
                    return string.Empty;
                return deletedId.TrimEnd(',');
            }
            set
            {
                if (value.Equals("-1"))
                    return;
                deletedId += value + ",";
            }
        }

    }
}
