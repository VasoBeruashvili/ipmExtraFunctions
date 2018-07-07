using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ipmPMBasic;
using System.Transactions;
using System.Collections;
using ipmControls;
namespace ipmExtraFunctions
{
    public partial class UniPayForm : Form
    {
        ProgramManagerBasic pm;
        Hashtable m_sqlParams = new Hashtable();
        public UniPayForm(ProgramManagerBasic pm)
        {
            InitializeComponent();
            this.pm = pm;
            btnContragents.Tag = "0";
        }

        private bool CheckParams()
        {
            if (comboContragent == null || comboContragent.Items.Count <= 0)
                return false;
            int cont_id = -1;
           
            if (!int.TryParse(Convert.ToString(btnContragents.Tag), out cont_id) || cont_id <= 0)
              return false;
           return true;
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return pm;
        }
        private void FillComboContragent()
        {
            string sql = @"select c.id, c.name from book.contragents as c INNER JOIN book.groupcontragents as gc on gc.id=c.group_id
                           WHERE gc.account='3190'";
            Dictionary<int, string> acc = GetProgramManager().GetDataManager().GetMultiIDAndNameValue(sql);
            if(acc==null||acc.Count<=0)
                return;
            comboContragent.DataSource = new BindingSource(acc, null);
            comboContragent.DisplayMember = "Value";
            comboContragent.ValueMember = "Key"; 
        }

        /*
        private void _onAdd()
        {
            if (!CheckParams())
                return;

            OpenFileDialog flDialog = new OpenFileDialog();
            flDialog.Filter = "ექსელის ფაილები|*.xls;*.xlsx";
            string filename = string.Empty;
            if (flDialog.ShowDialog(this) == DialogResult.OK)
                filename = flDialog.FileName;
            if (string.IsNullOrEmpty(filename))
                return;
            DataTable excelData = GetProgramManager().GetDataManager().GetTableDataFromExcel(filename);
            if (excelData == null || excelData.Rows.Count <= 0)
                return;
            GetProgramManager().ShowWaitForm();
            ipmElementList.ProductExcelImportLog logForm = new ipmElementList.ProductExcelImportLog();
            ImageList logFormImagelist = new ImageList();
            logFormImagelist.Images.Add(ipmElementList.Properties.Resources.exit_16);
            logFormImagelist.Images.Add(ipmElementList.Properties.Resources.edit_16);
            logForm.listView_Log.SmallImageList = logFormImagelist;
            logForm.listView_Log.Columns.Clear();
            logForm.listView_Log.Columns.Add(" ¹ ");
            logForm.listView_Log.Columns.Add("სტუდენტი");
            logForm.listView_Log.Columns.Add("პირადი N");
            logForm.listView_Log.Columns.Add("დარიცხული");
            logForm.listView_Log.Columns.Add("გრანტი");
            logForm.listView_Log.Columns.Add("შედეგი");
            StringBuilder error_str = new StringBuilder();



            foreach (DataRow row in excelData.Rows)
            {
                error_str.Length = 0;
                string contragent_code = Convert.ToString(row["პირადი N"]);
                int contragent_id = GetProgramManager().GetDataManager().GetContragentIDByCode(contragent_code);
                string contr_name = Convert.ToString(row["სტუდენტი"]);
                double pay_amount = 0;
                double.TryParse(Convert.ToString(row["დარიც."]), out pay_amount);
                double grant = 0;
                double.TryParse(Convert.ToString(row["გრ."]), out grant);
                string credit = Convert.ToString(row["კრედ."]);
                if (contragent_id <= 0 || pay_amount + grant <= 0 || grant < 0 || pay_amount < 0)
                {

                    if (contragent_id <= 0)
                        error_str.AppendLine("კონტრაგენტი ვერ მოიძებნა");
                    if (pay_amount + grant <= 0)
                        error_str.AppendLine("თანხა უნდა აღემატებოდეს 0-ს");
                    if (pay_amount < 0 || grant < 0)
                        error_str.AppendLine("უარყოფითი საფასური ან გრანტი დაუშვებელია");

                    ListViewItem item = new ListViewItem(excelData.Rows.IndexOf(row).ToString());
                    item.ImageIndex = 0;
                    item.SubItems.Add(contr_name);
                    item.SubItems.Add(contragent_code);
                    item.SubItems.Add(pay_amount.ToString());
                    item.SubItems.Add(grant.ToString());
                    item.SubItems.Add(error_str.ToString());
                    logForm.listView_Log.Items.Add(item);
                    continue;
                }
                GetProgramManager().GetDataManager().Close();
                using (TransactionScope tran = new TransactionScope(new TransactionScopeOption(), new TimeSpan(0, 10, 0)))
                {
                    GetProgramManager().GetDataManager().Open();

                    int doc_id = GetProgramManager().GetDataManager().GetIntegerValue("SELECT ISNULL(MAX(id),0)+1 FROM doc.special");
                    string purpose_text = "თანხის დარიცხვა: " + contr_name + " - " + contragent_code + " : " + pay_amount + " გრანტი : " + grant + " კრედიტი: " + credit;
                    int new_general_id = GetProgramManager().GetDataManager().Insert_GeneralDocs(txtDate.Value, "", doc_id,
                         88, purpose_text, pay_amount, 1, 1, 0, GetProgramManager().GetUserID(), 0, contragent_id, 0, 0, true, 1);
                    if (new_general_id <= 0)
                    {
                        Transaction.Current.Rollback();
                        ListViewItem item = new ListViewItem((excelData.Rows.IndexOf(row) + 1).ToString());
                        item.ImageIndex = 0;
                        item.SubItems.Add(contr_name);
                        item.SubItems.Add(contragent_code);
                        item.SubItems.Add(pay_amount.ToString());
                        item.SubItems.Add(grant.ToString());
                        item.SubItems.Add("ბაზაში ჩაწერა ვერ მოხერხდა.");
                        logForm.listView_Log.Items.Add(item);
                        continue;
                    }
                    string sql_spec = "INSERT INTO doc.Special (id, general_id, type) VALUES(" + doc_id + "," + new_general_id + "," + 5 + ")";
                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql_spec))
                    {
                        Transaction.Current.Rollback();
                        ListViewItem item = new ListViewItem((excelData.Rows.IndexOf(row) + 1).ToString());
                        item.ImageIndex = 0;
                        item.SubItems.Add(contr_name);
                        item.SubItems.Add(contragent_code);
                        item.SubItems.Add(pay_amount.ToString());
                        item.SubItems.Add(grant.ToString());
                        item.SubItems.Add("ბაზაში ჩაწერა ვერ მოხერხდა.");
                        logForm.listView_Log.Items.Add(item);
                        continue;
                    }
                    // "1410.1"- "3190" - granti
                    // "1410.2"- "3190" - ugranto
                    if (pay_amount > 0)
                    {
                        if (!GetProgramManager().GetDataManager().Insert_Entries(
                                GetProgramManager().GetDataManager().GetUniqueEntryID(),
                                new_general_id,
                                "1410",
                                "3190",
                                pay_amount,
                                1.0,
                                0,
                                contragent_id, 0, 0,
                                 Convert.ToInt32(comboContragent.SelectedValue), 0, 0, "დარიცხული საფასური", 1, 1))
                        {
                            Transaction.Current.Rollback();
                            ListViewItem item = new ListViewItem((excelData.Rows.IndexOf(row) + 1).ToString());
                            item.ImageIndex = 0;
                            item.SubItems.Add(contr_name);
                            item.SubItems.Add(contragent_code);
                            item.SubItems.Add(pay_amount.ToString());
                            item.SubItems.Add(grant.ToString());
                            item.SubItems.Add("ბაზაში ჩაწერა ვერ მოხერხდა.");
                            logForm.listView_Log.Items.Add(item);
                            continue;
                        }
                    }
                    if (grant > 0)
                    {
                        int grant_person_id = Convert.ToInt32(btnContragents.Tag);
                        if (!GetProgramManager().GetDataManager().Insert_Entries(
                           GetProgramManager().GetDataManager().GetUniqueEntryID(),
                           new_general_id,
                           "1410",
                           "3190",
                           grant,
                           1.0,
                           0,
                          grant_person_id, 0, 0,
                          Convert.ToInt32(comboContragent.SelectedValue), 0, 0, "დარიცხული გრანტი", 1, 1))
                        {
                            Transaction.Current.Rollback();
                            ListViewItem item = new ListViewItem((excelData.Rows.IndexOf(row) + 1).ToString());
                            item.ImageIndex = 0;
                            item.SubItems.Add(contr_name);
                            item.SubItems.Add(contragent_code);
                            item.SubItems.Add(pay_amount.ToString());
                            item.SubItems.Add(grant.ToString());
                            item.SubItems.Add("ბაზაში ჩაწერა ვერ მოხერხდა.");
                            logForm.listView_Log.Items.Add(item);
                            continue;
                        }
                    }



                    tran.Complete();
                }
                ListViewItem item_end = new ListViewItem((excelData.Rows.IndexOf(row) + 1).ToString());
                item_end.ImageIndex = 1;
                item_end.SubItems.Add(contr_name);
                item_end.SubItems.Add(contragent_code);
                item_end.SubItems.Add(pay_amount.ToString());
                item_end.SubItems.Add(grant.ToString());
                item_end.SubItems.Add("OK");
                logForm.listView_Log.Items.Add(item_end);

            }
            GetProgramManager().HideWaitForm();

            logForm.Show();
            Close();




        }
       */

        private void onAdd()
        {
            if (!CheckParams())
                return ;
            
            OpenFileDialog flDialog = new OpenFileDialog();
            flDialog.Filter = "ექსელის ფაილები|*.xls";
            string filename = string.Empty;
            if (flDialog.ShowDialog(this) == DialogResult.OK)
                filename = flDialog.FileName;
            if (string.IsNullOrEmpty(filename))
                return;
            DataTable excelData = GetProgramManager().GetDataManager().GetTableDataFromExcel(filename);
            if (excelData == null || excelData.Rows.Count <= 0)
                return;
            ProgressDispatcher.Activate();
            GetProgramManager().InitialiseLog(new List<string> { "სტუდენტი", "პირადი  №", "დარიცხული", "გრანტი", "შედეგი" });

            foreach (DataRow row in excelData.Rows)
            {
                string contragent_code = Convert.ToString(row["პირადი N"]);
                int contragent_id = GetProgramManager().GetDataManager().GetContragentIDByCode(contragent_code);
                string contr_name = Convert.ToString(row["სტუდენტი"]);
                double pay_amount = 0;
                double.TryParse(Convert.ToString(row["დარიც."]), out pay_amount);
                double grant = 0;
                double.TryParse(Convert.ToString(row["გრ."]), out grant);
                string credit = Convert.ToString(row["კრედ."]);
             
                Func<int> GetDocMaxID = () =>
                {
                    int year = DateTime.Now.Year;
                    DateTime dt = new DateTime(year, 1, 1, 0, 0, 0);
                    string sqlText = @"SELECT ISNULL(MAX(doc_num),0)+1 as res FROM doc.GeneralDocs 
                    INNER JOIN config.DocTypes ON doc.GeneralDocs.doc_type = config.DocTypes.id 
                    WHERE doc.GeneralDocs.tdate>='" + dt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AND config.DocTypes.tag='DOC_SINGLEENTRY'";
                    int max_value = GetProgramManager().GetDataManager().GetIntegerValue(sqlText);
                    return max_value;
                };
                if (contragent_id <= 0 || pay_amount+grant<=0 || grant<0 || pay_amount<0)
                {
                    
                    if (contragent_id <= 0)
                        GetProgramManager().AddLogFormItem(new List<string> { "კონტრაგენტი ვერ მოიძებნა", "", "", "", "ვერ შესრულდა" }, 0);
                    if (pay_amount+grant <= 0)
                        GetProgramManager().AddLogFormItem(new List<string> { "თანხა უნდა აღემატებოდეს 0-ს", "", "", "", "ვერ შესრულდა" }, 0);
                    if (pay_amount < 0 || grant<0)
                        GetProgramManager().AddLogFormItem(new List<string> { "უარყოფითი საფასური ან გრანტი დაუშვებელია", "", "", "", "ვერ შესრულდა" }, 0);
                    continue;
                }

                int doc_type = 70;
                GetProgramManager().GetDataManager().Close(); 
                using (TransactionScope tran = new TransactionScope(new TransactionScopeOption(), new TimeSpan(0, 10, 0)))
                {
                    GetProgramManager().GetDataManager().Open();
                    if (pay_amount > 0)
                    {
                        string purpose_text = "თანხის დარიცხვა: " + contr_name + " - " + contragent_code + " : " + " კრედიტი: " + credit + " დარიცხული : " + pay_amount;
                        int general_id = GetProgramManager().GetDataManager().Insert_GeneralDocs(
                                      txtDate.Value,
                                        "",
                                      GetDocMaxID(),
                                      doc_type,
                                      purpose_text,
                                      pay_amount, 1, 1, 0,
                                      GetProgramManager().GetUserID(),
                                      0, contragent_id, 0, 0, true, 1,1,0);
                        if (general_id <= 0)
                        {
                            GetProgramManager().AddLogFormItem(new List<string> { "ბაზაში ჩაწერა ვერ მოხერხდა.", "", "", "", "ვერ შესრულდა" }, 0);
                            Transaction.Current.Rollback();
                            continue;
                        }
                        string sqlText = @"INSERT INTO doc.SingleEntry (general_id, debit_acc, credit_acc, amount,a1,a2,a3,b1,b2,b3,n,n2) 
                                                    VALUES(@general_id, @debit_acc, @credit_acc, @amount,@a1,@a2,@a3,@b1,@b2,@b3,@n,@n2)";
                        m_sqlParams.Clear();
                        m_sqlParams.Add("@general_id", general_id);
                        m_sqlParams.Add("@debit_acc", "1410");
                        m_sqlParams.Add("@credit_acc", "3190");
                        m_sqlParams.Add("@amount", pay_amount);
                        m_sqlParams.Add("@a1", contragent_id);
                        m_sqlParams.Add("@a2", 0);
                        m_sqlParams.Add("@a3", 0);
                        m_sqlParams.Add("@b1", Convert.ToInt32(comboContragent.SelectedValue));
                        m_sqlParams.Add("@b2", 0);
                        m_sqlParams.Add("@b3", 0);
                        m_sqlParams.Add("@n", 0);
                        m_sqlParams.Add("@n2", 0);
                        if (!GetProgramManager().GetDataManager().ExecuteSql(sqlText, m_sqlParams))
                        {
                            GetProgramManager().AddLogFormItem(new List<string> { "ბაზაში ჩაწერა ვერ მოხერხდა.", "", "", "", "ვერ შესრულდა" }, 0);
                            Transaction.Current.Rollback();
                            continue;
                        }

                        if (!GetProgramManager().GetDataManager().Insert_Entries(
                                general_id,
                                "1410",
                                "3190",
                                pay_amount,
                                1.0,
                                0, 0,
                                contragent_id, 0, 0,
                                 Convert.ToInt32(comboContragent.SelectedValue), 0, 0, "დარიცხული საფასური", 1, 1))
                        {
                            GetProgramManager().AddLogFormItem(new List<string> { "ბაზაში ჩაწერა ვერ მოხერხდა.", "", "", "", "ვერ შესრულდა" }, 0);
                            Transaction.Current.Rollback();
                            continue;
                        }
                    }
                       

                    if (grant > 0)
                    {
                       string purpose_text = "თანხის დარიცხვა: " + contr_name + " - " + contragent_code + " : " + " კრედიტი: " + credit + " გრანტი : " + grant;
                       int grant_person_id = Convert.ToInt32(btnContragents.Tag);
                       int second_general_id = GetProgramManager().GetDataManager().Insert_GeneralDocs(
                                      txtDate.Value,
                                        "",
                                      GetDocMaxID(),
                                      doc_type,
                                      purpose_text,
                                      grant, 1, 1, 0,
                                      GetProgramManager().GetUserID(),
                                      0, grant_person_id, 0, 0, true, 1,1,0);

                       if (second_general_id <= 0)
                        {
                            GetProgramManager().AddLogFormItem(new List<string> { "ბაზაში ჩაწერა ვერ მოხერხდა.", "", "", "", "ვერ შესრულდა" }, 0);
                            Transaction.Current.Rollback();
                            continue;
                        }
                       string sqlText = @"INSERT INTO doc.SingleEntry (general_id, debit_acc, credit_acc, amount,a1,a2,a3,b1,b2,b3,n,n2) 
                                                    VALUES(@general_id, @debit_acc, @credit_acc, @amount,@a1,@a2,@a3,@b1,@b2,@b3,@n,@n2)";
                       m_sqlParams.Clear();
                       m_sqlParams.Add("@general_id", second_general_id);
                       m_sqlParams.Add("@debit_acc", "1410");
                       m_sqlParams.Add("@credit_acc", "3190");
                       m_sqlParams.Add("@amount", grant);
                       m_sqlParams.Add("@a1", grant_person_id);
                       m_sqlParams.Add("@a2", 0);
                       m_sqlParams.Add("@a3", 0);
                       m_sqlParams.Add("@b1", Convert.ToInt32(comboContragent.SelectedValue));
                       m_sqlParams.Add("@b2", 0);
                       m_sqlParams.Add("@b3", 0);
                       m_sqlParams.Add("@n", 0);
                       m_sqlParams.Add("@n2", 0);
                       if (!GetProgramManager().GetDataManager().ExecuteSql(sqlText, m_sqlParams))
                       {
                           GetProgramManager().AddLogFormItem(new List<string> { "ბაზაში ჩაწერა ვერ მოხერხდა.", "", "", "", "ვერ შესრულდა" }, 0);
                           Transaction.Current.Rollback();
                           continue;
                       }
                        
                        if (!GetProgramManager().GetDataManager().Insert_Entries(
                           second_general_id,
                           "1410",
                           "3190",
                           grant,
                           1.0,
                           0, 0,
                          grant_person_id, 0, 0,
                          Convert.ToInt32(comboContragent.SelectedValue), 0, 0, "დარიცხული გრანტი", 1, 1))
                        {
                            GetProgramManager().AddLogFormItem(new List<string> { "ბაზაში ჩაწერა ვერ მოხერხდა.", "", "", "", "ვერ შესრულდა" }, 0);
                            Transaction.Current.Rollback();
                            continue;
                        }
                    }


                    GetProgramManager().AddLogFormItem(new List<string> { contr_name, contragent_code, pay_amount.ToString(), grant.ToString(), "OK"}, 1);
                    tran.Complete();
                }

               
            }
            ProgressDispatcher.Deactivate();
            GetProgramManager().ShowLogForm();
            Close();
        }
       
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }




        private void btnAdd_Click(object sender, EventArgs e)
        {
            onAdd();
        }


        private void UniPayForm_Load(object sender, EventArgs e)
        {
            FillComboContragent();
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
            string tag = GetProgramManager().GetConfigParamValue("CustomerVendor") != "1" ? "TABLE_CONTRAGENT:CUSTOMER" : "TABLE_CONTRAGENT:CUSTOMER;VENDOR";
            int res = GetProgramManager().ShowSelectForm(tag, contragent_id);

            if (res != -1)
            {
                string sql = "SELECT name FROM book.Contragents WHERE id =" + res.ToString();

                DataTable data = new DataTable();
                data = GetProgramManager().GetDataManager().GetTableData(sql);

                if (data.Rows.Count == 0)
                    return;

                btnContragents.Tag = res.ToString();
                txtContragent.Text = GetProgramManager().GetDataManager().GetContragentNameByID(res);


            }
        }

        private void btnContragentSubAccount_Click(object sender, EventArgs e)
        {
            onAdd();
        }


       

    }
}
