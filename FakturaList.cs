using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using System.Collections;
using ipmFunc;
using System.Data.SqlClient;
using System.ComponentModel;
using ipmControls;
using System.Drawing;
using ipmPMBasic.Models;
using System.Transactions;
using ipmDocSubForms;
using ipmBLogic;

namespace ipmExtraFunctions
{
    public partial class FakturaList : Form
    {
        enum FacturaTips
        {
            [Description("ჩამოტვირთულია")]
            Downloaded,
            [Description("ჩამოტვირთვა შესაძლებელია")]
            Available,
            [Description("უცხო შიგთავსი")]
            Unavailable,
            [Description("უცხო კონტარგენტი")]
            UnavailableContragent,
            [Description("უცხო ზედნადებები")]
            UnavailableWaybills,
            [Description("ვერ ჩამოიტვირთა, პრობლემა RS-ზე")]
            UnDownloadable
        }
        private bool _LockSelection = false;
        private string clipboardData = string.Empty;
        private bool _unkown_contragent_exists = false;


        private ProgramManagerBasic m_Pm;
        private List<int> ExcluseServiceIns { get; set; }
        private Dictionary<int, string> m_Status = new Dictionary<int, string>() { { 0, "ატვითული" }, { 1, "გადაგზავნილი" }, { 2, "დადასტურებული" }, { 3, "პირველადი კორექტირებულის" }, { 4, "ახალი კორექტირების" },  { 5, "კორექტირების დასადასტურებელი" }, { 6, "გაუქმებული" },{ 7, "დადასტურებული გაუქმებული" }, { 8, "კორექტირების დადასტურებული" } };
        private Dictionary<FacturaTips, Color> StatusColors = new Dictionary<FacturaTips, Color> { { FacturaTips.Downloaded, Color.MediumSeaGreen }, { FacturaTips.Available, Color.Gold }, { FacturaTips.Unavailable, Color.Tomato }, { FacturaTips.UnavailableContragent, Color.Tomato }, { FacturaTips.UnavailableWaybills, Color.Tomato }, { FacturaTips.UnDownloadable, Color.Red } };

        public FakturaList(ProgramManagerBasic pm)
        {
            InitializeComponent();
            m_Pm = pm;
            m_Pm.TranslateControl(this);
        }       
        private ProgramManagerBasic GetProgramManager()
        {
            return this.m_Pm;
        }

        public void FillControls()
        {
            fillComboFilter();
            m_Period.dtp_From.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, 100);
            m_Period.dtp_To.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59, 59, 997);
        }

        private void fillComboFilter()
        {
            string[] data = m_Grid.Columns.OfType<DataGridViewColumn>()
                .Where(c => c.HeaderText != "" && c.Visible)
                .Select(c => c.HeaderText)
                .ToArray();
            comboFilter.Items.AddRange(data);
            comboFilter.SelectedIndex = 0;

            comboFilterMethod.DataSource = new BindingSource {DataSource=new Dictionary<byte, string> { { 1, "ოპერაციის თარიღი" }, { 2, "გამოწერის თარიღი" } } };
            comboFilterMethod.ValueMember = "Key";
            comboFilterMethod.DisplayMember = "Value";
        }

        private string GetFacturaStatus(int status_id)
        {
            if (m_Status.ContainsKey(status_id))
                return m_Status[status_id];
            return string.Empty;
        }
        private void onWaybillsLoad()
        {
            m_Grid.Rows.Clear();
            m_GridItems.Rows.Clear();
            m_GridWaybills.Rows.Clear();
            _unkown_contragent_exists = false;
            ProgressDispatcher.Activate(true);
            DateTime _from = new DateTime(m_Period.dtp_From.Value.Year, m_Period.dtp_From.Value.Month, m_Period.dtp_From.Value.Day, 0, 0, 0, 100);
            DateTime _to = new DateTime(m_Period.dtp_To.Value.Year, m_Period.dtp_To.Value.Month, m_Period.dtp_To.Value.Day, 23, 59, 59, 997);
            DateTime _from2 = _from;
            DateTime _to2 = _to;
            if ((byte)comboFilterMethod.SelectedValue == 1)
            {
                _from = DateTime.MinValue;
                _to = DateTime.MaxValue;
            }
            else
            {
                _from2 = DateTime.MinValue;
                _to2 = DateTime.MaxValue;
            }


            DataTable data = GetProgramManager().GetRSService().GetBuyerInvoices(_from, _to,  _from2, _to2, string.Empty, string.Empty);
            if (data == null || data.Rows.Count <= 0)
            {
                ProgressDispatcher.Deactivate();
                return;
            }
            _LockSelection = true;
            var _order_data = data.AsEnumerable().OrderByDescending(a => Convert.ToDateTime(a["reg_dt"])).ToList();
            Dictionary<string, int> myContragegents = GetProgramManager().GetDataManager().GetDictionary<string, int>("SELECT DISTINCT code, id FROM book.Contragents WHERE (path ='0#1#3' OR path LIKE '0#1#3#%') AND code IS NOT NULL AND code!='' ");
            if (myContragegents == null)
                return;
            int i = 1;
            foreach (DataRow row in _order_data)
            {
                decimal per = (decimal)i / (decimal)data.Rows.Count * 100M;
                ProgressDispatcher.Percent = ((int)per);

                int status = 0;
                if (!int.TryParse(Convert.ToString(row["status"]), out status))
                    continue;

                int contragent_id = -1;
                string contargent_code = Convert.ToString(row["SA_IDENT_NO"]);
               
                if (myContragegents.ContainsKey(contargent_code))
                    contragent_id = myContragegents[contargent_code];

                int index = m_Grid.Rows.Add(false, row["Id"], row["operation_dt"], row["reg_dt"], row["f_series"], row["f_number"], contragent_id, row["SA_IDENT_NO"], row["ORG_NAME"],  null , row["TANXA"], GetFacturaStatus(status), status);
                m_Grid.Rows[index].Cells[ColInside.Index].Value = new InvoiceItem { FinaGeneralData = new Dictionary<int, KeyValuePair<DateTime, string>>() };
                UpdateDetailInfo(index);
                i++;
            }
            UpdateFactStatusLabel();
            ProgressDispatcher.Deactivate();
            _LockSelection = false;
            ShowInside();
        }
        private bool UpdateDetailInfo(int index)
        {

            bool _rs_crash = false;
            DataGridViewRow row = m_Grid.Rows[index];
            int id = Convert.ToInt32(row.Cells[ColID.Index].Value);

            bool? f_exists = GetProgramManager().GetDataManager().GetScalar<bool>("IF EXISTS (SELECT d.id FROM doc.ProductIn_Doc d where d.series = @ser AND d.num=@num) SELECT 'TRUE' ELSE SELECT 'FALSE'", new SqlParameter[]
            {
                new SqlParameter("@ser", SqlDbType.NVarChar) { Value=row.Cells[f_series.Index].Value},
                new SqlParameter("@num", SqlDbType.NVarChar) { Value=row.Cells[f_number.Index].Value}
            });

            Func<string, Products> GetMyMaterial = (string name) =>
            {
                if (string.IsNullOrEmpty(name))
                    return new Products();
                using (DBContext _db = new DBContext())
                {
                    Products my_product = _db.GetList<Products>("SELECT TOP 1 id AS Id, name AS Name, path AS Path  FROM book.Products WHERE  name =@name AND (path='0#2#120' OR path='0#1#10' OR path LIKE '0#2#120#%' OR path LIKE '0#1#10#%')  ", new SqlParameter("@name", SqlDbType.NVarChar) { Value = name }).FirstOrDefault();
                    if (my_product == null)
                        my_product = _db.GetList<Products>("SELECT TOP 1 p.id AS Id, p.name AS Name, p.path AS Path FROM book.Products AS p INNER JOIN book.ProductNames AS pn ON pn.product_id=p.id WHERE  pn.product_nam =@name AND (p.path='0#2#120' OR p.path='0#1#10' OR p.path LIKE '0#2#120#%' OR p.path LIKE '0#1#10#%')", new SqlParameter("@name", SqlDbType.NVarChar) { Value = name }).FirstOrDefault();
                    return my_product ?? new Products();
                }
            };
            
            InvoiceItem _info = (InvoiceItem)row.Cells[ColInside.Index].Value;
            _info.InvoiceId = Convert.ToInt32(row.Cells[ColID.Index].Value);
            _info.OperationDate = Convert.ToDateTime(row.Cells[operation_dt.Index].Value);
            _info.RegistrationDate = Convert.ToDateTime(row.Cells[reg_dt.Index].Value);
            _info.Series = Convert.ToString(row.Cells[f_series.Index].Value);
            _info.Number = Convert.ToString(row.Cells[f_number.Index].Value);
            _info.ContargentId = Convert.ToInt32(row.Cells[ColcontragentId.Index].Value);
            _info.Amount = row.Cells[ColAmount.Index].Value.TryParseScalar<double>();
            _info.RsStatus = Convert.ToInt32(row.Cells[waybill_statusID.Index].Value);
            if (_info.GoodList == null)
            {
                bool is_all_match = true;
                DataTable waybills = GetProgramManager().GetRSService().GetFakturaWaybills(id);
                if (waybills.Rows.Count > 0)
                {
                    Dictionary<string, DateTime> rs_waybil_nums = waybills.AsEnumerable().Select(a => new KeyValuePair<string, DateTime>(Convert.ToString(a["OVERHEAD_NO"]), Convert.ToDateTime(a["OVERHEAD_DT"]))).Distinct().ToDictionary(k => k.Key, v => v.Value);
                    string sql = string.Format(@"SELECT ISNULL(waybill_num, ''), id FROM doc.generaldocs WHERE doc_type=16 AND waybill_num IN({0})", string.Join(",", rs_waybil_nums.Select(a => string.Format("'{0}'", a.Key)).ToArray()));
                    Dictionary<string, int> my_waybills = GetProgramManager().GetDataManager().GetDictionary<string, int>(sql);
                    string waybil_string = string.Format("RS-ზე: {0}, FINA-ში: {1}", string.Join(", ", rs_waybil_nums.Keys.ToArray()), string.Join(", ", my_waybills.Keys.ToArray()));
                    foreach (var r in rs_waybil_nums)
                    {
                        if (!my_waybills.ContainsKey(r.Key))
                            is_all_match = false;
                        else
                            _info.FinaGeneralData.Add(my_waybills[r.Key], new KeyValuePair<DateTime, string>(r.Value, r.Key));
                    }
                    row.Cells[ColWaybills.Index].Value = waybil_string;
                }
                _info.IsRsWaybillsMatch = is_all_match;
               
                try
                {
                    DataTable details = GetProgramManager().GetRSService().GetFakturaItems(id);
                    _info.GoodList = details.AsEnumerable().GroupBy(a => Convert.ToString(a["GOODS"])).Select(a => new { a = a, prods = GetMyMaterial(a.Key) })
                                             .Select(a => new Good
                                             {
                                                 InvoiceId = Convert.ToInt32(a.a.First()["INV_ID"]),
                                                 Name = a.a.Key,
                                                 UnitName = a.a.First()["G_UNIT"].ToString(),
                                                 Quantity = a.a.Select(b => b["G_NUMBER"].TryParseScalar<double>() == 0 ? 1 : b["G_NUMBER"].TryParseScalar<double>()).DefaultIfEmpty(1).Sum(),
                                                 Amount = a.a.Select(b => b["FULL_AMOUNT"].TryParseScalar<double>()).DefaultIfEmpty().Sum(),
                                                 VatType = a.a.First()["VAT_TYPE"].TryParseScalar<int>(),
                                                 VatAmount = a.a.Select(b => b["DRG_AMOUNT"].TryParseScalar<double>()).DefaultIfEmpty().Sum(),
                                                 IsService = (!string.IsNullOrEmpty(a.prods.Path) && (a.prods.Path == "0#2#120" || a.prods.Path.StartsWith("0#2#120#"))),
                                                 Id = a.prods.Id,
                                                 FinaName = a.prods.Name,
                                             }).ToList();
                }
                catch
                {
                    _rs_crash = true;
                }
            }
            else
            {
                List<int> _ii = _info.FinaGeneralData.Keys.ToList();
                var my = GetProgramManager().GetDataManager().GetSerInside(_ii).AsEnumerable().Select(a => new
                {
                    _id = Convert.ToInt32(a["service_id"]),
                    _quant = Convert.ToDouble(a["quant"]),
                    _total = Convert.ToDouble(a["total"]),
                }).ToList();

                _info.GoodList.ForEach(g => 
                {
                    Products _my_data = GetMyMaterial(g.Name);
                    g.Id = _my_data.Id;
                    g.IsService = (!string.IsNullOrEmpty(_my_data.Path) && (_my_data.Path == "0#2#120" || _my_data.Path.StartsWith("0#2#120#")));
                    g.FinaName = _my_data.Name;
                    g.FinaQuantity = my.Where(a => a._id == g.Id).Select(a => a._quant).FirstOrDefault();
                    g.FinaAmount = my.Where(a => a._id == g.Id).Select(a => a._total).FirstOrDefault();
                });
            }

            FacturaTips _tp = FacturaTips.Unavailable;
            if (_rs_crash)
                _tp = FacturaTips.UnDownloadable;
          
            else
            {
                if (f_exists.Value)
                    _tp = FacturaTips.Downloaded;
                else if (_info.ContargentId < 0)
                {
                    _tp = FacturaTips.UnavailableContragent;
                    _unkown_contragent_exists = true;
                }
                else if (!_info.IsRsWaybillsMatch)
                    _tp = FacturaTips.UnavailableWaybills;
                else if (_info.GoodList.Any() && !_info.GoodList.Where(a => a.Id <= 0).Any())
                {
                    _tp = FacturaTips.Available;
                    //analyze match docs
                }
            }

            row.Cells[load_status.Index].Value = _tp;
            row.Cells[ColResult.Index].Value = EnumEx.GetEnumDescription(_tp);
            row.Cells[ColInside.Index].Value = _info;
            row.Cells[ColCheck.Index].ReadOnly = _tp != FacturaTips.Available;
            row.DefaultCellStyle.BackColor = StatusColors[_tp];
            return true;
        }


        private void ShowInside()
        {
            if (m_Grid.SelectedRows.Count == 0)
                return;
            m_GridItems.Rows.Clear();
            m_GridWaybills.Rows.Clear();
            lblsum.ForeColor = Color.Black;
            lblsum.Text = "ჯამი:";
            FacturaTips _tp = (FacturaTips)m_Grid.SelectedRows[0].Cells[load_status.Index].Value;
            toolStripInside.Enabled =  toolStripWaybill.Enabled = _tp != FacturaTips.Downloaded;
            if (_tp == FacturaTips.UnDownloadable)
                return;
            InvoiceItem ins = (InvoiceItem)m_Grid.SelectedRows[0].Cells[ColInside.Index].Value;
            if (ins == null)
                return;
            ins.GoodList.ForEach(g =>
            {
                string prod_type = "უცნობია";
                Bitmap _product_exists = Properties.Resources.forbidden;
                if (g.Id > 0)
                {
                    _product_exists = Properties.Resources.select_16;
                    prod_type = g.IsService ? "მომსახურება" : "საქონელი";
                }
                Bitmap _match_document = g.ExistInDoc ? Properties.Resources.bullet_ball_glass_green : Properties.Resources.bullet_ball_glass_red;
                string info_string = !g.IsService ? " - " : string.Format("{0} -> რაოდ:{1:F2} -> თანხა:{2:F2}", g.FinaName, g.FinaQuantity, g.FinaAmount);
                int ind = m_GridItems.Rows.Add(g.Id, _product_exists, g.Name, prod_type, g.UnitName, g.Quantity, g.Amount, g.VatAmount, _match_document, info_string);
            });

            foreach (var doc in ins.FinaGeneralData)
                m_GridWaybills.Rows.Add(doc.Key, doc.Value.Key, doc.Value.Value);

            double _rs_sum = ins.GoodList.Select(a => a.Amount).DefaultIfEmpty().Sum();
            double _fina_sum = ins.GoodList.Select(a => a.FinaAmount).DefaultIfEmpty().Sum();
            lblsum.ForeColor = _rs_sum == _fina_sum ? Color.Green : Color.Red;
            lblsum.Text = string.Format("ჯამი (RS): {0:F2};    ჯამი (FINA):{1:F2}", _rs_sum, _fina_sum);
        }


        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (m_GridItems.SelectedRows.Count == 0)
                return;
            GetProgramManager().SetTempTable(new Hashtable()
            {
                {"waybill_prod_name",m_GridItems.SelectedRows[0].Cells[ColItemsName.Index].Value},
                {"path", "0#2#120" },
               // {"vat_info", m_GridItems.SelectedRows[0].Cells[ColItemsVat.Index].Value}
                });
            GetProgramManager().ExecuteCommandByTag("SHOW_DOC_PRODUCT");
            GetProgramManager().SetTempTable(null);
        }

        private void btnConnectItem_Click(object sender, EventArgs e)
        {
            if (m_GridItems.SelectedRows.Count == 0)
                return;
            int index = m_GridItems.SelectedRows[0].Index;
            int res = GetProgramManager().ShowSelectForm("TABLE_PRODUCT:SERVICE_IN", -1);
            if (res == -1)
                return;
            string name = m_GridItems.Rows[index].Cells[ColItemsName.Index].Value.ToString();

            int? ins_res = GetProgramManager().GetDataManager().ExecuteSql("INSERT INTO book.ProductNames (product_nam, product_id) VALUES(@nm, @id)", new SqlParameter[]
            {
                new SqlParameter("@nm", SqlDbType.NVarChar) {Value=name },
                new SqlParameter("@id", SqlDbType.Int) {Value=res }
            });
            if (!ins_res.HasValue || ins_res.Value <= 0)
                return;

            UpdateDetailInfo(m_Grid.SelectedRows[0].Index);
            ShowInside();
        }

        private void btnInsideRefresh_Click(object sender, EventArgs e)
        {
            if (m_Grid.SelectedRows.Count == 0)
                return;
            UpdateDetailInfo(m_Grid.SelectedRows[0].Index);
            ShowInside();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            onWaybillsLoad();
        }

        private void m_Grid_SelectionChanged(object sender, EventArgs e)
        {
            if (_LockSelection)
                return;
            ShowInside();
        }

        private void copyItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(clipboardData);
        }

        private void m_Grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                clipboardData = Convert.ToString(m_Grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                menuFactura.Show(m_Grid, m_Grid.PointToClient(Cursor.Position));
            }
        }

        private void selectIem_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0 || m_Grid.SelectedRows.Count == 0)
                return;
            foreach (DataGridViewRow rw in m_Grid.Rows)
            {
                if ((FacturaTips)rw.Cells[load_status.Index].Value != FacturaTips.Available)
                    continue;
                rw.Cells[ColCheck.Index].Value = true;
            }
        }

        private void unselectItem_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0)
                return;
            foreach (DataGridViewRow rw in m_Grid.Rows)
                rw.Cells[ColCheck.Index].Value = false;
        }

        private void m_GridItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (m_GridItems.Columns[e.ColumnIndex].Index == btnFinaDoc.Index)
            {
                if (Convert.ToInt32(m_GridItems.Rows[e.RowIndex].Cells[ColItemsId.Index].Value) <= 0)
                {
                    ShowInsideWarning();
                    return;
                }

                InvoiceItem ins = (InvoiceItem)m_Grid.SelectedRows[0].Cells[ColInside.Index].Value;
                if (ins == null)
                    return;
                using (ServiceInDocs x = new ServiceInDocs(GetProgramManager(), ins.ContargentId, m_Period.dtp_From.Value, m_Period.dtp_To.Value, false))
                {
                    if (x.ShowDialog() != DialogResult.OK)
                        return;
                    Dictionary<int, string> data = x.SelectedData;
                    if (UpdateInsideGeneralData(e.RowIndex, ins, x.SelectedData.First()))
                    {
                        UpdateDetailInfo(m_Grid.SelectedRows[0].Index);
                        ShowInside();
                    }
                }
            }
            */
        }

        private void ShowInsideWarning()
        {
            MessageBoxForm.Show(Application.ProductName, "დოკუმენტის არჩევამდე მიუთითეთ მომსახურება", null, null, SystemIcons.Warning);
        }
        private bool UpdateInsideGeneralData(InvoiceItem ins, KeyValuePair<int, KeyValuePair<DateTime, string>> general)
        {
            List<Good> goods = ins.GoodList.Where(a => a.IsService && !a.ExistInDoc).ToList();
            var my = GetProgramManager().GetDataManager().GetSerInside(new List<int> { general.Key }).AsEnumerable().Select(a => new
            {
                _id = Convert.ToInt32(a["service_id"]),
                _quant = Convert.ToDouble(a["quant"]),
                _total = Convert.ToDouble(a["total"]),
            }).ToList();

            Func<bool> _compare = () => 
            {
                if (my.Count > goods.Count)
                    return false;
                foreach (var m in my)
                {
                    Good s = goods.Find(a => a.Id == m._id);
                    if (s == null || s.Quantity < m._quant || s.Amount < m._total)
                        return false;
                }
              return true;
            };
            return _compare();
        }

        private void FakturaList_Load(object sender, EventArgs e)
        {
            FillControls();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
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

        private void btnExecute_Click(object sender, EventArgs e)
        {
            OnExecute();
        }

        private void btnAddoperation_Click(object sender, EventArgs e)
        {
            if (m_GridItems.Rows.Count == 0)
                return;
            InvoiceItem ins = (InvoiceItem)m_Grid.SelectedRows[0].Cells[ColInside.Index].Value;
            if (ins == null)
                return;
            if (!ins.IsRsWaybillsMatch)
            {
                MessageBoxForm.Show(Application.ProductName, "სასაქონლო ზედნადებები უნდა იძებნებოდეს FINA -ში.", null, null, SystemIcons.Warning);
                return;
            }
            var _bad_items = m_GridItems.Rows.OfType<DataGridViewRow>().Where(a => Convert.ToInt32(a.Cells[ColItemsId.Index].Value) <= 0).ToList();
            if (_bad_items.Any())
            {
                m_GridItems.ClearSelection();
                _bad_items.ForEach(a => a.Selected = true);
                ShowInsideWarning();
                return;
            }
          
            using (ServiceInDocs x = new ServiceInDocs(GetProgramManager(), ins.ContargentId, m_Period.dtp_From.Value, m_Period.dtp_To.Value, ins.FinaGeneralData.Keys.ToList()))
            {
                if (x.ShowDialog() != DialogResult.OK)
                    return;

                if (!UpdateInsideGeneralData(ins, x.SelectedData))
                {
                    MessageBoxForm.Show(Application.ProductName, "არჩეული დოკუმენტი არ შეესაბამება ფაქტურის შიგთავსს.", null, null, SystemIcons.Warning);
                    return;
                }
                ins.FinaGeneralData.Add(x.SelectedData.Key, new KeyValuePair<DateTime, string>(x.SelectedData.Value.Key, x.SelectedData.Value.Value));
                UpdateDetailInfo(m_Grid.SelectedRows[0].Index);
                ShowInside();
            }
        }

        private void btnRemoveoperation_Click(object sender, EventArgs e)
        {
            if (m_GridWaybills.Rows.Count == 0 || m_GridWaybills.SelectedRows.Count == 0)
                return;
            if (!MessageBoxForm.ShowDialog(GetProgramManager().GetTranslatorManager().Translate(Application.ProductName), GetProgramManager().GetTranslatorManager().Translate("შესრულდეს მონიშნული ოპერაციის ამოღება?"), SystemIcons.Question).Value)
                return;
            InvoiceItem ins = (InvoiceItem)m_Grid.SelectedRows[0].Cells[ColInside.Index].Value;
            if (ins == null)
                return;

            //შეამოწმოს, ზედნადები არ ამოაშლევინოს
            List<int> _removables = m_GridWaybills.SelectedRows.OfType<DataGridViewRow>().Select(a => Convert.ToInt32(a.Cells[ColWaybillGeneralId.Index].Value)).ToList();
            List<int> _excludes = GetProgramManager().GetDataManager().GetList<int>("SELECT id FROM doc.generaldocs WHERE doc_type=16 AND id IN(" + string.Join(",", _removables.ConvertAll(Convert.ToString).ToArray()) + ")");
            if (_excludes.Any())
            {
                MessageBoxForm.Show(Application.ProductName, "სასაქონლო ზედნადებების ამოშლა სიიდან დაუშვებელია.", null, null, SystemIcons.Warning);
                _removables.RemoveAll(a => _excludes.Contains(a));
            }
            _removables.ForEach(r => 
            {
                if (ins.FinaGeneralData.ContainsKey(r))
                    ins.FinaGeneralData.Remove(r);
            });
            UpdateDetailInfo(m_Grid.SelectedRows[0].Index);
            ShowInside();
        }

        private void UpdateFactStatusLabel()
        {
            double _success_sum = 0.0;
            double _unsuccesss_sum = 0.0;
            int _success_cnt = 0;
            int _unsuccess_cnt = 0;
            foreach (DataGridViewRow rw in m_Grid.Rows)
            {
                double _sm = rw.Cells[ColAmount.Index].Value.TryParseScalar<double>();
                if ((FacturaTips)rw.Cells[load_status.Index].Value == FacturaTips.Downloaded)
                {
                    _success_sum += _sm;
                    _success_cnt++;
                }
                else
                {
                    _unsuccesss_sum += _sm;
                    _unsuccess_cnt++;
                }
            }

            lblResult.Text = string.Format("შედეგი: ჩამოტვირთულია {0} ფაქტურა თანხით: {1};  დარჩენილია {2} - თანხით: {3}", _success_cnt, _success_sum, _unsuccess_cnt, _unsuccesss_sum);
        }

        private void OnExecute()
        {
            m_Grid.EndEdit();
            lblResult.Text = "შედეგი:";
          
            if (m_Grid.Rows.Count == 0)
                return;
            List<InvoiceItem> ready_invoices = m_Grid.Rows.OfType<DataGridViewRow>().Where(a => Convert.ToBoolean(a.Cells[ColCheck.Index].Value)).Select(a=> (InvoiceItem)a.Cells[ColInside.Index].Value).ToList();

            if (!ready_invoices.Any())
                return;

            if (!MessageBoxForm.ShowDialog(GetProgramManager().GetTranslatorManager().Translate(Application.ProductName),  GetProgramManager().GetTranslatorManager().Translate("შესრულდეს მონიშნული ა/ფაქტურების ჩამოტვირთვა?"),SystemIcons.Question).Value)
                return;

            GetProgramManager().InitialiseLog(new List<string> { "თარიღი", "რეგ. თარიღი", "სერია, ნომერი", "FINA ში შესრულება", "თანხა",  "შედეგი" });
            HashSet<int> affected_invoices = new HashSet<int>();
            List<string> fina_actiona = new List<string>();
            ProgressDispatcher.Activate(true);
            int i = 1;
            foreach (InvoiceItem inv in ready_invoices)
            {
                decimal per = (decimal)i / (decimal)ready_invoices.Count * 100M;
                ProgressDispatcher.Percent = ((int)per);
                i++;

                List<Good> need_create_servicein = inv.GoodList.Where(a => !a.ExistInDoc && a.IsService).GroupBy(a => a.Id).Select(a => new Good
                {
                    Id = a.Key,
                    IsService = a.First().IsService,
                    VatAmount = a.Select(b => b.VatAmount).DefaultIfEmpty().Sum(),
                    Quantity = a.Select(b => b.Quantity - b.FinaQuantity).DefaultIfEmpty().Sum(),
                    Amount = a.Select(b => b.Amount - b.FinaAmount).DefaultIfEmpty().Sum()
                }).ToList();

                fina_actiona.Clear();
                if (inv.FinaGeneralData.Any())
                    fina_actiona.Add(string.Format("არსებული ზედნადებები (მიღებული მომსახურებები): {0}", string.Join(", ", inv.FinaGeneralData.Values.Select(a => a.Value).ToArray())));

                GetProgramManager().GetDataManager().Close();
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 3, 0) }))
                {
                    GetProgramManager().GetDataManager().Open();
                    //save service_in
                    if (need_create_servicein.Any())
                    {
                        int _ser_id = SaveServiceIn(inv, need_create_servicein);
                        if (_ser_id <= 0)
                        {
                            Transaction.Current.Rollback();
                            GetProgramManager().AddLogFormItem(new List<string> { inv.OperationDate.ToString("dd/MM/yyyy HH:mm"), inv.RegistrationDate.ToString("dd/MM/yyyy HH:mm"), string.Format("{0}, {1}", inv.Series, inv.Number), GetProgramManager().GetDataManager().ErrorEx, inv.Amount.ToString(), "შეცდომა მომსახურების მიღებისას" }, 0);
                            continue;
                        }
                        fina_actiona.Add(string.Format("შეიქმნა ოპერაცია: მომსახურების მიღება ა/ფაქტურისთვის {0} {1}", inv.Series, inv.Number));
                    }
                    if (!SaveFactura(inv))
                    {
                        Transaction.Current.Rollback();
                        GetProgramManager().AddLogFormItem(new List<string> { inv.OperationDate.ToString("dd/MM/yyyy HH:mm"), inv.RegistrationDate.ToString("dd/MM/yyyy HH:mm"), string.Format("{0}, {1}", inv.Series, inv.Number), GetProgramManager().GetDataManager().ErrorEx, inv.Amount.ToString(), "შეცდომა ფაქტურის შენახვისას" }, 0);
                        continue;
                    }
                    scope.Complete();
                }


                fina_actiona.Add(string.Format("შეიქმნა ა/ფ: ჩამოტვირთული ანგარიშ ფაქტურა {0} {1}", inv.Series, inv.Number));
                GetProgramManager().AddLogFormItem(new List<string> { inv.OperationDate.ToString("dd/MM/yyyy HH:mm"), inv.RegistrationDate.ToString("dd/MM/yyyy HH:mm"), string.Format("{0}, {1}", inv.Series, inv.Number), string.Join("; ", fina_actiona.ToArray()), inv.Amount.ToString(), "წარმატებით ჩამოიტვირთა" }, 1);
                affected_invoices.Add(inv.InvoiceId);
            }

            foreach (DataGridViewRow rw in m_Grid.Rows)
            {
                if (affected_invoices.Contains(Convert.ToInt32(rw.Cells[ColID.Index].Value)))
                {
                    rw.Cells[ColCheck.Index].Value = false;
                    rw.Cells[ColCheck.Index].ReadOnly = true;
                    rw.Cells[load_status.Index].Value = FacturaTips.Downloaded;
                    rw.Cells[ColResult.Index].Value = EnumEx.GetEnumDescription(FacturaTips.Downloaded);
                    rw.DefaultCellStyle.BackColor = StatusColors[FacturaTips.Downloaded];
                }
            }
            UpdateFactStatusLabel();
            ProgressDispatcher.Deactivate();
            GetProgramManager().ShowLogForm();
        }

        private int SaveServiceIn(InvoiceItem invoice, List<Good> goods)
        {
           
            long doc_num = GetProgramManager().GetDataManager().GetDocMaxNumber("DOC_SERVICEIN");
            double amount = goods.Select(a => a.Amount).DefaultIfEmpty().Sum();
            double vat = GetProgramManager().GetDataManager().GetVatPercent();
            int project_id = GetProgramManager().GetDataManager().GetStoreProjectID(1);
            double vat_val = 0.0;
            DateTime tdate = invoice.RegistrationDate;
            if (!(tdate.Year == invoice.OperationDate.Year && tdate.Month == invoice.OperationDate.Month))
                tdate = invoice.OperationDate.Date.AddHours(10);

            string contragent_account = GetProgramManager().GetDataManager().GetContragentAccount(invoice.ContargentId);
            int contragentGroupID = GetProgramManager().GetDataManager().GetContragentGroupID(invoice.ContargentId);
            string sql_serviceIn = @"INSERT INTO doc.ProductIn (general_id,doc1,doc2,doc1date,doc2date, is_auto_project, vat_pass, chek_status,location_type ) VALUES (@general_id, @doc1, @doc2, @doc1date, @doc2date, @is_auto_project, @vat_pass, @chek_status,@location_type) SELECT SCOPE_IDENTITY()";
            string sql_serviceInFloow = @"INSERT INTO doc.ProductInExpenses (productIn_id, service_id, price, quantity, contragent_id, general_id)VALUES(@productIn_id, @service_id, @price, @quantity, @contragent_id, @general_id)";
            List<Entries> entr = new List<Entries>();

            int general_id = GetProgramManager().GetDataManager().Insert_GeneralDocs(
              tdate,
              "",
              doc_num,
              (int)DocTypes.ServiceIn,
              string.Format("მომსახურების მიღება ა/ფაქტურისთვის {0} {1}",  invoice.Series, invoice.Number),
              amount,
              1,
              1,
              vat,
              GetProgramManager().GetUserID(),
              0,
              invoice.ContargentId,
              1, 
              1, 
              true,
              project_id, 
              0,
              0);
            if (general_id <= 0)
                return -1;

            int? doc_id = GetProgramManager().GetDataManager().GetScalar<int>(sql_serviceIn, new SqlParameter[]
            {
                new SqlParameter("@general_id", SqlDbType.Int) {Value = general_id },
                new SqlParameter("@doc1", SqlDbType.NVarChar) {Value = "" },
                new SqlParameter("@doc2", SqlDbType.NVarChar) {Value = string.Format("{0}-{1}", invoice.Series, invoice.Number) },
                new SqlParameter("@doc1date", SqlDbType.DateTime) {Value = invoice.OperationDate.Date },
                new SqlParameter("@doc2date", SqlDbType.DateTime) {Value = invoice.OperationDate.Date },
                new SqlParameter("@is_auto_project", SqlDbType.Bit) {Value = false },
                new SqlParameter("@vat_pass", SqlDbType.Bit) {Value = false },
                new SqlParameter("@chek_status", SqlDbType.Int) {Value = 0 },
                new SqlParameter("@location_type", SqlDbType.Bit) {Value = false }
            });
            if (!doc_id.HasValue || doc_id.Value <= 0)
                return -1;

            foreach (Good g in goods)
            {
                int? _fl = GetProgramManager().GetDataManager().ExecuteSql(sql_serviceInFloow, new SqlParameter[]
                {
                    new SqlParameter("@general_id", SqlDbType.Int) {Value = general_id },
                    new SqlParameter("@productIn_id", SqlDbType.Int) {Value = doc_id.Value },
                    new SqlParameter("@service_id", SqlDbType.Int) {Value = g.Id },
                    new SqlParameter("@price", SqlDbType.Decimal) {Value = g.Amount/g.Quantity },
                    new SqlParameter("@quantity", SqlDbType.Decimal) {Value = g.Quantity },
                    new SqlParameter("@contragent_id", SqlDbType.Int) {Value = invoice.ContargentId }
                });

                if (!_fl.HasValue || _fl.Value <= 0)
                    return -1;

                string product_acc = GetProgramManager().GetDataManager().GetProductAccount(g.Id);
                int productGroupID = GetProgramManager().GetDataManager().GetProductGroupID(g.Id);
                double price_amount = g.Amount - g.VatAmount;
                vat_val += g.VatAmount;

                entr.Add(new Entries
                {
                    GeneralId = general_id,
                    DebitAcc = product_acc,
                    CreditAcc = contragent_account,
                    Amount = price_amount,
                    N = g.Quantity,
                    N2 = g.Quantity,
                    A1 = g.Id,
                    A3 = productGroupID,
                    B1 = invoice.ContargentId,
                    B3 = contragentGroupID,
                    Comment = "მომსახურების მიღება",
                    ProjectId = project_id,
                    CurrencyId = 1
                });
            }
            if (vat_val > 0)
            {
                entr.Add(new Entries
                {
                    GeneralId = general_id,
                    DebitAcc = "3340",
                    CreditAcc = contragent_account,
                    Amount = vat_val,
                    B1 = invoice.ContargentId,
                    B3 = contragentGroupID,
                    Comment = "გადახდილი დღგ",
                    ProjectId = project_id,
                    CurrencyId = 1
                });
            }
            int? aff = GetProgramManager().GetDataManager().InsertEntries(entr);
            if (!aff.HasValue || aff.Value <= 0)
                return -1;
            invoice.FinaGeneralData.Add(general_id, new KeyValuePair<DateTime, string>());
            return general_id;
        }

        private bool SaveFactura(InvoiceItem inv)
        {
            string sqlText = @"INSERT INTO doc.ProductIn_Doc (general_id,ref_general_id,series,num, recieve_date, operation_date) VALUES(@general_id, @ref_general_id, @series, @num, @recieve_date, @operation_date)";
            long inv_num = GetProgramManager().GetDataManager().GetDocMaxNumber("DOC_PRODUCTIN_DOC");
            double total_vat = inv.GoodList.Select(a => a.VatAmount).DefaultIfEmpty().Sum();

            int general_id = GetProgramManager().GetDataManager().Insert_GeneralDocs(
            inv.RegistrationDate,
            "",
            inv_num,
            (int)DocTypes.ProductInDoc,
            string.Format("ჩამოტვირთული ანგარიშ ფაქტურა {0} {1}", inv.Series, inv.Number),
            total_vat,
            1,
            1,
            GetProgramManager().GetDataManager().GetVatPercent(),
            GetProgramManager().GetUserID(),
            0,
            inv.ContargentId,
            0,
            1, false, 1, 1, 0);

            if (general_id <= 0)
                return false;

            string reason_string = string.Join(",", inv.FinaGeneralData.Keys.Select(a => a.ToString()).ToArray());
            int? _ins = GetProgramManager().GetDataManager().ExecuteSql(sqlText, new SqlParameter[]
            {
                        new SqlParameter("@general_id", SqlDbType.Int) {Value = general_id },
                        new SqlParameter("@ref_general_id", SqlDbType.VarChar) {Value = reason_string },
                        new SqlParameter("@series", SqlDbType.NVarChar) {Value = inv.Series },
                        new SqlParameter("@num", SqlDbType.VarChar) {Value = inv.Number },
                        new SqlParameter("@recieve_date", SqlDbType.DateTime) {Value = inv.RegistrationDate },
                        new SqlParameter("@operation_date", SqlDbType.Date) {Value=inv.OperationDate } 
            });

            if (!_ins.HasValue || _ins.Value <= 0)
                return false;

            return true;
        }

        private void menuFactura_Opening(object sender, CancelEventArgs e)
        {
            SepContragents.Visible = UnkownVendorItem.Visible = _unkown_contragent_exists;
           
        }

        private void UnkownVendorItem_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0 || m_Grid.SelectedRows.Count == 0)
                return;
            HashSet<string> failds = new HashSet<string>();
            ProgressDispatcher.Activate();
            using (BusinesContext _bc = new BusinesContext())
            {
                foreach (DataGridViewRow rw in m_Grid.Rows)
                {
                    if ((FacturaTips)rw.Cells[load_status.Index].Value != FacturaTips.UnavailableContragent)
                        continue;
                    string name = Convert.ToString(rw.Cells[ColContragent.Index].Value);
                    int id = _bc.SaveContragent(new Contragents
                    {
                        Code = Convert.ToString(rw.Cells[ColCode.Index].Value),
                        GroupId = 3,
                        Path = "0#1#3",
                        Name = name,
                        ShortName = name,
                        Type = 1,
                        VatType = 1,
                        IsResident = 0
                    });
                    if (id <= 0)
                        failds.Add(name);
                }
            }
            ProgressDispatcher.Deactivate();
            if (failds.Any())
                MessageBoxForm.Show(Application.ProductName, "ზოგიერთი მომწოდებელი ვერ დაემატა!", string.Join(", ", failds.ToArray()), null, SystemIcons.Warning);
            onWaybillsLoad();
        }
    }

  
    
   
  


}
