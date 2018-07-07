using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ipmPMBasic;
using System.Collections;
using ipmControls;


namespace ipmExtraFunctions
{
    public partial class ProfitabilitySubForm : Form
    {
        private ProgramManagerBasic m_Pm;
        Hashtable m_sqlParams = new Hashtable();

        public ProfitabilitySubForm(ProgramManagerBasic pm)
            : this(pm, string.Empty, DateTime.Now, DateTime.Now)
        {
            
        }


        public ProfitabilitySubForm(ProgramManagerBasic pm, string path, DateTime fromDate, DateTime Todate)
        {
             m_Pm = pm;
            InitializeComponent();
            this.m_Pm.TranslateControl(this);
           
            m_Period.dtp_From.Value = fromDate;
            m_Period.dtp_To.Value = Todate;
            if (!string.IsNullOrEmpty(path))
            {
                btnGroup.Tag = path;
                m_sqlParams.Clear();
                m_sqlParams.Add("@PATH",path);
                txtGroup.Text = GetProgramManager().GetDataManager().GetStringValue("SELECT name FROM book.GroupProducts WHERE path=@PATH", m_sqlParams);
            }
            m_Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
            
        }

        #region Methods

        private ProgramManagerBasic GetProgramManager()
        {
            return this.m_Pm;
        }
       

        private void updateGrid()
        {
            string path = Convert.ToString(btnGroup.Tag);
            if (string.IsNullOrEmpty(path))
                return;
            string sql = @"
            SELECT           
 res.id,
 res.code,         
 RES.name,
ISNULL(RES.outQuantity,0)AS outQuantity,
 ISNULL(RES.outAmountNoVat,0)AS outAmountNoVat,
ISNULL(RES.outAmountNoVatSingleOrDiscount/NULLIF(RES.outAmountNoVat,0) * 100  ,0) AS outSingleDiscountPercent,
ISNULL(RES.outAmountNoVatSaleOrOther/NULLIF(RES.outAmountNoVat,0) * 100  ,0) AS outOtherPercent,
ISNULL(RES.outAmountNoVatSingleOrDiscount,0) AS outSingleDiscount,
ISNULL(RES.outAmountNoVatSaleOrOther,0) AS outSaleOther,
ISNULL(res.outAmountNoVat-res.selfCost,0) AS gain,
 ISNULL((res.outAmountNoVat-res.selfCost)/(DATEDIFF(MONTH, @startdate, @enddate)+1),0) AS averageGain,
 ISNULL((res.outAmountNoVat-res.selfCost)/NULLIF(res.outAmountNoVat,0)*100,0) AS gainFromOutPerc,
 ISNULL(res.outAmountNoVat/NULLIF(res.outAmountNoVatSum,0) *100,0) AS shareFromOut,
 ISNULL(((res.outAmountNoVat-res.selfCost)/(DATEDIFF(MONTH, @startdate, @enddate)+1))/(NULLIF(res.average,0) *  CASE  WHEN res.selfCostOne>0 THEN res.selfCostOne ELSE res.selfCostLast END) *100,0) AS profitability,
 ISNULL(RES.average * CASE WHEN res.selfCostOne>0 THEN res.selfCostOne ELSE res.selfCostLast END ,0) as averageRest

FROM
(
SELECT Products.id, Products.code, Products.name,
(SELECT ISNULL(SUM(s.outQuantity),0) FROM (SELECT ISNULL(ROUND(( SELECT SUM(b.amount*(-b.coeff))
                                 FROM   doc.ProductsFlow b 
                                 INNER JOIN doc.GeneralDocs c ON b.general_id = c.id
                                 WHERE  c.tdate BETWEEN @startdate AND @enddate
                                        AND ( c.doc_type=21 OR c.doc_type=23) 
										AND b.product_id = a.id AND b.coeff <=0
										AND b.is_order = 0 AND b.is_expense = 0
										),6), 0) AS outQuantity
                                FROM    book.Products a WHERE a.id = Products.id) AS s) AS outQuantity,

(SELECT ISNULL(SUM(s.outAmount),0) FROM (SELECT ISNULL(ROUND(( SELECT SUM(b.amount*b.price*c.rate*100/(b.vat_percent + 100))
                                 FROM   doc.ProductsFlow b 
                                 INNER JOIN doc.GeneralDocs c ON b.general_id = c.id
                                 WHERE  c.tdate BETWEEN @startdate AND @enddate
                                        AND ( c.doc_type=21 OR c.doc_type=23) 
										AND b.product_id = a.id AND b.coeff <=0 AND b.is_order = 0 
										 ),6), 0) AS outAmount
                                FROM    book.Products a WHERE a.id = Products.id) AS s)AS outAmountNoVat,
                                

(SELECT ISNULL(SUM(s.outAmount),0) FROM (SELECT ISNULL(ROUND(( SELECT SUM(b.amount*b.price*c.rate*100/(b.vat_percent + 100))
                                 FROM   doc.ProductsFlow b 
                                 INNER JOIN doc.GeneralDocs c ON b.general_id = c.id
                                 WHERE  c.tdate BETWEEN @startdate AND @enddate
                                        AND ( c.doc_type=21 OR c.doc_type=23) 
										AND b.product_id = a.id AND b.coeff <=0 AND b.is_order = 0 
										 ),6), 0) AS outAmount
                                FROM    book.Products a WHERE a.path >= '" + path + "' AND a.path <= '" + path + "Z'" + @") AS s)AS outAmountNoVatSum,



(SELECT ISNULL(SUM(s.outAmount),0) 
  
								 FROM (SELECT ISNULL(ROUND(( SELECT SUM(b.amount*b.price*c.rate*100/(b.vat_percent + 100))
                                 FROM   doc.ProductsFlow b 
                                 INNER JOIN doc.GeneralDocs c ON b.general_id = c.id
                                 INNER JOIN doc.ProductOutSingle as psingle ON psingle.general_id=c.id 
                                 WHERE  c.tdate BETWEEN @startdate AND @enddate
                                        AND c.doc_type=23 AND b.product_id = a.id AND b.coeff <=0 AND b.is_order = 0 AND psingle.price_type IN (3,4)
										 ),6), 0) AS outAmount	 FROM    book.Products a WHERE  a.id = Products.id
                      UNION ALL
                                (SELECT ISNULL(ROUND(( SELECT SUM(b.amount*b.price*c.rate*100/(b.vat_percent + 100))
                                 FROM   doc.ProductsFlow b 
                                 INNER JOIN doc.GeneralDocs c ON b.general_id = c.id
                                 INNER JOIN doc.ProductOut as pout on pout.general_id=c.id 
                                 WHERE  c.tdate BETWEEN @startdate AND @enddate AND pout.price_type IN(3,4)
                                        AND  c.doc_type=21 AND b.product_id = a.id AND b.coeff <=0 AND b.is_order = 0),6), 0) 							 
                                FROM    book.Products a WHERE  a.id = Products.id)
                                )AS s) outAmountNoVatSingleOrDiscount,   
                                
 (SELECT ISNULL(SUM(s.outAmount),0) 
  
								 FROM (SELECT ISNULL(ROUND(( SELECT SUM(b.amount*b.price*c.rate*100/(b.vat_percent + 100))
                                 FROM   doc.ProductsFlow b 
                                 INNER JOIN doc.GeneralDocs c ON b.general_id = c.id
                                 INNER JOIN doc.ProductOutSingle as psingle ON psingle.general_id=c.id 
                                 WHERE  c.tdate BETWEEN @startdate AND @enddate
                                        AND c.doc_type=23 AND b.product_id = a.id AND b.coeff <=0 AND b.is_order = 0 AND psingle.price_type IN (5,6)
										 ),6), 0) AS outAmount	 FROM    book.Products a WHERE  a.id = Products.id
                      UNION ALL
                                (SELECT ISNULL(ROUND(( SELECT SUM(b.amount*b.price*c.rate*100/(b.vat_percent + 100))
                                 FROM   doc.ProductsFlow b 
                                 INNER JOIN doc.GeneralDocs c ON b.general_id = c.id
                                 INNER JOIN doc.ProductOut as pout on pout.general_id=c.id 
                                 WHERE  c.tdate BETWEEN @startdate AND @enddate AND pout.price_type IN(5,6)
                                        AND  c.doc_type=21 AND b.product_id = a.id AND b.coeff <=0 AND b.is_order = 0),6), 0) 							 
                                FROM    book.Products a WHERE  a.id = Products.id)
                                )AS s) outAmountNoVatSaleOrOther,    
                                
                                
(SELECT ISNULL(SUM(s.outAmount),0) FROM (SELECT ISNULL(ROUND(( SELECT SUM(b.amount*b.self_cost)
                                 FROM   doc.ProductsFlow b 
                                 INNER JOIN doc.GeneralDocs c ON b.general_id = c.id
                                 WHERE  c.tdate BETWEEN @startdate AND @enddate
                                        AND ( c.doc_type=21 OR c.doc_type=23) 
										AND b.product_id = a.id AND b.coeff <=0 AND b.is_order = 0 
										 ),6), 0) AS outAmount
                                FROM    book.Products a WHERE a.id = Products.id) AS s) AS selfCost," +
 //  (SELECT doc.fnc_GetProductAverageRest(@startdate,@enddate,0,'',Products.id)
 @"  (SELECT ISNULL(AVG(Res.rest),0)   FROM(
                                SELECT  r.rest_date, r.product_id,   sum(r.rest) AS rest
                                FROM book.AverageRest AS r
                                INNER JOIN book.Products as Products ON Products.id=r.product_id
                                group by r.product_id, r.rest_date) as res
                                WHERE res.product_id=Products.id and res.rest_date BETWEEN  @startdate AND @enddate
                                group by res.product_id) as average,


 (SELECT ISNULL(SUM(s.outAmount),0) FROM (SELECT ISNULL(ROUND(( SELECT SUM(b.amount*b.self_cost*b.coeff)
                                 FROM   doc.ProductsFlow b 
                                 INNER JOIN doc.GeneralDocs c ON b.general_id = c.id
                                 WHERE  c.tdate < @enddate
										AND b.product_id = a.id AND b.is_order = 0 
										 ),6), 0) AS outAmount
                                FROM    book.Products a WHERE a.id = Products.id) AS s)
                               /
  (SELECT nullif(SUM(a.amount*a.coeff),0)
                                 FROM   doc.ProductsFlow a 
                                        INNER JOIN doc.GeneralDocs c ON a.general_id = c.id
                                        INNER JOIN book.Products p ON p.id = a.product_id
                                 WHERE  c.tdate <  @enddate
										AND a.is_order = 0  AND a.is_expense = 0
										AND p.id = Products.id)  as selfCostOne,



 ( SELECT TOP 1 pf.self_cost
                            + ISNULL((SELECT SUM(p.self_cost) FROM doc.ProductsFlow p WHERE is_expense=1 AND p.ref_id=pf.id ),0) 
                            + ISNULL((SELECT SUM(p.self_cost) FROM doc.ProductsFlow p WHERE p.is_expense=1 AND p.coeff=1 AND p.product_id =Products.id AND p.general_id=pf.general_id),0) AS self_cost
                             FROM doc.ProductsFlow pf INNER JOIN doc.GeneralDocs c ON c.id = pf.general_id WHERE pf.product_id=Products.id AND coeff=1 AND is_expense=0  AND is_move=0  AND is_order=0 ORDER BY c.tdate DESC )
AS  selfCostLast








 FROM book.Products as Products  where Products.path >= '" + path+"' AND Products.path <= '"+path+"Z'"+ @" 

)AS RES";
            m_sqlParams.Clear();
            m_sqlParams.Add("@startdate", m_Period.dtp_From.Value.ToString("yyyy-MM-dd 00:00:00"));
            m_sqlParams.Add("@enddate", m_Period.dtp_To.Value.ToString("yyyy-MM-dd 23:59:59"));
          
            DataTable data = GetProgramManager().GetDataManager().GetTableData(sql, m_sqlParams);
            if (data == null || data.Rows.Count <= 0)
            {
                ProgressDispatcher.Deactivate();
                return;
            }
            m_Grid.Rows.Clear();

            double outQuantitySum = 0;
            double outAmountNoVatSum = 0;
            double outSingleDiscountSum = 0;
            double outSingleOtherSum = 0;
            double averageRestSum = 0;
            double averageGainSum = 0;
            double averageGainMontSum = 0;
            double GainFromRealizePriceSum = 0;
            double ShareFromOutSum = 0;
            double GainPercentFromRealisePrice = 0;
            double profitabilitySum = 0;
            foreach (DataRow row in data.Rows)
            {
                outQuantitySum += Convert.ToDouble(row["outQuantity"]);
                outAmountNoVatSum += Convert.ToDouble(row["outAmountNoVat"]);
                //outSingleDiscountSum +=Convert.ToDouble(row[""]);
                //outSingleOtherSum +=Convert.ToDouble(row[""]);
                averageRestSum += Convert.ToDouble(row["averageRest"]);
                averageGainSum += Convert.ToDouble(row["gain"]);
                averageGainMontSum += Convert.ToDouble(row["averageGain"]);
                GainFromRealizePriceSum += Convert.ToDouble(row["gainFromOutPerc"]);
                ShareFromOutSum += Convert.ToDouble(row["shareFromOut"]);
                outSingleDiscountSum += Convert.ToDouble(row["outSingleDiscount"]);
                outSingleOtherSum += Convert.ToDouble(row["outSaleOther"]);

            }
            GainPercentFromRealisePrice = averageGainSum / outAmountNoVatSum * 100;
            profitabilitySum = averageGainMontSum / averageRestSum * 100;
            double outSingleDiscountSumPercent=outSingleDiscountSum/outAmountNoVatSum *100;
            double outSingleOtherSumPercent = outSingleOtherSum / outAmountNoVatSum * 100;
            for (int i = 4; i < m_Grid.Rows.Count; i++)
                m_Grid.Columns[i].ValueType = typeof(double);


            for (int i = 0; i < data.Rows.Count; i++)
            {
                m_Grid.Rows.Add(
                    data.Rows[i]["id"].ToString(),
                    i+1,
                    data.Rows[i]["code"].ToString(),
                    data.Rows[i]["name"].ToString(),
                    Math.Round(Convert.ToDouble(data.Rows[i]["profitability"]),1),//.ToString("F1"),
                    Math.Round((Convert.ToDouble(data.Rows[i]["averageGain"]) / averageGainMontSum * 100),1),//.ToString("F1"),
                    Math.Round((Convert.ToDouble(data.Rows[i]["outAmountNoVat"]) / outAmountNoVatSum * 100),1),//.ToString("F1"),
                    Math.Round(Convert.ToDouble( data.Rows[i]["gainFromOutPerc"]),1),//.ToString("F1"),
                    Math.Round(Convert.ToDouble( data.Rows[i]["outSingleDiscountPercent"]),1),//.ToString("F1"),
                    Math.Round(Convert.ToDouble( data.Rows[i]["outOtherPercent"]),1),//.ToString("F1"),
                    Math.Round(Convert.ToDouble( data.Rows[i]["gain"]),0),//.ToString("F0"),
                    Math.Round(Convert.ToDouble( data.Rows[i]["averageGain"]),0),//.ToString("F0"),
                    Math.Round(Convert.ToDouble( data.Rows[i]["averageRest"]),0),//.ToString("F0"),
                    Math.Round(Convert.ToDouble( data.Rows[i]["outQuantity"]),0),//.ToString("F0"),
                    Math.Round(Convert.ToDouble( data.Rows[i]["outAmountNoVat"]),0)//.ToString("F0")
                    );
             }

            m_Grid.Rows.Add("", m_Grid.Rows.Count + 1, "სულ ჯამი:","",
              Math.Round(profitabilitySum,1)/*.ToString("F1")*/, 0.0, 0.0, Math.Round(GainPercentFromRealisePrice,1)/*.ToString("F1")*/, Math.Round(outSingleDiscountSumPercent,1)/*.ToString("F1")*/, 
             Math.Round(outSingleOtherSumPercent,1)/*.ToString("F1")*/, Math.Round(averageGainSum,0)/*.ToString("F0")*/, Math.Round(averageGainMontSum,0)/*.ToString("F0")*/,
                 Math.Round(averageRestSum,0)/*.ToString("F0")*/,  Math.Round(outQuantitySum,0)/*.ToString("F0")*/,  Math.Round(outAmountNoVatSum,0));//.ToString("F0"));


        }
        private void updateGridColor()
        {
            if (m_Grid.Rows.Count <= 0)
                return;
            foreach (DataGridViewRow row in m_Grid.Rows)
            {
                row.Cells[col_Profitability.Index].Style.BackColor = GetColorByVal(Convert.ToDouble(row.Cells[col_Profitability.Index].Value));
            }
        }



        private void updateGridAligment()
        {
            foreach (DataGridViewColumn col in m_Grid.Columns)
            {
                if (col.Index == 0 || col.Index == 1 || col.Index == 2 || col.Index == 3)
                    continue;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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

        private void onRefresh()
        {
            ProgressDispatcher.Activate();
            updateGrid();
            updateGridColor();
            updateGridAligment();
            ProgressDispatcher.Deactivate();
        }

        private void invokeDetailForm()
        {
            if (m_Grid.Rows.Count <= 0)
                return;
            int id=0;
           if(!int.TryParse(Convert.ToString(m_Grid.SelectedRows[0].Cells[col_id.Index].Value),out id))
               return;
            DateTime fromdate = m_Period.dtp_From.Value;
            DateTime todate = m_Period.dtp_To.Value;
            ProfitabilityDetailForm form = new ProfitabilityDetailForm(GetProgramManager(),id, fromdate,todate);
            form.Show();
            //this.Close();

        }
        private void onGroup()
        {
            string path = string.Empty;
            path = GetProgramManager().ShowGroupSelectForm("TABLE_PRODUCT:PRODUCT",0);
            if (!string.IsNullOrEmpty(path))
            {
                btnGroup.Tag = path;
                m_sqlParams.Clear();
                m_sqlParams.Add("@PATH", path);
                txtGroup.Text = GetProgramManager().GetDataManager().GetStringValue("SELECT name FROM book.GroupProducts WHERE path=@PATH", m_sqlParams);
            }
        }



        private void onExcelPreview()
        {
            ExcelManager Excel = new ExcelManager(GetProgramManager(), m_Grid, this.Text);
            Excel.OnExcelPreviewFast();

        }

        private void onExcelExport()
        {
            SaveFileDialog svd = new SaveFileDialog();
            svd.Title = GetProgramManager().GetTranslatorManager().Translate("მიუთითეთ შესანახი ადგილი");
            svd.Filter = "Excel 2003 Files.xls|*.xls|Excel 2007 Files.xlsx|*.xlsx";
            svd.FileName = "Exported Data";
            if (svd.ShowDialog(this) == DialogResult.OK)
            {
                ExcelManager Excel = new ExcelManager(GetProgramManager(), m_Grid, this.Text);
                Excel.OnExcelExportFast(svd.FileName);
            }
        }


        private void CorrectSumRow()
        {
            foreach (DataGridViewRow row in m_Grid.Rows)
            {
                if (string.IsNullOrEmpty(Convert.ToString(row.Cells[col_id.Index].Value)) && Convert.ToString(row.Cells[col_code.Index].Value) == "სულ ჯამი:")
                {
                    m_Grid.Rows.Remove(row);
                    m_Grid.Rows.Add(row);
                    break;
                }
            }
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

        private void btnGroup_Click(object sender, EventArgs e)
        {
            onGroup();
        }
        private void m_Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            invokeDetailForm();

        }
        #endregion

        private void btnExport_Click(object sender, EventArgs e)
        {
            onExcelExport();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            onExcelPreview();
        }

        private void m_Grid_Sorted(object sender, EventArgs e)
        {
            CorrectSumRow();
        }

       

        
    }
}
