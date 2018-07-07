using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ipmBLogic;
using ipmElementForms;
using ipmPMBasic;
using System.Globalization;
using System.ComponentModel;

namespace ipmExtraFunctions
{
    public partial class BankStatementForm : Form
    {
        private ProgramManagerBasic m_Pm;
        enum BankType
        {
            [Description("პროკრედიტ ბანკი")]
            ProcreditBank,
            [Description("საქართველოს ბანკი")]
            BankOfGeorgia,
            [Description("თიბისი ბანკი")]
            TBC,
            [Description("პრივატ ბანკი")]
            Tao,
            [Description("ლიბერთი ბანკი")]
            Liberty,
            [Description("ტერაბანკი")]
            TeraBank,
            [Description("ქართუ ბანკი")]
            Cartu,
            [Description("ბაზისბანკი")]
            BasisBank,
            [Description("ვითიბი ბანკი ჯორჯია")]
            VTB,
            [Description("ბანკი რესპუბლიკა")]
            BankRepublic,
            [Description("ბანკი კონსტანტა")]
            Constanta,
            [Description("კავკასიის განვითარების ბანკის თბილისის ფილიალი")]
            CaucasianBank,
            [Description("სილქ როუდ ბანკი")]
            SilkRoadBank,
            [Description("ხალიკ ბანკი საქართველო")]
            HalykBankGeorgia
        }

        bool m_bIsAnalytic = false;
        bool m_bog_new = false;
        bool m_bog_ultra_new = false;
        bool m_cartu_new = false;
        bool m_cartu_ultra_new = false;
        bool m_bazis_new = false;
        bool m_procreditCodeMode = false;
        Dictionary<string, KeyValuePair<int, string>> m_docTypes;

        public BankStatementForm(ProgramManagerBasic pm)
        {
            m_Pm = pm;
            InitializeComponent();
            comboBank.MouseWheel += new MouseEventHandler(comboBank_MouseWheel);


             string sqlText = "SELECT a.name,a.id " +
            "FROM book.CompanyAccounts a INNER JOIN book.Banks b ON a.bank_id = b.id INNER JOIN book.Currencies c ON a.currency_id = c.id WHERE c.code='GEL'";
            DataTable data= GetProgramManager().GetDataManager().GetTableData(sqlText);
            comboBank.DataSource= data;
            comboBank.DisplayMember = "name";
            comboBank.ValueMember = "id";
            splitContainer1.Panel2Collapsed = true;
            if (GetProgramManager().GetConfigParamValue("AnalyticCodes") == "1")
                m_bIsAnalytic = true;
            m_docTypes = new Dictionary<string, KeyValuePair<int, string>>() {
            {"customer_money", new KeyValuePair< int, string>(38,"DOC_OPERATION-CUSTOMERMONEYIN")},
            {"bank_money_in", new KeyValuePair< int, string>(38,"DOC_OPERATION-CUSTOMERMONEYIN")},
            {"vendor_money", new KeyValuePair< int, string>(39,"DOC_OPERATION-VENDORMONEYOUT")},
            {"staff_tax", new KeyValuePair< int, string>(47,"DOC_OPERATION-TAX")},
            {"person_tax", new KeyValuePair< int, string>(47,"DOC_OPERATION-TAX")},
            {"income_tax", new KeyValuePair< int, string>(47,"DOC_OPERATION-TAX")},
            {"inventory_tax", new KeyValuePair< int, string>(47,"DOC_OPERATION-TAX")},
            {"vat_tax", new KeyValuePair< int, string>(47,"DOC_OPERATION-TAX")},
            {"staff_money_in", new KeyValuePair< int, string>(45,"DOC_OPERATION-RESPONSIBLEMONEYIN")},
            {"staff_money_out", new KeyValuePair< int, string>(44,"DOC_OPERATION-RESPONSIBLEMONEYOUT")},
            {"staff_money_outM", new KeyValuePair< int, string>(44,"DOC_OPERATION-RESPONSIBLEMONEYOUT")},
            {"staff_money_outV", new KeyValuePair< int, string>(44,"DOC_OPERATION-RESPONSIBLEMONEYOUT")},
            {"staff_salary", new KeyValuePair< int, string>(48,"DOC_OPERATION-SALARY")},
            {"comission", new KeyValuePair< int, string>(51,"DOC_OPERATION-BANKCOMISSION")},
            {"pay_credit_percent", new KeyValuePair< int, string>(54,"DOC_OPERATION-PAYCREDITPERCENT")},
            {"import_vat_tax", new KeyValuePair< int, string>(39,"DOC_OPERATION-VENDORMONEYOUT")},
            {"pay_credit", new KeyValuePair< int, string>(57,"DOC_OPERATION-PAYCREDIT")},
            {"customer_money_out", new KeyValuePair<int, string>(65,"DOC_OPERATION-CUSTOMERMONEYOUT") },

             {"customer_advance", new KeyValuePair< int, string>(49,"DOC_OPERATION-CUSTOMERADVANCE")},
             {"vendor_advance", new KeyValuePair< int, string>(50,"DOC_OPERATION-VENDORADVANCE")},
             {"cash_to_bank", new KeyValuePair< int, string>(41, "DOC_OPERATION-CASHBANK")},
             {"bank_to_cash", new KeyValuePair< int, string>(40, "DOC_OPERATION-BANKCASH")},
             {"bank_to_bank", new KeyValuePair< int, string>(5, "DOC_BANKTRANSFER")}



            
            
            };
            if (m_bIsAnalytic)
            {
                m_Grid.Columns[ColAnalyticBtn.Index].Visible = true;
                m_Grid.Columns[ColAnalyticText.Index].Visible = true;
            }

            int? proj = GetProgramManager().GetDataManager().GetScalar<int>("SELECT COUNT(id) FROM book.Projects");
            if (proj.HasValue && proj.Value > 1)
            {
                m_Grid.Columns[ColProjectBtn.Index].Visible = true;
                m_Grid.Columns[ColProjectName.Index].Visible = true;
                btnSetProject.Enabled = true;
            }
                      fillComboFilter();
            //load extra functions
            btnExtra.Visible = false;
            string sql = "SELECT id,name FROM book.CustomDocTemplates ";
            DataTable data2 = GetProgramManager().GetDataManager().GetTableData(sql);

            if (data2 != null)
            {
                foreach (DataRow r in data2.Rows)
                {

                    ToolStripMenuItem btn = new ToolStripMenuItem();
                    btn.Text = GetProgramManager().GetTranslatorManager().Translate(r["name"].ToString());
                    btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.Tag = r["id"].ToString();
                    btn.Font = GetProgramManager().GetTranslatorManager().SetControlsSize((float)10, 11, System.Drawing.FontStyle.Regular);
                    btn.Click += new EventHandler(btnExtra_Click);
                    btnExtra.DropDownItems.Add(btn);
                    btnExtra.Visible = true; 
                }
            }

        }

        private void comboBank_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }
        private void btnExtra_Click(object sender, EventArgs e)
        {
            if(m_Grid.SelectedRows.Count==0)
                return;
            string tag = ((ToolStripMenuItem)sender).Tag.ToString();
            tag = "SHOW_DOC_CUSTOM-" + tag;
            int index= m_Grid.SelectedRows[0].Index;
            string id = m_Grid.Rows[index].Cells[ColContragentID.Index].Value.ToString();
            string date_str =m_Grid.Rows[index].Cells[ColDate.Index].Value.ToString();
            double amount = 0;
            double.TryParse(m_Grid.Rows[index].Cells[ColDebet.Index].Value.ToString(), out amount);
            if (amount <= 0)
            {
                double.TryParse(m_Grid.Rows[index].Cells[ColCredit.Index].Value.ToString(), out amount);
            }
            string temp = "DATE@"+date_str+"#BANK@-" + comboBank.SelectedValue.ToString()+"@0#CONTRAGENT@"+id+"@"+amount.ToString();
            GetProgramManager().SetTempString(temp);
            GetProgramManager().ExecuteCommandByTag(tag, false, false);
            GetProgramManager().SetTempString("");
        }
        private ProgramManagerBasic GetProgramManager()
        {
            return this.m_Pm;
        }
      
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            OnLoad();
        }

        private void OnLoad()
        {

            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "Excel Files |*.xls";

            BankType bank_type = BankType.BankOfGeorgia;
            if (comboBank.Text.IndexOf("საქართველოს ბანკი") >= 0)
                bank_type = BankType.BankOfGeorgia;
            else if (comboBank.Text.IndexOf("პროკრედიტ") >= 0)
                bank_type = BankType.ProcreditBank;
            else if (comboBank.Text.IndexOf("თიბისი") >= 0)
                bank_type = BankType.TBC;
            else if (comboBank.Text.IndexOf("პრივატ ბანკი") >= 0)
                bank_type = BankType.Tao;
            else if (comboBank.Text.IndexOf("ლიბერთი") >= 0)
                bank_type = BankType.Liberty;
            else if (comboBank.Text.IndexOf("ტერაბანკ") >= 0)
                bank_type = BankType.TeraBank;
            else if (comboBank.Text.IndexOf("ქართუ") >= 0)
                bank_type = BankType.Cartu;
            else if (comboBank.Text.IndexOf("ბაზისბანკი") >= 0)
                bank_type = BankType.BasisBank;
            else if (comboBank.Text.IndexOf("ვითიბი ბანკი") >= 0)
                bank_type = BankType.VTB;
            else if (comboBank.Text.IndexOf("ბანკი რესპუბლიკა") >= 0)
                bank_type = BankType.BankRepublic;
            else if (comboBank.Text.IndexOf("ბანკი კონსტანტა") >= 0)
                bank_type = BankType.Constanta;
            else if (comboBank.Text.IndexOf("კავკასიის განვითარების ბანკის") >= 0)
                bank_type = BankType.CaucasianBank;
            else if (comboBank.Text.IndexOf("სილქ როუდ") >= 0)
                bank_type = BankType.SilkRoadBank;
            else if (comboBank.Text.IndexOf("ხალიკ ბანკი") >= 0)
                bank_type = BankType.HalykBankGeorgia;
            else
            {
                MessageBox.Show("ბანკი ვერ მოიძებნა!");
                return;
            }
            string bank_name = EnumEx.GetEnumDescription(bank_type);
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                m_Grid.Rows.Clear();
                DataSet set = GetProgramManager().GetDataManager().GetTablesDataFromExcel(OpenFile.FileName);
                if (set == null || set.Tables.Count == 0)
                    return;

                DataTable excelData = set.Tables[0];
                if(excelData==null||excelData.Rows.Count==0)
                    return;

                switch (bank_type)
                {
                    case BankType.BankOfGeorgia:
                        {
                            if (excelData.Columns.Count == 7 && excelData.Columns[0].Caption.Trim() == "თარიღი" && excelData.Columns[1].Caption.Trim() == "საბუთის N"   && excelData.Columns[2].Caption.Trim() == "მოკორესპოდენტო ანგარიში" && excelData.Columns[3].Caption.Trim() == "დებეტი" && excelData.Columns[4].Caption.Trim() == "კრედიტი")
                                m_bog_new = true;
                            else if (excelData.Columns.Count > 20)
                                m_bog_ultra_new = true;
                            break;
                        }

                    case BankType.Cartu:
                        {
                            if (excelData.Columns.Count == 8 && excelData.Columns[0].Caption.Trim() == "თარიღი" && excelData.Columns[1].Caption.Trim() == "დანიშნულება")
                                m_cartu_new = true;
                            else if (excelData.Columns.Count == 9 && excelData.Columns[0].Caption.Trim().StartsWith(@"თარიღი /") && excelData.Columns[1].Caption.Trim().StartsWith(@"დანიშნულება /"))
                                m_cartu_ultra_new = true;
                            break;
                        }

                    case BankType.BasisBank:
                        {
                            if (excelData.Columns.Count >=22 && excelData.Columns[0].Caption.Trim() =="ამონაწერი :")
                                m_bazis_new = true;
                            break;
                        }
                   
                    case BankType.ProcreditBank:
                        {
                            if (excelData.Columns.Contains("TaxCode"))
                                m_procreditCodeMode = true;
                            break;
                        }
                }


                //exclude list
                Hashtable hExcludeContragents = new Hashtable();
                DataTable exclude_data = GetProgramManager().GetDataManager().GetTableData("SELECT * FROM book.BankStatementExcludeList");
                foreach(DataRow r in exclude_data.Rows)
                {
                    hExcludeContragents.Add(int.Parse(r["Contragent_id"].ToString()),1);
                }
                DataTable contragent_data= GetProgramManager().GetDataManager().GetTableData(
                    "SELECT res.id,res.name FROM (SELECT id,name,LEN(name) AS len FROM book.Contragents WHERE path LIKE '0#2%') res ORDER BY len DESC");
                //prepare customers
                int customer_n =contragent_data.Rows.Count;
                int[] customer_ids = new int[customer_n];
                string[] customer_original_names = new string[customer_n];
                string[] customer_short_names = new string[customer_n];
                string[] customer_names = new string[customer_n];
                string[] customer_reverse_names = new string[customer_n];
                for (int i = 0; i < customer_n; i++)
                {
                    customer_ids[i] = int.Parse(contragent_data.Rows[i]["id"].ToString());
                    customer_original_names[i] = contragent_data.Rows[i]["name"].ToString();

                    customer_names[i] = PrepareString(customer_original_names[i]);// original_names[i].ToLower().Replace("შპს", "").Replace("შ.პ.ს.", "").Replace("ი/მ", "").Replace("ი.მ", "").Replace("ი.მ.", "").Replace(",", "").Replace(".", "").Replace(":", "").Replace("'", "").Replace("\"", "").Replace(" ", "");
                    if (customer_names[i].IndexOf("ბანკი")>=0 || hExcludeContragents.ContainsKey(customer_ids[i]))
                    {
                        customer_names[i] = "@#@@#%^%";
                        customer_original_names[i] = "@#@@#%^%";
                    }
                    string[] vals = customer_original_names[i].Split(' ');
                    customer_reverse_names[i] = "";
                    for (int k = vals.Length-1; k >= 0; k--)
                        customer_reverse_names[i] = customer_reverse_names[i] + vals[k];
                }
                //prepare vendors
                contragent_data = GetProgramManager().GetDataManager().GetTableData("SELECT id,name FROM book.Contragents WHERE path LIKE '0#1%'");
                int vendor_n = contragent_data.Rows.Count;
                int[] vendor_ids = new int[vendor_n];
                string[] vendor_original_names = new string[vendor_n];
                string[] vendor_names = new string[vendor_n];
                string[] vendor_reverse_names = new string[vendor_n];
                for (int i = 0; i < vendor_n; i++)
                {
                    vendor_ids[i] = int.Parse(contragent_data.Rows[i]["id"].ToString());
                    vendor_original_names[i] = contragent_data.Rows[i]["name"].ToString();
                    vendor_names[i] = PrepareString(vendor_original_names[i]);// original_names[i].ToLower().Replace("შპს", "").Replace("შ.პ.ს.", "").Replace("ი/მ", "").Replace("ი.მ", "").Replace("ი.მ.", "").Replace(",", "").Replace(".", "").Replace(":", "").Replace("'", "").Replace("\"", "").Replace(" ", "");
                    if (vendor_names[i].IndexOf("ბანკი") >= 0 || hExcludeContragents.ContainsKey(vendor_ids[i]))
                    {
                        vendor_names[i] = "&*@#@@#%^%";
                        vendor_original_names[i] = "&*@#@@#%^%";
                    }
                    string[] vals = vendor_original_names[i].Split(' ');
                    vendor_reverse_names[i] = "";
                    for (int k = vals.Length-1; k >= 0; k--)
                        vendor_reverse_names[i] = vendor_reverse_names[i] + vals[k];
                }
                //prepare staff list
                contragent_data = GetProgramManager().GetDataManager().GetTableData("SELECT id,name FROM book.Staff ");
                int staff_n = contragent_data.Rows.Count;
                int[] staff_ids = new int[staff_n];
                string[] staff_original_names = new string[staff_n];
                string[] staff_names = new string[staff_n];
                string[] staff_reverse_names = new string[staff_n];
                for (int i = 0; i < staff_n; i++)
                {
                    staff_ids[i] = int.Parse(contragent_data.Rows[i]["id"].ToString());
                    string nm = contragent_data.Rows[i]["name"].ToString();
                    int r = 0;
                    try
                    {
                        int.TryParse(nm, out r);
                    }
                    catch {
                        r = 0;
                    }
                    if (r > 0)
                         nm = "7845%^&#$ 45244413444256UiopwTY";
                    staff_original_names[i] = nm;
                    staff_names[i] = PrepareString(staff_original_names[i]);
                    string[] vals = staff_original_names[i].Split(' ');
                    staff_reverse_names[i] = "";
                    for (int k = vals.Length - 1; k >= 0; k--)
                        staff_reverse_names[i] = staff_reverse_names[i] + vals[k];
                }
                int col_n = excelData.Columns.Count;
                int project_id = 1;
                string project_name = GetProgramManager().GetDataManager().GetProjectName(project_id);

                foreach (DataRow row in excelData.Rows)
                {

                    string description = "";
                    string type = "";
                    string status = "";
                    double debet = 0, credit = 0;
                    string purpose = "";
                    int contragent_id = 0;
                    int exist = -1;
                    string tdate = "";
                    string corr_account = "";
                    string purpose_eng = "";
                    string type_name = "";
                    DateTime date = DateTime.Now;
                    //set date
                    tdate = row[0].ToString();
                    try
                    {
                        if (bank_type == BankType.Tao)
                        {
                            if (excelData.Rows.IndexOf(row) < 4)
                                continue;
                            double t = 0;
                            if (double.TryParse(row[1].ToString(), out t))
                                date = DateTime.FromOADate(t);
                            else
                                DateTime.TryParse(row[1].ToString(), out date);
                            tdate = date.ToString("dd/MM/yyyy");
                        }

                        else if (bank_type == BankType.TeraBank)
                        {
                            if (excelData.Rows.IndexOf(row) < 9)
                                continue;
                            char sep = '/';
                            if (tdate.IndexOf(".") >= 0)
                                sep = '.';
                            string[] date_vals = tdate.Split(sep);
                            date = new DateTime(int.Parse(date_vals[2].ToString()), int.Parse(date_vals[1].ToString()), int.Parse(date_vals[0].ToString()));
                            tdate = date.ToString("dd/MM/yyyy");

                        }

                        else if (bank_type == BankType.HalykBankGeorgia)
                        {
                            if (excelData.Rows.IndexOf(row) < 10)
                                continue;
                            char sep = '/';
                            if (tdate.IndexOf(".") >= 0)
                                sep = '.';
                            string[] date_vals = tdate.Split(sep);
                            date = new DateTime(int.Parse(date_vals[2].ToString()), int.Parse(date_vals[1].ToString()), int.Parse(date_vals[0].ToString()));
                            tdate = date.ToString("dd/MM/yyyy");
                        }


                        else if (bank_type == BankType.BankOfGeorgia)
                        {
                            if (m_bog_ultra_new)
                            {
                                if (excelData.Rows.IndexOf(row) < 12)
                                    continue;

                                double t = 0;
                                if (double.TryParse(row[0].ToString(), out t))
                                    date = DateTime.FromOADate(t);
                                else
                                    DateTime.TryParse(row[0].ToString(), out date);
                                tdate = date.ToString("dd/MM/yyyy");
                            }
                            else if (m_bog_new)
                            {
                                DateTime.TryParseExact(tdate, "dd MMM yyyy", new CultureInfo("ka-GE"), DateTimeStyles.None, out date);
                                tdate = date.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                char sep = '/';
                                if (tdate.IndexOf(".") >= 0)
                                    sep = '.';
                                string[] date_vals = tdate.Split(sep);
                                if (date_vals[2].Length == 2)
                                    date_vals[2] = "20" + date_vals[2];
                                date = new DateTime(int.Parse(date_vals[2]), int.Parse(date_vals[1]), int.Parse(date_vals[0]));
                                tdate = date.ToString("dd/MM/yyyy");
                            }
                        }
                        else if (bank_type == BankType.BasisBank && m_bazis_new)
                        {
                            if (excelData.Rows.IndexOf(row) < 7)
                                continue;
                            DateTime.TryParse(tdate, out date);
                            tdate = date.ToString("dd/MM/yyyy");
                        }

                        else if (bank_type == BankType.CaucasianBank)
                        {
                            double t = 0;
                            if (double.TryParse(row[0].ToString(), out t))
                                date = DateTime.FromOADate(t);
                            else
                                DateTime.TryParse(row[0].ToString(), out date);
                            tdate = date.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            char sep = '/';
                            if (tdate.IndexOf(".") >= 0)
                                sep = '.';
                            else if (tdate.IndexOf("-") >= 0)
                                sep = '-';
                            string[] date_vals = tdate.Split(sep);
                            if (date_vals[2].Length == 2)
                                date_vals[2] = "20" + date_vals[2];
                            if(date_vals[1].Length==3)
                            {
                                date_vals[1]= date_vals[1].ToString().ToLower();
                                if (date_vals[1] == "jan")
                                    date_vals[1] = "1";
                                else if (date_vals[1] == "feb")
                                    date_vals[1] = "2";
                                else if (date_vals[1] == "mar")
                                    date_vals[1] = "3";
                                else if (date_vals[1] == "apr")
                                    date_vals[1] = "4";
                                else if (date_vals[1] == "may")
                                    date_vals[1] = "5";
                                else if (date_vals[1] == "jun")
                                    date_vals[1] = "6";
                                else if (date_vals[1] == "jul")
                                    date_vals[1] = "7";
                                else if (date_vals[1] == "aug")
                                    date_vals[1] = "8";
                                else if (date_vals[1] == "sep")
                                    date_vals[1] = "9";
                                else if (date_vals[1] == "oct")
                                    date_vals[1] = "10";
                                else if (date_vals[1] == "nov")
                                    date_vals[1] = "11";
                                else if (date_vals[1] == "dec")
                                    date_vals[1] = "12";


                            }
                            date = new DateTime(int.Parse(date_vals[2]), int.Parse(date_vals[1]), int.Parse(date_vals[0]));
                            tdate = date.ToString("dd/MM/yyyy");
                        }
                    }
                    catch
                    {
                        try
                        {
                            date = DateTime.FromOADate(Convert.ToDouble(tdate));
                            tdate = date.ToString("dd/MM/yyyy");
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    finally
                    {
                        date = new DateTime(date.Year, date.Month, date.Day, txtTime.Value.Hour, txtTime.Value.Minute, txtTime.Value.Second);
                    }


                    if (bank_type == BankType.ProcreditBank)
                    {
                        double.TryParse(row[4].ToString().Replace(',', '.'), out debet);
                        double.TryParse(row[5].ToString().Replace(',', '.'), out credit);
                        purpose = row[3].ToString();
                        if (m_procreditCodeMode)
                        {
                            string code = Convert.ToString(row["TaxCode"]);
                            if (!string.IsNullOrEmpty(code))
                            {
                                KeyValuePair<int, string>? con = GetProgramManager().GetDataManager().GetKeyValuePair<int, string>("SELECT id, ISNULL(name,'') AS name FROM book.Contragents WHERE code='" + code + "'");
                                if (con.HasValue)
                                {
                                    description = con.Value.Value;
                                    contragent_id = con.Value.Key;
                                }
                            }
                        }
                    }
                    if (bank_type == BankType.Constanta)
                    {
                        double.TryParse(row[4].ToString().Replace(',', '.'), out debet);
                        double.TryParse(row[5].ToString().Replace(',', '.'), out credit);
                        corr_account = Convert.ToString(row[1]);
                        purpose = string.Concat(row[1], ".", row[2]);

                    }
                    else if (bank_type == BankType.BankOfGeorgia)
                    {
                        corr_account = row[2].ToString();
                        double.TryParse(row[3].ToString().Replace(" ", ""), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out debet);
                        double.TryParse(row[4].ToString().Replace(" ", ""), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out credit);
                        if (!m_bog_new && !m_bog_ultra_new)
                        {
                            purpose_eng = row[col_n - 1].ToString();
                            purpose = AcadnusxToUnicode(purpose_eng);
                        }
                        else
                            purpose = row[5].ToString();

                    }
                    else if (bank_type == BankType.TBC)
                    {
                        double.TryParse(row[3].ToString().Replace(',', '.'), out debet);
                        double.TryParse(row[4].ToString().Replace(',', '.'), out credit);
                        purpose = row[1].ToString() + "." + row[2].ToString();
                    }
                    else if (bank_type == BankType.Cartu)
                    {
                        double.TryParse(row[4].ToString().Replace(',', '.'), out debet);
                        double.TryParse(row[5].ToString().Replace(',', '.'), out credit);
                        if (m_cartu_ultra_new)
                        {
                            double.TryParse(row[3].ToString().Replace(',', '.'), out debet);
                            double.TryParse(row[5].ToString().Replace(',', '.'), out credit);
                        }

                        if (debet <= 0 && credit <= 0)
                            continue;
                        if (!m_cartu_new && !m_cartu_ultra_new)
                            purpose = ConvertGeorgianStringRusU(row[8].ToString() + "." + row[9].ToString());
                        else
                            purpose = (row[1].ToString() + "." + row[2].ToString());
                    }
                    else if (bank_type == BankType.Liberty)
                    {
                        double.TryParse(row[2].ToString().Replace(',', '.'), out debet);
                        double.TryParse(row[3].ToString().Replace(',', '.'), out credit);
                        purpose = row[6].ToString() + "." + row[8].ToString();
                    }
                    else if (bank_type == BankType.Tao)
                    {
                        double am = 0;
                        double.TryParse(row[3].ToString().Replace(',', '.'), out am);
                        if (am < 0)
                            debet = -am;
                        else
                            credit = am;
                        purpose = row[6].ToString() + "." + row[7].ToString();
                    }
                    else if (bank_type == BankType.TeraBank)
                    {
                        corr_account = row[2].ToString();
                        double.TryParse(row[17].ToString().Replace(',', '.'), out debet);
                        double.TryParse(row[19].ToString().Replace(',', '.'), out credit);
                        purpose = row[8].ToString() + "." + row[24].ToString(); //GeoArialToUnicode(row[8].ToString() + "." + row[9].ToString()) ;
                    }


                    else if (bank_type == BankType.BasisBank)
                    {
                        corr_account = row[1].ToString();
                        if (!m_bazis_new)
                        {
                            double.TryParse(row[4].ToString().Replace(',', '.'), out debet);
                            double.TryParse(row[5].ToString().Replace(',', '.'), out credit);
                            string bad_string = row[8].ToString() + "." + row[9].ToString();
                            purpose = ConvertGeorgianStringRusU(bad_string);//GeoArialToUnicode(bad_string);//ConvertGeorgianStringRusU(bad_string); // 
                        }
                        else
                        {
                            double.TryParse(row[13].ToString().Replace(',', '.'), out debet);
                            double.TryParse(row[16].ToString().Replace(',', '.'), out credit);
                            purpose = row[20].ToString();
                        }
                    }

                    else if (bank_type == BankType.VTB)
                    {
                        if(row.ItemArray.Count()==6)
                        {
                            double.TryParse(row[1].ToString().Replace(',', '.'), out debet);
                            double.TryParse(row[2].ToString().Replace(',', '.'), out credit);
                            purpose = row[4].ToString() + "." + row[5].ToString(); 
                            if (debet <= 0 && credit <= 0)
                                continue;



                        }
                        else
                        {
                            double.TryParse(row[4].ToString().Replace(',', '.'), out debet);
                            double.TryParse(row[5].ToString().Replace(',', '.'), out credit);
                            purpose = Oris2Unicode(row[8].ToString() + "." + row[9].ToString()); //ConvertGeorgianStringRusU(row[8].ToString() + "." + row[9].ToString());//row[8].ToString() + "." + row[9].ToString(); 
                            if (debet <= 0 && credit <= 0)
                                continue;
                            if (purpose == "." && row[1].ToString() == "" && row[2].ToString() == "" && row[3].ToString() == "")
                                continue;
                        }
                    }
                    else if (bank_type == BankType.BankRepublic)
                    {

                        double.TryParse(row[4].ToString().Replace(',', '.'), out debet);
                        double.TryParse(row[5].ToString().Replace(',', '.'), out credit);
                        purpose = row[1].ToString() + "." + row[2].ToString();
                    }

                    else if (bank_type == BankType.CaucasianBank)
                    {
                        double.TryParse(row[1].ToString().Replace(',', '.'), out debet);
                        double.TryParse(row[3].ToString().Replace(',', '.'), out credit);
                        purpose = row[7].ToString() + "." + row[8].ToString();
                    }

                    else if (bank_type == BankType.SilkRoadBank)
                    {
                        double.TryParse(row[4].ToString().Replace(',', '.'), out debet);
                        double.TryParse(row[5].ToString().Replace(',', '.'), out credit);
                        corr_account = row[8].ToString();
                        purpose = row[1].ToString() + "." + row[9].ToString();
                    }

                    else if (bank_type == BankType.HalykBankGeorgia)
                    {
                        double.TryParse(row[10].ToString().Replace(',', '.'), out debet);
                        double.TryParse(row[11].ToString().Replace(',', '.'), out credit);
                        corr_account = row[7].ToString();
                        purpose = row[7].ToString()+"."+ row[14].ToString();
                    }


                    //read date

                    string text = PrepareString(purpose);//.ToLower().Replace("შპს", "").Replace("შ.პ.ს.", "").Replace("ი/მ", "").Replace("ი.მ", "").Replace("ი.მ.", "").Replace(",", "").Replace(".", "").Replace(":", "").Replace("'", "").Replace("\"", "").Replace(" ", "");
                    int bank_account_id = Convert.ToInt32(comboBank.SelectedValue);
                    double amount = debet + credit;
                    if (purpose.IndexOf("კონვერსია (კროს-კურსი") >= 0)
                    {
                        description = " ";
                        type = "conversion";
                        type_name = "კონვერსია";
                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                    }
                    else if (purpose.IndexOf("საკომისიო") >= 0 || purpose.IndexOf("გადარიცხვის საკომისიო") >= 0 || purpose.IndexOf("საკომისიო/Commission") >= 0 || purpose.IndexOf("ნაღდის გატანის საკომისიო") >= 0 || purpose.IndexOf("ინტერნეტ ბანკინგით მომსახურების საკომისიო") >= 0 || purpose.IndexOf("ინტერნეტ-ბანკის სისტემით სარგებლობის საკომისიო") >= 0 || purpose.IndexOf("საკასო მომსახურების საკომისიო") >= 0 || purpose.IndexOf("დიჯიპასის საკომისიო") > 0 || purpose == "დიგიპასის მომსახურების საკომისიო" || purpose == "მიმდინარე ანგარიშების მომსახურების საკომისიოს ჩამოჭრა" || purpose.IndexOf("თვის ანგარიშის მომსახურების საკომისიოს ჩამოჭრა") >= 0 || purpose.IndexOf("მიმდინარე ანგარიშების მომსახურების საკომისიოს ჩამოჭრა") >= 0 || purpose.IndexOf("თანხის განაღდების საკომისიო") >= 0)
                    {
                        description = " ";
                        type = "comission";
                        type_name = "ბანკის საკომისიო";
                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());

                    }
                    else if (bank_type == BankType.BankOfGeorgia && (corr_account == "6402900000GEL01" || corr_account == "6404900017GEL01" || corr_account == "6404000000GEL02" || corr_account == "64049810401400000000" || corr_account == "64099811331400000000" || (corr_account.StartsWith("6.4") && corr_account.ToUpper().EndsWith("+19")) || corr_account == "6.40498115401E+19"))
                    {
                        description = "-";
                        type_name = "ბანკის საკომისიო";
                        type = "comission";
                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                    }
                    else if (bank_type == BankType.BankOfGeorgia && purpose.IndexOf("საშემოსავლო გადასახადი") >= 0)
                    {
                        description = "საშემოსავლოს გადასახადი";
                        type_name = "საშემოსავლოს გადასახადი";
                        type = "staff_tax";

                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                    }
                    else if (purpose.IndexOf("დამქირავებლის მიერ დაკავებული საშემოსავლო გადასახადი არასაბიუჯეტო სექტორიდან") >= 0)
                    {
                        description = "საშემოსავლოს გადასახადი";
                        type_name = "საშემოსავლოს გადასახადი";
                        type = "staff_tax";
                        
                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                    }
                    else if (purpose.IndexOf("გადასახადი საქართველოს საწარმოების სხვა მოგებიდან ") >= 0)
                    {
                        description = " ";
                        type_name = "მოგების გადასახადი";
                        type = "income_tax";
                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                    }
                    else if ((purpose.IndexOf("გადახდა ორგ:") >= 0 || purpose.IndexOf("ყიდვა/Purchase via POS terminal") >= 0) && credit>0 )
                    {
                        description = bank_name;
                        type = "bank_money_in";
                        type_name = "თანხის მიღება ბანკიდან";
                        string path = GetProgramManager().GetDataManager().GetContragentGroupPathByTag("BANKTERMINAL");
                        contragent_id = GetProgramManager().GetDataManager().GetIntegerValue("SELECT id FROM book.Contragents WHERE path LIKE '"+path+"%' AND name LIKE N'%"+bank_name +"%'");
                        description = GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id);
                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                    }
                    else if (purpose.IndexOf("გადატანა:") >= 0 && purpose_eng.IndexOf("GE") > 0)
                    {
                        description = bank_name;
                        type = "bank_to_bank";
                        type_name = "ბანკიდან ბანკში თანხის გადატანა";
                        contragent_id = 0;
                        int b_acc = purpose_eng.IndexOf("GE");
                        string acc = purpose_eng.Substring(b_acc, 21);
                        contragent_id = GetProgramManager().GetDataManager().GetIntegerValue("SELECT  a.id FROM  book.CompanyAccounts a  where a.currency_id=1 AND a.account LIKE '%" + acc+"%'");
                        description = GetProgramManager().GetDataManager().GetStringValue("SELECT  b.name FROM   book.CompanyAccounts a  INNER JOIN book.Banks b ON a.bank_id = b.id   where a.currency_id=1 AND a.account LIKE '%" + acc + "%'");
                       
                        int bank_from = bank_account_id;
                        int bank_to = contragent_id;
                         amount = debet;
                        if (credit > 0)
                        {
                            bank_from = contragent_id;
                            bank_to = bank_account_id;
                            amount = credit;

                        }
                        exist = IsDocumentExist(type, date, bank_to, bank_from, amount.ToString());
                    }
                                
                    else if (purpose.IndexOf("გადასახადი საქართველოს საწარმოთა ქონებაზე (გარდა მიწისა)") >= 0)
                    {
                        description = " ";
                        type_name = "მოგების გადასახადი";
                        type = "inventory_tax";
                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                    }
                    else if (purpose.IndexOf("საშემოსავლო გადასახადი ფიზიკური პირის მიერ ქონების იჯარით გაცემის შედეგად მიღებული შემოსავლებიდან") >= 0)
                    {
                        description = " ";
                        type_name = "ფიზიკური პირების საშემოსავლოს გადასახადი";
                        type = "person_tax";
                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                    }
                    else if (purpose.IndexOf("დღგ საქართველოს ტერიტორიაზე რეზიდენტის მიერ გაწეული მომსახურებიდან") >= 0)
                    {
                        description = " ";
                        type_name = "დღგ-ს გადასახადი";
                        type = "vat_tax";
                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                    }
                    else if (purpose.IndexOf("დღგ იმპორტირებული პროდუქციიდან") >= 0)
                    {
                        description = " ";
                        type_name = "იმპორტის დღგ-ს გადასახადი";
                        type = "import_vat_tax";
                        exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                    }
                    else if(purpose.IndexOf("ხელფასი") >= 0 && debet>0)
                    {
                        int k = 0;
                        for (k = 0; k < staff_n; k++)
                        {
                            if (text.IndexOf(staff_names[k]) >= 0 || text.IndexOf(staff_reverse_names[k]) >= 0)
                                break;
                        }

                        if (k < staff_n)
                        {
                            description = staff_original_names[k];
                            contragent_id = staff_ids[k];
                            type = "staff_salary";
                            type_name = "ხელფასის გადარიცხვა";
                            exist = IsDocumentExist(type, date, contragent_id, bank_account_id, amount.ToString());
                        }
                    }
                        
                    else if (credit > 0)
                    {
                        int i = 0;
                        for (i = 0; i < customer_n; i++)
                        {
                            //if (bank_type == BankType.KORStandart && text.IndexOf(customer_ids[i].ToString()) >= 0)
                             //   break;
                            if (text.IndexOf(customer_names[i]) >= 0 || text.IndexOf(customer_reverse_names[i]) >= 0)
                                break;
                        }

                        if (i < customer_n)
                        {
                            description = customer_original_names[i];
                            contragent_id = customer_ids[i];
                            type = "customer_money";
                            type_name = "თანხის მიღება";
                            exist = IsDocumentExist(type, date, contragent_id, bank_account_id,amount.ToString());
                        }
                        else
                        {
                            for (i = 0; i < staff_n; i++)
                            {
                                if (text.IndexOf(staff_names[i]) >= 0 || text.IndexOf(staff_reverse_names[i]) >= 0)
                                    break;
                            }

                            if (i < staff_n)
                            {
                                description = staff_original_names[i];
                                contragent_id = staff_ids[i];
                                type = "staff_money_in";
                                type_name = "თანხის მიღება თანამშრომლისგან";
                                exist = IsDocumentExist(type, date, contragent_id, bank_account_id, amount.ToString());
                            }
                            else
                            {
                                type = "customer_money";
                                type_name = "თანხის მიღება";

                            }
                            

                        }
                    }
                    else if (debet > 0)
                    {


                        int i = 0;
                        for (i = 0; i < vendor_n; i++)
                        {
                            if (text.IndexOf(vendor_names[i]) >= 0 || text.IndexOf(vendor_reverse_names[i]) >= 0)
                                break;
                        }

                        if (i < vendor_n)
                        {
                            description = vendor_original_names[i];
                            contragent_id = vendor_ids[i];
                            type = "vendor_money";
                            type_name = "თანხის გადარიცხვა";
                            exist = IsDocumentExist(type, date, contragent_id,bank_account_id, amount.ToString());
                        }
                        else
                        {
                            for (i = 0; i < staff_n; i++)
                            {
                                if (text.IndexOf(staff_names[i]) >= 0 || text.IndexOf(staff_reverse_names[i]) >= 0)
                                    break;
                            }

                            if (i < staff_n)
                            {
                                description = staff_original_names[i];
                                contragent_id = staff_ids[i];
                                type = "staff_salary";
                                type_name = "ხელფასის გადარიცხვა";
                                exist = IsDocumentExist(type, date, contragent_id, bank_account_id, amount.ToString());
                            }
                            else
                            {
                                type = "vendor_money";
                                type_name = "თანხის გადარიცხვა";

                            }
                        }
                    }
                    int exist2 = 0;
                    if (type_name == "" || description == "")
                    {
                        exist2 = IsDocumentExistByAmount(date, amount.ToString());
                        if (exist2>0)
                            status = "სავარაოდოდ ჩამოტვირთულია";
                        else
                            status = "უცნობია";
                    }
                    else
                    {
                        if (exist>0)
                            status = "ჩამოტვირთულია";
                        else
                            status = "არ არის ჩამოტვირთული";
                    }
               
                    string debet_str = "",credit_str="";
                    if (debet > 0)
                        debet_str = debet.ToString("F2");
                    if (credit > 0)
                        credit_str = credit.ToString("F2");

                    int analyticID;
                    string analyticCode;
                    GetAnalyticInfoByType(type, contragent_id, debet, credit, out analyticID, out analyticCode);

                    //procreditbank
                    int cur_index = m_Grid.Rows.Add(0,date, purpose, debet_str.ToString(), credit_str, description, contragent_id.ToString(), status,type,type_name,"0","-1");
                    m_Grid.Rows[cur_index].Cells[ColAnalyticID.Index].Value = analyticID.ToString();
                    m_Grid.Rows[cur_index].Cells[ColAnalyticText.Index].Value = analyticCode;

                    int _cureent_project = project_id;
                    string _cureent_project_name = project_name;
                    if (exist > 0)
                    {
                        m_Grid.Rows[cur_index].Cells[ColGeneralID.Index].Value = exist;
                        var dt = GetProgramManager().GetDataManager().GetKeyValuePair<int, string>("SELECT p.id, p.name FROM book.Projects AS p INNER JOIN doc.generaldocs AS g ON g.project_id=p.id WHERE g.id=" + exist);
                        if (dt.HasValue)
                        {
                            _cureent_project = dt.Value.Key;
                            _cureent_project_name = dt.Value.Value;
                        }
                    }

                    m_Grid.Rows[cur_index].Cells[ColProjectId.Index].Value = _cureent_project;
                    m_Grid.Rows[cur_index].Cells[ColProjectName.Index].Value = _cureent_project_name;

                    if (type_name == "")
                        m_Grid.Rows[m_Grid.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightBlue;
                    else if (status == "უცნობია")
                        m_Grid.Rows[m_Grid.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    else if (status == "სავარაოდოდ ჩამოტვირთულია")
                    {
                        m_Grid.Rows[m_Grid.Rows.Count - 1].DefaultCellStyle.BackColor = Color.DarkGreen;
                        m_Grid.Rows[cur_index].Cells[ColGeneralID.Index].Value = exist2;
                    }
                    else
                    {
                        if (exist>0)
                            m_Grid.Rows[m_Grid.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGreen;
                        else
                            m_Grid.Rows[m_Grid.Rows.Count - 1].DefaultCellStyle.BackColor = Color.White;
                    }

                }
                //m_Grid.DataSource = excelData;
                comboBank.Enabled = false;
                btnClear.Enabled = true;
            }
        }


        private void fillComboFilter()
        {
            string[] data = m_Grid.Columns.OfType<DataGridViewColumn>().Where(c => !string.IsNullOrEmpty(c.HeaderText) && c.Visible).Select(c => c.HeaderText).ToArray<string>();
            comboFilter.Items.AddRange(data);
            comboFilter.SelectedIndex = 2;
        }

        private void GetAnalyticInfoByType(string operation_type, int contragent_id,  double debet, double credit, out int analytic_id, out string analytic_code)
        {
             analytic_id = 0;
             analytic_code = string.Empty;
             if (m_docTypes == null || !m_docTypes.ContainsKey(operation_type))
                return;
             int doc_id=m_docTypes[operation_type].Key;
            Dictionary<int, string> data = GetProgramManager().GetDataManager().GetDictionary<int, string>(
                                           @"SELECT  a.id, a.name FROM book.AnalyticCodes AS a 
                                              INNER JOIN  doc.GeneralDocs AS gd ON gd.analytic_code=a.id
                                              WHERE gd.doc_type=" + doc_id + "  AND gd.param_id2=" + contragent_id + "   ORDER BY gd.id DESC");
             if (data.Count > 0)
             {
                 analytic_id = data.Keys.ElementAt(0);
                 analytic_code = data.Values.ElementAt(0);
             }

             else
             {
                string sql = @"SELECT TOP 1 a.id, a.name 
                                FROM book.AnalyticCodes AS a
                                INNER JOIN book.ObjectAnalyticCodes AS oa ON oa.analitic_code_id=a.id
                                WHERE oa.operation_tag='{0}' ";

                data = GetProgramManager().GetDataManager().GetDictionary<int, string>(string.Format(sql, m_docTypes[operation_type].Value));
                //string path = debet > credit ? "0#1#3" : "0#1#2";
                //data = GetProgramManager().GetDataManager().GetSingleIDAndNameValue("SELECT id,name FROM book.AnalyticCodes WHERE path='" + path + "'");
                if (data.Count > 0)
                {
                    analytic_id = data.Keys.ElementAt(0);
                    analytic_code = data.Values.ElementAt(0);
                }
            }


            
        }

        public string Oris2Unicode(string str)
        {

            string result = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {

                    case 'À':
                        {
                            result += "ა";
                            break;
                        }
                    case 'Á':
                        {
                            result += "ბ";
                            break;
                        }
                    case 'Â':
                        {
                            result += "გ";
                            break;
                        }
                    case 'Ã':
                        {
                            result += "დ";
                            break;
                        }
                    case 'Ä':
                        {
                            result += "ე";
                            break;
                        }
                    case 'Å':
                        {
                            result += "ვ";
                            break;
                        }

                    case 'Æ':
                        {
                            result += "ზ";
                            break;
                        }

                    case 'È':
                        {
                            result += "თ";
                            break;
                        }
                    case 'É':
                        {
                            result += "ი";
                            break;
                        }
                    case 'Ê':
                        {
                            result += "კ";
                            break;
                        }
                    case 'Ë':
                        {
                            result += "ლ";
                            break;
                        }
                    case 'Ì':
                        {
                            result += "მ";
                            break;
                        }
                    case 'Í':
                        {
                            result += "ნ";
                            break;
                        }
                    case 'Ï':
                        {
                            result += "ო";
                            break;
                        }
                    case 'Ð':
                        {
                            result += "პ";
                            break;
                        }
                    case 'Ñ':
                        {
                            result += "ჟ";
                            break;
                        }
                    case 'Ò':
                        {
                            result += "რ";
                            break;
                        }
                    case 'Ó':
                        {
                            result += "ს";
                            break;
                        }
                    case 'Ô':
                        {
                            result += "ტ";
                            break;

                        }
                    case 'Ö':
                        {
                            result += "უ";
                            break;
                        }
                    case '×':
                        {
                            result += "ფ";
                            break;
                        }
                    case 'Ø':
                        {
                            result += "ქ";
                            break;
                        }
                    case 'Ù':
                        {
                            result += "ღ";
                            break;
                        }
                    case 'Ú':
                        {
                            result += "ყ";
                            break;
                        }
                    case 'Û':
                        {
                            result += "შ";
                            break;
                        }
                    case 'Ü':
                        {
                            result += "ჩ";
                            break;
                        }
                    case 'Ý':
                        {
                            result += "ც";
                            break;
                        }

                    case 'Þ':
                        {
                            result += "ძ";
                            break;
                        }
                    case 'ß':
                        {
                            result += "წ";
                            break;
                        }
                    case 'à':
                        {
                            result += "ჭ";
                            break;
                        }
                    case 'á':
                        {
                            result += "ხ";
                            break;
                        }
                    case 'ã':
                        {
                            result += "ჯ";
                            break;
                        }
                    case 'ä':
                        {
                            result += "ჰ";
                            break;
                        }

                    default:
                        {
                            result += str[i];
                            break;
                        }
                }
            }
            return result;
        }


        private string AcadnusxToUnicode(string source)
        {
            string result = string.Empty;
            for (int i = 0; i < source.Length; i++)
            {
                switch (source[i])
                {
                    case 'a':
                        {
                            result += "ა";
                            break;
                        }
                    case 'b':
                        {
                            result += "ბ";
                            break;
                        }
                    case 'c':
                        {
                            result += "ც";
                            break;
                        }
                    case 'd':
                        {
                            result += "დ";
                            break;
                        }
                    case 'e':
                        {
                            result += "ე";
                            break;
                        }
                    case 'f':
                        {
                            result += "ფ";
                            break;
                        }

                    case 'g':
                        {
                            result += "გ";
                            break;
                        }

                    case 'h':
                        {
                            result += "ჰ";
                            break;
                        }
                    case 'i':
                        {
                            result += "ი";
                            break;
                        }
                    case 'j':
                        {
                            result += "ჯ";
                            break;
                        }
                    case 'k':
                        {
                            result += "კ";
                            break;
                        }
                    case 'l':
                        {
                            result += "ლ";
                            break;
                        }
                    case 'm':
                        {
                            result += "მ";
                            break;
                        }
                    case 'n':
                        {
                            result += "ნ";
                            break;
                        }
                    case 'o':
                        {
                            result += "ო";
                            break;
                        }
                    case 'p':
                        {
                            result += "პ";
                            break;
                        }
                    case 'q':
                        {
                            result += "ქ";
                            break;
                        }
                    case 'r':
                        {
                            result += "რ";
                            break;
                        }
                    case 's':
                        {
                            result += "ს";
                            break;

                        }
                    case 't':
                        {
                            result += "ტ";
                            break;
                        }
                    case 'u':
                        {
                            result += "უ";
                            break;
                        }
                    case 'v':
                        {
                            result += "ვ";
                            break;
                        }
                    case 'w':
                        {
                            result += "წ";
                            break;
                        }
                    case 'x':
                        {
                            result += "ხ";
                            break;
                        }
                    case 'y':
                        {
                            result += "ყ";
                            break;
                        }
                    case 'z':
                        {
                            result += "ზ";
                            break;
                        }
                    case 'C':
                        {
                            result += "ჩ";
                            break;
                        }

                    case 'J':
                        {
                            result += "ჟ";
                            break;
                        }
                    case 'R':
                        {
                            result += "ღ";
                            break;
                        }
                    case 'S':
                        {
                            result += "შ";
                            break;
                        }
                    case 'T':
                        {
                            result += "თ";
                            break;
                        }
                    case 'W':
                        {
                            result += "ჭ";
                            break;
                        }
                    case 'Z':
                        {
                            result += "ძ";
                            break;
                        }
                   
                    default:
                        {
                            result += source[i];
                            break;
                        }
                }
            }
            return result;
        }
        private string GeoArialToUnicode(string src)
        {
            Dictionary<string, string> chars = new Dictionary<string, string>();
            chars.Add("À", "ა"); 
            chars.Add("Á", "ბ");
            chars.Add("Â", "გ");
            chars.Add("Ã", "დ");
            chars.Add("Ä", "ე");
            chars.Add("Å", "ვ");
            chars.Add("Æ", "ზ");
            chars.Add("È", "თ");
            chars.Add("É", "ი");
            chars.Add("Ê", "კ");
            chars.Add("Ë", "ლ");
            chars.Add("Ì", "მ");
            chars.Add("Í", "ნ");
            chars.Add("Ï", "ო");
            chars.Add("Ð", "პ");
            chars.Add("Ñ", "ჟ");
            chars.Add("Ò", "რ");
            chars.Add("Ó", "ს");
            chars.Add("Ô", "ტ");
            chars.Add("Ö", "უ");
            chars.Add("×", "ფ");
            chars.Add("Ø", "ქ");
            chars.Add("Ù", "ღ");
            chars.Add("Ú", "ყ");
            chars.Add("Û", "შ");
            chars.Add("Ü", "ჩ");
            chars.Add("Ý", "ც");
            chars.Add("Þ", "ძ");
            chars.Add("ß", "წ");
            chars.Add("à", "ჭ");
            chars.Add("á", "ხ");
            chars.Add("ã", "ჯ");
            chars.Add("ჰ", "ჰ");
            string dest = string.Empty;
            for (int i = 0; i < src.Length; i++)
            {
                if (chars.ContainsKey(src[i].ToString()))
                    dest += chars[src[i].ToString()];
                else
                    dest += src[i].ToString();
            }
            return dest;
        }

        public string ConvertGeorgianStringRusU(string src)
        {
            Dictionary<string, string> chars = new Dictionary<string, string>();
            chars.Add("А", "ა");
            chars.Add("Б", "ბ");
            chars.Add("В", "გ");
            chars.Add("Г", "დ");
            chars.Add("Д", "ე");
            chars.Add("Е", "ვ");
            chars.Add("Ж", "ზ");
            chars.Add("И", "თ");
            chars.Add("Й", "ი");
            chars.Add("К", "კ");
            chars.Add("Л", "ლ");
            chars.Add("М", "მ");
            chars.Add("Н", "ნ");
            chars.Add("П", "ო");
            chars.Add("Р", "პ");
            chars.Add("С", "ჟ");
            chars.Add("Т", "რ");
            chars.Add("У", "ს");
            chars.Add("Ф", "ტ");
            chars.Add("Ц", "უ");
            chars.Add("Ч", "ფ");
            chars.Add("Ш", "ქ");
            chars.Add("Щ", "ღ");
            chars.Add("Ъ", "ყ");
            chars.Add("Ы", "შ");
            chars.Add("Ь", "ჩ");
            chars.Add("Э", "ც");
            chars.Add("Ю", "ძ");
            chars.Add("Я", "წ");
            chars.Add("а", "ჭ");
            chars.Add("б", "ხ");
            chars.Add("г", "ჯ");
            chars.Add("д", "ჰ");

            string dest = "";
            for (int i = 0; i < src.Length; i++)
            {
                if (chars.ContainsKey(src[i].ToString()))
                    dest += chars[src[i].ToString()];
                else
                    dest += src[i].ToString();
            }
            return dest;
        }

        private string PrepareString(string str)
        {
            return str.ToLower().Replace("შპს", "").Replace("შ.პ.ს.", "").Replace("ი/მ", "").Replace("ინდ/მეწარმე", "").Replace("ი.მ", "").Replace("ი.მ.", "").Replace("ს/ს", "").Replace("ს.ს.", "").Replace("სს ", "").Replace("ს.ს ", "").Replace("-", "").Replace("-", "").Replace(",", "").Replace(".", "").Replace(".", "").Replace(":", "").Replace("'", "").Replace("\"", "").Replace(" ", "");
            //return str.ToLower().Replace("შპს", "").Replace("შ.პ.ს.", "").Replace("ი/მ", "").Replace("ი.მ", "").Replace("ი.მ.", "").Replace("'", "").Replace("\"", "").Trim();//.Replace(" ", "");

        }
        

        private int IsDocumentExist(string type, DateTime date, int id, int bank_account_id, string Amount)
        {
            string date1 = date.ToString("yyyy-MM-dd 00:00:00");
            string date2 = date.ToString("yyyy-MM-dd 23:59:59");
            string s = string.Empty;
            double amount = double.Parse(Amount, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            Hashtable s_params = new Hashtable();
            switch (type)
            {
                case "customer_money":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=38 AND tdate BETWEEN @date1 AND @date2  AND param_id1=@param_id1 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }
                case "vendor_money":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=39 AND tdate BETWEEN @date1 AND @date2 AND param_id1=@param_id1  AND  param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }
                case "comission":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=51 AND tdate BETWEEN @date1 AND @date2  AND param_id1=@param_id1 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1",-bank_account_id); s_params.Add("@amount", amount);
                        break;
                    }
                case "conversion":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=2 AND tdate BETWEEN @date1 AND @date2  AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@amount", amount);
                        break;
                    }
                case "staff_tax":
                    {
                        int account_id = GetProgramManager().GetDataManager().GetAccountIDByCode("3320.1");
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=47 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1  AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", account_id); s_params.Add("@amount", amount);
                        break;
                    }
                case "person_tax":
                    {
                        int account_id = GetProgramManager().GetDataManager().GetAccountIDByCode("3320.3");
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=47 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", account_id); s_params.Add("@amount", amount);
                        break;
                    }
                case "income_tax":
                    {
                        int account_id = GetProgramManager().GetDataManager().GetAccountIDByCode("3310");
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=47 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", account_id); s_params.Add("@amount", amount);
                        break;
                    }
                case "inventory_tax":
                    {
                        int account_id = GetProgramManager().GetDataManager().GetAccountIDByCode("3380");
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=47 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1  AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", account_id); s_params.Add("@amount", amount);
                        break;
                    }
                case "vat_tax":
                    {
                        int account_id = GetProgramManager().GetDataManager().GetAccountIDByCode("3330");
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=47 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1  AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", account_id); s_params.Add("@amount", amount);
                        break;

                    }
                case "import_vat_tax":
                    {
                        id = GetProgramManager().GetDataManager().GetIntegerValue("SELECT TOP 1 id FROM book.Contragents WHERE path LIKE '0#1#6%' AND name LIKE N'საბაჟო%'");
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=39 AND tdate BETWEEN @date1 AND @date2  AND  param_id1=@param_id1 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }
                case "staff_money_in":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=45 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }
                case "staff_money_out":
                    {
                        s = "SELECT g.id FROM doc.GeneralDocs AS g INNER JOIN doc.operation AS o ON o.general_id=g.id WHERE g.doc_type=44 AND ISNULL(o.type,0)==0 AND g.tdate BETWEEN @date1 AND @date2  AND  g.param_id1=@param_id1  AND g.param_id2=@param_id2 AND g.amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }

                case "staff_money_outM":
                    {
                        s = "SELECT g.id FROM doc.GeneralDocs AS g INNER JOIN doc.operation AS o ON o.general_id=g.id WHERE g.doc_type=44 AND ISNULL(o.type,0)==2 AND g.tdate BETWEEN @date1 AND @date2  AND  g.param_id1=@param_id1  AND g.param_id2=@param_id2 AND g.amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }
                case "staff_money_outV":
                    {
                        s = "SELECT g.id FROM doc.GeneralDocs AS g INNER JOIN doc.operation AS o ON o.general_id=g.id WHERE g.doc_type=44 AND ISNULL(o.type,0)==1 AND g.tdate BETWEEN @date1 AND @date2  AND  g.param_id1=@param_id1  AND g.param_id2=@param_id2 AND g.amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }
                case "bank_money_in":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=38 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }
                case "staff_salary":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=48 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }
                case "customer_advance":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=49 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }
                case "vendor_advance":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=50 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }
                case "cash_to_bank":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=41 AND tdate BETWEEN @date1 AND @date2 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id2", -bank_account_id); s_params.Add("@amount", amount);
                        break;
                    }

                case "bank_to_cash":
                    {
                        s = "SELECT id FROM doc.GeneralDocs WHERE doc_type=40 AND tdate BETWEEN @date1 AND @date2 AND  param_id1=@param_id1 AND param_id2=@param_id2 AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@param_id1", -bank_account_id); s_params.Add("@param_id2", id); s_params.Add("@amount", amount);
                        break;
                    }


                case "bank_to_bank":
                    {
                        s = @"SELECT g.id FROM doc.GeneralDocs AS g 
                             INNER JOIN doc.BankTransfer AS bt ON bt.general_id=g.id  WHERE g.doc_type=5 AND tdate BETWEEN @date1 AND @date2 
                             AND bt.accountFrom=@accountFrom AND bt.accountTo=@accountTo AND amount=@amount";
                        s_params.Add("@date1", date1); s_params.Add("@date2", date2); s_params.Add("@accountFrom", bank_account_id); s_params.Add("@accountTo",id); s_params.Add("@amount", amount);
                        break;
                    }


                default: { return -1; }

            }
            return GetProgramManager().GetDataManager().GetIntegerValue(s, s_params);

        }



        private int IsDocumentExistByAmount(DateTime date, string amount)
        {
            string date1 = date.ToString("yyyy-MM-dd 00:00:00");
            string date2 = date.ToString("yyyy-MM-dd 23:59:59");
            string s = "";
            s = "SELECT id FROM doc.GeneralDocs WHERE tdate BETWEEN '" + date1 + "' AND '" + date2 + "' AND amount=" + amount.Replace(",", ".");
            int res = GetProgramManager().GetDataManager().GetIntegerValue(s);

            return res;

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OnLoad();
        }

        private void m_Grid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(!OnViewItem())
                OnSelect();
        }

        private void OnSelect()
        {
            if (m_Grid.SelectedRows.Count == 0)
                return;
            int index= m_Grid.SelectedRows[0].Index;
            int bank_account_id = Convert.ToInt32(comboBank.SelectedValue);
            int contragent_id = int.Parse(m_Grid.Rows[index].Cells["ColContragentID"].Value.ToString());
            string type = m_Grid.Rows[index].Cells["ColType"].Value.ToString();
            if (type == "customer_money")
                contragent_id = GetProgramManager().ShowSelectForm("TABLE_CONTRAGENT:CUSTOMER", contragent_id);
            else if (type == "vendor_money")
                contragent_id = GetProgramManager().ShowSelectForm("TABLE_CONTRAGENT:VENDOR", contragent_id);
            else 
                return;
            if (contragent_id > 0)
            {
                m_Grid.Rows[index].Cells["ColContragentID"].Value = contragent_id;
                m_Grid.Rows[index].Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id);
                string[] date_vals = m_Grid.Rows[index].Cells["ColDate"].Value.ToString().Split('/');
                DateTime date = new DateTime(int.Parse(date_vals[2]), int.Parse(date_vals[1]), int.Parse(date_vals[0]));
                double debet = 0;
                if(m_Grid.Rows[index].Cells["ColDebet"].Value!=null)
                    double.TryParse(m_Grid.Rows[index].Cells["ColDebet"].Value.ToString(), out debet);
                double credit = 0;
                if (m_Grid.Rows[index].Cells["ColCredit"].Value != null)
                    double.TryParse(m_Grid.Rows[index].Cells["ColCredit"].Value.ToString(), out credit);
                int exist = IsDocumentExist(type, date, contragent_id, bank_account_id,(debet + credit).ToString());
               
                if (exist>0)
                {
                    m_Grid.Rows[index].DefaultCellStyle.BackColor = Color.LightGreen;
                    m_Grid.Rows[index].Cells["ColStatus"].Value = "ჩამოტვირთულია";
                    m_Grid.Rows[index].Cells["ColGeneralID"].Value = exist;
                }
                else
                {
                    m_Grid.Rows[index].DefaultCellStyle.BackColor = Color.White;
                    m_Grid.Rows[index].Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
                }
                
            }

        }

        private void btnFile_Click_1(object sender, EventArgs e)
        {
            OnLoad();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            OnExecute();
        }

        private void OnExecute()
        {
            if (MessageBox.Show(GetProgramManager().GetTranslatorManager().Translate("შესრულდეს ოპერაციების ჩატვირთვა?"), 
            GetProgramManager().GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                return;
            }
            BusinessLogic bLogic = new BusinessLogic();
            bLogic.Initialize(GetProgramManager().GetDataManager());
            int bank_account_id= int.Parse(comboBank.SelectedValue.ToString());
            int user_id = GetProgramManager().GetUserID();
            m_Grid.EndEdit();
            m_GridResult.Rows.Clear();
            int cnt = 0;
            
            foreach (DataGridViewRow row in m_Grid.Rows)
            {
                if (!row.Visible)
                    continue;
               
                string check = row.Cells["ColCheck"].Value.ToString().ToLower();
                if (check != "true")
                    continue;
                if (row.Cells["ColStatus"].Value.ToString() == "ჩამოტვირთულია")
                    continue;
                DataGridViewRow row2 = row;
                try
                {
                    if (row.Tag != null)
                    {
                        row2 = (DataGridViewRow) row.Tag ;
                    }
                }
                catch
                {
                }
                if (row2.Cells["ColTypeName"].Value.ToString() == "")
                    continue;
                if (row2.Cells["ColStatus"].Value.ToString() == "უცნობია")
                    continue;
                string type = row2.Cells["ColType"].Value.ToString();
                int contragent_id = int.Parse(row2.Cells["ColContragentID"].Value.ToString());
                DateTime date = Convert.ToDateTime(row.Cells["ColDate"].Value);
               // string[] date_vals = row.Cells["ColDate"].Value.ToString().Split('/');
               // DateTime date = new DateTime(int.Parse(date_vals[2]), int.Parse(date_vals[1]), int.Parse(date_vals[0]), DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                int analytic_id = 0;
                if (m_bIsAnalytic)
                {
                    try
                    { int.TryParse(row2.Cells[ColAnalyticID.Index].Value.ToString(), out analytic_id); }
                    catch
                    { }
                }

                double debet = 0;
                if (row.Cells["ColDebet"].Value != null)
                    double.TryParse(row.Cells["ColDebet"].Value.ToString(), out debet);
                double credit = 0;
                if (row.Cells["ColCredit"].Value != null)
                    double.TryParse(row.Cells["ColCredit"].Value.ToString(), out credit);

                string purpose = row.Cells["ColPurpose"].Value.ToString();
                string description = row.Cells["ColDescription"].Value.ToString();
                int project_id = Convert.ToInt32(row.Cells[ColProjectId.Index].Value);





                int res = -1;
                if (type == "customer_money" && credit > 0)
                {
                    if (!NeedAdvance(contragent_id, date))
                        res = bLogic.Insert_CustomerMoneyIn(date, purpose, credit, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, true);
                    else
                        res = bLogic.Insert_CustomerAdvance(date, purpose, credit, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, 2, 0, true);



                    
                }
                else if (type == "vendor_money" && debet > 0)
                {
                    res = bLogic.Insert_VendorMoneyOut(date, purpose, debet, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, true);

                }
                else if (type == "pay_credit" && debet > 0)
                {
                    res = bLogic.Insert_PayCredit(date, purpose, debet, 1, 1, -bank_account_id, contragent_id, project_id, user_id, analytic_id, true);

                }
                else if (type == "pay_credit_percent" && debet > 0)
                {
                    res = bLogic.Insert_PayCreditPercent(date, purpose, debet, 1, 1, -bank_account_id, contragent_id, project_id, user_id, analytic_id, true);

                }
                else if (type == "staff_salary" && debet > 0)
                {

                    res = bLogic.Insert_StaffSalary(date, purpose, debet, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, true);

                }
                else if (type == "comission" && debet > 0)
                {

                    res = bLogic.Insert_BankComission(date, purpose, debet, 1, 1, -bank_account_id, project_id, user_id, analytic_id, true);

                }
                else if (type == "staff_tax" && debet > 0)
                {
                    res = bLogic.Insert_PayTax(date, purpose, debet, -bank_account_id, "3320.1", project_id, user_id, analytic_id, true);
                }
                else if (type == "person_tax" && debet > 0)
                {

                    res = bLogic.Insert_PayTax(date, purpose, debet, -bank_account_id, "3320.3", project_id, user_id, analytic_id, true);

                }
                else if (type == "income_tax" && debet > 0)
                {
                    res = bLogic.Insert_PayTax(date, purpose, debet, -bank_account_id, "3310", project_id, user_id, analytic_id, true);
                }
                else if (type == "inventory_tax" && debet > 0)
                {
                    res = bLogic.Insert_PayTax(date, purpose, debet, -bank_account_id, "3380", project_id, user_id, analytic_id, true);
                }
                else if (type == "vat_tax" && debet > 0)
                {
                    res = bLogic.Insert_PayTax(date, purpose, debet, -bank_account_id, "3330", project_id, user_id, analytic_id, true);
                }//TOFIX
                else if (type == "staff_money_in" && credit > 0)
                {
                    res = bLogic.Insert_StaffMoneyIn(date, purpose, credit, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, true);
                }
                else if (type == "staff_money_out" && debet > 0)
                {
                    res = bLogic.Insert_StaffMoneyOut(date, purpose, debet, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, /*analytic_id*/0, 0, true);
                }
                else if (type == "staff_money_outM" && debet > 0)
                {
                    res = bLogic.Insert_StaffMoneyOut(date, purpose, debet, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, 2, true);
                }
                else if (type == "staff_money_outV" && debet > 0)
                {
                    res = bLogic.Insert_StaffMoneyOut(date, purpose, debet, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, 1, true);
                }
                else if (type == "bank_money_in" && credit > 0)
                {
                    if (!NeedAdvance(contragent_id, date))
                        res = bLogic.Insert_CustomerMoneyIn(date, purpose, credit, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, true);
                    else
                        res = bLogic.Insert_CustomerAdvance(date, purpose, credit, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, 2, 0, true);
                }
                else if (type == "import_vat_tax" && debet > 0)
                {
                    contragent_id = GetProgramManager().GetDataManager().GetIntegerValue("SELECT TOP 1 id FROM book.Contragents WHERE path LIKE '0#1#6%' AND name LIKE N'საბაჟო%'");
                    if (contragent_id > 0)
                        res = bLogic.Insert_VendorMoneyOut(date, purpose, debet, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, true);

                }
                else if (type == "conversion")
                {
                    /////////
                }

                else if (type == "customer_money_out")
                {
                    res = bLogic.Insert_CustomerMoneyOut(date,  purpose, debet, 1, 1, 18, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, true);
                }

                else if (type == "customer_advance" && credit > 0)
                {
                    res = bLogic.Insert_CustomerAdvance(date, purpose, credit, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, 2, 0, true);

                }
                else if (type == "vendor_advance" && debet > 0)
                {
                    res = bLogic.Insert_VendorAdvance(date, purpose, debet, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, 2,0, 0,0,true);

                }
                else if (type == "cash_to_bank" && credit > 0)
                {
                    res = bLogic.Insert_CashBank(date, purpose, credit, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, true);

                }
                else if (type == "bank_to_cash" && debet > 0)
                {
                    res = bLogic.Insert_BankCash(date, purpose, debet, 1, 1, -bank_account_id, description, contragent_id, project_id, user_id, analytic_id, true);

                }
                else if (type == "bank_to_bank" && (debet > 0 || credit > 0))
                {
                    int bank_from = bank_account_id;
                    int bank_to = contragent_id;
                    double amount = debet;
                    if (credit > 0)
                    {
                        bank_from = contragent_id;
                        bank_to = bank_account_id;
                        amount = credit;

                    }
                    res = bLogic.Insert_BankBank(date, purpose, amount, 1, 1, bank_from, bank_to, description, project_id, user_id, analytic_id, true);

                }
                else continue;

                if (res > 0)
                {

                    if (new string[] { "customer_advance", "customer_money" }.Contains(type) && GetProgramManager().GetConfigParamValue("StaffViaCustomer") == "1")
                    {
                        int staff_id = GetProgramManager().GetDataManager().GetDistributorByContragent(contragent_id);
                        if (staff_id > 0)
                            GetProgramManager().GetDataManager().ExecuteSql("UPDATE doc.generaldocs set staff_id=" + staff_id + " WHERE id=" + res + " UPDATE doc.operation SET person_id=" + staff_id + " WHERE general_id = " + res);
                    }

                    splitContainer1.Panel2Collapsed = false;

                    string s = "SELECT gd.id,gd.tdate,gd.purpose,dt.name, dt.tag, gd.amount FROM doc.GeneralDocs gd INNER JOIN config.DocTypes dt ON dt.id=gd.doc_type WHERE gd.id=" + res;
                    DataTable data = GetProgramManager().GetDataManager().GetTableData(s);
                    if (data == null || data.Rows.Count == 0)
                    {
                        continue;

                    }
                    DataRow r = data.Rows[0];
                    m_GridResult.Rows.Add(r["id"].ToString(), r["tag"].ToString(), r["tdate"].ToString(), r["name"].ToString(), r["purpose"].ToString(), r["amount"].ToString());

                    int exist = IsDocumentExist(type, date, contragent_id, bank_account_id,(debet + credit).ToString());
                    row.Cells["ColCheck"].Value = 0;
                    if (exist > 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                        row.Cells["ColStatus"].Value = "ჩამოტვირთულია";
                        row.Cells["ColGeneralID"].Value = exist;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
                    }
                    cnt++;
                }

            }

            if (cnt > 0)
            {
                MessageBox.Show(cnt + " "+GetProgramManager().GetTranslatorManager().Translate("დოკუმენტი წარმატებით ჩაიტვირთა!"), 
                GetProgramManager().GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show(GetProgramManager().GetTranslatorManager().Translate("დოკუმენტები ვერ ჩაიტვირთა!"), 
                GetProgramManager().GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


            }

        }

        private bool NeedAdvance(int contragent_id, DateTime tdate)
        {
            if (GetProgramManager().GetConfigParamValue("EntryTimeControl") != "1")
                return false;

            string account = GetProgramManager().GetDataManager().GetContragentAccount(contragent_id);
            double debet = 0, credit = 0;
            GetProgramManager().GetDataManager().GetAccountValueSimple(account, "", tdate, contragent_id, out debet, out credit, true);
            double _debt = debet - credit;
            return Math.Round(_debt, 2) <= 0;
        }

        private void btnViewResult_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OnEditResult();
        }

        private void OnEditResult()
        {
            if(m_GridResult.SelectedRows.Count==0)
                return;
            int index = m_GridResult.SelectedRows[0].Index;
            int id=int.Parse(m_GridResult.Rows[index].Cells["ColResultID"].Value.ToString());
            string tag = m_GridResult.Rows[index].Cells["ColResultTag"].Value.ToString();
            GetProgramManager().ExecuteCommandByTag("SHOW_"+tag + ":" + id);

        }

        private void m_GridResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnEditResult();
        }

        private void btnMoneyIn_Click(object sender, EventArgs e)
        {
            OnOperationMoneyIn();
        }

     

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            OnEnableMenuItems();
        }
        private DataGridViewRow GetSelectedRow()
        {
            if (m_Grid.SelectedRows.Count <= 0)
                return null;
            DataGridViewRow row = null;
            for (int i = 0; i < m_Grid.SelectedRows.Count; i++)
            {
                DataGridViewRow row2 = m_Grid.SelectedRows[i];
             if(!row2.Visible)
                    continue;
                row= m_Grid.SelectedRows[i];
            }
            return row;
        }
        private List<DataGridViewRow> GetSelectedRows()
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            if (m_Grid.SelectedRows.Count <= 0)
                return null;
            for (int i = 0; i < m_Grid.SelectedRows.Count; i++)
            {
                DataGridViewRow row2 = m_Grid.SelectedRows[i];
                if (!row2.Visible)
                    continue;
                rows.Add(m_Grid.SelectedRows[i]);
            }
            return rows;
        }

        private void მონიშვნაToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckDocs();
        }
        private void CheckDocs()
        {
            foreach (DataGridViewRow row in m_Grid.SelectedRows)
            {
                if (!row.Visible)
                    continue;
                row.Cells[0].Value = true;
            }

        }
        private void UnCheckDocs()
        {
            foreach (DataGridViewRow row in m_Grid.SelectedRows)
            {
                if (!row.Visible)
                    continue;
                row.Cells[0].Value = false;
            }

        }

        private void მონიშვნისმოხსნაToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UnCheckDocs();
        }
        private void btnStaffMoneyIn_Click(object sender, EventArgs e)
        {
            OnOperationStaffMoneyIn();
        }
        private void OnEnableMenuItems()
        {
            if (m_Grid.SelectedRows.Count <= 0)
                return;
            btnMoneyIn.Visible = false;
            btnStaffMoneyIn.Visible = false;
            btnBankMoneyIn.Visible = false;
            btnCustomerMoneyOut.Visible = false;

            btnMoneyOut.Visible = false;
            btnStaffSalary.Visible = false;
            btnStaffMoneyOut.Visible = false;
            btnStaffMoneyOutM.Visible = false;
            btnStaffMoneyOutV.Visible = false;
            btnBankComission.Visible = false;
            btnCredit.Visible = false;
            btnCreditPercent.Visible = false;
            btnBudget.Visible = false;

            btnAdvanceIn.Visible = false;
            btnAdvanceOut.Visible = false;
            btnCashToBank.Visible = false;
            btnBankToCash.Visible = false;
           


            DataGridViewRow row = GetSelectedRow();
            if (row == null)
                return;

            btnViewItem.Visible = false;
            try
            {
                if (row.Cells[ColGeneralID.Index].Value != null)
                {
                    int idd = 0;
                    int.TryParse(row.Cells[ColGeneralID.Index].Value.ToString(), out idd);
                    
                    if (idd > 0)
                        btnViewItem.Visible = true;
                }

            }
            catch
            {

            }


            double d = 0, c =0;
            for (int i = 0; i < m_Grid.SelectedRows.Count; i++)
            {
                DataGridViewRow row2 = m_Grid.SelectedRows[i];
                if (!row2.Visible)
                    continue;
                double v = 0;
                double.TryParse(row2.Cells[ColDebet.Index].Value.ToString(), out v);
                d += v;
                double.TryParse(row2.Cells[ColCredit.Index].Value.ToString(), out v);
                c += v;
                if (d > 0 && c > 0)
                    return;
            }
            if (row.Cells["ColStatus"].Value.ToString() == "ჩამოტვირთულია")
                return;
            double debet = 0;
            double.TryParse(row.Cells["ColDebet"].Value.ToString(), out debet);
            double credit = 0;
            double.TryParse(row.Cells["ColCredit"].Value.ToString(), out credit);
            if (credit > 0)
            {
                btnMoneyIn.Visible = true;
                btnStaffMoneyIn.Visible = true;
                btnBankMoneyIn.Visible = true;
                btnAdvanceIn.Visible = true;
                btnCashToBank.Visible = true;
            }
            else if (debet > 0)
            {
                btnMoneyOut.Visible = true;
                btnStaffSalary.Visible = true;
                btnStaffMoneyOut.Visible = true;
                btnStaffMoneyOutM.Visible = true;
                btnStaffMoneyOutV.Visible = true;
                btnBankComission.Visible = true;
                btnBudget.Visible = true;
                btnCredit.Visible = true;
                btnCreditPercent.Visible = true;
                btnAdvanceOut.Visible = true;
                btnBankToCash.Visible = true;
                btnCustomerMoneyOut.Visible = true;
            }
          

        }
       


        private void OnOperationCustomerMoneyOut()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int contragent_id = 0;
            contragent_id = GetProgramManager().ShowSelectForm("TABLE_CONTRAGENT:CUSTOMER", contragent_id);
            if (contragent_id <= 0)
                return;
            foreach (DataGridViewRow row in rows)
            {
                row.Cells["ColContragentID"].Value = contragent_id;
                row.Cells["ColType"].Value = "customer_money_out";
                row.Cells["ColTypeName"].Value = "თანხის დაბრუნება მყიდველთან";
                row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id);
                row.Cells["ColCheck"].Value = true;
                row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
                row.DefaultCellStyle.BackColor = Color.LightGray;
            }

        }

        private void OnOperationMoneyIn()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int contragent_id = 0;
            contragent_id = GetProgramManager().ShowSelectForm("TABLE_CONTRAGENT:CUSTOMER", contragent_id);
            if (contragent_id <= 0)
                return;
            foreach(DataGridViewRow row in rows)
            {
                row.Cells["ColContragentID"].Value = contragent_id;
                row.Cells["ColType"].Value = "customer_money";
                row.Cells["ColTypeName"].Value = "თანხის მიღება მყიდველისგან";
                row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id);
                row.Cells["ColCheck"].Value = true;
                row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
                row.DefaultCellStyle.BackColor = Color.LightGray;
            }

        }
        private void OnOperationBankMoneyIn()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int contragent_id = 0;
            contragent_id = GetProgramManager().ShowSelectForm("TABLE_CONTRAGENT:BANKTERMINAL;BANKCREDIT", contragent_id);
            if (contragent_id <= 0)
                return;
             foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = contragent_id;
            row.Cells["ColType"].Value = "bank_money_in";
            row.Cells["ColTypeName"].Value = "თანხის მიღება ბანკიდან";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
             }

        }
        private void OnOperationStaffMoneyIn()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int staff_id = 0;
            staff_id = GetProgramManager().ShowSelectForm("TABLE_STAFF", staff_id);
            if (staff_id <= 0)
                return;

              foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = staff_id;
            row.Cells["ColType"].Value = "staff_money_in";
            row.Cells["ColTypeName"].Value = "თანხის მიღება თანამშრომლისგან";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetStaffNameByID(staff_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
              }


        }

        private void btnMoneyOut_Click(object sender, EventArgs e)
        {
            OnOperationMoneyOut();
        }
        private void OnOperationMoneyOut()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int contragent_id = 0;
            contragent_id = GetProgramManager().ShowSelectForm("TABLE_CONTRAGENT:VENDOR", contragent_id);
            if (contragent_id <= 0)
                return;
             foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = contragent_id;
            row.Cells["ColType"].Value = "vendor_money";
            row.Cells["ColTypeName"].Value = "თანხის გაცემა მომწოდებელზე";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
                }

        }
        private void OnOperationCredit()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int credit_id = 0;
            credit_id = GetProgramManager().ShowSelectForm("TABLE_CREDIT", credit_id);
            if (credit_id <= 0)
                return;
              foreach(DataGridViewRow row in rows)
            {
                row.Cells["ColContragentID"].Value = credit_id;
                row.Cells["ColType"].Value = "pay_credit";
                row.Cells["ColTypeName"].Value = "სესხის დაფარვა";
                row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetCreditNameByID(credit_id);
                row.Cells["ColCheck"].Value = true;
                row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
                row.DefaultCellStyle.BackColor = Color.LightGray;
              }

        }
        private void OnOperationCreditPercent()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int credit_id = 0;
            credit_id = GetProgramManager().ShowSelectForm("TABLE_CREDIT", credit_id);
            if (credit_id <= 0)
                return;
                foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = credit_id;
            row.Cells["ColType"].Value = "pay_credit_percent";
            row.Cells["ColTypeName"].Value = "სესხის პროცენტის გადახდა";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetCreditNameByID(credit_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
                }

        }
        private void btnStaffSalary_Click(object sender, EventArgs e)
        {
            OnOperationStaffSalary();

        }
        private void OnOperationStaffSalary()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int staff_id = 0;
            staff_id = GetProgramManager().ShowSelectForm("TABLE_STAFF", staff_id);
            if (staff_id <= 0)
                return;

               foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = staff_id;
            row.Cells["ColType"].Value = "staff_salary";
            row.Cells["ColTypeName"].Value = "ხელფასის გადახდა თანამშრომელზე";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetStaffNameByID(staff_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
               }

        }

        private void btnStaffMoneyOut_Click(object sender, EventArgs e)
        {
            OnOperationStaffMoneyOut();
        }
        private void OnOperationStaffMoneyOut()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int staff_id = 0;
            staff_id = GetProgramManager().ShowSelectForm("TABLE_STAFF", staff_id);
            if (staff_id <= 0)
                return;

              foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = staff_id;
            row.Cells["ColType"].Value = "staff_money_out";
            row.Cells["ColTypeName"].Value = "თანხის გაცემა თანამშრომელზე";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetStaffNameByID(staff_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
              }



        }
        private void OnOperationStaffMoneyOutM()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int staff_id = 0;
            staff_id = GetProgramManager().ShowSelectForm("TABLE_STAFF", staff_id);
            if (staff_id <= 0)
                return;

              foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = staff_id;
            row.Cells["ColType"].Value = "staff_money_outM";
            row.Cells["ColTypeName"].Value = "სამივლინებო თანხის გაცემა";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetStaffNameByID(staff_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
              }



        }

        private void OnOperationStaffMoneyOutV()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int staff_id = 0;
            staff_id = GetProgramManager().ShowSelectForm("TABLE_STAFF", staff_id);
            if (staff_id <= 0)
                return;
              foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = staff_id;
            row.Cells["ColType"].Value = "staff_money_outV";
            row.Cells["ColTypeName"].Value = "თანამშრომლის ვალდებულების დაფარვა";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetStaffNameByID(staff_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
              }



        }


        private void OnOperationBankComission()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
             foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = 0;
            row.Cells["ColType"].Value = "comission";
            row.Cells["ColTypeName"].Value = "ბანკის საკომისიო";
            row.Cells["ColDescription"].Value = "-";// GetProgramManager().GetDataManager().GetStaffNameByID(staff_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
             }



        }

        private void btnBankComission_Click(object sender, EventArgs e)
        {
            OnOperationBankComission();
        }

        private void btnBankMoneyIn_Click(object sender, EventArgs e)
        {
            OnOperationBankMoneyIn();
        }
        private void OnOperationBudget(string tag)
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
                foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = 0;
            row.Cells["ColType"].Value = tag;
            if(tag=="staff_tax")
            {   
                row.Cells["ColTypeName"].Value = "საშემოსავლოს გადასახადი";
                row.Cells["ColDescription"].Value = "საშემოსავლოს გადასახადი";
            }
            if (tag == "income_tax")
            {
                row.Cells["ColTypeName"].Value = "მოგების გადასახადი";
                row.Cells["ColDescription"].Value = "მოგების გადასახადი";
            }
            if (tag == "inventory_tax")
            {
                row.Cells["ColTypeName"].Value = "ქომების გადასახადი";
                row.Cells["ColDescription"].Value = "ქომების გადასახადი";
            }
            if (tag == "vat_tax")
            {
                row.Cells["ColTypeName"].Value = "დღგ-ს გადასახადი";
                row.Cells["ColDescription"].Value = "დღგ-ს გადასახადი";
            }
            if (tag == "income_vat_tax")
            {
                row.Cells["ColTypeName"].Value = "იმპორტის დღგ-ს გადასახადი";
                row.Cells["ColDescription"].Value = "იმპორტის დღგ-ს გადასახადი";
            }
            if (tag == "person_tax")
            {
                row.Cells["ColTypeName"].Value = "ფიზიკური პირების საშემოსავლოს გადასახადი";
                row.Cells["ColDescription"].Value = "ფიზიკური პირების საშემოსავლოს გადასახადი";
            }
            
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
                }


        }

        private void ხელფასისსაშემოსავლოToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnOperationBudget("staff_tax");
        }

        private void ფიზპირისსაშემოსავლოToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnOperationBudget("person_tax");
        }

        private void მოგებისგადასახადიToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnOperationBudget("income_tax");
        }

        private void ქონებისგადასახადიToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnOperationBudget("inventory_tax");
        }

        private void დღგსგადასახადიToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnOperationBudget("vat_tax");

        }

        private void იმპორტისდღგსგადასახადიToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnOperationBudget("income_vat_tax");

        }

        private void btnCredit_Click(object sender, EventArgs e)
        {
            OnOperationCredit();
        }

        private void btnCreditPercent_Click(object sender, EventArgs e)
        {
            OnOperationCreditPercent();
        }

        private void m_Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is Button)
            {

                Button btn = e.Control as Button;

                btn.Click -= new EventHandler(btn_Click);

                btn.Click += new EventHandler(btn_Click);

            }
        }
        void btn_Click(object sender, EventArgs e)
        {

            int col = this.m_Grid.CurrentCell.ColumnIndex;

            int row = this.m_Grid.CurrentCell.RowIndex;

            double debet = 0;
            double credit = 0;
            try
            {
                double.TryParse(m_Grid.Rows[row].Cells[ColDebet.Index].Value.ToString(), out debet);
            }
            catch
            {
            }
            
            try
            {
                double.TryParse(m_Grid.Rows[row].Cells[ColCredit.Index].Value.ToString(), out credit);
            }
            catch
            {
            }

            int res = -1;
            //string tag = "TABLE_ANALYTICCODE:OUT";
            //if (credit > 0)
            //    tag = "TABLE_ANALYTICCODE:IN";
            try
            {
                int.TryParse(m_Grid.Rows[row].Cells[ColAnalyticID.Index].Value.ToString(), out res);
            }
            catch
            {

            }
            res = GetProgramManager().ShowSelectForm("TABLE_ANALYTICCODE", res);
            if (res != -1)
            {
                m_Grid.Rows[row].Cells[ColAnalyticID.Index].Value = res.ToString();
                m_Grid.Rows[row].Cells[ColAnalyticText.Index].Value = 
                    GetProgramManager().GetDataManager().GetStringValue("SELECT name FROM book.AnalyticCodes WHERE id=" + res);
            }


        }

        private void m_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (m_Grid.Columns[e.ColumnIndex].Name)
            {
                case "ColAnalyticBtn":
                    {
                        int row = m_Grid.CurrentCell.RowIndex;
                        double debet = 0;
                        double credit = 0;
                        double.TryParse(Convert.ToString(m_Grid.Rows[row].Cells[ColDebet.Index].Value), out debet);
                        double.TryParse(Convert.ToString(m_Grid.Rows[row].Cells[ColCredit.Index].Value), out credit);
                        int res = -1;
                        //string tag = "TABLE_ANALYTICCODE:OUT";
                        //if (credit > 0)
                        //    tag = "TABLE_ANALYTICCODE:IN";
                        int.TryParse(Convert.ToString(m_Grid.Rows[row].Cells[ColAnalyticID.Index].Value), out res);

                        res = GetProgramManager().ShowSelectForm("TABLE_ANALYTICCODE", res);
                        if (res != -1)
                        {
                            m_Grid.Rows[row].Cells[ColAnalyticID.Index].Value = res;
                            m_Grid.Rows[row].Cells[ColAnalyticText.Index].Value = GetProgramManager().GetDataManager().GetStringValue("SELECT name FROM book.AnalyticCodes WHERE id=" + res);
                        }
                        break;
                    }
                case "ColProjectBtn":
                    {
                        int row = m_Grid.CurrentCell.RowIndex;
                        int current_proj = Convert.ToInt32(m_Grid.Rows[row].Cells[ColProjectId.Index].Value);
                        current_proj = GetProgramManager().ShowSelectForm("TABLE_PROJECT", current_proj);
                        if (current_proj > 0)
                        {
                            m_Grid.Rows[row].Cells[ColProjectId.Index].Value = current_proj;
                            m_Grid.Rows[row].Cells[ColProjectName.Index].Value = GetProgramManager().GetDataManager().GetProjectName(current_proj);
                        }
                        break;
                    }

            }
           
        }
        private void onFilter()
        {
            string letter = txtFilter.Text.Trim();
            string header = comboFilter.SelectedItem.ToString();
            foreach (DataGridViewRow row in m_Grid.Rows)
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

        private void BankStatementForm_Load(object sender, EventArgs e)
        {
            txtFilter.Focus();
        }

        private void OnOperationAdvanceIn()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int contragent_id = 0;
            contragent_id = GetProgramManager().ShowSelectForm("TABLE_CONTRAGENT:CUSTOMER", contragent_id);
            if (contragent_id <= 0)
                return;
                foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = contragent_id;
            row.Cells["ColType"].Value = "customer_advance";
            row.Cells["ColTypeName"].Value = "ავანსის მიღება მყიდველისგან";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
                }

        }
        private void btnAdvanceIn_Click(object sender, EventArgs e)
        {
            OnOperationAdvanceIn();
        }
        private void OnOperationAdvanceOut()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int contragent_id = 0;
            contragent_id = GetProgramManager().ShowSelectForm("TABLE_CONTRAGENT:VENDOR", contragent_id);
            if (contragent_id <= 0)
                return;
                foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = contragent_id;
            row.Cells["ColType"].Value = "vendor_advance";
            row.Cells["ColTypeName"].Value = "ავანსის გაცემა მომწოდებელზე";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
                }

        }
        private void btnAdvanceOut_Click(object sender, EventArgs e)
        {
            OnOperationAdvanceOut();
        }
        private void OnOperationCashToBank()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int cash_id = 0;
            cash_id = GetProgramManager().ShowSelectForm("TABLE_CASH", cash_id);
            if (cash_id <= 0)
                return;
               foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = cash_id;
            row.Cells["ColType"].Value = "cash_to_bank";
            row.Cells["ColTypeName"].Value = "სალაროდან ბანკში თანხის მიღება";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetCashNameByID(cash_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
               }

        }
        private void btnCashToBank_Click(object sender, EventArgs e)
        {
            OnOperationCashToBank();
        }
        private void OnOperationBankToCash()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            int cash_id = 0;
            cash_id = GetProgramManager().ShowSelectForm("TABLE_CASH", cash_id);
            if (cash_id <= 0)
                return;
                 foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = cash_id;
            row.Cells["ColType"].Value = "bank_to_cash";
            row.Cells["ColTypeName"].Value = "ბანკიდან სალაროში თანხის გატანა";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetCashNameByID(cash_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
                 }

        }
        private void btnBankToCash_Click(object sender, EventArgs e)
        {
            OnOperationBankToCash();
        }
        private void OnOperationBankToBank()
        {
            List<DataGridViewRow> rows = GetSelectedRows();
            if (rows == null)
                return;
            GetProgramManager().SetTempString(" WHERE a.id<>" + Convert.ToInt32(comboBank.SelectedValue) + " AND c.id=" + 1);
            int contragent_id = 0;
            contragent_id = GetProgramManager().ShowSelectForm("TABLE_BANKACCOUNTS", contragent_id);
            if (contragent_id <= 0)
                return;
               foreach(DataGridViewRow row in rows)
            {
            row.Cells["ColContragentID"].Value = contragent_id;
            row.Cells["ColType"].Value = "bank_to_bank";
            row.Cells["ColTypeName"].Value = "ბანკიდან ბანკში თანხის გადატანა";
            row.Cells["ColDescription"].Value = GetProgramManager().GetDataManager().GetStringValue("SELECT  b.name FROM    book.CompanyAccounts a  INNER JOIN book.Banks b ON a.bank_id = b.id   where a.id=" + contragent_id);
            row.Cells["ColCheck"].Value = true;
            row.Cells["ColStatus"].Value = "არ არის ჩამოტვირთული";
            row.DefaultCellStyle.BackColor = Color.LightGray;
               }

        }
        private void onForbiddenContragents()
        {
            using (ForbiddenContragentsForm Form = new ForbiddenContragentsForm(GetProgramManager()))
            {
                Form.ShowDialog();
            }
        }
        private void btnBankToBank_Click(object sender, EventArgs e)
        {
            OnOperationBankToBank();
        }

        private void m_Grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
                
        }

        private void btnViewItem_Click(object sender, EventArgs e)
        {
            OnViewItem();
        }
        private bool OnViewItem()
        {
            if (m_Grid.SelectedRows.Count == 0)
                return false;
            int id = 0;
            int index = m_Grid.SelectedRows[0].Index;
            if (m_Grid.Rows[index].Cells[ColGeneralID.Index].Value == null)
                return false;
            int.TryParse(m_Grid.Rows[index].Cells[ColGeneralID.Index].Value.ToString(), out id);
            if (id > 0)
            {
                string sql = "SELECT tp.tag FROm doc.GeneralDocs gd INNER JOIN config.DocTypes tp ON tp.id= gd.doc_type WHERE gd.id=" + id;
                string tag = GetProgramManager().GetDataManager().GetStringValue(sql);
                if (tag != "")
                {
                    GetProgramManager().ExecuteCommandByTag("SHOW_" + tag+":"+id);
                    return true;
                }
            }
            return false;
        }

        private void btnForbiddenContragents_Click(object sender, EventArgs e)
        {
            onForbiddenContragents();
        }
        private void OnClear()
        {
            if (m_Grid.Rows.Count <= 0)
                return;
            if (MessageBox.Show(GetProgramManager().GetTranslatorManager().Translate("შესრულდეს მონაცემების გასუფთავება?"), 
                GetProgramManager().GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            m_Grid.Rows.Clear();
            comboBank.Enabled = true;
            btnClear.Enabled = false;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            OnClear();
        }

        private void btnStaffMoneyOutM_Click(object sender, EventArgs e)
        {
            OnOperationStaffMoneyOutM();
        }

        private void btnStaffMoneyOutV_Click(object sender, EventArgs e)
        {
            OnOperationStaffMoneyOutV();
        }

        private void btnCustomerMoneyOut_Click(object sender, EventArgs e)
        {
            OnOperationCustomerMoneyOut();
        }


        private void btnSetProject_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count <= 0 || m_Grid.SelectedRows.Count <= 0)
                return;

            int current_proj = GetProgramManager().ShowSelectForm("TABLE_PROJECT", -1);
            if (current_proj <= 0)
                return;
            string name = GetProgramManager().GetDataManager().GetProjectName(current_proj);
            foreach (DataGridViewRow row in m_Grid.SelectedRows)
            {
                row.Cells[ColProjectId.Index].Value = current_proj;
                row.Cells[ColProjectName.Index].Value = name;
            }
        }
    }
}
