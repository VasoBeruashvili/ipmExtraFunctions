using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using ipmBLogic;
using Excel = Microsoft.Office.Interop.Excel;

namespace ipmExtraFunctions
{
    public partial class ProvidersMoveForm :Form
    {
        private ProgramManagerBasic mPm;
        private List<int> storesCount = new List<int>();
        private string currPath = "0#1#10%";
        List<string> save_dates = new List<string>();
        public ProvidersMoveForm(ProgramManagerBasic pm)
        {
            InitializeComponent();
            this.mPm = pm;
            mGridProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return this.mPm;
        }

        private void ProvidersMoveForm_Load(object sender, EventArgs e)
        {
            FillProductGroups();
            FillProductsGrid(this.currPath);

            if (this.Tag.Equals("Move"))
                this.Text = "გაფართოებული გადანაწილება საწყობებში";
            else if (this.Tag.Equals("Order"))
            {
                this.Text = "გაფართოებული შეკვეთა მომწოდებლებთან";
                panelVendor.Visible = true;
            }
            this.Focus();
        }

        private void FillProductGroups()
        {
            string sql = @"WITH Groups (id,path,name,parentid,level) AS 
                            ( 
                             SELECT id,path,name,parentid,0 AS level FROM book.GroupProducts WHERE parentid=0
                             UNION ALL
                             SELECT t0.id,t0.path,t0.name,t0.parentid,level + 1 FROM book.GroupProducts AS t0 INNER JOIN Groups AS t1 ON t1.id=t0.parentid
                             )
                            SELECT * FROM Groups WHERE path LIKE '0#1#10%'";
            DataTable dt = GetProgramManager().GetDataManager().GetTableData(sql);

            foreach (DataRow row in dt.Rows)
            {
                TreeNode tNode = new TreeNode();
                tNode.Name = row["id"].ToString();
                tNode.Text = row["name"].ToString();
                tNode.Tag = row["path"];

                if (row["level"].Equals(1))
                {
                    treeProviders.Nodes.Add(tNode);
                }
                else
                {
                    TreeNode pNode = GetAllNodes(treeProviders.Nodes.OfType<TreeNode>()).Where(n => n.Name == row["parentid"].ToString()).SingleOrDefault();
                    if (pNode != null)
                    {
                        pNode.Nodes.Add(tNode);
                    }
                }
                treeProviders.SelectedNode = treeProviders.Nodes[0];
                treeProviders.Nodes[0].Expand();
            }
        }

        private IEnumerable<TreeNode> GetAllNodes(IEnumerable<TreeNode> data)
        {
            var result = new List<TreeNode>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    result.Add(item);
                    result.AddRange(GetAllNodes(item.Nodes.OfType<TreeNode>()));
                }
            }
            return result;
        }

        private void FillProductsGrid(string path)
        {
            Func<DataTable,bool> setColumns = (DataTable dtb) =>
                {
                    storesCount.Clear();
                    foreach (DataGridViewColumn col in mGridProducts.Columns)
                    {
                        switch (col.Name)
                        {
                            case "product_name":
                                col.Width = 110;
                                col.HeaderText = "საქონელი";
                                break;
                            case "unit_name":
                                col.Width = 70;
                                col.HeaderText = "ერთეული";
                                break;
                            case "store1":
                                col.Width = 60;
                                col.HeaderText = "მთავარი საწყობი";
                                break;
                            case "summ":
                                col.Width = 60;
                                col.HeaderText = "ჯამი";
                                break;
                            default:
                                if (!col.Name.EndsWith("_id"))
                                {
                                    col.Width = 60;
                                    storesCount.Add(col.Index);
                                    dtb.Columns[col.HeaderText].ReadOnly = false;
                                }
                                else
                                {
                                    col.Visible = false;
                                }
                                break;
                        }
                    }

                    return true;
                };
            string sql;

            if (this.Tag.Equals("Move"))
            {
                sql = @"DECLARE @str NVARCHAR(MAX) = N'',@sum VARCHAR(MAX) = '',@path VARCHAR(50) = '" + path + @"'
                           DECLARE @st_id INT 
                           DECLARE @st_name NVARCHAR(50)
                           DECLARE @table TABLE (row_id INT IDENTITY (1,1), store_id INT,store_name NVARCHAR(50)) INSERT @table SELECT id,name FROM book.Stores WHERE path LIKE '0#1#3%'
                           DECLARE @max_quant VARCHAR(50) = (SELECT c.db_name FROM config.Columns c INNER JOIN book.SystemColumns s ON s.name=c.full_name WHERE c.table_id='TABLE_PRODUCT' AND s.tag='MAX_QUANTITY')
                           DECLARE @rows_count INT = (SELECT COUNT(row_id) FROM @table),@intFlag INT = 0
                           
                           WHILE @intFlag < @rows_count 
                               BEGIN  
                                   SET @intFlag = @intFlag + 1
                                   SELECT @st_id=store_id,@st_name=store_name FROM @table WHERE row_id=@intFlag
                                   
                                   SET @str = @str + ','+CONVERT(varchar,@st_id)+' AS store'+CONVERT(varchar,@st_id)+'_id,ISNULL((SELECT SUM(CASE WHEN ISNULL(pr.rest_quant,0) <  (CASE WHEN ISNULL(p.'+@max_quant+',0) > ISNULL(p.min_Quantity,0) THEN p.'+@max_quant+' ELSE p.min_Quantity END) THEN (CASE WHEN ISNULL(p.'+@max_quant+',0) > ISNULL(p.min_Quantity,0) THEN p.'+@max_quant+' ELSE p.min_Quantity END) - ISNULL(pr.rest_quant,0) ELSE ISNULL(pr.rest_quant,0) END) AS rest_quant
                                                               FROM doc.ProductRestFinal pr
                                                               INNER JOIN book.Products p ON p.id = pr.product_id
                                                               WHERE pr.product_id=t0.id AND p.path LIKE '''+@path+''' AND pr.store_id='+CONVERT(varchar,@st_id)+'),0) AS N'''+@st_name+''''
                                   
                                   SET @sum = '+ISNULL((SELECT SUM(CASE WHEN ISNULL(pr.rest_quant,0) < (CASE WHEN ISNULL(p.'+@max_quant+',0) > ISNULL(p.min_Quantity,0) THEN p.'+@max_quant+' ELSE p.min_Quantity END) THEN (CASE WHEN ISNULL(p.'+@max_quant+',0) > ISNULL(p.min_Quantity,0) THEN p.'+@max_quant+' ELSE p.min_Quantity END) - ISNULL(pr.rest_quant,0) ELSE ISNULL(pr.rest_quant,0) END) AS rest_quant
                                                               FROM doc.ProductRestFinal pr
                                                               INNER JOIN book.Products p ON p.id = pr.product_id
                                                               WHERE pr.product_id=t0.id AND p.path LIKE '''+@path+''' AND pr.store_id='+CONVERT(varchar,@st_id)+'),0)'
                             
                               END 
                           SET @str = SUBSTRING(@str,2,LEN(@str))
                           SET @sum = SUBSTRING(@sum,2,LEN(@str))

                          EXEC(N'SELECT t0.id AS product_id,t2.id AS unit_id,t0.name AS product_name,t2.full_name AS unit_name,t3.id AS store1_id,t1.final_restQuantity AS store1,'+ @sum +' AS summ,'+@str+' FROM book.Products AS t0 
                          INNER JOIN doc.ProductRestFinal AS t1 ON t1.product_id = t0.id AND t1.final_restQuantity > 0
                          INNER JOIN book.Units AS t2 ON t2.id = t0.unit_id 
                          INNER JOIN book.Stores AS t3 ON t3.id=t1.store_id AND t3.path LIKE ''0#1#2%'' AND t0.path LIKE '''+@path+''' ')";
            }
            else
            {
                sql = @"DECLARE @str NVARCHAR(MAX) = N'',@sum VARCHAR(MAX) = '',@st_name NVARCHAR(50),@st_id VARCHAR(10),@path VARCHAR(50) = '" + path + @"'
                        DECLARE @table TABLE (row_id INT IDENTITY (1,1), store_id INT,store_name NVARCHAR(50)) INSERT @table SELECT id,name FROM book.Stores WHERE path LIKE '0#1#3%'
                        DECLARE @max_quant VARCHAR(50) = (SELECT c.db_name FROM config.Columns c INNER JOIN book.SystemColumns s ON s.name=c.full_name WHERE c.table_id='TABLE_PRODUCT' AND s.tag='MAX_QUANTITY')
                        DECLARE @rows_count INT = (SELECT COUNT(row_id) FROM @table),@intFlag INT = 0

                        WHILE @intFlag < @rows_count
                            BEGIN
                                SET @intFlag = @intFlag + 1
                                SELECT @st_id=CONVERT(varchar,store_id),@st_name=store_name FROM @table WHERE row_id=@intFlag
        
                                SET @str = @str + ','+CONVERT(varchar,@st_id)+' AS store'+@st_id+'_id,CASE WHEN ISNULL((SELECT SUM(final_restQuantity) FROM doc.ProductRestFinal WHERE (store_id='+@st_id+') AND product_id=t0.id),0) < 
                                          ((CASE WHEN LEFT(t0.min_Quantity,1)=''='' AND  t0.min_Quantity LIKE ''%;%''  THEN  ISNULL(CAST (SUBSTRING(t0.min_Quantity, CHARINDEX('';'',t0.min_Quantity)+1,LEN(t0.min_Quantity)-CHARINDEX('';'',t0.min_Quantity) )  AS FLOAT),0)  ELSE 0 END) +
                                          (CASE WHEN LEFT(t0.min_Quantity,1)=''='' AND  t0.min_Quantity LIKE ''%;%'' THEN  (  SELECT ISNULL(SUM(pf.amount),0) FROM doc.ProductsFlow pf INNER JOIN doc.GeneralDocs gd ON gd.id= pf.general_id  WHERE pf.coeff=-1 AND pf.product_id= t0.id  AND gd.tdate> DATEADD(day,-CAST( SUBSTRING(t0.min_Quantity,2,CHARINDEX('';'',t0.min_Quantity)-2) AS INT),GETDATE())  )  ELSE t0.min_Quantity END)) THEN
                                          ISNULL(NULLIF(t0.'+@max_quant+',''''),0) - ISNULL((SELECT SUM(final_restQuantity) FROM doc.ProductRestFinal WHERE (store_id='+@st_id+') AND product_id=t0.id),0) ELSE 0 END AS N'''+@st_name+''''
        
                        END

                        SET @str = CASE WHEN SUBSTRING(@str,2,LEN(@str)) = '' THEN '' ELSE ','+SUBSTRING(@str,2,LEN(@str)) END

                       EXEC(N'SELECT t0.id AS product_id,t2.id AS unit_id,t0.name AS product_name,t2.full_name AS unit_name,t3.id AS store1_id,t1.final_restQuantity AS store1'+@str+' FROM book.Products AS t0 
                          INNER JOIN doc.ProductRestFinal AS t1 ON t1.product_id = t0.id AND t1.final_restQuantity > 0
                          INNER JOIN book.Units AS t2 ON t2.id = t0.unit_id 
                          INNER JOIN book.Stores AS t3 ON t3.id=t1.store_id AND t3.path LIKE ''0#1#2%'' AND t0.path LIKE '''+@path+''' ')";
            }

            DataTable dt = GetProgramManager().GetDataManager().GetTableData(sql);
            mGridProducts.DataSource = dt;
            setColumns.Invoke(dt);

            splitPanelMoves.Panel2Collapsed = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveInExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void ExportToExcel()
        {
            Func<int, bool> AllowInsertRow = (int rowIndex) =>
                {
                    float val;
                    string quantity;
                    bool allow = false;
                    foreach (DataGridViewCell cell in mGridProducts.Rows[rowIndex].Cells)
                    {
                        if (mGridProducts.Columns[cell.ColumnIndex].Name.EndsWith("_id"))
                        {
                            quantity = Convert.ToString(cell.Value);
                            if (!string.IsNullOrEmpty(quantity))
                            {
                                if (float.TryParse(quantity, out val))
                                {
                                    if (val != 0)
                                        allow = allow | true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return allow;
                };


            if (mGridProducts.RowCount == 0)
                return;

            bool allowInsertRow = true;
            List<int> hiddenCols = new List<int>();
            string destPath = string.Empty;
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.FileName = "Export Data";
            savedialog.Filter = "ექსელის ფაილი (*.xlsx;*.xls)|*.xlsx;*.xls";
            if (savedialog.ShowDialog() == DialogResult.OK)
                if (savedialog.FileName == string.Empty)
                    return;

            destPath = savedialog.FileName;

            //System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Excel.Application oExcel = new Excel.Application();
            oExcel.Application.Workbooks.Add(true);
            Excel.Workbooks oBooks = oExcel.Workbooks;
            Excel.Workbook oBook = oBooks.get_Item(1);
            Excel.Sheets oSheets = oBook.Worksheets;
            Excel.Worksheet oSheet = (Excel.Worksheet)oSheets.get_Item(1);
            Excel.Range cells = (Excel.Range)oSheet.Cells;
            cells.NumberFormat = "@";

            int x = 1, y = 2;
            for (int i = 0; i <= mGridProducts.RowCount - 1; i++)
            {
                x = 1;
                for (int j = 0; j <= mGridProducts.ColumnCount - 1; j++)
                {
                    if (!mGridProducts.Columns[j].Visible) continue;

                    DataGridViewCell cell = mGridProducts[j, i];
                    if (i == 0)
                    {
                        if (mGridProducts.Columns[cell.ColumnIndex].Name.EndsWith("_id"))
                            oExcel.Cells[1, x] = GetProgramManager().GetDataManager().GetStoreAddressByID(6);

                        oExcel.Cells[2, x] = mGridProducts.Columns[j].HeaderText;
                    }

                    if (!AllowInsertRow(cell.RowIndex))
                    {
                        x++;
                        allowInsertRow = false;
                        continue;
                    }

                    oExcel.Cells[y + 1, x] = cell.Value;
                    allowInsertRow = true;

                    x++;
                }
                if (allowInsertRow)
                    y++;
            }

            oExcel.Columns.AutoFit();
            oExcel.ActiveWorkbook.SaveCopyAs(destPath);
            oExcel.ActiveWorkbook.Saved = true;
            oExcel.Quit();
        }

        private void btnApportion_Click(object sender, EventArgs e)
        {
            bool res = this.Tag.Equals("Move") ? OnApportionMoves() : OnApportionOrders();
            if (!res)
            {
                MessageBox.Show("ოპერაცია ვერ განხორციელდა!");
            }
            else
            {
                splitPanelMoves.Panel2Collapsed = false;
                fillSavedMoves();
            }
        }

        private bool fillSavedMoves()
        {
            Func<bool> setColumns = () =>
               {
                   foreach (DataGridViewColumn col in mGridSavedMoves.Columns)
                   {
                       switch (col.Name)
                       {
                           case "id":
                               col.Visible = false;
                               break;
                           case "num":
                               col.HeaderText = "ნომერი";
                               break;
                           case "amount":
                               col.HeaderText = "თანხა";
                               break;
                           case "from_store":
                               col.HeaderText = "საწყობიდან";
                               break;
                           case "to_store":
                               col.HeaderText = "საწყობში";
                               break;
                           case "vendor":
                               col.HeaderText = "მომწოდებელი";
                               break;
                           case "store":
                               col.HeaderText = "საწყობი";
                               break;
                       }
                   }

                   return true;
               };

            if (save_dates.Count() == 1)
                save_dates.Add(save_dates[0]);

            string sql;
            if (this.Tag.Equals("Move"))
            {
                sql = @"SELECT t0.id,(t0.doc_num_prefix+CONVERT(NVARCHAR(20),t0.doc_num)) AS num,ROUND(t0.amount,2) AS amount,N'მთავარი საწყობი' AS from_store,t2.name AS to_store FROM doc.GeneralDocs AS t0 
                                 INNER JOIN doc.ProductMove AS t1 ON t1.general_id=t0.id 
                                 INNER JOIN book.Stores AS t2 ON t0.param_id2 = t2.id
                           WHERE t0.doc_type=20 AND t0.tdate BETWEEN '" + save_dates[0] + "' AND '" + save_dates[1] + "'";
            }
            else
            {
                sql = @"SELECT t0.id,(t0.doc_num_prefix+CONVERT(NVARCHAR(20),t0.doc_num)) AS num,ROUND(t0.amount,2) AS amount,t3.name AS vendor,t2.name AS store FROM doc.GeneralDocs AS t0 
                                 INNER JOIN doc.VendorOrders AS t1 ON t1.general_id=t0.id 
                                 INNER JOIN book.Stores AS t2 ON t0.param_id2 = t2.id
                                 INNER JOIN book.Contragents AS t3 ON t3.id=t0.param_id1
                           WHERE t0.doc_type=31 AND t0.tdate BETWEEN '" + save_dates[0] + "' AND '" + save_dates[1] + "'";
            }

            mGridSavedMoves.DataSource = GetProgramManager().GetDataManager().GetTableData(sql);
            setColumns.Invoke();


            return true;
        }

        private bool OnApportionMoves()
        {
            mGridProducts.CurrentCell = null;
            BusinessLogic bLogic = new BusinessLogic();
            bLogic.Initialize(GetProgramManager().GetDataManager());

            int j = 0;
            DateTime cur_date = DateTime.Now;
            foreach (int index in storesCount)
            {
                IEnumerable<DataGridViewRow> curRows = mGridProducts.Rows.OfType<DataGridViewRow>()
                    .Where(r => r.Cells[index].ColumnIndex == index).Where(r => Convert.ToDouble(r.Cells[index].Value) != 0);
                if (curRows.Count() == 0)
                    continue;

                ProductsFlowItem[] flow_items = new ProductsFlowItem[curRows.Count()];

                int project_id = 1;
                double vat_value = GetProgramManager().GetDataManager().GetVatPercent();
                int store1 = curRows.Select(r => Convert.ToInt32(r.Cells["store1_id"].Value)).FirstOrDefault();
                int store2 = curRows.Select(r => Convert.ToInt32(r.Cells[index - 1].Value)).FirstOrDefault();
                string start_address = GetProgramManager().GetDataManager().GetStoreAddressByID(store1);
                string end_address = GetProgramManager().GetDataManager().GetStoreAddressByID(store2);
                string sender = GetProgramManager().GetDataManager().GetStorePersonByID(store1);
                string reciever = GetProgramManager().GetDataManager().GetStorePersonByID(store2);
                double amount = 0;

                DataTable driver = GetProgramManager().GetDataManager().GetTableData("SELECT t1.driver,t1.model,t1.driver_num,t1.num FROM book.Stores AS t0 INNER JOIN book.Cars AS t1 ON t1.id=t0.car_id INNER JOIN book.Staff AS t2 ON t1.staff_id=t2.id WHERE t0.id=" + store1);
                string driver_name = "";
                string transp_model = "";
                string driver_num = "";
                string num = "";
                if (driver.Rows.Count > 0)
                {
                    driver_name = driver.Rows[0]["driver"].ToString();
                    driver_num = driver.Rows[0]["driver_num"].ToString();
                    num = driver.Rows[0]["num"].ToString();
                    transp_model = driver.Rows[0]["model"].ToString();
                }

                if (j == 0)
                    save_dates.Add(cur_date.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                if (j == storesCount.Count - 1)
                    save_dates.Add(cur_date.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                j++;

                int i = 0;
                foreach (DataGridViewRow row in curRows)
                {
                    flow_items[i] = new ProductsFlowItem();
                    flow_items[i].uid = GetProgramManager().GetDataManager().GetProductUidById(Convert.ToInt32(row.Cells["product_id"].Value));
                    flow_items[i].quantity = Convert.ToInt32(row.Cells[index].Value);
                    flow_items[i].price = GetProgramManager().GetDataManager().GetProductInCostGEL(Convert.ToInt32(row.Cells["product_id"].Value), store1, false);
                    flow_items[i].unit_id = row.Cells["unit_name"].Value.ToString();
                    flow_items[i].ref_id = Convert.ToInt32(row.Cells["product_id"].Value);
                    amount += flow_items[i].price * flow_items[i].quantity;
                    i++;
                }

                if (bLogic.Insert_ProductMove("", cur_date, "", "1", "საქონლის გადატანა", amount, 1, 1.0, vat_value, 0, store1, store2, project_id,
                   GetProgramManager().GetUserID(), true, "", "", start_address, "", driver_num, "", "", end_address, "", "", cur_date,
                   num, transp_model, "", Convert.ToByte(true), Convert.ToByte(false), Convert.ToByte(false), -1, 1, 1, cur_date,
                   -1, cur_date.AddMinutes(1), 1, 1, 1, driver_name, "", flow_items)>0)
                {
                    foreach (ProductsFlowItem item in flow_items)
                    {
                        if (!GetProgramManager().GetDataManager().Prc_InsertOrUpdateRestFinal(
                    item.ref_id, "",
                    store1))
                            return false;

                        if (!GetProgramManager().GetDataManager().Prc_InsertOrUpdateRestFinal(
                           item.ref_id,
                           "",
                           store2))
                            return false;
                    }
                }
                else
                    return false;
            }
            return true;
        }

        private bool OnApportionOrders()
        {
            mGridProducts.CurrentCell = null;
            BusinessLogic bLogic = new BusinessLogic();
            bLogic.Initialize(GetProgramManager().GetDataManager());

            if (btnContragents.Tag == null)
                return false;

            int j = 0;
            DateTime cur_date = DateTime.Now;
            foreach (int index in storesCount)
            {
                IEnumerable<DataGridViewRow> curRows = mGridProducts.Rows.OfType<DataGridViewRow>()
                    .Where(r => r.Cells[index].ColumnIndex == index).Where(r => Convert.ToDouble(r.Cells[index].Value) != 0);
                if (curRows.Count() == 0)
                    continue;

                ProductsFlowItem[] flow_items = new ProductsFlowItem[curRows.Count()];

                int project_id = 1;
                double vat_value = GetProgramManager().GetDataManager().GetVatPercent();
                int vendor_id = Convert.ToInt32(btnContragents.Tag);
                int store = curRows.Select(r => Convert.ToInt32(r.Cells[index - 1].Value)).FirstOrDefault();
                string preffix = GetProgramManager().GetDataManager().GetStorePrefix(store);
                double amount = 0;
                DataTable contragent = GetProgramManager().GetDataManager().GetTableData("SELECT path,code,name,short_name,address,tel,vat_type,type FROM book.Contragents WHERE id=" + vendor_id);

                if (j == 0)
                    save_dates.Add(cur_date.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                if (j == storesCount.Count - 1)
                    save_dates.Add(cur_date.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                j++;

                int i = 0;
                foreach (DataGridViewRow row in curRows)
                {
                    flow_items[i] = new ProductsFlowItem();
                    flow_items[i].uid = GetProgramManager().GetDataManager().GetProductUidById(Convert.ToInt32(row.Cells["product_id"].Value));
                    flow_items[i].quantity = Convert.ToInt32(row.Cells[index].Value);
                    flow_items[i].price = GetProgramManager().GetDataManager().GetProductInCostGEL(Convert.ToInt32(row.Cells["product_id"].Value), 0, false);
                    flow_items[i].unit_id = row.Cells["unit_name"].Value.ToString();
                    flow_items[i].ref_id = Convert.ToInt32(row.Cells["product_id"].Value);
                    amount += flow_items[i].price * flow_items[i].quantity;
                    i++;
                }

                if (!bLogic.Insert_VendorOrder(GetProgramManager().GetDataManager().GetUID(), cur_date, preffix, "", "შეკვეთა მომწოდებელთან № " + preffix + "", amount,
                    1, 1.0, vat_value, store, 1, contragent.Rows[0]["path"].ToString(), contragent.Rows[0]["code"].ToString(), contragent.Rows[0]["name"].ToString(),
                    contragent.Rows[0]["short_name"].ToString(), contragent.Rows[0]["address"].ToString(), contragent.Rows[0]["tel"].ToString(),
                    Convert.ToInt32(contragent.Rows[0]["vat_type"]), Convert.ToInt32(contragent.Rows[0]["type"]), project_id, GetProgramManager().GetUserID(), true, flow_items))
                {
                    return false;
                }


            }

            return true;
        }

        private void mGridProducts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            mGridProducts.ReadOnly = true;
            mGridProducts.EndEdit();

            if (e.ColumnIndex > 6)
            {
                mGridProducts.ReadOnly = false;
                mGridProducts.BeginEdit(true);
            }
        }

        private void treeProviders_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.currPath = e.Node.Tag.ToString() + "%";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FillProductsGrid(this.currPath);
        }

        private void btnContragents_Click(object sender, EventArgs e)
        {
            OnContragentsList();
        }

        public void OnContragentsList()
        {
            int contragent_id = -1;
            if (btnContragents.Tag != null)
                contragent_id = int.Parse(btnContragents.Tag.ToString());

            int res = GetProgramManager().ShowSelectForm("TABLE_CONTRAGENT:VENDOR", contragent_id);
            if (res != -1)
            {
                btnContragents.Tag = res.ToString();
                txtContragent.Text = GetProgramManager().GetDataManager().GetStringValue("SELECT TOP(1) name FROM book.Contragents WHERE id =" + res);
            }
        }

    }
}
