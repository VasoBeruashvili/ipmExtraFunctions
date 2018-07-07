using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ipmPMBasic;
using System.Collections;
using ipmControls;

namespace ipmExtraFunctions
{
    public partial class ProfitabilityDetailForm : Form
    {
        private ProgramManagerBasic m_Pm;
        Hashtable m_sqlParams = new Hashtable();



        public ProfitabilityDetailForm(ProgramManagerBasic pm)
            : this(pm, 0, DateTime.Now, DateTime.Now)
        {
            
        }


        public ProfitabilityDetailForm(ProgramManagerBasic pm,  int  product_id, DateTime fromDate, DateTime todate)
        {
            m_Pm = pm;
            InitializeComponent();
            this.m_Pm.TranslateControl(this);
            m_Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
            UpdateControls(product_id, fromDate, todate);
        }


        #region Methods

        private ProgramManagerBasic GetProgramManager()
        {
            return this.m_Pm;
        }
        private void UpdateControls(int id, DateTime date1, DateTime date2)
        {
            m_Period.dtp_From.Value = date1;
            m_Period.dtp_To.Value = date2;
            updatePeriod();
            if (id <= 0)
                return;
            btnProduct.Tag = id;
            txtCode.Text = GetProgramManager().GetDataManager().GetProductCode(id);
            txtName.Text = GetProgramManager().GetDataManager().GetProductNameByID(id);
        }

        private void updatePeriod()
        {
            lblMonts.Text = (12 * (m_Period.dtp_To.Value.Year - m_Period.dtp_From.Value.Year) + m_Period.dtp_To.Value.Month - m_Period.dtp_From.Value.Month +1).ToString();

        }
        private void updateGrid()
        {
            string product_id=Convert.ToString(btnProduct.Tag);
            if(string.IsNullOrEmpty(product_id))
                return;
            string sql = @"
           SELECT           
 res.id,
 ROUND(ISNULL(CASE WHEN res.TotalSelfCost>0 THEN res.TotalSelfCost ELSE res.selfCostLast END /NULLIF(res.ToTalAmount,0),0),2) AS SelfCost, 
 ROUND(ISNULL(CASE WHEN res.TotalSelfCost>0 THEN res.TotalSelfCost ELSE res.selfCostLast END /NULLIF(res.ToTalAmount,0),0) * CASE WHEN res.vat>0 THEN 1.18 ELSE 1 END ,2) AS SelfCostPlusVat, 
 ROUND(ISNULL((res.SacaloPlusVat / CASE WHEN res.vat>0 THEN 1.18 ELSE 1 END),0),2) AS SacaloNoVat,
 ROUND(ISNULL((res.FasdaklebaPlusVat / CASE WHEN res.vat>0 THEN 1.18 ELSE 1 END),0),2) AS FasdaklebaNoVat,
 ROUND(ISNULL((res.SabitumoPlusVat / CASE WHEN res.vat>0 THEN 1.18 ELSE 1 END),0),2) AS SabitumoNoVat,
 ROUND(ISNULL((res.SxvaPlusVat / CASE WHEN res.vat>0 THEN 1.18 ELSE 1 END),0),2) AS SxvaNoVat,
 ROUND(ISNULL(res.StartRest,0),0) AS StartRest,
 ROUND(ISNULL(CASE WHEN res.TotalSelfCost>0 THEN res.TotalSelfCost ELSE res.selfCostLast END /NULLIF(res.ToTalAmount,0)*res.StartRest,0),2) AS StartAmount, 
 ROUND(ISNULL(res.ToTalAmount,0),0) AS EndRest,
 ROUND(ISNULL(CASE WHEN res.TotalSelfCost>0 THEN res.TotalSelfCost ELSE res.selfCostLast END /NULLIF(res.ToTalAmount,0)*res.ToTalAmount,0),2) AS EndAmount,
 ROUND(ISNULL(res.averageRest,0),0) AS AverageRest,
 ROUND(ISNULL(CASE WHEN res.TotalSelfCost>0 THEN res.TotalSelfCost ELSE res.selfCostLast END/NULLIF(res.ToTalAmount,0)*res.averageRest,0),2) AS AverageAmount
 
 FROM
 (
 SELECT Products.id,Products.code,  products.name, Products.vat,
( SELECT SUM(a.amount*a.self_cost*a.coeff)
                                 FROM   doc.ProductsFlow a 
                                        INNER JOIN doc.GeneralDocs c ON a.general_id = c.id
                                 WHERE  c.tdate < @enddate
										AND a.is_order = 0
										AND a.product_id = Products.id) AS TotalSelfCost, 
 

( SELECT TOP 1 pf.self_cost
                            + ISNULL((SELECT SUM(p.self_cost) FROM doc.ProductsFlow p WHERE is_expense=1 AND p.ref_id=pf.id ),0) 
                            + ISNULL((SELECT SUM(p.self_cost) FROM doc.ProductsFlow p WHERE p.is_expense=1 AND p.coeff=1 AND p.product_id =Products.id AND p.general_id=pf.general_id),0) AS self_cost
                             FROM doc.ProductsFlow pf INNER JOIN doc.GeneralDocs c ON c.id = pf.general_id WHERE pf.product_id=Products.id AND coeff=1 AND is_expense=0  AND is_move=0  AND is_order=0 ORDER BY c.tdate DESC )
AS  selfCostLast,

 (SELECT SUM(a.amount*a.coeff)
                                 FROM   doc.ProductsFlow a 
                                        INNER JOIN doc.GeneralDocs c ON a.general_id = c.id
                                        INNER JOIN book.Products p ON p.id = a.product_id
                                 WHERE  c.tdate <  @enddate
										AND a.is_order = 0  AND a.is_expense = 0
										AND a.product_id = Products.id) AS ToTalAmount,

 (SELECT manual_val FROM book.ProductPrices WHERE price_id =3 AND product_id =Products.id) AS SacaloPlusVat,
 (SELECT manual_val FROM book.ProductPrices WHERE price_id =4 AND product_id =Products.id) AS FasdaklebaPlusVat,
 (SELECT manual_val FROM book.ProductPrices WHERE price_id =5 AND product_id =Products.id) AS SabitumoPlusVat,
 (SELECT manual_val FROM book.ProductPrices WHERE price_id =6 AND product_id =Products.id) AS SxvaPlusVat,
 (SELECT SUM(a.amount*a.coeff) FROM   doc.ProductsFlow a 
                     INNER JOIN doc.GeneralDocs c ON a.general_id = c.id
                     INNER JOIN book.Products p ON p.id = a.product_id
                     WHERE  c.tdate <  @startdate AND a.is_order = 0 
                     AND a.is_expense=0 AND p.id=Products.id) AS StartRest, 
                     
 (SELECT ISNULL(AVG(Res.rest),0)   FROM(
                                SELECT  r.rest_date, r.product_id,   sum(r.rest) AS rest
                                FROM book.AverageRest AS r
                                INNER JOIN book.Products as Products ON Products.id=r.product_id
                                group by r.product_id, r.rest_date) as res
                                WHERE res.product_id=Products.id and res.rest_date BETWEEN  @startdate AND @enddate
                                group by res.product_id) as averageRest

 FROM book.Products as Products  where Products.id=@ProductsID

)AS RES";
            m_sqlParams.Clear();
            m_sqlParams.Add("@startdate", m_Period.dtp_From.Value.ToString("yyyy-MM-dd 00:00:00"));
            m_sqlParams.Add("@enddate", m_Period.dtp_To.Value.ToString("yyyy-MM-dd 23:59:59"));
            m_sqlParams.Add("@ProductsID", product_id);
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql, m_sqlParams);
            if (data == null || data.Rows.Count <= 0)
            {
                ProgressDispatcher.Deactivate();
                return;
            }
            m_Grid.Rows.Clear();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                m_Grid.Rows.Add(
                    data.Rows[i]["id"].ToString(),
                    data.Rows[i]["SelfCost"].ToString(),
                    data.Rows[i]["SelfCostPlusVat"].ToString(),
                    data.Rows[i]["SacaloNoVat"].ToString(),
                    data.Rows[i]["FasdaklebaNoVat"].ToString(),
                    data.Rows[i]["SabitumoNoVat"].ToString(),
                    data.Rows[i]["SxvaNoVat"].ToString(),
                    data.Rows[i]["StartRest"].ToString(),
                    data.Rows[i]["StartAmount"].ToString(),
                    data.Rows[i]["EndRest"].ToString(),
                    data.Rows[i]["EndAmount"].ToString(),
                    data.Rows[i]["AverageRest"].ToString(),
                    data.Rows[i]["AverageAmount"].ToString()
                    );
             }

        }

        private void updateGrid2()
        {
            if (m_Grid.Rows.Count <= 0)
                return;
            m_Grid2.Rows.Clear();
            int product_id=Convert.ToInt32(m_Grid.Rows[0].Cells[col_id.Index].Value);
            double selfCost_noVat = Convert.ToDouble(m_Grid.Rows[0].Cells[col_selfCostNoVat.Index].Value);
            double selfCost_plusVat = Convert.ToDouble(m_Grid.Rows[0].Cells[col_selfCostVat.Index].Value);
            double sacalo = Convert.ToDouble(m_Grid.Rows[0].Cells[col_outPriceSingle.Index].Value);
            double fasdakleba = Convert.ToDouble(m_Grid.Rows[0].Cells[col_outPriceDiscount.Index].Value);
            double sabitumo = Convert.ToDouble(m_Grid.Rows[0].Cells[col_outPriceSabitumo.Index].Value);
            double sxva = Convert.ToDouble(m_Grid.Rows[0].Cells[col_outPriceOther.Index].Value);
            double start_quantity = Convert.ToDouble(m_Grid.Rows[0].Cells[col_restPeriodStart.Index].Value);
            double start_amount = Convert.ToDouble(m_Grid.Rows[0].Cells[col_amountPeriodStart.Index].Value);
            double end_quantity = Convert.ToDouble(m_Grid.Rows[0].Cells[col_restPeriodEnd.Index].Value);
            double end_amount = Convert.ToDouble(m_Grid.Rows[0].Cells[col_amountPeriodEnd.Index].Value);
            double average_quantity = Convert.ToDouble(m_Grid.Rows[0].Cells[col_quantityAverageRest.Index].Value);
            double average_amount = Convert.ToDouble(m_Grid.Rows[0].Cells[col_amountAverageRest.Index].Value);
           
            double outQuantity_sacalo = GetProgramManager().GetDataManager().GetProductOutQuantityByPriceID(product_id, 3, m_Period.dtp_From.Value, m_Period.dtp_To.Value);
            double outQuantity_fsdakleba = GetProgramManager().GetDataManager().GetProductOutQuantityByPriceID(product_id, 4, m_Period.dtp_From.Value, m_Period.dtp_To.Value);
            double outQuantity_sabitumo = GetProgramManager().GetDataManager().GetProductOutQuantityByPriceID(product_id, 5, m_Period.dtp_From.Value, m_Period.dtp_To.Value);
            //double outQuantity_sum = GetProgramManager().GetDataManager().GetProductOutQuantityAllMainPrice(product_id,  m_Period.dtp_From.Value, m_Period.dtp_To.Value);
            //double outQuantity_other = (outQuantity_sum - (outQuantity_sacalo + outQuantity_fsdakleba + outQuantity_sabitumo)) < 0 ? 0 : outQuantity_sum - (outQuantity_sacalo + outQuantity_fsdakleba + outQuantity_sabitumo);
            double outQuantity_other =GetProgramManager().GetDataManager().GetProductOutQuantityByPriceID(product_id, 6, m_Period.dtp_From.Value, m_Period.dtp_To.Value);
            double outQuantity_sum = outQuantity_sacalo + outQuantity_fsdakleba + outQuantity_sabitumo + outQuantity_other;
            m_Grid2.Rows.Add("რაოდენობა", outQuantity_sacalo.ToString("F0"), outQuantity_fsdakleba.ToString("F0"), outQuantity_sabitumo.ToString("F0"), outQuantity_other.ToString("F0"), outQuantity_sum.ToString("F0"));

            double ProdyctOutPrice_sacalo = GetProgramManager().GetDataManager().GetProductPriceByID(product_id.ToString(), "3");
            double ProdyctOutPrice_fsdakleba = GetProgramManager().GetDataManager().GetProductPriceByID(product_id.ToString(), "4");
            double ProdyctOutPrice_sabitumo = GetProgramManager().GetDataManager().GetProductPriceByID(product_id.ToString(), "5");
            double ProductOutPrice_other = GetProgramManager().GetDataManager().GetProductPriceByID(product_id.ToString(), "6");
            double ProdyctOutPrice_Sum = GetProgramManager().GetDataManager().GetProductOutAmount(product_id, 0, m_Period.dtp_From.Value, m_Period.dtp_To.Value);
          
          
            double outNovat_sacalo=outQuantity_sacalo * sacalo;
            double outNovat_fasdakleba=outQuantity_fsdakleba * fasdakleba;
            double outNovat_sabitumo=outQuantity_sabitumo * sabitumo;
            double outNovat_other = outQuantity_other * sxva;
            double outNovat_sum = outNovat_sacalo + outNovat_fasdakleba + outNovat_sabitumo + outNovat_other;//ProdyctOutPrice_Sum / (GetProgramManager().GetDataManager().GetProductVat(product_id) ? 1.18 : 1);
          //  double outNovat_other = outNovat_sum - (outNovat_sacalo + outNovat_fasdakleba + outNovat_sabitumo);

            //double ProductOutPrice_other = outNovat_other * (GetProgramManager().GetDataManager().GetProductVat(product_id) ? 1.18 : 1) / outQuantity_other;
            // double realizacia_noVatSum= outQuantity_sacalo * sacalo+ outQuantity_fsdakleba * fasdakleba+ outQuantity_sabitumo * sabitumo+ outQuantity_other * ProdyctOutPrice_sxvayvelaNoVat;

            m_Grid2.Rows.Add("ფასი დღგ-ს ჩათვლით", ProdyctOutPrice_sacalo.ToString("F0"), ProdyctOutPrice_fsdakleba.ToString("F0"), ProdyctOutPrice_sabitumo.ToString("F0"), ProductOutPrice_other.ToString("F0"), "");
            m_Grid2.Rows.Add("რეალიზაცია დღგ-ს გარეშე", outNovat_sacalo.ToString("F1"), outNovat_fasdakleba.ToString("F1"), outNovat_sabitumo.ToString("F1"), outNovat_other.ToString("F1"), outNovat_sum.ToString("F1"));

            m_Grid2.Rows.Add("თვითღირებ. დღგ-ს გარეშე", (outQuantity_sacalo * selfCost_noVat).ToString("F1"), (outQuantity_fsdakleba * selfCost_noVat).ToString("F1"), (outQuantity_sabitumo * selfCost_noVat).ToString("F1"), (outQuantity_other * selfCost_noVat).ToString("F1"), (outQuantity_sum * selfCost_noVat).ToString("F1"));
            double mogeba_sacalo = outNovat_sacalo - (outQuantity_sacalo * selfCost_noVat);
            double mogeba_fasdakleba = outNovat_fasdakleba - (outQuantity_fsdakleba * selfCost_noVat);
            double mogeba_sabitumo = outNovat_sabitumo - (outQuantity_sabitumo * selfCost_noVat);
            double mogeba_other = outNovat_other - (outQuantity_other * selfCost_noVat);
           double mogeba_sum=mogeba_sacalo+mogeba_fasdakleba+mogeba_sabitumo+mogeba_other;
           m_Grid2.Rows.Add("მოგება მთელ პერიოდში", mogeba_sacalo.ToString("F1"), mogeba_fasdakleba.ToString("F1"), mogeba_sabitumo.ToString("F1"), mogeba_other.ToString("F1"), mogeba_sum.ToString("F1"));
           int date_diff = Convert.ToInt32(12 * (m_Period.dtp_To.Value.Year - m_Period.dtp_From.Value.Year) + m_Period.dtp_To.Value.Month - m_Period.dtp_From.Value.Month + 1);

           m_Grid2.Rows.Add("საშ. მოგება ერთ თვეზე", (mogeba_sacalo / date_diff).ToString("F1"), (mogeba_fasdakleba / date_diff).ToString("F1"), (mogeba_sabitumo / date_diff).ToString("F1"), (mogeba_other / date_diff).ToString("F1"), (mogeba_sum / date_diff).ToString("F1"));
           double average_rest_sacalo=outQuantity_sacalo / outQuantity_sum * average_amount;
           double average_rest_fasdakleba=outQuantity_fsdakleba / outQuantity_sum * average_amount;
           double average_rest_sabitumo=outQuantity_sabitumo / outQuantity_sum * average_amount;
           double average_rest_other=outQuantity_other / outQuantity_sum * average_amount;
           double average_rest_sum=average_rest_sacalo+average_rest_fasdakleba+average_rest_sabitumo+average_rest_other;
           m_Grid2.Rows.Add("საშუალო ნაშთი", average_rest_sacalo.ToString("F1"), average_rest_fasdakleba.ToString("F1"), average_rest_sabitumo.ToString("F1"), average_rest_other.ToString("F1"), average_rest_sum.ToString("F1"));
           double gainPercent_sacalo = mogeba_sacalo / outNovat_sacalo * 100;
           double gainPercent_fasdakleba = mogeba_fasdakleba / outNovat_fasdakleba * 100;
           double gainPercent_sabitumo = mogeba_sabitumo / outNovat_sabitumo * 100;
           double gainPercent_sxva = mogeba_other / outNovat_other * 100;
           double gainPercent_sum = mogeba_sum / outNovat_sum * 100;
           m_Grid2.Rows.Add("მოგების % რეალიზ.-დან", gainPercent_sacalo.ToString("F1"), gainPercent_fasdakleba.ToString("F1"), gainPercent_sabitumo.ToString("F1"), gainPercent_sxva.ToString("F1"), gainPercent_sum.ToString("F1"));

           double rentabeloba_Sacalo = (mogeba_sacalo / date_diff) / average_rest_sacalo * 100;
           double rentabeloba_fasdakleba = (mogeba_fasdakleba / date_diff) / average_rest_fasdakleba * 100;
           double rentabeloba_sabitumo = (mogeba_sabitumo / date_diff) / average_rest_sabitumo * 100;
           double rentabeloba_other = (mogeba_other / date_diff) / average_rest_other * 100;
           double rentabeloba_sum = (mogeba_sum / date_diff) / average_rest_sum * 100;
           m_Grid2.Rows.Add("რენტაბელობა %", rentabeloba_Sacalo.ToString("F1"), rentabeloba_fasdakleba.ToString("F1"), rentabeloba_sabitumo.ToString("F1"), rentabeloba_other.ToString("F1"), rentabeloba_sum.ToString("F1"));
           for (int i = 1; i < m_Grid2.Columns.Count; i++)
           {
               for (int j = 1; j < m_Grid2.Rows.Count; j++)
               {
                   double tmp = 0;
                   double.TryParse(Convert.ToString(m_Grid2.Rows[j].Cells[i].Value), out tmp);
                   m_Grid2.Rows[j].Cells[i].Value = Math.Round(tmp, 2);
               }
           }

        }

        private void updateGridColor()
        {
            if (m_Grid2.Rows.Count <= 0)
                return;
            for (int i = 1; i < m_Grid2.Columns.Count; i++)
            {
                m_Grid2.Rows[8].Cells[i].Style.BackColor = GetColorByVal(Convert.ToDouble(m_Grid2.Rows[8].Cells[i].Value));
            }
        }

        private Color GetColorByVal(double val)
        {

            if (val < 10)
                return Color.Red;
            else if (val >= 10 && val < 12)
                return Color.Yellow;
            else if (val >= 12)
                return Color.Green;
            else return Color.Empty;
        }
        private void updateGridAligment()
        {
            foreach (DataGridViewColumn col in m_Grid.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (col.Index == 0) 
                    continue;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            foreach (DataGridViewColumn col in m_Grid2.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                if (col.Index == 0)
                    continue;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }



        }

        private void onRefresh()
        {
            ProgressDispatcher.Activate();
            updatePeriod();
            updateGrid();
            updateGrid2();
            updateGridColor();
            updateGridAligment();
            ProgressDispatcher.Deactivate();
        }

        private void onProductList()
        {
             int res = GetProgramManager().ShowSelectForm("TABLE_PRODUCT:PRODUCT", 0);
             if (res <= 0)
                 return;
             btnProduct.Tag = res;
             txtCode.Text = GetProgramManager().GetDataManager().GetProductCode(res);
             txtName.Text = GetProgramManager().GetDataManager().GetProductNameByID(res);
        }

        

       



        #endregion








        #region events

        

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            onRefresh();
        }
       
        private void btnProduct_Click(object sender, EventArgs e)
        {
            onProductList();
        }


        #endregion

        private void m_Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

      

      

        
    }
}
