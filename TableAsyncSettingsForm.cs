using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using ipmControls;
using System.Drawing;
using System.Transactions;

namespace ipmExtraFunctions
{
    public partial class TableAsyncSettingsForm : Form
    {
        ProgramManagerBasic ProgramManager;
        DataTable GridData;
        Dictionary<int, string> BookSource;

        public TableAsyncSettingsForm(ProgramManagerBasic PM)
        {
            InitializeComponent();
            ProgramManager = PM;
            FillBooksCombo();
            grdData.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return ProgramManager;
        }

        private void FillBooksCombo()
        {
            BookSource = new Dictionary<int, string>()
            {
                { 0, "მყიდველები" },
                { 1, "მომწოდებლები" },
                { 2, "POS ტერმინალები" },
                { 3, "საკრედიტო ორგანიზაციები" },
                { 4, "ვალუტები" },
                { 5, "მომხმარებლები" },
                { 6, "პროქტები" },
                { 7, "თანამშრომლები" },
                { 8, "საწყობები" },
                { 9, "სალაროები" },
                { 10, "სასაქონლო მატერიალური მარაგები" },
                //{ 11, "ქვე კოდები" },
                { 12, "საზომი ერთეულები" },
                { 13, "ანალიზური კოდები" },
                { 14, "საბანკო ანგარიშები" },
                { 15, "ფასის ტიპები" },
                { 16, "კონტრაგენტის ხელშეკრულებები" },
                { 17, "მიღებული სესხები" },
                { 18, "გაცემული სესხები" },
                { 19, "თანამშრომლის დეპარტამენტები" },
                { 20, "თანამშრომლის თანამდებობები" },
                { 21, "სატრანსპორტო საშუალებები" },
                { 22, "კაფე - მაგიდები" },
                { 23, "კაფე - დარბაზები" },
                { 24, "კაფე - გაუქმების მიზეზები" },
                { 25, "პარტნიორები" }

            };

            comboBoxTables.DataSource = new BindingSource(BookSource, null);
            comboBoxTables.ValueMember = "Key";
            comboBoxTables.DisplayMember = "Value";
        }

        private void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            grdData.Rows.Clear();

            int selectedBookVal = (int)comboBoxTables.SelectedValue;
            string sql = string.Empty;
            bool codeVisible = true;

            switch (selectedBookVal)
            {
                case 0:
                    {
                        sql = "SELECT id, name, code, sync_id, 'book.contragents' AS table_name FROM book.contragents WHERE $where (path = '0#2#5' OR path LIKE '0#2#5#%')";
                        break;
                    }
                case 1:
                    {
                        sql = "SELECT id, name, code, sync_id, 'book.contragents' AS table_name FROM book.contragents WHERE $where (path = '0#1#3' OR path LIKE '0#1#3#%')";
                        break;
                    }
                case 2:
                    {
                        sql = "SELECT id, name, code, sync_id, 'book.contragents' AS table_name FROM book.contragents WHERE $where (path = '0#12#13' OR path LIKE '0#12#13#%')";
                        break;
                    }
                case 3:
                    {
                        sql = "SELECT id, name, code, sync_id, 'book.contragents' AS table_name FROM book.contragents WHERE $where (path = '0#14#15' OR path LIKE '0#14#15#%')";
                        break;
                    }
                case 4:
                    {
                        sql = "SELECT id, name, code, sync_id, 'book.Currencies' AS table_name FROM book.Currencies WHERE $where 1=1";
                        break;
                    }
                case 5:
                    {
                        sql = "SELECT id, name, sync_id, 'book.Users' AS table_name FROM book.Users WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }
                case 6:
                    {
                        sql = "SELECT id, name, sync_id, 'book.Projects' AS table_name FROM book.Projects WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }
                case 7:
                    {
                        sql = "SELECT id, name, code, sync_id, 'book.Staff' AS table_name FROM book.Staff WHERE $where 1=1";
                        break;
                    }
                case 8:
                    {
                        sql = "SELECT id, name, sync_id, 'book.Stores' AS table_name FROM book.Stores WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }
                case 9:
                    {
                        sql = "SELECT id, name, sync_id, 'book.Cashes' AS table_name FROM book.Cashes WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }
                case 10:
                    {
                        sql = "SELECT id, name, code, sync_id, 'book.Products' AS table_name FROM book.Products WHERE $where 1=1";
                        break;
                    }
                /*case 11:
                    {
                        sql = "SELECT id, name, code, sync_id, 'book.ProductSubCodes' AS table_name FROM book.ProductSubCodes WHERE $where 1=1";
                        break;
                    }*/
                case 12:
                    {
                        sql = "SELECT id, name, sync_id, 'book.Units' AS table_name FROM book.Units WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }
                case 13:
                    {
                        sql = "SELECT id, name, sync_id, 'book.AnalyticCodes' AS table_name FROM book.AnalyticCodes WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }
                case 14:
                    {
                        sql = "SELECT id, name, sync_id, 'book.CompanyAccounts' AS table_name FROM book.CompanyAccounts WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }
                case 15:
                    {
                        sql = "SELECT id, name, sync_id, 'book.Prices' AS table_name FROM book.Prices WHERE $where id > 2";
                        codeVisible = false;
                        break;
                    }
                case 16:
                    {
                        sql = "SELECT id, name, sync_id, 'book.ContragentAgreements' AS table_name FROM book.ContragentAgreements WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }
                case 17:
                    {
                        sql = "SELECT id, name, sync_id, 'book.Credits' AS table_name FROM book.Credits WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }
                case 18:
                    {
                        sql = "SELECT id, name, sync_id, 'book.OutCredits' AS table_name FROM book.OutCredits WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }
                case 19:
                    {
                        sql = "SELECT id, name, sync_id, 'book.GroupStaffDepartments' AS table_name FROM book.GroupStaffDepartments WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }

                case 20:
                    {
                        sql = "SELECT id, position AS name, sync_id, 'book.StaffPositions' AS table_name FROM book.StaffPositions WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }

                case 21:
                    {
                        sql = "SELECT id, num AS name, sync_id, 'book.Cars' AS table_name FROM book.Cars WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }

                case 22:
                    {
                        sql = "SELECT id, num AS name, sync_id, 'book.cafetables' AS table_name FROM book.cafetables WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }

                case 23:
                    {
                        sql = "SELECT id, name, sync_id, 'book.CafeRooms' AS table_name FROM book.CafeRooms WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }

                case 24:
                    {
                        sql = "SELECT id, name, sync_id, 'book.CafeComments' AS table_name FROM book.CafeComments WHERE $where 1=1";
                        codeVisible = false;
                        break;
                    }

                case 25:
                    {
                        sql = "SELECT id, name, code, sync_id, 'book.contragents' AS table_name FROM book.contragents WHERE $where (path = '0#8#9' OR path LIKE '0#8#9#%')";
                        break;
                    }

                default:
                    break;
            }

            sql = sql.Replace("$where", checkAllData.Checked ? "" : "sync_id IS NULL AND");
            GridData = GetProgramManager().GetDataManager().GetTableData(sql);
            this.FillGrid();
            grdData.Columns["col_code"].Visible = codeVisible;
        }

        private void grdData_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string col = grdData.Columns[e.ColumnIndex].Name;

            if (!col.Equals("col_sync_id")) return;

            string val = e.FormattedValue.ToString();

            int selSyncId = 0;

            if ((!string.IsNullOrEmpty(val) && !int.TryParse(val, out selSyncId)) || selSyncId < 0)
            {
                MessageBoxForm.Show(Application.ProductName, "იდენტიფიკატორი არასწორია", null, null, SystemIcons.Error);
                grdData.Rows[e.RowIndex].ErrorText = "იდენტიფიკატორი არასწორია";
                e.Cancel = true;

                return;
            }

            if (!string.IsNullOrEmpty(val))
            {
                DataGridViewRow selRow = grdData.Rows[e.RowIndex];

                foreach (DataGridViewRow row in grdData.Rows)
                {
                    int tmpSyncId;

                    if ((int.TryParse(Convert.ToString(row.Cells["col_sync_id"].Value), out tmpSyncId)
                        && selSyncId == tmpSyncId)
                        && row.Cells["col_id"].Value.ToString() != selRow.Cells["col_id"].Value.ToString())
                    {
                        MessageBoxForm.Show(Application.ProductName, "იდენტიფიკატორი უკვე არსებობს", null, null, SystemIcons.Error);
                        grdData.Rows[e.RowIndex].ErrorText = "იდენტიფიკატორი უკვე არსებობს";
                        e.Cancel = true;
                        break;
                    }
                }
            }
        }

        private void FillGrid()
        {
            grdData.Rows.Clear();

            foreach (DataRow item in GridData.Rows)
            {
                string code = string.Empty;
                if (GridData.Columns.Contains("code"))
                {
                    code = item["code"].ToString();
                }
                grdData.Rows.Add(item["id"], item["name"], code, item["sync_id"], item["table_name"]);
            }
        }

        private void btnAutoGenerate_Click(object sender, EventArgs e)
        {
            if (grdData.Rows.Count == 0)
                return;
            string table_name = string.Empty;
            switch ((int)comboBoxTables.SelectedValue)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 25:
                    table_name = "book.Contragents";
                    break;
                case 4:
                    table_name = "book.Currencies";
                    break;
                case 5:
                    table_name = "book.Users";
                    break;
                case 6:
                    table_name = "book.Projects";
                    break;
                case 7:
                    table_name = "book.Staff";
                    break;
                case 8:
                    table_name = "book.Stores";
                    break;
                case 9:
                    table_name = "book.Cashes";
                    break;
                case 10:
                    table_name = "book.Products";
                    break;
                case 12:
                    table_name = "book.Units";
                    break;
                case 13:
                    table_name = "book.AnalyticCodes";
                    break;
                case 14:
                    table_name = "book.CompanyAccounts";
                    break;
                case 15:
                    table_name = "book.Prices";
                    break;
                case 16:
                    table_name = "book.ContragentAgreements";
                    break;
                case 17:
                    table_name = "book.Credits";
                    break;
                case 18:
                    table_name = "book.OutCredits";
                    break;
                case 19:
                    table_name = "book.GroupStaffDepartments";
                    break;
                case 20:
                    table_name = "book.StaffPositions";
                    break;
                case 21:
                    table_name = "book.Cars";
                    break;
                case 22:
                    table_name = "book.cafetables";
                    break;
                case 23:
                    table_name = "book.CafeRooms";
                    break;
                case 24:
                    table_name = "book.CafeComments";
                    break;
                default:
                    break;
            }
           
            int maxSyncId = GetProgramManager().GetDataManager().GetScalar<int>(string.Format("SELECT ISNULL(MAX(sync_id),0) FROM {0}", table_name)).Value;
            for (int i = 0; i < grdData.Rows.Count; i++)
            {
                maxSyncId++;
                GridData.Rows[i]["sync_id"] = maxSyncId;
            }

            this.FillGrid();
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            grdData.EndEdit();
            if (grdData.Rows.Count == 0)
                return;

            ProgressDispatcher.Activate();

            List<int> sameIds = new List<int>();
            string table_name = string.Empty;
            Dictionary<int, string> ids = new Dictionary<int, string>();

            foreach (DataGridViewRow row in grdData.Rows)
            {
                int id = Convert.ToInt32(row.Cells["col_id"].Value);
                table_name = row.Cells["col_table_name"].Value.ToString();

                if (string.IsNullOrEmpty(Convert.ToString(row.Cells["col_sync_id"].Value)))
                {
                    ids[id] = "NULL";
                    continue;
                }
                int sync_id = Convert.ToInt32(row.Cells["col_sync_id"].Value);
                bool exist = GetProgramManager().GetDataManager().GetScalar<bool>(string.Format("IF EXISTS (SELECT id FROM {0} WHERE sync_id = {1} AND id != {2}) SELECT 'True' ELSE SELECT 'False'", table_name, sync_id, id)).Value;
                if (exist)
                    sameIds.Add(sync_id);
                else
                    ids[id] = sync_id.ToString();
            }

            if (sameIds.Count > 0)
            {
                ProgressDispatcher.Deactivate();
                MessageBoxForm.Show(Application.ProductName, "იდენტიფიკატორი უკვე არსებობს", string.Join("; ", sameIds.Select(s => s.ToString()).ToArray()), null, SystemIcons.Error);
                return;
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 20, 0) }))
            {
                foreach (int id in ids.Keys)
                {
                    string sql = string.Format("UPDATE {0} SET sync_id = {1} WHERE id = {2}", table_name, ids[id], id);
                    bool result = GetProgramManager().GetDataManager().ExecuteSql(sql);
                    if (!result)
                    {
                        Transaction.Current.Rollback();
                        ProgressDispatcher.Deactivate();
                        MessageBoxForm.Show(Application.ProductName, "დაფიქსირდა შეცდომა, გთხოვთ სცადეთ თავიდან", GetProgramManager().GetDataManager().ErrorEx, null, SystemIcons.Error);
                        return;
                    }
                }

                scope.Complete();
            }

            ProgressDispatcher.Deactivate();
            MessageBoxForm.Show(Application.ProductName, "წარმატებით შეინახა", null, null, SystemIcons.Information);
        }

        private void toolStripExport_Click(object sender, EventArgs e)
        {
            if (grdData.Rows.Count == 0)
                return;
            using (SaveFileDialog savedialog = new SaveFileDialog() { FileName = "Export Data", Filter = "ექსელის ფაილი" + " (*.xlsx)|*.xlsx" })
            {
                if (savedialog.ShowDialog() != DialogResult.OK)
                    return;
                ProgressDispatcher.Activate();
                new ExcelManager(GetProgramManager(), grdData, string.Empty).OnExcelExportFast(savedialog.FileName);
                ProgressDispatcher.Deactivate();
            }
        }
    }
}
