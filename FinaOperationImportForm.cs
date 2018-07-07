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
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ipmExtraFunctions
{
    public enum ImportBehavoir
    {
        [Description("ჩატვირთვადია")]
        Available,

        [Description("წარმატებით ჩაიტვირთა")]
        SuccessfullyImported,

        [Description("შეცდომა ოპერაციის იმპორტისას")]
        Faild,

        [Description("დოკუმენტი უკვე არსებობს")]
        AlreadyExist,

        [Description("ვალუტა ვერ მოიძებნა")]
        CurrencyUnavailable,

        [Description("მომხმარებელი ვერ მოიძებნა")]
        UserUnavailable,

        [Description("პროექტი ვერ მოიძებნა")]
        ProjectUnavailable,

        [Description("თანაშრომელი ვერ მოიძებნა")]
        StaffUnavailable,

        [Description("მყიდველი ვერ მოიძებნა")]
        CustomerUnavailable,

        [Description("მომწოდებელი ვერ მოიძებნა")]
        VendorUnavailable,

        [Description("პარტნიორი ვერ მოიძებნა")]
        PartnerUnavailable,

        [Description("საწყობი ვერ მოიძებნა")]
        StoreUnavailable,

        [Description("სალარო ვერ მოიძებნა")]
        CasheUnavailable,

        [Description("სასაქონლო მატერიალური მარაგი ვერ მოიძებნა")]
        ProductUnavailable,

        [Description("ერთეული ვერ მოიძებნა")]
        UnitUnavailable,

        [Description("ქვე-კოდი ვერ მოიძებნა")]
        SubCodeUnavailable,

        [Description("ანალიზური კოდი ვერ მოიძებნა")]
        AnalyticCodeUnavailable,

        [Description("POS ტერმინალი ვერ მოიძებნა")]
        POSUnavailable,

        [Description("ბანკის ანგარიში ვერ მოიძებნა")]
        BankAccountUnavailable,

        //[Description("ბუღალტრული ანგარიში ვერ მოიძებნა")]
        //AccountChartUnavailable,

        [Description("მიღებული სესხი ვერ მოიძებნა")]
        CreditInUnavailable,

        [Description("გაცემული სესხი ვერ მოიძებნა")]
        CreditOutUnavailable,

        [Description("საკრედიტო ორგანიზაცია ვერ მოიძებნა")]
        CreditBankUnavailable,

        [Description("ფასის ტიპი ვერ მოიძებნა")]
        PriceTypeUnavailable,

        [Description("კონტრაგენტის ხელშეკრულება ვერ მოიძებნა")]
        ContragentAgreementUnavailable,

        [Description("თანამშრომლის დეპარტამენტი ვერ მოიძებნა")]
        GroupStaffDepUnavailable,

        [Description("თანამშრომლის თანამდებობა ვერ მოიძებნა")]
        StaffPositionUnavailable,

        [Description("სატრანსპორტო საშუალება ვერ მოიძებნა")]
        CarUnavailable,

        [Description("კაფე - მაგიდა ვერ მოიძებნა")]
        CafeTableUnavailable,

        [Description("კაფე - დარბაზი ვერ მოიძებნა")]
        CafeRoomUnavailable,

        [Description("კაფე - გაუქმების მიზეზი ვერ მოიძებნა")]
        CafeCommentUnavailable,

        [Description("დოკუმენტის ტიპი არ არის მხარდაჭერილი")]
        UnsupportedDocType


    }
   
    public partial class FinaOperationImportForm : Form
    {
        ProgramManagerBasic pm;

        public FinaOperationImportForm(ProgramManagerBasic pm)
        {
            InitializeComponent();
            this.pm = pm;
            pm.TranslateControl(this);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string pt = null;
            using (OpenFileDialog _dlg = new OpenFileDialog { Filter = "JSON Data Files (*.json)|*.json" })
            {
                if (_dlg.ShowDialog() != DialogResult.OK)
                    return;
                pt = _dlg.FileName;
            }
            ReadFile(pt);
        }

        private void ReadFile(string path)
        {
            m_Grid.Rows.Clear();
            if (!File.Exists(path))
                return;
            ProgressDispatcher.Activate(true);
            using (StreamReader rd = new StreamReader(path, Encoding.UTF8))
            {
                string _res = rd.ReadToEnd();
                if (string.IsNullOrEmpty(_res))
                {
                    ProgressDispatcher.Deactivate();
                    return;
                }
                ExportedDocItem impo = null;
                try
                {
                    impo = Special.DeSerialiseJson<ExportedDocItem>(_res);
                }
                catch (Exception ex)
                {
                    ProgressDispatcher.Deactivate();
                    MessageBoxForm.Show(Application.ProductName, "შეცდომა ინფორმაციის დამუშავებისას", string.Concat(ex.Message, ex.StackTrace), null, SystemIcons.Error);
                    return;
                }
                if (impo.Docs == null || !impo.Docs.Any())
                {
                    ProgressDispatcher.Deactivate();
                    return;
                }
                HashSet<Guid> _existing = new HashSet<Guid>(pm.GetDataManager().GetList<Guid>("SELECT CAST(uid AS uniqueidentifier)FROM doc.Generaldocs WHERE uid IS NOT NULL AND uid !=''"));
                if (_existing == null)
                {
                    ProgressDispatcher.Deactivate();
                    return;
                }
                Dictionary<int, string> _doctypes = pm.GetDataManager().GetDictionary<int, string>("SELECT id, name FROM config.DocTypes");

                HashSet<int> _supported_docs = new HashSet<int> {2, 5, 8, 9, 15, 16, 18, 20, 21, 23, 25, 28, 29, 31, 32, 38, 39, 40, 41, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 61, 64, 65, 66, 74, 76, 77, 93, 94, 95, 96, 120, 124, 135, 138, 140 };

                HashSet<int> _store_paramid2 = new HashSet<int> { 8, 9, 11, 12, 13, 14, 15, 16, 17, 18, 20, 21, 23, 25, 29, 31, 32, 36, 62, 63, 74, 85, 86, 97, 99, 107, 134, 149, 150, 153, 154 };
                HashSet<int> _store_paramid1 = new HashSet<int> { 12, 20, 66, 74, 77, 84, 97, 98, 99, 137 };

                HashSet<int> _cashe_paramid1 = new HashSet<int> { 23, 38, 39, 41, 44, 45, 48, 49, 50, 51, 52, 54, 56, 57, 61, 64, 65, 93, 95, 96, 120, 124, 138 };
                HashSet<int> _cashe_paramid2 = new HashSet<int> { 40, 61 };

                HashSet<int> _customer_paramid1 = new HashSet<int> { 8, 9, 13, 21, 29, 107, 121, 129, 149, 150, 151, 153 };
                HashSet<int> _customer_paramid2 = new HashSet<int> { 38, 49, 65, 66 };

                HashSet<int> _vendor_paramid1 = new HashSet<int> { 11, 16, 17, 46, 63, 83, 28, 31, 32, 130, 154 };
                HashSet<int> _vendor_paramid2 = new HashSet<int> { 39, 50, 58, 120 };

                HashSet<int> _partner_param_id2 = new HashSet<int> { 55, 56 };


                HashSet < int> _staff_paramid2 = new HashSet<int> { 44, 45, 46, 48, 76, 124, 135, 138 };

                HashSet<int> _credit_in_paramid2 = new HashSet<int> { 52, 53, 54, 57 };
                HashSet<int> _credit_out_paramid2 = new HashSet<int> { 93, 94, 95, 96 };



                string my_customers_sql = @"SELECT id, id FROM book.contragents WHERE path = '0#2#5' OR path LIKE '0#2#5#%'";
                string my_vendors_sql = @"SELECT id, id FROM book.contragents WHERE path = '0#1#3' OR path LIKE '0#1#3#%'";
                string my_terminals_sql= @"SELECT id, id FROM book.contragents WHERE path = '0#12#13' OR path LIKE '0#12#13#%'";
                string my_creditbanks_sql = @"SELECT id, id FROM book.contragents WHERE path = '0#14#15' OR path LIKE '0#14#15#%'";
                string my_partners_sql = @"SELECT id, id FROM book.contragents WHERE path ='0#8#9' OR path LIKE '0#8#9#%'";
                string my_currencies_sql = @"SELECT id, id FROM book.Currencies";
                string my_users_sql = @"SELECT id, id FROM book.Users";
                string my_projects_sql = @"SELECT id, id FROM book.Projects";
                string my_staffs_sql = @"SELECT id, id FROM book.Staff";
                string my_stores_sql = @"SELECT id, id FROM book.Stores";
                string my_cashes_sql= @"SELECT id, id FROM book.Cashes";
                string my_products_sql = @"SELECT id, id FROM book.Products";
                string my_subcodes_sql = @"SELECT id, id FROM book.ProductSubCodes";
                string my_units_sql = @"SELECT id, id FROM book.Units";
                string my_analyticcodes_sql = @"SELECT id, id FROM book.AnalyticCodes";
                string my_bankaccounts_sql = @"SELECT id, id FROM book.CompanyAccounts";
                string my_prices_sql = @"SELECT id, id FROM book.Prices WHERE id > 2";
                string my_contragent_agreements_sql = @"SELECT id, id FROM book.ContragentAgreements";
                string my_credit_in_sql = @"SELECT id, id FROM book.Credits";
                string my_credit_out_sql = @"SELECT id, id FROM book.OutCredits";
                string my_group_staffdep_sql = @"SELECT id, id FROM book.GroupStaffDepartments";
                string my_staffposition_sql = @"SELECT id, id FROM book.StaffPositions";
                string my_cars_sql = @"SELECT id, id FROM book.Cars";
                string my_cafe_tables_sql = @"SELECT id, id FROM book.cafetables";
                string my_cafe_rooms_sql = @"SELECT id, id FROM book.CafeRooms";
                string my_cafe_comments_sql = @"SELECT id, id FROM book.CafeComments";




                if (impo.IsExternalUse)
                {
                    my_customers_sql= @"SELECT sync_id, id FROM book.contragents WHERE sync_id IS NOT NULL AND (path = '0#2#5' OR path LIKE '0#2#5#%')";
                    my_vendors_sql = @"SELECT sync_id, id FROM book.contragents WHERE sync_id IS NOT NULL AND (path = '0#1#3' OR path LIKE '0#1#3#%')";
                    my_terminals_sql = @"SELECT sync_id, id FROM book.contragents WHERE sync_id IS NOT NULL AND (path = '0#12#13' OR path LIKE '0#12#13#%')";
                    my_creditbanks_sql = @"SELECT sync_id, id FROM book.contragents WHERE sync_id IS NOT NULL AND (path = '0#14#15' OR path LIKE '0#14#15#%')";
                    my_partners_sql = @"SELECT id, id FROM book.contragents WHERE sync_id IS NOT NULL AND (path = '0#8#9' OR path LIKE '0#8#9#%')";
                    my_currencies_sql = @"SELECT sync_id, id FROM book.Currencies WHERE sync_id IS NOT NULL";
                    my_users_sql = @"SELECT sync_id, id FROM book.Users WHERE sync_id IS NOT NULL";
                    my_projects_sql = @"SELECT sync_id, id FROM book.Projects WHERE sync_id IS NOT NULL";
                    my_staffs_sql = @"SELECT sync_id, id FROM book.Staff WHERE sync_id IS NOT NULL";
                    my_stores_sql = @"SELECT sync_id, id FROM book.Stores WHERE sync_id IS NOT NULL";
                    my_cashes_sql = @"SELECT sync_id, id FROM book.Cashes WHERE sync_id IS NOT NULL";
                    my_products_sql = @"SELECT sync_id, id FROM book.Products WHERE sync_id IS NOT NULL";
                    my_subcodes_sql = @"SELECT sync_id, id FROM book.ProductSubCodes WHERE sync_id IS NOT NULL";
                    my_units_sql = @"SELECT sync_id, id FROM book.Units WHERE sync_id IS NOT NULL";
                    my_analyticcodes_sql = @"SELECT sync_id, id FROM book.AnalyticCodes WHERE sync_id IS NOT NULL";
                    my_bankaccounts_sql = @"SELECT sync_id, id FROM book.CompanyAccounts WHERE sync_id IS NOT NULL";
                    my_prices_sql = @"SELECT sync_id, id FROM book.Prices WHERE id > 2 AND sync_id IS NOT NULL";
                    my_contragent_agreements_sql = @"SELECT sync_id, id FROM book.ContragentAgreements WHERE sync_id IS NOT NULL";
                    my_credit_in_sql = @"SELECT sync_id, id FROM book.Credits WHERE sync_id IS NOT NULL";
                    my_credit_out_sql = @"SELECT sync_id, id FROM book.OutCredits WHERE sync_id IS NOT NULL";
                    my_group_staffdep_sql = @"SELECT sync_id, id FROM book.GroupStaffDepartments WHERE sync_id IS NOT NULL";
                    my_staffposition_sql = @"SELECT sync_id, id FROM book.StaffPositions WHERE sync_id IS NOT NULL";
                    my_cars_sql = @"SELECT sync_id, id FROM book.Cars WHERE sync_id IS NOT NULL";
                    my_cafe_tables_sql = @"SELECT sync_id, id FROM book.cafetables WHERE sync_id IS NOT NULL";
                    my_cafe_rooms_sql = @"SELECT sync_id, id FROM book.CafeRooms WHERE sync_id IS NOT NULL";
                    my_cafe_comments_sql = @"SELECT sync_id, id FROM book.CafeComments WHERE sync_id IS NOT NULL";
                }

                Dictionary<int, int> my_customers = pm.GetDataManager().GetDictionary<int, int>(my_customers_sql);
                Dictionary<int, int> my_vendors = pm.GetDataManager().GetDictionary<int, int>(my_vendors_sql);
                Dictionary<int, int> my_terminals = pm.GetDataManager().GetDictionary<int, int>(my_terminals_sql); 
                Dictionary<int, int> my_creditbanks = pm.GetDataManager().GetDictionary<int, int>(my_creditbanks_sql);
                Dictionary<int, int> my_currencies = pm.GetDataManager().GetDictionary<int, int>(my_currencies_sql);
                Dictionary<int, int> my_users = pm.GetDataManager().GetDictionary<int, int>(my_users_sql);
                Dictionary<int, int> my_projects = pm.GetDataManager().GetDictionary<int, int>(my_projects_sql);
                Dictionary<int, int> my_staffs = pm.GetDataManager().GetDictionary<int, int>(my_staffs_sql);
                Dictionary<int, int> my_stores = pm.GetDataManager().GetDictionary<int, int>(my_stores_sql);
                Dictionary<int, int> my_cashes = pm.GetDataManager().GetDictionary<int, int>(my_cashes_sql);
                Dictionary<int, int> my_products = pm.GetDataManager().GetDictionary<int, int>(my_products_sql);
                Dictionary<int, int> my_units = pm.GetDataManager().GetDictionary<int, int>(my_units_sql);
                Dictionary<int, int> my_subcodes = pm.GetDataManager().GetDictionary<int, int>(my_subcodes_sql);
                Dictionary<int, int> my_analyticcodes = pm.GetDataManager().GetDictionary<int, int>(my_analyticcodes_sql);
                Dictionary<int, int> my_bankaccounts = pm.GetDataManager().GetDictionary<int, int>(my_bankaccounts_sql);
                Dictionary<int, int> my_prices = pm.GetDataManager().GetDictionary<int, int>(my_prices_sql);
                Dictionary<int, int> my_contragent_agreements = pm.GetDataManager().GetDictionary<int, int>(my_contragent_agreements_sql);
                Dictionary<int, int> my_credit_in = pm.GetDataManager().GetDictionary<int, int>(my_credit_in_sql);
                Dictionary<int, int> my_credit_out = pm.GetDataManager().GetDictionary<int, int>(my_credit_out_sql);
                Dictionary<int, int> my_group_staffdeps = pm.GetDataManager().GetDictionary<int, int>(my_group_staffdep_sql);
                Dictionary<int, int> my_staffpositions = pm.GetDataManager().GetDictionary<int, int>(my_staffposition_sql);
                Dictionary<int, int> my_cars = pm.GetDataManager().GetDictionary<int, int>(my_cars_sql);
                Dictionary<int, int> my_cafe_tables = pm.GetDataManager().GetDictionary<int, int>(my_cafe_tables_sql);
                Dictionary<int, int> my_cafe_rooms = pm.GetDataManager().GetDictionary<int, int>(my_cafe_rooms_sql);
                Dictionary<int, int> my_cafe_comments = pm.GetDataManager().GetDictionary<int, int>(my_cafe_comments_sql);
                Dictionary<int, int> my_partners = pm.GetDataManager().GetDictionary<int, int>(my_partners_sql);


                Func<DocItem, KeyValuePair<ImportBehavoir, DocItem>> ValidateDoc = (DocItem T) =>
                {
                    ImportBehavoir _st = ImportBehavoir.Available;

                    if (!_supported_docs.Contains(T.GeneralDocsItem.DocType.Value))
                        return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.UnsupportedDocType, T);
                    if (_existing.Contains(new Guid(T.GeneralDocsItem.Uid)))
                        return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.AlreadyExist, T);
                    if (!my_currencies.ContainsKey(T.GeneralDocsItem.CurrencyId.Value))
                        return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CurrencyUnavailable, T);
                    if (!my_users.ContainsKey(T.GeneralDocsItem.UserId.Value))
                        return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.UserUnavailable, T);
                    if (!my_projects.ContainsKey(T.GeneralDocsItem.ProjectId.Value))
                        return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.ProjectUnavailable, T);
                    if (T.GeneralDocsItem.StaffId > 0 && !my_staffs.ContainsKey(T.GeneralDocsItem.StaffId))
                        return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StaffUnavailable, T);
                    if (T.GeneralDocsItem.AnalyticCode > 0 && !my_analyticcodes.ContainsKey(T.GeneralDocsItem.AnalyticCode.Value))
                        return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.AnalyticCodeUnavailable, T);
                    if (T.GeneralDocsItem.ContragentId > 0 && !my_customers.ContainsKey(T.GeneralDocsItem.ContragentId.Value))
                        return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CustomerUnavailable, T);

                    //validate store1
                    if (_store_paramid1.Contains(T.GeneralDocsItem.DocType.Value) && T.GeneralDocsItem.ParamId1 != 0)
                    {
                        if (!my_stores.ContainsKey(T.GeneralDocsItem.ParamId1.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StoreUnavailable, T);
                        T.GeneralDocsItem.ParamId1 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId1 : my_stores[T.GeneralDocsItem.ParamId1.Value];
                    }

                    //validate store2
                    if (_store_paramid2.Contains(T.GeneralDocsItem.DocType.Value) && T.GeneralDocsItem.ParamId2!=0)
                    {
                        if (!my_stores.ContainsKey(T.GeneralDocsItem.ParamId2.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StoreUnavailable, T);
                        T.GeneralDocsItem.ParamId2 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId2 : my_stores[T.GeneralDocsItem.ParamId2.Value];
                    }

                    //validate store3
                    if (T.GeneralDocsItem.StoreId > 0 && !my_stores.ContainsKey(T.GeneralDocsItem.StoreId.Value))
                        return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StoreUnavailable, T);

                    //validate productsflow
                    if (T.ProductsFlowList != null && T.ProductsFlowList.Any())
                    {
                        foreach (ProductsFlow f in T.ProductsFlowList)
                        {
                            if (!my_stores.ContainsKey(f.StoreId.Value))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StoreUnavailable, T);
                            if (!my_products.ContainsKey(f.ProductId) || (f.ParentProductId > 0 && !my_products.ContainsKey(f.ParentProductId.Value)) || (f.ServiceProductId > 0 && !my_products.ContainsKey(f.ServiceProductId.Value)))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.ProductUnavailable, T);
                            if (f.UnitId > 0 && !my_units.ContainsKey(f.UnitId.Value))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.UnitUnavailable, T);
                            if (f.ServiceStaffId > 0 && !my_staffs.ContainsKey(f.ServiceStaffId.Value))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StaffUnavailable, T);
                            if (T.GeneralDocsItem.DocType != (int)DocTypes.CafeOrder)
                            {
                                if (f.VendorId > 0 && !my_vendors.ContainsKey(f.VendorId.Value))
                                    return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.VendorUnavailable, T);
                                if (f.SubId > 0 && !my_subcodes.ContainsKey(f.SubId.Value))
                                    return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.SubCodeUnavailable, T);
                            }
                           
                            if (impo.IsExternalUse)
                            {
                                f.StoreId = my_stores[f.StoreId.Value];
                                f.ProductId = my_products[f.ProductId];
                                f.ParentProductId = f.ParentProductId > 0 ? my_products[f.ParentProductId.Value] : f.ParentProductId;
                                f.ServiceProductId = f.ServiceProductId > 0 ? my_products[f.ServiceProductId.Value] : f.ServiceProductId;
                                f.UnitId = f.UnitId > 0 ? my_units[f.UnitId.Value] : f.UnitId;
                                f.ServiceStaffId = f.ServiceStaffId > 0 ? my_staffs[f.ServiceStaffId.Value] : f.ServiceStaffId;
                                if (T.GeneralDocsItem.DocType != (int)DocTypes.CafeOrder)
                                {
                                    f.VendorId = f.VendorId > 0 ? my_vendors[f.VendorId.Value] : f.VendorId;
                                    f.SubId = f.SubId > 0 ? my_subcodes[f.SubId.Value] : f.SubId;
                                }
                            }
                        }
                    }


                    //validate productinexprensies
                    if (T.ProductInExpensesList != null && T.ProductInExpensesList.Any())
                    {
                        foreach (ProductInExpenses ex in T.ProductInExpensesList)
                        {
                            if (!my_vendors.ContainsKey(ex.ContragentId.Value))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.VendorUnavailable, T);
                            if (!my_products.ContainsKey(ex.ServiceId.Value))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.ProductUnavailable, T);

                            if (impo.IsExternalUse)
                            {
                                ex.ContragentId = my_vendors[ex.ContragentId.Value];
                                ex.ServiceId = my_products[ex.ServiceId.Value];
                            }
                        }
                    }

                    //validate docoperation
                    if (T.OperationItem != null && T.OperationItem.PersonId > 0)
                    {
                        if (!my_staffs.ContainsKey(T.OperationItem.PersonId.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StaffUnavailable, T);
                        T.OperationItem.PersonId = !impo.IsExternalUse ? T.OperationItem.PersonId : my_staffs[T.OperationItem.PersonId.Value];
                    }



                    //validate cash1
                    if (_cashe_paramid1.Contains(T.GeneralDocsItem.DocType.Value) && T.GeneralDocsItem.ParamId1 > 0)
                    {
                        if (!my_cashes.ContainsKey(T.GeneralDocsItem.ParamId1.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CasheUnavailable, T);
                        T.GeneralDocsItem.ParamId1 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId1 : my_cashes[T.GeneralDocsItem.ParamId1.Value];
                    }

                    //validate cash2
                    if (_cashe_paramid2.Contains(T.GeneralDocsItem.DocType.Value) && T.GeneralDocsItem.ParamId2 > 0)
                    {
                        if (!my_cashes.ContainsKey(T.GeneralDocsItem.ParamId2.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CasheUnavailable, T);
                        T.GeneralDocsItem.ParamId2 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId2 : my_cashes[T.GeneralDocsItem.ParamId2.Value];
                    }

                    //validate responsible staff
                    if (_staff_paramid2.Contains(T.GeneralDocsItem.DocType.Value))
                    {
                        if (!my_staffs.ContainsKey(T.GeneralDocsItem.ParamId2.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StaffUnavailable, T);
                        T.GeneralDocsItem.ParamId2 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId2 : my_staffs[T.GeneralDocsItem.ParamId2.Value];
                    }


                    //validate POS
                    if (T.GeneralDocsItem.ParamId1 < -100000 && T.GeneralDocsItem.ParamId1 > -200000)
                    {
                        int tmp_id = -T.GeneralDocsItem.ParamId1.Value - 100000;
                        if (!my_terminals.ContainsKey(tmp_id))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.POSUnavailable, T);
                        T.GeneralDocsItem.ParamId1 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId1 : -100000 - my_terminals[tmp_id];
                    }

                    //validate bankaccount
                    if ((T.GeneralDocsItem.ParamId1 < 0 && T.GeneralDocsItem.ParamId1 > -100000) || (T.GeneralDocsItem.DocType == (int)DocTypes.CashBank && T.GeneralDocsItem.ParamId2 < 0 && T.GeneralDocsItem.ParamId2 > -100000))
                    {
                        int tmp_id = T.GeneralDocsItem.DocType != (int)DocTypes.CashBank ? -T.GeneralDocsItem.ParamId1.Value : -T.GeneralDocsItem.ParamId2.Value;
                        if (!my_bankaccounts.ContainsKey(tmp_id))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.BankAccountUnavailable, T);
                        if (T.GeneralDocsItem.DocType != (int)DocTypes.CashBank)
                            T.GeneralDocsItem.ParamId1 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId1 : -my_bankaccounts[tmp_id];
                        else
                            T.GeneralDocsItem.ParamId2 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId2 : -my_bankaccounts[tmp_id];
                    }

                    if (T.GeneralDocsItem.DocType == (int)DocTypes.BankConvert)
                    {
                        if (!my_bankaccounts.ContainsKey(T.ConvertingItem.AccountFrom.Value) || !my_bankaccounts.ContainsKey(T.ConvertingItem.AccountTo.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.BankAccountUnavailable, T);
                        if (impo.IsExternalUse)
                        {
                            T.ConvertingItem.AccountFrom = my_bankaccounts[T.ConvertingItem.AccountFrom.Value];
                            T.ConvertingItem.AccountTo = my_bankaccounts[T.ConvertingItem.AccountTo.Value];
                        }
                    }

                    if (T.GeneralDocsItem.ParamId1 < -200000)
                    {
                        int tmp_id = -T.GeneralDocsItem.ParamId1.Value - 200000;
                        if (!my_creditbanks.ContainsKey(tmp_id))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CreditBankUnavailable, T);
                        T.GeneralDocsItem.ParamId1 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId1 : -200000 - my_creditbanks[tmp_id];
                    }

                    //validate customer
                    if (_customer_paramid1.Contains(T.GeneralDocsItem.DocType.Value))
                    {
                        if (!my_customers.ContainsKey(T.GeneralDocsItem.ParamId1.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CustomerUnavailable, T);
                        T.GeneralDocsItem.ParamId1 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId1 : my_customers[T.GeneralDocsItem.ParamId1.Value];
                    }

                    if (_customer_paramid2.Contains(T.GeneralDocsItem.DocType.Value))
                    {
                        if (!my_customers.ContainsKey(T.GeneralDocsItem.ParamId2.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CustomerUnavailable, T);
                        T.GeneralDocsItem.ParamId2 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId2 : my_customers[T.GeneralDocsItem.ParamId2.Value];
                    }


                    //validate vendor
                    if (_vendor_paramid1.Contains(T.GeneralDocsItem.DocType.Value))
                    {
                        if (!my_vendors.ContainsKey(T.GeneralDocsItem.ParamId1.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.VendorUnavailable, T);
                        T.GeneralDocsItem.ParamId1 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId1 : my_vendors[T.GeneralDocsItem.ParamId1.Value];
                    }

                    if (_vendor_paramid2.Contains(T.GeneralDocsItem.DocType.Value))
                    {
                        if (!my_vendors.ContainsKey(T.GeneralDocsItem.ParamId2.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.VendorUnavailable, T);
                        T.GeneralDocsItem.ParamId2 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId2 : my_vendors[T.GeneralDocsItem.ParamId2.Value];
                    }

                    //validate partner
                    if (_partner_param_id2.Contains(T.GeneralDocsItem.DocType.Value))
                    {
                        if(!my_partners.ContainsKey(T.GeneralDocsItem.ParamId2.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.PartnerUnavailable, T);
                        T.GeneralDocsItem.ParamId2 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId2 : my_partners[T.GeneralDocsItem.ParamId2.Value];
                    }



                    //validate creditin
                    if (_credit_in_paramid2.Contains(T.GeneralDocsItem.DocType.Value))
                    {
                        if (!my_credit_in.ContainsKey(T.GeneralDocsItem.ParamId2.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CreditInUnavailable, T);
                        T.GeneralDocsItem.ParamId2 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId2 : my_credit_in[T.GeneralDocsItem.ParamId2.Value];
                    }

                    //validate creditout
                    if (_credit_out_paramid2.Contains(T.GeneralDocsItem.DocType.Value))
                    {
                        if (!my_credit_out.ContainsKey(T.GeneralDocsItem.ParamId2.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CreditOutUnavailable, T);
                        T.GeneralDocsItem.ParamId2 = !impo.IsExternalUse ? T.GeneralDocsItem.ParamId2 : my_credit_out[T.GeneralDocsItem.ParamId2.Value];
                    }

                    //validate productoutsingle
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.ProductOutSingle)
                    {
                        if (!my_prices.ContainsKey(T.ProductOutSingleItem.PriceType.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.PriceTypeUnavailable, T);
                        T.ProductOutSingleItem.PriceType = !impo.IsExternalUse ? T.ProductOutSingleItem.PriceType : my_prices[T.ProductOutSingleItem.PriceType.Value];
                    }

                    //validate productout
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.ProductOut)
                    {
                        if (T.ProductOutItem.AgreementId > 0 && !my_contragent_agreements.ContainsKey(T.ProductOutItem.AgreementId.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.ContragentAgreementUnavailable, T);

                        if (T.ProductOutItem.InvoiceBankId > 0 && !my_bankaccounts.ContainsKey(T.ProductOutItem.InvoiceBankId.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.BankAccountUnavailable, T);

                        if (!my_prices.ContainsKey(T.ProductOutItem.PriceType.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.PriceTypeUnavailable, T);

                        if (impo.IsExternalUse)
                        {
                            T.ProductOutItem.AgreementId = T.ProductOutItem.AgreementId > 0 ? my_contragent_agreements[T.ProductOutItem.AgreementId.Value] : T.ProductOutItem.AgreementId;
                            T.ProductOutItem.InvoiceBankId = T.ProductOutItem.InvoiceBankId > 0 ? my_bankaccounts[T.ProductOutItem.InvoiceBankId.Value] : T.ProductOutItem.InvoiceBankId;
                            T.ProductOutItem.PriceType = my_prices[T.ProductOutItem.PriceType.Value];
                        }
                    }
                    //validate customerorder
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.CustomerOrder)
                    {
                        if(T.CustomerOrderItem.AgreementId>0 && !my_contragent_agreements.ContainsKey(T.CustomerOrderItem.AgreementId.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.ContragentAgreementUnavailable, T);

                        if (T.CustomerOrderItem.InvoiceBankId > 0 && !my_bankaccounts.ContainsKey(T.CustomerOrderItem.InvoiceBankId.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.BankAccountUnavailable, T);

                        if (!my_prices.ContainsKey(T.CustomerOrderItem.PriceType.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.PriceTypeUnavailable, T);

                        if (impo.IsExternalUse)
                        {
                            T.CustomerOrderItem.AgreementId = T.CustomerOrderItem.AgreementId > 0 ? my_contragent_agreements[T.CustomerOrderItem.AgreementId.Value] : T.CustomerOrderItem.AgreementId;
                            T.CustomerOrderItem.InvoiceBankId = T.CustomerOrderItem.InvoiceBankId > 0 ? my_bankaccounts[T.CustomerOrderItem.InvoiceBankId.Value] : T.CustomerOrderItem.InvoiceBankId;
                            T.CustomerOrderItem.PriceType = my_prices[T.CustomerOrderItem.PriceType.Value];
                        }

                    }


                    //validate banktransfer
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.BankTransfer)
                    {
                        if (!my_bankaccounts.ContainsKey(T.BankTransferItem.AccountFrom.Value) || !my_bankaccounts.ContainsKey(T.BankTransferItem.AccountTo.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.BankAccountUnavailable, T);
                        if (impo.IsExternalUse)
                        {
                            T.BankTransferItem.AccountFrom = my_bankaccounts[T.BankTransferItem.AccountFrom.Value];
                            T.BankTransferItem.AccountTo = my_bankaccounts[T.BankTransferItem.AccountTo.Value];
                        }
                    }

                    //validate cashconvert
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.CashConvert)
                    {
                        if(!my_currencies.ContainsKey(T.CashConvertingItem.AccountFrom.Value) || !my_currencies.ContainsKey(T.CashConvertingItem.AccountTo.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CurrencyUnavailable, T);
                        if (impo.IsExternalUse)
                        {
                            T.CashConvertingItem.AccountFrom = my_currencies[T.CashConvertingItem.AccountFrom.Value];
                            T.CashConvertingItem.AccountTo = my_currencies[T.CashConvertingItem.AccountTo.Value];
                        }
                    }

                    //validate bankconvert
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.BankConvert)
                    {
                        if (!my_bankaccounts.ContainsKey(T.ConvertingItem.AccountFrom.Value) || !my_bankaccounts.ContainsKey(T.ConvertingItem.AccountTo.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.BankAccountUnavailable, T);
                        if (impo.IsExternalUse)
                        {
                            T.ConvertingItem.AccountFrom = my_bankaccounts[T.ConvertingItem.AccountFrom.Value];
                            T.ConvertingItem.AccountTo = my_bankaccounts[T.ConvertingItem.AccountTo.Value];
                        }
                    }

                    //validate staffpaysalary
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.StaffPaySalary)
                    {
                        if(!my_group_staffdeps.ContainsKey(T.StaffPaySalaryItem.DepartmentId))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.GroupStaffDepUnavailable, T);
                        T.StaffPaySalaryItem.DepartmentId = !impo.IsExternalUse ? T.StaffPaySalaryItem.DepartmentId : my_group_staffdeps[T.StaffPaySalaryItem.DepartmentId];
                        foreach (StaffPaySalaryFlow ssf in T.StaffPaySalaryFlowList)
                        {
                            if (ssf.PositionId > 0 && !my_staffpositions.ContainsKey(ssf.PositionId.Value))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StaffPositionUnavailable, T);
                            if(!my_staffs.ContainsKey(ssf.StaffId.Value))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StaffUnavailable, T);
                            if (impo.IsExternalUse)
                            {
                                ssf.PositionId = ssf.PositionId > 0 ? my_staffpositions[ssf.PositionId.Value] : ssf.PositionId;
                                ssf.StaffId = my_staffs[ssf.StaffId.Value];
                            }
                        }
                    }

                    //validate fuel
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.Fuel)
                    {
                        if(!my_cars.ContainsKey(T.GeneralDocsItem.ParamId1.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CarUnavailable, T);
                        if(!my_products.ContainsKey(T.GeneralDocsItem.ParamId2.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.ProductUnavailable, T);
                        if (impo.IsExternalUse)
                        {
                            T.GeneralDocsItem.ParamId1 = my_cars[T.GeneralDocsItem.ParamId1.Value];
                            T.GeneralDocsItem.ParamId2 = my_products[T.GeneralDocsItem.ParamId2.Value];
                        }
                    }

                    //validate cafe
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.CafeOrder)
                    {
                        if (T.CafeOrderItem.TableId > 0 && !my_cafe_tables.ContainsKey(T.CafeOrderItem.TableId.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CafeTableUnavailable, T);
                        if (T.CafeOrderItem.WaiterId > 0 && !my_staffs.ContainsKey(T.CafeOrderItem.WaiterId.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StaffUnavailable, T);
                        if (T.CafeOrderItem.RoomId > 0 && !my_cafe_rooms.ContainsKey(T.CafeOrderItem.RoomId.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CafeRoomUnavailable, T);
                        if (T.CafeOrderItem.CommentId > 0 && !my_cafe_comments.ContainsKey(T.CafeOrderItem.CommentId.Value))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CafeCommentUnavailable, T);
                        if (T.CafeOrderItem.StaffFoodId > 0 && !my_staffs.ContainsKey(T.CafeOrderItem.StaffFoodId))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StaffUnavailable, T);
                        if (T.CafeOrderItem.PriceTypeId > 0 && !my_prices.ContainsKey(T.CafeOrderItem.PriceTypeId))
                            return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.PriceTypeUnavailable, T);

                        if (impo.IsExternalUse)
                        {
                            T.CafeOrderItem.TableId = T.CafeOrderItem.TableId > 0 ? my_cafe_tables[T.CafeOrderItem.TableId.Value] : T.CafeOrderItem.TableId;
                            T.CafeOrderItem.WaiterId = T.CafeOrderItem.WaiterId > 0 ? my_staffs[T.CafeOrderItem.WaiterId.Value] : T.CafeOrderItem.WaiterId;
                            T.CafeOrderItem.RoomId = T.CafeOrderItem.RoomId > 0 ? my_cafe_rooms[T.CafeOrderItem.RoomId.Value] : T.CafeOrderItem.RoomId;
                            T.CafeOrderItem.CommentId = T.CafeOrderItem.CommentId > 0 ? my_cafe_comments[T.CafeOrderItem.CommentId.Value] : T.CafeOrderItem.CommentId;
                            T.CafeOrderItem.StaffFoodId = T.CafeOrderItem.StaffFoodId > 0 ? my_staffs[T.CafeOrderItem.StaffFoodId] : T.CafeOrderItem.StaffFoodId;
                            T.CafeOrderItem.PriceTypeId = T.CafeOrderItem.PriceTypeId > 0 ? my_prices[T.CafeOrderItem.PriceTypeId] : T.CafeOrderItem.PriceTypeId;
                        }

                        if (T.ProductsFlowCafeList != null)
                        {
                            foreach (ProductsFlowCafe pf in T.ProductsFlowCafeList)
                            {
                                if (!my_products.ContainsKey(pf.ProductId.Value))
                                    return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.ProductUnavailable, T);
                                pf.ProductId = !impo.IsExternalUse ? pf.ProductId : my_products[pf.ProductId.Value];
                            }
                        }

                        if (T.CafeMiniFlowList != null)
                        {
                            foreach (CafeMiniFlow pf in T.CafeMiniFlowList)
                            {
                                if ((pf.ProductId > 0 && !my_products.ContainsKey(pf.ProductId.Value)) || (pf.SubProductId > 0 && !my_products.ContainsKey(pf.SubProductId.Value)))
                                    return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.ProductUnavailable, T);
                                if ((pf.UnitId > 0 && !my_units.ContainsKey(pf.UnitId.Value)) || (pf.SubUnitId > 0 && !my_units.ContainsKey(pf.SubUnitId.Value)))
                                    return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.UnitUnavailable, T);
                                if (pf.StoreId > 0 && !my_stores.ContainsKey(pf.StoreId.Value))
                                    return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.StoreUnavailable, T);

                                if (impo.IsExternalUse)
                                {
                                    pf.ProductId = pf.ProductId > 0 ? my_products[pf.ProductId.Value] : pf.ProductId;
                                    pf.SubProductId = pf.SubProductId > 0 ? my_products[pf.SubProductId.Value] : pf.SubProductId;
                                    pf.UnitId = pf.UnitId > 0 ? my_units[pf.UnitId.Value] : pf.UnitId;
                                    pf.SubUnitId = pf.SubUnitId > 0 ? my_units[pf.SubUnitId.Value] : pf.SubUnitId;
                                    pf.StoreId = pf.StoreId > 0 ? my_stores[pf.StoreId.Value] : pf.StoreId;
                                }
                            }
                        }
                    }




                    //validate manual entry
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.SingleEntry)
                    {
                        if (new string[] { "1110", "1120" }.Contains(T.SingleEntryItem.DebitAcc))
                            if (!my_cashes.ContainsKey(T.SingleEntryItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CasheUnavailable, T);
                            else T.SingleEntryItem.A1 = !impo.IsExternalUse ? T.SingleEntryItem.A1 : my_cashes[T.SingleEntryItem.A1];

                        else if (new string[] { "1210", "1220" }.Contains(T.SingleEntryItem.DebitAcc))
                            if (!my_bankaccounts.ContainsKey(T.SingleEntryItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.BankAccountUnavailable, T);
                            else T.SingleEntryItem.A1 = !impo.IsExternalUse ? T.SingleEntryItem.A1 : my_bankaccounts[T.SingleEntryItem.A1];

                        else if (new string[] { "1410", "3120" }.Contains(T.SingleEntryItem.DebitAcc))
                            if (!my_customers.ContainsKey(T.SingleEntryItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CustomerUnavailable, T);
                            else T.SingleEntryItem.A1 = !impo.IsExternalUse ? T.SingleEntryItem.A1 : my_customers[T.SingleEntryItem.A1];

                        else if (new string[] { "3110", "1480" }.Contains(T.SingleEntryItem.DebitAcc))
                            if (!my_vendors.ContainsKey(T.SingleEntryItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.VendorUnavailable, T);
                            else T.SingleEntryItem.A1 = !impo.IsExternalUse ? T.SingleEntryItem.A1 : my_vendors[T.SingleEntryItem.A1];

                        else if (T.SingleEntryItem.DebitAcc == "1411")
                            if (!my_terminals.ContainsKey(T.SingleEntryItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.POSUnavailable, T);
                            else T.SingleEntryItem.A1 = !impo.IsExternalUse ? T.SingleEntryItem.A1 : my_terminals[T.SingleEntryItem.A1];

                        else if (T.SingleEntryItem.DebitAcc == "1412")
                            if (!my_creditbanks.ContainsKey(T.SingleEntryItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CreditBankUnavailable, T);
                            else T.SingleEntryItem.A1 = !impo.IsExternalUse ? T.SingleEntryItem.A1 : my_creditbanks[T.SingleEntryItem.A1];



                        if (new string[] { "1110", "1120" }.Contains(T.SingleEntryItem.CreditAcc))
                            if (!my_cashes.ContainsKey(T.SingleEntryItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CasheUnavailable, T);
                            else T.SingleEntryItem.B1 = !impo.IsExternalUse ? T.SingleEntryItem.B1 : my_cashes[T.SingleEntryItem.B1];

                        else if (new string[] { "1210", "1220" }.Contains(T.SingleEntryItem.CreditAcc))
                            if (!my_bankaccounts.ContainsKey(T.SingleEntryItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.BankAccountUnavailable, T);
                            else T.SingleEntryItem.B1 = !impo.IsExternalUse ? T.SingleEntryItem.B1 : my_bankaccounts[T.SingleEntryItem.B1];

                        else if (new string[] { "1410", "3120" }.Contains(T.SingleEntryItem.CreditAcc))
                            if (!my_customers.ContainsKey(T.SingleEntryItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CustomerUnavailable, T);
                            else T.SingleEntryItem.B1 = !impo.IsExternalUse ? T.SingleEntryItem.B1 : my_customers[T.SingleEntryItem.B1];

                        else if (new string[] { "3110", "1480" }.Contains(T.SingleEntryItem.CreditAcc))
                            if (!my_vendors.ContainsKey(T.SingleEntryItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.VendorUnavailable, T);
                            else T.SingleEntryItem.B1 = !impo.IsExternalUse ? T.SingleEntryItem.B1 : my_vendors[T.SingleEntryItem.B1];

                        else if (T.SingleEntryItem.CreditAcc == "1411")
                            if (!my_terminals.ContainsKey(T.SingleEntryItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.POSUnavailable, T);
                            else T.SingleEntryItem.B1 = !impo.IsExternalUse ? T.SingleEntryItem.B1 : my_terminals[T.SingleEntryItem.B1];

                        else if (T.SingleEntryItem.CreditAcc == "1412")
                            if (!my_creditbanks.ContainsKey(T.SingleEntryItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CreditBankUnavailable, T);
                            else T.SingleEntryItem.B1 = !impo.IsExternalUse ? T.SingleEntryItem.B1 : my_creditbanks[T.SingleEntryItem.B1];
                    }

                    //validate remainder entry
                    if (T.GeneralDocsItem.DocType == (int)DocTypes.Remainder)
                    {
                        if (new string[] { "1110", "1120" }.Contains(T.RemainderItem.DebitAcc))
                            if (!my_cashes.ContainsKey(T.RemainderItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CasheUnavailable, T);
                            else T.RemainderItem.A1 = !impo.IsExternalUse ? T.RemainderItem.A1 : my_cashes[T.RemainderItem.A1];

                        else if (new string[] { "1210", "1220" }.Contains(T.RemainderItem.DebitAcc))
                            if (!my_bankaccounts.ContainsKey(T.RemainderItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.BankAccountUnavailable, T);
                            else T.RemainderItem.A1 = !impo.IsExternalUse ? T.RemainderItem.A1 : my_bankaccounts[T.RemainderItem.A1];

                        else if (new string[] { "1410", "3120" }.Contains(T.RemainderItem.DebitAcc))
                            if (!my_customers.ContainsKey(T.RemainderItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CustomerUnavailable, T);
                            else T.RemainderItem.A1 = !impo.IsExternalUse ? T.RemainderItem.A1 : my_customers[T.RemainderItem.A1];

                        else if (new string[] { "3110", "1480" }.Contains(T.RemainderItem.DebitAcc))
                            if (!my_vendors.ContainsKey(T.RemainderItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.VendorUnavailable, T);
                            else T.RemainderItem.A1 = !impo.IsExternalUse ? T.RemainderItem.A1 : my_vendors[T.RemainderItem.A1];

                        else if (T.RemainderItem.DebitAcc == "1411")
                            if (!my_terminals.ContainsKey(T.RemainderItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.POSUnavailable, T);
                            else T.RemainderItem.A1 = !impo.IsExternalUse ? T.RemainderItem.A1 : my_terminals[T.RemainderItem.A1];

                        else if (T.RemainderItem.DebitAcc == "1412")
                            if (!my_creditbanks.ContainsKey(T.RemainderItem.A1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CreditBankUnavailable, T);
                            else T.RemainderItem.A1 = !impo.IsExternalUse ? T.RemainderItem.A1 : my_creditbanks[T.RemainderItem.A1];



                        if (new string[] { "1110", "1120" }.Contains(T.RemainderItem.CreditAcc))
                            if (!my_cashes.ContainsKey(T.RemainderItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CasheUnavailable, T);
                            else T.RemainderItem.B1 = !impo.IsExternalUse ? T.RemainderItem.B1 : my_cashes[T.RemainderItem.B1];

                        else if (new string[] { "1210", "1220" }.Contains(T.RemainderItem.CreditAcc))
                            if (!my_bankaccounts.ContainsKey(T.RemainderItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.BankAccountUnavailable, T);
                            else T.RemainderItem.B1 = !impo.IsExternalUse ? T.RemainderItem.B1 : my_bankaccounts[T.RemainderItem.B1];

                        else if (new string[] { "1410", "3120" }.Contains(T.RemainderItem.CreditAcc))
                            if (!my_customers.ContainsKey(T.RemainderItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CustomerUnavailable, T);
                            else T.RemainderItem.B1 = !impo.IsExternalUse ? T.RemainderItem.B1 : my_customers[T.RemainderItem.B1];

                        else if (new string[] { "3110", "1480" }.Contains(T.RemainderItem.CreditAcc))
                            if (!my_vendors.ContainsKey(T.RemainderItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.VendorUnavailable, T);
                            else T.RemainderItem.B1 = !impo.IsExternalUse ? T.RemainderItem.B1 : my_vendors[T.RemainderItem.B1];

                        else if (T.RemainderItem.CreditAcc == "1411")
                            if (!my_terminals.ContainsKey(T.RemainderItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.POSUnavailable, T);
                            else T.RemainderItem.B1 = !impo.IsExternalUse ? T.RemainderItem.B1 : my_terminals[T.RemainderItem.B1];

                        else if (T.RemainderItem.CreditAcc == "1412")
                            if (!my_creditbanks.ContainsKey(T.RemainderItem.B1))
                                return new KeyValuePair<ImportBehavoir, DocItem>(ImportBehavoir.CreditBankUnavailable, T);
                            else T.RemainderItem.B1 = !impo.IsExternalUse ? T.RemainderItem.B1 : my_creditbanks[T.RemainderItem.B1];
                    }

                    if (impo.IsExternalUse)
                    {
                        T.GeneralDocsItem.CurrencyId = my_currencies[T.GeneralDocsItem.CurrencyId.Value];
                        T.GeneralDocsItem.UserId = my_users[T.GeneralDocsItem.UserId.Value];
                        T.GeneralDocsItem.ProjectId = my_projects[T.GeneralDocsItem.ProjectId.Value];
                        T.GeneralDocsItem.StaffId = T.GeneralDocsItem.StaffId > 0 ? my_staffs[T.GeneralDocsItem.StaffId] : T.GeneralDocsItem.StaffId;
                        T.GeneralDocsItem.StoreId = T.GeneralDocsItem.StoreId > 0 ? my_stores[T.GeneralDocsItem.StoreId.Value] : T.GeneralDocsItem.StoreId;
                        T.GeneralDocsItem.AnalyticCode = T.GeneralDocsItem.AnalyticCode > 0 ? my_analyticcodes[T.GeneralDocsItem.AnalyticCode.Value] : T.GeneralDocsItem.AnalyticCode;
                        T.GeneralDocsItem.ContragentId = T.GeneralDocsItem.ContragentId > 0 ? my_customers[T.GeneralDocsItem.ContragentId.Value] : T.GeneralDocsItem.ContragentId;
                    }

                    return new KeyValuePair<ImportBehavoir, DocItem>(_st, T);
                };

                int i = 1;
                impo.Docs.ForEach(d =>
                {
                    decimal per = (decimal)i / (decimal)impo.Docs.Count * 100M;
                    ProgressDispatcher.Percent = ((int)per);
                    i++;

                    int index = m_Grid.Rows.Add(false, d.GeneralDocsItem.Tdate, _doctypes[d.GeneralDocsItem.DocType.Value], d.GeneralDocsItem.Purpose, d.GeneralDocsItem.Amount);

                    var validated = ValidateDoc(d);
                    m_Grid.Rows[index].Cells[ColStatusId.Index].Value = validated.Key;
                    m_Grid.Rows[index].Cells[ColStatus.Index].Value = EnumEx.GetEnumDescription(validated.Key);
                    m_Grid.Rows[index].Cells[ColUid.Index].Value = new Guid(d.GeneralDocsItem.Uid);
                    m_Grid.Rows[index].Cells[ColInside.Index].Value = validated.Value;
                    m_Grid.Rows[index].Cells[ColCheck.Index].ReadOnly = validated.Key != ImportBehavoir.Available;
                });


            }
            m_Grid.EndEdit();
            ProgressDispatcher.Deactivate();
        }

        private void OnExecuteImport()
        {
            lblResult.Text = "შედეგი:";
            m_Grid.EndEdit();
            if (m_Grid.Rows.Count == 0)
                return;
            var ready_rows = m_Grid.Rows.OfType<DataGridViewRow>().Where(r => Convert.ToBoolean(r.Cells[ColCheck.Index].Value)).ToList();
            if (!ready_rows.Any())
                return;
            ProgressDispatcher.Activate(true);
            pm.InitialiseLog(new List<string> { "თარიღი", "ოპერაცია", "საფუძველი", "თანხა", "შედეგი" });
            List<DocItem> _docs = ready_rows.Select(g => (DocItem)g.Cells[ColInside.Index].Value).ToList();
            List<int> _types = _docs.Select(g => g.GeneralDocsItem.DocType.Value).Distinct().ToList();
            List<string> _prefix = _docs.Select(g => g.GeneralDocsItem.DocNumPrefix).Distinct().ToList();
            Dictionary<Guid, KeyValuePair<ImportBehavoir, string>> result = new Dictionary<Guid, KeyValuePair<ImportBehavoir, string>>();
            Stopwatch _watch = new Stopwatch();
            using (BusinesContext _bc = new BusinesContext())
            {
                Dictionary<KeyValuePair<int, string>, long> _nums = _bc.GenerateDocNums(_types, _prefix);
                Action<List<DocItem>> SaveDocGroup = (List<DocItem> T) =>
                {
                    int ref_id = 0;
                    foreach (DocItem d in T)
                    {
                        _watch.Reset();
                        int cur_id = 0;
                        int doc_type = d.GeneralDocsItem.DocType.Value;
                        KeyValuePair<int, string> num_key = new KeyValuePair<int, string>(doc_type, d.GeneralDocsItem.DocNumPrefix);
                        if (!_nums.ContainsKey(num_key))
                            _nums.Add(num_key, 1);
                        d.GeneralDocsItem.DocNum = _nums[num_key];
                        Guid current_uid = new Guid(d.GeneralDocsItem.Uid);
                        _watch.Start();
                        try
                        {
                            switch ((DocTypes)doc_type)
                            {
                                case DocTypes.CustomerOrder:
                                    {
                                        cur_id = _bc.SaveCustomerOrder(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.CustomerReturn:
                                    {
                                        cur_id = _bc.SaveCustomerReturn(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }

                                case DocTypes.StoreOrder:
                                    {
                                        cur_id = _bc.SaveStoreOrder(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.ProductIn:
                                    {
                                        cur_id = _bc.SaveProductIn(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.ServiceIn:
                                    {
                                        cur_id = _bc.SaveServiceIn(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.ProductOutSingle:
                                    {
                                        cur_id = _bc.SaveProductOutSingle(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.ProductOut:
                                case DocTypes.ServiceOut:
                                    {
                                        cur_id = _bc.SaveProductOut(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.ProductMove:
                                    {
                                        cur_id = _bc.SaveProductMove(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.ProductStart:
                                    {
                                        cur_id = _bc.SaveProductStart(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.ProductCancel:
                                    {
                                        cur_id = _bc.SaveProductCancel(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.BankTransfer:
                                    {
                                        cur_id = _bc.SaveBankTransfer(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.CashConvert:
                                    {
                                        cur_id = _bc.SaveCashConvert(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.BankConvert:
                                    {
                                        cur_id = _bc.SaveBankConvert(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.StaffPaySalary:
                                    {
                                        cur_id = _bc.SaveStaffPaySalary(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.VendorOrder:
                                    {
                                        cur_id = _bc.SaveVendorOrder(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.VendorReturn:
                                    {
                                        cur_id = _bc.SaveVendorReturn(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.Fuel:
                                    {
                                        cur_id = _bc.SaveFuel(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.CafeOrder:
                                    {
                                        cur_id = _bc.SaveCafeOrder(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.Production:
                                    {
                                        cur_id = _bc.SaveProduction(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.StaffVacation:
                                    {
                                        cur_id = _bc.SaveStaffVacation(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.ProductShipping:
                                    {
                                        cur_id = _bc.SaveProductShipping(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        ref_id = cur_id;
                                        break;
                                    }
                                case DocTypes.CustomerMoneyIn:
                                case DocTypes.VendorMoneyOut:
                                case DocTypes.CustomerAdvance:
                                case DocTypes.VendorAdvance:
                                case DocTypes.CustomerMoneyOut:
                                case DocTypes.VendorMoneyReturn:
                                case DocTypes.ResponsibleMoneyOut:
                                case DocTypes.ResponsibleMoneyIn:
                                case DocTypes.ResponsibleExpense:
                                case DocTypes.BankCash:
                                case DocTypes.CashBank:
                                case DocTypes.CashCash:
                                case DocTypes.BankComission:
                                case DocTypes.Tax:
                                case DocTypes.GetCredit:
                                case DocTypes.SetCreditPercent:
                                case DocTypes.PayCreditPercent:
                                case DocTypes.PayCredit:
                                case DocTypes.GetOutCredit:
                                case DocTypes.SetOutCreditPercent:
                                case DocTypes.PayOutCreditPercent:
                                case DocTypes.PayOutCredit:
                                case DocTypes.StaffAdvance:
                                case DocTypes.MoneyFromStaff:
                                case DocTypes.Salary:
                                case DocTypes.SetDividend:
                                case DocTypes.PayDividend:
                                case DocTypes.BusinessTrip:
                                case DocTypes.VendorTax:
                                    {
                                        d.GeneralDocsItem.RefId = ref_id;
                                        cur_id = _bc.SaveDocOperation(d);
                                        if (cur_id <= 0)
                                        {
                                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, _bc.ErrorEx));
                                            continue;
                                        }
                                        break;
                                    }
                            }
                            if (_nums.ContainsKey(num_key))
                                _nums[num_key] += 1;

                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.SuccessfullyImported, string.Format("({0:00}.{1:00} sec)", _watch.Elapsed.TotalSeconds, _watch.Elapsed.Milliseconds)));
                        }
                        catch (Exception ex)
                        {
                            result.Add(current_uid, new KeyValuePair<ImportBehavoir, string>(ImportBehavoir.Faild, string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace)));
                        }
                    }
                };

                List<List<DocItem>> grouped = _docs.OrderBy(a => a.GeneralDocsItem.Tdate).GroupBy(a => a.GroupId).Select(s => s.ToList()).ToList();

                int i = 1;
                grouped.ForEach(items =>
                {
                    decimal per = (decimal)i / (decimal)grouped.Count * 100M;
                    ProgressDispatcher.Percent = ((int)per);
                    i++;
                    List<DocItem> ii = items.OrderBy(a => a.OrderId).ThenBy(a => a.GeneralDocsItem.Tdate).ToList();
                    SaveDocGroup(ii);
                });
                if (_docs.Where(d => d.ProductsFlowList != null).Any())
                    _bc.RecalculateRestAmounts(null, null);
            }
            int ready_cnt = 0;
            bool need_show_log = false;
            ready_rows.ForEach(r =>
            {
                var sin = result[(Guid)r.Cells[ColUid.Index].Value];
                r.Cells[ColStatusId.Index].Value = sin.Key;
                r.Cells[ColStatus.Index].Value = string.Format("{0} {1}", EnumEx.GetEnumDescription(sin.Key), sin.Value);
                if (sin.Key == ImportBehavoir.SuccessfullyImported)
                {
                    r.Cells[ColCheck.Index].ReadOnly = true;
                    ready_cnt++;
                }
                else
                {
                    pm.AddLogFormItem(new List<string> { Convert.ToDateTime(r.Cells[ColTdate.Index].Value).ToString("dd/MM/yyyy HH:mm:ss"), r.Cells[ColOperation.Index].Value.ToString(), r.Cells[ColPurpose.Index].Value.ToString(), r.Cells[ColAmount.Index].Value.ToString(), r.Cells[ColStatus.Index].Value.ToString() }, 0);
                    need_show_log = true;
                }
                r.Cells[ColCheck.Index].Value = false;
            });
            lblResult.Text = string.Format("წარმატებით ჩაიტვირთა {0} ოპერაცია {1} დან.", ready_cnt, ready_rows.Count);
            ProgressDispatcher.Deactivate();
            if (need_show_log)
                pm.ShowLogForm();
        }

        private void selectIem_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0 )
                return;
            m_Grid.EndEdit();
            foreach (DataGridViewRow rw in m_Grid.Rows)
            {
                if (((ImportBehavoir)rw.Cells[ColStatusId.Index].Value) != ImportBehavoir.Available)
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

        private void btnPreviewToExcel_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0)
                return;
            ProgressDispatcher.Activate();
            new ExcelManager(pm, m_Grid, string.Empty).OnExcelPreviewFast();
            ProgressDispatcher.Deactivate();
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0)
                return;
            using (SaveFileDialog savedialog = new SaveFileDialog() { FileName = "Export Data", Filter = "ექსელის ფაილი" + " (*.xlsx)|*.xlsx" })
            {
                if (savedialog.ShowDialog() != DialogResult.OK)
                    return;
                ProgressDispatcher.Activate();
                new ExcelManager(pm, m_Grid, string.Empty).OnExcelExportFast(savedialog.FileName);
                ProgressDispatcher.Deactivate();
            }
        }
    }





   




}
