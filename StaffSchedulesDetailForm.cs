using ipmControls;
using ipmPMBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace ipmExtraFunctions
{
    public partial class StaffSchedulesDetailForm : Form
    {
        public int Id { get; set; }
        ProgramManagerBasic mPm { get; set; }
        private Dictionary<int, List<StaffScheduleFlow>> _staffProducts;


        public StaffSchedulesDetailForm(ProgramManagerBasic pm)
        {
            this.mPm = pm;
            _staffProducts = new Dictionary<int, List<StaffScheduleFlow>>();
            InitializeComponent();
            m_Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 228, 248);
            m_Grid.PManager = mPm;
        }

        private void StaffSchedulesDetailForm_Load(object sender, EventArgs e)
        {
            comboStaff.DataSource = mPm.GetDataManager().GetTableData("SELECT 0 AS id, N'ყველა' AS name UNION SELECT id,name FROM book.Staff");
            comboStaff.DisplayMember = "name";
            comboStaff.ValueMember = "id";

            if (this.Id > 0)
            {
                DataTable _data = mPm.GetDataManager().GetTableData("SELECT TOP(1) * FROM doc.StaffAmountSchedule WHERE id=" + this.Id);
                if (_data != null && _data.Rows.Count > 0)
                {
                    comboStatus.SelectedIndex = Convert.ToInt32(_data.Rows[0].Field<bool>("status"));
                    txtPurpose.Text = _data.Rows[0].Field<string>("purpose");
                    comboPeriod.SelectedIndex = _data.Rows[0].Field<int>("period");
                    comboScheduleType.SelectedIndex = Convert.ToInt32(_data.Rows[0].Field<bool>("type"));
                    if (comboStatus.SelectedIndex == 1)
                    {
                        btnSave.Enabled = false;
                    }

                    List<int> curStaffs = mPm.GetDataManager().GetList<int>("SELECT DISTINCT staff_id FROM doc.StaffAmountScheduleFlow WHERE schedule_id=" + this.Id);
                    foreach(int id in curStaffs)
                    {
                        _staffProducts.Add(id, mPm.GetDataManager().GetList<StaffScheduleFlow>(@"SELECT sf.id AS Id, p.id AS ProductId,p.code AS Code,gp.name AS GroupName, p.name AS Name,sf.amount AS Amount FROM doc.StaffAmountScheduleFlow AS sf 
                                                           INNER JOIN book.Products AS p ON p.id=sf.product_id 
                                                           INNER JOIN book.GroupProducts AS gp ON gp.id=p.group_id WHERE schedule_id=" + this.Id + " AND staff_id=" + id));
                    }
                }

            }
            else
            {
                txtPurpose.Text = "თანამშრომლების თანხის გეგმა № " + (mPm.GetDataManager().GetIntegerValue("SELECT COUNT(*) FROM doc.StaffAmountSchedule") + 1);
                comboStatus.SelectedIndex = 0;
                comboStatus.Enabled = false;

                comboPeriod.SelectedIndex = 0;

                comboScheduleType.SelectedIndex = 0;
            }
        }

        private void m_Grid_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            switch(e.Column.Name)
            {
                case "Id":
                    e.Column.Visible = false;
                    break;
                case "ProductId":
                    e.Column.Visible = false;
                    break;
                case "Code":
                    e.Column.HeaderText = "კოდი";
                    e.Column.ReadOnly = true;
                    break;
                case "GroupName":
                    e.Column.HeaderText = "ჯგუფი";
                    e.Column.ReadOnly = true;
                    break;
                case "Name":
                    e.Column.HeaderText = "დასახელება";
                    e.Column.ReadOnly = true;
                    break;
                case "Amount":
                    e.Column.HeaderText = "თანხა";
                    break;
            }
        }

        private void btnProductsGroup_Click(object sender, EventArgs e)
        {
            int[] p_ids = mPm.ShowSelectFormMulti("TABLE_PRODUCT:PRODUCT", 0);
            if (p_ids.Length < 0)
                return;

            int staff_id = (int)comboStaff.SelectedValue;
            if (staff_id == 0)
            {
                MessageBox.Show(mPm.GetTranslatorManager().Translate("აირჩიეთ თანამშრომელი"), mPm.GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<StaffScheduleFlow> _flow = mPm.GetDataManager().GetList<StaffScheduleFlow>(@"SELECT 0 AS Id, p.id AS ProductId,p.code AS Code,gp.name AS GroupName, p.name AS Name,CAST(0 AS DECIMAL(18,4)) AS Amount FROM book.Products AS p
                                                           INNER JOIN book.GroupProducts AS gp ON gp.id=p.group_id WHERE p.id IN (" + String.Join(",", p_ids.Select(c => c.ToString()).ToArray()) + ")");

            FillGrid(staff_id, _flow);
        }

        private void FillGrid(int staff_id, List<StaffScheduleFlow> _flow)
        {
            ProgressDispatcher.Activate();
            if (!_staffProducts.ContainsKey(staff_id))
            {
                _staffProducts.Add(staff_id, _flow);
            }
            else
            {
                var ids = _staffProducts[staff_id].Select(f => f.ProductId).ToList();
                _staffProducts[staff_id].AddRange(_flow.Where(c => !ids.Contains(c.ProductId)));
            }

            m_Grid.DataSource = null;
            m_Grid.DataSource = _staffProducts[staff_id];
            m_Grid.ReadOnly = false;

            ProgressDispatcher.Deactivate();
        }

        private void btnProductsClear_Click(object sender, EventArgs e)
        {
            int staff_id = (int)comboStaff.SelectedValue;
            if (staff_id > 0)
            {
                if (MessageBox.Show(mPm.GetTranslatorManager().Translate("ნამდვილად გსურთ " + comboStaff.Text + "-ის გეგმის გასუფთავება?"), mPm.GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                m_Grid.DataSource = null;
                if (_staffProducts.ContainsKey(staff_id))
                {
                    _staffProducts.Remove(staff_id);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            m_Grid.EndEdit();
            if (comboStatus.SelectedIndex == 1)
            {
                if (MessageBox.Show(mPm.GetTranslatorManager().Translate("ნამდვილად გსურთ გეგმის დასრულება?"), mPm.GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            if (_staffProducts.Count == 0)
            {
                MessageBox.Show(mPm.GetTranslatorManager().Translate("გეგმის სია ცარიელია"), mPm.GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            int schedule_id;
            Hashtable _params = new Hashtable();
            _params.Add("@purpose", txtPurpose.Text);
            _params.Add("@id", this.Id);
            _params.Add("@status", comboStatus.SelectedIndex);
            _params.Add("@period", comboPeriod.SelectedIndex);

            mPm.GetDataManager().Close();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.Serializable, Timeout = new TimeSpan(0, 30, 0) }))
            {
                mPm.GetDataManager().Open();

                if (this.Id > 0)
                {
                    schedule_id = this.Id;
                    if (!mPm.GetDataManager().ExecuteSql("DELETE FROM doc.StaffAmountScheduleFlow WHERE schedule_id=" + this.Id))
                    {
                        Transaction.Current.Rollback();
                        return;
                    }

                    if(!mPm.GetDataManager().ExecuteSql("UPDATE doc.StaffAmountSchedule SET purpose=@purpose,period=@period,status=@status WHERE id=@id", _params))
                    {
                        Transaction.Current.Rollback();
                        return;
                    }

                }
                else
                {
                    schedule_id = mPm.GetDataManager().GetIntegerValue("INSERT INTO doc.StaffAmountSchedule (purpose,period,status) VALUES(@purpose,@period,@status) SELECT SCOPE_IDENTITY()", _params);
                    if (schedule_id <= 0)
                    {
                        Transaction.Current.Rollback();
                        return;
                    }
                }


                foreach (var data in _staffProducts)
                {
                    foreach (var flow in data.Value)
                    {
                        if (!mPm.GetDataManager().ExecuteSql("INSERT INTO doc.StaffAmountScheduleFlow (schedule_id,staff_id,product_id,amount) VALUES(" + schedule_id + ", " + data.Key + ", " + flow.ProductId + ", " + flow.Amount + ")"))
                        {
                            Transaction.Current.Rollback();
                            return;
                        }
                    }
                }

                scope.Complete();
            }

            DialogResult = DialogResult.OK;
        }

        private void comboStaff_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int staff_id = (int)comboStaff.SelectedValue;
            if(staff_id > 0)
            {
                if(_staffProducts.ContainsKey(staff_id))
                {
                    m_Grid.DataSource = _staffProducts[staff_id];
                }
                else
                {
                    m_Grid.DataSource = null;
                }
                m_Grid.ReadOnly = false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            int staff_id = (int)comboStaff.SelectedValue;
            if (staff_id == 0)
            {
                MessageBox.Show(mPm.GetTranslatorManager().Translate("აირჩიეთ თანამშრომელი"), mPm.GetTranslatorManager().Translate(Application.ProductName), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenFileDialog _dialog = new OpenFileDialog();
            _dialog.Filter = "Excel Files|*.xls;*.xlsx;*";
            if (_dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                DataTable _data = mPm.GetDataManager().GetTableDataFromExcel(_dialog.FileName);
                if (_data == null || _data.Rows.Count == 0)
                {
                    MessageBoxForm.Show(mPm.GetTranslatorManager().Translate("შეცდომა"), mPm.GetTranslatorManager().Translate("იმპორტი ვერ მოხერხდა!"), mPm.GetTranslatorManager().Translate("ფაილის წაკითხვა ვერ მოხერხდა."), null, SystemIcons.Error);
                    return;
                }

                List<StaffScheduleFlow> _flow = new List<StaffScheduleFlow>();
                foreach (DataRow row in _data.Rows)
                {
                    int product_id = mPm.GetDataManager().GetIntegerValue("SELECT TOP(1) id FROM book.Products WHERE code='" + row[0] + "' ");
                    if (product_id <= 0)
                    {
                        product_id = mPm.GetDataManager().GetIntegerValue("SELECT TOP(1) id FROM book.Products WHERE name=N'" + row[1] + "' ");
                    }

                    if (product_id <= 0)
                        continue;

                    _flow.Add(new StaffScheduleFlow
                    {
                        Id = 0,
                        Code = row[0].ToString(),
                        Name = row[1].ToString(),
                        ProductId = product_id,
                        GroupName = row[2].ToString(),
                        Amount = Convert.ToDecimal(row[3])
                    });
                }

                FillGrid(staff_id, _flow);
            }
            catch(Exception ex)
            {
                MessageBoxForm.Show(mPm.GetTranslatorManager().Translate("შეცდომა"), "", ex.Message, null, SystemIcons.Error);
            }

        }
    }

    public class StaffScheduleFlow
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public decimal Amount { get; set; }
    }

}
