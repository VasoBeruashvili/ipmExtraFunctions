using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Transactions;
using ipmPMBasic;
using ipmControls;

namespace ipmExtraFunctions
{
    public partial class GlobalOperationForm : Form
    {
        private ProgramManagerBasic ProgramManager { get; set; }
        public List<int> InfoIDs { get; set; } 

        

        public GlobalOperationForm(ProgramManagerBasic pm)
        {
            InitializeComponent();
            ProgramManager = pm;
             m_GridServices.ReadOnly = true;
             m_GridServices.MultiSelect = true;

             m_GridDeleteds.ReadOnly = true;
             m_GridDeleteds.MultiSelect = true;

             //m_Period.dtp_From.ValueChanged += new EventHandler(m_Date1_ValueChanged);
             //m_Period.dtp_To.ValueChanged += new EventHandler(m_Date1_ValueChanged);
             m_GridServices.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
             m_GridDeleteds.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
             if (!ProgramManager.IsUserAdmin())
             {
                 btnExecute.Visible = false;
                 checkAll.Visible = false;
             }

             SetupControls();

        }

       

        private void InitialData()
        {
            m_GridServices.Rows.Clear();
            string sql = @"SELECT DISTINCT  ROW_NUMBER() OVER (ORDER BY i.id) AS row_number, i.id, i.tdate,  i.contract_number, b.name AS brigade_name, i.contragent_GSN, i.pasport_number, i.name_kind, u.name+space(1)+u.surname AS operator
                         FROM book.GlobalInfo AS i
                         INNER JOIN book.GlobalProduction AS p ON i.id=p.info_id
                         INNER JOIN book.Staff AS b ON b.id=i.brigade_id
                         INNER JOIN book.Users AS u ON u.id=i.user_id    
                         WHERE i.status_id=0     
                         GROUP BY i.id, i.tdate,  i.contract_number,  b.name, i.contragent_GSN, i.pasport_number, i.name_kind, u.name+space(1)+u.surname";
            DataTable data = ProgramManager.GetDataManager().GetTableData(sql);
            foreach (DataRow row in data.Rows)
            {
                m_GridServices.Rows.Add(row["row_number"], row["id"], Convert.ToDateTime(row["tdate"]).ToString("yyyy/MM/dd HH:mm"), row["contract_number"],
                                        row["brigade_name"], row["name_kind"], row["contragent_GSN"], row["pasport_number"], row["operator"] );
            }


        }
        private void InitialDeletedData()
        {
            ProgressDispatcher.Activate();
            m_GridDeleteds.Rows.Clear();
            string sql = @"SELECT DISTINCT  ROW_NUMBER() OVER (ORDER BY i.id) AS row_number, i.id, i.tdate,  i.contract_number, b.name AS brigade_name, i.contragent_GSN, i.pasport_number, i.name_kind, u.name+space(1)+u.surname AS operator, CASE i.status_id WHEN 0 THEN N'აქტიური' WHEN 1 THEN N'შესრულებული' END AS status
                         FROM book.GlobalInfo AS i
                         INNER JOIN book.GlobalProduction AS p ON i.id=p.info_id
                         INNER JOIN book.Staff AS b ON b.id=i.brigade_id
                         INNER JOIN book.Users AS u ON u.id=i.user_id    
                         WHERE  i.tdate BETWEEN @date1 AND @date2    
                         GROUP BY i.id, i.tdate,  i.contract_number,  b.name, i.contragent_GSN, i.pasport_number, i.name_kind, u.name+space(1)+u.surname, i.status_id ";
            DataTable data = ProgramManager.GetDataManager().GetTableData(sql, new Hashtable() { { "@date1", m_Period.dtp_From.Value.ToString("yyyy-MM-dd 00:00:00") }, { "@date2", m_Period.dtp_To.Value.ToString("yyyy-MM-dd 23:59:59") } });
            foreach (DataRow row in data.Rows)
            {
                m_GridDeleteds.Rows.Add(row["row_number"], row["id"], Convert.ToDateTime(row["tdate"]).ToString("yyyy/MM/dd HH:mm"), row["contract_number"],
                                        row["brigade_name"], row["name_kind"], row["contragent_GSN"], row["pasport_number"], row["operator"], row["status"]);
            }

            ProgressDispatcher.Deactivate();
        }
        private void onDataExport()
        {
            if (m_GridServices.Rows.Count <= 0)
                return;
            ProgressDispatcher.Activate();
            try
            {
                SaveFileDialog svd = new SaveFileDialog();
                svd.Filter = "Excel 2003 Files.xls|*.xls|Excel 2007 Files.xlsx|*.xlsx";
                svd.FileName = "PLUData";
                if (svd.ShowDialog() == DialogResult.OK)
                {
                    Excel.Application xlApp;
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlWorkSheet;
                    object misValue = System.Reflection.Missing.Value;

                    xlApp = new Excel.ApplicationClass();
                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    xlWorkSheet.Cells[1, 1] = "თარიღი";
                    xlWorkSheet.Cells[1, 2] = "ხელშეკრულების ნომერი";
                    xlWorkSheet.Cells[1, 3] = "ბრიგადა";
                    xlWorkSheet.Cells[1, 4] = "სახელი, გვარი";
                    xlWorkSheet.Cells[1, 5] = "პირადი ნომერი";
                    xlWorkSheet.Cells[1, 6] = "პასპორტის ნომერი";
                    int i = 2;

                    if (checkAll.Checked)
                    {
                        foreach (DataGridViewRow row in m_GridServices.Rows)
                        {
                            Excel.Range rng = xlWorkSheet.get_Range(xlApp.Cells[i, 1], xlApp.Cells[i, 7]);
                            rng.NumberFormat = "@";
                            xlWorkSheet.Cells[i, 1] = row.Cells[col_tdate.Index].Value;
                            xlWorkSheet.Cells[i, 2] = row.Cells[col_Cnumber.Index].Value;
                            xlWorkSheet.Cells[i, 3] = row.Cells[col_brigaed_name.Index].Value;
                            xlWorkSheet.Cells[i, 4] = row.Cells[col_name.Index].Value;
                            xlWorkSheet.Cells[i, 5] = row.Cells[col_contragent_code.Index].Value;
                            xlWorkSheet.Cells[i, 6] = row.Cells[col_pasport_number.Index].Value;
                            i += 1;
                        }
                    }
                    else
                    {
                        foreach (DataGridViewRow row in m_GridServices.SelectedRows)
                        {
                            Excel.Range rng = xlWorkSheet.get_Range(xlApp.Cells[i, 1], xlApp.Cells[i, 7]);
                            rng.NumberFormat = "@";
                            xlWorkSheet.Cells[i, 1] = row.Cells[col_tdate.Index].Value;
                            xlWorkSheet.Cells[i, 2] = row.Cells[col_Cnumber.Index].Value;
                            xlWorkSheet.Cells[i, 3] = row.Cells[col_brigaed_name.Index].Value;
                            xlWorkSheet.Cells[i, 4] = row.Cells[col_name.Index].Value;
                            xlWorkSheet.Cells[i, 5] = row.Cells[col_contragent_code.Index].Value;
                            xlWorkSheet.Cells[i, 6] = row.Cells[col_pasport_number.Index].Value;
                            i += 1;

                        }
                    }

                    xlWorkBook.SaveAs(svd.FileName,
                    Excel.XlFileFormat.xlWorkbookNormal,
                        misValue,
                        misValue,
                        misValue,
                        misValue,
                        Excel.XlSaveAsAccessMode.xlExclusive,
                        misValue, misValue, misValue, misValue, misValue);
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();
                    ReleaseObject(xlWorkSheet);
                    ReleaseObject(xlWorkBook);
                    ReleaseObject(xlApp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ProgressDispatcher.Deactivate();
            }

        }
        private void onExecute()
        {
            if (checkAll.Checked)
                InfoIDs = m_GridServices.Rows.OfType<DataGridViewRow>().Select(a => Convert.ToInt32(a.Cells[col_id.Index].Value)).ToList();
            else
                InfoIDs = m_GridServices.SelectedRows.OfType<DataGridViewRow>().Select(a => Convert.ToInt32(a.Cells[col_id.Index].Value)).ToList();
            if (InfoIDs == null || InfoIDs.Count <= 0)
                return;
            DialogResult = DialogResult.OK;
           
        }
        private void fillComboFilter()
        {
            comboFilter.Items.Clear();
            comboFilter.Items.AddRange(m_GridDeleteds.Columns.OfType<DataGridViewColumn>()
                .Where(c => c.Visible)
                .Select(c => c.HeaderText)
                .ToArray<string>());
            comboFilter.SelectedIndex = 0;
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
       
        private void onExcelExport()
        {
            SaveFileDialog flDialog = new SaveFileDialog();
            flDialog.Title = "Excel ში ექსპორტი";
            flDialog.Filter = "Excel Files (*.xls)|*.xls";
            flDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (flDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelManager Excel = new ExcelManager(ProgramManager, tabGlobal.SelectedTab == tabPageDelete? m_GridDeleteds : m_GridServices, string.Empty);
                Excel.OnExcelExportFast(flDialog.FileName);
            }
        }
     
        private void btnExport_Click(object sender, EventArgs e)
        {
            onExcelExport();
        }
        private void SetupControls()
        {
            if (tabGlobal.SelectedTab == tabPageDelete)
            {
                checkAll.Visible = false;
                btnDelete.Visible = false;
                if (ProgramManager.IsUserAdmin())
                {
                    checkAll.Visible = true;
                    btnDelete.Visible = true;
                }
                panelPeriod.Visible = true;
                btnLoad.Visible = false;
                fillComboFilter();
                txtFilter.Focus();
            }
            else
            {
                panelPeriod.Visible = false;

                btnLoad.Visible = false;
                checkAll.Visible = false;
                btnDelete.Visible = false;
                if (ProgramManager.IsUserAdmin())
                {
                    btnLoad.Visible = true;
                    checkAll.Visible = true;
                    btnDelete.Visible = true;
                }
                InitialData();
            }
        }

        private void tabGlobal_SelectedIndexChanged(object sender, EventArgs e)
        {

            SetupControls();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            onExecute();
        }
        private void onDelete()
        {
            //if (!ProgramManager.CheckWithLogin())
            //    return;
            if (MessageBox.Show("წაიშალოს მონიშნული ოპერაცია? ", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            List<string> ids = new List<string>();
            if (tabGlobal.SelectedTab == tabPageDelete)
            {
                if (checkAll.Checked)
                    ids = m_GridDeleteds.Rows.OfType<DataGridViewRow>().Select(a => Convert.ToString(a.Cells[Dcol_id.Index].Value)).ToList<string>();
                else
                    ids = m_GridDeleteds.SelectedRows.OfType<DataGridViewRow>().Select(a => Convert.ToString(a.Cells[Dcol_id.Index].Value)).ToList<string>();
            }
            else if (tabGlobal.SelectedTab == tabPageLoad)
            {
                if (checkAll.Checked)
                    ids = m_GridServices.Rows.OfType<DataGridViewRow>().Select(a => Convert.ToString(a.Cells[col_id.Index].Value)).ToList<string>();
                else
                    ids = m_GridServices.SelectedRows.OfType<DataGridViewRow>().Select(a => Convert.ToString(a.Cells[col_id.Index].Value)).ToList<string>();
            }
              
            if (ids == null || ids.Count <= 0)
                return;
            string id_string = string.Join(",", ids.ToArray());
            ProgramManager.GetDataManager().Close();
            using (TransactionScope tran = new TransactionScope(new TransactionScopeOption(), new TimeSpan(0, 3, 0)))
            {
                ProgramManager.GetDataManager().Open();
                if (!ProgramManager.GetDataManager().ExecuteSql("DELETE FROM book.GlobalInfo WHERE id IN(" + id_string + ")"))
                {
                    Transaction.Current.Rollback();
                    MessageBox.Show("წაშლის ოპერაცია ვერ შესრულდა! ", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!ProgramManager.GetDataManager().ExecuteSql("DELETE FROM book.GlobalProduction WHERE info_id IN(" + id_string + ")"))
                {
                    Transaction.Current.Rollback();
                    MessageBox.Show("წაშლის ოპერაცია ვერ შესრულდა (დონე 2)! ", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                tran.Complete();
                MessageBox.Show("წაშლის ოპერაცია წარმატებით შესრულდა ", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (tabGlobal.SelectedTab == tabPageDelete)
                InitialDeletedData();
            else if (tabGlobal.SelectedTab == tabPageLoad)
                InitialData();


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            onDelete();
        }
        private void onFilter()
        {
            string letter = txtFilter.Text.Trim();
            string header = comboFilter.SelectedItem.ToString();
            foreach (DataGridViewRow row in m_GridDeleteds.Rows)
            {
                row.Visible =
                (!row.Cells.Cast<DataGridViewCell>().Where(c => c.OwningColumn.HeaderText.ToString() == header).Where(c => c.Value.ToString().ToLower().Contains(letter.ToLower())).Any()) ?
                row.Visible = false : true;
            }

        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            onFilter();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            InitialDeletedData();
        }

   
        


  

    }
}
