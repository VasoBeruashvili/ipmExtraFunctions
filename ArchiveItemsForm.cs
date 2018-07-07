using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using ipmPMBasic;
using ipmBLogic;
using ipmControls;
using ipmPMBasic.Models;
using SerTimmer = System.Timers;
using System.Diagnostics;
using System.Windows.Threading;

namespace ipmExtraFunctions
{
    public partial class ArchiveItemsForm : Form
    {

        ProgramManagerBasic pm;
        
        public ArchiveItemsForm(ProgramManagerBasic pm)
        {
            InitializeComponent();
            this.pm = pm;
            DataTable data = new DataTable();
            data.Columns.Add("id");
            data.Columns.Add("name");
            data.Rows.Add("1", this.pm.GetTranslatorManager().Translate("საცალო გაყიდვები"));
            if (pm.GetConfigParamValue("Cafe")=="1")
            {
                data.Rows.Add("2",  this.pm.GetTranslatorManager().Translate("თანხის მიღება"));
                data.Rows.Add("3",  this.pm.GetTranslatorManager().Translate("კაფეს შეკვეთები"));
            }


            comboOperation.DataSource = data;
            comboOperation.DisplayMember = "name";
            comboOperation.ValueMember = "id";
           
            
        }

        //private SerTimmer.Timer ElapsTimer = new SerTimmer.Timer(500);
        //Stopwatch stopWatch = new Stopwatch();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GetProgramManager().TranslateControl(this);
            m_Picker.ProgramManager = GetProgramManager();
            m_Picker.translate();

            //ElapsTimer.Elapsed += (s, o) =>
            //{
            //    TimeSpan ts = stopWatch.Elapsed;
            //    if (InvokeRequired)
            //        lblElapsedTime.Invoke(new Action(
            //                              delegate ()
            //                              {
            //                                  lblElapsedTime.Text = string.Format("გასული დრო {0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            //                                  lblElapsedTime.Refresh();
            //                                  Application.DoEvents();

            //                              }));
            //    else
            //    {
            //        Application.DoEvents();
            //        lblElapsedTime.Text = string.Format("გასული დრო {0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            //        lblElapsedTime.Refresh();
            //        Application.DoEvents();
            //    }
            //};
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return pm;
        }
        private bool _stop = false;

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateControl(true);

            bool res = false;
            int type= int.Parse(comboOperation.SelectedValue.ToString());
            if (type == 1)
            {
                res = OnExecuteProductOutSingle();
                GetProgramManager().GetDataManager().RecalculateRestAmounts(null, null);
            }
            else if (type == 2)
                res = OnExecuteCashIn();
            else if (type == 3)
                res = OnExecuteCafeOrder();
            else
                return;
            string msg = "";
            if (res)
            {
                if (!_stop)
                    msg = "არქივაციის ოპერაცია წარმატებით შესრულდა";
                else
                    msg = "არქივაციის ოპერაცია შეჩერებულია მომხმარებლის მიერ!";
            }
            else
                msg = "არქივაციის ოპერაცია ვერ შესრულდა!";
            MessageBox.Show(pm.GetTranslatorManager().Translate(msg), pm.GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.OK, MessageBoxIcon.Information);
            _stop = false;
            UpdateControl(false);
        }
        private bool OnExecuteCafeOrder()
        {
            BusinessLogic bLogic = new BusinessLogic();
            bLogic.Initialize(GetProgramManager().GetDataManager());
            DateTime date1 = m_Picker.dtp_From.Value;
            DateTime date2 = m_Picker.dtp_To.Value;
            int user_id = GetProgramManager().GetUserID();
            bool auto_delete = true;// checkAutoDelete.Checked;
            DateTime dt = date1;
            string sql = "";
            while (dt <= date2)
            {
                double vat_value = 0;
                if (GetProgramManager().GetDataManager().IsCompanyVAT(dt))
                    vat_value = GetProgramManager().GetDataManager().GetVatPercent();

                string d1 = dt.ToString("yyyy-MM-dd 00:00:01");
                string d2 = dt.ToString("yyyy-MM-dd 23:59:59");




                sql = "SELECT gd.param_id1, gd.param_id2, c.service_type, c.service_perc, c.service_amount, c.staff_food_id FROM doc.GeneralDocs gd INNER JOIN doc.CafeOrders c ON c.general_id = gd.id " +
                    " WHERE gd.doc_type=66 AND gd.param_id2=0 AND gd.tdate BETWEEN '" + d1 + "' AND '" + d2 + "' AND ISNULL(gd.is_packed,0)=0 AND gd.status_id=2 "+
                    " GROUP BY gd.param_id1, gd.param_id2, c.service_type, c.service_perc, c.service_amount, c.staff_food_id ORDER BY gd.param_id1 ";
                DataTable group_data = GetProgramManager().GetDataManager().GetTableData(sql);

                if (group_data != null && group_data.Rows.Count > 0)
                {
                    
                    foreach (DataRow row in group_data.Rows)
                    {
                        int store_id = int.Parse(row["param_id1"].ToString());
                        int customer_id = int.Parse(row["param_id2"].ToString());
                        double service_perc = double.Parse(row["service_perc"].ToString());
                        int _staffFoodID = row.Field<int>("staff_food_id");
                        decimal _serviceAmount = Convert.ToDecimal(row["service_amount"]);
                        int service_type = int.Parse(row["service_type"].ToString());
                        string purpose = "დაჯგუფებული კაფეს შეკვეთა " + GetProgramManager().GetDataManager().GetStoreNameByID(store_id);

                        sql = "SELECT  pf.product_id, pf.unit_id, pf.price, SUM(pf.amount) AS quantity, pf.cafe_comment " +
                            " FROM doc.ProductsFlow pf INNER JOIN doc.GeneralDocs gd ON gd.id=pf.general_id "+
                            " INNER JOIN doc.CafeOrders c ON c.general_id=gd.id" +
                            " WHERE pf.visible=1 "+
                            " AND c.service_perc = "+service_perc +
                            " AND c.service_type = " + service_type + 
                            " AND gd.param_id1=" + store_id + " AND gd.param_id2=" + customer_id + " AND ISNULL(gd.is_packed,0)=0 AND gd.tdate BETWEEN '" + d1 + "' AND '" + d2 + "'" +
                            " GROUP BY pf.product_id, pf.price, pf.unit_id, pf.cafe_comment ";
                        DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);
                        if (data == null || data.Rows.Count == 0)
                            continue;
                        ProductsFlowItem[] ar = new ProductsFlowItem[data.Rows.Count];

                        List<ProductsFlowCafeItem> ar_cafe = new List<ProductsFlowCafeItem>();
                        int cnt = 0;
                        double sum = 0;
                        foreach (DataRow r in data.Rows)
                        {
                            ar[cnt] = new ProductsFlowItem();
                            ar[cnt].quantity = double.Parse(r["quantity"].ToString());
                            ar[cnt].uid = GetProgramManager().GetDataManager().GetProductUidById(int.Parse(r["product_id"].ToString()));
                            ar[cnt].unit_id = GetProgramManager().GetDataManager().GetUnitName(int.Parse(r["unit_id"].ToString()));
                            ar[cnt].price = double.Parse(r["price"].ToString());
                            ar[cnt].main_id = cnt;
                            string cafe_comment = r["cafe_comment"].ToString();
                            if (cafe_comment != "")//plus items
                            {
                                string[] plus_products = cafe_comment.Split('#');
                                for (int k = 0; k < plus_products.Length; k++)
                                {
                                    string[] p_items = plus_products[k].Split('@');
                                    if (p_items.Length != 2)
                                        continue;
                                    int sub_product_id = int.Parse(p_items[0]);
                                    double quantity = 0;
                                    double.TryParse(p_items[1], out quantity);
                                    if(quantity<=0)
                                        continue;
                                    ProductsFlowCafeItem cafe_item = new ProductsFlowCafeItem();
                                    cafe_item.parent_product_id = int.Parse(r["product_id"].ToString());
                                    cafe_item.product_id = sub_product_id;
                                    cafe_item.quantity = quantity;
                                    cafe_item.price = GetProgramManager().GetDataManager().GetProductPriceByID(sub_product_id.ToString(), "3"); 
                                    cafe_item.type = 1;
                                    cafe_item.parent_main_id = cnt;
                                    ar_cafe.Add(cafe_item);
                                }







                            }



                            sum += ar[cnt].quantity * ar[cnt].price;
                            cnt++;

                        }
                        DateTime dt2 = new DateTime(dt.Year, dt.Month, dt.Day, 23, 40, 0);
                        int table_id = 0;
                        int waiter_id = 0;
                        int room_id = 0;
                        double service_add_perc = 0;
                        double consign_amount = 0;
                        byte mode = 0;
                        int project_id = GetProgramManager().GetDataManager().GetStoreProjectID(store_id);



                        int new_general_id = bLogic.Insert_CafeOrder("", table_id, waiter_id,"", 1, room_id, service_type, service_perc, service_add_perc, 0, mode, dt2, 2, "", "", purpose, sum,
                            1, 1, 0, store_id, customer_id, project_id, user_id, false, consign_amount, _staffFoodID, _serviceAmount, ar, ar_cafe);
                            //"", dt2, "", "1", purpose, sum, 1, 1, vat_value, 0, store_id, cash_id, project_id, user_id, true, staff_id, ar);
                        if (new_general_id == -1)
                            return false;
                        sql = "UPDATE doc.GeneralDocs SET sync_status=1, is_packed=1  WHERE id=" + new_general_id;
                        GetProgramManager().GetDataManager().ExecuteSql(sql);

                        //FILL HISTORY LIST
                        sql = "SELECT  gd.id,gd.tdate,gd.uid, gd.purpose " +
                            " FROM doc.ProductsFlow pf INNER JOIN doc.GeneralDocs gd ON gd.id=pf.general_id INNER JOIN doc.CafeOrders c ON c.general_id=gd.id" +
                           " WHERE pf.visible=1 " +
                           " AND c.service_perc = " + service_perc +
                           " AND c.service_type = " + service_type +
                           " AND gd.param_id1=" + store_id + " AND gd.param_id2=" + customer_id + " AND ISNULL(gd.is_packed,0)=0 AND gd.tdate BETWEEN '" + d1 + "' AND '" + d2 + "'" +
                            " GROUP BY gd.id,gd.tdate,gd.uid, gd.purpose";
                       DataTable doc_data = GetProgramManager().GetDataManager().GetTableData(sql);
                       if (doc_data == null || doc_data.Rows.Count == 0)
                            continue;

                       int doc_id = 0;// GetProgramManager().GetDataManager().GetIntegerValue("SELECT ISNULL(MAX(id),0)+1 FROM doc.PackIems ");
                        GetProgramManager().GetDataManager().Close();
                        using (System.Transactions.TransactionScope tran = new System.Transactions.TransactionScope(new System.Transactions.TransactionScopeOption(), new TimeSpan(0, 50, 0)))
                        {
                            GetProgramManager().GetDataManager().Open();

                            foreach (DataRow row3 in doc_data.Rows)
                            {
                                int gd_id = int.Parse(row3["id"].ToString());

                                Hashtable m_SqlParams = new Hashtable();
                             /*   sql = @"INSERT INTO doc.ProductOutSingleItems (id, general_id, old_general_id, tdate,purpose, uid )
                                               VALUES(@id, @general_id, @old_general_id, @tdate,@purpose, @uid )";
                                m_SqlParams.Clear();
                                m_SqlParams.Add("@id", doc_id);
                                m_SqlParams.Add("@general_id", new_general_id);
                                m_SqlParams.Add("@old_general_id", gd_id);
                                m_SqlParams.Add("@tdate", Convert.ToDateTime(row3["tdate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                m_SqlParams.Add("@purpose", row3["purpose"].ToString());
                                m_SqlParams.Add("@uid", row3["uid"].ToString());
                                if (!GetProgramManager().GetDataManager().ExecuteSql(sql, m_SqlParams))
                                    return false;
                                sql = "SELECT  pf.product_id, pf.price, pf.unit_id, pf.amount AS quantity, pf.price " +
                                    " FROM doc.ProductsFlow pf INNER JOIN doc.GeneralDocs gd ON gd.id=pf.general_id INNER JOIN doc.ProductOutSingle c ON c.general_id=gd.id" +
                                    " WHERE gd.id=" + gd_id + " AND pf.visible=1 AND gd.project_id=" + project_id + " AND c.staff_id=" + staff_id +
                                    " AND gd.param_id2=" + store_id + " AND gd.param_id1=" + cash_id + " AND ISNULL(c.out_type,0)=0 AND gd.tdate BETWEEN '" + d1 + "' AND '" + d2 + "'" +
                                    " ";
                                DataTable data2 = GetProgramManager().GetDataManager().GetTableData(sql);
                                if (data2 == null || data2.Rows.Count == 0)
                                {
                                    System.Transactions.Transaction.Current.Rollback();
                                    return false;
                                }
                                foreach (DataRow row2 in data2.Rows)
                                {
                                    sql = @"INSERT INTO doc.ProductOutSingleSubItems (general_id, parent_id, product_id,unit_id,amount,price)
                                               VALUES(@general_id, @parent_id, @product_id,@unit_id,@amount,@price)";
                                    m_SqlParams.Clear();

                                    m_SqlParams.Add("@general_id", new_general_id);
                                    m_SqlParams.Add("@parent_id", doc_id);
                                    m_SqlParams.Add("@product_id", row2["product_id"].ToString());
                                    m_SqlParams.Add("@unit_id", row2["unit_id"].ToString());
                                    m_SqlParams.Add("@amount", row2["quantity"].ToString());
                                    m_SqlParams.Add("@price", row2["price"].ToString());
                                    if (!GetProgramManager().GetDataManager().ExecuteSql(sql, m_SqlParams))
                                    {
                                        System.Transactions.Transaction.Current.Rollback();
                                        return false;
                                    }
                                }
                                */
                                doc_id++;
                                if (auto_delete)
                                {
                                    if (!GetProgramManager().DeleteDocItem(gd_id, "DOC_CAFEORDER", true, false, false, false))
                                    {
                                        System.Transactions.Transaction.Current.Rollback();
                                        return false;
                                    }

                                }
                            }
                            tran.Complete();
                        }

                    }
                 
                }
                dt = dt.AddDays(1);
                if (dt > date2)
                    break;
            }

            return true;
        }
 
        private bool OnExecuteCashIn()
        {
            BusinessLogic bLogic = new BusinessLogic();
            bLogic.Initialize(GetProgramManager().GetDataManager());
            DateTime date1 = m_Picker.dtp_From.Value;
            DateTime date2 = m_Picker.dtp_To.Value;
            int user_id = GetProgramManager().GetUserID();
            bool auto_delete = true;// checkAutoDelete.Checked;
            DateTime dt = date1;
            string sql = "";
            while (dt <= date2)
            {
                double vat_value = 0;
                if (GetProgramManager().GetDataManager().IsCompanyVAT(dt))
                    vat_value = GetProgramManager().GetDataManager().GetVatPercent();

                string d1 = dt.ToString("yyyy-MM-dd 00:00:01");
                string d2 = dt.ToString("yyyy-MM-dd 23:59:59");
                sql = "SELECT gd.param_id1,  gd.project_id FROM doc.GeneralDocs gd INNER JOIN doc.Operation c ON c.general_id = gd.id "+
                    " WHERE gd.currency_id>0 AND ISNULL(gd.is_packed,0) = 0 AND gd.param_id2=0  AND doc_type=38 AND gd.tdate BETWEEN '" + d1 + "' AND '" + d2 + "'  GROUP BY gd.param_id1, gd.project_id ORDER BY gd.param_id1 ";
                DataTable group_data = GetProgramManager().GetDataManager().GetTableData(sql);


                if (group_data == null || group_data.Rows.Count == 0)
                {
                    dt = dt.AddDays(1);
                    if (dt > date2)
                        break;
                }
                    foreach (DataRow row in group_data.Rows)
                    {
                        int cash_id = int.Parse(row["param_id1"].ToString());
                        int project_id = int.Parse(row["project_id"].ToString());
                        string name = "გადარიცხვა";
                        if (cash_id > 0)
                            name= GetProgramManager().GetDataManager().GetCashNameByID(cash_id);
                        else
                            name = GetProgramManager().GetDataManager().GetContragentNameByID(cash_id);
                        string purpose = "დაჯგუფებული თანხის მიღება " + name;

                        sql = "SELECT  SUM(gd.amount) AS amount " +
                            " FROM doc.Operation c INNER JOIN doc.GeneralDocs gd ON gd.id=c.general_id " +
                            " WHERE ISNULL(gd.is_packed,0) = 0  AND gd.doc_type=38 AND gd.project_id=" + project_id + 
                            " AND gd.param_id1=" + cash_id + " AND gd.param_id2=0 AND gd.tdate BETWEEN '" + d1 + "' AND '" + d2 + "'";
                            
                        DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);
                        if (data == null || data.Rows.Count == 0)
                            continue;
                       
                            double sum = 0;
                            double.TryParse(data.Rows[0]["amount"].ToString(), out sum);
                            if (sum <= 0)
                                continue;
                            
                       
                        DateTime dt2 = new DateTime(dt.Year, dt.Month, dt.Day, 23, 40, 0);
                        int new_general_id = bLogic.Insert_CustomerMoneyIn(dt2, purpose, sum, 1, 1, cash_id, "", 0, project_id, user_id, 0, true);
                        if (new_general_id == -1)
                            return false;
                        sql = "UPDATE doc.GeneralDocs SET sync_status=1, is_packed=1 WHERE id=" + new_general_id;
                        GetProgramManager().GetDataManager().ExecuteSql(sql);

                        //FILL HISTORY LIST
                        sql = "SELECT  gd.id,gd.tdate,gd.uid, gd.purpose, gd.amount " +
                          " FROM doc.Operation c INNER JOIN doc.GeneralDocs gd ON gd.id=c.general_id " +
                          " WHERE ISNULL(gd.is_packed,0) = 0  AND gd.doc_type=38 AND gd.project_id=" + project_id + 
                          " AND gd.param_id1=" + cash_id + " AND gd.param_id2=0 AND gd.tdate BETWEEN '" + d1 + "' AND '" + d2 + "'";

                        DataTable doc_data = GetProgramManager().GetDataManager().GetTableData(sql);
                        if (doc_data == null || doc_data.Rows.Count == 0)
                            continue;

                        //int doc_id = GetProgramManager().GetDataManager().GetIntegerValue("SELECT ISNULL(MAX(id),0)+1 FROM doc.PackItems ");
                        GetProgramManager().GetDataManager().Close();
                        using (System.Transactions.TransactionScope tran = new System.Transactions.TransactionScope(new System.Transactions.TransactionScopeOption(), new TimeSpan(0, 50, 0)))
                        {
                            GetProgramManager().GetDataManager().Open();

                            foreach (DataRow row3 in doc_data.Rows)
                            {
                                int gd_id = int.Parse(row3["id"].ToString());

                                Hashtable m_SqlParams = new Hashtable();
                                   sql = @"INSERT INTO doc.PackItems (general_id, old_general_id, doc_type, tdate,purpose, uid, amount )
                                                  VALUES(@general_id, @old_general_id, @doc_type, @tdate,@purpose, @uid, @amount )";
                                   m_SqlParams.Clear();
                                   m_SqlParams.Add("@general_id", new_general_id);
                                   m_SqlParams.Add("@old_general_id", gd_id);
                                   m_SqlParams.Add("@doc_type", 38);
                                   m_SqlParams.Add("@tdate", Convert.ToDateTime(row3["tdate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                                   m_SqlParams.Add("@purpose", row3["purpose"].ToString());
                                   m_SqlParams.Add("@uid", row3["uid"].ToString());
                                   m_SqlParams.Add("@amount", row3["amount"].ToString());

                                   if (!GetProgramManager().GetDataManager().ExecuteSql(sql, m_SqlParams))
                                   {
                                       System.Transactions.Transaction.Current.Rollback();
                                       return false;
                                   }
                                 

                                //doc_id++;
                                if (auto_delete)
                                {
                                    if (!GetProgramManager().DeleteDocItem(gd_id, "DOC_OPERATION-CUSTOMERMONEYIN", true, false, false, false))
                                    {
                                        System.Transactions.Transaction.Current.Rollback();
                                        return false;
                                    }

                                }
                            }
                            tran.Complete();
                        }

                    }
                    dt = dt.AddDays(1);
                    if (dt > date2)
                        break;
                
            }

            return true;
        }

        //private bool OnExecuteProductOutSingle()
        //{
        //    labelProgress.Text = "";
        //    Application.DoEvents();

        //    string d1 = m_Picker.dtp_From.Value.ToString("yyyy-MM-dd 00:00:00");
        //    string d2 = m_Picker.dtp_To.Value.ToString("yyyy-MM-dd 23:59:59");

        //    ProgressDispatcher.Activate();

        //    string _sql_logic = string.Format(@"DECLARE @date1 AS DATETIME ='{0}'
        //                                        DECLARE @date2 AS DATETIME ='{1}'

        //                                        ;WITH Calendar(date_from, date_to)
        //                                        AS
        //                                        (
        //                                            SELECT @date1 AS date_from, DATEADD(ss, DATEDIFF(ss, 0, '23:59:59'), @date1) AS date_to
        //                                            UNION ALL
        //                                            SELECT DATEADD(dd, 1, date_from),  DATEADD(ss, DATEDIFF(ss, 0, '23:59:59'), DATEADD(dd, 1, date_from)) 
        //                                            FROM Calendar
        //                                            WHERE DATEADD(dd, 1, date_from) <= @date2 
        //                                        ),
        //                                        GroupData(date_from, date_to, cash_id, store_id, project_id, staff_id, staff_name)
        //                                        AS
        //                                        ( 
        //                                            SELECT DISTINCT dt.date_from, dt.date_to, gd.param_id1, gd.param_id2, gd.project_id, c.staff_id, st.name AS staff_name  
        //                                            FROM Calendar dt 
        //                                            INNER JOIN doc.GeneralDocs AS gd ON gd.tdate BETWEEN dt.date_from AND dt.date_to
        //                                            INNER JOIN doc.ProductOutSingle c ON c.general_id = gd.id 
        //                                            LEFT JOIN book.Staff AS st ON st.id=c.staff_id 
        //                                            WHERE gd.doc_type=23 AND (c.out_type IS NULL OR c.out_type = 0)
        //                                        )
        //                                        SELECT gen.date_from, gen.date_to, gen.cash_id, gen.store_id, gen.project_id, gen.staff_id, gen.staff_name, pf.product_id, p.uid, pf.price, pf.unit_id, u.full_name AS unit_name, pf.vat_percent, ISNULL(SUM(pf.amount),0) AS quantity 
        //                                        FROM GroupData AS gen
        //                                        INNER JOIN doc.GeneralDocs AS g ON g.tdate BETWEEN gen.date_from AND gen.date_to
        //                                        INNER JOIN doc.ProductsFlow AS  pf ON pf.general_id=g.id 
        //                                        INNER JOIN doc.ProductOutSingle AS c ON c.general_id=g.id
        //                                        INNER JOIN book.Products AS p ON p.id = pf.product_id 
        //                                        INNER JOIN book.Units AS u ON u.id = pf.unit_id
        //                                        WHERE g.doc_type=23 AND pf.visible=1  AND g.project_id=gen.project_id AND c.staff_id=gen.staff_id AND g.param_id2 = gen.store_id AND g.param_id1 = gen.cash_id AND (c.out_type IS NULL OR c.out_type = 0) 
        //                                        GROUP BY gen.date_from, gen.date_to, gen.cash_id, gen.store_id, gen.project_id, gen.staff_id, gen.staff_name, pf.product_id, p.uid, pf.unit_id, u.full_name, pf.price, pf.vat_percent
        //                                        ORDER BY  gen.date_from
        //                                        OPTION (MAXRECURSION 0)", d1, d2);

        //    DataTable _data = GetProgramManager().GetDataManager().GetTableData(_sql_logic);
        //    if (_data == null || _data.Rows.Count == 0)
        //    {
        //        ProgressDispatcher.Deactivate();
        //        return true;
        //    }
        //    var groupeds = _data.Rows.OfType<DataRow>().GroupBy(g => new
        //    {
        //        date_from = Convert.ToDateTime(g["date_from"]),
        //        date_to = Convert.ToDateTime(g["date_to"]),
        //        cash_id = Convert.ToInt32(g["cash_id"]),
        //        store_id = Convert.ToInt32(g["store_id"]),
        //        project_id = Convert.ToInt32(g["project_id"]),
        //        staff_id = Convert.ToInt32(g["staff_id"]),
        //        staff_name = Convert.ToString(g["staff_name"])
        //    }).Select(g => new { items = g.ToList() }).ToList();
        //    if (!groupeds.Any())
        //    {
        //        ProgressDispatcher.Deactivate();
        //        return true;
        //    }


        //    BusinessLogic bLogic = new BusinessLogic(GetProgramManager().GetDataManager()) { IgnoreSelfCost = checkSelfCost.Checked };
        //    int user_id = GetProgramManager().GetUserID();
        //    List<ProductsFlowItem> pf_item = new List<ProductsFlowItem>();

        //    int _count = groupeds.Count;
        //    foreach (var data in groupeds)
        //    {
        //        labelProgress.Text = (groupeds.IndexOf(data) + 1).ToString() + "/" + _count.ToString();
        //        labelProgress.Refresh();
        //        Application.DoEvents();

        //        DateTime _date1 = data.items.First().Field<DateTime>("date_from");
        //        DateTime _date2 = data.items.First().Field<DateTime>("date_to");
        //        int cash_id = data.items.First().Field<int>("cash_id");
        //        int store_id = data.items.First().Field<int>("store_id");
        //        int project_id = data.items.First().Field<int>("project_id");
        //        int staff_id = data.items.First().Field<int>("staff_id");
        //        string staff_name = data.items.First().Field<string>("staff_name");
        //        string purpose = "დაჯგუფებული საცალო გაყიდვა " + staff_name;

        //        double vat_value = 0;
        //        if (GetProgramManager().GetDataManager().IsCompanyVAT(_date1))
        //            vat_value = GetProgramManager().GetDataManager().GetVatPercent();

        //        pf_item.Clear();
        //        pf_item = data.items.Select(f => new ProductsFlowItem
        //        {
        //            quantity = Convert.ToDouble(f["quantity"]),
        //            uid = Convert.ToString(f["uid"]),
        //            unit_id = Convert.ToString(f["unit_name"]),
        //            price = Convert.ToDouble(f["price"]),
        //            vat = Convert.ToDouble(f["vat_percent"])
        //        }).ToList();



        //        int general_id = bLogic.Insert_ProductOutSingle("", _date2.AddSeconds(-1), "", "1", purpose, pf_item.Sum(a => a.quantity * a.price), 1, 1, vat_value, 0, store_id, cash_id, project_id, user_id, true, staff_id, pf_item.ToArray());
        //        if (general_id <= 0)
        //        {
        //            ProgressDispatcher.Deactivate();
        //            return false;
        //        }
        //        if (!GetProgramManager().GetDataManager().ExecuteSql("UPDATE doc.GeneralDocs SET sync_status=1 WHERE id=" + general_id))
        //        {
        //            ProgressDispatcher.Deactivate();
        //            return false;
        //        }
        //        if (!GetProgramManager().GetDataManager().ExecuteSql("UPDATE doc.ProductOutSingle set out_type = 1 WHERE general_id=" + general_id))
        //        {
        //            ProgressDispatcher.Deactivate();
        //            return false;
        //        }


        //    }




        //    string _sql_delete = string.Format(@"SELECT g.id, d.tag
        //                                    FROM doc.GeneralDocs AS g
        //                                    INNER JOIN doc.ProductOutSingle AS c ON c.general_id=g.id
        //                                    INNER JOIN config.DocTypes AS d ON d.id=g.doc_type
        //                                    WHERE g.doc_type=23  AND (c.out_type IS NULL OR c.out_type = 0)
        //                                    AND g.tdate BETWEEN '{0}' AND '{1}'", d1, d2);
        //    Dictionary<int, string> _deletables = GetProgramManager().GetDataManager().GetDictionary<int, string>(_sql_delete);
        //    if (!_deletables.Any())
        //    {
        //        ProgressDispatcher.Deactivate();
        //        return true;
        //    }
        //    Application.DoEvents();
        //    labelProgress.Text = "Deleting...";
        //    labelProgress.Refresh();
        //    KeyValuePair<bool, string> _err = GetProgramManager().OnDeleteDocs(_deletables);

        //    if (!_err.Key)
        //    {
        //        ProgressDispatcher.Deactivate();
        //        MessageBoxForm.Show(Application.ProductName, "შეცდომა ოპერაციის წაშლისას!", _err.Value, null, SystemIcons.Error);
        //        return false;
        //    }

        //    ProgressDispatcher.Deactivate();
        //    return true;
        //}

        private bool OnExecuteProductOutSingle()
        {
           
            labelProgress.Text = "მონაცემთა ანალიზი...";
            labelProgress.Refresh();
            checkSelfCost.Enabled = false;
            btnOK.Enabled = false;
            btnStop.Visible = true;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start(); 
            Application.DoEvents();

            string d1 = m_Picker.dtp_From.Value.ToString("yyyy-MM-dd 00:00:00");
            string d2 = m_Picker.dtp_To.Value.ToString("yyyy-MM-dd 23:59:59");

            
            string _sql_logic = string.Format(@"DECLARE @date1 AS DATETIME ='{0}'
                                                DECLARE @date2 AS DATETIME ='{1}'

                                                ;WITH Calendar(date_from, date_to)
                                                AS
                                                (
                                                    SELECT @date1 AS date_from, DATEADD(ss, DATEDIFF(ss, 0, '23:59:59'), @date1) AS date_to
                                                    UNION ALL
                                                    SELECT DATEADD(dd, 1, date_from),  DATEADD(ss, DATEDIFF(ss, 0, '23:59:59'), DATEADD(dd, 1, date_from)) 
                                                    FROM Calendar
                                                    WHERE DATEADD(dd, 1, date_from) <= @date2 
                                                ),
                                                GroupData(date_from, date_to, cash_id, store_id, project_id, staff_id, staff_name)
                                                AS
                                                ( 
                                                    SELECT DISTINCT dt.date_from, dt.date_to, gd.param_id1, gd.param_id2, gd.project_id, c.staff_id, st.name AS staff_name  
                                                    FROM Calendar dt 
                                                    INNER JOIN doc.GeneralDocs AS gd ON gd.tdate BETWEEN dt.date_from AND dt.date_to
                                                    INNER JOIN doc.ProductOutSingle c ON c.general_id = gd.id 
                                                    LEFT JOIN book.Staff AS st ON st.id=c.staff_id 
                                                    WHERE gd.doc_type=23 AND (c.out_type IS NULL OR c.out_type = 0)
                                                )
                                                SELECT gen.date_from, gen.date_to, gen.cash_id, gen.store_id, gen.project_id, gen.staff_id, gen.staff_name, pf.product_id, pf.price, pf.unit_id, pf.vat_percent, ISNULL(SUM(pf.amount),0) AS quantity 
                                                FROM GroupData AS gen
                                                INNER JOIN doc.GeneralDocs AS g ON g.tdate BETWEEN gen.date_from AND gen.date_to
                                                INNER JOIN doc.ProductsFlow AS  pf ON pf.general_id=g.id 
                                                INNER JOIN doc.ProductOutSingle AS c ON c.general_id=g.id
                                                WHERE g.doc_type=23 AND pf.visible=1  AND g.project_id=gen.project_id AND c.staff_id=gen.staff_id AND g.param_id2 = gen.store_id AND g.param_id1 = gen.cash_id AND (c.out_type IS NULL OR c.out_type = 0) 
                                                GROUP BY gen.date_from, gen.date_to, gen.cash_id, gen.store_id, gen.project_id, gen.staff_id, gen.staff_name, pf.product_id, pf.unit_id, pf.price, pf.vat_percent
                                                ORDER BY  gen.date_from
                                                OPTION (MAXRECURSION 0)", d1, d2);

            DataTable _data = GetProgramManager().GetDataManager().GetTableData(_sql_logic);
            if (_data == null || _data.Rows.Count == 0)
            {
                stopWatch.Stop();
                return true;
            }
            var groupeds = _data.Rows.OfType<DataRow>().GroupBy(g => new
            {
                date_from = Convert.ToDateTime(g["date_from"]),
                date_to = Convert.ToDateTime(g["date_to"]),
                cash_id = Convert.ToInt32(g["cash_id"]),
                store_id = Convert.ToInt32(g["store_id"]),
                project_id = Convert.ToInt32(g["project_id"]),
                staff_id = Convert.ToInt32(g["staff_id"]),
                staff_name = Convert.ToString(g["staff_name"])
            }).Select(g => new { items = g.ToList() }).ToList();
            if (!groupeds.Any())
            {
                stopWatch.Stop();
                return true;
            }

            int user_id = GetProgramManager().GetUserID();
            using (BusinesContext _bc = new BusinesContext() { IgnoreSelfCostOnSave = checkSelfCost.Checked })
            {
                Dictionary<KeyValuePair<int, string>, long> _nm = _bc.GenerateDocNums(new List<int> { 23 }, null);
                long doc_num = !_nm.Any() ? 1 : _nm.First().Value;

                foreach (var data in groupeds)
                {
                    if (_stop)
                        return true;
                    int per = (int)(((decimal)groupeds.IndexOf(data)+1) / (decimal)groupeds.Count * 100M);
                    TimeSpan ts = stopWatch.Elapsed;
                    labelProgress.Text = string.Format("{0} / {1} ({2}%)", groupeds.IndexOf(data) + 1, groupeds.Count, per);
                    lblElapsedTime.Text = string.Format("გასული დრო {0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                    lblElapsedTime.Refresh();
                    labelProgress.Refresh();
                    Application.DoEvents();

                    DateTime _date1 = data.items.First().Field<DateTime>("date_from");
                    DateTime _date2 = data.items.First().Field<DateTime>("date_to");
                    int store_id = data.items.First().Field<int>("store_id");
                    int staff_id = data.items.First().Field<int>("staff_id");
                    int cash_id = data.items.First().Field<int>("cash_id");
                    int project_id = data.items.First().Field<int>("project_id");
                    double vat_value = 0;
                    if (_bc.IsCompanyVat(_date1))
                        vat_value = double.Parse(GetProgramManager().GetConfigParamValue("vat"));

                    int general_id = _bc.SaveProductOutSingle(new DocItem
                    {
                        GeneralDocsItem = new GeneralDocs
                        {
                            Tdate = _date2.AddSeconds(-1),
                            DocNum = doc_num++,
                            DocType = 23,
                            Purpose = string.Concat("დაჯგუფებული საცალო გაყიდვა ", data.items.First().Field<string>("staff_name")),
                            Amount = data.items.Select(f => Convert.ToDouble(f["quantity"]) * Convert.ToDouble(f["price"])).DefaultIfEmpty().Sum(),
                            Vat = vat_value,
                            UserId = user_id,
                            MakeEntry = true,
                            ProjectId = project_id,
                            StaffId = staff_id,
                            ParamId1 = cash_id,
                            ParamId2 = store_id,
                            StoreId = store_id,
                            SyncStatus = 1,
                            Uid = Guid.NewGuid().ToString()
                        },
                        ProductOutSingleItem = new ProductOutSingle
                        {
                            OutType = 1,
                            StaffId = staff_id,
                        },
                        ProductsFlowList = data.items.Select(f => new ProductsFlow
                        {
                            ProductId = Convert.ToInt32(f["product_id"]),
                            Amount = Convert.ToDouble(f["quantity"]),
                            Price = Convert.ToDouble(f["price"]),
                            UnitId = Convert.ToInt32(f["unit_id"]),
                            StoreId = store_id,
                            VatPercent = Convert.ToDecimal(f["vat_percent"]),
                            Coeff = -1,
                        }).ToList()
                    });

                    if (general_id <= 0)
                    {
                        stopWatch.Stop();
                        return false;
                    }
                    
                    string _sql_delete = string.Format(@"SELECT g.id, d.tag
                                            FROM doc.GeneralDocs AS g
                                            INNER JOIN doc.ProductOutSingle AS c ON c.general_id=g.id
                                            INNER JOIN config.DocTypes AS d ON d.id=g.doc_type
                                            WHERE g.doc_type=23  AND (c.out_type IS NULL OR c.out_type = 0) 
                                            AND g.tdate BETWEEN '{0}' AND '{1}' AND g.param_id2={2} AND g.param_id1={3} AND g.staff_id={4} AND g.project_id={5}", _date1.ToString("yyyy-MM-dd HH:mm:ss.fff"), _date2.ToString("yyyy-MM-dd HH:mm:ss.fff"), store_id, cash_id, staff_id, project_id);
                    Dictionary<int, string> _deletables = GetProgramManager().GetDataManager().GetDictionary<int, string>(_sql_delete);
                    if (_deletables.Any())
                    {
                        KeyValuePair<bool, string> _err = GetProgramManager().OnDeleteDocs(_deletables);
                        if(!_err.Key)
                        {
                            stopWatch.Stop();
                            MessageBoxForm.Show(Application.ProductName, "შეცდომა ოპერაციის წაშლისას!", _err.Value, null, SystemIcons.Error);
                            return false;
                        }
                    }
                }
            }
            stopWatch.Stop();
            return true;
        }

       
        private void UpdateControl(bool block)
        {
            comboOperation.Enabled = !block;
            m_Picker.Enabled = !block;
            btnOK.Enabled = !block;
            checkSelfCost.Enabled = !block;
            btnStop.Visible = block;
            labelProgress.Text = "";
        }

        private void comboOperation_SelectionChangeCommitted(object sender, EventArgs e)
        {
            checkSelfCost.Visible = Convert.ToInt32(comboOperation.SelectedValue) == 1;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _stop = true;
        }

     
    }

    
}
