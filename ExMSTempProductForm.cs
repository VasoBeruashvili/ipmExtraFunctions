using System;
using System.Windows.Forms;
using ipmPMBasic;
using System.Collections;

namespace ipmExtraFunctions
{
    public partial class ExMSTempProductForm : Form
    {
        ProgramManagerBasic ProgramManager;
        int templateId;
        int tempProductId;
        Hashtable sqlParams = new Hashtable();
        private bool hasShowPrice = false;
        public ExMSTempProductForm(ProgramManagerBasic PM, int template_id, int temp_product_id)
        {
            InitializeComponent();
            ProgramManager = PM;
            templateId = template_id;
            tempProductId = temp_product_id;

            hasShowPrice = PM.GetDataManager().GetIntegerValue(@"select count(*) from book.users AS u 
                                                             Inner join book.groupusers gu on gu.id=u.group_id
                                                             where gu.name=N'ხელმძღვანელი' and u.id=" + PM.GetUserID()) > 0;

            txtPrice.Visible = hasShowPrice;
            panel4.Visible = hasShowPrice;
        }
        public void setParams(int product_id, string product_name, double quantity, string edge_value, string price)
        {
            btnProduct.Tag = product_id;
            txtProduct.Text = product_name;
            txtEdgeValue.Text = edge_value;
            txtPrice.Text = price;
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
            txtEdgeValue.Text = "";

        }
        private bool checkParams()
        {
            if (txtProduct.Text == string.Empty)
            {
                MessageBox.Show("აირჩიეთ საქონელი!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtQuantity.Text == string.Empty)
            {
                MessageBox.Show("მიუთითეთ რაოდენობა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool onInsertNewProduct()
        {
            int productId = 0;
            int.TryParse(btnProduct.Tag.ToString(), out productId);
            if (productId == 0) return false;

            double price = 0;
            double.TryParse(txtPrice.Text, out price);

            double quantity = 0;
            if (!double.TryParse(txtQuantity.Text, out quantity)) return false;

            string sql = @"INSERT INTO book.ExMSTemplateProducts (template_id, product_id, quantity,edge_value,price) VALUES (@template_id, @product_id, @quantity,@edge_value,@price)";
            sqlParams.Clear();
            sqlParams.Add(@"template_id", templateId);
            sqlParams.Add(@"product_id", productId);
            sqlParams.Add(@"quantity", quantity);
            sqlParams.Add(@"price", price);
            sqlParams.Add(@"edge_value", txtEdgeValue.Text);

            if (!ProgramManager.GetDataManager().ExecuteSql(sql, sqlParams)) return false;

            return true;
        }
        private bool onUpdateProduct()
        {
            int productId = 0;
            int.TryParse(btnProduct.Tag.ToString(), out productId);
            if (productId == 0) return false;

            double quantity = 0;
            if (!double.TryParse(txtQuantity.Text, out quantity)) return false;

            double price = 0;
            double.TryParse(txtPrice.Text, out price);


            string sql = @"UPDATE book.ExMSTemplateProducts SET product_id=@product_id, quantity=@quantity,edge_value=@edge_value,price=@price WHERE id=@id";
            sqlParams.Clear();
            sqlParams.Add(@"product_id", productId);
            sqlParams.Add(@"quantity", quantity);
            sqlParams.Add(@"edge_value", txtEdgeValue.Text);
            sqlParams.Add("@price", price);
            sqlParams.Add(@"id", tempProductId);

            if (!ProgramManager.GetDataManager().ExecuteSql(sql, sqlParams)) return false;

            return true;
        }
        private void onSave()
        {
            if (!checkParams()) return;

            if (tempProductId == 0)
            {
                if (!onInsertNewProduct())
                {
                    MessageBox.Show("საქონლის შენახვა ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (!onUpdateProduct())
                {
                    MessageBox.Show("საქონლის განახლება ვერ მოხერხდა!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            onAddProduct();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            onSave();
        }

       
        
    }
}
