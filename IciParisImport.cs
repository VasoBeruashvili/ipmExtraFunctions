using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using ipmPMBasic;
using ipmBLogic;
using ipmPMBasic.Models;


using System.Transactions;
using ipmControls;
namespace ipmExtraFunctions
{
    public class IciParisImport
    {
        public ProgramManagerBasic ProgramManager { get; set; }

        public IciParisImport() : this(null)
        { }
        public IciParisImport(ProgramManagerBasic pm)
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
            var groupeds = eData.GroupBy(g => Convert.ToInt32(g["storeid"])).Select(g => new { items = g.ToList() }).ToList();
            if (groupeds == null || !groupeds.Any())
                return false;
            ProgramManager.InitialiseLog(new List<string> { "№", "თარიღი", "თანხა", "საწყობი", "საქონელი", "შტრიხკოდი1", "შტრიხკოდი2", "შტრიხკოდი3", "კოდი", "რაოდენობა", "ფასი", "შედეგი" });
            foreach (var root in groupeds)
            {
                int unique_store = Convert.ToInt32(root.items.First()["storeid"]);
                DateTime unique_date = Convert.ToDateTime(root.items.First()["tdate"]).AddHours(23).AddMinutes(59);
                string unique_store_name =Convert.ToString(root.items.First()["store_name"]);

                int my_storeid = ProgramManager.GetDataManager().GetIntegerValue("SELECT id FROM book.Stores WHERE " + ProgramManager.GetDataManager().GetTableSystemColumnName("book.Stores", "STORE_EXPORT_ACCOUNT") + " = '" + unique_store+"'");
                if (my_storeid <= 0)
                {
                    ProgramManager.AddLogFormItem(new List<string> { "", unique_date.ToString(), "", unique_store_name, "","","","","","","", "საწყობი ვერ მოიძებნა" }, 0);
                    continue;
                }
                List<ProductsFlow> flow = new List<ProductsFlow>();
                foreach (var flow_item in root.items)
                {
                    string product_name = Convert.ToString(flow_item["product_name"]);
                    string barcode1 = Convert.ToString(flow_item["barcode1"]);
                    string barcode2 = Convert.ToString(flow_item["barcode2"]);
                    string barcode3 = Convert.ToString(flow_item["barcode3"]);
                    string mycode = Convert.ToString(flow_item["mycode"]);
                    double quantity = Convert.ToDouble(flow_item["quantity"]);
                    double price = Convert.ToDouble(flow_item["price"]) / quantity;

                    string product_uid = string.Empty;

                    if (!string.IsNullOrEmpty(barcode1))
                        product_uid = ProgramManager.GetDataManager().GetProductUIidByBarcodeWithSpace(barcode1);
                    if (string.IsNullOrEmpty(product_uid))
                        if (!string.IsNullOrEmpty(barcode2))
                            product_uid = ProgramManager.GetDataManager().GetProductUIidByBarcodeWithSpace(barcode2);
                    if (string.IsNullOrEmpty(product_uid))
                        if (!string.IsNullOrEmpty(barcode3))
                            product_uid = ProgramManager.GetDataManager().GetProductUIidByBarcodeWithSpace(barcode3);
                    if (string.IsNullOrEmpty(product_uid))
                        if (!string.IsNullOrEmpty(mycode))
                            product_uid = ProgramManager.GetDataManager().GetProductUIidByCodeWithSpace(mycode);
                    if (string.IsNullOrEmpty(product_uid))
                        product_uid = ProgramManager.GetDataManager().GetProductUIidByNameWithSpace(product_name);

                    if (string.IsNullOrEmpty(product_uid))
                    {
                        ProgramManager.AddLogFormItem(new List<string> { "", unique_date.ToString(), "", unique_store_name, product_name, barcode1, barcode2, barcode3, mycode, quantity.ToString(), price.ToString(), "პროდუქტი ვერ მოიძებნა" }, 0);
                        continue;
                    }

                    int product_id=ProgramManager.GetDataManager().GetProductIDByUID(product_uid);
                    flow.Add(new ProductsFlow
                    {
                        ProductId = product_id,
                        Amount = quantity,
                        Price = price,
                        UnitId = 1,
                        StoreId = my_storeid,
                        VatPercent = 18,
                        Coeff = -1
                    });
                }

                if (flow.Any())
                {
                    double amount = root.items.Sum(a => Convert.ToDouble(a["price"]));
                    int project_id = ProgramManager.GetDataManager().GetStoreProjectID(my_storeid);
                    using (BusinesContext bc = new BusinesContext())
                    {
                      int  dt = bc.SaveProductOutSingle(new DocItem
                        {
                            GeneralDocsItem = new GeneralDocs
                            {
                                Tdate = unique_date,
                                DocType = 23,
                                Purpose = string.Concat("საცალო გაყიდვა ", unique_store_name),
                                Amount = amount,
                                Vat = 18,
                                UserId = ProgramManager.GetUserID(),
                                ParamId1 = 0,
                                ParamId2 = my_storeid,
                                MakeEntry = true,
                                StoreId = my_storeid,
                                Uid = Guid.NewGuid().ToString()
                            },
                            ProductOutSingleItem = new ProductOutSingle
                            {
                                PriceType = 5
                            },
                            ProductsFlowList = flow
                        });

                        if (dt<=0)
                        {
                            ProgramManager.AddLogFormItem(new List<string> { "", unique_date.ToString(), amount.ToString(), unique_store_name, "", "", "", "", "", "", "", string.Concat("გაყიდვის ოპერაციის შენახვა ვერ მოხერხდა > ",  bc.ErrorEx) }, 0);
                                continue;
                        }
                        ProgramManager.AddLogFormItem(new List<string> { "0", unique_date.ToString(), amount.ToString(), unique_store_name, "", "", "", "", "", "", "", "ok" }, 1);
                    }

                }

                //if (pItem.Any())
                //{
                //    double amount = root.items.Sum(a => Convert.ToDouble(a["price"]));
                //    int project_id = ProgramManager.GetDataManager().GetStoreProjectID(my_storeid);
                //    BusinessLogic bLogic = new BusinessLogic(ProgramManager.GetDataManager());
                //    string doc_num = bLogic.GetDocMaxID("DOC_PRODUCTOUT_SINGLE");
                //    if (bLogic.Insert_ProductOutSingle(string.Empty, unique_date, string.Empty, doc_num, string.Concat("საცალო გაყიდვა  №", doc_num, " ", unique_store_name), amount, 1, 1, 18, 0, my_storeid, 0, project_id,
                //         ProgramManager.GetUserID(), true, 0, pItem.ToArray()) <= 0)
                //    {
                //        ProgramManager.AddLogFormItem(new List<string> { "", unique_date.ToString(), amount.ToString(), unique_store_name, "გაყიდვის ოპერაციის შენახვა ვერ მოხერხდა" }, 0);
                //        continue;
                //    }
                //    ProgramManager.AddLogFormItem(new List<string> { doc_num, unique_date.ToString(), amount.ToString(), unique_store_name, "ok" }, 1);
                //}

            }
            ProgramManager.ShowLogForm();
            return true;
        }

        public bool OnImportAroma()
        {
            OpenFileDialog oDialog = new OpenFileDialog() { Title = "Aroma დაგროვების ბარათების იმპორტი", Filter = "Excel Files|*.xls;*.xlsx" };
            if (oDialog.ShowDialog() != DialogResult.OK)
                return false;
            DataTable aData = ProgramManager.GetDataManager().GetTableDataFromExcel(oDialog.FileName);
            if (aData == null || aData.Rows.Count == 0)
                return false;

            string c_path = ProgramManager.GetDataManager().GetStringValue("SELECT path FROM book.GroupContragents WHERE name=N'Imported'");
            if (string.IsNullOrEmpty(c_path))
                return false;
            string col_contragent = ProgramManager.GetDataManager().GetTableSystemColumnName("book.Contragents", "CONTRAGENT_EXPORT_ACCOUNT");
            if (string.IsNullOrEmpty(col_contragent))
                return false;
            string col_store = ProgramManager.GetDataManager().GetTableSystemColumnName("book.Stores", "STORE_EXPORT_ACCOUNT");
            if (string.IsNullOrEmpty(col_store))
                return false;
            double vat_percent = ProgramManager.GetDataManager().GetVatPercent();
            int user_id = ProgramManager.GetUserID();
            DateTime current_date=DateTime.Now;
            ProgramManager.InitialiseLog(new List<string> { "კონტრ. კოდი", "კონტრ. სახელი", "რეგ. თარიღი", "ბარათის კოდი" ,"თანხა", "შედეგი" });
            BusinessLogic bLogic = new BusinessLogic(ProgramManager.GetDataManager());
            ProgressDispatcher.Activate();
            foreach (DataRow row in aData.Rows)
            {
                int c_id = Convert.ToInt32(row["customerid"]);
                string c_code = Convert.ToString(row["customer_code"]);
                string c_name = Convert.ToString(row["customer_name"]);
                string c_phone = Convert.ToString(row["mobile_phone"]);
                if (string.IsNullOrEmpty(c_phone))
                    c_phone = Convert.ToString(row["phone"]);
                string cc_code = Convert.ToString(row["card_code"]);
                int cc_store = Convert.ToInt32(row["card_store"]);
                double cc_amount = 0;
                double.TryParse(Convert.ToString(row["card_amount"]), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out cc_amount);
                DateTime cc_date = Convert.ToDateTime(row["card_regdate"]);
                int new_store = ProgramManager.GetDataManager().GetIntegerValue("SELECT id FROM book.Stores WHERE " + col_store + " = '" + cc_store + "'");
                if (new_store <= 0)
                {
                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "საწყობი ვერ მოიძებნა!" }, 0);
                    continue;
                }
                int new_contragent = ProgramManager.GetDataManager().GetIntegerValue("SELECT id FROM book.Contragents WHERE " + col_contragent + " = '" + c_id + "'");
                int card_id = ProgramManager.GetDataManager().GetCardIDByCode(cc_code, 1);
                if (card_id > 0)
                    continue;
                ProgramManager.GetDataManager().Close();
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(0, 2, 0) }))
                {
                    ProgramManager.GetDataManager().Open();
                    //ბარათის დამატება

                    if (new_contragent <= 0)
                    {
                        new_contragent = bLogic.Insert_Contragent(c_path, c_code, c_name, c_name, string.Empty, c_phone, 0, 0);
                        if (new_contragent <= 0)
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "კონტრაგენტი ვერ მოიძებნა და ვერც დაემატა!" }, 0);
                            continue;
                        }
                        if (!ProgramManager.GetDataManager().ExecuteSql("UPDATE book.Contragents SET " + col_contragent + " =" + c_id + " WHERE id=" + new_contragent))
                        {
                            {
                                Transaction.Current.Rollback();
                                ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "ვერ მოხერხდა კონტრაგენტის დაკავშირება!" }, 0);
                                continue;
                            }
                        }
                    }

                    int general_id = ProgramManager.GetDataManager().Insert_GeneralDocs(cc_date, string.Empty, 0, 118, string.Concat("Aroma ბარათის გაცემა: ", c_name, ", ", cc_code), 0, 1, 1.0, vat_percent, user_id, 0, new_contragent, new_store, 0, false, 1, 1,0);
                    if (general_id <= 0)
                    {
                        Transaction.Current.Rollback();
                        ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "ბარათის გაცემის შენახვა ვერ მოხერხდა ძირითად ცხრილში!" }, 0);
                        continue;
                    }
                    int _id = ProgramManager.GetDataManager().GetIntegerValue(@"INSERT INTO book.ContragentCardCodes (contragent_id, card_code, person_code, person_name, person_address, person_tel,  type_id, status, discount, general_id, store_id, coeff, operation_type, discount_id, bank_id, bank_code) 
                                                                VALUES
                                                                (@contragent_id, @card_code, @person_code, @person_name, @person_address, @person_tel, @type_id, @status, @discount, @general_id, @store_id, @coeff, @operation_type, @discount_id, @bank_id, @bank_code) SELECT SCOPE_IDENTITY()",

                            new Hashtable() {
                    {"@contragent_id",new_contragent }, 
                    {"@card_code", cc_code},
                    {"@person_code", string.Empty},
                    {"@person_name", string.Empty},
                    {"@person_address", string.Empty},
                    {"@person_tel", string.Empty},
                    {"@type_id", 1},
                    {"@status", 1},
                    {"@discount", 0},
                    {"@general_id", general_id},
                    {"@store_id", new_store},
                    {"@coeff", -1},
                    {"@operation_type", 118},
                    {"@discount_id", 0},
                    {"@bank_id", 0},
                    {"@bank_code", string.Empty},
                    });
                    if (_id <= 0)
                    {
                        Transaction.Current.Rollback();
                        ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "ბარათის გაცემის შენახვა ვერ მოხერხდა!" }, 0);
                        continue;
                    }
                    card_id = _id;

                    if (cc_amount > 0)
                    {
                        //ნაშთის დასმა
                        int new_general_id = ProgramManager.GetDataManager().Insert_GeneralDocs(
                            current_date, "", 0, 88, string.Concat("AROMA ბარათის მიმდინარე ნაშთის დასმა: ", c_code, ", ", c_name, ": ", cc_code, ", ", cc_amount), cc_amount, 1, 1, 0, user_id, 0, new_contragent, card_id, 0, false, 1, 1,0);
                        if (new_general_id <= 0)
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "AROMA ბარათის ნაშთის შენახვა ვერ მოხერხდა ძირითად ცხრილში!" }, 0);
                            continue;
                        }

                        if (!ProgramManager.GetDataManager().ExecuteSql("INSERT INTO doc.Special (general_id, type) VALUES(" + new_general_id + ", " + 3 + ")"))
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "AROMA ბარათის ნაშთის შენახვა ვერ მოხერხდა ცხრილში!" }, 0);
                            continue;
                        }
                        if (!ProgramManager.GetDataManager().InsertCardFlow(new_general_id, new_contragent, card_id, current_date, 1, cc_amount))
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "AROMA ბარათის ნაშთის შენახვა ვერ მოხერხდა ქვე ცხრილში!" }, 0);
                            continue;
                        }
                    }

                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "Ok" }, 1);
                    scope.Complete();
                }
            }
            ProgressDispatcher.Deactivate();
            ProgramManager.ShowLogForm();
            return true;
            
        }

        public bool OnImportDiscountPercent()
        {
            OpenFileDialog oDialog = new OpenFileDialog() { Title = "ფასდაკლების (%) ბარათების იმპორტი", Filter = "Excel Files|*.xls;*.xlsx" };
            if (oDialog.ShowDialog() != DialogResult.OK)
                return false;
            DataTable aData = ProgramManager.GetDataManager().GetTableDataFromExcel(oDialog.FileName);
            if (aData == null || aData.Rows.Count == 0)
                return false;

            string c_path = ProgramManager.GetDataManager().GetStringValue("SELECT path FROM book.GroupContragents WHERE name=N'Imported'");
            if (string.IsNullOrEmpty(c_path))
                return false;
            string col_contragent = ProgramManager.GetDataManager().GetTableSystemColumnName("book.Contragents", "CONTRAGENT_EXPORT_ACCOUNT");
            if (string.IsNullOrEmpty(col_contragent))
                return false;
            string col_store = ProgramManager.GetDataManager().GetTableSystemColumnName("book.Stores", "STORE_EXPORT_ACCOUNT");
            if (string.IsNullOrEmpty(col_store))
                return false;
            Dictionary<int, double> discounts = ProgramManager.GetDataManager().GetDictionary<int,double>("SELECT id, ISNULL(discount_percent,0) AS val FROM book.sales WHERE discount_percent IS NOT NULL ");
            if (discounts == null || discounts.Count <= 0)
                return false;
            double vat_percent = ProgramManager.GetDataManager().GetVatPercent();
            int user_id = ProgramManager.GetUserID();
            DateTime current_date = DateTime.Now;
            ProgramManager.InitialiseLog(new List<string> { "კონტრ. კოდი", "კონტრ. სახელი", "რეგ. თარიღი", "ბარათის კოდი", "თანხა", "შედეგი" });
            BusinessLogic bLogic = new BusinessLogic(ProgramManager.GetDataManager());
            ProgressDispatcher.Activate();
            foreach (DataRow row in aData.Rows)
            {
                int c_id = Convert.ToInt32(row["customerid"]);
                string c_code = Convert.ToString(row["customer_code"]);
                string c_name = Convert.ToString(row["customer_name"]);
                string c_phone = Convert.ToString(row["mobile_phone"]);
                if (string.IsNullOrEmpty(c_phone))
                    c_phone = Convert.ToString(row["phone"]);
                string cc_code = Convert.ToString(row["card_code"]);
                int cc_store = Convert.ToInt32(row["card_store"]);
                DateTime cc_date = Convert.ToDateTime(row["card_regdate"]);
                double cc_amount = 0;
                double.TryParse(Convert.ToString(row["discount_val"]), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out cc_amount);
                if(!discounts.ContainsValue(cc_amount))
                {
                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "ფასდაკლება ვერ მოიძებნა!" }, 0);
                    continue;
                }
                double bonus_discount_amount = 0;
                double.TryParse(Convert.ToString(row["card_amount"]), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out bonus_discount_amount);

                int discount_id = discounts.FirstOrDefault(a => a.Value == cc_amount).Key;
                int new_store = ProgramManager.GetDataManager().GetIntegerValue("SELECT id FROM book.Stores WHERE " + col_store + " = '" + cc_store + "'");
                if (new_store <= 0)
                {
                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "საწყობი ვერ მოიძებნა!" }, 0);
                    continue;
                }
                int new_contragent = ProgramManager.GetDataManager().GetIntegerValue("SELECT id FROM book.Contragents WHERE " + col_contragent + " = '" + c_id + "'");
                int card_id = ProgramManager.GetDataManager().GetCardIDByCode(cc_code, 2);
                if (card_id > 0)
                    continue;
                ProgramManager.GetDataManager().Close();
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(0, 2, 0) }))
                {
                    ProgramManager.GetDataManager().Open();
                        //ბარათის დამატება

                        if (new_contragent <= 0)
                        {
                            new_contragent = bLogic.Insert_Contragent(c_path, c_code, c_name, c_name, string.Empty, c_phone, 0, 0);
                            if (new_contragent <= 0)
                            {
                                Transaction.Current.Rollback();
                                ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "კონტრაგენტი ვერ მოიძებნა და ვერც დაემატა!" }, 0);
                                continue;
                            }
                            if (!ProgramManager.GetDataManager().ExecuteSql("UPDATE book.Contragents SET " + col_contragent + " =" + c_id + " WHERE id=" + new_contragent))
                            {
                                {
                                    Transaction.Current.Rollback();
                                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "ვერ მოხერხდა კონტრაგენტის დაკავშირება!" }, 0);
                                    continue;
                                }
                            }
                        }


                        int general_id = ProgramManager.GetDataManager().Insert_GeneralDocs(cc_date, string.Empty, 0, 118, string.Concat("ფასდაკლების (%) ბარათის გაცემა: ", c_name, ", ", cc_code), 0, 1, 1.0, vat_percent, user_id, 0, new_contragent, new_store, 0, false, 1, 1,0);
                        if (general_id <= 0)
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "ბარათის გაცემის შენახვა ვერ მოხერხდა ძირითად ცხრილში!" }, 0);
                            continue;
                        }

                        int _id = ProgramManager.GetDataManager().GetIntegerValue(@"INSERT INTO book.ContragentCardCodes (contragent_id, card_code, person_code, person_name, person_address, person_tel,  type_id, status, discount, general_id, store_id, coeff, operation_type, discount_id, bank_id, bank_code) 
                                                                VALUES
                                                                (@contragent_id, @card_code, @person_code, @person_name, @person_address, @person_tel, @type_id, @status, @discount, @general_id, @store_id, @coeff, @operation_type, @discount_id, @bank_id, @bank_code) SELECT SCOPE_IDENTITY()",

                            new Hashtable() {
                    {"@contragent_id",new_contragent }, 
                    {"@card_code", cc_code},
                    {"@person_code", string.Empty},
                    {"@person_name", string.Empty},
                    {"@person_address", string.Empty},
                    {"@person_tel", string.Empty},
                    {"@type_id", 2},
                    {"@status", 1},
                    {"@discount", cc_amount},
                    {"@general_id", general_id},
                    {"@store_id", new_store},
                    {"@coeff", -1},
                    {"@operation_type", 118},
                    {"@discount_id", discount_id},
                    {"@bank_id", 0},
                    {"@bank_code", string.Empty},
                    });
                    if(_id<=0)
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "ბარათის გაცემის შენახვა ვერ მოხერხდა!" }, 0);
                            continue;
                        }
                    card_id = _id;

                    if (bonus_discount_amount > 0)
                    {
                        //ნაშთის დასმა
                        int new_general_id = ProgramManager.GetDataManager().Insert_GeneralDocs(
                            current_date, "", 0, 88, string.Concat("ფასდაკლების ბარათის მიმდინარე ნაშთის დასმა: ", c_code, ", ", c_name, ": ", cc_code, ", ", bonus_discount_amount), bonus_discount_amount, 1, 1, 0, user_id, 0, new_contragent, card_id, 0, false, 1, 1,0);
                        if (new_general_id <= 0)
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, bonus_discount_amount.ToString(), "ფასდაკლების ბარათის ნაშთის შენახვა ვერ მოხერხდა ძირითად ცხრილში!" }, 0);
                            continue;
                        }

                        if (!ProgramManager.GetDataManager().ExecuteSql("INSERT INTO doc.Special (general_id, type) VALUES(" + new_general_id + ", " + 4 + ")"))
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, bonus_discount_amount.ToString(), "ფასდაკლების ბარათის ნაშთის შენახვა ვერ მოხერხდა ცხრილში!" }, 0);
                            continue;
                        }
                        if (!ProgramManager.GetDataManager().InsertDiscountCardFlow(new_general_id, 0, 0, 0, 0, new_contragent, card_id, bonus_discount_amount, 1, cc_amount, 0))
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, bonus_discount_amount.ToString(), "ფასდაკლების ბარათის ნაშთის შენახვა ვერ მოხერხდა ქვე ცხრილში!" }, 0);
                            continue;
                        }
                    }


                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "Ok" }, 1);
                    scope.Complete();
                }
            }
            ProgressDispatcher.Deactivate();
            ProgramManager.ShowLogForm();
            return true;

        }

        public bool OnImportGift()
        {
            OpenFileDialog oDialog = new OpenFileDialog() { Title = "სასაჩუქრე ბარათების იმპორტი", Filter = "Excel Files|*.xls;*.xlsx" };
            if (oDialog.ShowDialog() != DialogResult.OK)
                return false;
            IEnumerable aData = ProgramManager.GetDataManager().GetEnumerableDataFromExcel(oDialog.FileName);
            if (aData == null)
                return false;

            string c_path = ProgramManager.GetDataManager().GetStringValue("SELECT path FROM book.GroupContragents WHERE tag='GIFTCARD'");
            if (string.IsNullOrEmpty(c_path))
                return false;
            string col_contragent = ProgramManager.GetDataManager().GetTableSystemColumnName("book.Contragents", "CONTRAGENT_EXPORT_ACCOUNT");
            if (string.IsNullOrEmpty(col_contragent))
                return false;
          
            ProgramManager.InitialiseLog(new List<string> { "კონტრ. კოდი", "კონტრ. სახელი", "რეგ. თარიღი", "ბარათის კოდი", "თანხა", "შედეგი" });
            BusinessLogic bLogic = new BusinessLogic(ProgramManager.GetDataManager());
            ProgressDispatcher.Activate();


            string compare_string=string.Join(",", aData.OfType<DataRow>().Select(a=> string.Concat("N'", Convert.ToString(a["card_code"]),"'")).ToArray());
            List<string> existing = ProgramManager.GetDataManager().GetMultiStringVal("SELECT c.code FROM book.Contragents AS c INNER JOIN book.GroupContragents AS g ON g.id=c.group_id WHERE g.tag='GIFTCARD' AND c.code IN(" + compare_string + ")");
            var insertable_items = aData.OfType<DataRow>().Where(a => !existing.Contains(Convert.ToString(a["card_code"]))).ToList();
            if (insertable_items == null || insertable_items.Count <= 0)
            {
                ProgressDispatcher.Deactivate();
                return false;
            }
            int _user_id = ProgramManager.GetUserID();
            Hashtable m_sqlParams = new Hashtable();
            foreach (DataRow row in insertable_items)
            {
                string cc_code = Convert.ToString(row["card_code"]).Trim();
                double cc_amount = 0;
                double.TryParse(Convert.ToString(row["price"]), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out cc_amount);
                DateTime _dt;
                if (!DateTime.TryParse(Convert.ToString(row["money_in_date"]),  CultureInfo.InvariantCulture, DateTimeStyles.None, out _dt))
                    _dt = DateTime.MinValue;
                ProgramManager.GetDataManager().Close();
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(0, 2, 0) }))
                {
                    ProgramManager.GetDataManager().Open();
                    int new_contragent = bLogic.Insert_Contragent(c_path, cc_code, cc_code, cc_code,  cc_amount.ToString(), cc_amount.ToString(), 0, 0);
                    if (new_contragent <= 0)
                    {
                        Transaction.Current.Rollback();
                        ProgramManager.AddLogFormItem(new List<string> { cc_code, cc_code, string.Empty, cc_code, cc_amount.ToString(), "სასაჩუქრე ბარათი ვერ დაემატა!" }, 0);
                        continue;
                    }
                    
                    // for new
                    if (cc_code.Length != 6)
                    {
                        int general_id = ProgramManager.GetDataManager().Insert_GeneralDocs(_dt, "", 0, 70, string.Concat("სასაჩუქრე ბარათის ", cc_code, " თანხის ავანსად მიღება"), cc_amount, 1, 1.0, 0, 1, _user_id, 0, 0, 1, true, 1, 1,0);
                        if (general_id <= 0)
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { cc_code, cc_code, string.Empty, cc_code, cc_amount.ToString(), "თანხის ავანსად მიჭება ვერ შესრულდა <General>!" }, 0);
                            continue;
                        }

                        string sqlText = @"INSERT INTO doc.SingleEntry (general_id, debit_acc, credit_acc, amount,a1,b1) VALUES(@general_id, @debit_acc, @credit_acc, @amount,@a1,@b1)";
                        m_sqlParams.Clear();
                        m_sqlParams.Add("@general_id", general_id);
                        m_sqlParams.Add("@debit_acc", "1410");
                        m_sqlParams.Add("@credit_acc", "3121");
                        m_sqlParams.Add("@amount", cc_amount);
                        m_sqlParams.Add("@a1", 0);
                        m_sqlParams.Add("@b1", new_contragent);
                        if (!ProgramManager.GetDataManager().ExecuteSql(sqlText, m_sqlParams))
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { cc_code, cc_code, string.Empty, cc_code, cc_amount.ToString(), "თანხის ავანსად მიჭება ვერ შესრულდა <Single>!" }, 0);
                            continue;
                        }

                        if (!ProgramManager.GetDataManager().Insert_Entries(general_id, "1410", "3121", cc_amount, 1.0, 0, 0, 0, 0, 0, new_contragent, 0, 0, "", 1, 1))
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { cc_code, cc_code, string.Empty, cc_code, cc_amount.ToString(), "თანხის ავანსად მიჭება ვერ შესრულდა <Entry>!" }, 0);
                            continue;
                        }
                    }


                    ProgramManager.AddLogFormItem(new List<string> { cc_code, cc_code, string.Empty, cc_code, cc_amount.ToString(), "Ok" }, 1);
                    scope.Complete();
                }
            }
            ProgressDispatcher.Deactivate();
            ProgramManager.ShowLogForm();
            return true;

        }
    }
}
