using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using ipmPMBasic;
using ipmBLogic;
using System.Transactions;
using ipmControls;
using System.Data.SqlClient;
namespace ipmExtraFunctions
{
    public class CardImport
    {
        public ProgramManagerBasic ProgramManager { get; set; }

        public CardImport() : this(null)
        { }
        public CardImport(ProgramManagerBasic pm)
        {
            this.ProgramManager = pm;
        }


       

        public bool OnImportBonus()
        {
            OpenFileDialog oDialog = new OpenFileDialog() { Title = "დაგროვების ბარათების იმპორტი", Filter = "Excel Files|*.xls;*.xlsx" };
            if (oDialog.ShowDialog() != DialogResult.OK)
                return false;
            DataTable aData = ProgramManager.GetDataManager().GetTableDataFromExcel(oDialog.FileName);
            if (aData == null || aData.Rows.Count == 0)
                return false;

            double vat_percent = ProgramManager.GetDataManager().GetVatPercent();
            int user_id = ProgramManager.GetUserID();
            DateTime current_date = DateTime.Now;
            ProgramManager.InitialiseLog(new List<string> { "კონტრ. კოდი", "კონტრ. სახელი", "თარიღი", "ბარათის კოდი" ,"თანხა", "შედეგი" });
            BusinessLogic bLogic = new BusinessLogic(ProgramManager.GetDataManager());
            ProgressDispatcher.Activate();
            foreach (DataRow row in aData.Rows)
            {
                string cc_code = Convert.ToString(row["ბარათის კოდი"]);
                string c_code = Convert.ToString(row["მყიდველის კოდი"]);
                string c_name = Convert.ToString(row["მყიდველის სახელი"]);
                string c_phone = Convert.ToString(row["მყიდველის ტელ."]);
                int cc_store = Convert.ToInt32(row["გამცემი საწყობის ნომერი"]);

                double cc_amount = 0;
                double.TryParse(Convert.ToString(row["ნაშთი"]), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out cc_amount);
                DateTime cc_date = current_date;
                if (!DateTime.TryParse(Convert.ToString(row["გაცემის თარიღი"]), out cc_date))
                    cc_date = current_date;
                Nullable<bool> _store = ProgramManager.GetDataManager().GetScalar<bool>("IF EXISTS (SELECT id FROM book.Stores WHERE id=" + cc_store + ") SELECT 'TRUE' ELSE SELECT 'FALSE'");
                if (!_store.HasValue || !_store.Value)
                {
                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "საწყობი ვერ მოიძებნა!" }, 0);
                    continue;
                }

                int card_id = ProgramManager.GetDataManager().GetCardIDByCode(cc_code, 1);
                if (card_id > 0)
                {
                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "ბარათი აღნიშნული კოდით უკვე არსებობს." }, 0);
                    continue;
                }

                Nullable<int> new_contragent = null;
                if (!string.IsNullOrEmpty(c_code.Trim()))
                    new_contragent = ProgramManager.GetDataManager().GetScalar<int>("SELECT id FROM book.Contragents WHERE  @code = code", new SqlParameter("@code", c_code));


                ProgramManager.GetDataManager().Close();
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(0, 2, 0) }))
                {
                    ProgramManager.GetDataManager().Open();
                    //ბარათის დამატება

                    if (!new_contragent.HasValue || new_contragent <= 0)
                    {
                        new_contragent = bLogic.Insert_Contragent("0#2#5", c_code, c_name, c_name, string.Empty, c_phone, 0, 0);
                        if (new_contragent <= 0)
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "კონტრაგენტი ვერ მოიძებნა და ვერც დაემატა!" }, 0);
                            continue;
                        }
                    }

                    int general_id = ProgramManager.GetDataManager().Insert_GeneralDocs(cc_date, string.Empty, 0, 118, string.Concat("დაგროვების ბარათის გაცემა: ", c_name, ", ", cc_code), 0, 1, 1.0, vat_percent, user_id, 0, new_contragent.Value, cc_store, 0, false, 1, 1,0);
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
                    {"@store_id", cc_store},
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
                            current_date, "", 0, 88, string.Concat("დაგროვების ბარათის მიმდინარე ნაშთის დასმა: ", c_code, ", ", c_name, ": ", cc_code, ", ", cc_amount), cc_amount, 1, 1, 0, user_id, 0, new_contragent, card_id, 0, false, 1, 1,0);
                        if (new_general_id <= 0)
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "დაგროვების ბარათის ნაშთის შენახვა ვერ მოხერხდა ძირითად ცხრილში!" }, 0);
                            continue;
                        }

                        if (!ProgramManager.GetDataManager().ExecuteSql("INSERT INTO doc.Special (general_id, type) VALUES(" + new_general_id + ", " + 3 + ")"))
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "დაგროვების ბარათის ნაშთის შენახვა ვერ მოხერხდა ცხრილში!" }, 0);
                            continue;
                        }
                        if (!ProgramManager.GetDataManager().InsertCardFlow(new_general_id, new_contragent.Value, card_id, current_date, 1, cc_amount))
                        {
                            Transaction.Current.Rollback();
                            ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "დაგროვების ბარათის ნაშთის შენახვა ვერ მოხერხდა ქვე ცხრილში!" }, 0);
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

        public bool OnImportDiscount()
        {
            OpenFileDialog oDialog = new OpenFileDialog() { Title = "ფასდაკლების (%) ბარათების იმპორტი", Filter = "Excel Files|*.xls;*.xlsx" };
            if (oDialog.ShowDialog() != DialogResult.OK)
                return false;
            DataTable aData = ProgramManager.GetDataManager().GetTableDataFromExcel(oDialog.FileName);
            if (aData == null || aData.Rows.Count == 0)
                return false;

           
            Dictionary<int, double> discounts = ProgramManager.GetDataManager().GetDictionary<int,double>("SELECT id, ISNULL(discount_percent,0) AS val FROM book.sales WHERE discount_percent IS NOT NULL ");
            if (discounts == null || discounts.Count <= 0)
                return false;
            double vat_percent = ProgramManager.GetDataManager().GetVatPercent();
            int user_id = ProgramManager.GetUserID();
            DateTime current_date = DateTime.Now;
            ProgramManager.InitialiseLog(new List<string> { "კონტრ. კოდი", "კონტრ. სახელი", "თარიღი", "ბარათის კოდი", "თანხა", "შედეგი" });
            BusinessLogic bLogic = new BusinessLogic(ProgramManager.GetDataManager());
            ProgressDispatcher.Activate();
            foreach (DataRow row in aData.Rows)
            {
                string cc_code = Convert.ToString(row["ბარათის კოდი"]);
                string c_code = Convert.ToString(row["მყიდველის კოდი"]);
                string c_name = Convert.ToString(row["მყიდველის სახელი"]);
                string c_phone = Convert.ToString(row["მყიდველის ტელ."]);
                int cc_store = Convert.ToInt32(row["გამცემი საწყობის ნომერი"]);

                DateTime cc_date = current_date;
                if (!DateTime.TryParse(Convert.ToString(row["გაცემის თარიღი"]), out cc_date))
                    cc_date = current_date;

                double cc_amount = 0;
                double.TryParse(Convert.ToString(row["ფასდაკლების %"]), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out cc_amount);
                if(!discounts.ContainsValue(cc_amount))
                {
                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "ფასდაკლება ვერ მოიძებნა!" }, 0);
                    continue;
                }
                double bonus_discount_amount = 0;
                double.TryParse(Convert.ToString(row["ნაშთი"]), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out bonus_discount_amount);

                int discount_id = discounts.FirstOrDefault(a => a.Value == cc_amount).Key;

                Nullable<bool> _store = ProgramManager.GetDataManager().GetScalar<bool>("IF EXISTS (SELECT id FROM book.Stores WHERE id=" + cc_store + ") SELECT 'TRUE' ELSE SELECT 'FALSE'");
                if (!_store.HasValue || !_store.Value)
                {
                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "საწყობი ვერ მოიძებნა!" }, 0);
                    continue;
                }
                
                int card_id = ProgramManager.GetDataManager().GetCardIDByCode(cc_code, 2);
                if (card_id > 0)
                {
                    ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "ბარათი აღნიშნული კოდით უკვე არსებობს." }, 0);
                    continue;
                }

                Nullable<int> new_contragent = null;
                if (!string.IsNullOrEmpty(c_code.Trim()))
                    new_contragent = ProgramManager.GetDataManager().GetScalar<int>("SELECT id FROM book.Contragents WHERE  @code = code", new SqlParameter("@code", c_code));

                ProgramManager.GetDataManager().Close();
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(0, 2, 0) }))
                {
                    ProgramManager.GetDataManager().Open();
                        //ბარათის დამატება

                    if (!new_contragent.HasValue || new_contragent <= 0)
                        {
                            new_contragent = bLogic.Insert_Contragent("0#2#5", c_code, c_name, c_name, string.Empty, c_phone, 0, 0);
                            if (new_contragent <= 0)
                            {
                                Transaction.Current.Rollback();
                                ProgramManager.AddLogFormItem(new List<string> { c_code, c_name, cc_date.ToString(), cc_code, cc_amount.ToString(), "კონტრაგენტი ვერ მოიძებნა და ვერც დაემატა!" }, 0);
                                continue;
                            }
                        }

                        int general_id = ProgramManager.GetDataManager().Insert_GeneralDocs(cc_date, string.Empty, 0, 118, string.Concat("ფასდაკლების (%) ბარათის გაცემა: ", c_name, ", ", cc_code), 0, 1, 1.0, vat_percent, user_id, 0, new_contragent, cc_store, 0, false, 1, 1,0);
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
                    {"@store_id", cc_store},
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
                        if (!ProgramManager.GetDataManager().InsertDiscountCardFlow(new_general_id, 0, 0, 0, 0, new_contragent.Value, card_id, bonus_discount_amount, 1, cc_amount, 0))
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
           
            ProgramManager.InitialiseLog(new List<string> { "კონტრ. კოდი", "კონტრ. სახელი", "თარიღი", "ბარათის კოდი", "თანხა", "შედეგი" });
            BusinessLogic bLogic = new BusinessLogic(ProgramManager.GetDataManager());
            ProgressDispatcher.Activate();


            string compare_string = string.Join(",", aData.OfType<DataRow>().Select(a => string.Concat("N'", Convert.ToString(a["ბარათის კოდი"]), "'")).ToArray());
            List<string> existing = ProgramManager.GetDataManager().GetMultiStringVal("SELECT c.code FROM book.Contragents AS c INNER JOIN book.GroupContragents AS g ON g.id=c.group_id WHERE g.tag='GIFTCARD' AND c.code IN(" + compare_string + ")");
            var insertable_items = aData.OfType<DataRow>().Where(a => !existing.Contains(Convert.ToString(a["ბარათის კოდი"]))).ToList();
            if (insertable_items == null || insertable_items.Count <= 0)
            {
                ProgressDispatcher.Deactivate();
                return false;
            }
            int _user_id = ProgramManager.GetUserID();
           // Hashtable m_sqlParams = new Hashtable();
            foreach (DataRow row in insertable_items)
            {
                string cc_code = Convert.ToString(row["ბარათის კოდი"]);
                double cc_amount = 0;
                double.TryParse(Convert.ToString(row["ბარათის ღირებულება"]), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out cc_amount);
                DateTime _dt;
                if (!DateTime.TryParse(Convert.ToString(row["გაცემის თარიღი"]), CultureInfo.InvariantCulture, DateTimeStyles.None, out _dt))
                    _dt = DateTime.Now;
                ProgramManager.GetDataManager().Close();
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted, Timeout = new TimeSpan(0, 2, 0) }))
                {
                    ProgramManager.GetDataManager().Open();
                    int new_contragent = bLogic.Insert_Contragent(c_path, cc_code, cc_code, cc_code, cc_amount.ToString(), cc_amount.ToString(), 0, 0);
                    if (new_contragent <= 0)
                    {
                        Transaction.Current.Rollback();
                        ProgramManager.AddLogFormItem(new List<string> { cc_code, cc_code, string.Empty, cc_code, cc_amount.ToString(), "სასაჩუქრე ბარათი ვერ დაემატა!" }, 0);
                        continue;
                    }

                    // for new

                    int general_id = ProgramManager.GetDataManager().Insert_GeneralDocs(_dt, "", 0, 70, string.Concat("სასაჩუქრე ბარათის ", cc_code, " ნაშთის დასმა"), cc_amount, 1, 1.0, 0, 1, _user_id, 0, 0, 1, true, 1, 1,0);
                    if (general_id <= 0)
                    {
                        Transaction.Current.Rollback();
                        ProgramManager.AddLogFormItem(new List<string> { cc_code, cc_code, string.Empty, cc_code, cc_amount.ToString(), "ნაშთის დასმა ვერ შესრულდა <General>!" }, 0);
                        continue;
                    }

                    int _a1 = 0;
                    string sqlText = @"INSERT INTO doc.SingleEntry (general_id, debit_acc, credit_acc, amount,a1,b1) VALUES(@general_id, @debit_acc, @credit_acc, @amount, @a1, @b1)";
                    SqlParameter[] param = new SqlParameter[]
                    {
                     new  SqlParameter("@general_id", general_id),
                     new  SqlParameter("@debit_acc", "1410"),
                     new  SqlParameter("@credit_acc", "3121"),
                     new  SqlParameter("@amount", cc_amount),
                     new  SqlParameter("@a1", _a1),
                     new  SqlParameter("@b1", new_contragent)
                    };
                    Nullable<int> ress = ProgramManager.GetDataManager().ExecuteSql(sqlText, param);
                    if (!ress.HasValue || ress.Value < 0)
                    {
                        Transaction.Current.Rollback();
                        ProgramManager.AddLogFormItem(new List<string> { cc_code, cc_code, string.Empty, cc_code, cc_amount.ToString(), "ნაშთის დასმა ვერ შესრულდა <Single>!" }, 0);
                        continue;
                    }

                    if (!ProgramManager.GetDataManager().Insert_Entries(general_id, "0000", "3121", cc_amount, 1.0, 0, 0, 0, 0, 0, new_contragent, 0, 0, "", 1, 1))
                    {
                        Transaction.Current.Rollback();
                        ProgramManager.AddLogFormItem(new List<string> { cc_code, cc_code, string.Empty, cc_code, cc_amount.ToString(), "ნაშთის დასმა ვერ შესრულდა <Entry>!" }, 0);
                        continue;
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
