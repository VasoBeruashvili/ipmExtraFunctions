using ipmControls;
using ipmPMBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ipmExtraFunctions
{
    public partial class ContragentRegHistoryForm : Form
    {
        public ProgramManagerBasic ProgramManager { get; set; }
        DataTable m_Data;    
        public ContragentRegHistoryForm()
        {
            InitializeComponent();
            m_Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 229, 245);
            m_Grid.DefaultCellStyle.BackColor = Color.White;
        }
        private void FillHistory()
        {
            string _sql = @"SELECT ISNULL(c.code,'') AS Code, ISNULL(c.name,'') AS Name, ISNULL(c.tel,'') AS Tel, ISNULL(u.name,'')+' '+ISNULL(u.surname,'') AS UserName                                                      
                            FROM book.ContragentRelations AS r                          
                            INNER JOIN book.Contragents AS c ON r.child_id=c.id
                            LEFT JOIN book.Users AS u ON u.id=r.user_id
                            WHERE r.create_date BETWEEN  '" + txtDates.dtp_From.Value.ToString("yyyy-MM-dd HH:mm") + @"' AND '" + txtDates.dtp_To.Value.ToString("yyyy-MM-dd HH:mm") + @"'
                            ORDER BY r.id, r.parent_id";

            m_Data = ProgramManager.GetDataManager().GetTableData(_sql);
            m_Grid.DataSource = null;
            if (m_Data == null || m_Data.Rows.Count == 0)
            {
                lblClientCount.Text = string.Concat(ProgramManager.GetTranslatorManager().Translate("კლიენტების რაოდენობა:"), " 0");
                return;
            }

            m_Grid.DataSource = m_Data;
            foreach(DataGridViewColumn _col in m_Grid.Columns)
            {
                switch(_col.Name)
                {
                    case "Code": _col.HeaderText = ProgramManager.GetTranslatorManager().Translate("ს\\კ"); break;
                    case "Name": _col.HeaderText = ProgramManager.GetTranslatorManager().Translate("სახელი"); break;
                    case "Tel": _col.HeaderText = ProgramManager.GetTranslatorManager().Translate("სახელი"); break;
                    case "UserName": _col.HeaderText = ProgramManager.GetTranslatorManager().Translate("მომხმარებელი"); break;
                }
                _col.ReadOnly = true;
            }              
            lblClientCount.Text = string.Concat(ProgramManager.GetTranslatorManager().Translate("კლიენტების რაოდენობა:"), " ", m_Grid.Rows.Count);
        }

        private void ContragentRegHistoryForm_Load(object sender, EventArgs e)
        {
            txtDates.ProgramManager = this.ProgramManager;
            txtDates.translate();
            txtDates.SetThisMonth();
            m_Grid.PManager = this.ProgramManager;
            FillHistory();
        }
        private void OnCustomFilter()
        {            
            DataRow[] findRows;
            if (m_Grid.SortString == string.Empty)
            {
                findRows = m_Data.Select(m_Grid.FilterString);
            }
            else
                findRows = m_Data.Select(m_Grid.FilterString, m_Grid.SortString);

            if (findRows.Count() > 0)
            {
                m_Grid.DataSource = findRows.CopyToDataTable();
            }
            else
            {
                DataTable dt = m_Data.Clone();
                dt.Rows.Clear();
                m_Grid.DataSource = dt;
            }

            lblClientCount.Text = string.Concat(ProgramManager.GetTranslatorManager().Translate("კლიენტების რაოდენობა:"), " ", m_Grid.Rows.Count);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FillHistory();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (m_Grid.Rows.Count == 0)
                return;

            SaveFileDialog dlgSave = new SaveFileDialog();
            dlgSave.Title = ProgramManager.GetTranslatorManager().Translate("მიუთითეთ შესანახი ადგილი");
            dlgSave.Filter = "Excel 2003 Files|*.xls";
            if (dlgSave.ShowDialog(this) != DialogResult.OK)
                return;

            ProgressDispatcher.Activate();
            ExcelManager _excelManager = new ExcelManager(ProgramManager, m_Grid, "");
            _excelManager.OnExcelExportFast(dlgSave.FileName);
            ProgressDispatcher.Deactivate();
        }

        private void m_Grid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            foreach (DataGridViewRow row in m_Grid.Rows)
            {
                row.DefaultCellStyle.SelectionBackColor = Color.RoyalBlue;
            }
        }

        private void m_Grid_FilterStringChanged(object sender, EventArgs e)
        {
            OnCustomFilter();
        }      

        private void m_Grid_SortStringChanged(object sender, EventArgs e)
        {
            OnCustomFilter();
        }
    }
}
