using ipmControls;
using ipmPMBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ipmExtraFunctions
{
    public partial class ContragentRelationStatisticForm : Form
    {

        public ProgramManagerBasic ProgramManager { get; set; }
        private List<ContragentRelationItem> m_RelationItems;
        private Dictionary<decimal, decimal> SalePercentages = new Dictionary<decimal, decimal>()
        {
            { 16000, 22 },
            { 12000, 18 },
            { 9000, 15 },
            { 6000, 12 },
            { 3000, 9 },
            { 1200, 6 },
            { 300, 3 }
        };
        private decimal m_PercentFromGroup = 10;
        private decimal m_PercentFromCoords = 12;
        private decimal m_PercentFromSeniorCoords = 11;
        private decimal m_PercentFromManagers = 10;

        public ContragentRelationStatisticForm()
        {
            InitializeComponent();
            m_Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 229, 245);
            m_Grid.DefaultCellStyle.BackColor = Color.White;

            m_GridBonus.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(216, 229, 245);
            m_GridBonus.DefaultCellStyle.BackColor = Color.White;

            checkNewMethod.Checked = true;

            m_GridBonus.Visible = true;
            m_GridBonus.Dock = DockStyle.Fill;

            m_Grid.Visible = false;
            m_Grid.Dock = DockStyle.None;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ContragentRelationStatisticForm_Load(object sender, EventArgs e)
        {
            txtDates.ProgramManager = this.ProgramManager;
            txtDates.translate();
            txtDates.SetThisMonth();

            m_PercentFromGroup = Convert.ToDecimal(ProgramManager.GetDataManager().GetParamValue("PrcFromGroup"));
            m_PercentFromCoords = Convert.ToDecimal(ProgramManager.GetDataManager().GetParamValue("PrcFromCoords"));
            m_PercentFromSeniorCoords = Convert.ToDecimal(ProgramManager.GetDataManager().GetParamValue("PrcFromSeniorCoords"));
            m_PercentFromManagers = Convert.ToDecimal(ProgramManager.GetDataManager().GetParamValue("PrcFromManagers"));

            FillStatisticNew();            
        }

        private void btnRealtions_Click(object sender, EventArgs e)
        {
            using (ContragentRelationElementForm _form = new ContragentRelationElementForm() { ProgramManager =this.ProgramManager })
            {
                ProgramManager.TranslateControl(_form);
                _form.ShowDialog();
            }
        }
        private decimal GetChildrenSales(int parentId) 
        {
            if (m_RelationItems == null || m_RelationItems.Count == 0)
                return 0;
           
                List<ContragentRelationItem> _children = new List<ContragentRelationItem>(m_RelationItems.Where(p => p.ParentId == parentId && p.Id != p.ParentId ));
                if (_children == null || _children.Count == 0)
                    return 0;

                decimal _amount = _children.Select(p => p.Sales).DefaultIfEmpty().Sum();
                foreach (ContragentRelationItem _item in _children)
                {
                    _amount += GetChildrenSales(_item.Id);
                }
                return _amount;           
        } 
        private decimal GetPercent(decimal personalSales, decimal groupSales)
        {
            if (personalSales <= 50)
                return 0;

            decimal _percent = SalePercentages.Where(p => groupSales >= p.Key).Select(p => p.Value).FirstOrDefault();
            return _percent;
        }
        private KeyValuePair<decimal, decimal> GetBonus(int parentId, decimal personalSales, decimal groupSales)
        {
            if (m_RelationItems == null || m_RelationItems.Count == 0)
                return new KeyValuePair<decimal, decimal>(0, 0);

            decimal _amount = 0;
            decimal _bonus = 0;
            decimal _percent = SalePercentages.Where(p => groupSales >= p.Key).Select(p => p.Value).FirstOrDefault();
            List<ContragentRelationItem> _children = new List<ContragentRelationItem>(m_RelationItems.Where(p => p.ParentId == parentId));
            if (_children == null)
                return new KeyValuePair<decimal, decimal>(0, 0);

            if (_children.Count == 0 && personalSales > 50 && _percent > 0)
            {
                _bonus = (groupSales / (decimal)1.18 * _percent / 100) * (decimal)0.8;
                return new KeyValuePair<decimal, decimal>(_amount, _bonus);
            }


            KeyValuePair<decimal, decimal> _val = new KeyValuePair<decimal, decimal>();
            foreach (ContragentRelationItem _item in _children)
            {
                _val = GetBonus(_item.Id, _item.Sales, _item.GroupSales);
                _amount += _val.Key + _val.Value;

            }
            if (_percent > 0 && personalSales > 50)
                return new KeyValuePair<decimal, decimal>(_amount, (groupSales / (decimal)1.18 * _percent / 100 - _amount) * (decimal)0.8);
            else
                return new KeyValuePair<decimal, decimal>(_amount, 0);

        }
        private decimal GetChildrenBonus(int parentId)
        {          
            if (m_RelationItems == null || m_RelationItems.Count == 0)
                return 0;

            List<ContragentRelationItem> _children = new List<ContragentRelationItem>(m_RelationItems.Where(p => p.ParentId == parentId));
            if (_children == null|| _children.Count == 0)
                return 0;
 
            decimal _amount = 0;
            KeyValuePair<decimal, decimal> _val = new KeyValuePair<decimal, decimal>();
            foreach (ContragentRelationItem _item in _children)
            {
                _val = GetBonus(_item.Id, _item.Sales, _item.GroupSales);
                _amount += _val.Key + _val.Value;
            }
            return _amount;
        }
        private decimal GetDiscountPercent(decimal amount)
        {
            if (amount >= 0 && amount <= 49)
                return 0;

            if (amount >= 50 && amount <= 74)
                return 10;

            if (amount >= 75 && amount <= 149)
                return 20;

            if (amount >= 150 && amount <= 269)
                return 28;

            if (amount >= 270 && amount <= 499)
                return 33;

            if (amount >= 500)
                return 37;

            return 0;
        }
        private void FillStatisticNew()
        {
            string _sql = @"SELECT r.child_id AS Id, r.parent_id AS ParentId, ISNULL(c.code,'') AS Code,
                            CASE r.type WHEN 0 THEN ISNULL(c.name,'') 
                                WHEN 1 THEN ISNULL(c.name,'') +N' (კოორდინატორი)'
                                WHEN 2 THEN ISNULL(c.name,'') +N' (უფროსი კოორდინატორი)'
                                WHEN 3 THEN ISNULL(c.name,'') +N' (მენეჯერი)'
                                WHEN 4 THEN ISNULL(c.name,'') +N' (დირექტორი)' 
                            END AS Name, ISNULL(c.tel,'') AS Tel,
                            (SELECT CONVERT(DECIMAL(18,2), ISNULL(SUM(amount),0)) FROM doc.GeneralDocs WHERE doc_type=21 AND param_id1=c.id AND tdate BETWEEN 
                            '" + txtDates.dtp_From.Value.ToString("yyyy-MM-dd HH:mm") + @"' AND '" + txtDates.dtp_To.Value.ToString("yyyy-MM-dd HH:mm") + @"') AS Sales,                              
                            CAST(0 AS DECIMAL(18,2)) GroupSales,
                            CAST(0 AS DECIMAL(18,2)) PersonalBonus,
                            CAST(0 AS DECIMAL(18,2)) GroupBonus,
                            CAST(0 AS DECIMAL(18,2)) CoordBonus,
                            CAST(0 AS DECIMAL(18,2)) SeniorCoordBonus,
                            CAST(0 AS DECIMAL(18,2)) ManagerBonus, CAST(0 AS BIT) AS Succeeded, r.type
                            FROM book.ContragentRelations AS r 
                            INNER JOIN book.Contragents AS p ON r.parent_id=p.id
                            INNER JOIN book.Contragents AS c ON r.child_id=c.id
                            ORDER BY r.parent_id";

            List<ContragentBonusItem> m_BonusItems = ProgramManager.GetDataManager().GetList<ContragentBonusItem>(_sql);
            if (m_BonusItems == null || m_BonusItems.Count == 0)
                return;

            m_Grid.Rows.Clear();

           // decimal _discountPercent = 0;
            List<ContragentBonusItem> _coordinators = m_BonusItems.Where(p => p.Type == 1 && p.Sales >= 50).ToList();
            List<ContragentBonusItem> _seniorCoordinators = m_BonusItems.Where(p => p.Type == 2 && p.Sales >= 50).ToList();
            List<ContragentBonusItem> _managers = m_BonusItems.Where(p => p.Type == 3 && p.Sales >= 50).ToList();
            List<ContragentBonusItem> _directors = m_BonusItems.Where(p => p.Type == 4 && p.Sales >= 50).ToList();
            List<ContragentBonusItem> _consults;
            decimal _amount = 0;

            //კოორდინატორი
            foreach (ContragentBonusItem _coordinator in _coordinators)
            {
                // ბონუსი პირადი გაყიდვებიდან
                /* _discountPercent = GetDiscountPercent(_coordinator.Sales);
                 _coordinator.PersonalBonus = _coordinator.Sales * _discountPercent / 100;*/
                _coordinator.PersonalBonus = _coordinator.Sales;

                _consults = m_BonusItems.Where(p => p.ParentId == _coordinator.Id && p.Type == 0).ToList();
                if (_consults.Where(p => p.Sales < 50).Any())
                    continue;

                _coordinator.GroupSales = _consults.Select(p => p.Sales).Sum();
                _coordinator.GroupSales += _coordinator.Sales;
                if (_coordinator.GroupSales < 1620)
                    continue;

                // ბონუსი პირადი ჯგუფიდან
                _coordinator.GroupBonus = (((_coordinator.GroupSales - _coordinator.GroupSales * 33 / 100) * m_PercentFromGroup / 100) / (decimal)1.18) * (decimal)0.8;
                _coordinator.Succeeded = true;
            }

            //უფროსი კოორდინატორი
            foreach (ContragentBonusItem _sCoordinator in _seniorCoordinators)
            {
                // ბონუსი პირადი გაყიდვებიდან
                /*  _discountPercent = GetDiscountPercent(_sCoordinator.Sales);
                  _sCoordinator.PersonalBonus = _sCoordinator.Sales * _discountPercent / 100;*/
                _sCoordinator.PersonalBonus = _sCoordinator.Sales;

                //ბონუსი პირადი ჯგუფიდან
                _consults = m_BonusItems.Where(p => p.ParentId == _sCoordinator.Id && p.Type == 0).ToList();
                if (_consults.Where(p => p.Sales < 50).Any())
                    continue;

                _sCoordinator.GroupSales = _consults.Select(p => p.Sales).Sum();
                _sCoordinator.GroupSales += _sCoordinator.Sales;
                if (_sCoordinator.GroupSales < 3520)
                    continue;

                _sCoordinator.GroupBonus = (((_sCoordinator.GroupSales - _sCoordinator.GroupSales * 33 / 100) * m_PercentFromGroup / 100) / (decimal)1.18) * (decimal)0.8;

                //ბონუსი პირადი კოორდინატორებისგან
                if (_coordinators.Where(p => p.ParentId == _sCoordinator.Id && !p.Succeeded).Any())
                    continue;
                _amount = _coordinators.Where(p => p.ParentId == _sCoordinator.Id).Select(p => p.GroupSales).Sum();
                _sCoordinator.CoordBonus = (((_amount - _amount * 33 / 100) * m_PercentFromCoords / 100) / (decimal)1.18) * (decimal)0.8;
                _sCoordinator.Succeeded = true;
            }


            //მენეჯერი
            foreach (ContragentBonusItem _manager in _managers)
            {
                // ბონუსი პირადი გაყიდვებიდან
                /*_discountPercent = GetDiscountPercent(_manager.Sales);
                _manager.PersonalBonus = _manager.Sales * _discountPercent / 100;*/
                _manager.PersonalBonus = _manager.Sales;

                //ბონუსი პირადი ჯგუფიდან
                _consults = m_BonusItems.Where(p => p.ParentId == _manager.Id && p.Type == 0).ToList();
                if (_consults.Where(p => p.Sales < 50).Any())
                    continue;


                _manager.GroupSales = _consults.Select(p => p.Sales).Sum();
                _manager.GroupSales += _manager.Sales;
                if (_manager.GroupSales > 5120)
                    continue;
                _manager.GroupBonus = (((_manager.GroupSales - _manager.GroupSales * 33 / 100) * m_PercentFromGroup / 100) / (decimal)1.18) * (decimal)0.8;

                //ბონუსი უფროსი კოორდინატორებისგან
                if (_seniorCoordinators.Where(p => p.ParentId == _manager.Id && !p.Succeeded).Any())
                    continue;

                _amount = _seniorCoordinators.Where(p => p.ParentId == _manager.Id).Select(p => p.GroupSales).Sum();
                _manager.SeniorCoordBonus = (((_amount - _amount * 33 / 100) * m_PercentFromSeniorCoords / 100) / (decimal)1.18) * (decimal)0.8;

                //პირადი კოორდინატორები
                if (_coordinators.Where(p => p.ParentId == _manager.Id && !p.Succeeded).Any())
                    continue;

                _amount = _coordinators.Where(p => p.ParentId == _manager.Id).Select(p => p.GroupSales).Sum();

                //უფროსი კოორდინატორების კოორდინატორები
                List<int> _seniorCoordIds = _seniorCoordinators.Where(p => p.ParentId == _manager.Id).Select(p => p.Id).ToList();
                if (_coordinators.Where(p => _seniorCoordIds.Contains(p.ParentId) && !p.Succeeded).Any())
                    continue;

                _amount += _coordinators.Where(p => _seniorCoordIds.Contains(p.ParentId)).Select(p => p.GroupSales).Sum();
                _manager.CoordBonus = (((_amount - _amount * 33 / 100) * m_PercentFromCoords / 100) / (decimal)1.18) * (decimal)0.8;
                _manager.Succeeded = true;
            }

            //დირექტორი
            foreach (ContragentBonusItem _director in _directors)
            {
                // ბონუსი პირადი გაყიდვებიდან
                /* _discountPercent = GetDiscountPercent(_director.Sales);
                 _director.PersonalBonus = _director.Sales * _discountPercent / 100;*/
                _director.PersonalBonus = _director.Sales;

                //ბონუსი პირადი ჯგუფიდან
                _consults = m_BonusItems.Where(p => p.ParentId == _director.Id && p.Type == 0).ToList();
                if (!_consults.Where(p => p.Sales < 50).Any())
                    continue;


                _director.GroupSales = _consults.Select(p => p.Sales).Sum();
                _director.GroupSales += _director.Sales;
                if (_director.GroupSales < 12300)
                    continue;

                _director.GroupBonus = (((_director.GroupSales - _director.GroupSales * 33 / 100) * m_PercentFromGroup / 100) / (decimal)1.18) * (decimal)0.8;


                //ბონუსი მენეჯერებისგან
                if (_managers.Where(p => p.ParentId == _director.Id && !p.Succeeded).Any())
                    continue;

                _amount = _managers.Where(p => p.ParentId == _director.Id).Select(p => p.GroupSales).Sum();
                _director.ManagerBonus = (((_amount - _amount * 33 / 100) * m_PercentFromManagers / 100) / (decimal)1.18) * (decimal)0.8;


                //პირადი უფროსი კოორდინატორები
                List<ContragentBonusItem> _seniors = _seniorCoordinators.Where(p => p.ParentId == _director.Id).ToList();
                if (_seniors.Where(p => !p.Succeeded).Any())
                    continue;

                //მენეჯერების უფროსი კოორდინატორები
                List<int> _mIds = _managers.Where(p => p.ParentId == _director.Id).Select(p => p.Id).ToList();
                List<ContragentBonusItem> _managerSeniors = _seniorCoordinators.Where(p => _mIds.Contains(p.ParentId)).ToList();
                if (_managerSeniors.Where(p => !p.Succeeded).Any())
                    continue;

                _amount = _seniors.Select(p => p.GroupSales).Sum() + _managerSeniors.Select(p => p.GroupSales).Sum();
                _director.SeniorCoordBonus = (((_amount - _amount * 33 / 100) * m_PercentFromSeniorCoords / 100) / (decimal)1.18) * (decimal)0.8;

                //პირადი კოორდინატორები
                List<ContragentBonusItem> _coors = _coordinators.Where(p => p.ParentId == _director.Id).ToList();
                if (_coors.Where(p => !p.Succeeded).Any())
                    continue;

                //პირადი მენეჯერების კოორდინატორები
                List<ContragentBonusItem> _managerCoords = _coordinators.Where(p => _mIds.Contains(p.ParentId)).ToList();
                if (_managerCoords.Where(p => !p.Succeeded).Any())
                    continue;

                //პირადი უფროსი კოორდინატორების კოორდინატორები
                List<int> _sIds = _seniors.Select(p => p.Id).ToList();
                List<ContragentBonusItem> _seniorCoordCoords = _coordinators.Where(p => _sIds.Contains(p.ParentId)).ToList();
                if (_seniorCoordCoords.Where(p => !p.Succeeded).Any())
                    continue;

                //პირადი მენეჯერების უფროსი კოორდინატორების კოორდინატორები
                _sIds = _managerSeniors.Select(p => p.Id).ToList();
                List<ContragentBonusItem> _managerSeniorCoordCoords = _coordinators.Where(p => _sIds.Contains(p.ParentId)).ToList();
                if (_managerSeniorCoordCoords.Where(p => !p.Succeeded).Any())
                    continue;

                _amount = _coors.Select(p => p.GroupSales).Sum() + _managerCoords.Select(p => p.GroupSales).Sum() + _seniorCoordCoords.Select(p => p.GroupSales).Sum() + _managerSeniorCoordCoords.Select(p => p.GroupSales).Sum();

                _director.CoordBonus = (((_amount - _amount * 33 / 100) * m_PercentFromCoords / 100) / (decimal)1.18) * (decimal)0.8;
                _director.Succeeded = true;
            }

            m_GridBonus.Rows.Clear();
            foreach (ContragentBonusItem _item in m_BonusItems.Where(p => p.Type > 0).OrderByDescending(p => p.Type))
                m_GridBonus.Rows.Add(_item.Code, _item.Name, _item.Tel, _item.PersonalBonus, _item.GroupBonus, _item.CoordBonus, _item.SeniorCoordBonus, _item.ManagerBonus, (_item.PersonalBonus + _item.GroupBonus + _item.CoordBonus + _item.SeniorCoordBonus + _item.ManagerBonus));

        }
        private void FillStatistic()
        {           
            string _sql = @"SELECT r.child_id AS Id, r.parent_id AS ParentId, ISNULL(c.code,'') AS Code, ISNULL(c.name,'') AS Name, ISNULL(c.tel,'') AS Tel,
                            (SELECT CONVERT(DECIMAL(18,2), ISNULL(SUM(amount),0)) FROM doc.GeneralDocs WHERE doc_type=21 AND param_id1=c.id AND tdate BETWEEN 
                            '" + txtDates.dtp_From.Value.ToString("yyyy-MM-dd HH:mm") + @"' AND '" + txtDates.dtp_To.Value.ToString("yyyy-MM-dd HH:mm") + @"') AS Sales,  
                            CONVERT(DECIMAL(18,2), 0) AS ChildrenSales,  CONVERT(DECIMAL(18,2), 0) AS GroupSales, CONVERT(DECIMAL(18,2), 0) AS GroupBonus
                            FROM book.ContragentRelations AS r 
                            INNER JOIN book.Contragents AS p ON r.parent_id=p.id
                            INNER JOIN book.Contragents AS c ON r.child_id=c.id
                            ORDER BY r.parent_id";

            m_RelationItems = ProgramManager.GetDataManager().GetList<ContragentRelationItem>(_sql);
            if (m_RelationItems == null || m_RelationItems.Count == 0)
                return;
         
            m_Grid.Rows.Clear();

            foreach (ContragentRelationItem _item in m_RelationItems)
            {
                _item.ChildrenSales = GetChildrenSales(_item.Id);
                _item.GroupSales = _item.Sales + _item.ChildrenSales;
            }
            foreach (ContragentRelationItem _item in m_RelationItems)
            {
                m_Grid.Rows.Add(_item.Code, _item.Name, _item.Tel, _item.Sales, _item.GroupSales, _item.GroupSales / (decimal)1.18, GetBonus(_item.Id, _item.Sales, _item.GroupSales).Value, GetChildrenBonus(_item.Id), GetPercent(_item.Sales, _item.GroupSales));
            }
              
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (checkNewMethod.Checked)
                FillStatisticNew();
            else
                FillStatistic();
        }

        private void m_Grid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            foreach (DataGridViewRow row in m_Grid.Rows)
            {
                row.DefaultCellStyle.SelectionBackColor = Color.RoyalBlue;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataGridView _grid = checkNewMethod.Checked ? m_GridBonus : m_Grid;
            if (_grid.Rows.Count == 0)
                return;

            SaveFileDialog dlgSave = new SaveFileDialog();
            dlgSave.Title = ProgramManager.GetTranslatorManager().Translate("მიუთითეთ შესანახი ადგილი");
            dlgSave.Filter = "Excel 2003 Files|*.xls";
            if (dlgSave.ShowDialog(this) != DialogResult.OK)
                return;

            ProgressDispatcher.Activate();
            ExcelManager _excelManager = new ExcelManager(ProgramManager, _grid, "");
            _excelManager.OnExcelExportFast(dlgSave.FileName);
            ProgressDispatcher.Deactivate();
        }

        private void btnRegHistory_Click(object sender, EventArgs e)
        {
            ContragentRegHistoryForm _form = new ContragentRegHistoryForm() { ProgramManager = this.ProgramManager };
            ProgramManager.TranslateControl(_form);
            _form.Show();
        }

        private void btnPercentSettings_Click(object sender, EventArgs e)
        {
            using (BonusPercentForm _form = new BonusPercentForm() { ProgramManager = this.ProgramManager })
            {
                _form.ShowDialog();
            }
        }

        private void checkNewMethod_CheckedChanged(object sender, EventArgs e)
        {
            if (checkNewMethod.Checked)
            {
                m_GridBonus.Visible = true;
                m_GridBonus.Dock = DockStyle.Fill;

                m_Grid.Visible = false;
                m_Grid.Dock = DockStyle.None;
            }
            else
            {
                m_GridBonus.Visible = false;
                m_GridBonus.Dock = DockStyle.None;

                m_Grid.Visible = true;
                m_Grid.Dock = DockStyle.Fill;
            }
        }
    }
    public class ContragentRelationItem
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public decimal Sales { get; set; }
        public decimal ChildrenSales { get; set; }
        public decimal GroupSales { get; set; }
        public decimal GroupBonus { get; set; }
    }
    public class ContragentBonusItem
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public decimal Sales { get; set; }
        public decimal GroupSales { get; set; }
        public decimal PersonalBonus { get; set; }
        public decimal GroupBonus { get; set; }
        public decimal CoordBonus { get; set; }
        public decimal SeniorCoordBonus { get; set; }
        public decimal ManagerBonus { get; set; }
        public bool Succeeded { get; set; }
        public byte Type { get; set; }
    }
}
