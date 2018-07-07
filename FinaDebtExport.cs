using System;
using System.Windows.Forms;
using ipmPMBasic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using ipmFunc;
using System.IO;
using System.Text;
using ipmControls;
using System.Drawing;

namespace ipmExtraFunctions
{
    public partial class FinaDebtExport : Form
    {
        public FinaDebtExport()
        {
            InitializeComponent();
            m_Picker.dtp_From.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            m_Picker.dtp_To.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59, 59);
        }

        private void btnExposrt_Click(object sender, EventArgs e)
        {
            string file_path = null;
            using (SaveFileDialog savedialog = new SaveFileDialog() { FileName = "FINA დავალიანების ექსპორტი", Filter = "JSON Files" + " (*.json)|*.json" })
            {
                if (savedialog.ShowDialog() != DialogResult.OK)
                    return;
                file_path = savedialog.FileName;
            }

            DateTime date1 = new DateTime(m_Picker.dtp_From.Value.Year, m_Picker.dtp_From.Value.Month, m_Picker.dtp_From.Value.Day, 0, 0, 0);
            DateTime date2 = new DateTime(m_Picker.dtp_To.Value.Year, m_Picker.dtp_To.Value.Month, m_Picker.dtp_To.Value.Day, 23, 59, 59, 997);

            using (DBContext _db = new DBContext())
            {
                string sql_select = @"SELECT g.id, g.tdate, g.amount, g.amount-ISNULL(cur.am, 0) AS current_amount, g.doc_type, c.code AS contragent_code, c.name AS contragent_name, pr.name AS prod_name, ISNULL(pf.amount,0) AS quantity, ISNULL(pf.price,0) AS price
                                      FROM doc.GeneralDocs AS g
                                      LEFT JOIN doc.SingleEntry AS si ON si.general_id=g.id
                                      LEFT JOIN doc.ProductsFlow AS pf ON pf.general_id = g.id
                                      LEFT JOIN book.Products AS pr ON pr.id = pf.product_id
                                      INNER JOIN book.Contragents AS c ON c.id = CASE WHEN g.doc_type IN(38,65) THEN g.param_id2 WHEN g.doc_type=70 THEN si.a1  ELSE g.param_id1 END
                                      OUTER APPLY(SELECT SUM(pda.amount) AS am FROM doc.paymentdeadlines AS pda WHERE pda.general_id=g.id AND pda.tdate > GETDATE()) AS cur    
                                      WHERE (g.doc_type IN(21, 29, 13, 38, 9, 65) OR (g.doc_type=70 AND si.debit_acc='1415' AND si.credit_acc='1410'))  AND g.tdate BETWEEN @start AND @end 
                                      ORDER BY g.tdate";
                var data = _db.GetTableDictionary(sql_select, new SqlParameter[]
                {
                    new SqlParameter ("@start", SqlDbType.DateTime){Value = date1 },
                    new SqlParameter ("@end", SqlDbType.DateTime){Value = date2 }
                });
                if (data == null || data.Count == 0)
                {
                    MessageBoxForm.Show(Application.ProductName, "მონაცემები ვერ მოიძებნა.", null, null, SystemIcons.Information);
                    return;
                }
                   
                var grp = data.GroupBy(a => a["id"]);

                var js_data = new
                {
                    st_date = DateTime.SpecifyKind(date1, DateTimeKind.Utc),
                    end_date = DateTime.SpecifyKind(date2, DateTimeKind.Utc),
                    doc = grp.Select(a => new
                    {
                        id = a.First()["id"],
                        td = DateTime.SpecifyKind(Convert.ToDateTime(a.First()["tdate"]), DateTimeKind.Utc),
                        am = a.First()["amount"],
                        cm = a.First()["current_amount"],
                        dp = a.First()["doc_type"],
                        cc = a.First()["contragent_code"],
                        cn = a.First()["contragent_name"],
                        flw = (Convert.ToInt32(a.First()["doc_type"]) == 38 || Convert.ToInt32(a.First()["doc_type"]) == 65 || Convert.ToInt32(a.First()["doc_type"]) == 70) ? null : a.Select(b => new
                        {
                            prn = b["prod_name"],
                            prq = b["quantity"],
                            prp = b["price"]
                        })
                    })
                };

                if (js_data == null || js_data.doc == null || !js_data.doc.Any())
                    return;
                string json_data = Special.SerialiseJson(js_data);
                try
                {
                    File.WriteAllText(file_path, json_data, Encoding.UTF8);
                    MessageBoxForm.Show(Application.ProductName, "ოპერაცია წარმატებით შესრულდა.", null, null, SystemIcons.Information);
                    Close();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBoxForm.Show(Application.ProductName, "ოპერაცია ვერ შესრულდა!", ex.Message, null, SystemIcons.Error);
                }

            }



        }
    }
}
