using ipmPMBasic;
using System;
using System.Collections;
using System.Windows.Forms;

namespace ipmExtraFunctions
{
    public partial class ExMultitestSoilMethodForm : Form
    {
        ProgramManagerBasic ProgramManager;
        private int _id;
        public int Res { get; set; }
        public ExMultitestSoilMethodForm(ProgramManagerBasic PM, int id)
        {
            ProgramManager = PM;
            _id = id;
            InitializeComponent();

            onFill();
        }

        private void onFill()
        {
            if (_id > 0)
            {
                var _data = ProgramManager.GetDataManager().GetTableData("SELECT TOP(1) * FROM book.MultitestSoilMethods WHERE id=" + _id);
                if (_data != null && _data.Rows.Count > 0)
                {
                    txtParameter.Text = _data.Rows[0]["parameter"].ToString();
                    txtVeryLow.Text = _data.Rows[0]["very_low"].ToString();
                    txtLow.Text = _data.Rows[0]["low"].ToString();
                    txtAverage.Text = _data.Rows[0]["average"].ToString();
                    txtGood.Text = _data.Rows[0]["good"].ToString();
                    txtHigh.Text = _data.Rows[0]["high"].ToString();
                    txtMethod.Text = _data.Rows[0]["method"].ToString();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(txtParameter.Text) == "" || txtMethod.Text == "")
                return;

            string sql = "";
            if (_id == 0)
                sql = "INSERT INTO book.MultitestSoilMethods(parameter,very_low,low,average,good,high,method) VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7) SELECT SCOPE_IDENTITY()";
            else
                sql = "UPDATE book.MultitestSoilMethods SET parameter=@p1,very_low=@p2,low=@p3,average=@p4,good=@p5,high=@p6,method=@p7 WHERE id=" + _id;

            Hashtable _params = new Hashtable();
            _params.Add("p1", txtParameter.Text);
            _params.Add("p2", txtVeryLow.Text);
            _params.Add("p3", txtLow.Text);
            _params.Add("p4", txtAverage.Text);
            _params.Add("p5", txtGood.Text);
            _params.Add("p6", txtHigh.Text);
            _params.Add("p7", txtMethod.Text);

            Res = ProgramManager.GetDataManager().GetIntegerValue(sql, _params);
            if (Res >= 0)
                DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                DialogResult = System.Windows.Forms.DialogResult.No;

        }
    }
}
