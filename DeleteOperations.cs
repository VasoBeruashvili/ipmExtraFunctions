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
    public partial class DeleteOperations : Form
    {
        Hashtable sql_params = new Hashtable();
        ProgramManagerBasic M_Pm;


        private void onLoad()
        {
            M_Pm.TranslateControl(this);

            m_Picker.dtp_From.Value = DateTime.Now;
            m_Picker.dtp_To.Value = DateTime.Now;

            M_Pm.FillComboData(comboStore, "TABLE_STORE", true);
            comboMethod.DataSource = new BindingSource { DataSource = new Dictionary<byte, string> { { 1, "თვითღირებულებით" }, { 2, "შესყიდვის ფასით"} } };
            comboMethod.DisplayMember = "Value";
            comboMethod.ValueMember = "Key";
        }
        public DeleteOperations(ProgramManagerBasic pm)
        {
            this.M_Pm = pm;
            InitializeComponent();
            onLoad();
        }
       
        private void btnDel_Click(object sender, EventArgs e)
        {
            if(OnDelete())
                this.Close();
        }

        private void OnRestExport()
        {
            int store_id = 0;
            if (!int.TryParse(Convert.ToString(comboStore.SelectedValue), out store_id))
                return;
            List<int> stores = new List<int>();
            if (store_id != 0)
                stores.Add(store_id);
            else
            stores.AddRange((((DataTable)comboStore.DataSource).AsEnumerable().Where(a => a.Field<int>("id") != 0).Select(a => a.Field<int>("id"))).ToList<int>());
            FolderBrowserDialog dlg = new FolderBrowserDialog()
            {
                Description = "ნაშთების ექსპორტი",
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                ShowNewFolderButton = true
            };
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            DataTable final_table=new DataTable();
            ExcelManager excel = new ExcelManager(M_Pm, final_table, string.Empty);
            string file_path = dlg.SelectedPath;
            ProgressDispatcher.Activate();
            //int price_id = 1;
            //string path = "0#1#10";
            string dateString = txtDate.Value.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            int i = 0;
            foreach(int id in stores)
            {
                string sql = string.Format("EXEC {0} @date = '{1}',  @store_id = {2}", (byte)comboMethod.SelectedValue == 1 ? "doc.prc_vwr_ProductFinalRestExport" : "doc.prc_vwr_ProductFinalRestExport_In", dateString, id);
                DataTable data = M_Pm.GetDataManager().GetTableData(sql);
                if (data == null || data.Rows.Count <= 0)
                    continue;
                final_table = new DataTable();
                final_table.Columns.Add("კოდი", typeof(string));
                final_table.Columns.Add("რაოდენობა", typeof(double));
                final_table.Columns.Add("ფასი", typeof(double));
               
                foreach (DataRow row in data.Rows)
                    final_table.Rows.Add(row["code"], row["rest"], row["self_cost"]);

                excel.DataTable = final_table;
                string final_path = System.IO.Path.Combine(file_path, "Rest - " + M_Pm.GetDataManager().GetStoreNameByID(id) + " - " + txtDate.Value.AddSeconds(++i).ToString("yyyy-MM-dd HH-mm-ss"));
                excel.OnExcelExportFast(final_path);

 
            }





           // SaveFileDialog savedialog = new SaveFileDialog();
           // savedialog.FileName = "Rest - " + M_Pm.GetDataManager().GetStoreNameByID(Convert.ToInt32(comboStore.SelectedValue)) + " - " + txtDate.Value.ToString("yyyy-MM-dd HH-mm-ss");
           // savedialog.Filter = "ექსელის ფაილი (*.xls)|*.xls";
           // if (savedialog.ShowDialog() == DialogResult.OK)
           //     if (string.IsNullOrEmpty(savedialog.FileName))
           //         return;
           // //M_Pm.ShowWaitForm();
           // //int price_id = 1;
           // //string path = "0#1#10";
           // //string dateString = txtDate.Value.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
           //// string storeString = store_id == 0 ? "NULL" : store_id.ToString();

           // //string sql = string.Format("EXEC doc.prc_vwr_ProductFinalRestExport @date = '{0}', " +
           // //   " @store_id = {1}, " +
           // //   " @group_path = '{2}', " +
           // //   " @price_id = '{3}', " +
           // //   " @vendor_id = {4}, " +
           // //   " @customer_id = {5}, " +
           // //   " @excludeZeros = {6}  " +
           // //   " ", dateString, store_id.ToString(), path, price_id.ToString(), 0,0, 1);

           // //DataTable data = M_Pm.GetDataManager().GetTableData(sql);
           // //if (data == null || data.Rows.Count <= 0)
           // //{
           // //    M_Pm.HideWaitForm();
           // //    return;
           // //}
           // DataTable final_table = new DataTable();
           // final_table.Columns.Add("კოდი", typeof(string));
           // final_table.Columns.Add("რაოდენობა", typeof(double));
           // final_table.Columns.Add("ფასი", typeof(double));

         
           // double final_quantity = 0.0;
           // foreach (DataRow row in data.Rows)
           // {
           //     double sum = Convert.ToDouble(row["final_price"]);
           //     double coeff = Convert.ToDouble(row["coeff"]);
           //     if (coeff != 0)
           //         final_quantity = Convert.ToDouble(row["final_quantity"]) / coeff;
           //     //if (final_quantity <= 0)
           //     //    continue;
           //     DataRow new_row = final_table.NewRow();
           //     new_row["კოდი"] = row["code"].ToString();
           //     new_row["რაოდენობა"] = final_quantity;
           //     new_row["ფასი"] = Math.Round(sum / final_quantity, 8);
           //     final_table.Rows.Add(new_row);
           // }

           // ExcelBasic.ExcelManager excel = new ExcelBasic.ExcelManager(M_Pm, final_table,string.Empty);
           // excel.OnExcelExportFast(savedialog.FileName);

            ProgressDispatcher.Deactivate();

        }

        private bool OnDelete()
        {
            if (txtPassword.Text != "adminfina")
                return false;
            if (MessageBox.Show(M_Pm.GetTranslatorManager().Translate("გსურთ მონაცემების წაშლა?"), 
                M_Pm.GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                return true;

            sql_params.Clear();
            string date1 = m_Picker.dtp_From.Value.ToString("yyyy-MM-dd 00:00:00");
            string date2 = m_Picker.dtp_To.Value.ToString("yyyy-MM-dd 23:59:59");

         
                string sql = string.Empty;
                if(!checkAll.Checked)
                 sql = @"
                            begin transaction
                            DECLARE @TruncateStatement nvarchar(4000)
                            DECLARE @err INT 
                           IF OBJECT_ID('tempDB..#temp_Ids','U') IS NOT NULL
                            DROP TABLE #temp_Ids
                            CREATE TABLE #temp_Ids  (id int)
                            INSERT INTO #temp_Ids  SELECT  id FROM doc.Generaldocs WHERE  tdate BETWEEN '" + date1 + "' AND '" + date2 + @"'
                            DECLARE TruncateStatements CURSOR LOCAL FAST_FORWARD
                            FOR
                            SELECT
                             N'DELETE FROM ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)+ ' WITH (TABLOCK) WHERE general_id IN (SELECT  id FROM #temp_Ids)'
                            FROM INFORMATION_SCHEMA.COLUMNS
                            WHERE
                            COLUMN_NAME='general_id' AND OBJECTPROPERTY(OBJECT_ID(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)), 'IsMSShipped') = 0 order by TABLE_NAME
                            OPEN TruncateStatements
                            WHILE 1=1
                            BEGIN
                                FETCH NEXT FROM TruncateStatements INTO @TruncateStatement
                                IF @@FETCH_STATUS <> 0 BREAK
                               --PRINT (@TruncateStatement)
                               EXEC sp_executesql @TruncateStatement
                                SET @err=@@ERROR
                              IF @err != 0 
                             BEGIN
                              ROLLBACK TRANSACTION 
                              RETURN 
                             END
                            END
                            CLOSE TruncateStatements
                            DELETE FROM doc.Generaldocs WITH (TABLOCK) WHERE   id IN  (SELECT  id FROM #temp_Ids) 
                            IF OBJECT_ID('tempDB..#temp_Ids','U') IS NOT NULL
                            DROP TABLE #temp_Ids
                            SET @err=@@ERROR
                              IF @err != 0 
                             BEGIN
                              ROLLBACK TRANSACTION 
                              RETURN 
                             END 
                            DEALLOCATE TruncateStatements
                            commit transaction";
                else
                    sql = @"
                            begin transaction
                            DECLARE @TruncateStatement nvarchar(4000)
                            DECLARE @err INT 
                            DECLARE TruncateStatements CURSOR LOCAL FAST_FORWARD
                            FOR
                            SELECT
                             N'TRUNCATE TABLE ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)
                            FROM INFORMATION_SCHEMA.COLUMNS
                            WHERE
                            COLUMN_NAME='general_id' AND OBJECTPROPERTY(OBJECT_ID(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)), 'IsMSShipped') = 0 order by TABLE_NAME

                            OPEN TruncateStatements
                            WHILE 1=1
                            BEGIN
                                FETCH NEXT FROM TruncateStatements INTO @TruncateStatement
                                IF @@FETCH_STATUS <> 0 BREAK
                               --PRINT (@TruncateStatement)
                               EXEC sp_executesql @TruncateStatement
                                SET @err=@@ERROR
                              IF @err != 0 
                             BEGIN
                              ROLLBACK TRANSACTION 
                              RETURN 
                             END
                            END
                            CLOSE TruncateStatements
                           -- DELETE FROM  doc.Generaldocs WITH (TABLOCK)
                           -- DBCC CHECKIDENT ('doc.Generaldocs',RESEED, 0)
                            TRUNCATE TABLE doc.Generaldocs  
                            SET @err=@@ERROR
                              IF @err != 0 
                             BEGIN
                              ROLLBACK TRANSACTION 
                              RETURN 
                             END 
                            DEALLOCATE TruncateStatements
                            commit transaction";
                if (!M_Pm.GetDataManager().ExecuteSql(sql))
                {
                    MessageBox.Show(M_Pm.GetTranslatorManager().Translate("წაშლის ოპერაცია ვერ შესრულდა!"));
                    return false;
                }



                //if (!M_Pm.GetDataManager().CorrectDataBase())
                //{
                //    MessageBox.Show(M_Pm.GetTranslatorManager().Translate("კორექციის ოპერაცია ვერ შესრულდა!"));
                //    return false;
                //}
                
            
            MessageBox.Show(M_Pm.GetTranslatorManager().Translate("წაშლის ოპერაცია წარმატებით დასრულდა!"));
            return true;
        }
       

        private void btnRest_Click(object sender, EventArgs e)
        {
            OnRestExport();
        }
    }
}
