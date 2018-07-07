using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using ipmPMBasic;
using ipmBLogic;
using ipmPMBasic.Models;
using ipmControls;
using System.Globalization;
using ipmFunc;
using System.Collections;
using System.Drawing;

namespace ipmExtraFunctions
{
    public class Imports
    {
        public ProgramManagerBasic ProgramManager { get; set; }

        public Imports() : this(null)
        { }
        public Imports(ProgramManagerBasic pm)
        {
            this.ProgramManager = pm;
        }


        public bool OnImportOut()
        {
            OpenFileDialog oDialog = new OpenFileDialog() { Title = "რეალიზაციის ოპერაციების იმპორტი", Filter = "Excel Files|*.xls;*.xlsx" };
            if (oDialog.ShowDialog() != DialogResult.OK)
                return false;
            IEnumerable<DataRow> eData = ProgramManager.GetDataManager().GetEnumerableDataFromExcel(oDialog.FileName);
            if (eData == null || eData.Count() == 0)
                return false;
            var groupeds = eData.GroupBy(g => Convert.ToInt32(g["[ZEDNADEBIS KODI]"])).Select(g => new { items = g.ToList() }).ToList();
            if (groupeds == null || !groupeds.Any())
                return false;
            Dictionary<string, int> _customers;
            Dictionary<string, int> _stores;
            Dictionary<string, int> _units;
            Dictionary<string, Products> _products;

            ProgressDispatcher.Activate(true);

            using (DBContext _db = new DBContext())
            {
                _customers = _db.GetDictionary<string, int>("SELECT code, id FROM book.Contragents WHERE (path='0#2#5' OR path LIKE '0#2#5#%') AND NULLIF(code,'') IS NOT NULL AND id>0 ");
                _stores = _db.GetDictionary<string, int>("SELECT address, id FROM book.Stores WHERE  NULLIF(address,'') IS NOT NULL");
                _units = _db.GetDictionary<string, int>("SELECT full_name, id FROM book.Units WHERE  NULLIF(full_name,'') IS NOT NULL");
                List<Products> _prd = _db.GetList<Products>("SELECT id AS Id, code AS Code, (CASE WHEN ISNULL(vat,0)= 1 THEN 1 ELSE CASE WHEN ISNULL(vat_type,0)=0 THEN 2 ELSE 3  END END) AS VatType FROM book.Products WHERE (path='0#1#10' OR path LIKE '0#1#10#%') AND NULLIF(code,'') IS NOT NULL");
                _products = _prd.GroupBy(a => a.Code).Select(a => new KeyValuePair<string, Products>(a.Key, a.First())).ToDictionary(k => k.Key, v => v.Value);
            }

            ProgramManager.InitialiseLog(new List<string> {"თარიღი", "ზედნადების ნომერი", "მყიდველი", "თანხა", "შედეგი" });
            List<DocItem> _docs = new List<DocItem>();
            foreach (var root in groupeds)
            {
                string str_address = root.items.First().Field<string>("[TRANSPORTIRABIS_DAWYEBIS_MISAM]");
                int my_store_id = !_stores.ContainsKey(str_address) ? 1 : _stores[str_address];
              
                DateTime operation_date = Convert.ToDateTime(root.items.First()["[ZEDNADEBIS TARIGI]"]).AddHours(11);
                string waybill_num = root.items.First().Field<string>("[ZEDNADEBIS KODI]");
                string customer_code = root.items.First().Field<string>("[MOMXMAREBLIS KODI]");
                string customer_name = root.items.First().Field<string>("[MOMXAREBLIS SAXELI]");
                double amount = root.items.Select(a => Convert.ToDouble(a["[SUL TANXA]"], CultureInfo.InvariantCulture)).DefaultIfEmpty().Sum();
                
                if (!_customers.ContainsKey(customer_code))
                {
                    ProgramManager.AddLogFormItem(new List<string> { operation_date.ToString("yyyy/MM/dd"), waybill_num, string.Format("{0} {1}", customer_code, customer_name), amount.ToString(), "მყიდველი ვერ მოიძებნა" }, 0);
                    continue;
                }
                int my_customer_id = _customers[customer_code];
                List<ProductsFlow> flow = new List<ProductsFlow>();
                bool _is_all_match = true;
                foreach (var flow_item in root.items)
                {
                    string product_code = flow_item.Field<string>("[PRODUQTIS KODI]");
                    string product_name = flow_item.Field<string>("[PRODUQTI]");
                    string unit_name = flow_item.Field<string>("[ERTUELI]");
                    double product_price = Convert.ToDouble(flow_item["[ERTEULIS-PASI]"], CultureInfo.InvariantCulture);
                    double product_quant = Convert.ToDouble(flow_item["[RAODENOBA]"], CultureInfo.InvariantCulture);

                    bool unit_exists = _units.ContainsKey(unit_name);
                    if (!_products.ContainsKey(product_code) || !unit_exists)
                    {
                        _is_all_match = false;
                        string error_msg = string.Format("საქონელი - {0} {1} ვერ მოიძებნა", product_code, product_name);
                        if (!unit_exists)
                            error_msg = string.Format("საქონელი - {0} {1}, შესაბამისი ერთეული -{2} ვერ მოიძებნა", product_code, product_name, unit_name);
                        ProgramManager.AddLogFormItem(new List<string> { operation_date.ToString("yyyy/MM/dd"), waybill_num, string.Format("{0} {1}", customer_code, customer_name), amount.ToString(), error_msg }, 0);
                        break;
                    }
                    Products _p = _products[product_code];
                    flow.Add(new ProductsFlow
                    {
                        ProductId = _p.Id,
                        Amount = product_quant,
                        Price = product_price,
                        UnitId = _units[unit_name],
                        StoreId = my_store_id,
                        VatPercent = _p.VatType == 1 ? 18 : 0,
                        Coeff = -1
                    });
                }
                if (!_is_all_match)
                    continue;
                _docs.Add(new DocItem
                {
                    GeneralDocsItem = new GeneralDocs
                    {
                        Tdate = operation_date,
                        DocType = 21,
                        Purpose = string.Format("საქონლის რეალიზაცია <{0} {1}> ", customer_code, customer_name),
                        Amount = amount,
                        Vat = 18,
                        UserId = ProgramManager.GetUserID(),
                        ParamId1 = my_customer_id,
                        ParamId2 = my_store_id,
                        MakeEntry = true,
                        StoreId = my_store_id,
                        WaybillNum = waybill_num,
                        Uid = Guid.NewGuid().ToString()
                    },
                    ProductOutItem = new ProductOut
                    {
                        PayType = "0",
                        PayDate = operation_date,
                        DeliveryDate = operation_date,
                        WaybillType = 2,
                        TransportTypeId = 1,
                        TransportCostPayer = 2,
                        TransportBeginDate = operation_date.Date,
                        ActivateDate = operation_date.AddSeconds(10),
                        ResponsablePersonDate = operation_date,
                        IsForeign = 0,
                        DriverName = root.items.First().Field<string>("[MDZGOLIS SAHELI GVARI]"),
                        TransportStartPlace = str_address,
                        TransportEndPlace = root.items.First().Field<string>("[MISAMARTI-1]"),
                        TransporterIdNum = root.items.First().Field<string>("[MDZGOLIS PIRADI NOMERI]"),
                        TransportNumber = root.items.First().Field<string>("[MANKANIS NOMERI]"),
                        WaybillStatus = -1,
                        WaybillNum = waybill_num,
                        IsWaybill = 1
                        
                    },
                    ProductsFlowList = flow
                });
            }


            using (BusinesContext bc = new BusinesContext())
            {
                List<int> _for_calc = new List<int>();
                _docs.ForEach(doc =>
                {
                    decimal per = (decimal)(_docs.IndexOf(doc)+1) / (decimal)_docs.Count * 100M;
                    ProgressDispatcher.Percent = ((int)per);

                    if (bc.SaveProductOut(doc) <= 0)
                        ProgramManager.AddLogFormItem(new List<string> { doc.GeneralDocsItem.Tdate.Value.ToString("yyyy/MM/dd"), doc.GeneralDocsItem.WaybillNum, doc.GeneralDocsItem.Purpose, doc.GeneralDocsItem.Amount.ToString(), bc.ErrorEx }, 0);
                    else
                    {
                        ProgramManager.AddLogFormItem(new List<string> { doc.GeneralDocsItem.Tdate.Value.ToString("yyyy/MM/dd"), doc.GeneralDocsItem.WaybillNum, doc.GeneralDocsItem.Purpose, doc.GeneralDocsItem.Amount.ToString(), "Ok" }, 1);
                        _for_calc.AddRange(doc.ProductsFlowList.Select(a => a.ProductId));
                    }
                });
                if (_for_calc.Any())
                    bc.RecalculateRestAmounts(_for_calc.Distinct().ToList(), _docs.Select(s => s.GeneralDocsItem.ParamId2.Value).Distinct().ToList());
            }
            ProgressDispatcher.Deactivate();
            ProgramManager.ShowLogForm();

            return true;
        }

        public bool OnFregoImport()
        {
            OpenFileDialog oDialog = new OpenFileDialog() { Title = "რეალიზაციის ოპერაციების იმპორტი FREGO", Filter = "Excel Files|*.xls;*.xlsx" };
            if (oDialog.ShowDialog() != DialogResult.OK)
                return false;
            DataTable eData = ProgramManager.GetDataManager().GetTableDataFromExcel(oDialog.FileName);
            if (eData == null || eData.Rows.Count == 0)
                return false;

            List<DocItem> docs = new List<DocItem>();

            Dictionary<string, int> product_db = ProgramManager.GetDataManager().GetDictionary<string, int>("SELECT code, id FROM book.Products");
            Dictionary<string, string> product_map = new Dictionary<string, string>
            {
                {"PREMIUM","0290" },
                {"E. REGULAR", "0270" },
                {"REGULAR","0270 REG" },
                {"DIESEL","0690" },
                {"DIESEL FS","0690 D.FS" },
                {"DIESEL  ST.", "0690 D.S" }
            };

            Dictionary<string, int>terminal_db = ProgramManager.GetDataManager().GetDictionary<string, int>("SELECT code, id FROM book.Contragents WHERE  path = '0#12#13' OR path LIKE '0#12#13#%'");
            Dictionary<string, string> terminal_map = new Dictionary<string, string>
            {
                {"TBC - პლასტიკური ბარათი","1297/05" },
                {"BOG - პლასტიკური ბარათი", "1297/04" },
                {"LB - პლასტიკური ბარათი","1297/03" }
            };

            string brazers_date_string = Convert.ToString(eData.Rows[2][0]).Trim();
            string[] brazers_dates = brazers_date_string.Split(new char[] { ',', '.' });
            if (brazers_dates == null || brazers_dates.Length < 3)
            {
                MessageBoxForm.Show(Application.ProductName, "არასწორი თარიღის ფორმატი", null, null, SystemIcons.Error);
                return false;
            }
            DateTime operation_date = new DateTime(DateTime.Now.Year, int.Parse(brazers_dates[1]), int.Parse(brazers_dates[0]), 18, 0, 0);
            string col_ags_name = ProgramManager.GetDataManager().GetColumnDBName("TABLE_STORE", "იმპორტი");
            if (string.IsNullOrEmpty(col_ags_name))
            {
                MessageBoxForm.Show(Application.ProductName, "ველი - იმპორტი არ არის დამატებული საწყობებში", null, null, SystemIcons.Error);
                return false;
            }

            string col_card_name = ProgramManager.GetDataManager().GetColumnDBName("TABLE_CONTRAGENT", "იმპორტი");
            if (string.IsNullOrEmpty(col_ags_name))
            {
                MessageBoxForm.Show(Application.ProductName, "ველი - იმპორტი არ არის დამატებული მყიდველებში", null, null, SystemIcons.Error);
                return false;
            }

            int store_id = ProgramManager.GetDataManager().GetIntegerValue(string.Format("SELECT id FROM book.Stores WHERE {0} = @store_name", col_ags_name), new Hashtable { { "@store_name", eData.Columns[4].ColumnName.Trim() } });
            if (store_id <= 0)
            {
                MessageBoxForm.Show(Application.ProductName, "საწყობი ვერ მოიძებნა", null, null, SystemIcons.Error);
                return false;
            }

            Func<int, KeyValuePair<double,  List<ProductsFlow>>> analyse_outs = (int row_index) => 
            {
                List<ProductsFlow> pf = new List<ProductsFlow>();
                double total_sum = 0.00;
                for (int i = 1; i <= 6; i++)
                {
                    double _amount = eData.Rows[row_index+2][i].TryParseScalar<double>();
                    double _quant = eData.Rows[row_index+1][i].TryParseScalar<double>();
                    if (_amount > 0 && _quant > 0)
                    {
                        pf.Add(new ProductsFlow
                        {
                            ProductId = product_db[product_map[(eData.Rows[2][i]).ToString().Trim()]],
                            Amount = _quant,
                            Price = _amount / _quant,
                            UnitId = 4,
                            StoreId = store_id,
                            VatPercent = 18,
                            Coeff = -1
                        });
                        total_sum += _amount;
                    }
                }
                return new KeyValuePair<double, List<ProductsFlow>>(total_sum, pf);
            };

            Func<int, KeyValuePair<double, List<ProductsFlow>>> analyse_card_outs = (int row_index) =>
            {
                List<ProductsFlow> pf = new List<ProductsFlow>();
                double total_sum = 0.00;
                for (int i = 1; i <= 6; i++)
                {
                    double _quant = eData.Rows[row_index][i].TryParseScalar<double>();
                    double _amount = eData.Rows[3][i].TryParseScalar<double>();
                    if (_quant > 0)
                    {
                        pf.Add(new ProductsFlow
                        {
                            ProductId = product_db[product_map[(eData.Rows[2][i]).ToString().Trim()]],
                            Amount = _quant,
                            Price = _amount,
                            UnitId = 4,
                            StoreId = store_id,
                            VatPercent = 18,
                            Coeff = -1
                        });
                        total_sum += _amount * _quant;
                    }
                }
                return new KeyValuePair<double, List<ProductsFlow>>(total_sum, pf);
            };

            for (int i = 32; i <= 41; i += 3)
            {
                var realizes = analyse_outs(i);
                if (realizes.Key > 0 && realizes.Value.Any())
                {
                    int bank_cash_id = 0;
                    if (i > 32) //is terminal
                        bank_cash_id = -100000 - terminal_db[terminal_map[eData.Rows[i][0].ToString().Trim()]];
                    else
                    {
                        bank_cash_id = ProgramManager.GetDataManager().GetCashByStore(store_id);
                        if (bank_cash_id <= 0)
                            bank_cash_id = 1;
                    }

                  

                    docs.Add(new DocItem
                    {
                        GeneralDocsItem = new GeneralDocs
                        {
                            Tdate = operation_date,
                            DocType = 23,
                            Purpose = string.Format("გაყიდვა აგს <{0} {1} {2}> ", eData.Columns[4].ColumnName, brazers_date_string, eData.Rows[i][0]),
                            Amount = realizes.Key,
                            Vat = 18,
                            UserId = ProgramManager.GetUserID(),
                            ParamId1 = bank_cash_id,
                            ParamId2 = store_id,
                            MakeEntry = true,
                            StoreId = store_id,
                            Uid = Guid.NewGuid().ToString()
                        },
                        ProductOutSingleItem = new ProductOutSingle { PriceType = 3 },
                        ProductsFlowList = realizes.Value
                    });
                }
            }

            for (int i = 45; i <= 49; i++)
            {
                int card_id = ProgramManager.GetDataManager().GetIntegerValue(string.Format("SELECT id FROM book.Contragents WHERE {0} = @name", col_card_name), new Hashtable { { "@name", eData.Rows[i][0].ToString().Trim() } });
                if (card_id <= 0)
                {
                    MessageBoxForm.Show(Application.ProductName, "ტალონი ვერ მოიძებნა", null, null, SystemIcons.Error);
                    return false;
                }

                var card_realise = analyse_card_outs(i);
                if (card_realise.Key > 0 && card_realise.Value.Any())
                {
                    docs.Add(new DocItem
                    {
                        GeneralDocsItem = new GeneralDocs
                        {
                            Tdate = operation_date,
                            DocType = 21,
                            Purpose = string.Format("გაყიდვა ტალონი აგს <{0} {1} {2}> ", eData.Columns[4].ColumnName, brazers_date_string, eData.Rows[i][0]),
                            Amount = card_realise.Key,
                            Vat = 18,
                            UserId = ProgramManager.GetUserID(),
                            ParamId1 = card_id,
                            ParamId2 = store_id,
                            MakeEntry = true,
                            StoreId = store_id,
                            Uid = Guid.NewGuid().ToString()
                        },
                        ProductOutItem = new ProductOut
                        {
                            PriceType = 3,
                            PayType = "2",
                            PayDate = operation_date,
                            DeliveryDate = operation_date,
                            WaybillType = 2,
                            TransportTypeId = 1,
                            TransportCostPayer = 2,
                            TransportBeginDate = operation_date.Date,
                            ActivateDate = operation_date.AddSeconds(10),
                            ResponsablePersonDate = operation_date,
                            IsForeign = 0,
                            WaybillStatus = -1,
                            IsWaybill = 1
                        },
                        ProductsFlowList = card_realise.Value
                    });
                }
            }


         

            if (!docs.Any())
                return true;
            using (BusinesContext _logic = new BusinesContext())
            {
                Dictionary<KeyValuePair<int, string>, long> _nums = _logic.GenerateDocNums(new List<int> { 21, 23}, null );
                foreach (DocItem doc in docs)
                {
                    int res;
                    int tp = doc.GeneralDocsItem.DocType.Value;
                    KeyValuePair<int, string> num_key = new KeyValuePair<int, string>(tp, null);
                    if (!_nums.ContainsKey(num_key))
                        _nums.Add(num_key, 1);
                    doc.GeneralDocsItem.DocNum = _nums[num_key];

                    if (doc.GeneralDocsItem.DocType == 23)
                        res = _logic.SaveProductOutSingle(doc);
                    else
                        res = _logic.SaveProductOut(doc);
                    if (_nums.ContainsKey(num_key))
                        _nums[num_key] += 1;

                    if (res <= 0)
                    {
                        MessageBoxForm.Show(Application.ProductName, "შეცდომა ოპერაციის შენახვისას", _logic.ErrorEx, null, SystemIcons.Error);
                        return false;
                    }
                }
            }
            return true;
        }

     
    }
}
