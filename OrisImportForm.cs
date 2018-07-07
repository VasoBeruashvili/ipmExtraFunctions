using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using ipmFunc;
using System.ComponentModel;
using ipmPMBasic.Models;
using System.Globalization;
using ipmControls;
using System.Drawing;
using ipmBLogic;

namespace ipmExtraFunctions
{

    public enum EntryGroup
    {
        [Description("მყიდველი")]
        Customer,
        [Description("მომწოდებელი")]
        Vendor,
        [Description("სალარო")]
        Cash,
        [Description("საბანკო ანგარიში")]
        BankAccount,
        [Description("სასაქონლო მატერიალური მარაგი")]
        Product,
        [Description("არასტანდარტული გატარება")]
        Special
    }

    public enum ImportTip
    {
        [Description("იმპორტირებულია")]
        Imported,
        [Description("იმპორტი შესაძელბელია")]
        Available,
        [Description("უცნობი ტიპია")]
        UnkownType,
        [Description("უცნობი ვალუტა")]
        UnkownCurrency,
        [Description("უცნობი მყიდველი")]
        UnkownCustomer,
        [Description("უცნობი მომწოდებელი")]
        UnkownVendor,
        [Description("უცნობი სალარო")]
        UnkownCash,
        [Description("უცნობი საბანკო ანგარიში")]
        UnkownBankAccount,
        [Description("უცნობი სასაქონლო მატერიალური მარაგი")]
        UnkownProduct,
        [Description("უცნობი საწყობი")]
        UnkownStore,
        [Description("უცნობი დებეტი")]
        UnkownDebet,
        [Description("უცნობი კრედიტი")]
        UnkownCredit
    }
   
    public partial class OrisImportForm : Form
    {
        ProgramManagerBasic pm;
        private Dictionary<ImportTip, Color> StatusColors = new Dictionary<ImportTip, Color>
        {
            { ImportTip.Imported, Color.MediumSeaGreen },
            { ImportTip.Available, Color.Gold },
            { ImportTip.UnkownType, Color.Tomato },
            { ImportTip.UnkownCurrency, Color.Tomato },
            { ImportTip.UnkownCustomer, Color.Tomato },
            { ImportTip.UnkownVendor, Color.Tomato },
            { ImportTip.UnkownCash, Color.Tomato },
            { ImportTip.UnkownBankAccount, Color.Tomato },
            { ImportTip.UnkownProduct, Color.Tomato },
            { ImportTip.UnkownStore, Color.Tomato },
            { ImportTip.UnkownDebet, Color.Tomato },
            { ImportTip.UnkownCredit, Color.Tomato }
        };

        public OrisImportForm(ProgramManagerBasic pm)
        {
            InitializeComponent();
            this.pm = pm;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string pt = null;
            using (OpenFileDialog _dlg = new OpenFileDialog { Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx" })
            {
                if (_dlg.ShowDialog() != DialogResult.OK)
                    return;
                pt = _dlg.FileName;
            }
            ReadFile(pt);
        }

        private void ReadFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;
            m_Grid.Rows.Clear();
            DataSet _set = pm.GetDataManager().GetTablesDataFromExcel(path);
            if (_set == null)
                return;
            DataTable _map = _set.Tables["Map"];
            if (_map == null || _map.Rows.Count == 0)
            {
                MessageBoxForm.Show(Application.ProductName, "ფაილში ვერ მოიძებნა გვერდი - 'MAP' ან არ არის შევსებული", null, null, SystemIcons.Error);
                return;
            }
              
            DataTable _data = _set.Tables["Data"];
            if (_data == null || _data.Rows.Count == 0)
            {
                MessageBoxForm.Show(Application.ProductName, "ფაილში ვერ მოიძებნა გვერდი - 'Data' ან არ არის შევსებული", null, null, SystemIcons.Error);
                return;
            }
            IEnumerable<DataRow> _map_rows = _map.AsEnumerable();
            List<EntryGroup> _grp = _map_rows.Select(a => a.Field<string>("ტიპი")).Where(r => Enum.IsDefined(typeof(EntryGroup), r)).Select(res => res.ToEnum<EntryGroup>()).Distinct().ToList();
            if (!_grp.Any())
            {
                MessageBoxForm.Show(Application.ProductName, "შესაბამისი ტიპები ვერ მოიძებნა", null, null, SystemIcons.Error);
                return;
            }

            ProgressDispatcher.Activate();

            Dictionary<string, Contragents> _fina_customer = null;
            Dictionary<string, Contragents> _fina_vendor = null;
            Dictionary<string, Cashes> _fina_cash = null;
            Dictionary<string, CompanyAccounts> _fina_account = null;
            Dictionary<string, Products> _fina_product = null;
            Dictionary<string, Curencies> _fina_currency = null;
            Dictionary<string, int> _fina_store = null;

            using (DBContext _db = new DBContext())
            {
                _grp.ForEach(g =>
                {
                    switch (g)
                    {
                        case EntryGroup.Customer:
                            {
                                List<Contragents> _c = _db.GetList<Contragents>("SELECT id AS Id, code AS Code, name AS Name, group_id AS GroupId, path AS Path, account AS Account, account2 AS Account2 FROM book.Contragents WHERE (path='0#2#5' OR path LIKE '0#2#5#%') AND code IS NOT NULL AND code!=''");
                                _fina_customer = _c.GroupBy(a => a.Code).Select(a => new KeyValuePair<string, Contragents>(a.Key, a.First())).ToDictionary(k => k.Key, v => v.Value);
                                break;
                            }
                        case EntryGroup.Vendor:
                            {
                                List<Contragents> _c = _db.GetList<Contragents>("SELECT id AS Id, code AS Code, name AS Name, group_id AS GroupId, path AS Path, account AS Account, account2 AS Account2 FROM book.Contragents WHERE (path='0#1#3' OR path LIKE '0#1#3#%') AND code IS NOT NULL AND code!=''");
                                _fina_vendor = _c.GroupBy(a => a.Code).Select(a => new KeyValuePair<string, Contragents>(a.Key, a.First())).ToDictionary(k => k.Key, v => v.Value);

                                break;
                            }
                        case EntryGroup.Cash:
                            {
                                List<Cashes> _c = _db.GetList<Cashes>("SELECT id AS Id, name AS Name, group_id AS GroupId, path AS Path, account AS Account FROM book.Cashes WHERE name IS NOT NULL AND name!=''");
                                _fina_cash = _c.GroupBy(a => a.Name).Select(a => new KeyValuePair<string, Cashes>(a.Key, a.First())).ToDictionary(k => k.Key, v => v.Value);
                                break;
                            }
                        case EntryGroup.BankAccount:
                            {
                                List<CompanyAccounts> _c = _db.GetList<CompanyAccounts>("SELECT  b.id AS Id, b.bank_id AS BankId, b.account AS Account, b.account + c.code AS Name, b.currency_id AS CurrencyId FROM book.CompanyAccounts AS b INNER JOIN book.Currencies AS c ON b.currency_id=c.id WHERE b.account IS NOT NULL AND b.account!=''");
                                _fina_account = _c.GroupBy(a => a.Name).Select(a => new KeyValuePair<string, CompanyAccounts>(a.Key, a.First())).ToDictionary(k => k.Key, v => v.Value);
                                break;
                            }
                        case EntryGroup.Product:
                            {
                                List<Products> _c = _db.GetList<Products>("SELECT id AS Id, name AS Name, group_id AS GroupId, path AS Path FROM book.Products WHERE (path='0#1#10' OR path LIKE '0#1#10#%' OR path='0#2#120' OR path LIKE '0#2#120#%' OR path='0#2#110' OR path LIKE '0#2#110#%') AND name IS NOT NULL AND name!=''");
                                _fina_product = _c.GroupBy(a => a.Name).Select(a => new KeyValuePair<string, Products>(a.Key, a.First())).ToDictionary(k => k.Key, v => v.Value);
                                break;
                            }
                    }
                });

                List<Curencies> _curen= _db.GetList<Curencies>("SELECT code AS Code, id AS Id FROM book.Currencies");
                _fina_currency = _curen.GroupBy(a => a.Code).Select(a => new KeyValuePair<string, Curencies>(a.Key, a.First())).ToDictionary(k => k.Key, v => v.Value);
                _fina_store = _db.GetDictionary<string, int>("SELECT name AS Name, id AS Id FROM book.Stores");
            }
          
            Func<string, string, string, FinaEqual> GetFinaEqueal = (string tp, string f_code, string f_store) => 
            {
                FinaEqual f = new FinaEqual { FinaCode = f_code, ImportInfo = new KeyValuePair<ImportTip, string>(ImportTip.Available, null) };
                if (!Enum.IsDefined(typeof(EntryGroup), tp))
                {
                    f.ImportInfo = new KeyValuePair<ImportTip, string>(ImportTip.UnkownType, tp);
                    return f;
                }

                EntryGroup eg = tp.ToEnum<EntryGroup>();
                f.FinaGroup = eg;
                switch (eg)
                {
                    case EntryGroup.Customer:
                        {
                            if (!_fina_customer.ContainsKey(f_code))
                            {
                                f.ImportInfo = new KeyValuePair<ImportTip, string>(ImportTip.UnkownCustomer, f_code);
                                return f;
                            }
                            Contragents _c = _fina_customer[f_code];
                            f.Id = _c.Id;
                            f.Account = _c.Account;
                            break;
                        }
                    case EntryGroup.Vendor:
                        {
                            if (!_fina_vendor.ContainsKey(f_code))
                            {
                                f.ImportInfo = new KeyValuePair<ImportTip, string>(ImportTip.UnkownVendor, f_code);
                                return f;
                            }
                            Contragents _c = _fina_vendor[f_code];
                            f.Id = _c.Id;
                            f.Account = _c.Account;
                            break;
                        }
                    case EntryGroup.Cash:
                        {
                            if (!_fina_cash.ContainsKey(f_code))
                            {
                                f.ImportInfo = new KeyValuePair<ImportTip, string>(ImportTip.UnkownCash, f_code);
                                return f;
                            }
                            Cashes _c = _fina_cash[f_code];
                            f.Id = _c.Id;
                            string[] acc = _c.Account.Split('.');
                            f.Account = acc[0];
                            break;
                        }
                    case EntryGroup.BankAccount:
                        {
                            if (!_fina_account.ContainsKey(f_code))
                            {
                                f.ImportInfo = new KeyValuePair<ImportTip, string>(ImportTip.UnkownBankAccount, f_code);
                                return f;
                            }
                            CompanyAccounts _c = _fina_account[f_code];
                            f.Id = _c.Id;
                            f.Account = _c.Account;
                            break;
                        }
                    case EntryGroup.Product:
                        {
                            if (!_fina_product.ContainsKey(f_code))
                            {
                                f.ImportInfo = new KeyValuePair<ImportTip, string>(ImportTip.UnkownProduct, f_code);
                                return f;
                            }
                            if (!_fina_store.ContainsKey(f_store))
                            {
                                f.ImportInfo = new KeyValuePair<ImportTip, string>(ImportTip.UnkownStore, f_store);
                                return f;
                            }
                            Products _c = _fina_product[f_code];
                            f.Id = _c.Id;
                            f.Account = pm.GetDataManager().GetProductAccountByPath(_c.Path, 0);
                            f.StoreId = _fina_store[f_store];
                            break;
                        }

                    case EntryGroup.Special:
                        {
                            string[] acc = f_code.Split('.');
                            f.Account = acc[0];
                            if (acc.Length > 1)
                                f.Id = acc[1].TryParseScalar<int>();
                            break;
                        }

                    default:
                        {
                            f.ImportInfo = new KeyValuePair<ImportTip, string>(ImportTip.UnkownType, tp);
                            return f;
                        }
                }
                return f;
            };

            Func<EntryItem, KeyValuePair<ImportTip, string>> GetStatus = (EntryItem _en) =>
            {
                ImportTip it = ImportTip.Available;
                string msg = "";

                if (_en.Currency == null)
                {
                    it = ImportTip.UnkownCurrency;
                    msg = _en.CurrencyCode;
                }

                if (_en.FinaItemDebit == null || _en.FinaItemDebit.ImportInfo.Key != ImportTip.Available)
                {
                    if (_en.FinaItemDebit == null)
                    {
                        it = ImportTip.UnkownDebet;
                        msg = _en.DebitAcc;
                    }
                    else
                    {
                        it = _en.FinaItemDebit.ImportInfo.Key;
                        msg = _en.FinaItemDebit.ImportInfo.Value;
                    }
                }
                if (_en.FinaItemCredit == null || _en.FinaItemCredit.ImportInfo.Key != ImportTip.Available)
                {
                    if (_en.FinaItemCredit == null)
                    {
                        it = ImportTip.UnkownCredit;
                        msg = _en.CreditAcc;
                    }
                    else
                    {
                        it = _en.FinaItemCredit.ImportInfo.Key;
                        msg = _en.FinaItemCredit.ImportInfo.Value;
                    }
                }
                return new KeyValuePair<ImportTip, string>(it, string.Format("{0} {1}", EnumEx.GetEnumDescription(it), msg));
            };

            Dictionary<string, FinaEqual> fina_map = _map_rows.Select(a => new
            {
                _grp =  a.Field<string>("ტიპი"),
                _cod = a.Field<string>("შესაბამისობა FINA ში"),
                _str = a.Field<string>("საწყობი FINA ში"),
                _acc = a.Field<string>("ანგარიში")
            })
            .Select(a => new KeyValuePair<string, FinaEqual>(a._acc, GetFinaEqueal(a._grp, a._cod, a._str)))
            .ToDictionary(k => k.Key, v => v.Value);

            foreach (DataRow row in _data.Rows)
            {
                EntryItem _en = new EntryItem
                {
                    Tdate = Convert.ToDateTime(row["თარიღი"]),
                    Purpose = row.Field<string>("შინაარსი"),
                    DebitAcc = row.Field<string>("დებეტი"),
                    CreditAcc = row.Field<string>("კრედიტი"),
                    FinaItemDebit = !fina_map.ContainsKey(row.Field<string>("დებეტი")) ? null : fina_map[row.Field<string>("დებეტი")],
                    FinaItemCredit = !fina_map.ContainsKey(row.Field<string>("კრედიტი")) ? null : fina_map[row.Field<string>("კრედიტი")],
                    Quantity = Convert.ToDouble(row["რაოდ."], CultureInfo.InvariantCulture),
                    Amount = Convert.ToDouble(row["თანხა"], CultureInfo.InvariantCulture),
                    CurrencyCode = row.Field<string>("ვალუტა"),
                    Currency = !_fina_currency.ContainsKey(row.Field<string>("ვალუტა")) ? null : _fina_currency[row.Field<string>("ვალუტა")],
                    CurrencyRate = Convert.ToDecimal(row["კურსი"], CultureInfo.InvariantCulture)
                };
                _en.Status = GetStatus(_en);

                string _fina_deb = null;
                string _fina_cred = null;
                if (_en.FinaItemDebit != null)
                    _fina_deb = string.Format("{0}.{1}", _en.FinaItemDebit.Account, _en.FinaItemDebit.Id);
                if (_en.FinaItemCredit != null)
                    _fina_cred = string.Format("{0}.{1}", _en.FinaItemCredit.Account, _en.FinaItemCredit.Id);

                int index = m_Grid.Rows.Add(false, _en.Tdate, _en.Purpose, _en.DebitAcc, _en.CreditAcc, _en.Amount, _en.CurrencyCode, _en.CurrencyRate, _fina_deb, _fina_cred, _en.Status.Value);
                m_Grid.Rows[index].Cells[ColInside.Index].Value = _en;
                m_Grid.Rows[index].Cells[ColCheck.Index].ReadOnly = _en.Status.Key != ImportTip.Available;
                m_Grid.Rows[index].DefaultCellStyle.BackColor = StatusColors[_en.Status.Key];
            }
            m_Grid.EndEdit();
            ProgressDispatcher.Deactivate();
        }

        private void OnExecuteImport()
        {
            m_Grid.EndEdit();
            if (m_Grid.Rows.Count == 0)
                return;
            var ready_rows = m_Grid.Rows.OfType<DataGridViewRow>().Where(r => Convert.ToBoolean(r.Cells[ColCheck.Index].Value)).ToList();
            if (!ready_rows.Any())
                return;
            pm.InitialiseLog(new List<string> { "თარიღი", "საფუძველი", "დებეტი", "კრედიტი", "თანხა", "დებეტი (FINA)", "კრედიტი (FINA)", "შედეგი" });
            using (BusinesContext _bc = new BusinesContext())
            {
                Dictionary<KeyValuePair<int, string>, long> _nm = _bc.GenerateDocNums(new List<int> { (int)DocTypes.SingleEntry }, null);
                long doc_num = !_nm.Any() ? 1 : _nm.First().Value;
                double vat = Special.ConfigParams["vat"].TryParseScalar<double>();
                int user_id = pm.GetUserID();
                int staff = pm.GetDataManager().GetUserStaff(user_id);
                ProgressDispatcher.Activate(true);
                int i = 1;
                foreach (DataGridViewRow rw in ready_rows)
                {
                    decimal per = (decimal)i / (decimal)ready_rows.Count * 100M;
                    ProgressDispatcher.Percent = ((int)per);
                    i++;

                    EntryItem _en = (EntryItem)rw.Cells[ColInside.Index].Value;

                    //Hard
                    string debit_acc = _en.FinaItemDebit.Account;
                    string credit_acc = _en.FinaItemCredit.Account;
                    List<ProductsFlow> _flow = new List<ProductsFlow>();
                    SingleEntry s = new SingleEntry
                    {
                        StaffId = staff,
                        A1 = _en.FinaItemDebit.Id,
                        B1 = _en.FinaItemCredit.Id,
                        Amount = _en.Amount,
                    };
                    if (debit_acc == "6110")
                    {
                        if (s.A1 <= 4)
                        {
                            s.A2 = 1;
                            s.A3 = s.A1;
                        }
                        else
                            s.A3 = s.A1 - 4;
                        s.A1 = 0;
                    }
                    if (credit_acc == "6110")
                    {
                        if (s.B1 <= 4)
                        {
                            s.B2 = 1;
                            s.B3 = s.B1;
                        }
                        else
                            s.B3 = s.B1 - 4;
                        s.B1 = 0;
                    }

                    if (debit_acc == "3320")
                        debit_acc = string.Concat(debit_acc, ".", s.A1);

                    if (credit_acc == "3320")
                        credit_acc = string.Concat(credit_acc, ".", s.B1);

                    if (_en.FinaItemDebit.FinaGroup == EntryGroup.Product)
                    {
                        s.N = _en.Quantity;
                        s.A2 = _en.FinaItemDebit.StoreId;

                        int is_expense = 0;
                        double debetQuantity = _en.Quantity;
                        double am = _en.Amount;
                        if (debetQuantity == 0)
                        {
                            debetQuantity = 1;
                            is_expense = 1;
                        }
                        am = am / debetQuantity;
                        _flow.Add(new ProductsFlow
                        {
                            ProductId = s.A1,
                            Amount = debetQuantity,
                            UnitId = pm.GetDataManager().GetProductUnitID(s.A1),
                            Price = am,
                            StoreId = _en.FinaItemDebit.StoreId,
                            Coeff = 1,
                            IsExpense = (byte)is_expense,
                            SelfCost = am,
                        });
                    }

                    if (_en.FinaItemCredit.FinaGroup == EntryGroup.Product)
                    {
                        s.N2 = _en.Quantity;
                        s.B2 = _en.FinaItemCredit.StoreId;

                        int is_expense = 0;
                        double creditQuantity = _en.Quantity;
                        double am = _en.Amount;
                        if (creditQuantity == 0)
                        {
                            creditQuantity = 1;
                            is_expense = 1;
                        }
                        am = am / creditQuantity;
                        _flow.Add(new ProductsFlow
                        {
                            ProductId = s.B1,
                            Amount = creditQuantity,
                            UnitId = pm.GetDataManager().GetProductUnitID(s.B1),
                            Price = am,
                            StoreId = _en.FinaItemCredit.StoreId,
                            Coeff = -1,
                            IsExpense = (byte)is_expense,
                            SelfCost = am,
                        });
                    }
                    s.DebitAcc = debit_acc;
                    s.CreditAcc = credit_acc;

                    int insert_res = _bc.SaveSingleEntry(new DocItem
                    {
                        GeneralDocsItem = new GeneralDocs
                        {
                            Tdate = _en.Tdate,
                            DocNum = doc_num++,
                            DocType = (int)DocTypes.SingleEntry,
                            Purpose = string.Format("{0} (იმპორტირებული)", _en.Purpose),
                            Amount = _en.Amount,
                            CurrencyId = _en.Currency.Id,
                            Rate = (double)_en.CurrencyRate,
                            Vat = vat,
                            UserId = user_id,
                            StatusId = 1,
                            MakeEntry = true,
                            ProjectId = 1,
                            HouseId = 1,
                            StaffId = staff,
                            Uid = Guid.NewGuid().ToString()
                        },
                        SingleEntryItem = s,
                        ProductsFlowList = _flow
                    });

                    if (insert_res <= 0)
                    {
                        pm.AddLogFormItem(new List<string> { _en.Tdate.ToString("dd/MM/yyyy HH:mm"), _en.Purpose, _en.DebitAcc, _en.CreditAcc, _en.Amount.ToString(), debit_acc, credit_acc, string.Format("იმპორტი ვერ შესრულდა: {0}", _bc.ErrorEx) }, 0);
                    }
                    else
                    {
                        pm.AddLogFormItem(new List<string> { _en.Tdate.ToString("dd/MM/yyyy HH:mm"), _en.Purpose, _en.DebitAcc, _en.CreditAcc, _en.Amount.ToString(), debit_acc, credit_acc, "ok" }, 1);
                        _en.Status = new KeyValuePair<ImportTip, string>(ImportTip.Imported, EnumEx.GetEnumDescription(ImportTip.Imported));
                        rw.Cells[ColCheck.Index].ReadOnly = true;
                        rw.DefaultCellStyle.BackColor = StatusColors[_en.Status.Key];
                        rw.Cells[ColInside.Index].Value = _en;
                        rw.Cells[ColStatus.Index].Value = _en.Status.Value;
                    }
                    rw.Cells[ColCheck.Index].Value = false;
                }

            }
            ProgressDispatcher.Deactivate();
            pm.ShowLogForm();
        }

        private void selectIem_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0 )
                return;
            foreach (DataGridViewRow rw in m_Grid.Rows)
            {
                if (((EntryItem)rw.Cells[ColInside.Index].Value).Status.Key != ImportTip.Available)
                    continue;
                rw.Cells[ColCheck.Index].Value = true;
            }
            m_Grid.EndEdit();
        }

        private void unselectItem_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0)
                return;
            foreach (DataGridViewRow rw in m_Grid.Rows)
                rw.Cells[ColCheck.Index].Value = false;
            m_Grid.EndEdit();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            OnExecuteImport();
        }
    }





    public class FinaEqual
    {
        public EntryGroup FinaGroup { get; set; }
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string FinaCode { get; set; }
        public string Account { get; set; }
        public KeyValuePair<ImportTip, string> ImportInfo { get; set; }
    }

    public class EntryItem
    {
        public DateTime Tdate { get; set; }
        public string Purpose { get; set; }
        public string DebitAcc { get; set; }
        public string CreditAcc { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public Curencies Currency { get; set; }
        public decimal CurrencyRate { get; set; }
        public FinaEqual FinaItemDebit { get; set; }
        public FinaEqual FinaItemCredit { get; set; }
        public KeyValuePair<ImportTip, string> Status { get; set; }

      
    }




}
