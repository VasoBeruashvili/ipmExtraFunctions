using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using ipmBLogic;
using ipmControls;
using ipmPMBasic.Models;
using ipmFunc;
using System.ComponentModel;
using System.Drawing;

namespace FINA
{
    enum NsafFacturaTips
    {
        [Description("ჩამოტვირთულია")]
        Downloaded,
        [Description("ჩამოტვირთვა შესაძლებელია")]
        Available,
        [Description("Nsaf საქონელი ვერ მოიძებნა")]
        UnavailableNsaf,
        [Description("Nsaf საქონლის ერთეული არ ემთხვევა")]
        UnavailableNsafUnit,
        [Description("Nsaf რაოდენობის პრობლემა ერთეულებთან თანხვედრაში")]
        UnavailableNsafQuantity,
        [Description("უცხო კონტარგენტი")]
        UnavailableContragent,
        [Description("უცხო საწყობი (აგს)")]
        UnavailableStore,
        [Description("ვერ ჩამოიტვირთა, პრობლემა RS-ზე")]
        UnDownloadable
    }
    public partial class RSNsafWaybillsList : Form
    {
        private ProgramManagerBasic Pm;

        public RSNsafWaybillsList(ProgramManagerBasic pm)
        {
            InitializeComponent();
            Pm = pm;
            Pm.TranslateControl(this);
            m_Period.dtp_From.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, 100);
            m_Period.dtp_To.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59, 59, 997);
        }
        private Dictionary<int, string> m_Status = new Dictionary<int, string>() { { 0, "ატვითული" }, { 1, "გადაგზავნილი" }, { 2, "დადასტურებული" }, { 3, "პირველადი კორექტირებულის" }, { 4, "ახალი კორექტირების" }, { 5, "კორექტირების დასადასტურებელი" }, { 6, "გაუქმებული" }, { 7, "დადასტურებული გაუქმებული" }, { 8, "კორექტირების დადასტურებული" } };
        public Dictionary<string, byte> m_TransportType = new Dictionary<string, byte> { { "საავტომობილო", 1 }, { "სარკინიგზო", 2 }, { "მილსადენი", 3 }, { "სხვა", 4 } };
        List<DataRow> Waybills = null;
        Dictionary<int, KeyValuePair<int, KeyValuePair<string, string>>> WaybillsubItems = new Dictionary<int, KeyValuePair<int, KeyValuePair<string, string>>>();

        private void RSNsafWaybillsList_Load(object sender, EventArgs e)
        {
            comboType.DataSource = new BindingSource
            {
                DataSource = new Dictionary<byte, string>
            {
                { 0, "საქონლის მიღება (მყიდველის მხარის)" },
                { 1, "საცალო მიწოდება" },
                { 2, "საბითუმო მიწოდება" },
                { 3, "შიდა გადაზიდვა" },
                { 4, "იმპორტირება დასაწყობების ადგილამდე ტრანსპორტირება" },
                { 5, "ექსპორტირება დასაწყობების ადგილიდან ტრანსპორტირება" }
            }
            };
            comboType.DisplayMember = "Value";
            comboType.ValueMember = "Key";
            comboFilterMethod.DataSource = new BindingSource { DataSource = new Dictionary<byte, string> { { 1, "ოპერაციის თარიღი" }, { 2, "გამოწერის თარიღი" } } };
            comboFilterMethod.ValueMember = "Key";
            comboFilterMethod.DisplayMember = "Value";
            txtFilter.Select();
            txtFilter.Focus();
        }

        private void onWaybillsLoad()
        {
            m_Grid.Rows.Clear();
            ProgressDispatcher.Activate();
            byte type = (byte)comboType.SelectedValue;
            m_Grid.Columns[ColContragent.Index].Visible = type < 3;

            DateTime _from = new DateTime(m_Period.dtp_From.Value.Year, m_Period.dtp_From.Value.Month, m_Period.dtp_From.Value.Day, 0, 0, 0, 100);
            DateTime _to = new DateTime(m_Period.dtp_To.Value.Year, m_Period.dtp_To.Value.Month, m_Period.dtp_To.Value.Day, 23, 59, 59, 997);
            DateTime _from2 = _from;
            DateTime _to2 = _to;
            if (type == 1)
            {
                _from = DateTime.MinValue;
                _to = DateTime.MaxValue;
            }
            else
            {
                _from2 = DateTime.MinValue;
                _to2 = DateTime.MaxValue;
            }
            WaybillsubItems.Clear();
            
            DataTable data = type == 0 ? Pm.GetRSService().GetBayerNsafInvoice(_from, _to, _from2, _to2, string.Empty, string.Empty) : Pm.GetRSService().GetSellerNsafInvoice(_from, _to, _from2, _to2, string.Empty, string.Empty);
            if (data == null || data.Rows.Count <= 0)
            {
                ProgressDispatcher.Deactivate();
                return;
            }

            string _cd = Pm.GetDataManager().GetCompanyCode();
            Waybills = data.AsEnumerable().Where(x => (type != 0 || (type == 0 && _cd != Convert.ToString(x["SELLER_SN"])))).OrderByDescending(a => Convert.ToDateTime(a["reg_date"])).ToList();
            if (type != 0)
                Waybills = Waybills.Where(o => Convert.ToByte(o["INVOICE_TYPE"]) == type).ToList();
            ProgressDispatcher.Deactivate();
            UpdateGrid(type);
            txtFilter.Select();
            txtFilter.Focus();
        }

        private void UpdateGrid(byte type)
        {
            m_Grid.Rows.Clear();
            if (Waybills == null || !Waybills.Any())
                return;

            ProgressDispatcher.Activate(true);
            Dictionary<string, int> myContragegents = type < 3 ? Pm.GetDataManager().GetDictionary<string, int>("SELECT DISTINCT code, id FROM book.Contragents WHERE ((0=" + type + " AND (path ='0#1#3' OR path LIKE '0#1#3#%')) OR (0 !=" + type + " AND (path ='0#2#5' OR path LIKE '0#2#5#%'))) AND code IS NOT NULL AND code!='' ") : null;
            List<Products> _prods = Pm.GetDataManager().GetList<Products>("SELECT id AS Id, code AS Code, name AS Name, nsaf_good_id AS NsafGoodId, unit_id AS UnitId, (CASE WHEN ISNULL(vat,0)= 1 THEN 1 ELSE CASE WHEN ISNULL(vat_type,0)=0 THEN 2 ELSE 3  END END) AS VatType FROM book.Products WHERE nsaf_good_id IS NOT NULL AND (path='0#1#10' OR path LIKE '0#1#10#%')");
            Dictionary<KeyValuePair<int, string>, Products> myProduct = _prods.GroupBy(p => new { p.NsafGoodId, p.Code}).Select(p => new KeyValuePair<KeyValuePair<int, string>, Products>(new KeyValuePair<int, string>(p.Key.NsafGoodId.Value, p.Key.Code), p.First())).ToDictionary(k => k.Key, v => v.Value);

            List<Stores> _stors = Pm.GetDataManager().GetList<Stores>("SELECT id AS Id, name AS Name, code AS Code FROM book.Stores WHERE code IS NOT NULL AND code !=''");
            Dictionary<string, Stores> _myStores = _stors.GroupBy(s => s.Code).Select(s => new KeyValuePair<string, Stores>(s.Key, s.First())).ToDictionary(k => k.Key, v => v.Value);

            Dictionary<int, string> myUnits = Pm.GetDataManager().GetDictionary<int, string>("SELECT  id, name FROM book.Units WHERE name IN (N'კგ', N'ლ')");

            string _waybill_sql = @"SELECT g.waybill_num, g.purpose +' '+ISNULL(NULLIF(g.waybill_num, ''), g.doc_num_prefix+ CAST(g.doc_num AS VARCHAR(20))) + ISNULL(NULLIF((', '+o.doc2),', '),'') AS purpose FROM doc.GeneralDocs AS g INNER JOIN doc.ProductIn AS o ON o.general_id = g.id WHERE NOT(g.waybill_num IS NULL OR g.waybill_num = '') AND(g.is_deleted IS NULL OR g.is_deleted = 0)";
            switch (type)
            {
                case 1:
                case 2:
                    {
                        _waybill_sql = @"SELECT CAST(o.waybill_id AS VARCHAR(50)) AS waybill_num, g.purpose +' '+ ISNULL(ISNULL(NULLIF((o.nsaf_series+'-'),'-'),'') +   NULLIF(g.waybill_num, ''), g.doc_num_prefix+CONVERT(NVARCHAR(20),g.doc_num)) AS purpose FROM doc.GeneralDocs AS g INNER JOIN doc.ProductOut AS o ON o.general_id=g.id WHERE o.nsaf_mode=1 AND o.waybill_id IS NOT NULL AND o.waybill_id>0 AND (g.is_deleted IS NULL OR g.is_deleted=0)";
                        break;
                    }
                case 3:
                case 4:
                case 5:
                    {
                        _waybill_sql = @"SELECT CAST(o.waybill_id AS VARCHAR(50)) AS waybill_num, g.purpose +' '+ ISNULL(ISNULL(NULLIF((o.nsaf_series+'-'),'-'),'') +   NULLIF(g.waybill_num, ''), g.doc_num_prefix+CONVERT(NVARCHAR(20),g.doc_num)) AS purpose FROM doc.GeneralDocs AS g INNER JOIN doc.ProductMove AS o ON o.general_id = g.id WHERE o.nsaf_mode = 1 AND o.waybill_id IS NOT NULL AND o.waybill_id>0 AND (g.is_deleted IS NULL OR g.is_deleted = 0)";
                        break;
                    }
            }
            Dictionary<string, string> existing_waybills = Pm.GetDataManager().GetDictionary<string, string>(_waybill_sql);
            int i = 1;
            foreach (DataRow row in Waybills)
            {
                decimal per = (decimal)i / (decimal)Waybills.Count * 100M;
                ProgressDispatcher.Percent = ((int)per);
                i++;
                int invoice_id = Convert.ToInt32(row["Id"]);
                int status = 0;
                if (!int.TryParse(Convert.ToString(row["status"]), out status))
                    continue;
                int contragent_id = -1;
                string contargent_code = Convert.ToString(row["BUYER_SN"]);
                string contargent_name = Convert.ToString(row["BUYER_NM"]);
                if ((byte)comboType.SelectedValue == 0)
                {
                    contargent_code = Convert.ToString(row["SELLER_SN"]);
                    contargent_name = Convert.ToString(row["SELLER_NM"]);
                }

                string _store1 = Convert.ToString(row["OIL_ST_N"]);
                string _store2 = Convert.ToString(row["OIL_FN_N"]);

                if (type == 0)
                {
                    string tmp = _store1;
                    _store1 = _store2;
                    _store2 = tmp;
                }
                int my_store1 = -1;
                int my_store2 = -1;
                string my_store1_name = null;
                string my_store2_name = null;

                if (_myStores.ContainsKey(_store1))
                {
                    my_store1 = _myStores[_store1].Id;
                    my_store1_name = _myStores[_store1].Name;
                }
                if (_myStores.ContainsKey(_store2))
                {
                    my_store2 = _myStores[_store2].Id;
                    my_store2_name = _myStores[_store2].Name;
                }
                string store_string = type < 3 ? string.Format("{0} ({1})", my_store1_name, _store1) : string.Format("{0} ({1}) > {2} ({3})", my_store1_name, _store1, my_store2_name, _store2);

                double quantL = row["NUMBER_L"].TryParseScalar<double>();
                double quantKG = row["NUMBER_KG"].TryParseScalar<double>();

                int product_id = -1;
                string product_code = null;
                string product_name = null;
                double quantity = 0;
                int unit_id = -1;
                int vat_type = 0;

                int good_id = -1;
                string good_name = null;
                string good_code = null;
                if (!WaybillsubItems.ContainsKey(invoice_id))
                {
                    DataTable _desc = Pm.GetRSService().GetNsafInvoiceDesc(invoice_id);
                    if (_desc != null && _desc.Rows.Count > 0)
                        WaybillsubItems.Add(invoice_id, new KeyValuePair<int, KeyValuePair<string, string>>(Convert.ToInt32(_desc.Rows[0]["GOOD_ID"]), new KeyValuePair<string, string>(Convert.ToString(_desc.Rows[0]["AKCIZI_ID"]), Convert.ToString(_desc.Rows[0]["GOODS"]))));
                }
                if (WaybillsubItems.ContainsKey(invoice_id))
                {
                    good_id = WaybillsubItems[invoice_id].Key;
                    good_code = WaybillsubItems[invoice_id].Value.Key;
                    good_name = WaybillsubItems[invoice_id].Value.Value;
                }

                //სატრანსპორტო საშუალების მარკაში პრეფიქსად უნდა ეწეროს ის სიმბოლოები რაც საქონლის კოდში ეწერება სუფიქსად
                var match = myProduct.Where(x => x.Key.Key == good_id).ToList(); 
                if (match.Any())
                {
                    Products p = null;
                    if (match.Count == 1)
                        p = match.ElementAt(0).Value;
                    else
                    {
                        string info = Convert.ToString(row["TRANSPORT_MARK"]);
                        string _k = "";
                        if (info.StartsWith("D.S "))
                            _k = "D.S";
                        else if (info.StartsWith("D.FS "))
                            _k = "D.FS";
                        else if (info.StartsWith("REG "))
                            _k = "REG";
                        p = match.Where(x => x.Value.Code.EndsWith(_k)).Select(x => x.Value).FirstOrDefault() ?? match.ElementAt(0).Value;
                    }

                    product_id = p.Id;
                    product_name = p.Name;
                    product_code = p.Code;
                    vat_type = p.VatType.Value;
                    if (myUnits.ContainsKey(p.UnitId.Value))
                    {
                        unit_id = p.UnitId.Value;
                        string _un = myUnits[unit_id];
                        if (_un == "ლ")
                            quantity = quantL;
                        else if (_un == "კგ")
                            quantity = quantKG;
                    }
                }

                double amount = row["UNIT_PRICE"].TryParseScalar<double>();
                double unit_price = amount > 0 ? (double)Math.Round(((decimal)(amount / quantity)), 3) : 0;
                if (type < 3 && myContragegents.ContainsKey(contargent_code))
                    contragent_id = myContragegents[contargent_code];

                NsafFacturaTips _tp = NsafFacturaTips.UnDownloadable;
                string key = Convert.ToString(type == 0 ? row["f_number"] : row["Id"]);
                string fina_value = null;
                if (existing_waybills.ContainsKey(key))
                {
                    _tp = NsafFacturaTips.Downloaded;
                    fina_value = existing_waybills[key];
                }
                else if(my_store1 <= 0 || (type>=3 && my_store2<=0))
                    _tp = NsafFacturaTips.UnavailableStore;
                else if (type < 3 && contragent_id <= 0)
                    _tp = NsafFacturaTips.UnavailableContragent;
                else if (product_id <= 0)
                    _tp = NsafFacturaTips.UnavailableNsaf;
                else if (unit_id <= 0)
                    _tp = NsafFacturaTips.UnavailableNsafUnit;
                else if (quantity <= 0)
                    _tp = NsafFacturaTips.UnavailableNsafQuantity;
                else
                    _tp = NsafFacturaTips.Available;

                int index = m_Grid.Rows.Add(
                   false,
                   type,
                   invoice_id,
                   row["operation_dt"],
                   row["reg_date"],
                   row["TR_ST_DATE"],
                   row["f_series"],
                   row["f_number"],
                   string.Format("{0} {1}", row["f_series"], row["f_number"]),
                   
                   row["DRIVER_IS_GEO"],
                   row["DRIVER_INFO"],
                   row["DRIVER_NO"],
                   row["CARRIER_INFO"],
                   row["CARRIE_S_NO"],
                   row["TRANSPORT_MARK"],
                   row["TRANSPORT_TYPE"],
                   row["OIL_ST_ADDRESS"],
                   _store1,
                   row["OIL_FN_ADDRESS"],
                   _store2,
                   
                   contragent_id,
                   contargent_code,
                   string.Format("{0} ({1})", contargent_name, contargent_code),
                   my_store1,
                   my_store2,
                   store_string,
                   product_id,
                   good_id,
                   good_code,
                   good_name,
                   unit_id,
                   vat_type,
                   string.Format("RS: {0} {1} {2}, FINA: {3} {4}", good_name, good_code, good_id, product_name, product_code),
                   quantity,
                   quantKG,
                   quantL,
                   unit_price,
                   amount,
                   status,
                   m_Status.ContainsKey(status) ? m_Status[status] : null,
                   fina_value,
                   EnumEx.GetEnumDescription(_tp),
                   _tp
                   );
                m_Grid.Rows[index].Cells[ColCheck.Index].ReadOnly = _tp != NsafFacturaTips.Available;
            }
            m_Grid.EndEdit();
            ProgressDispatcher.Deactivate();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in m_Grid.Rows)
                r.Visible = string.Concat(r.Cells[ColFullNum.Index].Value, r.Cells[ColContragent.Index].Value, r.Cells[ColPRoductName.Index].Value, r.Cells[ColAmount.Index].Value, r.Cells[ColFinaOperation.Index].Value, r.Cells[ColConditionText.Index].Value).IndexOf(txtFilter.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            onWaybillsLoad();
        }

        private void menuNsafFactura_Opening(object sender, CancelEventArgs e)
        {
            if (m_Grid.Rows.Count == 0 || m_Grid.SelectedRows.Count == 0)
                return;
            byte type = (byte)m_Grid.SelectedRows[0].Cells[ColType.Index].Value;
            //menuContragentAdd.Enabled = type < 3 && (int)m_Grid.SelectedRows[0].Cells[ColContragentId.Index].Value < 0;
            //menuProductAdd.Enabled = (int)m_Grid.SelectedRows[0].Cells[ColProductId.Index].Value < 0;
        }

        private void menuUnSelect_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0)
                return;
            foreach (DataGridViewRow rw in m_Grid.Rows)
                rw.Cells[ColCheck.Index].Value = false;
        }

        private void menuSelect_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0 || m_Grid.SelectedRows.Count == 0)
                return;
            foreach (DataGridViewRow rw in m_Grid.Rows)
                rw.Cells[ColCheck.Index].Value = (NsafFacturaTips)rw.Cells[ColConditionValue.Index].Value == NsafFacturaTips.Available;
        }

        private void OnAddProduct(bool all)
        {
            if (m_Grid.Rows.Count == 0)
                return;
            var rows = m_Grid.Rows.OfType<DataGridViewRow>().Where(x => (all || (!all && x.Selected)) && (int)x.Cells[ColProductId.Index].Value < 0);
            if (!rows.Any())
            {
                MessageBoxForm.Show(Application.ProductName, string.Format("{0} საქონელი არ საჭიროებს დამატებას.", !all ? "მონიშნული" : "ცხრილში არსებული არცერთი"), null, null, SystemIcons.Information);
                return;
            }
            byte type = (byte)rows.First().Cells[ColType.Index].Value;
            HashSet<string> misseds = new HashSet<string>();
            ProgressDispatcher.Activate();
            foreach (var row in rows)
            {
                int unit_id = (int)(row.Cells[ColUnitId.Index].Value);
                if (unit_id <= 0)
                    unit_id = 4;
                double price_val = Convert.ToDouble(row.Cells[ColPrice.Index].Value);

                string sql_code = "SELECT MAX(CASE WHEN ISNUMERIC(code)=1 AND LEN(code)<7 THEN CAST(REPLACE(code,',','.') AS NUMERIC) ELSE null END) FROM book.products";
                int max_value = Pm.GetDataManager().GetIntegerValue(sql_code) + 1;
                string code = max_value.ToString("D5");
                string name = Convert.ToString(row.Cells[ColSSFName.Index].Value);
                using (BusinesContext _bc = new BusinesContext())
                {
                    List<int> prcs = _bc.DbContext.GetList<int>("select id from book.prices where id>2");
                    int id = _bc.SaveProduct(new Products
                    {
                        Code = code,
                        Name = name,
                        Uid = Guid.NewGuid().ToString(),
                        GroupId = 11,
                        Path = "0#1#10#11",
                        UnitId = unit_id,
                        DefaultUnitId = unit_id,
                        ReportUnitId = unit_id,
                        SpecialUnitId = unit_id,
                        Vat = true,
                        VatType = 1,
                        NsafGoodId = (int)row.Cells[ColSSFId.Index].Value,
                        NsafCode = Convert.ToString(row.Cells[ColSSFCode.Index].Value),
                        NsafName = name,
                        ProductPrices = prcs.Select(p => new ProductPrices
                        {
                            PriceId = p,
                            ManualVal = p == 3 ? price_val : 0,
                        }).ToList()
                    });
                    if (id <= 0)
                    {
                        misseds.Add(name);
                        continue;
                    }
                }
            }
            ProgressDispatcher.Deactivate();
            if (misseds.Any())
                MessageBoxForm.Show(Application.ProductName, "ზოგიერთი საქონლის დამატება ვერ მოხერხდა!", string.Join(", ", misseds.ToArray()), null, SystemIcons.Error);
            UpdateGrid(type);
        }

        private void OnAddContragent(bool all)
        {
            if (m_Grid.Rows.Count == 0)
                return;
            var rows = m_Grid.Rows.OfType<DataGridViewRow>().Where(x => (all || (!all && x.Selected)) && (byte)x.Cells[ColType.Index].Value < 3 && (int)x.Cells[ColContragentId.Index].Value < 0);
            if (!rows.Any())
            {
                MessageBoxForm.Show(Application.ProductName, string.Format("{0} კონტრაგენტი არ საჭიროებს დამატებას.", !all ? "მონიშნული" : "ცხრილში არსებული არცერთი"), null, null, SystemIcons.Information);
                return;
            }
            
            byte type = (byte)rows.First().Cells[ColType.Index].Value;
            HashSet<string> misseds = new HashSet<string>();
            ProgressDispatcher.Activate();
            foreach (var row in rows)
            {
                string code = Convert.ToString(row.Cells[ColContragentCode.Index].Value);
                string name = Convert.ToString(row.Cells[ColContragent.Index].Value);
                using (BusinesContext _bc = new BusinesContext())
                {
                    int id = _bc.SaveContragent(new Contragents
                    {
                        GroupId = type == 0 ? 3 : 5,
                        Path = type == 0 ? "0#1#3" : "0#2#5",
                        Code = code,
                        Name = name,
                        ShortName = name,
                        Type = 1,
                        VatType = Convert.ToInt32(Pm.GetRSService().isPayerVat(code)),
                        IsResident = 0
                    });
                    if (id <= 0)
                    {
                        misseds.Add(name);
                        continue;
                    }
                }
            }
            ProgressDispatcher.Deactivate();
            if (misseds.Any())
                MessageBoxForm.Show(Application.ProductName, "ზოგიერთი კონტრაგენტის დამატება ვერ მოხერხდა!", string.Join(", ", misseds.ToArray()),  null, SystemIcons.Error);
            UpdateGrid(type);
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            m_Grid.EndEdit();
            if (m_Grid.Rows.Count == 0)
                return;
            var ready_rows = m_Grid.Rows.OfType<DataGridViewRow>().Where(r => Convert.ToBoolean(r.Cells[ColCheck.Index].Value)).ToList();
            if (!ready_rows.Any())
                return;
            byte tp = (byte)ready_rows.First().Cells[ColType.Index].Value;
           
            ProgressDispatcher.Activate(true);
            Pm.InitialiseLog(new List<string> { "ოპერაციის თარიღი", "გამოწერის თარიღი", "ნომერი", "თანხა", "ოპერაცია (FINA)", "შედეგი" });
            double vat = Special.ConfigParams["vat"].ParseScalar<double>();
            int user_id = Pm.GetUserID();
            int staff = Pm.GetDataManager().GetUserStaff(user_id);
           
            List<int> recalculate = new List<int>();
            DocTypes _dp = tp == 0 ? DocTypes.ProductIn : (tp > 0 && tp < 3) ? DocTypes.ProductOut : DocTypes.ProductMove;
            int i = 1;
            using (BusinesContext _bc = new BusinesContext())
            {
                Dictionary<KeyValuePair<int, string>, long> _nm = _bc.GenerateDocNums(new List<int> { (int)_dp }, null);
                long doc_num = !_nm.Any() ? 1 : _nm.First().Value;
                foreach (DataGridViewRow row in ready_rows)
                {
                    decimal per = (decimal)i / (decimal)ready_rows.Count * 100M;
                    ProgressDispatcher.Percent = ((int)per);
                    i++;

                    DateTime tdate = Convert.ToDateTime(row.Cells[ColWriteDate.Index].Value);
                    DateTime oper_date = Convert.ToDateTime(row.Cells[ColOperDate.Index].Value);
                    int store_id = (int)row.Cells[ColStoreId1.Index].Value;
                    int project_id = Pm.GetDataManager().GetStoreProjectID(store_id);

                    //if (!(tdate.Year == oper_date.Year && tdate.Month == oper_date.Month))
                    //    tdate = oper_date.Date.AddHours(10);

                    // vigacas unda ase tvini shechama!!!!
                    tdate = oper_date;



                    DateTime _transport_begin = row.Cells[ColTransportStartDate.Index].Value is DateTime ? Convert.ToDateTime(row.Cells[ColTransportStartDate.Index].Value) : tdate;
                    string transport_mark = Convert.ToString(row.Cells[ColTransportMark.Index].Value);
                    string trans_model;
                    string trans_number;
                    trans_model = trans_number = transport_mark;
                    if (transport_mark.Contains('|'))
                    {
                        string[] tmp = transport_mark.Split('|');
                        trans_model = tmp[0];
                        trans_number= tmp[1];
                    }

                    DocItem doc = new DocItem
                    {
                        GeneralDocsItem = new GeneralDocs
                        {
                            Tdate = tdate,
                            DocNum = doc_num++,
                            DocType = (int)_dp,
                            Purpose = string.Format("ჩამოტვირთული Nsaf ({0}) ", row.Cells[ColFullNum.Index].Value),
                            Amount = (double)row.Cells[ColAmount.Index].Value,
                            Vat = vat,
                            UserId = user_id,
                            StatusId = 1,
                            MakeEntry = true,
                            ProjectId = project_id,
                            HouseId = 1,
                            StaffId = staff,
                            ParamId1 = (int)row.Cells[ColContragentId.Index].Value,
                            ParamId2 = store_id,
                            WaybillNum = row.Cells[ColNum.Index].Value.ToString(),
                            Uid = Guid.NewGuid().ToString()
                        },
                        ProductsFlowList = new List<ProductsFlow>()
                    };

                    int res = -1;
                    switch (_dp)
                    {
                        case DocTypes.ProductIn:
                            {
                                doc.ProductInItem = new ProductIn
                                {
                                    Doc1 = Convert.ToString(row.Cells[ColNum.Index].Value),
                                    Doc2 = Convert.ToString(row.Cells[ColFullNum.Index].Value),
                                    Doc1Date = oper_date,
                                    Doc2Date = oper_date,
                                };
                                doc.ProductsFlowList.Add(new ProductsFlow
                                {
                                    ProductId = (int)row.Cells[ColProductId.Index].Value,
                                    Amount = (double)row.Cells[ColQuant.Index].Value,
                                    Price = (double)row.Cells[ColPrice.Index].Value,
                                    UnitId = (int)row.Cells[ColUnitId.Index].Value,
                                    StoreId = store_id,
                                    VatPercent = (int)row.Cells[ColVatType.Index].Value == 1 ? 18 : 0,
                                    Coeff = 1,
                                    VendorId = (int)row.Cells[ColContragentId.Index].Value
                                });
                                res = _bc.SaveProductIn(doc);
                                break;
                            }
                        case DocTypes.ProductOut:
                            {
                                doc.ProductOutItem = new ProductOut
                                {
                                    PayType = "0",
                                    PayDate = tdate,
                                    DeliveryDate = tdate,
                                    WaybillType = 2,
                                    TransportTypeId = 1,
                                    TransportCostPayer = 2,
                                    TransportBeginDate = _transport_begin,
                                    ActivateDate = _transport_begin.AddSeconds(10),
                                    ResponsablePersonDate = oper_date.Date,
                                    IsForeign = Convert.ToByte(!Convert.ToBoolean(row.Cells[ColDriverIsGeo.Index].Value)),
                                    TransporterIdNum = Convert.ToString(row.Cells[ColDriverId.Index].Value),
                                    DriverName = Convert.ToString(row.Cells[ColDriverName.Index].Value),
                                    WaybillStatus = (int)row.Cells[ColStatusId.Index].Value,
                                    WaybillNum = Convert.ToString(row.Cells[ColNum.Index].Value),
                                    IsWaybill = 1,
                                    TransportStartPlace = Convert.ToString(row.Cells[ColStartAddress.Index].Value),
                                    TransportEndPlace = Convert.ToString(row.Cells[ColEndAddress.Index].Value),
                                    TransportNumber = trans_number,
                                    TransportModel = trans_model,
                                    NsafType = (byte)row.Cells[ColType.Index].Value,
                                    NsafTransportType = m_TransportType.ContainsKey(Convert.ToString(row.Cells[ColTransportType.Index].Value)) ? m_TransportType[Convert.ToString(row.Cells[ColTransportType.Index].Value)] : (byte)1,
                                    NsafStartNum = Convert.ToString(row.Cells[ColStartAddressN.Index].Value),
                                    NsafEndNum = Convert.ToString(row.Cells[ColEndAddressN.Index].Value),
                                    NsafMode = true,
                                    NsafSeries = Convert.ToString(row.Cells[ColSeries.Index].Value),
                                    NsafTransporterNum = Convert.ToString(row.Cells[ColCarrierId.Index].Value),
                                    NsafTransporterName = Convert.ToString(row.Cells[ColCarrierName.Index].Value),
                                    WaybillId = Convert.ToInt32(row.Cells[ColID.Index].Value)
                                };
                                doc.ProductsFlowList.Add(new ProductsFlow
                                {
                                    ProductId = (int)row.Cells[ColProductId.Index].Value,
                                    Amount = (double)row.Cells[ColQuant.Index].Value,
                                    Price = (double)row.Cells[ColPrice.Index].Value,
                                    UnitId = (int)row.Cells[ColUnitId.Index].Value,
                                    StoreId = store_id,
                                    VatPercent = (int)row.Cells[ColVatType.Index].Value == 1 ? 18 : 0,
                                    Coeff = -1
                                });
                                res = _bc.SaveProductOut(doc);
                                break;
                            }
                        case DocTypes.ProductMove:
                            {
                                doc.GeneralDocsItem.ParamId1 = store_id;
                                doc.GeneralDocsItem.ParamId2 = (int)row.Cells[ColStoreId2.Index].Value;
                                doc.ProductMoveItem = new ProductMove
                                {
                                    TransportCostPayer = 2,
                                    TransportBeginDate = _transport_begin,
                                    ActivateDate = _transport_begin.AddSeconds(10),
                                    ResponsablePersonDate = oper_date.Date,
                                    IsForeign = Convert.ToByte(!Convert.ToBoolean(row.Cells[ColDriverIsGeo.Index].Value)),
                                    TransporterIdNum = Convert.ToString(row.Cells[ColDriverId.Index].Value),
                                    DriverName = Convert.ToString(row.Cells[ColDriverName.Index].Value),
                                    WaybillStatus = (int)row.Cells[ColStatusId.Index].Value,
                                    WaybillNum = Convert.ToString(row.Cells[ColNum.Index].Value),
                                    TransportStartPlace = Convert.ToString(row.Cells[ColStartAddress.Index].Value),
                                    TransportEndPlace = Convert.ToString(row.Cells[ColEndAddress.Index].Value),
                                    TransportNumber = trans_number,
                                    TransportModel = trans_model,
                                    NsafType = (byte)row.Cells[ColType.Index].Value,
                                    NsafTransportType = m_TransportType.ContainsKey(Convert.ToString(row.Cells[ColTransportType.Index].Value)) ? m_TransportType[Convert.ToString(row.Cells[ColTransportType.Index].Value)] : (byte)1,
                                    NsafStartNum = Convert.ToString(row.Cells[ColStartAddressN.Index].Value),
                                    NsafEndNum = Convert.ToString(row.Cells[ColEndAddressN.Index].Value),
                                    NsafMode = true,
                                    NsafSeries = Convert.ToString(row.Cells[ColSeries.Index].Value),
                                    NsafTransporterNum = Convert.ToString(row.Cells[ColCarrierId.Index].Value),
                                    NsafTransporterName = Convert.ToString(row.Cells[ColCarrierName.Index].Value),
                                    WaybillId = Convert.ToInt32(row.Cells[ColID.Index].Value)
                                };
                                doc.ProductsFlowList.AddRange(new List<ProductsFlow>()
                                {
                                    new ProductsFlow { ProductId = (int)row.Cells[ColProductId.Index].Value, Amount = (double)row.Cells[ColQuant.Index].Value,  UnitId = (int)row.Cells[ColUnitId.Index].Value, StoreId = store_id, Coeff = -1, IsMove = 1 },
                                    new ProductsFlow { ProductId = (int)row.Cells[ColProductId.Index].Value, Amount = (double)row.Cells[ColQuant.Index].Value,  UnitId = (int)row.Cells[ColUnitId.Index].Value, StoreId = (int)row.Cells[ColStoreId2.Index].Value, Coeff = 1, IsMove = 1, Visible = 0}
                                });
                                res = _bc.SaveProductMove(doc);
                                break;
                            }
                    }
                    if (res < 0)
                        Pm.AddLogFormItem(new List<string> { oper_date.ToString("dd/MM/yyyy HH:mm"), Convert.ToDateTime(row.Cells[ColWriteDate.Index].Value).ToString("dd/MM/yyyy HH:mm"), row.Cells[ColNum.Index].Value.ToString(), row.Cells[ColAmount.Index].Value.ToString(), null, string.Format("შეცდომა ჩატვირთვისას ({0})", _bc.ErrorEx) }, 0);
                    else
                    {
                        recalculate.Add(doc.ProductsFlowList.First().ProductId);
                        row.Cells[ColCheck.Index].Value = false;
                        row.Cells[ColCheck.Index].ReadOnly = true;
                        row.Cells[ColConditionValue.Index].Value = NsafFacturaTips.Downloaded;
                        row.Cells[ColConditionText.Index].Value = EnumEx.GetEnumDescription(NsafFacturaTips.Downloaded);
                        row.Cells[ColFinaOperation.Index].Value = doc.GeneralDocsItem.Purpose;
                        Pm.AddLogFormItem(new List<string> { oper_date.ToString("dd/MM/yyyy HH:mm"), Convert.ToDateTime(row.Cells[ColWriteDate.Index].Value).ToString("dd/MM/yyyy HH:mm"), row.Cells[ColNum.Index].Value.ToString(), row.Cells[ColAmount.Index].Value.ToString(), doc.GeneralDocsItem.Purpose, "წარმატებით ჩაიტვირთა" }, 1);
                    }
                }
                if (recalculate.Any())
                    _bc.RecalculateRestAmounts(recalculate, null);
            }
            ProgressDispatcher.Deactivate();
            Pm.ShowLogForm();
        }

        private void menuProductAddSel_Click(object sender, EventArgs e)
        {
            OnAddProduct(false);
        }

        private void menuProductAddAll_Click(object sender, EventArgs e)
        {
            OnAddProduct(true);
        }

        private void menuContragentAddSel_Click(object sender, EventArgs e)
        {
            OnAddContragent(false);
        }

        private void menuContragentAddAll_Click(object sender, EventArgs e)
        {
            OnAddContragent(true);
        }
    }
}
