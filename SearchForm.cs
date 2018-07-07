using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using ipmPMBasic;
using Excel = Microsoft.Office.Interop.Excel;
using ipmControls;
using System.IO;

namespace ipmExtraFunctions
{
    public partial class SearchForm : Form
    {

        Dictionary<string, int> mDocs = new Dictionary<string, int>()
        {
            {"გაყიდვები",21},
            {"შესყიდვები",16},
            {"დაბრუნებები მყიდველებისაგან",9},
            {"დაბრუნებები მომწოდებლებთან",32},
        };
        ProgramManagerBasic pm;
        public SearchForm(ProgramManagerBasic pm)
        {
            InitializeComponent();

            this.pm = pm;
           
            m_Picker.dtp_From.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 5);
            m_Picker.dtp_To.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 55, 55) ;
            m_Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FillInitialData();
            comboDocs.SelectedIndex = 0;
            GenerateAdditionColumnControls(panelProductColumns, "book.Products", "TABLE_PRODUCT", 113, 387);
            GenerateAdditionColumnControls(panelContragentColumns, "book.Contragents", "TABLE_CONTRAGENT", 190, 387);
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return pm;
        }

        private void FillInitialData()
        {
            //FILL STORES
            comboStore.DataSource = GetProgramManager().GetDataManager().GetTableData("SELECT 0 AS id,N'ყველა' AS name UNION ALL SELECT id,name FROM book.Stores ORDER BY id");
            comboStore.ValueMember = "id";
            comboStore.DisplayMember = "name";

            //FILL STAFF
            comboStaff.DataSource = GetProgramManager().GetDataManager().GetTableData("SELECT 0 AS id,N'' AS name UNION SELECT id,name FROM book.Staff ORDER BY id");
            comboStaff.ValueMember = "id";
            comboStaff.DisplayMember = "name";


            //FILL PRODUCTS
            DataTable product_data = GetProgramManager().GetDataManager().GetTableData("SELECT 0 AS id,N'' AS name,N'' AS code UNION ALL SELECT id,name,code FROM book.Products WHERE path LIKE '0#1#10%' ORDER BY name");
            comboProductName.DataSource = product_data;
            comboProductName.DisplayMember = "name";
            comboProductName.ValueMember = "id";
           
        }

        private void GenerateAdditionColumnControls(Panel panel, string table_name, string table_tag, int p_width, int c_width)
        {
            panel.Controls.Clear();

            string sqlColumnSelect = "SELECT full_name,[db_name], ISNULL(type,0) AS type,id  FROM config.Columns WHERE ISNULL(isSystem,1) = 0 AND table_id = '" + table_tag + "' ORDER BY order_by";
            DataTable tableColumns = GetProgramManager().GetDataManager().GetTableData(sqlColumnSelect);
            if (tableColumns == null || tableColumns.Rows.Count == 0)
                return;

            //additonal parameters
            int i = 0, type;

            foreach (DataRow row in tableColumns.Rows)
            {
                type = int.Parse(row["type"].ToString());

                //main panel
                Panel tempPanel = new Panel();
                tempPanel.Size = new Size(100, 30);
                tempPanel.Dock = DockStyle.Top;
                panel.Controls.Add(tempPanel);
                tempPanel.BringToFront();

                //label Panel
                Panel labelPanel = new Panel();
                labelPanel.Size = new Size(p_width, 30);
                labelPanel.Dock = DockStyle.Left;
                tempPanel.Controls.Add(labelPanel);

                //label with name
                Label templblName = new Label();
                templblName.AutoSize = true;
                templblName.Text = string.Format("{0} :", row["full_name"].ToString());
                templblName.Dock = DockStyle.Right;
                labelPanel.Controls.Add(templblName);

                //panel for control
                Panel controlPanel = new Panel();
                controlPanel.Dock = DockStyle.Left;
                controlPanel.Size = new Size(c_width, 30);
                controlPanel.Padding = new Padding(5, 0, 0, 20);
                controlPanel.Tag = row["db_name"].ToString();
                tempPanel.Controls.Add(controlPanel);
                controlPanel.BringToFront();

                if (type == 0)
                {
                    TextBox tempTxtValue = new TextBox();
                    tempTxtValue.Name = "txtAdditional" + i;
                    tempTxtValue.TabIndex = i;
                    tempTxtValue.Dock = DockStyle.Fill;
                    controlPanel.Controls.Add(tempTxtValue);
                }
                else if (type == 1)
                {
                    LoockupComboBox tempTxtValue = new LoockupComboBox();
                    tempTxtValue.Name = "comboAdditional" + i;
                    tempTxtValue.Dock = DockStyle.Fill;

                    string sql = "SELECT name FROM book.CustomParams WHERE column_id=@ID ORDER BY name";
                    Hashtable m_sqlparams = new Hashtable();
                    m_sqlparams.Clear();
                    m_sqlparams.Add("@ID", row["id"].ToString());
                    DataTable paramsTable = GetProgramManager().GetDataManager().GetTableData(sql, m_sqlparams);
                    if (paramsTable != null)
                        for (int j = 0; j < paramsTable.Rows.Count; j++)
                        {
                            tempTxtValue.Items.Add(paramsTable.Rows[j]["name"].ToString());
                        }

                    tempTxtValue.Tag = row["id"].ToString();
                    controlPanel.Controls.Add(tempTxtValue);
                }
                else if (type == 2)//group params
                {
                    ComboBox tempTxtValue = new ComboBox();
                    tempTxtValue.Name = "groupAdditional" + i;
                    tempTxtValue.Dock = DockStyle.Fill;
                    tempTxtValue.DropDownStyle = ComboBoxStyle.DropDownList;
                    string sql = "SELECT id, path, name FROM book.GroupProducts WHERE path LIKE '0#1#10#11#%'" + " ORDER BY path";

                    DataTable paramsTable = GetProgramManager().GetDataManager().GetTableData(sql);
                    int sel_index = -1;
                    string res_id = "";

                    if (paramsTable != null)
                        for (int j = 0; j < paramsTable.Rows.Count; j++)
                        {
                            string r_id = paramsTable.Rows[j]["id"].ToString();
                            if (r_id == res_id)
                                sel_index = j;
                            string[] seps = paramsTable.Rows[j]["path"].ToString().Split('#');
                            int len = seps.Length;
                            string nm = paramsTable.Rows[j]["name"].ToString();
                            for (int k = 5; k < seps.Length; k++)
                                nm = "  " + nm;
                            tempTxtValue.Items.Add(nm);
                        }
                    if (sel_index >= 0)
                        tempTxtValue.SelectedIndex = sel_index;

                    tempTxtValue.Tag = "###";
                    controlPanel.Controls.Add(tempTxtValue);
                }

                i++;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            onSearch();
        }

        private void onSearch()
        {
            if (comboDocs.SelectedIndex == -1)
            {
                MessageBox.Show("აირჩიეთ ოპერაცია!", Application.ProductName, System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
                m_Grid.Columns["ColVendorName"].Visible = comboDocs.SelectedIndex == 0 || comboDocs.SelectedIndex == 2;

            int doc_type = this.mDocs[comboDocs.SelectedItem.ToString()];
            string inner_sql = " ";
            if (doc_type == 21)
                inner_sql = "INNER JOIN doc.ProductOut AS po ON po.general_id=gd.id AND po.staff_id=(CASE WHEN @staff=0 THEN po.staff_id ELSE @staff END)";
            else if (doc_type == 9)
                inner_sql = "INNER JOIN doc.CustomerReturns AS cr ON cr.general_id=gd.id AND cr.staff_id=(CASE WHEN @staff=0 THEN cr.staff_id ELSE @staff END)";

            string additional_params = "";
            foreach (Control ctrl in panelContragentColumns.Controls)
            {
                if (ctrl.Controls[0].Controls[0].Text != "")
                    additional_params += " AND c." + ctrl.Controls[0].Tag + " LIKE '%" + ctrl.Controls[0].Controls[0].Text + "%'";
            }
            foreach (Control ctrl in panelProductColumns.Controls)
            {
                if (ctrl.Controls[0].Controls[0].Text != "")
                    additional_params += " AND p." + ctrl.Controls[0].Tag + " LIKE '%" + ctrl.Controls[0].Controls[0].Text + "%'";
            }

            string sql = @"SELECT gd.id,gd.tdate,gd.doc_type,
       (CASE WHEN gd.waybill_num IS NULL THEN gd.doc_num_prefix+ CAST(gd.doc_num AS VARCHAR(20)) ELSE gd.waybill_num END) AS doc_num,
       c.name AS contragent_name,
       p.name AS product_name,
       p.code AS product_code,
       ps.sub_column_1  AS vendor_name,
       pf.amount,
       pf.price,
       (pf.amount*pf.price*gd.rate) AS summ
       FROM doc.GeneralDocs AS gd
INNER JOIN book.Contragents AS c ON c.id= gd.param_id1
INNER JOIN doc.ProductsFlow AS pf ON pf.general_id=gd.id AND pf.visible=1
INNER JOIN book.Products AS p ON p.id=pf.product_id
INNER JOIN book.ProductSubCodes AS ps ON ps.id=pf.sub_id
" + inner_sql + @"
WHERE gd.tdate BETWEEN @date1 AND @date2 AND gd.doc_type=@doc_type AND gd.param_id2 = (CASE WHEN @store_id=0 THEN gd.param_id2 ELSE @store_id END) AND 
      gd.param_id1 = (CASE WHEN @contragent_id=0 THEN gd.param_id1 ELSE @contragent_id END) AND
      pf.product_id = (CASE WHEN @product_id=0 THEN pf.product_id ELSE @product_id END) AND
      ps.vendor_id = (CASE WHEN @vendor_id=0 THEN ps.vendor_id ELSE @vendor_id END) AND
      gd.doc_num_prefix+ISNULL(gd.waybill_num,'')+CAST(gd.doc_num AS VARCHAR(20)) LIKE '%'+@doc_num+''" + additional_params;

            DataTable res_data = GetProgramManager().GetDataManager().GetTableData(sql, new Hashtable() 
            {
             {"@doc_type",doc_type},
             {"@store_id",comboStore.SelectedValue},
             {"@contragent_id",comboContragentName.SelectedValue},
             {"@product_id",comboProductName.SelectedValue},
             {"@staff",comboStaff.SelectedValue},
             {"@doc_num",txtNum.Text},
             {"@date1",m_Picker.dtp_From.Value},
             {"@date2",m_Picker.dtp_To.Value},
             {"@vendor_id", (comboVendorName.SelectedValue == null ? 0 : Convert.ToInt32(comboVendorName.SelectedValue))}
            });

            m_Grid.Rows.Clear();
            int index = 0;
            double amount_summ = 0;
            double summ = 0;
            foreach (DataRow row in res_data.Rows)
            {
                index = m_Grid.Rows.Add();
                m_Grid.Rows[index].Cells["ColId"].Value = row["id"];
                m_Grid.Rows[index].Cells["ColDate"].Value = row["tdate"];
                m_Grid.Rows[index].Cells["ColDoc"].Value = row["doc_type"];
                m_Grid.Rows[index].Cells["ColNum"].Value = row["doc_num"];
                m_Grid.Rows[index].Cells["ColContragent"].Value = row["contragent_name"];
                m_Grid.Rows[index].Cells["ColName"].Value = row["product_name"];
                m_Grid.Rows[index].Cells["ColVendorName"].Value = row["vendor_name"];
                m_Grid.Rows[index].Cells["ColQuantity"].Value = row["amount"];
                m_Grid.Rows[index].Cells["ColAmount"].Value = row["price"];
                m_Grid.Rows[index].Cells["ColSum"].Value = row["summ"];

                amount_summ += Convert.ToDouble(row["amount"]);
                summ += Convert.ToDouble(row["summ"]);
            }
            panelBottom.Visible = true;
            lblItemsCount.Text = res_data.Rows.Count.ToString();
            lblCount.Text = amount_summ.ToString();
            lblFullSumm.Text = summ.ToString();
        }

        private void m_Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string execute_tag = GetProgramManager().GetDataManager().GetStringValue("SELECT 'SHOW_'+tag FROM config.DocTypes WHERE id='" + m_Grid.SelectedRows[0].Cells["ColDoc"].Value + "'") + ":" + m_Grid.SelectedRows[0].Cells["ColId"].Value;
            GetProgramManager().ExecuteCommandByTag(execute_tag);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void btnReview_Click(object sender, EventArgs e)
        {
            PreviewToExcel();
        }

        private void PreviewToExcel()
        {
            if (m_Grid.RowCount == 0)
                return;
            string destPath = Path.GetTempFileName().Replace(".tmp", ".xls");
         //   Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Excel.Application oExcel = new Excel.Application();
            oExcel.Application.Workbooks.Add(true);
            oExcel.Visible = true;
            Excel.Workbooks oBooks = oExcel.Workbooks;
            Excel.Workbook oBook = oBooks.get_Item(1);
            Excel.Sheets oSheets = oBook.Worksheets;
            Excel.Worksheet oSheet = (Excel.Worksheet)oSheets.get_Item(1);


            //Excel.Range cells = (Excel.Range)oSheet.Cells;
            //cells.NumberFormat = "@";
            int x = 1, y = 1;
            for (int i = 0; i <= m_Grid.RowCount - 1; i++)
            {
                x = 1;
                for (int j = 0; j <= m_Grid.ColumnCount - 1; j++)
                {
                    if (!m_Grid.Columns[j].Visible) continue;

                    DataGridViewCell cell = m_Grid[j, i];
                    if (i == 0)
                    {
                        oExcel.Cells[1, x] = m_Grid.Columns[j].HeaderText.ToString();

                    }
                    oExcel.Cells[y + 1, x] = cell.Value;
                    x++;

                }
                y++;
            }
            oExcel.Columns.AutoFit();

            ReleaseObject(oSheet);
            ReleaseObject(oBook);
            ReleaseObject(oExcel);
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void ExportToExcel()
        {
            if (m_Grid.RowCount == 0)
                return;
            string destPath = string.Empty;
            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.FileName = "Export Data";
            savedialog.Filter = "ექსელის ფაილი (*.xls)|*.xls";
            if (savedialog.ShowDialog() == DialogResult.OK)
                if (savedialog.FileName == string.Empty)
                    return;
            destPath = savedialog.FileName;

        //    Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Excel.Application oExcel = new Excel.Application();
            oExcel.Application.Workbooks.Add(true);
            Excel.Workbooks oBooks = oExcel.Workbooks;
            Excel.Workbook oBook = oBooks.get_Item(1);
            Excel.Sheets oSheets = oBook.Worksheets;
            Excel.Worksheet oSheet = (Excel.Worksheet)oSheets.get_Item(1);


            //Excel.Range cells = (Excel.Range)oSheet.Cells;
            //cells.NumberFormat = "@";
            // exporting Loop
            int x = 1, y = 1;
            for (int i = 0; i <= m_Grid.RowCount - 1; i++)
            {
                x = 1;
                for (int j = 0; j <= m_Grid.ColumnCount - 1; j++)
                {
                    if (!m_Grid.Columns[j].Visible) continue;

                    DataGridViewCell cell = m_Grid[j, i];
                    if (i == 0)
                    {
                        oExcel.Cells[1, x] = m_Grid.Columns[j].HeaderText.ToString();

                    }
                    oExcel.Cells[y + 1, x] = cell.Value;
                    x++;

                }
                y++;
            }
            oExcel.Columns.AutoFit();

            oExcel.ActiveWorkbook.SaveCopyAs(destPath);
            oExcel.ActiveWorkbook.Saved = true;
            oExcel.Quit();

        }

        private void comboDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable contragent_data = null; 
            DataTable vendor_data = null; 

            int doc_type = this.mDocs[comboDocs.SelectedItem.ToString()];
            if (doc_type == 21 || doc_type == 9)
            {
                contragent_data = GetProgramManager().GetDataManager().GetTableData("SELECT 0 AS id,N'' AS name,N'' AS code UNION ALL SELECT id,name,code FROM book.Contragents WHERE path LIKE '0#2#5%' ORDER BY name");
                if (comboVendorName.DataSource == null)
                    vendor_data = GetProgramManager().GetDataManager().GetTableData("SELECT 0 AS id,N'' AS name,N'' AS code UNION ALL SELECT id,name,code FROM book.Contragents WHERE path LIKE '0#1#3%' ORDER BY name");
                groupBoxVendor.Visible = true;
            }
            else
            {
                contragent_data = GetProgramManager().GetDataManager().GetTableData("SELECT 0 AS id,N'' AS name,N'' AS code UNION ALL SELECT id,name,code FROM book.Contragents ORDER BY name");
                groupBoxVendor.Visible = false;
            }

            comboContragentName.DataSource = contragent_data;
            comboContragentName.ValueMember = "id";
            comboContragentName.DisplayMember = "name";

            if (comboVendorName.DataSource == null)
            {
                comboVendorName.DataSource = vendor_data;
                comboVendorName.ValueMember = "id";
                comboVendorName.DisplayMember = "name";
            }
        }

    }
}
