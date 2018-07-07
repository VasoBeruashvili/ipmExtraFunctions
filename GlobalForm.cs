using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using ipmBLogic;
using ipmPMBasic;
using ipmControls;
using System.Transactions;

namespace ipmExtraFunctions
{
    public partial class GlobalForm : Form
    {
        private class Pitems
        {
            public string uid { get; set; }
            public int id { get; set; }
            public string amount { get; set; }
            public int realize_type { get; set; }
        }



        ProgramManagerBasic ProgramManager;
        Hashtable m_SqlParams = new Hashtable();
        List<Pitems> ProductItems = new List<Pitems>();
        private bool m_blockReciever = false;
        private bool m_blockLocation = false;
        private bool m_blockBrigade = false;


        private struct InfoItems
        {
            public DateTime tdate { get; set; }
            public DateTime sallary_date { get; set; }
            public string contract_number { get; set; }
            public int brigade_id { get; set; }
            public string contragent_GSN { get; set; }
            public string pasport_number { get; set; }
            public string name_kind { get; set; }
            public string region { get; set; }
            public string city { get; set; }
            public string address_I { get; set; }
            public string address_real { get; set; }
            public string tel { get; set; }
            public string degree { get; set; }
            public int recievers { get; set; }
            public int user_id { get; set; }
            public int money_in { get; set; }
            public int cash_id { get; set; }
            public string cash_comment { get; set; }
            public decimal money_amount { get; set; }
            public decimal staff_sallary { get; set; }
            public int location_id { get; set; }


        }
        private string m_LastError { get; set; }

        public GlobalForm(ProgramManagerBasic PM)
        {
            InitializeComponent();
            ProgramManager = PM;
        }
        private ProgramManagerBasic GetProgramManager()
        {
            return ProgramManager;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private bool InitialTable()
        {
            if (!GetProgramManager().GetDataManager().oncheckTable("book", "GlobalInfo"))
                if (!CreateTables(true, false))
                    return false;
            if (!GetProgramManager().GetDataManager().oncheckTable("book", "GlobalProduction"))
                if (!CreateTables(false, true))
                    return false;
            return true;
        }
        private void ResizeWindow()
        {
            int default_height = 690;
            int real_height = 100;
            foreach (Control con in panelDynamic.Controls)
            {
                if (!(con is Panel) || !((Panel)con).Visible)
                    continue;
                real_height += ((Panel)con).Height;
            }
            this.Height = real_height < default_height ? default_height : real_height;
        }
        private void FillBrigades()
        {
            string filter_string = string.Empty;
            if (comboLocation.SelectedIndex == 1)
                filter_string = "WHERE gst.name=N'თბილისის ჯგუფები'";

            DataTable data = GetProgramManager().GetDataManager().GetTableData(@"
                            SELECT staff.id, staff.name FROM book.staff AS  staff 
                            INNER JOIN book.GroupStaff AS gst ON gst.id=staff.group_id
                            INNER JOIN book.staffbrigades AS brigades ON brigades.parent_staff_id=staff.id 
                            " + filter_string + " GROUP BY staff.id, staff.name ORDER BY staff.name");
            m_blockBrigade = true;
            comboTeamNumber.ValueMember = "id";
            comboTeamNumber.DisplayMember = "name";
            comboTeamNumber.DataSource = data;
            m_blockBrigade = false;
        }
        private void FillLocation()
        {
            m_blockLocation = true;
            comboLocation.Items.Clear();
            comboLocation.Items.AddRange(new List<string>() {"რეგიონები","კომერციული" }.ToArray());
            comboLocation.SelectedIndex = 0;
            m_blockLocation = false;
        }
        private void FillDegrees()
        {
            List<string> items = GetProgramManager().GetDataManager().GetMultiStringVal(@"
                    SELECT cus.name FROM book.customparams AS cus 
                    INNER JOIN config.columns AS col ON cus.column_id=col.id 
                    WHERE col.table_id='TABLE_PRODUCT' AND full_name=N'თეფშის გრადუსი'");
            if (items.Count <= 0)
                return;
            comboDegree.Items.Clear();
            comboDegree.Items.AddRange(items.ToArray());
            comboDegree.SelectedIndex = 0;
        }
        private void FillReciever()
        {
            m_blockReciever = true;
            comboRecieverN.Items.Clear();
            comboRecieverN.Items.AddRange(Enumerable.Range(0, 150).Cast<object>().ToArray());
            comboRecieverN.SelectedIndex = 0;
            m_blockReciever = false;
        }
        private void FillPayMethods()
        {
            comboPayMode.Items.Clear();
            comboPayMode.Items.AddRange(new List<string>() { "უსასყიდლო", "ნაღდი" }.ToArray());
            comboPayMode.SelectedIndex = 0;
        }
        private void FillCashPayCombo()
        {
            string filter_string = @" WHERE gc.name <>N'ჩგდ'";
            if(radioChgd.Checked)
                filter_string = @" WHERE gc.name = N'ჩგდ'";
            DataTable data = GetProgramManager().GetDataManager().GetTableData(@"
                            SELECT cash.id, cash.name FROM book.Cashes AS  cash 
                            INNER JOIN book.GroupCashes AS gc ON gc.id=cash.group_id
                            " + filter_string + "  ORDER BY cash.name");
            comboCashType.ValueMember = "id";
            comboCashType.DisplayMember = "name";
            comboCashType.DataSource = data;
            if (radioChgd.Checked)
            {
                string sign = GetProgramManager().GetDataManager().GetStaffNameByID(Convert.ToInt32(comboTeamNumber.SelectedValue));
                if ((comboCashType.DataSource as DataTable).AsEnumerable().Where(a => a.Field<string>("name").Equals(sign)).Any())
                    comboCashType.SelectedValue = GetProgramManager().GetDataManager().GetCashIDByName(sign);

            }
            txtChgdNum.Enabled = radioChgd.Checked;
        }


        private void FillPanels()
        {
            string sql = "SELECT id,path, name FROM book.GroupProducts WHERE parentid='134' ORDER BY order_id ";
            DataTable data = ProgramManager.GetDataManager().GetTableData(sql);

            panelDynamic.Controls.Clear();
            ProductItems.Clear();
            int y = 18; int height = 26;
            int sep_height = 5;
            int label_col = 100;
            int combo_col = 240;
            int edit_col = 160;

            foreach (DataRow r in data.Rows)
            {

                string label_name = r["name"].ToString();
                int n = 1;
                int textbox_size = 60;
                bool is_reciever = false;
                if (label_name == "რესივერი")
                {
                    n = Convert.ToInt32(comboRecieverN.SelectedItem);
                    if (n == 0)
                        continue;
                    if (n > 3)
                        panelDynamic.AutoScroll = true;
                    textbox_size = 140;
                    is_reciever = true;
                }

                for (int i = 1; i <= n; i++)
                {
                    if (n > 1)
                        label_name = "რესივერი " + (i).ToString();

                    Panel panelItem = new Panel();
                    //panelItem.Dock = DockStyle.Top;
                    panelItem.Size = new Size(800, height);
                    panelItem.Location = new Point(0, y);
                    panelDynamic.Controls.Add(panelItem);
                    y += height;
                    //separator
                    Panel panelSeparator = new Panel();
                    //panelSeparator.Dock = DockStyle.Top;
                    panelSeparator.Size = new Size(400, sep_height);
                    panelSeparator.Location = new Point(0, y);
                    panelDynamic.Controls.Add(panelSeparator);
                    y += sep_height;

                    //label
                    Panel panelLabel = new Panel();
                    panelLabel.Size = new Size(label_col, height);
                    panelLabel.Location = new Point(0, 0);
                   // panelLabel.Dock = DockStyle.Left;
                    panelItem.Controls.Add(panelLabel);

                    Label labelTitle = new Label();
                    labelTitle.AutoSize = true;
                    labelTitle.Dock = DockStyle.Right;
                    labelTitle.Text = label_name + ":";
                    ToolTip labeltooltip = new ToolTip();
                    labeltooltip.SetToolTip(labelTitle, label_name);


                    labelTitle.Font = new Font("Microsoft Sans Serif", (float)11.25, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)204);
                    panelLabel.Controls.Add(labelTitle);

                    //combo
                    string uid = !is_reciever ? Guid.NewGuid().ToString() : string.Concat("RECIEVER:", Guid.NewGuid());
                    Panel panelCombo = new Panel();
                    panelCombo.Size = new Size(combo_col, height);
                    panelCombo.Location = new Point(label_col, 0);
                    // panelCombo.Dock = DockStyle.Left;


                    LoockupComboBox comboItems = new LoockupComboBox();
                    comboItems.Dock = DockStyle.Left;
                    comboItems.Size = new Size(combo_col - 10, height);
                    comboItems.Font = new Font("Microsoft Sans Serif", (float)11.25, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)204);

                    string items_sql = "SELECT id,name FROM book.Products WHERE path LIKE '" + r["path"].ToString() + "%' ORDER BY code";
                    DataTable items_data = GetProgramManager().GetDataManager().GetTableData(items_sql);
                    comboItems.DataSource = items_data;
                    comboItems.DisplayMember = "name";
                    comboItems.ValueMember = "id";
                    comboItems.Tag = uid;

                    panelCombo.Controls.Add(comboItems);
                    panelItem.Controls.Add(panelCombo);
                    comboItems.SelectedValueChanged += new EventHandler(comboItems_SelectedValueChanged);
                  

                    //editbox

                    Panel panelTextbox = new Panel();
                    panelTextbox.Size = new Size(textbox_size, height);
                    panelTextbox.Location = new Point(label_col + combo_col, 0);
                    //panelTextbox.Dock = DockStyle.Left;

                    var textBox = !is_reciever ? new TextBoxDecimalInput() : new TextBox();
                    //TextBox textBox = new TextBox();
                    textBox.Dock = DockStyle.Left;
                    textBox.Size = new Size(textbox_size, height);
                    textBox.Font = new Font("Microsoft Sans Serif", (float)11.25, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)204);
                    if (!is_reciever)
                    {
                        textBox.Text = "1";
                    }
                    textBox.Tag = uid;
                    textBox.TextChanged += new EventHandler(textBox_TextChanged);
                    ProductItems.Add(new Pitems() { uid = uid, id = Convert.ToInt32(comboItems.SelectedValue), amount = textBox.Text, realize_type= 0 });
                    panelTextbox.Controls.Add(textBox);
                    panelItem.Controls.Add(panelTextbox);
                    if (comboLocation.SelectedIndex == 1)
                    {
                        if (!is_reciever)
                        {
                            //realize combo
                            Panel panelComborealize = new Panel();
                            panelComborealize.Size = new Size(edit_col, height);
                            panelComborealize.Location = new Point(label_col + combo_col + 75, 0);
                            // panelCombo.Dock = DockStyle.Left;
                            ComboBox comborealize = new ComboBox();
                            comborealize.DropDownStyle = ComboBoxStyle.DropDownList;
                            comborealize.Dock = DockStyle.Left;
                            comborealize.Size = new Size(105, height);
                            comborealize.Font = new Font("Microsoft Sans Serif", (float)11.25, FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)204);
                            comborealize.Items.AddRange(new List<string>() { "ჩამოწერა", "რეალიზაცია" }.ToArray());
                            comborealize.SelectedIndex = 0;
                            comborealize.Tag = uid;
                            panelComborealize.Controls.Add(comborealize);
                            panelItem.Controls.Add(panelComborealize);
                            comborealize.SelectedIndexChanged += new EventHandler(comborealize_SelectedIndexChanged);
                        }

                    }

                }

            }

        }
        private void FillControls()
        {
            FillDegrees();
            FillReciever();
            FillLocation();
            FillBrigades();
            FillPayMethods();
            FillPanels();
        }
        private void InitialControls()
        {
            txtNum.Text = "G";
            txtPassportNumber.Text = "";
            txtCode.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtRegion.Text = "";
            txtPhone.Text = "";
            txtMoneyAmount.Text = "0.00";
            m_LastError = string.Empty;
            txtName.Clear();
            txtAddressSecond.Clear();
            txtPhone.Clear();

            txtDate.Value = new DateTime(txtDate.Value.Year, txtDate.Value.Month, txtDate.Value.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            txtSallaryDate.Value = new DateTime(txtSallaryDate.Value.Year, txtSallaryDate.Value.Month, txtSallaryDate.Value.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            if (GetProgramManager().IsExportVersion())
            {
                btnWeb.Visible = false;
                txtCode.Width = 250;
            }
        }
        private void GlobalForm_Load(object sender, EventArgs e)
        {
            InitialControls();
            FillControls();
            if (!InitialTable())
            {
                MessageBox.Show("ცხრილების ინიციალიზება ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void comboItems_SelectedValueChanged(object sender, EventArgs e)
        {
            object tag = ((LoockupComboBox)sender).Tag;
            if (tag == null || ((LoockupComboBox)sender).DataSource == null)
                return;
            string uid = Convert.ToString(tag);
            int product_id = Convert.ToInt32(((LoockupComboBox)sender).SelectedValue);
            Pitems it = ProductItems.Where(a => a.uid.Equals(uid)).FirstOrDefault();
            if (it != null)
                it.id = product_id;
           
        }

        private void comborealize_SelectedIndexChanged(object sender, EventArgs e)
        {
            object tag = ((ComboBox)sender).Tag;
            if (tag == null)
                return;
            string uid = Convert.ToString(tag);

            int realize_type = Convert.ToInt32(((ComboBox)sender).SelectedIndex);
            Pitems it = ProductItems.Where(a => a.uid.Equals(uid)).FirstOrDefault();
            if (it != null)
                it.realize_type = realize_type;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            object tag = ((TextBox)sender).Tag;
            if (tag == null)
                return;
            string uid = Convert.ToString(tag);
            Pitems it = ProductItems.Where(a => a.uid.Equals(uid)).FirstOrDefault();
            if (it != null)
                it.amount = ((TextBox)sender).Text;
        }

        private void comboRecieverN_SelectedValueChanged(object sender, EventArgs e)
        {
            if (m_blockReciever)
                return;
            FillPanels();
        }
       
        private void btnWeb_Click(object sender, EventArgs e)
        {
            OnCheckCode();
        }

        public void OnCheckCode()
        {
            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
                return;
            string name = "";
            name = GetProgramManager().GetRSService().GetContragentNameByCode(txtCode.Text.Trim());
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show(GetProgramManager().GetTranslatorManager().Translate("კონტრაგენტი მითითებული კოდით ვერ მოიძებნა"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            txtName.Text = name;


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (OnSaveIntoTables())
                Close();
            else
                MessageBox.Show(m_LastError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            if (OnSaveIntoTables())
            {
                InitialControls();
                FillPanels();
            }
            else
                MessageBox.Show(m_LastError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private bool CheckParams()

        {
            m_LastError = string.Empty;
            if (comboTeamNumber.DataSource == null || comboTeamNumber.SelectedValue == null)
            {
                m_LastError = "ბრიგადა არ არის არჩეული!";
                return false;
            }
            if (string.IsNullOrEmpty(txtName.Text.Trim()) || string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                m_LastError = "პირადი ნომერი ან დასახელება არ არის მითითებული!";
                return false;
            }
            if (GetProgramManager().GetDataManager().GetProductIDByCode(txtNum.Text.Trim()) > 0
                    || GetProgramManager().GetDataManager().GetIntegerValue("SELECT count(id) FROM book.GlobalInfo WHERE contract_number='" + txtNum.Text.Trim() + "'") > 0)
            {
                m_LastError = "ხელშეკრულების ნომერი  - " + txtNum.Text.Trim() + " უკვე გამოიყენება!";
                return false;
            }

            if (comboLocation.SelectedIndex==0 &&(!txtNum.Text.Trim().StartsWith("G") || txtNum.Text.Trim().Length < 7))
            {
                m_LastError = "ხელშეკრულების ნომერი  - " + txtNum.Text.Trim() + " არასწორია!";
                return false;
            }
            if (comboPayMode.SelectedIndex != 0 && double.Parse(txtMoneyAmount.Text)<=0 )
            {
                m_LastError = "მიღებული თანხა არასწორია !";
                return false;
            }

            return true;
        }
        
        private bool SaveInventaryRent(InfoItems items, int contragent_id, int inventory_id, int store_idfrom, int currency_id, double rate, int user_id)
        {
          
            int doc_type = 97;
            int staff_id = items.brigade_id;//Convert.ToInt32(comboTeamNumber.SelectedValue);
            int rent_storeID = GetProgramManager().GetDataManager().GetStoreIDByName("გაქირავება");
            if (rent_storeID <= 0)
            {
                m_LastError = "ინვენტარის გადასაცემი საწყობი ვერ მოიძებნა!";
                return false;
            }
            int project_id = GetProgramManager().GetDataManager().GetStoreProjectID(store_idfrom);
            int project_id2 = GetProgramManager().GetDataManager().GetStoreProjectID(rent_storeID);
            int house_id = GetProgramManager().GetDataManager().GetHouseIdByStoreID(store_idfrom);
            string start_address = GetProgramManager().GetDataManager().GetStoreAddressByID(rent_storeID);
            string end_address = GetProgramManager().GetDataManager().GetStoreAddressByID(contragent_id);
            string sender = GetProgramManager().GetDataManager().GetStorePersonByID(rent_storeID);
            string reciever = GetProgramManager().GetDataManager().GetStorePersonByID(contragent_id);
           // double sum_price = ProductItems.Where(b => !b.Key.StartsWith("RECIEVER:")).Sum(a => double.Parse(a.Value.Value) * GetProgramManager().GetDataManager().GetProductPriceByID(a.Value.Key.ToString(), "3"));
            double sum_price = ProductItems.Where(b => !b.uid.StartsWith("RECIEVER:")).Sum(a => double.Parse(a.amount) * GetProgramManager().GetDataManager().GetProductPriceByID(a.id.ToString(), "3"));
            double vatValStr = GetProgramManager().GetDataManager().GetVatPercent();
            string number_prefix = GetProgramManager().GetDataManager().GetStorePrefix(rent_storeID);
            long doc_num = GetProgramManager().GetDataManager().GetDocMaxNumber("DOC_INVENTORY_RENT");
            int general_id = GetProgramManager().GetDataManager().Insert_GeneralDocs(
                items.tdate,//txtDate.Value,
                 number_prefix,
               doc_num,
                doc_type,
               string.Concat("ინვენტარის გადაცემა - კომპლექტი ", items.contract_number),//txtNum.Text.Trim()),
                 sum_price,
                currency_id,
                rate,
                vatValStr,
                user_id,//GetProgramManager().GetUserID(),
                0,
                store_idfrom,
               rent_storeID,
                1, true, project_id, house_id, staff_id);
            if (general_id <= 0)
                return false;
            string sql_update = string.Format("UPDATE doc.GeneralDocs SET contragent_id = {0} WHERE id = {1}", contragent_id, general_id);
            if (!GetProgramManager().GetDataManager().ExecuteSql(sql_update))
                return false;
            string sqlText = @"INSERT INTO doc.InventoryRent
                      (general_id, sender_IdNum, sender_name, transp_start_place, transporter_name,
                      transporter_IdNum, reciever_IdNum, reciever_name, transp_end_place, responsable_person, responsable_person_num,
                      responsable_person_date, transport_number, transport_model, driver_card_number, avto, railway, other, 
                      waybill_id, waybill_type, waybill_cost, delivery_date, waybill_status, transport_begin_date, transport_cost_payer,
                      transport_type_id, is_waybill, driver_name,  activate_date, staff_id, is_foreign, comment,rent_date)
                      VALUES
                      (@general_id, @sender_IdNum, @sender_name, @transp_start_place, @transporter_name,
                      @transporter_IdNum, @reciever_IdNum, @reciever_name, @transp_end_place, @responsable_person, @responsable_person_num,
                      @responsable_person_date, @transport_number, @transport_model, @driver_card_number, @avto, @railway, @other, 
                      @waybill_id, @waybill_type, @waybill_cost, @delivery_date, @waybill_status, @transport_begin_date, @transport_cost_payer,
                      @transport_type_id, @is_waybill, @driver_name,  @activate_date, @staff_id, @is_foreign, @comment,@rent_date)";
            m_SqlParams.Clear();
          
            m_SqlParams.Add("@general_id", general_id);
            m_SqlParams.Add("@sender_IdNum", string.Empty);
            m_SqlParams.Add("@sender_name", sender);
            m_SqlParams.Add("@transp_start_place", start_address);
            m_SqlParams.Add("@transporter_name", string.Empty);
            m_SqlParams.Add("@transporter_IdNum", string.Empty);
            m_SqlParams.Add("@reciever_IdNum", string.Empty);
            m_SqlParams.Add("@reciever_name", reciever);
            m_SqlParams.Add("@transp_end_place", end_address);
            m_SqlParams.Add("@responsable_person", string.Empty);
            m_SqlParams.Add("@responsable_person_num", string.Empty);
            m_SqlParams.Add("@responsable_person_date", items.tdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));//txtDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            m_SqlParams.Add("@transport_number", string.Empty);
            m_SqlParams.Add("@transport_model", string.Empty);
            m_SqlParams.Add("@driver_card_number", string.Empty);
            m_SqlParams.Add("@avto", Convert.ToByte(false));
            m_SqlParams.Add("@railway", Convert.ToByte(false));
            m_SqlParams.Add("@other", Convert.ToByte(true));
            m_SqlParams.Add("@waybill_id", 0);
            m_SqlParams.Add("@waybill_type", 1);
            m_SqlParams.Add("@waybill_cost", string.Empty);
            m_SqlParams.Add("@delivery_date", items.tdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));//txtDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            m_SqlParams.Add("@waybill_status", -1);
            m_SqlParams.Add("@transport_begin_date", items.tdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));//txtDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            m_SqlParams.Add("@transport_cost_payer", 2);
            m_SqlParams.Add("@transport_type_id", 1);
            m_SqlParams.Add("@is_waybill", 1);
            m_SqlParams.Add("@driver_name", string.Empty);
            m_SqlParams.Add("@activate_date", items.tdate.ToString("yyyy-MM-dd HH:mm:ss.fff"));// txtDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            m_SqlParams.Add("@staff_id", staff_id);
            m_SqlParams.Add("@is_foreign", 0);
            m_SqlParams.Add("@comment", string.Empty);
            m_SqlParams.Add("@rent_date", items.tdate); //txtDate.Value);
            if (!GetProgramManager().GetDataManager().ExecuteSql(sqlText, m_SqlParams))
                return false;

            Hashtable hAmounts = new Hashtable();
            int coeff1 = -1;
            int coeff2 = 1;


            double amount = 1;
            // double price = GetProgramManager().GetDataManager().GetProductPriceByID(inventory_id.ToString(), "3");
            //  string vv = currnent_row.Cells[col_ProductTreePath.Index].Value.ToString();
            int unit_id = GetProgramManager().GetDataManager().GetProductUnitID(inventory_id);
            double coeff = GetProgramManager().GetDataManager().GetProductUnitCoeff(inventory_id, unit_id);
            amount = amount / coeff;
            // price = price * coeff;
            ArrayList amounts = new ArrayList(), self_costs = new ArrayList(), in_ids = new ArrayList(), vendor_ids = new ArrayList();
            int res = 1;
            double s_cost = 0.0;
            double qAddAmount = 0;
            if (!hAmounts.ContainsKey(inventory_id))
                hAmounts.Add(inventory_id, amount);
            else
            {
                qAddAmount = Convert.ToDouble(hAmounts[inventory_id].ToString());
                hAmounts[inventory_id] = qAddAmount + amount;
            }
            res = GetProgramManager().GetDataManager().GetProductSelfCostLF(inventory_id, general_id, rent_storeID, items.tdate /*txtDate.Value*/, amount, ref amounts, ref self_costs, ref in_ids, ref vendor_ids, qAddAmount, 0);
            int in_id = 0;
            int vendor_id = 0;
            s_cost = 0;
            if (res > 0)
            {
                s_cost = Convert.ToDouble(self_costs[0]);
                in_id = int.Parse(in_ids[0].ToString());
                vendor_id = int.Parse(vendor_ids[0].ToString());

            }
            if (res == 0)
                res = 1;
            if (res == 1)//out single case - fits to self_costs
            {
                if (!GetProgramManager().GetDataManager().Insert_ProductsFlow(
                    inventory_id, /*vv*/string.Empty, general_id,
                    amount, unit_id, s_cost, store_idfrom, /*txtDate.Value*/items.tdate, 0,
                    coeff1, 0, 0, 1, 1, 1, s_cost, 0, 0, 0, "", in_id, vendor_id, 0, 0, ""))
                    return false;

                if (!GetProgramManager().GetDataManager().Insert_ProductsFlow(
                    inventory_id, string.Empty, general_id,
                    amount, unit_id, s_cost, rent_storeID, /*txtDate.Value*/ items.tdate, 0,
                    coeff2, 0, 0, 1, 0, 1, s_cost, 0, 0, 0, "", in_id, vendor_id, 0, 0, ""))
                    return false;
            }
            else
            {

                double v = 0;
                for (int k = 0; k < amounts.Count; k++)
                {
                    v += Convert.ToDouble(amounts[k]) * Convert.ToDouble(self_costs[k]);
                }
                v /= amount;
                if (!GetProgramManager().GetDataManager().Insert_ProductsFlow(
                       inventory_id, string.Empty, general_id,
                       amount, unit_id, v, store_idfrom, /*txtDate.Value*/items.tdate, 0,
                       0, 0, 0, 1, 1, 1, 0.0, 0, 0, 0, "", 0, 0, 0, 0, ""))
                    return false;


                for (int j = 0; j < self_costs.Count; j++)
                {
                    if (!GetProgramManager().GetDataManager().Insert_ProductsFlow(
                           inventory_id, string.Empty, general_id,
                           Convert.ToDouble(amounts[j]), unit_id, 0.0, store_idfrom, /*txtDate.Value*/items.tdate, 0,
                           coeff1, 0, 0, 1, 0, 1, Convert.ToDouble(self_costs[j]), 0, 0, 0, "", int.Parse(in_ids[j].ToString()), int.Parse(vendor_ids[j].ToString()), 0, 0, ""))
                        return false;
                    if (!GetProgramManager().GetDataManager().Insert_ProductsFlow(
                               inventory_id, string.Empty, general_id,
                               Convert.ToDouble(amounts[j]), unit_id, Convert.ToDouble(self_costs[j]), rent_storeID, /*txtDate.Value*/items.tdate, 0,
                               coeff2, 0, 0, 1, 0, 1, Convert.ToDouble(self_costs[j]), 0, 0, 0, "", int.Parse(in_ids[j].ToString()), int.Parse(vendor_ids[j].ToString()), 0, 0, ""))
                        return false;
                }

            }





            if (!GetProgramManager().GetDataManager().ExecuteSql("DELETE FROM doc.Entries where general_id = " + general_id))
                return false;

            //get entry id
           // int entry_id = GetProgramManager().GetDataManager().GetUniqueEntryID();
            //double vat = 0.0;
            int i = 0;
            string sql = string.Format("SELECT product_id, price, amount, self_cost, vat_percent, store_id FROM doc.ProductsFlow WHERE general_id={0}  AND coeff>0", general_id.ToString());
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            //end preparations for expenses
            for (i = 0; i < data.Rows.Count; i++)
            {
                // DataGridViewRow row = m_Grid.Rows[i];

                int product_id = int.Parse(data.Rows[i]["product_id"].ToString());
                string productAccount = GetProgramManager().GetDataManager().GetProductAccount(product_id);
                int productGroupID = GetProgramManager().GetDataManager().GetProductGroupID(product_id);

                double product_quantity = double.Parse(data.Rows[i]["amount"].ToString());
                double self_cost = Convert.ToDouble(data.Rows[i]["self_cost"].ToString());

                if (project_id == project_id2)
                {
                    if (!GetProgramManager().GetDataManager().Insert_Entries(
                    general_id,
                    productAccount,
                    productAccount,
                    self_cost * product_quantity,
                    1.0,
                    product_quantity, product_quantity,
                    product_id, rent_storeID, productGroupID,
                    product_id, store_idfrom, productGroupID,
                    "საქონლის გადაადგილება", project_id, 1))
                        return false;
                  
                }
                else
                {
                    if (!GetProgramManager().GetDataManager().Insert_Entries(
              
                general_id,
                "1696",
                productAccount,
                self_cost * product_quantity,
                1.0,
                product_quantity, product_quantity,
                product_id, rent_storeID, productGroupID,
                product_id, store_idfrom, productGroupID,
                "საქონლის გადაადგილება", project_id, 1))
                        return false;
                    

                    if (!GetProgramManager().GetDataManager().Insert_Entries(
                   
                    general_id,
                    productAccount,
                    "1696",
                    self_cost * product_quantity,
                    1.0,
                    product_quantity, product_quantity,
                    product_id, rent_storeID, productGroupID,
                    product_id, store_idfrom, productGroupID,
                    "საქონლის გადაადგილება", project_id2, 1))
                        return false;
                }

            }


            return true;
        }
        private bool CreateTables(bool info, bool productions)
        {
            if (info)
            {
                if (!GetProgramManager().GetDataManager().ExecuteSql(
                                @"CREATE TABLE book.GlobalInfo
                            (
                            id  INTEGER IDENTITY(1,1) PRIMARY KEY,
                            tdate  DATETIME NOT NULL,
                            sallary_date  DATETIME NOT NULL,
                            contract_number NVARCHAR(70) NOT NULL,
                            brigade_id INT NOT NULL,
                            contragent_GSN NVARCHAR(20) NOT NULL,
                            pasport_number NVARCHAR(50) NOT NULL,
                            name_kind NVARCHAR(50) NOT NULL,
                            region  NVARCHAR(100) NOT NULL,
                            city    NVARCHAR(100) NOT NULL,
                            address_I  NVARCHAR(300) NOT NULL,
                            address_real NVARCHAR(300) NOT NULL, 
                            tel    NVARCHAR(30) NOT NULL, 
                            degree  NVARCHAR(30) NOT NULL,     
                            recievers INT NOT NULL DEFAULT(0),
                            status_id INT NOT NULL DEFAULT(0),
                            user_id INT NOT NULL DEFAULT(1),
                            money_in INT NOT NULL DEFAULT(0),
                            cash_id INT NOT NULL DEFAULT(0),
                            cash_comment NVARCHAR(100) NOT NULL DEFAULT '', 
                            money_amount DEC(18,2) NOT NULL DEFAULT(0),
                            staff_sallary DEC(18,2) NOT NULL DEFAULT(0),
                            location_id INTEGER NOT NULL DEFAULT(0)
                            )"))

                    return false;
            }

            if (productions)
            {
                if (!GetProgramManager().GetDataManager().ExecuteSql(
                            @"CREATE TABLE book.GlobalProduction
                            (
                            id  INTEGER IDENTITY(1,1) PRIMARY KEY,
                            info_id INTEGER NOT NULL,
                            product_id INTEGER NOT NULL, 
                            quantity FLOAT NOT NULL DEFAULT(1),
                            comment  NVARCHAR(100) NOT NULL,
                            realize_type INT NOT NULL DEFAULT(0)
                            )"))

                    return false;
            }



            return true;
        }




        private bool OnSave(InfoItems IItems)
        {
            BusinessLogic logic = new BusinessLogic(GetProgramManager().GetDataManager());
            ProductItems = ProductItems.Where(k => k.uid.StartsWith("RECIEVER:") || Convert.ToDouble(k.amount) > 0).ToList<Pitems>();
            m_LastError = string.Empty;
            GetProgramManager().GetDataManager().Close();
            using (TransactionScope tran = new TransactionScope(new TransactionScopeOption(), new TimeSpan(0, 5, 0)))
            {
                GetProgramManager().GetDataManager().Open();
                //მყიდველის დამატება
                int contragent_id = GetProgramManager().GetDataManager().GetContragentIDByCode(IItems.contragent_GSN);
                if (contragent_id <= 0)
                {
                    string sign = IItems.location_id == 0 ? "პროექტის აბონენტები" : "კომერციული აბონენტები";
                    string path = GetProgramManager().GetDataManager().GetStringValue("SELECT path FROM  book.groupContragents WHERE name = @name", new Hashtable() { { "@name", sign } });
                    if (string.IsNullOrEmpty(path))
                        path = "0#2#5";
                    contragent_id = logic.Insert_Contragent(path, IItems.contragent_GSN, IItems.name_kind, IItems.name_kind, IItems.address_I, IItems.tel, 0, 1);
                    if (contragent_id <= 0)
                    {
                        m_LastError = "აბონემენტის შენახვა ვერ მოხერხდა!";
                        Transaction.Current.Rollback();
                        return false;
                    }
                    string col_name = GetProgramManager().GetDataManager().GetColumnDBName("TABLE_CONTRAGENT", "პასპორტის ნომერი");
                    if (!string.IsNullOrEmpty(col_name))
                    {
                        if (!GetProgramManager().GetDataManager().ExecuteSql("UPDATE book.contragents SET " + col_name + "=@col_name WHERE id=@id", new Hashtable() { { "@col_name", IItems.pasport_number }, { "@id", contragent_id } }))
                        {
                            m_LastError = "პასპორტის ნომრის შენახვა ვერ მოხერხდა!";
                            Transaction.Current.Rollback();
                            return false;
                        }
                    }
                    col_name = GetProgramManager().GetDataManager().GetColumnDBName("TABLE_CONTRAGENT", "რაიონი");
                    if (!string.IsNullOrEmpty(col_name))
                    {
                        if (!GetProgramManager().GetDataManager().ExecuteSql("UPDATE book.contragents SET " + col_name + "=@col_name WHERE id=@id", new Hashtable() { { "@col_name", IItems.region }, { "@id", contragent_id } }))
                        {
                            m_LastError = "რაიონის შენახვა ვერ მოხერხდა!";
                            Transaction.Current.Rollback();
                            return false;
                        }
                    }
                    col_name = GetProgramManager().GetDataManager().GetColumnDBName("TABLE_CONTRAGENT", "ქალაქი");
                    if (!string.IsNullOrEmpty(col_name))
                    {
                        if (!GetProgramManager().GetDataManager().ExecuteSql("UPDATE book.contragents SET " + col_name + "=@col_name WHERE id=@id", new Hashtable() { { "@col_name", IItems.city }, { "@id", contragent_id } }))
                        {
                            m_LastError = "ქალაქის შენახვა ვერ მოხერხდა!";
                            Transaction.Current.Rollback();
                            return false;
                        }
                    }
                    col_name = GetProgramManager().GetDataManager().GetColumnDBName("TABLE_CONTRAGENT", "ფაქტობრივი მისამართი");
                    if (!string.IsNullOrEmpty(col_name))
                    {
                        if (!GetProgramManager().GetDataManager().ExecuteSql("UPDATE book.contragents SET " + col_name + "=@col_name WHERE id=@id", new Hashtable() { { "@col_name", IItems.address_real }, { "@id", contragent_id } }))
                        {
                            m_LastError = "ფაქტობრივი მისამართის შენახვა ვერ მოხერხდა!";
                            Transaction.Current.Rollback();
                            return false;
                        }
                    }
                    col_name = GetProgramManager().GetDataManager().GetColumnDBName("TABLE_CONTRAGENT", "ხელშეკრულების ნომერი");
                    if (!string.IsNullOrEmpty(col_name))
                    {
                        if (!GetProgramManager().GetDataManager().ExecuteSql("UPDATE book.contragents SET " + col_name + "=@col_name WHERE id=@id", new Hashtable() { { "@col_name", IItems.contract_number }, { "@id", contragent_id } }))
                        {
                            m_LastError = "კონტრაგენტის ხელშეკრულების ნომრის შენახვა ვერ მოხერხდა!";
                            Transaction.Current.Rollback();
                            return false;
                        }
                    }
                    //UpdateContragentAccount
                    DataTable d = GetProgramManager().GetDataManager().GetTableData("SELECT id,path,account,account2 FROM book.Contragents WHERE id=" + contragent_id);
                    if (d != null && d.Rows.Count > 0)
                        GetProgramManager().GetDataManager().UpdateContragentsAccounts(d);
                }
                //ძირ. საშუალების შენახვა
                string uid = GetProgramManager().GetDataManager().GetUID();
                int vat = 1;
                int vat_type = 0;
                string k = IItems.location_id == 0 ? "კომპლექტები" : "კომერციული კომპლექტები";
                int group_id = GetProgramManager().GetDataManager().GetIntegerValue("SELECT id FROM book.groupproducts WHERE name=@name", new Hashtable() { { "@name", k } });
                int inventory_id = logic.Insert_Product(uid, IItems.contract_number, string.Concat("კომპლექტი ", IItems.contract_number), group_id, 1, 1, 1, string.Empty,
                                                        0, 0, 0, vat, vat_type, string.Empty, string.Empty, 0, 0, 0);
                if (inventory_id <= 0)
                {
                    m_LastError = "ძირითადი საშუალების შენახვა ვერ მოხერხდა!";
                    Transaction.Current.Rollback();
                    return false;
                }
                // Update Inventory Data 
                string col_degree_name = GetProgramManager().GetDataManager().GetColumnDBName("TABLE_PRODUCT", "თეფშის გრადუსი");
                if (!string.IsNullOrEmpty(col_degree_name))
                    if (!GetProgramManager().GetDataManager().ExecuteSql("UPDATE book.Products SET " + col_degree_name + "=@degree  WHERE id=@inventory_id", new Hashtable() { { "@degree", IItems.degree }, { "@inventory_id", inventory_id } }))
                    {
                        m_LastError = "თეფშის გრადუსის შენახვა  ვერ მოხერხდა!";
                        Transaction.Current.Rollback();
                        return false;
                    }
                int i = 0;
                foreach (Pitems key in ProductItems)
                {
                    if (!key.uid.StartsWith("RECIEVER:"))
                        continue;
                    string val = key.amount;// ProductItems[key].Value;
                    string col = GetProgramManager().GetDataManager().GetColumnDBName("TABLE_PRODUCT", "რესივერი" + (++i));
                    if (!string.IsNullOrEmpty(col))
                        if (!GetProgramManager().GetDataManager().ExecuteSql("UPDATE book.Products SET " + col + "=@col_val WHERE id = @inventory_id", new Hashtable() { { "@col_val", val }, { "@inventory_id", inventory_id } }))
                        {
                            m_LastError = "რესივერის  შენახვა  ვერ მოხერხდა!";
                            Transaction.Current.Rollback();
                            return false;
                        }
                }

                int brigade_id = IItems.brigade_id;
                string brigade_name = GetProgramManager().GetDataManager().GetStaffNameByID(brigade_id);
                int store_id = GetProgramManager().GetDataManager().GetStoreIDByName(brigade_name);
                store_id = store_id > 0 ? store_id : 1;
                int project_id = GetProgramManager().GetDataManager().GetStoreProjectID(store_id);
                int currency_id = 1;
                double rate = 1;
                //წარმოება
                List<Pitems> ProductionPitems = ProductItems.Where(a => a.realize_type == 0).ToList<Pitems>();
                if (ProductionPitems != null && ProductionPitems.Count > 0)
                {
                    ProductsFlowItem[] P_I = new ProductsFlowItem[ProductionPitems.Count + 1];
                    int index = 0;
                    string doc_prefix = GetProgramManager().GetDataManager().GetStorePrefix(store_id);
                    long doc_number = GetProgramManager().GetDataManager().GetDocMaxNumber("DOC_PRODUCTION");
                    double sum_price = ProductionPitems.Where(b => !b.uid.StartsWith("RECIEVER:")).Sum(a => double.Parse(a.amount) * GetProgramManager().GetDataManager().GetProductSelfCost(a.id, store_id, txtDate.Value));

                    Guid UID = Guid.NewGuid();
                    foreach (Pitems Key in ProductionPitems)
                    {
                        int product_id = Key.id;//ProductItems[Key].Key;
                        string uu = GetProgramManager().GetDataManager().GetProductUidById(product_id);
                        if (string.IsNullOrEmpty(uu))
                            return false;
                        P_I[index] = new ProductsFlowItem()
                        {
                            uid = GetProgramManager().GetDataManager().GetProductUidById(product_id),
                            unit_id = GetProgramManager().GetDataManager().GetProductUnitID(product_id).ToString(),
                            quantity = !Key.uid.StartsWith("RECIEVER:") ? double.Parse(Key.amount) : 1,
                            parent_product_id = inventory_id,
                            vat = 0,
                            ref_id = 0,
                            coeff = -1
                        };
                        index++;
                    }
                    P_I[index] = new ProductsFlowItem()
                    {
                        uid = GetProgramManager().GetDataManager().GetProductUidById(inventory_id),
                        unit_id = GetProgramManager().GetDataManager().GetProductUnitID(inventory_id).ToString(),
                        quantity = 1,
                        vat = 0,
                        ref_id = 0,
                        coeff = 1
                    };
                    if (!logic.Insert_Production(UID.ToString(), IItems.tdate, doc_prefix, doc_number.ToString(), string.Concat(new List<string>() { "კომპლექტი ", IItems.contract_number, " <", brigade_name, ">" }.ToArray<string>()), sum_price, currency_id,
                                             rate, 0, store_id, project_id, IItems.user_id, true, 0, P_I))
                    {
                        m_LastError = "წარმოების ოპერაციის  შენახვა  ვერ მოხერხდა!";
                        Transaction.Current.Rollback();
                        return false;
                    }
                    if (!GetProgramManager().GetDataManager().ExecuteSql(@"UPDATE P  SET P.end_date=@EndDate FROM doc.production P JOIN doc.Generaldocs G ON
                    P.general_id=G.id WHERE G.uid=@uid", new Hashtable() { { "@EndDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") }, { "@uid", UID } }))
                    {
                        m_LastError = "წარმოების ოპერაციის დასრულება ვერ მოხერხდა!";
                        Transaction.Current.Rollback();
                        return false;
                    }
                }


                //რეალიზაცია
                List<Pitems> ProductOutPitems = ProductItems.Where(a => a.realize_type == 1).ToList<Pitems>();
                if (ProductOutPitems != null && ProductOutPitems.Count > 0)
                {
                    ProductsFlowItem[] P_I = new ProductsFlowItem[ProductOutPitems.Count];
                    int index = 0;
                    //   double amount=0.0;
                    store_id = store_id > 0 ? store_id : 1;
                    string doc_prefix = GetProgramManager().GetDataManager().GetStorePrefix(store_id);
                    long doc_number = GetProgramManager().GetDataManager().GetDocMaxNumber("DOC_PRODUCTOUT");
                    double sum_price = ProductOutPitems.Where(b => !b.uid.StartsWith("RECIEVER:")).Sum(a => double.Parse(a.amount) * GetProgramManager().GetDataManager().GetProductPriceByID(a.id.ToString(),"3"));

                    Guid UID = Guid.NewGuid();
                    foreach (Pitems Key in ProductOutPitems)
                    {
                        int product_id = Key.id;
                        string uu = GetProgramManager().GetDataManager().GetProductUidById(product_id);
                        if (string.IsNullOrEmpty(uu))
                            return false;
                        P_I[index] = new ProductsFlowItem()
                        {
                            uid = GetProgramManager().GetDataManager().GetProductUidById(product_id),
                            unit_id = GetProgramManager().GetDataManager().GetProductUnitID(product_id).ToString(),
                            quantity = !Key.uid.StartsWith("RECIEVER:") ? double.Parse(Key.amount) : 1,
                            price=GetProgramManager().GetDataManager().GetProductPriceByID(product_id.ToString(),"3"),
                        };
                        index++;
                    }
                    if (!logic.Insert_ProductOut(UID.ToString(), IItems.tdate, doc_prefix, doc_number.ToString(), string.Concat(new List<string>() { "რეალიზაცია ", IItems.contract_number, " <", brigade_name, ">" }.ToArray<string>()), sum_price, currency_id,
                                             rate, 0, 0, store_id, GetProgramManager().GetDataManager().GetContragentPath(contragent_id),GetProgramManager().GetDataManager().GetContragentCodeByID(contragent_id),
                                             GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id),GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id), string.Empty, string.Empty,0,
                                             0,                                       
                                             project_id, IItems.user_id, true, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                             string.Empty, string.Empty, string.Empty, string.Empty, IItems.tdate, string.Empty, string.Empty, string.Empty, Convert.ToByte(false),
                                             Convert.ToByte(false),Convert.ToByte(true),0,3,0, IItems.tdate, -1, 2, 0,IItems.tdate, 0, IItems.tdate, 1, 1,1, string.Empty,
                                             IItems.tdate.AddSeconds(10), brigade_id, 0, string.Empty,0,0,0, P_I))
                    {
                        m_LastError = "რეალიზაციის ოპერაციის  შენახვა  ვერ მოხერხდა!";
                        Transaction.Current.Rollback();
                        return false;
                    }

                }

                //გადაცემა აბონემენტზე (გაქირავება)
                if (!SaveInventaryRent(IItems, contragent_id, inventory_id, store_id, currency_id, rate,IItems.user_id))
                {
                    m_LastError = "აბონემენტზე გადაცემის ოპერაციის შენახვა  ვერ მოხერხდა!";
                    Transaction.Current.Rollback();
                    return false;
                }

                if (IItems.staff_sallary > 0)
                {
                    // ხელფასის გადახდა
                    List<StaffSalaryItems> salaryItems = new List<StaffSalaryItems>();
                    List<int> staffs = new List<int>();
                    staffs.Add(brigade_id);
                    if (GetProgramManager().GetDataManager().isStaffBrigade(brigade_id))
                    {
                        staffs.Clear();
                        staffs.AddRange(GetProgramManager().GetDataManager().GetStaffBrigades(brigade_id));
                    }
                    decimal full_amount = IItems.staff_sallary;

                    //int service_id = GetProgramManager().GetDataManager().GetProductIDByName("მონტაჟი");
                    //if (service_id <= 0) return false;
                    //double full_amount = GetProgramManager().GetDataManager().GetDoubleValue("SELECT ISNULL( SUM(value),0) FROM book.staffservices WHERE staff_id IN(" + string.Join(",", staffs.Select(a => a.ToString()).ToArray()) + ") AND service_id=" + service_id + " AND type=2");
                    string purpose_string = "ხელფასის დარიცხვა - " + brigade_name + " " + IItems.contract_number;
                    foreach (int staff_id in staffs)
                    {
                        decimal serv_staff_amount = full_amount / staffs.Count; //GetProgramManager().GetDataManager().GetDoubleValue("SELECT ISNULL(value,0) AS value FROM book.Staffservices  WHERE staff_id=" + staff_id + " AND service_id=" + service_id + " AND type=2");
                        double percent_val = GetProgramManager().GetDataManager().GetStaffPercentValue(staff_id);
                        salaryItems.Add(new StaffSalaryItems()
                        {
                            amount = (double)serv_staff_amount,
                            credit = 0,
                            extra = 0,
                            food = 0,
                            insurance = 0,
                            others = 0,
                            pensia = 0,
                            percent = percent_val,
                            premia = 0,
                            selfsalary = 0,
                            self_insurance = 0,
                            self_food = 0,
                            salary = (double)serv_staff_amount,
                            salary_full = (double)serv_staff_amount,
                            staff_id = staff_id,
                            extra_columns = new Dictionary<string, double>()
                        });
                    }
                    if (logic.Insert_StaffSalaryOperation(IItems.sallary_date, purpose_string, (double)full_amount, currency_id, rate, project_id, IItems.user_id, true, 0, 0, 0, inventory_id, salaryItems) <= 0)
                    {
                        m_LastError = "ხელფასის დარიცხვის ოპერაციის  შენახვა  ვერ მოხერხდა!";
                        Transaction.Current.Rollback();
                        return false;
                    }
                }

                //თანხის მიღება
                if (IItems.money_in == 1 && IItems.money_amount>0)
                {
                    if (logic.Insert_CustomerMoneyIn(IItems.tdate, string.Concat("თანხის მიღება მყიდველისგან - ", GetProgramManager().GetDataManager().GetContragentNameByID(contragent_id), " ",IItems.cash_comment),
                         Convert.ToDouble(IItems.money_amount), 1, 1, IItems.cash_id, "", contragent_id, project_id, IItems.user_id, -1, true) <= 0)
                    {
                        m_LastError = "თანხის მიღების ოპერაციის  შენახვა  ვერ მოხერხდა!";
                        Transaction.Current.Rollback();
                        return false;
                    }
                }


                 tran.Complete();
            }
            return true;
        }
        private bool OnSaveIntoTables()
        {
            if (!CheckParams())
                return false;

            string sql = @"INSERT INTO book.GlobalInfo(location_id,  tdate,sallary_date, contract_number, brigade_id, contragent_GSN, pasport_number, name_kind, region,  city,
                           address_I, address_real, tel, degree, recievers,  user_id, money_in, cash_id, cash_comment, money_amount, staff_sallary)
                            VALUES
                            (@location_id, @tdate, @sallary_date, @contract_number, @brigade_id,@contragent_GSN, @pasport_number, @name_kind, @region,  @city,
                           @address_I, @address_real, @tel, @degree, @recievers,   @user_id, @money_in, @cash_id, @cash_comment, @money_amount, @staff_sallary)  SELECT SCOPE_IDENTITY()";
            m_SqlParams.Clear();
            m_SqlParams.Add("@location_id", comboLocation.SelectedIndex);
            m_SqlParams.Add("@tdate", txtDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            m_SqlParams.Add("@sallary_date", txtSallaryDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            m_SqlParams.Add("@contract_number", txtNum.Text.Trim());
            m_SqlParams.Add("@brigade_id", comboTeamNumber.SelectedValue);
            m_SqlParams.Add("@contragent_GSN", txtCode.Text.Trim());
            m_SqlParams.Add("@pasport_number", txtPassportNumber.Text.Trim());
            m_SqlParams.Add("@name_kind", txtName.Text.Trim());
            m_SqlParams.Add("@region", txtRegion.Text.Trim());
            m_SqlParams.Add("@city", txtCity.Text.Trim());
            m_SqlParams.Add("@address_I", txtAddress.Text.Trim());
            m_SqlParams.Add("@address_real", txtAddressSecond.Text.Trim());
            m_SqlParams.Add("@tel", txtPhone.Text.Trim());
            m_SqlParams.Add("@degree", comboDegree.SelectedItem);
            m_SqlParams.Add("@recievers", comboRecieverN.SelectedItem);
            m_SqlParams.Add("@user_id", GetProgramManager().GetUserID());
            m_SqlParams.Add("@money_in", comboPayMode.SelectedIndex);
            m_SqlParams.Add("@cash_id", comboPayMode.SelectedIndex > 0 ? comboCashType.SelectedValue : 0);
            m_SqlParams.Add("@money_amount", comboPayMode.SelectedIndex > 0 ? double.Parse(txtMoneyAmount.Text) : 0);
            m_SqlParams.Add("@staff_sallary", decimal.Parse(textStaffSallary.Text));
            m_SqlParams.Add("@cash_comment", radioChgd.Checked ? txtChgdNum.Text : string.Empty);

            GetProgramManager().GetDataManager().Close();
            using (TransactionScope tran = new TransactionScope(new TransactionScopeOption(), new TimeSpan(0, 3, 0)))
            {
                GetProgramManager().GetDataManager().Open();

                int info_id = GetProgramManager().GetDataManager().GetIntegerValue(sql, m_SqlParams);
                if (info_id < 0)
                {
                    Transaction.Current.Rollback();
                    m_LastError = "ვერ მოხერხდა ძირითად ცხრილში შენახვა!";
                    return false;
                }
                ProductItems = ProductItems.Where(k => k.uid.StartsWith("RECIEVER:") || (Convert.ToDouble(k.amount) > 0 && k.id>0)).ToList<Pitems>();
                foreach (Pitems Key in ProductItems)
                {
                    sql = @"INSERT INTO book.GlobalProduction (info_id,product_id,quantity,comment, realize_type) 
                        VALUES
                        (@info_id,@product_id,@quantity,@comment, @realize_type)";
                    m_SqlParams.Clear();
                    m_SqlParams.Add("@info_id", info_id);
                    m_SqlParams.Add("@product_id", Key.id);
                    m_SqlParams.Add("@quantity", !Key.uid.StartsWith("RECIEVER:") ? double.Parse(Key.amount) : 1);
                    m_SqlParams.Add("@comment", !Key.uid.StartsWith("RECIEVER:") ? string.Empty : Key.amount);
                    m_SqlParams.Add("@realize_type", Key.realize_type);
                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql, m_SqlParams))
                    {
                        Transaction.Current.Rollback();
                        m_LastError = "ვერ მოხერხდა ქვე ცხრილში შენახვა!";
                        return false;
                    }
                }
                tran.Complete();
            }
            return true;
        }
        private void onClearTable()
        {
            if (!GetProgramManager().IsUserAdmin())
                return;
            if (MessageBox.Show("შესრულდეს შენახული ოპერაციების წაშლა? ", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                return;

            bool res = GetProgramManager().GetDataManager().ExecuteSql("TRUNCATE TABLE book.GlobalInfo  TRUNCATE TABLE book.GlobalProduction ");
            MessageBox.Show("ცხრილების გასუფთავება" + (!res ? " ვერ" : " წარმატებით ") + " შესრულდა", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

       

        private bool OnExecuteBulk(List<int> Ids)
        {
            ProgressDispatcher.Activate();
            string id_string = string.Join(",", Ids.ConvertAll<string>(Convert.ToString).ToArray());
            string sql = @"SELECT i.id, i.location_id,  i.tdate, i.sallary_date, i.contract_number, i.brigade_id, i.contragent_GSN, i.pasport_number, i.name_kind, i.region,  i.city,
                           i.address_I, i.address_real, i.tel, i.degree, i.recievers, i.user_id, i.money_in, i.cash_id, i.cash_comment, i.money_amount, i.staff_sallary, p.product_id,p.quantity,p.comment, p.realize_type 
                    FROM book.GlobalInfo AS i
                    INNER JOIN book.GlobalProduction AS p ON i.id=p.info_id
                    WHERE i.id IN (" + id_string + ")";
            DataTable full_data = GetProgramManager().GetDataManager().GetTableData(sql);
            if (full_data == null || full_data.Rows.Count <= 0)
                return false;

            var unique_docs = full_data.AsEnumerable().Select(dr => dr[0]).Distinct().OrderBy(m => m);
            ProgramManager.InitialiseLog(new List<string> { "№", "თარიღი", "ხელშეკრულების ნომერი", "სახელი, გვარი", "პირადი ნომერი", "პასპორტის ნომერი", "შედეგი" });
            int count = 0;
            List<int> Deletables = new List<int>();

            foreach (var item in unique_docs)
            {
                IEnumerable<DataRow> rows = full_data.AsEnumerable().Where(a => a[0].Equals(item));     
                DataRow first = rows.First();

                int location_id = first.Field<int>("location_id");
                DateTime tdate = first.Field<DateTime>("tdate");
                DateTime sallary_date = first.Field<DateTime>("sallary_date");
                string contract_number = first.Field<string>("contract_number");
                int brigade_id = first.Field<int>("brigade_id");
                string contragent_GSN = first.Field<string>("contragent_GSN");
                string pasport_number = first.Field<string>("pasport_number");
                string name_kind = first.Field<string>("name_kind");
                string region = first.Field<string>("region");
                string city = first.Field<string>("city");
                string address_I = first.Field<string>("address_I");
                string address_real = first.Field<string>("address_real");
                string tel = first.Field<string>("tel");
                string degree = first.Field<string>("degree");
                int recievers = first.Field<int>("recievers");
                int user_id=first.Field<int>("user_id");
                int money_in = first.Field<int>("money_in");
                int cash_id = first.Field<int>("cash_id");
                string cash_comment = first.Field<string>("cash_comment");
                decimal money_amount = first.Field<decimal>("money_amount");
                decimal staff_sallary = first.Field<decimal>("staff_sallary");

                ProductItems.Clear();
                foreach (DataRow row in rows)
                {
                    string comment = row.Field<string>("comment");
                    int realize_type = row.Field<int>("realize_type");
                    string quantity = string.Empty;
                    string uid = string.Empty;
                    if (!string.IsNullOrEmpty(comment))
                    {
                        quantity = comment;
                        uid = string.Concat("RECIEVER:", Guid.NewGuid());
                    }
                    else
                    {
                        quantity = row.Field<double>("quantity").ToString();
                        uid = Guid.NewGuid().ToString();
                    }
                    ProductItems.Add(new Pitems() { uid = uid, id = row.Field<int>("product_id"), amount = quantity, realize_type = realize_type });
                }

                if (!OnSave(new InfoItems()
                {
                    location_id=location_id,
                    tdate = tdate,
                    sallary_date = sallary_date,
                    contract_number = contract_number,
                    brigade_id = brigade_id,
                    contragent_GSN = contragent_GSN,
                    pasport_number = pasport_number,
                    name_kind = name_kind,
                    region = region,
                    city = city,
                    address_I = address_I,
                    address_real = address_real,
                    tel = tel,
                    degree = degree,
                    recievers = recievers,
                    user_id=user_id,
                    money_in=money_in,
                    cash_id=cash_id,
                    cash_comment=cash_comment,
                    money_amount=money_amount,
                    staff_sallary=staff_sallary
                }))
                {
                    ProgramManager.AddLogFormItem(new List<string> { (++count).ToString(), tdate.ToString("yyyy/MM/dd HH:mm:ss"), contract_number, name_kind, contragent_GSN, pasport_number, m_LastError }, 0);
                    continue;
                }
                ProgramManager.AddLogFormItem(new List<string> { (++count).ToString(), tdate.ToString("yyyy/MM/dd HH:mm:ss"), contract_number, name_kind, contragent_GSN, pasport_number, "წარმატებით შესრულდა" }, 1);
                Deletables.Add(first.Field<int>("id"));
            }
            string deletable_ids = string.Join(",", Deletables.ConvertAll<string>(Convert.ToString).ToArray());
            if (!string.IsNullOrEmpty(deletable_ids))
            {
                if (!GetProgramManager().GetDataManager().ExecuteSql(@"UPDATE book.GlobalInfo SET status_id=1 WHERE id IN(" + deletable_ids + ")"))
                {
                    ProgressDispatcher.Deactivate();
                    GetProgramManager().ShowLogForm();
                    return false;
                }
                  
            }
            ProgressDispatcher.Deactivate();
            GetProgramManager().ShowLogForm();
            return true;
        }
        private void btnDropData_Click(object sender, EventArgs e)
        {
            onClearTable();
        }

        private void btnFillData_Click(object sender, EventArgs e)
        {
            using (GlobalOperationForm form = new GlobalOperationForm(GetProgramManager()))
            {
                if (!form.ShowDialog().Equals(DialogResult.OK))
                    return;
                if (!OnExecuteBulk(form.InfoIDs))
                    Close();

            }
        }

        private void comboLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blockLocation)
                return;
            InitialControls();
            FillBrigades();
            FillPanels();
            comboPayMode.Enabled = true;
            if (comboLocation.SelectedIndex == 0)
            {
                comboPayMode.SelectedIndex = 0;
                comboPayMode.Enabled = false;
            }
        }

        private void comboPayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelPayCashes.Visible = comboPayMode.SelectedIndex == 1;
            if (comboPayMode.SelectedIndex == 1)
                FillCashPayCombo();
        }

        private void radioChgd_CheckedChanged(object sender, EventArgs e)
        {
            FillCashPayCombo();
        }

        private void radioCashe_CheckedChanged(object sender, EventArgs e)
        {
            FillCashPayCombo();
        }

        private void comboTeamNumber_SelectedValueChanged(object sender, EventArgs e)
        {
            if (m_blockBrigade)
                return;
            FillCashPayCombo();
        }


    
    }
}
