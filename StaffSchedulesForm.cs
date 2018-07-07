using ipmPMBasic;
using System;
using System.Data;
using System.Windows.Forms;

namespace ipmExtraFunctions
{
    public partial class StaffSchedulesForm : Form
    {
        ProgramManagerBasic mPm { get; set; }
        public StaffSchedulesForm(ProgramManagerBasic pm)
        {
            this.mPm = pm;
            InitializeComponent();
        }

        private void StaffSchedulesForm_Load(object sender, EventArgs e)
        {
            FillSchedules();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            OnGetDetail(0);
        }

        private void FillSchedules()
        {
            DataTable _data = this.mPm.GetDataManager().GetTableData("SELECT id, (CASE period WHEN 0 THEN N'კვირა' WHEN 1 THEN N'თვე' WHEN 2 THEN N'კვარტალი' WHEN 3 THEN N'წელი' ELSE '' END) AS period, tdate, purpose, (CASE WHEN status=1 THEN N'დასრულებული' ELSE N'აქტიური' END) AS status FROM doc.StaffAmountSchedule ORDER BY tdate");
            if(_data != null)
            {
                int index;
                foreach(DataRow row in _data.Rows)
                {
                    index = m_Grid.Rows.Add();
                    m_Grid.Rows[index].Cells[col_Id.Index].Value = row.Field<int>("id");
                    m_Grid.Rows[index].Cells[col_purpose.Index].Value = row.Field<string>("purpose");
                    m_Grid.Rows[index].Cells[col_date.Index].Value = row.Field<DateTime>("tdate").ToString("dd/MM/yyyy HH:mm");
                    m_Grid.Rows[index].Cells[col_status.Index].Value = row.Field<string>("status");
                    m_Grid.Rows[index].Cells[col_period.Index].Value = row.Field<string>("period");
                }
            }
        }

        private void m_Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OnGetDetail(Convert.ToInt32(m_Grid.Rows[e.RowIndex].Cells[col_Id.Index].Value));
        }

        private void OnGetDetail(int id)
        {
            using (StaffSchedulesDetailForm _form = new StaffSchedulesDetailForm(this.mPm) { Id = id })
            {
                if (_form.ShowDialog() == DialogResult.OK)
                {
                    m_Grid.Rows.Clear();
                    FillSchedules();
                }
            }
        }

    }
}
