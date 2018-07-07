using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using System.Collections;

namespace ipmExtraFunctions
{
    public partial class ExMSColumnsForm : Form
    {
        ProgramManagerBasic ProgramManager;
        bool isNew;
        int columnId;
        string dbName;
        public ExMSColumnsForm(ProgramManagerBasic pm, bool is_new, int column_id, string column_name, string db_name, bool is_interval)
        {
            InitializeComponent();
            this.ProgramManager = pm;
            isNew = is_new;
            columnId = column_id;
            if (is_new)
            {
                SetNew();
            }
            else
            {
                SetExisting(column_id, column_name, db_name);
            }
            fillTypeCombo(is_interval);
        }
        private void fillTypeCombo(bool is_interval)
        {
            comboType.Items.AddRange(new string[] { "მნიშვნელობა", "ინტერვალი" });
            comboType.SelectedIndex = is_interval ? 1 : 0;
        }
        private void onTypeChange(int value)
        {
            m_GridValues.Visible = value == 0 ? true : false;
           
        }
        private void SetNew()
        {
            txtColumnName.Text = string.Empty;
            m_GridValues.Rows.Clear();
        }
        private void SetExisting(int column_id, string column_name, string db_name)
        {
            txtColumnName.Text = column_name;
            dbName = db_name;
            DataTable DataValueTypes = new DataTable();
            DataValueTypes = ProgramManager.GetDataManager().GetTableData("SELECT * FROM book.ExMSColumnValues WHERE col_id=" + column_id + "order by order_by");
            foreach (DataRow dr in DataValueTypes.Rows)
            {
                m_GridValues.Rows.Add(dr.Field<int>("id"), dr.Field<int>("col_id"), dr.Field<string>("name"), dr.Field<int>("order_by"));
            }

        }
        private void changeRowReadOnlyStatus(DataGridViewRow dgr, bool status)
        {
            m_GridValues.Columns["name"].ReadOnly = status;
            m_GridValues.CurrentCell = dgr.Cells["name"];
            if (!status) m_GridValues.BeginEdit(false);
        }
        private bool insertOrUpdateColumnValues(int column_id)
        {
            string sql = "DELETE FROM book.ExMSColumnValues WHERE col_id=" + column_id;
            if (!ProgramManager.GetDataManager().ExecuteSql(sql)) return false;
            string valueName;
            int orderBy = 0;
            if (comboType.SelectedIndex == 0)
            {
                Hashtable parameters = new Hashtable();

                foreach (DataGridViewRow dgr in m_GridValues.Rows)
                {
                    valueName = dgr.Cells["name"].Value==null?"":dgr.Cells["name"].Value.ToString();
                    int.TryParse(dgr.Cells["order_by"].Value.ToString(), out orderBy);

                    parameters.Clear();
                    sql = "INSERT INTO book.ExMSColumnValues (col_id, name, order_by) VALUES (@col_id, @name, @order_by)";
                    parameters.Add(@"col_id", column_id);
                    parameters.Add(@"name", valueName);
                    parameters.Add(@"order_by", orderBy);

                    if (!ProgramManager.GetDataManager().ExecuteSql(sql, parameters)) return false;
                }
            }

            return true;
        }
        
        private bool executeInsertColumns()
        {
            string dbName = "ex_col_" + columnId.ToString();
            string name = txtColumnName.Text;
            
            bool is_interval = comboType.SelectedIndex == 1 ? true : false;

            string sql = "INSERT INTO book.ExMSColumns (db_name, name, is_interval) VALUES (@db_name, @name, @is_interval)";
            Hashtable parameters = new Hashtable();
            parameters.Add(@"db_name", dbName);
            parameters.Add(@"name", name);
            parameters.Add(@"is_interval", is_interval);

            if (!ProgramManager.GetDataManager().ExecuteSql(sql, parameters)) return false;

            columnId = ProgramManager.GetDataManager().GetIntegerValue("SELECT MAX(id) FROM  book.ExMSColumns");
            
            string sqlParam =string.Format("{0} NVARCHAR(MAX) ", dbName);
            if (is_interval)
            {
                sqlParam=string.Format("{0}_from FLOAT , {0}_to FLOAT", dbName);
            }

            sql = @"ALTER TABLE book.ExMSTemplates ADD " + sqlParam;

            if (!ProgramManager.GetDataManager().ExecuteSql(sql)) return false;


            if (!insertOrUpdateColumnValues(columnId)) return false;

            return true;

        }
        private bool executeUpdateColumns()
        {
            bool is_interval = comboType.SelectedIndex == 1 ? true : false;

            string sql = "UPDATE book.ExMSColumns SET name=@name, is_interval=@is_interval WHERE id=@id";
            Hashtable parameters = new Hashtable();
            parameters.Add("@name", txtColumnName.Text);
            parameters.Add("@is_interval", is_interval);
            parameters.Add("@id", columnId);

            if (!ProgramManager.GetDataManager().ExecuteSql(sql, parameters)) return false;

            string dropParam = is_interval ? string.Format("(Name = N'{0}')", dbName) : string.Format("((Name = N'{0}_from') OR (Name = N'{0}_to'))", dbName);
            string dropParam2 = is_interval ?  dbName : string.Format("{0}_from , {0}_to", dbName);

            sql= @"IF EXISTS(SELECT * from sys.columns 
            WHERE "+dropParam+@" AND Object_ID = Object_ID(N'book.ExMSTemplates'))
            BEGIN
                    ALTER TABLE book.ExMSTemplates DROP COLUMN "+dropParam2+@"
            END";

            if (!ProgramManager.GetDataManager().ExecuteSql(sql)) return false;

            string addParam = is_interval ? string.Format("(Name = N'{0}_from')", dbName) : string.Format("(Name = N'{0}')", dbName);
            string addParam2=  is_interval? string.Format("{0}_from FLOAT , {0}_to FLOAT", dbName):  string.Format("{0} NVARCHAR(MAX) ", dbName);



            sql = @"IF NOT EXISTS(SELECT * from sys.columns 
            WHERE " + addParam + @" AND Object_ID = Object_ID(N'book.ExMSTemplates'))
            BEGIN
                    ALTER TABLE book.ExMSTemplates ADD " + addParam2 + @"
            END";

            if (!ProgramManager.GetDataManager().ExecuteSql(sql)) return false;

            if (!insertOrUpdateColumnValues(columnId)) return false;

            return true;
        }
        private void onUp()
        {
            if (m_GridValues.SelectedRows.Count == 0) return;

            var selectedRow = m_GridValues.SelectedRows[0];
            if (selectedRow == null) return;

            int selectedIndex = selectedRow.Index;
            if (selectedIndex == 0) return;
           
            m_GridValues.Rows.RemoveAt(selectedIndex);
            m_GridValues.Rows.Insert(selectedIndex-1, selectedRow);

            m_GridValues.Rows.Cast<DataGridViewRow>().ToList().ForEach(p => p.Cells["order_by"].Value = p.Index);

            m_GridValues.ClearSelection();

            selectedRow.Selected = true;

        }
        private void onDown()
        {
            if (m_GridValues.SelectedRows.Count == 0) return;

            var selectedRow = m_GridValues.SelectedRows[0];
            if (selectedRow == null) return;

            int selectedIndex = selectedRow.Index;
            if (selectedIndex == m_GridValues.Rows.Count-1) return;

            m_GridValues.Rows.RemoveAt(selectedIndex);
            m_GridValues.Rows.Insert(selectedIndex + 1, selectedRow);

            m_GridValues.Rows.Cast<DataGridViewRow>().ToList().ForEach(p => p.Cells["order_by"].Value = p.Index);

            m_GridValues.ClearSelection();

            selectedRow.Selected = true; 
        }
        private bool onSave()
        {           
            if (isNew)
            {
                if (!executeInsertColumns()) return false;
            }
            else
            {
                if (!executeUpdateColumns()) return false;
            }

            return true;

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int order_by = 0;
            m_GridValues.Rows.Add(1);

           // m_GridValues.Rows.Add("", "", "", "");
            var newRow = m_GridValues.Rows.Cast<DataGridViewRow>().Last();
            if (newRow != null)
            {
                order_by = newRow.Index;
                newRow.Cells["order_by"].Value = order_by;
                m_GridValues.Columns["name"].ReadOnly=false;
                m_GridValues.CurrentCell = newRow.Cells["name"];
                m_GridValues.BeginEdit(false);
            }
        }

        private void m_Grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var SelectedRow = m_GridValues.Rows[e.RowIndex];
            changeRowReadOnlyStatus(SelectedRow, true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var SelectedRow = m_GridValues.SelectedRows.Cast<DataGridViewRow>().ToList().OrderBy(p=>p.Index).FirstOrDefault();
            changeRowReadOnlyStatus(SelectedRow, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (m_GridValues.SelectedRows.Count == 0) return;

            if (MessageBox.Show("გინათ ჩანაწერის წაშლა?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m_GridValues.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(p => m_GridValues.Rows.Remove(p));
                m_GridValues.Rows.Cast<DataGridViewRow>().ToList().ForEach(p => p.Cells["order_by"].Value = p.Index);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            m_GridValues.EndEdit();
            if (txtColumnName.Text == string.Empty)
            {
                MessageBox.Show("შეიყვანეთ სვეტის დასახელება!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!onSave())
            {
                MessageBox.Show("შენახვა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);                
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            onUp();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            onDown();
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            onTypeChange(comboType.SelectedIndex);
        }

       
    }
}
