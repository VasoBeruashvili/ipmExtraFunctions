using ipmPMBasic;
using System;
using System.Collections;
using System.Windows.Forms;

namespace ipmExtraFunctions
{
    public partial class ExMultitestPriceForm : Form
    {
        ProgramManagerBasic ProgramManager;
        private int _id;
        public int Res { get; set; }
        public ExMultitestPriceForm(ProgramManagerBasic PM, int id)
        {
            ProgramManager = PM;
            _id = id;
            InitializeComponent();

            onFill();
        }

        private void onFill()
        {
            comboMethods.DataSource = ProgramManager.GetDataManager().GetTableData("SELECT id,name FROM book.Departments");
            comboMethods.DisplayMember = "name";
            comboMethods.ValueMember = "id";

            if (_id > 0)
            {
                var _data = ProgramManager.GetDataManager().GetTableData("SELECT * FROM book.MultitestStaffSalaries WHERE id=" + _id);
                if (_data != null && _data.Rows.Count > 0)
                {
                    comboMethods.SelectedValue = _data.Rows[0]["id"];
                    txtPrice.Text = Convert.ToString(_data.Rows[0]["price"]);
                    txtProduct.Text = ProgramManager.GetDataManager().GetString("SELECT TOP(1) name FROM book.Products WHERE id=" + _data.Rows[0]["product_id"]);
                    btnProduct.Tag = _data.Rows[0]["product_id"];
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(btnProduct.Tag) == "" || txtPrice.Text == "")
                return;

            string sql = "";
            if (_id == 0)
                sql = "INSERT INTO book.MultitestStaffSalaries(product_id,department_id,price) VALUES(@p1,@p2,@p3) SELECT SCOPE_IDENTITY()";
            else
                sql = "UPDATE book.MultitestStaffSalaries SET product_id=@p1,department_id=@p2,price=@p3 WHERE id=" + _id;

            Hashtable _params = new Hashtable();
            _params.Add("p1", btnProduct.Tag);
            _params.Add("p2", comboMethods.SelectedValue);
            _params.Add("p3", txtPrice.Text);

            Res = ProgramManager.GetDataManager().GetIntegerValue(sql, _params);
            if (Res >= 0)
                DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            onAddProduct();
        }

        private void onAddProduct()
        {

            int value = -1;
            int.TryParse(Convert.ToString(btnProduct.Tag), out value);
            int productId = ProgramManager.ShowSelectForm("TABLE_PRODUCT", value);
            if (productId <= 0)
                return;
            txtProduct.Text = ProgramManager.GetDataManager().GetProductNameByID(productId);
            btnProduct.Tag = productId;
        }
    }
}
