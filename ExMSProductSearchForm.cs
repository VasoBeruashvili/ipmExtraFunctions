using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ipmPMBasic;
using ipmControls;
using System.Collections;
using ipmDocForms.ProductsProcessing;

namespace ipmExtraFunctions
{
    
   
    public partial class ExMSProductSearchForm : Form
    {
        
        private bool typesAreFilled = false;
        ProgramManagerBasic ProgramManager;
        Hashtable columnValues = new Hashtable();        
        string priceId;
        double m_CurrencyRate;
        double USDRate;
        double m_DiscountPercent;
        DateTime m_ActionDate;
        int m_StoreID;
        int m_CurrencyID;
        DiscountRoundType m_DiscountRoundType=DiscountRoundType.None;
        Dictionary<int, string> productWithCurrency;
        double m_DiscountRound;
        DataTable dataProducts;
        DataTable DataContragent=null;
        string columnName;
        double discountUSD;         

        private bool saveAndNew=false;
        public bool SaveAndNew
        {
            get { return saveAndNew; }
        }
        public string InvoiceNumber { get; set; }
        public string InvoiceTerm { get; set; }
        public int BankID { get; set; }        
        public string AdditioanlPurpose
        {
            get { return comboProductType.Text; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    comboProductType.SelectedItem = value;
            }
        }       
        public ExMSProductSearchForm(ProgramManagerBasic pm, DataTable data_products, DataTable data_contragents, string price_id, double discount_round, double discount_percent, DateTime action_date, int store_id)
        {
            InitializeComponent();
            
            ProgramManager = pm;
            priceId = price_id;
            m_DiscountRound = discount_round;
            m_DiscountPercent = discount_percent;
            m_CurrencyID = 2;
            m_ActionDate = action_date;
            m_StoreID = store_id;

            fillProductList();
            setControls();
            dataProducts = data_products;
            if (!onSetExistingProducts(data_products))
            {
                MessageBox.Show("ზოგიერთი საქონლის კოდი შეიცავს სიმბოლოს!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            setContragent(data_contragents);
            fillTypeCombo();
            setPanelDefaultState();           
            setCategories();

            setupGrids();
        }
        private void setupGrids()
        {
            gridComplects.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 229, 245);
            gridComplects.DefaultCellStyle.BackColor = Color.White;

            gridProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 229, 245);
            gridProducts.DefaultCellStyle.BackColor = Color.White;

            gridProductList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 229, 245);
            gridProductList.DefaultCellStyle.BackColor = Color.White;

            
        }
        public DataTable getProduct()
        {
            return dataProducts;
        }
        public DataTable getContragent()
        {
            return DataContragent;
        }
        public void getParameters(ref double discount_round, ref double discount_percent)
        {
            discount_round = m_DiscountRound;
            discount_percent = m_DiscountPercent;
        }
        private void setPanelDefaultState()
        {
            tPanelComplects.Visible = true;
            int indexVisible = tPnlMain.GetCellPosition(tPanelComplects).Row;
            tPnlMain.RowStyles[indexVisible].SizeType = SizeType.Percent;
            tPnlMain.RowStyles[indexVisible].Height = 100;
            btnComplects.Tag = "UP";
            btnComplects.Image = Properties.Resources.nav_up_blue;

            tPanelProducts.Visible = false;
            indexVisible = tPnlMain.GetCellPosition(tPanelProducts).Row;
            tPnlMain.RowStyles[indexVisible].SizeType = SizeType.AutoSize;
            btnProducts.Tag = "DOWN";
            btnProducts.Image = Properties.Resources.nav_down_blue;

            gridProducts.Visible = true;
            indexVisible = tPanelBaskets.GetCellPosition(gridProducts).Row;
            tPanelBaskets.RowStyles[indexVisible].SizeType = SizeType.Percent;
            tPanelBaskets.RowStyles[indexVisible].Height = 100;
            btnBasket.Tag = "UP";
            btnBasket.Image = Properties.Resources.nav_up_blue;

            gridComplects.Visible = false;
            indexVisible = tPanelBaskets.GetCellPosition(gridComplects).Row;
            tPanelBaskets.RowStyles[indexVisible].SizeType = SizeType.AutoSize;
            btnComplectBasket.Tag = "DOWN";
            btnComplectBasket.Image = Properties.Resources.nav_down_blue;           
            
        }
        private void setPanelState(TableLayoutPanel pnlOwner, Control conVisible, Control conHide)
        {
            int indexVisible = pnlOwner.GetCellPosition(conVisible).Row;

            pnlOwner.RowStyles[indexVisible].SizeType = SizeType.Percent;
            pnlOwner.RowStyles[indexVisible].Height = 100;

            int indexHide = pnlOwner.GetCellPosition(conHide).Row;
            pnlOwner.RowStyles[indexHide].SizeType = SizeType.AutoSize;
        }
        private void setCategories()
        {
            comboProductType.Items.AddRange(new string[] { "საკეტები", "პროფილები" });
            comboProductType.SelectedIndex = 0;

            comboInFeild.Items.AddRange(new string[] {"კოდი", "დასახელება" });
            comboInFeild.SelectedIndex = 0;
        }
        private void filterProductList(string filterString)
        {
            (gridProductList.DataSource as DataTable).DefaultView.RowFilter = filterString;
            gridProductList.Columns["id"].Visible = false;
        }
        private void fillProductList()
        {           
            columnName = GetProgramManager().GetDataManager().GetColumnDBName("TABLE_PRODUCT", "კატეგორია");
            string additional = string.Empty;
            if (!string.IsNullOrEmpty(columnName))
                additional = string.Format(", prod.{0}", columnName);

            string sql = @"SELECT prod.id, CASE WHEN ISNUMERIC(prod.code)=1 THEN CONVERT(NUMERIC, prod.code) ELSE null END AS num_code, prod.code, prod.name, unit.full_name AS unit_name, prod.vat, prod.default_unit_id AS unit_id, prod.group_id, 
                          prod_prices.manual_currency_id AS currency_id, currency.code AS currency_name " + additional + @" FROM book.Products AS prod INNER JOIN book.Units AS unit ON prod.default_unit_id=unit.id 
                          cross apply (SELECT TOP(1) product_id, manual_currency_id FROM book.ProductPrices WHERE product_id=prod.id) AS prod_prices INNER JOIN book.Currencies AS currency ON currency.id=prod_prices.manual_currency_id
                          ORDER BY num_code"; 

            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);

            if (data == null) return;

            gridProductList.DataSource = data;

            foreach (DataGridViewColumn column in gridProductList.Columns)
            {
                switch (column.Name)
                {
                    case "id": column.Visible = false; break;
                    case "num_code": column.Visible = false; break;
                    case "name": column.HeaderText = "დასახელება"; column.ReadOnly = true; column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; break;
                    case "code": column.HeaderText = "კოდი"; column.ReadOnly = true; column.FillWeight = 25; column.AutoSizeMode = DataGridViewAutoSizeColumnMode.NotSet; break;                    
                    case "unit_name": column.Visible = false; break;
                    case "vat": column.Visible = false; break;
                    case "unit_id": column.Visible = false; break;
                    case "group_id": column.Visible = false; break;
                    case "currency_id": column.Visible = false; break;
                    case "currency_name": column.Visible = false; break;
                }
            }            
            
            if (!string.IsNullOrEmpty(columnName))
                gridProductList.Columns[columnName].Visible = false;

            productWithCurrency = new Dictionary<int, string>();
            productWithCurrency = data.AsEnumerable().ToDictionary(key => key.Field<int>("id"), value => value.Field<string>("currency_name"));
        }
        private void setCurrentProductControls()
        {
            if (gridProductList.SelectedRows.Count == 0) return;

            var row = gridProductList.SelectedRows.Cast<DataGridViewRow>().OrderBy(p => p.Index).FirstOrDefault();

            int product_id = Convert.ToInt32(row.Cells["id"].Value);

            double price = GetProgramManager().GetDataManager().GetProductPriceByID(product_id.ToString(), priceId);

            txtProdUnit.Text = row.Cells["unit_name"].Value.ToString();
            txtProdPrice.Text = price.ToString();
            txtProdCurrency.Text = row.Cells["currency_name"].Value.ToString();

            double balance = GetProgramManager().GetDataManager().GetProductRestOriginal(product_id, m_StoreID, m_ActionDate);
            txtProdBalance.Text = balance.ToString();

            double quantity = Convert.ToDouble(txtProdQuantity.Text);
            txtProdPriceSum.Text = (price * quantity).ToString();

        }
        private void setProductFullPrice()
        {
            double price = Convert.ToDouble(txtProdPrice.Text);
            double quantity = Convert.ToDouble(txtProdQuantity.Text);
            txtProdPriceSum.Text = (price * quantity).ToString();
        }
        private void setContragent(DataTable data)
        {
            if (data==null) return;

            btnContragents.Tag = data.Rows[0].Field<int>("contragent_id");
            txtContragent.Text = data.Rows[0].Field<string>("contragent_name");
        }
        private void setControls()
        {
            labelDiscountPercent.Text = m_DiscountPercent.ToString() + "%";

            txtRate.Enabled = true;
            lblCurrencyName.Text = GetProgramManager().GetDataManager().GetCurrencyCodeByID(m_CurrencyID.ToString());
            m_CurrencyRate = GetProgramManager().GetDataManager().GetCurrencyRateByID(m_CurrencyID.ToString());
            txtRate.Text = m_CurrencyRate.ToString();
            USDRate=Math.Round(GetProgramManager().GetDataManager().GetCurrencyRateByID("2"), 3);

            lblUSDRate.Text = "დოლარის კურსი:" + USDRate.ToString("F3");

        }
        private ProgramManagerBasic GetProgramManager()
        {
            return ProgramManager;
        }
      
        private bool onSetExistingProducts(DataTable products)
        {
            bool result = true;
            double priceGEL=0, priceUSD=0, priceSumGEL=0, priceSumUSD=0, discountAmountGEL=0, discountAmountUSD=0, quantity=0;
            foreach (DataRow dr in products.Rows)
            {
                priceGEL = dr.Field<double>("price");
                priceUSD = dr.Field<double>("price_USD");
                double convertedPriceUSD = Math.Round(priceGEL / USDRate, 4);
                double diff=priceUSD-convertedPriceUSD;
                if (!IComparableExtension.InRange(diff, -0.1, 0.1))
                    priceUSD = convertedPriceUSD;

                if (priceUSD <= 0 && priceGEL > 0)               
                    priceUSD = convertedPriceUSD;              

                quantity= dr.Field<double>("quantity");

                discountAmountGEL = Math.Round(dr.Field<double>("discount_amount"), 4);
                discountAmountUSD = Math.Round(discountAmountGEL / USDRate, 4);

                priceSumGEL = Math.Round(priceGEL*quantity-discountAmountGEL, 2);
                priceSumUSD =Math.Round(priceUSD*quantity-discountAmountUSD, 2);



                decimal code;
                if (!decimal.TryParse(dr["code"].ToString(), out code))
                {
                    result = false;
                    continue;
                }

                gridProducts.Rows.Add(dr.Field<int>("id"), code, dr.Field<string>("name"), 
                                      dr.Field<string>("unit_name"), priceGEL, priceSumGEL, discountAmountGEL, priceUSD, priceSumUSD, discountAmountUSD,
                                      quantity, dr.Field<double>("discount"), dr.Field<bool>("vat"), dr.Field<int>("unit_id"), dr.Field<int>("group_id"));

                
            }

            updateSumPrices();

            dataProducts.Rows.Clear();
            
            return result;
        }
        private void fillTypeCombo()
        {
            string sql = "SELECT id, name FROM book.ExMSGeneralTypes UNION ALL SELECT 0 AS id, '' AS name order by id";
            DataTable data = new DataTable();
            data = GetProgramManager().GetDataManager().GetTableData(sql);
            comboTypes.DataSource = data;
            comboTypes.DisplayMember = "name";
            comboTypes.ValueMember = "id";

            typesAreFilled = true;
        }
        private void fillColumnValues(int typeId)
        {
            pnlColumnValues.Controls.Clear();

            string sql = "SELECT col.id, col.db_name, col.name, col.is_interval FROM book.ExMSGeneralTypeColumns AS tc INNER JOIN book.ExMSColumns AS col ON tc.col_id=col.id WHERE tc.type_id=" + typeId;
            string subSql = "";
            DataTable subData;
            DataTable data = new DataTable();
            data = GetProgramManager().GetDataManager().GetTableData(sql);

            columnValues.Clear();
            columnValues.Add("type_id", typeId);
            foreach (DataRow dr in data.Rows)
            {

                if (!dr.Field<bool>("is_interval"))
                {
                    GroupBox groupColumn = new GroupBox();
                    groupColumn.AutoSize = true;
                    groupColumn.Dock = DockStyle.Top;
                    groupColumn.Padding = new Padding(0);
                    groupColumn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    groupColumn.Text = dr.Field<string>("name");
                    pnlColumnValues.Controls.Add(groupColumn);
                    groupColumn.BringToFront();

                    subSql = "SELECT id, name FROM book.ExMSColumnValues WHERE col_id=" + dr.Field<int>("id");
                    subData = new DataTable();
                    subData = GetProgramManager().GetDataManager().GetTableData(subSql);
                    int x = 15, y = 15, counter = 0;
                    bool isFirst = true;
                    foreach (DataRow row in subData.Rows)
                    {

                        RadioButton rButton = new RadioButton();
                        rButton.Location = new Point(x, y);
                        rButton.Size = new Size(160, 25);
                        rButton.Margin = new Padding(0, 0, 0, 0);
                        rButton.Tag = dr.Field<string>("db_name");
                        rButton.Text = row.Field<string>("name");

                        groupColumn.Controls.Add(rButton);
                        rButton.CheckedChanged += new EventHandler(radioBtn_checkChanged);
                        if (isFirst)
                        {
                            rButton.Checked = true;
                            isFirst = false;
                        }
                        rButton.BringToFront();
                        x += 165;
                        counter++;
                        if (counter > 1)
                        {
                            counter = 0;
                            y += 20;
                            x = 15;
                        }
                    }

                }
                else
                {
                    Panel pnlInterval = new Panel();
                    pnlInterval.AutoSize = true;
                    pnlInterval.Dock = DockStyle.Top;
                    pnlColumnValues.Controls.Add(pnlInterval);
                    pnlInterval.BringToFront();

                    Panel pnlLabelContainer = new Panel();
                    pnlLabelContainer.Size = new Size(100, 30);
                    pnlLabelContainer.Dock = DockStyle.Left;
                    pnlInterval.Controls.Add(pnlLabelContainer);

                    Label lblFrom = new Label();
                    lblFrom.Size = new Size(100, 30);
                    lblFrom.Text = dr.Field<string>("name") + ":";
                    lblFrom.Dock = DockStyle.Right;
                    lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;  
                    pnlLabelContainer.Controls.Add(lblFrom);

                    TextBoxDecimalInput txtFrom = new TextBoxDecimalInput();
                    txtFrom.Size = new Size(70, 30);
                    txtFrom.Location = new Point(105, 5);
                    txtFrom.Tag = dr.Field<string>("db_name") + "_interval";
                    txtFrom.Text = "0.00";
                    txtFrom.TextChanged += new EventHandler(txt_Changed);

                    columnValues.Add(txtFrom.Tag.ToString(), 0);
                    pnlInterval.Controls.Add(txtFrom);

                }

            }

        }
       
        private void onSerachProduct()
        {
             Hashtable parameters=new Hashtable();
             parameters = (Hashtable)columnValues.Clone();

             double searchQuantity = 0;

             if (!double.TryParse(txtQuantity.Text, out searchQuantity)) return;

             string complectName = comboTypes.Text + ", " + string.Join(", ", columnValues.Keys.OfType<string>().Where(p=>p!="type_id").OrderBy(p => p).Select(p => columnValues[p].ToString()).ToArray());
             var keys = columnValues.Keys.Cast<string>().AsEnumerable();
             string from, to;
             foreach (string p in keys)
             {
                 if (p.Contains("interval"))
                 {
                     from = p.Replace("interval", "from");
                     to = p.Replace("interval", "to");
                     parameters[p] = string.Format("(tem.{0}<=@{1}) AND (@{1}<={2})", from, p, to); continue;
                 }
                 parameters[p] = string.Format("tem.{0}=@{0}", p);
             }

            string whereCondition = string.Join(" AND ",parameters.Values.Cast<string>().ToArray());

            string sql = @"SELECT prod.id, CASE WHEN ISNUMERIC(prod.code)=1 THEN CONVERT(NUMERIC, prod.code) ELSE null END AS code, prod.name, unit.full_name AS unit_name, SUM(temprod.quantity) AS quantity, prod.vat, prod.default_unit_id AS unit_id, prod.group_id FROM book.ExMSTemplates AS tem INNER JOIN book.ExMSTemplateProducts AS temprod 
                        ON tem.id=temprod.template_id INNER JOIN book.Products AS prod ON prod.id=temprod.product_id INNER JOIN book.Units AS unit ON prod.default_unit_id=unit.id WHERE " +
                        whereCondition + " GROUP BY prod.id, prod.name, prod.code, unit.full_name, prod.vat, prod.default_unit_id, prod.group_id ORDER BY prod.id";

            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql, columnValues);
            if ((data == null)||(data.Rows.Count==0)) return;

            double priceGEL = 0, priceUSD = 0, exsitingQuantity=0, complectPrice = 0, complectPriceSum=0;
            int product_id=0;
            List<int> existingIds = gridProducts.Rows.Cast<DataGridViewRow>().Select(p => Convert.ToInt32(p.Cells[0].Value)).ToList();

            foreach (DataRow dr in data.Rows)
            {
                product_id = dr.Field<int>("id");
                double productQuantity = dr.Field<double>("quantity");
                priceUSD = GetProgramManager().GetDataManager().GetProductPriceByID(dr["id"].ToString(), priceId);
                double productSumQuantity = Math.Round(productQuantity * searchQuantity, 4);

                complectPrice += Math.Round(priceUSD * productQuantity, 4);

                if (existingIds.Contains(product_id))
                {
                    DataGridViewRow dgr = gridProducts.Rows.Cast<DataGridViewRow>().Where(p => (Convert.ToInt32(p.Cells[id.Index].Value) == product_id)&&(Convert.ToDouble(p.Cells[discount.Index].Value) == m_DiscountPercent)).FirstOrDefault();
                    if (dgr != null)
                    {                   
                        double.TryParse(dgr.Cells[quantity.Index].Value.ToString(), out exsitingQuantity);
                        dgr.Cells[quantity.Index].Value = exsitingQuantity + productSumQuantity;
                        continue;
                    }
                }

                priceGEL = Math.Round(priceUSD * USDRate, 4);

                double discountGEL = Math.Round(priceGEL * m_DiscountPercent / 100, 4);
                double sumGEL = priceGEL - discountGEL;

                double discountUSD = Math.Round(priceUSD * m_DiscountPercent / 100, 4);
                double sumUSD = priceUSD - discountUSD;

                gridProducts.Rows.Add(dr.Field<int>("id"), dr.Field<decimal>("code"), dr.Field<string>("name"), dr.Field<string>("unit_name"), priceGEL, sumGEL, discountGEL, priceUSD,
                     sumUSD, discountUSD, productSumQuantity, m_DiscountPercent, dr.Field<bool>("vat"), dr.Field<int>("unit_id"), dr.Field<int>("group_id"));
            }

            foreach (DataGridViewRow dgr in gridProducts.Rows)
            {
                UpdateRowPriceInfo(dgr);
            }
            updateSumPrices();                       

            var existringRow = gridComplects.Rows.Cast<DataGridViewRow>().Where(p => p.Cells[complect_name.Index].Value.ToString() == complectName).FirstOrDefault();
            if (existringRow != null)
            {
                double exstingQuantity = Convert.ToDouble(existringRow.Cells[complect_quantity.Index].Value);
                double newQuantity=exstingQuantity+searchQuantity;
                existringRow.Cells[complect_quantity.Index].Value = newQuantity;
               
                existringRow.Cells[complect_USD_sum.Index].Value =Math.Round(complectPrice * newQuantity, 4);
            }
            else
            {
                complectPriceSum = Math.Round(complectPrice * searchQuantity, 4);
                gridComplects.Rows.Add(complectName, searchQuantity, complectPrice, complectPriceSum);
            }
        }
        private void onSave()
        {            
            dataProducts.Rows.Clear();
            gridProducts.EndEdit();
            foreach (DataGridViewRow dgr in gridProducts.Rows)
            {
                dataProducts.Rows.Add(Convert.ToInt32(dgr.Cells[id.Index].Value), Convert.ToString(dgr.Cells[code.Index].Value), Convert.ToString(dgr.Cells[name.Index].Value),
                                     Convert.ToString(dgr.Cells[unit.Index].Value), Convert.ToDouble(dgr.Cells[quantity.Index].Value), Convert.ToDouble(dgr.Cells[price_GEL.Index].Value),
                                     Convert.ToDouble(dgr.Cells[sum_GEL.Index].Value), Convert.ToDouble(dgr.Cells[discount.Index].Value), Convert.ToDouble(dgr.Cells[discount_val_GEL.Index].Value),
                                     Convert.ToBoolean(dgr.Cells[vat.Index].Value), Convert.ToInt32(dgr.Cells[unit_id.Index].Value), Convert.ToInt32(dgr.Cells[group_id.Index].Value), Convert.ToDouble(dgr.Cells[price_USD.Index].Value));
            }
            this.DialogResult = DialogResult.OK;
        }
        private void txt_Changed(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            decimal value;
            if (!decimal.TryParse(txt.Text, out value))
            {
                txt.Text = "0.00";
            }
            columnValues[txt.Tag.ToString()] = value;
        }
        private void radioBtn_checkChanged(object sender, EventArgs e)
        {
            RadioButton btn = (RadioButton)sender;
            if (btn.Checked)
                columnValues[btn.Tag.ToString()] = btn.Text;
        }
        private void onDeleteProduct()
        {
            if ((gridProducts.SelectedRows.Count==0)||(gridProducts.Visible==false)) return;

            if (MessageBox.Show("გინდათ მონიშნული საქონლის წაშლა?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var rows=gridProducts.SelectedRows;
                foreach (DataGridViewRow dgr in rows)
                {
                    gridProducts.Rows.Remove(dgr);
                }
               
                updateSumPrices();
            }
           
        }
        private void onDeleteComplect()
        {
            if ((gridComplects.SelectedRows.Count == 0)||(gridComplects.Visible==false)) return;

            if (MessageBox.Show("გინდათ მონიშნული კომპლექტების წაშლა?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var rows = gridComplects.SelectedRows;
                foreach (DataGridViewRow dgr in rows)
                {
                    gridComplects.Rows.Remove(dgr);
                }

            }
        }
        private void onComplectQuantityChange(DataGridViewRow row)
        {
            if (row == null) return;
            double quantity = Convert.ToDouble(row.Cells[complect_quantity.Index].Value);
            double price = Convert.ToDouble(row.Cells[complect_USD_price.Index].Value);
            row.Cells[complect_USD_sum.Index].Value = Math.Round(quantity * price, 4);
        }
        private void onAddProduct()
        {
            if (gridProductList.SelectedRows.Count == 0) return;

            var rows = gridProductList.SelectedRows.Cast<DataGridViewRow>().OrderBy(p => p.Cells["num_code"].Value).AsEnumerable();

            List<int> existingIds = gridProducts.Rows.Cast<DataGridViewRow>().Select(p => Convert.ToInt32(p.Cells[0].Value)).ToList();

            double priceGEL = 0, priceUSD = 0, prodQuantity = Convert.ToDouble(txtProdQuantity.Text), sumQuantity = 0, discountPercent = 0,
                sumGEL = 0, sumUSD = 0, discountGEL = 0, discountUSD = 0;
            int product_id;
            foreach (DataGridViewRow row in rows)
            {
                product_id = Convert.ToInt32(row.Cells["id"].Value);
                if (existingIds.Contains(product_id))
                {
                    DataGridViewRow dgr = gridProducts.Rows.Cast<DataGridViewRow>().Where(p => (Convert.ToInt32(p.Cells[id.Index].Value) == product_id)&&(Convert.ToDouble(p.Cells[discount.Index].Value) == m_DiscountPercent)).FirstOrDefault();
                    if (dgr != null)
                    {
                        double exsitingQuantity = 0;

                        double.TryParse(dgr.Cells[quantity.Index].Value.ToString(), out exsitingQuantity);

                        sumQuantity = exsitingQuantity + prodQuantity;
                        dgr.Cells[quantity.Index].Value = sumQuantity;

                        priceGEL = Convert.ToDouble(dgr.Cells[price_GEL.Index].Value);
                        priceUSD = Convert.ToDouble(dgr.Cells[price_USD.Index].Value);

                        discountPercent = Convert.ToDouble(dgr.Cells[discount.Index].Value);

                        sumGEL = Math.Round(priceGEL * sumQuantity, 2);
                        sumUSD = Math.Round(priceUSD * sumQuantity, 2);

                        discountGEL = Math.Round(sumGEL*discountPercent/100, 4);
                        discountUSD = Math.Round(sumUSD * discountPercent / 100, 4);

                        dgr.Cells[sum_GEL.Index].Value = sumGEL - discountGEL;
                        dgr.Cells[sum_USD.Index].Value = sumUSD - discountUSD;

                        continue;
                    }
                }
                priceUSD = GetProgramManager().GetDataManager().GetProductPriceByID(product_id.ToString(), priceId);
                priceGEL = Math.Round(priceUSD * USDRate, 4);

                sumGEL = priceGEL * prodQuantity;
                sumUSD = priceUSD * prodQuantity;

                discountGEL = Math.Round(sumGEL * m_DiscountPercent / 100, 4);
                discountUSD = Math.Round(sumUSD * m_DiscountPercent / 100, 4);

                sumGEL -= discountGEL;
                sumUSD -= discountUSD;

                sumGEL = Math.Round(sumGEL, 2);
                sumUSD = Math.Round(sumUSD, 2);

                gridProducts.Rows.Add(product_id, Convert.ToDecimal(row.Cells["num_code"].Value), row.Cells["name"].Value.ToString(), row.Cells["unit_name"].Value.ToString(), priceGEL, sumGEL, discountGEL, priceUSD,
                     sumUSD, discountUSD, prodQuantity, m_DiscountPercent, Convert.ToBoolean(row.Cells["vat"].Value.ToString()), Convert.ToInt32(row.Cells["unit_id"].Value.ToString()), Convert.ToInt32(row.Cells["group_id"].Value.ToString()));
            }


            updateSumPrices();
        }
        private void UpdateRowPriceInfo(DataGridViewRow dgr)
        {
            double prodPriceUSD = 0;
            double prodPriceGEL = 0;

            double prodQuant = 0;

            double prodAmountUSD = 0;
            double prodAmountGEL = 0;

            double discountPercent = 0;

            double discountValUSD = 0;
            double discountValGEL = 0;

            if (dgr.Cells[discount.Index].Value != null)
                double.TryParse(dgr.Cells[discount.Index].Value.ToString(), out discountPercent);

            double.TryParse(dgr.Cells[quantity.Index].Value.ToString(), out prodQuant);
            if (prodQuant <= 0)
                return;

            double.TryParse(dgr.Cells[price_USD.Index].Value.ToString(), out prodPriceUSD);
            prodPriceGEL = Math.Round(prodPriceUSD * USDRate, 4);
            dgr.Cells[price_GEL.Index].Value = prodPriceGEL;


            prodAmountUSD = prodPriceUSD * prodQuant;
            prodAmountGEL = prodPriceGEL * prodQuant;

            discountValUSD = Math.Round(prodAmountUSD * discountPercent / 100, 4);
            discountValGEL = Math.Round(prodAmountGEL * discountPercent / 100, 4);

            prodAmountUSD -= discountValUSD;
            prodAmountGEL -= discountValGEL;

            prodAmountUSD = Math.Round(roundAmount(prodAmountUSD), 2);
            prodAmountGEL = Math.Round(roundAmount(prodAmountGEL), 2);

            dgr.Cells[sum_GEL.Index].Value = prodAmountGEL;
            dgr.Cells[sum_USD.Index].Value = prodAmountUSD;

            dgr.Cells[discount_val_GEL.Index].Value = discountValGEL;
            dgr.Cells[discount_value_USD.Index].Value = discountValUSD;
        }
        public double roundAmount(double amount)
        {           
            if (m_DiscountRoundType != DiscountRoundType.None)
            {
                if (m_DiscountRoundType == DiscountRoundType.Discount100)
                    amount = Math.Round(amount, 0);
                else if (m_DiscountRoundType == DiscountRoundType.Discount10)
                    amount = Math.Round(amount, 1);
                else if (m_DiscountRoundType == DiscountRoundType.Discount5)
                    amount = Math.Round((Math.Round(amount * 20, MidpointRounding.AwayFromZero) / 20), 2);
                else if (m_DiscountRoundType == DiscountRoundType.Discount50)
                    amount = Math.Round(amount, 1, MidpointRounding.AwayFromZero);
            }
            return amount;
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
                string sql = "SELECT id, name, address, driver_num, car_model, car_num,ISNULL(vat_type,0) AS is_vat, person   FROM book.Contragents WHERE id =" + res.ToString();

                DataContragent = new DataTable();
                DataContragent = GetProgramManager().GetDataManager().GetTableData(sql);

                if (DataContragent.Rows.Count == 0)
                    return;

                btnContragents.Tag = res.ToString();
                txtContragent.Text = DataContragent.Rows[0]["name"].ToString();

            }
        }
        public void OnCurrenciesList()
        {
            if (btnCurrency.Tag == null)
                btnCurrency.Tag = 1;

            int res = GetProgramManager().ShowSelectForm("TABLE_CURRENCY", Convert.ToInt32(btnCurrency.Tag));

            if (res != -1)
            {
                m_CurrencyID = res;
                m_CurrencyRate = GetProgramManager().GetDataManager().GetCurrencyRateByID(res.ToString());
                lblCurrencyName.Text = GetProgramManager().GetDataManager().GetCurrencyCodeByID(res.ToString());
                txtRate.Text = GetProgramManager().GetDataManager().GetCurrencyRateByID(res.ToString()).ToString();
                USDRate =Math.Round(GetProgramManager().GetDataManager().GetCurrencyRateByID("2"), 3);
                lblUSDRate.Text = "დოლარის კურსი:" + USDRate.ToString("F3");

                if (m_CurrencyID == 1)
                    txtRate.Enabled = false;
                else
                    txtRate.Enabled = true;

            }
        }
        private void OnDiscount()
        {
            ipmDocSubForms.DiscountForm form = new ipmDocSubForms.DiscountForm();
            double disc_round = 0;

            form.Initialize(GetProgramManager(), m_DiscountPercent, m_DiscountRound);
            form.ShowDialog();
            if (form.IsSaved())
            {
                form.GetData(out m_DiscountPercent, out disc_round);
                m_DiscountRound = disc_round;
                m_DiscountRoundType = GetRoundTypeByVal(disc_round);
                labelDiscountPercent.Text = m_DiscountPercent.ToString() + "%";
            }
        }
    
        private DiscountRoundType GetRoundTypeByVal(double disc_round)
        {
            if (disc_round == 0)
                return DiscountRoundType.None;
            else if (disc_round == 0.05)
                return DiscountRoundType.Discount5;
            else if (disc_round == 0.1)
                return  DiscountRoundType.Discount10;
            else if (disc_round == 0.5)
                return  DiscountRoundType.Discount50;
            else if (disc_round == 1.0)
                return  DiscountRoundType.Discount100;
            else return DiscountRoundType.None;
        }
        private void updateSumPrices()
        {
            if (gridProducts.Rows.Count == 0)
            {
                lblSumGEL.Text = "ჯამი ლარში: 0.00";
                lblSumUSD.Text = "ჯამი დოლარში: 0.00";
                lblDiscountGEL.Text = "ფასდ. ლარში: 0.00";
                lblDiscountUSD.Text = "ფასდ. დოლარში: 0.00";
                return;
            }

            double sumUSD = 0, sumGEL = 0, discountGEL=0;

            var rows=gridProducts.Rows.Cast<DataGridViewRow>().AsEnumerable();

            sumUSD = rows.Select(p => Convert.ToDouble(p.Cells[sum_USD.Index].Value)).Sum();
            discountUSD = rows.Select(p => Convert.ToDouble(p.Cells[discount_value_USD.Index].Value)).Sum();
            
            sumGEL = rows.Select(p => Convert.ToDouble(p.Cells[sum_GEL.Index].Value)).Sum();
            discountGEL = rows.Select(p => Convert.ToDouble(p.Cells[discount_val_GEL.Index].Value)).Sum();
          
            lblSumGEL.Text = "ჯამი ლარში: " + sumGEL.ToString("F2");
            lblSumUSD.Text = "ჯამი დოლარში: " + sumUSD.ToString("F2");

            lblDiscountGEL.Text = "ფასდ. ლარში: " + discountGEL.ToString("F2");
            lblDiscountUSD.Text = "ფასდ. დოლარში: " + discountUSD.ToString("F2");
        }
        private void onGenerateInvoice()
        {
            string invoice_tamplate_name = "invoice_house_of_future";
            ipmReportGenerator.ReportForm form = new ipmReportGenerator.ReportForm(GetProgramManager(), "ინვოისი", invoice_tamplate_name, 0);

            int contragentID=0;
            string contragentName=txtContragent.Text;
            string contragentCode="";
            string contragentAddress="";
            string contragentTel="";
            string companyInfo = "";
            string sql = "";
            DataTable data;

            int.TryParse(Convert.ToString(btnContragents.Tag), out contragentID);

            if (contragentID>0)
            {
                sql="SELECT code, isnull(address, '') as address, isnull(tel, '') as tel FROM book.Contragents WHERE id="+contragentID;
                data = new DataTable();
                data=GetProgramManager().GetDataManager().GetTableData(sql);
                if ((data!=null)&&(data.Rows.Count>0))
                {
                    contragentCode=data.Rows[0].Field<string>("code");
                    contragentAddress=data.Rows[0].Field<string>("address");
                    contragentTel=data.Rows[0].Field<string>("tel");
                }
            }

            companyInfo = GetProgramManager().GetDataManager().GetStringValue("SELECT TOP(1) info FROM book.Companies", new Hashtable());           

            Hashtable staticFeilds = new Hashtable();
            staticFeilds.Add("date",  "თარიღი: "+ DateTime.Now.ToString("dd.MM.yyyy"));
            staticFeilds.Add("doc_num", "ინვოისის №: "+InvoiceNumber);
            staticFeilds.Add("payment_days", "გადახდის ვადა: "+InvoiceTerm);
            staticFeilds.Add("currency", USDRate);
            staticFeilds.Add("discount", discountUSD);

            double sumGEL =gridProducts.Rows.Cast<DataGridViewRow>().Select(p => Convert.ToDouble(p.Cells[sum_GEL.Index].Value)).Sum();
            staticFeilds.Add("sum_gel", sumGEL);

            staticFeilds.Add("contragent_name", contragentName);

            if (!string.IsNullOrEmpty(contragentCode))
            staticFeilds.Add("contragent_code", "საიდენტიფიკაციო კოდი: "+contragentCode);

            if (!string.IsNullOrEmpty(contragentAddress))
                staticFeilds.Add("contragent_address", "მისამართი: " + contragentAddress);

            if (!string.IsNullOrEmpty(contragentTel))
                staticFeilds.Add("contragent_tel", "ტელეფონი: " + contragentTel);

            staticFeilds.Add("company_info", companyInfo);           

            byte[] bytes = GetProgramManager().GetDataManager().GetCompanyLogoImage();
            if (bytes != null)
                form.GetWorkBookControl().AddImage("logo_image", bytes);

            DataTable data_table_complects = new DataTable();
            data_table_complects.Columns.Add("complect_name", typeof(string));
            data_table_complects.Columns.Add("complect_quantity", typeof(double));
            data_table_complects.Columns.Add("complect_price", typeof(double));
            data_table_complects.Columns.Add("complect_price_sum", typeof(double));

            foreach (DataGridViewRow row in gridComplects.Rows)
            {
                data_table_complects.Rows.Add(row.Cells[complect_name.Index].Value.ToString(), Convert.ToDouble(row.Cells[complect_quantity.Index].Value),
                    Convert.ToDouble(row.Cells[complect_USD_price.Index].Value), Convert.ToDouble(row.Cells[complect_USD_sum.Index].Value));
            }

            DataTable data_table_basket = new DataTable();
            data_table_basket.Columns.Add("product_code", typeof(string));
            data_table_basket.Columns.Add("product_name", typeof(string));
            data_table_basket.Columns.Add("product_quantity", typeof(double));
            data_table_basket.Columns.Add("product_unit", typeof(string));
            data_table_basket.Columns.Add("product_price", typeof(double));
            data_table_basket.Columns.Add("product_discount", typeof(double));
            data_table_basket.Columns.Add("product_price_sum", typeof(double));
            data_table_basket.Columns.Add("product_currency", typeof(string));

            int productId;
            foreach (DataGridViewRow row in gridProducts.Rows)
            {
                productId = Convert.ToInt32(row.Cells[id.Index].Value);
                data_table_basket.Rows.Add(row.Cells[code.Index].Value.ToString(), row.Cells[name.Index].Value.ToString(), Convert.ToDouble(row.Cells[quantity.Index].Value), row.Cells[unit.Index].Value.ToString(),
                    Convert.ToDouble(row.Cells[price_USD.Index].Value), Convert.ToDouble(row.Cells[discount.Index].Value), Convert.ToDouble(row.Cells[sum_USD.Index].Value), productWithCurrency[productId]);
            }


            form.GetWorkBookControl().AddTable("data_table_complects", data_table_complects);
            form.GetWorkBookControl().AddTable("data_table_basket", data_table_basket);
            form.GetWorkBookControl().setDynamicRowNum(4);
            form.GetWorkBookControl().setRequiredRowsCount(2);
            form.GetWorkBookControl().SetStaticMembers(staticFeilds);

            form.ShowWindow(false);
            form.GetWorkBookControl().Generate();

        }
        public void setNewUSDRate(string rate)
        {
            double value = 0;
            if (!double.TryParse(rate, out value)) return;

            USDRate =Math.Round(value, 3);
            lblUSDRate.Text = "დოლარის კურსი:" + USDRate.ToString("F3");
        }
        private void filterProductList()
        {
            if (string.IsNullOrEmpty(columnName)) return;

            Func<string, string> EscapeLikeValue = (value) =>
            {
                StringBuilder sb = new StringBuilder(value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    char c = value[i];
                    switch (c)
                    {
                        case ']':
                        case '[':
                        case '%':
                        case '*':
                            sb.Append("[").Append(c).Append("]");
                            break;
                        case '\'':
                            sb.Append("''");
                            break;
                        default:
                            sb.Append(c);
                            break;
                    }
                }
                return sb.ToString();
            };

            filterProductList(string.Format("{0}='{1}' AND [{2}] LIKE '%{3}%'", columnName, comboProductType.SelectedItem, comboInFeild.SelectedIndex == 0 ? "code" : "name", EscapeLikeValue(txtSearchProduct.Text)));

        }
        private void comboTypes_SelectedValueChanged(object sender, EventArgs e)
        {
            if (typesAreFilled)
            {
                int typeId=0;
                int.TryParse(comboTypes.SelectedValue.ToString(), out typeId);
                if (typeId == 0)
                {
                    pnlColumnValues.Controls.Clear();
                    return;
                }
                fillColumnValues(typeId);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {           
            onSerachProduct();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            onSave();
        }       

        private void btnDelete_Click(object sender, EventArgs e)
        {
            onDeleteProduct();
        }

        private void gridProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            if (new List<int>(){4,7,10,11}.Contains(e.ColumnIndex))
            {
                gridProducts.CurrentCell = gridProducts.Rows[e.RowIndex].Cells[e.ColumnIndex];
                gridProducts.BeginEdit(true);
            }
        }

        private void btnContragents_Click_1(object sender, EventArgs e)
        {
            OnContragentsList();
        }

        private void btnCurrency_Click(object sender, EventArgs e)
        {
            OnCurrenciesList();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            OnDiscount();
        }

        private void gridProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (new List<int>() { 7, 10, 11 }.Contains(e.ColumnIndex))
            {               
                DataGridViewRow dgr = gridProducts.Rows[e.RowIndex];
                UpdateRowPriceInfo(dgr);
                updateSumPrices();
            }
        }

        private void btnNewCurrency_Click(object sender, EventArgs e)
        {
            if (m_CurrencyID == 2)
                setNewUSDRate(txtRate.Text);
        }

        private void btnResetUSDRate_Click(object sender, EventArgs e)
        {
            USDRate = GetProgramManager().GetDataManager().GetCurrencyRateByID("2");
            lblUSDRate.Text = "დოლარის კურსი:" + USDRate.ToString("F3");
            txtRate.Text = USDRate.ToString();

        }
        private void btnComplects_Click(object sender, EventArgs e)
        {
            if (btnComplects.Tag.ToString()== "DOWN")
            {
                tPanelComplects.Visible = true;
                tPanelProducts.Visible = false;
                btnComplects.Image = Properties.Resources.nav_up_blue;
                btnProducts.Image = Properties.Resources.nav_down_blue;
                setPanelState(tPnlMain, tPanelComplects, tPanelProducts);
                btnComplects.Tag = "UP";
                btnProducts.Tag = "DOWN";
            }
            else
            {
                tPanelComplects.Visible = false;                  
                btnComplects.Image = Properties.Resources.nav_down_blue;
                setPanelState(tPnlMain, tPanelProducts, tPanelComplects);
                btnComplects.Tag = "DOWN";
            }
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            if (btnProducts.Tag.ToString() == "DOWN")
            {
                tPanelProducts.Visible = true;
                tPanelComplects.Visible = false;                
                btnProducts.Image = Properties.Resources.nav_up_blue;
                btnComplects.Image = Properties.Resources.nav_down_blue;

                setPanelState(tPnlMain, tPanelProducts, tPanelComplects);
                btnProducts.Tag = "UP";
                btnComplects.Tag = "DOWN";
            }
            else
            {
                tPanelProducts.Visible = false;
                btnProducts.Image = Properties.Resources.nav_down_blue;
                btnProducts.Tag = "DOWN";
            }
        }

        private void gridProductList_SelectionChanged(object sender, EventArgs e)
        {
            setCurrentProductControls();
        }

        private void txtProdQuantity_Leave(object sender, EventArgs e)
        {
            setProductFullPrice();
        }

        private void comboProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterProductList();
        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            filterProductList();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            onAddProduct();
        }

        private void btnComplectBasket_Click(object sender, EventArgs e)
        {
            if (btnComplectBasket.Tag.ToString() == "DOWN")
            {
                gridComplects.Visible = true;
                gridProducts.Visible = false;
                btnComplectBasket.Image = Properties.Resources.nav_up_blue;
                btnBasket.Image = Properties.Resources.nav_down_blue;
                setPanelState(tPanelBaskets, gridComplects, gridProducts);
                btnComplectBasket.Tag = "UP";
                btnBasket.Tag = "DOWN";
            }
            else
            {
                gridComplects.Visible = false;
                btnComplectBasket.Image = Properties.Resources.nav_down_blue;
                setPanelState(tPanelBaskets, gridProducts, gridComplects);
                btnComplectBasket.Tag = "DOWN";
            }
        }

        private void btnBasket_Click(object sender, EventArgs e)
        {
            if (btnBasket.Tag.ToString() == "DOWN")
            {
                gridProducts.Visible = true;
                gridComplects.Visible = false;
                btnBasket.Image = Properties.Resources.nav_up_blue;
                btnComplectBasket.Image = Properties.Resources.nav_down_blue;

                setPanelState(tPanelBaskets, gridProducts, gridComplects);
                btnBasket.Tag = "UP";
                btnComplectBasket.Tag = "DOWN";
            }
            else
            {
                gridProducts.Visible = false;
                btnBasket.Image = Properties.Resources.nav_down_blue;
                btnBasket.Tag = "DOWN";
            }
        }

        private void btnDeleteComplect_Click(object sender, EventArgs e)
        {
            onDeleteComplect();
        }

        private void gridComplects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            if ((e.ColumnIndex==1)||(e.ColumnIndex==2))
            {
                gridComplects.CurrentCell = gridProducts.Rows[e.RowIndex].Cells[e.ColumnIndex];
                gridComplects.BeginEdit(true);
            }
        }

        private void gridComplects_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            if ((e.ColumnIndex==1)||(e.ColumnIndex==2))
            onComplectQuantityChange(gridComplects.Rows[e.RowIndex]);
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            onGenerateInvoice();
        }

        private void gridProducts_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.gridProducts.Sort(this.gridProducts.Columns["code"], ListSortDirection.Ascending);
        }

        private void comboInFeild_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterProductList();
        }

        private void gridProducts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            foreach (DataGridViewRow row in gridProducts.Rows)
            {
                row.DefaultCellStyle.SelectionBackColor = Color.RoyalBlue;
            }            
        }

        private void gridComplects_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            foreach (DataGridViewRow row in gridComplects.Rows)
            {
                row.DefaultCellStyle.SelectionBackColor = Color.RoyalBlue;
            }
        }

        private void gridProductList_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            foreach(DataGridViewRow row in gridProductList.Rows)
            {
               row.DefaultCellStyle.SelectionBackColor = Color.RoyalBlue;
            }
        }

        private void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            saveAndNew = true;
            onSave();
        }
       
                    
    }
    public static class IComparableExtension
    {
        public static bool InRange<T>(this T value, T from, T to) where T : IComparable<T>
        {
            return value.CompareTo(from) >= 1 && value.CompareTo(to) <= -1;
        }
    }
}
