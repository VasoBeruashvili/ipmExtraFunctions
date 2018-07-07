using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using System.Collections;
using ipmControls;

namespace ipmExtraFunctions
{

    public partial class ExtraConstructionForm : Form
    {
        ProgramManagerBasic ProgramManager;
        Hashtable sqlParams = new Hashtable();
        bool typesAreFilled = false;
        Dictionary<string, string> columnValues = new Dictionary<string, string>();
        DataTable dataTempProducts = new DataTable();
        DataTable dataTemptales = new DataTable();
        private bool hasShowPrice = false;
        public ExtraConstructionForm(ProgramManagerBasic pm)
        {
            InitializeComponent();
            this.ProgramManager = pm;
            fillColumns();
            fillTypes();
            fillTypeCombo();
            fillTempProducts();
            try
            {
                OnFillprices();
                OnFillSoilethods();
            }
            catch
            {

            }
            gridTemplate.PManager = this.ProgramManager;

            hasShowPrice = pm.GetDataManager().GetIntegerValue(@"select count(*) from book.users AS u 
                                                             Inner join book.groupusers gu on gu.id=u.group_id
                                                             where gu.name=N'ხელმძღვანელი' and u.id=" + pm.GetUserID()) > 0;
            if (!hasShowPrice)
                tabControl1.TabPages.Remove(tabMultitestPrices);
            
        }
        private ProgramManagerBasic GetProgramManager()
        {
            return this.ProgramManager;
        }
        private void fillTempProducts()
        {
            string sql = @"SELECT temp_pr.id, temp_pr.template_id, temp_pr.product_id, CASE WHEN ISNUMERIC(prod.code)=1 THEN CONVERT(NUMERIC, prod.code) ELSE null END AS code, 
						   prod.name,temp_pr.edge_value, temp_pr.quantity,temp_pr.price FROM book.ExMSTemplateProducts AS temp_pr INNER JOIN book.Products AS prod 
                           ON temp_pr.product_id=prod.id ORDER BY temp_pr.id";

            dataTempProducts = GetProgramManager().GetDataManager().GetTableData(sql);
            
        }
        private void fillColumns()
        {
            string sql = "SELECT * FROM book.ExMSColumns order by id";
            DataTable data = new DataTable();
            data = GetProgramManager().GetDataManager().GetTableData(sql);
            m_GridColumns.Rows.Clear();
            foreach (DataRow dr in data.Rows)
            {
                m_GridColumns.Rows.Add(dr.Field<int>("id"), dr.Field<string>("db_name"), dr.Field<string>("name"), dr.Field<bool>("is_interval"));
            }
        }
        private void fillTypes()
        {
            string sql = "SELECT id, name, type_id FROM book.ExMSGeneralTypes order by id";
            DataTable data = new DataTable();
            data = GetProgramManager().GetDataManager().GetTableData(sql);
            m_GridTypes.Rows.Clear();
            foreach (DataRow dr in data.Rows)
            {
               m_GridTypes.Rows.Add(dr["id"], dr["name"], dr["type_id"]);
            }
        }
        private void fillTypeCombo()
        {
            string sql = "SELECT id, name FROM book.ExMSGeneralTypes UNION ALL SELECT 0 AS id, '' AS name order by id";
            DataTable data = new DataTable();
            data = GetProgramManager().GetDataManager().GetTableData(sql);
            comboTypes.DataSource = data;
            comboTypes.DisplayMember = "name";
            comboTypes.ValueMember = "id";

            typesAreFilled = true;
        }
        private void fillTemplates()
        {
            Dictionary<string, string> columns = new Dictionary<string, string>();
            if (comboTypes.SelectedValue == null) return;

            int typeId = 0;

            if (!int.TryParse(comboTypes.SelectedValue.ToString(), out typeId)) return;

            if (typeId == 0)
            {
                gridTemplate.DataSource = null;
                gridProducts.DataSource = null;
                return;
            }

            string sql = "SELECT col.db_name, col.name, col.is_interval FROM book.ExMSGeneralTypeColumns AS tc INNER JOIN book.ExMSColumns AS col ON tc.col_id=col.id WHERE tc.type_id=" + typeId + "order by tc.col_id";

            dataTemptales = GetProgramManager().GetDataManager().GetTableData(sql);
            columnValues.Clear();
            foreach (DataRow dr in dataTemptales.Rows)
            {

                if (dr.Field<bool>("is_interval"))
                {
                    columns.Add(dr.Field<string>("db_name") + "_from", dr.Field<string>("name") + "-დან");
                    columns.Add(dr.Field<string>("db_name") + "_to", dr.Field<string>("name") + "-მდე");
                }
                else
                {
                    columns.Add(dr.Field<string>("db_name"), dr.Field<string>("name"));
                }
 
            }

            if (columns.Count == 0) return;

            string columnParameter = string.Join(", ", columns.Keys.ToArray());

            sql = @"SELECT id, " + columnParameter + " FROM book.ExMSTemplates WHERE type_id=" + typeId+@" order by id";
            dataTemptales = new DataTable();
            dataTemptales = GetProgramManager().GetDataManager().GetTableData(sql);
            gridTemplate.DataSource = null;
            gridTemplate.DataSource = dataTemptales;
            

            gridTemplate.Columns.Cast<DataGridViewColumn>().ToList().
                ForEach(p =>
                {
                    if (p.HeaderText == "id")
                    {
                        p.Visible = false;
                    }
                    else
                    {
                        p.HeaderText = columns[p.Name];
                    }
                    p.ReadOnly = true;
                });

        }
        private void OnCustomFilter()
        {
            DataRow[] findRows;
            if (gridTemplate.SortString == string.Empty)
            {
                findRows = dataTemptales.Select(gridTemplate.FilterString);
            }
            else
                findRows = dataTemptales.Select(gridTemplate.FilterString, gridTemplate.SortString);
            if (findRows.Count() > 0)
            {
                gridTemplate.DataSource = findRows.CopyToDataTable();
            }
            else
            {
                DataTable dt = dataTemptales.Clone();
                dt.Rows.Clear();
                gridTemplate.DataSource = dt;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control|Keys.Shift|Keys.Delete:
                    {
                        if (MessageBox.Show("გინდათ სახეობების გასუფთავება?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (!clearAllTypes())
                            {
                                MessageBox.Show("სახეობების გასუფთავება ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                            MessageBox.Show("სახეობები გასუფთავებულია!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    }
                case Keys.Control | Keys.U:
                    {
                        if (MessageBox.Show("გინდათ საქონლის ვალუტის შეცვლა დოლარით?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (!changeProductCurrenciesToUSD())
                            {
                                MessageBox.Show("საქონლის ვალუტის შეცვლა დოლარით ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                            MessageBox.Show("საქონლის ვალუტა შეცვლილია დოლარით!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                    }
               
            }



            return base.ProcessCmdKey(ref msg, keyData);

        }
        private bool clearAllTypes()
        {
            foreach (DataGridViewRow dgr in m_GridTypes.Rows)
            {
                int typeId = 0;
                if (!int.TryParse(dgr.Cells["type_id"].Value.ToString(), out typeId)) return false;

                string sql = @"DELETE FROM book.ExMSGeneralTypeColumns WHERE type_id=@type_id 
                                    DELETE FROM book.ExMSGeneralTypes WHERE id=@type_id
                                    DELETE FROM book.ExMSTemplates WHERE type_id=@type_id";
                sqlParams.Clear();
                sqlParams.Add(@"type_id", typeId);

                if (!GetProgramManager().GetDataManager().ExecuteSql(sql, sqlParams))
                {
                    return false;
                }
                
            }

            m_GridTypes.Rows.Clear();
            fillTypeCombo(); 

            return true;
        }
        private bool changeProductCurrenciesToUSD()
        {
            if (!GetProgramManager().GetDataManager().ExecuteSql("UPDATE book.ProductPrices SET manual_currency_id=2"))
                return false;

            return true;
        }
        private void fillColumnValues(int templateId,  Hashtable columns)
        {
            int typeId = 0;

            if (!int.TryParse(comboTypes.SelectedValue.ToString(), out typeId)) return;

            if (typeId == 0) return;


            using (ExMSTemplatesForm Form = new ExMSTemplatesForm(GetProgramManager(), templateId, typeId, columns))
            {
                Form.Text = templateId == 0 ? "შაბლონი: ახალი" : "შაბლონი: რედაქტირება";
                if (Form.ShowDialog() == DialogResult.OK)
                    fillTemplates();
            }
            
        }
        private void combo_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            columnValues[combo.Tag.ToString()] = combo.SelectedValue.ToString();
        }
        private void onAddColumn()
        {
            int columnId = ProgramManager.GetDataManager().GetIntegerValue("select isnull(max(id), 0)+1 from book.ExMSColumns");
            using (ExMSColumnsForm Form = new ExMSColumnsForm(GetProgramManager(), true, columnId, "","", false))
            {
                Form.Text = "სვეტები: ახალი";
                if (Form.ShowDialog() == DialogResult.OK)
                {
                    fillColumns();
                }
            }

        }
        private void onEditColumn()
        {
            if (m_GridColumns.SelectedRows.Count==0) return;

            var selectedRow = m_GridColumns.SelectedRows.Cast<DataGridViewRow>().ToList().OrderBy(p => p.Index).FirstOrDefault();
            if (selectedRow==null) return;

            int columnId = 0;
            if (!int.TryParse(selectedRow.Cells["id"].Value.ToString(), out columnId)) return;

            string name = selectedRow.Cells["name"].Value.ToString();
            string dbName = selectedRow.Cells["db_name"].Value.ToString();
            bool is_interval = Convert.ToBoolean(selectedRow.Cells["is_interval"].Value);

            using (ExMSColumnsForm Form = new ExMSColumnsForm(GetProgramManager(), false, columnId, name, dbName,is_interval))
            {
                Form.Text = "სვეტები: რედაქტირება";
                if (Form.ShowDialog() == DialogResult.OK)
                {
                    fillColumns();
                }
            }

        }
        private void onDeleteColumn()
        {
            if (m_GridColumns.SelectedRows.Count == 0) return;

            if (MessageBox.Show("გინათ სვეტის წაშლა?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var selectedRows = m_GridColumns.SelectedRows;
                bool deleteStatus = true;
                foreach (DataGridViewRow dgr in selectedRows)
                {
                    int columnId = 0;
                    if (!int.TryParse(dgr.Cells["id"].Value.ToString(), out columnId)) { deleteStatus = false; continue; }

                    string dbName = dgr.Cells["db_name"].Value.ToString();

                    int value = GetProgramManager().GetDataManager().GetIntegerValue("SELECT COUNT(id) FROM book.ExMSGeneralTypeColumns WHERE col_id=" + columnId);
                    if (value > 0)
                    {
                        MessageBox.Show("სვეტი გამოიყენება სახეობებში!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string sql = @"DELETE FROM book.ExMSColumnValues WHERE col_id=@col_id 
                             DELETE FROM book.ExMSColumns WHERE id=@col_id";
                    sqlParams.Clear();
                    sqlParams.Add(@"col_id", columnId);

                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql, sqlParams))
                    {
                        deleteStatus = false; 
                        continue;
                    }
                    bool is_interval = Convert.ToBoolean(dgr.Cells["is_interval"].Value);

                    string sqlParam=dbName;

                    if (is_interval)
                        sqlParam = string.Format("{0}_from, {0}_to", dbName);

                    string sqlDrop = @"ALTER TABLE book.ExMSTemplates DROP COLUMN " + sqlParam;

                    if (!ProgramManager.GetDataManager().ExecuteSql(sqlDrop)) return;
                    m_GridColumns.Rows.Remove(dgr);
                }
                if (!deleteStatus)
                {
                    MessageBox.Show("სვეტის წაშლა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("სვეტი წაშლილია!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void onAddType()
        {
            using (ExMSTypeColumnsForm Form = new ExMSTypeColumnsForm(GetProgramManager(), true, 0, "", 0))
            {
                Form.Text = "სახეობა: ახალი";
                if (Form.ShowDialog() == DialogResult.OK)
                {
                    fillTypes();
                    fillTypeCombo();                    
                }
            }

        }
        private void onEditType()
        {
            if (m_GridTypes.SelectedRows.Count == 0) return;

            var selectedRow = m_GridTypes.SelectedRows.Cast<DataGridViewRow>().ToList().OrderBy(p => p.Index).FirstOrDefault();
            if (selectedRow == null) return;

            int typeId = 0;
            if (!int.TryParse(selectedRow.Cells["type_id"].Value.ToString(), out typeId)) return;

            string name = selectedRow.Cells["type_name"].Value.ToString();

            using (ExMSTypeColumnsForm Form = new ExMSTypeColumnsForm(GetProgramManager(), false, typeId, name, (int)selectedRow.Cells["type_type"].Value))
            {
                Form.Text = "სახეობა: რედაქტირება";
                if (Form.ShowDialog() == DialogResult.OK)
                {
                    fillTypes();
                    fillTypeCombo(); 
                }
            }

        }
        private void onDeleteType()
        {
             if (m_GridTypes.SelectedRows.Count == 0) return;

             if (MessageBox.Show("გინათ სახეობის წაშლა?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
             {

                 var selectedRows = m_GridTypes.SelectedRows;
                 foreach (DataGridViewRow dgr in selectedRows)
                 {
                     int typeId = 0;
                     if (!int.TryParse(dgr.Cells["type_id"].Value.ToString(), out typeId))  continue; 

                     string sql = @"DELETE FROM book.ExMSGeneralTypeColumns WHERE type_id=@type_id 
                                    DELETE FROM book.ExMSGeneralTypes WHERE id=@type_id
                                    DELETE FROM book.ExMSTemplates WHERE type_id=@type_id";
                     sqlParams.Clear();
                     sqlParams.Add(@"type_id", typeId);

                     if (!GetProgramManager().GetDataManager().ExecuteSql(sql, sqlParams))
                     {
                         MessageBox.Show("ჩანაწერის წაშლა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                         return;
                     }

                     m_GridTypes.Rows.Remove(dgr);
                 }

                 MessageBox.Show("სახეობა წაშლილია!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                 fillTypeCombo(); 
             }

        }
        private void onAddTemplates()
        {
            if ((comboTypes.SelectedValue==null)||(Convert.ToInt32(comboTypes.SelectedValue)==0)) return;

            columnValues.Add("type_id", comboTypes.SelectedValue.ToString());

            string insertParatemets = string.Join(", ", columnValues.Keys.ToArray());
            string insertValues = string.Join(", ", columnValues.Values.Select(p=>p= string.Format("'{0}'", p)) .ToArray());

            string sql = @"INSERT INTO book.ExMSTemplates (" + insertParatemets + ") VALUES (" + insertValues + ")";
            if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
            {
                MessageBox.Show("შაბლონის შენახვა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            fillTemplates();
        }
        private void onEditTemplates(string template_id)
        {            
            string[] parameters=columnValues.Select(p =>string.Format("{0}='{1}'", p.Key, p.Value)).ToArray();

            string updateParametes = string.Join(", ", parameters);
            string sql = "UPDATE book.ExMSTemplates SET " + updateParametes + " WHERE id=" + template_id;
            if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
            {
                MessageBox.Show("შაბლონის გადახლება ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            fillTemplates();

        }
        private void onEditTemplate()
        {
            if (gridTemplate.SelectedRows.Count == 0) return;              

            var selectedRow = gridTemplate.SelectedRows.Cast<DataGridViewRow>().ToList().OrderBy(p=>p.Index).FirstOrDefault();
            if (selectedRow==null) return;

            int templateId = 0;
            if (!int.TryParse(selectedRow.Cells["id"].Value.ToString(), out templateId)) return;

            Hashtable columnValues = new Hashtable();

            foreach (DataGridViewCell cell in selectedRow.Cells)
            {
                columnValues.Add( cell.OwningColumn.Name, cell.Value);
            }

            fillColumnValues(templateId, columnValues);
        }
        
        private void onDeleteTemplate()
        {
            if (gridTemplate.SelectedRows.Count == 0) return;

            if (MessageBox.Show("გინდათ შაბლონის წაშლა?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool deleteStatus = true;
                var selectedRows = gridTemplate.SelectedRows;
                foreach (DataGridViewRow dgr in selectedRows)
                {
                    int templateId = 0;
                    if (!int.TryParse(dgr.Cells["id"].Value.ToString(), out templateId)) { deleteStatus = false; continue; }

                    int value = GetProgramManager().GetDataManager().GetIntegerValue("SELECT COUNT (id) FROM book.ExMSTemplateProducts WHERE template_id=" + templateId);
                    if (value > 0)
                    {
                        deleteStatus = false; continue; 
                    }

                    string sql = "DELETE FROM book.ExMSTemplates WHERE id=" + templateId;
                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
                    {
                        MessageBox.Show("შაბლონის წაშლა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (!deleteStatus)
                    MessageBox.Show("შაბლონი შეიცავს საქონელს!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                fillTemplates();
            }
        }
        private void onTempProductSelect()
        {
            if (gridTemplate.SelectedRows.Count == 0) return;
            var selectedRow = gridTemplate.SelectedRows[0];
            int tempId = 0;
            int.TryParse(selectedRow.Cells["id"].Value.ToString(), out tempId);
            if (tempId == 0) return;

            gridProducts.DataSource = null;
            gridProducts.DataSource = dataTempProducts;
            foreach (DataGridViewColumn col in gridProducts.Columns)
            {
                switch (col.Name)
                {
                    case "id": col.Name = "rec_id"; col.Visible = false; break;
                    case "template_id": col.Visible = false; break;
                    case "product_id": col.Visible = false; break;
                    case "code": col.Name = "product_code"; col.ReadOnly = true; col.HeaderText = "კოდი"; col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = 150; break;
                    case "name": col.Name = "product_name"; col.ReadOnly = true; col.HeaderText = "დასახელება"; col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; break;
                    case "quantity": col.HeaderText = "რაოდენობა"; col.ReadOnly = true; col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = 150; break;
                    case "edge_value": col.HeaderText = "ზღვრული მნიშვნელობა"; col.ReadOnly = true; col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; col.Width = 150; break;
                    case "price": col.HeaderText = "თანხა"; col.Visible = hasShowPrice; break;
                }
            }
            (gridProducts.DataSource as DataTable).DefaultView.RowFilter = string.Format("template_id={0}", tempId); 
        }
        
        private void onAddTempProduct()
        {
            if (gridTemplate.SelectedRows.Count == 0) return;
            int templateId=0;
            if (!int.TryParse(gridTemplate.SelectedRows[0].Cells["id"].Value.ToString(), out templateId)) return;

            using (ExMSTempProductForm Form = new ExMSTempProductForm(GetProgramManager(), templateId, 0))
            {
                Form.Text = "საქონელი: ახალი";
                if (Form.ShowDialog()== DialogResult.OK)
                {
                    fillTempProducts();
                    onTempProductSelect();
                }
            }
        }
        private void onEditTempProduct()
        {
            if ((gridTemplate.SelectedRows.Count == 0)||(gridProducts.SelectedRows.Count == 0)) return;

            var selectedRow = gridProducts.SelectedRows.Cast<DataGridViewRow>().ToList().OrderBy(p => p.Index).FirstOrDefault();
            if (selectedRow == null) return;

            int templateId = 0;
            if (!int.TryParse(gridTemplate.SelectedRows[0].Cells["id"].Value.ToString(), out templateId)) return;

            int recordId = 0;
            if (!int.TryParse(selectedRow.Cells["rec_id"].Value.ToString(), out recordId)) return;

            int productId = 0;
            if (!int.TryParse(selectedRow.Cells["product_id"].Value.ToString(), out productId)) return;

            string productName = selectedRow.Cells["product_name"].Value.ToString();

            double quantity = 0;
            if (!double.TryParse(selectedRow.Cells["quantity"].Value.ToString(), out quantity)) return;

            using (ExMSTempProductForm Form = new ExMSTempProductForm(GetProgramManager(), templateId, recordId))
            {
                Form.Text = "საქონელი: რედაქტირება";
                Form.setParams(productId, productName, quantity, Convert.ToString(selectedRow.Cells["edge_value"].Value), Convert.ToString(selectedRow.Cells["price"].Value));
                if (Form.ShowDialog() == DialogResult.OK)
                {
                    fillTempProducts();
                    onTempProductSelect();
                }
            }
        }
        private void onDeleteTempProduct()
        {
            if (gridProducts.SelectedRows.Count == 0) return;
            if (MessageBox.Show("გინდათ საქონლის წაშლა?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var selectedRows = gridProducts.SelectedRows; 
                foreach (DataGridViewRow dgr in selectedRows)
                {
                    int rec_id = 0;
                    if (!int.TryParse(dgr.Cells["rec_id"].Value.ToString(), out rec_id)) return;

                    string sql = "DELETE FROM book.ExMSTemplateProducts WHERE id=" + rec_id;
                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql))
                    {
                        MessageBox.Show("საქონლის წაშლა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                fillTempProducts();
                onTempProductSelect();
            }
        }
        private bool insertOrUpdateColumnValues(int column_id, IQueryable<string> columnValues)
        {
            int orderBy = GetProgramManager().GetDataManager().GetIntegerValue("SELECT MAX(order_by)+1 FROM book.ExMSColumnValues WHERE col_id=" + column_id);

            Hashtable parameters = new Hashtable();
            string sql = string.Empty;

            foreach (string value in columnValues)
            {
                parameters.Clear();
                sql = "INSERT INTO book.ExMSColumnValues (col_id, name, order_by) VALUES (@col_id, @name, @order_by)";
                parameters.Add(@"col_id", column_id);
                parameters.Add(@"name", value);
                parameters.Add(@"order_by", orderBy);

                if (!ProgramManager.GetDataManager().ExecuteSql(sql, parameters)) return false;

                orderBy++;
            }


            return true;
        }
        private bool insertTemplates(int type_id, DataTable m_data, Dictionary<string, int> allProducts)
        {
            string sql = @"SELECT type_columns.type_id, type.name AS type_name, col.db_name, col.name, col.is_interval FROM book.ExMSGeneralTypes AS type INNER JOIN 
                           book.ExMSGeneralTypeColumns AS type_columns ON type.id=type_columns.type_id
                           INNER JOIN book.ExMSColumns AS col ON type_columns.col_id=col.id where type_columns.type_id=" + type_id;

            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            if (data == null) return false;

            Hashtable sqlParams = new Hashtable();
            string dbName = string.Empty;
            string columnName = string.Empty;
            string queryParameters, queryValues;            

            var typeData = m_data.Rows.Cast<DataRow>().AsQueryable().Where(p => p.Field<string>("დასახელება") == data.Rows[0].Field<string>("type_name")).AsQueryable<DataRow>();
            if (typeData == null) return false;

            var productColumns = m_data.Columns.Cast<DataColumn>().AsQueryable().Where(p => p.ColumnName.StartsWith("Column")).Select(p=>p.ColumnName).AsQueryable<string>();

            int templateId;
            List<string> ProductValues = new List<string>();

            foreach (DataRow typeRow in typeData)
            {
                ProductValues.Clear();
                sqlParams.Clear();
                foreach (DataRow dr in data.Rows)
                {
                    dbName = dr.Field<string>("db_name");
                    columnName = dr.Field<string>("name");
                    if (typeRow.Field<string>(columnName) == null) continue;
                   
                    if (dr.Field<bool>("is_interval"))
                    {
                        string paramFrom = typeRow.Field<string>(columnName).Split('-').FirstOrDefault();
                        string paramTo = typeRow.Field<string>(columnName).Split('-').LastOrDefault();

                        if ((paramFrom == null) || (paramTo == null)) return false;

                        double valueFrom = 0, valueTo = 0;

                        if (!double.TryParse(paramFrom, out valueFrom)) return false;

                        if (!double.TryParse(paramTo, out valueTo)) return false;

                        sqlParams.Add(string.Format("{0}_from", dbName), valueFrom);
                        sqlParams.Add(string.Format("{0}_to", dbName), valueTo);
                    }
                    else
                    {
                        sqlParams.Add(dbName, typeRow.Field<string>(columnName));
                    }
                    
                    
                }

                sqlParams.Add("type_id", type_id);
                queryParameters = string.Join(", ", sqlParams.Keys.Cast<string>().ToArray());
                queryValues = string.Join(", ", sqlParams.Keys.Cast<string>().Select(p=>string.Format("@{0}", p)).ToArray());

                sql = string.Format("INSERT INTO book.ExMSTemplates ({0}) VALUES ({1})", queryParameters, queryValues);

                if (!GetProgramManager().GetDataManager().ExecuteSql(sql, sqlParams)) return false;


                templateId = GetProgramManager().GetDataManager().GetIntegerValue("SELECT MAX(id) FROM book.ExMSTemplates");
                if (templateId == -1) return false;

                string code, value;

                foreach (string productColumn in productColumns)
                {
                    string product=typeRow.Field<string>(productColumn);
                    if (string.IsNullOrEmpty(product)) continue;

                    code = product;
                    value = string.Empty;

                    if (product.Contains("("))
                    {
                        code = product.Split(new char[] { '(', ')' })[0];
                        value = product.Split(new char[] { '(', ')' })[1];
                    }

                    double quantity = 1;
                    if (!string.IsNullOrEmpty(value))
                    {                    
                        if (!double.TryParse(value, out quantity)) return false;
                    }

                    if (!allProducts.ContainsKey(code)) continue;

                    int productId=allProducts[code];
                    

                    ProductValues.Add(string.Format("('{0}', '{1}', '{2}')", templateId, productId, quantity));
                }


                string sqlProductValues = string.Join(", ", ProductValues.ToArray());

                sql = string.Format("INSERT INTO book.ExMSTemplateProducts (template_id, product_id, quantity) VALUES {0}", sqlProductValues);
                if (!GetProgramManager().GetDataManager().ExecuteSql(sql)) return false;



            }
            return true;
        }
        private bool onImportFromExcel(ref string msg)
        {                        
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files|*.xls;*.xlsx;*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ProgressDispatcher.Activate();
                Dictionary<string, int> allProducts = new Dictionary<string, int>();
                DataTable data = new DataTable();
                data = GetProgramManager().GetDataManager().GetTableData(@"SELECT DISTINCT id, code FROM book.Products WHERE path like '0#1#10%'");

                if (data==null || data.Rows.Count==0)
                {
                    MessageBox.Show("ბაზაში საქონელი ვერ მოიძებნა!", "შეცდომა", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                bool ask=true;
                string code;
                foreach (DataRow row in data.Rows)
                {
                    code=row.Field<string>("code");
                    if (!allProducts.ContainsKey(code))
                    {
                        allProducts.Add(code, row.Field<int>("id"));
                    }
                    else
                    {
                        if (ask && MessageBox.Show(@"სასაქონლო სიაში ზოგიერთი კოდი მეორდება!"+ '\n'+"მოხდება ერთ-ერთი საქონლის მინიჭება კომოლექტზე!"+ 
                            '\n' +"შესრულდეს იმპორტი?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            ProgressDispatcher.Deactivate();
                            return false;
                           
                        }
                        else
                        {
                            ask = false;
                            continue;
                        }
                    }

                }
                                 
                List<string> existingColumnNames = new List<string>();
                List<string> existingTypeNames = new List<string>();
                string sql = @"SELECT name FROM book.ExMSColumns";
                data = new DataTable();
                data = GetProgramManager().GetDataManager().GetTableData(sql);
                existingColumnNames.AddRange(data.Rows.Cast<DataRow>().Select(p => p.Field<string>("name")).ToList());

                sql = @"SELECT name FROM book.ExMSGeneralTypes";
                data = new DataTable();
                data = GetProgramManager().GetDataManager().GetTableData(sql);
                existingTypeNames.AddRange(data.Rows.Cast<DataRow>().Select(p => p.Field<string>("name")).ToList());

                string fileName = dialog.FileName;

                DataTable m_Data = GetProgramManager().GetDataManager().GetTableDataFromExcel(fileName);
                if ((m_Data == null) || (m_Data.Rows.Count == 0))
                {
                    ProgressDispatcher.Deactivate();
                    return false;
                }

                Dictionary<int, string> columns = new Dictionary<int, string>();

                var columnNames = m_Data.Columns.Cast<DataColumn>().Where(p => (p.ColumnName != "დასახელება") && (!p.ColumnName.StartsWith("Column"))).Select(p => p.ColumnName).AsEnumerable();
                int columnId = ProgramManager.GetDataManager().GetIntegerValue("select isnull(max(id), 0)+1 from book.ExMSColumns");
                string dbName = "ex_col_" + columnId.ToString();
                string name = string.Empty;
                sql = string.Empty;
                string sqlParam = string.Empty;
                Hashtable parameters = new Hashtable();
                foreach (string columnName in columnNames)
                {
                    name = columnName;
                    if (existingColumnNames.Contains(name))
                    {
                        if (MessageBox.Show("სვეტი \"" + name + "\" უკვე არსებობს! \r\n შესრულდეს სვეტის დამატება?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            continue;
                    }
                    bool isInterval = isInretval(m_Data.Rows[0], columnName);

                    sql = "INSERT INTO book.ExMSColumns (db_name, name, is_interval) VALUES (@db_name, @name, @is_interval)";
                    parameters.Clear();
                    parameters.Add(@"db_name", dbName);
                    parameters.Add(@"name", name);
                    parameters.Add(@"is_interval", isInterval);

                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql, parameters))
                    {
                        ProgressDispatcher.Deactivate();
                        return false;
                    }

                    sqlParam = string.Format("{0} NVARCHAR(MAX) ", dbName);
                    if (isInterval)
                    {
                        sqlParam = string.Format("{0}_from FLOAT , {0}_to FLOAT", dbName);
                    }

                    sql = @"ALTER TABLE book.ExMSTemplates ADD " + sqlParam;

                    if (!ProgramManager.GetDataManager().ExecuteSql(sql))
                    {
                        ProgressDispatcher.Deactivate();
                        return false;
                    }
                    columnId = ProgramManager.GetDataManager().GetIntegerValue("SELECT MAX(id) FROM  book.ExMSColumns");
                    dbName = "ex_col_" + (columnId + 1).ToString();

                    columns.Add(columnId, columnName);

                    if (isInterval) continue;

                    var columnValues = m_Data.Rows.Cast<DataRow>().AsQueryable().Where(p =>(!string.IsNullOrEmpty(p.Field<string>(columnName)))).Select(p => p.Field<string>(columnName)).Distinct().AsQueryable<string>();

                    if (!insertOrUpdateColumnValues(columnId, columnValues))
                    {
                        ProgressDispatcher.Deactivate();
                        return false;
                    }
                }

                if (!m_Data.Columns.Contains("დასახელება"))
                {
                    msg = "აუცილებელი ველი \"დასახელება\" ვერ მოიძებნა!";
                    ProgressDispatcher.Deactivate();
                    return false;
                }
                var types = m_Data.Rows.Cast<DataRow>().AsQueryable().Where(p => p.Field<string>("დასახელება") != null).Select(m=>m.Field<string>("დასახელება")).Distinct().AsQueryable<string>();

                int typeId;
                string typeName = string.Empty;
                foreach (string type in types)
                {
                    DataRow row = m_Data.Rows.Cast<DataRow>().Where(p => (p.Field<string>("დასახელება") != null) && (p.Field<string>("დასახელება") == type)).FirstOrDefault();
                    if (row == null) continue;
                    typeName = row.Field<string>("დასახელება");

                    if (existingTypeNames.Contains(typeName))
                    {
                        if (MessageBox.Show("სახეობა \"" + typeName + "\" უკვე არსებობს! \r\n შესრულდეს სახეობის დამატება?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            continue;
                    }

                    sql = "INSERT INTO book.ExMSGeneralTypes (name) VALUES (@name)";

                    sqlParams.Clear();
                    sqlParams.Add(@"name", typeName);
                    if (!ProgramManager.GetDataManager().ExecuteSql(sql, sqlParams))
                    {
                        ProgressDispatcher.Deactivate();
                        return false;
                    }

                    typeId = GetProgramManager().GetDataManager().GetIntegerValue("SELECT MAX(id) FROM book.ExMSGeneralTypes");

                    foreach (int colId in columns.Keys)
                    {
                        if (!string.IsNullOrEmpty(row.Field<string>(columns[colId])))
                        {
                            sql = "INSERT INTO book.ExMSGeneralTypeColumns (col_id, type_id, order_by) VALUES (@col_id, @type_id, @order_by)";
                            sqlParams.Clear();
                            sqlParams.Add(@"col_id", colId);
                            sqlParams.Add(@"type_id", typeId);
                            sqlParams.Add(@"order_by", 0);

                            if (!GetProgramManager().GetDataManager().ExecuteSql(sql, sqlParams))
                            {
                                ProgressDispatcher.Deactivate();
                                return false;
                            }
                        }
                    }

                    if (!insertTemplates(typeId, m_Data, allProducts))
                    {
                        ProgressDispatcher.Deactivate();
                        return false;
                    }

                }


                fillColumns();
                fillTypes();
                fillTemplates();
                fillTempProducts();
                fillTypeCombo();
                msg = "იმპორტი წარმატებით შესრულდა!";
                ProgressDispatcher.Deactivate();
            }
            else
            {
                msg = string.Empty;
                return false;
            }
            
            return true;
        }
        private bool isInretval(DataRow row, string columnName)
        {
            bool isInterval = false;
            if (row[columnName].ToString().Contains("-")) isInterval = true;
            return isInterval;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            onAddColumn();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            onEditColumn();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            onDeleteColumn();
        }

        private void m_GridColumns_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex<0) return;
            onEditColumn();
        }

        private void btnAddType_Click(object sender, EventArgs e)
        {
            onAddType();
        }

        private void btnEditType_Click(object sender, EventArgs e)
        {
            onEditType();
        }

        private void btnDeleteType_Click(object sender, EventArgs e)
        {
            onDeleteType();
        }

        private void m_GridTypes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; 
            onEditType();
        }

        private void comboTypes_SelectedValueChanged(object sender, EventArgs e)
        {
            if (typesAreFilled)
            {
                fillTemplates();
                onTempProductSelect();                
            }
        }

        private void btnAddTemplate_Click(object sender, EventArgs e)
        {
            fillColumnValues(0, null);
        }

     
        private void btnEditTemplate_Click(object sender, EventArgs e)
        {
            onEditTemplate();
        }

        private void gridTemplate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; 
            onEditTemplate();
        }
        private void btnDeleteTemplate_Click(object sender, EventArgs e)
        {            
            onDeleteTemplate();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            onAddTempProduct();
        }
        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            onEditTempProduct();
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            onDeleteTempProduct();
        }

        private void gridProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; 
            onEditTempProduct();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string msgText = "იმპორტი ვერ მოხერხდა!";
            MessageBoxIcon icon = MessageBoxIcon.Error;            
            if (onImportFromExcel(ref msgText))
            {
                icon = MessageBoxIcon.Information;
            }
            if (msgText!=string.Empty)
                MessageBox.Show(msgText, Application.ProductName, MessageBoxButtons.OK, icon);

        }

        private void gridTemplate_SelectionChanged_1(object sender, EventArgs e)
        {
            onTempProductSelect();
        }

        private void gridTemplate_FilterStringChanged(object sender, EventArgs e)
        {
            OnCustomFilter();
        }

        private void gridTemplate_SortStringChanged(object sender, EventArgs e)
        {
            OnCustomFilter();
        }

        private void OnFillprices()
        {
            DataTable _data = GetProgramManager().GetDataManager().GetTableData(@"SELECT s.id,p.name AS product_name,d.name AS method_name,s.price FROM book.MultitestStaffSalaries AS s
INNER JOIN book.Products AS p ON p.id=s.product_id
INNER JOIN book.Departments AS d ON d.id=s.department_id");

            foreach (DataRow row in _data.Rows)
            {
                mGridPrices.Rows.Add(row["id"], row["product_name"], row["method_name"], row["price"]);
            }

        }

        private void OnFillSoilethods()
        {
            DataTable _data = GetProgramManager().GetDataManager().GetTableData(@"SELECT * FROM book.MultitestSoilMethods");

            foreach (DataRow row in _data.Rows)
            {
                mGridSoilMethods.Rows.Add(row["id"], row["parameter"], row["very_low"], row["low"], row["average"], row["good"], row["high"], row["method"]);
            }
        }

        private void btnAddPrice_Click(object sender, EventArgs e)
        {
            OnAddPrice(0);
        }

        private void btnEditPrice_Click(object sender, EventArgs e)
        {
            if (mGridPrices.SelectedRows.Count == 0) return;
            OnAddPrice(Convert.ToInt32(mGridPrices.SelectedRows[0].Cells[0].Value));
        }

        private void OnAddPrice(int id)
        {
            using (ExMultitestPriceForm _form = new ExMultitestPriceForm(GetProgramManager(), id))
            {
                if (_form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable _data = GetProgramManager().GetDataManager().GetTableData(@"SELECT TOP(1) s.id,p.name AS product_name,d.name AS method_name,s.price FROM book.MultitestStaffSalaries AS s
INNER JOIN book.Products AS p ON p.id=s.product_id
INNER JOIN book.Departments AS d ON d.id=s.department_id WHERE s.id=" + (id == 0 ? _form.Res : id));

                    if (id == 0)
                    {
                        mGridPrices.Rows.Add(_data.Rows[0]["id"], _data.Rows[0]["product_name"], _data.Rows[0]["method_name"], _data.Rows[0]["price"]);
                    }
                    else
                    {
                        mGridPrices.SelectedRows[0].Cells[0].Value = _data.Rows[0]["id"];
                        mGridPrices.SelectedRows[0].Cells[1].Value = _data.Rows[0]["product_name"];
                        mGridPrices.SelectedRows[0].Cells[2].Value = _data.Rows[0]["method_name"];
                        mGridPrices.SelectedRows[0].Cells[3].Value = _data.Rows[0]["price"];
                    }
                }
                else
                {
                    if (_form.Res < 0)
                        MessageBox.Show("შენახვა ვერ მოხერხდა");
                }
            }
        }

        private void btnDeletePrice_Click(object sender, EventArgs e)
        {
            if (mGridPrices.SelectedRows.Count == 0) return;

            if (MessageBox.Show("წაიშალოს ჩანაწერი?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (GetProgramManager().GetDataManager().ExecuteSql("DELETE FROM book.MultitestStaffSalaries WHERE id=" + mGridPrices.SelectedRows[0].Cells[0].Value))
                {
                    mGridPrices.Rows.Remove(mGridPrices.SelectedRows[0]);
                }
            }
        }

        private void mGridPrices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OnAddPrice(Convert.ToInt32(mGridPrices.SelectedRows[e.RowIndex].Cells[0].Value));
        }

        private void btnAddMethod_Click(object sender, EventArgs e)
        {
            OnAddSoilMethod(0);
        }

        private void btnEditMethod_Click(object sender, EventArgs e)
        {
            if (mGridSoilMethods.SelectedRows.Count == 0) return;
            OnAddSoilMethod(Convert.ToInt32(mGridSoilMethods.SelectedRows[0].Cells[0].Value));
        }

        private void btnDelMethod_Click(object sender, EventArgs e)
        {
            if (mGridSoilMethods.SelectedRows.Count == 0) return;

            if (MessageBox.Show("წაიშალოს ჩანაწერი?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (GetProgramManager().GetDataManager().ExecuteSql("DELETE FROM book.MultitestSoilMethods WHERE id=" + mGridSoilMethods.SelectedRows[0].Cells[0].Value))
                {
                    mGridSoilMethods.Rows.Remove(mGridSoilMethods.SelectedRows[0]);
                }
            }
        }

        private void OnAddSoilMethod(int id)
        {
            using (ExMultitestSoilMethodForm _form = new ExMultitestSoilMethodForm(GetProgramManager(), id))
            {
                if (_form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable _data = GetProgramManager().GetDataManager().GetTableData(@"SELECT TOP(1) * FROM book.MultitestSoilMethods WHERE id=" + (id == 0 ? _form.Res : id));
                    if (id == 0)
                    {
                        mGridSoilMethods.Rows.Add(_data.Rows[0]["id"], _data.Rows[0]["parameter"], _data.Rows[0]["very_low"], _data.Rows[0]["low"], _data.Rows[0]["average"], _data.Rows[0]["good"], _data.Rows[0]["high"], _data.Rows[0]["method"]);
                    }
                    else
                    {
                        mGridSoilMethods.SelectedRows[0].Cells[0].Value = _data.Rows[0]["id"];
                        mGridSoilMethods.SelectedRows[0].Cells[1].Value = _data.Rows[0]["parameter"];
                        mGridSoilMethods.SelectedRows[0].Cells[2].Value = _data.Rows[0]["very_low"];
                        mGridSoilMethods.SelectedRows[0].Cells[3].Value = _data.Rows[0]["low"];
                        mGridSoilMethods.SelectedRows[0].Cells[4].Value = _data.Rows[0]["average"];
                        mGridSoilMethods.SelectedRows[0].Cells[5].Value = _data.Rows[0]["good"];
                        mGridSoilMethods.SelectedRows[0].Cells[6].Value = _data.Rows[0]["high"];
                        mGridSoilMethods.SelectedRows[0].Cells[7].Value = _data.Rows[0]["method"];
                    }
                }
                else
                {
                    if (_form.Res < 0)
                        MessageBox.Show("შენახვა ვერ მოხერხდა");
                }
            }
        }
        
    }
}
