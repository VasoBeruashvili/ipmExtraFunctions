using System;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using ipmPMBasic;

namespace ipmExtraFunctions
{
    public partial class ExtractVatForm : Form
    {
        
        ProgramManagerBasic pm;
        

        public ExtractVatForm(ProgramManagerBasic pm)
        {
            InitializeComponent();

            this.pm = pm;
            m_GridProducts.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            m_Grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            m_Grid.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(216, 228, 248);
            m_GridProducts.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(216, 228, 248);
            string sql = "SELECT ISNULL(tdate,'2010-01-01 01:00:00.000') FROM book.Companies WHERE id = 1";
            string res = GetProgramManager().GetDataManager().GetStringValue(sql);
            m_Picker.dtp_From.Value = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 5);
            m_Picker.dtp_To.Value = DateTime.Now;
            try
            {
                if (res != "")
                    m_Picker.dtp_To.Value = DateTime.Parse(res);
            }
            catch
            {

            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
        }

        private ProgramManagerBasic GetProgramManager()
        {
            return pm;
        }

     

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            OnSearchDocuments();
        }

        Hashtable hBadProductIds = new Hashtable();
        private void OnSearchDocuments()
        {
            hBadProductIds.Clear();
            DateTime tdate = new DateTime(m_Picker.dtp_To.Value.Year, m_Picker.dtp_To.Value.Month, m_Picker.dtp_To.Value.Day, 0, 0, 1);
            string sql = "SELECT * FROM book.Products WHERE path LIKE '0#1#%' OR path LIKE '0#3#%'";
            DataTable product_data = GetProgramManager().GetDataManager().GetTableData(sql);
            if (product_data == null || product_data.Rows.Count == 00)
                return;
            m_Grid.Rows.Clear();
            Hashtable hIds = new Hashtable();
            foreach (DataRow row in product_data.Rows)
            {
                int product_id = int.Parse(row["id"].ToString());
                //if (product_id != 888)
               // {
               //     continue;
               // }
                int vat_type = GetProgramManager().GetDataManager().GetProductVatType(product_id);
                if (vat_type != 1)
                {
                    hBadProductIds.Add(product_id, product_id);
                    continue;
                }
                ArrayList in_ids = new ArrayList();
                int n = GetProductDocs(tdate, m_Picker.dtp_From.Value,product_id, ref in_ids);
                if (n == 0)
                {
                    hBadProductIds.Add(product_id, product_id);
                    continue;
                }

                
                
                for(int i=0;i<n;i++)
                {
                    int general_id = (int) in_ids[i];
                if(hIds.ContainsKey(general_id))
                    continue;
                    hIds.Add(general_id, general_id);
                }
            }
            foreach (int key in hIds.Keys)
            {
                sql = "SELECT gd.id, gd.tdate, doc.doc1, doc.doc2, c.name, gd.purpose FROM doc.GeneralDocs gd INNER JOIN doc.ProductIn doc ON doc.general_id=gd.id  INNER JOIN book.Contragents c ON c.id=gd.param_id1 WHERE gd.id=" + key;
                DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);
                if (data == null || data.Rows.Count == 0)
                    continue;
                DataRow row = data.Rows[0];
                m_Grid.Rows.Add(0, row["id"].ToString(), row["tdate"].ToString(), row["doc1"].ToString(), row["doc2"].ToString(), row["name"].ToString(), row["purpose"].ToString());

            }

            foreach (int key in hIds.Keys)
            {
                sql = "SELECT gd.id, gd.tdate, '' AS doc1 , '' AS doc2, '' AS name, gd.purpose FROM doc.GeneralDocs gd INNER JOIN doc.ProductStart doc ON doc.general_id=gd.id  WHERE gd.id=" + key;
                DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);
                if (data == null || data.Rows.Count == 0)
                    continue;
                DataRow row = data.Rows[0];
                m_Grid.Rows.Add(0, row["id"].ToString(), row["tdate"].ToString(), row["doc1"].ToString(), row["doc2"].ToString(), row["name"].ToString(), row["purpose"].ToString());
            }
            


        }

        private int GetProductDocs(DateTime tdate, DateTime dt2, int product_id, ref ArrayList in_ids)
        {
            int store_idd = 0;
           // double ret_value = 0;
            double out_amount = 0;
//            DateTime tdate2 = new DateTime(tdate.Year, 1, 1, 0, 0, 5);// tdate.AddMonths(-3);
            DateTime tdate2 = new DateTime(dt2.Year, dt2.Month, dt2.Day, 0, 0, 5);// tdate.AddMonths(-3);

            string sql = string.Format("SELECT c.param_id1, pf.general_id, (CASE WHEN ISNULL(pf.in_id,0)>0 THEN pf.in_id ELSE pf.general_id END) AS in_id, pf.amount" +
                " FROM doc.ProductsFlow pf INNER JOIN doc.GeneralDocs c ON c.id = pf.general_id WHERE pf.product_id={0} AND (pf.is_move=0) AND c.tdate<'{2}' AND ( c.tdate>'{3}' OR c.currency_id>1) AND coeff=1 AND is_expense=0  AND is_order=0 ORDER BY c.tdate ASC",
            product_id.ToString(),
            store_idd.ToString(),
            tdate.ToString("yyyy-MM-dd 00:00:05"), tdate2.ToString("yyyy-MM-dd 00:00:05"));
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);
            if (data == null || data.Rows.Count == 0)
            {
                return 0;
            }
            // SaveDataTableToFile(data, "c:\\Temp\\2.csv");
            double rest = GetProgramManager().GetDataManager().GetProductRest(product_id.ToString(), store_idd.ToString(), tdate, 0, 0);
            if (rest <= 0)
                return 0;
            out_amount = rest;
            int N = data.Rows.Count;
            double am = 0;
            int found_i = -1;
            double start_amount = 0;
            for (int i = N - 1; i >= 0; i--)
            {
                double a = Convert.ToDouble(data.Rows[i]["amount"].ToString());
                am += a;
                if (Math.Round(rest, 6) <= Math.Round(am, 6))
                {
                    start_amount = a - (am - rest);
                    found_i = i;
                    break;
                }
            }
            start_amount = Math.Round(start_amount, 6);
            if (found_i == -1)
            {
                double a = Convert.ToDouble(data.Rows[N-1]["amount"].ToString());
                if (rest > a)
                {
                    in_ids.Add(Convert.ToInt32(data.Rows[N - 1]["in_id"].ToString()));
                    return in_ids.Count;
                }
                return 0;

            }
            if (out_amount <= start_amount) //if fits out amount to existing current available in ware
            {
              //  double sf = Convert.ToDouble(data.Rows[found_i]["self_cost"].ToString());


                in_ids.Add(Convert.ToInt32(data.Rows[found_i]["in_id"].ToString()));
                return in_ids.Count;
            }
            else
            {
               // double sf = Convert.ToDouble(data.Rows[found_i]["self_cost"].ToString());
                in_ids.Add(Convert.ToInt32(data.Rows[found_i]["in_id"].ToString()));
                out_amount -= start_amount;
                found_i++;
            }


            //int cnt = 0;
            for (int i = found_i; i < N; i++)
            {
                double a = Convert.ToDouble(data.Rows[i]["amount"].ToString());
               // double sf = Convert.ToDouble(data.Rows[i]["self_cost"].ToString());
                int in_id = Convert.ToInt32(data.Rows[i]["in_id"].ToString());
               

                if (Math.Round(out_amount, 6) <= Math.Round(a, 6))
                {
                    in_ids.Add(in_id);
                    return in_ids.Count;
                }
                else
                {
                    in_ids.Add(in_id);
                    out_amount -= a;
                }
            }


            return in_ids.Count;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OnCalculate();
        }
        private int GetProductDetailedDocs(DateTime tdate, DateTime dt2, int product_id, int store_id, ref ArrayList in_ids, ref ArrayList self_costs, ref ArrayList amounts)
        {
            int store_idd = 0;
            //double ret_value = 0;
            double out_amount = 0;
            DateTime tdate2 = new DateTime(dt2.Year, dt2.Month, dt2.Day, 0, 0, 5);// tdate.AddMonths(-3);

            string sql = "";
            if (checkAddExpense.Checked)
            {
                sql = string.Format("SELECT c.param_id1, pf.general_id, (CASE WHEN ISNULL(pf.in_id,0)>0 THEN pf.in_id ELSE pf.general_id END) AS in_id, pf.amount,ISNULL(pf.vendor_id,0) AS vendor_id, pf.self_cost " +
                     " + ISNULL((SELECT SUM(p.self_cost) FROM doc.ProductsFlow p INNER JOIN doc.GeneralDocs c2 ON c2.id = p.general_id INNER JOIN book.Contragents con ON con.id = c2.param_id1 WHERE is_expense=1 AND con.vat_type=1 AND p.ref_id=pf.id ),0) " +
                     " + ISNULL((SELECT SUM(p.self_cost) FROM doc.ProductsFlow p INNER JOIN doc.GeneralDocs c2 ON c2.id = p.general_id INNER JOIN book.Contragents con ON con.id = c2.param_id1 WHERE p.is_expense=1 AND con.vat_type=1 AND p.coeff=1 AND p.product_id ={0} AND p.general_id=pf.general_id),0) AS self_cost" +
                     " FROM doc.ProductsFlow pf INNER JOIN doc.GeneralDocs c ON c.id = pf.general_id " +
                     " WHERE pf.product_id={0} AND (pf.is_move=0) AND c.tdate<'{2}' AND ( c.tdate>'{3}' OR c.currency_id>1) AND coeff=1 AND is_expense=0  AND is_order=0 ORDER BY c.tdate ASC",
                 product_id.ToString(),
                 store_idd.ToString(),
                 tdate.ToString("yyyy-MM-dd HH:mm:ss.fff"), tdate2.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                sql = string.Format("SELECT c.param_id1, pf.general_id, (CASE WHEN ISNULL(pf.in_id,0)>0 THEN pf.in_id ELSE pf.general_id END) AS in_id, pf.amount,ISNULL(pf.vendor_id,0) AS vendor_id, pf.self_cost " +
                     " FROM doc.ProductsFlow pf INNER JOIN doc.GeneralDocs c ON c.id = pf.general_id WHERE pf.product_id={0} AND (pf.is_move=0) AND c.tdate<'{2}' AND ( c.tdate>'{3}' OR c.currency_id>1) AND coeff=1 AND is_expense=0  AND is_order=0 ORDER BY c.tdate ASC",
                 product_id.ToString(),
                 store_idd.ToString(),
                 tdate.ToString("yyyy-MM-dd HH:mm:ss.fff"), tdate2.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);
            if (data == null || data.Rows.Count == 0)
            {
                return 0;
            }
            // SaveDataTableToFile(data, "c:\\Temp\\2.csv");
            double rest = GetProgramManager().GetDataManager().GetProductRest(product_id.ToString(), store_id.ToString(), tdate, 0, 0);
            if (rest <= 0)
                return 0;
            out_amount = rest;

            int N = data.Rows.Count;
            double am = 0;
            int found_i = -1;
            double start_amount = 0;
            for (int i = N - 1; i >= 0; i--)
            {
                double a = Convert.ToDouble(data.Rows[i]["amount"].ToString());
                am += a;
                if (Math.Round(rest, 6) <= Math.Round(am, 6))
                {
                    start_amount = a - (am - rest);
                    found_i = i;
                    break;
                }
            }
            start_amount = Math.Round(start_amount, 6);
            if (found_i == -1)
                if (found_i == -1)
                {
                    double a = Convert.ToDouble(data.Rows[N - 1]["amount"].ToString());
                    if (rest > a)
                    {
                        in_ids.Add(Convert.ToInt32(data.Rows[N - 1]["in_id"].ToString()));
                        double sf = Convert.ToDouble(data.Rows[N - 1]["self_cost"].ToString());
                        amounts.Add(a);
                        self_costs.Add(sf);
                        return in_ids.Count;
                    }
                    return 0;

                }

            if (out_amount <= start_amount) //if fits out amount to existing current available in ware
            {
                double sf = Convert.ToDouble(data.Rows[found_i]["self_cost"].ToString());
               
                self_costs.Add(sf);
                in_ids.Add(Convert.ToInt32(data.Rows[found_i]["in_id"].ToString()));
                amounts.Add(out_amount);
                return in_ids.Count;
            }
            else
            {
                double sf = Convert.ToDouble(data.Rows[found_i]["self_cost"].ToString());
              
                self_costs.Add(sf);
                in_ids.Add(Convert.ToInt32(data.Rows[found_i]["in_id"].ToString()));
                amounts.Add(start_amount);
                out_amount -= start_amount;
                found_i++;
            }


            //int cnt = 0;
            for (int i = found_i; i < N; i++)
            {
                double a = Convert.ToDouble(data.Rows[i]["amount"].ToString());
                double sf = Convert.ToDouble(data.Rows[i]["self_cost"].ToString());
                int in_id = Convert.ToInt32(data.Rows[i]["in_id"].ToString());

                if (Math.Round(out_amount, 6) <= Math.Round(a, 6))
                {
                    self_costs.Add(sf);
                    amounts.Add(out_amount);
                    in_ids.Add(in_id);
                    return in_ids.Count;
                }
                else
                {
                    self_costs.Add(sf);
                    amounts.Add(a);
                    in_ids.Add(in_id);
                    out_amount -= a;
                }
            }
            return in_ids.Count;
        }

        private void OnCalculate()
        {
            m_Grid.EndEdit();
            labelSum.Text = "";
            DateTime tdate = new DateTime(m_Picker.dtp_To.Value.Year,m_Picker.dtp_To.Value.Month,m_Picker.dtp_To.Value.Day,0,0,1);
            string sql = "SELECT * FROM book.Products WHERE path LIKE '0#1#%' OR path LIKE '0#3#%'";
            DataTable product_data = GetProgramManager().GetDataManager().GetTableData(sql);
            if (product_data == null || product_data.Rows.Count == 00)
                return;

            Hashtable hIds = new Hashtable();
            foreach (DataGridViewRow r in m_Grid.Rows)
            {
                string res= r.Cells[0].Value.ToString().ToLower();
                if (res != "true")
                    continue;
                int idd= int.Parse(r.Cells[1].Value.ToString().ToLower());
                hIds.Add(idd, idd);
            }
            DataTable data_stores = GetProgramManager().GetDataManager().GetTableData("SELECT id,name FROM book.Stores");
            

            m_GridProducts.Rows.Clear();
            double sum_total = 0;
            double vat_total = 0;
            int product_data_cnt = 0;
            foreach (DataRow row in product_data.Rows)
            {
                product_data_cnt++;
                int product_id = int.Parse(row["id"].ToString());
                if (hBadProductIds.ContainsKey(product_id))
                    continue;
                //if (product_id != 888)
               // {
                //    continue;
               // }

                string product_name = GetProgramManager().GetDataManager().GetProductNameByID(product_id);
                string product_code = GetProgramManager().GetDataManager().GetProductCode(product_id);
                int vat_type = GetProgramManager().GetDataManager().GetProductVatType(product_id);
                if (vat_type != 1)
                    continue;
      //      foreach (DataRow st_row in data_stores.Rows)
       //        {
                    ArrayList in_ids = new ArrayList();
                    ArrayList self_costs = new ArrayList();
                    ArrayList amounts = new ArrayList();
                    ArrayList new_in_ids = new ArrayList();
                    ArrayList new_self_costs = new ArrayList();
                    ArrayList new_stores = new ArrayList();
                    ArrayList new_amounts = new ArrayList();
                    string store_name = "";// st_row["name"].ToString();
                    int store_id = 0;// int.Parse(st_row["id"].ToString());
                    int n = GetProductDetailedDocs(tdate, m_Picker.dtp_From.Value, product_id, store_id, ref in_ids, ref self_costs, ref amounts);
                    if (n == 0)
                        continue;

                    double full_rest = GetProgramManager().GetDataManager().GetProductRest(product_id.ToString(), store_id.ToString(), tdate, 0, 0);
                    double f_rest = 0;
                for(int i=0;i<n;i++)
                {
                    f_rest += (double )amounts[i];

                }

                    string sql2= "SELECT res.id, res.sum_amount FROM  (SELECT st.id, "+
                        "ISNULL(( SELECT SUM(a.amount*a.coeff) AS sum_amount"+
                                 " FROM   doc.ProductsFlow a "+
                                         "INNER JOIN doc.GeneralDocs c ON a.general_id = c.id "+
                                 " WHERE  c.tdate < '"+tdate.ToString("yyyy-MM-dd HH:mm:ss.fff")+"'"+
									"	AND a.is_order = 0 AND a.is_expense = 0 "+
										"AND a.product_id = "+product_id+
                                        " AND a.store_id = st.id ),0) AS sum_amount"+ 
                        
                        "  FROM book.Stores st ) res WHERE res.sum_amount>0";

                    DataTable rest_data= GetProgramManager().GetDataManager().GetTableData(sql2);
                   // int cc = 0; double ssum = 0;
                
                    double f2_rest = 0;
                    foreach (DataRow r in rest_data.Rows)
                    {
                        double st_amount = double.Parse(r["sum_amount"].ToString());
                        f2_rest += st_amount;

                    }
                    if (f_rest != full_rest||f2_rest!=f_rest)
                    {
                        //int k = 0;
                        full_rest = 0;

                    }

                    int active_storeindex = 0;
                       

                        double am = (double)amounts[0];
                        double store_amount=  double.Parse(rest_data.Rows[active_storeindex]["sum_amount"].ToString());
                        int p = 0;
                        int id34 = 0;
                    while(id34==0)
                    {
                        if (am < store_amount)
                        {
                            new_stores.Add(int.Parse(rest_data.Rows[active_storeindex]["id"].ToString()));
                            new_self_costs.Add((double)self_costs[p]);
                            new_amounts.Add(am);
                            new_in_ids.Add((int)in_ids[p]);
                            store_amount -= am;
                            p++;
                            if (p >= amounts.Count)
                                break;
                            am = (double)amounts[p];
                            continue;
                        }
                        else if (am > store_amount)
                        {
                            new_stores.Add(int.Parse(rest_data.Rows[active_storeindex]["id"].ToString()));
                            new_self_costs.Add((double)self_costs[p]);
                            new_amounts.Add(store_amount);
                            new_in_ids.Add((int)in_ids[p]);
                            active_storeindex++;
                            if (active_storeindex >= rest_data.Rows.Count)
                                break;
                            am -= store_amount;
                            store_amount = double.Parse(rest_data.Rows[active_storeindex]["sum_amount"].ToString());
                            continue;
                        }
                        else if (am == store_amount)
                        {
                            new_stores.Add(int.Parse(rest_data.Rows[active_storeindex]["id"].ToString()));
                            new_self_costs.Add((double)self_costs[p]);
                            new_amounts.Add(am);
                            new_in_ids.Add((int)in_ids[p]);
                            p++;
                            if (p >= amounts.Count)
                                break;
                            am = (double)amounts[p];
                            
                            active_storeindex++;
                            if (active_storeindex >= rest_data.Rows.Count)
                                break;
                            store_amount = double.Parse(rest_data.Rows[active_storeindex]["sum_amount"].ToString());
                            
                            continue;
                        }

                    }
                /*
                foreach(DataRow r in rest_data.Rows)
                    {
                        int st_id=int.Parse(r["id"].ToString());
                        double st_amount=double.Parse(r["sum_amount"].ToString());
                        if (st_amount <= 0)
                            continue;
                        ssum += st_amount;
                        if (amounts.Count == cc)
                            break;
                        if (st_amount <= (double)amounts[cc])
                        {
                            new_stores.Add(st_id);
                            new_self_costs.Add((double)self_costs[cc]);
                            new_amounts.Add((double)amounts[cc]);
                            new_in_ids.Add((int)in_ids[cc]);
                            if (st_amount == (double)amounts[cc])
                                cc++;
                        }
                        else if (st_amount > (double)amounts[cc])
                        {
                            while (st_amount > (double)amounts[cc])
                            {
                                new_stores.Add(st_id);
                                new_self_costs.Add((double)self_costs[cc]);
                                new_amounts.Add((double)amounts[cc]);
                                new_in_ids.Add((int)in_ids[cc]);
                                st_amount -= (double)amounts[cc];
                                cc++;
                                if (amounts.Count == cc)
                                    break;
                                if (st_amount <= (double) amounts[cc])
                                    break;
                            }
                        }
                        
                        
                    }
                */
                    int n2 = new_in_ids.Count;
                    for (int i = 0; i < n2; i++)
                    {
                        int general_id = (int)new_in_ids[i];
                        if (!hIds.ContainsKey(general_id))
                            continue;
                        sql = "SELECT gd.id, gd.tdate, doc.doc1, doc.doc2, c.name, gd.purpose,currency_id FROM doc.GeneralDocs gd INNER JOIN doc.ProductIn doc ON doc.general_id=gd.id  INNER JOIN book.Contragents c ON c.id=gd.param_id1 WHERE gd.id=" + general_id;
                        DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);
                        if (data == null || data.Rows.Count == 0)
                            continue;
                        DataRow row2 = data.Rows[0];
                        store_id = (int)new_stores[i];
                        store_name = GetProgramManager().GetDataManager().GetStoreNameByID(store_id);
                        double am2 = (double)new_self_costs[i];
                        double quantity = (double)new_amounts[i];
                        int currency_id = int.Parse(row2["currency_id"].ToString());
                        double total = am2 * quantity;
                        sum_total += total;
                        double vat = 0;
                        if(currency_id==1)
                            vat = total - total / 1.18;
                        else
                            vat = total*0.18;
                        vat_total += vat;
                        m_GridProducts.Rows.Add(row2["id"].ToString(), row2["tdate"].ToString(), row2["doc1"].ToString(), row2["doc2"].ToString(), row2["name"].ToString(), store_id, store_name, product_id, product_code, product_name, quantity, am2, total, vat);

                    }
                    for (int i = 0; i < n2; i++)
                    {
                        int general_id = (int)new_in_ids[i];
                        if (!hIds.ContainsKey(general_id))
                            continue;
                        sql = "SELECT gd.id, gd.tdate, '' AS doc1, '' AS doc2, '' AS name, gd.purpose,currency_id FROM doc.GeneralDocs gd INNER JOIN doc.ProductStart doc ON doc.general_id=gd.id  WHERE gd.id=" + general_id;
                        DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);
                        if (data == null || data.Rows.Count == 0)
                            continue;
                        DataRow row2 = data.Rows[0];
                        store_id = (int)new_stores[i];
                        store_name = GetProgramManager().GetDataManager().GetStoreNameByID(store_id);
                        double am2 = (double)new_self_costs[i];
                        double quantity = (double)new_amounts[i];
                        int currency_id = int.Parse(row2["currency_id"].ToString());
                        double total = am2 * quantity;
                        sum_total += total;
                        double vat = 0;
                        if (currency_id == 1)
                            vat = total - total / 1.18;
                        else
                            vat = total * 0.18;
                        vat_total += vat;
                        m_GridProducts.Rows.Add(row2["id"].ToString(), row2["tdate"].ToString(), row2["doc1"].ToString(), row2["doc2"].ToString(), row2["name"].ToString(), store_id, store_name, product_id, product_code, product_name, quantity, am2, total, vat);

                    }
                /*
                    
                    for (int i = 0; i < n; i++)
                    {
                        int general_id = (int)in_ids[i];
                        if (!hIds.ContainsKey(general_id))
                            continue;
                        sql = "SELECT gd.id, gd.tdate, doc.doc1, doc.doc2, c.name, gd.purpose FROM doc.GeneralDocs gd INNER JOIN doc.ProductIn doc ON doc.general_id=gd.id  INNER JOIN book.Contragents c ON c.id=gd.param_id1 WHERE gd.id=" + general_id;
                        DataTable data = GetProgramManager().GetDataManager().GetTableData(sql);
                        if (data == null || data.Rows.Count == 0)
                            continue;
                        DataRow row2 = data.Rows[0];
                        double am = (double)self_costs[i];
                        double quantity = (double)amounts[i];
                        double total = am * quantity;
                        sum_total += total;
                        double vat = total - total / 1.18;
                        vat_total += vat;
                        m_GridProducts.Rows.Add(row2["id"].ToString(), row2["tdate"].ToString(), row2["doc1"].ToString(), row2["doc2"].ToString(), row2["name"].ToString(), store_id, store_name, product_id, product_code, product_name, quantity, am, total, vat);

                    }
                      
               }*/
            }
            labelSum.Text = "ჯამი: " + sum_total.ToString("F2") + ", დღგ: " + vat_total.ToString("F2");
            
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            

        }

        private bool OnGenerate()
        {
            if (m_GridProducts.Rows.Count == 0)
                return false;
            int doc_type = 88;
            DateTime tdate = new DateTime(m_Picker.dtp_To.Value.Year, m_Picker.dtp_To.Value.Month, m_Picker.dtp_To.Value.Day, 23, 55, 50);
            tdate = tdate.AddDays(-1);
            int general_id = GetProgramManager().GetDataManager().Insert_GeneralDocs(
    tdate,
    "",
    0,
    doc_type,
    "დღგ–ს გამოყოფა",
    0,
    1,
    1,
    0,
    GetProgramManager().GetUserID(),
    0,
    0,
    0, 1, true, 1,1,0);
            if (general_id <= 0)
                return false;

            string sqlText = "INSERT INTO doc.Special (general_id,type) VALUES(" + general_id.ToString() + ", 0)";

            if (!GetProgramManager().GetDataManager().ExecuteSql(sqlText))
                return false;
           // int entry_id = GetProgramManager().GetDataManager().GetUniqueEntryID();
            double sum = 0;
            foreach (DataGridViewRow r in m_GridProducts.Rows)
            {
                int product_id = int.Parse(r.Cells["ColProductID"].Value.ToString());
                int store_id = int.Parse(r.Cells["ColStoreID"].Value.ToString());
                int project_id = GetProgramManager().GetDataManager().GetStoreProjectID(store_id);
                int unit_id = GetProgramManager().GetDataManager().GetProductUnitID(product_id);
                double quantity = double.Parse(r.Cells["ColProductQuantity"].Value.ToString());
                double price = double.Parse(r.Cells["ColProductPrice"].Value.ToString());
                double total = quantity * price;
                double vat = double.Parse(r.Cells["ColProductVat"].Value.ToString());// total - total / 1.18;
                string productAccount = GetProgramManager().GetDataManager().GetProductAccount(product_id);

                if (!GetProgramManager().GetDataManager().Insert_ProductsFlow(
                       product_id, "", general_id,
                       1, unit_id, -vat, store_id, tdate, 0,
                       1, 0, 1, 0, 1, 1, -vat, 0, 0, 0, "", 0, 0, 0, 0, ""))
                    return false;

                if (!GetProgramManager().GetDataManager().Insert_Entries(
                                 general_id,
                                 "3340",
                                 productAccount,
                                 vat,
                                 1,
                                 0,
                                 0,
                                 0,
                                 0,
                                 0,
                                 product_id,
                                 store_id,
                                 0,
                                 "", project_id, 1))
                    return false;
                sum += vat;

            }

            GetProgramManager().GetDataManager().UpdateGeneralDocAmount(general_id, sum);
            Close();
            return true;

        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            OnSearchDocuments();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            OnCalculate();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            bool res= OnGenerate();
            if (res)
                MessageBox.Show("დარიცხვის ოპერაცია წარმატებით შესრულდა", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Question);
            else
                MessageBox.Show("დარიცხვის ოპერაცია  ვერ შესრულდა", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void m_GridProducts_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
               
                case Keys.Control | Keys.C:
                    {
                        GetProgramManager().CopyDataToClipboard(m_GridProducts);
                        
                        break;
                    }
               
            }
       
        }

        private void m_Grid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {

                case Keys.Control | Keys.C:
                    {
                        GetProgramManager().CopyDataToClipboard(m_Grid);

                        break;
                    }

            }
       
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            CheckDocs();           
        }
        private void CheckDocs()
        {
            foreach (DataGridViewRow row in m_Grid.SelectedRows)
            {
                row.Cells[0].Value = true;
            }

        }
        private void UnCheckDocs()
        {
            foreach (DataGridViewRow row in m_Grid.SelectedRows)
            {
                row.Cells[0].Value = false;
            }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            UnCheckDocs();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckDocs();

        }
    }
}
