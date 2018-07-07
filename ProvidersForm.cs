using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using Excel = Microsoft.Office.Interop.Excel;
using ipmBLogic;

namespace ipmExtraFunctions
{
    public partial class ProvidersForm : Form
    {
        ProgramManagerBasic pm;
        private List<int> storesCount = new List<int>();
        string path = "0#1#3%";
        List<string> save_dates = new List<string>();

        public ProvidersForm(ProgramManagerBasic pm)
        {
            InitializeComponent();

            this.pm = pm;
            m_GridProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
        }

        private void ProvidersForm_Load(object sender, EventArgs e)
        {
            this.Focus();
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return pm;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FillProductsGrid();
        }

        private void FillProductsGrid()
        {
            if (btnContragents.Tag == null)
                return;

            Func<DataTable, bool> setColumns = (DataTable dtb) =>
            {
                storesCount.Clear();
                foreach (DataGridViewColumn col in m_GridProducts.Columns)
                {
                    switch (col.Name)
                    {
                        case "product_name":
                            col.Width = 150;
                            col.HeaderText = "საქონელი";
                            break;
                        case "unit_name":
                            col.Width = 80;
                            col.HeaderText = "ერთეული";
                            break;
                        case "summ":
                            col.Width = 70;
                            col.HeaderText = "ჯამი";
                            break;
                        default:
                            if (!col.Name.EndsWith("_id"))
                            {
                                col.Width = 90;
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

            string sql = @"DECLARE @str NVARCHAR(MAX) = N'',@sum VARCHAR(MAX) = '',@st_name NVARCHAR(50),@st_id VARCHAR(10)
DECLARE @table TABLE (row_id INT IDENTITY (1,1), store_id INT,store_name NVARCHAR(50)) INSERT @table SELECT id,name FROM book.Stores WHERE id=1 OR path LIKE '" + this.path + @"'
DECLARE @max_quant VARCHAR(50) = (SELECT c.db_name FROM config.Columns c INNER JOIN book.SystemColumns s ON s.name=c.full_name WHERE c.table_id='TABLE_PRODUCT' AND s.tag='MAX_QUANTITY')
DECLARE @rows_count INT = (SELECT COUNT(row_id) FROM @table),@intFlag INT = 0,@vendor_id INT = " + btnContragents.Tag.ToString() + @"

WHILE @intFlag < @rows_count
    BEGIN
        SET @intFlag = @intFlag + 1
        SELECT @st_id=CONVERT(varchar,store_id),@st_name=store_name FROM @table WHERE row_id=@intFlag
        
        SET @str = @str + ','+CONVERT(varchar,@st_id)+' AS store'+@st_id+'_id,CASE WHEN ISNULL((SELECT SUM(final_restQuantity) FROM doc.ProductRestFinal WHERE (store_id='+@st_id+') AND product_id=p.id),0) < 
        ((CASE WHEN LEFT(p.min_Quantity,1)=''='' AND  p.min_Quantity LIKE ''%;%''  THEN  ISNULL(CAST (SUBSTRING(p.min_Quantity, CHARINDEX('';'',p.min_Quantity)+1,LEN(p.min_Quantity)-CHARINDEX('';'',p.min_Quantity) )  AS FLOAT),0)  ELSE 0 END) +
        (CASE WHEN LEFT(p.min_Quantity,1)=''='' AND  p.min_Quantity LIKE ''%;%'' THEN  (  SELECT ISNULL(SUM(pf.amount),0) FROM doc.ProductsFlow pf INNER JOIN doc.GeneralDocs gd ON gd.id= pf.general_id  WHERE pf.coeff=-1 AND pf.product_id= p.id  AND gd.tdate> DATEADD(day,-CAST( SUBSTRING(p.min_Quantity,2,CHARINDEX('';'',p.min_Quantity)-2) AS INT),GETDATE())  )  ELSE p.min_Quantity END)) THEN
         ISNULL(p.'+@max_quant+',0) - ISNULL((SELECT SUM(final_restQuantity) FROM doc.ProductRestFinal WHERE (store_id='+@st_id+') AND product_id=p.id),0) ELSE 0 END AS N'''+@st_name+''''
        
    END

SET @str = SUBSTRING(@str,2,LEN(@str))

EXEC('SELECT p.id AS product_id,t2.id AS unit_id,p.name AS product_name,t2.full_name AS unit_name,'+@str+' FROM book.Products AS p
                         INNER JOIN book.Units AS t2 ON t2.id = p.unit_id 
                         INNER JOIN (SELECT DISTINCT product_id FROM doc.ProductsFlow WHERE vendor_id='+@vendor_id+') AS t4 ON t4.product_id=p.id
                         WHERE p.path LIKE ''0#1#10%''')";

            DataTable dt = GetProgramManager().GetDataManager().GetTableData(sql);
            m_GridProducts.DataSource = dt;
            setColumns.Invoke(dt);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSaveInExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void ExportToExcel()
        {
            if (m_GridProducts.RowCount == 0)
                return;

            bool allowInsertRow = true;
            //bool isHiddenCell = false;
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
            for (int i = 0; i <= m_GridProducts.RowCount - 1; i++)
            {
                x = 1;
                for (int j = 0; j <= m_GridProducts.ColumnCount - 1; j++)
                {
                    if (!m_GridProducts.Columns[j].Visible) continue;

                    DataGridViewCell cell = m_GridProducts[j, i];
                    if (i == 0)
                    {
                        //if (m_GridProducts.Columns[cell.ColumnIndex].Name.Contains(colStoreName))
                            //if (AllowVendorOrder(m_GridProducts.Columns[cell.ColumnIndex].Name))
                            oExcel.Cells[1, x] = GetProgramManager().GetDataManager().GetStoreAddressByID((int)m_GridProducts.Columns[cell.ColumnIndex].Tag);
                        //else
                        //{
                        //    hiddenCols.Add(cell.ColumnIndex);
                        //    continue;
                        //}

                        oExcel.Cells[2, x] = m_GridProducts.Columns[j].HeaderText;
                    }

                    //if (!AllowInsertRow(cell.RowIndex))
                    //{
                    //    x++;
                    //    allowInsertRow = false;
                    //    continue;
                    //}

                    //foreach (int index in hiddenCols)
                    //    if (index == x)
                    //        isHiddenCell = true;

                    //if (isHiddenCell)
                    //{
                    //    isHiddenCell = false;
                    //    continue;
                    //}

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

        private void CreateOrderButton()
        {
            DeleteOrderButton();

            if (m_GridProducts.Rows.Count == 0)
                return;

            ToolStripButton btnOrder = new ToolStripButton();
            btnOrder.Image = global::ipmExtraFunctions.Properties.Resources.note_pinned_16;
            btnOrder.ImageTransparentColor = Color.Magenta;
            btnOrder.AutoSize = true;
            btnOrder.Name = "btnOrder";
            btnOrder.Text = "შეკვეთა მომწოდებელთან";
            btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            toolStripMain.Items.Add(btnOrder);
        }

        private void DeleteOrderButton()
        {
            if (toolStripMain.Items.IndexOfKey("btnOrder") != -1)
                toolStripMain.Items.RemoveByKey("btnOrder");
        }

        private void m_GridProducts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            m_GridProducts.ReadOnly = true;
            m_GridProducts.EndEdit();

            if (e.ColumnIndex > 6)
            {
                m_GridProducts.ReadOnly = false;
                m_GridProducts.BeginEdit(true);
            }
        }
        private void btnOrder_Click(object sender, EventArgs e)
        {
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        //SelectNextRow();
                        return false;
                    default:
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnApportion_Click(object sender, EventArgs e)
        {
            if (!OnApportion())
            {
                MessageBox.Show("ოპერაცია ვერ განხორციელდა!");
            }
            else
            {
                pnlProviders.Panel2Collapsed = false;
                fillSavedOrders();
            }
        }

        private void fillSavedOrders()
        {
            Func<bool> setColumns = () =>
            {
                foreach (DataGridViewColumn col in m_GridProducts.Columns)
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

            string sql = @"SELECT t0.id,(t0.doc_num_prefix+CONVERT(NVARCHAR(20),t0.doc_num)) AS num,ROUND(t0.amount,2) AS amount,t3.name AS vendor,t2.name AS store FROM doc.GeneralDocs AS t0 
                                 INNER JOIN doc.VendorOrders AS t1 ON t1.general_id=t0.id 
                                 INNER JOIN book.Stores AS t2 ON t0.param_id2 = t2.id
                                 INNER JOIN book.Contragents AS t3 ON t3.id=t0.param_id1
                           WHERE t0.doc_type=31 AND t0.tdate BETWEEN '" + save_dates[0] + "' AND '" + save_dates[1] + "'";
            mGridSavedOrders.DataSource = GetProgramManager().GetDataManager().GetTableData(sql);
            setColumns.Invoke();
        }

        private bool OnApportion()
        {
            m_GridProducts.CurrentCell = null;
            BusinessLogic bLogic = new BusinessLogic();
            bLogic.Initialize(GetProgramManager().GetDataManager());

            int j = 0;
            foreach (int index in storesCount)
            {
                IEnumerable<DataGridViewRow> curRows = m_GridProducts.Rows.OfType<DataGridViewRow>()
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
                DateTime cur_date = DateTime.Now;
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
                    flow_items[i].price = GetProgramManager().GetDataManager().GetProductInCostGEL(Convert.ToInt32(row.Cells["product_id"].Value), store, false);
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
