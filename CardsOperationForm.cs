using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using ipmPMBasic;

using System.Transactions;
using ipmControls;
using System.Globalization;

namespace ipmExtraFunctions
{

    public enum FormMode { StartBalance, StartBalanceDiscount, ResetBalance };

    public partial class CardsOperationForm : Form
    {

        ProgramManagerBasic pm;
        Hashtable m_Sqlparams = new Hashtable();
        FormMode m_mode;
        DataGridView m_CurrentGrid;
        public CardsOperationForm(ProgramManagerBasic pm, FormMode mode)
        {
            InitializeComponent();
            this.pm = pm;
            m_mode = mode;
            initializeWithMode();
        }
        private void initializeWithMode()
        {
            switch (m_mode)
            {
                case FormMode.ResetBalance:
                    {
                        m_CurrentGrid = m_Grid;
                        this.Text = GetProgramManager().GetTranslatorManager().Translate("ბარათების განულება");
                        tabControlCards.TabPages.Remove(tabPageStart);
                        txtNumber.Focus();
                        break;
                    }
                case FormMode.StartBalance:
                    {
                        m_CurrentGrid = m_GridStart;
                        this.Text = GetProgramManager().GetTranslatorManager().Translate("დაგროვების ბარათის საწყისი ნაშთი");
                        tabControlCards.TabPages.Remove(tabPageReset);
                        textStartNumber.Focus();
                        break;
                    }
                case FormMode.StartBalanceDiscount:
                    {
                        m_CurrentGrid = m_GridStart;
                        this.Text = GetProgramManager().GetTranslatorManager().Translate("ფასდაკლების ბარათის საწყისი ნაშთი");
                        tabControlCards.TabPages.Remove(tabPageReset);
                        textStartNumber.Focus();
                        break;
                    }
                default:
                    {
                        this.Close();
                        return;
                    }
            }
            textStartNumber.Text = GetDocMaxID().ToString();
            textStartPurpose.Text = this.Text + ":";

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }
     
        private ProgramManagerBasic GetProgramManager()
        {
            return pm;
        }
        
        private int GetDocMaxID()
        {
            int res = m_mode == FormMode.ResetBalance ? 2 : m_mode == FormMode.StartBalance ? 3 : 4;
            return GetProgramManager().GetDataManager().GetIntegerValue("SELECT ISNULL(MAX(id),0)+1 FROM doc.special  WITH(NOLOCK) where type=" + res);
            
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void onExecute()
        {

            if (m_CurrentGrid.Rows.Count <= 0)
                return;
            if (MessageBox.Show("შესრულდეს  ოპერაცია?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            bool res = false;
            switch (m_mode)
            {
                case FormMode.ResetBalance:
                    res = OnReset();
                    break;
                case FormMode.StartBalance:
                case FormMode.StartBalanceDiscount:
                    res = onStart();
                    break;
              
            }
            
            if (res)
            {
                MessageBox.Show("ოპერაცია წარმატებით შესრულდა", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Question);
                Close();
            }
            else
            {
                MessageBox.Show("ოპერაცია ვერ შესრულდა", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }


        private bool onStart()
        {
            
            m_CurrentGrid.EndEdit();
            ProgressDispatcher.Activate(true);
            int i = 1;
            foreach (DataGridViewRow row in m_CurrentGrid.Rows)
            {
                decimal per = (decimal)i++ / (decimal)m_CurrentGrid.Rows.Count * 100M;
                ProgressDispatcher.Percent = ((int)per);
                double start_val = 0;
                if (!double.TryParse(Convert.ToString(row.Cells[ColStartVal.Index].Value, CultureInfo.InvariantCulture), out start_val))
                    continue;
                start_val = (double)Math.Round((decimal)start_val, 2);
                int card_id = Convert.ToInt32(row.Cells[ColIDStart.Index].Value);
                int contragent_id = Convert.ToInt32(row.Cells[ColContragentIDStart.Index].Value);

               
                string card_code = Convert.ToString(row.Cells[ColCardCodeStart.Index].Value);
                //string contragent_name = GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id);
                string person_name = Convert.ToString(row.Cells[ColHolderNameStart.Index].Value);
                string purpose_text = this.Text + " : " + start_val.ToString() + "; " /*+ contragent_name + "; "*/ + person_name + "; " + card_code;

                GetProgramManager().GetDataManager().Close();
                using (TransactionScope tran = new TransactionScope(new TransactionScopeOption(), new TimeSpan(0, 10, 0)))
                {
                    GetProgramManager().GetDataManager().Open();
                    int new_general_id = GetProgramManager().GetDataManager().Insert_GeneralDocs( txtDateStart.Value, "", /*GetDocMaxID()*/ 0,
                         88, purpose_text, start_val, 1, 1, 0, GetProgramManager().GetUserID(), 0, contragent_id, card_id, 0, false, 1,1,0);
                    if (new_general_id <= 0)
                    {
                        Transaction.Current.Rollback();
                        ProgressDispatcher.Deactivate();
                        return false;
                    }
                    string sql_spec = "INSERT INTO doc.Special (general_id, type) VALUES(@general_id, @type)";
                    m_Sqlparams.Clear();
                    m_Sqlparams.Add("@general_id", new_general_id);
                    m_Sqlparams.Add("@type", m_mode == FormMode.StartBalance ? 3 : 4);
                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql_spec, m_Sqlparams))
                    {
                        Transaction.Current.Rollback();
                        ProgressDispatcher.Deactivate();
                        return false;
                    }
                    switch (m_mode)
                    {
                        case FormMode.StartBalance:
                            {
                                if (!GetProgramManager().GetDataManager().InsertCardFlow(new_general_id, contragent_id, card_id, txtDate.Value, 1, Math.Abs(start_val)))
                                {
                                    Transaction.Current.Rollback();
                                    ProgressDispatcher.Deactivate();
                                    return false;
                                }
                                break;
                            }

                        case FormMode.StartBalanceDiscount:
                            {
                                if (!GetProgramManager().GetDataManager().InsertDiscountCardFlow(new_general_id, 0, 0, 0, 0, contragent_id, card_id, Math.Abs(start_val), 1, 0, 0))
                                {
                                    Transaction.Current.Rollback();
                                    ProgressDispatcher.Deactivate();
                                    return false;
                                }
                                break;
                            }
                    }
                    tran.Complete();
                }

            }
            m_CurrentGrid.Rows.Clear();
            ProgressDispatcher.Deactivate();
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            onExecute();
        }
       
        private bool OnReset()
        {
            string tdate = txtDate.Value.ToString("yyyy-MM-dd 23:59:59");

            ProgressDispatcher.Activate();
            StringBuilder SyncGeneralids = new StringBuilder();

            foreach (DataGridViewRow row in m_Grid.Rows)
            {
                int card_id = GetProgramManager().GetDataManager().GetCardIDByCode(Convert.ToString(row.Cells[ColCardCode.Index].Value),1);
                int contragent_id = GetProgramManager().GetDataManager().GetContragentIDByCard_Code(Convert.ToString(row.Cells[ColCardCode.Index].Value));
                string contragent_name = GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id);
                string person_name = Convert.ToString(row.Cells[ColHolderName.Index].Value);
                double total_bonus = Convert.ToDouble(row.Cells[ColBonus.Index].Value);
                string purpose_text = "ბარათის განულება: " + total_bonus.ToString() + "; " + contragent_name + "; " + person_name;
                GetProgramManager().GetDataManager().Close();
                using (TransactionScope tran = new TransactionScope(new TransactionScopeOption(), new TimeSpan(0, 10, 0)))
                {
                    GetProgramManager().GetDataManager().Open();
                    int new_general_id = GetProgramManager().GetDataManager().Insert_GeneralDocs(Convert.ToDateTime(tdate), "", GetDocMaxID(),
                        88, purpose_text, total_bonus, 1, 1, 0, GetProgramManager().GetUserID(), 0, contragent_id, card_id, 0, false, 1,1,0);
                    if(new_general_id<=0)
                    {
                        Transaction.Current.Rollback();
                        continue;
                    }
                    string sql_spec = "INSERT INTO doc.Special (general_id, type) VALUES(@general_id, @type)";
                    m_Sqlparams.Clear();
                    m_Sqlparams.Add("@general_id", new_general_id);
                    m_Sqlparams.Add("@type", 2);
                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql_spec, m_Sqlparams))
                    {
                        Transaction.Current.Rollback();
                        continue;
                    }
                    int coeff = total_bonus > 0 ? -1 : 1;
                    if (!GetProgramManager().GetDataManager().InsertCardFlow(new_general_id, contragent_id, card_id, Convert.ToDateTime(tdate), coeff, Math.Abs(total_bonus)))
                    {
                        Transaction.Current.Rollback();
                        continue;
                    }
                    SyncGeneralids.Append(new_general_id.ToString()).Append(',');
                    tran.Complete();
                }

            }


            if (GetProgramManager().GetConfigParamValue("CheckExternalCard") == "1")
            {
                if (SyncGeneralids != null && SyncGeneralids.Length > 0)
                {
                    RestFulManager restmanager = new RestFulManager(GetProgramManager().GetDataManager());
                    if (restmanager.SyncCardOperations(SyncGeneralids.ToString().TrimEnd(',')))
                        MessageBox.Show("სინქრონიზაცია წარამტებით შესრულდა", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            m_CurrentGrid.Rows.Clear();
            ProgressDispatcher.Deactivate();
            return true;
        }

        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            //panel6.Enabled = !checkAll.Checked;
        }

        private bool OnCheckItem(int id, int col_index )
        {
            foreach (DataGridViewRow row in m_CurrentGrid.Rows)
            {
                if (row.Cells[col_index].Value.ToString() == id.ToString())
                    return false;
            }
            return true;
        }
        private void onAdd()
        {
            int[] res = GetProgramManager().ShowSelectFormMulti("TABLE_CARDCODES", 0);
            if (res == null || res.Length <= 0)
                return;
            bool IsExternalCards = GetProgramManager().GetConfigParamValue("CheckExternalCard") == "1" ? true : false;
            RestFulManager restmanager;
            GetCardBonus card_info;
            List<CardBonus> iList = new List<CardBonus>();
            int id = res[0];
            string card_code = string.Empty;
            string holder_code = string.Empty;
            string holder_name = string.Empty;
            double total_bonus = 0;

            if (IsExternalCards)
            {
                restmanager = new RestFulManager(GetProgramManager().GetDataManager());
                card_info = restmanager.GetCardBonusInfo("all", txtDate.Value.ToString("yyyy-MM-dd 23-59-59"));
                if (card_info == null || card_info.Cbonus == null || card_info.Cbonus.Count <= 0)
                    return;
                iList = card_info.Cbonus;
            }

            for (int i = 0; i < res.Length; i++)
            {
                if (!OnCheckItem(res[i], 0))
                    continue;
                if (IsExternalCards)
                {
                    string current_card_code = GetProgramManager().GetDataManager().GetCardCodeByID(res[i]);
                    foreach (CardBonus cbonus in iList)
                    {
                        if (cbonus.card_code == current_card_code)
                        {
                            card_code = cbonus.card_code;
                            holder_code = cbonus.code;
                            holder_name = cbonus.name;
                            total_bonus = cbonus.bonus;
                            break;
                        }
                    }
                }

                else
                {
                    string sql = @"SELECT CC.id, CC.card_code AS card_code, CC.person_name AS holder_name, CC.person_code AS holder_code,
                            ISNULL((SELECT SUM(p.bonus_coeff*p.bonus)   FROM doc.CardsFlow as p where p.card_id=cc.id AND p.bonus_coeff<>0 AND p.date<=@tdate),0) AS totalBonus FROM book.ContragentCardCodes AS CC  
                            WHERE ISNULL((SELECT SUM(p.bonus_coeff*p.bonus) FROM doc.CardsFlow as p where p.card_id=cc.id AND p.bonus_coeff<>0 AND p.date<=@tdate),0)<>0 AND CC.id=" + res[i] +"  AND CC.operation_type=118 AND CC.type_id=1 AND CC.status>0";
                    DataTable data = GetProgramManager().GetDataManager().GetTableData(sql, new Hashtable() { { "@tdate", txtDate.Value.ToString("yyyy-MM-dd 23:59:59") } });
                    if (data == null || data.Rows.Count <= 0)
                        continue;
                    card_code = data.Rows[0]["card_code"].ToString();
                    holder_code = data.Rows[0]["holder_code"].ToString();
                    holder_name = data.Rows[0]["holder_name"].ToString();
                    total_bonus = Convert.ToDouble(data.Rows[0]["totalBonus"]);
                }
                if (string.IsNullOrEmpty(card_code) || total_bonus == 0)
                    continue;
                m_Grid.Rows.Insert(0);
                DataGridViewRow row = m_Grid.Rows[0];
                row.Cells[ColID.Index].Value = GetProgramManager().GetDataManager().GetCardIDByCode(card_code,1);
                row.Cells[ColCardCode.Index].Value = card_code;
                row.Cells[ColHolderCode.Index].Value = holder_code;
                row.Cells[ColHolderName.Index].Value = holder_name;
                row.Cells[ColBonus.Index].Value = total_bonus.ToString();
            }
        }

        private void onDelete()
        {
            if (m_CurrentGrid.Rows.Count <= 0)
                return;
            foreach (DataGridViewRow row in m_CurrentGrid.SelectedRows)
                m_CurrentGrid.Rows.Remove(row);
        }

        private void OnContragentsList()
        {
            int contragent_id = -1;
            if (btnContragents.Tag != null)
                contragent_id = int.Parse(btnContragents.Tag.ToString());

            int res = GetProgramManager().ShowSelectForm("TABLE_CONTRAGENT", contragent_id);
            if (res == -1)
                return;
            string name = GetProgramManager().GetDataManager().GetContragentNameByID(res);
          
            if (!OnCheckItem(res, 1))
                return;
            string sql = @"SELECT CC.id, CC.Contragent_id, CC.card_code AS card_code, COALESCE(NULLIF(CC.person_name,''), c.name) AS holder_name, COALESCE(NULLIF(CC.person_code,''),c.code) AS holder_code,
                            (SELECT SUM(p.bonus_coeff*p.bonus)   FROM doc.CardsFlow as p where p.card_id=cc.id) AS totalBonus 
                            FROM book.ContragentCardCodes AS CC  
                            INNER JOIN book.Contragents AS c ON c.id=cc.contragent_id
                            WHERE CC.contragent_id=" + res + "  AND CC.operation_type=118 AND CC.type_id=1 AND CC.status>0";

            if (m_mode == FormMode.StartBalanceDiscount)
                sql = @"SELECT CC.id, CC.Contragent_id, CC.card_code AS card_code, COALESCE(NULLIF(CC.person_name,''), c.name) AS holder_name, COALESCE(NULLIF(CC.person_code,''),c.code) AS holder_code,
                            (SELECT SUM(amount)   FROM doc.generaldocs  where id IN(select distinct general_id from doc.discountcardsflow where contragent_id="+res+@")) AS totalBonus 
                            FROM book.ContragentCardCodes AS CC 
                            INNER JOIN book.Contragents AS c ON c.id=cc.contragent_id 
                            WHERE CC.contragent_id=" + res + "  AND CC.operation_type=118 AND CC.type_id=2 AND CC.status>0";


            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);
            if (data == null || data.Rows.Count <= 0)
                return;
            m_GridStart.Rows.Insert(0);
            DataGridViewRow row = m_GridStart.Rows[0];
            row.Cells[ColIDStart.Index].Value = data.Rows[0]["id"].ToString();
            row.Cells[ColContragentIDStart.Index].Value = data.Rows[0]["Contragent_id"].ToString();
            row.Cells[ColCardCodeStart.Index].Value = data.Rows[0]["card_code"].ToString();
            row.Cells[ColHolderCodeStart.Index].Value = data.Rows[0]["holder_code"].ToString();
            row.Cells[ColHolderNameStart.Index].Value = data.Rows[0]["holder_name"].ToString();
            row.Cells[ColBonusStart.Index].Value = data.Rows[0]["totalBonus"].ToString();
            btnContragents.Tag = res.ToString();
            txtContragent.Text = name;

        }
        private void btnAddDependence_Click(object sender, EventArgs e)
        {
            onAdd();
        }

        private void btnDeleteDependence_Click(object sender, EventArgs e)
        {
            onDelete();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            onExecute();
        }

        private void tabPageStart_Click(object sender, EventArgs e)
        {

        }

        private void btnContragents_Click(object sender, EventArgs e)
        {
            OnContragentsList();
        }

        private void btnDeleteDependenceStart_Click(object sender, EventArgs e)
        {
            onDelete();
        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        private void btnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog _dlg = new OpenFileDialog { Filter = "Excel File (*.xls)|*.xls" })
            {
                if (_dlg.ShowDialog() != DialogResult.OK)
                    return;
                DataTable res = GetProgramManager().GetDataManager().GetTableDataFromExcel(_dlg.FileName);
                if (res == null || res.Rows.Count == 0)
                    return;
                m_GridStart.Rows.Clear();
                foreach (DataRow rw in res.Rows)
                {
                    m_GridStart.Rows.Add(rw["cc_id"], rw["c_id"], rw["ბარათის კოდი"], rw["მყიდველი"], rw["მყიდველის კოდი"], null, rw["ნაშთი"]);
                }
            }
        }
    }

    
}
